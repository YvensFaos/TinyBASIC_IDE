using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TinyBASICAnalizer.ADS.GrammarAutomata;
using TinyBASICAnalizer.Model;
using TinyBASICAnalizer.ADS;

namespace TinyBASICAnalizer.Control
{
    public enum SyntaxAnalizerType
    {
        RECURSIVE_DESCENDENT, NON_RECURSIVE_DESCENDENT, ASCENDENT
    }

    public class SyntaxAnalizer
    {
        #region Strings de Feedback
        public static string LEXIC_ERROR = "Erro na análise léxica!";
        public static string LEXIC_INVALID = "Erro na análise léxica! Token inválido encontrado, valor: ";
        public static string PARENTHESIS_ERROR = "Falta um ')'";
        public static string SEPARATOR_INVALID = "Separador inválido, valor: ";
        public static string OPERATOR_INVALID = "Operador inválido, valor: ";
        public static string FUNCTION_INVALID = "Função inválida chamada, valor: ";
        public static string PARAMETER_INVALID = "Parâmetro inválido chamado, valor: ";
        public static string INSTRUCTION_INVALID = "Instrução inválida chamada, valor: ";
        public static string INCOMPLETE_TENSE_MISS = "Expressão incompleta, está faltando um termo. Valor esperado: ";
        public static string INCOMPLETE_TENSE_FOUND = "Valor encontrado: ";
        #endregion

        private NonRecursiveDealer _dealer;
        /// <summary>
        /// Classe responsável por lidar com a análise sintática não recursiva
        /// </summary>
        public NonRecursiveDealer Dealer
        {
            get { return _dealer; }
            set { _dealer = value; }
        }

        private SyntaxAnalizerType _type;
        public SyntaxAnalizerType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private bool negativeTerm;
        private int line;
        private LexicAnalizer _lexic;
        /// <summary>
        /// Analisador léxico
        /// </summary>
        public LexicAnalizer Lexic
        {
            get { return _lexic; }
            set { _lexic = value; }
        }

        /// <summary>
        /// Por default, a análise sintática executada é a Descendente Recursiva
        /// </summary>
        /// <param name="Lexic"></param>
        public SyntaxAnalizer(LexicAnalizer Lexic)
            : this(Lexic, SyntaxAnalizerType.RECURSIVE_DESCENDENT)
        { }

        public SyntaxAnalizer(LexicAnalizer Lexic, SyntaxAnalizerType type)
        {
            this.Lexic = Lexic;
            this.Type = type;
        }

        /// <summary>
        /// Realiza a análise sintática dos tokens provindos da análise léxica. A variável feedback é um ponteiro para uma string 
        /// que conterá o feedback final da análise léxica
        /// </summary>
        /// <param name="feedback"></param>
        /// <returns></returns>
        public bool Analize(ref string feedback)
        {
            //Realiza a análise léxica
            line = 1;
            #region Análise Sintática seguida da Análise Léxica
            if (Lexic.Analize())
            {
                //Se não for encontrado nenhum token inválido, então a análise sintática acontece
                if (!Lexic.HasInvalidToken(ref feedback))
                {
                    //É verificado qual tipo de análise sintática está sendo executada
                    switch (Type)
                    {
                        case SyntaxAnalizerType.RECURSIVE_DESCENDENT:
                            {
                                return Code(ref feedback);
                            }
                        case SyntaxAnalizerType.NON_RECURSIVE_DESCENDENT:
                            {
                                return NonRecursiveAnalizer(ref feedback);
                            }
                        case SyntaxAnalizerType.ASCENDENT:
                            {
                                return AscendentAnalizer(ref feedback);
                            }
                    }
                }
                else
                {
                    feedback = LEXIC_INVALID + feedback;
                }
            }
            else
            {
                feedback = LEXIC_INVALID + feedback;
            }
            #endregion
            return false;
        }

        #region Analisador Sintático Descendente Recursivo
        /// <summary>
        /// Método para o não-terminal código, dado por: "codigo	  :=	linha {linha}"
        /// </summary>
        /// <param name="feedback"></param>
        /// <returns></returns>
        private bool Code(ref string feedback)
        {
            bool validation = true;

            validation &= Line(ref feedback);

            while (Lexic.IsValidPosition() && Lexic.NextToken() != null)
            {

                Lexic.Position--;
                validation &= Line(ref feedback);
            }

            return validation;
        }

        /// <summary>
        /// Método para o não-terminal linha, dado por: "linha     :=	num instrução CR | instrução CR"
        /// </summary>
        /// <param name="feedback"></param>
        /// <returns></returns>
        private bool Line(ref string feedback)
        {
            bool validation = true;

            Token token = Lexic.NextToken();

            if (token.Type == TokenType.LINE_BREAK)
            {
                line++;
                return validation;
            }

            if (token.Type != TokenType.VALUE)
            {
                //Retrocede uma posição porque o valor não era um número
                Lexic.Position--;
            }

            validation &= Instruction(ref feedback);

            return validation;
        }

        /// <summary>
        /// Método para o não-terminal instrução, dado por: 
        ///"instrução :=	PRINT listaprint |
        /// 				INPUT var {,var} |
        ///	    			LET var = exp	 |
        /// 	    		GOTO exp		 |
        ///     	    	GOSUB exp		 |
        ///         	    RETURN			 |
        /// 				IF exp oprel exp THEN exp	|
        ///	    			REM string		 |
        ///		    		CLEAR			 |
        ///			    	RUN				 |
        ///				    RUN exp {,exp}   |
        /// 				LIST			 |
        ///	    			LIST exp {,exp}  "
        /// </summary>
        /// <param name="feedback"></param>
        /// <returns></returns>
        private bool Instruction(ref string feedback)
        {
            bool validation = true;

            Token token = Lexic.NextToken();

            if (token == null)
            {
                feedback += INSTRUCTION_INVALID + " , linha: " + line + "\r\n";
                return false;
            }

            if (token.Type != TokenType.RESERVED_WORD)
            {
                feedback += INSTRUCTION_INVALID + token.Value + " , linha: " + line + "\r\n";
                return false;
            }

            ReservedWord reservedWord = ReservedWordUtils.GetReservedEnum(token.Value);

            switch (reservedWord)
            {
                case ReservedWord.PRINT:
                    {
                        validation &= PrintList(ref feedback);
                        break;
                    }
                case ReservedWord.INPUT:
                    {
                        token = Lexic.NextToken();

                        if (token == null)
                        {
                            feedback += INCOMPLETE_TENSE_MISS + "variável, linha: " + line + "\r\n";
                            return false;
                        }

                        if (token.Type != TokenType.VARIABLE)
                        {
                            feedback += PARAMETER_INVALID + token.Value + " , linha: " + line + "\r\n";
                            return false;
                        }

                        while (true)
                        {
                            token = Lexic.NextToken();

                            if (token == null)
                            {
                                feedback += INCOMPLETE_TENSE_MISS + "variável, linha: " + line + "\r\n";
                                break;
                            }

                            if (token.Type == TokenType.SEPARATOR)
                            {
                                if (token.Value.Equals(","))
                                {
                                    token = Lexic.NextToken();

                                    if (token == null)
                                    {
                                        feedback += INCOMPLETE_TENSE_MISS + "variável, linha: " + line + "\r\n";
                                        return false;
                                    }

                                    if (token.Type != TokenType.VARIABLE)
                                    {
                                        feedback += PARAMETER_INVALID + token.Value + " , linha: " + line + "\r\n";
                                        return false;
                                    }
                                }
                                else
                                {
                                    feedback += SEPARATOR_INVALID + token.Value + " , linha: " + line + "\r\n";
                                    return false;
                                }
                            }
                            else
                            {
                                Lexic.Position--;
                                break;
                            }
                        }

                        break;
                    }
                case ReservedWord.LET:
                    {
                        token = Lexic.NextToken();

                        if (token == null)
                        {
                            feedback += INCOMPLETE_TENSE_MISS + "variável, linha: " + line + "\r\n";
                            return false;
                        }

                        if (token.Type == TokenType.VARIABLE)
                        {
                            token = Lexic.NextToken();

                            if (token == null)
                            {
                                feedback += INCOMPLETE_TENSE_MISS + "=";
                                return false;
                            }

                            if (token.Type == TokenType.RELATIONAL_OPERATOR && token.Value.Equals("="))
                            {
                                validation &= Expression(ref feedback);
                            }
                            else
                            {
                                feedback += OPERATOR_INVALID + token.Value + " , linha: " + line + "\r\n"; ;
                                return false;
                            }
                        }
                        else
                        {
                            feedback += PARAMETER_INVALID + token.Value + " , linha: " + line + "\r\n";
                            return false;
                        }
                        break;
                    }
                case ReservedWord.GOTO:
                    {
                        validation &= Expression(ref feedback);
                        break;
                    }
                case ReservedWord.GOSUB:
                    {
                        //Casos análogos
                        goto case ReservedWord.GOTO;
                    }
                case ReservedWord.IF:
                    {
                        validation &= Expression(ref feedback);

                        token = Lexic.NextToken();
                        if (token == null)
                        {
                            feedback += INCOMPLETE_TENSE_MISS + "variável, linha: " + line + "\r\n";
                            return false;
                        }
                        if (token.Type == TokenType.RELATIONAL_OPERATOR)
                        {
                            validation &= Expression(ref feedback);
                            token = Lexic.NextToken();

                            if (token.Type == TokenType.RESERVED_WORD && token.Value.Equals(ReservedWord.THEN.ToString()))
                            {
                                token = Lexic.NextToken();

                                if (token == null)
                                {
                                    feedback += INCOMPLETE_TENSE_MISS + "instrução";
                                    return false;
                                }

                                if (token.Type == TokenType.RESERVED_WORD && !token.Value.Equals(ReservedWord.RND.ToString()))
                                {
                                    //Retorna uma posição
                                    Lexic.Position--;
                                    validation &= Instruction(ref feedback);
                                }
                                else
                                {
                                    Lexic.Position--;
                                    validation &= Expression(ref feedback);
                                }
                            }
                            else
                            {
                                feedback += INCOMPLETE_TENSE_MISS + ReservedWord.THEN.ToString() + INCOMPLETE_TENSE_FOUND + token.Value;
                                return false;
                            }
                        }
                        else
                        {
                            feedback += OPERATOR_INVALID + token.Value;
                            return false;
                        }
                        break;
                    }
                case ReservedWord.REM:
                    {
                        token = Lexic.NextToken();
                        if (token.Type != TokenType.STRING)
                        {
                            feedback += PARAMETER_INVALID + token.Value + " , linha: " + line + "\r\n";
                            return false;
                        }
                        break;
                    }
                case ReservedWord.RUN:
                    {
                        validation &= Expression(ref feedback);

                        //Talvez se a gente deixar passar aqui, deixando o validation sempre true, ele servirá para os casos
                        //onde o RUN e o LIST são chamados sem parâmetro

                        while (true)
                        {
                            token = Lexic.NextToken();

                            if (token == null)
                            {
                                break;
                            }
                            if (token.Type == TokenType.SEPARATOR)
                            {
                                if (token.Value.Equals(","))
                                {
                                    validation &= Expression(ref feedback);
                                }
                                else
                                {
                                    feedback += SEPARATOR_INVALID + token.Value + " , linha: " + line + "\r\n";
                                    return false;
                                }
                            }
                            else
                            {
                                Lexic.Position--;
                                break;
                            }
                        }
                        break;
                    }
                case ReservedWord.LIST:
                    {
                        //Casos análogos
                        goto case ReservedWord.RUN;
                    }

                case ReservedWord.RETURN:
                    break;
                case ReservedWord.CLEAR:
                    break;

                case ReservedWord.THEN: return false;
                case ReservedWord.RND: return false;
                case ReservedWord.INVALID: return false;
            }

            return validation;
        }

        /// <summary>
        /// Método para o não-terminal listaprint, dado por: "listaprint:=	itemprint {separador itemprint}"
        /// </summary>
        /// <param name="feedback"></param>
        /// <returns></returns>
        private bool PrintList(ref string feedback)
        {
            bool validation = true;

            validation &= PrintItem(ref feedback);

            while (true)
            {
                Token token = Lexic.NextToken();

                if (token == null)
                {
                    feedback += INCOMPLETE_TENSE_MISS + "itemprint";
                    break;
                }

                if (token.Type == TokenType.SEPARATOR)
                {
                    if (token.Value.Equals(",") || token.Value.Equals(";"))
                    {
                        validation &= PrintItem(ref feedback);
                    }
                    else
                    {
                        feedback += SEPARATOR_INVALID + token.Value + " , linha: " + line + "\r\n";
                        return false;
                    }
                }
                else
                {
                    Lexic.Position--;
                    break;
                }
            }

            return validation;
        }

        /// <summary>
        /// Método para o não-terminal itemprint, dado por: "itemprint :=	exp | string"
        /// </summary>
        /// <param name="feedback"></param>
        /// <returns></returns>
        private bool PrintItem(ref string feedback)
        {
            bool validation = true;

            Token token = Lexic.NextToken();

            if (token == null)
            {
                return false;
            }

            if (token.Type == TokenType.STRING)
            {
                return validation;
            }
            else
            {
                //Retrocede uma posição porque o valor não era um número
                Lexic.Position--;

                validation &= Expression(ref feedback);
            }

            return validation;
        }

        /// <summary>
        /// Método para o não-terminal exp, dado por: "exp		  :=	termo {op termo}"
        /// </summary>
        /// <param name="feedback"></param>
        /// <returns></returns>
        private bool Expression(ref string feedback)
        {
            bool validation = true;

            validation &= Term(ref feedback);

            while (true)
            {
                Token token = Lexic.NextToken();

                if (token == null)
                {
                    feedback += INCOMPLETE_TENSE_MISS + "termo, linha: " + line + "\r\n";
                    break;
                }
                if (token.Type == TokenType.MATH_OPERATOR)
                {
                    validation &= Term(ref feedback);
                }
                else
                {
                    Lexic.Position--;
                    break;
                }
            }

            return validation;
        }

        /// <summary>
        /// Método para o não-terminal termo, dado por: "termo	  :=	valor | var | (exp) | func"
        /// </summary>
        /// <param name="feedback"></param>
        /// <returns></returns>
        private bool Term(ref string feedback)
        {
            bool validation = true;

            Token token = Lexic.NextToken();
            if (token == null)
            {
                return false;
            }
            switch (token.Type)
            {
                case TokenType.VALUE:
                    negativeTerm = false;
                    return validation;
                case TokenType.MATH_OPERATOR:
                    if (negativeTerm)
                    {
                        feedback += OPERATOR_INVALID + token.Value + " , linha: " + line + "\r\n";
                        negativeTerm = false;
                        return false;
                    }
                    if (token.Value.Equals("-"))
                    {
                        //indica que o termo é negativo e não deve aceitar mais sinais "-" à frente do termo
                        negativeTerm = true;
                        validation &= Term(ref feedback);
                    }

                    break;
                case TokenType.VARIABLE:
                    negativeTerm = false;
                    return validation;
                case TokenType.SEPARATOR:
                    negativeTerm = false;
                    {
                        if (token.Value.Equals("("))
                        {
                            validation &= Expression(ref feedback);
                            token = Lexic.NextToken();
                            if (token == null)
                            {
                                return false;
                            }
                            if (token.Type == TokenType.SEPARATOR && token.Value.Equals(")"))
                            {
                                return validation;
                            }
                            else
                            {
                                feedback += PARENTHESIS_ERROR + " , linha: " + line + "\r\n";
                                return false;
                            }
                        }
                        else
                        {
                            feedback += SEPARATOR_INVALID + token.Value + " , linha: " + line + "\r\n";
                            return false;
                        }
                    }
                case TokenType.RESERVED_WORD:
                    negativeTerm = false;
                    {
                        if (token.Value.Equals(ReservedWord.RND.ToString()))
                        {
                            token = Lexic.NextToken();

                            if (token.Value.Equals("("))
                            {
                                validation &= Expression(ref feedback);

                                token = Lexic.NextToken();

                                if (token.Type == TokenType.SEPARATOR && token.Value.Equals(")"))
                                {
                                    return validation;
                                }
                                else
                                {
                                    feedback += PARENTHESIS_ERROR + " , linha: " + line + "\r\n";
                                    return false;
                                }
                            }
                            else
                            {
                                feedback += SEPARATOR_INVALID + token.Value + " , linha: " + line + "\r\n";
                                return false;
                            }
                        }
                        else
                        {
                            feedback += FUNCTION_INVALID + token.Value + " , linha: " + line + "\r\n";
                            return false;
                        }
                    }
            }

            return validation;
        }
        #endregion

        #region Analisador Sintático Descendente Não Recursivo
        private bool NonRecursiveAnalizer(ref string feedback)
        {
            NonRecursiveDealer dealer = new NonRecursiveDealer();
            SimpleStack<Symbol> analizerStack = new SimpleStack<Symbol>();
            analizerStack.Push(Terminal.Initial);
            analizerStack.Push(NonTerminal.InitialSymbol);
            //Empilha código

            Token token = null;
            bool nextToken = true;
            bool errorFound = false;
            if (Lexic.ListToken != null && Lexic.ListToken.Count > 0)
            {
                //Quando um token é removido da pilha, o seu valor fica null
                while (analizerStack.Size > 0 && !errorFound)
                {
                    if (nextToken)
                    {
                        token = Lexic.NextToken();
                        if (token != null)
                        {
                            while (token.Type == TokenType.LINE_BREAK)
                            {
                                line++;
                                token = Lexic.NextToken();
                            }
                        }
                        nextToken = false;
                    }

                    Symbol symbol = analizerStack.Pop();

                    if (symbol is NonTerminal)
                    {
                        NonTerminal nonTerminal = (NonTerminal)symbol;
                        List<Symbol> list = nonTerminal.GetRuleForToken(token);

                        if (list != null && list.Count != 0)
                        {
                            //Coloca os elementos da lista na ordem
                            for (int i = list.Count - 1; i >= 0; i--)
                            {
                                analizerStack.Push(list[i]);
                            }
                        }
                        else
                        {
                            if (!nonTerminal.ContainsEmpty())
                            {
                                feedback += INCOMPLETE_TENSE_MISS + nonTerminal.Value + ", linha " + line + "\r\n";
                                errorFound = true;
                            }
                            else if ((nonTerminal.Equals(NonTerminal.InitialSymbol)) && (token != null))
                            {
                                feedback += INSTRUCTION_INVALID + token.Value + ", linha " + line + "\r\n";
                                errorFound = true;
                            }
                        }
                    }
                    else
                    {
                        if (symbol.Value != "$")
                        {

                            Terminal terminal = (Terminal)symbol;

                            if (terminal != null)
                            {
                                if (token == null)
                                {
                                    feedback += INCOMPLETE_TENSE_MISS + terminal.Value + ", linha " + line + "\r\n";
                                    errorFound = true;
                                }
                                else
                                {
                                    if (terminal.EquivalentToToken(token))
                                    {
                                        //Seta a variável para buscar o próximo token
                                        nextToken = true;
                                    }
                                    else
                                    {
                                        feedback += INCOMPLETE_TENSE_MISS + terminal.Value + ", linha " + line + "\r\n";
                                        errorFound = true;
                                    }
                                }
                            }
                        }
                    }
                }

                token = Lexic.NextToken();

                if ((token != null) && (token.Type != TokenType.LINE_BREAK) && (!errorFound))
                {
                    feedback += INSTRUCTION_INVALID + token.Value + ", linha " + line + "\r\n";
                    errorFound = true;
                }
            }
            return !errorFound;
        }
        #endregion

        #region Analisador Sintático Ascendente
        private bool AscendentAnalizer(ref string feedback)
        {
            GrammarDeterministicAutomata afd = new GrammarDeterministicAutomata();

            AscendentDealer dealer = new AscendentDealer(afd);
            
            //Pilha do estado
            SimpleStack<GrammarState> stateStack = new SimpleStack<GrammarState>();

            GrammarState ascendentState = GrammarState.ConvertFrom(afd.InitialState);
            stateStack.Push(ascendentState);

            Token token = null;
            bool nextToken = true;
            bool errorFound = false;

            while (!errorFound)
            {
                GrammarState state = null;
                Symbol symbol = null;

                #region Leitura e análise do token
                if (nextToken)
                {
                    token = Lexic.NextToken();
                    if (token != null)
                    {
                        if (token.Type == TokenType.LINE_BREAK)
                        {
                            line++;
                            //Eu anulo o token para ele mais à frente preencherem o símbolo com $
                            token = null;
                        }
                    }

                    nextToken = false;
                }

                if (token != null)
                {
                    if (token.Type == TokenType.VARIABLE)
                    {
                        symbol = Symbol.GetSymbol("var");
                    }
                    else if (token.Type == TokenType.VALUE)
                    {
                        symbol = Symbol.GetSymbol("num");
                    }
                    else if (token.Type == TokenType.STRING)
                    {
                        symbol = Symbol.GetSymbol("string");
                    }
                    else
                    {
                        symbol = Symbol.GetSymbol(token.Value);
                    }
                }
                else
                {
                    //Se chegou ao fim da linha lida, começa a ler $
                    symbol = Terminal.Initial;
                }
                #endregion

                state = stateStack.Top();

                TableEntry entry = dealer.Table.GetEntry(symbol, state);
                if (entry != null)
                {
                    switch (entry.Type)
                    {
                        case EntryType.SHIFT:
                            {
                                GrammarState nextState = entry.NextState;
                                stateStack.Push(nextState);
                                nextToken = true;
                                break;
                            }
                        case EntryType.REDUCTION:
                            {
                                GrammarState grammarState = stateStack.Top();
                                int reduction = dealer.GenerateReduction(grammarState).ReductionNumber;

                                Symbol newSymbol = grammarState.GetStateSymbol();

                                for (int i = 0; i < reduction; i++)
                                {
                                    stateStack.Pop();
                                }

                                GrammarState newStateTop = stateStack.Top();
                                entry = dealer.Table.GetEntry(newSymbol, newStateTop);

                                if (entry.Type.Equals(EntryType.GOTO))
                                {
                                    goto case EntryType.GOTO;
                                }

                                //Falta empilhar aqui
                                break;
                            }
                        case EntryType.GOTO:
                            {
                                stateStack.Push(entry.NextState);
                                break;
                            }
                        case EntryType.ACCEPTED:
                            {
                                //Fim de arquivo
                                if (Lexic.ListToken.Count == Lexic.Position)
                                {
                                    return true;
                                }
                                else
                                {
                                    //Ler próxima linha, esvaziando a pilha
                                    nextToken = true;
                                    while (!stateStack.IsEmpty())
                                    {
                                        stateStack.Pop();
                                    }
                                    stateStack.Push(ascendentState);
                                }
                                break;
                            }
                        default:
                            {
                                feedback += PARAMETER_INVALID + token.Value + ", linha " + line + "\r\n";
                                errorFound = true;
                                break;
                            }
                    }
                }
                else
                {
                    if (state.IsFinal)
                    {
                        //GrammarState grammarState = stateStack.Top();
                        int reduction = dealer.GenerateReduction(state).ReductionNumber;

                        Symbol newSymbol = state.GetStateSymbol();

                        for (int i = 0; i < reduction; i++)
                        {
                            stateStack.Pop();
                        }

                        GrammarState newStateTop = stateStack.Top();
                        entry = dealer.Table.GetEntry(newSymbol, newStateTop);

                        if (entry.Type.Equals(EntryType.GOTO))
                        {
                            stateStack.Push(entry.NextState);
                        }
                    }
                    else
                    {
                        String expectedState = null;
                        if (state.StateValue.Contains('.'))
                        {
                            expectedState = state.StateValue.Split('.')[1].Split(' ')[1].Trim();
                        }
                        
                        if (expectedState == null)
                        {
                            feedback += PARAMETER_INVALID + token.Value + ", linha " + line + "\r\n";
                            errorFound = true;
                            break;
                        }
                        else
                        {
                            feedback += INCOMPLETE_TENSE_MISS + expectedState + ", linha " + line + "\r\n";
                            errorFound = true;
                            break;
                        }
                        
                    }
                }
                    
                #region código antigo
                /*
                //Para debug
                List<TableEntry> list = dealer.Table.TableEntriesFor(state);
                List<TableEntry> list2 = dealer.Table.TableEntriesIn(symbol);
                TableEntry entry = dealer.Table.GetEntry(symbol, state);

                if (entry != null)
                {
                    switch (entry.Type)
                    {
                        case EntryType.SHIFT:
                            {
                                GrammarState nextState = entry.NextState;
                                stateStack.Push(nextState);

                                if (nextState.IsFinal)
                                {
                                    goto case EntryType.REDUCTION;
                                }
                                else
                                {
                                    nextToken = true;
                                }
                                break;
                            }
                        case EntryType.REDUCTION:
                            {
                                #region código abandonado
                                //for (int i = 0; i < entry.Vertical.ReductionNumber; i++)
                                //{
                                //    stateStack.Pop();
                                //}
                                #endregion

                                //Desempilhar (reductionNumber) estados da pilha

                                //Topo: itemprint:string . -> reduction = 1
                                int reduction = dealer.GenerateReduction(stateStack.Top()).ReductionNumber;

                                for (int i = 0; i < reduction; i++)
                                {
                                    stateStack.Pop();
                                }

                                //entry.NextState: itemprint: string .
                                //nextSymbol = itemprint
                                Symbol nextSymbol = null;
                                if (entry.NextState != null)
                                {
                                    nextSymbol = entry.NextState.GetStateSymbol();
                                }
                                else
                                {
                                    nextSymbol = entry.Vertical.GetStateSymbol();
                                }

                                //Topo: instrução:PRINT . listaprint
                                GrammarState topState = stateStack.Top();

                                //Voltar para o não-terminal à esquerda da produção                       
                                if (topState == null)
                                {
                                    entry = dealer.Table.GetEntry(nextSymbol, ascendentState);
                                }
                                else
                                {
                                    //Deve ter uma transição de PRINT. listaprint em itemprint
                                    entry = dealer.Table.GetEntry(nextSymbol, topState);
                                }

                                if (entry != null)
                                {
                                    if (entry.Type == EntryType.GOTO)
                                    {
                                        nextToken = true;
                                        goto case EntryType.GOTO;
                                    }
                                    else
                                    {
                                        errorFound = true;
                                    }
                                }
                                else
                                {
                                    feedback += PARAMETER_INVALID + token.Value + ", linha " + line + "\r\n";
                                    errorFound = true;
                                }

                                break;

                            }
                        case EntryType.GOTO:
                            {
                                stateStack.Push(entry.NextState);
                                break;
                            }
                        case EntryType.ACCEPTED:
                            {
                                //Fim de arquivo
                                if (Lexic.ListToken.Count == Lexic.Position)
                                {
                                    return true;
                                }
                                else
                                {
                                    //Ler próxima linha, esvaziando a pilha
                                    nextToken = true;
                                    while (!stateStack.IsEmpty())
                                    {
                                        stateStack.Pop();
                                    }
                                    stateStack.Push(ascendentState);
                                }
                                break;
                            }
                        default:
                            {
                                feedback += PARAMETER_INVALID + token.Value + ", linha " + line + "\r\n";  
                                errorFound = true;
                                break;
                            }
                    }
                }
                    //Caso a entry seja nula
                else
                {
                    if (state != null)
                    {
                        if (state.IsFinal)
                        {                            
                            //feedback += PARAMETER_INVALID + token.Value + ", linha " + line + "\r\n";                            

                            //Topo: listaprint:itemprint. separador listaprint
                            GrammarState stackTop = stateStack.Top();

                            //Realiza redução daquele estado
                            int reduction = dealer.GenerateReduction(stackTop).ReductionNumber;
                            for (int i = 0; i < reduction; i++)
                            {
                                stateStack.Pop();
                            }

                            //nextSymbol = listaprint
                            Symbol nextSymbol = stackTop.GetStateSymbol();

                            //Topo: instrução:PRINT . listaprint
                            stackTop = stateStack.Top();

                            //Deve ter uma transição de listaprint. em instrução

                            //List<TableEntry> debugList = dealer.Table.TableEntriesFor(topState);
                            entry = dealer.Table.GetEntry(nextSymbol, stackTop);

                            if (entry != null)
                            {
                                GrammarState nextState = entry.NextState;
                                //Verificar aqui para quando pular e quando não pular o token
                                stateStack.Push(nextState);

                                //if (nextState.VerifyFinal())
                                //{
                                //    nextToken = true;
                                //}

                                TableEntry entry2 = dealer.Table.GetEntry(symbol, nextState);

                                if (entry2 == null || entry2.NextState == null)
                                {
                                    nextToken = true;
                                }
                            }
                            else
                            {
                                feedback += PARAMETER_INVALID + token.Value + ", linha " + line + "\r\n";
                                errorFound = true;
                            }
                        }
                        else if (token != null)
                        {
                            errorFound = true;
                            feedback += INSTRUCTION_INVALID + token.Value + ", linha " + line + "\r\n";
                        }
                        else
                        {
                            errorFound = true;
                        }
                    }
                    else
                    {
                        errorFound = true;
                    }
                }
                */
                #endregion
            }
            return !errorFound;
        }

        /// <summary>
        /// Análise sintática aceitando qualquer gramática. Não pode necessitar de análise léxica para identificar tokens.
        /// </summary>
        /// <param name="feedback"></param>
        /// <returns></returns>
        private bool AscendentAnalizerNonLexic(ref string feedback)
        {
            GrammarDeterministicAutomata afd = new GrammarDeterministicAutomata();

            AscendentDealer dealer = new AscendentDealer(afd);

            //Pilha dos símbolos
            SimpleStack<Symbol> symbolStack = new SimpleStack<Symbol>();
            symbolStack.Push(Terminal.Initial);
            //Pilha do estado
            SimpleStack<GrammarState> stateStack = new SimpleStack<GrammarState>();

            GrammarState ascendentState = GrammarState.ConvertFrom(afd.InitialState);
            stateStack.Push(ascendentState);

            string[] file = Lexic.File;

            bool success = false;
            bool errorFound = false;

            GrammarState state;
            Symbol symbol;

            foreach (string line in file)
            {
                int indexToken = 0;
                List<string> tokens = new List<string>(line.Split(' '));
                tokens.Add("$");

                string token = "";
                state = null;
                symbol = null;

                while (!errorFound || !success)
                {
                    token = tokens[indexToken++];
                    symbol = Symbol.GetSymbol(token);

                    state = stateStack.Top();

                    //Para debug
                    List<TableEntry> list1 = dealer.Table.TableEntriesFor(state);
                    List<TableEntry> list2 = dealer.Table.TableEntriesIn(symbol);

                    TableEntry entry = dealer.Table.GetEntry(symbol, state);

                    if (entry != null)
                    {
                        switch (entry.Type)
                        {
                            case EntryType.SHIFT:
                                {
                                    GrammarState nextState = entry.NextState;

                                    symbolStack.Push(entry.Horizontal);
                                    stateStack.Push(nextState);
                                    if (nextState.IsFinal)
                                    {
                                        goto case EntryType.REDUCTION;
                                    }
                                    break;
                                }
                            case EntryType.REDUCTION:
                                {
                                    GrammarState nextState = entry.NextState;

                                    int reduction = dealer.GenerateReduction(nextState).ReductionNumber;
                                    for (int i = 0; i < reduction; i++)
                                    {
                                        symbolStack.Pop();
                                        stateStack.Pop();
                                    }

                                    Symbol trans = nextState.GetStateSymbol();
                                    entry = dealer.Table.GetEntry(trans, stateStack.Top());

                                    nextState = entry.NextState;

                                    symbolStack.Push(entry.Horizontal);
                                    stateStack.Push(nextState);
                                    break;
                                }
                            default:
                                break;
                        }
                    }
                }
            }
            
            return !errorFound;
        }
        #endregion
    }
}
