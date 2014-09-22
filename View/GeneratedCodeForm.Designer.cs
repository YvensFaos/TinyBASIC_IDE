namespace TinyBASICAnalizer.View
{
    partial class GeneratedCodeForm
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
            this.labelGrammar = new System.Windows.Forms.Label();
            this.textBoxCode = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // labelGrammar
            // 
            this.labelGrammar.AutoSize = true;
            this.labelGrammar.Location = new System.Drawing.Point(12, 9);
            this.labelGrammar.Name = "labelGrammar";
            this.labelGrammar.Size = new System.Drawing.Size(55, 13);
            this.labelGrammar.TabIndex = 2;
            this.labelGrammar.Text = "Built Code";
            // 
            // textBoxCode
            // 
            this.textBoxCode.Cursor = System.Windows.Forms.Cursors.Default;
            this.textBoxCode.Location = new System.Drawing.Point(15, 35);
            this.textBoxCode.Multiline = true;
            this.textBoxCode.Name = "textBoxCode";
            this.textBoxCode.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxCode.Size = new System.Drawing.Size(368, 403);
            this.textBoxCode.TabIndex = 3;
            // 
            // GeneratedCodeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(395, 450);
            this.Controls.Add(this.textBoxCode);
            this.Controls.Add(this.labelGrammar);
            this.Name = "GeneratedCodeForm";
            this.Text = "GeneratedCodeForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelGrammar;
        private System.Windows.Forms.TextBox textBoxCode;
    }
}