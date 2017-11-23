using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace Markdown
{
    public class Syntax
    {
        private Dictionary<string, SyntaxElem> syntaxElems = new Dictionary<string, SyntaxElem>();
        public Func<SyntaxElem, string> ConvertStartTag;
        public Func<SyntaxElem, string> ConvertEndTag;

        public Syntax(Func<SyntaxElem, string> convertStartConversionFunction, Func<SyntaxElem, string> convertEndConversionFunction)
        {
            ConvertStartTag = convertStartConversionFunction;
            ConvertEndTag = convertEndConversionFunction;
        }
        public SyntaxElem Register(Regex startSequence, Regex endSequence, string name)
        {
            var syntaxElem = new SyntaxElem(startSequence, endSequence, name);
            syntaxElems[name] = syntaxElem;
            return syntaxElem;
        }

        public SyntaxElem this[string elemName] => syntaxElems.ContainsKey(elemName) ? syntaxElems[elemName] : null;
        public IEnumerable<SyntaxElem> GetSyntaxElements() => syntaxElems.Values;
    }
}
