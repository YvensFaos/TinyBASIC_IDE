using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TinyBASICAnalizer.ADS.AutomataStructure;

namespace TinyBASICAnalizer.ADS
{
    /// <summary>
    /// Classe genérica representando um automato
    /// </summary>
    /// <typeparam name="T">Valor contido em cada estado.</typeparam>
    /// <typeparam name="E">Valor esperado para as transições de estados.</typeparam>
    public class Automata <T, E>
    {
        private State<T, E> _initialState;
        /// <summary>
        /// Estado inicial do automato
        /// </summary>
        public State<T, E> InitialState
        {
            get { return _initialState; }
            set { _initialState = value; }
        }

        private Dictionary<T, State<T, E>> _stateHash;
        /// <summary>
        /// Hash para manipulação de todos os estados
        /// </summary>
        protected Dictionary<T, State<T, E>> StateHash
        {
            get 
            {
                if (_stateHash == null)
                {
                    _stateHash = new Dictionary<T, State<T, E>>();
                }
                return _stateHash; 
            }
            set { _stateHash = value; }
        }

        private List<State<T, E>> _stateList;
        /// <summary>
        /// Lista dos estados do automato
        /// </summary>
        public List<State<T, E>> StateList
        {
            get 
            {
                if (_stateList == null)
                {
                    _stateList = new List<State<T, E>>();
                }
                return _stateList; 
            }
            set { _stateList = value; }
        }

        private E _nullSymbol;
        /// <summary>
        /// Representa o valor nulo/vazio do automato
        /// </summary>
        public E NullSymbol
        {
            get { return _nullSymbol; }
            set { _nullSymbol = value; }
        }

        private List<E> _transictionList;
        /// <summary>
        /// Lista das diferentes transições de estado do automato
        /// </summary>
        public List<E> TransictionList
        {
            get { return _transictionList; }
            set { _transictionList = value; }
        }

        /// <summary>
        /// Construtor default do automato
        /// </summary>
        public Automata()
        { }

        /// <summary>
        /// Construtor para um automata recebendo o estado inicial e a lista de todos os estados já com suas transições.
        /// </summary>
        /// <param name="initialState">Estado inicial do automato</param>
        /// <param name="stateList">Lista dos estados com suas transições</param>
        /// <param name="nullSymbol">Representa o valor nulo ou vazio do automato</param>
        public Automata(State<T, E> initialState, List<State<T, E>> stateList, List<E> transictionList, E nullSymbol)
        {
            _initialState = initialState;
            _stateList = stateList;

            foreach (State<T, E> state in stateList)
            {
                StateHash.Add(state.StateValue, state);
            }

            _transictionList = transictionList;
            _nullSymbol = nullSymbol;
        }

        /// <summary>
        /// Converte um afn para afd
        /// </summary>
        /// <param name="afn">Automato finito não determinístico</param>
        /// <param name="transictionList">Lista dos possíveis valores para transição de estado</param>
        /// <param name="nullSymbol">Símbolo representando do nulo/vazio dos automatos</param>
        /// <returns>Retorna um automato finito determínisco usando a construção de conjuntos do automato passado</returns>
        public static Automata<T, E> AFNtoAFD(Automata<T, E> afn, List<E> transictionList, E nullSymbol)
        {
            State<T, E> initialState = afn.InitialState;
            Dictionary<State<T, E>, PowerSet<T, E>> hash = new Dictionary<State<T, E>, PowerSet<T, E>>();

            //Retorna com o hash completo com todos os fechos
            PowerSet<T, E> initialPowerSet = GeneratePowerSet(initialState, transictionList, nullSymbol, hash);

            //Carrega o valor do estado do primeiro fecho transitivo
            initialState = initialPowerSet.PowerSetToState();

            //Carrega a lista com os estados do fecho transitivo criado
            List<State<T, E>> stateList = new List<State<T, E>>();
            foreach (State<T, E> state in hash.Keys)
            {
                PowerSet<T, E> powerSet = hash[state];
                stateList.Add(powerSet.PowerSetToState());
            }

            return new Automata<T, E>(initialState, stateList, transictionList, nullSymbol);
        }

        /// <summary>
        /// Gera o fecho transitivo de um determinado estado. Realiza as inserções no hash.
        /// </summary>
        /// <param name="state">Estado do fecho transitivo</param>
        /// <param name="transictionList">Lista dos possíveis valores para a transição de estados</param>
        /// <param name="nullSymbol">Símbolo representando do nulo/vazio dos automatos</param>
        /// <param name="hash">Hash dos fechos já criados</param>
        /// <returns>Fecho transitivo do estado</returns>
        private static PowerSet<T, E> GeneratePowerSet(State<T, E> state, List<E> transictionList, E nullSymbol, Dictionary<State<T, E>, PowerSet<T, E>> hash)
        {
            PowerSet<T, E> powerSet = new PowerSet<T, E>(state);
            //List<State<T, E>> reachableState = state.ReachableStatesBy(nullSymbol);
            hash.Add(state, powerSet);

            //Itera sobre a lista de terminais
            foreach (E value in transictionList)
            {
                //Verifica se não é o valor nulo
                if (!value.Equals(nullSymbol))
                {
                    //Busca o estado que é alcançável a partir do terminal value
                    List<State<T, E>> reachable = state.ReachableStatesBy(value, nullSymbol);
                    if (reachable != null)
                    {
                        if (reachable.Count > 1)
                        {
                            //Adiciona transições de estados com recursão
                            if (!reachable[0].StateValue.Equals(reachable[1].StateValue))
                            {
                                powerSet.AddTransictionToPowerSet(value, reachable[0]);
                                powerSet.AddTransictionToPowerSet(value, reachable[1]);
                            }
                        }
                        else
                        {
                            //Adiciona transição no valor para o estaddo
                            powerSet.AddTransictionToPowerSet(value, reachable[0]);
                        }
                    }
                }
            }

            //Itera sobre os estados que não contém fecho e os gera
            foreach (StateTransiction<T, E> transiction in powerSet.Transictions)
            {
                State<T, E> nextState = transiction.NextState;
                if (!hash.ContainsKey(nextState))
                {
                    GeneratePowerSet(nextState, transictionList, nullSymbol, hash);
                }
            }

            return powerSet;
        }

        /// <summary>
        /// Busca o estado passado no hash de estados
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public State<T, E> GetState(T value)
        {
            if (StateHash.ContainsKey(value))
            {
                return StateHash[value];
            }
            return null;
        }
    }
}
