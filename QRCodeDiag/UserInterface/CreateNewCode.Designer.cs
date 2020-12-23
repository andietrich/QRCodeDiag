namespace QRCodeDiag.UserInterface
{
    partial class CreateNewCode
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
            this.buttonCancel = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.labelVersion = new System.Windows.Forms.Label();
            this.buttonCreate = new System.Windows.Forms.Button();
            this.comboBoxECCLevel = new System.Windows.Forms.ComboBox();
            this.labelECCLevel = new System.Windows.Forms.Label();
            this.comboBoxXORMask = new System.Windows.Forms.ComboBox();
            this.labelXORMask = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(93, 143);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(15, 25);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown1.TabIndex = 1;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Location = new System.Drawing.Point(12, 9);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(42, 13);
            this.labelVersion.TabIndex = 2;
            this.labelVersion.Text = "Version";
            // 
            // buttonCreate
            // 
            this.buttonCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCreate.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonCreate.Location = new System.Drawing.Point(12, 143);
            this.buttonCreate.Name = "buttonCreate";
            this.buttonCreate.Size = new System.Drawing.Size(75, 23);
            this.buttonCreate.TabIndex = 3;
            this.buttonCreate.Text = "Create";
            this.buttonCreate.UseVisualStyleBackColor = true;
            // 
            // comboBoxECCLevel
            // 
            this.comboBoxECCLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxECCLevel.FormattingEnabled = true;
            this.comboBoxECCLevel.Items.AddRange(new object[] {
            "Low",
            "Medium",
            "Quartile",
            "High"});
            this.comboBoxECCLevel.Location = new System.Drawing.Point(15, 64);
            this.comboBoxECCLevel.Name = "comboBoxECCLevel";
            this.comboBoxECCLevel.Size = new System.Drawing.Size(121, 21);
            this.comboBoxECCLevel.TabIndex = 4;
            // 
            // labelECCLevel
            // 
            this.labelECCLevel.AutoSize = true;
            this.labelECCLevel.Location = new System.Drawing.Point(12, 48);
            this.labelECCLevel.Name = "labelECCLevel";
            this.labelECCLevel.Size = new System.Drawing.Size(53, 13);
            this.labelECCLevel.TabIndex = 5;
            this.labelECCLevel.Text = "ECC level";
            // 
            // comboBoxXORMask
            // 
            this.comboBoxXORMask.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxXORMask.FormattingEnabled = true;
            this.comboBoxXORMask.Items.AddRange(new object[] {
            "None",
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7"});
            this.comboBoxXORMask.Location = new System.Drawing.Point(15, 104);
            this.comboBoxXORMask.Name = "comboBoxXORMask";
            this.comboBoxXORMask.Size = new System.Drawing.Size(121, 21);
            this.comboBoxXORMask.TabIndex = 6;
            // 
            // labelXORMask
            // 
            this.labelXORMask.AutoSize = true;
            this.labelXORMask.Location = new System.Drawing.Point(12, 88);
            this.labelXORMask.Name = "labelXORMask";
            this.labelXORMask.Size = new System.Drawing.Size(59, 13);
            this.labelXORMask.TabIndex = 5;
            this.labelXORMask.Text = "XOR Mask";
            // 
            // CreateNewCode
            // 
            this.AcceptButton = this.buttonCreate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(198, 181);
            this.Controls.Add(this.comboBoxXORMask);
            this.Controls.Add(this.labelXORMask);
            this.Controls.Add(this.labelECCLevel);
            this.Controls.Add(this.comboBoxECCLevel);
            this.Controls.Add(this.buttonCreate);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.buttonCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateNewCode";
            this.Text = "Create new code";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Button buttonCreate;
        private System.Windows.Forms.ComboBox comboBoxECCLevel;
        private System.Windows.Forms.Label labelECCLevel;
        private System.Windows.Forms.ComboBox comboBoxXORMask;
        private System.Windows.Forms.Label labelXORMask;
    }
}