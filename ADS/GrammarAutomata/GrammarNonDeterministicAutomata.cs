using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TinyBASICAnalizer.Model;
using TinyBASICAnalizer.ADS.AutomataStructure;

namespace TinyBASICAnalizer.ADS.GrammarAutomata
{
    /// <summary>
    /// Classe que gera o automato finito não deterministico com transição em vazio a partir da gramática
    /// string - regra com o ponto
    /// symbol - símbolo da transição
    /// </summary>
    public class GrammarNonDeterministicAutomata : Automata<string, Symbol>
    {
        private static string _grammarFile 
        {
            get { return "Model/Grammar/Grammar" + Symbol.GrammarIdentifier + ".txt"; }
        }
        private string ascendentSymbol = Symbol.AscendentSymbolString;

        public GrammarNonDeterministicAutomata()
        {
            GenerateAutomata();
        }

        /// <summary>
        /// Método para geração do automato baseando-se na gramática
        /// </summary>
        private void GenerateAutomata()
        {
            #region Gerar o primeiro nó do automato
            //Regra no formato
            //S:símbolo inicial
            string baseRule = ascendentSymbol + ":";
            string ascendentRule = baseRule + NonTerminal.InitialSymbol;

            //Primeiro nó do automato é só S
            //Não é final
            InitialState = new State<string, Symbol>(ascendentSymbol);
            StateHash.Add(ascendentRule, InitialState);

            //Regra no fomato
            //S:. símbolo inicial
            //Trata-se da primeira regra com ponto
            string initialRule = baseRule + ". " + NonTerminal.InitialSymbol.Value;
            GrammarState initialSymbolState = new GrammarState(initialRule);
            StateHash.Add(initialRule, initialSymbolState);

            //Cria a transição em vazio do estado inicial para o estado do símbolo inicial
            //Adiciona a regra à lista de transições do estado inicial
            InitialState.AddTransiction(Terminal.Empty, initialSymbolState);

            //Chama o método que recursivamente vai gerar os outros estados
            GenerateTransictions(initialSymbolState, InitialState);

            //Gera a lista dos estados
            GenerateStateList();
            #endregion
        }


        /// <summary>
        /// Método para iterar sobre o hash de estados e adicioná-los à lista de estados
        /// </summary>
        public void GenerateStateList()
        {
            foreach (string key in StateHash.Keys)
            {
                GrammarState state = GrammarState.ConvertFrom(StateHash[key]);
                if (state != null)
                {
                    if (!StateList.Contains(state))
                    {
                        StateList.Add(state);
                    }
                }
            }
        }

        /// <summary>
        /// Método para gerar as transições a partir de regra de um dado estado
        /// </summary>
        /// <param name="state">Novo estado</param>
        /// <param name="before">Estado que está apontando diretamente para este novo estado</param>
        private void GenerateTransictions(State<string, Symbol> state, State<string, Symbol> before)
        {
            //StateValue é, no caso, a regra com o ponto
            Symbol symbol = GetNextToPoint(state.StateValue);

            if (symbol != null)
            {
                //Preciso verificar se já existe transição para aquele símbolo, se já existir, não faz nada
                if (before.TransictionIn(symbol) != null)
                {
                    return;
                }

                if (symbol is NonTerminal)
                {
                    #region Regra 2
                    NonTerminal nonTerminal = (NonTerminal)symbol;

                    //Regra 2

                    //Criar uma nova regra para as substituições do não terminal e adiciona as transições em vazio para elas
                    string nonTerminalRule = nonTerminal.Rule;
                    string[] parameters = new string[]{ };

                    if (nonTerminalRule.Contains("|"))
                    {
                        parameters = nonTerminalRule.Split('|');
                    }
                    else
                    {
                        parameters = new string[] { nonTerminalRule };
                    }

                    bool exists = false;
                    GrammarState newState = null;
                    foreach (string ruleElement in parameters)
                    {
                        //Criar apenas um estado para a substituição do não terminal
                        string pointedRule = nonTerminal.Value + ":. " + ruleElement;

                        //Verifica se o estado já existe
                        exists = false;
                        newState = null;
                        if (StateHash.ContainsKey(pointedRule))
                        {
                            newState = (GrammarState)StateHash[pointedRule];
                            exists = true;
                        }
                        else
                        {
                            newState = new GrammarState(pointedRule);
                            StateHash.Add(pointedRule, newState);
                        }

                        state.AddTransiction(Terminal.Empty, newState);

                        //Se o estado não existia ainda, gerar as transições a partir dele
                        if (!exists)
                        {
                            GenerateTransictions(newState, state);
                        }

                        //Mover o point
                        string newRule = MovePointToRight(state.StateValue);

                        exists = false;
                        newState = null;
                        if (StateHash.ContainsKey(newRule))
                        {
                            newState = (GrammarState)StateHash[newRule];
                            exists = true;
                        }
                        else
                        {
                            newState = new GrammarState(newRule);
                            StateHash.Add(newRule, newState);
                        }                        

                        if (!exists)
                        {
                            state.AddTransiction(nonTerminal, newState);
                            GenerateTransictions(newState, state);
                        }
                    }

                    if (nonTerminal.FirstContainsEmpty())
                    {
                        state.IsFinal = true;
                    }
                    
                    #endregion
                }
                else
                {
                    Terminal terminal = (Terminal)symbol;
                    if (terminal.Equals(Terminal.FinalPoint))
                    {
                        //Estado de item completo
                        state.IsFinal = true;
                    }
                    else
                    {
                        if (terminal.Equals(Terminal.Empty))
                        {
                            //Estado anterior é final
                            before.IsFinal = true;
                        }
                        #region Regra 3
                        //Regra 3

                        //Mover o ponto, criar um estado com a nova regra e a transição será neste terminal
                        string newRule = MovePointToRight(state.StateValue);

                        bool exists = false;
                        GrammarState newState = null;
                        if (StateHash.ContainsKey(newRule))
                        {
                            newState = (GrammarState)StateHash[newRule];
                            exists = true;
                        }
                        else
                        {
                            newState = new GrammarState(newRule);
                            StateHash.Add(newRule, newState);
                        }

                        state.AddTransiction(terminal, newState);

                        if (!exists)
                        {
                            GenerateTransictions(newState, state);
                        }
                    }
                    #endregion
                }
            }
        }

        /// <summary>
        /// Retorna o indice da posição do ponto dentro da regra
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        private int PointIndex(string rule)
        {
            string point = Terminal.Point.Value;
            return rule.IndexOf(point);
        }

        /// <summary>
        /// Retorna a nova regra depois de mover o ponto uma posição para a direita.
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        private string MovePointToRight(string rule)
        {
            int index = PointIndex(rule);
            if (index != -1)
            {
                if (index == rule.Length - 1)
                {
                    return "";
                }
                else
                {                    
                    string newRule = rule.Split(new char[] { ':' }, 2)[1];

                    string[] parameters = newRule.Split(' ');

                    newRule = rule.Split(':')[0] + ":";
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        if (!parameters[i].Equals(""))
                        {
                            if (parameters[i][0] == '.')
                            {
                                parameters[i] = parameters[i + 1];
                                parameters[i + 1] = ".";
                                i++;
                            }
                        }
                    }

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        newRule += parameters[i] + " ";
                    }

                    //Para remover o último espaço
                    newRule = newRule.Substring(0, newRule.Length - 1);

                    return newRule;
                }
            }
            return null;
        }

        /// <summary>
        /// Retorna o próximo terminal/não terminal ao lado direito do ponto
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        private Symbol GetNextToPoint(string rule)
        {
            int index = PointIndex(rule);
            if(index != -1)
            {
                if (index == rule.Length - 1)
                {
                    //Retorna o símbolo de que o ponto está na última posição
                    return Terminal.FinalPoint;
                }
                else
                {
                    //Index+1 por causa do espaço que vem depois do .
                    string aux = rule.Substring(index);
                    //Posição 0 é a posição do .
                    string value = aux.Split(' ')[1];
                    return Symbol.GetSymbol(value);
                }
            }
            return null;
        }

        /// <summary>
        /// Retorna o automato genérico a partir do automato de gramática
        /// </summary>
        /// <returns></returns>
        public Automata<string, Symbol> ToGenericAutomata()
        {
            Automata<string, Symbol> automata = new Automata<string, Symbol>(InitialState, StateList, TransictionList, NullSymbol);
            return automata;
        }
    }
}
