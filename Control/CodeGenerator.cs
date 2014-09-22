using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyBASICAnalizer.Model;
using TinyBASICAnalizer.Model.Code;

namespace TinyBASICAnalizer.Control
{
    /// <summary>
    /// Classe responsável pela geração de código e interpretação dos dados relevante à geração
    /// </summary>
    public class CodeGenerator
    {
        //Importante:
        //Os input são obrigatoriamente no início

        private string[] _file;
        /// <summary>
        /// Arquivo do código vindo do TinyBasic para ser convertido em código para VM
        /// </summary>
        public string[] File
        {
            get { return _file; }
            set { _file = value; }
        }

        private SyntaxAnalizerType _syntaxType;
        /// <summary>
        /// Tipo da análise sintática realizada
        /// </summary>
        public SyntaxAnalizerType SyntaxType
        {
            get { return _syntaxType; }
            set { _syntaxType = value; }
        }

        private LexicAnalizer _lexicAnalizer;
        /// <summary>
        /// Analisador léxico
        /// </summary>
        public LexicAnalizer LexicAnalizer
        {
            get 
            {
                if (_lexicAnalizer == null)
                {
                    _lexicAnalizer = new LexicAnalizer(File);
                }
                return _lexicAnalizer; 
            }
            set { _lexicAnalizer = value; }
        }

        private SyntaxAnalizer _syntaxAnalizer;
        /// <summary>
        /// Analisador sintático
        /// </summary>
        public SyntaxAnalizer SyntaxAnalizer
        {
            get 
            {
                if (_syntaxAnalizer == null)
                {
                    _syntaxAnalizer = new SyntaxAnalizer(LexicAnalizer, SyntaxType);
                }
                return _syntaxAnalizer; 
            }
            set { _syntaxAnalizer = value; }
        }

        public CodeGenerator()
            : this(null, SyntaxAnalizerType.ASCENDENT)
        { }

        public CodeGenerator(string[] File, SyntaxAnalizerType SyntaxType)
        {
            this.File = File;
            this.SyntaxType = SyntaxType;
        }

        public string[] GenerateCode()
        {
            /*
             * Código gerado
             * LSP 00 10 <- carrega a quantidade de memória
             * JMP XX 00 <- pula as variáveis
             * YYYYY     <- linha das variáveis
             * QTD INPUT <- qtde. de inputs
             * ZZZZZ     <- end. de cada variável de input
             * CODIGO    <- começa o código
             * STOP      <- termina o código
             */ 

            /*
             * Código STR = para strings
             * STR XX 00 <- qtde. da caracteres
             * BBBB      <- código do caractere
             */ 

            /*
             * Existem 2 JMP
             * JMP XX XX -> jump para o endereço
             * JMP -> jump para o topo da pilha
             */

            //Importante: A leitura dos hexadecimais é feita da esquerda para a direita
            List<string> code = new List<string>();

            string hexValue;
            string hexValue2;

            //6 bytes das duas primeiras instruções + 2 bytes para cada variável
            IntToHexa(1024, out hexValue, out hexValue2);

            //Começa sempre com LSP 00 10 -> 0100
            code.Add("LSP");
            code.Add(hexValue);
            code.Add(hexValue2);

            Dictionary<string, Variable> variables = GetVariables(File);

            //Utilizando para guardar a correspondência das linhas no código em TinyBasic com as linhas do código gerado
            Dictionary<int, int> lineNumber = new Dictionary<int, int>();

            int numberVariables = variables.Count;

            // Convert integer 182 as a hex in a string variable

            //6 bytes das duas primeiras instruções + 2 bytes para cada variável
            IntToHexa(numberVariables+3, out hexValue, out hexValue2);

            //Aqui vem o jump para a posição após a declaração das variáveis
            code.Add("JMP");
            code.Add(hexValue);
            code.Add(hexValue2);

            //Depois vem a declaração de cada variável
            int initialPosition = 3;
            foreach (string variableName in variables.Keys)
            {
                Variable variable = variables[variableName];
                variable.CodePosition = initialPosition;

                IntToHexa(0, out hexValue, out hexValue2);                
                code.Add(hexValue);
                code.Add(hexValue2);
                initialPosition++;
            }

            string[] inputList = GetInputs(variables, File);

            // Divide por 2, porque são 2 valores para cada input
            IntToHexa(inputList.Length/2, out hexValue, out hexValue2);
            code.Add(hexValue);
            code.Add(hexValue2);

            foreach (string input in inputList)
            {
                code.Add(input);
            }

            //Começa a leitura das linhas
            foreach (string line in File)
            {
                code.AddRange(ReadLine(variables, lineNumber, line, code.Count));
            }

            code.Add("STOP");

            return code.ToArray();
        }

        /// <summary>
        /// Converte um inteiro para dois valores HEXA do tipo 00 00 para as operações
        /// </summary>
        /// <param name="intValue"></param>
        /// <param name="hexValue"></param>
        /// <param name="hexValue2"></param>
        private void IntToHexa(int intValue, out string hexValue, out string hexValue2)
        {
            hexValue = intValue.ToString("X");
            hexValue2 = "0";

            if (hexValue.Length > 2)
            {
                hexValue2 = "";
                if (hexValue.Length == 4)
                {
                    hexValue2 += hexValue[0];
                    hexValue2 += hexValue[1];

                    hexValue = hexValue[2] + "" + hexValue[3];
                }
                else
                {
                    hexValue2 += hexValue[0];

                    hexValue = hexValue[1] + "" + hexValue[2];
                }
            }
        }

        private string[] ReadLine(Dictionary<string, Variable> variables, Dictionary<int, int> lineNumber, string line, int offset)
        {
            //Importante: A leitura dos hexadecimais é feita da esquerda para a direita
            List<string> code = new List<string>();

            if (line == null || line.Equals(""))
            {
                return new string[] { };
            }

            LexicAnalizer localLexic = new LexicAnalizer();
            localLexic.Analize(new string[] { line });

            Token token = localLexic.NextToken();
            string hexValue;
            string hexValue2;

            //Expressão começou com um identificador de linha
            if (token.Type == TokenType.VALUE)
            {
                int number = int.Parse(token.Value);
                if (!lineNumber.ContainsKey(number))
                {
                    lineNumber[number] = offset;
                }

                token = localLexic.NextToken();
            }
            switch (token.Value)
            {
                case "GOTO":
                    {
                        string expression = line.Substring(line.IndexOf("GOTO") + 4);

                        string[] commands = CalculateExpressionVariable(expression, variables);

                        int jmpNumber = lineNumber[int.Parse(commands[1])]+1;
                        IntToHexa(jmpNumber, out hexValue, out hexValue2);

                        code.Add("JMP");
                        code.Add(hexValue);
                        code.Add(hexValue2);

                    }
                    break;
                case "LET":
                    {
                        token = localLexic.NextToken();

                        Variable variable = variables[token.Value];

                        string expression = line.Substring(line.IndexOf("=") + 2);

                        string[] commands = CalculateExpressionVariable(expression, variables);
                        code.AddRange(commands);

                        int address = variable.CodePosition;

                        IntToHexa(address, out hexValue, out hexValue2);
                        code.Add("STO");
                        code.Add(hexValue);
                        code.Add(hexValue2);
                    }
                    break;
                case "PRINT":
                    {
                        string[] printCode = null;

                        do
                        {
                            token = localLexic.NextToken();

                            if (token != null)
                            {
                                if (token.Type == TokenType.STRING)
                                {
                                    printCode = PrintString(token.Value);
                                }
                                else
                                {
                                    #region Print de Expressão
                                    string expression = "";
                                    do
                                    {
                                        expression = token.Value + " ";
                                        token = localLexic.NextToken();
                                    } while (token != null && token.Type != TokenType.SEPARATOR && !token.Value.Equals(","));

                                    printCode = CalculateExpressionVariable(expression, variables);
                                    #endregion
                                }
                                code.AddRange(printCode);
                                code.Add("OUT");
                            }
                        } while (token != null);
                    }
                    break;
                case "REM":
                    {
                        token = localLexic.NextToken();

                        string[] printCode = PrintString(token.Value);
                        code.AddRange(printCode);
                    }
                    break;
                case "RETURN":
                    {
                        code.Add("STOP");
                    }
                    break;
                case "IF":
                    {
                        string firstExp = "";
                        string compareExp = "";

                        #region Carrega o valor da primeira expressão
                        token = localLexic.NextToken();
                        do
                        {
                            firstExp += token.Value + " ";
                            token = localLexic.NextToken();
                        } while (token.Type != TokenType.RELATIONAL_OPERATOR);

                        string[] firstCode = CalculateExpressionVariable(firstExp, variables);

                        code.AddRange(firstCode);
                        #endregion

                        //Busca o comparador da expressão do IF
                        string comparator = GetComparator(token);

                        #region Carrega o valor da segunda expressão

                        token = localLexic.NextToken();
                        do
                        {
                            compareExp += token.Value + " ";
                            token = localLexic.NextToken();
                        } while (token.Type != TokenType.RESERVED_WORD);

                        string[] compareCode = CalculateExpressionVariable(compareExp, variables);

                        code.AddRange(compareCode);
                        #endregion

                        code.Add(comparator);

                        //Carrega o valor do THEN
                        string thenExpression = line.Substring(line.IndexOf("THEN") + 5);

                        string[] thenCode = ReadLine(variables, lineNumber, thenExpression, offset);

                        int actualNumberOfAddresses = code.Count;                          
                        
                        int numberOfAddresses = thenCode.Length;                        

                        //+1 para pular a linha do JF
                        //+1 para a próxima expressão depois do IF
                        int jumpTo = offset + actualNumberOfAddresses + numberOfAddresses + 4;

                        IntToHexa(jumpTo, out hexValue, out hexValue2);
                        code.Add("JF");
                        code.Add(hexValue);
                        code.Add(hexValue2);

                        for (int i = 0; i < thenCode.Length;i++)
                        {
                            /*Atualiza os endereços JF dos IF aninhados
                            para o valor do JF relativo ao IF mais externo*/
                            if (thenCode[i].Contains("JF"))
                            {
                                thenCode[i] = "JF";
                                thenCode[i + 1] = hexValue;
                                thenCode[i + 2] = hexValue2;
                                break;
                            }
                        }

                        code.AddRange(thenCode);
                    }
                    break;
            }
            
            return code.ToArray();
        }

        /// <summary>
        /// Pega o valor do token e retorna o valor em linguagem de código, por exemplo: = é EQ
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private string GetComparator(Token token)
        {
            string comparator = "";
            switch (token.Value)
            {
                case "=":
                    comparator = "EQ";
                    break;
                case "<>":
                    comparator = "NE";
                    break;
                case "><":
                    comparator = "NE";
                    break;
                case "<=":
                    comparator = "LE";
                    break;
                case "<":
                    comparator = "LT";
                    break;
                case "=>":
                    comparator = "GE";
                    break;
                case ">":
                    comparator = "GT";
                    break;
            }

            return comparator;
        }

        /// <summary>
        /// Faz a leitura do código e retorna um hash com as variáveis que existem no mesmo
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, Variable> GetVariables()
        {
            return GetVariables(File);
        }

        /// <summary>
        /// Retorna o código para a impressão da string
        /// </summary>
        /// <param name="print"></param>
        /// <returns></returns>
        private string[] PrintString(string print)
        {
            print = print.Replace("\"", "");

            List<string> code = new List<string>();
            string hexValue;
            string hexValue2;
            IntToHexa(print.Length, out hexValue, out hexValue2);
            code.Add("STR");
            code.Add(hexValue);
            code.Add(hexValue2);

            foreach (char character in print)
            {
                int value = (int)character;
                IntToHexa(value, out hexValue, out hexValue2);                
                code.Add(hexValue);
                code.Add(hexValue2);
            }

            return code.ToArray();
        }

        /// <summary>
        /// Busca as linhas que correspondem aos input do código
        /// </summary>
        /// <param name="variables"></param>
        /// <param name="File"></param>
        /// <returns></returns>
        private string[] GetInputs(Dictionary<string, Variable> variables, string[] File)
        {
            //Utiliza um analisador léxico próprio para a expressão, para não confundir os tokens
            LexicAnalizer lexic = new LexicAnalizer();
            lexic.Analize(File);

            List<string> inputList = new List<string>();

            Token token = null;
            Variable variable = null;
            bool nextToken = true;
            string hexValue;
            string hexValue2;
            do
            {
                if (nextToken)
                {
                    token = lexic.NextToken();
                    nextToken = false;
                }
                if (token != null)
                {
                    if (token.Type == TokenType.RESERVED_WORD)
                    {
                        if (token.Value.Equals("INPUT"))
                        {
                            //Pega a primeira variável
                            token = lexic.NextToken();
                            variable = variables[token.Value];

                            IntToHexa(variable.CodePosition, out hexValue, out hexValue2);
                            inputList.Add(hexValue);
                            inputList.Add(hexValue2);

                            token = lexic.NextToken();
                            //Procura por uma lista de inputs
                            if (token != null)
                            {
                                while (token != null && token.Type == TokenType.SEPARATOR)
                                {
                                    token = lexic.NextToken();
                                    variable = variables[token.Value];

                                    IntToHexa(variable.CodePosition, out hexValue, out hexValue2);
                                    inputList.Add(hexValue);
                                    inputList.Add(hexValue2);

                                    token = lexic.NextToken();
                                }
                            }
                        }
                        else
                        {
                            nextToken = true;
                        }
                    }
                    else
                    {
                        nextToken = true;
                    }
                }
            } while (token != null);

            return inputList.ToArray();
        }

        /// <summary>
        /// Faz a leitura do código e retorna um hash com as variáveis que existem no mesmo
        /// </summary>
        /// <param name="File"></param>
        /// <returns></returns>
        private Dictionary<string, Variable> GetVariables(string[] File)
        {
            Dictionary<string, Variable> variables = new Dictionary<string, Variable>();

            LexicAnalizer.Analize(File);

            Token token = null;
            do
            {
                token = LexicAnalizer.NextToken();
                if (token != null)
                {
                    if (token.Type == TokenType.VARIABLE)
                    {
                        string variableName = token.Value;
                        if (!variables.ContainsKey(variableName))
                        {
                            variables.Add(variableName, new Variable(variableName));
                        }
                    }
                }
            } while (token != null);

            return variables;
        }

        /// <summary>
        /// Organiza a expressão colocando-a na ordem dos parêntesis
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public string AdjustParenthesis(string expression)
        {
            //Esse método pode gerar espaços ' ' ocassionais desnecessários

            if (expression.Contains('('))
            {
                string parenthesisExp = GetParenthesis(expression);
                string newExp = AdjustParenthesis(parenthesisExp);

                expression = expression.Replace("(" + parenthesisExp + ")", "");

                newExp += " ";
                if (!expression.Contains('('))
                {
                    for (int i = expression.Length - 1; i > -1; i--)
                    {
                        newExp += expression[i];
                    }
                }
                else
                {
                    int index = expression.IndexOf('(');
                    newExp += expression.Substring(0, index);
                    string anotherExp = AdjustParenthesis(expression.Substring(index));
                    newExp += anotherExp;
                }

                expression = newExp;
            }

            return expression;
        }

        /// <summary>
        /// Retorna a expressão dentro do parêntesis mais externo da expressão passada
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private string GetParenthesis(string expression)
        {
            string parenthesis = "";
            int openParenthesisCount = 0;
            int stringPosition = expression.IndexOf('(');
            int size = 0;

            for (int i = stringPosition + 1; i < expression.Length; i++)
            {
                if (expression[i] == ')')
                {
                    if (openParenthesisCount > 0)
                    {
                        openParenthesisCount--;
                    }
                    else
                    {
                        size = i;
                        break;
                    }
                }

                if (expression[i] == '(')
                {
                    openParenthesisCount++;
                }
            }

            parenthesis = expression.Substring(stringPosition + 1, size - stringPosition - 1);

            //Retorna a expressão dentro dos parênteses, excluindo os parênteses externos
            return parenthesis;
        }

        public string[] CalculateExpressionVariable(string expression, Dictionary<string, Variable> variables)
        {
            //Não é aceitável ter RND com variável
            List<string> commands = new List<string>();
            String commandCache = null;

            bool hasParenthesis = expression.Contains('(');
            if (hasParenthesis)
            {
                //Remove todos os parênteses colocando a expressão na forma que deve acontecer
                expression = AdjustParenthesis(expression);
            }

            LexicAnalizer lexic = new LexicAnalizer();
            lexic.Analize(new string[] { expression });

            bool hasVariables = lexic.HasVariable();

            string hexValue;
            string hexValue2;
            if (hasVariables)
            {
                Token token = null;
                do
                {
                    token = lexic.NextToken();

                    if (token != null)
                    {
                        switch (token.Type)
                        {
                            case TokenType.VALUE:
                                {
                                    IntToHexa(int.Parse(token.Value), out hexValue, out hexValue2);
                                    commands.Add("LDI");
                                    commands.Add(hexValue);
                                    commands.Add(hexValue2);
                                    if (commandCache != null)
                                    {
                                        commands.Add(commandCache);
                                        commandCache = null;
                                    }
                                }
                                break;
                            case TokenType.VARIABLE:
                                {
                                    Variable variable = variables[token.Value];
                                    IntToHexa(variable.CodePosition, out hexValue, out hexValue2);
                                    commands.Add("LOD");
                                    commands.Add(hexValue);
                                    commands.Add(hexValue2);
                                    if (commandCache != null)
                                    {
                                        commands.Add(commandCache);
                                        commandCache = null;
                                    }
                                }
                                break;
                            case TokenType.MATH_OPERATOR:
                                {
                                    string command = "";
                                    switch (token.Value)
                                    {
                                        case "+":
                                            command = "ADD";
                                            break;
                                        case "-":
                                            command = "MIN";
                                            break;
                                        case "*":
                                            command = "TIM";
                                            break;
                                        case "/":
                                            command = "DIV";
                                            break;
                                    }
                                    commandCache = command;
                                }
                                break;
                        }
                    }
                } while (token != null);
            }
            else
            {
                int result = CalculateExpression(expression);
                IntToHexa(result, out hexValue, out hexValue2);
                commands.Add("LDI");
                commands.Add(hexValue);
                commands.Add(hexValue2);
            }

            return commands.ToArray();
        }

        /// <summary>
        /// Para calcular expressões sem parênteses e sem variáveis
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public int CalculateExpression(string expression)
        {
            LexicAnalizer lexic = new LexicAnalizer();
            lexic.Analize(new string[] { expression });

            int result = 0;
            Token token = null;

            //Primeiro deve vir um número
            token = lexic.NextToken();
            result = int.Parse(token.Value);

            do
            {
                token = lexic.NextToken();

                if (token != null)
                {
                    string mathOperator = token.Value;

                    token = lexic.NextToken();
                    int nextValue = int.Parse(token.Value);

                    switch (mathOperator)
                    {
                        case "+":
                            result += nextValue;
                            break;
                        case "-":
                            result -= nextValue;
                            break;
                        case "*":
                            result *= nextValue;
                            break;
                        case "/":
                            result /= nextValue;
                            break;
                    }
                }
            } while (token != null);

            return result;
        }

        /// <summary>
        /// Retorna o código gerado como um array de bytes
        /// </summary>
        /// <returns></returns>
        public byte[] CodeByteStream()
        {
            string[] code = GenerateCode();
            List<byte> byteCode = new List<byte>();

            foreach (string line in code)
            {
                //if (line.Contains(" "))
                //{
                //    string[] elements = line.Split(' ');

                //    int i = 0;
                //    if (elements.Length == 3)
                //    {
                //        byteCode.Add(CommandToCode(elements[i++]));
                //    }
                //    byteCode.Add((byte)Convert.ToInt32(elements[i++], 16));
                //    byteCode.Add((byte)Convert.ToInt32(elements[i], 16));
                //}
                //else
                //{
                //}
                int command = CommandToCode(line);

                if (command == 0)
                {
                    byteCode.Add((byte)Convert.ToInt32(line, 16));
                }
                else
                {
                    byteCode.Add((byte)command);
                }
            }

            return byteCode.ToArray();
        }

        /// <summary>
        /// Retorna o código correspondente ao comando passado
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static byte CommandToCode(string command)
        {
            switch (command)
            {
                case "LSP": return 1;
                case "JMP": return 2;
                case "STOP": return 3;
                case "LOD": return 4;
                case "LDI": return 5;
                case "STR": return 6;
                case "EQ": return 7;
                case "NE": return 8;
                case "LE": return 9;
                case "LT": return 10;
                case "GE": return 11;
                case "GT": return 12;
                case "ADD": return 13;
                case "TIM": return 14;
                case "MIN": return 15;
                case "DIV": return 16;
                case "STO": return 17;
                case "OUT": return 18;
                case "JF": return 19;
            }

            return 0;
        }

        /// <summary>
        /// Retorna o comando correspondente ao código passado
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string CodeToCommand(byte code)
        {
            switch (code)
            {
                case 1: return "LSP";
                case 2: return "JMP";
                case 3: return "STOP";
                case 4: return "LOD";
                case 5: return "LDI";
                case 6: return "STR";
                case 7: return "EQ";
                case 8: return "NE";
                case 9: return "LE";
                case 10: return "LT";
                case 11: return "GE";
                case 12: return "GT";
                case 13: return "ADD";
                case 14: return "TIM";
                case 15: return "MIN";
                case 16: return "DIV";
                case 17: return "STO";
                case 18: return "OUT";
                case 19: return "JF";
            }

            return null;
        }

        #region código abandonado
        ///// <summary>
        ///// Para resolver expressões que não contém parentesis e nem variáveis
        ///// </summary>
        ///// <param name="expression"></param>
        ///// <param name="numberOfTokens"></param>
        ///// <returns></returns>
        //public int CalculateExpression(string expression, ref int numberOfTokens)
        //{
        //    //Utiliza um analisador léxico próprio para a expressão, para não confundir os tokens
        //    LexicAnalizer lexic = new LexicAnalizer();
        //    lexic.Analize(new string[] { expression });

        //    numberOfTokens = lexic.ListToken.Count;
        //    int result = 0;
        //    bool firstValue = true;
        //    bool errorFound = false;
        //    Token token = null;

        //    do
        //    {
        //        token = lexic.NextToken();

        //        if (token != null)
        //        {
        //            switch (token.Type)
        //            {
        //                #region case RND
        //                //case TokenType.RESERVED_WORD:
        //                //    {
        //                //        if (token.Value.Equals("RND"))
        //                //        {
        //                //            if (firstValue)
        //                //            {
        //                //                firstValue = false;
        //                //                token = lexic.NextToken();
        //                //                string parenthesis = GetParenthesis(expression, token);
        //                //                int jumpTokens = 0;
        //                //                int parenthesisResult = CalculateExpression(parenthesis, ref jumpTokens);

        //                //                Random random = new Random();
        //                //                result = random.Next(parenthesisResult);

        //                //                jumpTokens += 1; //Por causa do ultimo )
        //                //                if (jumpTokens > 0)
        //                //                {
        //                //                    //Salta os tokens do parêntesis
        //                //                    while (jumpTokens > 0)
        //                //                    {
        //                //                        lexic.NextToken();
        //                //                        jumpTokens--;
        //                //                    }
        //                //                }
        //                //            }
        //                //        }
        //                //    }
        //                //    break;
        //                #endregion
        //                case TokenType.VALUE:
        //                    if (firstValue)
        //                    {
        //                        firstValue = false;
        //                        result = int.Parse(token.Value);
        //                    }
        //                    break;
        //                #region case parentesis
        //                //case TokenType.SEPARATOR:
        //                //    if (firstValue)
        //                //    {
        //                //        firstValue = false;
        //                //        string parenthesis = GetParenthesis(expression, token);
        //                //        int jumpTokens = 0;
        //                //        int parenthesisResult = CalculateExpression(parenthesis, ref jumpTokens);

        //                //        result = parenthesisResult;

        //                //        jumpTokens += 1; //Por causa do ultimo )
        //                //        if (jumpTokens > 0)
        //                //        {
        //                //            //Salta os tokens do parêntesis
        //                //            while (jumpTokens > 0)
        //                //            {
        //                //                lexic.NextToken();
        //                //                jumpTokens--;
        //                //            }
        //                //        }
        //                //    }
        //                //    break;
        //                #endregion
        //                #region case variable
        //                //case TokenType.VARIABLE:
        //                //    if (firstValue)
        //                //    {
        //                //        firstValue = false;
        //                //        result = variables[token.Value].Value;
        //                //    }
        //                //    break;
        //                #endregion
        //                case TokenType.MATH_OPERATOR:
        //                    if (firstValue)
        //                    {
        //                        firstValue = false;
        //                        #region verificação de primeiro valor
        //                        if (token.Value.Equals("-"))
        //                        {
        //                            token = lexic.NextToken();

        //                            switch (token.Type)
        //                            {
        //                                case TokenType.VALUE:
        //                                    result = -1 * int.Parse(token.Value);
        //                                    break;
        //                                #region case variable
        //                                //case TokenType.VARIABLE:
        //                                //    int variableValue = variables[token.Value].Value;
        //                                //    result = -1 * variableValue;
        //                                //    break;
        //                                #endregion
        //                                #region case parenthesis
        //                                //case TokenType.SEPARATOR:
        //                                //    if (token.Value.Equals("("))
        //                                //    {
        //                                //        string parenthesis = GetParenthesis(expression, token);

        //                                //        int jumpTokens = 0;
        //                                //        int parenthesisResult = CalculateExpression(parenthesis, ref jumpTokens);

        //                                //        result = -1 * parenthesisResult;

        //                                //        jumpTokens += 1; //Por causa do ultimo )
        //                                //        if (jumpTokens > 0)
        //                                //        {
        //                                //            //Salta os tokens do parêntesis
        //                                //            while (jumpTokens > 0)
        //                                //            {
        //                                //                lexic.NextToken();
        //                                //                jumpTokens--;
        //                                //            }
        //                                //        }
        //                                //    }
        //                                //    else
        //                                //    {
        //                                //        errorFound = true;
        //                                //    }
        //                                //    break;
        //                                #endregion
        //                                default:
        //                                    errorFound = true;
        //                                    break;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            errorFound = true;
        //                        }
        //                        #endregion
        //                    }
        //                    else
        //                    {
        //                        string mathOperator = token.Value;

        //                        token = lexic.NextToken();

        //                        int nextValue = 0;

        //                        switch (token.Type)
        //                        {
        //                            case TokenType.VALUE:
        //                                nextValue = int.Parse(token.Value);
        //                                break;
        //                            #region case RND
        //                            //case TokenType.RESERVED_WORD:
        //                            //    {
        //                            //        if (token.Value.Equals("RND"))
        //                            //        {
        //                            //            token = lexic.NextToken();
        //                            //            string parenthesis = GetParenthesis(expression, token);
        //                            //            int jumpTokens = 0;
        //                            //            int parenthesisResult = CalculateExpression(parenthesis, ref jumpTokens);

        //                            //            Random random = new Random();
        //                            //            nextValue = random.Next(parenthesisResult);

        //                            //            jumpTokens += 1; //Por causa do ultimo )
        //                            //            if (jumpTokens > 0)
        //                            //            {
        //                            //                //Salta os tokens do parêntesis
        //                            //                while (jumpTokens > 0)
        //                            //                {
        //                            //                    lexic.NextToken();
        //                            //                    jumpTokens--;
        //                            //                }
        //                            //            }
        //                            //        }
        //                            //    }
        //                            //    break;
        //                            #endregion
        //                            #region case variables
        //                            //case TokenType.VARIABLE:
        //                            //    int variableValue = variables[token.Value].Value;
        //                            //    nextValue = variableValue;
        //                            //    break;
        //                            #endregion
        //                            #region case parenthesis
        //                            //case TokenType.SEPARATOR:
        //                            //    if (token.Value.Equals("("))
        //                            //    {
        //                            //        string parenthesis = GetParenthesis(expression, token);

        //                            //        int jumpTokens = 0;
        //                            //        int parenthesisResult = CalculateExpression(parenthesis, ref jumpTokens);

        //                            //        nextValue = parenthesisResult;

        //                            //        jumpTokens += 1; //Por causa do ultimo )
        //                            //        if (jumpTokens > 0)
        //                            //        {
        //                            //            //Salta os tokens do parêntesis
        //                            //            while (jumpTokens > 0)
        //                            //            {
        //                            //                lexic.NextToken();
        //                            //                jumpTokens--;
        //                            //            }
        //                            //        }
        //                            //    }
        //                            //    break;
        //                            #endregion
        //                        }

        //                        switch (mathOperator)
        //                        {
        //                            case "+":
        //                                result += nextValue;
        //                                break;
        //                            case "-":
        //                                result -= nextValue;
        //                                break;
        //                            case "*":
        //                                result *= nextValue;
        //                                break;
        //                            case "/":
        //                                result /= nextValue;
        //                                break;
        //                        }
        //                    }
        //                    break;
        //            }
        //        }
        //    } while (token != null && !errorFound);

        //    return result;
        //}

        //private string GetParenthesis(string expression, Token token)
        //{
        //    string parenthesis = "";
        //    int openParenthesisCount = 0;
        //    int stringPosition = token.StringPosition;
        //    int size = 0;

        //    for (int i = stringPosition + 1; i < expression.Length; i++)
        //    {
        //        if (expression[i] == ')')
        //        {
        //            if (openParenthesisCount > 0)
        //            {
        //                openParenthesisCount--;
        //            }
        //            else
        //            {
        //                size = i;
        //                break;
        //            }
        //        }

        //        if (expression[i] == '(')
        //        {
        //            openParenthesisCount++;
        //        }
        //    }

        //    parenthesis = expression.Substring(stringPosition + 1, size - stringPosition - 1);

        //    //Retorna a expressão dentro dos parênteses, excluindo os parênteses externos
        //    return parenthesis;
        //}
#endregion
    }
}