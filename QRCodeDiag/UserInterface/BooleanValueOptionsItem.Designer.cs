namespace QRCodeDiag.UserInterface
{
    partial class BooleanValueOptionsItem
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
            this.valueNameLabel = new System.Windows.Forms.Label();
            this.enableCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // valueNameLabel
            // 
            this.valueNameLabel.AutoSize = true;
            this.valueNameLabel.Location = new System.Drawing.Point(3, 0);
            this.valueNameLabel.Name = "valueNameLabel";
            this.valueNameLabel.Size = new System.Drawing.Size(94, 13);
            this.valueNameLabel.TabIndex = 0;
            this.valueNameLabel.Text = "Name of the value";
            // 
            // enableCheckBox
            // 
            this.enableCheckBox.AutoSize = true;
            this.enableCheckBox.Location = new System.Drawing.Point(6, 16);
            this.enableCheckBox.Name = "enableCheckBox";
            this.enableCheckBox.Size = new System.Drawing.Size(127, 17);
            this.enableCheckBox.TabIndex = 1;
            this.enableCheckBox.Text = "checkbox description";
            this.enableCheckBox.UseVisualStyleBackColor = true;
            // 
            // BooleanValueOptionsItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.enableCheckBox);
            this.Controls.Add(this.valueNameLabel);
            this.Name = "BooleanValueOptionsItem";
            this.Size = new System.Drawing.Size(287, 48);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.Label valueNameLabel;
        private System.Windows.Forms.CheckBox enableCheckBox;
    }
}
