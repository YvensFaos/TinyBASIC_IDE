using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TinyBASICAnalizer.Persistence;
namespace TinyBASICAnalizer.Model
{
    /// <summary>
    /// Classe pai na hierarquia dos não terminais e terminais
    /// </summary>
    public abstract class Symbol
    {
        public static string AscendentSymbolString = "X";

        private static string _grammarIdentifier = "10";
        /// <summary>
        /// Grammar Identifier
        /// </summary>
        public static string GrammarIdentifier
        {
            get { return _grammarIdentifier; }
            set
            {
                _grammarIdentifier = value;
                ResetGrammar();
            }
        }

        private static void ResetGrammar()
        {
            _initialSymbol = null;
            _terminalHash = null;
            _nonTerminalHash = null;
        }

        #region Região de propriedades estáticas do Símbolo
        private static string _grammarFile
        {
            get { return "Model/Grammar/Grammar" + GrammarIdentifier + ".txt"; }
        }
        private static string _terminalFile
        {
            get { return "Model/Grammar/TerminalList" + GrammarIdentifier + ".txt"; }
        }

        private static NonTerminal _initialSymbol;
        /// <summary>
        /// Símbolo inicial da gramática
        /// </summary>
        public static NonTerminal InitialSymbol
        {
            get 
            {
                if (_initialSymbol == null)
                {
                    string[] grammar = IOUtils.ReadTxtFile(_grammarFile);
                    string line = grammar[0];

                    string[] parameters = line.Split(new char[] { ':' }, 2);
                    string value = parameters[0];
                    string rule  = parameters[1];

                    _initialSymbol = new NonTerminal(value, rule);
                }
                return Symbol._initialSymbol; 
            }
            private set { Symbol._initialSymbol = value; }
        }

        private static Dictionary<string, Terminal> _terminalHash;
        /// <summary>
        /// Lista de todos os terminais
        /// </summary>
        public static Dictionary<string, Terminal> TerminalHash
        {
            get 
            {
                if (_terminalHash == null)
                {
                    _terminalHash = new Dictionary<string, Terminal>();
                    _terminalHash.Add(Terminal.Empty.Value, Terminal.Empty);
                    _terminalHash.Add(Terminal.Initial.Value, Terminal.Initial);

                    string[] terminalFile = IOUtils.ReadTxtFile(_terminalFile);
                    foreach (string line in terminalFile)
                    {
                        _terminalHash.Add(line, new Terminal(line));
                    }
                }
                return _terminalHash; 
            }
            private set { _terminalHash = value; }
        }

        private static List<Terminal> _terminalList;
        /// <summary>
        /// Lista de todos os terminais
        /// </summary>
        public static List<Terminal> TerminalList
        {
            get
            {
                if (_terminalList == null)
                {
                    _terminalList = new List<Terminal>();

                    foreach (string key in TerminalHash.Keys)
                    {
                        _terminalList.Add(TerminalHash[key]);
                    }

                }
                return _terminalList;
            }
            private set { _terminalList = value; }
        }

        private static Dictionary<string, NonTerminal> _nonTerminalHash;
        /// <summary>
        /// Lista de todos os não terminais
        /// </summary>
        public static Dictionary<string, NonTerminal> NonTerminalHash
        {
            get 
            {
                if (_nonTerminalHash == null)
                {
                    bool initialSymbol = true;
                    _nonTerminalHash = new Dictionary<string, NonTerminal>();
                    string[] grammar = IOUtils.ReadTxtFile(_grammarFile);

                    foreach (string line in grammar)
                    {
                        string[] parameters = line.Split(new char[] { ':' }, 2);
                        NonTerminal nonTerminal = new NonTerminal(parameters[0], parameters[1]);
                        if (initialSymbol)
                        {
                            initialSymbol = false;
                            InitialSymbol = nonTerminal;

                            //Para os ascendentes

                            _nonTerminalHash.Add(AscendentSymbolString, new NonTerminal(AscendentSymbolString, AscendentSymbolString + parameters[0]));
                        }
                        _nonTerminalHash.Add(parameters[0], nonTerminal);
                    }
                }
                return _nonTerminalHash; 
            }
            private set { _nonTerminalHash = value; }
        }

        private static List<NonTerminal> _nonTerminalList;
        /// <summary>
        /// Lista de todos os terminais
        /// </summary>
        public static List<NonTerminal> NonTerminalList
        {
            get
            {
                if (_nonTerminalList == null)
                {
                    _nonTerminalList = new List<NonTerminal>();

                    foreach (string key in NonTerminalHash.Keys)
                    {
                        //if (!key.Equals(Terminal.Empty.Value) && !key.Equals(Terminal.Initial.Value))
                        //{
                            _nonTerminalList.Add(NonTerminalHash[key]);
                        //}
                    }

                }
                return _nonTerminalList;
            }
            private set { _nonTerminalList = value; }
        }
        #endregion

        private string _value;
        /// <summary>
        /// Valor do não terminal
        /// </summary>
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public Symbol(string Value)
        {
            this.Value = Value;
        }

        /// <summary>
        /// Dado um símbolo retorna um não terminal ou um terminal dependendo de seu tipo
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static Symbol GetSymbol(string symbol)
        {
            if (NonTerminalHash.ContainsKey(symbol))
            {
                return NonTerminalHash[symbol];

            }
            //Se não contiver, verificamos a lista de terminais
            else if (TerminalHash.ContainsKey(symbol))
            {
                return TerminalHash[symbol];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Dado uma regra é retornado a lista dos símbolos que compõem a regra
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        public static List<Symbol> InterpreteRule(string rule)
        {
            List<Symbol> ruleList = new List<Symbol>();
            if (rule.Contains(" "))
            {
                string[] parameters = rule.Split(' ');
                foreach (string stringSymbol in parameters)
                {
                    Symbol symbol = GetSymbol(stringSymbol);
                    if (symbol != null)
                    {
                        ruleList.Add(symbol);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                Symbol symbol = GetSymbol(rule);
                if (symbol != null)
                {
                    ruleList.Add(symbol);
                }
                else
                {
                    return null;
                }
            }

            return ruleList;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
