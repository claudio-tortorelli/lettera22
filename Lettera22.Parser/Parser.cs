using Lettera22.Common;
using Lettera22.Metadoc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lettera22.Parser 
{
    /// <project>Lettera22</project>
    /// <copyright company="Claudio Tortorelli">
    /// Copyright (c) 2019 All Rights Reserved
    /// </copyright>        
    /// <author>Claudio Tortorelli</author>
    /// <email>claudio.tortorelli@gmail.com</email>
    /// <web>http://www.claudiotortorelli.it</web>
    /// <date>Feb 2019</date>
    /// <summary>
    /// parse the options and start the program
    /// </summary>
    /// https://choosealicense.com/licenses/mit/
    class Parser : Lettera22Program
    {
        static void Main(string[] args)
        {
            Lettera22Program.Main(args);
            try 
            {
                if (cmdLine.GetDocToProcess().Length > 0)
                {
                    ProcessFile(cmdLine.GetDocToProcess(), cmdLine.IsForced(), true);
                }
                else
                {
                    ProcessFolder();                
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

        public static bool ProcessFile(string txtFilePath, bool bForce = false, bool bOpen = false)
        {
            if (!Globals.IsInitialized())
                return false;

            if (!File.Exists(txtFilePath))
                return false;

            string xmlFilePath = Utils.ChangePathExtension(txtFilePath, Globals.XML_EXT);
            if (!File.Exists(xmlFilePath))
                BuildTextWorkFromScratch(txtFilePath);
            else
            {
                long xmlLength = new System.IO.FileInfo(xmlFilePath).Length;
                if (xmlLength <= 10)
                    BuildTextWorkFromScratch(txtFilePath);
            }
            if (!File.Exists(xmlFilePath))
            {
                Globals.m_Logger.Error(string.Format("Unable to create XML"));
                return false;
            }

            MetaDoc xmlTextWork = new Metadoc.MetaDoc();
            if (!xmlTextWork.Load(xmlFilePath))
            {
                Globals.m_Logger.Error(string.Format("Unable to load XML"));
                return false;
            }

            bool bTextWorkResaved = false;
            if (!xmlTextWork.IsConsistent() || bForce)
            {
                int rev = xmlTextWork.GetRevision();
                if (!xmlTextWork.IsConsistent())
                    Globals.m_Logger.Info(string.Format("Metadoc content is not consistent: rebuilding it..."));
                else
                    Globals.m_Logger.Info(string.Format("Metadoc content is forced to be rebuilt"));
                
                BuildTextWorkFromScratch(txtFilePath);
                
                if (!xmlTextWork.Load(xmlFilePath))
                {
                    Globals.m_Logger.Error(string.Format("Unable to load XML"));
                    return false;
                }
                xmlTextWork.SetRevision(rev + 1);
                bTextWorkResaved = xmlTextWork.Save();                
            }
            return true;
        }

        public static bool BuildTextWorkFromScratch(string txtFilePath)
        {
            if (!Globals.IsInitialized())
                return false;

            Globals.m_Logger.Info(string.Format("Processing document {0}", Path.GetFileName(txtFilePath)));

            bool bRet = false;
            MetadocParser parser = new MetadocParser(txtFilePath);
            if (!parser.IsParsed())
                Globals.m_Logger.Error(string.Format("Unable to parse {0}: error {1}!", Path.GetFileName(txtFilePath), parser.GetRetCode()));
            else if (!parser.Save())
                Globals.m_Logger.Error(string.Format("Unable to save {0}!", parser.GetOutFilePath()));
            else
            {
                Globals.m_Logger.Info(string.Format("Parsing done!"));
                bRet = true;
            }
            return bRet;
        }

        public static bool ProcessFolder()
        {
            if (!Globals.IsInitialized())
                return false;

            if (Globals.TextWorkFolder().Length == 0)
                return false;

            if (!Directory.Exists(Globals.TextWorkFolder()))
                return false;

            Globals.m_Logger.Info(string.Format("Processing the whole document folder..."));


            bool bRet = true;
            string[] filePaths = Directory.GetFiles(Globals.TextWorkFolder(), "*.txt");
            for (int iFile = 0; iFile < filePaths.Length; iFile++)
            {
                bRet &= ProcessFile(filePaths[iFile], false);
            }
            return bRet;
        }

    }
}
