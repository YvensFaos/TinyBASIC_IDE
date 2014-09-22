using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyBASICAnalizer.Model
{
    /// <summary>
    /// Classificação do tipo de token
    /// </summary>
    public enum TokenType
    {
        MATH_OPERATOR,
        VALUE,
        SEPARATOR,
        RELATIONAL_OPERATOR,
        VARIABLE,
        RESERVED_WORD,
        STRING_DELIMITER,
        STRING,
        LINE_BREAK,
        INVALID,
        INITIAL
    }
}
