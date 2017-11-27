using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Parser
{
    public class SyntaxStringBuilder
    {
        private Syntax syntax;
        public SyntaxStringBuilder(Syntax syntax) => this.syntax = syntax;

        public string Build(StringAnalyzerState tree)
        {
            var targetSrtingBuiler = new StringBuilder();  //TODO RV(atolstov): почему htmlBuilder? Это билдер подходит для любого синтаксиса
            var curNode = tree.Tree;
            Dfs(targetSrtingBuiler, curNode, tree);
            var targetText = targetSrtingBuiler.ToString();
            return targetText;
        }

        private void Dfs(StringBuilder builder, ASTNode curNode, StringAnalyzerState tree)
        {
            if (curNode.IsLeaf)
            {
                builder.Append(curNode.Value);
                return;
            }
            if (curNode != tree.Tree)
                builder.Append(syntax.ConvertStartTag(curNode.Elem));
            foreach (var child in curNode.Childs)
                Dfs(builder, child, tree);
            if (curNode != tree.Tree)
                builder.Append(syntax.ConvertEndTag(curNode.Elem));
        }
    }
}
