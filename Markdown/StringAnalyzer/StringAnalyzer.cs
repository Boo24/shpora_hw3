using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class StringAnalyzer
    {
        private readonly Syntax syntax;

        public StringAnalyzer(Syntax syntax) => this.syntax = syntax;
        public List<FindedPartsInfo> Analyze(string str)
        {
            var curIndex = 0;
            var prevIndex = 0;
            var res = new List<FindedPartsInfo>();
            while (str.Length-1 != curIndex)
            {
                foreach (var syntaxElem in syntax.SyntaxElems)
                {
                    var matchInfo = syntaxElem.CheckMatchWithElem(str.Substring(curIndex));
                    if (!matchInfo.IsMatch) continue;
                    HandleMatch(str, matchInfo, curIndex, syntaxElem, res);
                    curIndex = matchInfo.EndIndex.Item2+curIndex;
                    prevIndex = curIndex;
                    break;
                }
                if (prevIndex != curIndex) continue;
                curIndex += 1;
                prevIndex += 1;
            }
            return res;
        }

        private void HandleMatch(string str, MatchInfo matchInfo, int shift, SyntaxElem syntaxElem, List<FindedPartsInfo> res)
        {
            var startPart = new FindedPartsInfo(matchInfo.StartIndex.Item1 + shift, matchInfo.StartIndex.Item2 + shift,
                syntaxElem, true);
            res.Add(startPart);
            var indOfStartOfNestedConstruction = matchInfo.StartIndex.Item2 + 1 + shift;
            var lenOfNestedConstruction = matchInfo.EndIndex.Item1 - matchInfo.StartIndex.Item2 - 1;
            AnalyzeNestedContructions(res, str.Substring(indOfStartOfNestedConstruction, lenOfNestedConstruction), syntaxElem,
                indOfStartOfNestedConstruction);
            var indOfFirstSymOfEnd = matchInfo.EndIndex.Item1 + shift;
            var indOfLastSymOfEnd = matchInfo.EndIndex.Item2 + shift;
            var endPart = new FindedPartsInfo(indOfFirstSymOfEnd, indOfLastSymOfEnd, syntaxElem);
            res.Add(endPart);
        }


        private void AnalyzeNestedContructions(List<FindedPartsInfo> indexes, string str, SyntaxElem elem, int index=0)
        {
            foreach (var nestedElem in elem.NestedElems)
            {
                var ind = 0;
                var currentSyntaxElem = syntax.GetSyntaxElemByName(nestedElem);
                var matchInfo = currentSyntaxElem.CheckMatchWithElem(str);
                while (!matchInfo.IsMatch && ind < str.Length)
                {
                    matchInfo = currentSyntaxElem.CheckMatchWithElem(str.Substring(ind));
                    if(matchInfo.IsMatch)
                        break;
                    ind += 1;
                }
                if (matchInfo.IsMatch)
                    HandleMatch(str, matchInfo, index+ind, currentSyntaxElem, indexes);
            }
        }
    }
}

