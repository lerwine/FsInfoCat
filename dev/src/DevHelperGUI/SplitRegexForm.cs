using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DevHelperGUI
{
    public partial class SplitRegexForm : Form
    {
        private RegexOptions _options = RegexOptions.None;
        private string[] _inputText = Array.Empty<string>();

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

        patternTextBox
        expressionErrorLabel
        inputTextBox
        evaluateOnChangeCheckBox
        evaluateButton
        */
        public SplitRegexForm()
        {
            InitializeComponent();
        }

        private void OnInputTextChanged()
        {
            throw new NotImplementedException();
        }

        private void SetRawMode()
        {
            throw new NotImplementedException();
        }

        private void SetEscapedValue(bool escapeTabsAndNewLines)
        {
            throw new NotImplementedException();
        }

        private void SetQuotedLines(bool escapeTabsAndNewLines)
        {
            throw new NotImplementedException();
        }

        private void SetUriEncoded(bool escapeTabsAndNewLines)
        {
            throw new NotImplementedException();
        }

        private void SetXmlEncoded(bool escapeTabsAndNewLines)
        {
            throw new NotImplementedException();
        }

        private void InputModeAsIsToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (inputModeAsIsToolStripMenuItem.Checked)
            {
                inputModeBackslashedToolStripMenuItem.Checked = inputModeUriEncodedToolStripMenuItem.Checked = inputModeXmlEncodedToolStripMenuItem.Checked = false;
                inputModeBackslashedToolStripMenuItem.Enabled = inputModeUriEncodedToolStripMenuItem.Enabled = inputModeXmlEncodedToolStripMenuItem.Enabled = false;
                inputModeAsIsToolStripMenuItem.Enabled = false;
                string[] text = (separateLinesToolStripMenuItem.Checked) ? CaptureItem.FromRawTextLines(inputTextBox.Text) : new string[] { inputTextBox.Text };
                if (text.Length.Equals(_inputText.Length) && !text.Zip(_inputText, (a, b) => a.Equals(b)).Contains(false))
                    return;
                _inputText = text;
                OnInputTextChanged();
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
                string[] text = (separateLinesToolStripMenuItem.Checked) ? CaptureItem.FromEscapedTextLines(inputTextBox.Text) :
                    new string[] { CaptureItem.FromEscapedText(inputTextBox.Text) };
                if (text.Length.Equals(_inputText.Length) && !text.Zip(_inputText, (a, b) => a.Equals(b)).Contains(false))
                    return;
                _inputText = text;
                OnInputTextChanged();
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
                string[] text = (separateLinesToolStripMenuItem.Checked) ? CaptureItem.FromUriEncodedLines(inputTextBox.Text) :
                    new string[] { CaptureItem.FromUriEncoded(inputTextBox.Text) };
                if (text.Length.Equals(_inputText.Length) && !text.Zip(_inputText, (a, b) => a.Equals(b)).Contains(false))
                    return;
                _inputText = text;
                OnInputTextChanged();
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
                string[] text = (separateLinesToolStripMenuItem.Checked) ? CaptureItem.FromXmlEncodedLines(inputTextBox.Text) :
                    new string[] { CaptureItem.FromXmlEncoded(inputTextBox.Text) };
                if (text.Length.Equals(_inputText.Length) && !text.Zip(_inputText, (a, b) => a.Equals(b)).Contains(false))
                    return;
                _inputText = text;
                OnInputTextChanged();
            }
            else
                inputModeXmlEncodedToolStripMenuItem.Enabled = true;
        }

        private void SeparateLinesToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            string[] text;
            if (separateLinesToolStripMenuItem.Checked)
            {
                separateLinesToolStripMenuItem.Enabled = false;
                if (inputModeBackslashedToolStripMenuItem.Checked)
                    text = CaptureItem.FromEscapedTextLines(inputTextBox.Text);
                else if (inputModeUriEncodedToolStripMenuItem.Checked)
                    text = CaptureItem.FromUriEncodedLines(inputTextBox.Text);
                else if (inputModeXmlEncodedToolStripMenuItem.Checked)
                    text = CaptureItem.FromXmlEncodedLines(inputTextBox.Text);
                else
                    text = CaptureItem.FromRawTextLines(inputTextBox.Text);
            }
            else
            {
                separateLinesToolStripMenuItem.Enabled = true;
                if (inputModeBackslashedToolStripMenuItem.Checked)
                    text = new string[] { CaptureItem.FromEscapedText(inputTextBox.Text) };
                else if (inputModeUriEncodedToolStripMenuItem.Checked)
                    text = new string[] { CaptureItem.FromUriEncoded(inputTextBox.Text) };
                else if (inputModeXmlEncodedToolStripMenuItem.Checked)
                    text = new string[] { CaptureItem.FromXmlEncoded(inputTextBox.Text) };
                else
                    text = new string[] { inputTextBox.Text };
            }
            if (text.Length.Equals(_inputText.Length) && !text.Zip(_inputText, (a, b) => a.Equals(b)).Contains(false))
                return;
            _inputText = text;
            OnInputTextChanged();
        }

        private void InputModeWordWrapToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void NoneToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
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
                _options = RegexOptions.None;
            }
            else
                noneToolStripMenuItem.Enabled = true;
        }

        private void IgnoreCaseToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (ignoreCaseToolStripMenuItem.Checked)
            {
                noneToolStripMenuItem.Enabled = true;
                noneToolStripMenuItem.Checked = false;
                _options |= RegexOptions.IgnoreCase;
            }
            else if (multilineToolStripMenuItem.Checked || explicitCaptureToolStripMenuItem.Checked || compiledToolStripMenuItem.Checked ||
                singleLineToolStripMenuItem.Checked || ignorePatternWhitespaceToolStripMenuItem.Checked || rightToLeftToolStripMenuItem.Checked ||
                ecmaScriptToolStripMenuItem.Checked || cultureInvariantToolStripMenuItem.Checked)
            {
                _options ^= RegexOptions.IgnoreCase;
            }
            else
            {
                noneToolStripMenuItem.Checked = true;
                _options = RegexOptions.None;
            }
        }

        private void CultureInvariantToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (cultureInvariantToolStripMenuItem.Checked)
            {
                noneToolStripMenuItem.Enabled = true;
                noneToolStripMenuItem.Checked = false;
                _options |= RegexOptions.CultureInvariant;
            }
            else if (ignoreCaseToolStripMenuItem.Checked || multilineToolStripMenuItem.Checked || explicitCaptureToolStripMenuItem.Checked ||
                    compiledToolStripMenuItem.Checked || singleLineToolStripMenuItem.Checked || ignorePatternWhitespaceToolStripMenuItem.Checked ||
                    rightToLeftToolStripMenuItem.Checked || ecmaScriptToolStripMenuItem.Checked)
            {
                _options ^= RegexOptions.CultureInvariant;
            }
            else
            {
                noneToolStripMenuItem.Checked = true;
                _options = RegexOptions.None;
            }
        }

        private void MultilineToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (multilineToolStripMenuItem.Checked)
            {
                noneToolStripMenuItem.Enabled = true;
                noneToolStripMenuItem.Checked = false;
                _options |= RegexOptions.Multiline;
            }
            else if (ignoreCaseToolStripMenuItem.Checked || explicitCaptureToolStripMenuItem.Checked || compiledToolStripMenuItem.Checked ||
                singleLineToolStripMenuItem.Checked || ignorePatternWhitespaceToolStripMenuItem.Checked || rightToLeftToolStripMenuItem.Checked ||
                ecmaScriptToolStripMenuItem.Checked || cultureInvariantToolStripMenuItem.Checked)
            {
                _options ^= RegexOptions.Multiline;
            }
            else
            {
                noneToolStripMenuItem.Checked = true;
                _options = RegexOptions.None;
            }
        }

        private void SinglelineToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (singleLineToolStripMenuItem.Checked)
            {
                noneToolStripMenuItem.Enabled = true;
                noneToolStripMenuItem.Checked = false;
                _options |= RegexOptions.Singleline;
            }
            else if (ignoreCaseToolStripMenuItem.Checked || multilineToolStripMenuItem.Checked || explicitCaptureToolStripMenuItem.Checked ||
                compiledToolStripMenuItem.Checked || ignorePatternWhitespaceToolStripMenuItem.Checked || rightToLeftToolStripMenuItem.Checked ||
                ecmaScriptToolStripMenuItem.Checked || cultureInvariantToolStripMenuItem.Checked)
            {
                _options ^= RegexOptions.Singleline;
            }
            else
            {
                noneToolStripMenuItem.Checked = true;
                _options = RegexOptions.None;
            }
        }

        private void ExplicitCaptureToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (explicitCaptureToolStripMenuItem.Checked)
            {
                noneToolStripMenuItem.Enabled = true;
                noneToolStripMenuItem.Checked = false;
                _options |= RegexOptions.ExplicitCapture;
            }
            else if (ignoreCaseToolStripMenuItem.Checked || multilineToolStripMenuItem.Checked || compiledToolStripMenuItem.Checked ||
                singleLineToolStripMenuItem.Checked || ignorePatternWhitespaceToolStripMenuItem.Checked || rightToLeftToolStripMenuItem.Checked ||
                ecmaScriptToolStripMenuItem.Checked || cultureInvariantToolStripMenuItem.Checked)
            {
                _options ^= RegexOptions.ExplicitCapture;
            }
            else
            {
                noneToolStripMenuItem.Checked = true;
                _options = RegexOptions.None;
            }
        }

        private void IgnorePatternWhitespaceToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (ignorePatternWhitespaceToolStripMenuItem.Checked)
            {
                noneToolStripMenuItem.Enabled = true;
                noneToolStripMenuItem.Checked = false;
                _options |= RegexOptions.IgnorePatternWhitespace;
            }
            else if (ignoreCaseToolStripMenuItem.Checked || multilineToolStripMenuItem.Checked || explicitCaptureToolStripMenuItem.Checked ||
                compiledToolStripMenuItem.Checked || singleLineToolStripMenuItem.Checked || rightToLeftToolStripMenuItem.Checked ||
                ecmaScriptToolStripMenuItem.Checked || cultureInvariantToolStripMenuItem.Checked)
            {
                _options ^= RegexOptions.IgnorePatternWhitespace;
            }
            else
            {
                noneToolStripMenuItem.Checked = true;
                _options = RegexOptions.None;
            }
        }

        private void RightToLeftToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (rightToLeftToolStripMenuItem.Checked)
            {
                noneToolStripMenuItem.Enabled = true;
                noneToolStripMenuItem.Checked = false;
                _options |= RegexOptions.RightToLeft;
            }
            else if (ignoreCaseToolStripMenuItem.Checked || multilineToolStripMenuItem.Checked || explicitCaptureToolStripMenuItem.Checked ||
                compiledToolStripMenuItem.Checked || singleLineToolStripMenuItem.Checked || ignorePatternWhitespaceToolStripMenuItem.Checked ||
                ecmaScriptToolStripMenuItem.Checked || cultureInvariantToolStripMenuItem.Checked)
            {
                _options ^= RegexOptions.RightToLeft;
            }
            else
            {
                noneToolStripMenuItem.Checked = true;
                _options = RegexOptions.None;
            }
        }

        private void ECMAScriptToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (ecmaScriptToolStripMenuItem.Checked)
            {
                noneToolStripMenuItem.Enabled = true;
                noneToolStripMenuItem.Checked = false;
                _options |= RegexOptions.ECMAScript;
            }
            else if (ignoreCaseToolStripMenuItem.Checked || multilineToolStripMenuItem.Checked || explicitCaptureToolStripMenuItem.Checked ||
                compiledToolStripMenuItem.Checked || singleLineToolStripMenuItem.Checked || ignorePatternWhitespaceToolStripMenuItem.Checked ||
                rightToLeftToolStripMenuItem.Checked || cultureInvariantToolStripMenuItem.Checked)
            {
                _options ^= RegexOptions.ECMAScript;
            }
            else
            {
                noneToolStripMenuItem.Checked = true;
                _options = RegexOptions.None;
            }
        }

        private void CompiledToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (compiledToolStripMenuItem.Checked)
            {
                noneToolStripMenuItem.Enabled = true;
                noneToolStripMenuItem.Checked = false;
                _options |= RegexOptions.Compiled;
            }
            else if (ignoreCaseToolStripMenuItem.Checked || multilineToolStripMenuItem.Checked || explicitCaptureToolStripMenuItem.Checked ||
                singleLineToolStripMenuItem.Checked || ignorePatternWhitespaceToolStripMenuItem.Checked || rightToLeftToolStripMenuItem.Checked ||
                ecmaScriptToolStripMenuItem.Checked || cultureInvariantToolStripMenuItem.Checked)
            {
                _options ^= RegexOptions.Compiled;
            }
            else
            {
                noneToolStripMenuItem.Checked = true;
                _options = RegexOptions.None;
            }
        }

        private void RawValueToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (rawValueToolStripMenuItem.Checked)
            {
                rawValueToolStripMenuItem.Enabled = escapedValueNormalToolStripMenuItem.Enabled = escapedValueTnlToolStripMenuItem.Enabled = quotedLinesNormalToolStripMenuItem.Enabled = quotedLinesTnlToolStripMenuItem.Enabled = uriEncodedNormalToolStripMenuItem.Enabled = uriEncodedNlToolStripMenuItem.Enabled = xmlEncodedNormalToolStripMenuItem.Enabled = xmlEncodedTnlToolStripMenuItem.Enabled = true;
                rawValueToolStripMenuItem.Checked = escapedValueNormalToolStripMenuItem.Checked = escapedValueTnlToolStripMenuItem.Checked = quotedLinesNormalToolStripMenuItem.Checked = quotedLinesTnlToolStripMenuItem.Checked = uriEncodedNormalToolStripMenuItem.Checked = uriEncodedNlToolStripMenuItem.Checked = xmlEncodedNormalToolStripMenuItem.Checked = xmlEncodedTnlToolStripMenuItem.Checked = false;
                rawValueToolStripMenuItem.Enabled = false;
                SetRawMode();
            }
            else
                rawValueToolStripMenuItem.Enabled = true;
        }

        private void EscapedValueNormalToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (escapedValueNormalToolStripMenuItem.Checked)
            {
                rawValueToolStripMenuItem.Enabled = escapedValueNormalToolStripMenuItem.Enabled = escapedValueTnlToolStripMenuItem.Enabled = quotedLinesNormalToolStripMenuItem.Enabled = quotedLinesTnlToolStripMenuItem.Enabled = uriEncodedNormalToolStripMenuItem.Enabled = uriEncodedNlToolStripMenuItem.Enabled = xmlEncodedNormalToolStripMenuItem.Enabled = xmlEncodedTnlToolStripMenuItem.Enabled = true;
                rawValueToolStripMenuItem.Checked = escapedValueNormalToolStripMenuItem.Checked = escapedValueTnlToolStripMenuItem.Checked = quotedLinesNormalToolStripMenuItem.Checked = quotedLinesTnlToolStripMenuItem.Checked = uriEncodedNormalToolStripMenuItem.Checked = uriEncodedNlToolStripMenuItem.Checked = xmlEncodedNormalToolStripMenuItem.Checked = xmlEncodedTnlToolStripMenuItem.Checked = false;
                escapedValueNormalToolStripMenuItem.Enabled = false;
                SetEscapedValue(false);
            }
            else
                escapedValueNormalToolStripMenuItem.Enabled = true;
        }

        private void EscapedValueTnlToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (escapedValueTnlToolStripMenuItem.Checked)
            {
                rawValueToolStripMenuItem.Enabled = escapedValueNormalToolStripMenuItem.Enabled = escapedValueTnlToolStripMenuItem.Enabled = quotedLinesNormalToolStripMenuItem.Enabled = quotedLinesTnlToolStripMenuItem.Enabled = uriEncodedNormalToolStripMenuItem.Enabled = uriEncodedNlToolStripMenuItem.Enabled = xmlEncodedNormalToolStripMenuItem.Enabled = xmlEncodedTnlToolStripMenuItem.Enabled = true;
                rawValueToolStripMenuItem.Checked = escapedValueNormalToolStripMenuItem.Checked = escapedValueTnlToolStripMenuItem.Checked = quotedLinesNormalToolStripMenuItem.Checked = quotedLinesTnlToolStripMenuItem.Checked = uriEncodedNormalToolStripMenuItem.Checked = uriEncodedNlToolStripMenuItem.Checked = xmlEncodedNormalToolStripMenuItem.Checked = xmlEncodedTnlToolStripMenuItem.Checked = false;
                escapedValueTnlToolStripMenuItem.Enabled = false;
                SetEscapedValue(true);
            }
            else
                escapedValueTnlToolStripMenuItem.Enabled = true;
        }

        private void QuotedLinesNormalToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (quotedLinesNormalToolStripMenuItem.Checked)
            {
                rawValueToolStripMenuItem.Enabled = escapedValueNormalToolStripMenuItem.Enabled = escapedValueTnlToolStripMenuItem.Enabled = quotedLinesNormalToolStripMenuItem.Enabled = quotedLinesTnlToolStripMenuItem.Enabled = uriEncodedNormalToolStripMenuItem.Enabled = uriEncodedNlToolStripMenuItem.Enabled = xmlEncodedNormalToolStripMenuItem.Enabled = xmlEncodedTnlToolStripMenuItem.Enabled = true;
                rawValueToolStripMenuItem.Checked = escapedValueNormalToolStripMenuItem.Checked = escapedValueTnlToolStripMenuItem.Checked = quotedLinesNormalToolStripMenuItem.Checked = quotedLinesTnlToolStripMenuItem.Checked = uriEncodedNormalToolStripMenuItem.Checked = uriEncodedNlToolStripMenuItem.Checked = xmlEncodedNormalToolStripMenuItem.Checked = xmlEncodedTnlToolStripMenuItem.Checked = false;
                quotedLinesNormalToolStripMenuItem.Enabled = false;
                SetQuotedLines(false);
            }
            else
                quotedLinesNormalToolStripMenuItem.Enabled = true;
        }

        private void QuotedLinesTnlToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (quotedLinesTnlToolStripMenuItem.Checked)
            {
                rawValueToolStripMenuItem.Enabled = escapedValueNormalToolStripMenuItem.Enabled = escapedValueTnlToolStripMenuItem.Enabled = quotedLinesNormalToolStripMenuItem.Enabled = quotedLinesTnlToolStripMenuItem.Enabled = uriEncodedNormalToolStripMenuItem.Enabled = uriEncodedNlToolStripMenuItem.Enabled = xmlEncodedNormalToolStripMenuItem.Enabled = xmlEncodedTnlToolStripMenuItem.Enabled = true;
                rawValueToolStripMenuItem.Checked = escapedValueNormalToolStripMenuItem.Checked = escapedValueTnlToolStripMenuItem.Checked = quotedLinesNormalToolStripMenuItem.Checked = quotedLinesTnlToolStripMenuItem.Checked = uriEncodedNormalToolStripMenuItem.Checked = uriEncodedNlToolStripMenuItem.Checked = xmlEncodedNormalToolStripMenuItem.Checked = xmlEncodedTnlToolStripMenuItem.Checked = false;
                quotedLinesTnlToolStripMenuItem.Enabled = false;
                SetQuotedLines(true);
            }
            else
                quotedLinesTnlToolStripMenuItem.Enabled = true;
        }

        private void UriEncodedNormalToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (uriEncodedNormalToolStripMenuItem.Checked)
            {
                rawValueToolStripMenuItem.Enabled = escapedValueNormalToolStripMenuItem.Enabled = escapedValueTnlToolStripMenuItem.Enabled = quotedLinesNormalToolStripMenuItem.Enabled = quotedLinesTnlToolStripMenuItem.Enabled = uriEncodedNormalToolStripMenuItem.Enabled = uriEncodedNlToolStripMenuItem.Enabled = xmlEncodedNormalToolStripMenuItem.Enabled = xmlEncodedTnlToolStripMenuItem.Enabled = true;
                rawValueToolStripMenuItem.Checked = escapedValueNormalToolStripMenuItem.Checked = escapedValueTnlToolStripMenuItem.Checked = quotedLinesNormalToolStripMenuItem.Checked = quotedLinesTnlToolStripMenuItem.Checked = uriEncodedNormalToolStripMenuItem.Checked = uriEncodedNlToolStripMenuItem.Checked = xmlEncodedNormalToolStripMenuItem.Checked = xmlEncodedTnlToolStripMenuItem.Checked = false;
                uriEncodedNormalToolStripMenuItem.Enabled = false;
                SetUriEncoded(false);
            }
            else
                uriEncodedNormalToolStripMenuItem.Enabled = true;
        }

        private void UriEncodedNlToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (uriEncodedNlToolStripMenuItem.Checked)
            {
                rawValueToolStripMenuItem.Enabled = escapedValueNormalToolStripMenuItem.Enabled = escapedValueTnlToolStripMenuItem.Enabled = quotedLinesNormalToolStripMenuItem.Enabled = quotedLinesTnlToolStripMenuItem.Enabled = uriEncodedNormalToolStripMenuItem.Enabled = uriEncodedNlToolStripMenuItem.Enabled = xmlEncodedNormalToolStripMenuItem.Enabled = xmlEncodedTnlToolStripMenuItem.Enabled = true;
                rawValueToolStripMenuItem.Checked = escapedValueNormalToolStripMenuItem.Checked = escapedValueTnlToolStripMenuItem.Checked = quotedLinesNormalToolStripMenuItem.Checked = quotedLinesTnlToolStripMenuItem.Checked = uriEncodedNormalToolStripMenuItem.Checked = uriEncodedNlToolStripMenuItem.Checked = xmlEncodedNormalToolStripMenuItem.Checked = xmlEncodedTnlToolStripMenuItem.Checked = false;
                uriEncodedNlToolStripMenuItem.Enabled = false;
                SetUriEncoded(true);
            }
            else
                uriEncodedNlToolStripMenuItem.Enabled = true;
        }

        private void XmlEncodedNormalToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (xmlEncodedNormalToolStripMenuItem.Checked)
            {
                rawValueToolStripMenuItem.Enabled = escapedValueNormalToolStripMenuItem.Enabled = escapedValueTnlToolStripMenuItem.Enabled = quotedLinesNormalToolStripMenuItem.Enabled = quotedLinesTnlToolStripMenuItem.Enabled = uriEncodedNormalToolStripMenuItem.Enabled = uriEncodedNlToolStripMenuItem.Enabled = xmlEncodedNormalToolStripMenuItem.Enabled = xmlEncodedTnlToolStripMenuItem.Enabled = true;
                rawValueToolStripMenuItem.Checked = escapedValueNormalToolStripMenuItem.Checked = escapedValueTnlToolStripMenuItem.Checked = quotedLinesNormalToolStripMenuItem.Checked = quotedLinesTnlToolStripMenuItem.Checked = uriEncodedNormalToolStripMenuItem.Checked = uriEncodedNlToolStripMenuItem.Checked = xmlEncodedNormalToolStripMenuItem.Checked = xmlEncodedTnlToolStripMenuItem.Checked = false;
                xmlEncodedNormalToolStripMenuItem.Enabled = false;
                SetXmlEncoded(false);
            }
            else
                xmlEncodedNormalToolStripMenuItem.Enabled = true;
        }

        private void XmlEncodedTnlToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (xmlEncodedTnlToolStripMenuItem.Checked)
            {
                rawValueToolStripMenuItem.Enabled = escapedValueNormalToolStripMenuItem.Enabled = escapedValueTnlToolStripMenuItem.Enabled = quotedLinesNormalToolStripMenuItem.Enabled = quotedLinesTnlToolStripMenuItem.Enabled = uriEncodedNormalToolStripMenuItem.Enabled = uriEncodedNlToolStripMenuItem.Enabled = xmlEncodedNormalToolStripMenuItem.Enabled = xmlEncodedTnlToolStripMenuItem.Enabled = true;
                rawValueToolStripMenuItem.Checked = escapedValueNormalToolStripMenuItem.Checked = escapedValueTnlToolStripMenuItem.Checked = quotedLinesNormalToolStripMenuItem.Checked = quotedLinesTnlToolStripMenuItem.Checked = uriEncodedNormalToolStripMenuItem.Checked = uriEncodedNlToolStripMenuItem.Checked = xmlEncodedNormalToolStripMenuItem.Checked = xmlEncodedTnlToolStripMenuItem.Checked = false;
                xmlEncodedTnlToolStripMenuItem.Enabled = false;
                SetXmlEncoded(true);
            }
            else
                xmlEncodedTnlToolStripMenuItem.Enabled = true;
        }

        private void ViewModeWordWrapToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void PatternTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void InputTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void EvaluateOnChangeCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void EvaluateButton_Click(object sender, EventArgs e)
        {

        }
    }
}
