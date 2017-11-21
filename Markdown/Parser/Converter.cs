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
        private Syntax syntax;
        public Converter(Syntax syntax) => this.syntax = syntax;

        public string Convert(AbstractSyntaxTree tree)
        {
            var htmlBuilder = new StringBuilder();
            var curNode = tree.Root;
            Dfs(htmlBuilder, curNode, tree);
            var htmlText = htmlBuilder.ToString();
            return htmlText;
        }

        private void Dfs(StringBuilder builder, ASTNode curNode, AbstractSyntaxTree tree)
        {
            if (curNode.IsLeaf)
            {
                builder.Append(curNode.Value);
                return;
            }
            if (curNode != tree.Root)
                builder.Append(syntax.ConvertStartTag(curNode.Elem));
            foreach (var child in curNode.Childs)
                Dfs(builder, child, tree);
            if (curNode != tree.Root)
                builder.Append(syntax.ConvertEndTag(curNode.Elem));
        }
    }
}
