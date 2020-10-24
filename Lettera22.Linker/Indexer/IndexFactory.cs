using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lettera22.Common;
using System.IO;
using Lettera22.Metadoc;

namespace Lettera22.Linker
{
    public class IndexerFactory
    {
        /// <project>Lettera22</project>
        /// <copyright company="Claudio Tortorelli">
        /// Copyright (c) 2017 All Rights Reserved
        /// </copyright>        
        /// <author>Claudio Tortorelli</author>
        /// <email>claudio.tortorelli@gmail.com</email>
        /// <web>http://www.claudiotortorelli.it</web>
        /// <date>May 2017</date>
        /// <summary>
        /// Metadoc html index builder
        /// It builds a index.html with some data from each text and a sortable table
        /// to search them.
        /// </summary>
        /// https://choosealicense.com/licenses/mit/
        public IndexerFactory()
        {
        }

        public bool BuildIndex(List<MetaDoc> textWorks)
        {
            try
            {
                IndexSkeleton skeleton = null;
                try
                {
                    if (Globals.IndexLibrary().Equals("sorttable.js", StringComparison.InvariantCultureIgnoreCase))
                        skeleton = new SortTableSkeleton(textWorks);
                }
                catch (Exception ex)
                {
                    Globals.m_Logger.Error("Index build error: " + ex.ToString());
                    return false;
                }

                string htmlOut = "";
                if (skeleton != null)
                {
                    Globals.m_Logger.Info("Start building the index");
                    htmlOut = skeleton.BuildIt();
                    Globals.m_Logger.Info("End building the index");
                }
                if (htmlOut.Length == 0)
                {
                    Globals.m_Logger.Error("Index library not recognized");
                    return false;
                }

                string indexFilePath = Globals.IndexFolder() + Globals.IndexFileName();
                if (File.Exists(indexFilePath))
                    File.Delete(indexFilePath);
                using (FileStream fs = new FileStream(indexFilePath, FileMode.Create))
                {
                    using (StreamWriter sw = new StreamWriter(fs, new UTF8Encoding(false))) // write document as UTF-8 with Byte Order Mark
                    {
                        sw.Write(htmlOut);
                    }
                }
                
                // setup script
                string scriptSource = Utils.NormalizePath(Globals.IndexResourceFolder() + Path.DirectorySeparatorChar + skeleton.GetScriptFileName());
                string scriptDest = Utils.NormalizePath(Globals.IndexFolder() + Path.DirectorySeparatorChar + skeleton.GetScriptFileName());
                if (!File.Exists(scriptDest))
                {
                    Globals.m_Logger.Warn("Missing script file " + skeleton.GetScriptFileName() + " was copied");
                    File.Copy(scriptSource, scriptDest);
                }
                
                return File.Exists(indexFilePath);
            }
            catch (Exception ex)
            {
                Globals.m_Logger.Error(ex.ToString());
                return false;
            }
        }
    }
}
