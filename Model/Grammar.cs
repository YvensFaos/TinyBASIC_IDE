using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TinyBASICAnalizer.Control;
using TinyBASICAnalizer.Persistence;

namespace TinyBASICAnalizer.Model
{
    /// <summary>
    /// Classe representante de uma gramática
    /// Deprecated
    /// </summary>
    public class Grammar
    {
        private string[] _grammarFile;
        /// <summary>
        /// Arquivo da gramática
        /// </summary>
        public string[] GrammarFile
        {
            get { return _grammarFile; }
            private set { _grammarFile = value; }
        }

        private string[] _terminalFile;
        /// <summary>
        /// Arquivo dos terminais da gramática
        /// </summary>
        public string[] TerminalFile
        {
            get { return _terminalFile; }
            private set { _terminalFile = value; }
        }

        private Symbol _initialSymbol;
        /// <summary>
        /// Símbolo inicial da gramática
        /// </summary>
        public Symbol InitialSymbol
        {
            get
            {
                if (_initialSymbol == null)
                {
                    string line = _grammarFile[0];

                    string[] parameters = line.Split(new char[] { ':' }, 2);
                    string value = parameters[0];
                    string rule = parameters[1];

                    _initialSymbol = new NonTerminal(value, rule);
                }
                return _initialSymbol;
            }
            private set { _initialSymbol = value; }
        }

        private Terminal _emptySymbol;
        /// <summary>
        /// Símbolo representante da cadeia nula ou vazia da gramática
        /// </summary>
        public Terminal EmptySymbol
        {
            get { return _emptySymbol; }
            set { _emptySymbol = value; }
        }

        private Dictionary<string, Terminal> _terminalHash;
        /// <summary>
        /// Lista de todos os terminais
        /// </summary>
        public Dictionary<string, Terminal> TerminalHash
        {
            get
            {
                if (_terminalHash == null)
                {
                    _terminalHash = new Dictionary<string, Terminal>();
                    _terminalHash.Add(EmptySymbol.Value, EmptySymbol);
                    _terminalHash.Add(Terminal.Initial.Value, Terminal.Initial);

                    foreach (string line in _terminalFile)
                    {
                        _terminalHash.Add(line, new Terminal(line));
                    }
                }
                return _terminalHash;
            }
            private set { _terminalHash = value; }
        }

        private Dictionary<string, NonTerminal> _nonTerminalHash;
        /// <summary>
        /// Lista de todos os não terminais
        /// </summary>
        public Dictionary<string, NonTerminal> NonTerminalHash
        {
            get
            {
                if (_nonTerminalHash == null)
                {
                    bool initialSymbol = true;
                    _nonTerminalHash = new Dictionary<string, NonTerminal>();

                    foreach (string line in _grammarFile)
                    {
                        string[] parameters = line.Split(new char[] { ':' }, 2);
                        NonTerminal nonTerminal = new NonTerminal(parameters[0], parameters[1]);
                        if (initialSymbol)
                        {
                            initialSymbol = false;
                            InitialSymbol = nonTerminal;
                        }
                        _nonTerminalHash.Add(parameters[0], nonTerminal);
                    }
                }
                return _nonTerminalHash;
            }
            private set { _nonTerminalHash = value; }
        }

        /// <summary>
        /// Construtor recebendo os arquivos já lidos da gramática e da lista de terminais
        /// </summary>
        /// <param name="grammarFile"></param>
        /// <param name="terminalFile"></param>
        public Grammar(string[] grammarFile, string[] terminalFile, Terminal emptySymbol)
        {
            _grammarFile = grammarFile;
            _terminalFile = terminalFile;
            _emptySymbol = emptySymbol;
        }

        /// <summary>
        /// Construtor recebendo o path para os arquivos da gramática e da lista de terminaiss
        /// </summary>
        /// <param name="grammarFilePath"></param>
        /// <param name="terminalFilePath"></param>
        public Grammar(string grammarFilePath, string terminalFilePath, Terminal emptySymbol)
            : this(IOUtils.ReadTxtFile(grammarFilePath), IOUtils.ReadTxtFile(terminalFilePath), emptySymbol)
        { }

    }
}
