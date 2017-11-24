using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Parser
{
    public class StringAnalyzerState
    {
        public ASTNode Tree;
        private ASTNode currentNode;
        private string sourceString;
        public int lastIndex = 0 ;
        public StringAnalyzerState(string sourceString)
        {
            Tree = new ASTNode("Tree", null);
            currentNode = Tree;
            this.sourceString = sourceString;
        }
        public void AddTerminalNode(SyntaxElem elem)
        {
            var newNode = new ASTNode(currentNode, elem);
            currentNode.Childs.Add(newNode);
            currentNode = newNode;
        }
        public void UpToParent() => currentNode = currentNode.Parent;
        public void AddNotTerminalNode(int len) =>
            currentNode.Childs.Add(new ASTNode(sourceString.Substring(lastIndex, len), currentNode));
        

    }
}
