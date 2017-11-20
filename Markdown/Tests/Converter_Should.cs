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
    class Converter_Should
    {
        private Syntax syntax;
        private StringAnalyzer analyzer;

        [SetUp]
        public void SetUp()
        {
            syntax = new Syntax();
            syntax.Register("[^_](_)[^_]", "[^_](_)[^_]", "em");
            syntax.Register("\\s(__)\\S", "\\S(__)\\s", "strong").With("em");
            
            analyzer = new StringAnalyzer(syntax);
        }

        [TestCase(" _em_ ", " <em>em</em> ", TestName = "Only em construction")]
        [TestCase("Text without tags", "Text without tags", TestName = "Text without tags")]
        [TestCase(" __Strong__ ", " <strong>Strong</strong> ", TestName = "Only strong construction")]
        [TestCase(" _em __strong__ _ ", " <em>em __strong__ </em> ", TestName = "Strong in em construction")]
        [TestCase(" __em _in_ strong__ ", " <strong>em <em>in</em> strong</strong> ", TestName = "Em in strong construction")]
        [TestCase("\\_Similar to em))_", "\\_Similar to em))_", TestName = "Text with escape symbol")]
        public void CheckConvertToHtml(string strInMarkdown, string expectedHtmlText)
        {
            var syntaxParts = analyzer.Analyze(strInMarkdown);
            var tree = new AbstractSyntaxTree();
            tree.Build(syntaxParts, strInMarkdown);
            var converter = new Converter(tree);
            var actualHtmlText = converter.Convert();
            actualHtmlText.ShouldBeEquivalentTo(expectedHtmlText);
        }
    }
}
