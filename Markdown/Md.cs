using System.Collections.Generic;
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
	        syntax = new Syntax();
	        syntax.Register("[^_](_)[^_]", "[^_](_)[^_]", "em");
	        syntax.Register("\\s(__)\\S", "\\S(__)\\s", "strong").With("em");
	        analyzer = new StringAnalyzer(syntax);
        }

	    public string Render(string markdownStr)
	    {
	        var syntaxParts = analyzer.Analyze(markdownStr);
	        var tree = new AbstractSyntaxTree();
	        tree.Build(syntaxParts, markdownStr);
	        var converter = new Converter(tree);
	        return converter.Convert();
	    }
	}

}