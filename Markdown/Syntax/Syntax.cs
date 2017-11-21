using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Markdown
{
    //TODO RV(atolstov) Чем вообще данный класс отличается от просто листа? Возможно стоит добавить в него больше логики
    public class Syntax
    {
        public List<SyntaxElem> SyntaxElems = new List<SyntaxElem>(); //TODO RV(atolstov) И сделать поле приватным
        public SyntaxElem Register(string startSequence, string endSequence, string name) 
        {
            var syntaxElem =   new SyntaxElem(startSequence, endSequence, name);
            SyntaxElems.Add(syntaxElem);
            return syntaxElem;
        }

        //TODO RV(atolstov) Зачем вообще нужен этот класс, если SyntaxElem по имени он возвращает полным перебором за линию?
        public SyntaxElem GetSyntaxElemByName(string name) => SyntaxElems.FirstOrDefault(a => a.NameOfEquivalentConstructionInHtml == name);
    }
}
