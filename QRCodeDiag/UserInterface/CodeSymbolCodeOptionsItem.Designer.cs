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
            this.checkBoxDrawElement = new System.Windows.Forms.CheckBox();
            this.checkBoxDrawValues = new System.Windows.Forms.CheckBox();
            this.stringValueOptionsItem1 = new QRCodeDiag.UserInterface.StringValueOptionsItem();
            this.checkBoxSymbolIndices = new System.Windows.Forms.CheckBox();
            this.checkBoxBitIndices = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // checkBoxDrawElement
            // 
            this.checkBoxDrawElement.AutoSize = true;
            this.checkBoxDrawElement.Location = new System.Drawing.Point(7, 57);
            this.checkBoxDrawElement.Name = "checkBoxDrawElement";
            this.checkBoxDrawElement.Size = new System.Drawing.Size(120, 17);
            this.checkBoxDrawElement.TabIndex = 1;
            this.checkBoxDrawElement.Text = "Draw Code Element";
            this.checkBoxDrawElement.UseVisualStyleBackColor = true;
            this.checkBoxDrawElement.CheckedChanged += new System.EventHandler(this.checkBoxDrawElement_CheckedChanged);
            // 
            // checkBoxDrawValues
            // 
            this.checkBoxDrawValues.AutoSize = true;
            this.checkBoxDrawValues.Location = new System.Drawing.Point(174, 57);
            this.checkBoxDrawValues.Name = "checkBoxDrawValues";
            this.checkBoxDrawValues.Size = new System.Drawing.Size(123, 17);
            this.checkBoxDrawValues.TabIndex = 2;
            this.checkBoxDrawValues.Text = "Draw Symbol Values";
            this.checkBoxDrawValues.UseVisualStyleBackColor = true;
            this.checkBoxDrawValues.CheckedChanged += new System.EventHandler(this.checkBoxDrawValues_CheckedChanged);
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
            // checkBoxSymbolIndices
            // 
            this.checkBoxSymbolIndices.AutoSize = true;
            this.checkBoxSymbolIndices.Location = new System.Drawing.Point(7, 80);
            this.checkBoxSymbolIndices.Name = "checkBoxSymbolIndices";
            this.checkBoxSymbolIndices.Size = new System.Drawing.Size(125, 17);
            this.checkBoxSymbolIndices.TabIndex = 4;
            this.checkBoxSymbolIndices.Text = "Draw Symbol Indices";
            this.checkBoxSymbolIndices.UseVisualStyleBackColor = true;
            this.checkBoxSymbolIndices.CheckedChanged += new System.EventHandler(this.checkBoxSymbolIndices_CheckedChanged);
            // 
            // checkBoxBitIndices
            // 
            this.checkBoxBitIndices.AutoSize = true;
            this.checkBoxBitIndices.Location = new System.Drawing.Point(174, 80);
            this.checkBoxBitIndices.Name = "checkBoxBitIndices";
            this.checkBoxBitIndices.Size = new System.Drawing.Size(103, 17);
            this.checkBoxBitIndices.TabIndex = 4;
            this.checkBoxBitIndices.Text = "Draw Bit Indices";
            this.checkBoxBitIndices.UseVisualStyleBackColor = true;
            this.checkBoxBitIndices.CheckedChanged += new System.EventHandler(this.checkBoxBitIndices_CheckedChanged);
            // 
            // CodeSymbolCodeOptionsItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBoxBitIndices);
            this.Controls.Add(this.checkBoxSymbolIndices);
            this.Controls.Add(this.stringValueOptionsItem1);
            this.Controls.Add(this.checkBoxDrawValues);
            this.Controls.Add(this.checkBoxDrawElement);
            this.MaximumSize = new System.Drawing.Size(303, 104);
            this.MinimumSize = new System.Drawing.Size(303, 84);
            this.Name = "CodeSymbolCodeOptionsItem";
            this.Size = new System.Drawing.Size(303, 104);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox checkBoxDrawElement;
        private System.Windows.Forms.CheckBox checkBoxDrawValues;
        private StringValueOptionsItem stringValueOptionsItem1;
        private System.Windows.Forms.CheckBox checkBoxSymbolIndices;
        private System.Windows.Forms.CheckBox checkBoxBitIndices;
    }
}
