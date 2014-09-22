using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyBASICAnalizer.Model
{
    public class Terminal : Symbol
    {
        public static Terminal Empty = new Terminal("#");
        public static Terminal Initial = new Terminal("$");
        public static Terminal Point = new Terminal(".");
        public static Terminal FinalPoint = new Terminal("¬");

        public Terminal(string Value)
            : base(Value)
        { }

        public override string ToString()
        {
            return base.ToString();
        }

        /// <summary>
        /// Retorna verdadeiro se o terminal for equivalente ao token passado
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool EquivalentToToken(Token token)
        {
            TokenType type = TokenType.INVALID;
            if (Value.Equals("num"))
            {
                type = TokenType.VALUE;
            }
            else if (Value.Equals("string"))
            {
                type = TokenType.STRING;
            }
            else if (Value.Equals("var"))
            {
                type = TokenType.VARIABLE;
            }
            else
            {
                type = Token.GetType(Value);
            }
            return type == token.Type;
        }
    }
}
