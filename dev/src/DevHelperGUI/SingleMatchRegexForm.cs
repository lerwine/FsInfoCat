using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace DevHelperGUI
{
    public partial class SingleMatchRegexForm : Form
    {
        private const string ElementName_RegexTestSession = "RegexTestSession";
        private const string AttributeName_EvaluateOnChange = "EvaluateOnChange";
        private const string ElementName_ResultMode = "ResultMode";
        private const string XmlContent_BackslashEscaped = "BackslashEscaped";
        private const string AttributeName_ShowLineSeparatorsAndTabs = "ShowLineSeparatorsAndTabs";
        private const string XmlContent_QuotedLines = "QuotedLines";
        private const string XmlContent_UriEncoded = "UriEncoded";
        private const string XmlContent_XmlEncoded = "XmlEncoded";
        private const string XmlContent_HexidecimalValues = "HexidecimalValues";
        private const string XmlContent_RawValue = "RawValue";
        private const string AttributeName_ShowMatchIndex = "ShowMatchIndex";
        private const string AttributeName_ShowMatchLength = "ShowMatchLength";
        private const string AttributeName_ShowMatchValue = "ShowMatchValue";
        private const string AttributeName_ShowGroupName = "ShowGroupName";
        private const string AttributeName_ShowGroupIndex = "ShowGroupIndex";
        private const string AttributeName_ShowGroupLength = "ShowGroupLength";
        private const string AttributeName_ShowGroupValue = "ShowGroupValue";
        private const string ElementName_Pattern = "Pattern";
        private const string AttributeName_AcceptsTab = "AcceptsTab";
        private const string AttributeName_IgnoreCase = "IgnoreCase";
        private const string AttributeName_CultureInvariant = "CultureInvariant";
        private const string AttributeName_SingleLine = "SingleLine";
        private const string AttributeName_Multiline = "Multiline";
        private const string AttributeName_ExplicitCapture = "ExplicitCapture";
        private const string AttributeName_IgnorePatternWhitespace = "IgnorePatternWhitespace";
        private const string AttributeName_RightToLeft = "RightToLeft";
        private const string AttributeName_ECMAScript = "ECMAScript";
        private const string AttributeName_Compiled = "Compiled";
        private const string AttributeName_EvaluateLinesSeparately = "EvaluateLinesSeparately";
        private const string ElementName_InputText = "InputText";
        private const string AttributeName_Mode = "Mode";
        private const string XmlContent_AsIs = "AsIs";
        private RegexOptions _options = RegexOptions.None;
        private bool _ignoreOptionChange = true;
        private bool _ignoreViewModeCheckChange = true;
        private readonly DataGridViewTextBoxColumn lineResultItemNumberDataGridViewColumn;
        private readonly DataGridViewTextBoxColumn lineResultSuccessDataGridViewColumn;
        private readonly DataGridViewTextBoxColumn groupNumberDataGridViewColumn;
        private readonly DataGridViewTextBoxColumn groupNameDataGridViewColumn;
        private readonly DataGridViewTextBoxColumn groupSuccessDataGridViewColumn;
        private readonly DataGridViewTextBoxColumn groupIndexDataGridViewColumn;
        private readonly DataGridViewTextBoxColumn groupLengthDataGridViewColumn;
        private readonly DataGridViewTextBoxColumn groupRawValueDataGridViewColumn;
        private readonly DataGridViewTextBoxColumn groupEscapedValueDataGridViewColumn;
        private readonly DataGridViewTextBoxColumn groupEscapedValueLEDataGridViewColumn;
        private readonly DataGridViewTextBoxColumn groupQuotedLinesDataGridViewColumn;
        private readonly DataGridViewTextBoxColumn groupQuotedLinesLEDataGridViewColumn;
        private readonly DataGridViewTextBoxColumn groupUriEncodedValueDataGridViewColumn;
        private readonly DataGridViewTextBoxColumn groupUriEncodedValueLEDataGridViewColumn;
        private readonly DataGridViewTextBoxColumn groupXmlEncodedValueDataGridViewColumn;
        private readonly DataGridViewTextBoxColumn groupXmlEncodedValueLEDataGridViewColumn;
        private readonly DataGridViewTextBoxColumn groupHexidecimalValuesDataGridViewColumn;
        private readonly DataGridViewColumn[] _groupValueColumns;
        private readonly BindingList<MatchResult> _matchResults = new BindingList<MatchResult>();
        private readonly BindingList<GroupResult> _groupResults = new BindingList<GroupResult>();
        private Regex _parsedRegex;
        private bool _patternParsePending = false;
        private bool _evaluationPending = false;
        private CaptureItem _selectedItem;
        private string _currentSessionFileName = null;

        /*
        inputModeAsIsToolStripMenuItem
        inputModeBackslashedToolStripMenuItem
        inputModeUriEncodedToolStripMenuItem
        inputModeXmlEncodedToolStripMenuItem

        separateLinesToolStripMenuItem

        inputModeWordWrapToolStripMenuItem

        noneToolStripMenuItem
        ignoreCaseToolStripMenuItem
        multilineToolStripMenuItem
        explicitCaptureToolStripMenuItem
        compiledToolStripMenuItem
        singleLineToolStripMenuItem
        ignorePatternWhitespaceToolStripMenuItem
        rightToLeftToolStripMenuItem
        ecmaScriptToolStripMenuItem
        cultureInvariantToolStripMenuItem

        rawValueToolStripMenuItem
        escapedValueNormalToolStripMenuItem
        escapedValueTnlToolStripMenuItem
        quotedLinesNormalToolStripMenuItem
        quotedLinesTnlToolStripMenuItem
        uriEncodedNormalToolStripMenuItem
        uriEncodedNlToolStripMenuItem
        xmlEncodedNormalToolStripMenuItem
        xmlEncodedTnlToolStripMenuItem
        viewModeWordWrapToolStripMenuItem

        showMatchNumberToolStripMenuItem
        showMatchIndexToolStripMenuItem
        showMatchLengthToolStripMenuItem
        showMatchValueToolStripMenuItem

        showGroupNumberToolStripMenuItem
        showGroupNameToolStripMenuItem
        showGroupIndexToolStripMenuItem
        showGroupLengthToolStripMenuItem
        showGroupValueToolStripMenuItem

        patternTextBox
        expressionErrorLabel
        inputTextBox
        evaluateOnChangeCheckBox
        evaluateButton
        */
        public SingleMatchRegexForm()
        {
            InitializeComponent();
            lineResultItemNumberDataGridViewColumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(MatchItem.ItemNumber),
                HeaderText = "#",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true
            };
            lineResultSuccessDataGridViewColumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(CaptureItem.Success),
                HeaderText = "Success",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader,
                ReadOnly = true
            };
            lineResultDataGridView.AutoGenerateColumns = false;
            lineResultDataGridView.Columns.AddRange(new DataGridViewColumn[] { lineResultItemNumberDataGridViewColumn, lineResultSuccessDataGridViewColumn });

            groupNumberDataGridViewColumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(MatchItem.ItemNumber),
                HeaderText = "#",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader,
                Visible = false
            };
            groupNameDataGridViewColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "Name",
                DataPropertyName = nameof(GroupResult.Name),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader
            };
            groupSuccessDataGridViewColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "Success",
                DataPropertyName = nameof(GroupResult.Success),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader
            };
            groupIndexDataGridViewColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "Index",
                DataPropertyName = nameof(GroupResult.Index),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader,
                Visible = false
            };
            groupLengthDataGridViewColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "Length",
                DataPropertyName = nameof(GroupResult.Length),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader,
                Visible = false
            };
            groupRawValueDataGridViewColumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(GroupResult.RawValue)
            };
            groupEscapedValueDataGridViewColumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(GroupResult.EscapedValue)
            };
            groupEscapedValueLEDataGridViewColumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(GroupResult.EscapedValueLE)
            };
            groupQuotedLinesDataGridViewColumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(GroupResult.QuotedLines)
            };
            groupQuotedLinesLEDataGridViewColumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(GroupResult.QuotedLinesLE)
            };
            groupUriEncodedValueDataGridViewColumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(GroupResult.UriEncodedValue)
            };
            groupUriEncodedValueLEDataGridViewColumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(GroupResult.UriEncodedValueLE)
            };
            groupXmlEncodedValueDataGridViewColumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(GroupResult.XmlEncodedValue)
            };
            groupXmlEncodedValueLEDataGridViewColumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(GroupResult.XmlEncodedValueLE)
            };
            groupHexidecimalValuesDataGridViewColumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(GroupResult.HexEncoded)
            };

            _groupValueColumns = new DataGridViewColumn[] { groupRawValueDataGridViewColumn, groupEscapedValueDataGridViewColumn, groupEscapedValueLEDataGridViewColumn,
                groupQuotedLinesDataGridViewColumn, groupQuotedLinesLEDataGridViewColumn, groupUriEncodedValueDataGridViewColumn,
                groupUriEncodedValueLEDataGridViewColumn, groupXmlEncodedValueDataGridViewColumn, groupXmlEncodedValueLEDataGridViewColumn,
                groupHexidecimalValuesDataGridViewColumn };

            DataGridViewColumn[] columns = (new DataGridViewColumn[] { groupNumberDataGridViewColumn, groupNameDataGridViewColumn, groupSuccessDataGridViewColumn,
                groupIndexDataGridViewColumn, groupLengthDataGridViewColumn }).Concat(_groupValueColumns).ToArray();
            groupsDataGridView.AutoGenerateColumns = false;
            foreach (DataGridViewColumn col in _groupValueColumns)
            {
                col.HeaderText = "Value";
                col.Visible = false;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            groupsDataGridView.Columns.AddRange(columns);
            foreach (DataGridViewColumn col in columns)
                col.ReadOnly = true;
            lineResultDataGridView.DataSource = _matchResults;
            groupsDataGridView.DataSource = _groupResults;
            lineResultDataGridView.SelectionChanged += LineResultDataGridView_SelectionChanged;
            groupsDataGridView.SelectionChanged += GroupsDataGridView_SelectionChanged;
            _ignoreOptionChange = false;
            _ignoreViewModeCheckChange = false;
        }

        internal static string ExceptionTostring(Exception exception) =>
            (exception is null) ? "" : (string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message);

        private void SetRawResultMode()
        {
            valueTextBox.Text = (_selectedItem is null) ? "" : _selectedItem.RawValue;
            if (showGroupValueToolStripMenuItem.Checked)
            {
                foreach (DataGridViewColumn col in _groupValueColumns)
                    col.Visible = false;
                groupRawValueDataGridViewColumn.Visible = true;
            }
        }

        private void SetEscapedResultMode(bool escapeTabsAndNewLines)
        {
            valueTextBox.Text = (_selectedItem is null) ? "" : (escapeTabsAndNewLines ? _selectedItem.EscapedValue : _selectedItem.EscapedValueLE);
            if (showGroupValueToolStripMenuItem.Checked)
            {
                foreach (DataGridViewColumn col in _groupValueColumns)
                    col.Visible = false;
                (escapeTabsAndNewLines ? groupEscapedValueDataGridViewColumn : groupEscapedValueLEDataGridViewColumn).Visible = true;
            }
        }

        private void SetQuotedResultMode(bool escapeTabsAndNewLines)
        {
            valueTextBox.Text = (_selectedItem is null) ? "" : (escapeTabsAndNewLines ? _selectedItem.QuotedLines : _selectedItem.QuotedLinesLE);
            if (showGroupValueToolStripMenuItem.Checked)
            {
                foreach (DataGridViewColumn col in _groupValueColumns)
                    col.Visible = false;
                (escapeTabsAndNewLines ? groupQuotedLinesDataGridViewColumn : groupQuotedLinesLEDataGridViewColumn).Visible = true;
            }
        }

        private void SetUriEncodedResulMode(bool escapeTabsAndNewLines)
        {
            valueTextBox.Text = (_selectedItem is null) ? "" : (escapeTabsAndNewLines ? _selectedItem.UriEncodedValue : _selectedItem.UriEncodedValueLE);
            if (showGroupValueToolStripMenuItem.Checked)
            {
                foreach (DataGridViewColumn col in _groupValueColumns)
                    col.Visible = false;
                (escapeTabsAndNewLines ? groupUriEncodedValueDataGridViewColumn : groupUriEncodedValueLEDataGridViewColumn).Visible = true;
            }
        }

        private void SetXmlEncodedResulMode(bool escapeTabsAndNewLines)
        {
            valueTextBox.Text = (_selectedItem is null) ? "" : (escapeTabsAndNewLines ? _selectedItem.XmlEncodedValue : _selectedItem.XmlEncodedValueLE);
            if (showGroupValueToolStripMenuItem.Checked)
            {
                foreach (DataGridViewColumn col in _groupValueColumns)
                    col.Visible = false;
                (escapeTabsAndNewLines ? groupXmlEncodedValueDataGridViewColumn : groupXmlEncodedValueLEDataGridViewColumn).Visible = true;
            }
        }

        private void SetHexidecimalDisplayMode()
        {
            valueTextBox.Text = (_selectedItem is null) ? "" : _selectedItem.HexEncoded;
            if (showGroupValueToolStripMenuItem.Checked)
            {
                foreach (DataGridViewColumn col in _groupValueColumns)
                    col.Visible = false;
                groupHexidecimalValuesDataGridViewColumn.Visible = true;
            }
        }

        private void UpdateCurrentItemDisplay()
        {
            if (displayMatchPropertiesToolStripMenuItem.Checked || (_selectedItem = groupsDataGridView.SelectedRows.Cast<DataGridViewRow>().Select(r => (GroupResult)r.DataBoundItem).FirstOrDefault()) is null)
                _selectedItem = lineResultDataGridView.SelectedRows.Cast<DataGridViewRow>().Select(r => (MatchResult)r.DataBoundItem).Concat(_matchResults).FirstOrDefault();

            if (_selectedItem is null)
            {
                successLabel.Text = indexTextBox.Text = lengthTextBox.Text = valueTextBox.Text = "";
                nameLabel.Visible = nameTextBox.Visible = false;
            }
            else
            {
                if (_selectedItem is GroupResult groupResult)
                {
                    nameLabel.Visible = nameTextBox.Visible = true;
                    nameTextBox.Text = groupResult.Name;
                }
                else
                    nameLabel.Visible = nameTextBox.Visible = false;
                if (_selectedItem.Success)
                {
                    if (escapedValueNormalToolStripMenuItem.Checked)
                        SetEscapedResultMode(false);
                    else if (escapedValueTnlToolStripMenuItem.Checked)
                        SetEscapedResultMode(true);
                    else if (quotedLinesNormalToolStripMenuItem.Checked)
                        SetQuotedResultMode(false);
                    else if (quotedLinesTnlToolStripMenuItem.Checked)
                        SetQuotedResultMode(true);
                    else if (uriEncodedNormalToolStripMenuItem.Checked)
                        SetUriEncodedResulMode(false);
                    else if (uriEncodedNlToolStripMenuItem.Checked)
                        SetUriEncodedResulMode(true);
                    else if (xmlEncodedNormalToolStripMenuItem.Checked)
                        SetXmlEncodedResulMode(false);
                    else if (xmlEncodedTnlToolStripMenuItem.Checked)
                        SetXmlEncodedResulMode(true);
                    else if (hexidecimalValuesToolStripMenuItem.Checked)
                        SetHexidecimalDisplayMode();
                    else
                        SetRawResultMode();
                    successLabel.Text = "True";
                    successLabel.ForeColor = Color.Green;
                    indexTextBox.Text = _selectedItem.Index.ToString();
                    lengthTextBox.Text = _selectedItem.Length.ToString();
                }
                else
                {
                    successLabel.Text = "False";
                    successLabel.ForeColor = Color.Red;
                    indexTextBox.Text = lengthTextBox.Text = "";
                }
            }
        }

        private bool AnyFlagToolStripMenuItemsSet() => ignoreCaseToolStripMenuItem.Checked || multilineToolStripMenuItem.Checked || explicitCaptureToolStripMenuItem.Checked || compiledToolStripMenuItem.Checked ||
            singleLineToolStripMenuItem.Checked || ignorePatternWhitespaceToolStripMenuItem.Checked || rightToLeftToolStripMenuItem.Checked || ecmaScriptToolStripMenuItem.Checked || cultureInvariantToolStripMenuItem.Checked;

        private void ApplyOptionToolStripMenuItemChange(RegexOptions option, bool isSet)
        {
            if (_ignoreOptionChange)
                return;
            _ignoreOptionChange = true;
            try
            {
                if (isSet)
                {
                    noneToolStripMenuItem.Enabled = true;
                    noneToolStripMenuItem.Checked = false;
                    if (_options.HasFlag(option))
                        return;
                    _options |= option;
                }
                else if (AnyFlagToolStripMenuItemsSet())
                {
                    if (!_options.HasFlag(option))
                        return;
                    _options ^= option;
                }
                else
                {
                    noneToolStripMenuItem.Checked = true;
                    if (_options == RegexOptions.None)
                        return;
                    _options = RegexOptions.None;
                }

                StartRegexParseAsync();
            }
            finally { _ignoreOptionChange = false; }
        }

        private void GroupsDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            displayMatchPropertiesToolStripMenuItem.Checked = false;
            displayMatchPropertiesToolStripMenuItem.Enabled = true;
            UpdateCurrentItemDisplay();
        }

        private void LineResultDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            displayMatchPropertiesToolStripMenuItem.Checked = true;
            displayMatchPropertiesToolStripMenuItem.Enabled = false;
            UpdateCurrentItemDisplay();
        }

        private void InputModeAsIsToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (inputModeAsIsToolStripMenuItem.Checked)
            {
                inputModeBackslashedToolStripMenuItem.Checked = inputModeUriEncodedToolStripMenuItem.Checked = inputModeXmlEncodedToolStripMenuItem.Checked = false;
                inputModeBackslashedToolStripMenuItem.Enabled = inputModeUriEncodedToolStripMenuItem.Enabled = inputModeXmlEncodedToolStripMenuItem.Enabled = false;
                inputModeAsIsToolStripMenuItem.Enabled = false;
                _evaluationPending = true;
                if (evaluateOnChangeCheckBox.Checked)
                    StartEvaluationAsync();
                else
                    evaluateButton.Enabled = true;
            }
            else
                inputModeAsIsToolStripMenuItem.Enabled = true;
        }

        private void InputModeBackslashedToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (inputModeBackslashedToolStripMenuItem.Checked)
            {
                inputModeAsIsToolStripMenuItem.Checked = inputModeUriEncodedToolStripMenuItem.Checked = inputModeXmlEncodedToolStripMenuItem.Checked = false;
                inputModeAsIsToolStripMenuItem.Enabled = inputModeUriEncodedToolStripMenuItem.Enabled = inputModeXmlEncodedToolStripMenuItem.Enabled = false;
                inputModeBackslashedToolStripMenuItem.Enabled = false;
                _evaluationPending = true;
                if (evaluateOnChangeCheckBox.Checked)
                    StartEvaluationAsync();
                else
                    evaluateButton.Enabled = true;
            }
            else
                inputModeBackslashedToolStripMenuItem.Enabled = true;
        }

        private void InputModeUriEncodedToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (inputModeUriEncodedToolStripMenuItem.Checked)
            {
                inputModeAsIsToolStripMenuItem.Checked = inputModeBackslashedToolStripMenuItem.Checked = inputModeXmlEncodedToolStripMenuItem.Checked = false;
                inputModeAsIsToolStripMenuItem.Enabled = inputModeBackslashedToolStripMenuItem.Enabled = inputModeXmlEncodedToolStripMenuItem.Enabled = false;
                inputModeUriEncodedToolStripMenuItem.Enabled = false;
                _evaluationPending = true;
                if (evaluateOnChangeCheckBox.Checked)
                    StartEvaluationAsync();
                else
                    evaluateButton.Enabled = true;
            }
            else
                inputModeUriEncodedToolStripMenuItem.Enabled = true;
        }

        private void InputModeXmlEncodedToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (inputModeXmlEncodedToolStripMenuItem.Checked)
            {
                inputModeAsIsToolStripMenuItem.Checked = inputModeBackslashedToolStripMenuItem.Checked = inputModeUriEncodedToolStripMenuItem.Checked = false;
                inputModeAsIsToolStripMenuItem.Enabled = inputModeBackslashedToolStripMenuItem.Enabled = inputModeUriEncodedToolStripMenuItem.Enabled = true;
                inputModeXmlEncodedToolStripMenuItem.Enabled = false;
                _evaluationPending = true;
                if (evaluateOnChangeCheckBox.Checked)
                    StartEvaluationAsync();
                else
                    evaluateButton.Enabled = true;
            }
            else
                inputModeXmlEncodedToolStripMenuItem.Enabled = true;
        }

        private void SeparateLinesToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            separateLinesToolStripMenuItem.Enabled = !separateLinesToolStripMenuItem.Checked;
            _evaluationPending = true;
            if (evaluateOnChangeCheckBox.Checked)
                StartEvaluationAsync();
            else
                evaluateButton.Enabled = true;
        }

        private void InputModeWordWrapToolStripMenuItem_CheckedChanged(object sender, EventArgs e) => inputTextBox.WordWrap = inputModeWordWrapToolStripMenuItem.Checked;

        private void NoneToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (_ignoreOptionChange)
                return;
            _ignoreOptionChange = true;
            try
            {
                if (noneToolStripMenuItem.Checked)
                {
                    ignoreCaseToolStripMenuItem.Checked = multilineToolStripMenuItem.Checked = explicitCaptureToolStripMenuItem.Checked =
                        compiledToolStripMenuItem.Checked = singleLineToolStripMenuItem.Checked = ignorePatternWhitespaceToolStripMenuItem.Checked =
                        rightToLeftToolStripMenuItem.Checked = ecmaScriptToolStripMenuItem.Checked = cultureInvariantToolStripMenuItem.Checked = false;
                    ignoreCaseToolStripMenuItem.Enabled = multilineToolStripMenuItem.Enabled = explicitCaptureToolStripMenuItem.Enabled =
                        compiledToolStripMenuItem.Enabled = singleLineToolStripMenuItem.Enabled = ignorePatternWhitespaceToolStripMenuItem.Enabled =
                        rightToLeftToolStripMenuItem.Enabled = ecmaScriptToolStripMenuItem.Enabled = cultureInvariantToolStripMenuItem.Enabled = true;
                    noneToolStripMenuItem.Enabled = false;
                    if (_options != RegexOptions.None)
                    {
                        _options = RegexOptions.None;
                        StartRegexParseAsync();
                    }
                }
                else
                    noneToolStripMenuItem.Enabled = true;
            }
            finally { _ignoreOptionChange = false; }
        }

        private void IgnoreCaseToolStripMenuItem_CheckedChanged(object sender, EventArgs e) =>
            ApplyOptionToolStripMenuItemChange(RegexOptions.IgnoreCase, ignoreCaseToolStripMenuItem.Checked);

        private void CultureInvariantToolStripMenuItem_CheckedChanged(object sender, EventArgs e) =>
            ApplyOptionToolStripMenuItemChange(RegexOptions.CultureInvariant, cultureInvariantToolStripMenuItem.Checked);

        private void MultilineToolStripMenuItem_CheckedChanged(object sender, EventArgs e) =>
            ApplyOptionToolStripMenuItemChange(RegexOptions.Multiline, multilineToolStripMenuItem.Checked);

        private void SinglelineToolStripMenuItem_CheckedChanged(object sender, EventArgs e) =>
            ApplyOptionToolStripMenuItemChange(RegexOptions.Singleline, singleLineToolStripMenuItem.Checked);

        private void ExplicitCaptureToolStripMenuItem_CheckedChanged(object sender, EventArgs e) =>
            ApplyOptionToolStripMenuItemChange(RegexOptions.ExplicitCapture, explicitCaptureToolStripMenuItem.Checked);

        private void IgnorePatternWhitespaceToolStripMenuItem_CheckedChanged(object sender, EventArgs e) =>
            ApplyOptionToolStripMenuItemChange(RegexOptions.IgnorePatternWhitespace, ignorePatternWhitespaceToolStripMenuItem.Checked);

        private void RightToLeftToolStripMenuItem_CheckedChanged(object sender, EventArgs e) =>
            ApplyOptionToolStripMenuItemChange(RegexOptions.RightToLeft, rightToLeftToolStripMenuItem.Checked);

        private void ECMAScriptToolStripMenuItem_CheckedChanged(object sender, EventArgs e) =>
            ApplyOptionToolStripMenuItemChange(RegexOptions.ECMAScript, ecmaScriptToolStripMenuItem.Checked);

        private void CompiledToolStripMenuItem_CheckedChanged(object sender, EventArgs e) =>
            ApplyOptionToolStripMenuItemChange(RegexOptions.Compiled, compiledToolStripMenuItem.Checked);

        private void RawValueToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (_ignoreViewModeCheckChange)
                return;
            _ignoreViewModeCheckChange = true;
            try
            {
                if (rawValueToolStripMenuItem.Checked)
                {
                    escapedValueNormalToolStripMenuItem.Enabled = escapedValueTnlToolStripMenuItem.Enabled = quotedLinesNormalToolStripMenuItem.Enabled =
                        quotedLinesTnlToolStripMenuItem.Enabled = uriEncodedNormalToolStripMenuItem.Enabled = uriEncodedNlToolStripMenuItem.Enabled =
                        xmlEncodedNormalToolStripMenuItem.Enabled = xmlEncodedTnlToolStripMenuItem.Enabled = hexidecimalValuesToolStripMenuItem.Enabled = true;
                    escapedValueNormalToolStripMenuItem.Checked = escapedValueTnlToolStripMenuItem.Checked = quotedLinesNormalToolStripMenuItem.Checked =
                        quotedLinesTnlToolStripMenuItem.Checked = uriEncodedNormalToolStripMenuItem.Checked = uriEncodedNlToolStripMenuItem.Checked =
                        xmlEncodedNormalToolStripMenuItem.Checked = xmlEncodedTnlToolStripMenuItem.Checked = hexidecimalValuesToolStripMenuItem.Checked = false;
                    rawValueToolStripMenuItem.Enabled = false;
                    SetRawResultMode();
                }
                else
                    rawValueToolStripMenuItem.Enabled = true;
            }
            finally { _ignoreViewModeCheckChange = false; }
        }

        private void EscapedValueNormalToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (_ignoreViewModeCheckChange)
                return;
            _ignoreViewModeCheckChange = true;
            try
            {
                if (escapedValueNormalToolStripMenuItem.Checked)
                {
                    rawValueToolStripMenuItem.Enabled = escapedValueTnlToolStripMenuItem.Enabled = quotedLinesNormalToolStripMenuItem.Enabled =
                        quotedLinesTnlToolStripMenuItem.Enabled = uriEncodedNormalToolStripMenuItem.Enabled = uriEncodedNlToolStripMenuItem.Enabled =
                        xmlEncodedNormalToolStripMenuItem.Enabled = xmlEncodedTnlToolStripMenuItem.Enabled = hexidecimalValuesToolStripMenuItem.Enabled = true;
                    rawValueToolStripMenuItem.Checked = escapedValueTnlToolStripMenuItem.Checked = quotedLinesNormalToolStripMenuItem.Checked =
                        quotedLinesTnlToolStripMenuItem.Checked = uriEncodedNormalToolStripMenuItem.Checked = uriEncodedNlToolStripMenuItem.Checked =
                        xmlEncodedNormalToolStripMenuItem.Checked = xmlEncodedTnlToolStripMenuItem.Checked = hexidecimalValuesToolStripMenuItem.Checked = false;
                    escapedValueNormalToolStripMenuItem.Enabled = false;
                    SetEscapedResultMode(false);
                }
                else
                    escapedValueNormalToolStripMenuItem.Enabled = true;
            }
            finally { _ignoreViewModeCheckChange = false; }
        }

        private void EscapedValueTnlToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (_ignoreViewModeCheckChange)
                return;
            _ignoreViewModeCheckChange = true;
            try
            {
                if (escapedValueTnlToolStripMenuItem.Checked)
                {
                    rawValueToolStripMenuItem.Enabled = escapedValueNormalToolStripMenuItem.Enabled = quotedLinesNormalToolStripMenuItem.Enabled =
                        quotedLinesTnlToolStripMenuItem.Enabled = uriEncodedNormalToolStripMenuItem.Enabled = uriEncodedNlToolStripMenuItem.Enabled =
                        xmlEncodedNormalToolStripMenuItem.Enabled = xmlEncodedTnlToolStripMenuItem.Enabled = hexidecimalValuesToolStripMenuItem.Enabled = true;
                    rawValueToolStripMenuItem.Checked = escapedValueNormalToolStripMenuItem.Checked = quotedLinesNormalToolStripMenuItem.Checked =
                        quotedLinesTnlToolStripMenuItem.Checked = uriEncodedNormalToolStripMenuItem.Checked = uriEncodedNlToolStripMenuItem.Checked =
                        xmlEncodedNormalToolStripMenuItem.Checked = xmlEncodedTnlToolStripMenuItem.Checked = hexidecimalValuesToolStripMenuItem.Checked = false;
                    escapedValueTnlToolStripMenuItem.Enabled = false;
                    SetEscapedResultMode(true);
                }
                else
                    escapedValueTnlToolStripMenuItem.Enabled = true;
            }
            finally { _ignoreViewModeCheckChange = false; }
        }

        private void QuotedLinesNormalToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (_ignoreViewModeCheckChange)
                return;
            _ignoreViewModeCheckChange = true;
            try
            {
                if (quotedLinesNormalToolStripMenuItem.Checked)
                {
                    rawValueToolStripMenuItem.Enabled = escapedValueNormalToolStripMenuItem.Enabled = escapedValueTnlToolStripMenuItem.Enabled =
                        quotedLinesTnlToolStripMenuItem.Enabled = uriEncodedNormalToolStripMenuItem.Enabled = uriEncodedNlToolStripMenuItem.Enabled =
                        xmlEncodedNormalToolStripMenuItem.Enabled = xmlEncodedTnlToolStripMenuItem.Enabled = hexidecimalValuesToolStripMenuItem.Enabled = true;
                    rawValueToolStripMenuItem.Checked = escapedValueNormalToolStripMenuItem.Checked = escapedValueTnlToolStripMenuItem.Checked =
                        quotedLinesTnlToolStripMenuItem.Checked = uriEncodedNormalToolStripMenuItem.Checked = uriEncodedNlToolStripMenuItem.Checked =
                        xmlEncodedNormalToolStripMenuItem.Checked = xmlEncodedTnlToolStripMenuItem.Checked = hexidecimalValuesToolStripMenuItem.Checked = false;
                    quotedLinesNormalToolStripMenuItem.Enabled = false;
                    SetQuotedResultMode(false);
                }
                else
                    quotedLinesNormalToolStripMenuItem.Enabled = true;
            }
            finally { _ignoreViewModeCheckChange = false; }
        }

        private void QuotedLinesTnlToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (_ignoreViewModeCheckChange)
                return;
            _ignoreViewModeCheckChange = true;
            try
            {
                if (quotedLinesTnlToolStripMenuItem.Checked)
                {
                    rawValueToolStripMenuItem.Enabled = escapedValueNormalToolStripMenuItem.Enabled = escapedValueTnlToolStripMenuItem.Enabled =
                        quotedLinesNormalToolStripMenuItem.Enabled = uriEncodedNormalToolStripMenuItem.Enabled = uriEncodedNlToolStripMenuItem.Enabled =
                        xmlEncodedNormalToolStripMenuItem.Enabled = xmlEncodedTnlToolStripMenuItem.Enabled = hexidecimalValuesToolStripMenuItem.Enabled = true;
                    rawValueToolStripMenuItem.Checked = escapedValueNormalToolStripMenuItem.Checked = escapedValueTnlToolStripMenuItem.Checked =
                        quotedLinesNormalToolStripMenuItem.Checked = uriEncodedNormalToolStripMenuItem.Checked = uriEncodedNlToolStripMenuItem.Checked =
                        xmlEncodedNormalToolStripMenuItem.Checked = xmlEncodedTnlToolStripMenuItem.Checked = hexidecimalValuesToolStripMenuItem.Checked = false;
                    quotedLinesTnlToolStripMenuItem.Enabled = false;
                    SetQuotedResultMode(true);
                }
                else
                    quotedLinesTnlToolStripMenuItem.Enabled = true;
            }
            finally { _ignoreViewModeCheckChange = false; }
        }

        private void UriEncodedNormalToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (_ignoreViewModeCheckChange)
                return;
            _ignoreViewModeCheckChange = true;
            try
            {
                if (uriEncodedNormalToolStripMenuItem.Checked)
                {
                    rawValueToolStripMenuItem.Enabled = escapedValueNormalToolStripMenuItem.Enabled = escapedValueTnlToolStripMenuItem.Enabled =
                        quotedLinesNormalToolStripMenuItem.Enabled = quotedLinesTnlToolStripMenuItem.Enabled = uriEncodedNlToolStripMenuItem.Enabled =
                        xmlEncodedNormalToolStripMenuItem.Enabled = xmlEncodedTnlToolStripMenuItem.Enabled = hexidecimalValuesToolStripMenuItem.Enabled = true;
                    rawValueToolStripMenuItem.Checked = escapedValueNormalToolStripMenuItem.Checked = escapedValueTnlToolStripMenuItem.Checked =
                        quotedLinesNormalToolStripMenuItem.Checked = quotedLinesTnlToolStripMenuItem.Checked = uriEncodedNlToolStripMenuItem.Checked =
                        xmlEncodedNormalToolStripMenuItem.Checked = xmlEncodedTnlToolStripMenuItem.Checked = hexidecimalValuesToolStripMenuItem.Checked = false;
                    uriEncodedNormalToolStripMenuItem.Enabled = false;
                    SetUriEncodedResulMode(false);
                }
                else
                    uriEncodedNormalToolStripMenuItem.Enabled = true;
            }
            finally { _ignoreViewModeCheckChange = false; }
        }

        private void UriEncodedNlToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (_ignoreViewModeCheckChange)
                return;
            _ignoreViewModeCheckChange = true;
            try
            {
                if (uriEncodedNlToolStripMenuItem.Checked)
                {
                    rawValueToolStripMenuItem.Enabled = escapedValueNormalToolStripMenuItem.Enabled = escapedValueTnlToolStripMenuItem.Enabled =
                        quotedLinesNormalToolStripMenuItem.Enabled = quotedLinesTnlToolStripMenuItem.Enabled = uriEncodedNormalToolStripMenuItem.Enabled =
                        xmlEncodedNormalToolStripMenuItem.Enabled = xmlEncodedTnlToolStripMenuItem.Enabled = hexidecimalValuesToolStripMenuItem.Enabled = true;
                    rawValueToolStripMenuItem.Checked = escapedValueNormalToolStripMenuItem.Checked = escapedValueTnlToolStripMenuItem.Checked =
                        quotedLinesNormalToolStripMenuItem.Checked = quotedLinesTnlToolStripMenuItem.Checked = uriEncodedNormalToolStripMenuItem.Checked =
                        xmlEncodedNormalToolStripMenuItem.Checked = xmlEncodedTnlToolStripMenuItem.Checked = hexidecimalValuesToolStripMenuItem.Checked = false;
                    uriEncodedNlToolStripMenuItem.Enabled = false;
                    SetUriEncodedResulMode(true);
                }
                else
                    uriEncodedNlToolStripMenuItem.Enabled = true;
            }
            finally { _ignoreViewModeCheckChange = false; }
        }

        private void XmlEncodedNormalToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (_ignoreViewModeCheckChange)
                return;
            _ignoreViewModeCheckChange = true;
            try
            {
                if (xmlEncodedNormalToolStripMenuItem.Checked)
                {
                    rawValueToolStripMenuItem.Enabled = escapedValueNormalToolStripMenuItem.Enabled = escapedValueTnlToolStripMenuItem.Enabled =
                        quotedLinesNormalToolStripMenuItem.Enabled = quotedLinesTnlToolStripMenuItem.Enabled = uriEncodedNormalToolStripMenuItem.Enabled =
                        uriEncodedNlToolStripMenuItem.Enabled = xmlEncodedTnlToolStripMenuItem.Enabled = hexidecimalValuesToolStripMenuItem.Enabled = true;
                    rawValueToolStripMenuItem.Checked = escapedValueNormalToolStripMenuItem.Checked = escapedValueTnlToolStripMenuItem.Checked =
                        quotedLinesNormalToolStripMenuItem.Checked = quotedLinesTnlToolStripMenuItem.Checked = uriEncodedNormalToolStripMenuItem.Checked =
                        uriEncodedNlToolStripMenuItem.Checked = xmlEncodedTnlToolStripMenuItem.Checked = hexidecimalValuesToolStripMenuItem.Checked = false;
                    xmlEncodedNormalToolStripMenuItem.Enabled = false;
                    SetXmlEncodedResulMode(false);
                }
                else
                    xmlEncodedNormalToolStripMenuItem.Enabled = true;
            }
            finally { _ignoreViewModeCheckChange = false; }
        }

        private void XmlEncodedTnlToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (_ignoreViewModeCheckChange)
                return;
            _ignoreViewModeCheckChange = true;
            try
            {
                if (xmlEncodedTnlToolStripMenuItem.Checked)
                {
                    rawValueToolStripMenuItem.Enabled = escapedValueNormalToolStripMenuItem.Enabled = escapedValueTnlToolStripMenuItem.Enabled =
                        quotedLinesNormalToolStripMenuItem.Enabled = quotedLinesTnlToolStripMenuItem.Enabled = uriEncodedNormalToolStripMenuItem.Enabled =
                        uriEncodedNlToolStripMenuItem.Enabled = xmlEncodedNormalToolStripMenuItem.Enabled = hexidecimalValuesToolStripMenuItem.Enabled = true;
                    rawValueToolStripMenuItem.Checked = escapedValueNormalToolStripMenuItem.Checked = escapedValueTnlToolStripMenuItem.Checked =
                        quotedLinesNormalToolStripMenuItem.Checked = quotedLinesTnlToolStripMenuItem.Checked = uriEncodedNormalToolStripMenuItem.Checked =
                        uriEncodedNlToolStripMenuItem.Checked = xmlEncodedNormalToolStripMenuItem.Checked = hexidecimalValuesToolStripMenuItem.Checked = false;
                    xmlEncodedTnlToolStripMenuItem.Enabled = false;
                    SetXmlEncodedResulMode(true);
                }
                else
                    xmlEncodedTnlToolStripMenuItem.Enabled = true;
            }
            finally { _ignoreViewModeCheckChange = false; }
        }

        private void HexidecimalValuesToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (_ignoreViewModeCheckChange)
                return;
            _ignoreViewModeCheckChange = true;
            try
            {
                if (hexidecimalValuesToolStripMenuItem.Checked)
                {
                    rawValueToolStripMenuItem.Enabled = escapedValueNormalToolStripMenuItem.Enabled = escapedValueTnlToolStripMenuItem.Enabled =
                        quotedLinesNormalToolStripMenuItem.Enabled = quotedLinesTnlToolStripMenuItem.Enabled = uriEncodedNormalToolStripMenuItem.Enabled =
                        uriEncodedNlToolStripMenuItem.Enabled = xmlEncodedNormalToolStripMenuItem.Enabled = xmlEncodedTnlToolStripMenuItem.Enabled = true;
                    rawValueToolStripMenuItem.Checked = escapedValueNormalToolStripMenuItem.Checked = escapedValueTnlToolStripMenuItem.Checked =
                        quotedLinesNormalToolStripMenuItem.Checked = quotedLinesTnlToolStripMenuItem.Checked = uriEncodedNormalToolStripMenuItem.Checked =
                        uriEncodedNlToolStripMenuItem.Checked = xmlEncodedNormalToolStripMenuItem.Checked = xmlEncodedTnlToolStripMenuItem.Checked = false;
                    hexidecimalValuesToolStripMenuItem.Enabled = false;
                    SetHexidecimalDisplayMode();
                }
                else
                    hexidecimalValuesToolStripMenuItem.Enabled = true;
            }
            finally { _ignoreViewModeCheckChange = false; }
        }

        private void ViewModeWordWrapToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            valueTextBox.WordWrap = viewModeWordWrapToolStripMenuItem.Checked;
        }

        private void DisplayMatchPropertiesToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (displayMatchPropertiesToolStripMenuItem.Checked)
            {
                displayMatchPropertiesToolStripMenuItem.Enabled = false;
                foreach (DataGridViewRow row in groupsDataGridView.SelectedRows.Cast<DataGridViewRow>().ToArray())
                    row.Selected = false;
                UpdateCurrentItemDisplay();
            }
            else
                displayMatchPropertiesToolStripMenuItem.Enabled = true;
        }


        private void ShowGroupNumberToolStripMenuItem_CheckedChanged(object sender, EventArgs e) =>
            groupNumberDataGridViewColumn.Visible = showGroupNumberToolStripMenuItem.Checked;

        private void ShowGroupNameToolStripMenuItem_CheckedChanged(object sender, EventArgs e) =>
            groupNameDataGridViewColumn.Visible = showGroupNameToolStripMenuItem.Checked;

        private void ShowGroupIndexToolStripMenuItem_CheckedChanged(object sender, EventArgs e) =>
            groupIndexDataGridViewColumn.Visible = showGroupIndexToolStripMenuItem.Checked;

        private void ShowGroupLengthToolStripMenuItem_CheckedChanged(object sender, EventArgs e) =>
            groupLengthDataGridViewColumn.Visible = showGroupLengthToolStripMenuItem.Checked;

        private void ShowGroupValueToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn col in _groupValueColumns)
                col.Visible = false;
            if (!showGroupValueToolStripMenuItem.Checked)
                return;
            if (escapedValueNormalToolStripMenuItem.Checked)
                groupRawValueDataGridViewColumn.Visible = true;
            else if (escapedValueTnlToolStripMenuItem.Checked)
                groupEscapedValueDataGridViewColumn.Visible = true;
            else if (quotedLinesNormalToolStripMenuItem.Checked)
                groupQuotedLinesDataGridViewColumn.Visible = true;
            else if (quotedLinesTnlToolStripMenuItem.Checked)
                groupQuotedLinesLEDataGridViewColumn.Visible = true;
            else if (uriEncodedNormalToolStripMenuItem.Checked)
                groupUriEncodedValueDataGridViewColumn.Visible = true;
            else if (uriEncodedNlToolStripMenuItem.Checked)
                groupUriEncodedValueLEDataGridViewColumn.Visible = true;
            else if (xmlEncodedNormalToolStripMenuItem.Checked)
                groupXmlEncodedValueDataGridViewColumn.Visible = true;
            else if (xmlEncodedTnlToolStripMenuItem.Checked)
                groupXmlEncodedValueLEDataGridViewColumn.Visible = true;
            else if (hexidecimalValuesToolStripMenuItem.Checked)
                groupHexidecimalValuesDataGridViewColumn.Visible = true;
            else
                groupRawValueDataGridViewColumn.Visible = true;
        }

        private void PatternTextBox_TextChanged(object sender, EventArgs e) => StartRegexParseAsync();

        private void InputTextBox_TextChanged(object sender, EventArgs e)
        {
            _evaluationPending = true;
            if (evaluateOnChangeCheckBox.Checked)
                StartEvaluationAsync();
            else
                evaluateButton.Enabled = true;
        }

        private void EvaluateOnChangeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (evaluateOnChangeCheckBox.Checked)
            {
                evaluateButton.Enabled = false;
                if (_evaluationPending)
                    StartEvaluationAsync();
            }
            else
                evaluateButton.Enabled = true;
        }

        private void EvaluateButton_Click(object sender, EventArgs e)
        {
            evaluateButton.Enabled = false;
        }

        private void StartRegexParseAsync()
        {
            _patternParsePending = true;
            _evaluationPending = evaluateOnChangeCheckBox.Checked;
            if (parseRegexBackgroundWorker.IsBusy)
                parseRegexBackgroundWorker.CancelAsync();
            else
                parseRegexBackgroundWorker.RunWorkerAsync();
        }

        private Func<string, string[]> GetEvaluationLinesFunc()
        {
            if (separateLinesToolStripMenuItem.Checked)
            {
                if (inputModeBackslashedToolStripMenuItem.Checked)
                    return s => CaptureItem.FromEscapedTextLines(s);
                if (inputModeUriEncodedToolStripMenuItem.Checked)
                    return s => CaptureItem.FromUriEncodedLines(s);
                if (inputModeXmlEncodedToolStripMenuItem.Checked)
                    return s => CaptureItem.FromXmlEncodedLines(s);
                return s => CaptureItem.FromRawTextLines(s);
            }
            if (inputModeBackslashedToolStripMenuItem.Checked)
                return s => new string[] { CaptureItem.FromEscapedText(s) };
            if (inputModeUriEncodedToolStripMenuItem.Checked)
                return s => new string[] { CaptureItem.FromUriEncoded(s) };
            if (inputModeXmlEncodedToolStripMenuItem.Checked)
                return s => new string[] { CaptureItem.FromXmlEncoded(s) };
            return s => new string[] { s };
        }

        private void StartEvaluationAsync()
        {
            Regex regex = _parsedRegex;
            if (regex is null)
                return;
            _evaluationPending = true;
            if (evaluateExpressionBackgroundWorker.IsBusy)
                evaluateExpressionBackgroundWorker.CancelAsync();
            else
                evaluateExpressionBackgroundWorker.RunWorkerAsync(new object[] { regex, inputTextBox.Text, GetEvaluationLinesFunc() });
        }

        private void ParseRegexBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (parseRegexBackgroundWorker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
            _patternParsePending = false;
            
            // TODO: Implement ParseRegexBackgroundWorker_DoWork
        }

        private void ParseRegexBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_patternParsePending && !parseRegexBackgroundWorker.IsBusy)
                evaluateExpressionBackgroundWorker.RunWorkerAsync(new object[] { _parsedRegex, inputTextBox.Text, GetEvaluationLinesFunc() });
            else if (!e.Cancelled)
            {
                if (e.Error is null)
                {
                    if (e.Result is string message)
                        expressionErrorLabel.Text = message;
                    else
                    {
                        expressionErrorLabel.Visible = false;
                        _parsedRegex = e.Result as Regex;
                        if (_evaluationPending)
                            StartEvaluationAsync();
                        else
                            evaluateButton.Enabled = true;
                        return;
                    }
                }
                else
                    expressionErrorLabel.Text = string.IsNullOrWhiteSpace(e.Error.Message) ? e.Error.ToString() : e.Error.Message;
                _parsedRegex = null;
                expressionErrorLabel.Visible = true;
                evaluateButton.Enabled = false;
            }
        }

        private void EvaluateExpressionBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (evaluateExpressionBackgroundWorker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
            _evaluationPending = false;
            object[] args = (object[])e.Argument;
            string s = (string)args[1];
            Regex regex = (Regex)args[0];
            e.Result = (string.IsNullOrEmpty(s) ? new string[] { "" } : ((Func<string, string[]>)args[2]).Invoke(s)).Select((line, i) =>
                MatchResult.EvaluateMatch(regex, line, i + 1)).ToArray();
        }

        private void EvaluateExpressionBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_evaluationPending && !evaluateExpressionBackgroundWorker.IsBusy)
                evaluateExpressionBackgroundWorker.RunWorkerAsync();
            else if (!e.Cancelled)
            {
                displayMatchPropertiesToolStripMenuItem.Checked = true;
                if (e.Error is null)
                {
                    if (e.Result is string errorMessage)
                        evaluationErrorLabel.Text = errorMessage;
                    else
                    {
                        evaluationErrorLabel.Visible = false;
                        _matchResults.Clear();
                        _groupResults.Clear();
                        listingsSplitContainer.Panel1Collapsed = !separateLinesToolStripMenuItem.Checked;
                        MatchResult[] matchResults = e.Result as MatchResult[];
                        if (!(matchResults is null) && matchResults.Length > 0)
                        {
                            foreach (MatchResult m in matchResults)
                                _matchResults.Add(m);
                        }
                        UpdateCurrentItemDisplay();
                    }
                    return;
                }
                else
                    evaluationErrorLabel.Text = string.IsNullOrWhiteSpace(e.Error.Message) ? e.Error.ToString() : e.Error.Message;
                evaluationErrorLabel.Visible = true;
                successLabel.Text = "False";
                successLabel.ForeColor = Color.Red;
                indexTextBox.Visible = lengthTextBox.Visible = false;
                valueTextBox.Text = "";
            }
        }


        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _patternParsePending = _evaluationPending = false;
            if (parseRegexBackgroundWorker.IsBusy)
                parseRegexBackgroundWorker.CancelAsync();
            if (evaluateExpressionBackgroundWorker.IsBusy)
                evaluateExpressionBackgroundWorker.CancelAsync();
            _matchResults.Clear();
            _groupResults.Clear();
            patternTextBox.Text = inputTextBox.Text = valueTextBox.Text = indexTextBox.Text = lengthTextBox.Text = "";
            nameLabel.Visible = nameTextBox.Visible = evaluationErrorLabel.Visible = false;
            listingsSplitContainer.Panel1Collapsed = true;
            _currentSessionFileName = null;
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(_currentSessionFileName is null))
            {
                saveSessionFileDialog.InitialDirectory = Path.GetDirectoryName(_currentSessionFileName);
                saveSessionFileDialog.FileName = Path.GetFileName(_currentSessionFileName);
            }
            openSessionFileDialog.ShowDialog(this);
        }

        private void ImportPatternToolStripMenuItem_Click(object sender, EventArgs e)
        {
            importPatternFileDialog.ShowDialog(this);
        }

        private void ImportInputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            importInputFileDialog.ShowDialog(this);
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentSessionFileName is null)
                saveSessionFileDialog.ShowDialog(this);
            else
                SaveSessionConfig(_currentSessionFileName);
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(_currentSessionFileName is null))
            {
                saveSessionFileDialog.InitialDirectory = Path.GetDirectoryName(_currentSessionFileName);
                saveSessionFileDialog.FileName = Path.GetFileName(_currentSessionFileName);
            }
            saveSessionFileDialog.ShowDialog(this);
        }

        private void SavePatternToolStripMenuItem_Click(object sender, EventArgs e)
        {
            savePatternFileDialog.ShowDialog(this);
        }

        private void SaveInputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveInputFileDialog.ShowDialog(this);
        }

        private void SaveResultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveResultsFileDialog.ShowDialog(this);
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void OpenSessionFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            if (e.Cancel || string.IsNullOrEmpty(openSessionFileDialog.FileName))
                return;

            string sessionFileName;
            XDocument document;
            try
            {
                sessionFileName = Path.GetFullPath(openSessionFileDialog.FileName);
                document = XDocument.Load(sessionFileName);
            }
            catch (Exception exc)
            {
                MessageBox.Show(this, $"Unexpected error reading from {openSessionFileDialog.FileName}: {(string.IsNullOrWhiteSpace(exc.Message) ? exc.ToString() : exc.Message)}", "Open Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (document.Root is null || document.Root.Name.LocalName != ElementName_RegexTestSession || document.Root.Name.NamespaceName.Length > 0)
            {
                MessageBox.Show(this, $"Document root element name for {openSessionFileDialog.FileName} is not {ElementName_RegexTestSession}.\n\nNo session loaded.", "Invalid file",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            evaluateOnChangeCheckBox.Checked = document.Root.Attributes(AttributeName_EvaluateOnChange).Any(a => a.Value == "true");
            XElement xElement = document.Root.Element(ElementName_ResultMode);
            if (xElement is null || xElement.IsEmpty)
                MessageBox.Show(this, $"Result mode setting not found. Result mode not changed.", "Data not found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
                switch (xElement.Value.Trim())
                {
                    case XmlContent_BackslashEscaped:
                        if (xElement.Attributes(AttributeName_ShowLineSeparatorsAndTabs).Any(a => a.Value == "true"))
                            escapedValueTnlToolStripMenuItem.Checked = true;
                        else
                            escapedValueNormalToolStripMenuItem.Checked = true;
                        break;
                    case XmlContent_QuotedLines:
                        if (xElement.Attributes(AttributeName_ShowLineSeparatorsAndTabs).Any(a => a.Value == "true"))
                            quotedLinesTnlToolStripMenuItem.Checked = true;
                        else
                            quotedLinesNormalToolStripMenuItem.Checked = true;
                        break;
                    case XmlContent_UriEncoded:
                        if (xElement.Attributes(AttributeName_ShowLineSeparatorsAndTabs).Any(a => a.Value == "true"))
                            uriEncodedNlToolStripMenuItem.Checked = true;
                        else
                            uriEncodedNormalToolStripMenuItem.Checked = true;
                        break;
                    case XmlContent_XmlEncoded:
                        if (xElement.Attributes(AttributeName_ShowLineSeparatorsAndTabs).Any(a => a.Value == "true"))
                            xmlEncodedTnlToolStripMenuItem.Checked = true;
                        else
                            xmlEncodedNormalToolStripMenuItem.Checked = true;
                        break;
                    case XmlContent_HexidecimalValues:
                        hexidecimalValuesToolStripMenuItem.Checked = true;
                        break;
                    default:
                        rawValueToolStripMenuItem.Checked = true;
                        break;
                }
            showGroupNameToolStripMenuItem.Checked = document.Root.Attributes(AttributeName_ShowGroupName).Any(a => a.Value == "true");
            showGroupIndexToolStripMenuItem.Checked = document.Root.Attributes(AttributeName_ShowGroupIndex).Any(a => a.Value == "true");
            showGroupLengthToolStripMenuItem.Checked = document.Root.Attributes(AttributeName_ShowGroupLength).Any(a => a.Value == "true");
            showGroupValueToolStripMenuItem.Checked = document.Root.Attributes(AttributeName_ShowGroupValue).Any(a => a.Value == "true");
            xElement = document.Root.Element(ElementName_Pattern);
            if (xElement is null || xElement.IsEmpty)
                MessageBox.Show(this, $"Patterm settings not found. Pattern config not changed.", "Data not found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                patternAcceptsTabToolStripMenuItem.Checked = xElement.Attributes(AttributeName_AcceptsTab).Any(a => a.Value == "true");
                ignoreCaseToolStripMenuItem.Checked = xElement.Attributes(AttributeName_IgnoreCase).Any(a => a.Value == "true");
                cultureInvariantToolStripMenuItem.Checked = xElement.Attributes(AttributeName_CultureInvariant).Any(a => a.Value == "true");
                singleLineToolStripMenuItem.Checked = xElement.Attributes(AttributeName_SingleLine).Any(a => a.Value == "true");
                multilineToolStripMenuItem.Checked = xElement.Attributes(AttributeName_Multiline).Any(a => a.Value == "true");
                explicitCaptureToolStripMenuItem.Checked = xElement.Attributes(AttributeName_ExplicitCapture).Any(a => a.Value == "true");
                ignorePatternWhitespaceToolStripMenuItem.Checked = xElement.Attributes(AttributeName_IgnorePatternWhitespace).Any(a => a.Value == "true");
                rightToLeftToolStripMenuItem.Checked = xElement.Attributes(AttributeName_RightToLeft).Any(a => a.Value == "true");
                ecmaScriptToolStripMenuItem.Checked = xElement.Attributes(AttributeName_ECMAScript).Any(a => a.Value == "true");
                compiledToolStripMenuItem.Checked = xElement.Attributes(AttributeName_Compiled).Any(a => a.Value == "true");
                patternTextBox.Text = (xElement.IsEmpty) ? "" : xElement.Value;
            }
            xElement = document.Root.Element(ElementName_InputText);
            if (xElement is null || xElement.IsEmpty)
                MessageBox.Show(this, $"Input settings not found. Input config not changed.", "Data not found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                inputAcceptsTabToolStripMenuItem.Checked = xElement.Attributes(AttributeName_AcceptsTab).Any(a => a.Value == "true");
                separateLinesToolStripMenuItem.Checked = xElement.Attributes(AttributeName_EvaluateLinesSeparately).Any(a => a.Value == "true");
                switch (xElement.Attributes(AttributeName_Mode).Select(a => a.Value).DefaultIfEmpty("").First())
                {
                    case XmlContent_BackslashEscaped:
                        inputModeBackslashedToolStripMenuItem.Checked = true;
                        break;
                    case XmlContent_UriEncoded:
                        inputModeUriEncodedToolStripMenuItem.Checked = true;
                        break;
                    case XmlContent_XmlEncoded:
                        inputModeXmlEncodedToolStripMenuItem.Checked = true;
                        break;
                    default:
                        inputModeAsIsToolStripMenuItem.Checked = true;
                        break;
                }
                inputTextBox.Text = (xElement.IsEmpty) ? "" : xElement.Value;
            }
            _currentSessionFileName = sessionFileName;
        }

        private XElement CreateSessionElement(bool forSessionSave)
        {
            XElement sessionElement = new XElement(ElementName_RegexTestSession);

            #region Session Element settings

            if (forSessionSave && evaluateOnChangeCheckBox.Checked)
                sessionElement.SetAttributeValue(AttributeName_EvaluateOnChange, true);
            if (escapedValueNormalToolStripMenuItem.Checked)
                sessionElement.Add(new XElement(ElementName_ResultMode, XmlContent_BackslashEscaped));
            else if (escapedValueTnlToolStripMenuItem.Checked)
                sessionElement.Add(new XElement(ElementName_ResultMode, new XAttribute(AttributeName_ShowLineSeparatorsAndTabs, true), new XText(XmlContent_BackslashEscaped)));
            else if (quotedLinesNormalToolStripMenuItem.Checked)
                sessionElement.Add(new XElement(ElementName_ResultMode, XmlContent_QuotedLines));
            else if (quotedLinesTnlToolStripMenuItem.Checked)
                sessionElement.Add(new XElement(ElementName_ResultMode, new XAttribute(AttributeName_ShowLineSeparatorsAndTabs, true), new XText(XmlContent_QuotedLines)));
            else if (uriEncodedNormalToolStripMenuItem.Checked)
                sessionElement.Add(new XElement(ElementName_ResultMode, XmlContent_UriEncoded));
            else if (uriEncodedNlToolStripMenuItem.Checked)
                sessionElement.Add(new XElement(ElementName_ResultMode, new XAttribute(AttributeName_ShowLineSeparatorsAndTabs, true), new XText(XmlContent_UriEncoded)));
            else if (xmlEncodedNormalToolStripMenuItem.Checked)
                sessionElement.Add(new XElement(ElementName_ResultMode, XmlContent_XmlEncoded));
            else if (xmlEncodedTnlToolStripMenuItem.Checked)
                sessionElement.Add(new XElement(ElementName_ResultMode, new XAttribute(AttributeName_ShowLineSeparatorsAndTabs, true), new XText(XmlContent_XmlEncoded)));
            else if (hexidecimalValuesToolStripMenuItem.Checked)
                sessionElement.Add(new XElement(ElementName_ResultMode, XmlContent_HexidecimalValues));
            else
                sessionElement.Add(new XElement(ElementName_ResultMode, XmlContent_RawValue));

            if (forSessionSave)
            {
                if (showGroupNameToolStripMenuItem.Checked)
                    sessionElement.SetAttributeValue(AttributeName_ShowGroupName, true);
                if (showGroupIndexToolStripMenuItem.Checked)
                    sessionElement.SetAttributeValue(AttributeName_ShowGroupIndex, true);
                if (showGroupLengthToolStripMenuItem.Checked)
                    sessionElement.SetAttributeValue(AttributeName_ShowGroupLength, true);
                if (showGroupValueToolStripMenuItem.Checked)
                    sessionElement.SetAttributeValue(AttributeName_ShowGroupValue, true);
            }

            #endregion

            #region Pattern Element settings

            XElement xElement = new XElement(ElementName_Pattern,
                new XCData(patternTextBox.Text)
            );
            if (forSessionSave && patternAcceptsTabToolStripMenuItem.Checked)
                xElement.SetAttributeValue(AttributeName_AcceptsTab, true);
            if (ignoreCaseToolStripMenuItem.Checked)
                xElement.SetAttributeValue(AttributeName_IgnoreCase, true);
            if (cultureInvariantToolStripMenuItem.Checked)
                xElement.SetAttributeValue(AttributeName_CultureInvariant, true);
            if (singleLineToolStripMenuItem.Checked)
                xElement.SetAttributeValue(AttributeName_SingleLine, true);
            if (multilineToolStripMenuItem.Checked)
                xElement.SetAttributeValue(AttributeName_Multiline, true);
            if (explicitCaptureToolStripMenuItem.Checked)
                xElement.SetAttributeValue(AttributeName_ExplicitCapture, true);
            if (ignorePatternWhitespaceToolStripMenuItem.Checked)
                xElement.SetAttributeValue(AttributeName_IgnorePatternWhitespace, true);
            if (rightToLeftToolStripMenuItem.Checked)
                xElement.SetAttributeValue(AttributeName_RightToLeft, true);
            if (ecmaScriptToolStripMenuItem.Checked)
                xElement.SetAttributeValue(AttributeName_ECMAScript, true);
            if (compiledToolStripMenuItem.Checked)
                xElement.SetAttributeValue(AttributeName_Compiled, true);

            sessionElement.Add(xElement);

            #endregion

            #region Input Element settings

            if (forSessionSave)
            {
                xElement = new XElement(ElementName_InputText, new XCData(inputTextBox.Text));
                if (inputAcceptsTabToolStripMenuItem.Checked)
                    xElement.SetAttributeValue(AttributeName_AcceptsTab, true);
                if (separateLinesToolStripMenuItem.Checked)
                    xElement.SetAttributeValue(AttributeName_EvaluateLinesSeparately, true);
            }
            else if (separateLinesToolStripMenuItem.Checked)
            {
                xElement = new XElement(ElementName_InputText);
                int itemNumber = 0;
                foreach (string line in CaptureItem.FromRawTextLines(inputTextBox.Text))
                    xElement.Add(new XElement("Input"), new XAttribute("Number", ++itemNumber), new XCData(line));
            }
            else
                xElement = new XElement(ElementName_InputText, new XCData(inputTextBox.Text));
            if (inputModeBackslashedToolStripMenuItem.Checked)
                xElement.SetAttributeValue(AttributeName_Mode, XmlContent_BackslashEscaped);
            else if (inputModeUriEncodedToolStripMenuItem.Checked)
                xElement.SetAttributeValue(AttributeName_Mode, XmlContent_UriEncoded);
            else if (inputModeXmlEncodedToolStripMenuItem.Checked)
                xElement.SetAttributeValue(AttributeName_Mode, XmlContent_XmlEncoded);
            else
                xElement.SetAttributeValue(AttributeName_Mode, XmlContent_AsIs);

            sessionElement.Add(xElement);

            #endregion

            return sessionElement;
        }

        private void SaveSessionConfig(string sessionFileName)
        {
            XElement sessionElement = CreateSessionElement(true);
            try
            {
                sessionFileName = Path.GetFullPath(sessionFileName);
                using (XmlWriter writer = XmlWriter.Create(sessionFileName, new XmlWriterSettings
                {
                    ConformanceLevel = ConformanceLevel.Document,
                    Encoding = new UTF8Encoding(false, true),
                    Indent = true
                }))
                {
                    new XDocument(sessionElement).WriteTo(writer);
                    writer.Flush();
                }
                _currentSessionFileName = sessionFileName;
            }
            catch (Exception exc)
            {
                MessageBox.Show(this, $"Unexpected error saving to {sessionFileName}: {(string.IsNullOrWhiteSpace(exc.Message) ? exc.ToString() : exc.Message)}", "Save Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveSessionFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            if (!(e.Cancel || string.IsNullOrEmpty(saveResultsFileDialog.FileName)))
                SaveSessionConfig(saveResultsFileDialog.FileName);
        }

        private void ImportPatternFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            if (e.Cancel || string.IsNullOrEmpty(importPatternFileDialog.FileName))
                return;

            try
            {
                patternTextBox.Text = File.ReadAllText(importPatternFileDialog.FileName);
            }
            catch (Exception exc)
            {
                MessageBox.Show(this, $"Unexpected error reading from {importPatternFileDialog.FileName}: {(string.IsNullOrWhiteSpace(exc.Message) ? exc.ToString() : exc.Message)}", "Read Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SavePatternFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            if (e.Cancel || string.IsNullOrEmpty(savePatternFileDialog.FileName))
                return;

            try
            {
                File.WriteAllText(savePatternFileDialog.FileName, patternTextBox.Text);
            }
            catch (Exception exc)
            {
                MessageBox.Show(this, $"Unexpected error writing to {savePatternFileDialog.FileName}: {(string.IsNullOrWhiteSpace(exc.Message) ? exc.ToString() : exc.Message)}", "Write Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ImportInputFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            if (e.Cancel || string.IsNullOrEmpty(importInputFileDialog.FileName))
                return;

            try
            {
                inputTextBox.Text = File.ReadAllText(importInputFileDialog.FileName);
            }
            catch (Exception exc)
            {
                MessageBox.Show(this, $"Unexpected error reading from {importInputFileDialog.FileName}: {(string.IsNullOrWhiteSpace(exc.Message) ? exc.ToString() : exc.Message)}", "Read Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveInputFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            if (e.Cancel || string.IsNullOrEmpty(saveInputFileDialog.FileName))
                return;

            try
            {
                File.WriteAllText(saveInputFileDialog.FileName, patternTextBox.Text);
            }
            catch (Exception exc)
            {
                MessageBox.Show(this, $"Unexpected error writing to {saveInputFileDialog.FileName}: {(string.IsNullOrWhiteSpace(exc.Message) ? exc.ToString() : exc.Message)}", "Write Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveResultsFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            if (e.Cancel || string.IsNullOrEmpty(saveResultsFileDialog.FileName))
                return;

            XElement sessionElement = CreateSessionElement(false);
            if (separateLinesToolStripMenuItem.Checked)
                foreach (MatchResult result in _matchResults)
                    sessionElement.Add(result.ToXElement());
            else
            {
                MatchResult result = _matchResults.FirstOrDefault();
                if (result is null || !result.Success)
                    sessionElement.AddFirst(new XElement(nameof(Match), new XAttribute(nameof(CaptureItem.Success), false)));
                else
                {
                    XElement element = new XElement(nameof(Match), new XCData(result.RawValue));
                    element.SetAttributeValue(nameof(CaptureItem.Success), true);
                    element.SetAttributeValue(nameof(CaptureItem.Index), result.Index);
                    element.SetAttributeValue(nameof(CaptureItem.Length), result.Length);
                    sessionElement.AddFirst(element);
                    foreach (GroupResult item in _groupResults)
                    {
                        XElement g = item.ToXElement();
                        element.AddAfterSelf(g);
                        element = g;
                    }
                }
            }
            try
            {
                using (XmlWriter writer = XmlWriter.Create(saveResultsFileDialog.FileName, new XmlWriterSettings
                {
                    ConformanceLevel = ConformanceLevel.Document,
                    Encoding = new UTF8Encoding(false, true),
                    Indent = true
                }))
                {
                    new XDocument(sessionElement).WriteTo(writer);
                    writer.Flush();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(this, $"Unexpected error saving to {saveResultsFileDialog.FileName}: {(string.IsNullOrWhiteSpace(exc.Message) ? exc.ToString() : exc.Message)}", "Save Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
