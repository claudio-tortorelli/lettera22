using Lettera22.Common;
using Lettera22.HTML;
using Lettera22.Linker;
using Lettera22.Metadoc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lettera22.Publisher
{
    class Linker : Lettera22Program
    {
        /// <project>Lettera22</project>
        /// <copyright company="Claudio Tortorelli">
        /// Copyright (c) 2019 All Rights Reserved
        /// </copyright>        
        /// <author>Claudio Tortorelli</author>
        /// <email>claudio.tortorelli@gmail.com</email>
        /// <web>http://www.claudiotortorelli.it</web>
        /// <date>Nov 2019</date>
        /// <summary>
        /// </summary>
        /// https://choosealicense.com/licenses/mit/        
        static void Main(string[] args)            
        {
            Lettera22Program.Main(args);

            if (cmdLine.IsHash())
            {
                if (!File.Exists(cmdLine.GetDocToProcess()))
                {
                    Globals.m_Logger.Error("File not found: " + cmdLine.GetDocToProcess());
                    Lettera22Program.Close();
                }
                Globals.m_Logger.Info("SHA256:");
                Globals.m_Logger.Info(Utils.GetHashSha256(cmdLine.GetDocToProcess()));
                Lettera22Program.Close();
            }

            try
            {                
                List<MetaDoc> textWorks = LoadTextWorks();
                List<MetaDoc> worksToProcess = textWorks;
                if (cmdLine.GetDocToProcess().Length > 0)
                {                   
                    worksToProcess = new List<MetaDoc>();
                    MetaDoc txtWK = new MetaDoc();
                    if (txtWK.Load(cmdLine.GetDocToProcess()) && txtWK.IsShowInGlobalIndex())
                    {
                        Globals.m_Logger.Warn("Linking specific doc: " + cmdLine.GetDocToProcess());
                        worksToProcess.Add(txtWK);
                    }
                }

                if (worksToProcess.Count == 0)
                    throw new Exception("No textwork found: index is not processed");

                List<MetaDoc> updatedWorks = UpgradeHTMLToPublishFolder(worksToProcess, cmdLine.IsForced());
                if (updatedWorks.Count > 0)
                {
                    string indexHash = Index.ProcessIndex(textWorks);
                    if (indexHash.Length == 0)
                        throw new Exception("Unable to rebuild index");

                    if (!Utils.IsConnectionAvailable())
                    {
                        Globals.m_Logger.Warn("No connection. No more to do...");
                        Lettera22Program.Close();
                    }
                    if (!Globals.IsFTPEnabled())
                    {
                        Globals.m_Logger.Warn("FTP is not enabled in options. No more action is possible");
                        Lettera22Program.Close();
                    }
                                        
                    if (Globals.IsIndexSignatureEnabled())
                    {
                        string indexFilePath = Globals.IndexFolder() + Globals.IndexFileName();
                        if (!Index.SignIndex(indexFilePath, indexHash) && !cmdLine.IsForced())
                            throw new Exception("Unable to sign index");
                    }

                    Ftp ftpClient = new Ftp(Globals.FTPHost(), Globals.FTPUser(), Globals.FTPPassword());

                    if (!SynchIndex(ftpClient))
                        throw new Exception("Unable to update remote index");
                    if (!SynchSharedRes(ftpClient))
                        throw new Exception("Unable to update remote resources");
                    if (!SynchDocuments(ftpClient, updatedWorks))
                        throw new Exception("Unable to update remote documents");
                    if (cmdLine.IsShowResult())
                        System.Diagnostics.Process.Start(Globals.GetMainUrl());
                }
                else
                {
                    Globals.m_Logger.Info("Nothing to update...");
                }
            }
            catch (Exception ex)
            {
                Globals.m_Logger.Error(ex.Message, ex);
            }
            finally
            {
                Lettera22Program.Close();
            }
        }

        public static bool SynchIndex(Ftp ftpClient)
        {
            if (!Globals.IsInitialized())
                return false;

            Globals.m_Logger.Info(string.Format("Publishing index"));

            string indexFilePath = Globals.IndexFolder() + Globals.IndexFileName();

            ftpClient.upload(Globals.FTPRemoteFolder() + "/"+ Globals.IndexFileName(), indexFilePath);
            if (Globals.IsIndexSignatureEnabled())
            {
                string signatureFilePath = Globals.IndexFolder() + "signature.xml";
                ftpClient.upload(Globals.FTPRemoteFolder() + "/" + "signature.xml", signatureFilePath);
            }
            return true;
        }

        public static bool SynchSharedRes(Ftp ftpClient)
        {
            Globals.m_Logger.Info(string.Format("Publishing shared resources"));
            string script = Globals.IndexResourceFolder() + "sorttable.js";
            ftpClient.upload(Globals.FTPRemoteFolder() + "/" + "sorttable.js", script);
            Globals.m_Logger.Info("synch remote: sorttable.js");
            return true;
        }

        public static bool SynchDocuments(Ftp ftpClient, List<MetaDoc> textWorks)
        {
           Globals.m_Logger.Info(string.Format("Publishing documents..."));
           for (int iTw = 0; iTw < textWorks.Count; iTw++)
            {
                MetaDoc txtWK = textWorks[iTw];
                if (!txtWK.IsShowInGlobalIndex())
                    continue;

                string htmlFilePath = Utils.ChangePathExtension(Globals.IndexFolder() + txtWK.GetFileName(), Globals.HTML_EXT);
                
                string fileName = Path.GetFileName(htmlFilePath);

                if (txtWK.GetHashes().Count > 1)
                {
                    // 1 because here 0 is the current one
                    ftpClient.rename(Globals.FTPRemoteFolder() + "/" + fileName, txtWK.GetHashes()[1].GetHash() + Globals.HTML_EXT);
                }
                // TODO, move to revisions...
                ftpClient.upload(Globals.FTPRemoteFolder() + "/" + fileName, htmlFilePath);
                Globals.m_Logger.Info(string.Format("{0} added to remote ftp folder {1}", htmlFilePath, Globals.FTPRemoteFolder()));

            }
            return true;
        }

        private static List<MetaDoc> LoadTextWorks()
        {
            List<MetaDoc> textWorks = new List<MetaDoc>();

            if (Globals.TextWorkFolder().Length == 0)
                return textWorks;

            if (!Directory.Exists(Globals.TextWorkFolder()))
                return textWorks;

            string[] filePaths = Directory.GetFiles(Globals.TextWorkFolder(), "*.xml");
            for (int iFile = 0; iFile < filePaths.Length; iFile++)
            {
                MetaDoc txtWK = new MetaDoc();
                if (txtWK.Load(filePaths[iFile]) && txtWK.IsShowInGlobalIndex())
                    textWorks.Add(txtWK);
            }
            // sort by last revision descending
            textWorks.Sort((x, y) => -1 * x.GetLastRevisionDate(true).CompareTo(y.GetLastRevisionDate(true)));
            return textWorks;
        }

        private static List<MetaDoc> UpgradeHTMLToPublishFolder(List<MetaDoc> textWorks, Boolean forced)
        {
            string destFolder = Globals.IndexFolder();
            destFolder = destFolder.TrimEnd('\\') + "\\"; // normalize the end

            List<MetaDoc> updatedWorks = new List<MetaDoc>();
            for (int iTw = 0; iTw < textWorks.Count; iTw++)
            {
                if (!textWorks[iTw].IsShowInGlobalIndex())
                    continue;

                if (textWorks[iTw].IsAlreadyPublished() && !forced)
                    continue;
                
                HTMLWriter writer = new HTMLWriter(textWorks[iTw]);
                string htmlFilePath = Utils.ChangePathExtension(destFolder + textWorks[iTw].GetFileName(), Globals.HTML_EXT);
                if (textWorks[iTw].GetHashes().Count > 0)
                {
                    string archivedHTHMFilePath = Globals.IndexFolder() + textWorks[iTw].GetHashes()[0].GetHash() + Globals.HTML_EXT;
                    if (!File.Exists(archivedHTHMFilePath))
                        File.Move(htmlFilePath, archivedHTHMFilePath);
                }

                writer.Save(htmlFilePath, true, Globals.IsAbstractOnly());

                textWorks[iTw].AddHash(new DocHash(textWorks[iTw].GetCurrentHash()));
                textWorks[iTw].Save();
                updatedWorks.Add(textWorks[iTw]);
                Globals.m_Logger.Info(string.Format("{0} selected to be updated", htmlFilePath));
            }
            return updatedWorks;
        }

    }


}
