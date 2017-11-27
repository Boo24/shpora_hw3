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
    class SyntaxStringBuilder_Should
    {
        private Syntax syntax;
        private StringAnalyzer analyzer;

        [SetUp]
        public void SetUp()
        {
            syntax = new Syntax(elem => $"<{elem.NameOfEquivalentConstructionInAnotherSyntax}>",
                elem => $"</{elem.NameOfEquivalentConstructionInAnotherSyntax}>");
            syntax.Register(new Regex( "[^_](_)[^_]"),new Regex("[^_](_)[^_]"), "em");
            syntax.Register(new Regex("\\s(__)\\S"), new Regex("\\S(__)\\s"), "strong").With("em");
            
            analyzer = new StringAnalyzer(syntax);
        }

        [TestCase(" _em_ ", ExpectedResult = " <em>em</em> ", TestName = "Only em construction")]
        [TestCase("Text without tags", ExpectedResult = "Text without tags", TestName = "Text without tags")]
        [TestCase(" __Strong__ ", ExpectedResult = " <strong>Strong</strong> ", TestName = "Only strong construction")]
        [TestCase(" _em __strong__ _ ", ExpectedResult = " <em>em __strong__ </em> ", TestName = "Strong in em construction")]
        [TestCase(" __em _in_ strong__ ", ExpectedResult = " <strong>em <em>in</em> strong</strong> ", TestName = "Em in strong construction")]
        [TestCase("\\_Similar to em))_", ExpectedResult = "\\_Similar to em))_", TestName = "Text with escape symbol")]
        public string CheckConvertToHtml(string strInMarkdown)
        {
            var syntaxTree = analyzer.Analyze(strInMarkdown);
            var converter = new SyntaxStringBuilder(syntax);
            return converter.Build(syntaxTree);
        }
    }
}
