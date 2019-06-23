namespace QRCodeDiag.UserInterface
{
    partial class CodeSymbolCodeOptionsItem
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
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.stringValueOptionsItem1 = new QRCodeDiag.UserInterface.StringValueOptionsItem();
            this.SuspendLayout();
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(7, 57);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(120, 17);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "Draw Code Element";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(174, 57);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(120, 17);
            this.checkBox2.TabIndex = 2;
            this.checkBox2.Text = "Hide Symbol Values";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // stringValueOptionsItem1
            // 
            this.stringValueOptionsItem1.Location = new System.Drawing.Point(7, 3);
            this.stringValueOptionsItem1.Name = "stringValueOptionsItem1";
            this.stringValueOptionsItem1.Size = new System.Drawing.Size(287, 48);
            this.stringValueOptionsItem1.StringValue = "";
            this.stringValueOptionsItem1.TabIndex = 3;
            this.stringValueOptionsItem1.ValueName = "Name of the Value";
            // 
            // CodeSymbolCodeOptionsItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.stringValueOptionsItem1);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.MaximumSize = new System.Drawing.Size(303, 84);
            this.MinimumSize = new System.Drawing.Size(303, 84);
            this.Name = "CodeSymbolCodeOptionsItem";
            this.Size = new System.Drawing.Size(303, 84);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private StringValueOptionsItem stringValueOptionsItem1;
    }
}
