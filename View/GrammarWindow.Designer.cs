namespace TinyBASICAnalizer.View
{
    partial class GrammarWindow
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
            this.label1 = new System.Windows.Forms.Label();
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Tiny Basic Grammar";
            // 
            // GrammarWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 441);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxGrammar);
            this.Name = "GrammarWindow";
            this.Text = "GrammarWindow";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxGrammar;
        private System.Windows.Forms.Label label1;
    }
}