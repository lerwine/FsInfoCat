using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Filter
{
    public interface ITimeValidation : INotifyDataErrorInfo
    {
        event DependencyPropertyChangedEventHandler HasErrorsPropertyChanged;
    }

    public interface ITimeReference : ITimeValidation, IComparable<DateTime?>
    {
    }

    public abstract class TimeReference : DependencyObject, ITimeReference
    {
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        protected DataErrorInfo ErrorInfo { get; }

        #region IncludeNull Property Members

        /// <summary>
        /// Identifies the <see cref="IncludeNull"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IncludeNullProperty = DependencyPropertyBuilder<TimeReference, bool>
            .Register(nameof(IncludeNull))
            .DefaultValue(false)
            .AsReadWrite();

        public bool IncludeNull { get => (bool)GetValue(IncludeNullProperty); set => SetValue(IncludeNullProperty, value); }

        #endregion

        #region HasErrors Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="HasErrors"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler HasErrorsPropertyChanged;

        private static readonly DependencyPropertyKey HasErrorsPropertyKey = DependencyPropertyBuilder<TimeReference, bool>
            .Register(nameof(HasErrors))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as TimeReference)?.RaiseHasErrorsPropertyChanged(e))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="HasErrors"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HasErrorsProperty = HasErrorsPropertyKey.DependencyProperty;

        public bool HasErrors { get => (bool)GetValue(HasErrorsProperty); private set => SetValue(HasErrorsPropertyKey, value); }

        /// <summary>
        /// Called when the <see cref="PropertyChangedCallback">PropertyChanged</see> event on <see cref="HasErrorsProperty"/> is raised.
        /// </summary>
        /// <param name="args">The Event data that is issued by the event on <see cref="HasErrorsProperty"/> that tracks changes to its effective value.</param>
        protected void RaiseHasErrorsPropertyChanged(DependencyPropertyChangedEventArgs args) => HasErrorsPropertyChanged?.Invoke(this, args);

        #endregion

        protected TimeReference()
        {
            (ErrorInfo = new()).ErrorsChanged += TimeReference_ErrorsChanged;
        }

        protected abstract int CompareTo(DateTime other);

        public int CompareTo(DateTime? other) => other.HasValue ? CompareTo(other.Value) : IncludeNull ? 0 : 1;

        private void TimeReference_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            HasErrors = ErrorInfo.HasErrors;
            ErrorsChanged?.Invoke(this, e);
        }

        public IEnumerable GetErrors(string propertyName) => ErrorInfo.GetErrors(propertyName);
    }

    public interface IAbsoluteTimeReference : ITimeReference
    {
        DateTime Value { get; }
    }

    public interface IRelativeTimeReference : ITimeReference
    {
        int Days { get; }
        int Hours { get; }
    }

    public interface IRelativeWeekReference : IRelativeTimeReference
    {
        int Weeks { get; }
    }

    public interface IRelativeMonthReference : IRelativeTimeReference
    {
        int Years { get; }
        int Months { get; }
    }

    public interface ITimeRange : ITimeValidation
    {
        TimeReference Start { get; }

        TimeReference End { get; }
    }

    public interface IHistoricalTimeRange : ITimeRange
    {
        new Historical.HistoricalTimeReference Start { get; }

        new Historical.HistoricalTimeReference End { get; }
    }

    public interface IScheduleTimeRange : ITimeRange
    {
        new Scheduled.SchedulableTimeReference Start { get; }

        new Scheduled.SchedulableTimeReference End { get; }
    }

    public abstract class TimeRange<T> : DependencyObject, ITimeRange
        where T : TimeReference
    {
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        protected DataErrorInfo ErrorInfo { get; }

        #region Start Property Members

        /// <summary>
        /// Identifies the <see cref="Start"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StartProperty = DependencyPropertyBuilder<TimeRange<T>, T>
            .Register(nameof(Start))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as TimeRange<T>)?.OnStartPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public T Start { get => (T)GetValue(StartProperty); set => SetValue(StartProperty, value); }

        TimeReference ITimeRange.Start => Start;

        /// <summary>
        /// Called when the value of the <see cref="Start"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Start"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Start"/> property.</param>
        protected virtual void OnStartPropertyChanged(T oldValue, T newValue)
        {
            if (oldValue is not null)
                oldValue.ErrorsChanged -= RangeStart_ErrorsChanged;
            if (newValue is not null)
            {
                newValue.ErrorsChanged += RangeStart_ErrorsChanged;
                if (HasErrors)
                    HasErrors = newValue.HasErrors || (Start?.HasErrors ?? false);
                else
                    HasErrors = newValue.HasErrors;
            }
        }

        private void RangeStart_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            IEnumerable enumerable = Start.GetErrors(null);
            if (enumerable is string s && (s = s.NullIfWhiteSpaceOrNormalized()) is not null)
                ErrorInfo.SetError(s, nameof(Start));
            else if (!enumerable.OfType<string>().AsWsNormalizedOrEmptyValues().Where(s => s.Length > 0).Any())
                ErrorInfo.ClearErrors(nameof(Start));
            else
                ErrorInfo.SetErrors(nameof(Start), enumerable.OfType<string>());
        }

        #endregion
        #region End Property Members

        /// <summary>
        /// Identifies the <see cref="End"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EndProperty = DependencyPropertyBuilder<TimeRange<T>, T>
            .Register(nameof(End))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as TimeRange<T>)?.OnEndPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public T End { get => (T)GetValue(EndProperty); set => SetValue(EndProperty, value); }

        TimeReference ITimeRange.End => End;

        /// <summary>
        /// Called when the value of the <see cref="End"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="End"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="End"/> property.</param>
        protected virtual void OnEndPropertyChanged(T oldValue, T newValue)
        {
            if (oldValue is not null)
                oldValue.ErrorsChanged -= RangeEnd_ErrorsChanged;
            if (newValue is not null)
            {
                newValue.ErrorsChanged += RangeEnd_ErrorsChanged;
                if (HasErrors)
                    HasErrors = newValue.HasErrors || (Start?.HasErrors ?? false);
                else
                    HasErrors = newValue.HasErrors;
            }
        }

        private void RangeEnd_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            IEnumerable enumerable = End.GetErrors(null);
            if (enumerable is string s && (s = s.NullIfWhiteSpaceOrNormalized()) is not null)
                ErrorInfo.SetError(s, nameof(End));
            else if (!enumerable.OfType<string>().AsWsNormalizedOrEmptyValues().Where(s => s.Length > 0).Any())
                ErrorInfo.ClearErrors(nameof(End));
            else
                ErrorInfo.SetErrors(nameof(End), enumerable.OfType<string>());
        }

        #endregion
        #region HasErrors Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="HasErrors"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler HasErrorsPropertyChanged;

        private static readonly DependencyPropertyKey HasErrorsPropertyKey = DependencyPropertyBuilder<TimeRange<T>, bool>
            .Register(nameof(HasErrors))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as TimeRange<T>)?.RaiseHasErrorsPropertyChanged(e))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="HasErrors"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HasErrorsProperty = HasErrorsPropertyKey.DependencyProperty;

        public bool HasErrors { get => (bool)GetValue(HasErrorsProperty); private set => SetValue(HasErrorsPropertyKey, value); }

        /// <summary>
        /// Called when the <see cref="PropertyChangedCallback">PropertyChanged</see> event on <see cref="HasErrorsProperty"/> is raised.
        /// </summary>
        /// <param name="args">The Event data that is issued by the event on <see cref="HasErrorsProperty"/> that tracks changes to its effective value.</param>
        protected void RaiseHasErrorsPropertyChanged(DependencyPropertyChangedEventArgs args) => HasErrorsPropertyChanged?.Invoke(this, args);

        #endregion

        public bool IsValid(DateTime? value)
        {
            T time = Start;
            return (time is null || time.CompareTo(value) <= 0) && ((time = End) is null || time.CompareTo(value) >= 0);
        }

        public IEnumerable GetErrors(string propertyName) => ErrorInfo.GetErrors(propertyName);
    }
}
