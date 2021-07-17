using Lettera22.Common;
using Lettera22.Metadoc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lettera22.Linker
{
    public class Index
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
        /// </summary>
        /// https://choosealicense.com/licenses/mit/
        public static bool SignIndex(string indexHTMLPath, string fileHash)
        {
            Globals.m_Logger.Info("Sign the index");
            string nowID = DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss");
            string mailID = Globals.IndexSignPrefix() + fileHash;
            string body = "Lettera22 - FEA signature of content index hash";

            Directory.CreateDirectory(Globals.IndexHistoryFolder());

            bool contentSigned = false;
            Email.Send(Globals.EmailAddress(), Globals.EmailAddress(), mailID, body, indexHTMLPath);
            for (int iTrial = 0; iTrial < 2; iTrial++)
            {
                List<OpenPop.Mime.Message> signedMessages = Email.Receive(Globals.POP3Host(), Globals.POP3Port(), Globals.POP3SSLEnabled(), Globals.POP3Username(), Globals.POP3Password(), Globals.IndexSignPrefix());

                for (int iMsg = 0; iMsg < signedMessages.Count; iMsg++)
                {
                    // check if it is the right email
                    System.Net.Mail.MailMessage msg = signedMessages[iMsg].ToMailMessage();
                    if (!msg.Subject.Contains(mailID) || !msg.Subject.ToLower().Contains("consegna"))
                        continue; // looking for current message

                    // extract the attachements
                    foreach (var attachment in signedMessages[iMsg].FindAllAttachments())
                    {
                        string ext = Path.GetExtension(attachment.FileName);
                        if (!ext.ToLower().Equals(Globals.XML_EXT))
                            continue;
                        string name = Path.GetFileNameWithoutExtension(attachment.FileName);
                        string filename = String.Format("{0}_{1}{2}", name, nowID, ext);

                        string filePath = Path.Combine(Globals.IndexHistoryFolder(), filename);

                        FileStream Stream = new FileStream(filePath, FileMode.Create);
                        BinaryWriter BinaryStream = new BinaryWriter(Stream);
                        BinaryStream.Write(attachment.Body);
                        BinaryStream.Close();
                        contentSigned = true;

                        File.Copy(filePath, Path.Combine(Globals.IndexFolder(), "signature.xml"), true);

                        File.Copy(indexHTMLPath, Path.Combine(Globals.IndexHistoryFolder(), String.Format("index_{0}.html", nowID)), true);
                        break;
                    }
                }
                if (contentSigned)
                {
                    Globals.m_Logger.Info("done!");
                    break;
                }
                Globals.m_Logger.Info(String.Format("Trial n.{0}...", iTrial + 1));
                System.Threading.Thread.Sleep(1000);
            }

            // ok now you can store the index anche the recipt on a local folder...

            return contentSigned;
        }

        public static string ProcessIndex(List<MetaDoc> textWorks)
        {
            if (!Globals.IsInitialized())
                return "";

            if (Globals.IndexResourceFolder().Length == 0 || !Directory.Exists(Globals.IndexResourceFolder()))
            {
                Globals.m_Logger.Error("Indexer options not available: resource folder not present");
                return "";
            }

            if (Globals.IndexFolder().Length == 0)
            {
                Globals.m_Logger.Error("Indexer options not available: index folder not defined");
                return "";
            }

            if (!Directory.Exists(Globals.IndexFolder()))
            {
                Directory.CreateDirectory(Globals.IndexFolder());
            }

            IndexerFactory index = new IndexerFactory();
            bool bIndexBuilt = index.BuildIndex(textWorks);

            string indexHash = Utils.GetHashSha256(Globals.IndexFolder() + Globals.IndexFileName());
            Globals.m_Logger.Info("Index new hash: " + indexHash);
            return indexHash;
        }
    }
}
