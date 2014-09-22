using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TinyBASICAnalizer.Model;

namespace TinyBASICAnalizer.View
{
    public partial class ChangeIndex : Form
    {
        public ChangeIndex()
        {
            InitializeComponent();

            textBoxIndex.Text = Symbol.GrammarIdentifier;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Symbol.GrammarIdentifier = textBoxIndex.Text;

            buttonCancel_Click(sender, e);
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            textBoxIndex.Text = "";
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void textBoxIndex_keyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Symbol.GrammarIdentifier = textBoxIndex.Text;

                buttonCancel_Click(sender, e);
            }
        }
    }
}
