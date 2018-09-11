namespace DADSTORM
{
    partial class Form1
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
            this.fileText = new System.Windows.Forms.TextBox();
            this.commandText = new System.Windows.Forms.TextBox();
            this.execFileButton = new System.Windows.Forms.Button();
            this.execCommandButton = new System.Windows.Forms.Button();
            this.logText = new System.Windows.Forms.TextBox();
            this.stepButton = new System.Windows.Forms.Button();
            this.resetButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // fileText
            // 
            this.fileText.AccessibleName = "FileText";
            this.fileText.Location = new System.Drawing.Point(12, 21);
            this.fileText.Name = "fileText";
            this.fileText.Size = new System.Drawing.Size(433, 20);
            this.fileText.TabIndex = 0;
            // 
            // commandText
            // 
            this.commandText.AccessibleName = "CommandText";
            this.commandText.Location = new System.Drawing.Point(12, 61);
            this.commandText.Name = "commandText";
            this.commandText.Size = new System.Drawing.Size(221, 20);
            this.commandText.TabIndex = 1;
            // 
            // execFileButton
            // 
            this.execFileButton.AccessibleName = "ExecFileButton";
            this.execFileButton.Location = new System.Drawing.Point(451, 21);
            this.execFileButton.Name = "execFileButton";
            this.execFileButton.Size = new System.Drawing.Size(112, 20);
            this.execFileButton.TabIndex = 2;
            this.execFileButton.Text = "Execute File";
            this.execFileButton.UseVisualStyleBackColor = true;
            this.execFileButton.Click += new System.EventHandler(this.execFileClick);
            // 
            // execCommandButton
            // 
            this.execCommandButton.AccessibleName = "ExecCommandButton";
            this.execCommandButton.Location = new System.Drawing.Point(253, 61);
            this.execCommandButton.Name = "execCommandButton";
            this.execCommandButton.Size = new System.Drawing.Size(112, 20);
            this.execCommandButton.TabIndex = 3;
            this.execCommandButton.Text = "Execute Command";
            this.execCommandButton.UseVisualStyleBackColor = true;
            // 
            // logText
            // 
            this.logText.AccessibleName = "LogText";
            this.logText.Location = new System.Drawing.Point(12, 98);
            this.logText.Multiline = true;
            this.logText.Name = "logText";
            this.logText.ReadOnly = true;
            this.logText.Size = new System.Drawing.Size(551, 284);
            this.logText.TabIndex = 4;
            // 
            // stepButton
            // 
            this.stepButton.Location = new System.Drawing.Point(451, 60);
            this.stepButton.Name = "stepButton";
            this.stepButton.Size = new System.Drawing.Size(112, 20);
            this.stepButton.TabIndex = 5;
            this.stepButton.Text = "Step";
            this.stepButton.UseVisualStyleBackColor = true;
            this.stepButton.Click += new System.EventHandler(this.stepButton_Click);
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(371, 61);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(74, 23);
            this.resetButton.TabIndex = 6;
            this.resetButton.Text = "reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(575, 412);
            this.Controls.Add(this.resetButton);
            this.Controls.Add(this.stepButton);
            this.Controls.Add(this.logText);
            this.Controls.Add(this.execCommandButton);
            this.Controls.Add(this.execFileButton);
            this.Controls.Add(this.commandText);
            this.Controls.Add(this.fileText);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox fileText;
        private System.Windows.Forms.TextBox commandText;
        private System.Windows.Forms.Button execFileButton;
        private System.Windows.Forms.Button execCommandButton;
        private System.Windows.Forms.TextBox logText;
        private System.Windows.Forms.Button stepButton;
        private System.Windows.Forms.Button resetButton;
    }
}

