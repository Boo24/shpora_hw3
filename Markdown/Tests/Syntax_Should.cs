using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    class Syntax_Should
    {
        private Syntax syntax;

        [SetUp]
        public void SetUp()
        {
            syntax = new Syntax();
            syntax.Register("_", "_", "a", "bold");
            syntax.Register("__", "__", "aa", "italic").With("bold");
        }

        [Test]
        public void GetSyntaxElemByStartPart_WhenExistElemInSyntax()
        {
            var actualSyntaxElem = syntax.GetSyntaxElemWithStart("_");
            var expectedElem = new SyntaxElem("_", "_", "a", "bold");
            actualSyntaxElem.ShouldBeEquivalentTo(expectedElem);
        }

        [Test]
        public void GetSyntaxElemReturnNull_WhenNotExistElemInSyntax()
        {
            var actualSyntaxElem = syntax.GetSyntaxElemWithStart("_=");
            SyntaxElem expectedElem = null;
            actualSyntaxElem.ShouldBeEquivalentTo(expectedElem);
        }
    }
}
