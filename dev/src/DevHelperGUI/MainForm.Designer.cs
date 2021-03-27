
namespace DevHelperGUI
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.regularExpressionsGroupBox = new System.Windows.Forms.GroupBox();
            this.regularExpressionsFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.singleMatchRegexButton = new System.Windows.Forms.Button();
            this.matchAllRegexButton = new System.Windows.Forms.Button();
            this.splitRegexButton = new System.Windows.Forms.Button();
            this.flowLayoutPanel1.SuspendLayout();
            this.regularExpressionsGroupBox.SuspendLayout();
            this.regularExpressionsFlowLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.regularExpressionsGroupBox);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(800, 450);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // regularExpressionsGroupBox
            // 
            this.regularExpressionsGroupBox.AutoSize = true;
            this.regularExpressionsGroupBox.Controls.Add(this.regularExpressionsFlowLayoutPanel);
            this.regularExpressionsGroupBox.Location = new System.Drawing.Point(3, 3);
            this.regularExpressionsGroupBox.Name = "regularExpressionsGroupBox";
            this.regularExpressionsGroupBox.Size = new System.Drawing.Size(260, 53);
            this.regularExpressionsGroupBox.TabIndex = 0;
            this.regularExpressionsGroupBox.TabStop = false;
            this.regularExpressionsGroupBox.Text = "Regular Expressions";
            // 
            // regularExpressionsFlowLayoutPanel
            // 
            this.regularExpressionsFlowLayoutPanel.AutoSize = true;
            this.regularExpressionsFlowLayoutPanel.Controls.Add(this.singleMatchRegexButton);
            this.regularExpressionsFlowLayoutPanel.Controls.Add(this.matchAllRegexButton);
            this.regularExpressionsFlowLayoutPanel.Controls.Add(this.splitRegexButton);
            this.regularExpressionsFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.regularExpressionsFlowLayoutPanel.Location = new System.Drawing.Point(3, 19);
            this.regularExpressionsFlowLayoutPanel.Name = "regularExpressionsFlowLayoutPanel";
            this.regularExpressionsFlowLayoutPanel.Size = new System.Drawing.Size(254, 31);
            this.regularExpressionsFlowLayoutPanel.TabIndex = 0;
            // 
            // singleMatchRegexButton
            // 
            this.singleMatchRegexButton.AutoSize = true;
            this.singleMatchRegexButton.Location = new System.Drawing.Point(3, 3);
            this.singleMatchRegexButton.Name = "singleMatchRegexButton";
            this.singleMatchRegexButton.Size = new System.Drawing.Size(86, 25);
            this.singleMatchRegexButton.TabIndex = 0;
            this.singleMatchRegexButton.Text = "Single Match";
            this.singleMatchRegexButton.UseVisualStyleBackColor = true;
            this.singleMatchRegexButton.Click += new System.EventHandler(this.SingleMatchRegexButton_Click);
            // 
            // matchAllRegexButton
            // 
            this.matchAllRegexButton.AutoSize = true;
            this.matchAllRegexButton.Location = new System.Drawing.Point(95, 3);
            this.matchAllRegexButton.Name = "matchAllRegexButton";
            this.matchAllRegexButton.Size = new System.Drawing.Size(75, 25);
            this.matchAllRegexButton.TabIndex = 1;
            this.matchAllRegexButton.Text = "Match All";
            this.matchAllRegexButton.UseVisualStyleBackColor = true;
            this.matchAllRegexButton.Click += new System.EventHandler(this.MatchAllRegexButton_Click);
            // 
            // splitRegexButton
            // 
            this.splitRegexButton.AutoSize = true;
            this.splitRegexButton.Location = new System.Drawing.Point(176, 3);
            this.splitRegexButton.Name = "splitRegexButton";
            this.splitRegexButton.Size = new System.Drawing.Size(75, 25);
            this.splitRegexButton.TabIndex = 2;
            this.splitRegexButton.Text = "Split";
            this.splitRegexButton.UseVisualStyleBackColor = true;
            this.splitRegexButton.Click += new System.EventHandler(this.SplitRegexButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "MainForm";
            this.Text = "Dev Helper";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.regularExpressionsGroupBox.ResumeLayout(false);
            this.regularExpressionsGroupBox.PerformLayout();
            this.regularExpressionsFlowLayoutPanel.ResumeLayout(false);
            this.regularExpressionsFlowLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.GroupBox regularExpressionsGroupBox;
        private System.Windows.Forms.FlowLayoutPanel regularExpressionsFlowLayoutPanel;
        private System.Windows.Forms.Button singleMatchRegexButton;
        private System.Windows.Forms.Button matchAllRegexButton;
        private System.Windows.Forms.Button splitRegexButton;
    }
}