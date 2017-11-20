using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class MatchInfo
    {
        public bool IsMatch { get; }
        public (int, int) StartIndex { get; }
        public (int, int) EndIndex { get; }
        public MatchInfo(bool isMatch, (int, int) startSeq=default((int, int)), (int, int) endSeq=default((int, int)))
        {
            IsMatch = isMatch;
            StartIndex = startSeq;
            EndIndex = endSeq;
        }
    }
}
