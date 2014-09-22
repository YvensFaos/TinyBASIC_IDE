using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyBASICAnalizer.Model
{
    public class Token
    {
        private string _value;
        /// <summary>
        /// O valor do Token em texto
        /// </summary>
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        private TokenType _type;
        /// <summary>
        /// O tipo do Token no formato enum TokenType
        /// </summary>
        public TokenType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private int _stringPosition;
        /// <summary>
        /// Posição na string do token
        /// </summary>
        public int StringPosition
        {
            get { return _stringPosition; }
            set { _stringPosition = value; }
        }

        public Token(string Value, TokenType Type)
        {
            this.Value = Value;
            this.Type = Type;
        }

        /// <summary>
        /// Retorna a lista com todas as palavras reservadas da linguagem
        /// </summary>
        /// <returns></returns>
        public static List<string> GetReservedWords()
        {
            /*
                PRINT
                INPUT
                LET
                GOTO
                GOSUB
                RETURN
                IF
                THEN
                REM
                CLEAR
                RUN
                LIST
                RND
             * */

            List<string> reservedWordList = new List<string>();

            reservedWordList.Add("PRINT");
            reservedWordList.Add("INPUT");
            reservedWordList.Add("LET");
            reservedWordList.Add("GOTO");
            reservedWordList.Add("GOSUB");
            reservedWordList.Add("RETURN");
            reservedWordList.Add("IF");
            reservedWordList.Add("THEN");
            reservedWordList.Add("REM");
            reservedWordList.Add("CLEAR");
            reservedWordList.Add("RUN");
            reservedWordList.Add("LIST");
            reservedWordList.Add("RND");

            return reservedWordList;
        }

        /// <summary>
        /// Retorna o tipo que o caractere pertence\n
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public static TokenType GetType(char character)
        {
            return GetType(character.ToString());
        }

        /// <summary>
        /// Retorna o tipo que a palavra pertence\n
        /// Verificação é apenas para a ultima posição da string
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static TokenType GetType(string word)
        {
            if (IsReservedWord(word))
            {
                return TokenType.RESERVED_WORD;
            }
            if (IsVariable(word))
            {
                return TokenType.VARIABLE;
            }
            if (IsMathOperator(word))
            {
                return TokenType.MATH_OPERATOR;
            }
            if (IsValue(word))
            {
                return TokenType.VALUE;
            }
            if (IsSeparator(word))
            {
                return TokenType.SEPARATOR;
            }
            if (IsRelationalOperator(word))
            {
                return TokenType.RELATIONAL_OPERATOR;
            }
            if (IsStringDelimiter(word))
            {
                return TokenType.STRING_DELIMITER;
            } 
            if (IsString(word))
            {
                return TokenType.STRING;
            }
            if (IsInitial(word))
            {
                return TokenType.INITIAL;
            }
            return TokenType.INVALID;
        }

        #region Verificadores
        /// <summary>
        /// Retorna se a palavra passada é uma palavra reservada
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool IsReservedWord(string word)
        {
            return GetReservedWords().Contains(word);
        }

        /// <summary>
        /// Retorna se a palavra é igual a + ou - 
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool IsSignal(string word)
        {
            return word.Equals("+") || word.Equals("-");
        }

        /// <summary>
        /// Retorna se a palavra é igual a + - / ou *
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool IsMathOperator(string word)
        {
            return IsSignal(word) || word.Equals("/") || word.Equals("*");
        }

        /// <summary>
        /// Retorna se a palavra é um dígito de 0 a 9\n
        /// Verificação é apenas para a ultima posição da string
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool IsValue(string word)
        {
            return char.IsDigit(word[word.Length - 1]);
        }

        /// <summary>
        /// Retorna se a palavra é igual a , ; ( ou )
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool IsSeparator(string word)
        {
            return word.Equals(",") || word.Equals(";") || word.Equals("(") || word.Equals(")");
        }

        /// <summary>
        /// Retorna se a palavra é igual a < > ou =
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool IsRelationalOperator(string word)
        {
            return word.Equals("<") || word.Equals(">") || word.Equals("=") || word.Equals("<=") || word.Equals("=>") || word.Equals("<>") || word.Equals("><");
        }

        /// <summary>
        /// Retorna se a palavra é uma letra de A a Z (maiúscula)\n
        /// Verificação é apenas para a ultima posição da string
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool IsVariable(string word)
        {
            if (word.Length > 1)
            {                
                return false;
            }
            return char.IsLetter(word[word.Length - 1]) && char.IsUpper(word[word.Length - 1]);
        }

        /// <summary>
        /// Retorna se a palavra é aspas
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool IsStringDelimiter(string word)
        {

            return word.Equals("\"");
        }

        /// <summary>
        /// Retorna se a palavra é uma string
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool IsString(string word)
        {            
            
            return (((word.IndexOf("\""))==0) && ((word.LastIndexOf("\""))==(word.Length-1)));
        }

        /// <summary>
        /// Retorna se a palavra é o símbolo inicial $
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool IsInitial(string word)
        {
            return word.Equals("$");
        }
#endregion

        public override string ToString()
        {
            return "(" + Value + ") Tipo: " + Type.ToString();
        }

        /// <summary>
        /// Realiza a comparação entre dois tokens verificando se os seus valores\n
        /// para o Value e o Type são iguais.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Token token = (Token)obj;

            return token.Value.Equals(Value) && token.Type == Type;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
