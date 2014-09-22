using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyBASICAnalizer.Model
{
    public class NonTerminal : Symbol
    {
        private string _rule;
        /// <summary>
        /// Regra de formação do não terminal
        /// </summary>
        public string Rule
        {
            get { return _rule; }
            set { _rule = value; }
        }

        private List<First> _firsts;
        /// <summary>
        /// Lista de firsts do não terminal
        /// </summary>
        public List<First> Firsts
        {
            get 
            {
                if (_firsts == null)
                {
                    _firsts = new List<First>();
                }
                return _firsts; 
            }
            set { _firsts = value; }
        }

        private Dictionary<string, bool> _firstList;
        /// <summary>
        /// Hash de terminais do first para agilizar a busca por firsts repetidos
        /// </summary>
        private Dictionary<string, bool> FirstList
        {
            get 
            {
                if (_firstList == null)
                {
                    _firstList = new Dictionary<string, bool>();
                }
                return _firstList; 
            }
            set { _firstList = value; }
        }

        private List<Terminal> _follows;
        /// <summary>
        /// Lista de follows do não terminal
        /// </summary>
        public List<Terminal> Follows
        {
            get 
            {
                if (_follows == null)
                {
                    _follows = new List<Terminal>();
                }
                return _follows; 
            }
            set { _follows = value; }
        }

        private Dictionary<string, bool> _followList;
        /// <summary>
        /// Hash de terminais do follows para agilizar a busca por follows repetidos
        /// </summary>
        private Dictionary<string, bool> FollowList
        {
            get
            {
                if (_followList == null)
                {
                    _followList = new Dictionary<string, bool>();
                }
                return _followList;
            }
            set { _followList = value; }
        }

        public NonTerminal(string Value)
            : this(Value, "")
        { }

        public NonTerminal(string Value, string Rule)
            : base(Value)
        {
            this.Rule = Rule;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        /// <summary>
        /// Método para verificar se conjunto de regras do não terminal contém o símbolo vazio
        /// </summary>
        /// <returns></returns>
        public bool ContainsEmpty()
        {
            if (Rule == null || Rule.Length <= 0)
            {
                return false;
            }
            else
            {
                return Rule.Contains('#');
            }
        }
        #region Métodos do First
        /// <summary>
        /// Gera a lista de firsts do não terminal
        /// </summary>
        /// <returns></returns>
        public List<First> First(bool initialSymbol)
        {
            if (Rule == null || Rule.Length == 0)
            {
                return null;
            }
           
            //Separa as regras pelo |            
            string[] rules = Rule.Split('|');
            foreach (string rule in rules)
            {                
                if (rule.Contains(' '))
                {
                    string[] symbols = rule.Split(' ');

                    int i = 0;
                    string symbol = symbols[i];
                    bool loopBreaker = false;

                    while (!loopBreaker)
                    {
                        //Se contiver e o símbolo, logo ele é um não terminal
                        if (NonTerminalHash.ContainsKey(symbol))
                        {
                            NonTerminal anotherNonTerminal = NonTerminalHash[symbol];
                            if (anotherNonTerminal.Value.Equals(Value))
                            {
                                return null;
                            }

                            List<First> anotherNonTerminalFirsts = anotherNonTerminal.First();

                            foreach (First firstElement in anotherNonTerminalFirsts)
                            {
                                if (AddTerminalToFirst(firstElement.FirstValue, rule) == false)
                                {
                                    loopBreaker = true;
                                }
                               
                            }

                            if (anotherNonTerminal.FirstContainsEmpty())
                            {
                                if (i < symbols.Length - 1)
                                {
                                    i++;
                                    symbol = symbols[i];
                                }
                                else
                                {
                                    AddTerminalToFirst(Terminal.Empty, rule);
                                    loopBreaker = true;
                                }
                            }
                            else
                            {
                                loopBreaker = true;
                            }
                        }
                        //Se não contiver, verificamos a lista de terminais
                        else if (TerminalHash.ContainsKey(symbol))
                        {
                            Terminal terminal = TerminalHash[symbol];

                            AddTerminalToFirst(terminal, rule);

                            loopBreaker = true;
                        }
                        else
                        //Se não contiver, é uma situação de erro
                        {
                            return null;
                        }
                    }
                }
                else
                {
                    //Apenas para facilitar a leitura
                    string symbol = rule;

                    //Se contiver e o símbolo, logo ele é um não terminal
                    if (NonTerminalHash.ContainsKey(symbol))
                    {
                        NonTerminal anotherNonTerminal = NonTerminalHash[symbol];
                        List<First> anotherNonTerminalFirsts = anotherNonTerminal.First();

                        if ((anotherNonTerminalFirsts!=null)&&(anotherNonTerminalFirsts.Count > 0))
                        {
                            foreach (First firstElement in anotherNonTerminalFirsts)
                            {
                                AddTerminalToFirst(firstElement.FirstValue, rule);
                            }
                        }
                    }
                        //Se não contiver, verificamos a lista de terminais
                    else if (TerminalHash.ContainsKey(symbol))
                    {
                        Terminal terminal = TerminalHash[symbol];

                        AddTerminalToFirst(terminal, rule);
                    }
                    else
                        //Se não contiver, é uma situação de erro
                    {
                        return null;
                    }
                }
            }

            return Firsts;
        }

        /// <summary>
        /// Gera a lista de firsts do não terminal
        /// </summary>
        /// <returns></returns>
        public List<First> First()
        {
            return First(false);
        }

        /// <summary>
        /// Método para fazer a verificação e inserção do terminal
        /// </summary>
        /// <param name="terminal"></param>
        /// <returns></returns>
        private bool AddTerminalToFirst(Terminal terminal, string rule)
        {
            string value = terminal.Value;
            if (!FirstList.ContainsKey(value))
            {
                FirstList.Add(value, true);
                First element = new First(terminal, rule);
                Firsts.Add(element);


                return true;
            }
            return false;
        }

        /// <summary>
        /// Retorna verdadeiro caso a lista do first contiver o terminal empty
        /// </summary>
        /// <returns></returns>
        public bool FirstContainsEmpty()
        {
            return FirstContains(Terminal.Empty);
        }

        /// <summary>
        /// Retorna verdadeiro caso a lista do first contiver o terminal
        /// </summary>
        /// <param name="terminal"></param>
        /// <returns></returns>
        public bool FirstContains(Terminal terminal)
        {
            foreach (First firstElement in Firsts)
            {
                if (firstElement.FirstValue.Equals(terminal))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Retorna verdadeiro caso a lista do first contiver o valor passado
        /// </summary>
        /// <param name="terminal"></param>
        /// <returns></returns>
        public bool FirstContains(string value)
        {
            return FirstContains(TerminalHash[value]);
        }
#endregion

        #region Métodos do Follow
        /// <summary>
        /// Adiciona a lista de follow de um não terminal a lista de follows de outro
        /// </summary>
        /// <param name="anotherNonTerminal"></param>
        /// <returns></returns>
        public bool AddFollowsToFollows(NonTerminal anotherNonTerminal)
        {
            List<Terminal> anotherFollows = anotherNonTerminal.Follows;

            bool add = false;
            foreach (Terminal terminal in anotherFollows)
            {
                //O retorno da função é verdadeiro caso o terminal seja novo e seja adicionado
                add |= AddTerminalToFollow(terminal);
            }

            return add;
        }

        /// <summary>
        /// Adiciona a lista de firsts de um não terminal a lista de follows de outro. Ignora o vazio
        /// </summary>
        /// <param name="anotherNonTerminal"></param>
        /// <returns></returns>
        public bool AddFirstsToFollows(NonTerminal anotherNonTerminal)
        {
            List<First> anotherFollows = anotherNonTerminal.Firsts;

            bool add = false;
            foreach (First firstElement in anotherFollows)
            {
                Terminal terminal = firstElement.FirstValue;
                if(!terminal.Equals(Terminal.Empty))
                {
                    //O retorno da função é verdadeiro caso o terminal seja novo e seja adicionado
                    add |= AddTerminalToFollow(terminal);
                }
            }

            return add;
        }

        /// <summary>
        /// Retorna verdadeiro caso a lista do follow contiver o terminal
        /// </summary>
        /// <param name="terminal"></param>
        /// <returns></returns>
        public bool FollowContains(Terminal terminal)
        {
            return FollowList.ContainsKey(terminal.Value);
        }

        /// <summary>
        /// Retorna verdadeiro caso a lista do follow contiver o valor passado
        /// </summary>
        /// <param name="terminal"></param>
        /// <returns></returns>
        public bool FollowContains(string value)
        {
            return FollowContains(TerminalHash[value]);
        }

        /// <summary>
        /// Retorna verdadeiro caso a lista do follow contiver o valor inicial representado por $
        /// </summary>
        /// <returns></returns>
        public bool FollowContainsInitial()
        {
            return FollowContains(Terminal.Initial);
        }

        /// <summary>
        /// Método para fazer a verificação e inserção do terminal
        /// </summary>
        /// <param name="terminal"></param>
        /// <returns></returns>
        public bool AddTerminalToFollow(Terminal terminal)
        {
            string value = terminal.Value;
            if (!FollowList.ContainsKey(value))
            {
                FollowList.Add(value, true);
                Follows.Add(terminal);

                return true;
            }
            return false;
        }
        #endregion

        /// <summary>
        /// Método para retornar a lista de símbolos da regra que gera o token. A lista de regras usadas é a lista de firsts.
        /// Se tiver vazio na lista de firsts, são verificados também os follows.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public List<Symbol> GetRuleForToken(Token token)
        {
            bool useFollow = ContainsEmpty();
            First firstElement = null;

            firstElement = getFirstFromTokenType(token);
            if (firstElement != null)
            {
                string rule = firstElement.Rule;
                List<Symbol> list = new List<Symbol>();

                if (rule.Contains(' '))
                {
                    string[] parameters = rule.Split(' ');

                    foreach (string element in parameters)
                    {
                        Symbol symbol = Symbol.GetSymbol(element);
                        list.Add(symbol);
                    }
                }
                else
                {
                    Symbol symbol = Symbol.GetSymbol(rule);
                    list.Add(symbol);

                }
                return list;
            }

            return null;
        }

        /// <summary>
        /// Retorna o elemento First que contém o tipo do token passado
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private First getFirstFromTokenType(Token token)
        {
            if (token == null)
            {
                return null;
            }

            TokenType type = token.Type;

            foreach (First element in Firsts)
            {
                string value = element.FirstValue.Value;

                switch (type)
                {
                    case TokenType.RESERVED_WORD:
                        {
                            if (value.Equals(token.Value))
                            {
                                return element;
                            }
                            break;
                        }
                    case TokenType.VARIABLE:
                        {
                            if ((value.Equals("var")) && (Symbol.GetSymbol("var") is Terminal))
                            {
                                return element;
                            }
                            else
                            {
                                goto case TokenType.RESERVED_WORD;
                            }                            
                        }
                    case TokenType.MATH_OPERATOR:
                        {
                            goto case TokenType.RESERVED_WORD;
                        }
                    case TokenType.RELATIONAL_OPERATOR:
                        {
                            goto case TokenType.RESERVED_WORD;
                        }
                    case TokenType.VALUE:
                        {
                            if (value.Equals("num"))
                            {
                                return element;
                            }
                            break;
                        }
                    case TokenType.STRING:
                        {
                            if (value.Equals("string"))
                            {
                                return element;
                            }
                            break;
                        }
                    default:
                        {
                            TokenType anotherType = Token.GetType(value);
                            if (anotherType.Equals(token.Type))
                            {
                                return element;
                            }
                            break;
                        }
                }

            }
            return null;
        }
    }
}
