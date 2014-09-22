using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TinyBASICAnalizer.ADS.GrammarAutomata;

namespace TinyBASICAnalizer.Model
{
    public enum EntryType
    {
        SHIFT, GOTO, ACCEPTED, REDUCTION
    }

    /// <summary>
    /// Classe que representa a entrada na tabela
    /// </summary>
    public class TableEntry
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

        private GrammarState _nextState;
        /// <summary>
        /// Estado que a tabela aponta
        /// </summary>
        public GrammarState NextState
        {
            get { return _nextState; }
            set { _nextState = value; }
        }

        private EntryType _type;
        /// <summary>
        /// Tipo da entrada: Shift ou GOTO
        /// </summary>
        public EntryType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public TableEntry()
            : this(null, null, null, default(EntryType))
        { }

        public TableEntry(Symbol horizontal, GrammarState vertical, GrammarState nextState, EntryType type)
        {
            _horizontal = horizontal;
            _vertical = vertical;
            _nextState = nextState;
            _type = type;
        }

        public override string ToString()
        {
            return Type.ToString() + " [" + Vertical.ToString() + ", " + Horizontal.ToString() + "] " + NextState.ToString();
        } 
    }
}
