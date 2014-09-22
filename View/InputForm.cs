using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TinyBASICAnalizer.View
{
    public partial class InputForm : Form
    {
        private int _value;
        /// <summary>
        /// Valor setado
        /// </summary>
        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public InputForm()
        {
            InitializeComponent();
        }

        public InputForm(string variable)
        {
            InitializeComponent();
            labelText.Text += variable;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if ((textBoxInput.Text == null) || (textBoxInput.Text.Equals("")))
            {
                Value = 0;
            }
            else
            {
                Value = int.Parse(textBoxInput.Text);
            }

            this.Close();
        }

        private void textBoxInput_keyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if ((textBoxInput.Text == null)||(textBoxInput.Text.Equals("")))
                {
                    Value = 0;
                }
                else
                {
                    Value = int.Parse(textBoxInput.Text);
                }

                this.Close();
            }
        }
    }
}
