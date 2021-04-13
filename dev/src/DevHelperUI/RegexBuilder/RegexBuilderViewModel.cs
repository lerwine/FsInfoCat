using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DevHelperUI.RegexBuilder
{
    public class RegexBuilderViewModel : DependencyObject
    {
        private bool _ignoreOptionChange = false;
        private Collection<InputTextItem> _inputTextItems = new Collection<InputTextItem>();
        private Collection<PatternGroupItem> _patternGroupItems = new Collection<PatternGroupItem>();

        public string PatternText
        {
            get { return (string)GetValue(PatternTextProperty); }
            set { SetValue(PatternTextProperty, value); }
        }

        public static readonly DependencyProperty PatternTextProperty = DependencyProperty.Register(nameof(PatternText), typeof(string), typeof(RegexBuilderViewModel),
            new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                ((RegexBuilderViewModel)d).OnPatternTextPropertyChanged((string)e.OldValue, (string)e.NewValue)));

        public ReadOnlyCollection<InputTextItem> InputTextItems
        {
            get { return (ReadOnlyCollection<InputTextItem>)GetValue(InputTextItemsProperty); }
            private set { SetValue(InputTextItemsPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey InputTextItemsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(InputTextItems),
            typeof(ReadOnlyCollection<InputTextItem>), typeof(RegexBuilderViewModel), new PropertyMetadata(null));

        public static readonly DependencyProperty InputTextItemsProperty = InputTextItemsPropertyKey.DependencyProperty;

        private Command.RelayCommand _addCommand = null;

        public Command.RelayCommand AddCommand
        {
            get
            {
                if (_addCommand == null)
                    _addCommand = new Command.RelayCommand(() =>
                    {
                        InputTextItem item = new InputTextItem();
                        item.Delete += Item_Delete;
                        if (_inputTextItems.Count == 1)
                            _inputTextItems[0].DeleteCommand.IsEnabled = true;
                        _inputTextItems.Add(item);
                    });

                return _addCommand;
            }
        }

        public RegexOptions SelectedOptions
        {
            get { return (RegexOptions)GetValue(SelectedOptionsProperty); }
            private set { SetValue(SelectedOptionsPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey SelectedOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SelectedOptions), typeof(RegexOptions),
            typeof(RegexBuilderViewModel), new PropertyMetadata(RegexOptions.None, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                ((RegexBuilderViewModel)d).OnSelectedOptionsPropertyChanged((RegexOptions)e.OldValue, (RegexOptions)e.NewValue)));

        public static readonly DependencyProperty SelectedOptionsProperty = SelectedOptionsPropertyKey.DependencyProperty;

        public RegexOptions EffectiveOptions
        {
            get { return (RegexOptions)GetValue(EffectiveOptionsProperty); }
            private set { SetValue(EffectiveOptionsPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey EffectiveOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(EffectiveOptions), typeof(RegexOptions),
            typeof(RegexBuilderViewModel), new PropertyMetadata(RegexOptions.None, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                ((RegexBuilderViewModel)d).OnEffectiveOptionsPropertyChanged((RegexOptions)e.OldValue, (RegexOptions)e.NewValue)));

        public static readonly DependencyProperty EffectiveOptionsProperty = EffectiveOptionsPropertyKey.DependencyProperty;

        public bool NoneOption
        {
            get { return (bool)GetValue(NoneOptionProperty); }
            set { SetValue(NoneOptionProperty, value); }
        }

        public static readonly DependencyProperty NoneOptionProperty = DependencyProperty.Register(nameof(NoneOption), typeof(bool), typeof(RegexBuilderViewModel),
            new PropertyMetadata(true, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                ((RegexBuilderViewModel)d).OnOptionPropertyChanged(RegexOptions.None, (bool)e.OldValue, (bool)e.NewValue)));

        public bool NoneEnabled
        {
            get { return (bool)GetValue(NoneEnabledProperty); }
            private set { SetValue(NoneEnabledPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey NoneEnabledPropertyKey = DependencyProperty.RegisterReadOnly(nameof(NoneEnabled), typeof(bool),
            typeof(RegexBuilderViewModel), new PropertyMetadata(false));

        public static readonly DependencyProperty NoneEnabledProperty = NoneEnabledPropertyKey.DependencyProperty;

        public bool? IgnoreCaseOption
        {
            get { return (bool?)GetValue(IgnoreCaseOptionProperty); }
            set { SetValue(IgnoreCaseOptionProperty, value); }
        }

        public static readonly DependencyProperty IgnoreCaseOptionProperty = DependencyProperty.Register(nameof(IgnoreCaseOption), typeof(bool?),
            typeof(RegexBuilderViewModel), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                ((RegexBuilderViewModel)d).OnOptionPropertyChanged(RegexOptions.IgnoreCase, (bool?)e.OldValue, (bool?)e.NewValue)));

        public bool IgnoreCaseThreeState
        {
            get { return (bool)GetValue(IgnoreCaseThreeStateProperty); }
            private set { SetValue(IgnoreCaseThreeStatePropertyKey, value); }
        }

        private static readonly DependencyPropertyKey IgnoreCaseThreeStatePropertyKey = DependencyProperty.RegisterReadOnly(nameof(IgnoreCaseThreeState), typeof(bool),
            typeof(RegexBuilderViewModel), new PropertyMetadata(false));

        public static readonly DependencyProperty IgnoreCaseThreeStateProperty = IgnoreCaseThreeStatePropertyKey.DependencyProperty;

        public bool? MultilineOption
        {
            get { return (bool?)GetValue(MultilineOptionProperty); }
            set { SetValue(MultilineOptionProperty, value); }
        }

        public static readonly DependencyProperty MultilineOptionProperty = DependencyProperty.Register(nameof(MultilineOption), typeof(bool?),
            typeof(RegexBuilderViewModel), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                ((RegexBuilderViewModel)d).OnOptionPropertyChanged(RegexOptions.Multiline, (bool?)e.OldValue, (bool?)e.NewValue)));

        public bool MultilineThreeState
        {
            get { return (bool)GetValue(MultilineThreeStateProperty); }
            private set { SetValue(MultilineThreeStatePropertyKey, value); }
        }

        private static readonly DependencyPropertyKey MultilineThreeStatePropertyKey = DependencyProperty.RegisterReadOnly(nameof(MultilineThreeState), typeof(bool),
            typeof(RegexBuilderViewModel), new PropertyMetadata(false));

        public static readonly DependencyProperty MultilineThreeStateProperty = MultilineThreeStatePropertyKey.DependencyProperty;

        public bool ExplicitCaptureOption
        {
            get { return (bool)GetValue(ExplicitCaptureOptionProperty); }
            set { SetValue(ExplicitCaptureOptionProperty, value); }
        }

        public static readonly DependencyProperty ExplicitCaptureOptionProperty = DependencyProperty.Register(nameof(ExplicitCaptureOption), typeof(bool),
            typeof(RegexBuilderViewModel), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                ((RegexBuilderViewModel)d).OnOptionPropertyChanged(RegexOptions.ExplicitCapture, (bool)e.OldValue, (bool)e.NewValue)));

        public bool? CompiledOption
        {
            get { return (bool?)GetValue(CompiledOptionProperty); }
            set { SetValue(CompiledOptionProperty, value); }
        }

        public static readonly DependencyProperty CompiledOptionProperty = DependencyProperty.Register(nameof(CompiledOption), typeof(bool?),
            typeof(RegexBuilderViewModel), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                ((RegexBuilderViewModel)d).OnOptionPropertyChanged(RegexOptions.Compiled, (bool?)e.OldValue, (bool?)e.NewValue)));

        public bool CompiledThreeState
        {
            get { return (bool)GetValue(CompiledThreeStateProperty); }
            private set { SetValue(CompiledThreeStatePropertyKey, value); }
        }

        private static readonly DependencyPropertyKey CompiledThreeStatePropertyKey = DependencyProperty.RegisterReadOnly(nameof(CompiledThreeState), typeof(bool),
            typeof(RegexBuilderViewModel), new PropertyMetadata(false));

        public static readonly DependencyProperty CompiledThreeStateProperty = CompiledThreeStatePropertyKey.DependencyProperty;

        public bool SinglelineOption
        {
            get { return (bool)GetValue(SinglelineOptionProperty); }
            set { SetValue(SinglelineOptionProperty, value); }
        }

        public static readonly DependencyProperty SinglelineOptionProperty = DependencyProperty.Register(nameof(SinglelineOption), typeof(bool),
            typeof(RegexBuilderViewModel), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                ((RegexBuilderViewModel)d).OnOptionPropertyChanged(RegexOptions.Singleline, (bool)e.OldValue, (bool)e.NewValue)));

        public bool IgnorePatternWhitespaceOption
        {
            get { return (bool)GetValue(IgnorePatternWhitespaceOptionProperty); }
            set { SetValue(IgnorePatternWhitespaceOptionProperty, value); }
        }

        public static readonly DependencyProperty IgnorePatternWhitespaceOptionProperty = DependencyProperty.Register(nameof(IgnorePatternWhitespaceOption),
            typeof(bool), typeof(RegexBuilderViewModel), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                ((RegexBuilderViewModel)d).OnOptionPropertyChanged(RegexOptions.IgnorePatternWhitespace, (bool)e.OldValue, (bool)e.NewValue)));

        public bool RightToLeftOption
        {
            get { return (bool)GetValue(RightToLeftOptionProperty); }
            set { SetValue(RightToLeftOptionProperty, value); }
        }

        public static readonly DependencyProperty RightToLeftOptionProperty = DependencyProperty.Register(nameof(RightToLeftOption), typeof(bool),
            typeof(RegexBuilderViewModel), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                ((RegexBuilderViewModel)d).OnOptionPropertyChanged(RegexOptions.RightToLeft, (bool)e.OldValue, (bool)e.NewValue)));

        public bool ECMAScriptOption
        {
            get { return (bool)GetValue(ECMAScriptOptionProperty); }
            set { SetValue(ECMAScriptOptionProperty, value); }
        }

        public static readonly DependencyProperty ECMAScriptOptionProperty = DependencyProperty.Register(nameof(ECMAScriptOption), typeof(bool),
            typeof(RegexBuilderViewModel), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                ((RegexBuilderViewModel)d).OnOptionPropertyChanged(RegexOptions.ECMAScript, (bool)e.OldValue, (bool)e.NewValue)));

        public bool CultureInvariantOption
        {
            get { return (bool)GetValue(CultureInvariantOptionProperty); }
            set { SetValue(CultureInvariantOptionProperty, value); }
        }

        public static readonly DependencyProperty CultureInvariantOptionProperty = DependencyProperty.Register(nameof(CultureInvariantOption), typeof(bool),
            typeof(RegexBuilderViewModel), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                ((RegexBuilderViewModel)d).OnOptionPropertyChanged(RegexOptions.CultureInvariant, (bool)e.OldValue, (bool)e.NewValue)));

        public string PatternErrorMessage
        {
            get { return (string)GetValue(PatternErrorMessageProperty); }
            private set { SetValue(PatternErrorMessagePropertyKey, value); }
        }

        private static readonly DependencyPropertyKey PatternErrorMessagePropertyKey = DependencyProperty.RegisterReadOnly(nameof(PatternErrorMessage), typeof(string),
            typeof(RegexBuilderViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty PatternErrorMessageProperty = PatternErrorMessagePropertyKey.DependencyProperty;

        public ReadOnlyCollection<PatternGroupItem> PatternGroupItems
        {
            get { return (ReadOnlyCollection<PatternGroupItem>)GetValue(PatternGroupItemsProperty); }
            private set { SetValue(PatternGroupItemsPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey PatternGroupItemsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(PatternGroupItems),
            typeof(ReadOnlyCollection<PatternGroupItem>), typeof(RegexBuilderViewModel), new PropertyMetadata(null));

        public static readonly DependencyProperty PatternGroupItemsProperty = PatternGroupItemsPropertyKey.DependencyProperty;

        private Command.RelayCommand _copyDecodedTextCommand = null;

        /// <summary>
        /// Copies the current text selection in it's decoded form to the clipboard.
        /// </summary>
        public Command.RelayCommand CopyDecodedTextCommand
        {
            get
            {
                if (_copyDecodedTextCommand == null)
                    _copyDecodedTextCommand = new Command.RelayCommand(OnCopyDecodedText);
                return _copyDecodedTextCommand;
            }
        }

        protected virtual void OnCopyDecodedText(object parameter)
        {
            // TODO: Implement OnCopyDecodedText Logic
        }

        private Command.RelayCommand _copyAsIsCommand = null;

        /// <summary>
        /// Copies the current text selection to the clipboard in it's current encoded format.
        /// </summary>
        public Command.RelayCommand CopyAsIsCommand
        {
            get
            {
                if (_copyAsIsCommand == null)
                    _copyAsIsCommand = new Command.RelayCommand(OnCopyAsIs);
                return _copyAsIsCommand;
            }
        }

        protected virtual void OnCopyAsIs(object parameter)
        {
            // TODO: Implement OnCopyAsIs Logic
        }

        private Command.RelayCommand _copyCsStringLiteralCommand = null;

        /// <summary>
        /// Copies the current text selection as a backslash-escaped CS-compatible string literal, including surrounding quotation marks.
        /// </summary>
        public Command.RelayCommand CopyCsStringLiteralCommand
        {
            get
            {
                if (_copyCsStringLiteralCommand == null)
                    _copyCsStringLiteralCommand = new Command.RelayCommand(OnCopyCsStringLiteral);
                return _copyCsStringLiteralCommand;
            }
        }

        protected virtual void OnCopyCsStringLiteral(object parameter)
        {
            // TODO: Implement OnCopyCsStringLiteral Logic
        }

        private Command.RelayCommand _copyCsVerbatimLiteralCommand = null;

        /// <summary>
        /// Copies the current text selection as a CS-compatible verbatim string literal (<c>@&quot;&#x2026;&quot;</c>), including surrounding quotation marks.
        /// </summary>
        /// <remarks>New line character sequences <c>\r</c>, <c>\n</c> and <c>\r\n</c> will result in line breaks.
        /// <para>https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/verbatim</para></remarks>
        public Command.RelayCommand CopyCsVerbatimLiteralCommand
        {
            get
            {
                if (_copyCsVerbatimLiteralCommand == null)
                    _copyCsVerbatimLiteralCommand = new Command.RelayCommand(OnCopyCsVerbatimLiteral);
                return _copyCsVerbatimLiteralCommand;
            }
        }

        protected virtual void OnCopyCsVerbatimLiteral(object parameter)
        {
            // TODO: Implement OnCopyCsVerbatimLiteral Logic
        }

        private Command.RelayCommand _copyPsStringLiteralCommand = null;

        /// <summary>
        /// Copies the current text selection as a PowerShell-compatible string literal, including surrounding quotation marks.
        /// </summary>
        /// <remarks>The string literal will be single-quoted (<c>'</c>) unless a double-quoted (<c>&quot;</c>) string literal produces a shorter result.</remarks>
        public Command.RelayCommand CopyPsStringLiteralCommand
        {
            get
            {
                if (_copyPsStringLiteralCommand == null)
                    _copyPsStringLiteralCommand = new Command.RelayCommand(OnCopyPsStringLiteral);
                return _copyPsStringLiteralCommand;
            }
        }

        protected virtual void OnCopyPsStringLiteral(object parameter)
        {
            // TODO: Implement OnCopyPsStringLiteral Logic
        }

        private Command.RelayCommand _copyPsHereToLiteralCommand = null;

        /// <summary>
        /// Copies the current text selection as a PowerShell-compatible Here-to string literal (<c>@&quot;&lt;newline&gt;&#x2026;&lt;newline&gt;&quot;@</c>), including surrounding quotation marks.
        /// </summary>
        /// <remarks>New line character sequences <c>`r</c>, <c>`n</c> and <c>`r`n</c> will result in additional line breaks.</remarks>
        public Command.RelayCommand CopyPsHereToLiteralCommand
        {
            get
            {
                if (_copyPsHereToLiteralCommand == null)
                    _copyPsHereToLiteralCommand = new Command.RelayCommand(OnCopyPsHereToLiteral);
                return _copyPsHereToLiteralCommand;
            }
        }

        protected virtual void OnCopyPsHereToLiteral(object parameter)
        {
            // TODO: Implement OnCopyPsHereToLiteral Logic
        }

        private Command.RelayCommand _copyXmlEncodedCommand = null;

        /// <summary>
        /// Copies the current text selection as an XML-Encoded string suitable for the content of an XML element.
        /// </summary>
        public Command.RelayCommand CopyXmlEncodedCommand
        {
            get
            {
                if (_copyXmlEncodedCommand == null)
                    _copyXmlEncodedCommand = new Command.RelayCommand(OnCopyXmlEncoded);
                return _copyXmlEncodedCommand;
            }
        }

        protected virtual void OnCopyXmlEncoded(object parameter)
        {
            // TODO: Implement OnCopyXmlEncoded Logic
        }

        private Command.RelayCommand _copyAttributeEncodedCommand = null;

        /// <summary>
        /// Copies the current text selection as an XML Attribute-Encoded string suitable for the content of an XML attribute, NOT including surrounding quotation marks.
        /// </summary>
        public Command.RelayCommand CopyAttributeEncodedCommand
        {
            get
            {
                if (_copyAttributeEncodedCommand == null)
                    _copyAttributeEncodedCommand = new Command.RelayCommand(OnCopyAttributeEncoded);
                return _copyAttributeEncodedCommand;
            }
        }

        protected virtual void OnCopyAttributeEncoded(object parameter)
        {
            // TODO: Implement OnCopyAttributeEncoded Logic
        }

        private Command.RelayCommand _copyUriPathEncodedCommand = null;

        /// <summary>
        /// Copies the current text selection as a URI-Encoded string, suitable as a relative or absolute URI string.
        /// </summary>
        /// <remarks>This simply encodes any individual characters that are not valid for relative or absolute URI strings. It does not verify that the URI components are in proper order.</remarks>
        public Command.RelayCommand CopyUriPathEncodedCommand
        {
            get
            {
                if (_copyUriPathEncodedCommand == null)
                    _copyUriPathEncodedCommand = new Command.RelayCommand(OnCopyUriPathEncoded);
                return _copyUriPathEncodedCommand;
            }
        }

        protected virtual void OnCopyUriPathEncoded(object parameter)
        {
            // TODO: Implement OnCopyUriPathEncoded Logic
        }

        private Command.RelayCommand _copyDataEncodedCommand = null;

        /// <summary>
        /// Copies the current text selection as a URI-Encoded data string, compatible with path segments as well as query string keys and values.
        /// </summary>
        public Command.RelayCommand CopyDataEncodedCommand
        {
            get
            {
                if (_copyDataEncodedCommand == null)
                    _copyDataEncodedCommand = new Command.RelayCommand(OnCopyDataEncoded);
                return _copyDataEncodedCommand;
            }
        }

        protected virtual void OnCopyDataEncoded(object parameter)
        {
            // TODO: Implement OnCopyDataEncoded Logic
        }

        private Command.RelayCommand _pasteAutoDetectCommand = null;

        /// <summary>
        /// Attemps to detect the format of the text in the clipbord before decoding and pasting.
        /// </summary>
        public Command.RelayCommand PasteAutoDetectCommand
        {
            get
            {
                if (_pasteAutoDetectCommand == null)
                    _pasteAutoDetectCommand = new Command.RelayCommand(OnPasteAutoDetect);
                return _pasteAutoDetectCommand;
            }
        }

        protected virtual void OnPasteAutoDetect(object parameter)
        {
            // TODO: Implement OnPasteAutoDetect Logic
        }

        private Command.RelayCommand _pasteCurrentFormatCommand = null;

        /// <summary>
        /// Pastes contents of the clipboard into the current text selection and assumes the clipboard is uses the same format.
        /// </summary>
        public Command.RelayCommand PasteCurrentFormatCommand
        {
            get
            {
                if (_pasteCurrentFormatCommand == null)
                    _pasteCurrentFormatCommand = new Command.RelayCommand(OnPasteCurrentFormat);
                return _pasteCurrentFormatCommand;
            }
        }

        protected virtual void OnPasteCurrentFormat(object parameter)
        {
            // TODO: Implement OnPasteCurrentFormat Logic
        }

        private Command.RelayCommand _pasteAsIsCommand = null;

        /// <summary>
        /// Directly pastes contents of the clipboard with no decoding.
        /// </summary>
        public Command.RelayCommand PasteAsIsCommand
        {
            get
            {
                if (_pasteAsIsCommand == null)
                    _pasteAsIsCommand = new Command.RelayCommand(OnPasteAsIs);
                return _pasteAsIsCommand;
            }
        }

        protected virtual void OnPasteAsIs(object parameter)
        {
            // TODO: Implement OnPasteAsIs Logic
        }

        private Command.RelayCommand _pasteBackslashEscapedCommand = null;

        /// <summary>
        /// Pastes contents of the clipboard into the current text selection, decoding any valid CS-style escape sequences.
        /// </summary>
        /// <remarks>This assumes the clipboard contains the CONTENTS of a string literal, and not the actual string literal itself.</remarks>
        public Command.RelayCommand PasteBackslashEscapedCommand
        {
            get
            {
                if (_pasteBackslashEscapedCommand == null)
                    _pasteBackslashEscapedCommand = new Command.RelayCommand(OnPasteBackslashEscaped);
                return _pasteBackslashEscapedCommand;
            }
        }

        protected virtual void OnPasteBackslashEscaped(object parameter)
        {
            // TODO: Implement OnPasteBackslashEscaped Logic
        }

        private Command.RelayCommand _pasteBacktickEscapedCommand = null;

        /// <summary>
        /// Pastes contents of the clipboard into the current text selection, decoding any valid PowerShell-style escape sequences.
        /// </summary>
        /// <remarks>This assumes the clipboard contains the CONTENTS of a string literal, and not the actual string literal itself.
        /// <para>This supports the unicode escape sequences (<c>`u{xxxx}</c>) as well. Any invalid unicode sequences will result in an error.</para></remarks>
        public Command.RelayCommand PasteBacktickEscapedCommand
        {
            get
            {
                if (_pasteBacktickEscapedCommand == null)
                    _pasteBacktickEscapedCommand = new Command.RelayCommand(OnPasteBacktickEscaped);
                return _pasteBacktickEscapedCommand;
            }
        }

        protected virtual void OnPasteBacktickEscaped(object parameter)
        {
            // TODO: Implement OnPasteBacktickEscaped Logic
        }

        private Command.RelayCommand _pasteXmlEncodedCommand = null;

        /// <summary>
        /// Pastes the contents of the clipboard into the currently selected text, decoding any valid XML entities.
        /// </summary>
        public Command.RelayCommand PasteXmlEncodedCommand
        {
            get
            {
                if (_pasteXmlEncodedCommand == null)
                    _pasteXmlEncodedCommand = new Command.RelayCommand(OnPasteXmlEncoded);
                return _pasteXmlEncodedCommand;
            }
        }

        protected virtual void OnPasteXmlEncoded(object parameter)
        {
            // TODO: Implement OnPasteXmlEncoded Logic
        }

        private Command.RelayCommand _pasteUriEncodedCommand = null;

        /// <summary>
        /// Pastes the contents of the clipboard into the currently selected text, decoding any valid URI escape sequences.
        /// </summary>
        public Command.RelayCommand PasteUriEncodedCommand
        {
            get
            {
                if (_pasteUriEncodedCommand == null)
                    _pasteUriEncodedCommand = new Command.RelayCommand(OnPasteUriEncoded);
                return _pasteUriEncodedCommand;
            }
        }

        protected virtual void OnPasteUriEncoded(object parameter)
        {
            // TODO: Implement OnPasteUriEncoded Logic
        }

        private Command.RelayCommand _regexGroupCommand = null;

        /// <summary>
        /// Creates a grouping from the current text selection <c>(&#x2026;)</c>.
        /// </summary>
        public Command.RelayCommand RegexGroupCommand
        {
            get
            {
                if (_regexGroupCommand == null)
                    _regexGroupCommand = new Command.RelayCommand(() =>
                    {
                        MessageBox.Show("RegexGroupCommand not implemented");
                    });
                return _regexGroupCommand;
            }
        }

        private Command.RelayCommand _regexEscapeCommand = null;

        /// <summary>
        /// Escapes special regular expression characters in the current text selection.
        /// </summary>
        public Command.RelayCommand RegexEscapeCommand
        {
            get
            {
                if (_regexEscapeCommand == null)
                    _regexEscapeCommand = new Command.RelayCommand(() =>
                    {
                        MessageBox.Show("RegexEscapeCommand not implemented");
                    });
                return _regexEscapeCommand;
            }
        }

        public InputTextItem SelectedInputItem
        {
            get { return (InputTextItem)GetValue(SelectedInputItemProperty); }
            set { SetValue(SelectedInputItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedInputItemProperty = DependencyProperty.Register(nameof(SelectedInputItem), typeof(InputTextItem),
            typeof(RegexBuilderViewModel), new PropertyMetadata(null));

        public static readonly DependencyProperty InputTextFormatProperty = DependencyProperty.Register(nameof(InputTextFormat), typeof(TextFormat),
            typeof(RegexBuilderViewModel), new PropertyMetadata(TextFormat.Normal, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as RegexBuilderViewModel).OnInputTextFormatPropertyChanged((TextFormat)e.OldValue, (TextFormat)e.NewValue)));

        public TextFormat InputTextFormat
        {
            get { return (TextFormat)GetValue(InputTextFormatProperty); }
            set { this.SetValue(InputTextFormatProperty, value); }
        }

        protected virtual void OnInputTextFormatPropertyChanged(TextFormat oldValue, TextFormat newValue)
        {
            // TODO: Implement OnInputTextFormatPropertyChanged Logic
        }

        public static readonly DependencyProperty ResultTextFormatProperty = DependencyProperty.Register(nameof(ResultTextFormat), typeof(TextFormat),
            typeof(RegexBuilderViewModel), new PropertyMetadata(TextFormat.Normal, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as RegexBuilderViewModel).OnResultTextFormatPropertyChanged((TextFormat)e.OldValue, (TextFormat)e.NewValue)));

        public TextFormat ResultTextFormat
        {
            get { return (TextFormat)GetValue(ResultTextFormatProperty); }
            set { this.SetValue(ResultTextFormatProperty, value); }
        }

        protected virtual void OnResultTextFormatPropertyChanged(TextFormat oldValue, TextFormat newValue)
        {
            // TODO: Implement OnResultTextFormatPropertyChanged Logic
        }

        public RegexBuilderViewModel()
        {
            InputTextItems = new ReadOnlyCollection<InputTextItem>(_inputTextItems);
            InputTextItem item = new InputTextItem();
            item.Delete += Item_Delete;
            item.DeleteCommand.IsEnabled = false;
            _inputTextItems.Add(item);
            SelectedInputItem = item;
        }

        private void Item_Delete(object sender, EventArgs e)
        {
            InputTextItem item = (InputTextItem)sender;
            int index = _inputTextItems.IndexOf(item);
            if (_inputTextItems.Remove(item))
                item.Delete -= Item_Delete;
            if (_inputTextItems.Count == 1)
                _inputTextItems[0].DeleteCommand.IsEnabled = false;
        }

        private void OnOptionPropertyChanged(RegexOptions value, bool? wasSet, bool? isSet)
        {
            if (_ignoreOptionChange || !isSet.HasValue || (wasSet.HasValue && isSet.Value == wasSet.Value))
                return;
            _ignoreOptionChange = true;
            try
            {
                if (value == RegexOptions.None)
                    SelectedOptions = RegexOptions.None;
                else if (isSet.Value)
                    SelectedOptions |= value;
                else
                    SelectedOptions ^= value;
            }
            finally { _ignoreOptionChange = false; }
        }

        private void OnSelectedOptionsPropertyChanged(RegexOptions oldValue, RegexOptions newValue)
        {
            if (oldValue == newValue)
                return;
            bool ignoreOptionChange = _ignoreOptionChange;
            _ignoreOptionChange = true;
            try
            {
                if (newValue == RegexOptions.None)
                {
                    NoneOption = true;
                    NoneEnabled = false;
                    IgnoreCaseOption = MultilineOption = CompiledOption = ExplicitCaptureOption = SinglelineOption = IgnorePatternWhitespaceOption = ECMAScriptOption = CultureInvariantOption = false;
                    IgnoreCaseThreeState = MultilineThreeState = CompiledThreeState = false;
                    EffectiveOptions = newValue;
                }
                else
                {
                    NoneEnabled = true;
                    NoneOption = false;
                    ExplicitCaptureOption = newValue.HasFlag(RegexOptions.ExplicitCapture);
                    SinglelineOption = newValue.HasFlag(RegexOptions.Singleline);
                    IgnorePatternWhitespaceOption = newValue.HasFlag(RegexOptions.IgnorePatternWhitespace);
                    CultureInvariantOption = newValue.HasFlag(RegexOptions.CultureInvariant);
                    if (oldValue.HasFlag(RegexOptions.ECMAScript))
                    {
                        if (newValue.HasFlag(RegexOptions.ECMAScript))
                        {
                            bool f = newValue.HasFlag(RegexOptions.IgnoreCase);
                            if (f != oldValue.HasFlag(RegexOptions.IgnoreCase))
                            {
                                if (f)
                                {
                                    IgnoreCaseOption = true;
                                    IgnoreCaseThreeState = false;
                                }
                                else
                                {
                                    SelectedOptions = newValue ^ RegexOptions.ECMAScript;
                                    return;
                                }
                            }
                            f = newValue.HasFlag(RegexOptions.Multiline);
                            if (f != oldValue.HasFlag(RegexOptions.Multiline))
                            {
                                if (f)
                                {
                                    MultilineOption = true;
                                    MultilineThreeState = false;
                                }
                                else
                                {
                                    SelectedOptions = newValue ^ RegexOptions.ECMAScript;
                                    return;
                                }
                            }
                            f = newValue.HasFlag(RegexOptions.Compiled);
                            if (f != oldValue.HasFlag(RegexOptions.Compiled))
                            {
                                if (f)
                                {
                                    CompiledOption = true;
                                    CompiledThreeState = false;
                                }
                                else
                                {
                                    SelectedOptions = newValue ^ RegexOptions.ECMAScript;
                                    return;
                                }
                            }
                            EffectiveOptions = newValue | RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled;
                            return;
                        }
                        else
                        {
                            IgnoreCaseOption = newValue.HasFlag(RegexOptions.IgnoreCase);
                            MultilineOption = newValue.HasFlag(RegexOptions.Multiline);
                            CompiledOption = newValue.HasFlag(RegexOptions.Compiled);
                            IgnoreCaseThreeState = MultilineThreeState = CompiledThreeState = ECMAScriptOption = false;
                            EffectiveOptions = newValue;
                        }
                    }
                    else if (newValue.HasFlag(RegexOptions.ECMAScript))
                    {
                        ECMAScriptOption = true;
                        if (!newValue.HasFlag(RegexOptions.IgnoreCase))
                        {
                            IgnoreCaseThreeState = true;
                            IgnoreCaseOption = null;
                        }
                        if (!newValue.HasFlag(RegexOptions.Multiline))
                        {
                            MultilineThreeState = true;
                            MultilineOption = null;
                        }
                        if (!newValue.HasFlag(RegexOptions.Compiled))
                        {
                            CompiledThreeState = true;
                            CompiledOption = null;
                        }
                        EffectiveOptions = newValue | RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled;
                    }
                    else
                    {
                        IgnoreCaseOption = newValue.HasFlag(RegexOptions.IgnoreCase);
                        MultilineOption = newValue.HasFlag(RegexOptions.Multiline);
                        CompiledOption = newValue.HasFlag(RegexOptions.Compiled);
                        EffectiveOptions = newValue;
                    }
                }
            }
            finally { _ignoreOptionChange = ignoreOptionChange; }

        }

        private void OnEffectiveOptionsPropertyChanged(RegexOptions oldValue, RegexOptions newValue)
        {
            if (oldValue != newValue)
                StartParsePattern(PatternText, newValue);
        }

        private void OnPatternTextPropertyChanged(string oldValue, string newValue)
        {
            if (oldValue != newValue)
                StartParsePattern(newValue, EffectiveOptions);
        }

        private CancellationTokenSource _parseTokenSource;
        private Regex _parsedRegex;
        private CancellationTokenSource _evaluateTokenSource;

        private void StartParsePattern(string patternText, RegexOptions options)
        {
            CancellationTokenSource tokenSource = _parseTokenSource;
            _parseTokenSource = new CancellationTokenSource();
            if (!(tokenSource is null))
            {
                if (!tokenSource.IsCancellationRequested)
                    tokenSource.Cancel();
                tokenSource.Dispose();
            }

            CancellationToken token = _parseTokenSource.Token;
            Task.Factory.StartNew(ParsePattern, new object[] { patternText, options, token }, token).ContinueWith(OnParseComplete);
        }

        private void StartEvaluation(IEnumerable<InputTextItem> items, Regex regex)
        {
            CancellationTokenSource tokenSource = _evaluateTokenSource;
            _evaluateTokenSource = new CancellationTokenSource();
            if (!(tokenSource is null))
            {
                if (!tokenSource.IsCancellationRequested)
                    tokenSource.Cancel();
                tokenSource.Dispose();
            }

            CancellationToken token = _evaluateTokenSource.Token;
            Task.Factory.StartNew(EvaluateRegex, new object[] { items.Select(i => new Tuple<string, InputTextItem>(i.RawValue, i)).ToArray(), regex, token }, token)
                .ContinueWith(OnEvaluationComplete);
        }

        private void OnParseComplete(Task<Regex> task)
        {
            if (task.IsCanceled)
                return;
            if (task.IsFaulted)
                Dispatcher.Invoke(() =>
                {
                    _parseTokenSource.Dispose();
                    _parseTokenSource = null;
                    _parsedRegex = null;
                    _patternGroupItems.Clear();
                    PatternErrorMessage = string.IsNullOrWhiteSpace(task.Exception.Message) ? task.Exception.ToString() : task.Exception.Message;
                });
            else
                Dispatcher.Invoke(() =>
                {
                    PatternErrorMessage = "";
                    _parseTokenSource.Dispose();
                    _parseTokenSource = null;
                    _patternGroupItems.Clear();
                    _parsedRegex = task.Result;
                    foreach (PatternGroupItem item in PatternGroupItem.Create(_parsedRegex))
                        _patternGroupItems.Add(item);
                    StartEvaluation(_inputTextItems, _parsedRegex);
                });
        }

        private static Regex ParsePattern(object args)
        {
            object[] arr = (object[])args;
            if (((CancellationToken)arr[2]).IsCancellationRequested)
                return null;
            return new Regex((string)arr[0], (RegexOptions)arr[1], TimeSpan.FromMilliseconds(500));
        }

        private void OnEvaluationComplete(Task<Tuple<BinaryAlternate<Match, Exception>, InputTextItem>[]> task)
        {
            if (task.IsCanceled)
                return;
            if (task.IsFaulted)
                Dispatcher.Invoke(() =>
                {
                    _evaluateTokenSource.Dispose();
                    _evaluateTokenSource = null;
                    PatternErrorMessage = string.IsNullOrWhiteSpace(task.Exception.Message) ? task.Exception.ToString() : task.Exception.Message;
                });
            else
                Dispatcher.Invoke(() =>
                {
                    PatternErrorMessage = "";
                    _evaluateTokenSource.Dispose();
                    _evaluateTokenSource = null;
                    foreach (PatternGroupItem g in _patternGroupItems)
                        g.MatchCount = 0;
                    foreach (Tuple<BinaryAlternate<Match, Exception>, InputTextItem>item in task.Result)
                    {
                        item.Item2.IsMatch = item.Item1.Flatten(m =>
                        {
                            if (m.Success)
                            {
                                foreach (PatternGroupItem g in _patternGroupItems)
                                    if (m.Groups[g.Number].Success)
                                        g.MatchCount++;
                                return true;
                            }
                            return false;
                        }, e => false);
                    }
                });
        }

        private static Tuple<BinaryAlternate<Match, Exception>, InputTextItem>[] EvaluateRegex(object args)
        {
            object[] arr = (object[])args;
            if (((CancellationToken)arr[2]).IsCancellationRequested)
                return null;
            Regex regex = (Regex)arr[2];
            return ((Tuple<string, InputTextItem>[])arr[1]).Select(item =>
                new Tuple<BinaryAlternate<Match, Exception>, InputTextItem>(BinaryAlternate.TryInvoke(() => regex.Match(item.Item1)), item.Item2)).ToArray();
        }
    }
}
