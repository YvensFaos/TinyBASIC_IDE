using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyBASICAnalizer.Model
{
    public class First
    {
        private Terminal _firstValue;

        public Terminal FirstValue
        {
            get { return _firstValue; }
            set { _firstValue = value; }
        }

        private string _rule;

        public string Rule
        {
            get { return _rule; }
            set { _rule = value; }
        }

        public First(Terminal terminal, string rule)
        {
            FirstValue = terminal;
            Rule = rule;
        }
    }
}
