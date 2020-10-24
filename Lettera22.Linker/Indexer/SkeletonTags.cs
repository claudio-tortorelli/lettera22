using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lettera22.Linker
{
    public class SkeletonTags
    {
        /// <project>Lettera22</project>
        /// <copyright company="Claudio Tortorelli">
        /// Copyright (c) 2017 All Rights Reserved
        /// </copyright>        
        /// <author>Claudio Tortorelli</author>
        /// <email>claudio.tortorelli@gmail.com</email>
        /// <web>http://www.claudiotortorelli.it</web>
        /// <date>June 2017</date>
        /// <summary>
        /// </summary>
        /// https://choosealicense.com/licenses/mit/
        private readonly string m_DocType;
        private readonly string m_HTMLOpen;
        private readonly string m_HeadOpen;
        private readonly string m_HeadTitle;        
        private readonly string m_Meta;    
        private readonly string m_CSS;
        private readonly string m_Script;
        private readonly string m_HeadClose;
        private readonly string m_BodyOpen;
        private readonly string m_Header;
        private readonly string m_Title;
        private readonly string m_SubTitle;
        private readonly string m_OtherInfo;
        private readonly string m_TableOpen;
        private readonly string m_Content;
        private readonly string m_TableClose;
        private readonly string m_Footer;
        private readonly string m_BodyClose;
        private readonly string m_HTMLClose;

        public SkeletonTags()
        {
            m_DocType = "[DOCTYPE]";
            m_HTMLOpen = "[HTMLOPEN]";
            m_HeadOpen = "[HEADOPEN]";
            m_HeadTitle = "[HEADTITLE]";
            m_Meta = "[META]";
            m_CSS = "[CSS]";
            m_Script = "[SCRIPT]";
            m_HeadClose = "[HEADCLOSE]";
            m_BodyOpen = "[BODYOPEN]";
            m_Header = "[HEADER]";
            m_Title = "[TITLE]";
            m_SubTitle = "[SUBTITLE]";
            m_OtherInfo = "[OTHERINFO]";
            m_TableOpen = "[TABLEOPEN]";
            m_Content = "[CONTENT]";
            m_TableClose = "[TABLECLOSE]";
            m_Footer = "[FOOTER]";
            m_BodyClose = "[BODYCLOSE]";
            m_HTMLClose = "[HTMLCLOSE]";
        }

        public string DocType(){return m_DocType;}
        public string HTMLOpen() { return m_HTMLOpen; }
        public string HeadOpen() { return m_HeadOpen; }
        public string HeadTitle() { return m_HeadTitle; }
        public string Meta() { return m_Meta; }
        public string CSS() { return m_CSS; }
        public string Script() { return m_Script; }
        public string HeadClose() { return m_HeadClose; }
        public string BodyOpen() { return m_BodyOpen; }
        public string Header() { return m_Header; }
        public string Title(){return m_Title;}
        public string SubTitle(){return m_SubTitle;}
        public string OtherInfo() { return m_OtherInfo; }
        public string TableOpen(){return m_TableOpen;}
        public string Content(){return m_Content;}
        public string TableClose(){return m_TableClose;}
        public string Footer(){return m_Footer ;}
        public string BodyClose() {return m_BodyClose; }
        public string HTMLClose() {return m_HTMLClose;}
    }
    
}
