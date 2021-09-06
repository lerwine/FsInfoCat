using System;
using System.Collections;
using System.ComponentModel;
using System.Threading;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class DateTimeViewModel : DependencyObject, INotifyDataErrorInfo
    {
        private int _valueChanging = 0;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        #region HasErrors Property Members

        private static readonly DependencyPropertyKey HasErrorsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(HasErrors), typeof(bool), typeof(DateTimeViewModel),
                new PropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="HasErrors"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HasErrorsProperty = HasErrorsPropertyKey.DependencyProperty;

        public bool HasErrors { get => (bool)GetValue(HasErrorsProperty); private set => SetValue(HasErrorsPropertyKey, value); }

        #endregion
        #region IsRequired Property Members

        /// <summary>
        /// Identifies the <see cref="IsRequired"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsRequiredProperty = DependencyProperty.Register(nameof(IsRequired), typeof(bool), typeof(DateTimeViewModel),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as DateTimeViewModel)?.OnIsRequiredPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool IsRequired { get => (bool)GetValue(IsRequiredProperty); set => SetValue(IsRequiredProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="IsRequired"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="IsRequired"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="IsRequired"/> property.</param>
        protected virtual void OnIsRequiredPropertyChanged(bool oldValue, bool newValue)
        {

            if (!(Value.HasValue || SelectedDate.HasValue))
            {
                if (newValue)
                {
                    ValueValidationMessage = FsInfoCat.Properties.Resources.ErrorMessage_Required;
                    SelectedValidationMessage = FsInfoCat.Properties.Resources.ErrorMessage_Required;
                }
                else
                {
                    ValueValidationMessage = string.Empty;
                    SelectedValidationMessage = string.Empty;
                }
            }
        }

        #endregion
        #region Value Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="Value"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler ValuePropertyChanged;

        public event EventHandler<PropertyValidatingEventArgs<DateTime?, (string SelectedValueMessage, string TimeMessage)>> ValidateValue;

        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(DateTime?), typeof(DateTimeViewModel),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as DateTimeViewModel)?.OnValuePropertyChanged(e),
                    DateTimeCoersion.NormalizedToMinutesLocal.ToCoerceValueCallback()));

        public DateTime? Value { get => (DateTime?)GetValue(ValueProperty); set => SetValue(ValueProperty, value); }

        /// <summary>
        /// Called when the <see cref="PropertyChangedCallback">PropertyChanged</see> event on <see cref="ValueProperty"/> is raised.
        /// </summary>
        /// <param name="args">The Event data that is issued by the event on <see cref="ValueProperty"/> that tracks changes to its effective value.</param>
        protected virtual void OnValuePropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try { OnValuePropertyChanged((DateTime?)args.OldValue, (DateTime?)args.NewValue); }
            finally { ValuePropertyChanged?.Invoke(this, args); }
        }

        /// <summary>
        /// Called when the value of the <see cref="Value"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Value"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Value"/> property.</param>
        protected virtual void OnValuePropertyChanged(DateTime? oldValue, DateTime? newValue)
        {
            bool isChange = Interlocked.Increment(ref _valueChanging) == 1;
            try
            {
                if (isChange)
                {
                    if (newValue.HasValue)
                    {
                        SelectedDate = newValue.Value.Date;
                        Time.Value = newValue.Value.TimeOfDay;
                    }
                    else
                    {
                        SelectedDate = null;
                        Time.Value = null;
                    }
                }
            }
            finally { _ = Interlocked.Decrement(ref _valueChanging); }
            PropertyValidatingEventArgs<DateTime?, (string SelectedValueMessage, string TimeMessage)> args = new(newValue, nameof(Value),
                (SelectedValidationMessage, TimeValidationMesage));
            if (IsRequired && !(newValue.HasValue || SelectedDate.HasValue))
                ValueValidationMessage = FsInfoCat.Properties.Resources.ErrorMessage_Required;
            OnValidateValue(args);
            ValueValidationMessage = args.ValidationMessage;
        }

        protected virtual void OnValidateValue(PropertyValidatingEventArgs<DateTime?, (string SelectedValueMessage, string TimeMessage)> args) =>
            ValidateValue?.Invoke(this, args);

        #endregion
        #region AggregateValidationMessage Property Members

        private static readonly DependencyPropertyKey AggregateValidationMessagePropertyKey = DependencyProperty.RegisterReadOnly(nameof(AggregateValidationMessage), typeof(string), typeof(DateTimeViewModel),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as DateTimeViewModel)?.OnAggregateValidationMessagePropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Identifies the <see cref="AggregateValidationMessage"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AggregateValidationMessageProperty = AggregateValidationMessagePropertyKey.DependencyProperty;

        public string AggregateValidationMessage { get => GetValue(AggregateValidationMessageProperty) as string; private set => SetValue(AggregateValidationMessagePropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="AggregateValidationMessage"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="AggregateValidationMessage"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="AggregateValidationMessage"/> property.</param>
        protected virtual void OnAggregateValidationMessagePropertyChanged(string oldValue, string newValue) { }

        private void UpdateAggregateValidationMessage(string valueMessage, string seletedDateMessage, string timeMessage)
        {
            if (string.IsNullOrEmpty(timeMessage))
                AggregateValidationMessage = string.IsNullOrEmpty(seletedDateMessage) ? valueMessage : @$"{valueMessage}
{string.Format(FsInfoCat.Properties.Resources.Format_InvalidDate, seletedDateMessage)}";
            else if (string.IsNullOrWhiteSpace(seletedDateMessage))
                AggregateValidationMessage = @$"{valueMessage}
{string.Format(FsInfoCat.Properties.Resources.Format_InvalidTime, timeMessage)}";
            else
                AggregateValidationMessage = @$"{valueMessage}
{string.Format(FsInfoCat.Properties.Resources.Format_InvalidDate, seletedDateMessage)}
{string.Format(FsInfoCat.Properties.Resources.Format_InvalidTime, timeMessage)}";
        }

        #endregion
        #region ValueValidationMessage Property Members

        /// <summary>
        /// Identifies the <see cref="ValueValidationMessage"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueValidationMessageProperty = DependencyProperty.Register(nameof(ValueValidationMessage), typeof(string),
            typeof(DateTimeViewModel), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as DateTimeViewModel)?.OnValueValidationMessagePropertyChanged(e.OldValue as string, e.NewValue as string),
                NormalizedOrEmptyStringCoersion.Default.ToCoerceValueCallback()));

        public string ValueValidationMessage { get => GetValue(ValueValidationMessageProperty) as string; set => SetValue(ValueValidationMessageProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ValueValidationMessage"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ValueValidationMessage"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ValueValidationMessage"/> property.</param>
        protected virtual void OnValueValidationMessagePropertyChanged(string oldValue, string newValue)
        {
            try { UpdateAggregateValidationMessage(newValue, SelectedValidationMessage, TimeValidationMesage); }
            finally { RaiseDataErrorsChanged(nameof(Value)); }
        }

        #endregion
        #region SelectedDate Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="Value"/> dependency property is being validated.
        /// </summary>
        public event EventHandler<PropertyValidatingEventArgs<DateTime?, TimeSpan?>> ValidateSelectedDate;

        /// <summary>
        /// Identifies the <see cref="SelectedDate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedDateProperty = DependencyProperty.Register(nameof(SelectedDate), typeof(DateTime?), typeof(DateTimeViewModel),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DateTimeViewModel)?.OnSelectedDatePropertyChanged((DateTime?)e.OldValue, (DateTime?)e.NewValue),
                    DateTimeCoersion.NormalizedToDaysLocal.ToCoerceValueCallback()));

        public DateTime? SelectedDate { get => (DateTime?)GetValue(SelectedDateProperty); set => SetValue(SelectedDateProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="SelectedDate"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="SelectedDate"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="SelectedDate"/> property.</param>
        protected virtual void OnSelectedDatePropertyChanged(DateTime? oldValue, DateTime? newValue)
        {
            PropertyValidatingEventArgs<DateTime?, TimeSpan?> args = new(newValue, nameof(SelectedDate), Time.Value);
            if (IsRequired && !newValue.HasValue)
                ValueValidationMessage = FsInfoCat.Properties.Resources.ErrorMessage_Required;
            OnValidateSelectedDate(args);
            SelectedValidationMessage = args.ValidationMessage;
            bool ignoreChange = Interlocked.Increment(ref _valueChanging) != 1;
            try
            {
                if (ignoreChange)
                    return;
                TimeSpan timeSpan;
                if (newValue.HasValue && args.ValidationMessage.Length == 0 && TimeValidationMesage.Length == 0 && args.Context.HasValue &&
                    (timeSpan = args.Context.Value) >= TimeSpan.Zero)
                {
                    if (timeSpan.Days != 0)
                        Value = newValue.Value.Add(new TimeSpan(0, timeSpan.Hours, timeSpan.Minutes, 0, 0));
                    else
                        Value = newValue.Value.Add(timeSpan);
                }
                else
                    Value = null;
            }
            finally { _ = Interlocked.Decrement(ref _valueChanging); }
        }

        protected virtual void OnValidateSelectedDate(PropertyValidatingEventArgs<DateTime?, TimeSpan?> args) =>
            ValidateSelectedDate?.Invoke(this, args);

        #endregion
        #region SelectedValidationMessage Property Members

        /// <summary>
        /// Identifies the <see cref="SelectedValidationMessage"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedValidationMessageProperty = DependencyProperty.Register(nameof(SelectedValidationMessage), typeof(string),
            typeof(DateTimeViewModel), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as DateTimeViewModel)?.OnSelectedValidationMessagePropertyChanged(e.OldValue as string, e.NewValue as string),
                NormalizedOrEmptyStringCoersion.Default.ToCoerceValueCallback()));

        public string SelectedValidationMessage { get => GetValue(SelectedValidationMessageProperty) as string; set => SetValue(SelectedValidationMessageProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="SelectedValidationMessage"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="SelectedValidationMessage"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="SelectedValidationMessage"/> property.</param>
        protected virtual void OnSelectedValidationMessagePropertyChanged(string oldValue, string newValue)
        {
            try { UpdateAggregateValidationMessage(ValueValidationMessage, newValue, TimeValidationMesage); }
            finally { RaiseDataErrorsChanged(nameof(SelectedDate)); }
        }

        #endregion
        #region Time Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="Value"/> dependency property is being validated.
        /// </summary>
        public event EventHandler<PropertyValidatingEventArgs<TimeSpan?, DateTime?>> ValidateTime;

        private static readonly DependencyPropertyKey TimePropertyKey = DependencyProperty.RegisterReadOnly(nameof(Time), typeof(TimeSpanViewModel), typeof(DateTimeViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Time"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TimeProperty = TimePropertyKey.DependencyProperty;

        public TimeSpanViewModel Time => (TimeSpanViewModel)GetValue(TimeProperty);

        private void Time_ValuePropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            TimeSpan? time = e.NewValue as TimeSpan?;
            if (time.HasValue && time.Value >= TimeSpan.Zero)
            {
                if (time.Value.Days != 0)
                    Value = SelectedDate?.Add(new TimeSpan(0, time.Value.Hours, time.Value.Minutes, 0, 0));
                else
                    Value = SelectedDate?.Add(time.Value);
            }
            else
                Value = null;
        }

        protected virtual void OnValidateTime(PropertyValidatingEventArgs<TimeSpan?, DateTime?> args) => ValidateTime?.Invoke(this, args);

        private void Time_ValidateValue(object sender, PropertyValidatingEventArgs<TimeSpan?, (string DaysMessage, string HoursMessage, string MinutesMessage)> e)
        {
            TimeSpan? timeSpan = e.Value;
            if (timeSpan.HasValue && timeSpan.Value.Days != 0)
                timeSpan = new TimeSpan(0, timeSpan.Value.Hours, timeSpan.Value.Minutes, 0, 0);
            PropertyValidatingEventArgs<TimeSpan?, DateTime?> args = new(timeSpan, nameof(Time), SelectedDate);
            if (timeSpan.HasValue && timeSpan.Value < TimeSpan.Zero)
                args.ValidationMessage = e.ValidationMessage = FsInfoCat.Properties.Resources.ErrorMessage_InvalidTime;
            else if (string.IsNullOrWhiteSpace(e.Context.HoursMessage))
            {
                if (!string.IsNullOrWhiteSpace(e.Context.MinutesMessage))
                    args.ValidationMessage = string.Format(FsInfoCat.Properties.Resources.FormatMessage_InvalidMinutes, e.Context.MinutesMessage);
            }
            else if (string.IsNullOrWhiteSpace(e.Context.MinutesMessage))
                args.ValidationMessage = string.Format(FsInfoCat.Properties.Resources.FormatMessage_InvalidHour, e.Context.HoursMessage);
            else
                args.ValidationMessage = $"{string.Format(FsInfoCat.Properties.Resources.FormatMessage_InvalidHour, e.Context.HoursMessage)}; {string.Format(FsInfoCat.Properties.Resources.FormatMessage_InvalidMinutes, e.Context.MinutesMessage)}";
            OnValidateTime(args);
            e.ValidationMessage = args.ValidationMessage;
        }

        private void Time_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            if (Time.HasErrors)
            {
                string message = Time.ValidationMessage;
                TimeValidationMesage = string.IsNullOrEmpty(message) ? string.Join("; ", Time.GetErrors(null)) : message;
            }
            else
                TimeValidationMesage = "";
        }

        #endregion
        #region TimeValidationMesage Property Members

        private static readonly DependencyPropertyKey TimeValidationMesagePropertyKey = DependencyProperty.RegisterReadOnly(nameof(TimeValidationMesage), typeof(string), typeof(DateTimeViewModel),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DateTimeViewModel)?.OnTimeValidationMesagePropertyChanged(e.OldValue as string, e.NewValue as string),
                NormalizedOrEmptyStringCoersion.Default.ToCoerceValueCallback()));

        /// <summary>
        /// Identifies the <see cref="TimeValidationMesage"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TimeValidationMesageProperty = TimeValidationMesagePropertyKey.DependencyProperty;

        public string TimeValidationMesage { get => GetValue(TimeValidationMesageProperty) as string; private set => SetValue(TimeValidationMesagePropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="TimeValidationMesage"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="TimeValidationMesage"/> property.</param>
        /// <param name="timeMessage">The new value of the <see cref="TimeValidationMesage"/> property.</param>
        protected virtual void OnTimeValidationMesagePropertyChanged(string oldValue, string newValue)
        {
            try { UpdateAggregateValidationMessage(ValueValidationMessage, SelectedValidationMessage, newValue); }
            finally { RaiseDataErrorsChanged(nameof(Time)); }
        }

        #endregion

        public DateTimeViewModel(DateTime? value)
        {
            TimeSpanViewModel timeSpanViewModel = new(value.HasValue ? value.Value.TimeOfDay : null);
            SetValue(TimePropertyKey, timeSpanViewModel);
            timeSpanViewModel.ValuePropertyChanged += Time_ValuePropertyChanged;
            timeSpanViewModel.ValidateValue += Time_ValidateValue;
            timeSpanViewModel.ErrorsChanged += Time_ErrorsChanged;
            Value = value;
        }

        private void RaiseDataErrorsChanged(string propertyName) => ErrorsChanged?.Invoke(this, new(propertyName));

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                string time = TimeValidationMesage;
                string date = SelectedValidationMessage;
                string value = ValueValidationMessage;
                if (!string.IsNullOrEmpty(value))
                    yield return value;
                if (!(string.IsNullOrEmpty(time) || value == time) && (string.IsNullOrEmpty(date) || value == date))
                {
                    if (!string.IsNullOrEmpty(date))
                        yield return string.Format(FsInfoCat.Properties.Resources.Format_InvalidDate, date);
                    if (!string.IsNullOrEmpty(time))
                        yield return string.Format(FsInfoCat.Properties.Resources.Format_InvalidTime, time);
                }
            }
            else
            {
                string message, format;
                switch (propertyName)
                {
                    case nameof(Value):
                        message = ValueValidationMessage;
                        if (!string.IsNullOrEmpty(message))
                            yield return message;
                        yield break;
                    case nameof(SelectedDate):
                        message = SelectedValidationMessage;
                        format = FsInfoCat.Properties.Resources.Format_InvalidDate;
                        break;
                    case nameof(Time):
                        message = TimeValidationMesage;
                        format = FsInfoCat.Properties.Resources.Format_InvalidTime;
                        break;
                    default:
                        yield break;
                }
                if (!string.IsNullOrEmpty(message))
                    yield return string.Format(format, message);
            }
        }
    }
}
