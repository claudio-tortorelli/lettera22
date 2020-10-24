using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using Lettera22.Common;

namespace Lettera22.Metadoc
{
    public class Quote : Content
    {
        /// <project>Lettera22</project>
        /// <copyright company="Claudio Tortorelli">
        /// Copyright (c) 2016 All Rights Reserved
        /// </copyright>        
        /// <author>Claudio Tortorelli</author>
        /// <email>claudio.tortorelli@gmail.com</email>
        /// <web>http://www.claudiotortorelli.it</web>
        /// <date>April 2016</date>
        /// <summary>
        /// </summary>
        /// https://choosealicense.com/licenses/mit/

        public Quote(int id, string content)
            : base(id, content)
        {
            m_bMultiline = true;
        }

        public override bool ToXML(XmlWriter writer)
        {
            if (writer == null)
                return false;

            try
            {
                writer.WriteStartElement("Quote");
                writer.WriteAttributeString("id", GetId().ToString());
                writer.WriteAttributeString("content", GetContent());
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
            }
            catch (Exception ex)
            {
                Globals.m_Logger.Error(ex.ToString());
                return false;
            }
            return true;
        }

        public void AddText(string text, bool bAddBR = true)
        {
            string curContent = GetContent();
            curContent += text;
            if (bAddBR)
                curContent += "\n";
            SetContent(curContent);
        }
    }
}
