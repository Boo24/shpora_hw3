using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Parser
{
    //TODO RV(atolstov) Попробуй сделать Converter чистым, это очень полезно
    public class Converter      //TODO RV(atolstov) 1) Converter должен быть Builder-ом
    {
        private AbstractSyntaxTree tree;
        public Converter(AbstractSyntaxTree tree) => //TODO RV(atolstov) 2) принимать в конструкторе Syntax
            this.tree = tree;

        public string Convert() //TODO RV(atolstov) 3) а в методе конвертации - само дерево
        {
            var htmlBuilder = new StringBuilder();
            var curNode = tree.Root;
            Dfs(htmlBuilder, curNode);
            var htmlText = htmlBuilder.ToString();
            return htmlText;
        }

        private void Dfs(StringBuilder builder, ASTNode curNode)
        {
            if (curNode.IsLeaf)
            {
                builder.Append(curNode.Value);
                return;
            }
            if(curNode!=tree.Root)
                TagRender.Render(builder, curNode.Elem, true);
            foreach (var child in curNode.Childs)
                Dfs(builder, child);
            if (curNode != tree.Root)
                TagRender.Render(builder, curNode.Elem, false);
        }
    }
}
