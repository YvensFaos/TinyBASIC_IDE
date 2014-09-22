using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyBASICAnalizer.Model;

namespace TinyBASICAnalizer.Control
{
    public class Transducer
    {
        /// <summary>
        /// Realiza a leitura de uma entrada em texto e retorna os tokens do mesmo.\n
        /// Pode lançar a exceção de token inválido (a ser estudado).
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<Token> getTokens(String input)
        {
            return getTokens(new String[] { input });
        }

        /// <summary>
        /// Realiza a leitura de uma entrada em texto e retorna os tokens do mesmo.\n
        /// Pode lançar a exceção de token inválido (a ser estudado).
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<Token> getTokens(String[] input)
        {
            List<Token> list = new List<Token>();

            //Cada index do input corresponde a uma linha do arquivo

            TokenType type;
            string buffer;

            #region Main Transducer Loop
            for (int i = 0; i < input.Length; i++)
            {
                
               
                buffer = "";
                type = TokenType.INVALID;

                string line = input[i];

                int state = 0;

                int j = 0;
                bool running = true;
                while (running)
                {
                    switch (state)
                    {
                        case 0:
                            {
                                if (j == line.Length)
                                {
                                    running = false;
                                }
                                else if (line[j] == ' ')
                                {
                                    if (buffer.Length == 0)
                                    {
                                        j++;
                                    }
                                    else
                                    {
                                        //O buffer já possui valor que deve ser armazenado na lista de Tokens
                                        list.Add(new Token(buffer, type));
                                        buffer = "";
                                        type = TokenType.INVALID;
                                    }
                                }
                                else
                                {
                                    switch (Token.GetType(line[j]))
                                    {
                                        case TokenType.INVALID:
                                            {
                                                list.Add(new Token(line[j].ToString(), TokenType.INVALID));
                                                j++;
                                                break;
                                            }
                                        case TokenType.MATH_OPERATOR:
                                            {
                                                list.Add(new Token(line[j].ToString(), TokenType.MATH_OPERATOR));
                                                j++;
                                                break;
                                            }
                                        case TokenType.SEPARATOR:
                                            {
                                                Token token = new Token(line[j].ToString(), TokenType.SEPARATOR);
                                                token.StringPosition = j;
                                                list.Add(token);
                                                j++;
                                                break;
                                            }
                                        case TokenType.RELATIONAL_OPERATOR:
                                            {
                                                switch (line[j])
                                                {
                                                    case '<':
                                                        buffer += line[j++];
                                                        state = 1;
                                                        type = TokenType.RELATIONAL_OPERATOR;
                                                        break;
                                                    case '>':
                                                        buffer += line[j++];
                                                        state = 2;
                                                        type = TokenType.RELATIONAL_OPERATOR;
                                                        break;
                                                    case '=':
                                                        list.Add(new Token('='.ToString(), TokenType.RELATIONAL_OPERATOR));
                                                        j++;
                                                        break;
                                                }
                                                break;
                                            }
                                        case TokenType.VALUE:
                                            {
                                                buffer += line[j++];
                                                state = 3;
                                                type = TokenType.VALUE;
                                                break;
                                            }
                                        case TokenType.VARIABLE:
                                            {
                                                buffer += line[j++];
                                                state = 4;
                                                type = TokenType.VARIABLE;
                                                break;
                                            }

                                        case TokenType.STRING_DELIMITER:
                                            {
                                               
                                                buffer += line[j++];
                                                state = 5;                                                                              
                                                break;
                                                
                                            }
                                        case TokenType.INITIAL:
                                            {
                                                list.Add(new Token(line[j].ToString(), TokenType.INITIAL));
                                                j++;
                                                break;
                                            }
                                    }
                                }
                                break;
                            }
                        case 1: 
                            {
                                //No momento, o buffer carrega somente o valor '<'
                                if (j == line.Length)
                                {
                                    running = false;
                                }
                                else if (line[j] == ' ')
                                {
                                    //Caso seja lido ' '
                                    list.Add(new Token(buffer, TokenType.RELATIONAL_OPERATOR));
                                    buffer = "";
                                    type = TokenType.INVALID;
                                    state = 0;
                                }
                                else
                                {
                                    switch (line[j])
                                    {
                                        case '>':
                                            goto case '=';
                                        case '=':
                                            //A formação é <= ou <>
                                            buffer += line[j++];
                                            list.Add(new Token(buffer, TokenType.RELATIONAL_OPERATOR));
                                            buffer = "";
                                            type = TokenType.INVALID;
                                            state = 0;
                                            break;
                                        default:
                                            //Quer dizer que o próximo elemento não é um operador relacional, então devemos voltar ao estado 0
                                            list.Add(new Token(buffer, TokenType.RELATIONAL_OPERATOR));
                                            buffer = "";
                                            type = TokenType.INVALID;
                                            state = 0;
                                            break;
                                    }
                                }
                                break; 
                            }
                        case 2: 
                            {
                                //No momento, o buffer carrega somente o valor '>'
                                if (j == line.Length)
                                {
                                    running = false;
                                }
                                else if (line[j] == ' ')
                                {
                                    //Caso seja lido ' '
                                    list.Add(new Token(buffer, TokenType.RELATIONAL_OPERATOR));
                                    buffer = "";
                                    type = TokenType.INVALID;
                                    state = 0;
                                }
                                else
                                {
                                    switch (line[j])
                                    {
                                        case '<':
                                            goto case '=';
                                        case '=':
                                            //A formação é >= ou ><
                                            buffer += line[j++];
                                            list.Add(new Token(buffer, TokenType.RELATIONAL_OPERATOR));
                                            buffer = "";
                                            type = TokenType.INVALID;
                                            state = 0;
                                            break;
                                        default:
                                            //Quer dizer que o próximo elemento não é um operador relacional, então devemos voltar ao estado 0
                                            list.Add(new Token(buffer, TokenType.RELATIONAL_OPERATOR));
                                            buffer = "";
                                            type = TokenType.INVALID;
                                            state = 0;
                                            break;
                                    }
                                }
                                break; 
                            }
                        case 3: 
                            { 
                                //Caso tenha lido algum valor numérico
                                if (j == line.Length)
                                {
                                    running = false;
                                }
                                else if (line[j] == ' ')
                                {
                                    //Caso seja lido ' '
                                    list.Add(new Token(buffer, TokenType.VALUE));
                                    buffer = "";
                                    type = TokenType.INVALID;
                                    state = 0;
                                }
                                else
                                {
                                    //Se o próximo elemento é mais um valor numérico
                                    if (Token.GetType(line[j]) == TokenType.VALUE)
                                    {
                                        buffer += line[j++];
                                    }
                                    else
                                    {
                                        //Caso não seja, o valor guardado no buffer é armazenado e continuamos a partir do estado 0
                                        list.Add(new Token(buffer, TokenType.VALUE));
                                        buffer = "";
                                        type = TokenType.INVALID;
                                        state = 0;
                                    }
                                }
                                break; 
                            }
                        case 4: 
                            {
                                //Caso tenha lido algum valor numérico
                                if (j == line.Length)
                                {
                                    running = false;
                                }
                                else if (line[j] == ' ')
                                {
                                    //Caso seja lido ' '
                                    type = Token.GetType(buffer);
                                    list.Add(new Token(buffer, type));
                                    buffer = "";
                                    type = TokenType.INVALID;
                                    state = 0;
                                }
                                else
                                {
                                    if (Token.GetType(line[j]) == TokenType.VARIABLE)
                                    {
                                        buffer += line[j++];                                        
                                    }
                                    else
                                    {
                                        //Caso não seja, o valor guardado no buffer é armazenado e continuamos a partir do estado 0
                                        
                                        type = Token.GetType(buffer);
                                        list.Add(new Token(buffer, type));
                                        buffer = "";
                                        type = TokenType.INVALID;
                                        state = 0;
                                    }
                                }
                                break; 
                            }
                        case 5:
                            {
                                //Reconhecer strings
                                while (j < line.Length)
                                {                      

                                     if (Token.GetType(line[j]) == TokenType.STRING_DELIMITER)
                                     {
                                         buffer += line[j++];
                                         break;
                                     }

                                     buffer += line[j++];
                                    
                                }

                                type = Token.GetType(buffer);
                                list.Add(new Token(buffer, type));
                                buffer = "";
                                type = TokenType.INVALID;
                                state = 0;

                                break;
                            }
                    }
                }

                if (buffer.Length != 0)
                {
                    list.Add(new Token(buffer, Token.GetType(buffer)));
                }

                //Adicionado para tirar o null e evitar erro de nullpointer exception ao comparar o token com outro
                list.Add(new Token("\n", TokenType.LINE_BREAK));
            }
            #endregion

            
            return list;
        }
    }
}
