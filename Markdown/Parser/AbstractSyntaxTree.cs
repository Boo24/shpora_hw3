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
        private ASTNode currentNode;

        public AbstractSyntaxTree()
        {
            Root = new ASTNode("Root", null);
            currentNode = Root;
        }
        public void AddTerminalNode(SyntaxElem elem)
        {
            var newNode = new ASTNode(currentNode, elem);
            currentNode.Childs.Add(newNode);
            currentNode = newNode;
        }

        public void UpToParent() => currentNode = currentNode.Parent;
        public void AddNotTerminalNode(string str) => currentNode.Childs.Add(new ASTNode(str, currentNode));
    }
}
