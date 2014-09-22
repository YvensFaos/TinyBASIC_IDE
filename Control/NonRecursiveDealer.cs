using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TinyBASICAnalizer.Model;
using TinyBASICAnalizer.Persistence;

namespace TinyBASICAnalizer.Control
{
    public class NonRecursiveDealer
    {
        private static string _grammarFile
        {
            get { return "Model/Grammar/Grammar" + Symbol.GrammarIdentifier + ".txt"; }
        }

        private Dictionary<string, NonTerminal> _nonTerminalHash;
        /// <summary>
        /// Hash de não terminais
        /// </summary>
        public Dictionary<string, NonTerminal> NonTerminalHash
        {
            get { return _nonTerminalHash; }
            private set { _nonTerminalHash = value; }
        }

        private Symbol _initialSymbol;
        /// <summary>
        /// Símbolo inicial da gramática
        /// </summary>
        public Symbol InitialSymbol
        {
            get { return _initialSymbol; }
            set { _initialSymbol = value; }
        }

        /// <summary>
        /// Lista de não terminais
        /// </summary>
        public List<NonTerminal> NonTerminals
        {
            get
            {
                if (_nonTerminalHash == null || _nonTerminalHash.Count == 0)
                {
                    return null;
                }
                else
                {
                    List<NonTerminal> list = new List<NonTerminal>();

                    foreach (string key in _nonTerminalHash.Keys)
                    {
                        NonTerminal nonTerminal = _nonTerminalHash[key];
                        list.Add(nonTerminal);
                    }

                    return list;
                }
            }
        }

        /// <summary>
        /// Construtor do NonRecursive realiza a leitura da gramática e a geração dos firsts e follows
        /// </summary>
        public NonRecursiveDealer()
            : this(null)
        { }

        /// <summary>
        /// Construtor recebendo uma gramática
        /// </summary>
        /// <param name="grammar"></param>
        public NonRecursiveDealer(Grammar grammar)
        {
            #region Leitura e interpretação da gramática
            string[] grammarFile = null;
            if (grammar == null)
            {
                grammarFile = IOUtils.ReadTxtFile(_grammarFile);
                _nonTerminalHash = Symbol.NonTerminalHash;
                _initialSymbol = Symbol.InitialSymbol;
            }
            else
            {
                _nonTerminalHash = grammar.NonTerminalHash;
                grammarFile = grammar.GrammarFile;
                _initialSymbol = grammar.InitialSymbol;
            }
            
            //Regras da gramática
            //Não terminal:regras
            //Regras separadas sem espaço por |
            //Vazio é igual a #
            //Não existe repetição que não seja recursão
            //O símbolo @ indica qualquer cadeia de caracteres alfanuméricos e é usado para a string

            foreach (string line in grammarFile)
            {
                string[] parameters = line.Split(new char[] { ':' }, 2);

                NonTerminal nonTerminal = _nonTerminalHash[parameters[0]];
                nonTerminal.Rule = parameters[1];
            }
            #endregion

            GenerateFirst();
            GenerateFollow();
        }

        private void GenerateFirst()
        {
            NonTerminal initialSymbol = (NonTerminal)InitialSymbol;
            initialSymbol.First(true);

            //Carrega o first dos não terminais que não foram alcançados na iteração anterior
            foreach (string key in _nonTerminalHash.Keys)
            {
                NonTerminal nonTerminal = _nonTerminalHash[key];
                if (nonTerminal.Firsts == null || nonTerminal.Firsts.Count == 0)
                {                    
                    nonTerminal.First();
                }
            }
        }

        private void GenerateFollow()
        {
            NonTerminal initialSymbol = (NonTerminal)InitialSymbol;
            initialSymbol.AddTerminalToFollow(Terminal.Initial);

            bool change = true;

            while (change)
            {
                change = false;
                foreach (string key in _nonTerminalHash.Keys)
                {
                    NonTerminal nonTerminal = _nonTerminalHash[key];

                    string rule = nonTerminal.Rule;
                    if (rule == null || rule.Length == 0)
                    {
                        return;
                    }

                    //Separa as regras pelo |
                    string[] rules = rule.Split('|');

                    foreach (string simpleRule in rules)
                    {
                        if (rule.Contains(' '))
                        {
                            string[] symbols = simpleRule.Split(' ');
                            bool containsEmpty;
                            bool isTerminal;

                            //For avançando, lidando com a regra
                            //A -> X1 X2 X3 ... XN
                            for (int i = 0; i < symbols.Length; i++)
                            {
                                Symbol ruleSymbol = Symbol.GetSymbol(symbols[i]);
                                if (ruleSymbol is NonTerminal)
                                {
                                    //Variável para representar o primeiro não terminal
                                    NonTerminal nonTerm1 = (NonTerminal)ruleSymbol;

                                    containsEmpty = true;
                                    isTerminal = false;
                                    //Iteração para os não terminais seguintes a ele
                                    //Esse for só acontece caso cada não terminal seguinte tiver vazio
                                    //Caso um não terminal seguinte não tenha vazio ou seja um terminal, o for pára
                                    for (int j = i + 1; j < symbols.Length && containsEmpty && !isTerminal; j++)
                                    {
                                        ruleSymbol = Symbol.GetSymbol(symbols[j]);
                                        if (ruleSymbol is NonTerminal)
                                        {
                                            //Variável para representar o segundo não terminal
                                            NonTerminal nonTerm2 = (NonTerminal)ruleSymbol;

                                            containsEmpty = nonTerm2.ContainsEmpty();

                                            change |= nonTerm1.AddFirstsToFollows(nonTerm2);
                                        }
                                        else
                                        {
                                            isTerminal = true;
                                        }
                                    }
                                }
                                else if (ruleSymbol!=null)
                                {                                    
                                    if (i != 0)
                                    {
                                        //Adiciona o terminal no follow do não-terminal imediatamente anterior                                         
                                        Symbol cacheSymbol = Symbol.GetSymbol(symbols[i-1]);
                                        if (cacheSymbol is NonTerminal)
                                        {
                                            NonTerminal nonTerm1 = (NonTerminal)cacheSymbol;
                                            nonTerm1.AddTerminalToFollow((Terminal)ruleSymbol);
                                        }                                    

                                    }
                                    
                                }
                            }

                            containsEmpty = true;
                            isTerminal = false;
                            
                            for (int i = symbols.Length - 1; i > - 1 && containsEmpty && !isTerminal; i--)
                            {
                                Symbol ruleSymbol = Symbol.GetSymbol(symbols[i]);
                                if (ruleSymbol is NonTerminal)
                                {
                                    //Variável para representar o primeiro não terminal
                                    NonTerminal nonTerm1 = (NonTerminal)ruleSymbol;

                                    containsEmpty = nonTerm1.ContainsEmpty();
                                    

                                    nonTerm1.AddFollowsToFollows(nonTerminal);
                                }
                                else if (ruleSymbol!=null)
                                {
                                    //Adiciona o terminal no follow do não-terminal imediatamente anterior
                                    if (i != 0)
                                    {
                                        Symbol cacheSymbol = Symbol.GetSymbol(symbols[i - 1]);
                                        if (cacheSymbol is NonTerminal)
                                        {
                                            NonTerminal nonTerm1 = (NonTerminal)cacheSymbol;
                                            nonTerm1.AddTerminalToFollow((Terminal)ruleSymbol);
                                        }
                                    }
                                    
                                    isTerminal = true;
                                }
                            }
                        }
                        else
                        {
                            //Para os casos
                            //A -> a             ou     A -> B
                            //No caso do terminal, nada é adicionado.
                            //No caso do não terminal, os follows de A são passados para os follows de B

                            Symbol ruleSymbol = Symbol.GetSymbol(simpleRule);
                            if (ruleSymbol is NonTerminal)
                            {
                                NonTerminal anotherNonTerminal = (NonTerminal)ruleSymbol;

                                //O retorno da função é verdadeiro caso algum novo terminal seja adicionado
                                change |= anotherNonTerminal.AddFollowsToFollows(nonTerminal);
                            }
                        }
                    }
                }
            }
        }
    }
}
