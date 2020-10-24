using Lettera22.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lettera22.Common
{
    /// <project>Lettera22</project>
    /// <copyright company="Claudio Tortorelli">
    /// Copyright (c) 2018 All Rights Reserved
    /// </copyright>           /// 
    /// <author>Claudio Tortorelli</author>
    /// <email>claudio.tortorelli@gmail.com</email>
    /// <web>http://www.claudiotortorelli.it</web>
    /// <date>Apr 2018</date>
    /// <summary>
    /// parse the options from command line
    /// </summary>
    /// 
    /// https://choosealicense.com/licenses/mit/
    public class CmdLine
    { 
        private string m_OptFilePath;
        private string m_DocToProcess;
        
        private bool m_bForce;
        private bool m_bShow;
        private bool m_bDoHash;
        
        // command line argument parsing
        public CmdLine(string[] args)
        {
            m_OptFilePath = "";
            m_DocToProcess = "";
            m_bForce = false;
            m_bShow = false;
            m_bDoHash = false;

            for (int iS = 0; iS < args.Length; iS++)
            {
                string cmdArg = args[iS];
                cmdArg = cmdArg.ToLower().Trim();

                string value = args[Math.Min(args.Length-1,iS+1)];
                switch (cmdArg)
                {
                    case "-opt":
                        m_OptFilePath = value;
                        break;                                        
                     case "-force":
                        m_bForce = true;
                        break;
                    case "-show":
                        m_bShow = true;
                        break;
                    case "-hash":
                        m_bDoHash = true;
                        break;
                    default:
                        if (m_DocToProcess.Length == 0 && File.Exists(value))
                            m_DocToProcess = value;
                        break;
                }
            }           
        }

        public bool IsForced() { return m_bForce; }
        public bool IsShowResult() { return m_bShow; }
        public bool IsHash() { return m_bDoHash; }
        
        public string GetOptionFile() { return m_OptFilePath; }
        public string GetDocToProcess() { return m_DocToProcess; }

    }
}
