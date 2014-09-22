using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TinyBASICAnalizer.ADS.AutomataStructure;
using TinyBASICAnalizer.Model;

namespace TinyBASICAnalizer.ADS.GrammarAutomata
{
    /// <summary>
    /// Classe para representar um estado específico do automato da gramática
    /// </summary>
    public class GrammarState : State<string, Symbol>
    {
        public GrammarState(string stateValue)
            : base(stateValue, false)
        { }

        public GrammarState(string stateValue, bool isFinal)
            : base(stateValue, isFinal)
        { }

        public GrammarState(GrammarState anotherState)
        {
            StateValue = anotherState.StateValue;
            IsFinal = anotherState.IsFinal;
        }


        private int _reductionNumber;
        /// <summary>
        /// Número da redução do estado
        /// </summary>
        public int ReductionNumber
        {
            get { return _reductionNumber; }
            set { _reductionNumber = value; }
        }

        /// <summary>
        /// Retorna a lista de todos os estados alcançáveis por vázio do estado
        /// </summary>
        /// <returns></returns>
        public List<GrammarState> ReachableStatesByEmpty()
        {
            List<GrammarState> list = new List<GrammarState>();
            List<State<string, Symbol>> reachableList = base.ReachableStatesBy(Terminal.Empty);

            if (reachableList == null)
            {
                return null;
            }

            foreach (State<string, Symbol> state in reachableList)
            {
                list.Add((GrammarState)state);
            }

            return list;
        }

        /// <summary>
        /// Retorna a lista de todos os estados alcançável pelo símbolo passado
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        new public List<GrammarState> ReachableStatesBy(Symbol symbol)
        {
            List<GrammarState> list = new List<GrammarState>();
            List<State<string, Symbol>> reachableList = base.ReachableStatesBy(symbol, Terminal.Empty);

            if (reachableList == null)
            {
                return null;
            }

            foreach (State<string, Symbol> state in reachableList)
            {
                list.Add((GrammarState)state);
            }

            return list;
        }

        /// <summary>
        /// Retorna a lista de todos os estados alcançáveis pelo token passado
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public List<GrammarState> NextStateFrom(Token token)
        {
            if (token != null)
            {
                Symbol symbol = Symbol.GetSymbol(token.Value);
                if (symbol == null)
                {
                    switch (token.Type)
                    {
                        case TokenType.VALUE:
                            symbol = Symbol.TerminalHash["num"];
                            break;
                        case TokenType.VARIABLE:
                            symbol = Symbol.TerminalHash["var"];
                            break;
                    }
                }
                return ReachableStatesBy(symbol);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Converte um estado genérico com T = string e E = símbolo para um GrammarState
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static GrammarState ConvertFrom(State<string, Symbol> state)
        {
            if (state == null)
            {
                return null;
            }

            GrammarState newState = new GrammarState(state.StateValue, state.IsFinal);
            List<StateTransiction<string, Symbol>> transictions = state.Transictions;

            foreach(StateTransiction<string, Symbol> transiction in transictions)
            {
                newState.AddTransiction(transiction);
            }

            return newState;
        }

        public State<string, Symbol> ConvertTo()
        {
            State<string, Symbol> newState = new State<string, Symbol>(StateValue, IsFinal);
            newState.Transictions.AddRange(Transictions);

            return newState;
        }

        /// <summary>
        /// Retorna o símbolo do estado
        /// </summary>
        /// <returns></returns>
        public Symbol GetStateSymbol()
        {
            Symbol stateSymbol = null;
            string stateValue = StateValue;

            //Adquire o símbolo do estado atual
            if (stateValue.Contains(':'))
            {
                stateSymbol = Symbol.GetSymbol(stateValue.Split(':')[0]);
            }
            else
            {
                stateSymbol = Symbol.GetSymbol(stateValue);
            }

            return stateSymbol;
        }

        /// <summary>
        /// Métoo para verificar se o estado é final segundo a regra do ponto
        /// </summary>
        /// <returns></returns>
        public bool VerifyFinal()
        {
            if (StateValue.Contains("."))
            {
                int lastIndex = StateValue.LastIndexOf('.');
                return StateValue[StateValue.Length - 1] == StateValue[lastIndex];
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj is GrammarState)
            {
                GrammarState anotherState = (GrammarState)obj;
                return anotherState.StateValue.Equals(StateValue) && (anotherState.IsFinal == IsFinal);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return StateValue.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
