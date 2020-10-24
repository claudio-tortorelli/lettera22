using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Lettera22.Metadoc
{
    abstract public class Content
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

        protected int m_Id;
        protected string m_ContentStr;

        // attributes
        protected bool m_bMultiline; // the content can be on more than a line
        protected bool m_bEmbeddable; // the contenct can be embedded inside another content in a line
        
        public Content(int id, string content)
            : base()
        {
            m_Id = id;
            m_ContentStr = content;
            m_bMultiline = false;
            m_bEmbeddable = false;
        }

        public int GetId() { return m_Id; }
        public void SetId(int id) { m_Id = id; }

        public string GetContent() { return m_ContentStr; }
        public void SetContent(string contentStr) { m_ContentStr = contentStr; }

        public bool IsMultiline() { return m_bMultiline; }
        public bool IsEmbeddable() { return m_bEmbeddable; }

        public abstract bool ToXML(XmlWriter writer);
        public abstract bool FromXML(XmlReader reader);
    }
}
