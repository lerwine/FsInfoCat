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
        private bool _handlePatternOptionChange = false;
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
        private MatchResult _selectedMatch;
        private GroupResult _selectedGroup;
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
            _handlePatternOptionChange = true;
            _ignoreViewModeCheckChange = false;
        }

        private void EvaluationInputs_ExpressionEvaluated(object sender, EvaluationFinishedEventArgs e)
        {
            if (e.Cancelled)
                return;
            if (e.Result is BinaryAlternate<IList<MatchResult>, Exception> multipleResult)
            {
                multipleResult.Apply(matchResults =>
                {
                    lineResultDataGridView.ClearSelection();
                    _selectedMatch = null;
                    indexTextBox.Visible = lengthTextBox.Visible = true;
                    listingsSplitContainer.Panel1Collapsed = false;
                    OnMatchItemsChanged();
                }, error =>
                {
                    listingsSplitContainer.Panel1Collapsed = true;
                    successLabel.Text = "False";
                    successLabel.ForeColor = Color.Red;
                    indexTextBox.Visible = lengthTextBox.Visible = false;
                    valueTextBox.Text = "";
                });
            }
            else if (e.Result is BinaryAlternate<MatchResult, Exception> singleResult)
            {
                listingsSplitContainer.Panel1Collapsed = true;
                singleResult.Apply(matchCollection =>
                {
                    indexTextBox.Visible = lengthTextBox.Visible = true;
                    _matchResults.Add(matchCollection);
                    OnMatchItemsChanged();
                }, error =>
                {
                    listingsSplitContainer.Panel1Collapsed = true;
                    successLabel.Text = "False";
                    successLabel.ForeColor = Color.Red;
                    indexTextBox.Visible = lengthTextBox.Visible = false;
                    valueTextBox.Text = "";
                });
            }
        }

        private void EvaluationInputs_EvaluateExpressionAsync(object sender, RegexEvaluatableEventArgs e)
        {
            if (e.IsSingleInput)
                e.Result = e.GetMatch().Map(mc => new MatchResult(e.PrimaryInput, mc));
            else
                e.Result = e.GetAllMatches((itemNumber, input, mc) => new MatchResult(input, mc, itemNumber));
        }

        internal static string ExceptionTostring(Exception exception) =>
            (exception is null) ? "" : (string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message);

        private void SetRawResultMode()
        {
            valueTextBox.Text = (_selectedGroup is null) ? ((_selectedMatch is null) ? "" : _selectedMatch.RawValue) : _selectedGroup.RawValue;
            if (showGroupValueToolStripMenuItem.Checked)
            {
                foreach (DataGridViewColumn col in _groupValueColumns)
                    col.Visible = false;
                groupRawValueDataGridViewColumn.Visible = true;
            }
        }

        private void SetEscapedResultMode(bool escapeTabsAndNewLines)
        {
            valueTextBox.Text = (_selectedGroup is null) ? (_selectedMatch is null) ? "" :
                (escapeTabsAndNewLines ? _selectedMatch.EscapedValue : _selectedMatch.EscapedValueLE) :
                (escapeTabsAndNewLines ? _selectedGroup.EscapedValue : _selectedGroup.EscapedValueLE);
            if (showGroupValueToolStripMenuItem.Checked)
            {
                foreach (DataGridViewColumn col in _groupValueColumns)
                    col.Visible = false;
                (escapeTabsAndNewLines ? groupEscapedValueDataGridViewColumn : groupEscapedValueLEDataGridViewColumn).Visible = true;
            }
        }

        private void SetQuotedResultMode(bool escapeTabsAndNewLines)
        {
            valueTextBox.Text = (_selectedGroup is null) ? (_selectedMatch is null) ? "" :
                (escapeTabsAndNewLines ? _selectedMatch.QuotedLines : _selectedMatch.QuotedLinesLE) :
                (escapeTabsAndNewLines ? _selectedGroup.QuotedLines : _selectedGroup.QuotedLinesLE);
            if (showGroupValueToolStripMenuItem.Checked)
            {
                foreach (DataGridViewColumn col in _groupValueColumns)
                    col.Visible = false;
                (escapeTabsAndNewLines ? groupQuotedLinesDataGridViewColumn : groupQuotedLinesLEDataGridViewColumn).Visible = true;
            }
        }

        private void SetUriEncodedResulMode(bool escapeTabsAndNewLines)
        {
            valueTextBox.Text = (_selectedGroup is null) ? (_selectedMatch is null) ? "" :
                (escapeTabsAndNewLines ? _selectedMatch.UriEncodedValue : _selectedMatch.UriEncodedValueLE) :
                (escapeTabsAndNewLines ? _selectedGroup.UriEncodedValue : _selectedGroup.UriEncodedValueLE);
            if (showGroupValueToolStripMenuItem.Checked)
            {
                foreach (DataGridViewColumn col in _groupValueColumns)
                    col.Visible = false;
                (escapeTabsAndNewLines ? groupUriEncodedValueDataGridViewColumn : groupUriEncodedValueLEDataGridViewColumn).Visible = true;
            }
        }

        private void SetXmlEncodedResulMode(bool escapeTabsAndNewLines)
        {
            valueTextBox.Text = (_selectedGroup is null) ? (_selectedMatch is null) ? "" :
                (escapeTabsAndNewLines ? _selectedMatch.XmlEncodedValue : _selectedMatch.XmlEncodedValueLE) :
                (escapeTabsAndNewLines ? _selectedGroup.XmlEncodedValue : _selectedGroup.XmlEncodedValueLE);
            if (showGroupValueToolStripMenuItem.Checked)
            {
                foreach (DataGridViewColumn col in _groupValueColumns)
                    col.Visible = false;
                (escapeTabsAndNewLines ? groupXmlEncodedValueDataGridViewColumn : groupXmlEncodedValueLEDataGridViewColumn).Visible = true;
            }
        }

        private void SetHexidecimalDisplayMode()
        {
            valueTextBox.Text = (_selectedGroup is null) ? ((_selectedMatch is null) ? "" : _selectedMatch.HexEncoded) : _selectedGroup.HexEncoded;
            if (showGroupValueToolStripMenuItem.Checked)
            {
                foreach (DataGridViewColumn col in _groupValueColumns)
                    col.Visible = false;
                groupHexidecimalValuesDataGridViewColumn.Visible = true;
            }
        }

        private void OnMatchItemsChanged()
        {
            if (lineResultDataGridView.TryGetSelectedOrFirstDataBoundItemOfType(out MatchResult item, out DataGridViewRow row))
            {
                _selectedMatch = null;
                if (!row.Selected)
                    row.Selected = true;
                if (_selectedMatch is null)
                {
                    _selectedMatch = item;
                    OnSelectedMatchChanged();
                }
            }
            else if (!(_selectedMatch is null))
            {
                _selectedMatch = null;
                OnSelectedMatchChanged();
            }
        }

        private void OnSelectedMatchChanged()
        {
            if (_selectedMatch is null)
            {
                groupsDataGridView.ClearSelection();
                _groupResults.Clear();
                _selectedGroup = null;
            }
            else
            {
                groupsDataGridView.ClearSelection();
                _groupResults.Clear();
                _selectedGroup = null;
                foreach (GroupResult group in _selectedMatch.Groups)
                    _groupResults.Add(group);
                if (!(_selectedGroup is null))
                {
                    groupsDataGridView.ClearSelection();
                    _selectedGroup = null;
                }
            }
            OnSelectedGroupChanged();
        }

        private void OnSelectedGroupChanged()
        {
            CaptureItem selectedItem = _selectedGroup;
            if (selectedItem is null)
            {
                displayMatchPropertiesToolStripMenuItem.Checked = false;
                displayMatchPropertiesToolStripMenuItem.Enabled = true;
                nameLabel.Visible = nameTextBox.Visible = false;
                if ((selectedItem = _selectedMatch) is null)
                {
                    successLabel.Text = indexTextBox.Text = lengthTextBox.Text = valueTextBox.Text = "";
                    return;
                }
            }
            else
            {
                displayMatchPropertiesToolStripMenuItem.Checked = false;
                displayMatchPropertiesToolStripMenuItem.Enabled = true;
                nameTextBox.Text = _selectedGroup.Name;
                nameLabel.Visible = nameTextBox.Visible = true;
            }
            if (selectedItem.Success)
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
                indexTextBox.Text = selectedItem.Index.ToString();
                lengthTextBox.Text = selectedItem.Length.ToString();
            }
            else
            {
                successLabel.Text = "False";
                successLabel.ForeColor = Color.Red;
                indexTextBox.Text = lengthTextBox.Text = valueTextBox.Text = "";
            }
        }

        private void GroupsDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (groupsDataGridView.SelectedRows.TryGetFirstDataBoundItemOfType(out GroupResult item))
            {
                if (ReferenceEquals(item, _selectedGroup))
                    return;
                _selectedGroup = item;
            }
            else
            {
                if (_selectedGroup is null)
                    return;
                _selectedGroup = null;
            }
            OnSelectedGroupChanged();
        }

        private void LineResultDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (lineResultDataGridView.SelectedRows.TryGetFirstDataBoundItemOfType(out MatchResult item))
            {
                if (ReferenceEquals(item, _selectedMatch))
                    return;
                _selectedMatch = item;
            }
            else
            {
                if (_selectedMatch is null)
                    return;
                _selectedMatch = null;
            }
            OnSelectedMatchChanged();
        }

        private void InputModeAsIsToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (inputModeAsIsToolStripMenuItem.Checked)
            {
                inputModeBackslashedToolStripMenuItem.Checked = inputModeUriEncodedToolStripMenuItem.Checked = inputModeXmlEncodedToolStripMenuItem.Checked = false;
                inputModeBackslashedToolStripMenuItem.Enabled = inputModeUriEncodedToolStripMenuItem.Enabled = inputModeXmlEncodedToolStripMenuItem.Enabled = false;
                inputModeAsIsToolStripMenuItem.Enabled = false;
                evaluationInputs.Mode = EvaluationInputsMode.PlainText;
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
                evaluationInputs.Mode = EvaluationInputsMode.BackslashEscaped;
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
                evaluationInputs.Mode = EvaluationInputsMode.UriEncoded;
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
                evaluationInputs.Mode = EvaluationInputsMode.XmlEncoded;
            }
            else
                inputModeXmlEncodedToolStripMenuItem.Enabled = true;
        }

        private void SeparateLinesToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            evaluationInputs.ParseLinesSeparately = separateLinesToolStripMenuItem.Checked;
        }

        private void InputModeWordWrapToolStripMenuItem_CheckedChanged(object sender, EventArgs e) => evaluationInputs.WordWrap = inputModeWordWrapToolStripMenuItem.Checked;

        private void EvaluationInputs_PatternOptionsChanged(object sender, PatternOptionsEventArgs e)
        {
            _handlePatternOptionChange = false;
            try
            {
                RegexOptions options = e.PatternOptions;
                noneToolStripMenuItem.Enabled = !(noneToolStripMenuItem.Checked = options == RegexOptions.None);
                ignoreCaseToolStripMenuItem.Enabled = !(ignoreCaseToolStripMenuItem.Checked = options.HasFlag(RegexOptions.IgnoreCase));
                multilineToolStripMenuItem.Enabled = !(multilineToolStripMenuItem.Checked = options.HasFlag(RegexOptions.Multiline));
                explicitCaptureToolStripMenuItem.Enabled = !(explicitCaptureToolStripMenuItem.Checked = options.HasFlag(RegexOptions.ExplicitCapture));
                compiledToolStripMenuItem.Enabled = !(compiledToolStripMenuItem.Checked = options.HasFlag(RegexOptions.Compiled));
                singleLineToolStripMenuItem.Enabled = !(singleLineToolStripMenuItem.Checked = options.HasFlag(RegexOptions.Singleline));
                ignorePatternWhitespaceToolStripMenuItem.Enabled = !(ignorePatternWhitespaceToolStripMenuItem.Checked = options.HasFlag(RegexOptions.IgnorePatternWhitespace));
                rightToLeftToolStripMenuItem.Enabled = !(rightToLeftToolStripMenuItem.Checked = options.HasFlag(RegexOptions.RightToLeft));
                ecmaScriptToolStripMenuItem.Enabled = !(ecmaScriptToolStripMenuItem.Checked = options.HasFlag(RegexOptions.ECMAScript));
                cultureInvariantToolStripMenuItem.Enabled = !(cultureInvariantToolStripMenuItem.Checked = options.HasFlag(RegexOptions.CultureInvariant));
            }
            finally { _handlePatternOptionChange = true; }
        }

        private void EvaluationInputs_ParseLinesSeparatelyChanged(object sender, BooleanValueEventArgs e)
        {
            separateLinesToolStripMenuItem.Checked = e.Value;
        }

        private void NoneToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (_handlePatternOptionChange && noneToolStripMenuItem.Checked)
                evaluationInputs.PatternOptions = RegexOptions.None;
        }

        private void IgnoreCaseToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (_handlePatternOptionChange)
            {
                if (ignoreCaseToolStripMenuItem.Checked)
                    evaluationInputs.PatternOptions |= RegexOptions.IgnoreCase;
                else
                    evaluationInputs.PatternOptions ^= RegexOptions.IgnoreCase;
            }
        }

        private void CultureInvariantToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (_handlePatternOptionChange)
            {
                if (cultureInvariantToolStripMenuItem.Checked)
                    evaluationInputs.PatternOptions |= RegexOptions.CultureInvariant;
                else
                    evaluationInputs.PatternOptions ^= RegexOptions.CultureInvariant;
            }
        }

        private void MultilineToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (_handlePatternOptionChange)
            {
                if (multilineToolStripMenuItem.Checked)
                    evaluationInputs.PatternOptions |= RegexOptions.Multiline;
                else
                    evaluationInputs.PatternOptions ^= RegexOptions.Multiline;
            }
        }

        private void SinglelineToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (_handlePatternOptionChange)
            {
                if (singleLineToolStripMenuItem.Checked)
                    evaluationInputs.PatternOptions |= RegexOptions.Singleline;
                else
                    evaluationInputs.PatternOptions ^= RegexOptions.Singleline;
            }
        }

        private void ExplicitCaptureToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (_handlePatternOptionChange)
            {
                if (explicitCaptureToolStripMenuItem.Checked)
                    evaluationInputs.PatternOptions |= RegexOptions.ExplicitCapture;
                else
                    evaluationInputs.PatternOptions ^= RegexOptions.ExplicitCapture;
            }
        }

        private void IgnorePatternWhitespaceToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (_handlePatternOptionChange)
            {
                if (ignorePatternWhitespaceToolStripMenuItem.Checked)
                    evaluationInputs.PatternOptions |= RegexOptions.IgnorePatternWhitespace;
                else
                    evaluationInputs.PatternOptions ^= RegexOptions.IgnorePatternWhitespace;
            }
        }

        private void RightToLeftToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (_handlePatternOptionChange)
            {
                if (rightToLeftToolStripMenuItem.Checked)
                    evaluationInputs.PatternOptions |= RegexOptions.RightToLeft;
                else
                    evaluationInputs.PatternOptions ^= RegexOptions.RightToLeft;
            }
        }

        private void ECMAScriptToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (_handlePatternOptionChange)
            {
                if (ecmaScriptToolStripMenuItem.Checked)
                    evaluationInputs.PatternOptions |= RegexOptions.ECMAScript;
                else
                    evaluationInputs.PatternOptions ^= RegexOptions.ECMAScript;
            }
        }

        private void CompiledToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (_handlePatternOptionChange)
            {
                if (compiledToolStripMenuItem.Checked)
                    evaluationInputs.PatternOptions |= RegexOptions.Compiled;
                else
                    evaluationInputs.PatternOptions ^= RegexOptions.Compiled;
            }
        }

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
                groupsDataGridView.ClearSelection();
                _selectedGroup = null;
                OnSelectedGroupChanged();
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

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            evaluationInputs.CancelWorkers();
            _matchResults.Clear();
            _groupResults.Clear();
            valueTextBox.Text = indexTextBox.Text = lengthTextBox.Text = "";
            nameLabel.Visible = nameTextBox.Visible = false;
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
            evaluationInputs.EvaluateOnChange = document.Root.Attributes(AttributeName_EvaluateOnChange).Any(a => a.Value == "true");
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
                evaluationInputs.PatternText = (xElement.IsEmpty) ? "" : xElement.Value;
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
                evaluationInputs.InputText = (xElement.IsEmpty) ? "" : xElement.Value;
            }
            _currentSessionFileName = sessionFileName;
        }

        private XElement CreateSessionElement(bool forSessionSave)
        {
            XElement sessionElement = new XElement(ElementName_RegexTestSession);

            #region Session Element settings

            if (forSessionSave && evaluationInputs.EvaluateOnChange)
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
                new XCData(evaluationInputs.PatternText)
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
                xElement = new XElement(ElementName_InputText, new XCData(evaluationInputs.InputText));
                if (inputAcceptsTabToolStripMenuItem.Checked)
                    xElement.SetAttributeValue(AttributeName_AcceptsTab, true);
                if (separateLinesToolStripMenuItem.Checked)
                    xElement.SetAttributeValue(AttributeName_EvaluateLinesSeparately, true);
            }
            else if (separateLinesToolStripMenuItem.Checked)
            {
                xElement = new XElement(ElementName_InputText);
                int itemNumber = 0;
                foreach (string line in CaptureItem.FromRawTextLines(evaluationInputs.InputText))
                    xElement.Add(new XElement("Input"), new XAttribute("Number", ++itemNumber), new XCData(line));
            }
            else
                xElement = new XElement(ElementName_InputText, new XCData(evaluationInputs.InputText));
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
                evaluationInputs.PatternText = File.ReadAllText(importPatternFileDialog.FileName);
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
                File.WriteAllText(savePatternFileDialog.FileName, evaluationInputs.PatternText);
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
                evaluationInputs.InputText = File.ReadAllText(importInputFileDialog.FileName);
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
                File.WriteAllText(saveInputFileDialog.FileName, evaluationInputs.InputText);
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
