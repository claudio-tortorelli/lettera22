﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Drawing;
using Lettera22.Common;

namespace Lettera22.Metadoc
{
    public class ItemList : Content
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

        protected bool m_bOrdered;

        public ItemList(int id, string content)
            : base(id, content)
        {
            m_bOrdered = false;
        }

        public void SetOrdered(bool bRet) { m_bOrdered = bRet; }
        public bool IsOrdered() { return m_bOrdered; }

        public override bool ToXML(XmlWriter writer)
        {
            if (writer == null)
                return false;

            try
            {
                writer.WriteStartElement("ItemList");
                writer.WriteAttributeString("id", GetId().ToString());
                writer.WriteAttributeString("content", GetContent());
                writer.WriteAttributeString("ordered", m_bOrdered.ToString());
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
                if (reader.GetAttribute("ordered") != null)
                {
                    if (!bool.TryParse(reader["ordered"], out m_bOrdered))
                        m_bOrdered = false;
                }
            }
            catch (Exception ex)
            {
                Globals.m_Logger.Error(ex.ToString());
                return false;
            }
            return true;
        }

        public void AddItem(string text, bool bAddBR = true)
        {
            string curContent = GetContent();
            curContent += ("<li>"+text+"</li>");
            if (bAddBR)
                curContent += "\n";
            SetContent(curContent);
        }
    }
}
