using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace DevHelperGUI
{
    public partial class SingleMatchResultForm : Form
    {
        private string _rawValue = "";
        private readonly BindingList<GroupResult> _groups = new BindingList<GroupResult>();
        private bool _showSelected = false;
        public SingleMatchResultForm()
        {
            InitializeComponent();
            groupsDataGridView.AutoGenerateColumns = false;
            groupsDataGridView.DataSource = _groups;
            groupsDataGridView.SelectionChanged += GroupsDataGridView_SelectionChanged;
        }

        private void GroupsDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (groupsDataGridView.SelectedRows.Count > 0)
            {
                _showSelected = true;
                showMatchValueButton.Enabled = true;
                groupNameHeadingLabel.Visible = groupNameValueLabel.Visible = groupSuccessHeadingLabel.Visible = groupSuccessValueLabel.Visible =
                    groupIndexHeadingLabel.Visible = groupIndexValueLabel.Visible = groupLengthHeadingLabel.Visible = groupLengthValueLabel.Visible = true;
                GroupResult item = groupsDataGridView.SelectedRows[0].DataBoundItem as GroupResult;
                groupNameValueLabel.Text = item.Name;
                groupSuccessValueLabel.Text = item.Success.ToString();
                groupIndexValueLabel.Text = item.Index.ToString();
                groupLengthValueLabel.Text = item.Length.ToString();
                if (xmlEncodedRadioButton.Checked)
                    SetXmlEncodedValue();
                else if (uriEncodedRadioButton.Checked)
                    SetUriEncodedValue();
                else if (hexValuesRadioButton.Checked)
                    SetHexEncodedValue();
                else
                    SetRawValue();
            }
        }

        internal void SetResults(bool success, string rawValue, int index, int length, IEnumerable<GroupResult> groups)
        {
            successLabel.Text = success.ToString();
            indexLabel.Text = index.ToString();
            lengthLabel.Text = length.ToString();
            foreach (DataGridViewRow row in groupsDataGridView.SelectedRows.Cast<DataGridViewRow>().ToArray())
                row.Selected = false;
            _showSelected = false;
            showMatchValueButton.Enabled = false;
            groupNameHeadingLabel.Visible = groupNameValueLabel.Visible = groupSuccessHeadingLabel.Visible = groupSuccessValueLabel.Visible =
                groupIndexHeadingLabel.Visible = groupIndexValueLabel.Visible = groupLengthHeadingLabel.Visible = groupLengthValueLabel.Visible = false;
            _rawValue = rawValue;
            if (xmlEncodedRadioButton.Checked)
                SetXmlEncodedValue();
            else if (uriEncodedRadioButton.Checked)
                SetUriEncodedValue();
            else if (hexValuesRadioButton.Checked)
                SetHexEncodedValue();
            else
                SetRawValue();
            _groups.Clear();
            foreach (GroupResult g in groups)
                _groups.Add(g);
        }

        private void SetRawValue()
        {
            if (_showSelected && groupsDataGridView.SelectedRows.Count > 0)
                valueTextBox.Text = (groupsDataGridView.SelectedRows[0].DataBoundItem as GroupResult).RawValue;
            else
                valueTextBox.Text = _rawValue;
            groupXmlEncodedColumn.Visible = false;
            groupUriEncodedValueColumn.Visible = false;
            groupRawValueColumn.Visible = true;
        }

        private void SetXmlEncodedValue()
        {
            if (_showSelected && groupsDataGridView.SelectedRows.Count > 0)
                valueTextBox.Text = (groupsDataGridView.SelectedRows[0].DataBoundItem as GroupResult).XmlEncodedValue;
            else
                valueTextBox.Text = new XAttribute("A", _rawValue).ToString().Substring(2);
            groupRawValueColumn.Visible = false;
            groupUriEncodedValueColumn.Visible = false;
            groupXmlEncodedColumn.Visible = true;
        }

        private void SetUriEncodedValue()
        {
            if (_showSelected && groupsDataGridView.SelectedRows.Count > 0)
                valueTextBox.Text = (groupsDataGridView.SelectedRows[0].DataBoundItem as GroupResult).UriEncodedValue;
            else
                valueTextBox.Text = Uri.EscapeUriString(_rawValue);
            groupRawValueColumn.Visible = false;
            groupXmlEncodedColumn.Visible = false;
            groupUriEncodedValueColumn.Visible = true;
        }

        private void SetHexEncodedValue()
        {
            string text;
            if (_showSelected && groupsDataGridView.SelectedRows.Count > 0)
                text = (groupsDataGridView.SelectedRows[0].DataBoundItem as GroupResult).RawValue;
            else
                text = _rawValue;
            if (text.Length == 0)
                valueTextBox.Text = "";
            else
            {
                int index = 0;
                StringBuilder sb = new StringBuilder();
                if (text.Any(c => c > '\xfe'))
                    using (CharEnumerator enumerator = text.GetEnumerator())
                    {
                        enumerator.MoveNext();
                        sb.Append(enumerator.Current);
                        while (enumerator.MoveNext())
                            switch (++index % 16)
                            {
                                case 0:
                                    sb.Append(" ").Append(((int)enumerator.Current).ToString("x4"));
                                    break;
                                case 4:
                                case 8:
                                case 12:
                                    sb.Append("_").Append(((int)enumerator.Current).ToString("x4"));
                                    break;
                                default:
                                    sb.Append(":").Append(((int)enumerator.Current).ToString("x4"));
                                    break;
                            }
                    }
                else
                    using (CharEnumerator enumerator = text.GetEnumerator())
                    {
                        enumerator.MoveNext();
                        sb.Append(enumerator.Current);
                        while (enumerator.MoveNext())
                            switch (++index % 16)
                            {
                                case 0:
                                    sb.Append(" ").Append(((int)enumerator.Current).ToString("x2"));
                                    break;
                                case 4:
                                case 8:
                                case 12:
                                    sb.Append("_").Append(((int)enumerator.Current).ToString("x2"));
                                    break;
                                default:
                                    sb.Append(":").Append(((int)enumerator.Current).ToString("x2"));
                                    break;
                            }
                    }
                valueTextBox.Text = sb.ToString();
            }
        }

        private void rawValueRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (rawValueRadioButton.Checked)
                SetRawValue();
        }

        private void xmlEncodedRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (xmlEncodedRadioButton.Checked)
                SetXmlEncodedValue();
        }

        private void uriEncodedRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (uriEncodedRadioButton.Checked)
                SetUriEncodedValue();
        }

        private void hexValuesRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (hexValuesRadioButton.Checked)
                SetHexEncodedValue();
        }

        private void wordWrapCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            valueTextBox.WordWrap = wordWrapCheckBox.Checked;
        }

        private void showMatchValueButton_Click(object sender, EventArgs e)
        {
            showMatchValueButton.Enabled = false;
            _showSelected = false;
            groupNameHeadingLabel.Visible = groupNameValueLabel.Visible = groupSuccessHeadingLabel.Visible = groupSuccessValueLabel.Visible =
                groupIndexHeadingLabel.Visible = groupIndexValueLabel.Visible = groupLengthHeadingLabel.Visible = groupLengthValueLabel.Visible = false;
            if (xmlEncodedRadioButton.Checked)
                SetXmlEncodedValue();
            else if (uriEncodedRadioButton.Checked)
                SetUriEncodedValue();
            else if (hexValuesRadioButton.Checked)
                SetHexEncodedValue();
            else
                SetRawValue();
        }
    }
}
