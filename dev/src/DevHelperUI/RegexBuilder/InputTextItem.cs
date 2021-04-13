using FsInfoCat.Util;
using System;
using System.Windows;

namespace DevHelperUI.RegexBuilder
{
    public class InputTextItem : DependencyObject
    {
        public event DependencyPropertyChangedEventHandler RawValuePropertyChanged;

        public event DependencyPropertyChangedEventHandler EscapedValuePropertyChanged;

        public event EventHandler Delete;

        private bool _ignoreChange = false;

        public static readonly DependencyProperty RawValueProperty = DependencyProperty.Register(nameof(RawValue), typeof(string), typeof(InputTextItem),
            new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            {
                try { (d as InputTextItem).OnRawValuePropertyChanged(e.OldValue as string, e.NewValue as string); }
                finally { (d as InputTextItem).RawValuePropertyChanged?.Invoke(e, e); }
            }));

        public string RawValue
        {
            get { return GetValue(RawValueProperty) as string; }
            set { SetValue(RawValueProperty, value); }
        }

        public static readonly DependencyProperty EscapedValueProperty = DependencyProperty.Register(nameof(EscapedValue), typeof(string), typeof(InputTextItem),
            new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            {
                try { (d as InputTextItem).OnEscapedValuePropertyChanged(e.OldValue as string, e.NewValue as string); }
                finally { (d as InputTextItem).EscapedValuePropertyChanged?.Invoke(e, e); }
            }));

        public string EscapedValue
        {
            get { return GetValue(EscapedValueProperty) as string; }
            set { SetValue(EscapedValueProperty, value); }
        }

        public static readonly DependencyProperty FormatProperty = DependencyProperty.Register(nameof(Format), typeof(TextFormat), typeof(InputTextItem),
            new PropertyMetadata(TextFormat.Normal, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as InputTextItem).OnFormatPropertyChanged((TextFormat)e.OldValue, (TextFormat)e.NewValue)));

        public TextFormat Format
        {
            get { return (TextFormat)GetValue(FormatProperty); }
            set { this.SetValue(FormatProperty, value); }
        }

        public bool IsMatch
        {
            get { return (bool)GetValue(IsMatchProperty); }
            set { SetValue(IsMatchProperty, value); }
        }

        public static readonly DependencyProperty IsMatchProperty = DependencyProperty.Register(nameof(IsMatch), typeof(bool), typeof(InputTextItem),
            new PropertyMetadata(false));

        private Command.RelayCommand _deleteCommand = null;

        public Command.RelayCommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                    _deleteCommand = new Command.RelayCommand(() => Delete?.Invoke(this, EventArgs.Empty), false, true);

                return _deleteCommand;
            }
        }

        protected virtual void OnRawValuePropertyChanged(string oldValue, string newValue)
        {
            if (_ignoreChange || oldValue == newValue)
                return;
            _ignoreChange = true;
            try
            {
                switch (Format)
                {
                    case TextFormat.BackslashEscaped:
                        EscapedValue = StringEscapingHelper.EscapeCsString(newValue, true);
                        break;
                    case TextFormat.BacktickEscaped:
                        EscapedValue = StringEscapingHelper.EscapePsStringDoubleQuote(newValue, true);
                        break;
                    case TextFormat.XmlEncoded:
                        
                        break;
                    case TextFormat.UriEncoded:
                        break;
                    default:
                        EscapedValue = newValue;
                        break;
                }
                // TODO: Implement OnRawValuePropertyChanged Logic
            }
            finally { _ignoreChange = false; }
        }

        protected virtual void OnEscapedValuePropertyChanged(string oldValue, string newValue)
        {
            if (_ignoreChange || oldValue == newValue)
                return;
            _ignoreChange = true;
            try
            {
                // TODO: Implement OnEscapedValuePropertyChanged Logic
            }
            finally { _ignoreChange = false; }
        }

        protected virtual void OnFormatPropertyChanged(TextFormat oldValue, TextFormat newValue)
        {
            if (oldValue == newValue)
                return;
            bool ignoreChange = _ignoreChange;
            _ignoreChange = true;
            try
            {
                // TODO: Implement OnFormatPropertyChanged Logic
            }
            finally { _ignoreChange = ignoreChange; }
        }
    }
}
