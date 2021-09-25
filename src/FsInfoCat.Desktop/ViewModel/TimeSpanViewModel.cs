using Microsoft.Extensions.Logging;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class TimeSpanViewModel : OptionalValueViewModel<TimeSpan>
    {
        private TimeSpan? _pendingResultValueChange;

        #region Days Property Members

        private int? _pendingDaysChange;

        private static readonly DependencyPropertyKey DaysPropertyKey = DependencyPropertyBuilder<TimeSpanViewModel, OptionalValueViewModel<int>>
            .Register(nameof(Days))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Days"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DaysProperty = DaysPropertyKey.DependencyProperty;

        public OptionalValueViewModel<int> Days => (OptionalValueViewModel<int>)GetValue(DaysProperty);

        private void Days_ValidateInputValue(object sender, PropertyValidatingEventArgs<int> e)
        {
            if (e.Value < 0)
                ErrorInfo.SetError(FsInfoCat.Properties.Resources.ErrorMessage_InvalidDays, DaysProperty.Name);
            else
                ErrorInfo.ClearErrors(DaysProperty.Name);
        }

        private void Days_ResultValuePropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            int? newValue = e.NewValue as int?;
            if (_pendingDaysChange == newValue)
                return;
            _pendingDaysChange = newValue;
            OnComponentValueChanged(newValue, Hours.ResultValue, Minutes.ResultValue, Seconds.ResultValue, Milliseconds.ResultValue);
        }

        #endregion
        #region Hours Property Members

        private int? _pendingHoursChange;

        private static readonly DependencyPropertyKey HoursPropertyKey = DependencyPropertyBuilder<TimeSpanViewModel, OptionalValueViewModel<int>>
            .Register(nameof(Hours))
            .DefaultValue(null)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Hours"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HoursProperty = HoursPropertyKey.DependencyProperty;

        public OptionalValueViewModel<int> Hours { get => (OptionalValueViewModel<int>)GetValue(HoursProperty); private set => SetValue(HoursPropertyKey, value); }

        protected override void OnInputValuePropertyChanged(TimeSpan? newValue)
        {
            if (newValue.HasValue)
            {
                if (newValue.Value.Milliseconds != 0 && !Milliseconds.IsEnabled)
                    base.OnInputValuePropertyChanged(new(newValue.Value.Days, Hours.IsEnabled ? newValue.Value.Hours : 0, Minutes.IsEnabled ? newValue.Value.Minutes : 0, Seconds.IsEnabled ? newValue.Value.Seconds : 0, 0));
                else if (newValue.Value.Seconds != 0 && !Seconds.IsEnabled)
                    base.OnInputValuePropertyChanged(new(newValue.Value.Days, Hours.IsEnabled ? newValue.Value.Hours : 0, Minutes.IsEnabled ? newValue.Value.Minutes : 0, 0, newValue.Value.Milliseconds));
                else if (newValue.Value.Minutes != 0 && !Minutes.IsEnabled)
                    base.OnInputValuePropertyChanged(new(newValue.Value.Days, Hours.IsEnabled ? newValue.Value.Hours : 0, newValue.Value.Seconds, newValue.Value.Milliseconds));
                else if (newValue.Value.Hours != 0 && !Hours.IsEnabled)
                    base.OnInputValuePropertyChanged(new(newValue.Value.Days, 0, newValue.Value.Minutes, newValue.Value.Seconds, newValue.Value.Milliseconds));
                else
                    base.OnInputValuePropertyChanged(newValue);
            }
            else
                base.OnInputValuePropertyChanged(newValue);
        }

        private void Hours_ValidateInputValue(object sender, PropertyValidatingEventArgs<int> e)
        {
            if (e.Value < 0 || e.Value > 23)
                ErrorInfo.SetError(FsInfoCat.Properties.Resources.ErrorMessage_InvalidHours, HoursProperty.Name);
            else
                ErrorInfo.ClearErrors(HoursProperty.Name);
        }

        private void Hours_ResultValuePropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            int? newValue = e.NewValue as int?;
            if (_pendingHoursChange == newValue)
                return;
            _pendingHoursChange = newValue;
            OnComponentValueChanged(Days.ResultValue, newValue, Minutes.ResultValue, Seconds.ResultValue, Milliseconds.ResultValue);
        }

        #endregion
        #region Minutes Property Members

        private int? _pendingMinutesChange;

        private static readonly DependencyPropertyKey MinutesPropertyKey = DependencyPropertyBuilder<TimeSpanViewModel, OptionalValueViewModel<int>>
            .Register(nameof(Minutes))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Minutes"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinutesProperty = MinutesPropertyKey.DependencyProperty;

        public OptionalValueViewModel<int> Minutes => (OptionalValueViewModel<int>)GetValue(MinutesProperty);

        private void Minutes_ValidateInputValue(object sender, PropertyValidatingEventArgs<int> e)
        {
            if (e.Value < 0 || e.Value > 59)
                ErrorInfo.SetError(FsInfoCat.Properties.Resources.ErrorMessage_InvalidMinutes, MinutesProperty.Name);
            else
                ErrorInfo.ClearErrors(MinutesProperty.Name);
        }

        private void Minutes_ResultValuePropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            int? newValue = e.NewValue as int?;
            if (_pendingMinutesChange == newValue)
                return;
            _pendingMinutesChange = newValue;
            OnComponentValueChanged(Days.ResultValue, Hours.ResultValue, newValue, Seconds.ResultValue, Milliseconds.ResultValue);
        }

        #endregion
        #region Seconds Property Members

        private int? _pendingSecondsChange;

        private static readonly DependencyPropertyKey SecondsPropertyKey = DependencyPropertyBuilder<TimeSpanViewModel, OptionalValueViewModel<int>>
            .Register(nameof(Seconds))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Seconds"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SecondsProperty = SecondsPropertyKey.DependencyProperty;

        public OptionalValueViewModel<int> Seconds { get => (OptionalValueViewModel<int>)GetValue(SecondsProperty); private set => SetValue(SecondsPropertyKey, value); }

        private void Seconds_ValidateInputValue(object sender, PropertyValidatingEventArgs<int> e)
        {
            if (e.Value < 0 || e.Value > 59)
                ErrorInfo.SetError(FsInfoCat.Properties.Resources.ErrorMessage_InvalidSeconds, SecondsProperty.Name);
            else
                ErrorInfo.ClearErrors(SecondsProperty.Name);
        }

        private void Seconds_ResultValuePropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            int? newValue = e.NewValue as int?;
            if (_pendingSecondsChange == newValue)
                return;
            _pendingSecondsChange = newValue;
            OnComponentValueChanged(Days.ResultValue, Hours.ResultValue, Minutes.ResultValue, newValue, Milliseconds.ResultValue);
        }

        #endregion
        #region Milliseconds Property Members

        private int? _pendingMillisecondsChange;

        private static readonly DependencyPropertyKey MillisecondsPropertyKey = DependencyPropertyBuilder<TimeSpanViewModel, OptionalValueViewModel<int>>
            .Register(nameof(Milliseconds))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Milliseconds"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MillisecondsProperty = MillisecondsPropertyKey.DependencyProperty;

        public OptionalValueViewModel<int> Milliseconds { get => (OptionalValueViewModel<int>)GetValue(MillisecondsProperty); private set => SetValue(MillisecondsPropertyKey, value); }

        private void Milliseconds_ValidateInputValue(object sender, PropertyValidatingEventArgs<int> e)
        {
            if (e.Value < 0 || e.Value > 999)
                ErrorInfo.SetError(FsInfoCat.Properties.Resources.ErrorMessage_InvalidMilliseconds, MillisecondsProperty.Name);
            else
                ErrorInfo.ClearErrors(MillisecondsProperty.Name);
        }

        private void Milliseconds_ResultValuePropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            int? newValue = e.NewValue as int?;
            if (_pendingMillisecondsChange == newValue)
                return;
            _pendingMillisecondsChange = newValue;
            OnComponentValueChanged(Days.ResultValue, Hours.ResultValue, Minutes.ResultValue, Seconds.ResultValue, newValue);
        }

        #endregion
        #region HasComponentValueErrors Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="HasComponentValueErrors"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler HasComponentValueErrorsPropertyChanged;

        private static readonly DependencyPropertyKey HasComponentValueErrorsPropertyKey = DependencyPropertyBuilder<TimeSpanViewModel, bool>
            .Register(nameof(HasComponentValueErrors))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as TimeSpanViewModel)?.RaiseHasComponentValueErrorsPropertyChanged(e))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="HasComponentValueErrors"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HasComponentValueErrorsProperty = HasComponentValueErrorsPropertyKey.DependencyProperty;
        private readonly ILogger<TimeSpanViewModel> _logger;

        public bool HasComponentValueErrors { get => (bool)GetValue(HasComponentValueErrorsProperty); private set => SetValue(HasComponentValueErrorsPropertyKey, value); }

        /// <summary>
        /// Called when the <see cref="PropertyChangedCallback">PropertyChanged</see> event on <see cref="HasComponentValueErrorsProperty"/> is raised.
        /// </summary>
        /// <param name="args">The Event data that is issued by the event on <see cref="HasComponentValueErrorsProperty"/> that tracks changes to its effective value.</param>
        protected void RaiseHasComponentValueErrorsPropertyChanged(DependencyPropertyChangedEventArgs args) => HasComponentValueErrorsPropertyChanged?.Invoke(this, args);

        #endregion

        public TimeSpanViewModel()
        {
            _logger = App.GetLogger(this);
            OptionalValueViewModel<int> days = new(), hours = new(), minutes = new(), seconds = new(), milliseconds = new();
            SetValue(DaysPropertyKey, days);
            SetValue(HoursPropertyKey, hours);
            SetValue(MinutesPropertyKey, minutes);
            SetValue(SecondsPropertyKey, seconds);
            SetValue(MillisecondsPropertyKey, milliseconds);
            days.ValidateInputValue += Days_ValidateInputValue;
            hours.ValidateInputValue += Hours_ValidateInputValue;
            minutes.ValidateInputValue += Minutes_ValidateInputValue;
            seconds.ValidateInputValue += Seconds_ValidateInputValue;
            milliseconds.ValidateInputValue += Milliseconds_ValidateInputValue;
            days.ResultValuePropertyChanged += Days_ResultValuePropertyChanged;
            hours.ResultValuePropertyChanged += Hours_ResultValuePropertyChanged;
            minutes.ResultValuePropertyChanged += Minutes_ResultValuePropertyChanged;
            seconds.ResultValuePropertyChanged += Seconds_ResultValuePropertyChanged;
            milliseconds.ResultValuePropertyChanged += Milliseconds_ResultValuePropertyChanged;
            days.HasErrorsPropertyChanged += Days_HasErrorsPropertyChanged;
            hours.HasErrorsPropertyChanged += Hours_HasErrorsPropertyChanged;
            minutes.HasErrorsPropertyChanged += Minutes_HasErrorsPropertyChanged;
            seconds.HasErrorsPropertyChanged += Seconds_HasErrorsPropertyChanged;
            milliseconds.HasErrorsPropertyChanged += Milliseconds_HasErrorsPropertyChanged;
        }

        private void Days_HasErrorsPropertyChanged(object sender, DependencyPropertyChangedEventArgs e) => HasComponentValueErrors = ((bool)e.NewValue) || Hours.HasErrors || Minutes.HasErrors || Seconds.HasErrors ||
            Milliseconds.HasErrors;

        private void Hours_HasErrorsPropertyChanged(object sender, DependencyPropertyChangedEventArgs e) => HasComponentValueErrors = ((bool)e.NewValue) || Days.HasErrors || Minutes.HasErrors || Seconds.HasErrors ||
            Milliseconds.HasErrors;

        private void Minutes_HasErrorsPropertyChanged(object sender, DependencyPropertyChangedEventArgs e) => HasComponentValueErrors = ((bool)e.NewValue) || Days.HasErrors || Hours.HasErrors || Seconds.HasErrors ||
            Milliseconds.HasErrors;

        private void Seconds_HasErrorsPropertyChanged(object sender, DependencyPropertyChangedEventArgs e) => HasComponentValueErrors = ((bool)e.NewValue) || Days.HasErrors || Hours.HasErrors || Minutes.HasErrors ||
            Milliseconds.HasErrors;

        private void Milliseconds_HasErrorsPropertyChanged(object sender, DependencyPropertyChangedEventArgs e) => HasComponentValueErrors = ((bool)e.NewValue) || Days.HasErrors || Hours.HasErrors || Minutes.HasErrors ||
            Seconds.HasErrors;

        private void OnComponentValueChanged(int? days, int? hours, int? minutes, int? seconds, int? milliseconds)
        {
            if (days.HasValue || hours.HasValue || minutes.HasValue)
                _pendingResultValueChange = new TimeSpan(days ?? 0, hours ?? 0, minutes ?? 0, seconds ?? 0, milliseconds ?? 0);
            else
                _pendingResultValueChange = null;
            InputValue = _pendingResultValueChange;
        }

        protected override void OnResultValuePropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            base.OnResultValuePropertyChanged(args);
            TimeSpan? newValue = args.NewValue as TimeSpan?;
            if (newValue == _pendingResultValueChange)
                return;
            _pendingResultValueChange = newValue;
            if (newValue.HasValue)
            {
                _pendingDaysChange = newValue.Value.Days;
                _pendingHoursChange = newValue.Value.Hours;
                _pendingMinutesChange = newValue.Value.Minutes;
                _pendingSecondsChange = newValue.Value.Seconds;
                _pendingMillisecondsChange = newValue.Value.Milliseconds;
                ImmutableArray<int?> values = ImmutableArray.Create(_pendingDaysChange, _pendingHoursChange, _pendingMinutesChange, _pendingSecondsChange, _pendingMillisecondsChange);
                Days.InputValue = _pendingDaysChange;
                if (values.SequenceEqual(ImmutableArray.Create(_pendingDaysChange, _pendingHoursChange, _pendingMinutesChange, _pendingSecondsChange, _pendingMillisecondsChange)))
                {
                    Hours.InputValue = _pendingHoursChange;
                    if (values.SequenceEqual(ImmutableArray.Create(_pendingDaysChange, _pendingHoursChange, _pendingMinutesChange, _pendingSecondsChange, _pendingMillisecondsChange)))
                    {
                        Minutes.InputValue = _pendingMinutesChange;
                        if (values.SequenceEqual(ImmutableArray.Create(_pendingDaysChange, _pendingHoursChange, _pendingMinutesChange, _pendingSecondsChange, _pendingMillisecondsChange)))
                        {
                            Seconds.InputValue = _pendingSecondsChange;
                            if (values.SequenceEqual(ImmutableArray.Create(_pendingDaysChange, _pendingHoursChange, _pendingMinutesChange, _pendingSecondsChange, _pendingMillisecondsChange)))
                                Milliseconds.InputValue = _pendingMillisecondsChange;
                        }
                    }
                }
            }
            else
            {
                _pendingDaysChange = _pendingHoursChange = _pendingMinutesChange = _pendingSecondsChange = _pendingMillisecondsChange = null;
                Days.InputValue = null;
                if (!(_pendingDaysChange.HasValue || _pendingHoursChange.HasValue || _pendingMinutesChange.HasValue || _pendingSecondsChange.HasValue || _pendingMillisecondsChange.HasValue))
                {
                    Hours.InputValue = null;
                    if (!(_pendingDaysChange.HasValue || _pendingHoursChange.HasValue || _pendingMinutesChange.HasValue || _pendingSecondsChange.HasValue || _pendingMillisecondsChange.HasValue))
                    {
                        Minutes.InputValue = null;
                        if (!(_pendingDaysChange.HasValue || _pendingHoursChange.HasValue || _pendingMinutesChange.HasValue || _pendingSecondsChange.HasValue || _pendingMillisecondsChange.HasValue))
                        {
                            Seconds.InputValue = null;
                            if (!(_pendingDaysChange.HasValue || _pendingHoursChange.HasValue || _pendingMinutesChange.HasValue || _pendingSecondsChange.HasValue || _pendingMillisecondsChange.HasValue))
                                Milliseconds.InputValue = null;
                        }
                    }
                }
            }
        }
    }
}
