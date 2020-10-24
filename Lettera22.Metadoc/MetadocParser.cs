using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Lettera22.Common;
using System.Drawing.Imaging;
using Lettera22.Metadoc;

namespace Lettera22.Parser
{
    public class MetadocParser
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

        public enum RetCode
        {
            NO_ERR = 0,
            ERR_WRONGPATH = 1,
            ERR_FILENOTEXIST = 2,
            ERR_FILEEMPTY = 3,
            ERR_EXCEPTION = 4,
            ERR_INVALIDTREE = 5
        };

        public enum ParseCode
        {
            NO_PARSE = 0,
            PARSED = 1,
            ERROR = 2
        };

        // TAGS
        protected struct Tag
        {
            public string Comment;
            public string Header;
            public string Title;
            public string Subtitle;
            public string Author;
            public string Place;
            public string PublishDate;
            public string Revision;
            public string RebuildDate;
            public string Abstract;
            public string Summary;
            public string Intro;
            public string Bibliography;
            public string Start;
            public string NoGlobal;
            public string ShowUnitNumber;
            public string UnitStart;
            public string UnitNoSummary;
            public string UnitNoTitle;
            public string BiblioRef;
            public string Attachment;
            public string Image;
            public string Quote;
            public string List;
            public string ListOrd;
            public string Category;
            public string NoProcess;
            public string Dedication;
            public string Creation;
        };

        protected string m_FileInPath = "";
        protected RetCode m_RetCode = RetCode.NO_ERR;
        protected Tag m_Tags = new Tag();
        protected MetaDoc m_ParsedWork = null;

        public MetadocParser(string FileInPath)
        {
            // define the supported syntax - start
            m_Tags.Comment = "#";
            m_Tags.Header = "[header]";
            m_Tags.Title = "[title]";
            m_Tags.Subtitle = "[subtitle]";
            m_Tags.Author = "[author]";
            m_Tags.Place = "[place]";
            m_Tags.PublishDate = "[showPublishDate]";
            m_Tags.Revision = "[showRevision]";
            m_Tags.RebuildDate = "[showRebuildDate]";
            m_Tags.Abstract = "[abstract]";
            m_Tags.Summary = "[noSummary]";
            m_Tags.Intro = "[introduction]";
            m_Tags.Start = "[start]";
            m_Tags.NoGlobal = "[noGlobal]";
            m_Tags.NoProcess = "[noProcess]";
            m_Tags.Dedication = "[dedication]";
            m_Tags.Creation = "[creation]";            
            m_Tags.ShowUnitNumber = "[showUnitNumber]";
            m_Tags.UnitStart = "::";
            m_Tags.UnitNoSummary = "::noSummary";
            m_Tags.UnitNoTitle = "::noTitle";
            m_Tags.Image = "[i:";
            m_Tags.BiblioRef = "[b:";
            m_Tags.Attachment = "[a:";
            m_Tags.Quote = "''";
            m_Tags.List = "-";
            m_Tags.ListOrd = "*";
            m_Tags.Category = "[category]";
            // define the supported syntax - end

            m_FileInPath = FileInPath;
            m_RetCode = Parse();
        }

        public bool IsParsed() { return (m_RetCode == RetCode.NO_ERR); }
        public RetCode GetRetCode() { return m_RetCode; }

        /**
         * this methods parse the input text line by line and allocate related units and their contents
         */ 
        protected RetCode Parse()
        {
            if (m_FileInPath.Length == 0)
                return RetCode.ERR_WRONGPATH;

            if (!File.Exists(m_FileInPath))
                return RetCode.ERR_FILENOTEXIST;

            string[] lines = File.ReadAllLines(m_FileInPath);
            if (lines.Length == 0)
                return RetCode.ERR_FILEEMPTY;

            m_ParsedWork = new MetaDoc();

            int curLevel = 1;
            bool bInsideContent = false;
            bool bParsingIntro = false;
                    
            int unitContentCount = 1;

            Unit curUnit = null;
            List<Unit> unitList = new List<Unit>();

            string line = ""; // line currently read
            string prevLine = "";
            string introText = "";

            try
            {
                for (int iLine = 0; iLine < lines.Length; iLine++)
                {
                    prevLine = line;
                    line = lines[iLine];

                    // skip comments
                    if (line.StartsWith(m_Tags.Comment))
                        continue;

                    if (line.Length == 0 && !bInsideContent) // keep empty line inside units only
                        continue;

                    if (!bInsideContent)
                    {
                        // here parse the document's general attribute tags
                        if (line.IndexOf(m_Tags.Header) >= 0)
                        {
                            m_ParsedWork.SetHeader(StripTag(line, m_Tags.Header));
                            bParsingIntro = false;
                        }
                        else if (line.IndexOf(m_Tags.Title) >= 0)
                        {
                            m_ParsedWork.SetTitle(StripTag(line, m_Tags.Title));
                            bParsingIntro = false;
                        }
                        else if (line.IndexOf(m_Tags.Subtitle) >= 0)
                        {
                            m_ParsedWork.SetSubTitle(StripTag(line, m_Tags.Subtitle));
                            bParsingIntro = false;
                        }
                        else if (line.IndexOf(m_Tags.Author) >= 0)
                        {
                            m_ParsedWork.SetAuthor(StripTag(line, m_Tags.Author));
                            bParsingIntro = false;
                        }
                        else if (line.IndexOf(m_Tags.Place) >= 0)
                        {
                            m_ParsedWork.SetPlace(StripTag(line, m_Tags.Place));
                            bParsingIntro = false;
                        }
                        else if (line.IndexOf(m_Tags.PublishDate) >= 0)
                        {
                            m_ParsedWork.SetShowPublishDate(true);
                            bParsingIntro = false;
                        }
                        else if (line.IndexOf(m_Tags.Revision) >= 0)
                        {
                            m_ParsedWork.SetShowRevision(true);
                            bParsingIntro = false;
                        }
                        else if (line.IndexOf(m_Tags.RebuildDate) >= 0)
                        {
                            m_ParsedWork.SetShowRebuildDate(true);
                            bParsingIntro = false;
                        }
                        else if (line.IndexOf(m_Tags.Abstract) >= 0)
                        {
                            m_ParsedWork.SetAbstract(StripTag(line, m_Tags.Abstract));
                            bParsingIntro = false;
                        }
                        else if (line.IndexOf(m_Tags.Summary) >= 0)
                        {
                            m_ParsedWork.SetSummaryEnabled(false);
                            bParsingIntro = false;
                        }
                        else if (line.IndexOf(m_Tags.Intro) >= 0)
                        {
                            introText += line;
                            bParsingIntro = true;
                        }                        
                        else if (line.IndexOf(m_Tags.Category) >= 0)
                        {
                            m_ParsedWork.SetCategory(StripTag(line, m_Tags.Category));
                            bParsingIntro = false;
                        }
                        else if (line.IndexOf(m_Tags.NoGlobal) >= 0)
                        {
                            m_ParsedWork.SetShowInGlobalIndex(false);
                            bParsingIntro = false;
                        }
                        else if (line.IndexOf(m_Tags.NoProcess) >= 0)
                        {
                            m_ParsedWork.SetNoProcess(true);
                            bParsingIntro = false;
                        }
                        else if (line.IndexOf(m_Tags.Dedication) >= 0)
                        {
                            m_ParsedWork.SetDedication(StripTag(line, m_Tags.Dedication));
                            bParsingIntro = false;
                        }
                        else if (line.IndexOf(m_Tags.Creation) >= 0)
                        {
                            m_ParsedWork.SetCreationDate(StripTag(line, m_Tags.Creation));
                            bParsingIntro = false;
                        }                            
                        else if (line.IndexOf(m_Tags.ShowUnitNumber) >= 0)
                        {
                            m_ParsedWork.SetShowUnitNumber(true);
                            bParsingIntro = false;
                        }                       
                        else if (line.IndexOf(m_Tags.Start) >= 0) // now start!
                        {
                            bParsingIntro = false;
                            m_ParsedWork.SetIntro(StripTag(introText, m_Tags.Intro));
                            bInsideContent = true;
                        }
                        else
                        {
                            if (bParsingIntro)
                                introText += ("\n" + line);                            
                        }
                        continue;
                    }

                    //// from here start parsing the text
                    
                    // normalize the line stripping the indentation level
                    curLevel = GetIndentationLevel(line);
                    line = StripIndentation(line);

                    if (curUnit == null && line.Length == 0)
                        continue; // skip starting empty lines

                    if (line.StartsWith(m_Tags.UnitStart)) // new unit is found
                    {
                        // store previous unit
                        if (curUnit != null)
                           unitList.Add(curUnit);
                        
                        // create a new unit
                        curUnit = new Unit(unitList.Count + 1);
                        curUnit.SetLevel(curLevel);
                        curUnit.SetTitleVisible(!line.Contains(m_Tags.UnitNoTitle));
                        curUnit.SetVisibleInSummary(!line.Contains(m_Tags.UnitNoSummary));
                        line = line.Replace(m_Tags.UnitNoTitle, "");
                        line = line.Replace(m_Tags.UnitNoSummary, "");
                        line = line.Replace(m_Tags.UnitStart, "");
                        curUnit.SetTitle(line);
                        unitContentCount = 0;
                        continue;
                    }

                    //// parse contents
                    if (curUnit == null)
                        throw new Exception("No unit defined in the body text! Every content must be included inside a unit");

                    if (prevLine.Length == 0 && line.Length == 0)
                        continue; // avoid multiple empty lines

                    Content predictedContent = TestLineContentType(line, curUnit.GetLastContent());
                    if (predictedContent == null)
                        continue;

                    ParseCode parseRet = ParseCode.NO_PARSE;
                    if (predictedContent.GetType() == typeof(Quote))
                        parseRet = LineParseQuote(ref line, ref unitContentCount, ref curUnit);
                    else if (predictedContent.GetType() == typeof(MetaImage))
                        parseRet = LineParseImage(ref line, ref unitContentCount, ref curUnit);
                    else if (predictedContent.GetType() == typeof(ItemList))
                        parseRet = LineParseItemList(ref line, ref unitContentCount, ref curUnit);
                    else if (predictedContent.GetType() == typeof(Paragraph))
                        parseRet = LineParseParagraph(ref line, ref unitContentCount, ref curUnit);
                    else
                        throw new Exception("Not handled content detection");

                    if (parseRet == ParseCode.ERROR)
                        Globals.m_Logger.Error(string.Format("Content parser error parsing line nr. {0}",iLine));
                    else if (parseRet == ParseCode.NO_PARSE)
                        Globals.m_Logger.Warn(string.Format("Content parser warning: nothing to parse at line {0}", iLine));
                }

                // handle last unit
                if (curUnit != null)
                    unitList.Add(curUnit);
            }
            catch (Exception ex)
            {
                Globals.m_Logger.Error(ex.ToString());
                return RetCode.ERR_EXCEPTION;
            }

            // ok, parsed. Now we organize the units' gerarchy
            Unit root = BuildUnitTree(unitList);
            if (root == null)
                return RetCode.ERR_INVALIDTREE;

            m_ParsedWork.SetUnit(root);
            m_ParsedWork.SetCurrentHash(Utils.GetHashSha256(m_FileInPath));

            return RetCode.NO_ERR;
        }

        protected string StripTag(string line, string tag)
        {
            if (line.Length == 0 || tag.Length == 0)
                return "";
            line = line.TrimStart(' ');
            return line.Replace(tag, "");
        }

        public string GetOutFilePath()
        {
            return Utils.ChangePathExtension(m_FileInPath, Globals.XML_EXT);
        }

        public string GetOutFileName()
        {
            return Path.GetFileName(GetOutFilePath());
        }

        public bool Save(string outXmlPath = "", bool bDeleteExist = false)
        {
            if (!IsParsed())
                return false;
            if (m_ParsedWork == null)
                return false;
            if (outXmlPath == null || outXmlPath.Length == 0)
                outXmlPath = GetOutFilePath();
            return m_ParsedWork.Save(outXmlPath, bDeleteExist);
        }

        public MetaDoc GetParsedWork() { return m_ParsedWork; }

        /**
         * Il livello delle unit è sempre >= 1
         * Il livello 0 è la root del doc
         */
        protected int GetIndentationLevel(string line)
        {
            if (line == null || line.Length == 0)
                return 0;
            int index = 0;

            try
            {
                while (index < line.Length && line[index].Equals('\t'))
                {
                    index++;                    
                }
            }
            catch (Exception ex)
            {
                Globals.m_Logger.Error(ex.ToString());
                return 0;
            }
            return index+1;
        }

        protected string StripIndentation(string line)
        {
            try
            {
                if (line == null || line.Length == 0)
                    return "";
                line = line.TrimStart(' ');
                int lastTabIndex = 0;
                
                for (int i = 0; i < line.Length; i++)
                {
                    if (!line.ElementAt(i).Equals('\t'))
                    {
                        lastTabIndex = i;
                        break;
                    }
                }
                line = line.Substring(lastTabIndex, line.Length - lastTabIndex);
            }
            catch (Exception ex)
            {
                Globals.m_Logger.Error(ex.ToString());
                return "";
            }
            return line;
        }

        protected ParseCode LineParseParagraph(ref string line, ref int unitContentCount, ref Unit curUnit)
        {
            if (curUnit == null || line == null || unitContentCount < 0)
                return ParseCode.ERROR;

            Paragraph curContent = new Paragraph(unitContentCount++, "");
            List<BiblioRef> refs = LineParseBiblioRef(ref line, ref unitContentCount);
            for (int iLink = 0; iLink < refs.Count; iLink++)
            {
                refs[iLink].SetContentId(curContent.GetId());
                curUnit.AddContent(refs[iLink]);
            }

            List<Attachment> attachments = LineParseAttachment(ref line, ref unitContentCount);
            for (int iLink = 0; iLink < attachments.Count; iLink++)
            {
                attachments[iLink].SetContentId(curContent.GetId());
                curUnit.AddContent(attachments[iLink]);
            }

            Content lastUnitContent = curUnit.GetLastContent();
            if (lastUnitContent != null && lastUnitContent.GetType() == typeof(Paragraph) && lastUnitContent.GetContent().EndsWith("//"))
            {
                ((Paragraph)lastUnitContent).AddText(line, false);
            }
            else 
            {
                curContent.SetContent(line);
                curUnit.AddContent(curContent); 
            }
            return ParseCode.PARSED;
        }

        protected ParseCode LineParseQuote(ref string line, ref int unitContentCount, ref Unit curUnit)
        {
            if (curUnit == null || line == null || unitContentCount < 0)
                return ParseCode.ERROR;

            line = line.Replace(m_Tags.Quote, "");

            Content lastUnitContent = curUnit.GetLastContent();
            if (lastUnitContent != null && lastUnitContent.GetType() == typeof(Quote))
            {
                // quote already open                
                ((Quote)lastUnitContent).AddText(line, false);                
            }
            else
            {
                Quote curContent = new Quote(unitContentCount++, "");
                curContent.AddText(line);
                curUnit.AddContent(curContent);
            }
            return ParseCode.PARSED;
        }

        protected ParseCode LineParseItemList(ref string line, ref int unitContentCount, ref Unit curUnit)
        {
            if (curUnit == null || line == null || unitContentCount < 0)
                return ParseCode.ERROR;

            ItemList curContent = new ItemList(unitContentCount++, "");
            if (line.StartsWith(m_Tags.ListOrd))
                curContent.SetOrdered(true);
            line = line.Substring(1, line.Length - 1);
            line = line.Trim();
            curContent.AddItem(line);
            curUnit.AddContent(curContent);
            return ParseCode.PARSED;
        }

        protected ParseCode LineParseImage(ref string line, ref int unitContentCount, ref Unit curUnit)
        {
            if (curUnit == null || line == null || unitContentCount < 0)
                return ParseCode.ERROR;                

             // an image cannot be inside text or other element
            if (!(line.StartsWith(m_Tags.Image) && line.EndsWith("]")))
                return ParseCode.NO_PARSE;

            line = line.Replace(m_Tags.Image, "");
            line = line.Replace("]", "");
            string relPath = line;
            relPath = relPath.Trim();

            string imgPath = Utils.NormalizePath(string.Format("{0}{1}{2}", Directory.GetParent(m_FileInPath), Path.DirectorySeparatorChar, relPath));
            if (!File.Exists(imgPath))
                return ParseCode.ERROR;

            Image origImg = Image.FromFile(imgPath);
            if (origImg == null)
                return ParseCode.ERROR;

            Image scaledImg = Utils.ScaleImage(origImg, 500);
            string b64Img = Utils.ImageToBase64String(scaledImg, ImageFormat.Jpeg);

            curUnit.AddContent(new MetaImage(unitContentCount++, b64Img));
            return ParseCode.PARSED;            
        }


        protected List<BiblioRef> LineParseBiblioRef(ref string line, ref int unitContentCount)
        {
            List<BiblioRef> refs = new List<BiblioRef>();
            while (line.Contains(m_Tags.BiblioRef))
            {
                // there is a biblio reference: extract it and go on
                int iStart = line.IndexOf(m_Tags.BiblioRef);
                int iEnd = iStart;
                for (int iL = iStart; iL < line.Length; iL++)
                {
                    char c = line[iL];
                    if (c == ']')
                    {
                        iEnd = iL;
                        break;
                    }
                }

                if (iEnd == iStart)
                    break; // something wrong!...
                // replace the link tag and insert it as placeholder (remember the original tag position)
                string refTag = line.Substring(iStart, iEnd - iStart + 1);
                refTag = refTag.Replace(m_Tags.BiblioRef, "");
                refTag = refTag.Replace("]", "");
                string biblioRef = refTag.Trim();

                BiblioRef linkContent = new BiblioRef(unitContentCount++, biblioRef);
                linkContent.SetPos(iStart);

                refs.Add(linkContent);

                string cleanLine = line.Substring(0, iStart);
                cleanLine += line.Substring(iEnd + 1);
                line = cleanLine;
            }
            return refs;
        }

        protected List<Attachment> LineParseAttachment(ref string line, ref int unitContentCount)
        {
            List<Attachment> refs = new List<Attachment>();
            while (line.Contains(m_Tags.Attachment))
            {
                // there is an attachment: extract it and go on
                int iStart = line.IndexOf(m_Tags.Attachment);
                int iEnd = iStart;
                for (int iL = iStart; iL < line.Length; iL++)
                {
                    char c = line[iL];
                    if (c == ']')
                    {
                        iEnd = iL;
                        break;
                    }
                }

                if (iEnd == iStart)
                    break; // something wrong!...
                // replace the link tag and insert it as placeholder (remember the original tag position)
                string refTag = line.Substring(iStart, iEnd - iStart + 1);
                refTag = refTag.Replace(m_Tags.Attachment, "");
                refTag = refTag.Replace("]", "");
                string attachment = refTag.Trim();

                string[] tokens = attachment.Split('|');
                if (tokens.Length != 2)
                    break; // something wrong!...

                Attachment linkAttachment = new Attachment(unitContentCount++, tokens[0], tokens[1]);
                linkAttachment.SetPos(iStart);

                refs.Add(linkAttachment);

                string cleanLine = line.Substring(0, iStart);
                cleanLine += line.Substring(iEnd + 1);
                line = cleanLine;
            }
            return refs;
        }

        /**
         * Analyze the current line and return the related content type
         * */
        public Content TestLineContentType(string curLine, Content curContent = null)
        {
            if (curLine.StartsWith(m_Tags.Image) && curLine.EndsWith("]"))
                return new MetaImage(0, "");
            else if (curLine.StartsWith(m_Tags.List) || curLine.StartsWith(m_Tags.ListOrd))
                return new ItemList(0, "");
            else if (curLine.StartsWith(m_Tags.Quote) || curLine.EndsWith(m_Tags.Quote) || (curContent != null && curContent.GetType() == typeof(Quote)))
                return new Quote(0, "");
            else if (curLine.Length > 0)
                return new Paragraph(0, "");
            return null;
        }


        /***
         * Descrizione Algoritmo
         * ---------------------
         * Si parte da una lista di unit parsate.
         * La lista è ordinata secondo l'ordine di parsing
         * Ogni unit contiene 0 o più contenuti (al momento paragrafi di testo)
         * Ogni unit ha un livello maggiore o uguale a 1. Se il livello minimo è maggiore di 1, vengono di conseguenza traslati tutti i livelli maggiori della differenza (gap)
         * 
         * Si crea una root di livello 0
         * 1 Si cerca tra le unit quella con livello massimo
         * 2 Se due o più unit hanno lo stesso livello si seleziona la prima
         * 3 Dalla unit selezionata si risale la lista ordinata verso l'indice 0 fino alla prima unit con livello inferiore a quello corrente
         * 4 Trovata la unit con livello inferiore gli si assegna come figlia quella con livello massimo e si elimina quest'ultima della lista
         * 5 Si ripete da punto 1
         * 6 Quando nella lista sono rimaste solo con unit di livello 1, si assegnano alla root e si ritorna
         * 
         * Costo dell'algoritmo caso peggiore: O(n*(n-1)) ???
         */
        protected Unit BuildUnitTree(List<Unit> unitList)
        {
            if (unitList == null || unitList.Count == 0)
                return null;

            int baseLevel = 1000;
            for (int i = 0; i < unitList.Count(); i++) // the root unit could have a level bigger than 1
            {
                if (baseLevel > unitList[i].GetLevel())
                    baseLevel = unitList[i].GetLevel();
            }

            int gap = baseLevel - 1; // difference to standard base level (1)

            try
            {
                Unit root = new Unit(0);
                root.SetLevel(0);
                root.SetTitleVisible(false);
                root.SetVisibleInSummary(false);
                while (unitList.Count > 0)
                {
                    int maxLevelIndex = 0;
                    int maxLevel = 0;
                    for (int i = 0; i < unitList.Count; i++)
                    {
                        if (unitList[i].GetLevel() > maxLevel)
                        {
                            maxLevel = unitList[i].GetLevel();
                            maxLevelIndex = i;
                        }
                    }
                    if (maxLevel == baseLevel)
                    {
                        // finish: add residual units to root
                        for (int i = 0; i < unitList.Count; i++)
                        {
                            unitList[i].SetLevel(unitList[i].GetLevel() - gap);
                            root.AddUnit(unitList[i]);
                        }
                        unitList.Clear();
                    }
                    else
                    {
                        // search for unit's parent
                        for (int i = maxLevelIndex - 1; i >= 0; i--)
                        {
                            if (unitList[i].GetLevel() < maxLevel)
                            {
                                unitList[i].SetLevel(unitList[i].GetLevel() - gap);
                                unitList[i].AddUnit(unitList[maxLevelIndex]);
                                unitList.RemoveAt(maxLevelIndex);
                                break;
                            }
                        }
                    }
                }
                return root;
            }
            catch (Exception ex)
            {
                Globals.m_Logger.Error(ex.ToString());
                return null;
            }           
        }
    }
}
