namespace TinyBASICAnalizer.View
{
    partial class ActualGrammarWindow
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
            this.textBoxGrammar = new System.Windows.Forms.TextBox();
            this.labelGrammar = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxGrammar
            // 
            this.textBoxGrammar.Cursor = System.Windows.Forms.Cursors.Default;
            this.textBoxGrammar.Location = new System.Drawing.Point(12, 39);
            this.textBoxGrammar.Multiline = true;
            this.textBoxGrammar.Name = "textBoxGrammar";
            this.textBoxGrammar.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.textBoxGrammar.Size = new System.Drawing.Size(566, 390);
            this.textBoxGrammar.TabIndex = 0;
            // 
            // labelGrammar
            // 
            this.labelGrammar.AutoSize = true;
            this.labelGrammar.Location = new System.Drawing.Point(9, 15);
            this.labelGrammar.Name = "labelGrammar";
            this.labelGrammar.Size = new System.Drawing.Size(101, 13);
            this.labelGrammar.TabIndex = 1;
            this.labelGrammar.Text = "Tiny Basic Grammar";
            // 
            // ActualGrammarWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 441);
            this.Controls.Add(this.labelGrammar);
            this.Controls.Add(this.textBoxGrammar);
            this.Name = "ActualGrammarWindow";
            this.Text = "GrammarWindow";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxGrammar;
        private System.Windows.Forms.Label labelGrammar;
    }
}