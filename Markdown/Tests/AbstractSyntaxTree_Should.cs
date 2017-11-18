using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Markdown.Parser;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Markdown.Tests
{   
    [TestFixture]
    class AbstractSyntaxTree_Should
    {
        private Syntax syntax;
        private Automata.Automata automata;

        [SetUp]
        public void SetUp()
        {
            syntax = new Syntax();
            syntax.Register("_", "_", "a", "bold");
            syntax.Register("__", "__", "aa", "italic").With("bold");
            syntax.Register("#+", "\n", "\\w+", "title");

        }

        [Test]
        public void BuildSyntaxTree_WithSyntaxAndTokens()
        {
            var tokens = new List<string>() { "__", "aa", "__" };
            var actualTree = new AbstractSyntaxTree(syntax, tokens);

            var expectedTree = new AbstractSyntaxTree {Root = new ASTNode("Root", null)};
            expectedTree.Root.Childs.Add(new ASTNode("__", expectedTree.Root));
            var leafNode = new ASTNode("aa", expectedTree.Root.Childs[0]);
            expectedTree.Root.Childs[0].Childs.Add(leafNode);
            actualTree.ShouldBeEquivalentTo(expectedTree, options => options
                .AllowingInfiniteRecursion());
        }

    }
}
