
namespace DevHelperGUI
{
    partial class MatchCollectionResultForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.matchCollectionDataGridView = new System.Windows.Forms.DataGridView();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.seletedMatchGroupBox = new System.Windows.Forms.GroupBox();
            this.seletedGroupGroupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.matchSuccessLabel = new System.Windows.Forms.Label();
            this.matchIndexLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.matchLengthLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.matchValueTextBox = new System.Windows.Forms.TextBox();
            this.groupsDataGridView = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupSuccessLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupIndexLabel = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.groupLengthLabel = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.groupValueTextBox = new System.Windows.Forms.TextBox();
            this.matchIndexColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.matchSuccessColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.matchLengthColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.matchValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupSuccessColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupIndexColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupLengthColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.matchCollectionDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.seletedMatchGroupBox.SuspendLayout();
            this.seletedGroupGroupBox.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupsDataGridView)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.matchCollectionDataGridView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(800, 450);
            this.splitContainer1.SplitterDistance = 266;
            this.splitContainer1.TabIndex = 0;
            // 
            // matchCollectionDataGridView
            // 
            this.matchCollectionDataGridView.AllowUserToAddRows = false;
            this.matchCollectionDataGridView.AllowUserToDeleteRows = false;
            this.matchCollectionDataGridView.AllowUserToOrderColumns = true;
            this.matchCollectionDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.matchCollectionDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.matchIndexColumn,
            this.matchSuccessColumn,
            this.matchLengthColumn,
            this.matchValueColumn});
            this.matchCollectionDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.matchCollectionDataGridView.Location = new System.Drawing.Point(0, 0);
            this.matchCollectionDataGridView.Name = "matchCollectionDataGridView";
            this.matchCollectionDataGridView.ReadOnly = true;
            this.matchCollectionDataGridView.RowTemplate.Height = 25;
            this.matchCollectionDataGridView.Size = new System.Drawing.Size(266, 450);
            this.matchCollectionDataGridView.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.seletedMatchGroupBox);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.seletedGroupGroupBox);
            this.splitContainer2.Size = new System.Drawing.Size(530, 450);
            this.splitContainer2.SplitterDistance = 275;
            this.splitContainer2.TabIndex = 0;
            // 
            // seletedMatchGroupBox
            // 
            this.seletedMatchGroupBox.AutoSize = true;
            this.seletedMatchGroupBox.Controls.Add(this.tableLayoutPanel1);
            this.seletedMatchGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.seletedMatchGroupBox.Location = new System.Drawing.Point(0, 0);
            this.seletedMatchGroupBox.Name = "seletedMatchGroupBox";
            this.seletedMatchGroupBox.Size = new System.Drawing.Size(530, 275);
            this.seletedMatchGroupBox.TabIndex = 0;
            this.seletedMatchGroupBox.TabStop = false;
            this.seletedMatchGroupBox.Text = "Selected Match";
            // 
            // seletedGroupGroupBox
            // 
            this.seletedGroupGroupBox.AutoSize = true;
            this.seletedGroupGroupBox.Controls.Add(this.tableLayoutPanel2);
            this.seletedGroupGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.seletedGroupGroupBox.Location = new System.Drawing.Point(0, 0);
            this.seletedGroupGroupBox.Name = "seletedGroupGroupBox";
            this.seletedGroupGroupBox.Size = new System.Drawing.Size(530, 171);
            this.seletedGroupGroupBox.TabIndex = 0;
            this.seletedGroupGroupBox.TabStop = false;
            this.seletedGroupGroupBox.Text = "Selected Group";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.matchSuccessLabel, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.matchIndexLabel);
            this.tableLayoutPanel1.Controls.Add(this.label3, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.matchLengthLabel, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.matchValueTextBox, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.groupsDataGridView, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 19);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(524, 253);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Success:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(177, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Index:";
            // 
            // matchSuccessLabel
            // 
            this.matchSuccessLabel.AutoSize = true;
            this.matchSuccessLabel.Location = new System.Drawing.Point(90, 0);
            this.matchSuccessLabel.Name = "matchSuccessLabel";
            this.matchSuccessLabel.Size = new System.Drawing.Size(28, 15);
            this.matchSuccessLabel.TabIndex = 2;
            this.matchSuccessLabel.Text = "true";
            // 
            // matchIndexLabel
            // 
            this.matchIndexLabel.AutoSize = true;
            this.matchIndexLabel.Location = new System.Drawing.Point(264, 0);
            this.matchIndexLabel.Name = "matchIndexLabel";
            this.matchIndexLabel.Size = new System.Drawing.Size(13, 15);
            this.matchIndexLabel.TabIndex = 3;
            this.matchIndexLabel.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(351, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Length:";
            // 
            // matchLengthLabel
            // 
            this.matchLengthLabel.AutoSize = true;
            this.matchLengthLabel.Location = new System.Drawing.Point(438, 0);
            this.matchLengthLabel.Name = "matchLengthLabel";
            this.matchLengthLabel.Size = new System.Drawing.Size(13, 15);
            this.matchLengthLabel.TabIndex = 5;
            this.matchLengthLabel.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "Value:";
            // 
            // matchValueTextBox
            // 
            this.matchValueTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.tableLayoutPanel1.SetColumnSpan(this.matchValueTextBox, 6);
            this.matchValueTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.matchValueTextBox.Location = new System.Drawing.Point(3, 33);
            this.matchValueTextBox.Multiline = true;
            this.matchValueTextBox.Name = "matchValueTextBox";
            this.matchValueTextBox.ReadOnly = true;
            this.matchValueTextBox.Size = new System.Drawing.Size(518, 83);
            this.matchValueTextBox.TabIndex = 7;
            // 
            // groupsDataGridView
            // 
            this.groupsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.groupsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.groupNameColumn,
            this.groupSuccessColumn,
            this.groupIndexColumn,
            this.groupLengthColumn,
            this.groupValueColumn});
            this.tableLayoutPanel1.SetColumnSpan(this.groupsDataGridView, 6);
            this.groupsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupsDataGridView.Location = new System.Drawing.Point(3, 122);
            this.groupsDataGridView.Name = "groupsDataGridView";
            this.groupsDataGridView.RowTemplate.Height = 25;
            this.groupsDataGridView.Size = new System.Drawing.Size(518, 128);
            this.groupsDataGridView.TabIndex = 8;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 6;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.Controls.Add(this.groupSuccessLabel, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label6, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.groupIndexLabel, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label8, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label9, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.groupLengthLabel, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.label11, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.groupValueTextBox, 0, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 19);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(524, 149);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // groupSuccessLabel
            // 
            this.groupSuccessLabel.AutoSize = true;
            this.groupSuccessLabel.Location = new System.Drawing.Point(90, 0);
            this.groupSuccessLabel.Name = "groupSuccessLabel";
            this.groupSuccessLabel.Size = new System.Drawing.Size(28, 15);
            this.groupSuccessLabel.TabIndex = 0;
            this.groupSuccessLabel.Text = "true";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(177, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 15);
            this.label6.TabIndex = 1;
            this.label6.Text = "Index:";
            // 
            // groupIndexLabel
            // 
            this.groupIndexLabel.AutoSize = true;
            this.groupIndexLabel.Location = new System.Drawing.Point(264, 0);
            this.groupIndexLabel.Name = "groupIndexLabel";
            this.groupIndexLabel.Size = new System.Drawing.Size(13, 15);
            this.groupIndexLabel.TabIndex = 2;
            this.groupIndexLabel.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(51, 15);
            this.label8.TabIndex = 3;
            this.label8.Text = "Success:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(351, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 15);
            this.label9.TabIndex = 4;
            this.label9.Text = "Length:";
            // 
            // groupLengthLabel
            // 
            this.groupLengthLabel.AutoSize = true;
            this.groupLengthLabel.Location = new System.Drawing.Point(438, 0);
            this.groupLengthLabel.Name = "groupLengthLabel";
            this.groupLengthLabel.Size = new System.Drawing.Size(13, 15);
            this.groupLengthLabel.TabIndex = 5;
            this.groupLengthLabel.Text = "0";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 15);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(38, 15);
            this.label11.TabIndex = 6;
            this.label11.Text = "Value:";
            // 
            // groupValueTextBox
            // 
            this.groupValueTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.tableLayoutPanel2.SetColumnSpan(this.groupValueTextBox, 6);
            this.groupValueTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupValueTextBox.Location = new System.Drawing.Point(3, 33);
            this.groupValueTextBox.Multiline = true;
            this.groupValueTextBox.Name = "groupValueTextBox";
            this.groupValueTextBox.ReadOnly = true;
            this.groupValueTextBox.Size = new System.Drawing.Size(518, 113);
            this.groupValueTextBox.TabIndex = 7;
            // 
            // matchIndexColumn
            // 
            this.matchIndexColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.matchIndexColumn.HeaderText = "Index";
            this.matchIndexColumn.Name = "matchIndexColumn";
            this.matchIndexColumn.ReadOnly = true;
            this.matchIndexColumn.Width = 61;
            // 
            // matchSuccessColumn
            // 
            this.matchSuccessColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.matchSuccessColumn.HeaderText = "Success";
            this.matchSuccessColumn.Name = "matchSuccessColumn";
            this.matchSuccessColumn.ReadOnly = true;
            this.matchSuccessColumn.Width = 5;
            // 
            // matchLengthColumn
            // 
            this.matchLengthColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.matchLengthColumn.HeaderText = "Length";
            this.matchLengthColumn.Name = "matchLengthColumn";
            this.matchLengthColumn.ReadOnly = true;
            this.matchLengthColumn.Width = 5;
            // 
            // matchValueColumn
            // 
            this.matchValueColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.matchValueColumn.HeaderText = "Value";
            this.matchValueColumn.Name = "matchValueColumn";
            this.matchValueColumn.ReadOnly = true;
            // 
            // groupNameColumn
            // 
            this.groupNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.groupNameColumn.HeaderText = "Name";
            this.groupNameColumn.Name = "groupNameColumn";
            this.groupNameColumn.ReadOnly = true;
            this.groupNameColumn.Width = 64;
            // 
            // groupSuccessColumn
            // 
            this.groupSuccessColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.groupSuccessColumn.HeaderText = "Success";
            this.groupSuccessColumn.Name = "groupSuccessColumn";
            this.groupSuccessColumn.ReadOnly = true;
            this.groupSuccessColumn.Width = 21;
            // 
            // groupIndexColumn
            // 
            this.groupIndexColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.groupIndexColumn.HeaderText = "Index";
            this.groupIndexColumn.Name = "groupIndexColumn";
            this.groupIndexColumn.ReadOnly = true;
            this.groupIndexColumn.Width = 21;
            // 
            // groupLengthColumn
            // 
            this.groupLengthColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.groupLengthColumn.HeaderText = "Length";
            this.groupLengthColumn.Name = "groupLengthColumn";
            this.groupLengthColumn.ReadOnly = true;
            this.groupLengthColumn.Width = 21;
            // 
            // groupValueColumn
            // 
            this.groupValueColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.groupValueColumn.HeaderText = "Value";
            this.groupValueColumn.Name = "groupValueColumn";
            this.groupValueColumn.ReadOnly = true;
            // 
            // MatchCollectionResultForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MatchCollectionResultForm";
            this.Text = "Match Collection Result";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.matchCollectionDataGridView)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.seletedMatchGroupBox.ResumeLayout(false);
            this.seletedGroupGroupBox.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupsDataGridView)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView matchCollectionDataGridView;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox seletedMatchGroupBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox seletedGroupGroupBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label matchSuccessLabel;
        private System.Windows.Forms.Label matchIndexLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label matchLengthLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox matchValueTextBox;
        private System.Windows.Forms.DataGridView groupsDataGridView;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label groupSuccessLabel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label groupIndexLabel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label groupLengthLabel;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox groupValueTextBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn matchIndexColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn matchSuccessColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn matchLengthColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn matchValueColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupSuccessColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupIndexColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupLengthColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupValueColumn;
    }
}