using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Filter
{
    public class Duration : DependencyObject, IFilter, IComparable<TimeSpan?>
    {
        private readonly DataErrorInfo _errorInfo;

        /// <summary>
        /// Occurs when the value of the <see cref="HasErrors"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler HasErrorsPropertyChanged;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        #region IsExclusive Property Members

        /// <summary>
        /// Identifies the <see cref="IsExclusive"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsExclusiveProperty = DependencyPropertyBuilder<Duration, bool>
            .Register(nameof(IsExclusive))
            .DefaultValue(false)
            .AsReadWrite();

        public bool IsExclusive { get => (bool)GetValue(IsExclusiveProperty); set => SetValue(IsExclusiveProperty, value); }

        #endregion
        #region IncludeNull Property Members

        /// <summary>
        /// Identifies the <see cref="IncludeNull"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IncludeNullProperty = DependencyPropertyBuilder<Duration, bool>
            .Register(nameof(IncludeNull))
            .DefaultValue(false)
            .AsReadWrite();

        public bool IncludeNull { get => (bool)GetValue(IncludeNullProperty); set => SetValue(IncludeNullProperty, value); }

        #endregion
        #region Days Property Members

        private static readonly DependencyPropertyKey DaysPropertyKey = DependencyPropertyBuilder<Duration, int?>
            .Register(nameof(Days))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as Duration)?.OnDaysPropertyChanged(oldValue, newValue))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Days"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DaysProperty = DaysPropertyKey.DependencyProperty;

        public int? Days { get => (int?)GetValue(DaysProperty); private set => SetValue(DaysPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="Days"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Days"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Days"/> property.</param>
        protected virtual void OnDaysPropertyChanged(int? oldValue, int? newValue)
        {
            if (newValue.HasValue && newValue.Value < 0)
                _errorInfo.SetErrors(nameof(Days), "Invalid days value");
            else
                _errorInfo.ClearErrors(nameof(Days));
        }

        #endregion
        #region Hours Property Members

        private static readonly DependencyPropertyKey HoursPropertyKey = DependencyPropertyBuilder<Duration, int?>
            .Register(nameof(Hours))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as Duration)?.OnHoursPropertyChanged(oldValue, newValue))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Hours"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HoursProperty = HoursPropertyKey.DependencyProperty;

        public int? Hours { get => (int?)GetValue(HoursProperty); private set => SetValue(HoursPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="Hours"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Hours"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Hours"/> property.</param>
        protected virtual void OnHoursPropertyChanged(int? oldValue, int? newValue)
        {
            if (newValue.HasValue && (newValue.Value < 0 || newValue.Value > 23))
                _errorInfo.SetErrors(nameof(Hours), "Invalid hours value");
            else
                _errorInfo.ClearErrors(nameof(Hours));
        }

        #endregion
        #region Minutes Property Members

        private static readonly DependencyPropertyKey MinutesPropertyKey = DependencyPropertyBuilder<Duration, int?>
            .Register(nameof(Minutes))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as Duration)?.OnMinutesPropertyChanged(newValue))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Minutes"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinutesProperty = MinutesPropertyKey.DependencyProperty;

        public int? Minutes { get => (int?)GetValue(MinutesProperty); private set => SetValue(MinutesPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="Minutes"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Minutes"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Minutes"/> property.</param>
        protected virtual void OnMinutesPropertyChanged(int? newValue)
        {
            if (newValue.HasValue && (newValue.Value < 0 || newValue.Value > 59))
                _errorInfo.SetErrors(nameof(Minutes), "Invalid minutes value");
            else
                _errorInfo.ClearErrors(nameof(Minutes));
        }

        #endregion
        #region HasErrors Property Members

        private static readonly DependencyPropertyKey HasErrorsPropertyKey = DependencyPropertyBuilder<Duration, bool>
            .Register(nameof(HasErrors))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as Duration)?.RaiseHasErrorsPropertyChanged(e))
            .AsReadOnly();

        internal static bool AreSame(Duration x, Duration y)
        {
            if (x is null)
                return y is null || (y.Days == 0 && y.Hours == 0 && y.Minutes == 0 && !y.IncludeNull);
            if (y is null)
                return x.Days == 0 && y.Hours == 0 && y.Minutes == 0 && !x.IncludeNull;
            return ReferenceEquals(x, y) || (x.Days == y.Days && x.Hours == y.Hours && x.Minutes == y.Minutes && x.IncludeNull == y.IncludeNull);
        }

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

        public Duration()
        {
            _errorInfo = new();
            _errorInfo.ErrorsChanged += ErrorInfo_ErrorsChanged;
        }

        private void ErrorInfo_ErrorsChanged(object sender, DataErrorsChangedEventArgs e) => ErrorsChanged?.Invoke(this, e);

        public IEnumerable GetErrors(string propertyName) => _errorInfo.GetErrors(propertyName);

        public TimeSpan ToTimeSpan() => new(Days ?? 0, Hours ?? 0, Minutes ?? 0, 0, 0);

        public int CompareTo(TimeSpan? other) => (other.HasValue)? ToTimeSpan().CompareTo(other.Value) : IncludeNull ? 0 : 1;
    }
}
