namespace TinyBASICAnalizer.View
{
    partial class FileHandlerForm
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._textBoxCode = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // _textBoxCode
            // 
            this._textBoxCode.Location = new System.Drawing.Point(3, 3);
            this._textBoxCode.Multiline = true;
            this._textBoxCode.Name = "_textBoxCode";
            this._textBoxCode.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._textBoxCode.Size = new System.Drawing.Size(438, 369);
            this._textBoxCode.TabIndex = 0;
            // 
            // FileHandlerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this._textBoxCode);
            this.Name = "FileHandlerForm";
            this.Size = new System.Drawing.Size(444, 375);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _textBoxCode;
        /// <summary>
        /// TextBox que contém o código do form
        /// </summary>
        public System.Windows.Forms.TextBox TextBoxCode
        {
            get { return _textBoxCode; }
            set { _textBoxCode = value; }
        }
    }
}
