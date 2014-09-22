using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using TinyBASICAnalizer.Model;
using TinyBASICAnalizer.ADS.AutomataStructure;

namespace TinyBASICAnalizer.ADS.GrammarAutomata
{
    public class GrammarDeterministicAutomata : Automata<string, Symbol>
    {
        /// <summary>
        /// Construtor default de um automato de gramática. Criado a partir da gramática setada em Symbol.
        /// </summary>
        public GrammarDeterministicAutomata()
        {
            GrammarNonDeterministicAutomata nonDeterministic = new GrammarNonDeterministicAutomata();

            //Cria uma referência aos terminais para comparações
            Dictionary<string, Terminal> terminalHash = Symbol.TerminalHash;
            Dictionary<string, NonTerminal> nonTerminalHash = Symbol.NonTerminalHash;

            GrammarState initialState = GrammarState.ConvertFrom(nonDeterministic.InitialState);

            Dictionary<GrammarState, GrammarPowerSet> hash = new Dictionary<GrammarState, GrammarPowerSet>();

            List<Symbol> transictionList = new List<Symbol>();
            transictionList.AddRange(Symbol.TerminalList);
            transictionList.AddRange(Symbol.NonTerminalList);

            //Retorna com o hash completo com todos os fechos
            GrammarPowerSet initialPowerSet = GeneratePowerSet(initialState, transictionList, Terminal.Empty, hash);

            //Carrega o valor do estado do primeiro fecho transitivo
            initialState = initialPowerSet.PowerSetToState();

            //Carrega a lista com os estados do fecho transitivo criado
            List<GrammarState> stateList = new List<GrammarState>();
            foreach (GrammarState state in hash.Keys)
            {
                GrammarPowerSet powerSet = hash[state];
                stateList.Add(powerSet.PowerSetToState());
            }

            InitialState = initialPowerSet.PowerSetToState();
            //StateList = stateList;
            foreach (GrammarState state in stateList)
            {
                StateList.Add(state.ConvertTo());
                StateHash.Add(state.StateValue, state);
            }
            TransictionList = transictionList;
            NullSymbol = Terminal.Empty;

            #region código abandonado
            //List<Symbol> transictionList = new List<Symbol>();
            //foreach (string name in terminalHash.Keys)
            //{
            //    transictionList.Add(terminalHash[name]);
            //}
            //foreach (string name in nonTerminalHash.Keys)
            //{
            //    transictionList.Add(nonTerminalHash[name]);
            //}

            //Automata<string, Symbol> afd = Automata<string, Symbol>.AFNtoAFD(afn, transictionList, Terminal.Empty);

            //InitialState = afd.InitialState;
            //StateList = afd.StateList;
            //TransictionList = afd.TransictionList;
            //NullSymbol = afd.NullSymbol;

            ////Popular o hash
            //foreach(State<string, Symbol> afdState in StateList)
            //{
            //    StateHash.Add(afdState.StateValue, afdState);
            //}
            #endregion
        }

        /// <summary>
        /// Gera o fecho transitivo de um determinado estado. Realiza as inserções no hash.
        /// </summary>
        /// <param name="state">Estado do fecho transitivo</param>
        /// <param name="transictionList">Lista dos possíveis valores para a transição de estados</param>
        /// <param name="nullSymbol">Símbolo representando do nulo/vazio dos automatos</param>
        /// <param name="hash">Hash dos fechos já criados</param>
        /// <returns>Fecho transitivo do estado</returns>
        private static GrammarPowerSet GeneratePowerSet(GrammarState state, List<Symbol> transictionList, Terminal nullSymbol, Dictionary<GrammarState, GrammarPowerSet> hash)
        {
            GrammarPowerSet powerSet = new GrammarPowerSet(state);
            if (hash.ContainsKey(state))
            {
                return null;
            }

            hash.Add(state, powerSet);

            foreach (Symbol symbol in transictionList)
            {
                if (!symbol.Equals(Terminal.Empty))
                {
                    List<GrammarState> reachable = state.ReachableStatesBy(symbol);
                    if (reachable != null)
                    {
                        if (reachable.Count > 1)
                        {
                            //Adiciona transições de estados com recursão
                            if (!reachable[0].StateValue.Equals(reachable[1].StateValue))
                            {
                                //Um pouco gambiarra

                                //Busca os estados únicos na lista do reachable, porque as vezes o mesmo estado aparece mais de uma vez
                                Dictionary<string, GrammarState> uniqueHash = new Dictionary<string, GrammarState>();

                                foreach (GrammarState reachableState in reachable)
                                {
                                    if (!uniqueHash.ContainsKey(reachableState.StateValue))
                                    {
                                        uniqueHash.Add(reachableState.StateValue ,reachableState);
                                    }
                                }

                                //Sempre devem vir no estilo 
                                //A := B (final)
                                //A := B C (não final)
                                //ou ambos são finais, então eu pego o que tem a maior regra

                                GrammarState unionState = null;

                                //Itero o hash; A prioridade é o estado que não é final
                                //Caso não tenha estado não-final, fica o com a maior regra
                                foreach (string key in uniqueHash.Keys)
                                {
                                    GrammarState reachableState = uniqueHash[key];
                                    if (!reachableState.IsFinal)
                                    {
                                        unionState = reachableState;
                                    }
                                    if (unionState == null)
                                    {
                                        unionState = reachableState;
                                    }
                                    else
                                    {
                                        if (unionState.StateValue.Length < reachableState.StateValue.Length)
                                        {
                                            unionState = reachableState;
                                        }
                                    }
                                }
                                //Por garantia
                                unionState.IsFinal = true;

                                //Realiza o fecho a partir desse estado
                                powerSet.AddTransictionToPowerSet(symbol, unionState);

                            }
                            else
                            {
                                //Se são iguais, então basta inserir um deles
                                powerSet.AddTransictionToPowerSet(symbol, reachable[0]);
                            }
                        }
                        else
                        {
                            //Adiciona transição no valor para o estaddo
                            powerSet.AddTransictionToPowerSet(symbol, reachable[0]);
                        }
                    }
                }
            }

            //Itera sobre os estados que não contém fecho e os gera
            foreach (StateTransiction<string, Symbol> transiction in powerSet.Transictions)
            {
                GrammarState nextState = GrammarState.ConvertFrom(transiction.NextState);
                if (!hash.ContainsKey(nextState))
                {
                    GeneratePowerSet(nextState, transictionList, nullSymbol, hash);
                }
            }

            return powerSet;
        }

        /// <summary>
        /// Construtor para um automato de gramática a partir de um automato genérico com os parâmetros string, Symbol
        /// </summary>
        /// <param name="automata">Original</param>
        public GrammarDeterministicAutomata(Automata<string, Symbol> automata)
            : base(automata.InitialState, automata.StateList, automata.TransictionList, automata.NullSymbol)
        { }

        /// <summary>
        /// Construtor para um automata recebendo o estado inicial e a lista de todos os estados já com suas transições.
        /// </summary>
        /// <param name="initialState">Estado inicial do automato</param>
        /// <param name="listStates">Lista dos estados com suas transições</param>
        /// <param name="nullSymbol">Representa o valor nulo ou vazio do automato</param>
        public GrammarDeterministicAutomata(State<string, Symbol> initialState, List<State<string, Symbol>> listStates, List<Symbol> transictionList, Terminal nullSymbol)
            : base(initialState, listStates, transictionList, nullSymbol)
        { }
    }
}
