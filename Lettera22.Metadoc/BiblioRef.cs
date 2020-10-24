using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using Lettera22.Common;

namespace Lettera22.Metadoc
{
    public class BiblioRef : Content
    {
        /// <project>Lettera22</project>
        /// <copyright company="Claudio Tortorelli">
        /// Copyright (c) 2016 All Rights Reserved
        /// </copyright>        
        /// <author>Claudio Tortorelli</author>
        /// <email>claudio.tortorelli@gmail.com</email>
        /// <web>http://www.claudiotortorelli.it</web>
        /// <date>September 2016</date>
        /// <summary>
        /// </summary>
        /// https://choosealicense.com/licenses/mit/

        private int m_ContentId; // content which contains the link 
        private int m_ContentPos; // position inside the content
        

        public BiblioRef(int id, string biblioRef)
            : base(id, "")
        {
            m_bEmbeddable = true;

            SetContent(biblioRef);
            m_ContentPos = 0;
            m_ContentId = 0;
        }

        public override bool ToXML(XmlWriter writer)
        {
            if (writer == null)
                return false;

            try
            {
                writer.WriteStartElement("BiblioRef");
                writer.WriteAttributeString("id", GetId().ToString());
                writer.WriteAttributeString("content", GetContent());
                writer.WriteAttributeString("contentId", m_ContentId.ToString());                
                writer.WriteAttributeString("pos", m_ContentPos.ToString());
                writer.WriteEndElement();
            }
            catch (Exception ex)
            {
                Globals.m_Logger.Error(ex.ToString());
                return false;
            }
            return true;
        }

        public override bool FromXML(XmlReader reader)
        {
            if (reader == null)
                return false;

            try
            {
                if (reader.GetAttribute("id") != null)
                    m_Id = int.Parse(reader["id"]);
                if (reader.GetAttribute("content") != null)
                    m_ContentStr = reader["content"];
                if (reader.GetAttribute("contentId") != null)
                    m_ContentId = int.Parse(reader["contentId"]);
                if (reader.GetAttribute("pos") != null)
                    m_ContentPos = int.Parse(reader["pos"]);
            }
            catch (Exception ex)
            {
                Globals.m_Logger.Error(ex.ToString());
                return false;
            }
            return true;
        }

        public void SetUrl(string url, bool bHttpsOnly = false)
        {
            if (Utils.IsValidUrl(url, bHttpsOnly))
                SetContent(url);
        }


        public void SetPos(int iPos)
        {
            m_ContentPos = Math.Max(0, iPos);
        }

        public int GetPos()
        {
            return m_ContentPos;
        }

        public void SetContentId(int id)
        {
            m_ContentId = Math.Max(0, id);
        }

        public int GetContentId()
        {
            return m_ContentId;
        }
    }
}
