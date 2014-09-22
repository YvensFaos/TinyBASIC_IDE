using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TinyBASICAnalizer.Model;
using TinyBASICAnalizer.Persistence;

namespace TinyBASICAnalizer.View
{
    public partial class ActualGrammarWindow : Form
    {
        private static string _grammarPath
        {
            get { return "Model/Grammar/Grammar" + Symbol.GrammarIdentifier + ".txt"; }
        }
        public ActualGrammarWindow()
        {
            InitializeComponent();

            labelGrammar.Text = "Selected grammar = " + Symbol.GrammarIdentifier;

            string[] grammar = IOUtils.ReadTxtFile(_grammarPath);
            string text = "";
            foreach (string line in grammar)
            {
                text += line + "\r\n";
            }

            textBoxGrammar.Text = text;
        }
    }
}
