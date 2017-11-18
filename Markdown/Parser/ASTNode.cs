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
        public string value;
        public ASTNode Parent;
        public bool IsLeaf => Childs.Count == 0;
        public ASTNode(string value, ASTNode parent)
        {
            this.value = value;
            Childs = new List<ASTNode>();
            Parent = parent;
        }
    }
}
