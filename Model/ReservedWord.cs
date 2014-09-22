using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyBASICAnalizer.Model
{
    /// <summary>
    /// Enum para os tipos de palavra reservada da linguagem
    /// </summary>
    public enum ReservedWord
    {
        PRINT,
        INPUT,
        LET,
        GOTO,
        GOSUB,
        RETURN,
        IF,
        THEN,
        REM,
        CLEAR,
        RUN,
        LIST,
        RND,
        INVALID
    }

    public class ReservedWordUtils
    {
        /// <summary>
        /// Método de equals para comparar o valor do ReservedWord com string ou outro ReservedWord
        /// </summary>
        /// <param name="word"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Equals(ReservedWord word, object obj)
        {
            if (obj is ReservedWord)
            {
                ReservedWord comparedWord = (ReservedWord)obj;
                return word.Equals(comparedWord);
            }

            if (obj is string)
            {
                string comparedWord = (string)obj;
                return word.ToString().Equals(comparedWord);
            }

            return false;
        }

        /// <summary>
        /// Método para adquirir o valor Enum equivalente
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static ReservedWord GetReservedEnum(string word)
        {
            try
            {
                return (ReservedWord)Enum.Parse(typeof(ReservedWord), word);
            }
            catch (ArgumentException)
            {
                return ReservedWord.INVALID;
            }
        }
    }
}
