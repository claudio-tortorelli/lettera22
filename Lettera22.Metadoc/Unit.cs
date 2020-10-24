using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using Lettera22.Common;

namespace Lettera22.Metadoc
{
    public class Unit
    {
        /// <project>Lettera22</project>
        /// <copyright company="Claudio Tortorelli">
        /// Copyright (c) 2016 All Rights Reserved
        /// </copyright>        
        /// <author>Claudio Tortorelli</author>
        /// <email>claudio.tortorelli@gmail.com</email>
        /// <web>http://www.claudiotortorelli.it</web>
        /// <date>July 2016</date>
        /// <summary>
        /// </summary>
        /// https://choosealicense.com/licenses/mit/

        protected int m_Id;        
        protected string m_Title;
        protected bool m_bShowTitle;
        protected bool m_bVisibileInSummary;
        protected List<Unit> m_SubUnits;
        protected List<Content> m_Contents;
        protected int m_Level;
        
        public Unit(int id) 
        {
            m_Id = id;
            m_Title = "";
            m_Level = 0;
            m_bShowTitle = true;
            m_bVisibileInSummary = true;

            m_SubUnits = new List<Unit>();
            m_Contents = new List<Content>();
        }

        public int GetId() { return m_Id; }
        public void SetId(int id) { m_Id = id; }
        public string GetTitle() { return m_Title; }
        public void SetTitle(string title) { m_Title = title; }
        public int GetLevel() { return m_Level; }
        public void SetLevel(int level) { m_Level = level; }
        public bool IsTitleVisible() { return m_bShowTitle; }
        public void SetTitleVisible(bool bVisible) { m_bShowTitle = bVisible; }
        public bool IsVisibleInSummary() { return m_bVisibileInSummary; }
        public void SetVisibleInSummary(bool bVisible) { m_bVisibileInSummary = bVisible; }
        public bool IsTitleNumerable() { return IsTitleVisible() && GetTitle().Length > 0 && IsVisibleInSummary(); }

        public virtual bool ToXML(XmlWriter writer)
        {
            if (writer == null)
                return false;

            try
            {
                writer.WriteStartElement("Unit");
                writer.WriteAttributeString("id", m_Id.ToString());
                writer.WriteAttributeString("title", m_Title);
                writer.WriteAttributeString("level", m_Level.ToString());
                writer.WriteAttributeString("titleVisible", m_bShowTitle.ToString());
                writer.WriteAttributeString("visibleInSummary", m_bVisibileInSummary.ToString());
                writer.WriteAttributeString("nSubUnit", m_SubUnits.Count().ToString());
                writer.WriteAttributeString("nContent", m_Contents.Count().ToString());
                foreach (Content content in m_Contents)
                {
                    if (content.GetType() == typeof(Paragraph))
                        ((Paragraph)content).ToXML(writer);
                    else if (content.GetType() == typeof(Quote))
                        ((Quote)content).ToXML(writer);
                    else if (content.GetType() == typeof(ItemList))
                        ((ItemList)content).ToXML(writer);
                    else if (content.GetType() == typeof(MetaImage))
                        ((MetaImage)content).ToXML(writer);
                    else if (content.GetType() == typeof(BiblioRef))
                        ((BiblioRef)content).ToXML(writer);
                    else if (content.GetType() == typeof(Attachment))
                        ((Attachment)content).ToXML(writer);
                    else
                        throw new Exception("Unknown content type!");
                }                
                foreach (Unit unit in m_SubUnits)
                    unit.ToXML(writer);
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

            int nSubUnit = 0;
            int nContent = 0;

            m_SubUnits.Clear();
            m_Contents.Clear();
            try
            {
                if (reader.GetAttribute("id") != null)
                    m_Id = int.Parse(reader["id"]);
                if (reader.GetAttribute("title") != null)
                    m_Title = reader["title"];
                if (reader.GetAttribute("level") != null)
                    m_Level = int.Parse(reader["level"]);
                if (reader.GetAttribute("titleVisible") != null)
                    m_bShowTitle = bool.Parse(reader["titleVisible"]);
                if (reader.GetAttribute("visibleInSummary") != null)
                    m_bVisibileInSummary = bool.Parse(reader["visibleInSummary"]);
                if (reader.GetAttribute("nSubUnit") != null)
                    nSubUnit = int.Parse(reader["nSubUnit"]);
                if (reader.GetAttribute("nContent") != null)
                    nContent = int.Parse(reader["nContent"]);
                            
                // contents
                for (int i = 0; i < nContent; i++)
                {
                    reader.Read();
                    switch (reader.Name)
                    {
                        case "Paragraph":
                        {
                            Paragraph itemRead = new Paragraph(i, "");
                            if (itemRead.FromXML(reader))
                                m_Contents.Add(itemRead);
                            break;
                        }
                        case "ItemList":
                        {
                            ItemList itemRead = new ItemList(i, "");
                            if (itemRead.FromXML(reader))
                                m_Contents.Add(itemRead);
                            break;
                        }
                        case "Quote":
                        {
                            Quote itemRead = new Quote(i, "");
                            if (itemRead.FromXML(reader))
                                m_Contents.Add(itemRead);
                            break;
                        }
                        case "Image":
                        {
                            MetaImage itemRead = new MetaImage(i, "");
                            if (itemRead.FromXML(reader))
                                m_Contents.Add(itemRead);
                            break;
                        }
                        case "BiblioRef":
                        {
                            BiblioRef itemRead = new BiblioRef(i, "");
                            if (itemRead.FromXML(reader))
                                m_Contents.Add(itemRead);
                            break;
                        }
                        case "Attachment":
                        {
                            Attachment itemRead = new Attachment(i, "", "");
                            if (itemRead.FromXML(reader))
                                m_Contents.Add(itemRead);
                            break;
                        }
                        default:
                            throw new Exception("Unknown content type!");
                    }
                }
                for (int i = 0; i < nSubUnit; i++)
                {
                    reader.Read();
                    Unit itemRead = new Unit(0);
                    if (itemRead.FromXML(reader))
                        m_SubUnits.Add(itemRead);                                
                }
                if (nContent > 0 || nSubUnit > 0)
                    reader.Read();                       
            }
            catch (Exception ex)
            {
                Globals.m_Logger.Error(ex.ToString());
                return false;
            }
            return true;
        }

        public bool AddUnit(Unit unit)
        {
            m_SubUnits.Add(unit);
            return true;
        }

        public bool RemoveUnit(int id)
        {
            for (int i = 0; i < m_SubUnits.Count(); i++)
            {
                if (m_SubUnits[i].GetId() == id)
                {
                    m_SubUnits.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public Unit GetUnitById(int id)
        {
            for (int i = 0; i < m_SubUnits.Count(); i++)
            {
                if (m_SubUnits[i].GetId() == id)
                    return m_SubUnits[i];             
            }
            return null;
        }

        public Unit GetUnitByPos(int index)
        {
            if (index < 0 || index >= m_SubUnits.Count)
                return null;
            return m_SubUnits[index];
        }

        public int GetUnitCount() { return m_SubUnits.Count; }

        public bool AddContent(Content content)
        {
            m_Contents.Add(content);
            return true;
        }

        public bool RemoveContent(int id)
        {
            for (int i = 0; i < m_SubUnits.Count(); i++)
            {
                if (m_Contents[i].GetId() == id)
                {
                    m_Contents.RemoveAt(id);
                    return true;
                }
            }            
            return false;
        }

        public Content GetContent(int id)
        {
            for (int i = 0; i < m_SubUnits.Count(); i++)
            {
                if (m_Contents[i].GetId() == id)
                    return m_Contents[i];             
            }
            return null;
        }

        public Content GetContentByPos(int index)
        {
            if (index < 0 || index >= m_Contents.Count)
                return null;
            return m_Contents[index];            
        }

        public Content GetLastContent()
        {
            if (GetContentCount() == 0)
                return null;
            return m_Contents[GetContentCount()-1];
        }

        public int GetContentCount() { return m_Contents.Count; }
    }
}
