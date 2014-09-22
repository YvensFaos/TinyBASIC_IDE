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
    /// Classe para representar o fecho de uma gramática
    /// </summary>
    public class GrammarPowerSet : PowerSet<string, Symbol>
    {
        private GrammarState _state;
        /// <summary>
        /// Estado específico do fecho
        /// </summary>
        public GrammarState State
        {
            get { return _state; }
            set { _state = value; }
        }

        public GrammarPowerSet()
            : base()
        { }

        public GrammarPowerSet(GrammarState state)
            : base(state)
        {
            _state = new GrammarState(state);
        }

        /// <summary>
        /// Retorna o estado correspondente ao fecho.
        /// </summary>
        /// <returns>O estado correspondente.</returns>
        new public GrammarState PowerSetToState()
        {
            GrammarState newState = new GrammarState(State);

            foreach (StateTransiction<string, Symbol> transiction in Transictions)
            {
                if (!transiction.Transiction.Value.Equals(Terminal.Empty))
                {
                    newState.AddTransiction(transiction);
                }
            }

            return newState;
        }
    }
}
