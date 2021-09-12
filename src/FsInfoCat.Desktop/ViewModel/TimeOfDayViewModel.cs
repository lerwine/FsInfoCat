using System;
using System.Collections.Immutable;
using System.Linq;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class TimeOfDayViewModel : OptionalValueViewModel<TimeSpan>
    {
        private TimeSpan? _pendingResultValueChange;

        #region Hours24 Property Members

        private int? _pendingHours24Change;

        private static readonly DependencyPropertyKey Hours24PropertyKey = DependencyPropertyBuilder<TimeOfDayViewModel, OptionalValueViewModel<int>>
            .Register(nameof(Hours24))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Hours24"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty Hours24Property = Hours24PropertyKey.DependencyProperty;

        public OptionalValueViewModel<int> Hours24 => (OptionalValueViewModel<int>)GetValue(Hours24Property);

        #endregion
        #region Hours12 Property Members

        private int? _pendingHours12Change;

        private static readonly DependencyPropertyKey Hours12PropertyKey = DependencyPropertyBuilder<TimeOfDayViewModel, OptionalValueViewModel<int>>
            .Register(nameof(Hours12))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Hours12"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty Hours12Property = Hours12PropertyKey.DependencyProperty;

        public OptionalValueViewModel<int> Hours12 { get => (OptionalValueViewModel<int>)GetValue(Hours12Property); private set => SetValue(Hours12PropertyKey, value); }

        #endregion
        #region IsPm Property Members

        private bool _pendingIsPmChange;

        /// <summary>
        /// Identifies the <see cref="IsPm"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsPmProperty = DependencyPropertyBuilder<TimeOfDayViewModel, bool>
            .Register(nameof(IsPm))
            .DefaultValue(false)
            .OnChanged((d, oldValue, newValue) => (d as TimeOfDayViewModel)?.OnIsPmPropertyChanged(newValue))
            .AsReadWrite();

        public bool IsPm { get => (bool)GetValue(IsPmProperty); set => SetValue(IsPmProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="IsPm"/> dependency property has changed.
        /// </summary>
        /// <param name="newValue">The new value of the <see cref="IsPm"/> property.</param>
        protected virtual void OnIsPmPropertyChanged(bool newValue)
        {
            // TODO: Implement OnIsPmPropertyChanged Logic
        }

        #endregion
        #region Minutes Property Members

        private int? _pendingMinutesChange;

        private static readonly DependencyPropertyKey MinutesPropertyKey = DependencyPropertyBuilder<TimeOfDayViewModel, OptionalValueViewModel<int>>
            .Register(nameof(Minutes))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Minutes"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinutesProperty = MinutesPropertyKey.DependencyProperty;

        public OptionalValueViewModel<int> Minutes { get => (OptionalValueViewModel<int>)GetValue(MinutesProperty); private set => SetValue(MinutesPropertyKey, value); }

        #endregion
        #region Seconds Property Members

        private int? _pendingSecondsChange;

        private static readonly DependencyPropertyKey SecondsPropertyKey = DependencyPropertyBuilder<TimeOfDayViewModel, OptionalValueViewModel<int>>
            .Register(nameof(Seconds))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Seconds"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SecondsProperty = SecondsPropertyKey.DependencyProperty;

        public OptionalValueViewModel<int> Seconds { get => (OptionalValueViewModel<int>)GetValue(SecondsProperty); private set => SetValue(SecondsPropertyKey, value); }

        #endregion
        #region Milliseconds Property Members

        private int? _pendingMillisecondsChange;

        private static readonly DependencyPropertyKey MillisecondsPropertyKey = DependencyPropertyBuilder<TimeOfDayViewModel, OptionalValueViewModel<int>>
            .Register(nameof(Milliseconds))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Milliseconds"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MillisecondsProperty = MillisecondsPropertyKey.DependencyProperty;

        public OptionalValueViewModel<int> Milliseconds { get => (OptionalValueViewModel<int>)GetValue(MillisecondsProperty); private set => SetValue(MillisecondsPropertyKey, value); }

        #endregion
        #region HasComponentValueErrors Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="HasComponentValueErrors"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler HasComponentValueErrorsPropertyChanged;

        private static readonly DependencyPropertyKey HasComponentValueErrorsPropertyKey = DependencyPropertyBuilder<TimeOfDayViewModel, bool>
            .Register(nameof(HasComponentValueErrors))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as TimeOfDayViewModel)?.RaiseHasComponentValueErrorsPropertyChanged(e))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="HasComponentValueErrors"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HasComponentValueErrorsProperty = HasComponentValueErrorsPropertyKey.DependencyProperty;

        public bool HasComponentValueErrors { get => (bool)GetValue(HasComponentValueErrorsProperty); private set => SetValue(HasComponentValueErrorsPropertyKey, value); }

        /// <summary>
        /// Called when the <see cref="PropertyChangedCallback">PropertyChanged</see> event on <see cref="HasComponentValueErrorsProperty"/> is raised.
        /// </summary>
        /// <param name="args">The Event data that is issued by the event on <see cref="HasComponentValueErrorsProperty"/> that tracks changes to its effective value.</param>
        protected void RaiseHasComponentValueErrorsPropertyChanged(DependencyPropertyChangedEventArgs args) => HasComponentValueErrorsPropertyChanged?.Invoke(this, args);

        #endregion

        public TimeOfDayViewModel()
        {
            OptionalValueViewModel<int> hours24 = new(), hours12 = new(), minutes = new(), seconds = new(), milliseconds = new();
            SetValue(Hours24PropertyKey, hours24);
            SetValue(Hours12PropertyKey, hours12);
            SetValue(MinutesPropertyKey, minutes);
            SetValue(SecondsPropertyKey, seconds);
            SetValue(MillisecondsPropertyKey, milliseconds);
            hours24.ValidateInputValue += Hours24_ValidateInputValue;
            hours12.ValidateInputValue += Hours12_ValidateInputValue;
            minutes.ValidateInputValue += Minutes_ValidateInputValue;
            seconds.ValidateInputValue += Seconds_ValidateInputValue;
            milliseconds.ValidateInputValue += Milliseconds_ValidateInputValue;
            hours24.ResultValuePropertyChanged += Hours24_ResultValuePropertyChanged;
            hours12.ResultValuePropertyChanged += Hours12_ResultValuePropertyChanged;
            minutes.ResultValuePropertyChanged += Minutes_ResultValuePropertyChanged;
            seconds.ResultValuePropertyChanged += Seconds_ResultValuePropertyChanged;
            milliseconds.ResultValuePropertyChanged += Milliseconds_ResultValuePropertyChanged;
            hours24.HasErrorsPropertyChanged += Hours24_HasErrorsPropertyChanged;
            hours12.HasErrorsPropertyChanged += Hours12_HasErrorsPropertyChanged;
            minutes.HasErrorsPropertyChanged += Minutes_HasErrorsPropertyChanged;
            seconds.HasErrorsPropertyChanged += Seconds_HasErrorsPropertyChanged;
            milliseconds.HasErrorsPropertyChanged += Milliseconds_HasErrorsPropertyChanged;
        }

        protected override void OnInputValuePropertyChanged(TimeSpan? newValue)
        {
            if (newValue.HasValue)
            {
                if (newValue.Value.Milliseconds != 0 && !Milliseconds.IsEnabled)
                    base.OnInputValuePropertyChanged(new(newValue.Value.Days, (Hours12.IsEnabled || Hours24.IsEnabled) ? newValue.Value.Hours : 0, Minutes.IsEnabled ? newValue.Value.Minutes : 0, Seconds.IsEnabled ? newValue.Value.Seconds : 0, 0));
                else if (newValue.Value.Seconds != 0 && !Seconds.IsEnabled)
                    base.OnInputValuePropertyChanged(new(newValue.Value.Days, (Hours12.IsEnabled || Hours24.IsEnabled) ? newValue.Value.Hours : 0, Minutes.IsEnabled ? newValue.Value.Minutes : 0, 0, newValue.Value.Milliseconds));
                else if (newValue.Value.Minutes != 0 && !Minutes.IsEnabled)
                    base.OnInputValuePropertyChanged(new(newValue.Value.Days, (Hours12.IsEnabled || Hours24.IsEnabled) ? newValue.Value.Hours : 0, newValue.Value.Seconds, newValue.Value.Milliseconds));
                else if (newValue.Value.Hours != 0 && !(Hours24.IsEnabled || Hours12.IsEnabled))
                    base.OnInputValuePropertyChanged(new(newValue.Value.Days, 0, newValue.Value.Minutes, newValue.Value.Seconds, newValue.Value.Milliseconds));
                else
                    base.OnInputValuePropertyChanged(newValue);
            }
            else
                base.OnInputValuePropertyChanged(newValue);
        }

        protected override void OnValidateInputValue(PropertyValidatingEventArgs<TimeSpan> args)
        {
            TimeSpan value = args.Value;
            base.OnValidateInputValue(args);
            if (string.IsNullOrWhiteSpace(args.ValidationMessage) && (args.Value < TimeSpan.Zero || args.Value.Days != 0))
                args.ValidationMessage = FsInfoCat.Properties.Resources.ErrorMessage_InvalidTime;
        }

        private void Hours24_ValidateInputValue(object sender, PropertyValidatingEventArgs<int> e)
        {
            if (e.Value < 0 || e.Value > 23)
                ErrorInfo.SetError(FsInfoCat.Properties.Resources.ErrorMessage_InvalidHours, Hours24Property.Name);
            else
                ErrorInfo.ClearErrors(Hours24Property.Name);
        }

        private void Hours12_ValidateInputValue(object sender, PropertyValidatingEventArgs<int> e)
        {
            if (e.Value < 1 || e.Value > 12)
                ErrorInfo.SetError(FsInfoCat.Properties.Resources.ErrorMessage_InvalidHours, Hours12Property.Name);
            else
                ErrorInfo.ClearErrors(Hours12Property.Name);
        }

        private void Minutes_ValidateInputValue(object sender, PropertyValidatingEventArgs<int> e)
        {
            if (e.Value < 0 || e.Value > 59)
                ErrorInfo.SetError(FsInfoCat.Properties.Resources.ErrorMessage_InvalidMinutes, MinutesProperty.Name);
            else
                ErrorInfo.ClearErrors(MinutesProperty.Name);
        }

        private void Seconds_ValidateInputValue(object sender, PropertyValidatingEventArgs<int> e)
        {
            if (e.Value < 0 || e.Value > 59)
                ErrorInfo.SetError(FsInfoCat.Properties.Resources.ErrorMessage_InvalidSeconds, SecondsProperty.Name);
            else
                ErrorInfo.ClearErrors(SecondsProperty.Name);
        }

        private void Milliseconds_ValidateInputValue(object sender, PropertyValidatingEventArgs<int> e)
        {
            if (e.Value < 0 || e.Value > 59)
                ErrorInfo.SetError(FsInfoCat.Properties.Resources.ErrorMessage_InvalidMilliseconds, MillisecondsProperty.Name);
            else
                ErrorInfo.ClearErrors(MillisecondsProperty.Name);
        }

        protected override void OnResultValuePropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            base.OnResultValuePropertyChanged(args);
            TimeSpan? newValue = args.NewValue as TimeSpan?;
            if (newValue == _pendingResultValueChange)
                return;
            _pendingResultValueChange = newValue;
            bool isPm = _pendingIsPmChange = IsPm;
            if (newValue.HasValue)
            {
                _pendingHours24Change = newValue.Value.Hours;
                int h = Math.Abs(newValue.Value.Hours);
                switch (h)
                {
                    case 0:
                        _pendingHours12Change = (newValue.Value.Hours < 0) ? -12 : 12;
                        _pendingIsPmChange = false;
                        break;
                    case 12:
                        _pendingHours12Change = 12;
                        _pendingIsPmChange = true;
                        break;
                    default:
                        if (h > 12)
                        {
                            _pendingHours12Change = (newValue.Value.Hours < 0) ? newValue.Value.Hours + 12 : newValue.Value.Hours - 12;
                            _pendingIsPmChange = true;
                        }
                        else
                        {
                            _pendingHours12Change = newValue.Value.Hours;
                            _pendingIsPmChange = false;
                        }
                        break;
                }
                _pendingMinutesChange = (Minutes.ResultValue.HasValue || newValue.Value.Minutes > 0 || newValue.Value.Seconds > 0 || newValue.Value.Milliseconds > 0) ? newValue.Value.Minutes : null;
                _pendingSecondsChange = (Minutes.ResultValue.HasValue || newValue.Value.Seconds > 0 || newValue.Value.Milliseconds > 0) ? newValue.Value.Seconds : null;
                _pendingMillisecondsChange = (Minutes.ResultValue.HasValue || newValue.Value.Milliseconds > 0) ? newValue.Value.Milliseconds : null;
                ImmutableArray<int?> values = ImmutableArray.Create(_pendingHours24Change, _pendingHours12Change, _pendingMinutesChange, _pendingSecondsChange, _pendingMillisecondsChange);
                Hours24.InputValue = _pendingHours24Change;
                if (isPm == _pendingIsPmChange && values.SequenceEqual(ImmutableArray.Create(_pendingHours24Change, _pendingHours12Change, _pendingMinutesChange, _pendingSecondsChange, _pendingMillisecondsChange)))
                {
                    Hours12.InputValue = _pendingHours12Change;
                    if (isPm == _pendingIsPmChange && values.SequenceEqual(ImmutableArray.Create(_pendingHours24Change, _pendingHours12Change, _pendingMinutesChange, _pendingSecondsChange, _pendingMillisecondsChange)))
                    {
                        IsPm = _pendingIsPmChange;
                        if (isPm == _pendingIsPmChange && values.SequenceEqual(ImmutableArray.Create(_pendingHours24Change, _pendingHours12Change, _pendingMinutesChange, _pendingSecondsChange, _pendingMillisecondsChange)))
                        {
                            Minutes.InputValue = _pendingMinutesChange;
                            if (isPm == _pendingIsPmChange && values.SequenceEqual(ImmutableArray.Create(_pendingHours24Change, _pendingHours12Change, _pendingMinutesChange, _pendingSecondsChange, _pendingMillisecondsChange)))
                            {
                                Seconds.InputValue = _pendingSecondsChange;
                                if (isPm == _pendingIsPmChange && values.SequenceEqual(ImmutableArray.Create(_pendingHours24Change, _pendingHours12Change, _pendingMinutesChange, _pendingSecondsChange, _pendingMillisecondsChange)))
                                    Milliseconds.InputValue = _pendingMillisecondsChange;
                            }
                        }
                    }
                }
            }
            else
            {
                _pendingHours24Change = _pendingHours12Change = _pendingMinutesChange = _pendingSecondsChange = _pendingMillisecondsChange = null;
                Hours24.InputValue = null;
                if (!(_pendingHours24Change.HasValue || _pendingHours12Change.HasValue || _pendingMinutesChange.HasValue || _pendingSecondsChange.HasValue || _pendingMillisecondsChange.HasValue))
                {
                    Hours12.InputValue = null;
                    if (!(_pendingHours24Change.HasValue || _pendingHours12Change.HasValue || _pendingMinutesChange.HasValue || _pendingSecondsChange.HasValue || _pendingMillisecondsChange.HasValue))
                    {
                        Minutes.InputValue = null;
                        if (!(_pendingHours24Change.HasValue || _pendingHours12Change.HasValue || _pendingMinutesChange.HasValue || _pendingSecondsChange.HasValue || _pendingMillisecondsChange.HasValue))
                        {
                            Seconds.InputValue = null;
                            if (!(_pendingHours24Change.HasValue || _pendingHours12Change.HasValue || _pendingMinutesChange.HasValue || _pendingSecondsChange.HasValue || _pendingMillisecondsChange.HasValue))
                                Milliseconds.InputValue = null;
                        }
                    }
                }
            }
        }

        private void OnComponentValueChanged(int? hours24, int? minutes, int? seconds, int? milliseconds)
        {
            if (hours24.HasValue || minutes.HasValue || seconds.HasValue || milliseconds.HasValue)
                _pendingResultValueChange = new(0, hours24 ?? 0, minutes ?? 0, seconds ?? 0, milliseconds ?? 0);
            else
                _pendingResultValueChange = null;
            InputValue = _pendingResultValueChange;
        }

        private void Hours24_ResultValuePropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            int? value = e.NewValue as int?;
            if (value == _pendingHours24Change)
                return;
            _pendingHours24Change = value;
            _pendingMinutesChange = Minutes.ResultValue;
            _pendingSecondsChange = Seconds.ResultValue;
            _pendingMillisecondsChange = Milliseconds.ResultValue;
            ImmutableArray<int?> values;
            bool isPm;
            if (value.HasValue)
            {
                switch (value.Value)
                {
                    case 0:
                        _pendingHours12Change = 12;
                        isPm = false;
                        break;
                    case 12:
                        _pendingHours12Change = 12;
                        isPm = true;
                        break;
                    default:
                        isPm = value < 12;
                        _pendingHours12Change = isPm ? value - 12 : value;
                        break;
                }
                values = ImmutableArray.Create(_pendingHours24Change, _pendingHours12Change, _pendingMinutesChange, _pendingSecondsChange, _pendingMillisecondsChange);
                IsPm = isPm;
                if (_pendingIsPmChange != isPm || !values.SequenceEqual(ImmutableArray.Create(_pendingHours24Change, _pendingHours12Change, _pendingMinutesChange, _pendingSecondsChange, _pendingMillisecondsChange)))
                    return;
            }
            else
            {
                _pendingIsPmChange = isPm = IsPm;
                _pendingHours12Change = null;
                values = ImmutableArray.Create(_pendingHours24Change, _pendingHours12Change, _pendingMinutesChange, _pendingSecondsChange, _pendingMillisecondsChange);
            }
            Hours12.InputValue = _pendingHours12Change;
            if (_pendingIsPmChange == isPm && values.SequenceEqual(ImmutableArray.Create(_pendingHours24Change, _pendingHours12Change, _pendingMinutesChange, _pendingSecondsChange, _pendingMillisecondsChange)))
                OnComponentValueChanged(_pendingHours24Change, _pendingMinutesChange, _pendingSecondsChange, _pendingMillisecondsChange);
        }

        private void Hours12_ResultValuePropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            int? value = e.NewValue as int?;
            if (value == _pendingHours24Change)
                return;
            _pendingHours12Change = value;
            _pendingMinutesChange = Minutes.ResultValue;
            _pendingSecondsChange = Seconds.ResultValue;
            _pendingMillisecondsChange = Milliseconds.ResultValue;
            bool isPm = _pendingIsPmChange = IsPm;
            if (value.HasValue)
                _pendingHours24Change = isPm ? ((value.Value == 12) ? value : value.Value + 12) : (value.Value == 12) ? 0 : value;
            else
                _pendingHours24Change = null;
            ImmutableArray<int?> values = ImmutableArray.Create(_pendingHours24Change, _pendingHours12Change, _pendingMinutesChange, _pendingSecondsChange, _pendingMillisecondsChange);
            Hours24.InputValue = _pendingHours24Change;
            if (_pendingIsPmChange == isPm && values.SequenceEqual(ImmutableArray.Create(_pendingHours24Change, _pendingHours12Change, _pendingMinutesChange, _pendingSecondsChange, _pendingMillisecondsChange)))
                OnComponentValueChanged(_pendingHours24Change, _pendingMinutesChange, _pendingSecondsChange, _pendingMillisecondsChange);
        }

        private void Minutes_ResultValuePropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _pendingHours24Change = Hours12.ResultValue;
            bool isPm = _pendingIsPmChange = IsPm;
            _pendingMinutesChange = Minutes.ResultValue;
            _pendingSecondsChange = Seconds.ResultValue;
            _pendingMillisecondsChange = Milliseconds.ResultValue;
            ImmutableArray<int?> values = ImmutableArray.Create(_pendingHours24Change, _pendingHours12Change, _pendingMinutesChange, _pendingSecondsChange, _pendingMillisecondsChange);
            Minutes.InputValue = _pendingMinutesChange;
            if (_pendingIsPmChange == isPm && values.SequenceEqual(ImmutableArray.Create(_pendingHours24Change, _pendingHours12Change, _pendingMinutesChange, _pendingSecondsChange, _pendingMillisecondsChange)))
                OnComponentValueChanged(_pendingHours24Change, _pendingMinutesChange, _pendingSecondsChange, _pendingMillisecondsChange);
        }

        private void Seconds_ResultValuePropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _pendingHours24Change = Hours12.ResultValue;
            bool isPm = _pendingIsPmChange = IsPm;
            _pendingMinutesChange = Minutes.ResultValue;
            _pendingSecondsChange = Seconds.ResultValue;
            _pendingMillisecondsChange = Milliseconds.ResultValue;
            ImmutableArray<int?> values = ImmutableArray.Create(_pendingHours24Change, _pendingHours12Change, _pendingMinutesChange, _pendingSecondsChange, _pendingMillisecondsChange);
            Seconds.InputValue = _pendingSecondsChange;
            if (_pendingIsPmChange == isPm && values.SequenceEqual(ImmutableArray.Create(_pendingHours24Change, _pendingHours12Change, _pendingMinutesChange, _pendingSecondsChange, _pendingMillisecondsChange)))
                OnComponentValueChanged(_pendingHours24Change, _pendingMinutesChange, _pendingSecondsChange, _pendingMillisecondsChange);
        }

        private void Milliseconds_ResultValuePropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _pendingHours24Change = Hours12.ResultValue;
            bool isPm = _pendingIsPmChange = IsPm;
            _pendingMinutesChange = Minutes.ResultValue;
            _pendingSecondsChange = Seconds.ResultValue;
            _pendingMillisecondsChange = Milliseconds.ResultValue;
            ImmutableArray<int?> values = ImmutableArray.Create(_pendingHours24Change, _pendingHours12Change, _pendingMinutesChange, _pendingSecondsChange, _pendingMillisecondsChange);
            Milliseconds.InputValue = _pendingMillisecondsChange;
            if (_pendingIsPmChange == isPm && values.SequenceEqual(ImmutableArray.Create(_pendingHours24Change, _pendingHours12Change, _pendingMinutesChange, _pendingSecondsChange, _pendingMillisecondsChange)))
                OnComponentValueChanged(_pendingHours24Change, _pendingMinutesChange, _pendingSecondsChange, _pendingMillisecondsChange);
        }

        private void Hours24_HasErrorsPropertyChanged(object sender, DependencyPropertyChangedEventArgs e) => HasComponentValueErrors = (bool)e.NewValue || Hours12.HasErrors || Minutes.HasErrors || Seconds.HasErrors || Milliseconds.HasErrors;

        private void Hours12_HasErrorsPropertyChanged(object sender, DependencyPropertyChangedEventArgs e) => HasComponentValueErrors = (bool)e.NewValue || Hours24.HasErrors || Minutes.HasErrors || Seconds.HasErrors || Milliseconds.HasErrors;

        private void Minutes_HasErrorsPropertyChanged(object sender, DependencyPropertyChangedEventArgs e) => HasComponentValueErrors = (bool)e.NewValue || Hours12.HasErrors || Hours24.HasErrors || Seconds.HasErrors || Milliseconds.HasErrors;

        private void Seconds_HasErrorsPropertyChanged(object sender, DependencyPropertyChangedEventArgs e) => HasComponentValueErrors = (bool)e.NewValue || Hours12.HasErrors || Hours24.HasErrors || Minutes.HasErrors || Milliseconds.HasErrors;

        private void Milliseconds_HasErrorsPropertyChanged(object sender, DependencyPropertyChangedEventArgs e) => HasComponentValueErrors = (bool)e.NewValue || !(Hours12.HasErrors || Hours24.HasErrors || Minutes.HasErrors || Seconds.HasErrors);
    }
}
