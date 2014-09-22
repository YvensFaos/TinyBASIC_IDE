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

namespace TinyBASICAnalizer.View
{
    public partial class LexicAnalizerForm : Form
    {
        public LexicAnalizerForm() : this(null, null) { }

        public LexicAnalizerForm(string title, List<Token> result)
        {
            InitializeComponent();

            Text = title;

            SetResult(result);
        }

        private void SetResult(List<Token> result)
        {
            string text = "";
            int line = 1;
            text += "Linha 1: \r\n\r\n";
            for (int i = 0; i < result.Count;i++)
            {
                if ((result[i].Type == TokenType.LINE_BREAK) && (i!=result.Count-1))
                {
                    line++;
                    text += "\r\nLinha " + line + ": \r\n";
                }
                else if (result[i].Type != TokenType.LINE_BREAK)
                {
                    text += result[i].ToString() + "\r\n";
                }

            }

            textBoxResult.Text = text;
        }
    }
}
