using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Markdown.Tests
{
    [TestFixture()]
    class StringAnalyzer_Should
    {
        private Syntax syntax;

        [SetUp]
        public void SetUp()
        {
            syntax = new Syntax();
            syntax.Register("[^_](_)[^_]", "[^_](_)[^_]", "em");
            syntax.Register("\\s(__)\\S", "\\S(__)\\s", "strong").With("em");
            //Задаем начало и конец, выделяя в группу непосредственно начало и конец тега.
        }

        public void ReturnTwoSytnaxParts_WhenEmConstructionsInString()
        {
            var markdownStr = " _Em Here_ ";
            var strAnalyzer = new StringAnalyzer(syntax);
            var actualSytaxParts = strAnalyzer.Analyze(markdownStr);
            var expectedSyntaxParts = new List<FindedPartsInfo>()
            {
                new FindedPartsInfo(1, 1, syntax.GetSyntaxElemByName("em"), true),
                new FindedPartsInfo(9, 9, syntax.GetSyntaxElemByName("em"), false)
            };
            actualSytaxParts.ShouldBeEquivalentTo(expectedSyntaxParts);
        }

        [Test]
        public void ReturnZeroSytnaxParts_WhenNotSyntaxConstructions()
        {
            var markdownStr = "Simple string";
            var strAnalyzer = new StringAnalyzer(syntax);
            var actualSytaxParts = strAnalyzer.Analyze(markdownStr);
            var expectedSyntaxParts = new List<FindedPartsInfo>();
            actualSytaxParts.ShouldBeEquivalentTo(expectedSyntaxParts);
        }

        [Test]
        public void ReturnTwoSytnaxParts_WhenStrongConstructions()
        {
            var markdownStr = " __It's text with strong__ ";
            var strAnalyzer = new StringAnalyzer(syntax);
            var actualSytaxParts = strAnalyzer.Analyze(markdownStr);
            var expectedSyntaxParts = new List<FindedPartsInfo>()
            {
                new FindedPartsInfo(1, 2, syntax.GetSyntaxElemByName("strong"), true),
                new FindedPartsInfo(24, 25, syntax.GetSyntaxElemByName("strong"), false)
            };
            actualSytaxParts.ShouldBeEquivalentTo(expectedSyntaxParts);
        }

        [Test]
        public void ReturnFourSytnaxParts_WhenEmInStrong()
        {
            var markdownStr = " __strong _em!_ ))__ ";
            var strAnalyzer = new StringAnalyzer(syntax);
            var actualSytaxParts = strAnalyzer.Analyze(markdownStr);
            var expectedSyntaxParts = new List<FindedPartsInfo>()
            {
                new FindedPartsInfo(1, 2, syntax.GetSyntaxElemByName("strong"), true),
                new FindedPartsInfo(10, 10, syntax.GetSyntaxElemByName("em"), true),
                new FindedPartsInfo(14, 14, syntax.GetSyntaxElemByName("em"), false),
                new FindedPartsInfo(18, 19, syntax.GetSyntaxElemByName("strong"), false)
            };
            actualSytaxParts.ShouldBeEquivalentTo(expectedSyntaxParts);
        }

        [Test]
        public void ReturnTwoSytnaxParts_WhenStrongInEm()
        {
            var markdownStr = " _em __strong__ h_ ";
            var strAnalyzer = new StringAnalyzer(syntax);
            var actualSytaxParts = strAnalyzer.Analyze(markdownStr);
            var expectedSyntaxParts = new List<FindedPartsInfo>()
            {
                new FindedPartsInfo(1, 1, syntax.GetSyntaxElemByName("em"), true),
                new FindedPartsInfo(17, 17, syntax.GetSyntaxElemByName("em"), false)
            };
            actualSytaxParts.ShouldBeEquivalentTo(expectedSyntaxParts);
        }


    }
}
