using Lettera22.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lettera22.Compiler
{
    class ExternalResource
    {
        /// <project>Lettera22</project>
        /// <copyright company="Claudio Tortorelli">
        /// Copyright (c) 2018 All Rights Reserved
        /// </copyright>        
        /// <author>Claudio Tortorelli</author>
        /// <email>claudio.tortorelli@gmail.com</email>
        /// <web>http://www.claudiotortorelli.it</web>
        /// <date>October 2018</date>
        /// <summary>
        /// This class represent an external resource of template and index
        /// </summary>
        /// https://choosealicense.com/licenses/mit/
        private string m_FileName; // id
        private string m_FilePath; 
        private string m_HttpURL;
        private string m_IPFSURL;

        public ExternalResource(string filePath)
        {
            m_FileName = Path.GetFileName(filePath);
            m_FilePath = filePath;
        }

        public string GetFileName() { return m_FileName; }
        public string GetFilePath() { return m_FilePath; }
        public string GetHttpURL() { return m_HttpURL; }
        public void SetHttpURL(string url) { m_HttpURL = url; }
        public string GetIPFSURL() { return m_IPFSURL; }
        public void SetIPFSURL(string url) { m_IPFSURL = url; }

    }
}
