using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TinyBASICAnalizer.ADS;
using TinyBASICAnalizer.Model.Code;
using TinyBASICAnalizer.View;


namespace TinyBASICAnalizer.Control
{
    public class VirtualMachine
    {
        private byte[] _code;
        /// <summary>
        /// Código de máquina
        /// </summary>
        public byte[] Code
        {
            get { return _code; }
            set { _code = value; }
        }

        private SimpleStack<int> _memory;

        public VirtualMachine()
            : this(null)
        { }

        public VirtualMachine(byte[] code)
        {
            _code = code;
        }

        /// <summary>
        /// Retorna um vetor de string com os OUTs do código gerado
        /// </summary>
        /// <returns></returns>
        public string[] Result()
        {
            /* LET A = 12
             *  1 1
                2 128
                3 0
                4 2
                5 4
                6 0
                7 0
                8 0
                9 0
                10 0
                11 5
                12 12
                13 0
                14 17
                15 3
                16 0
                17 3
             */
            List<string> output = new List<string>();

            //Guarda o resultado verdadeiro ou falso da última comparação
            bool ifFlag = false;

            //Primeiras 3 linhas são do LSP
            int memoryNeeded = BytesToInt(Code[1], Code[2]); // (?)

            _memory = new SimpleStack<int>();
            _memory.Limit = memoryNeeded;

            //Próximas 3 linhas é o jump das variáveis
            int numberVariables = BytesToInt(Code[4], Code[5]) - 3;

            Dictionary<string, Variable> variables = new Dictionary<string, Variable>();
            for (int i = 0, f = 3; i < numberVariables; i++, f++)
            {
                variables.Add(f.ToString(), new Variable(f.ToString()));
            }

            int actualPosition = 5 + numberVariables * 2 + 1;
            int numberInputs = BytesToInt(Code[actualPosition++], Code[actualPosition]);

            //Janela de input
            for (int i = 0; i < numberInputs; i++)
            {
                InputForm input = new InputForm((i + 1).ToString());
                input.ShowDialog();
                int value = input.Value;

                int variableCode = BytesToInt(Code[++actualPosition], Code[++actualPosition]);
                Variable variable = variables[variableCode.ToString()];
                variable.Value = value;
            }

            //actualPosition += numberInputs + 1;
            actualPosition++;

            //Code
            for (; actualPosition < Code.Length; actualPosition++)
            {
                //Leitura do código
                string command = CodeGenerator.CodeToCommand(Code[actualPosition]);
                switch (command)
                {
                    case "JMP":
                        {
                            int jumpTo = BytesToInt(Code[++actualPosition], Code[++actualPosition]);
                            actualPosition = jumpTo - 2;
                        }
                        break;
                    case "STOP":
                        {
                            //Encerra a execução do código
                            actualPosition = Code.Length;
                        }
                        break;
                    case "LOD":
                        {
                            int variableCode = BytesToInt(Code[++actualPosition], Code[++actualPosition]);
                            Variable variable = variables[variableCode.ToString()];

                            _memory.Push(variable.Value);
                        }
                        break;
                    case "LDI":
                        {
                            int integerValue = BytesToInt(Code[++actualPosition], Code[++actualPosition]);

                            _memory.Push(integerValue);
                        }
                        break;
                    case "STR":
                        {
                            int numberCharacters = BytesToInt(Code[++actualPosition], Code[++actualPosition]);

                            string print = "";
                            for (int j = 0; j < numberCharacters; j++)
                            {
                                int characterInt = BytesToInt(Code[++actualPosition], Code[++actualPosition]);
                                char character = (char)characterInt;

                                print += character.ToString();
                            }

                            output.Add(print);
                        }
                        break;
                    case "EQ":
                        {
                            int first = _memory.Pop();
                            int second = _memory.Pop();

                            ifFlag = second == first;
                        }
                        break;
                    case "NE":
                        {
                            int first = _memory.Pop();
                            int second = _memory.Pop();

                            ifFlag = second != first;
                        }
                        break;
                    case "LE":
                        {
                            int first = _memory.Pop();
                            int second = _memory.Pop();

                            ifFlag = second <= first;
                        }
                        break;
                    case "LT":
                        {
                            int first = _memory.Pop();
                            int second = _memory.Pop();

                            ifFlag = second < first;
                        }
                        break;
                    case "GE":
                        {
                            int first = _memory.Pop();
                            int second = _memory.Pop();

                            ifFlag = second >= first;
                        }
                        break;
                    case "GT":
                        {
                            int first = _memory.Pop();
                            int second = _memory.Pop();

                            ifFlag = second > first;
                        }
                        break;
                    case "ADD":
                        {
                            int first = _memory.Pop();
                            int second = _memory.Pop();

                            _memory.Push(first + second);
                        }
                        break;
                    case "TIM":
                        {
                            int first = _memory.Pop();
                            int second = _memory.Pop();

                            _memory.Push(first * second);
                        }
                        break;
                    case "MIN":
                        {
                            int first = _memory.Pop();
                            int second = _memory.Pop();

                            _memory.Push(second - first);
                        }
                        break;
                    case "DIV":
                        {
                            int first = _memory.Pop();
                            int second = _memory.Pop();

                            _memory.Push(second / first);
                        }
                        break;
                    case "STO":
                        {
                            int value = _memory.Pop();

                            int variableCode = BytesToInt(Code[++actualPosition], Code[++actualPosition]);
                            Variable variable = variables[variableCode.ToString()];

                            variable.Value = value;
                        }
                        break;
                    case "OUT":
                        {
                            output.Add(_memory.Top().ToString());
                        }
                        break;
                    case "JF":
                        {
                            if (!ifFlag)
                            {
                                goto case "JMP";
                            }
                        }
                        break;
                }
            }

            //foreach (byte command in Code)
            //{
            //    output.Add(command.ToString());
            //}

            return output.ToArray();
        }

        /// <summary>
        /// Converter dois bytes para um inteiro
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        private int BytesToInt(byte first, byte second)
        {
            if (second == 0)
            {
                return (int)first;
            }
            else
            {
                return ((int)second << 8) + (int)first;
            }
        }
    }
}
