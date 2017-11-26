using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    //TODO RV(atolstov): что за артефакт :)?
    public class FindedPartsInfo
    {
        public int StartInd;
        public int EndInd;
        public SyntaxElem SyntaxElem;

        public FindedPartsInfo(int start, int end, SyntaxElem elem = null)
        {
            StartInd = start;
            EndInd = end;
            SyntaxElem = elem;
        }
    }
}
