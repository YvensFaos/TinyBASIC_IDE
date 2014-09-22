using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyBASICAnalizer.Model.Code
{
    /// <summary>
    /// Classe para representar uma variável no gerador de código
    /// </summary>
    public class Variable
    {
        private int _value;
        /// <summary>
        /// Valor carregado pela variável
        /// </summary>
        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }

        private string _name;
        /// <summary>
        /// Nome da variável
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private int _codePosition;
        /// <summary>
        /// Indica em que posição da memória a variável está declarada
        /// </summary>
        public int CodePosition
        {
            get { return _codePosition; }
            set { _codePosition = value; }
        }

        public Variable(string Name)
            : this(Name, 0)
        { }

        public Variable(string Name, int Value)
        {
            this.Name = Name;
            this.Value = Value;
        }
    }
}
