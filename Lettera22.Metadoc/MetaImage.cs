using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using Lettera22.Common;

namespace Lettera22.Metadoc
{
    public class MetaImage : Content
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

        public MetaImage(int id, string content)
            : base(id, "")        
        {
            SetBase64Content(content);
        }

        public override bool ToXML(XmlWriter writer)
        {
            if (writer == null)
                return false;

            try
            {
                writer.WriteStartElement("Image");
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

        public void SetBase64Content(string base64Image)
        {
            if (Utils.IsBase64(base64Image))
                SetContent(base64Image);
        }

        public string GetBase64Content()
        {
            if (Utils.IsBase64(m_ContentStr))
                return m_ContentStr;
            return "";
        }
    }
}
