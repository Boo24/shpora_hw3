using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class SyntaxElem
    {

        public string Name { get; }
        public string StartSequence { get; }
        public string EndSequence { get; }
        public string MiddleSequence { get; }
        public List<string> NestedElems;
        public SyntaxElem(string startSequence, string endSequence, string middleSequence, string name)
        {
            StartSequence = startSequence;
            EndSequence = endSequence;
            MiddleSequence = middleSequence;
            NestedElems = new List<string>();
            Name = name;
        }

        public SyntaxElem With(string nestedName)
        {
            NestedElems.Add(nestedName);
            return this;
        }
 



    }
}
