using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TinyBASICAnalizer.Control;
using TinyBASICAnalizer.Model;

namespace TinyBASICAnalizer.View
{
    public partial class FirstAndFollowForm : Form
    {
        private List<NonTerminal> _nonTerminalList;
        private Dictionary<string, NonTerminal> _nonTerminalHash;

        public FirstAndFollowForm()
        {
            InitializeComponent();

            //Carregar combobox
            NonRecursiveDealer nonRecursive = new NonRecursiveDealer();
            _nonTerminalHash = nonRecursive.NonTerminalHash;
            _nonTerminalList = nonRecursive.NonTerminals;

            comboBoxNonTerminals.Items.Clear();
            foreach(NonTerminal nonTerminal in _nonTerminalList)
            {
                comboBoxNonTerminals.Items.Add(nonTerminal);
            }

            comboBoxNonTerminals.SelectedIndexChanged += new System.EventHandler(this.comboBoxNonTerminals_SelectedIndexChanged);
        }

        private void comboBoxNonTerminals_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedItem = comboBoxNonTerminals.SelectedItem.ToString();
            if (!labelSelected.Text.Equals(selectedItem))
            {
                if (_nonTerminalHash.ContainsKey(selectedItem))
                {
                    NonTerminal nonTerminal = _nonTerminalHash[selectedItem];

                    textBoxFirsts.Text = "";
                    List<First> firsts = nonTerminal.Firsts;
                    foreach (First firstElement in firsts)
                    {
                        textBoxFirsts.Text += firstElement.FirstValue.ToString() + " Regra: " + nonTerminal.Value + "::= " + firstElement.Rule + "\r\n";
                    }

                    textBoxFollows.Text = "";
                    List<Terminal> follows = nonTerminal.Follows;
                    foreach (Terminal terminal in follows)
                    {
                        textBoxFollows.Text += terminal.ToString() + "\r\n";
                    }

                    labelSelected.Text = nonTerminal.ToString();
                }
                else
                {
                    MessageBox.Show("Identificador de Não Terminal inválido");
                }
            }
        }
    }
}
