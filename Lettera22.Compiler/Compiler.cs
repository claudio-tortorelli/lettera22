using Lettera22.Common;
using Lettera22.HTML;
using Lettera22.Metadoc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lettera22.Compiler
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
    class Compiler : Lettera22Program
    {
        static void Main(string[] args)
        {
            Lettera22Program.Main(args);
            try
            {
                if (cmdLine.GetDocToProcess().Length > 0)
                {
                    ProcessFile(cmdLine.GetDocToProcess(), cmdLine.IsForced(), cmdLine.IsShowResult());
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

        public static bool ProcessFile(string xmlFilePath, bool bForce = false, bool bOpen = false)
        {
            
            MetaDoc xmlTextWork = new Metadoc.MetaDoc();
            if (!xmlTextWork.Load(xmlFilePath))
            {
                Globals.m_Logger.Error(string.Format("Unable to load metadoc"));
                return false;
            }

            if (xmlTextWork.IsNoProcess())
            {
                Globals.m_Logger.Info("Skip: no process enabled");
                return false;
            }

            // ok: here the xml is available and consistent
            string htmlFilePath = Utils.ChangePathExtension(Globals.HtmlWorkFolder() + xmlTextWork.GetFileName(), Globals.HTML_EXT);
            if (!File.Exists(htmlFilePath) || Globals.IsForcedHTMLRebuild() || bForce)
            {
                Globals.m_Logger.Info(string.Format("Processing document {0}", Path.GetFileName(xmlFilePath)));

                if (!BuildHTMLFromTextWork(xmlTextWork))
                {
                    Globals.m_Logger.Error("Unable to build HTML");
                    return false;
                }
                Globals.m_Logger.Info(string.Format("Building done!"));
            }
            
            if (bOpen)
                System.Diagnostics.Process.Start(htmlFilePath);

            return true;
        }

        private static bool BuildHTMLFromTextWork(MetaDoc xmlTextWork)
        {
            bool bRet = false;
            Globals.m_Logger.Info(string.Format("Converting {0} to html", xmlTextWork.GetFileName()));
            HTMLWriter htmlWriter = new HTMLWriter(xmlTextWork);
            if (!htmlWriter.Save())
                Globals.m_Logger.Error(string.Format("Unable to save {0}!", htmlWriter.GetOutFileName()));
            else
            {
                Globals.m_Logger.Info(string.Format("{0} generated", htmlWriter.GetOutFileName()));
                bRet = true;
            }
            bRet &= htmlWriter.ValidateHTML();
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

            Globals.m_Logger.Info(string.Format("Processing the whole document folder"));

            bool bRet = true;
            string[] filePaths = Directory.GetFiles(Globals.TextWorkFolder(), "*.xml");
            for (int iFile = 0; iFile < filePaths.Length; iFile++)
            {
                bRet &= ProcessFile(filePaths[iFile]);
            }
            return bRet;
        }
    }
}
