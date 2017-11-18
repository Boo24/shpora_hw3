using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Parser
{
    public class AbstractSyntaxTree
    {
        private Syntax syntax;
        public ASTNode Root;
        public AbstractSyntaxTree(Syntax syntax, List<string> tokens)
        {
            this.syntax = syntax;
            Root = new ASTNode("Root", null);
            Build(tokens);
        }

        public AbstractSyntaxTree()
        {
          
        }
        private void Build(List<string> tokens)
        {
            var currentNode = Root;
            var openTags = new Stack<SyntaxElem>();
            foreach (var token in tokens)
            {
                var curSyntaxElem = syntax.GetSyntaxElemWithStart(token);
                if (curSyntaxElem != null)
                {
                    if (openTags.Count == 0 || openTags.Peek().EndSequence != token)
                    {
                        openTags.Push(curSyntaxElem);
                        var newNode = new ASTNode(token, currentNode);
                        currentNode.Childs.Add(newNode);
                        currentNode = newNode;
                    }
                    else
                    {
                        openTags.Pop();
                        currentNode = currentNode.Parent;
                    }
                }
                else
                    currentNode.Childs.Add(new ASTNode(token, currentNode));
            }
       }
   
    }
}
