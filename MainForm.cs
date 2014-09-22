using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TinyBASICAnalizer.ADS;
using TinyBASICAnalizer.Control;
using TinyBASICAnalizer.Model;
using TinyBASICAnalizer.Persistence;
using TinyBASICAnalizer.View;

namespace TinyBASICAnalizer
{
    public partial class MainForm : Form
    {
        private int tabCounter;
        private bool turnOnLexic;

        private SyntaxAnalizerType syntaxAnalizerType;

        private ToolStripControlHost toolStripRD;
        private ToolStripControlHost toolStripNRD;
        private ToolStripControlHost toolStripA;

        //private ToolStripControlHost toolStripGrammarIndex;

        private RadioButton radioRD;
        private RadioButton radioNRD;
        private RadioButton radioA;
        //private TextBox txtBoxGrammarIndex;

        private bool syntaxAnalyzeSuccess;

        public MainForm()
        {
            InitializeComponent();

            FileHandlerHash = new Dictionary<int, FileHandlerForm>();

            tabCounter = tabAdd.TabIndex;
            turnOnLexic = true;

            FileHandlerForm fileHandler = new FileHandlerForm();
            tabDefaultNew.Controls.Add(fileHandler);
            FileHandlerHash.Add(tabDefaultNew.TabIndex, fileHandler);

            Text = "TinyBASIC Analyzer";

            Lexic = new LexicAnalizer();

            radioRD = new RadioButton();
            radioRD.Name = "syntaxType";
            radioRD.Text = "Recursive Descendent";
            toolStripRD = new ToolStripControlHost(radioRD);

            radioNRD = new RadioButton();
            radioNRD.Name = "syntaxType";
            radioNRD.Text = "Non Recursive Descendent";
            toolStripNRD = new ToolStripControlHost(radioNRD);

            radioA = new RadioButton();
            radioA.Name = "syntaxType";
            radioA.Text = "Ascendent";
            radioA.Checked = true;
            toolStripA = new ToolStripControlHost(radioA);

            syntaxAnalizeTypeToolStripMenuItem.DropDownItems.Add(toolStripRD);
            syntaxAnalizeTypeToolStripMenuItem.DropDownItems.Add(toolStripNRD);
            syntaxAnalizeTypeToolStripMenuItem.DropDownItems.Add(toolStripA);

        }

        private Dictionary<int, FileHandlerForm> _fileHandlerHash;
        /// <summary>
        /// Hash dos formulários de código contidos nas abas
        /// </summary>
        public Dictionary<int, FileHandlerForm> FileHandlerHash
        {
            get { return _fileHandlerHash; }
            set { _fileHandlerHash = value; }
        }
        
        private LexicAnalizer _lexic;
        /// <summary>
        /// Analizador léxico da classe principal
        /// </summary>
        public LexicAnalizer Lexic
        {
            get { return _lexic; }
            set { _lexic = value; }
        }

        private SyntaxAnalizer _syntax;
        /// <summary>
        /// Analizador sintático da classe principal
        /// </summary>
        public SyntaxAnalizer Syntax
        {
            get { return _syntax; }
            set { _syntax = value; }
        }

        /// <summary>
        /// Responsável pelas ações quando é aberto uma nova aba
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabEvent(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == tabAdd.TabIndex)
            {
                CreateTab();
            }
        }

        private void CreateTab()
        {
            CreateTab("Novo (" + (tabCounter) + ")", null);
        }

        private void CreateTab(string title, string[] code)
        {
            int newIndex = tabAdd.TabIndex;
            tabAdd.TabIndex++;

            TabPage tabNewTab = new TabPage();
            tabNewTab.Location = new System.Drawing.Point(4, 22);
            tabNewTab.Name = "tabPage" + tabCounter++;
            tabNewTab.Padding = new System.Windows.Forms.Padding(3);
            tabNewTab.Size = new System.Drawing.Size(444, 375);
            tabNewTab.TabIndex = newIndex;
            tabNewTab.Text = title;
            tabNewTab.UseVisualStyleBackColor = true;

            FileHandlerForm fileHandler = null;

            if (code == null)
            {
                fileHandler = new FileHandlerForm();
            }
            else
            {
                fileHandler = new FileHandlerForm(code);
            }

            tabNewTab.Controls.Add(fileHandler);
            FileHandlerHash.Add(tabNewTab.TabIndex, fileHandler);

            tabControl.TabPages.Insert(newIndex, tabNewTab);
            tabControl.SelectedTab = tabNewTab;
        }

        #region Botões
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateTab();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = IOUtils.OpenFileDialogShow(this);
            string[] file = IOUtils.ReadTxtFile(path);
            string fileName = Path.GetFileName(path);

            CreateTab(fileName, file);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = IOUtils.SaveFileDialogShow(this, IOUtils.TXT_FILTER);
            int index = tabControl.SelectedIndex;
            FileHandlerForm fileHandler = FileHandlerHash[index];

            IOUtils.CreateTxtFile(path, fileHandler.GetText());

            tabControl.TabPages[index].Text = Path.GetFileName(path);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lexicAnalizerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = tabControl.SelectedIndex;
            FileHandlerForm fileHandler = FileHandlerHash[index];

            Lexic.File = fileHandler.GetTextLines();
            List<Token> list = Lexic.ListToken;

            if (list != null)
            {
                new LexicAnalizerForm(tabControl.TabPages[index].Text, list).Show();
            }
        }

        private void tinyBasicGrammarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new GrammarWindow().Show();
        }

        private void selectedGrammarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ActualGrammarWindow().Show();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void aFNDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = IOUtils.SaveFileDialogShow(this, IOUtils.IMAGE_FILTER);
            if (GraphicAutomata.AFNDEtoPNG(path))
            {
                MessageBox.Show("AFNDE gerado com sucesso!");
            }
        }

        private void aFDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = IOUtils.SaveFileDialogShow(this, IOUtils.IMAGE_FILTER);
            if (GraphicAutomata.AFDtoPNG(path))
            {
                MessageBox.Show("AFD gerado com sucesso!");
            }
        }

        private void syntaxAnalyzerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = tabControl.SelectedIndex;
            FileHandlerForm fileHandler = FileHandlerHash[index];

            Lexic.File = fileHandler.GetTextLines();

            if ((Lexic.File != null)&&(Lexic.File.Length>0))
            {

                if (radioA.Checked)
                {
                    syntaxAnalizerType = SyntaxAnalizerType.ASCENDENT;
                }
                else if (radioRD.Checked)
                {
                    syntaxAnalizerType = SyntaxAnalizerType.RECURSIVE_DESCENDENT;
                }
                else if (radioNRD.Checked)
                {
                    syntaxAnalizerType = SyntaxAnalizerType.NON_RECURSIVE_DESCENDENT;
                }

                //Chama um analisador sintático do tipo setado, por default o tipo é Descendente Recursivo
                Syntax = new SyntaxAnalizer(Lexic, syntaxAnalizerType);

                string feedback = "";
                syntaxAnalyzeSuccess = Syntax.Analize(ref feedback);

                string msg = "";
                string title = "";
                if (syntaxAnalyzeSuccess)
                {
                    msg = "Sucesso na Análise Sintática!";
                    title = "Sucesso";
                }
                else
                {
                    msg = "Erro na Análise Sintática, feedback: \r\n" + feedback;
                    title = "Erro";
                }

                MessageBox.Show(msg, title);
            }
            
        }

        private void firstsAndFollowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FirstAndFollowForm().Show();
        }

        private void changeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ChangeIndex().Show();
        }

        private void switchLexicAnalizerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string text = "\n\rExclusivo para Análise Sintática Ascendente.";
            if (turnOnLexic)
            {
                MessageBox.Show("Analisador léxico desligado!" + text, "Análise Léxica");
            }
            else
            {
                MessageBox.Show("Analisador léxico ligado!" + text, "Análise Léxica");
            }
            turnOnLexic = !turnOnLexic;
        }

        private void buildToolStripMenuItem_Click(object sender, EventArgs e)
        {
            syntaxAnalyzerToolStripMenuItem_Click(sender, e);

            //Só manda compilar se der sucesso na análise sintática
            if (syntaxAnalyzeSuccess)
            {
                int index = tabControl.SelectedIndex;
                FileHandlerForm fileHandler = FileHandlerHash[index];

                string[] file = fileHandler.GetTextLines();
                if (file != null)
                {
                    CodeGenerator generator = new CodeGenerator(file, syntaxAnalizerType);

                    string[] code = generator.GenerateCode();

                    new GeneratedCodeForm(code).Show();
                }
            }
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            syntaxAnalyzerToolStripMenuItem_Click(sender, e);

            //Só manda compilar se der sucesso na análise sintática
            if (syntaxAnalyzeSuccess)
            {
                int index = tabControl.SelectedIndex;
                FileHandlerForm fileHandler = FileHandlerHash[index];

                string[] file = fileHandler.GetTextLines();
                if (file != null)
                {
                    CodeGenerator generator = new CodeGenerator(file, syntaxAnalizerType);

                    VirtualMachine virtualMachine = new VirtualMachine(generator.CodeByteStream());

                    string[] output = virtualMachine.Result();

                    new GeneratedCodeForm(output).Show();
                }
            }
        }
        #endregion

    }
}
