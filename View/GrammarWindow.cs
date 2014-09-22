using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TinyBASICAnalizer.Persistence;

namespace TinyBASICAnalizer.View
{
    public partial class GrammarWindow : Form
    {
        private static string _grammarPath = "View/View Files/grammar.txt";

        public GrammarWindow()
        {
            InitializeComponent();

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
