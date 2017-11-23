using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentAssertions;
using Markdown.Parser;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Markdown.Tests
{
    [TestFixture]
    class StringAnalyzer_Should
    {
        private Syntax syntax;

        [SetUp]
        public void SetUp()
        {
            syntax = new Syntax(elem => $"<{elem.NameOfEquivalentConstructionInAnotherSyntax}>",
                elem => $"</{elem.NameOfEquivalentConstructionInAnotherSyntax}>");
            syntax.Register(new Regex("[^_](_)[^_]"), new Regex("[^_](_)[^_]"), "em");
            syntax.Register(new Regex("\\s(__)\\S"), new Regex("\\S(__)\\s"), "strong").With("em");
            //Задаем начало и конец, выделяя в группу непосредственно начало и конец тега.
        }
        [Test]
        public void ReturnSytnaxTree_WhenEmConstructionsInString()
        {
            var markdownStr = " _Em Here_ ";
            var strAnalyzer = new StringAnalyzer(syntax);
            var actualSytaxTree = strAnalyzer.Analyze(markdownStr);
            var expectedSyntaxTree = new AbstractSyntaxTree();
            expectedSyntaxTree.AddNotTerminalNode(" ");
            expectedSyntaxTree.AddTerminalNode(syntax["em"]);
            expectedSyntaxTree.AddNotTerminalNode("Em Here");
            expectedSyntaxTree.UpToParent();
            expectedSyntaxTree.AddNotTerminalNode(" ");
            actualSytaxTree.ShouldBeEquivalentTo(expectedSyntaxTree, options => options.IgnoringCyclicReferences());
        }

        [Test]
        public void ReturnSytnaxTree_WhenNotSyntaxConstructions()
        {
            var markdownStr = "Simple string";
            var strAnalyzer = new StringAnalyzer(syntax);
            var actualSytaxTree = strAnalyzer.Analyze(markdownStr);
            var expectedSyntaxTree = new AbstractSyntaxTree();
            expectedSyntaxTree.AddNotTerminalNode(markdownStr);
            actualSytaxTree.ShouldBeEquivalentTo(expectedSyntaxTree, options => options.IgnoringCyclicReferences());
        }

        [Test]
        public void ReturnSytnaxTree_WhenStrongConstructions()
        {
            var markdownStr = " __It's text with strong__ ";
            var strAnalyzer = new StringAnalyzer(syntax);
            var actualSytaxTree = strAnalyzer.Analyze(markdownStr);
            var expectedSyntaxTree = new AbstractSyntaxTree();
            expectedSyntaxTree.AddNotTerminalNode(" ");
            expectedSyntaxTree.AddTerminalNode(syntax["strong"]);
            expectedSyntaxTree.AddNotTerminalNode("It's text with strong");
            expectedSyntaxTree.UpToParent();
            expectedSyntaxTree.AddNotTerminalNode(" ");
            actualSytaxTree.ShouldBeEquivalentTo(expectedSyntaxTree, options => options.IgnoringCyclicReferences());
        }

        [Test]
        public void ReturnSytnaxTree_WhenEmInStrong()
        {
            var markdownStr = " __strong _em!_ ))__ ";
            var strAnalyzer = new StringAnalyzer(syntax);
            var actualSytaxTree = strAnalyzer.Analyze(markdownStr);
            var expectedSyntaxTree = new AbstractSyntaxTree();
            expectedSyntaxTree.AddNotTerminalNode(" ");
            expectedSyntaxTree.AddTerminalNode(syntax["strong"]);
            expectedSyntaxTree.AddNotTerminalNode("strong ");
            expectedSyntaxTree.AddTerminalNode(syntax["em"]);
            expectedSyntaxTree.AddNotTerminalNode("em!");
            expectedSyntaxTree.UpToParent();
            expectedSyntaxTree.AddNotTerminalNode(" ))");
            expectedSyntaxTree.UpToParent();
            expectedSyntaxTree.AddNotTerminalNode(" ");
            actualSytaxTree.ShouldBeEquivalentTo(expectedSyntaxTree, options => options.IgnoringCyclicReferences());
        }

        [Test]
        public void ReturnSytnaxTree_WhenStrongInEm()
        {
            var markdownStr = " _em __strong__ h_ ";
            var strAnalyzer = new StringAnalyzer(syntax);
            var actualSytaxTree = strAnalyzer.Analyze(markdownStr);
            var expectedSyntaxTree = new AbstractSyntaxTree();
            expectedSyntaxTree.AddNotTerminalNode(" ");
            expectedSyntaxTree.AddTerminalNode(syntax["em"]);
            expectedSyntaxTree.AddNotTerminalNode("em __strong__ h");
            expectedSyntaxTree.UpToParent();
            expectedSyntaxTree.AddNotTerminalNode(" ");
            actualSytaxTree.ShouldBeEquivalentTo(expectedSyntaxTree, options => options.IgnoringCyclicReferences());
        }



    }
}
