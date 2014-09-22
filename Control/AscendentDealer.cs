using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TinyBASICAnalizer.ADS.AutomataStructure;
using TinyBASICAnalizer.ADS.GrammarAutomata;
using TinyBASICAnalizer.Model;
using TinyBASICAnalizer.Persistence;

namespace TinyBASICAnalizer.Control
{
    public class AscendentDealer
    {
        private SyntaxTable _table;
        /// <summary>
        /// Tabela da análise ascendente
        /// </summary>
        public SyntaxTable Table
        {
            get { return _table; }
            set { _table = value; }
        }

        private GrammarDeterministicAutomata _afd;
        /// <summary>
        /// Automato finito determínistico da gramática
        /// </summary>
        public GrammarDeterministicAutomata Afd
        {
            get { return _afd; }
            set { _afd = value; }
        }

        private List<GrammarState> _finalStatesList;
        /// <summary>
        /// Lista dos estados finais do afd
        /// </summary>
        public List<GrammarState> FinalStatesList
        {
            get { return _finalStatesList; }
            set { _finalStatesList = value; }
        }

        private NonRecursiveDealer _nonRecursiveDealer;

        private List<String> _productions;

        private static string _grammarFile
        {
            get { return "Model/Grammar/Grammar" + Symbol.GrammarIdentifier + ".txt"; }
        }

        public AscendentDealer(GrammarDeterministicAutomata afd)
        {
            _afd = afd;
            //Para a geração dos firsts e follows 
            _nonRecursiveDealer = new NonRecursiveDealer();
            _finalStatesList = new List<GrammarState>();
            _table = new SyntaxTable();
            _productions = new List<String>();

            //Numera todas as produções
            NumerateProductions();

            //Método que gera a tabela
            GenerateTable();
        }

        /// <summary>
        /// Gera a tabela da análise ascendente a partir do afd
        /// </summary>
        private void GenerateTable()
        {
            GrammarState initialState = GrammarState.ConvertFrom(Afd.InitialState);

            _table.AddEntry(Terminal.Initial, initialState, null, EntryType.ACCEPTED);

            GenerateTableEntries(initialState);

            //A partir daqui, todos os estados finais devem estar listados
            foreach (GrammarState state in FinalStatesList)
            {
                GrammarState usedState = GrammarState.ConvertFrom(Afd.GetState(state.StateValue));

                Symbol stateSymbol = state.GetStateSymbol();

                if (stateSymbol.Value.Equals(NonTerminal.AscendentSymbolString))
                {
                    _table.AddEntry(Terminal.Initial, state, null, EntryType.ACCEPTED);
                }
                else
                {
                    NonTerminal nonTerminal = (NonTerminal)stateSymbol;
                    List<Terminal> follows = nonTerminal.Follows;

                    foreach (Terminal follow in follows)
                    {      
                        _table.AddEntry(follow, GenerateReduction(state), null, EntryType.REDUCTION);
                    }
                }
            }
        }

        /// <summary>
        /// Gera a entrada das tabelas
        /// </summary>
        /// <param name="state"></param>
        private List<TableEntry> GenerateTableEntries(GrammarState state)
        {
            GrammarState stateC = new GrammarState(state.StateValue);
            if (!state.Equals(GrammarState.ConvertFrom(Afd.InitialState)))
            {
                state = GrammarState.ConvertFrom(Afd.GetState(state.StateValue));
            }

            List<TableEntry> entries = new List<TableEntry>();
            Symbol stateSymbol = state.GetStateSymbol();

            //Verifica se o estado é final para adicioná-lo à lista
            if (state.VerifyFinal())
            {
                //Aparentemente, essa verificação não está correta
                if (!FinalStatesList.Contains(state))
                {
                    state.IsFinal = true;
                    FinalStatesList.Add(state);
                }
            }

            //Varre todas as transições do estado
            foreach (StateTransiction<string, Symbol> transiction in state.Transictions)
            {
                Symbol symbol = transiction.Transiction;

                GrammarState grammarNextState = GrammarState.ConvertFrom(transiction.NextState);

                if (symbol is NonTerminal)
                {
                    TableEntry entry = new TableEntry(symbol, state, grammarNextState, EntryType.GOTO);
                    if (_table.AddEntry(entry))
                    {
                        GenerateTableEntries(grammarNextState);
                        entries.Add(entry);
                    }
                }
                else
                {
                    //Manda gerar as próximas entradas na tabela, mas não adiciona a entrada
                    if (grammarNextState != null)
                    {
                        TableEntry entry = new TableEntry(symbol, state, grammarNextState, EntryType.SHIFT);
                        if (_table.AddEntry(entry))
                        {
                            GenerateTableEntries(grammarNextState);
                            entries.Add(entry);
                        }
                    }
                    else
                    {
                        TableEntry entry = new TableEntry(symbol, state, null, EntryType.REDUCTION);
                        if (_table.AddEntry(entry))
                        {
                            entries.Add(entry);
                        }
                    }
                }
            }

            return entries;
        }

        /// <summary>
        /// 
        /// </summary>
        private void NumerateProductions()
        {
            string[] grammarFile = IOUtils.ReadTxtFile(_grammarFile);
            foreach (string line in grammarFile)
            {
                string[] parameters = line.Split(new char[] { ':' }, 2);
                string[] rules = parameters[1].Split('|');

                foreach (string rule in rules)
                {
                    _productions.Add(parameters[0] + ":" + rule);
                }
            }
        }

        //Gera o número associado à redução
        public GrammarState GenerateReduction(GrammarState state)
        {
            for (int i = 0; i < _productions.Count; i++)
            {
                NonTerminal nonterminal = (NonTerminal) state.GetStateSymbol();
                if (state.ReductionNumber==0)
                {
                    if (nonterminal.FirstContainsEmpty())
                    {                      
                        int reduction = _productions[i].Split(' ').Length - 1;
                        state.ReductionNumber = reduction;
                    }
                    else if (_productions[i].Equals(state.StateValue.Remove(state.StateValue.LastIndexOf('.') - 1)))
                    {
                        int reduction = _productions[i].Split(' ').Length;
                        state.ReductionNumber = reduction;
                    }
                }
            }
            return state;            
        }
    }
}