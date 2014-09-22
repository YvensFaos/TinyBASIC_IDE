using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyBASICAnalizer.ADS.AutomataStructure
{
    /// <summary>
    /// Classe genérica da transição de estados.
    /// </summary>
    /// <typeparam name="T">Valor contido em cada estado.</typeparam>
    /// <typeparam name="E">Valor esperado para as transições de estados.</typeparam>
    public class StateTransiction <T, E>
    {
        private E _transiction;
        /// <summary>
        /// Valor T que invoca essa transição
        /// </summary>
        public E Transiction
        {
            get { return _transiction; }
            set { _transiction = value; }
        }

        private State<T, E> _nextState;
        /// <summary>
        /// Próximo estado que liga essa transição
        /// </summary>
        public State<T, E> NextState
        {
            get { return _nextState; }
            set { _nextState = value; }
        }

        public StateTransiction()
            : this(default(E), new State<T, E>())
        { }

        public StateTransiction(E transiction, State<T, E> nextState)
        {
            _transiction = transiction;
            _nextState = nextState;
        }

        public StateTransiction(StateTransiction<T, E> anotherTransiction)
        {
            _transiction = anotherTransiction.Transiction;
            _nextState = anotherTransiction.NextState.Clone();
        }

        public override string ToString()
        {
            return "[" + Transiction.ToString() + " : " + NextState.ToString() + "]";
        }

        /// <summary>
        /// Retorna uma cópia profunda da transição
        /// </summary>
        /// <returns></returns>
        public StateTransiction<T, E> Clone()
        {
            StateTransiction<T, E> clonedTransiction = new StateTransiction<T, E>(this);
            return clonedTransiction;
        }
    }
}
