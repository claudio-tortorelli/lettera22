using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lettera22.Common;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Mail;
using Lettera22;
using System.Collections;


namespace Lettera22.Common
{
    public class Lettera22Program
    {
        /// <project>Lettera22</project>
        /// <copyright company="Claudio Tortorelli">
        /// Copyright (c) 2016 All Rights Reserved
        /// </copyright>        
        /// <author>Claudio Tortorelli</author>
        /// <email>claudio.tortorelli@gmail.com</email>
        /// <web>http://www.claudiotortorelli.it</web>
        /// <date>Aug 2016</date>
        /// <summary>
        /// parse the options and start the program
        /// </summary>
        /// https://choosealicense.com/licenses/mit/

        protected static CmdLine cmdLine = null;

        public static void Main(string[] args)
        {
            cmdLine = new CmdLine(args);
            
            // use the options file in the exe folder
            string optionFile = Utils.NormalizePath(Directory.GetParent(System.Reflection.Assembly.GetEntryAssembly().Location) + Path.DirectorySeparatorChar.ToString() + Globals.OPT_FILE_NAME);
            if (cmdLine.GetOptionFile().Length > 0)
                optionFile = cmdLine.GetOptionFile();

            // parse the options
            if (!Globals.InitGlobals(optionFile))
            {
                Console.Error.Write(optionFile);
                Console.ReadLine();
                Environment.Exit(1);
            }
            
            String curAppName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            Globals.m_Logger.Info(string.Format("**** {0} - ver. {1} ****", curAppName, Globals.SoftwareVersion()));
            if (!Utils.IsConnectionAvailable())
                Globals.m_Logger.Warn("No internet connection available?");
        }

        public static void Close(bool stop = false)
        {
            Globals.m_Logger.Info("**** Finished ****");
            Globals.m_Logger.Info(" ");
            if (stop)
            {
                Console.ReadLine();
                System.Environment.Exit(1);
            }
            System.Environment.Exit(0);
        }
    }
}
