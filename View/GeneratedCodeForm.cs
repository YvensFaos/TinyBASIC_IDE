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
    public partial class GeneratedCodeForm : Form
    {
        public GeneratedCodeForm(string[] code)
        {
            InitializeComponent();

            int lineNumber = 1;
            foreach (string line in code)
            {
                textBoxCode.Text += lineNumber.ToString("X") + " " + line + "\r\n";
                lineNumber++;
            }
        }

        private GeneratedCodeForm()
            : this(new string[] { "" })
        { }
    }
}
