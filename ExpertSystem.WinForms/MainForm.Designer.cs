namespace ExpertSystem.WinForms
{
    partial class MainForm
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
            this.RunButton = new System.Windows.Forms.Button();
            this.DetailsListBox = new System.Windows.Forms.ListBox();
            this.DetailsGroupBox = new System.Windows.Forms.GroupBox();
            this.UnknowButton = new System.Windows.Forms.Button();
            this.ApplyButton = new System.Windows.Forms.Button();
            this.DetailsLabel = new System.Windows.Forms.Label();
            this.QuestionComboBox = new System.Windows.Forms.ComboBox();
            this.DetailsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // RunButton
            // 
            this.RunButton.Location = new System.Drawing.Point(301, 10);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(112, 23);
            this.RunButton.TabIndex = 0;
            this.RunButton.Text = "Анализировать";
            this.RunButton.UseVisualStyleBackColor = true;
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // DetailsListBox
            // 
            this.DetailsListBox.FormattingEnabled = true;
            this.DetailsListBox.Location = new System.Drawing.Point(6, 46);
            this.DetailsListBox.Name = "DetailsListBox";
            this.DetailsListBox.Size = new System.Drawing.Size(389, 251);
            this.DetailsListBox.TabIndex = 2;
            this.DetailsListBox.DoubleClick += new System.EventHandler(this.DetailsListBox_DoubleClick);
            // 
            // DetailsGroupBox
            // 
            this.DetailsGroupBox.Controls.Add(this.UnknowButton);
            this.DetailsGroupBox.Controls.Add(this.ApplyButton);
            this.DetailsGroupBox.Controls.Add(this.DetailsLabel);
            this.DetailsGroupBox.Controls.Add(this.DetailsListBox);
            this.DetailsGroupBox.Location = new System.Drawing.Point(12, 39);
            this.DetailsGroupBox.Name = "DetailsGroupBox";
            this.DetailsGroupBox.Size = new System.Drawing.Size(401, 337);
            this.DetailsGroupBox.TabIndex = 3;
            this.DetailsGroupBox.TabStop = false;
            this.DetailsGroupBox.Text = "Детали";
            // 
            // UnknowButton
            // 
            this.UnknowButton.Location = new System.Drawing.Point(320, 308);
            this.UnknowButton.Name = "UnknowButton";
            this.UnknowButton.Size = new System.Drawing.Size(75, 23);
            this.UnknowButton.TabIndex = 5;
            this.UnknowButton.Text = "Не знаю";
            this.UnknowButton.UseVisualStyleBackColor = true;
            this.UnknowButton.Click += new System.EventHandler(this.UnknowButton_Click);
            // 
            // ApplyButton
            // 
            this.ApplyButton.Location = new System.Drawing.Point(7, 308);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(75, 23);
            this.ApplyButton.TabIndex = 4;
            this.ApplyButton.Text = "Ответить";
            this.ApplyButton.UseVisualStyleBackColor = true;
            this.ApplyButton.Click += new System.EventHandler(this.ApplyButton_Click);
            // 
            // DetailsLabel
            // 
            this.DetailsLabel.AutoSize = true;
            this.DetailsLabel.Location = new System.Drawing.Point(13, 30);
            this.DetailsLabel.Name = "DetailsLabel";
            this.DetailsLabel.Size = new System.Drawing.Size(69, 13);
            this.DetailsLabel.TabIndex = 3;
            this.DetailsLabel.Text = "Укажите {0}";
            // 
            // QuestionComboBox
            // 
            this.QuestionComboBox.FormattingEnabled = true;
            this.QuestionComboBox.Location = new System.Drawing.Point(12, 10);
            this.QuestionComboBox.Name = "QuestionComboBox";
            this.QuestionComboBox.Size = new System.Drawing.Size(283, 21);
            this.QuestionComboBox.TabIndex = 4;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 450);
            this.Controls.Add(this.QuestionComboBox);
            this.Controls.Add(this.DetailsGroupBox);
            this.Controls.Add(this.RunButton);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.DetailsGroupBox.ResumeLayout(false);
            this.DetailsGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button RunButton;
        private System.Windows.Forms.ListBox DetailsListBox;
        private System.Windows.Forms.GroupBox DetailsGroupBox;
        private System.Windows.Forms.Label DetailsLabel;
        private System.Windows.Forms.Button UnknowButton;
        private System.Windows.Forms.Button ApplyButton;
        private System.Windows.Forms.ComboBox QuestionComboBox;
    }
}

