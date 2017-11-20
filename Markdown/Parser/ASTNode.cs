using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Parser
{
    public class ASTNode
    {
        public List<ASTNode> Childs;
        public string Value { get; }
        public ASTNode Parent { get; }
        public SyntaxElem Elem { get; }
        public bool IsLeaf => Childs.Count == 0;
        public ASTNode(string value, ASTNode parent)
        {
            Value = value;
            Childs = new List<ASTNode>();
            Parent = parent;
        }

        public ASTNode(ASTNode parent, SyntaxElem elem)
        {
            Childs = new List<ASTNode>();
            Parent = parent;
            Elem = elem;
        }
    }
}
