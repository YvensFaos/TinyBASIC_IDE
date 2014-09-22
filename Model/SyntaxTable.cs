using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TinyBASICAnalizer.ADS.GrammarAutomata;

namespace TinyBASICAnalizer.Model
{
    class SymbolPair
    {
        private GrammarState _vertical;
        /// <summary>
        /// Coluna vertical da tabela
        /// </summary>
        public GrammarState Vertical
        {
            get { return _vertical; }
            set { _vertical = value; }
        }

        private Symbol _horizontal;
        /// <summary>
        /// Coluna horizontal da tabela
        /// </summary>
        public Symbol Horizontal
        {
            get { return _horizontal; }
            set { _horizontal = value; }
        }

        public SymbolPair(Symbol horizontal, GrammarState vertical)
        {
            _vertical = vertical;
            _horizontal = horizontal;
        }

        public override string ToString()
        {
            return "[" + Vertical + ", " + Horizontal + "]";
        }
    }

    public class SyntaxTable
    {
        Dictionary<String, TableEntry> _tableEntryHash;

        public SyntaxTable()
        {
            _tableEntryHash = new Dictionary<String, TableEntry>();
        }

        /// <summary>
        /// Retorna todas as entradas na tabela que contém aquele estado na vertical da tabela
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public List<TableEntry> TableEntriesFor(GrammarState state)
        {
            List<TableEntry> listEntry = new List<TableEntry>();
            foreach (string entryString in _tableEntryHash.Keys)
            {
                TableEntry entry = _tableEntryHash[entryString];

                if (entry.Vertical.Equals(state))
                {
                    listEntry.Add(entry);
                }
            }

            return listEntry;
        }

        /// <summary>
        /// Retorna todas as entradas na tabela que contém transição vertical no símbolo passado
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public List<TableEntry> TableEntriesIn(Symbol symbol)
        {
            List<TableEntry> listEntry = new List<TableEntry>();
            foreach (string entryString in _tableEntryHash.Keys)
            {
                TableEntry entry = _tableEntryHash[entryString];

                if (entry.Horizontal.Equals(symbol))
                {
                    listEntry.Add(entry);
                }
            }

            return listEntry;
        }

        /// <summary>
        /// Adiciona uma entrada na tabela
        /// </summary>
        /// <param name="horizontal"></param>
        /// <param name="vertical"></param>
        /// <param name="nextState"></param>
        /// <param name="type"></param>
        /// <returns>Verdadeiro caso essa entrada seja nova na tabela</returns>
        public bool AddEntry(Symbol horizontal, GrammarState vertical, GrammarState nextState, EntryType type)
        {
            return AddEntry(new TableEntry(horizontal, vertical, nextState, type));
        }
        
        /// <summary>
        /// Adiciona uma entrada na tabela
        /// </summary>
        /// <param name="entry"></param>
        /// <returns>Verdadeiro caso essa entrada seja nova na tabela</returns>
        public bool AddEntry(TableEntry entry)
        {
            SymbolPair pair = new SymbolPair(entry.Horizontal, entry.Vertical);
            if (!HasEntry(pair))
            {
                _tableEntryHash.Add(pair.ToString(), entry);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Retorna verdadeiro ou false se tiver a entrada com os símbolos passados
        /// </summary>
        /// <param name="horizontal"></param>
        /// <param name="vertical"></param>
        /// <returns></returns>
        public bool HasEntry(Symbol horizontal, GrammarState vertical)
        {
            SymbolPair pair = new SymbolPair(horizontal, vertical);
            return HasEntry(pair);
        }

        /// <summary>
        /// Retorna verdadeiro ou false se tiver a entrada com os símbolos passados
        /// </summary>
        /// <param name="pair"></param>
        /// <returns></returns>
        private bool HasEntry(SymbolPair pair)
        {
            return _tableEntryHash.ContainsKey(pair.ToString());
        }

        /// <summary>
        /// Retorna a entrada para o par passado. Returna nulo caso a entrada não exista.
        /// </summary>
        /// <param name="horizontal"></param>
        /// <param name="vertical"></param>
        /// <returns></returns>
        public TableEntry GetEntry(Symbol horizontal, GrammarState vertical)
        {
            SymbolPair pair = new SymbolPair(horizontal, vertical);
            return GetEntry(pair);
        }

        /// <summary>
        /// Retorna a entrada para o par passado. Returna nulo caso a entrada não exista.
        /// </summary>
        /// <param name="pair"></param>
        /// <returns></returns>
        private TableEntry GetEntry(SymbolPair pair)
        {
            if (HasEntry(pair))
            {
                return _tableEntryHash[pair.ToString()];
            }
            return null;
        }
    }
}
