using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Parser
{
    public class TagRender
    {
        public static StringBuilder Render(StringBuilder htmlBuilder, SyntaxElem syntaxElem, bool isOpen)
        {
            return isOpen
                ? htmlBuilder.Append("<").Append(syntaxElem.NameOfEquivalentConstructionInHtml).Append(">")
                : htmlBuilder.Append("</").Append(syntaxElem.NameOfEquivalentConstructionInHtml).Append(">");
        }
    }
}
