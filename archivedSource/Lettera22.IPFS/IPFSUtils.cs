using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Lettera22.Common;
using System.Diagnostics;
using System.IO;
using System.ComponentModel;
using System.Collections.Specialized;

namespace Lettera22.IPFS
{
    public class IPFSUtils
    {
        /// <project>Lettera22</project>
        /// <copyright company="Claudio Tortorelli">
        /// Copyright (c) 2018 All Rights Reserved
        /// </copyright>        
        /// <author>Claudio Tortorelli</author>
        /// <email>claudio.tortorelli@gmail.com</email>
        /// <web>http://www.claudiotortorelli.it</web>
        /// <date>Apr 2018</date>
        /// <summary>
        /// </summary>
        /// https://choosealicense.com/licenses/mit/

        private static Executable m_ipfs = null;
        private static Executable m_ipfsDaemon = null;
        private static string m_ipfsConfig = "config";

        private static string m_lastError = "";

        private static string m_peerIdentity = "";
        private static string m_lastAddedHash = "";

        private static bool m_daemonStarted = false;
        private static bool m_bInitialized = false;
        
        
        public static IPFSRet Init()
        {
            Globals.m_Logger.Info("IPFS initialization");

            string exePath = Globals.IPFSFolder() + "init.bat";
            //Globals.m_Logger.Info(exePath);
            
            Executable ipfs = new Executable(exePath);

            bool bHomeAlreadyInitialized = false;
            string ipfsHome = Globals.IPFSOutFolder();
            if (!Directory.Exists(Globals.IPFSHomeFolder()))
                Directory.CreateDirectory(ipfsHome);
            else
                bHomeAlreadyInitialized = true;

            string ipfsError = string.Format("{0}\\error.txt", Globals.IPFSOutFolder());
            File.Delete(ipfsError);

            ipfs.StandardErrorFileName = ipfsError;
            ipfs.Arguments = Globals.IPFSHomeFolder();
            int retCode = ipfs.Run();

            Globals.m_Logger.Info(string.Format("return {0}", retCode));

            // check any errors
            if (retCode != 0 && File.Exists(ipfsError))
            {
                if (!bHomeAlreadyInitialized || retCode != 1)
                {
                    ReadLastError(ipfsError);
                    return IPFSRet.Error;
                }
            }
            
            // Read the file and display it line by line.  
            string line;
            StreamReader file = new StreamReader(Globals.IPFSHomeFolder() + Path.DirectorySeparatorChar + m_ipfsConfig);
            while ((line = file.ReadLine()) != null)
            {
                if (line.IndexOf("PeerID") >= 0)
                {
                    //"PeerID": "QmfG3b1nXgayKKTJN6YnxJYSAWnMZbeZjaynRBQB5SQBnP",
                    m_peerIdentity = line.Replace("\"PeerID\": \"", "");
                    m_peerIdentity = m_peerIdentity.Replace("\",", "");
                    break;
                }
            }
            file.Close();
            Globals.m_Logger.Info(string.Format("IPFS Initialized with PeerID {0}", m_peerIdentity));

            // daemon is started in background             
            StartDaemon();
            int daemonTrial = 0;
            while (!IsIPFSDaemonRunning())
            {
                Thread.Sleep(500);
                Globals.m_Logger.Info(".");
                daemonTrial++;
                if (daemonTrial == 10)
                    break;
            }
            m_bInitialized = IsIPFSDaemonRunning();
            if (m_bInitialized)
                return IPFSRet.Ok;
            return IPFSRet.Error;
        }
        


        public static IPFSRet Add(string fileToAdd, bool traceError = true)
        {
            if (fileToAdd == null || !IsIPFSDaemonRunning())
                return IPFSRet.Error;

            fileToAdd = Utils.AddQuotesIfRequired(fileToAdd);

            Globals.m_Logger.Info(string.Format("IPFS add {0}", Path.GetFileName(fileToAdd)));

            Directory.CreateDirectory(Globals.IPFSOutFolder());
            string ipfsError = string.Format("{0}\\error.txt", Globals.IPFSOutFolder());
            if (traceError)
                File.Delete(ipfsError);

            string ipfsOut = string.Format("{0}\\add.txt", Globals.IPFSOutFolder());
            int retCode = 0;

            string exePath = Globals.IPFSFolder() + "add.bat";
            Executable ipfs = new Executable(exePath);
            
            if (traceError)
                ipfs.StandardErrorFileName = ipfsError;
            ipfs.Arguments = string.Format("{0} {1}", fileToAdd, Globals.IPFSHomeFolder());
            retCode = ipfs.Run();

            Globals.m_Logger.Info(string.Format("return {0}", retCode));

            // check any errors
            if (retCode != 0 && File.Exists(ipfsError))
            {
                if (traceError)
                    ReadLastError(ipfsError);
                return IPFSRet.Error;
            }

            // Read the file and display it line by line.  
            string line;
            StreamReader file = new StreamReader(ipfsOut);
            try
            {
                while ((line = file.ReadLine()) != null)
                {
                    if (line.IndexOf("added ") >= 0)
                    {
                        string[] tokens = line.Replace("added ", "").Split(' ');
                        if (tokens.Length > 1)
                        {
                            m_lastAddedHash = tokens[0];

                            exePath = Globals.IPFSFolder() + "pin_add.bat";
                            ipfs = new Executable(exePath);

                            Globals.m_Logger.Info(string.Format("IPFS pin add {0}", m_lastAddedHash));

                            ipfsOut = string.Format("{0}\\pin_add.txt", Globals.IPFSOutFolder());
                            if (traceError)
                                ipfs.StandardErrorFileName = ipfsError;
                            ipfs.Arguments = string.Format("{0} {1}", m_lastAddedHash, Globals.IPFSHomeFolder());
                            retCode = ipfs.Run();

                            Globals.m_Logger.Info(string.Format("return {0}", retCode));
                            break;
                        }
                    }
                }
            }
            finally
            {
                file.Close();
            }
            return IPFSRet.Ok;
        }

        //public static IPFSRet NamePublish(string indexHash, Boolean traceError = true)
        //{
        //    if (!IsIPFSDaemonRunning())
        //        return IPFSRet.Error;

        //    Globals.m_Logger.Info(string.Format("IPFS name publish of index.html...please wait"));

        //    Directory.CreateDirectory(Globals.IPFSOutFolder());
        //    string ipfsError = string.Format("{0}\\error.txt", Globals.IPFSOutFolder());
        //    if (traceError)
        //        File.Delete(ipfsError);

        //    string ipfsOut = string.Format("{0}\\publish.txt", Globals.IPFSOutFolder());
        //    int retCode = 0;

        //    m_ipfs.StandardOutputFileName = ipfsOut;
        //    if (traceError)
        //        m_ipfs.StandardErrorFileName = ipfsError;
        //    m_ipfs.Arguments = string.Format("name publish {0}", indexHash);
        //    retCode = m_ipfs.Run();

        //    Globals.m_Logger.Info(string.Format("return {0}", retCode));

        //     check any errors
        //    if (retCode != 0 && File.Exists(ipfsError))
        //    {
        //        if (traceError)
        //            ReadLastError(ipfsError);
        //        return IPFSRet.Error;
        //    }

        //    return IPFSRet.Ok;
        //}

        public static string GetLastAddedHash() { return m_lastAddedHash; }

        private static void ReadLastError(string errorFilePath)
        {
            m_lastError = "";

            if (!File.Exists(errorFilePath))
                return;

            string line;
            StreamReader file = new StreamReader(errorFilePath);
            while ((line = file.ReadLine()) != null)
            {
                m_lastError += line;
                m_lastError += "\n";
            }

            Globals.m_Logger.Error(m_lastError);
        }

        public static string GetPeerId() { return m_peerIdentity; }

        public static bool IsIPFSDaemonRunning()
        {
            Process[] pname = Process.GetProcessesByName("ipfs");
            if (pname.Length > 0)
            {
                return true;
            }
            return false;
        }
        
        private static void StartDaemon()
        {
            if (IsIPFSDaemonRunning())
            {
                Globals.m_Logger.Info("IPFS daemon already started");
                return;
            }

            bool ret = false;
            new Thread(delegate()
            {
                string exePath = Globals.IPFSFolder() + "daemon.bat";

                ProcessStartInfo processStartInfo = new ProcessStartInfo();
                processStartInfo.FileName = exePath;
                processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                processStartInfo.Arguments = Globals.IPFSHomeFolder();

                using (Process process = new Process())
                {
                    process.StartInfo = processStartInfo;
                    ret = process.Start();
                }
            }).Start();

            Thread.Sleep(5000);
            Globals.m_Logger.Info("IPFS daemon started...");
        }

        private static void KillDaemon()
        {
            if (!IsIPFSDaemonRunning())
            {
                Globals.m_Logger.Info("IPFS daemon already killed");
                return;
            }

            Process[] pname = Process.GetProcessesByName("ipfs");
            if (pname.Length > 0)
            {
                Globals.m_Logger.Info("Killing IPFS daemon...");
                pname[0].Kill();                
            }
        }

    }
}
