using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    class Automata_Should
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
            automata = syntax.Build();
            
        }

        [Test]
        public void BuildAutomata()
        {
            automata.RootState.NextStates.Count.ShouldBeEquivalentTo(5);
        }

        [Test]
        public void ReturnTokens_WhenNotNestedConstruction()
        {
            var str = "__aa__";
            var actualTokens = automata.AnalyzeString(str);
            var expectedTokens = new List<string>() {"__", "aa", "__"};
            actualTokens.Should().BeEquivalentTo(expectedTokens);
        }

        [Test]
        public void ReturnTokens_WhenNestedConstruction()
        {
            var str = "__aa _a_ __";
            var actualTokens = automata.AnalyzeString(str);
            var expectedTokens = new List<string>() { "__", "aa", " ", "_", "a", "_", " ", "__" };
            actualTokens.Should().BeEquivalentTo(expectedTokens);
        }

        [Test]
        public void ReturnTokens_WhenNotConstruction()
        {
            var str = "aaa";
            var actualTokens = automata.AnalyzeString(str);
            var expectedTokens = new List<string>() { "a", "a", "a" };
            actualTokens.Should().BeEquivalentTo(expectedTokens);
        }

        [Test]
        public void ReturnTokens_WhenTitleTempLength()
        {
            var str = "####aaa\n";
            var actualTokens = automata.AnalyzeString(str);
            var expectedTokens = new List<string>() { "####", "aaa", "\n" };
            actualTokens.Should().BeEquivalentTo(expectedTokens);
        }

    }
}
