using System.Collections.Generic;
using Markdown.Parser;
using NUnit.Framework;

namespace Markdown
{
	public class Md
	{
	    private Automata.Automata syntaxAutomata;
	    private Syntax syntax;
	    public Md()
	    {
	        syntax = new Syntax();
	        syntax.Register("_", "_", "aa", "bold");
	        syntax.Register("__", "__", "aa", "italic").With("bold");
	        syntax.Register("#+", "\n", "\\w+", "title").With("bold");
	        syntaxAutomata = syntax.Build();
           
        }
		public string RenderToHtml(string markdown)
		{
		    var tokens = syntaxAutomata.AnalyzeString(markdown);
		    var syntaxTree = new AbstractSyntaxTree(syntax, tokens);
            return markdown;
		}

	    private string Do()
	    {
	        var visited = new HashSet<ASTNode>();

	        return null;
	    }

	    private void Dfs(ASTNode node)
	    {
	        
	    }
	}

	[TestFixture]
	public class Md_ShouldRender
	{
	}
}