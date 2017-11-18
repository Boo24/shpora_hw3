using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Markdown.Parser;

namespace Markdown.Automata
{
    public class Automata
    {
        public State RootState;
        private Syntax syntax;
        private const string NameStateOfTransitionBetweenBranch = " ";
        private const string NameOfRootState = "Start";
        private const string NameOfMissMathState = "Default";
        private const string SymbolForTransactionBetweenBrach = " ";
        private State curState;
        private State prevState;
        public Automata(Syntax syntax)
        {
            this.syntax = syntax;
            RootState = new State(false,  null, false, StateType.Start, NameOfRootState);
        }

        public Automata Build()
        {
            foreach (var syntaxElem in syntax.SyntaxElems)
                BuildAutomataForSyntaxElem(syntaxElem);
            AddDefaultState();
            return this;
        }

        private void AddDefaultState()
        {
            var startState = new State(true, RootState, false, StateType.Start, NameOfMissMathState, GeneralNotation.LetterOrDigit);
            var lastState = startState;
            lastState.NextStates.Add(startState);
            lastState = lastState.NextStates.Last();
            lastState.NextStates.Add(lastState);
            RootState.NextStates.Add(startState);
            var st = new State(true, RootState, false, StateType.Start, NameOfMissMathState, SymbolForTransactionBetweenBrach);
            RootState.NextStates.Add(st);
        }
        public List<string> AnalyzeString(string str)
        {
            var index = 0;
            var indexes = new SortedSet<int>();
            while (index != str.Length)
            {
                foreach (var startState in RootState.NextStates)
                {
                    var match = CheckMatchInBranch(startState, str, ref index);
                    if (!match.Item1) continue;
                    indexes.SymmetricExceptWith(match.Item2);
                    break;
                }
            }
             return GetStringsPartsFromIndexes(indexes, str);
        }

        private List<string> GetStringsPartsFromIndexes(SortedSet<int> indexses, string str)
        {
            var res = new List<string>();
            var ind = indexses.ToList();
            res.Add(str.Substring(0, ind[0] + 1));
            for (int i = 1; i < ind.Count; i++)
                res.Add(str.Substring(ind[i-1] + 1, ind[i] - ind[i - 1]));
            return res;
        }
        private (bool, SortedSet<int>) CheckMatchInBranch(State state, string str, ref  int index)
        {
            var indexOfTerminalsStates = new SortedSet<int>();
            var nestedStates = new Stack<(State, int)>();
            var visited = new Dictionary<int, State>();
            var curIndex = index;
            curState = state;
            prevState = curState;
            while (curState!=RootState)
            {
                if (NeedStoreIndexOfState())
                    indexOfTerminalsStates.Add(curIndex - 1);
                if (curState.IsFinal && !CheckPossibilityOfContinuingBypass(ref curIndex, str,
                                                    indexOfTerminalsStates, nestedStates))
                    break;
                if (CompareSymbols(str[curIndex].ToString(), curState.Sym))
                    HandleTransition(str, nestedStates, visited, ref curIndex, indexOfTerminalsStates);
                else
                    return (false, null);
                if (curState != RootState) continue;
                if (nestedStates.Count != 0)
                {
                    HandleTransitionInStartState(str, nestedStates, visited, ref curIndex);
                    if (prevState.Name != curState.Name)
                        indexOfTerminalsStates.Add(curIndex + 1);
                }
                else
                    break;
            }
            if (!curState.IsFinal) return (false, null);
            index = curIndex+1;
            indexOfTerminalsStates.Add(index -1);
            return (true, indexOfTerminalsStates);
        }

        private bool CheckPossibilityOfContinuingBypass(ref int curIndex, string str, SortedSet<int> indexOfTerminalsStates,
            Stack<(State, int)> nestedStates)
        {
            if (curIndex < str.Length - 1 && curState.GetNextState(NameStateOfTransitionBetweenBranch)[0] != RootState)
            {
                indexOfTerminalsStates.Add(curIndex);
                prevState = curState;
                curState = prevState.GetNextState(NameStateOfTransitionBetweenBranch)[0];
                indexOfTerminalsStates.Add(curIndex + 1);
                curIndex += 1;
                return true;
            }
            if (nestedStates.Count == 0) return false;
            curIndex = ReturnToPastBranch(str, indexOfTerminalsStates, curIndex, nestedStates);
            return true;
        }
        private bool NeedStoreIndexOfState() =>
            (!curState.IsTerminal && prevState.IsTerminal)
            || (prevState.Type != curState.Type && prevState.IsTerminal)
            || curState.Name == NameStateOfTransitionBetweenBranch;

        private bool CompareSymbols(string s1, string s2)
        {
            if (s1 == s2) return true;
            return GeneralNotation.IsLetterOrDigit(s1[0]) && s2 == GeneralNotation.LetterOrDigit;
        }

        private void HandleTransitionInStartState(string str, Stack<(State, int)> nestedStates, Dictionary<int, State> visited,
             ref int curIndex)
        {
            var prevData = nestedStates.Pop();
            prevState = prevData.Item1;
            var next = prevState.GetNextState(str[curIndex].ToString());
            foreach (var elem in next)
                if (elem != visited[prevData.Item2])
                {
                    curState = elem;
                    break;
                }
        }

        private void HandleTransition(string str, Stack<(State, int)> nestedStates,
                    Dictionary<int, State> visited, ref int curIndex, SortedSet<int> indexOfTerminalsStates)
        {
            prevState = curState;
            curState = prevState.GetNextState(str[curIndex + 1].ToString())[0];
            if(curState.Name != prevState.Name)
                indexOfTerminalsStates.Add(curIndex);
            if (curState.Name != prevState.Name && curState.Name != NameOfRootState && prevState.Name!=NameStateOfTransitionBetweenBranch
                && curState.Name!=NameStateOfTransitionBetweenBranch && prevState.Sym!=GeneralNotation.EscapeSymbol)
            {
                nestedStates.Push((prevState, curIndex));
                visited[curIndex] = curState;
            }
            curIndex += 1;
        }

        private int ReturnToPastBranch(string str, SortedSet<int> stack, int curIndex, Stack<(State, int)> nestedStates)
        {
            stack.Add(curIndex);
            var prevData = nestedStates.Pop();
            prevState = prevData.Item1;
            curState = prevState.GetNextState(str[curIndex + 1].ToString())[0];
            stack.Add(prevData.Item2);
            curIndex += 1;
            return curIndex;
        }

        private void BuildAutomataForSyntaxElem(SyntaxElem syntax)
        {
            var startState = new State(false, RootState, false, StateType.Start, syntax.Name, syntax.StartSequence[0].ToString());
            var lastState = startState;
            char previous = ' ';
            var lastAndPrev = BuildAutomateFromPattern(syntax.StartSequence, previous,StateType.Start, lastState, RootState);
            var nestedStates = new List<State>();
            if (syntax.NestedElems.Count!=0)
                nestedStates = new List<State>(){RootState.GetStateByName(syntax.NestedElems[0])};
            lastAndPrev= BuildAutomateFromPattern(syntax.MiddleSequence, lastAndPrev.prev,
                StateType.Middle,lastAndPrev.last, RootState, nestedStates);
            lastAndPrev= BuildAutomateFromPattern(syntax.EndSequence, lastAndPrev.prev,
                StateType.End, lastAndPrev.last, RootState);
            RootState.NextStates.Add(startState.NextStates[0]);
            lastAndPrev.last.IsFinal = true;
            lastAndPrev.last.NextStates.Add(new State(false, RootState, true,StateType.Middle, 
                NameStateOfTransitionBetweenBranch, SymbolForTransactionBetweenBrach));

        }

        private (State last , char prev) BuildAutomateFromPattern(string pattern, char previous, StateType type,
            State lastState, State startState, List<State> branchesState = null)
        {   
            for(var i=0; i<pattern.Length; i++)
            {
                if (i!=0 && pattern[i] == GeneralNotation.OneOrMoreCountOfSymbol && !GeneralNotation.IsEscapeSymbol(pattern[i-1]))
                    lastState = AddNewTransactionWhenOneOfMoreRepeat(type, lastState);
                else if (pattern[i] == GeneralNotation.AnyCountOfSymbol && !GeneralNotation.IsEscapeSymbol(pattern[i-1]))
                    lastState.NextStates.Add(lastState);
                else if(i<=pattern.Length-2 && (pattern.Substring(i, 2)==GeneralNotation.LetterOrDigit))
                {
                    lastState = AddTransactionWhenSpecialSymbol(type, lastState, startState);
                    i += 1;
                }
                else
                    lastState = AddNewTransaction(pattern, type, lastState, startState, branchesState, i);
                previous = pattern[i];
            }
            lastState.IsTerminal = true;
            return (lastState, previous);
        }

        private State AddTransactionWhenSpecialSymbol(StateType type, State lastState, State startState)
        {
            var newState = new State(false, startState, false, type, lastState.Name, GeneralNotation.LetterOrDigit);
            lastState.NextStates.Add(newState);
            lastState = newState;
            return lastState;
        }

        private State AddNewTransactionWhenOneOfMoreRepeat(StateType type, State lastState)
        {
            var newState = new State(false, RootState, false, type, lastState.Name, lastState.Sym);
            lastState.NextStates.Add(newState);
            lastState = lastState.NextStates.Last();
            lastState.NextStates.Add(lastState);
            return lastState;
        }

        private State AddNewTransaction(string pattern, StateType type, State lastState, State startState, List<State> branchesState,
            int i)
        {
            var newState = new State(false, startState, false, type, lastState.Name, pattern[i].ToString());
            if (branchesState != null)
                foreach (var state in branchesState)
                {
                    newState.NextStates.Add(new State(false, startState, true, type, state.Name, SymbolForTransactionBetweenBrach));
                    newState.NextStates.Last().NextStates.Add(state);
                }
            lastState.NextStates.Add(newState);
            lastState = newState;
            return lastState;
        }
    }
}