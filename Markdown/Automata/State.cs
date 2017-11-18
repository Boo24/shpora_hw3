using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Automata
{
    public class State
    {
        public bool IsFinal { get; set; }
        public List<State> NextStates;
        public string Sym;
        public State MissMathState;
        public bool IsTerminal;
        public StateType Type;
        public string Name; 
        public State(bool isFinal, State missMathState, bool isTerminal, StateType type, string name, string sym="")
        {
            IsFinal = isFinal;
            NextStates = new List<State>();
            Sym = sym;
            MissMathState = missMathState;
            IsTerminal = isTerminal;
            Type = type;
            Name = name;
        }

        public List<State> GetNextState(string ch)
        {
            var res =  NextStates.Where(x=>x.Sym==ch).ToList();
            if (GeneralNotation.IsLetterOrDigit(ch[0]))
                foreach (var state in NextStates)
                {
                    if(state.Sym=="\\w")
                        res.Add(state);
                }
            res.Add(MissMathState);
            return res;
        }

        public State GetStateByName(string name)
        {
            return NextStates.FirstOrDefault(state => state.Name == name);
        }
        


    }
}
