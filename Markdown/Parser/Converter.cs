﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Parser
{
    public class Converter
    {
        private AbstractSyntaxTree tree;
        public Converter(AbstractSyntaxTree tree) =>
            this.tree = tree;

        public string Convert()
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