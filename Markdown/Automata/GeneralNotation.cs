using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Automata
{
    public static class GeneralNotation
    {
        public const string AnySymbol = ".";
        public const string LetterOrDigit = "\\w";
        public const string EscapeSymbol = "\\";
        public const char OneOrMoreCountOfSymbol = '+';
        public const char AnyCountOfSymbol = '*';

        public static bool IsLetterOrDigit(char ch)
        {
            return Char.IsLetterOrDigit(ch);
        }

        public static bool IsEscapeSymbol(char ch) => ch.ToString() == EscapeSymbol;
    }
}
