using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Parser
{
    public class AbstractSyntaxTree
    {
        public ASTNode Root;
        public AbstractSyntaxTree()=> Root = new ASTNode("Root", null);
      
        public void Build(List<FindedPartsInfo> syntaxParts, string str)
        {
            if (syntaxParts.Count == 0)
            {
                Root.Childs.Add(new ASTNode(str, Root));
                return;
            }
            var currentNode = Root;
            var openTags = new Stack<FindedPartsInfo>();
            syntaxParts.Aggregate(0, (current, part) => HandleAddNewNode(str, current, part, openTags, ref currentNode));
            currentNode.Childs.Add(new ASTNode(str.Substring(syntaxParts.Last().EndInd+1), currentNode));
        }

        private int HandleAddNewNode(string str, int lastIndex, FindedPartsInfo part, Stack<FindedPartsInfo> openTags, ref ASTNode currentNode)
        {
            var value = str.Substring(lastIndex, part.StartInd - lastIndex);
            lastIndex = part.EndInd + 1;
            currentNode.Childs.Add(new ASTNode(value, currentNode));
            if (part.IsOpen)
            {
                openTags.Push(part);
                var newNode = new ASTNode(currentNode, part.SyntaxElem);
                currentNode.Childs.Add(newNode);
                currentNode = newNode;
            }
            else
            {
                openTags.Pop();
                currentNode = currentNode.Parent;
            }
            return lastIndex;
        }
    }
}
