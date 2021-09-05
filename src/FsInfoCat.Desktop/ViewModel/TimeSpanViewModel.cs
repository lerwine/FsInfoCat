using System;
using System.Collections;
using System.ComponentModel;
using System.Threading;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class TimeSpanViewModel : DependencyObject, INotifyDataErrorInfo
    {
        private int _valueChanging = 0;

        /// <summary>
        /// Occurs when the validation errors have changed for a property or for the entire object.
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

#pragma warning disable IDE0060 // Remove unused parameter
        #region HasErrors Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="HasErrors"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler HasErrorsPropertyChanged;

        private static readonly DependencyPropertyKey HasErrorsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(HasErrors), typeof(bool),
            typeof(TimeSpanViewModel), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as TimeSpanViewModel)?.HasErrorsPropertyChanged?.Invoke(d, e)));

        /// <summary>
        /// Identifies the <see cref="HasErrors"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HasErrorsProperty = HasErrorsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool HasErrors { get => (bool)GetValue(HasErrorsProperty); private set => SetValue(HasErrorsPropertyKey, value); }

        #endregion
        #region IsRequired Property Members

        private static readonly DependencyPropertyKey IsRequiredPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsRequired), typeof(bool), typeof(TimeSpanViewModel),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as TimeSpanViewModel).OnIsRequiredPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        /// <summary>
        /// Identifies the <see cref="IsRequired"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsRequiredProperty = IsRequiredPropertyKey.DependencyProperty;

        public bool IsRequired { get => (bool)GetValue(IsRequiredProperty); private set => SetValue(IsRequiredPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="IsRequired"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="IsRequired"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="IsRequired"/> property.</param>
        protected virtual void OnIsRequiredPropertyChanged(bool oldValue, bool newValue)
        {
            if (!(Value.HasValue || DaysValue.HasValue || Hours24Value.HasValue || MinutesValue.HasValue))
            {
                if (newValue)
                {
                    ValidationMessage = FsInfoCat.Properties.Resources.ErrorMessage_Required;
                    DaysValidationMessage = FsInfoCat.Properties.Resources.ErrorMessage_Required;
                    Hours12ValidationMessage = FsInfoCat.Properties.Resources.ErrorMessage_Required;
                    Hours24ValidationMessage = FsInfoCat.Properties.Resources.ErrorMessage_Required;
                    MinutesValidationMessage = FsInfoCat.Properties.Resources.ErrorMessage_Required;
                }
                else
                {
                    ValidationMessage = string.Empty;
                    DaysValidationMessage = string.Empty;
                    Hours12ValidationMessage = string.Empty;
                    Hours24ValidationMessage = string.Empty;
                    MinutesValidationMessage = string.Empty;
                }
            }
        }

        #endregion
        #region Value Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="Value"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler ValuePropertyChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Value"/> dependency property is being validated.
        /// </summary>
        public event EventHandler<PropertyValidatingEventArgs<TimeSpan?, (string DaysMessage, string HoursMessage, string MinutesMessage)>> ValidateValue;

        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(TimeSpan?), typeof(TimeSpanViewModel),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as TimeSpanViewModel)?.OnValuePropertyChanged(e),
                    TimeSpanCoersion.NormalizedToMinutes.ToCoerceValueCallback()));

        /// <summary>
        /// Gets or sets the timespan value.
        /// </summary>
        /// <value>The .</value>
        public TimeSpan? Value { get => (TimeSpan?)GetValue(ValueProperty); set => SetValue(ValueProperty, value); }

        /// <summary>
        /// Called when the <see cref="PropertyChangedCallback">PropertyChanged</see> event on <see cref="ValueProperty"/> is raised.
        /// </summary>
        /// <param name="args">The Event data that is issued by the event on <see cref="ValueProperty"/> that tracks changes to its effective value.</param>
        protected virtual void OnValuePropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try { OnValuePropertyChanged((TimeSpan?)args.OldValue, (TimeSpan?)args.NewValue); }
            finally { ValuePropertyChanged?.Invoke(this, args); }
        }

        /// <summary>
        /// Called when the value of the <see cref="Value"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Value"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Value"/> property.</param>
        protected virtual void OnValuePropertyChanged(TimeSpan? oldValue, TimeSpan? newValue)
        {
            bool isChange = Interlocked.Increment(ref _valueChanging) == 1;
            try
            {
                if (isChange)
                {
                    if (newValue.HasValue)
                    {
                        DaysValue = newValue.Value.Days;
                        Hours24Value = newValue.Value.Hours;
                        MinutesValue = newValue.Value.Minutes;
                    }
                    else
                    {
                        DaysValue = null;
                        Hours24Value = null;
                        MinutesValue = null;
                    }
                }
            }
            finally { Interlocked.Decrement(ref _valueChanging); }
            PropertyValidatingEventArgs<TimeSpan?, (string DaysMessage, string HoursMessage, string MinutesMessage)> args = new(newValue, nameof(Value),
                (DaysValidationMessage, HoursValidationMessage, MinutesValidationMessage));
            if (newValue.HasValue)
            {
                if (newValue.Value < TimeSpan.Zero)
                    args.ValidationMessage = FsInfoCat.Properties.Resources.ErrorMessage_InvalidTime;
            }
            else if (IsRequired && (DaysValue.HasValue || Hours24Value.HasValue || MinutesValue.HasValue) &&
                DaysValidationMessage.Length == 0 && HoursValidationMessage.Length == 0 && MinutesValidationMessage.Length == 0)
                args.ValidationMessage = FsInfoCat.Properties.Resources.ErrorMessage_Required;
            OnValidateValue(args);
            ValidationMessage = args.ValidationMessage;
        }

        protected virtual void OnValidateValue(PropertyValidatingEventArgs<TimeSpan?, (string DaysMessage, string HoursMessage, string MinutesMessage)> args) =>
            ValidateValue?.Invoke(this, args);

        #endregion
        #region AggregateValidationMessage Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="AggregateValidationMessage"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler AggregateValidationMessagePropertyChanged;

        private static readonly DependencyPropertyKey AggregateValidationMessagePropertyKey = DependencyProperty.RegisterReadOnly(nameof(AggregateValidationMessage),
            typeof(string), typeof(TimeSpanViewModel), new PropertyMetadata("",
                (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as TimeSpanViewModel)?.OnAggregateValidationMessagePropertyChanged(e)));

        /// <summary>
        /// Identifies the <see cref="AggregateValidationMessage"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AggregateValidationMessageProperty = AggregateValidationMessagePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets aggregate error message string.
        /// </summary>
        /// <value>The error message string that is an aggregate of <see cref="ValidationMessage"/>, <see cref="DaysValidationMessage"/>,
        /// <see cref="HoursValidationMessage"/> and <see cref="MinutesValidationMessage"/>.</value>
        public string AggregateValidationMessage
        {
            get => GetValue(AggregateValidationMessageProperty) as string;
            private set => SetValue(AggregateValidationMessagePropertyKey, value);
        }

        protected virtual void OnAggregateValidationMessagePropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try { AggregateValidationMessagePropertyChanged?.Invoke(this, args); }
            finally { HasErrors = !string.IsNullOrWhiteSpace(args.NewValue as string); }
        }

        private void UpdateAggregateValidationMessage(string minutes, string hours, string days, string value)
        {
            if ((string.IsNullOrEmpty(minutes) || value == minutes) && (string.IsNullOrEmpty(hours) || value == hours) && (string.IsNullOrEmpty(days) || value == days))
                AggregateValidationMessage = value;
            else if (string.IsNullOrEmpty(minutes))
            {
                if (string.IsNullOrEmpty(hours))
                {
                    AggregateValidationMessage = string.IsNullOrEmpty(value) ? string.Format(FsInfoCat.Properties.Resources.FormatMessage_InvalidDays, days) :
                        $@"{value}
{string.Format(FsInfoCat.Properties.Resources.FormatMessage_InvalidDays, days)}";
                }
                else if (string.IsNullOrEmpty(days))
                    AggregateValidationMessage = string.IsNullOrEmpty(value) ? string.Format(FsInfoCat.Properties.Resources.FormatMessage_InvalidHours, hours) :
                        $@"{value}
{string.Format(FsInfoCat.Properties.Resources.FormatMessage_InvalidHours, hours)}";
                else
                    AggregateValidationMessage = string.IsNullOrEmpty(value) ? $@"{string.Format(FsInfoCat.Properties.Resources.FormatMessage_InvalidDays, days)}
{string.Format(FsInfoCat.Properties.Resources.FormatMessage_InvalidHours, hours)}" :
                        $@"{value}
{string.Format(FsInfoCat.Properties.Resources.FormatMessage_InvalidDays, days)}
{string.Format(FsInfoCat.Properties.Resources.FormatMessage_InvalidHours, hours)}";
            }
            else if (string.IsNullOrEmpty(hours))
            {
                if (string.IsNullOrEmpty(days))
                    AggregateValidationMessage = string.IsNullOrEmpty(value) ? string.Format(FsInfoCat.Properties.Resources.FormatMessage_InvalidMinutes, minutes) :
                        $@"{value}
{string.Format(FsInfoCat.Properties.Resources.FormatMessage_InvalidMinutes, minutes)}";
                else
                    AggregateValidationMessage = string.IsNullOrEmpty(value) ? $@"{string.Format(FsInfoCat.Properties.Resources.FormatMessage_InvalidDays, days)}
{string.Format(FsInfoCat.Properties.Resources.FormatMessage_InvalidMinutes, minutes)}" :
                        $@"{value}
{string.Format(FsInfoCat.Properties.Resources.FormatMessage_InvalidDays, days)}
{string.Format(FsInfoCat.Properties.Resources.FormatMessage_InvalidMinutes, minutes)}";
            }
            else if (string.IsNullOrEmpty(days))
                AggregateValidationMessage = string.IsNullOrEmpty(value) ? $@"{string.Format(FsInfoCat.Properties.Resources.FormatMessage_InvalidHours, hours)}
{string.Format(FsInfoCat.Properties.Resources.FormatMessage_InvalidMinutes, minutes)}" :
                    $@"{value}
{string.Format(FsInfoCat.Properties.Resources.FormatMessage_InvalidHours, hours)}
{string.Format(FsInfoCat.Properties.Resources.FormatMessage_InvalidMinutes, minutes)}";
            else
                AggregateValidationMessage = string.IsNullOrEmpty(value) ? $@"{string.Format(FsInfoCat.Properties.Resources.FormatMessage_InvalidDays, days)}
{string.Format(FsInfoCat.Properties.Resources.FormatMessage_InvalidHours, hours)}
{string.Format(FsInfoCat.Properties.Resources.FormatMessage_InvalidMinutes, minutes)}" :
                    $@"{value}
{string.Format(FsInfoCat.Properties.Resources.FormatMessage_InvalidDays, days)}
{string.Format(FsInfoCat.Properties.Resources.FormatMessage_InvalidHours, hours)}
{string.Format(FsInfoCat.Properties.Resources.FormatMessage_InvalidMinutes, minutes)}";
        }

        #endregion
        #region ValidationMessage Property Members

        /// <summary>
        /// Identifies the <see cref="ValidationMessage"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValidationMessageProperty = DependencyProperty.Register(nameof(ValidationMessage), typeof(string),
            typeof(TimeSpanViewModel), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as TimeSpanViewModel)?.OnValidationMessagePropertyChanged(e.OldValue as string, e.NewValue as string),
                NormalizedOrEmptyStringCoersion.Default.ToCoerceValueCallback()));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string ValidationMessage { get => GetValue(ValidationMessageProperty) as string; set => SetValue(ValidationMessageProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ValidationMessage"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ValidationMessage"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ValidationMessage"/> property.</param>
        protected virtual void OnValidationMessagePropertyChanged(string oldValue, string newValue)
        {
            try { UpdateAggregateValidationMessage(MinutesValidationMessage, HoursValidationMessage, DaysValidationMessage, newValue); }
            finally { RaiseDataErrorsChanged(nameof(Value)); }
        }

        #endregion
        #region DaysValue Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="DaysValue"/> dependency property is being validated.
        /// </summary>
        public event EventHandler<PropertyValidatingEventArgs<int?, (int? Hours, int? Minutes)>> ValidateDays;

        /// <summary>
        /// Identifies the <see cref="DaysValue"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DaysValueProperty = DependencyProperty.Register(nameof(DaysValue), typeof(int?), typeof(TimeSpanViewModel),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as TimeSpanViewModel)?.OnDaysValuePropertyChanged((int?)e.OldValue, (int?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public int? DaysValue { get => (int?)GetValue(DaysValueProperty); set => SetValue(DaysValueProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="DaysValue"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="DaysValue"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="DaysValue"/> property.</param>
        protected virtual void OnDaysValuePropertyChanged(int? oldValue, int? newValue)
        {
            PropertyValidatingEventArgs<int?, (int? Hours, int? Minutes)> args = new(newValue, nameof(DaysValue), (Hours24Value, MinutesValue));
            if (newValue.HasValue)
            {
                if (newValue.Value < 0)
                    args.ValidationMessage = FsInfoCat.Properties.Resources.ErrorMessage_InvalidDays;
                if (!Hours12Value.HasValue)
                    Hours12ValidationMessage = "";
                if (!Hours24Value.HasValue)
                    Hours24ValidationMessage = "";
                if (!MinutesValue.HasValue)
                    MinutesValidationMessage = "";
            }
            else if (IsRequired && !(Hours24Value.HasValue || MinutesValue.HasValue) && HoursValidationMessage.Length == 0 && MinutesValidationMessage.Length == 0)
                args.ValidationMessage = FsInfoCat.Properties.Resources.ErrorMessage_Required;
            OnValidateDaysValue(args);
            DaysValidationMessage = args.ValidationMessage;
            bool ignoreChange = Interlocked.Increment(ref _valueChanging) != 1;
            try
            {
                if (ignoreChange)
                    return;
                if (newValue.HasValue)
                {
                    if (args.ValidationMessage.Length == 0 && HoursValidationMessage.Length == 0 && MinutesValidationMessage.Length == 0)
                        Value = new(newValue.Value, Hours24Value ?? 0, MinutesValue ?? 0, 0, 0);
                    else
                        Value = null;
                }
                else if (HoursValidationMessage.Length == 0 && MinutesValidationMessage.Length == 0)
                    Value = new(0, Hours24Value ?? 0, MinutesValue ?? 0, 0, 0);
                else
                    Value = null;
            }
            finally { Interlocked.Decrement(ref _valueChanging); }
        }

        protected virtual void OnValidateDaysValue(PropertyValidatingEventArgs<int?, (int? Hours, int? Minutes)> args) =>
            ValidateDays?.Invoke(this, args);

        #endregion
        #region DaysValidationMessage Property Members

        /// <summary>
        /// Identifies the <see cref="DaysValidationMessage"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DaysValidationMessageProperty = DependencyProperty.Register(nameof(DaysValidationMessage), typeof(string),
            typeof(TimeSpanViewModel), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as TimeSpanViewModel)?.OnDaysValidationMessagePropertyChanged(e.OldValue as string, e.NewValue as string),
                NormalizedOrEmptyStringCoersion.Default.ToCoerceValueCallback()));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string DaysValidationMessage { get => GetValue(DaysValidationMessageProperty) as string; set => SetValue(DaysValidationMessageProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="DaysValidationMessage"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="DaysValidationMessage"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="DaysValidationMessage"/> property.</param>
        protected virtual void OnDaysValidationMessagePropertyChanged(string oldValue, string newValue)
        {
            try { UpdateAggregateValidationMessage(MinutesValidationMessage, HoursValidationMessage, newValue, ValidationMessage); }
            finally { RaiseDataErrorsChanged(nameof(DaysValue)); }
        }

        #endregion
        #region Hours24Value Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="Hours24Value"/> dependency property is being validated.
        /// </summary>
        public event EventHandler<PropertyValidatingEventArgs<int?, (int? Days, int? Minutes)>> ValidateHours24Value;

        /// <summary>
        /// Identifies the <see cref="Hours24Value"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty Hours24ValueProperty = DependencyProperty.Register(nameof(Hours24Value), typeof(int?), typeof(TimeSpanViewModel),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as TimeSpanViewModel)?.OnHours24ValuePropertyChanged((int?)e.OldValue, (int?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public int? Hours24Value { get => (int?)GetValue(Hours24ValueProperty); set => SetValue(Hours24ValueProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Hours24Value"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Hours24Value"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Hours24Value"/> property.</param>
        protected virtual void OnHours24ValuePropertyChanged(int? oldValue, int? newValue)
        {
            PropertyValidatingEventArgs<int?, (int? Days, int? Minutes)> args = new(newValue, nameof(Hours24Value), (DaysValue, MinutesValue));
            if (newValue.HasValue)
            {
                if (newValue.Value < 0 || newValue.Value > 23)
                    args.ValidationMessage = FsInfoCat.Properties.Resources.ErrorMessage_InvalidHours;
                if (!DaysValue.HasValue)
                    DaysValidationMessage = "";
                if (!MinutesValue.HasValue)
                    MinutesValidationMessage = "";
            }
            else if (IsRequired && !(DaysValue.HasValue || MinutesValue.HasValue) && DaysValidationMessage.Length == 0 && MinutesValidationMessage.Length == 0)
                args.ValidationMessage = FsInfoCat.Properties.Resources.ErrorMessage_Required;
            OnValidateHours24(args);
            bool isChange = Interlocked.Increment(ref _valueChanging) == 1;
            try
            {
                if (isChange)
                {
                    if (newValue.HasValue)
                    {
                        switch (newValue.Value)
                        {
                            case 0:
                                Hours12Value = 12;
                                IsPm = false;
                                break;
                            case 12:
                            case -12:
                                Hours12Value = 12;
                                IsPm = true;
                                break;
                            default:
                                if (Math.Abs(newValue.Value) > 12)
                                {
                                    Hours12Value = (newValue.Value < 0) ? newValue.Value + 12 : newValue.Value - 12;
                                    IsPm = true;
                                }
                                else
                                {
                                    Hours12Value = newValue;
                                    IsPm = false;
                                }
                                break;
                        }
                        if (args.ValidationMessage.Length == 0 && DaysValidationMessage.Length == 0 && MinutesValidationMessage.Length == 0)
                            Value = new(DaysValue ?? 0, newValue.Value, MinutesValue ?? 0, 0, 0);
                        else
                            Value = null;
                    }
                    else
                    {
                        Hours12Value = null;
                        if (DaysValidationMessage.Length == 0 && MinutesValidationMessage.Length == 0)
                            Value = new(DaysValue ?? 0, 0, MinutesValue ?? 0, 0, 0);
                        else
                            Value = null;
                    }
                }
            }
            finally { Interlocked.Decrement(ref _valueChanging); }
            Hours24ValidationMessage = args.ValidationMessage;
        }

        protected virtual void OnValidateHours24(PropertyValidatingEventArgs<int?, (int? Days, int? Hours)> args) =>
            ValidateHours24Value?.Invoke(this, args);

        #endregion
        #region Hours24ValidationMessage Property Members

        /// <summary>
        /// Identifies the <see cref="Hours24ValidationMessage"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty Hours24ValidationMessageProperty = DependencyProperty.Register(nameof(Hours24ValidationMessage), typeof(string),
            typeof(TimeSpanViewModel), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as TimeSpanViewModel)?.OnHours24ValidationMessagePropertyChanged(e.OldValue as string, e.NewValue as string),
                NormalizedOrEmptyStringCoersion.Default.ToCoerceValueCallback()));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Hours24ValidationMessage { get => GetValue(Hours24ValidationMessageProperty) as string; set => SetValue(Hours24ValidationMessageProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Hours24ValidationMessage"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Hours24ValidationMessage"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Hours24ValidationMessage"/> property.</param>
        protected virtual void OnHours24ValidationMessagePropertyChanged(string oldValue, string newValue)
        {
            try { HoursValidationMessage = string.IsNullOrEmpty(newValue) ? Hours12ValidationMessage : newValue; }
            finally { RaiseDataErrorsChanged(nameof(Hours24Value)); }
        }

        #endregion
        #region IsPm Property Members

        /// <summary>
        /// Identifies the <see cref="IsPm"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsPmProperty = DependencyProperty.Register(nameof(IsPm), typeof(bool), typeof(TimeSpanViewModel),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as TimeSpanViewModel)?.OnIsPmPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool IsPm { get => (bool)GetValue(IsPmProperty); set => SetValue(IsPmProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="IsPm"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="IsPm"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="IsPm"/> property.</param>
        protected virtual void OnIsPmPropertyChanged(bool oldValue, bool newValue)
        {
            bool ignoreChange = Interlocked.Increment(ref _valueChanging) != 1;
            try
            {
                if (ignoreChange)
                    return;
                int? hours = Hours12Value;
                if (hours.HasValue)
                {
                    int h = Math.Abs(hours.Value);
                    if (h == 12)
                        Hours24Value = newValue ? hours : 0;
                    else
                        unchecked
                        {
                            if (newValue)
                                Hours24Value = (hours.Value < 0) ? hours.Value - 12 : hours.Value + 12;
                            else
                                Hours24Value = hours;
                        }
                }
            }
            finally { Interlocked.Decrement(ref _valueChanging); }
            PropertyValidatingEventArgs<int?, (int? Days, int? Minutes, bool IsPm)> args = new(Hours12Value, nameof(Hours12Value), (DaysValue, MinutesValue, newValue),
                Hours12ValidationMessage);
            OnValidateHours12Value(args);
            Hours12ValidationMessage = args.ValidationMessage;
        }

        #endregion
        #region Hours12Value Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="HoursValue"/> dependency property is being validated.
        /// </summary>
        public event EventHandler<PropertyValidatingEventArgs<int?, (int? Days, int? Minutes, bool IsPm)>> ValidateHours12Value;

        /// <summary>
        /// Identifies the <see cref="Hours12Value"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty Hours12ValueProperty = DependencyProperty.Register(nameof(Hours12Value), typeof(int?), typeof(TimeSpanViewModel),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as TimeSpanViewModel)?.OnHours12ValuePropertyChanged((int?)e.OldValue, (int?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public int? Hours12Value { get => (int?)GetValue(Hours12ValueProperty); set => SetValue(Hours12ValueProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Hours12Value"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Hours12Value"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Hours12Value"/> property.</param>
        protected virtual void OnHours12ValuePropertyChanged(int? oldValue, int? newValue)
        {
            PropertyValidatingEventArgs<int?, (int? Days, int? Minutes, bool IsPm)> args = new(newValue, nameof(Hours12Value), (DaysValue, MinutesValue, IsPm));
            if (newValue.HasValue)
            {
                if (newValue.Value < 1 || newValue.Value > 12)
                    args.ValidationMessage = FsInfoCat.Properties.Resources.ErrorMessage_InvalidHours;
                if (!DaysValue.HasValue)
                    DaysValidationMessage = "";
                if (!MinutesValue.HasValue)
                    MinutesValidationMessage = "";
            }
            else if (IsRequired && !(DaysValue.HasValue || MinutesValue.HasValue) && DaysValidationMessage.Length == 0 && MinutesValidationMessage.Length == 0)
                args.ValidationMessage = FsInfoCat.Properties.Resources.ErrorMessage_Required;
            OnValidateHours12Value(args);
            bool isChange = Interlocked.Increment(ref _valueChanging) == 1;
            try
            {
                if (isChange)
                {
                    if (newValue.HasValue)
                    {
                        int h = Math.Abs(newValue.Value);
                        if (h == 12)
                            Hours24Value = IsPm ? newValue : 0;
                        else
                            unchecked
                            {
                                if (IsPm)
                                    Hours24Value = (newValue.Value < 0) ? newValue.Value - 12 : newValue.Value + 12;
                                else
                                    Hours24Value = newValue;
                            }
                    }
                    else
                        Hours24Value = null;
                }
            }
            finally { Interlocked.Decrement(ref _valueChanging); }
            Hours12ValidationMessage = args.ValidationMessage;
        }

        protected virtual void OnValidateHours12Value(PropertyValidatingEventArgs<int?, (int? Days, int? Minutes, bool IsPm)> args) =>
            ValidateHours12Value?.Invoke(this, args);

        #endregion
        #region Hours12ValidationMessage Property Members

        /// <summary>
        /// Identifies the <see cref="Hours12ValidationMessage"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty Hours12ValidationMessageProperty = DependencyProperty.Register(nameof(Hours12ValidationMessage), typeof(string),
            typeof(TimeSpanViewModel), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as TimeSpanViewModel)?.OnHours12ValidationMessagePropertyChanged(e.OldValue as string, e.NewValue as string),
                NormalizedOrEmptyStringCoersion.Default.ToCoerceValueCallback()));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Hours12ValidationMessage { get => GetValue(Hours12ValidationMessageProperty) as string; set => SetValue(Hours12ValidationMessageProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Hours12ValidationMessage"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Hours12ValidationMessage"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Hours12ValidationMessage"/> property.</param>
        protected virtual void OnHours12ValidationMessagePropertyChanged(string oldValue, string newValue)
        {
            try { HoursValidationMessage = string.IsNullOrEmpty(newValue) ? Hours24ValidationMessage : newValue; }
            finally { RaiseDataErrorsChanged(nameof(Hours12Value)); }
        }

        #endregion
        #region HoursValidationMessage Property Members

        private static readonly DependencyPropertyKey HoursValidationMessagePropertyKey = DependencyProperty.RegisterReadOnly(nameof(HoursValidationMessage), typeof(string), typeof(TimeSpanViewModel),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as TimeSpanViewModel)?.OnHoursValidationMessagePropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Identifies the <see cref="HoursValidationMessage"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HoursValidationMessageProperty = HoursValidationMessagePropertyKey.DependencyProperty;

        public string HoursValidationMessage { get => GetValue(HoursValidationMessageProperty) as string; private set => SetValue(HoursValidationMessagePropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="HoursValidationMessage"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="HoursValidationMessage"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="HoursValidationMessage"/> property.</param>
        protected virtual void OnHoursValidationMessagePropertyChanged(string oldValue, string newValue) =>
            UpdateAggregateValidationMessage(MinutesValidationMessage, newValue, DaysValidationMessage, ValidationMessage);

        #endregion
        #region MinutesValue Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="HoursValue"/> dependency property is being validated.
        /// </summary>
        public event EventHandler<PropertyValidatingEventArgs<int?, (int? Days, int? Hours)>> ValidateMinutesValue;

        /// <summary>
        /// Identifies the <see cref="MinutesValue"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinutesValueProperty = DependencyProperty.Register(nameof(MinutesValue), typeof(int?), typeof(TimeSpanViewModel),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as TimeSpanViewModel)?.OnMinutesValuePropertyChanged((int?)e.OldValue, (int?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public int? MinutesValue { get => (int?)GetValue(MinutesValueProperty); set => SetValue(MinutesValueProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="MinutesValue"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="MinutesValue"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="MinutesValue"/> property.</param>
        protected virtual void OnMinutesValuePropertyChanged(int? oldValue, int? newValue)
        {
            PropertyValidatingEventArgs<int?, (int? Days, int? Hours)> args = new(newValue, nameof(MinutesValue), (DaysValue, Hours24Value));
            if (newValue.HasValue)
            {
                if (newValue.Value < 0 || newValue.Value > 59)
                    args.ValidationMessage = FsInfoCat.Properties.Resources.ErrorMessage_InvalidMinutes;
                if (!DaysValue.HasValue)
                    DaysValidationMessage = "";
                if (!Hours12Value.HasValue)
                    Hours12ValidationMessage = "";
                if (!Hours24Value.HasValue)
                    Hours24ValidationMessage = "";
            }
            else if (IsRequired && !(DaysValue.HasValue || Hours24Value.HasValue) && DaysValidationMessage.Length == 0 && HoursValidationMessage.Length == 0)
                args.ValidationMessage = FsInfoCat.Properties.Resources.ErrorMessage_Required;
            OnValidateMinutes(args);
            MinutesValidationMessage = args.ValidationMessage;
            bool ignoreChange = Interlocked.Increment(ref _valueChanging) != 1;
            try
            {
                if (ignoreChange)
                    return;
                if (newValue.HasValue)
                {
                    if (args.ValidationMessage.Length == 0 && DaysValidationMessage.Length == 0 && HoursValidationMessage.Length == 0)
                        Value = new(DaysValue ?? 0, Hours24Value ?? 0, newValue.Value, 0, 0);
                    else
                        Value = null;
                }
                else if (DaysValidationMessage.Length == 0 && HoursValidationMessage.Length == 0)
                    Value = new(DaysValue ?? 0, Hours24Value ?? 0, 0, 0, 0);
                else
                    Value = null;
            }
            finally { Interlocked.Decrement(ref _valueChanging); }
        }

        protected virtual void OnValidateMinutes(PropertyValidatingEventArgs<int?, (int? Days, int? Hours)> args) =>
            ValidateMinutesValue?.Invoke(this, args);

        #endregion
        #region MinutesValidationMessage Property Members

        /// <summary>
        /// Identifies the <see cref="MinutesValidationMessage"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinutesValidationMessageProperty = DependencyProperty.Register(nameof(MinutesValidationMessage), typeof(string),
            typeof(TimeSpanViewModel), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as TimeSpanViewModel)?.OnMinutesValidationMessagePropertyChanged(e.OldValue as string, e.NewValue as string),
                NormalizedOrEmptyStringCoersion.Default.ToCoerceValueCallback()));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string MinutesValidationMessage { get => GetValue(MinutesValidationMessageProperty) as string; set => SetValue(MinutesValidationMessageProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="MinutesValidationMessage"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="MinutesValidationMessage"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="MinutesValidationMessage"/> property.</param>
        protected virtual void OnMinutesValidationMessagePropertyChanged(string oldValue, string newValue)
        {
            try { UpdateAggregateValidationMessage(newValue, HoursValidationMessage, DaysValidationMessage, ValidationMessage); }
            finally { RaiseDataErrorsChanged(nameof(MinutesValue)); }
        }

        #endregion
#pragma warning restore IDE0060 // Remove unused parameter

        private void RaiseDataErrorsChanged(string propertyName) => ErrorsChanged?.Invoke(this, new(propertyName));

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                string minutes = MinutesValidationMessage;
                string hours = HoursValidationMessage;
                string days = DaysValidationMessage;
                string value = ValidationMessage;
                if (!string.IsNullOrEmpty(value))
                    yield return value;
                if (!(string.IsNullOrEmpty(minutes) || value == minutes) && (string.IsNullOrEmpty(hours) || value == hours) && (string.IsNullOrEmpty(days) || value == days))
                {
                    if (!string.IsNullOrEmpty(days))
                        yield return string.Format(FsInfoCat.Properties.Resources.FormatMessage_InvalidDays, days);
                    if (!string.IsNullOrEmpty(hours))
                        yield return string.Format(FsInfoCat.Properties.Resources.FormatMessage_InvalidHours, hours);
                    if (!string.IsNullOrEmpty(minutes))
                        yield return string.Format(FsInfoCat.Properties.Resources.FormatMessage_InvalidMinutes, minutes);
                }
            }
            else
            {
                string message, format;
                switch (propertyName)
                {
                    case nameof(Value):
                        message = ValidationMessage;
                        if (!string.IsNullOrEmpty(message))
                            yield return message;
                        yield break;
                    case nameof(DaysValue):
                        message = DaysValidationMessage;
                        format = FsInfoCat.Properties.Resources.FormatMessage_InvalidDays;
                        break;
                    case nameof(Hours12Value):
                        message = Hours12ValidationMessage;
                        format = FsInfoCat.Properties.Resources.FormatMessage_InvalidHours;
                        break;
                    case nameof(Hours24Value):
                        message = Hours24ValidationMessage;
                        format = FsInfoCat.Properties.Resources.FormatMessage_InvalidHours;
                        break;
                    case nameof(MinutesValue):
                        message = ValidationMessage;
                        format = FsInfoCat.Properties.Resources.FormatMessage_InvalidMinutes;
                        break;
                    default:
                        yield break;
                }
                if (!string.IsNullOrEmpty(message))
                    yield return string.Format(format, message);
            }
        }

        public TimeSpanViewModel(TimeSpan? value = null)
        {
            Value = value;
        }
    }
}
