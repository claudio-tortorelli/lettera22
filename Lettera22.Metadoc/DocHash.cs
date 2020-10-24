using Lettera22.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Lettera22.Metadoc
{
    public class DocHash
    {
        /// <project>Lettera22</project>
        /// <copyright company="Claudio Tortorelli">
        /// Copyright (c) 2018 All Rights Reserved
        /// </copyright>        
        /// <author>Claudio Tortorelli</author>
        /// <email>claudio.tortorelli@gmail.com</email>
        /// <web>http://www.claudiotortorelli.it</web>
        /// <date>June 2018</date>
        /// <summary>
        /// </summary>
        /// https://choosealicense.com/licenses/mit/

        protected string m_Hash;
        protected string m_DateTime;

        public DocHash(string hash, string date = "")
            : base()
        {
            m_Hash = hash;
            if (date.Length == 0)
                m_DateTime = DateTime.Now.ToString();
            else
                m_DateTime = date;

        }

        public string GetGlobalUrl() { return Globals.GetMainUrl() + "/" + m_Hash + Globals.HTML_EXT; }
        public string GetHash() { return m_Hash; }
        public void SetHash(string hash) { m_Hash = hash; }
        public string GetDate() { return m_DateTime; }
        public void SetDate(string dateTime) { m_DateTime = dateTime; }

        public virtual bool ToXML(XmlWriter writer)
        {
            if (writer == null)
                return false;

            try
            {
                writer.WriteStartElement("HashVersion");
                writer.WriteAttributeString("hash", m_Hash);
                writer.WriteAttributeString("date", m_DateTime);
                writer.WriteEndElement();
            }
            catch (Exception ex)
            {
                Globals.m_Logger.Error(ex.ToString());
                return false;
            }

            return true;
        }

        public virtual bool FromXML(XmlReader reader)
        {
            if (reader == null)
                return false;

            try
            {
                if (reader.GetAttribute("hash") != null)
                    m_Hash = reader["hash"];
                if (reader.GetAttribute("date") != null)
                    m_DateTime = reader["date"];

                if (m_Hash.Length == 0 || m_DateTime.Length == 0)
                    throw new Exception("Invalid hash detected");
            }
            catch (Exception ex)
            {
                Globals.m_Logger.Error(ex.ToString());
                return false;
            }
            return true;
        }
    }
}
