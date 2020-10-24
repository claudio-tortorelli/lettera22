using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using Lettera22.Common;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Lettera22.Metadoc
{
    public class MetaDoc
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
        private int m_Id;
        private string m_Header;
        private string m_Title;
        private string m_SubTitle;
        private string m_Author;
        private string m_Place;
        private bool m_bShowPublishDate;
        private bool m_bShowRevision;
        private int m_Revision;
        private bool m_bShowRebuildDate;
        private string m_Abstract;
        private bool m_bSummary;
        private string m_Introduction;
        private Unit m_Unit;
        private string m_Bibliography;
        private string m_Category;
        private int m_FileVer;
        private string m_currentSHA256;
        private bool m_bShowInGlobalIndex;
        private bool m_bShowUnitNumber;
        private bool m_bNoProcess;
        private string m_Dedication;
        private string m_CreationDate;
        private List<DocHash> m_VersionHashes;

        private string m_FileInPath;

        public MetaDoc()
        {
            m_Id = 0;
            m_Header = "";
            m_Title = "";
            m_SubTitle = "";
            m_Author = "";
            m_Place = "";
            m_bShowPublishDate = false;
            m_Revision = 1;       
            m_bShowRevision = false;
            m_bShowRebuildDate = false;
            m_Abstract = "";
            m_bSummary = true;
            m_Introduction = "";
            m_Unit = new Unit(0);
            m_Bibliography = "";
            m_Category = "";
            m_currentSHA256 = "";
            m_bShowInGlobalIndex = true;
            m_bShowUnitNumber = false;
            m_bNoProcess = false;
            m_Dedication = "";
            m_CreationDate = "";
            m_VersionHashes = new List<DocHash>();

            m_FileInPath = "";

            m_FileVer = 103;
        }

        public int GetId() { return m_Id; }
        public void SetId(int id) { m_Id = id; }

        public string GetHeader() { return m_Header; }
        public void SetHeader(string header) { m_Header = header; }

        public string GetTitle() { return m_Title; }
        public void SetTitle(string title) { m_Title = title; }

        public string GetSubTitle() { return m_SubTitle; }
        public void SetSubTitle(string subtitle) { m_SubTitle = subtitle; }

        public string GetAuthor() { return m_Author; }
        public void SetAuthor(string author) { m_Author = author; }

        public string GetPlace() { return m_Place; }
        public void SetPlace(string place) { m_Place = place; }

        public int GetRevision() { return m_Revision; }
        public void SetRevision(int rev) { m_Revision = rev; }

        public bool IsShowPublishDate() { return m_bShowPublishDate; }
        public void SetShowPublishDate(bool bVal) { m_bShowPublishDate = bVal; }

        public bool IsShowRebuildDate() { return m_bShowRebuildDate; }
        public void SetShowRebuildDate(bool bVal) { m_bShowRebuildDate = bVal; }

        public bool IsShowRevision() { return m_bShowRevision; }
        public void SetShowRevision(bool bVal) { m_bShowRevision = bVal; }

        public string GetAbstract() { return m_Abstract; }
        public void SetAbstract(string abstr) { m_Abstract = abstr; }

        public bool IsSummaryEnabled() { return m_bSummary; }
        public void SetSummaryEnabled(bool bVal) { m_bSummary = bVal; }

        public string GetIntro() { return m_Introduction; }
        public void SetIntro(string intro) { m_Introduction = intro; }

        public Unit GetUnit() { return m_Unit; }
        public void SetUnit(Unit unit) { m_Unit = unit; }

        public string GetBibliography() { return m_Bibliography; }
        public void SetBibliography(string biblio) { m_Bibliography = biblio; }

        public string GetCategory() { return m_Category; }
        public void SetCategory(string category) { m_Category = category; }

        public int GetFileVer() { return m_FileVer; }

        public string GetCurrentHash() { return m_currentSHA256; }
        public void SetCurrentHash(string sha256) { m_currentSHA256 = sha256; }

        public bool IsShowInGlobalIndex() { return m_bShowInGlobalIndex; }
        public void SetShowInGlobalIndex(bool bVal) { m_bShowInGlobalIndex = bVal; }

        public bool IsShowUnitNumber() { return m_bShowUnitNumber; }
        public void SetShowUnitNumber(bool bVal) { m_bShowUnitNumber = bVal; }

        public bool IsNoProcess() { return m_bNoProcess; }
        public void SetNoProcess(bool bVal) { m_bNoProcess = bVal; }

        public string GetDedication() { return m_Dedication; }
        public void SetDedication(string dedication) { m_Dedication = dedication; }

        public string GetPublicIndexUrl() { return Globals.GetMainUrl() + "\\" + Globals.IndexFileName(); }

        public List<DocHash> GetHashes() { return m_VersionHashes; }
        public void AddHash(DocHash hash) { m_VersionHashes.Insert(0, hash); }

        public bool IsAlreadyPublished()
        {
            if (GetHashes().Count == 0)
                return false;
            if (GetHashes()[0].GetHash().Equals(GetCurrentHash()))
                return true;
            return false;
        }
                
        public bool Save(string contentXMLPath = "", bool bDeleteExist = false)
        {
            try
            {
                if (contentXMLPath.Length == 0)
                    contentXMLPath = m_FileInPath;

                if (contentXMLPath.Length == 0)
                    return false;

                if (bDeleteExist && File.Exists(contentXMLPath))
                    File.Delete(contentXMLPath);

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;

                using (XmlWriter writer = XmlWriter.Create(contentXMLPath, settings))
                {
                    writer.WriteStartDocument();
                    string comment = string.Format("This file is generated by {0} version {1}", Globals.SoftwareName(), Globals.SoftwareVersion());
                    writer.WriteComment(comment);

                    writer.WriteStartElement("Content");

                    ////////////////
                    writer.WriteStartElement("FileVer");
                    writer.WriteAttributeString("value", m_FileVer.ToString());
                    writer.WriteEndElement();

                    writer.WriteStartElement("Id");
                    writer.WriteAttributeString("value", m_Id.ToString());
                    writer.WriteEndElement();

                    writer.WriteStartElement("curHash");
                    writer.WriteAttributeString("value", m_currentSHA256);
                    writer.WriteEndElement();

                    writer.WriteStartElement("NoGlobal");
                    writer.WriteAttributeString("value", (!IsShowInGlobalIndex()).ToString());
                    writer.WriteEndElement();

                    writer.WriteStartElement("ShowUnitNumber");
                    writer.WriteAttributeString("value", (IsShowUnitNumber()).ToString());
                    writer.WriteEndElement();

                    writer.WriteStartElement("Header");
                    writer.WriteAttributeString("value", m_Header);
                    writer.WriteEndElement();

                    writer.WriteStartElement("Title");
                    writer.WriteAttributeString("value", m_Title);
                    writer.WriteEndElement();

                    writer.WriteStartElement("SubTitle");
                    writer.WriteAttributeString("value", m_SubTitle);
                    writer.WriteEndElement();

                    writer.WriteStartElement("Author");
                    writer.WriteAttributeString("value", m_Author);
                    writer.WriteEndElement();

                    writer.WriteStartElement("Place");
                    writer.WriteAttributeString("value", m_Place);
                    writer.WriteEndElement();

                    writer.WriteStartElement("PublishDate");
                    writer.WriteAttributeString("enabled", m_bShowPublishDate.ToString());
                    writer.WriteEndElement();

                    writer.WriteStartElement("Revision");
                    writer.WriteAttributeString("enabled", m_bShowRevision.ToString());
                    writer.WriteAttributeString("value", m_Revision.ToString());
                    writer.WriteEndElement();

                    writer.WriteStartElement("RebuildDate");
                    writer.WriteAttributeString("enabled", m_bShowRebuildDate.ToString());
                    writer.WriteEndElement();

                    writer.WriteStartElement("Abstract");
                    writer.WriteAttributeString("value", m_Abstract);
                    writer.WriteEndElement();

                    writer.WriteStartElement("Summary");
                    writer.WriteAttributeString("value", m_bSummary.ToString());
                    writer.WriteEndElement();

                    writer.WriteStartElement("Introduction");
                    writer.WriteAttributeString("value", m_Introduction);
                    writer.WriteEndElement();

                    m_Unit.ToXML(writer);                        

                    writer.WriteStartElement("Bibliography");
                    writer.WriteAttributeString("value", m_Bibliography);
                    writer.WriteEndElement();

                    writer.WriteStartElement("Category");
                    writer.WriteAttributeString("value", m_Category);
                    writer.WriteEndElement();

                    writer.WriteStartElement("Hashes");
                    writer.WriteAttributeString("value", m_VersionHashes.Count.ToString());
                    for (int iH = 0; iH < m_VersionHashes.Count; iH++)
                    {
                        m_VersionHashes[iH].ToXML(writer);
                    }
                    writer.WriteEndElement();

                    writer.WriteStartElement("NoProcess");
                    writer.WriteAttributeString("value", IsNoProcess().ToString());
                    writer.WriteEndElement();

                    writer.WriteStartElement("Dedication");
                    writer.WriteAttributeString("value", m_Dedication);
                    writer.WriteEndElement();

                    writer.WriteStartElement("CreationDate");
                    writer.WriteAttributeString("value", m_CreationDate);
                    writer.WriteEndElement();

                    ////////////////

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                    writer.Flush();
                    writer.Close();
                }
            }
            catch (System.Exception ex)
            {
                Globals.m_Logger.Error(ex.ToString());    
                return false;
            }
            return true;
        }

        public bool Load(string contentXMLPath)
        {
            try
            {
                if (contentXMLPath.Length == 0 || !File.Exists(contentXMLPath))
                    return false;

                m_FileInPath = contentXMLPath;

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.DtdProcessing = DtdProcessing.Parse;
                settings.IgnoreWhitespace = true;

                using (XmlReader reader = XmlReader.Create(contentXMLPath, settings))
                {
                    while (reader.Read())
                    {
                        switch (reader.Name)
                        {                              
                            case "FileVer":
                            {
                                if (reader.GetAttribute("value") != null)
                                    m_FileVer = int.Parse(reader["value"]);
                                break;
                            }
                            case "Id":
                            {
                                if (reader.GetAttribute("value") != null)
                                    m_Id = int.Parse(reader["value"]);
                                break;
                            }
                            case "curHash":
                            {
                                if (reader.GetAttribute("value") != null)
                                    m_currentSHA256 = reader["value"];
                                break;
                            }
                            case "NoGlobal":
                            {
                                if (reader.GetAttribute("value") != null)
                                    m_bShowInGlobalIndex = !bool.Parse(reader["value"]);
                                break;
                            }
                            case "ShowUnitNumber":
                            {
                                if (reader.GetAttribute("value") != null)
                                    m_bShowUnitNumber = bool.Parse(reader["value"]);
                                break;
                            }                            
                            case "Header":
                            {
                                if (reader.GetAttribute("value") != null)
                                    m_Header = reader["value"];
                                break;
                            }
                            case "Title":
                            {
                                if (reader.GetAttribute("value") != null)
                                    m_Title = reader["value"];
                                break;
                            }
                            case "SubTitle":
                            {
                                if (reader.GetAttribute("value") != null)
                                    m_SubTitle = reader["value"];
                                break;
                            }
                            case "Author":
                            {
                                if (reader.GetAttribute("value") != null)
                                    m_Author = reader["value"];
                                break;
                            }
                            case "Place":
                            {
                                if (reader.GetAttribute("value") != null)
                                    m_Place = reader["value"];
                                break;
                            }
                            case "PublishDate":
                            {
                                if (reader.GetAttribute("enabled") != null)
                                {
                                    if (!bool.TryParse(reader["enabled"], out m_bShowPublishDate))
                                        m_bShowPublishDate = false;
                                }
                                break;
                            }
                            case "Revision":
                            {
                                if (reader.GetAttribute("value") != null)
                                {
                                    if (!int.TryParse(reader.GetAttribute("value"), out m_Revision))
                                        m_Revision = 1;
                                }
                                if (reader.GetAttribute("enabled") != null)
                                {
                                    if (!bool.TryParse(reader["enabled"], out m_bShowRevision))
                                        m_bShowRevision = false;
                                }
                                break;
                            }
                            case "RebuildDate":
                            {                                
                                if (reader.GetAttribute("enabled") != null)
                                {
                                    if (!bool.TryParse(reader["enabled"], out m_bShowRebuildDate))
                                        m_bShowRebuildDate = false;
                                }
                                break;
                            }
                            case "Abstract":
                            {
                                if (reader.GetAttribute("value") != null)
                                    m_Abstract = reader["value"];
                                break;
                            }
                            case "Summary":
                            {
                                if (!bool.TryParse(reader["value"], out m_bSummary))
                                    m_bSummary = false;
                                break;
                            }
                            case "Introduction":
                            {
                                if (reader.GetAttribute("value") != null)
                                    m_Introduction = reader["value"];
                                break;
                            }
                            case "Unit":
                            {
                                m_Unit = new Unit(0);
                                m_Unit.FromXML(reader);                                
                                break;
                            }
                            case "Bibliography":
                            {
                                if (reader.GetAttribute("value") != null)
                                    m_Bibliography = reader["value"];
                                break;
                            }
                            case "Category":
                            {
                                if (reader.GetAttribute("value") != null)
                                    m_Category = reader["value"];
                                break;
                            }
                            case "Hashes":
                            {
                                int nHashes = 0;
                                if (reader.GetAttribute("value") != null)
                                {
                                    if (!int.TryParse(reader.GetAttribute("value"), out nHashes))
                                        nHashes = 0;
                                }
                                if (nHashes > 0)
                                    m_VersionHashes.Clear();                                
                                for (int iH = 0; iH < nHashes; iH++)
                                {
                                    reader.Read();

                                    DocHash curHash = new DocHash("", "");
                                    curHash.FromXML(reader);
                                    m_VersionHashes.Add(curHash);
                                }
                                break;
                            }
                            case "NoProcess":
                            {
                                if (reader.GetAttribute("value") != null)
                                    m_bNoProcess = bool.Parse(reader["value"]);
                                break;
                            }
                            case "Dedication":
                            {
                                if (reader.GetAttribute("value") != null)
                                    m_Dedication = reader["value"];
                                break;
                            }
                            case "CreationDate":
                            {
                                if (reader.GetAttribute("value") != null)
                                    m_CreationDate = reader["value"];
                                break;
                            } 
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Globals.m_Logger.Error(ex.ToString());
                return false;
            }
            return true;
        }

        /**
         * check for md5 consistency with original text file
         */
        public bool IsConsistent()
        {
            if (!Globals.IsCheckingHash())
                return true;

            string txtFilePath = Utils.ChangePathExtension(m_FileInPath, ".txt");
            return GetCurrentHash().Equals(Utils.GetHashSha256(txtFilePath));
        }

        public string GetCreationDate()
        {
            if (m_FileInPath.Length == 0)
                return "";

            if (!File.Exists(m_FileInPath))
                return "";

            if (m_CreationDate.Length > 0)
                return m_CreationDate;
            
            // use the file date
            return File.GetCreationTime(m_FileInPath).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);            
        }

        public void SetCreationDate(string cDate)
        {
            m_CreationDate = cDate;
        }

        public string GetLastRevisionDate(bool bComparable = false)
        {
            if (m_FileInPath.Length == 0)
                return "";

            if (!File.Exists(m_FileInPath))
                return "";

            if (!bComparable)
                return File.GetLastWriteTime(m_FileInPath).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            return File.GetLastWriteTime(m_FileInPath).ToString("yyyyMMdd", CultureInfo.InvariantCulture);
        }

        public string GetFileName()
        {
            if (m_FileInPath.Length == 0)
                return "";
            return Path.GetFileName(m_FileInPath);   
        }

        public string GetFilePath()
        {
            return m_FileInPath;
        }

        public bool HasLinks(Unit curUnit)
        {
            if (curUnit == null)
                curUnit = m_Unit;
            for (int idContent = 0; idContent < curUnit.GetContentCount(); idContent++)
            {
                Content content = curUnit.GetContentByPos(idContent);
                if (content == null)
                    continue;
                if (content.GetType() == typeof(BiblioRef))
                    return true;                
            }
            for (int iSubUnit = 0; iSubUnit < curUnit.GetUnitCount(); iSubUnit++)
            {
                if (HasLinks(curUnit.GetUnitByPos(iSubUnit)))
                    return true;
            }
            return false;
        }
    }
}
