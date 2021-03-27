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
    public class RegexForm : Form
    {
        protected const string ElementName_RegexTestSession = "RegexTestSession";
        protected const string AttributeName_EvaluateOnChange = "EvaluateOnChange";
        protected const string ElementName_ResultMode = "ResultMode";
        protected const string XmlContent_BackslashEscaped = "BackslashEscaped";
        protected const string AttributeName_ShowLineSeparatorsAndTabs = "ShowLineSeparatorsAndTabs";
        protected const string XmlContent_QuotedLines = "QuotedLines";
        protected const string XmlContent_UriEncoded = "UriEncoded";
        protected const string XmlContent_XmlEncoded = "XmlEncoded";
        protected const string XmlContent_HexidecimalValues = "HexidecimalValues";
        protected const string XmlContent_RawValue = "RawValue";
        protected const string AttributeName_ShowMatchIndex = "ShowMatchIndex";
        protected const string AttributeName_ShowMatchLength = "ShowMatchLength";
        protected const string AttributeName_ShowMatchValue = "ShowMatchValue";
        protected const string AttributeName_ShowGroupName = "ShowGroupName";
        protected const string AttributeName_ShowGroupIndex = "ShowGroupIndex";
        protected const string AttributeName_ShowGroupLength = "ShowGroupLength";
        protected const string AttributeName_ShowGroupValue = "ShowGroupValue";
        protected const string ElementName_Pattern = "Pattern";
        protected const string AttributeName_AcceptsTab = "AcceptsTab";
        protected const string AttributeName_IgnoreCase = "IgnoreCase";
        protected const string AttributeName_CultureInvariant = "CultureInvariant";
        protected const string AttributeName_SingleLine = "SingleLine";
        protected const string AttributeName_Multiline = "Multiline";
        protected const string AttributeName_ExplicitCapture = "ExplicitCapture";
        protected const string AttributeName_IgnorePatternWhitespace = "IgnorePatternWhitespace";
        protected const string AttributeName_RightToLeft = "RightToLeft";
        protected const string AttributeName_ECMAScript = "ECMAScript";
        protected const string AttributeName_Compiled = "Compiled";
        protected const string AttributeName_EvaluateLinesSeparately = "EvaluateLinesSeparately";
        protected const string ElementName_InputText = "InputText";
        protected const string AttributeName_Mode = "Mode";
        protected const string XmlContent_AsIs = "AsIs";
        private RegexOptions __options = RegexOptions.None;
        protected DataGridViewTextBoxColumn groupNumberDataGridViewColumn;
        protected DataGridViewTextBoxColumn groupNameDataGridViewColumn;
        protected DataGridViewTextBoxColumn groupSuccessDataGridViewColumn;
        protected DataGridViewTextBoxColumn groupIndexDataGridViewColumn;
        protected DataGridViewTextBoxColumn groupLengthDataGridViewColumn;
        protected DataGridViewTextBoxColumn groupRawValueDataGridViewColumn;
        protected DataGridViewTextBoxColumn groupEscapedValueDataGridViewColumn;
        protected DataGridViewTextBoxColumn groupEscapedValueLEDataGridViewColumn;
        protected DataGridViewTextBoxColumn groupQuotedLinesDataGridViewColumn;
        protected DataGridViewTextBoxColumn groupQuotedLinesLEDataGridViewColumn;
        protected DataGridViewTextBoxColumn groupUriEncodedValueDataGridViewColumn;
        protected DataGridViewTextBoxColumn groupUriEncodedValueLEDataGridViewColumn;
        protected DataGridViewTextBoxColumn groupXmlEncodedValueDataGridViewColumn;
        protected DataGridViewTextBoxColumn groupXmlEncodedValueLEDataGridViewColumn;
        protected DataGridViewTextBoxColumn groupHexidecimalValuesDataGridViewColumn;
        protected DataGridViewColumn[] _groupValueColumns;
        protected readonly BindingList<GroupResult> _groupResults = new BindingList<GroupResult>();
        protected Regex _parsedRegex;
        protected bool _patternParsePending = false;
        protected bool _evaluationPending = false;
        protected CaptureItem _selectedItem;
        protected string _currentSessionFileName = null;

        protected RegexOptions _options
        {
            get => __options;
            set
            {
                __options = value;
            }
        }

        protected RegexForm()
        {
        }

        protected void InitializeCustomComponents()
        {
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
        }

        internal static string ExceptionTostring(Exception exception) =>
            (exception is null) ? "" : (string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message);
    }
}
