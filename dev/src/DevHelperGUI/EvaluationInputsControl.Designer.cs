
namespace DevHelperGUI
{
    partial class EvaluationInputsControl
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
            this.inputSplitContainer = new System.Windows.Forms.SplitContainer();
            this.patternGroupBox = new System.Windows.Forms.GroupBox();
            this.patternTextBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.patternErrorLabel = new System.Windows.Forms.Label();
            this.inputTextGroupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.evaluateOnChangeCheckBox = new System.Windows.Forms.CheckBox();
            this.evaluateButton = new System.Windows.Forms.Button();
            this.inputTextBox = new System.Windows.Forms.TextBox();
            this.evaluationErrorLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.inputSplitContainer)).BeginInit();
            this.inputSplitContainer.Panel1.SuspendLayout();
            this.inputSplitContainer.Panel2.SuspendLayout();
            this.inputSplitContainer.SuspendLayout();
            this.patternGroupBox.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.inputTextGroupBox.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // inputSplitContainer
            // 
            this.inputSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inputSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.inputSplitContainer.Name = "inputSplitContainer";
            this.inputSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // inputSplitContainer.Panel1
            // 
            this.inputSplitContainer.Panel1.Controls.Add(this.patternGroupBox);
            // 
            // inputSplitContainer.Panel2
            // 
            this.inputSplitContainer.Panel2.Controls.Add(this.tableLayoutPanel2);
            this.inputSplitContainer.Size = new System.Drawing.Size(275, 450);
            this.inputSplitContainer.SplitterDistance = 92;
            this.inputSplitContainer.TabIndex = 1;
            // 
            // patternGroupBox
            // 
            this.patternGroupBox.AutoSize = true;
            this.patternGroupBox.Controls.Add(this.patternTextBox);
            this.patternGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.patternGroupBox.Location = new System.Drawing.Point(0, 0);
            this.patternGroupBox.Name = "patternGroupBox";
            this.patternGroupBox.Size = new System.Drawing.Size(275, 92);
            this.patternGroupBox.TabIndex = 0;
            this.patternGroupBox.TabStop = false;
            this.patternGroupBox.Text = "Pattern";
            // 
            // patternTextBox
            // 
            this.patternTextBox.AcceptsReturn = true;
            this.patternTextBox.AcceptsTab = true;
            this.patternTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.patternTextBox.Location = new System.Drawing.Point(3, 19);
            this.patternTextBox.Multiline = true;
            this.patternTextBox.Name = "patternTextBox";
            this.patternTextBox.Size = new System.Drawing.Size(269, 70);
            this.patternTextBox.TabIndex = 1;
            this.patternTextBox.TextChanged += new System.EventHandler(this.PatternTextBox_TextChanged);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.patternErrorLabel, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.inputTextGroupBox, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(275, 354);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // patternErrorLabel
            // 
            this.patternErrorLabel.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.patternErrorLabel, 2);
            this.patternErrorLabel.ForeColor = System.Drawing.Color.Red;
            this.patternErrorLabel.Location = new System.Drawing.Point(3, 0);
            this.patternErrorLabel.Name = "patternErrorLabel";
            this.patternErrorLabel.Size = new System.Drawing.Size(117, 15);
            this.patternErrorLabel.TabIndex = 4;
            this.patternErrorLabel.Text = "No pattern provided.";
            // 
            // inputTextGroupBox
            // 
            this.inputTextGroupBox.Controls.Add(this.tableLayoutPanel4);
            this.inputTextGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inputTextGroupBox.Location = new System.Drawing.Point(3, 18);
            this.inputTextGroupBox.Name = "inputTextGroupBox";
            this.inputTextGroupBox.Size = new System.Drawing.Size(269, 333);
            this.inputTextGroupBox.TabIndex = 1;
            this.inputTextGroupBox.TabStop = false;
            this.inputTextGroupBox.Text = "Input Text";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.Controls.Add(this.evaluateOnChangeCheckBox, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.evaluateButton, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.inputTextBox, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.evaluationErrorLabel, 0, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 19);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(263, 311);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // evaluateOnChangeCheckBox
            // 
            this.evaluateOnChangeCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.evaluateOnChangeCheckBox.AutoSize = true;
            this.evaluateOnChangeCheckBox.Location = new System.Drawing.Point(48, 285);
            this.evaluateOnChangeCheckBox.Name = "evaluateOnChangeCheckBox";
            this.evaluateOnChangeCheckBox.Size = new System.Drawing.Size(131, 19);
            this.evaluateOnChangeCheckBox.TabIndex = 0;
            this.evaluateOnChangeCheckBox.Text = "Evaluate on Change";
            this.evaluateOnChangeCheckBox.UseVisualStyleBackColor = true;
            this.evaluateOnChangeCheckBox.CheckedChanged += new System.EventHandler(this.EvaluateOnChangeCheckBox_CheckedChanged);
            // 
            // evaluateButton
            // 
            this.evaluateButton.Enabled = false;
            this.evaluateButton.Location = new System.Drawing.Point(185, 285);
            this.evaluateButton.Name = "evaluateButton";
            this.evaluateButton.Size = new System.Drawing.Size(75, 23);
            this.evaluateButton.TabIndex = 1;
            this.evaluateButton.Text = "Evaluate";
            this.evaluateButton.UseVisualStyleBackColor = true;
            this.evaluateButton.Click += new System.EventHandler(this.EvaluateButton_Click);
            // 
            // inputTextBox
            // 
            this.tableLayoutPanel4.SetColumnSpan(this.inputTextBox, 2);
            this.inputTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inputTextBox.Location = new System.Drawing.Point(3, 3);
            this.inputTextBox.Multiline = true;
            this.inputTextBox.Name = "inputTextBox";
            this.inputTextBox.Size = new System.Drawing.Size(257, 256);
            this.inputTextBox.TabIndex = 2;
            this.inputTextBox.TextChanged += new System.EventHandler(this.InputTextBox_TextChanged);
            // 
            // evaluationErrorLabel
            // 
            this.evaluationErrorLabel.AutoSize = true;
            this.tableLayoutPanel4.SetColumnSpan(this.evaluationErrorLabel, 2);
            this.evaluationErrorLabel.ForeColor = System.Drawing.Color.Red;
            this.evaluationErrorLabel.Location = new System.Drawing.Point(3, 262);
            this.evaluationErrorLabel.Name = "evaluationErrorLabel";
            this.evaluationErrorLabel.Size = new System.Drawing.Size(159, 15);
            this.evaluationErrorLabel.TabIndex = 3;
            this.evaluationErrorLabel.Text = "Unexpected evaluation error.";
            this.evaluationErrorLabel.Visible = false;
            // 
            // EvaluationInputsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.inputSplitContainer);
            this.Name = "EvaluationInputsControl";
            this.Size = new System.Drawing.Size(275, 450);
            this.inputSplitContainer.Panel1.ResumeLayout(false);
            this.inputSplitContainer.Panel1.PerformLayout();
            this.inputSplitContainer.Panel2.ResumeLayout(false);
            this.inputSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inputSplitContainer)).EndInit();
            this.inputSplitContainer.ResumeLayout(false);
            this.patternGroupBox.ResumeLayout(false);
            this.patternGroupBox.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.inputTextGroupBox.ResumeLayout(false);
            this.inputTextGroupBox.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer inputSplitContainer;
        private System.Windows.Forms.GroupBox patternGroupBox;
        private System.Windows.Forms.TextBox patternTextBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label patternErrorLabel;
        private System.Windows.Forms.GroupBox inputTextGroupBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.CheckBox evaluateOnChangeCheckBox;
        private System.Windows.Forms.Button evaluateButton;
        private System.Windows.Forms.TextBox inputTextBox;
        private System.Windows.Forms.Label evaluationErrorLabel;
    }
}
