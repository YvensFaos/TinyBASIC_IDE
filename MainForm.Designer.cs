namespace TinyBASICAnalizer
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configurationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.syntaxAnalizeTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setGrammarIndexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.switchLexicAnalizerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.analizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lexicAnalizerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.syntaxAnalyzerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.firstsAndFollowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectedGrammarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tinyBasicGrammarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aFNDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aFDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabDefaultNew = new System.Windows.Forms.TabPage();
            this.tabAdd = new System.Windows.Forms.TabPage();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 454);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(484, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.configurationsToolStripMenuItem,
            this.compileToolStripMenuItem,
            this.analizeToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(484, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // configurationsToolStripMenuItem
            // 
            this.configurationsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.syntaxAnalizeTypeToolStripMenuItem,
            this.setGrammarIndexToolStripMenuItem,
            this.switchLexicAnalizerToolStripMenuItem});
            this.configurationsToolStripMenuItem.Name = "configurationsToolStripMenuItem";
            this.configurationsToolStripMenuItem.Size = new System.Drawing.Size(98, 20);
            this.configurationsToolStripMenuItem.Text = "Configurations";
            // 
            // syntaxAnalizeTypeToolStripMenuItem
            // 
            this.syntaxAnalizeTypeToolStripMenuItem.Name = "syntaxAnalizeTypeToolStripMenuItem";
            this.syntaxAnalizeTypeToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.syntaxAnalizeTypeToolStripMenuItem.Text = "Syntax Analyze Type";
            // 
            // setGrammarIndexToolStripMenuItem
            // 
            this.setGrammarIndexToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeToolStripMenuItem});
            this.setGrammarIndexToolStripMenuItem.Name = "setGrammarIndexToolStripMenuItem";
            this.setGrammarIndexToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.setGrammarIndexToolStripMenuItem.Text = "Set Grammar Index";
            // 
            // changeToolStripMenuItem
            // 
            this.changeToolStripMenuItem.Name = "changeToolStripMenuItem";
            this.changeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.changeToolStripMenuItem.Text = "Change";
            this.changeToolStripMenuItem.Click += new System.EventHandler(this.changeToolStripMenuItem_Click);
            // 
            // switchLexicAnalizerToolStripMenuItem
            // 
            this.switchLexicAnalizerToolStripMenuItem.Name = "switchLexicAnalizerToolStripMenuItem";
            this.switchLexicAnalizerToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.switchLexicAnalizerToolStripMenuItem.Text = "Switch Lexic Analizer";
            this.switchLexicAnalizerToolStripMenuItem.Click += new System.EventHandler(this.switchLexicAnalizerToolStripMenuItem_Click);
            // 
            // compileToolStripMenuItem
            // 
            this.compileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buildToolStripMenuItem,
            this.runToolStripMenuItem});
            this.compileToolStripMenuItem.Name = "compileToolStripMenuItem";
            this.compileToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
            this.compileToolStripMenuItem.Text = "Compile";
            // 
            // buildToolStripMenuItem
            // 
            this.buildToolStripMenuItem.Name = "buildToolStripMenuItem";
            this.buildToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
            this.buildToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.buildToolStripMenuItem.Text = "Build";
            this.buildToolStripMenuItem.Click += new System.EventHandler(this.buildToolStripMenuItem_Click);
            // 
            // analizeToolStripMenuItem
            // 
            this.analizeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lexicAnalizerToolStripMenuItem,
            this.syntaxAnalyzerToolStripMenuItem});
            this.analizeToolStripMenuItem.Name = "analizeToolStripMenuItem";
            this.analizeToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.analizeToolStripMenuItem.Text = "Analyze";
            // 
            // lexicAnalizerToolStripMenuItem
            // 
            this.lexicAnalizerToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
            this.lexicAnalizerToolStripMenuItem.Name = "lexicAnalizerToolStripMenuItem";
            this.lexicAnalizerToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.lexicAnalizerToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.lexicAnalizerToolStripMenuItem.Text = "Lexical Analyzer";
            this.lexicAnalizerToolStripMenuItem.Click += new System.EventHandler(this.lexicAnalizerToolStripMenuItem_Click);
            // 
            // syntaxAnalyzerToolStripMenuItem
            // 
            this.syntaxAnalyzerToolStripMenuItem.Name = "syntaxAnalyzerToolStripMenuItem";
            this.syntaxAnalyzerToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.syntaxAnalyzerToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.syntaxAnalyzerToolStripMenuItem.Text = "Syntax Analyzer";
            this.syntaxAnalyzerToolStripMenuItem.Click += new System.EventHandler(this.syntaxAnalyzerToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.firstsAndFollowsToolStripMenuItem,
            this.selectedGrammarToolStripMenuItem,
            this.tinyBasicGrammarToolStripMenuItem,
            this.generateToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // firstsAndFollowsToolStripMenuItem
            // 
            this.firstsAndFollowsToolStripMenuItem.Name = "firstsAndFollowsToolStripMenuItem";
            this.firstsAndFollowsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.firstsAndFollowsToolStripMenuItem.Text = "Firsts and Follows";
            this.firstsAndFollowsToolStripMenuItem.Click += new System.EventHandler(this.firstsAndFollowsToolStripMenuItem_Click);
            // 
            // selectedGrammarToolStripMenuItem
            // 
            this.selectedGrammarToolStripMenuItem.Name = "selectedGrammarToolStripMenuItem";
            this.selectedGrammarToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.selectedGrammarToolStripMenuItem.Text = "Selected Grammar";
            this.selectedGrammarToolStripMenuItem.Click += new System.EventHandler(this.selectedGrammarToolStripMenuItem_Click);
            // 
            // tinyBasicGrammarToolStripMenuItem
            // 
            this.tinyBasicGrammarToolStripMenuItem.Name = "tinyBasicGrammarToolStripMenuItem";
            this.tinyBasicGrammarToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.tinyBasicGrammarToolStripMenuItem.Text = "Tiny Basic Grammar";
            this.tinyBasicGrammarToolStripMenuItem.Click += new System.EventHandler(this.tinyBasicGrammarToolStripMenuItem_Click);
            // 
            // generateToolStripMenuItem
            // 
            this.generateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aFNDToolStripMenuItem,
            this.aFDToolStripMenuItem});
            this.generateToolStripMenuItem.Name = "generateToolStripMenuItem";
            this.generateToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.generateToolStripMenuItem.Text = "Generate";
            // 
            // aFNDToolStripMenuItem
            // 
            this.aFNDToolStripMenuItem.Name = "aFNDToolStripMenuItem";
            this.aFNDToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.aFNDToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.aFNDToolStripMenuItem.Text = "AFND";
            this.aFNDToolStripMenuItem.Click += new System.EventHandler(this.aFNDToolStripMenuItem_Click);
            // 
            // aFDToolStripMenuItem
            // 
            this.aFDToolStripMenuItem.Name = "aFDToolStripMenuItem";
            this.aFDToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.aFDToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.aFDToolStripMenuItem.Text = "AFD";
            this.aFDToolStripMenuItem.Click += new System.EventHandler(this.aFDToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabDefaultNew);
            this.tabControl.Controls.Add(this.tabAdd);
            this.tabControl.Location = new System.Drawing.Point(20, 35);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(452, 401);
            this.tabControl.TabIndex = 2;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.TabEvent);
            // 
            // tabDefaultNew
            // 
            this.tabDefaultNew.Location = new System.Drawing.Point(4, 22);
            this.tabDefaultNew.Name = "tabDefaultNew";
            this.tabDefaultNew.Padding = new System.Windows.Forms.Padding(3);
            this.tabDefaultNew.Size = new System.Drawing.Size(444, 375);
            this.tabDefaultNew.TabIndex = 0;
            this.tabDefaultNew.Text = "Novo";
            this.tabDefaultNew.UseVisualStyleBackColor = true;
            // 
            // tabAdd
            // 
            this.tabAdd.Location = new System.Drawing.Point(4, 22);
            this.tabAdd.Name = "tabAdd";
            this.tabAdd.Padding = new System.Windows.Forms.Padding(3);
            this.tabAdd.Size = new System.Drawing.Size(444, 375);
            this.tabAdd.TabIndex = 1;
            this.tabAdd.Text = "    +";
            this.tabAdd.UseVisualStyleBackColor = true;
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.runToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.runToolStripMenuItem.Text = "Run";
            this.runToolStripMenuItem.Click += new System.EventHandler(this.runToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 476);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Main Form";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem analizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lexicAnalizerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tinyBasicGrammarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabDefaultNew;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.TabPage tabAdd;
        private System.Windows.Forms.ToolStripMenuItem syntaxAnalyzerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem firstsAndFollowsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configurationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem syntaxAnalizeTypeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aFNDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aFDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setGrammarIndexToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectedGrammarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem switchLexicAnalizerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem buildToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
    }
}

