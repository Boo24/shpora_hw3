using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Markdown.Parser;

namespace Markdown
{

    //TODO RV(atolstov) регэкспы умеют матч с определенного индекса
    //Умеют. но мне нужно проверять обязательно с начала строки, чтоб корректно обрабатывались вложенные конструкции,
    //Поэтому я убрала сабстринг отсюда непосредственно в тот метод, но передаю сдвиг все-таки
    public class StringAnalyzer
    {
        private readonly Syntax syntax;
        private int lastIndex = 0;
        private string originalString;
        public StringAnalyzer(Syntax syntax) => this.syntax = syntax;
        public AbstractSyntaxTree Analyze(string str) 
        {
            originalString = str;
            var curIndex = 0;
            var prevIndex = 0;
            var abstractSyntaxTree = new AbstractSyntaxTree();
            while (curIndex < str.Length)  
            {
                foreach (var syntaxElem in syntax.GetSyntaxElements())
                {
                    var matchInfo = syntaxElem.CheckMatchWithElem(str, curIndex); 
                    if (!matchInfo.IsMatch) continue;
                    HandleMatch(str, matchInfo, curIndex, syntaxElem, abstractSyntaxTree);
                    curIndex = matchInfo.EndIndex.Item2+curIndex;
                    prevIndex = curIndex;
                    break;
                }
                if (prevIndex != curIndex) continue;
                curIndex += 1;
                prevIndex += 1;
            }
            abstractSyntaxTree.AddNotTerminalNode(originalString.Substring(lastIndex));
            return abstractSyntaxTree;
        }

        private void HandleMatch(string str, MatchInfo matchInfo, int shift, SyntaxElem syntaxElem, AbstractSyntaxTree tree)
        {
            tree.AddNotTerminalNode(originalString.Substring(lastIndex, shift+ matchInfo.StartIndex.Item1-lastIndex));
            lastIndex = matchInfo.StartIndex.Item2 + 1+shift;
            tree.AddTerminalNode(syntaxElem);
            var indOfStartOfNestedConstruction = matchInfo.StartIndex.Item2 + 1 + shift;
            var lenOfNestedConstruction = matchInfo.EndIndex.Item1 - matchInfo.StartIndex.Item2 - 1;
            AnalyzeNestedContructions(tree, str.Substring(indOfStartOfNestedConstruction, lenOfNestedConstruction), syntaxElem,
                indOfStartOfNestedConstruction);
            var indOfLastSymOfEnd = matchInfo.EndIndex.Item2 + shift;
            tree.AddNotTerminalNode(originalString.Substring(lastIndex,  shift+matchInfo.EndIndex.Item1 - lastIndex));
            lastIndex = indOfLastSymOfEnd + 1;
            tree.UpToParent();
        }


        private void AnalyzeNestedContructions(AbstractSyntaxTree tree, string str, SyntaxElem elem, int shift=0)
        {
            foreach (var nestedElem in elem.NestedElems)
            {
                var ind = 0;
                var currentSyntaxElem = syntax.GetSyntaxElemByName(nestedElem);
                var matchInfo = currentSyntaxElem.CheckMatchWithElem(str, 0);
                while (!matchInfo.IsMatch && ind < str.Length)
                {
                    matchInfo = currentSyntaxElem.CheckMatchWithElem(str, ind);
                    if(matchInfo.IsMatch)
                        break;
                    ind += 1;
                }
                if (matchInfo.IsMatch)
                    HandleMatch(str, matchInfo, shift+ind, currentSyntaxElem, tree);
            }
        }
    }
}

