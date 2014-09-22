using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyBASICAnalizer.ADS.AutomataStructure
{
    /// <summary>
    /// Classe que representa o fecho transitivo
    /// </summary>
    /// <typeparam name="T">Valor contido em cada estado.</typeparam>
    /// <typeparam name="E">Valor esperado para as transições de estados.</typeparam>
    public class PowerSet<T,E>
    {
        private string _identifier;
        /// <summary>
        /// Identificador do fecho transitivo
        /// </summary>
        public string Identifier
        {
            get { return _identifier; }
            set { _identifier = value; }
        }

        private bool _isFinal;
        /// <summary>
        /// Indica se o fecho transitivo é final
        /// </summary>
        public bool IsFinal
        {
            get { return _isFinal; }
            set { _isFinal = value; }
        }

        private State<T, E> _powerSetState;
        /// <summary>
        /// Estado do fecho transitivo
        /// </summary>
        public State<T, E> PowerSetState
        {
            get { return _powerSetState; }
            set { _powerSetState = value; }
        }

        private State<T, E> _originalState;
        /// <summary>
        /// Estado original do fecho transitivo
        /// </summary>
        public State<T, E> OriginalState
        {
            get { return _originalState; }
            set { _originalState = value; }
        }

        private List<StateTransiction<T, E>> _transictions;
        /// <summary>
        /// Lista das transições do fecho transitivo
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
            set { _transictions = value; }
        }

        /// <summary>
        /// Variáveis para controle e identificação dos fechos
        /// </summary>
        private static char ident = (char)65; //65 = 'A'
        private static int  identAux = 0;

        /// <summary>
        /// Construtor seta o identificador do estado para ser um valor char de A-Za-z [identAux]
        /// </summary>
        public PowerSet()
            : this(null, "")
        { }

        /// <summary>
        /// Construtor seta o identificador para o fecho transitivo
        /// </summary>
        /// <param name="identifier">Identificador do fecho transitivo</param>
        public PowerSet(string identifier) 
            : this(null, identifier)
        { }

        /// <summary>
        /// Construtor seta o identificador do estado para ser um valor char de A-Za-z [identAux] e o estado.
        /// </summary>
        /// <param name="originalState">Estado do fecho transitivo</param>
        public PowerSet(State<T, E> originalState)
            : this(originalState, "")
        { }

        /// <summary>
        /// Construtor seta o identificador para o fecho transitivo e o estado
        /// </summary>
        /// <param name="identifier">Identificador do fecho transitivo</param>
        /// <param name="originalState">Estado do fecho transitivo</param>
        public PowerSet(State<T, E> originalState, string identifier)
        {
            if (identifier == null || identifier.Equals(""))
            {
                _identifier = char.ToString(ident++) + "" + ((ident != 0) ? "" : identAux.ToString());
            }
            IncrementIdentifier();

            _powerSetState = new State<T,E>(originalState);
            _originalState = originalState;
        }

        /// <summary>
        /// Incrementa e gerenciar o identificador
        /// </summary>
        private static void IncrementIdentifier()
        {
            if (ident > 90)
            {
                ident = (char)97; //97 = 'a'
            }
            else if (ident > 122)
            {
                ident = (char)65; //Reinicia a contagem
                identAux++; //Acrescenta 1 no identificador auxiliar
            }
        }

        /// <summary>
        /// Retorna verdadeiro caso o fecho possua pelo menos 1 transição sobre um valor T
        /// </summary>
        /// <returns></returns>
        public bool HasTransictions()
        {
            return Transictions.Count > 0;
        }

        /// <summary>
        /// Adiciona uma transição ao fecho
        /// </summary>
        /// <param name="transiction"></param>
        /// <param name="nextState"></param>
        public void AddTransictionToPowerSet(E transiction, State<T, E> nextState)
        {
            Transictions.Add(new StateTransiction<T,E>(transiction, nextState));
        }

        /// <summary>
        /// Verifica se há transição para o valor passado
        /// </summary>
        /// <param name="transiction"></param>
        /// <returns></returns>
        public bool HasTransictionIn(E transiction)
        {
            foreach (StateTransiction<T, E> stateTransiction in Transictions)
            {
                if (stateTransiction.Transiction.Equals(transiction))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Retorna o _identifier do fecho e o seu estado
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "[" + _identifier + " : " + PowerSetState.ToString() + "]";
        }

        /// <summary>
        /// Retorna o estado correspondente ao fecho.
        /// </summary>
        /// <returns>O estado correspondente.</returns>
        public State<T, E> PowerSetToState()
        {
            State<T, E> newState = new State<T, E>(OriginalState);
            
            foreach (StateTransiction<T, E> transiction in Transictions)
            {
                newState.AddTransiction(transiction);
            }

            return newState;
        }
    }
}