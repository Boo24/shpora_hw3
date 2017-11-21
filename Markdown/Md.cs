using System.Collections.Generic;
using System.Text.RegularExpressions;
using Markdown.Parser;
using NUnit.Framework;

namespace Markdown
{
	public class Md
	{
	    private Syntax syntax;
	    private StringAnalyzer analyzer;
	    public Md()
	    {
	        syntax = new Syntax(elem => $"<{elem.NameOfEquivalentConstructionInAnotherSyntax}>",
	            elem => $"</{elem.NameOfEquivalentConstructionInAnotherSyntax}>");
            syntax.Register(new Regex("[^_](_)[^_]"), new Regex("[^_](_)[^_]"), "em");
	        syntax.Register(new Regex("\\s(__)\\S"), new Regex("\\S(__)\\s"), "strong").With("em");
            analyzer = new StringAnalyzer(syntax);
        }

	    public string Render(string markdownStr)
	    {
            var syntaxTree = analyzer.Analyze(markdownStr);
            var converter = new Converter(syntax);
            return converter.Convert(syntaxTree);
        }
	}

}