using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Markdown
{
    public class SyntaxElem
    {
        public string NameOfEquivalentConstructionInAnotherSyntax { get; }
        private readonly Regex startRegex;
        private readonly Regex endRegex;
        public List<string> NestedElems;
        public SyntaxElem(Regex startRegex, Regex endRegex, string nameOfEquivalentConstructionInAnotherSyntax) //TODO RV(atolsotv): nameOfEquivalentConstructionInAnotherSyntax - ужасно :). Хватило бы просто name
        {
            this.startRegex = new Regex("^"+startRegex.ToString()); //Не очень понимаю, где должна быть описана эта логика,
            //потому что я это делаю для того, чтоб искать совпадение именно в начале анализируемого фрагмента
            //для корректного парсинга всех вложенных конструкций.
            //TODO RV(atolstov):             syntax.Register(new Regex("^[^_](_)[^_]"), new Regex("[^_](_)[^_]"), "em");
            // Она должна быть в месте регистрации, так как может существовать тег не обязательно идущий в начале, или с более сложно логикой:
            //      (?<=\s|^|#)[ ...
            this.endRegex = endRegex;
            NestedElems = new List<string>();
            NameOfEquivalentConstructionInAnotherSyntax = nameOfEquivalentConstructionInAnotherSyntax;
        }

        public SyntaxElem With(string nestedName)
        {
            NestedElems.Add(nestedName);
            return this;
        }

        public MatchInfo CheckMatchWithElem(string str, int shift)
        {
            var partToAnalyze = str.Substring(shift);
            var startSeqPosition = startRegex.Match(partToAnalyze);
            if (!startSeqPosition.Success) return new MatchInfo(false);
            var indOfLastSymOfStart = startSeqPosition.Groups[1].Index + startSeqPosition.Groups[1].Length;
            var endInd = endRegex.Match(partToAnalyze, indOfLastSymOfStart);
            if (!endInd.Success) return new MatchInfo(false);
            var indOfLastSymOfEnd = endInd.Groups[1].Length + endInd.Groups[1].Index;
            return new MatchInfo(true,
                (startSeqPosition.Groups[1].Index, indOfLastSymOfStart - 1),
                (endInd.Groups[1].Index,  indOfLastSymOfEnd- 1));
        }
 



    }
}
