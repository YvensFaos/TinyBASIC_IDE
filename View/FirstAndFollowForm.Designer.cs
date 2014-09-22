namespace TinyBASICAnalizer.View
{
    partial class FirstAndFollowForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxNonTerminals = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxFirsts = new System.Windows.Forms.TextBox();
            this.textBoxFollows = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelSelected = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Non Terminals";
            // 
            // comboBoxNonTerminals
            // 
            this.comboBoxNonTerminals.FormattingEnabled = true;
            this.comboBoxNonTerminals.Location = new System.Drawing.Point(102, 9);
            this.comboBoxNonTerminals.Name = "comboBoxNonTerminals";
            this.comboBoxNonTerminals.Size = new System.Drawing.Size(207, 21);
            this.comboBoxNonTerminals.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Selected: ";
            // 
            // textBoxFirsts
            // 
            this.textBoxFirsts.Location = new System.Drawing.Point(12, 99);
            this.textBoxFirsts.Multiline = true;
            this.textBoxFirsts.Name = "textBoxFirsts";
            this.textBoxFirsts.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxFirsts.Size = new System.Drawing.Size(294, 105);
            this.textBoxFirsts.TabIndex = 3;
            // 
            // textBoxFollows
            // 
            this.textBoxFollows.Location = new System.Drawing.Point(12, 223);
            this.textBoxFollows.Multiline = true;
            this.textBoxFollows.Name = "textBoxFollows";
            this.textBoxFollows.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxFollows.Size = new System.Drawing.Size(294, 105);
            this.textBoxFollows.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Firsts";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 207);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Follows";
            // 
            // labelSelected
            // 
            this.labelSelected.AutoSize = true;
            this.labelSelected.Location = new System.Drawing.Point(99, 53);
            this.labelSelected.Name = "labelSelected";
            this.labelSelected.Size = new System.Drawing.Size(33, 13);
            this.labelSelected.TabIndex = 7;
            this.labelSelected.Text = "None";
            // 
            // FirstAndFollowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(321, 340);
            this.Controls.Add(this.labelSelected);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxFollows);
            this.Controls.Add(this.textBoxFirsts);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxNonTerminals);
            this.Controls.Add(this.label1);
            this.Name = "FirstAndFollowForm";
            this.Text = "FirstAndFollowForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxNonTerminals;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxFirsts;
        private System.Windows.Forms.TextBox textBoxFollows;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelSelected;
    }
}