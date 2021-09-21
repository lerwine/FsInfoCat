using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class DateTimeViewModel : OptionalValueViewModel<DateTime>
    {
        private DateTime? _pendingResultValueChange;

        #region Date Property Members

        private DateTime? _pendingDateChange;

        private static readonly DependencyPropertyKey DatePropertyKey = DependencyPropertyBuilder<DateTimeViewModel, OptionalValueViewModel<DateTime>>
            .Register(nameof(Date))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Date"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DateProperty = DatePropertyKey.DependencyProperty;

        public OptionalValueViewModel<DateTime> Date { get => (OptionalValueViewModel<DateTime>)GetValue(DateProperty); private set => SetValue(DatePropertyKey, value); }

        private void Date_ResultValuePropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            DateTime? newValue = e.NewValue as DateTime?;
            if (_pendingDateChange == newValue)
                return;
            _pendingDateChange = newValue;
            TimeSpan? time = Time.ResultValue;
            _pendingResultValueChange = (newValue.HasValue && time.HasValue && !ForceNullResult) ? newValue.Value.Date.Add(time.Value) : null;
            InputValue = _pendingResultValueChange;
        }

        private void Date_HasErrorsPropertyChanged(object sender, DependencyPropertyChangedEventArgs e) => HasComponentValueErrors = (bool)e.NewValue || Time.HasErrors;

        #endregion
        #region Time Property Members

        private TimeSpan? _pendingTimeChange;

        private static readonly DependencyPropertyKey TimePropertyKey = DependencyPropertyBuilder<DateTimeViewModel, TimeOfDayViewModel>
            .Register(nameof(Time))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Time"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TimeProperty = TimePropertyKey.DependencyProperty;

        public TimeOfDayViewModel Time { get => (TimeOfDayViewModel)GetValue(TimeProperty); private set => SetValue(TimePropertyKey, value); }

        private void Time_ValidateInputValue(object sender, PropertyValidatingEventArgs<TimeSpan> e)
        {
            // TODO: Implement Time_ValidateInputValue
            MessageBox.Show("You  have invoked a command which has not yet been implemented.", "Not Implemented", MessageBoxButton.OK, MessageBoxImage.Hand);
        }

        private void Time_ResultValuePropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            TimeSpan? newValue = e.NewValue as TimeSpan?;
            if (_pendingTimeChange == newValue)
                return;
            _pendingTimeChange = newValue;
            DateTime? date = Date.ResultValue;
            _pendingResultValueChange = (newValue.HasValue && date.HasValue && !ForceNullResult) ? date.Value.Date.Add(newValue.Value) : null;
            InputValue = _pendingResultValueChange;
        }

        private void Time_HasErrorsPropertyChanged(object sender, DependencyPropertyChangedEventArgs e) => HasComponentValueErrors = (bool)e.NewValue || Date.HasErrors;

        #endregion
        #region HasComponentValueErrors Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="HasComponentValueErrors"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler HasComponentValueErrorsPropertyChanged;

        private static readonly DependencyPropertyKey HasComponentValueErrorsPropertyKey = DependencyPropertyBuilder<DateTimeViewModel, bool>
            .Register(nameof(HasComponentValueErrors))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as DateTimeViewModel)?.RaiseHasComponentValueErrorsPropertyChanged(e))
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
        public DateTimeViewModel()
        {
            OptionalValueViewModel<DateTime> date = new();
            TimeOfDayViewModel time = new();
            SetValue(DatePropertyKey, date);
            SetValue(TimePropertyKey, time);
            date.ResultValuePropertyChanged += Date_ResultValuePropertyChanged;
            date.HasErrorsPropertyChanged += Date_HasErrorsPropertyChanged;
            time.ValidateInputValue += Time_ValidateInputValue;
            time.ResultValuePropertyChanged += Time_ResultValuePropertyChanged;
            time.HasErrorsPropertyChanged += Time_HasErrorsPropertyChanged;
        }

        protected override void OnResultValuePropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            base.OnResultValuePropertyChanged(args);
            DateTime? newValue = args.NewValue as DateTime?;
            if (newValue == _pendingResultValueChange)
                return;
            _pendingResultValueChange = newValue;
            DateTime? pendingDateChange = newValue?.Date;
            TimeSpan? pendingTimeChange = newValue?.TimeOfDay;
            _pendingDateChange = pendingDateChange;
            _pendingTimeChange = pendingTimeChange;
            _pendingResultValueChange = newValue;
            Date.InputValue = pendingDateChange;
            if (_pendingTimeChange == pendingTimeChange)
                Time.InputValue = pendingTimeChange;
        }
    }
}
