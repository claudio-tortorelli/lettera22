using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

/***
 * based on
 * http://www.codeproject.com/Tips/891190/Simple-Console-Logger
 */

namespace Lettera22.Common
{
    public interface ServerLogger
    {
        ServerLogLevel LogLevel { get; }

        void Debug(string text, params object[] args);
        void Info(string text, params object[] args);
        void Warn(string text, params object[] args);
        void Error(string text, params object[] args);
        void Error(Exception ex);
        void Error(Exception ex, string text, params object[] args);
    }

    public enum ServerLogLevel
    {
        Debug = 4,
        Info = 3,
        Warn = 2,
        Error = 1,
        None = 0
    }
    public class ConsoleLogger : ServerLogger
    {   
        public ServerLogLevel LogLevel { get; set; }
        private string m_LogFolderPath;
        private string m_LogFilePath;
        private bool m_bCreated;
        
        public ConsoleLogger(ServerLogLevel logLevel, string logFolderPath)
        {
            m_bCreated = false;
            LogLevel = logLevel;
            m_LogFolderPath = logFolderPath;

            try
            {
                if (!Directory.Exists(m_LogFolderPath))
                    Directory.CreateDirectory(m_LogFolderPath);

                m_LogFilePath = String.Format("{0}{1}.log", m_LogFolderPath, DateTime.Now.ToString("yyyyMMdd"));
                if (!File.Exists(m_LogFilePath))
                {
                    using (StreamWriter sw = File.CreateText(m_LogFilePath))
                    {
                        sw.WriteLine("--------- Created");
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                return;
            }
            m_bCreated = true;
        }

        public void Info(string text, params object[] args)
        {
            if (!m_bCreated)
                return;
            if (LogLevel < ServerLogLevel.Info)
                return;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Conlog("(I) ", text, args);

            string argsS = "";
            for (int i = 0; i < args.Length; i++)
            {
                if (Object.ReferenceEquals(args[i].GetType(), argsS.GetType()))
                    argsS += args[i];                
            }
            toLogFile("(I) " + text + " " + argsS);
        }

        public void Warn(string text, params object[] args)
        {
            if (!m_bCreated)
                return;
            if (LogLevel < ServerLogLevel.Warn)
                return;
            Console.ForegroundColor = ConsoleColor.Green;
            Conlog("(W) ", text, args);

            string argsS = "";
            for (int i = 0; i < args.Length; i++)
            {
                if (Object.ReferenceEquals(args[i].GetType(), argsS.GetType()))
                    argsS += args[i];
            }
            toLogFile("(W) " + text + " " + argsS);
        }

        public void Error(Exception ex)
        {
            if (!m_bCreated)
                return;
            if (LogLevel < ServerLogLevel.Error)
                return;
            Console.ForegroundColor = ConsoleColor.Red;
            Conlog("(E) ", ex.ToString());
            toLogFile("(E) " + ex.ToString());
        }

        public void Error(string text, params object[] args)
        {
            if (!m_bCreated)
                return;
            if (LogLevel < ServerLogLevel.Error)
                return;
            Console.ForegroundColor = ConsoleColor.Red;
            Conlog("(E) ", text, args);

            string argsS = "";
            for (int i = 0; i < args.Length; i++)
            {
                if (Object.ReferenceEquals(args[i].GetType(), argsS.GetType()))
                    argsS += args[i];
            }
            toLogFile("(E) " + text + " " + argsS);
        }

        public void Error(Exception ex, string text, params object[] args)
        {
            if (!m_bCreated)
                return;
            if (LogLevel < ServerLogLevel.Error)
                return;
            Console.ForegroundColor = ConsoleColor.Red;
            Conlog("(E) ", text, args);

            string argsS = "";
            for (int i = 0; i < args.Length; i++)
            {
                if (Object.ReferenceEquals(args[i].GetType(), argsS.GetType()))
                    argsS += args[i];
            }
            toLogFile("(E) " + text + " " + argsS);
        }

        public void Debug(string text, params object[] args)
        {
            if (!m_bCreated)
                return;
            if (LogLevel < ServerLogLevel.Debug)
                return;
            Conlog("(D) ", text, args);

            string argsS = "";
            for (int i = 0; i < args.Length; i++)
            {
                if (Object.ReferenceEquals(args[i].GetType(), argsS.GetType()))
                    argsS += args[i];
            }
            toLogFile("(D) " + text + " " + argsS);        
        }

        private static void Conlog(string prefix, string text, params object[] args)
        {
            //            If you want to add unique thread identifier
            //            int threadId = Thread.CurrentThread.ManagedThreadId;
            //            Console.Write("[{0:D4}] [{1}] ", threadId, DateTime.Now.ToString("HH:mm:ss.ffff"));
            Console.Write(DateTime.Now.ToString("HH:mm:ss.ffff"));
            Console.Write(prefix);
            Console.WriteLine(text, args);
            Console.ResetColor();
        }

        private bool toLogFile(string text)
        {
            if (!m_bCreated)
                return false;
            try
            {
                using (StreamWriter sw = File.AppendText(m_LogFilePath))
                {
                    sw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " " + text);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
            return true;
        }

        public bool IsCreated() { return m_bCreated; }
    }
}