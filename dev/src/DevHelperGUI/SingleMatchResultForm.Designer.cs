
namespace DevHelperGUI
{
    partial class SingleMatchResultForm
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
            this.matchResultTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.successLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.indexLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lengthLabel = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupsDataGridView = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rawValueRadioButton = new System.Windows.Forms.RadioButton();
            this.uriEncodedRadioButton = new System.Windows.Forms.RadioButton();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.xmlEncodedRadioButton = new System.Windows.Forms.RadioButton();
            this.wordWrapCheckBox = new System.Windows.Forms.CheckBox();
            this.valueTextBox = new System.Windows.Forms.TextBox();
            this.hexValuesRadioButton = new System.Windows.Forms.RadioButton();
            this.groupNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupSuccessColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupIndexColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupLengthColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupRawValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupXmlEncodedColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupUriEncodedValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.showMatchValueButton = new System.Windows.Forms.Button();
            this.groupNameHeadingLabel = new System.Windows.Forms.Label();
            this.groupNameValueLabel = new System.Windows.Forms.Label();
            this.groupSuccessHeadingLabel = new System.Windows.Forms.Label();
            this.groupSuccessValueLabel = new System.Windows.Forms.Label();
            this.groupIndexHeadingLabel = new System.Windows.Forms.Label();
            this.groupIndexValueLabel = new System.Windows.Forms.Label();
            this.groupLengthHeadingLabel = new System.Windows.Forms.Label();
            this.groupLengthValueLabel = new System.Windows.Forms.Label();
            this.matchResultTableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupsDataGridView)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // matchResultTableLayoutPanel
            // 
            this.matchResultTableLayoutPanel.AutoSize = true;
            this.matchResultTableLayoutPanel.ColumnCount = 7;
            this.matchResultTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.matchResultTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.matchResultTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.matchResultTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.matchResultTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.matchResultTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.matchResultTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.matchResultTableLayoutPanel.Controls.Add(this.label1, 0, 0);
            this.matchResultTableLayoutPanel.Controls.Add(this.successLabel, 1, 0);
            this.matchResultTableLayoutPanel.Controls.Add(this.label2, 2, 0);
            this.matchResultTableLayoutPanel.Controls.Add(this.indexLabel, 3, 0);
            this.matchResultTableLayoutPanel.Controls.Add(this.label3, 4, 0);
            this.matchResultTableLayoutPanel.Controls.Add(this.lengthLabel, 5, 0);
            this.matchResultTableLayoutPanel.Controls.Add(this.splitContainer1, 0, 1);
            this.matchResultTableLayoutPanel.Controls.Add(this.showMatchValueButton, 6, 0);
            this.matchResultTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.matchResultTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.matchResultTableLayoutPanel.Name = "matchResultTableLayoutPanel";
            this.matchResultTableLayoutPanel.RowCount = 2;
            this.matchResultTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.matchResultTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.matchResultTableLayoutPanel.Size = new System.Drawing.Size(800, 450);
            this.matchResultTableLayoutPanel.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Success:";
            // 
            // successLabel
            // 
            this.successLabel.AutoSize = true;
            this.successLabel.ForeColor = System.Drawing.Color.Green;
            this.successLabel.Location = new System.Drawing.Point(59, 0);
            this.successLabel.Name = "successLabel";
            this.successLabel.Size = new System.Drawing.Size(28, 15);
            this.successLabel.TabIndex = 1;
            this.successLabel.Text = "true";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(93, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Index:";
            // 
            // indexLabel
            // 
            this.indexLabel.AutoSize = true;
            this.indexLabel.Location = new System.Drawing.Point(138, 0);
            this.indexLabel.Name = "indexLabel";
            this.indexLabel.Size = new System.Drawing.Size(13, 15);
            this.indexLabel.TabIndex = 3;
            this.indexLabel.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(157, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Length:";
            // 
            // lengthLabel
            // 
            this.lengthLabel.AutoSize = true;
            this.lengthLabel.Location = new System.Drawing.Point(209, 0);
            this.lengthLabel.Name = "lengthLabel";
            this.lengthLabel.Size = new System.Drawing.Size(13, 15);
            this.lengthLabel.TabIndex = 5;
            this.lengthLabel.Text = "0";
            // 
            // splitContainer1
            // 
            this.matchResultTableLayoutPanel.SetColumnSpan(this.splitContainer1, 7);
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 34);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.groupsPanel
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupsDataGridView);
            // 
            // splitContainer1.valuePanel
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Size = new System.Drawing.Size(794, 413);
            this.splitContainer1.SplitterDistance = 350;
            this.splitContainer1.TabIndex = 6;
            // 
            // groupsDataGridView
            // 
            this.groupsDataGridView.AllowUserToAddRows = false;
            this.groupsDataGridView.AllowUserToDeleteRows = false;
            this.groupsDataGridView.AllowUserToOrderColumns = true;
            this.groupsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.groupsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.groupNameColumn,
            this.groupSuccessColumn,
            this.groupIndexColumn,
            this.groupLengthColumn,
            this.groupRawValueColumn,
            this.groupXmlEncodedColumn,
            this.groupUriEncodedValueColumn});
            this.groupsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupsDataGridView.Location = new System.Drawing.Point(0, 0);
            this.groupsDataGridView.Name = "groupsDataGridView";
            this.groupsDataGridView.ReadOnly = true;
            this.groupsDataGridView.RowTemplate.Height = 25;
            this.groupsDataGridView.Size = new System.Drawing.Size(350, 413);
            this.groupsDataGridView.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.valueTextBox, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.groupNameHeadingLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupNameValueLabel, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupSuccessHeadingLabel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.groupSuccessValueLabel, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.groupIndexHeadingLabel, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.groupIndexValueLabel, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.groupLengthHeadingLabel, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.groupLengthValueLabel, 5, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(440, 413);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // rawValueRadioButton
            // 
            this.rawValueRadioButton.AutoSize = true;
            this.rawValueRadioButton.Checked = true;
            this.rawValueRadioButton.Location = new System.Drawing.Point(3, 3);
            this.rawValueRadioButton.Name = "rawValueRadioButton";
            this.rawValueRadioButton.Size = new System.Drawing.Size(78, 19);
            this.rawValueRadioButton.TabIndex = 0;
            this.rawValueRadioButton.TabStop = true;
            this.rawValueRadioButton.Text = "Raw Value";
            this.rawValueRadioButton.UseVisualStyleBackColor = true;
            this.rawValueRadioButton.CheckedChanged += new System.EventHandler(this.rawValueRadioButton_CheckedChanged);
            // 
            // uriEncodedRadioButton
            // 
            this.uriEncodedRadioButton.AutoSize = true;
            this.uriEncodedRadioButton.Location = new System.Drawing.Point(193, 3);
            this.uriEncodedRadioButton.Name = "uriEncodedRadioButton";
            this.uriEncodedRadioButton.Size = new System.Drawing.Size(94, 19);
            this.uriEncodedRadioButton.TabIndex = 1;
            this.uriEncodedRadioButton.Text = "URI-Encoded";
            this.uriEncodedRadioButton.UseVisualStyleBackColor = true;
            this.uriEncodedRadioButton.CheckedChanged += new System.EventHandler(this.uriEncodedRadioButton_CheckedChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel1, 6);
            this.flowLayoutPanel1.Controls.Add(this.rawValueRadioButton);
            this.flowLayoutPanel1.Controls.Add(this.xmlEncodedRadioButton);
            this.flowLayoutPanel1.Controls.Add(this.uriEncodedRadioButton);
            this.flowLayoutPanel1.Controls.Add(this.hexValuesRadioButton);
            this.flowLayoutPanel1.Controls.Add(this.wordWrapCheckBox);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(434, 25);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // xmlEncodedRadioButton
            // 
            this.xmlEncodedRadioButton.AutoSize = true;
            this.xmlEncodedRadioButton.Location = new System.Drawing.Point(87, 3);
            this.xmlEncodedRadioButton.Name = "xmlEncodedRadioButton";
            this.xmlEncodedRadioButton.Size = new System.Drawing.Size(100, 19);
            this.xmlEncodedRadioButton.TabIndex = 2;
            this.xmlEncodedRadioButton.Text = "XML-Encoded";
            this.xmlEncodedRadioButton.UseVisualStyleBackColor = true;
            this.xmlEncodedRadioButton.CheckedChanged += new System.EventHandler(this.xmlEncodedRadioButton_CheckedChanged);
            // 
            // wordWrapCheckBox
            // 
            this.wordWrapCheckBox.AutoSize = true;
            this.wordWrapCheckBox.Checked = true;
            this.wordWrapCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.wordWrapCheckBox.Location = new System.Drawing.Point(345, 3);
            this.wordWrapCheckBox.Name = "wordWrapCheckBox";
            this.wordWrapCheckBox.Size = new System.Drawing.Size(86, 19);
            this.wordWrapCheckBox.TabIndex = 3;
            this.wordWrapCheckBox.Text = "Word Wrap";
            this.wordWrapCheckBox.UseVisualStyleBackColor = true;
            this.wordWrapCheckBox.CheckedChanged += new System.EventHandler(this.wordWrapCheckBox_CheckedChanged);
            // 
            // valueTextBox
            // 
            this.valueTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.tableLayoutPanel1.SetColumnSpan(this.valueTextBox, 6);
            this.valueTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.valueTextBox.Location = new System.Drawing.Point(3, 64);
            this.valueTextBox.Multiline = true;
            this.valueTextBox.Name = "valueTextBox";
            this.valueTextBox.ReadOnly = true;
            this.valueTextBox.Size = new System.Drawing.Size(434, 346);
            this.valueTextBox.TabIndex = 3;
            // 
            // hexValuesRadioButton
            // 
            this.hexValuesRadioButton.AutoSize = true;
            this.hexValuesRadioButton.Location = new System.Drawing.Point(293, 3);
            this.hexValuesRadioButton.Name = "hexValuesRadioButton";
            this.hexValuesRadioButton.Size = new System.Drawing.Size(46, 19);
            this.hexValuesRadioButton.TabIndex = 4;
            this.hexValuesRadioButton.TabStop = true;
            this.hexValuesRadioButton.Text = "Hex";
            this.hexValuesRadioButton.UseVisualStyleBackColor = true;
            this.hexValuesRadioButton.CheckedChanged += new System.EventHandler(this.hexValuesRadioButton_CheckedChanged);
            // 
            // groupNameColumn
            // 
            this.groupNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.groupNameColumn.DataPropertyName = "Name";
            this.groupNameColumn.HeaderText = "Name";
            this.groupNameColumn.Name = "groupNameColumn";
            this.groupNameColumn.ReadOnly = true;
            this.groupNameColumn.Width = 64;
            // 
            // groupSuccessColumn
            // 
            this.groupSuccessColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.groupSuccessColumn.DataPropertyName = "Success";
            this.groupSuccessColumn.HeaderText = "Success";
            this.groupSuccessColumn.Name = "groupSuccessColumn";
            this.groupSuccessColumn.ReadOnly = true;
            this.groupSuccessColumn.Width = 5;
            // 
            // groupIndexColumn
            // 
            this.groupIndexColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.groupIndexColumn.DataPropertyName = "Index";
            this.groupIndexColumn.HeaderText = "Index";
            this.groupIndexColumn.Name = "groupIndexColumn";
            this.groupIndexColumn.ReadOnly = true;
            this.groupIndexColumn.Width = 5;
            // 
            // groupLengthColumn
            // 
            this.groupLengthColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.groupLengthColumn.DataPropertyName = "Length";
            this.groupLengthColumn.HeaderText = "Length";
            this.groupLengthColumn.Name = "groupLengthColumn";
            this.groupLengthColumn.ReadOnly = true;
            this.groupLengthColumn.Width = 5;
            // 
            // groupRawValueColumn
            // 
            this.groupRawValueColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.groupRawValueColumn.DataPropertyName = "RawValue";
            this.groupRawValueColumn.HeaderText = "Value";
            this.groupRawValueColumn.Name = "groupRawValueColumn";
            this.groupRawValueColumn.ReadOnly = true;
            // 
            // groupXmlEncodedColumn
            // 
            this.groupXmlEncodedColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.groupXmlEncodedColumn.DataPropertyName = "XmlEncodedValue";
            this.groupXmlEncodedColumn.HeaderText = "Value";
            this.groupXmlEncodedColumn.Name = "groupXmlEncodedColumn";
            this.groupXmlEncodedColumn.ReadOnly = true;
            this.groupXmlEncodedColumn.Visible = false;
            // 
            // groupUriEncodedValueColumn
            // 
            this.groupUriEncodedValueColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.groupUriEncodedValueColumn.DataPropertyName = "UriEncodedValue";
            this.groupUriEncodedValueColumn.HeaderText = "Value";
            this.groupUriEncodedValueColumn.Name = "groupUriEncodedValueColumn";
            this.groupUriEncodedValueColumn.ReadOnly = true;
            this.groupUriEncodedValueColumn.Visible = false;
            // 
            // showMatchValueButton
            // 
            this.showMatchValueButton.AutoSize = true;
            this.showMatchValueButton.Location = new System.Drawing.Point(683, 3);
            this.showMatchValueButton.Name = "showMatchValueButton";
            this.showMatchValueButton.Size = new System.Drawing.Size(114, 25);
            this.showMatchValueButton.TabIndex = 7;
            this.showMatchValueButton.Text = "Show Match Value";
            this.showMatchValueButton.UseVisualStyleBackColor = true;
            this.showMatchValueButton.Click += new System.EventHandler(this.showMatchValueButton_Click);
            // 
            // groupNameHeadingLabel
            // 
            this.groupNameHeadingLabel.AutoSize = true;
            this.groupNameHeadingLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.groupNameHeadingLabel.Location = new System.Drawing.Point(3, 31);
            this.groupNameHeadingLabel.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.groupNameHeadingLabel.Name = "groupNameHeadingLabel";
            this.groupNameHeadingLabel.Size = new System.Drawing.Size(43, 15);
            this.groupNameHeadingLabel.TabIndex = 4;
            this.groupNameHeadingLabel.Text = "Name:";
            this.groupNameHeadingLabel.Visible = false;
            // 
            // groupNameValueLabel
            // 
            this.groupNameValueLabel.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.groupNameValueLabel, 5);
            this.groupNameValueLabel.Location = new System.Drawing.Point(59, 31);
            this.groupNameValueLabel.Name = "groupNameValueLabel";
            this.groupNameValueLabel.Size = new System.Drawing.Size(13, 15);
            this.groupNameValueLabel.TabIndex = 5;
            this.groupNameValueLabel.Text = "0";
            this.groupNameValueLabel.Visible = false;
            // 
            // groupSuccessHeadingLabel
            // 
            this.groupSuccessHeadingLabel.AutoSize = true;
            this.groupSuccessHeadingLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.groupSuccessHeadingLabel.Location = new System.Drawing.Point(3, 46);
            this.groupSuccessHeadingLabel.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.groupSuccessHeadingLabel.Name = "groupSuccessHeadingLabel";
            this.groupSuccessHeadingLabel.Size = new System.Drawing.Size(53, 15);
            this.groupSuccessHeadingLabel.TabIndex = 6;
            this.groupSuccessHeadingLabel.Text = "Success:";
            this.groupSuccessHeadingLabel.Visible = false;
            // 
            // groupSuccessValueLabel
            // 
            this.groupSuccessValueLabel.AutoSize = true;
            this.groupSuccessValueLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.groupSuccessValueLabel.ForeColor = System.Drawing.Color.Green;
            this.groupSuccessValueLabel.Location = new System.Drawing.Point(59, 46);
            this.groupSuccessValueLabel.Name = "groupSuccessValueLabel";
            this.groupSuccessValueLabel.Size = new System.Drawing.Size(28, 15);
            this.groupSuccessValueLabel.TabIndex = 7;
            this.groupSuccessValueLabel.Text = "true";
            this.groupSuccessValueLabel.Visible = false;
            // 
            // groupIndexHeadingLabel
            // 
            this.groupIndexHeadingLabel.AutoSize = true;
            this.groupIndexHeadingLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.groupIndexHeadingLabel.Location = new System.Drawing.Point(93, 46);
            this.groupIndexHeadingLabel.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.groupIndexHeadingLabel.Name = "groupIndexHeadingLabel";
            this.groupIndexHeadingLabel.Size = new System.Drawing.Size(42, 15);
            this.groupIndexHeadingLabel.TabIndex = 8;
            this.groupIndexHeadingLabel.Text = "Index:";
            this.groupIndexHeadingLabel.Visible = false;
            // 
            // groupIndexValueLabel
            // 
            this.groupIndexValueLabel.AutoSize = true;
            this.groupIndexValueLabel.Location = new System.Drawing.Point(138, 46);
            this.groupIndexValueLabel.Name = "groupIndexValueLabel";
            this.groupIndexValueLabel.Size = new System.Drawing.Size(13, 15);
            this.groupIndexValueLabel.TabIndex = 9;
            this.groupIndexValueLabel.Text = "0";
            this.groupIndexValueLabel.Visible = false;
            // 
            // groupLengthHeadingLabel
            // 
            this.groupLengthHeadingLabel.AutoSize = true;
            this.groupLengthHeadingLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.groupLengthHeadingLabel.Location = new System.Drawing.Point(157, 46);
            this.groupLengthHeadingLabel.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.groupLengthHeadingLabel.Name = "groupLengthHeadingLabel";
            this.groupLengthHeadingLabel.Size = new System.Drawing.Size(49, 15);
            this.groupLengthHeadingLabel.TabIndex = 10;
            this.groupLengthHeadingLabel.Text = "Length:";
            this.groupLengthHeadingLabel.Visible = false;
            // 
            // groupLengthValueLabel
            // 
            this.groupLengthValueLabel.AutoSize = true;
            this.groupLengthValueLabel.Location = new System.Drawing.Point(209, 46);
            this.groupLengthValueLabel.Name = "groupLengthValueLabel";
            this.groupLengthValueLabel.Size = new System.Drawing.Size(13, 15);
            this.groupLengthValueLabel.TabIndex = 11;
            this.groupLengthValueLabel.Text = "0";
            this.groupLengthValueLabel.Visible = false;
            // 
            // SingleMatchResultForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.matchResultTableLayoutPanel);
            this.Name = "SingleMatchResultForm";
            this.Text = "Match Result";
            this.matchResultTableLayoutPanel.ResumeLayout(false);
            this.matchResultTableLayoutPanel.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupsDataGridView)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel matchResultTableLayoutPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label successLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label indexLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lengthLabel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView groupsDataGridView;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.RadioButton rawValueRadioButton;
        private System.Windows.Forms.RadioButton xmlEncodedRadioButton;
        private System.Windows.Forms.RadioButton uriEncodedRadioButton;
        private System.Windows.Forms.CheckBox wordWrapCheckBox;
        private System.Windows.Forms.TextBox valueTextBox;
        private System.Windows.Forms.RadioButton hexValuesRadioButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupSuccessColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupIndexColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupLengthColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupRawValueColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupXmlEncodedColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupUriEncodedValueColumn;
        private System.Windows.Forms.Button showMatchValueButton;
        private System.Windows.Forms.Label groupNameHeadingLabel;
        private System.Windows.Forms.Label groupNameValueLabel;
        private System.Windows.Forms.Label groupSuccessHeadingLabel;
        private System.Windows.Forms.Label groupSuccessValueLabel;
        private System.Windows.Forms.Label groupIndexHeadingLabel;
        private System.Windows.Forms.Label groupIndexValueLabel;
        private System.Windows.Forms.Label groupLengthHeadingLabel;
        private System.Windows.Forms.Label groupLengthValueLabel;
    }
}