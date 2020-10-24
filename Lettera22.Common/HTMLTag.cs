using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lettera22.Common
{
    public class HTMLTag
    {
        /// <project>Lettera22</project>
        /// <copyright company="Claudio Tortorelli">
        /// Copyright (c) 2017 All Rights Reserved
        /// </copyright>        
        /// <author>Claudio Tortorelli</author>
        /// <email>claudio.tortorelli@gmail.com</email>
        /// <web>http://www.claudiotortorelli.it</web>
        /// <date>Feb 2017</date>
        /// <summary>
        /// Utils methods around w3c official tags
        /// www.w3schools.com/tags/
        /// </summary>
        /// https://choosealicense.com/licenses/mit/

        protected List<string> m_HTMLTags = new List<string>();

        public HTMLTag()
        {            
            m_HTMLTags.Add("<!DOCTYPE>");
            m_HTMLTags.Add("<a>");
            m_HTMLTags.Add("<abbr>");
            m_HTMLTags.Add("<acronym>");
            m_HTMLTags.Add("<address>");
            m_HTMLTags.Add("<applet>");
            m_HTMLTags.Add("<area>");
            m_HTMLTags.Add("<article>");
            m_HTMLTags.Add("<aside>");
            m_HTMLTags.Add("<audio>");
            m_HTMLTags.Add("<b>");
            m_HTMLTags.Add("<base>");
            m_HTMLTags.Add("<basefont>");
            m_HTMLTags.Add("<bdi>");
            m_HTMLTags.Add("<bdo>");
            m_HTMLTags.Add("<big>");
            m_HTMLTags.Add("<blockquote>");
            m_HTMLTags.Add("<body>");
            m_HTMLTags.Add("<br>");
            m_HTMLTags.Add("<button>");
            m_HTMLTags.Add("<canvas>");
            m_HTMLTags.Add("<caption>");
            m_HTMLTags.Add("<center>");
            m_HTMLTags.Add("<cite>");
            m_HTMLTags.Add("<code>");
            m_HTMLTags.Add("<col>");
            m_HTMLTags.Add("<colgroup>");
            m_HTMLTags.Add("<datalist>");
            m_HTMLTags.Add("<dd>");
            m_HTMLTags.Add("<del>");
            m_HTMLTags.Add("<details>");
            m_HTMLTags.Add("<dfn>");
            m_HTMLTags.Add("<dialog>");
            m_HTMLTags.Add("<dir>");
            m_HTMLTags.Add("<div>");
            m_HTMLTags.Add("<dl>");
            m_HTMLTags.Add("<dt>");
            m_HTMLTags.Add("<em>");
            m_HTMLTags.Add("<embed>");
            m_HTMLTags.Add("<fieldset>");
            m_HTMLTags.Add("<figcaption>");
            m_HTMLTags.Add("<figure>");
            m_HTMLTags.Add("<font>");
            m_HTMLTags.Add("<footer>");
            m_HTMLTags.Add("<form>");
            m_HTMLTags.Add("<frame>");
            m_HTMLTags.Add("<frameset>");
            m_HTMLTags.Add("<h1>");
            m_HTMLTags.Add("<h2>");
            m_HTMLTags.Add("<h3>");
            m_HTMLTags.Add("<h4>");
            m_HTMLTags.Add("<h5>");
            m_HTMLTags.Add("<h6>");
            m_HTMLTags.Add("<head>");
            m_HTMLTags.Add("<header>");
            m_HTMLTags.Add("<hr>");
            m_HTMLTags.Add("<html>");
            m_HTMLTags.Add("<i>");
            m_HTMLTags.Add("<iframe>");
            m_HTMLTags.Add("<img>");
            m_HTMLTags.Add("<input>");
            m_HTMLTags.Add("<ins>");
            m_HTMLTags.Add("<kbd>");
            m_HTMLTags.Add("<keygen>");
            m_HTMLTags.Add("<label>");
            m_HTMLTags.Add("<legend>");
            m_HTMLTags.Add("<li>");
            m_HTMLTags.Add("<link>");
            m_HTMLTags.Add("<main>");
            m_HTMLTags.Add("<map>");
            m_HTMLTags.Add("<mark>");
            m_HTMLTags.Add("<menu>");
            m_HTMLTags.Add("<menuitem>");
            m_HTMLTags.Add("<meta>");
            m_HTMLTags.Add("<meter>");
            m_HTMLTags.Add("<nav>");
            m_HTMLTags.Add("<noframes>");
            m_HTMLTags.Add("<noscript>");
            m_HTMLTags.Add("<object>");
            m_HTMLTags.Add("<ol>");
            m_HTMLTags.Add("<optgroup>");
            m_HTMLTags.Add("<option>");
            m_HTMLTags.Add("<output>");
            m_HTMLTags.Add("<p>");
            m_HTMLTags.Add("<param>");
            m_HTMLTags.Add("<picture>");
            m_HTMLTags.Add("<pre>");
            m_HTMLTags.Add("<progress>");
            m_HTMLTags.Add("<q>");
            m_HTMLTags.Add("<rp>");
            m_HTMLTags.Add("<rt>");
            m_HTMLTags.Add("<ruby>");
            m_HTMLTags.Add("<s>");
            m_HTMLTags.Add("<samp>");
            m_HTMLTags.Add("<script>");
            m_HTMLTags.Add("<section>");
            m_HTMLTags.Add("<select>");
            m_HTMLTags.Add("<small>");
            m_HTMLTags.Add("<source>");
            m_HTMLTags.Add("<span>");
            m_HTMLTags.Add("<strike>");
            m_HTMLTags.Add("<strong>");
            m_HTMLTags.Add("<style>");
            m_HTMLTags.Add("<sub>");
            m_HTMLTags.Add("<summary>");
            m_HTMLTags.Add("<sup>");
            m_HTMLTags.Add("<table>");
            m_HTMLTags.Add("<tbody>");
            m_HTMLTags.Add("<td>");
            m_HTMLTags.Add("<textarea>");
            m_HTMLTags.Add("<tfoot>");
            m_HTMLTags.Add("<th>");
            m_HTMLTags.Add("<thead>");
            m_HTMLTags.Add("<time>");
            m_HTMLTags.Add("<title>");
            m_HTMLTags.Add("<tr>");
            m_HTMLTags.Add("<track>");
            m_HTMLTags.Add("<tt>");
            m_HTMLTags.Add("<u>");
            m_HTMLTags.Add("<ul>");
            m_HTMLTags.Add("<var>");
            m_HTMLTags.Add("<video>");
            m_HTMLTags.Add("<wbr>");
            m_HTMLTags.Add("</a>");
            m_HTMLTags.Add("</abbr>");
            m_HTMLTags.Add("</acronym>");
            m_HTMLTags.Add("</address>");
            m_HTMLTags.Add("</applet>");
            m_HTMLTags.Add("</area>");
            m_HTMLTags.Add("</article>");
            m_HTMLTags.Add("</aside>");
            m_HTMLTags.Add("</audio>");
            m_HTMLTags.Add("</b>");
            m_HTMLTags.Add("</base>");
            m_HTMLTags.Add("</basefont>");
            m_HTMLTags.Add("</bdi>");
            m_HTMLTags.Add("</bdo>");
            m_HTMLTags.Add("</big>");
            m_HTMLTags.Add("</blockquote>");
            m_HTMLTags.Add("</body>");
            m_HTMLTags.Add("</br>");
            m_HTMLTags.Add("</button>");
            m_HTMLTags.Add("</canvas>");
            m_HTMLTags.Add("</caption>");
            m_HTMLTags.Add("</center>");
            m_HTMLTags.Add("</cite>");
            m_HTMLTags.Add("</code>");
            m_HTMLTags.Add("</col>");
            m_HTMLTags.Add("</colgroup>");
            m_HTMLTags.Add("</datalist>");
            m_HTMLTags.Add("</dd>");
            m_HTMLTags.Add("</del>");
            m_HTMLTags.Add("</details>");
            m_HTMLTags.Add("</dfn>");
            m_HTMLTags.Add("</dialog>");
            m_HTMLTags.Add("</dir>");
            m_HTMLTags.Add("</div>");
            m_HTMLTags.Add("</dl>");
            m_HTMLTags.Add("</dt>");
            m_HTMLTags.Add("</em>");
            m_HTMLTags.Add("</embed>");
            m_HTMLTags.Add("</fieldset>");
            m_HTMLTags.Add("</figcaption>");
            m_HTMLTags.Add("</figure>");
            m_HTMLTags.Add("</font>");
            m_HTMLTags.Add("</footer>");
            m_HTMLTags.Add("</form>");
            m_HTMLTags.Add("</frame>");
            m_HTMLTags.Add("</frameset>");
            m_HTMLTags.Add("</h1>");
            m_HTMLTags.Add("</h2>");
            m_HTMLTags.Add("</h3>");
            m_HTMLTags.Add("</h4>");
            m_HTMLTags.Add("</h5>");
            m_HTMLTags.Add("</h6>");
            m_HTMLTags.Add("</head>");
            m_HTMLTags.Add("</header>");
            m_HTMLTags.Add("</hr>");
            m_HTMLTags.Add("</html>");
            m_HTMLTags.Add("</i>");
            m_HTMLTags.Add("</iframe>");
            m_HTMLTags.Add("</img>");
            m_HTMLTags.Add("</input>");
            m_HTMLTags.Add("</ins>");
            m_HTMLTags.Add("</kbd>");
            m_HTMLTags.Add("</keygen>");
            m_HTMLTags.Add("</label>");
            m_HTMLTags.Add("</legend>");
            m_HTMLTags.Add("</li>");
            m_HTMLTags.Add("</link>");
            m_HTMLTags.Add("</main>");
            m_HTMLTags.Add("</map>");
            m_HTMLTags.Add("</mark>");
            m_HTMLTags.Add("</menu>");
            m_HTMLTags.Add("</menuitem>");
            m_HTMLTags.Add("</meta>");
            m_HTMLTags.Add("</meter>");
            m_HTMLTags.Add("</nav>");
            m_HTMLTags.Add("</noframes>");
            m_HTMLTags.Add("</noscript>");
            m_HTMLTags.Add("</object>");
            m_HTMLTags.Add("</ol>");
            m_HTMLTags.Add("</optgroup>");
            m_HTMLTags.Add("</option>");
            m_HTMLTags.Add("</output>");
            m_HTMLTags.Add("</p>");
            m_HTMLTags.Add("</param>");
            m_HTMLTags.Add("</picture>");
            m_HTMLTags.Add("</pre>");
            m_HTMLTags.Add("</progress>");
            m_HTMLTags.Add("</q>");
            m_HTMLTags.Add("</rp>");
            m_HTMLTags.Add("</rt>");
            m_HTMLTags.Add("</ruby>");
            m_HTMLTags.Add("</s>");
            m_HTMLTags.Add("</samp>");
            m_HTMLTags.Add("</script>");
            m_HTMLTags.Add("</section>");
            m_HTMLTags.Add("</select>");
            m_HTMLTags.Add("</small>");
            m_HTMLTags.Add("</source>");
            m_HTMLTags.Add("</span>");
            m_HTMLTags.Add("</strike>");
            m_HTMLTags.Add("</strong>");
            m_HTMLTags.Add("</style>");
            m_HTMLTags.Add("</sub>");
            m_HTMLTags.Add("</summary>");
            m_HTMLTags.Add("</sup>");
            m_HTMLTags.Add("</table>");
            m_HTMLTags.Add("</tbody>");
            m_HTMLTags.Add("</td>");
            m_HTMLTags.Add("</textarea>");
            m_HTMLTags.Add("</tfoot>");
            m_HTMLTags.Add("</th>");
            m_HTMLTags.Add("</thead>");
            m_HTMLTags.Add("</time>");
            m_HTMLTags.Add("</title>");
            m_HTMLTags.Add("</tr>");
            m_HTMLTags.Add("</track>");
            m_HTMLTags.Add("</tt>");
            m_HTMLTags.Add("</u>");
            m_HTMLTags.Add("</ul>");
            m_HTMLTags.Add("</var>");
            m_HTMLTags.Add("</video>");
            m_HTMLTags.Add("</wbr>");

            // include tags with attributes, so not closed
            List<string> withAttrib = new List<string>();
            for (int iT = 0; iT < m_HTMLTags.Count; iT++)
            {
                string tagS = m_HTMLTags[iT].Replace(">", "");
                withAttrib.Add(tagS);
            }
            for (int iT = 0; iT < withAttrib.Count; iT++)
            {
                m_HTMLTags.Add(withAttrib[iT]);
            }
        }

        public bool IsHTMLTag(string tag)
        {
            tag = tag.ToLower();
            tag = tag.Trim();
            for (int iT = 0; iT < m_HTMLTags.Count; iT++)
            {
                if (tag.StartsWith(m_HTMLTags[iT]))
                    return true;
            }
            return false;
        }

        public string GetEscapedTag(string tag)
        {
            if (!IsHTMLTag(tag))
                return "";

            string escaped = tag;
            escaped = escaped.Replace("\"", "&#34;");
            escaped = escaped.Replace("<", "&#60;");
            escaped = escaped.Replace(">", "&#62;");
            return escaped;
        }
    }
}
