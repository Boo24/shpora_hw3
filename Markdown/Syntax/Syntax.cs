using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Markdown
{
    public class Syntax
    {
        public List<SyntaxElem> SyntaxElems = new List<SyntaxElem>();
        public SyntaxElem Register(string startSequence, string endSequence, string name)
        {
            var syntaxElem =   new SyntaxElem(startSequence, endSequence, name);
            SyntaxElems.Add(syntaxElem);
            return syntaxElem;
        }

        public SyntaxElem GetSyntaxElemByName(string name) => SyntaxElems.FirstOrDefault(a => a.NameOfEquivalentConstructionInHtml == name);
    }
}
