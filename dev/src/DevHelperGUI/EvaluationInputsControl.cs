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
    public partial class EvaluationInputsControl : UserControl
    {
        private const RegexOptions DefaultValue_PatternOptions = RegexOptions.None;
        private readonly object _syncRoot = new object();
        private BackgroundWorker _parseRegexBackgroundWorker;
        private BackgroundWorker _evaluateExpressionBackgroundWorker;
        private Regex _parsedRegex;
        private bool _patternParsePending = false;
        private bool _evaluationPending = false;
        private bool _parseLinesSeparately = false;
        private EvaluationInputsMode _mode;
        private RegexOptions _patternOptions = DefaultValue_PatternOptions;
        private EvaluationState _state = EvaluationState.NotEvaluated;
        private IEnumerable<string> _messages = Array.Empty<string>();
        public event EventHandler<EvaluationStateEventArgs> EvaluationStateChanged;
        public event EventHandler<BooleanValueEventArgs> ParseLinesSeparatelyChanged;
        public event EventHandler<EvaluationInputsModeEventArgs> ModeChanged;
        public event EventHandler<PatternOptionsEventArgs> PatternOptionsChanged;
        public event EventHandler<RegexEvaluatableEventArgs> EvaluateExpressionAsync;
        public event EventHandler<EvaluationFinishedEventArgs> ExpressionEvaluated;

        public EvaluationState State
        {
            get => _state;
            set
            {
                IEnumerable<string> messages;
                lock (_syncRoot)
                {
                    if (value == _state)
                        return;
                    _state = value;
                    messages = _messages;
                    _messages = Array.Empty<string>();
                }
                EvaluationStateChanged?.Invoke(this, new EvaluationStateEventArgs(State, messages));
            }
        }

        public EvaluationInputsMode Mode
        {
            get => _mode;
            set
            {
                lock (_syncRoot)
                {
                    if (_mode == value)
                        return;
                    _mode = value;
                }
                if (evaluateOnChangeCheckBox.Checked)
                    StartEvaluationAsync();
                else
                    evaluateButton.Enabled = true;
                ModeChanged?.Invoke(this, new EvaluationInputsModeEventArgs(_mode));
            }
        }

        public bool ParseLinesSeparately
        {
            get => _parseLinesSeparately;
            set
            {
                lock (_syncRoot)
                {
                    if (_parseLinesSeparately == value)
                        return;
                    _parseLinesSeparately = value;
                }
                if (evaluateOnChangeCheckBox.Checked)
                    StartEvaluationAsync();
                else
                    evaluateButton.Enabled = true;
                ParseLinesSeparatelyChanged?.Invoke(this, new BooleanValueEventArgs(_parseLinesSeparately));
            }
        }

        [DefaultValue(DefaultValue_PatternOptions)]
        [Localizable(true)]
        public RegexOptions PatternOptions
        {
            get => _patternOptions;
            set
            {
                lock (_syncRoot)
                {
                    if (value == _patternOptions)
                        return;
                    _patternOptions = value;
                    StartRegexParseAsync();
                }
                PatternOptionsChanged?.Invoke(this, new PatternOptionsEventArgs(_patternOptions));
            }
        }

        [DefaultValue(true)]
        [Localizable(true)]
        [Category("Behavior")]
        [Description("Indicates whether a multiline text box control automatically wraps words to the beginning of the next line when necessary.")]
        public bool WordWrap
        {
            get
            {
                TextBox textBox = inputTextBox;
                return textBox is null || textBox.WordWrap;
            }
            set
            {
                if (inputTextBox is null)
                {
                    if (!value)
                        throw new InvalidOperationException();
                }
                else
                    inputTextBox.WordWrap = value;
            }
        }

        [DefaultValue(true)]
        [Localizable(true)]
        [Category("Behavior")]
        [Description("Indicates whether the pattern is re-evaluated whenever the pattern, input text or options are changed.")]
        public bool EvaluateOnChange
        {
            get => !(evaluateOnChangeCheckBox is null) && evaluateOnChangeCheckBox.Checked;
            set
            {
                if (evaluateOnChangeCheckBox is null)
                {
                    if (value)
                        throw new InvalidOperationException();
                }
                else
                    evaluateOnChangeCheckBox.Checked = value;
            }
        }

        [DefaultValue(true)]
        [Localizable(true)]
        [Category("Appearance")]
        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor))]
        [Description("Contains the regular expression to use for the evaluation.")]
        public string PatternText
        {
            get => (patternTextBox is null) ? "" : patternTextBox.Text;
            set
            {
                if (patternTextBox is null)
                {
                    if (!string.IsNullOrEmpty(value))
                        throw new InvalidOperationException();
                }
                else
                    patternTextBox.Text = value ?? "";
            }
        }

        [DefaultValue(true)]
        [Localizable(true)]
        [Category("Appearance")]
        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor))]
        [Description("Contains the text to be evaluated.")]
        public string InputText
        {
            get => (inputTextBox is null) ? "" : inputTextBox.Text;
            set
            {
                if (inputTextBox is null)
                {
                    if (!string.IsNullOrEmpty(value))
                        throw new InvalidOperationException();
                }
                else
                    inputTextBox.Text = value ?? "";
            }
        }

        public EvaluationInputsControl()
        {
            InitializeComponent();
            _parseRegexBackgroundWorker = new BackgroundWorker();
            _evaluateExpressionBackgroundWorker = new BackgroundWorker();
            _parseRegexBackgroundWorker.WorkerSupportsCancellation = true;
            _parseRegexBackgroundWorker.DoWork += ParseRegexBackgroundWorker_DoWork;
            _parseRegexBackgroundWorker.RunWorkerCompleted += ParseRegexBackgroundWorker_RunWorkerCompleted;
            _evaluateExpressionBackgroundWorker.WorkerSupportsCancellation = true;
            _evaluateExpressionBackgroundWorker.DoWork += EvaluateExpressionBackgroundWorker_DoWork;
            _evaluateExpressionBackgroundWorker.RunWorkerCompleted += EvaluateExpressionBackgroundWorker_RunWorkerCompleted;
        }

        private void StartRegexParseAsync()
        {
            _patternParsePending = true;
            _evaluationPending = evaluateOnChangeCheckBox.Checked;
            if (_parseRegexBackgroundWorker.IsBusy)
                _parseRegexBackgroundWorker.CancelAsync();
            else
                _parseRegexBackgroundWorker.RunWorkerAsync(new object[] { patternTextBox.Text, _patternOptions });
        }

        private Func<Regex, string, RegexEvaluatableEventArgs> GetRegexEvaluatableEventArgsFactory()
        {
            if (_parseLinesSeparately)
            {
                switch (_mode)
                {
                    case EvaluationInputsMode.BackslashEscaped:
                        return (r, s) => new MultiRegexEvaluatableEventArgs(r, CaptureItem.FromEscapedTextLines(s));
                    case EvaluationInputsMode.UriEncoded:
                        return (r, s) => new MultiRegexEvaluatableEventArgs(r, CaptureItem.FromUriEncodedLines(s));
                    case EvaluationInputsMode.XmlEncoded:
                        return (r, s) => new MultiRegexEvaluatableEventArgs(r, CaptureItem.FromXmlEncodedLines(s));
                    default:
                        return (r, s) => new MultiRegexEvaluatableEventArgs(r, CaptureItem.FromRawTextLines(s));
                }
            }
            switch (_mode)
            {
                case EvaluationInputsMode.BackslashEscaped:
                    return (r, s) => new SingleRegexEvaluatableEventArgs(r, CaptureItem.FromEscapedText(s));
                case EvaluationInputsMode.UriEncoded:
                    return (r, s) => new SingleRegexEvaluatableEventArgs(r, CaptureItem.FromUriEncoded(s));
                case EvaluationInputsMode.XmlEncoded:
                    return (r, s) => new SingleRegexEvaluatableEventArgs(r, CaptureItem.FromXmlEncoded(s));
                default:
                    return (r, s) => new SingleRegexEvaluatableEventArgs(r, s);
            }
        }

        private void StartEvaluationAsync()
        {
            Regex regex = _parsedRegex;
            if (regex is null)
                return;
            _evaluationPending = true;
            if (_evaluateExpressionBackgroundWorker.IsBusy)
                _evaluateExpressionBackgroundWorker.CancelAsync();
            else
                _evaluateExpressionBackgroundWorker.RunWorkerAsync(new object[] { regex, inputTextBox.Text, GetRegexEvaluatableEventArgsFactory() });
        }

        private void ParseRegexBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (_parseRegexBackgroundWorker.CancellationPending)
                e.Cancel = true;
            else
            {
                
                _patternParsePending = false;
                object[] args = (object[])e.Argument;
                try { e.Result = new Regex((string)args[0], (RegexOptions)args[1]); }
                catch (Exception exception) { e.Result = string.IsNullOrWhiteSpace(exception.Message) ? e.ToString() : exception.Message;  }
            }
        }

        private void ParseRegexBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_patternParsePending && !_parseRegexBackgroundWorker.IsBusy)
                StartRegexParseAsync();
            else if (!e.Cancelled)
            {
                if (e.Error is null)
                {
                    if (e.Result is string message)
                        patternErrorLabel.Text = message;
                    else
                    {
                        patternErrorLabel.Visible = false;
                        _parsedRegex = e.Result as Regex;
                        try { State = EvaluationState.NotEvaluated; }
                        finally
                        {
                            if (_evaluationPending)
                                StartEvaluationAsync();
                            else
                                evaluateButton.Enabled = true;
                        }
                        return;
                    }
                }
                else
                    patternErrorLabel.Text = string.IsNullOrWhiteSpace(e.Error.Message) ? e.Error.ToString() : e.Error.Message;
                _parsedRegex = null;
                patternErrorLabel.Visible = true;
                evaluateButton.Enabled = false;
                State = EvaluationState.PatternError;
            }
        }

        private void EvaluateExpressionBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (_evaluateExpressionBackgroundWorker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
            _evaluationPending = false;
            object[] args = (object[])e.Argument;
            string s = (string)args[1];
            Regex regex = (Regex)args[0];
            EventHandler<RegexEvaluatableEventArgs> eventHandler = EvaluateExpressionAsync;
            if (eventHandler is null)
                throw new InvalidOperationException("No expression evaluation handler defined");
            RegexEvaluatableEventArgs eventArgs = ((Func<Regex, string, RegexEvaluatableEventArgs>)args[2]).Invoke((Regex)args[0], (string)args[1]);
            try
            {
                eventHandler.Invoke(this, eventArgs);
            }
            catch (OperationCanceledException)
            {
                if (!eventArgs.Cancel)
                    throw;
            }
            if (eventArgs.Cancel)
                e.Cancel = true;
            else
                e.Result = eventArgs;
        }

        private void EvaluateExpressionBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_evaluationPending && !_evaluateExpressionBackgroundWorker.IsBusy)
                StartEvaluationAsync();
            else if (!e.Cancelled)
            {
                string message;
                if (e.Error is null)
                {
                    RegexEvaluatableEventArgs result = e.Result as RegexEvaluatableEventArgs;
                    if (result is null)
                        return;
                    try
                    {
                        switch (result.State)
                        {
                            case EvaluationState.NotEvaluated:
                                message = string.IsNullOrWhiteSpace(result.ErrorMessage) ? "Nothing was evaluated" : result.ErrorMessage;
                                break;
                            case EvaluationState.EvaluationError:
                                message = result.ErrorMessage;
                                break;
                            default:
                                evaluationErrorLabel.Visible = false;
                                State = result.State;
                                evaluateButton.Enabled = false;
                                return;
                        }
                        lock (_syncRoot)
                            _messages = (_messages is null) ? new string[] { message } : _messages.Concat(new string[] { message }).ToArray();
                        evaluateButton.Enabled = false;
                        evaluationErrorLabel.Text = message;
                        evaluationErrorLabel.Visible = true;
                        State = result.State;
                    }
                    finally
                    {
                        ExpressionEvaluated?.Invoke(this, new EvaluationFinishedEventArgs(result, null));
                    }
                }
                else
                {
                    message = string.IsNullOrWhiteSpace(e.Error.Message) ? e.Error.ToString() : e.Error.Message;
                    lock (_syncRoot)
                        _messages = (_messages is null) ? new string[] { message } : _messages.Concat(new string[] { message }).ToArray();
                    evaluationErrorLabel.Text = message;
                    evaluationErrorLabel.Visible = true;
                    State = EvaluationState.EvaluationError;
                }
            }
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

        private void EvaluateButton_Click(object sender, EventArgs e)
        {
            evaluateButton.Enabled = false;
            StartEvaluationAsync();
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

        internal void CancelWorkers()
        {
            _patternParsePending = _evaluationPending = false;
            if (_parseRegexBackgroundWorker.IsBusy)
                _parseRegexBackgroundWorker.CancelAsync();
            if (_evaluateExpressionBackgroundWorker.IsBusy)
                _evaluateExpressionBackgroundWorker.CancelAsync();
            patternTextBox.Text = inputTextBox.Text = "";
            patternErrorLabel.Visible = evaluationErrorLabel.Visible = false;
        }
    }
}
