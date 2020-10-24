using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;
using System.Net;

namespace Lettera22.Common
{
    public static class Globals
    {
        /// <project>Lettera22</project>
        /// <copyright company="Claudio Tortorelli">
        /// Copyright (c) 2016 All Rights Reserved
        /// </copyright>        
        /// <author>Claudio Tortorelli</author>
        /// <email>claudio.tortorelli@gmail.com</email>
        /// <web>http://www.claudiotortorelli.it</web>
        /// <date>February 2016</date>
        /// <summary>
        /// This class wrap all variables used in the whole project as constant definition        
        /// it must be created when main form starts
        /// </summary>
        /// https://choosealicense.com/licenses/mit/

        /// start option file's values
        private static string OPT_FILE_VER = "";

        private static string SOFTWARE_NAME = "";
        private static string SOFTWARE_VER = "";
        private static string SOFTWARE_EXE_PATH = "";

        private static string ROOT_FOLDER_PATH = "";
        private static string RESOURCES_FOLDER_NAME = "";
        private static string INDEXRESOURCE_FOLDER_NAME = "";
        private static string INDEX_FOLDER_NAME = "";
        private static string TEXTWORKS_FOLDER_NAME = "";
        private static string HTMLWORKS_FOLDER_NAME = "";
        private static string LOG_FOLDER_NAME = "";
        private static string INDEX_HISTORY_FOLDER_NAME = "";

        private static bool CHECK_HASH = true;
        private static bool FORCE_REBUILD_HTML = false;
        private static string MAIN_URL = "";
        private static bool SIGN_INDEX = false;
        private static bool SHOW_PREVIOUS_VERSION = true;

        private static string INDEX_LIB = "";
        private static string INDEX_FILE_NAME = "";
        private static string INDEX_TITLE = "";
        private static string INDEX_SUBTITLE = "";
        private static string INDEX_OTHERINFO = "";
        private static bool ABSTRACT_ONLY = false;
                
        private static string EMAIL_ADDRESS = "";
        private static string EMAIL_FROM_NAME = "";

        private static string SMTP_HOST = "";
        private static int SMTP_PORT = 25;
        private static bool SMTP_SSL = true;
        private static string SMTP_FOLDER_NAME = "";
        private static string SMTP_USERNAME = "";
        private static string SMTP_PASSWORD = "";

        private static string POP3_HOST = "";
        private static int POP3_PORT = 995;
        private static bool POP3_SSL = true;
        private static string POP3_USERNAME = "";
        private static string POP3_PASSWORD = "";

        private static string FTP_HOST = "";
        private static string FTP_USER = "";
        private static string FTP_PASSWORD = "";
        private static string FTP_REMOTEFOLDER = "";
        /// end option file's values

        public static ConsoleLogger m_Logger = null;
        private static bool m_bInitialized = false;
        
        public static string HTML_EXT = ".html";
        public static string XML_EXT = ".xml";

        public static string OPT_FILE_NAME = "options.txt";

        //public static int PARAGRAPH_LINE_CHARS = 66;
        public static int PARAGRAPH_LINE_CHARS = 64; // aumentato carattere a 18px, compromesso leggibilità
       
        /// <summary>
        /// Initialize options
        /// </summary>
        /// <param name="optFilePath"></param>
        /// <returns></returns>
        public static bool InitGlobals(string optFilePath)
        {
            if (m_bInitialized)
                return true;

            System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12; 

            if (!File.Exists(optFilePath))
            {
                Console.Error.WriteLine("Options file not found!");
                return false;
            }

            string[] lines = File.ReadAllLines(optFilePath);
            if (lines.Length == 0)
            {
                Console.Error.WriteLine("Options file is empty");
                return false;
            }

            Assembly assembly = Assembly.GetEntryAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            SOFTWARE_NAME = "Lettera22";
            SOFTWARE_VER = fileVersionInfo.ProductVersion;
            SOFTWARE_EXE_PATH = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace("file:\\", "");

            try
            {
                for (int iLn = 0; iLn < lines.Length; iLn++ )
                {
                    string ln = lines[iLn];
                    if (ln.StartsWith("#") || ln.Length == 0)
                        continue;

                    ln.TrimStart();
                    ln.TrimEnd();
                    
                    int space = ln.IndexOf(" ");
                    if (space < 0)
                        continue;

                    string key = ln.Substring(0, space);
                    string value = ln.Substring(space, ln.Length - space);
                    value = value.Trim();

                    switch (key)
                    {
                        case "OPT_FILE_VER":
                        {
                            OPT_FILE_VER = value;
                            break;
                        }
                        case "ROOT_FOLDER_PATH":
                        {
                            ROOT_FOLDER_PATH = value;
                            break;
                        }
                        case "RESOURCES_FOLDER_NAME":
                        {
                            RESOURCES_FOLDER_NAME = value;
                            break;
                        }
                        case "INDEX_HISTORY_FOLDER_NAME":
                        {
                            INDEX_HISTORY_FOLDER_NAME = value;
                            break;
                        }
                        case "TEXTWORKS_FOLDER_NAME":
                        {
                            TEXTWORKS_FOLDER_NAME = value;
                            break;
                        }
                        case "HTMLWORKS_FOLDER_NAME":
                        {
                            HTMLWORKS_FOLDER_NAME = value;
                            break;
                        }
                        case "LOG_FOLDER_NAME":
                        {
                            LOG_FOLDER_NAME = value;
                            break;
                        }
                        case "INDEXRESOURCE_FOLDER_NAME":
                        {
                            INDEXRESOURCE_FOLDER_NAME = value;
                            break;
                        }
                        case "INDEX_FOLDER_NAME":
                        {
                            INDEX_FOLDER_NAME = value;
                            break;
                        }
                        case "CHECK_HASH":
                        {
                            if (!bool.TryParse(value, out CHECK_HASH))
                                CHECK_HASH = false;
                            break;
                        }
                        case "FORCE_REBUILD_HTML":
                        {
                            if (!bool.TryParse(value, out FORCE_REBUILD_HTML))
                                FORCE_REBUILD_HTML = true;
                            break;
                        }
                        case "MAIN_URL":
                        {
                            MAIN_URL = value;
                            break;
                        }
                        case "SIGN_INDEX":
                        {
                            if (!bool.TryParse(value, out SIGN_INDEX))
                                SIGN_INDEX = false;
                            break;
                        }
                        case "SHOW_PREVIOUS_VERSION":
                        {
                            if (!bool.TryParse(value, out SHOW_PREVIOUS_VERSION))
                                SHOW_PREVIOUS_VERSION = false;
                            break;
                        }                            
                        case "INDEX_FILE_NAME":
                        {
                            INDEX_FILE_NAME = value;
                            break;
                        }                            
                        case "INDEX_LIB":
                        {
                            INDEX_LIB = value;
                            break;
                        }
                        case "INDEX_TITLE":
                        {
                            INDEX_TITLE = value;
                            break;
                        }
                        case "INDEX_SUBTITLE":
                        {
                            INDEX_SUBTITLE = value;
                            break;
                        }
                        case "INDEX_OTHERINFO":
                        {
                            INDEX_OTHERINFO = value;
                            break;
                        }
                        case "ABSTRACT_ONLY":
                        {
                            if (!bool.TryParse(value, out ABSTRACT_ONLY))
                                ABSTRACT_ONLY = false;
                            break;
                        }                            
                        case "EMAIL_ADDRESS":
                        {
                            EMAIL_ADDRESS = value;
                            break;
                        }
                        case "EMAIL_FROM_NAME":
                        {
                            EMAIL_FROM_NAME = value;
                            break;
                        }
                        case "SMTP_HOST":
                        {
                            SMTP_HOST = value;
                            break;
                        }
                        case "SMTP_PORT":
                        {
                            if (!int.TryParse(value, out SMTP_PORT))
                                SMTP_PORT = 25;
                            break;
                        }
                        case "SMTP_SSL":
                        {
                            if (!bool.TryParse(value, out SMTP_SSL))
                                SMTP_SSL = false;
                            break;
                        }
                        case "SMTP_FOLDER_NAME":
                        {
                            SMTP_FOLDER_NAME = value;
                            break;
                        }
                        case "SMTP_USERNAME":
                        {
                            SMTP_USERNAME = value;
                            break;
                        }
                        case "SMTP_PASSWORD":
                        {
                            SMTP_PASSWORD = value;
                            break;
                        }
                        case "POP3_HOST":
                        {
                            POP3_HOST = value;
                            break;
                        }
                        case "POP3_PORT":
                        {
                            if (!int.TryParse(value, out POP3_PORT))
                                POP3_PORT = 995;
                            break;
                        }
                        case "POP3_SSL":
                        {
                            if (!bool.TryParse(value, out POP3_SSL))
                                POP3_SSL = false;
                            break;
                        }
                        case "POP3_USERNAME":
                        {
                            POP3_USERNAME = value;
                            break;
                        }
                        case "POP3_PASSWORD":
                        {
                            POP3_PASSWORD = value;
                            break;
                        }
                        case "FTP_HOST":
                        {
                            FTP_HOST = value.ToLower();
                            if (!FTP_HOST.StartsWith("ftp://"))
                            {
                                FTP_HOST = "ftp://" + FTP_HOST;
                            }
                            break;
                        }
                        case "FTP_USER":
                        {
                            FTP_USER = value;
                            break;
                        }
                        case "FTP_PASSWORD":
                        {
                            FTP_PASSWORD = value;
                            break;
                        }
                        case "FTP_REMOTEFOLDER":
                        {
                            FTP_REMOTEFOLDER = value;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Parser error. Check global options:");
                Console.Error.WriteLine(ex.ToString());
                return false;
            }

            CreateFolders();
            
            m_Logger = new ConsoleLogger(ServerLogLevel.Info, LogFolder());
            if (!m_Logger.IsCreated())             
            {
                Console.Error.WriteLine("Unable to create the logger");
                return false;
            }

            m_bInitialized = true;
            return m_bInitialized;
        }

        public static void CreateFolders()
        {
            string fold = RootFolder();
            Directory.CreateDirectory(fold);
            fold = ResourceFolder();
            Directory.CreateDirectory(fold);
            fold = TextWorkFolder();
            Directory.CreateDirectory(fold);
            fold = HtmlWorkFolder();
            Directory.CreateDirectory(fold);
            fold = IndexHistoryFolder();
            Directory.CreateDirectory(fold);
            fold = IndexResourceFolder();
            Directory.CreateDirectory(fold);
            fold = IndexFolder();
            Directory.CreateDirectory(fold);
            fold = LogFolder();
            Directory.CreateDirectory(fold);           
        }

        public static bool IsInitialized() { return m_bInitialized; }

        // ACCESS TO OPTIONS VALUES
        public static string SoftwareName() { return SOFTWARE_NAME; }
        public static string SoftwareVersion() { return SOFTWARE_VER; }
        public static string ExePath() { return SOFTWARE_EXE_PATH; }

        public static string RootFolder() { return Utils.NormalizePath(ROOT_FOLDER_PATH + Path.DirectorySeparatorChar, true); }

        public static string ResourceFolder() { return Utils.NormalizePath(RESOURCES_FOLDER_NAME + Path.DirectorySeparatorChar, true); }
        public static string LogFolder() { return Utils.NormalizePath(LOG_FOLDER_NAME + Path.DirectorySeparatorChar, true); }

        public static string TextWorkFolder() { return Utils.NormalizePath(RootFolder() + TEXTWORKS_FOLDER_NAME + Path.DirectorySeparatorChar, true); }
        public static string HtmlWorkFolder() { return Utils.NormalizePath(RootFolder() + HTMLWORKS_FOLDER_NAME + Path.DirectorySeparatorChar, true); }
        
        public static string IndexHistoryFolder() { return Utils.NormalizePath(RootFolder() + INDEX_HISTORY_FOLDER_NAME + Path.DirectorySeparatorChar, true); }
        public static string IndexResourceFolder() { return Utils.NormalizePath(ResourceFolder() + INDEXRESOURCE_FOLDER_NAME + Path.DirectorySeparatorChar, true); }
        public static string IndexFolder() { return Utils.NormalizePath(RootFolder() + INDEX_FOLDER_NAME + Path.DirectorySeparatorChar, true); }

        public static string IndexLibrary() { return INDEX_LIB; }
        public static string IndexFileName() { return INDEX_FILE_NAME; }
        public static string IndexTitle() { return INDEX_TITLE; }
        public static string IndexSubTitle() { return INDEX_SUBTITLE; }
        public static string IndexOtherInfo() { return INDEX_OTHERINFO; }

        public static bool IsAbstractOnly() { return ABSTRACT_ONLY; }
                
        public static bool IsCheckingHash() { return CHECK_HASH; }
        public static bool IsForcedHTMLRebuild() { return FORCE_REBUILD_HTML; }
        public static string GetMainUrl() { return MAIN_URL; }
        public static bool IsIndexSignatureEnabled() { return SIGN_INDEX; }
        public static bool IsShowPreviousVersion() { return SHOW_PREVIOUS_VERSION; }        

        public static string EmailAddress() { return EMAIL_ADDRESS; }
        public static string EmailFromName() { return EMAIL_FROM_NAME; }
        
        public static string SMTPHost() { return SMTP_HOST; }
        public static int SMTPPort() { return SMTP_PORT; }
        public static bool SMTPSSLEnabled() { return SMTP_SSL; }
        public static string SMTPFolder() { if (SMTP_FOLDER_NAME != null) return Utils.NormalizePath(RootFolder() + SMTP_FOLDER_NAME + Path.DirectorySeparatorChar, true); return ""; }
        public static string SMTPUsername() { return SMTP_USERNAME; }
        public static string SMTPPassword() { return SMTP_PASSWORD; }

        public static string POP3Host() { return POP3_HOST; }
        public static int POP3Port() { return POP3_PORT; }
        public static bool POP3SSLEnabled() { return POP3_SSL; }
        public static string POP3Username() { return POP3_USERNAME; }
        public static string POP3Password() { return POP3_PASSWORD; }

        public static string FTPHost() { return FTP_HOST; }
        public static string FTPUser() { return FTP_USER;}
        public static string FTPPassword() { return FTP_PASSWORD;}
        public static string FTPRemoteFolder() { return FTP_REMOTEFOLDER; }
        public static bool IsFTPEnabled() { return FTPHost().Length > 0; }
        
        /// <summary>
        /// Output all parsed or default options to string
        /// </summary>
        /// <returns>string</returns>
        public static string OptionsToString()
        {
            string outStr = "\n\nCurrent options:\n";
            outStr += string.Format("SOFTWARE_NAME: {0}\n", SOFTWARE_NAME);
            outStr += string.Format("SOFTWARE_VER: {0}\n", SOFTWARE_VER);
            outStr += string.Format("SOFTWARE_EXE_PATH: {0}\n", SOFTWARE_EXE_PATH);
            outStr += string.Format("ROOT_FOLDER_PATH: {0}\n", ROOT_FOLDER_PATH);
            outStr += string.Format("TEXTWORKS_FOLDER_NAME: {0}\n", TEXTWORKS_FOLDER_NAME);
            outStr += string.Format("HTMLWORKS_FOLDER_NAME: {0}\n", HTMLWORKS_FOLDER_NAME);
            outStr += string.Format("INDEXRESOURCE_FOLDER_NAME: {0}\n", INDEXRESOURCE_FOLDER_NAME);
            outStr += string.Format("INDEX_FOLDER_NAME: {0}\n", INDEX_FOLDER_NAME);
            outStr += string.Format("INDEX_HISTORY_FOLDER_NAME: {0}\n", INDEX_HISTORY_FOLDER_NAME);
            outStr += string.Format("INDEX_LIB: {0}\n", INDEX_LIB);
            outStr += string.Format("INDEX_FILE_NAME: {0}\n", INDEX_FILE_NAME);
            outStr += string.Format("ABSTRACT_ONLY: {0}\n", ABSTRACT_ONLY);
            outStr += string.Format("LOG_FOLDER_NAME: {0}\n", LOG_FOLDER_NAME);
            outStr += string.Format("CHECK_HASH: {0}\n", CHECK_HASH);
            outStr += string.Format("FORCE_REBUILD_HTML: {0}\n", FORCE_REBUILD_HTML);
            outStr += string.Format("MAIN_URL: {0}\n", MAIN_URL);
            outStr += string.Format("SIGN_INDEX: {0}\n", SIGN_INDEX);
            outStr += string.Format("SHOW_PREVIOUS_VERSION: {0}\n", SHOW_PREVIOUS_VERSION);
            outStr += string.Format("SMTP_HOST: {0}\n", SMTP_HOST);
            outStr += string.Format("SMTP_PORT: {0}\n", SMTP_PORT);
            outStr += string.Format("SMTP_SSL: {0}\n", SMTP_SSL);
            outStr += string.Format("SMTP_FOLDER_NAME: {0}\n", SMTP_FOLDER_NAME);
            outStr += string.Format("SMTP_USERNAME: {0}\n", SMTP_USERNAME);
            outStr += string.Format("SMTP_PASSWORD: {0}\n", SMTP_PASSWORD);
            outStr += string.Format("POP3_HOST: {0}\n", POP3_HOST);
            outStr += string.Format("POP3_PORT: {0}\n", POP3_PORT);
            outStr += string.Format("POP3_SSL: {0}\n", POP3_SSL);
            outStr += string.Format("POP3_USERNAME: {0}\n", POP3_USERNAME);
            outStr += string.Format("POP3_PASSWORD: {0}\n", POP3_PASSWORD);
            outStr += string.Format("FTP_HOST: {0}\n", FTP_HOST);
            outStr += string.Format("FTP_USER: {0}\n", FTP_USER);
            outStr += string.Format("FTP_PASSWORD: {0}\n", FTP_PASSWORD);
            outStr += string.Format("FTP_REMOTEFOLDER: {0}\n", FTP_REMOTEFOLDER);            

            return outStr;
        }
        
    }   
}
