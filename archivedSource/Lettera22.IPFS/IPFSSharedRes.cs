using Lettera22.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lettera22.IPFS
{

    public class IPFSSharedRes
    {
        /// <project>Lettera22</project>
        /// <copyright company="Claudio Tortorelli">
        /// Copyright (c) 2018 All Rights Reserved
        /// </copyright>        
        /// <author>Claudio Tortorelli</author>
        /// <email>claudio.tortorelli@gmail.com</email>
        /// <web>http://www.claudiotortorelli.it</web>
        /// <date>July 2018</date>
        /// <summary>        
        /// </summary>
        /// https://choosealicense.com/licenses/mit/
        protected static string m_SharedResFilePath = "";
        protected static StringDictionary m_ResEntries = null;
      
        public static void Init(string sharedResFileName = "sharedRes.txt")
        {
            m_ResEntries = new StringDictionary();

            // l'applicativo quando parte ha conoscenza di tutte le risorse esterne alle pagine che potrà
            // utilizzare. Carica quindi la mappa con i path assoluti a tali risorse
            // Questa classe poi si preoccupa di verificare il proprio file delle entry e se manca un hash
            // aggiunge la risorsa a IPFS. 
            // Quando l'applicativo ha bisogno di generare una pagina per il web, allora sostituisce i nomi
            // alle risorse locali, con gli url IPFS che questa classe gli fornisce

            m_SharedResFilePath = string.Format("{0}\\{1}", Globals.IPFSOutFolder(), sharedResFileName);
            //LoadSharedResFile();
        }
        
        public static void AddResource(string filePath)
        {
            filePath = Utils.AddQuotesIfRequired(filePath);
            m_ResEntries.Add(filePath, "");
        }

        public static void Reset()
        {
            m_ResEntries.Clear();
        }

        public static void SynchToIPFS()
        {
            if (!IPFSUtils.IsIPFSDaemonRunning())
                IPFSUtils.Init();

            Globals.m_Logger.Info("Start synch shared resources to IPFS");

            //LoadSharedResFile();

            Globals.m_Logger.Info(string.Format("{0} shared resources already added to IPFS", m_ResEntries.Count));

            StringDictionary dictUpdate = new StringDictionary();

            int newItemAdded = 0;
            bool bChanged = false;
            ICollection keys = m_ResEntries.Keys;
            IEnumerator entries = keys.GetEnumerator();
            string path = "";
            while (entries.MoveNext())
            {
                path = (string)entries.Current;
                if (IPFSUtils.Add(path, false) == IPFSRet.Ok)
                {
                    string hashAdd = IPFSUtils.GetLastAddedHash();
                    if (m_ResEntries[path] != hashAdd)
                    {   
                        dictUpdate.Add(path, hashAdd);
                        newItemAdded++;
                        bChanged = true;
                    }
                }
            }

            if (bChanged)
            {
                m_ResEntries = dictUpdate;                
                Globals.m_Logger.Info(string.Format("{0} resources synchronized to IPFS", newItemAdded));
            }
        }

        /***
         * the url is get by the resource name only
         */
        public static string GetResourceUrl(string resourceFileName)
        {
            string hash = "";
            ICollection keys = m_ResEntries.Keys;
            IEnumerator entries = keys.GetEnumerator();
            string path = "";
            while (entries.MoveNext())
            {
                path = (string)entries.Current;                
                if (Path.GetFileName(Utils.RemoveQuotes(path)).ToLower().Contains(resourceFileName.ToLower()))
                {
                    hash = m_ResEntries[path];
                    break;
                }
            }
            if (hash.Length == 0)
                return "";
            return "https://ipfs.io/ipfs/" + hash;
        }

        public static bool LoadSharedResFile()
        {
            if (!File.Exists(m_SharedResFilePath))
                return false;

            m_ResEntries.Clear();
            string[] entries = File.ReadAllLines(m_SharedResFilePath);
            for (int iEnt = 0; iEnt < entries.Count(); iEnt++)
            {
                string ent = entries[iEnt];
                string[] kv = ent.Split(';');
                if (kv.Count() < 2)
                    continue;
                string key = kv[0];
                string path = kv[1];
                m_ResEntries.Add(key, path);
            }
            return true;
        }

    }
}
