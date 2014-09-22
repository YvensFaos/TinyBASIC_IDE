using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyBASICAnalizer.ADS.AutomataStructure
{
    /// <summary>
    /// Classe genérica do estado de um automato
    /// </summary>
    /// <typeparam name="T">Valor contido em cada estado.</typeparam>
    /// <typeparam name="E">Valor esperado para as transições de estados.</typeparam>
    public class State <T, E>
    {
        private T _stateValue;
        /// <summary>
        /// Valor carregado pelo estado
        /// </summary>
        public T StateValue
        {
            get { return _stateValue; }
            set { _stateValue = value; }
        }

        private bool _isFinal;
        /// <summary>
        /// Determina se o estado é final
        /// </summary>
        public bool IsFinal
        {
            get { return _isFinal; }
            set { _isFinal = value; }
        }

        private List<StateTransiction<T, E>> _transictions;
        /// <summary>
        /// Lista de transições do estado
        /// </summary>
        public List<StateTransiction<T, E>> Transictions
        {
            get 
            {
                if (_transictions == null)
                {
                    _transictions = new List<StateTransiction<T, E>>();
                }
                return _transictions; 
            }
            private set { _transictions = value; }
        }

        public State()
            : this(default(T))
        { }

        public State(T stateValue)
            : this(stateValue, false)
        { }

        public State(T stateValue, bool isFinal)
        {
            _stateValue = stateValue;
            _isFinal = isFinal;
        }

        /// <summary>
        /// Construtor de cópia profunda
        /// </summary>
        /// <param name="anotherState"></param>
        public State(State<T, E> anotherState)
        {
            _stateValue = anotherState.StateValue;
            _isFinal = anotherState.IsFinal;
        }

        /// <summary>
        /// Adiciona a transição à lista de transições
        /// </summary>
        /// <param name="transiction"></param>
        public void AddTransiction(StateTransiction<T, E> transiction)
        {
            Transictions.Add(transiction);            
        }

        /// <summary>
        /// Adiciona a transição à lista de transições
        /// </summary>
        /// <param name="transictionValue"></param>
        /// <param name="state"></param>
        public void AddTransiction(E transictionValue, State<T, E> state)
        {
            AddTransiction(new StateTransiction<T,E>(transictionValue, state));
        }

        /// <summary>
        /// Retorna a lista de estados com transição no parâmetro value alcançáveis direta e indiretamente pelo estado
        /// </summary>
        /// <param name="value">Valor da transição</param>
        /// <returns></returns>
        public List<State<T, E>> ReachableStatesBy(E value)
        {
            List<State<T, E>> reachableStates = new List<State<T, E>>();

            return ReachableStatesBy(value, value, reachableStates);
        }

        /// <summary>
        /// Retorna a lista de estados com transição no parâmetro value alcançáveis direta e indiretamente pelo estado
        /// </summary>
        /// <param name="value">Valor da transição</param>
        /// <param name="nullSymbol">Valor nulo ou vazio do automato</param>
        /// <returns></returns>
        public List<State<T, E>> ReachableStatesBy(E value, E nullSymbol)
        {
            List<State<T, E>> reachableStates = new List<State<T, E>>();

            return ReachableStatesBy(value, nullSymbol, reachableStates);
        }

        /// <summary>
        /// Retorna a lista de estados com transição no parâmetro value alcançáveis direta e indiretamente pelo estado
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private List<State<T, E>> ReachableStatesBy(E value, E nullSymbol, List<State<T, E>> reachableStates)
        {
            foreach (StateTransiction<T, E> transiction in Transictions)
            {
                if (transiction.Transiction.Equals(value))
                {
                    reachableStates.Add(transiction.NextState);
                }
                else
                {
                    if (transiction.Transiction.Equals(nullSymbol))
                    {
                        State<T, E> nextState = transiction.NextState;

                        //Para evitar recursão
                        if (!StateValue.Equals(nextState.StateValue))
                        {
                            List<State<T, E>> auxList = nextState.ReachableStatesBy(value, nullSymbol);
                            if (auxList != null)
                            {
                                reachableStates.AddRange(auxList);
                            }
                        }
                    }
                }
            }
            
            if (reachableStates.Count == 0)
            {
                return null;
            }
            else
            {
                return reachableStates;
            }
        }

        /// <summary>
        /// Retorna o estado que é alcançado pelo estado atual a partir do valor passado
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public State<T,E> TransictionIn(E value)
        {
            foreach (StateTransiction<T, E> transiction in Transictions)
            {
                if (transiction.Transiction.Equals(value))
                {
                    return transiction.NextState;
                }
            }

            return null;
        }

        public override string ToString()
        {
            return "State:[ " + StateValue.ToString() + " , " + IsFinal.ToString() + " ]";
        }
        
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            State<T, E> other = (State<T, E>)obj;

            return other.StateValue.Equals(StateValue) && other.IsFinal == IsFinal;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Retorna uma cópia profunda do estado
        /// </summary>
        /// <returns></returns>
        public State<T, E> Clone()
        {
            State<T, E> clonedState = new State<T, E>(this);

            return clonedState;
        }
    }

}
