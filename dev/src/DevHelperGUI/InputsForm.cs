using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DevHelperGUI
{
    public partial class inputsForm : Form
    {
        private static readonly Regex _lineBreakRegex = new Regex(@"\r\n?|\n", RegexOptions.Compiled);
        private SingleMatchResultForm _singleMatchForm;
        private MultipleMatchForm _multipleMatchForm;
        private MatchCollectionResultForm _matchCollectionResultForm;
        private MultipleMatchCollectionForm _multipleMatchCollectionForm;
        private SplitTextForm _splitTextForm;
        private MultipleSplitTextForm _multipleSplitTextForm;
        private Regex _expression;
        private bool _ignoreOptionChange = false;
        private RegexOptions _options = RegexOptions.None;

        public inputsForm()
        {
            InitializeComponent();
        }

        private void Evaluate()
        {
            string inputText = inputTextTextBox.Text;
            if (lineBreaksToCrLfRadioButton.Checked)
                inputText = _lineBreakRegex.Replace(inputText, "\r\n");
            else if (lineBreaksToLfRadioButton.Checked)
                inputText = _lineBreakRegex.Replace(inputText, "\n");
            else if (ignoreLineBreaksRadioButton.Checked)
                inputText = _lineBreakRegex.Replace(inputText, "");
            else
            {
                IEnumerable<string> lines = _lineBreakRegex.Split(inputText);
                if (decodeUriEncodedSequencesCheckBox.Checked)
                    lines = lines.Select(l => Uri.UnescapeDataString(l));
                if (splitRadioButton.Checked)
                {
                    EnsureClosed(_singleMatchForm, _multipleMatchForm, _matchCollectionResultForm, _multipleMatchCollectionForm, _splitTextForm);
                    _singleMatchForm = null;
                    _multipleMatchForm = null;
                    _matchCollectionResultForm = null;
                    _multipleMatchCollectionForm = null;
                    _splitTextForm = null;
                    if (_multipleSplitTextForm is null)
                        _multipleSplitTextForm = new MultipleSplitTextForm();
                    _multipleSplitTextForm.SetResults(lines.Select(l => new LineSplitResult(_expression.Split(l))));
                }
                else if (multipleMatchRadioButton.Checked)
                {
                    EnsureClosed(_singleMatchForm, _multipleMatchForm, _matchCollectionResultForm, _splitTextForm, _multipleSplitTextForm);
                    _singleMatchForm = null;
                    _multipleMatchForm = null;
                    _matchCollectionResultForm = null;
                    _splitTextForm = null;
                    _multipleSplitTextForm = null;
                    if (_multipleMatchCollectionForm is null)
                        _multipleMatchCollectionForm = new MultipleMatchCollectionForm();
                    _multipleMatchCollectionForm.SetResults(lines.Select(l => new MatchCollectionResult(_expression.Matches(l))));
                }
                else
                {
                    EnsureClosed(_singleMatchForm, _matchCollectionResultForm, _multipleMatchCollectionForm, _splitTextForm, _multipleSplitTextForm);
                    _singleMatchForm = null;
                    _matchCollectionResultForm = null;
                    _multipleMatchCollectionForm = null;
                    _splitTextForm = null;
                    _multipleSplitTextForm = null;
                    if (_multipleMatchForm is null)
                        _multipleMatchForm = new MultipleMatchForm();
                    _multipleMatchForm.SetResults(lines.Select(l => new MatchResult(_expression.Match(l))));
                }
                return;
            }
            if (decodeUriEncodedSequencesCheckBox.Checked)
                inputText = Uri.UnescapeDataString(inputText);
            if (singlelineOptionCheckBox.Checked)
            {
                EnsureClosed(_singleMatchForm, _multipleMatchForm, _matchCollectionResultForm, _multipleMatchCollectionForm, _multipleSplitTextForm);
                _singleMatchForm = null;
                _multipleMatchForm = null;
                _matchCollectionResultForm = null;
                _multipleMatchCollectionForm = null;
                _multipleSplitTextForm = null;
                if (_splitTextForm is null)
                    _splitTextForm = new SplitTextForm();
                _splitTextForm.SetResults(_expression.Split(inputText).Select((s, i) => new LineResult(s, i + 1)));
            }
            else if (multipleMatchRadioButton.Checked)
            {
                EnsureClosed(_singleMatchForm, _multipleMatchForm, _multipleMatchCollectionForm, _splitTextForm, _multipleSplitTextForm);
                _singleMatchForm = null;
                _multipleMatchForm = null;
                _multipleMatchCollectionForm = null;
                _splitTextForm = null;
                _multipleSplitTextForm = null;
                if (_matchCollectionResultForm is null)
                    _matchCollectionResultForm = new MatchCollectionResultForm();
                _matchCollectionResultForm.SetResults(_expression.Matches(inputText).Cast<Match>().Select((m, i) => new MatchResult(m, i + 1)));
            }
            else
            {
                EnsureClosed(_multipleMatchForm, _matchCollectionResultForm, _multipleMatchCollectionForm, _splitTextForm, _multipleSplitTextForm);
                _multipleMatchForm = null;
                _matchCollectionResultForm = null;
                _multipleMatchCollectionForm = null;
                _splitTextForm = null;
                _multipleSplitTextForm = null;
                if (_singleMatchForm is null)
                    _singleMatchForm = new SingleMatchResultForm();
                Match match = _expression.Match(inputText);
                if (match.Success)
                    _singleMatchForm.SetResults(true, match.Value, match.Index, match.Length, match.Groups.Cast<Group>().Select((g, i) => new GroupResult(g)));
                else
                    _singleMatchForm.SetResults(false, "", -1, 0, match.Groups.Cast<Group>().Select((g, i) => new GroupResult(g)));
            }
        }

        private void EnsureClosed(params Form[] forms)
        {
            foreach (Form f in forms)
            {
                if (!(f is null))
                    using (f)
                        f.Close();
            }
        }

        private void ExpressionTextBox_TextChanged(object sender, EventArgs e)
        {
            string pattern = expressionTextBox.Text;
            if (pattern.Length > 0)
            {
                expressionErrorLabel.Text = "Expression required.";
                expressionErrorLabel.Visible = true;
                _expression = null;
            }
            else
                try
                {
                    if (noneOptionCheckBox.Checked)
                        _expression = new Regex(pattern);
                    else
                        _expression = new Regex(pattern, _options);
                    expressionErrorLabel.Visible = false;
                }
                catch (Exception exception)
                {
                    expressionErrorLabel.Text = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
                    expressionErrorLabel.Visible = true;
                    _expression = null;
                }
            if (_expression is null)
                evaluateButton.Enabled = false;
            else if (evaluateOnChangeCheckBox.Checked)
                Evaluate();
            else
                evaluateButton.Enabled = true;
        }

        private void InputText_Changed(object sender, EventArgs e)
        {
            if (evaluateOnChangeCheckBox.Checked && !(_expression is null))
                Evaluate();
        }

        private void InputTextOption_Changed(object sender, EventArgs e)
        {
            if (evaluateOnChangeCheckBox.Checked && (sender as RadioButton).Checked && !(_expression is null))
                Evaluate();
        }

        private void LineWrapCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            inputTextTextBox.WordWrap = lineWrapCheckBox.Checked;
        }

        private void EvaluateOnChangeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (evaluateOnChangeCheckBox.Checked)
            {
                evaluateButton.Enabled = false;
                if (!(_expression is null))
                    Evaluate();
            }
            else
                evaluateButton.Enabled = !(_expression is null);
        }

        private void EvaluateButton_Click(object sender, EventArgs e) => Evaluate();

        private void NoneOptionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (_ignoreOptionChange || !noneOptionCheckBox.Checked)
                return;
            _ignoreOptionChange = true;
            try
            {
                ignoreCaseOptionCheckBox.Checked = cultureInvariantOptionCheckBox.Checked = multilineOptionCheckBox.Checked = singlelineOptionCheckBox.Checked =
                    ignorePatternWhitespaceOptionCheckBox.Checked = rightToLeftOptionCheckBox.Checked = eCMAScriptOptionCheckBox.Checked =
                    explicitCaptureOptionCheckBox.Checked = compiledOptionCheckBox.Checked = false;
                _options = RegexOptions.None;
            }
            finally { _ignoreOptionChange = false; }
            if (evaluateOnChangeCheckBox.Checked && !(_expression is null))
                Evaluate();
        }

        private void OptionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (_ignoreOptionChange)
                return;
            _ignoreOptionChange = true;
            try
            {
                noneOptionCheckBox.Checked = false;
                _options = RegexOptions.None;
                if (ignoreCaseOptionCheckBox.Checked)
                    _options |= RegexOptions.IgnoreCase;
                if (cultureInvariantOptionCheckBox.Checked)
                    _options |= RegexOptions.CultureInvariant;
                if (multilineOptionCheckBox.Checked)
                    _options |= RegexOptions.Multiline;
                if (singlelineOptionCheckBox.Checked)
                    _options |= RegexOptions.Singleline;
                if (ignorePatternWhitespaceOptionCheckBox.Checked)
                    _options |= RegexOptions.IgnorePatternWhitespace;
                if (rightToLeftOptionCheckBox.Checked)
                    _options |= RegexOptions.RightToLeft;
                if (eCMAScriptOptionCheckBox.Checked)
                    _options |= RegexOptions.ECMAScript;
                if (explicitCaptureOptionCheckBox.Checked)
                    _options |= RegexOptions.ExplicitCapture;
                if (compiledOptionCheckBox.Checked)
                    _options |= RegexOptions.Compiled;
            }
            finally { _ignoreOptionChange = false; }
            if (evaluateOnChangeCheckBox.Checked && !(_expression is null))
                Evaluate();
        }
    }
}
