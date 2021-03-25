
namespace DevHelperGUI
{
    partial class inputsForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.formTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.inputsSplitContainer = new System.Windows.Forms.SplitContainer();
            this.expressionGroupBox = new System.Windows.Forms.GroupBox();
            this.expressionTextBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.inputTextGroupBox = new System.Windows.Forms.GroupBox();
            this.inputTextTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.inputTextTextBox = new System.Windows.Forms.TextBox();
            this.inputTextFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.decodeUriEncodedSequencesCheckBox = new System.Windows.Forms.CheckBox();
            this.lineBreaksToCrLfRadioButton = new System.Windows.Forms.RadioButton();
            this.lineBreaksToLfRadioButton = new System.Windows.Forms.RadioButton();
            this.ignoreLineBreaksRadioButton = new System.Windows.Forms.RadioButton();
            this.evaluateLinesSeparatelyRadioButton = new System.Windows.Forms.RadioButton();
            this.expressionErrorLabel = new System.Windows.Forms.Label();
            this.optionsGroupBox = new System.Windows.Forms.GroupBox();
            this.optionsTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.noneOptionCheckBox = new System.Windows.Forms.CheckBox();
            this.ignoreCaseOptionCheckBox = new System.Windows.Forms.CheckBox();
            this.cultureInvariantOptionCheckBox = new System.Windows.Forms.CheckBox();
            this.multilineOptionCheckBox = new System.Windows.Forms.CheckBox();
            this.singlelineOptionCheckBox = new System.Windows.Forms.CheckBox();
            this.ignorePatternWhitespaceOptionCheckBox = new System.Windows.Forms.CheckBox();
            this.rightToLeftOptionCheckBox = new System.Windows.Forms.CheckBox();
            this.eCMAScriptOptionCheckBox = new System.Windows.Forms.CheckBox();
            this.explicitCaptureOptionCheckBox = new System.Windows.Forms.CheckBox();
            this.compiledOptionCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.behaviorFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.singleMatchRadioButton = new System.Windows.Forms.RadioButton();
            this.multipleMatchRadioButton = new System.Windows.Forms.RadioButton();
            this.splitRadioButton = new System.Windows.Forms.RadioButton();
            this.evaluateOnChangeCheckBox = new System.Windows.Forms.CheckBox();
            this.evaluateButton = new System.Windows.Forms.Button();
            this.lineWrapCheckBox = new System.Windows.Forms.CheckBox();
            this.formTableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inputsSplitContainer)).BeginInit();
            this.inputsSplitContainer.Panel1.SuspendLayout();
            this.inputsSplitContainer.Panel2.SuspendLayout();
            this.inputsSplitContainer.SuspendLayout();
            this.expressionGroupBox.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.inputTextGroupBox.SuspendLayout();
            this.inputTextTableLayoutPanel.SuspendLayout();
            this.inputTextFlowLayoutPanel.SuspendLayout();
            this.optionsGroupBox.SuspendLayout();
            this.optionsTableLayoutPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.behaviorFlowLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // formTableLayoutPanel
            // 
            this.formTableLayoutPanel.ColumnCount = 2;
            this.formTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.formTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.formTableLayoutPanel.Controls.Add(this.inputsSplitContainer, 0, 0);
            this.formTableLayoutPanel.Controls.Add(this.optionsGroupBox, 1, 0);
            this.formTableLayoutPanel.Controls.Add(this.groupBox1, 0, 1);
            this.formTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.formTableLayoutPanel.Name = "formTableLayoutPanel";
            this.formTableLayoutPanel.RowCount = 2;
            this.formTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.formTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.formTableLayoutPanel.Size = new System.Drawing.Size(800, 450);
            this.formTableLayoutPanel.TabIndex = 0;
            // 
            // inputsSplitContainer
            // 
            this.inputsSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inputsSplitContainer.Location = new System.Drawing.Point(3, 3);
            this.inputsSplitContainer.Name = "inputsSplitContainer";
            this.inputsSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // inputsSplitContainer.Panel1
            // 
            this.inputsSplitContainer.Panel1.Controls.Add(this.expressionGroupBox);
            this.inputsSplitContainer.Panel1MinSize = 50;
            // 
            // inputsSplitContainer.Panel2
            // 
            this.inputsSplitContainer.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.inputsSplitContainer.Panel2MinSize = 100;
            this.inputsSplitContainer.Size = new System.Drawing.Size(611, 387);
            this.inputsSplitContainer.SplitterDistance = 100;
            this.inputsSplitContainer.TabIndex = 0;
            // 
            // expressionGroupBox
            // 
            this.expressionGroupBox.AutoSize = true;
            this.expressionGroupBox.Controls.Add(this.expressionTextBox);
            this.expressionGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.expressionGroupBox.Location = new System.Drawing.Point(0, 0);
            this.expressionGroupBox.Name = "expressionGroupBox";
            this.expressionGroupBox.Size = new System.Drawing.Size(611, 100);
            this.expressionGroupBox.TabIndex = 0;
            this.expressionGroupBox.TabStop = false;
            this.expressionGroupBox.Text = "Expression";
            // 
            // expressionTextBox
            // 
            this.expressionTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.expressionTextBox.Location = new System.Drawing.Point(3, 19);
            this.expressionTextBox.Multiline = true;
            this.expressionTextBox.Name = "expressionTextBox";
            this.expressionTextBox.Size = new System.Drawing.Size(605, 78);
            this.expressionTextBox.TabIndex = 1;
            this.expressionTextBox.TextChanged += new System.EventHandler(this.ExpressionTextBox_TextChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.inputTextGroupBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.expressionErrorLabel, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(611, 283);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // inputTextGroupBox
            // 
            this.inputTextGroupBox.Controls.Add(this.inputTextTableLayoutPanel);
            this.inputTextGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inputTextGroupBox.Location = new System.Drawing.Point(3, 18);
            this.inputTextGroupBox.Name = "inputTextGroupBox";
            this.inputTextGroupBox.Size = new System.Drawing.Size(605, 262);
            this.inputTextGroupBox.TabIndex = 1;
            this.inputTextGroupBox.TabStop = false;
            this.inputTextGroupBox.Text = "Input Text";
            // 
            // inputTextTableLayoutPanel
            // 
            this.inputTextTableLayoutPanel.AutoSize = true;
            this.inputTextTableLayoutPanel.ColumnCount = 1;
            this.inputTextTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.inputTextTableLayoutPanel.Controls.Add(this.inputTextTextBox, 0, 0);
            this.inputTextTableLayoutPanel.Controls.Add(this.inputTextFlowLayoutPanel, 0, 1);
            this.inputTextTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inputTextTableLayoutPanel.Location = new System.Drawing.Point(3, 19);
            this.inputTextTableLayoutPanel.Name = "inputTextTableLayoutPanel";
            this.inputTextTableLayoutPanel.RowCount = 2;
            this.inputTextTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.inputTextTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.inputTextTableLayoutPanel.Size = new System.Drawing.Size(599, 240);
            this.inputTextTableLayoutPanel.TabIndex = 0;
            // 
            // inputTextTextBox
            // 
            this.inputTextTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inputTextTextBox.Location = new System.Drawing.Point(3, 3);
            this.inputTextTextBox.Multiline = true;
            this.inputTextTextBox.Name = "inputTextTextBox";
            this.inputTextTextBox.Size = new System.Drawing.Size(593, 178);
            this.inputTextTextBox.TabIndex = 0;
            this.inputTextTextBox.TextChanged += new System.EventHandler(this.InputText_Changed);
            // 
            // inputTextFlowLayoutPanel
            // 
            this.inputTextFlowLayoutPanel.AutoSize = true;
            this.inputTextFlowLayoutPanel.Controls.Add(this.decodeUriEncodedSequencesCheckBox);
            this.inputTextFlowLayoutPanel.Controls.Add(this.lineBreaksToCrLfRadioButton);
            this.inputTextFlowLayoutPanel.Controls.Add(this.lineBreaksToLfRadioButton);
            this.inputTextFlowLayoutPanel.Controls.Add(this.ignoreLineBreaksRadioButton);
            this.inputTextFlowLayoutPanel.Controls.Add(this.evaluateLinesSeparatelyRadioButton);
            this.inputTextFlowLayoutPanel.Controls.Add(this.lineWrapCheckBox);
            this.inputTextFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inputTextFlowLayoutPanel.Location = new System.Drawing.Point(3, 187);
            this.inputTextFlowLayoutPanel.Name = "inputTextFlowLayoutPanel";
            this.inputTextFlowLayoutPanel.Size = new System.Drawing.Size(593, 50);
            this.inputTextFlowLayoutPanel.TabIndex = 1;
            // 
            // decodeUriEncodedSequencesCheckBox
            // 
            this.decodeUriEncodedSequencesCheckBox.AutoSize = true;
            this.decodeUriEncodedSequencesCheckBox.Location = new System.Drawing.Point(3, 3);
            this.decodeUriEncodedSequencesCheckBox.Name = "decodeUriEncodedSequencesCheckBox";
            this.decodeUriEncodedSequencesCheckBox.Size = new System.Drawing.Size(197, 19);
            this.decodeUriEncodedSequencesCheckBox.TabIndex = 0;
            this.decodeUriEncodedSequencesCheckBox.Text = "Decode URI-Encoded Sequences";
            this.decodeUriEncodedSequencesCheckBox.UseVisualStyleBackColor = true;
            this.decodeUriEncodedSequencesCheckBox.CheckedChanged += new System.EventHandler(this.InputText_Changed);
            this.decodeUriEncodedSequencesCheckBox.TextChanged += new System.EventHandler(this.InputText_Changed);
            // 
            // lineBreaksToCrLfRadioButton
            // 
            this.lineBreaksToCrLfRadioButton.AutoSize = true;
            this.lineBreaksToCrLfRadioButton.Checked = true;
            this.lineBreaksToCrLfRadioButton.Location = new System.Drawing.Point(206, 3);
            this.lineBreaksToCrLfRadioButton.Name = "lineBreaksToCrLfRadioButton";
            this.lineBreaksToCrLfRadioButton.Size = new System.Drawing.Size(128, 19);
            this.lineBreaksToCrLfRadioButton.TabIndex = 1;
            this.lineBreaksToCrLfRadioButton.TabStop = true;
            this.lineBreaksToCrLfRadioButton.Text = "Line Breaks to CRLF";
            this.lineBreaksToCrLfRadioButton.UseVisualStyleBackColor = true;
            this.lineBreaksToCrLfRadioButton.CheckedChanged += new System.EventHandler(this.InputTextOption_Changed);
            this.lineBreaksToCrLfRadioButton.TextChanged += new System.EventHandler(this.InputTextOption_Changed);
            // 
            // lineBreaksToLfRadioButton
            // 
            this.lineBreaksToLfRadioButton.AutoSize = true;
            this.lineBreaksToLfRadioButton.Location = new System.Drawing.Point(340, 3);
            this.lineBreaksToLfRadioButton.Name = "lineBreaksToLfRadioButton";
            this.lineBreaksToLfRadioButton.Size = new System.Drawing.Size(113, 19);
            this.lineBreaksToLfRadioButton.TabIndex = 2;
            this.lineBreaksToLfRadioButton.TabStop = true;
            this.lineBreaksToLfRadioButton.Text = "Line Breaks to LF";
            this.lineBreaksToLfRadioButton.UseVisualStyleBackColor = true;
            this.lineBreaksToLfRadioButton.CheckedChanged += new System.EventHandler(this.InputTextOption_Changed);
            // 
            // ignoreLineBreaksRadioButton
            // 
            this.ignoreLineBreaksRadioButton.AutoSize = true;
            this.ignoreLineBreaksRadioButton.Location = new System.Drawing.Point(459, 3);
            this.ignoreLineBreaksRadioButton.Name = "ignoreLineBreaksRadioButton";
            this.ignoreLineBreaksRadioButton.Size = new System.Drawing.Size(121, 19);
            this.ignoreLineBreaksRadioButton.TabIndex = 3;
            this.ignoreLineBreaksRadioButton.TabStop = true;
            this.ignoreLineBreaksRadioButton.Text = "Ignore Line Breaks";
            this.ignoreLineBreaksRadioButton.UseVisualStyleBackColor = true;
            this.ignoreLineBreaksRadioButton.CheckedChanged += new System.EventHandler(this.InputTextOption_Changed);
            // 
            // evaluateLinesSeparatelyRadioButton
            // 
            this.evaluateLinesSeparatelyRadioButton.AutoSize = true;
            this.evaluateLinesSeparatelyRadioButton.Location = new System.Drawing.Point(3, 28);
            this.evaluateLinesSeparatelyRadioButton.Name = "evaluateLinesSeparatelyRadioButton";
            this.evaluateLinesSeparatelyRadioButton.Size = new System.Drawing.Size(179, 19);
            this.evaluateLinesSeparatelyRadioButton.TabIndex = 4;
            this.evaluateLinesSeparatelyRadioButton.Text = "Evaluate Each Line Separately";
            this.evaluateLinesSeparatelyRadioButton.UseVisualStyleBackColor = true;
            this.evaluateLinesSeparatelyRadioButton.CheckedChanged += new System.EventHandler(this.InputTextOption_Changed);
            // 
            // expressionErrorLabel
            // 
            this.expressionErrorLabel.AutoSize = true;
            this.expressionErrorLabel.ForeColor = System.Drawing.Color.Red;
            this.expressionErrorLabel.Location = new System.Drawing.Point(3, 0);
            this.expressionErrorLabel.Name = "expressionErrorLabel";
            this.expressionErrorLabel.Size = new System.Drawing.Size(113, 15);
            this.expressionErrorLabel.TabIndex = 0;
            this.expressionErrorLabel.Text = "Expression required.";
            // 
            // optionsGroupBox
            // 
            this.optionsGroupBox.AutoSize = true;
            this.optionsGroupBox.Controls.Add(this.optionsTableLayoutPanel);
            this.optionsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.optionsGroupBox.Location = new System.Drawing.Point(620, 3);
            this.optionsGroupBox.Name = "optionsGroupBox";
            this.optionsGroupBox.Size = new System.Drawing.Size(177, 387);
            this.optionsGroupBox.TabIndex = 1;
            this.optionsGroupBox.TabStop = false;
            this.optionsGroupBox.Text = "Options";
            // 
            // optionsTableLayoutPanel
            // 
            this.optionsTableLayoutPanel.AutoSize = true;
            this.optionsTableLayoutPanel.ColumnCount = 1;
            this.optionsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.optionsTableLayoutPanel.Controls.Add(this.noneOptionCheckBox, 0, 0);
            this.optionsTableLayoutPanel.Controls.Add(this.ignoreCaseOptionCheckBox, 0, 1);
            this.optionsTableLayoutPanel.Controls.Add(this.cultureInvariantOptionCheckBox, 0, 2);
            this.optionsTableLayoutPanel.Controls.Add(this.multilineOptionCheckBox, 0, 3);
            this.optionsTableLayoutPanel.Controls.Add(this.singlelineOptionCheckBox, 0, 4);
            this.optionsTableLayoutPanel.Controls.Add(this.ignorePatternWhitespaceOptionCheckBox, 0, 5);
            this.optionsTableLayoutPanel.Controls.Add(this.rightToLeftOptionCheckBox);
            this.optionsTableLayoutPanel.Controls.Add(this.eCMAScriptOptionCheckBox, 0, 7);
            this.optionsTableLayoutPanel.Controls.Add(this.explicitCaptureOptionCheckBox, 0, 8);
            this.optionsTableLayoutPanel.Controls.Add(this.compiledOptionCheckBox, 0, 9);
            this.optionsTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.optionsTableLayoutPanel.Location = new System.Drawing.Point(3, 19);
            this.optionsTableLayoutPanel.Name = "optionsTableLayoutPanel";
            this.optionsTableLayoutPanel.RowCount = 10;
            this.optionsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.optionsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.optionsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.optionsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.optionsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.optionsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.optionsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.optionsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.optionsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.optionsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.optionsTableLayoutPanel.Size = new System.Drawing.Size(171, 365);
            this.optionsTableLayoutPanel.TabIndex = 0;
            // 
            // noneOptionCheckBox
            // 
            this.noneOptionCheckBox.AutoSize = true;
            this.noneOptionCheckBox.Location = new System.Drawing.Point(3, 3);
            this.noneOptionCheckBox.Name = "noneOptionCheckBox";
            this.noneOptionCheckBox.Size = new System.Drawing.Size(55, 19);
            this.noneOptionCheckBox.TabIndex = 0;
            this.noneOptionCheckBox.Text = "None";
            this.noneOptionCheckBox.UseVisualStyleBackColor = true;
            this.noneOptionCheckBox.CheckedChanged += new System.EventHandler(this.NoneOptionCheckBox_CheckedChanged);
            // 
            // ignoreCaseOptionCheckBox
            // 
            this.ignoreCaseOptionCheckBox.AutoSize = true;
            this.ignoreCaseOptionCheckBox.Location = new System.Drawing.Point(3, 28);
            this.ignoreCaseOptionCheckBox.Name = "ignoreCaseOptionCheckBox";
            this.ignoreCaseOptionCheckBox.Size = new System.Drawing.Size(88, 19);
            this.ignoreCaseOptionCheckBox.TabIndex = 1;
            this.ignoreCaseOptionCheckBox.Text = "Ignore Case";
            this.ignoreCaseOptionCheckBox.UseVisualStyleBackColor = true;
            this.ignoreCaseOptionCheckBox.CheckedChanged += new System.EventHandler(this.OptionCheckBox_CheckedChanged);
            // 
            // cultureInvariantOptionCheckBox
            // 
            this.cultureInvariantOptionCheckBox.AutoSize = true;
            this.cultureInvariantOptionCheckBox.Location = new System.Drawing.Point(3, 53);
            this.cultureInvariantOptionCheckBox.Name = "cultureInvariantOptionCheckBox";
            this.cultureInvariantOptionCheckBox.Size = new System.Drawing.Size(114, 19);
            this.cultureInvariantOptionCheckBox.TabIndex = 2;
            this.cultureInvariantOptionCheckBox.Text = "Culture Invariant";
            this.cultureInvariantOptionCheckBox.UseVisualStyleBackColor = true;
            this.cultureInvariantOptionCheckBox.CheckedChanged += new System.EventHandler(this.OptionCheckBox_CheckedChanged);
            // 
            // multilineOptionCheckBox
            // 
            this.multilineOptionCheckBox.AutoSize = true;
            this.multilineOptionCheckBox.Location = new System.Drawing.Point(3, 78);
            this.multilineOptionCheckBox.Name = "multilineOptionCheckBox";
            this.multilineOptionCheckBox.Size = new System.Drawing.Size(78, 19);
            this.multilineOptionCheckBox.TabIndex = 3;
            this.multilineOptionCheckBox.Text = "Multi-line";
            this.multilineOptionCheckBox.UseVisualStyleBackColor = true;
            this.multilineOptionCheckBox.CheckedChanged += new System.EventHandler(this.OptionCheckBox_CheckedChanged);
            // 
            // singlelineOptionCheckBox
            // 
            this.singlelineOptionCheckBox.AutoSize = true;
            this.singlelineOptionCheckBox.Location = new System.Drawing.Point(3, 103);
            this.singlelineOptionCheckBox.Name = "singlelineOptionCheckBox";
            this.singlelineOptionCheckBox.Size = new System.Drawing.Size(83, 19);
            this.singlelineOptionCheckBox.TabIndex = 4;
            this.singlelineOptionCheckBox.Text = "Single Line";
            this.singlelineOptionCheckBox.UseVisualStyleBackColor = true;
            this.singlelineOptionCheckBox.CheckedChanged += new System.EventHandler(this.OptionCheckBox_CheckedChanged);
            // 
            // ignorePatternWhitespaceOptionCheckBox
            // 
            this.ignorePatternWhitespaceOptionCheckBox.AutoSize = true;
            this.ignorePatternWhitespaceOptionCheckBox.Location = new System.Drawing.Point(3, 128);
            this.ignorePatternWhitespaceOptionCheckBox.Name = "ignorePatternWhitespaceOptionCheckBox";
            this.ignorePatternWhitespaceOptionCheckBox.Size = new System.Drawing.Size(165, 19);
            this.ignorePatternWhitespaceOptionCheckBox.TabIndex = 5;
            this.ignorePatternWhitespaceOptionCheckBox.Text = "Ignore Pattern Whitespace";
            this.ignorePatternWhitespaceOptionCheckBox.UseVisualStyleBackColor = true;
            this.ignorePatternWhitespaceOptionCheckBox.CheckedChanged += new System.EventHandler(this.OptionCheckBox_CheckedChanged);
            // 
            // rightToLeftOptionCheckBox
            // 
            this.rightToLeftOptionCheckBox.AutoSize = true;
            this.rightToLeftOptionCheckBox.Location = new System.Drawing.Point(3, 153);
            this.rightToLeftOptionCheckBox.Name = "rightToLeftOptionCheckBox";
            this.rightToLeftOptionCheckBox.Size = new System.Drawing.Size(95, 19);
            this.rightToLeftOptionCheckBox.TabIndex = 6;
            this.rightToLeftOptionCheckBox.Text = "Right-to-Left";
            this.rightToLeftOptionCheckBox.UseVisualStyleBackColor = true;
            this.rightToLeftOptionCheckBox.CheckedChanged += new System.EventHandler(this.OptionCheckBox_CheckedChanged);
            // 
            // eCMAScriptOptionCheckBox
            // 
            this.eCMAScriptOptionCheckBox.AutoSize = true;
            this.eCMAScriptOptionCheckBox.Location = new System.Drawing.Point(3, 178);
            this.eCMAScriptOptionCheckBox.Name = "eCMAScriptOptionCheckBox";
            this.eCMAScriptOptionCheckBox.Size = new System.Drawing.Size(89, 19);
            this.eCMAScriptOptionCheckBox.TabIndex = 7;
            this.eCMAScriptOptionCheckBox.Text = "ECMAScript";
            this.eCMAScriptOptionCheckBox.UseVisualStyleBackColor = true;
            this.eCMAScriptOptionCheckBox.CheckedChanged += new System.EventHandler(this.OptionCheckBox_CheckedChanged);
            // 
            // explicitCaptureOptionCheckBox
            // 
            this.explicitCaptureOptionCheckBox.AutoSize = true;
            this.explicitCaptureOptionCheckBox.Location = new System.Drawing.Point(3, 203);
            this.explicitCaptureOptionCheckBox.Name = "explicitCaptureOptionCheckBox";
            this.explicitCaptureOptionCheckBox.Size = new System.Drawing.Size(109, 19);
            this.explicitCaptureOptionCheckBox.TabIndex = 8;
            this.explicitCaptureOptionCheckBox.Text = "Explicit Capture";
            this.explicitCaptureOptionCheckBox.UseVisualStyleBackColor = true;
            this.explicitCaptureOptionCheckBox.CheckedChanged += new System.EventHandler(this.OptionCheckBox_CheckedChanged);
            // 
            // compiledOptionCheckBox
            // 
            this.compiledOptionCheckBox.AutoSize = true;
            this.compiledOptionCheckBox.Location = new System.Drawing.Point(3, 228);
            this.compiledOptionCheckBox.Name = "compiledOptionCheckBox";
            this.compiledOptionCheckBox.Size = new System.Drawing.Size(78, 19);
            this.compiledOptionCheckBox.TabIndex = 9;
            this.compiledOptionCheckBox.Text = "Compiled";
            this.compiledOptionCheckBox.UseVisualStyleBackColor = true;
            this.compiledOptionCheckBox.CheckedChanged += new System.EventHandler(this.OptionCheckBox_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.formTableLayoutPanel.SetColumnSpan(this.groupBox1, 2);
            this.groupBox1.Controls.Add(this.behaviorFlowLayoutPanel);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 396);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(794, 51);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Behavior";
            // 
            // behaviorFlowLayoutPanel
            // 
            this.behaviorFlowLayoutPanel.AutoSize = true;
            this.behaviorFlowLayoutPanel.Controls.Add(this.singleMatchRadioButton);
            this.behaviorFlowLayoutPanel.Controls.Add(this.multipleMatchRadioButton);
            this.behaviorFlowLayoutPanel.Controls.Add(this.splitRadioButton);
            this.behaviorFlowLayoutPanel.Controls.Add(this.evaluateOnChangeCheckBox);
            this.behaviorFlowLayoutPanel.Controls.Add(this.evaluateButton);
            this.behaviorFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.behaviorFlowLayoutPanel.Location = new System.Drawing.Point(3, 19);
            this.behaviorFlowLayoutPanel.Name = "behaviorFlowLayoutPanel";
            this.behaviorFlowLayoutPanel.Size = new System.Drawing.Size(788, 29);
            this.behaviorFlowLayoutPanel.TabIndex = 0;
            // 
            // singleMatchRadioButton
            // 
            this.singleMatchRadioButton.AutoSize = true;
            this.singleMatchRadioButton.Checked = true;
            this.singleMatchRadioButton.Location = new System.Drawing.Point(3, 3);
            this.singleMatchRadioButton.Name = "singleMatchRadioButton";
            this.singleMatchRadioButton.Size = new System.Drawing.Size(94, 19);
            this.singleMatchRadioButton.TabIndex = 1;
            this.singleMatchRadioButton.TabStop = true;
            this.singleMatchRadioButton.Text = "Single Match";
            this.singleMatchRadioButton.UseVisualStyleBackColor = true;
            this.singleMatchRadioButton.CheckedChanged += new System.EventHandler(this.InputTextOption_Changed);
            // 
            // multipleMatchRadioButton
            // 
            this.multipleMatchRadioButton.AutoSize = true;
            this.multipleMatchRadioButton.Location = new System.Drawing.Point(103, 3);
            this.multipleMatchRadioButton.Name = "multipleMatchRadioButton";
            this.multipleMatchRadioButton.Size = new System.Drawing.Size(102, 19);
            this.multipleMatchRadioButton.TabIndex = 2;
            this.multipleMatchRadioButton.Text = "Muliple Match";
            this.multipleMatchRadioButton.UseVisualStyleBackColor = true;
            this.multipleMatchRadioButton.CheckedChanged += new System.EventHandler(this.InputTextOption_Changed);
            // 
            // splitRadioButton
            // 
            this.splitRadioButton.AutoSize = true;
            this.splitRadioButton.Location = new System.Drawing.Point(211, 3);
            this.splitRadioButton.Name = "splitRadioButton";
            this.splitRadioButton.Size = new System.Drawing.Size(48, 19);
            this.splitRadioButton.TabIndex = 3;
            this.splitRadioButton.Text = "Split";
            this.splitRadioButton.UseVisualStyleBackColor = true;
            this.splitRadioButton.CheckedChanged += new System.EventHandler(this.InputTextOption_Changed);
            // 
            // evaluateOnChangeCheckBox
            // 
            this.evaluateOnChangeCheckBox.AutoSize = true;
            this.evaluateOnChangeCheckBox.Location = new System.Drawing.Point(265, 3);
            this.evaluateOnChangeCheckBox.Name = "evaluateOnChangeCheckBox";
            this.evaluateOnChangeCheckBox.Size = new System.Drawing.Size(131, 19);
            this.evaluateOnChangeCheckBox.TabIndex = 0;
            this.evaluateOnChangeCheckBox.Text = "Evaluate on Change";
            this.evaluateOnChangeCheckBox.UseVisualStyleBackColor = true;
            this.evaluateOnChangeCheckBox.CheckedChanged += new System.EventHandler(this.EvaluateOnChangeCheckBox_CheckedChanged);
            // 
            // evaluateButton
            // 
            this.evaluateButton.Location = new System.Drawing.Point(402, 3);
            this.evaluateButton.Name = "evaluateButton";
            this.evaluateButton.Size = new System.Drawing.Size(75, 23);
            this.evaluateButton.TabIndex = 4;
            this.evaluateButton.Text = "Evaluate";
            this.evaluateButton.UseVisualStyleBackColor = true;
            this.evaluateButton.Click += new System.EventHandler(this.EvaluateButton_Click);
            // 
            // lineWrapCheckBox
            // 
            this.lineWrapCheckBox.AutoSize = true;
            this.lineWrapCheckBox.Location = new System.Drawing.Point(188, 28);
            this.lineWrapCheckBox.Name = "lineWrapCheckBox";
            this.lineWrapCheckBox.Size = new System.Drawing.Size(79, 19);
            this.lineWrapCheckBox.TabIndex = 5;
            this.lineWrapCheckBox.Text = "Line Wrap";
            this.lineWrapCheckBox.UseVisualStyleBackColor = true;
            this.lineWrapCheckBox.CheckedChanged += new System.EventHandler(this.LineWrapCheckBox_CheckedChanged);
            // 
            // inputsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.formTableLayoutPanel);
            this.Name = "inputsForm";
            this.Text = "Inputs";
            this.formTableLayoutPanel.ResumeLayout(false);
            this.formTableLayoutPanel.PerformLayout();
            this.inputsSplitContainer.Panel1.ResumeLayout(false);
            this.inputsSplitContainer.Panel1.PerformLayout();
            this.inputsSplitContainer.Panel2.ResumeLayout(false);
            this.inputsSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inputsSplitContainer)).EndInit();
            this.inputsSplitContainer.ResumeLayout(false);
            this.expressionGroupBox.ResumeLayout(false);
            this.expressionGroupBox.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.inputTextGroupBox.ResumeLayout(false);
            this.inputTextGroupBox.PerformLayout();
            this.inputTextTableLayoutPanel.ResumeLayout(false);
            this.inputTextTableLayoutPanel.PerformLayout();
            this.inputTextFlowLayoutPanel.ResumeLayout(false);
            this.inputTextFlowLayoutPanel.PerformLayout();
            this.optionsGroupBox.ResumeLayout(false);
            this.optionsGroupBox.PerformLayout();
            this.optionsTableLayoutPanel.ResumeLayout(false);
            this.optionsTableLayoutPanel.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.behaviorFlowLayoutPanel.ResumeLayout(false);
            this.behaviorFlowLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel formTableLayoutPanel;
        private System.Windows.Forms.SplitContainer inputsSplitContainer;
        private System.Windows.Forms.GroupBox expressionGroupBox;
        private System.Windows.Forms.GroupBox optionsGroupBox;
        private System.Windows.Forms.TableLayoutPanel optionsTableLayoutPanel;
        private System.Windows.Forms.CheckBox noneOptionCheckBox;
        private System.Windows.Forms.CheckBox ignoreCaseOptionCheckBox;
        private System.Windows.Forms.CheckBox cultureInvariantOptionCheckBox;
        private System.Windows.Forms.CheckBox multilineOptionCheckBox;
        private System.Windows.Forms.CheckBox singlelineOptionCheckBox;
        private System.Windows.Forms.CheckBox ignorePatternWhitespaceOptionCheckBox;
        private System.Windows.Forms.CheckBox rightToLeftOptionCheckBox;
        private System.Windows.Forms.CheckBox eCMAScriptOptionCheckBox;
        private System.Windows.Forms.CheckBox explicitCaptureOptionCheckBox;
        private System.Windows.Forms.CheckBox compiledOptionCheckBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.FlowLayoutPanel behaviorFlowLayoutPanel;
        private System.Windows.Forms.CheckBox evaluateOnChangeCheckBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox inputTextGroupBox;
        private System.Windows.Forms.TableLayoutPanel inputTextTableLayoutPanel;
        private System.Windows.Forms.TextBox inputTextTextBox;
        private System.Windows.Forms.FlowLayoutPanel inputTextFlowLayoutPanel;
        private System.Windows.Forms.CheckBox decodeUriEncodedSequencesCheckBox;
        private System.Windows.Forms.RadioButton lineBreaksToCrLfRadioButton;
        private System.Windows.Forms.RadioButton lineBreaksToLfRadioButton;
        private System.Windows.Forms.Label expressionErrorLabel;
        private System.Windows.Forms.TextBox expressionTextBox;
        private System.Windows.Forms.RadioButton singleMatchRadioButton;
        private System.Windows.Forms.RadioButton multipleMatchRadioButton;
        private System.Windows.Forms.RadioButton splitRadioButton;
        private System.Windows.Forms.Button evaluateButton;
        private System.Windows.Forms.RadioButton ignoreLineBreaksRadioButton;
        private System.Windows.Forms.RadioButton evaluateLinesSeparatelyRadioButton;
        private System.Windows.Forms.CheckBox lineWrapCheckBox;
    }
}

