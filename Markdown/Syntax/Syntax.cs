using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Automata;

namespace Markdown
{
    public class Syntax
    {
        public List<SyntaxElem> SyntaxElems = new List<SyntaxElem>();
        public SyntaxElem Register(string startSequence, string endSequence, string middleSeauince, string name)
        {
            var syntaxElem =   new SyntaxElem(startSequence, endSequence, middleSeauince, name);
            SyntaxElems.Add(syntaxElem);
            return syntaxElem;
        }

        public Automata.Automata Build()
        {
            return new Automata.Automata(this).Build();
        }

        public SyntaxElem GetSyntaxElemWithStart(string start)
        {
            return SyntaxElems.FirstOrDefault(syntaxElem => syntaxElem.StartSequence == start);
        }
    }
}
