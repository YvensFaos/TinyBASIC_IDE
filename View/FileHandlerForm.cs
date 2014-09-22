using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TinyBASICAnalizer.View
{
    public partial class FileHandlerForm : UserControl
    {
        public FileHandlerForm()
        {
            InitializeComponent();

            TextBoxCode.Text = "LET A = 12";
        }

        public FileHandlerForm(string[] code)
        {
            InitializeComponent();

            TextBoxCode.Text = ArrayToString(code);
        }

        /// <summary>
        /// Converte um array de strings para uma string só separada por \r\n
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        private string ArrayToString(string[] array)
        {
            string text = "";
            foreach (string line in array)
            {
                text += line + "\r\n";
            }

            return text;
        }

        /// <summary>
        /// Converte uma string em um array de strings separados por \n
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string[] StringToArray(string text)
        {
            //Removo os \r
            string newText = text.Replace("\r", "");
            //Faço o split somente nos \n
            string[] array = newText.Split('\n');
            return array;
        }

        /// <summary>
        /// Carrega o valor string do conteúdo da textarea
        /// </summary>
        /// <returns></returns>
        public string GetText()
        {
            return TextBoxCode.Text;
        }

        /// <summary>
        /// Carrega o valor string do conteúdo da textarea e retorna como um array de strings
        /// </summary>
        /// <returns></returns>
        public string[] GetTextLines()
        {
            if (!TextBoxCode.Text.Equals(""))
            {
                return StringToArray(TextBoxCode.Text);
            }
            else
            {
                return null;
            }
        }
    }
}
