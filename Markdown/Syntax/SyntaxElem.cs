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
        public string NameOfEquivalentConstructionInHtml { get; }
        public string StartSequence { get; }
        public string EndSequence { get; }
        private readonly Regex startRegex;
        private readonly Regex endRegex;
        public List<string> NestedElems;
        //TODO RV(atolstov) Ты описываешь абстрактный синтаксис, но завязываешься на nameOfEquivalentConstructionInHtml? Не хорошо
        //TODO RV(atolstov) Не sequence, а Regexp. Можешь прямо в конструктор принимать не строку Regexp
        //TODO RV(atolstov) Для того чтобы описать билдер абстрактных деревьев стоит принимать (помимо паттерна начала/конца) саму строку начала конца (чтобы знать, как восстановить строку из дерева)
        public SyntaxElem(string startSequence, string endSequence, string nameOfEquivalentConstructionInHtml) 
        {
            StartSequence = startSequence;
            EndSequence = endSequence;
            startRegex = new Regex("^"+startSequence);  //TODO RV(atolstov) WTF? Этой логики не должно быть здесь
            endRegex = new Regex(endSequence);
            NestedElems = new List<string>();
            NameOfEquivalentConstructionInHtml = nameOfEquivalentConstructionInHtml;
        }

        public SyntaxElem With(string nestedName)
        {
            NestedElems.Add(nestedName);
            return this;
        }

        public MatchInfo CheckMatchWithElem(string str)
        {
            var startSeqPosition = startRegex.Match(str);
            if (!startSeqPosition.Success) return new MatchInfo(false);
            var indOfLastSymOfStart = startSeqPosition.Groups[1].Index + startSeqPosition.Groups[1].Length;
            var endInd = endRegex.Match(str, indOfLastSymOfStart);
            if (!endInd.Success) return new MatchInfo(false);
            var indOfLastSymOfEnd = endInd.Groups[1].Length + endInd.Groups[1].Index;
            return new MatchInfo(true,
                (startSeqPosition.Groups[1].Index, indOfLastSymOfStart - 1),
                (endInd.Groups[1].Index,  indOfLastSymOfEnd- 1));
        }
 



    }
}
