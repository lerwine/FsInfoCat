using System;
using System.Collections;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Filter
{
    public class DurationRange : DependencyObject, IFilter
    {
        private readonly DataErrorInfo _errorInfo;

        /// <summary>
        /// Occurs when the value of the <see cref="HasErrors"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler HasErrorsPropertyChanged;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        #region Min Property Members

        /// <summary>
        /// Identifies the <see cref="Min"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinProperty = DependencyPropertyBuilder<DurationRange, Duration>
            .Register(nameof(Min))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as DurationRange)?.OnMinPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public Duration Min { get => (Duration)GetValue(MinProperty); set => SetValue(MinProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Min"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Min"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Min"/> property.</param>
        protected virtual void OnMinPropertyChanged(Duration oldValue, Duration newValue)
        {
            // TODO: Implement OnMinPropertyChanged Logic
        }

        #endregion
        #region Max Property Members

        /// <summary>
        /// Identifies the <see cref="Max"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxProperty = DependencyPropertyBuilder<DurationRange, Duration>
            .Register(nameof(Max))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as DurationRange)?.OnMaxPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public Duration Max { get => (Duration)GetValue(MaxProperty); set => SetValue(MaxProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Max"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Max"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Max"/> property.</param>
        protected virtual void OnMaxPropertyChanged(Duration oldValue, Duration newValue)
        {
            // TODO: Implement OnMaxPropertyChanged Logic
        }

        #endregion
        #region HasErrors Property Members

        private static readonly DependencyPropertyKey HasErrorsPropertyKey = DependencyPropertyBuilder<DurationRange, bool>
            .Register(nameof(HasErrors))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as DurationRange)?.RaiseHasErrorsPropertyChanged(e))
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

        public bool IsInRange(long? seconds) => IsInRange(seconds.HasValue ? TimeSpan.FromSeconds(seconds.Value) : null);

        public bool IsInRange(TimeSpan? value)
        {
            Duration duration = Min;
            if (value.HasValue)
                return (duration is null || (duration.IsExclusive ? duration.CompareTo(value) < 0 : duration.CompareTo(value) <= 0)) && ((duration = Max) is null || (duration.IsExclusive ? duration.CompareTo(value) > 0 : duration.CompareTo(value) >= 0));
            return (duration is null) ? ((duration = Max) is null || duration.IncludeNull) : (duration.IncludeNull || ((duration = Max) is not null && duration.IncludeNull));
        }

        public DurationRange()
        {
            _errorInfo = new();
            _errorInfo.ErrorsChanged += ErrorInfo_ErrorsChanged;
        }
        public BinaryExpression GetExpression<TEntity>(MemberExpression memberExpression)
        {
            Duration min = Min;
            Duration max = Max;

            ConstantExpression endExp;
            BinaryExpression binaryExpression;
            ConstantExpression nc = System.Linq.Expressions.Expression.Constant(null);
            if (min is null)
            {
                if (max is null)
                    return null;
                endExp = System.Linq.Expressions.Expression.Constant(Convert.ToInt64(max.ToTimeSpan().TotalSeconds));
                binaryExpression = max.IsExclusive ? System.Linq.Expressions.Expression.LessThan(memberExpression, endExp) : System.Linq.Expressions.Expression.LessThanOrEqual(memberExpression, endExp);
                return max.IncludeNull ? System.Linq.Expressions.Expression.OrElse(System.Linq.Expressions.Expression.Equal(memberExpression, nc), binaryExpression) :
                    System.Linq.Expressions.Expression.AndAlso(System.Linq.Expressions.Expression.NotEqual(memberExpression, nc), binaryExpression);
            }
            ConstantExpression startExp = System.Linq.Expressions.Expression.Constant(Convert.ToInt64(min.ToTimeSpan().TotalSeconds));
            binaryExpression = min.IsExclusive ? System.Linq.Expressions.Expression.GreaterThan(memberExpression, startExp) : System.Linq.Expressions.Expression.GreaterThanOrEqual(memberExpression, startExp);
            if (max is null)
                return min.IncludeNull ? System.Linq.Expressions.Expression.OrElse(System.Linq.Expressions.Expression.Equal(memberExpression, nc), binaryExpression) :
                    System.Linq.Expressions.Expression.AndAlso(System.Linq.Expressions.Expression.NotEqual(memberExpression, nc), binaryExpression);
            endExp = System.Linq.Expressions.Expression.Constant(Convert.ToInt64(max.ToTimeSpan().TotalSeconds));
            binaryExpression = System.Linq.Expressions.Expression.AndAlso(binaryExpression, max.IsExclusive ? System.Linq.Expressions.Expression.LessThan(memberExpression, endExp) :
                System.Linq.Expressions.Expression.LessThanOrEqual(memberExpression, endExp));
            return (min.IncludeNull || max.IncludeNull) ? System.Linq.Expressions.Expression.OrElse(System.Linq.Expressions.Expression.Equal(memberExpression, nc), binaryExpression) :
                    System.Linq.Expressions.Expression.AndAlso(System.Linq.Expressions.Expression.NotEqual(memberExpression, nc), binaryExpression);
        }

        public static (TimeSpan? MinValue, bool MinIsExclusive, TimeSpan? MaxValue, bool MaxIsExclusive, bool IncludesNull) GetRangeValues(DurationRange range)
        {
            if (range is not null)
            {
                Duration start = range.Min;
                Duration end = range.Max;
                if (start is null)
                {
                    if (end is not null)
                        return (null, false, end.ToTimeSpan(), end.IsExclusive, end.IncludeNull);
                }
                else
                {
                    if (end is null)
                        return (start.ToTimeSpan(), start.IsExclusive, null, false, start.IncludeNull);
                    return (start.ToTimeSpan(), start.IsExclusive, end.ToTimeSpan(), end.IsExclusive, start.IncludeNull || end.IncludeNull);
                }
            }
            return (null, false, null, false, true);
        }

        private void ErrorInfo_ErrorsChanged(object sender, DataErrorsChangedEventArgs e) => ErrorsChanged?.Invoke(this, e);

        public IEnumerable GetErrors(string propertyName) => _errorInfo.GetErrors(propertyName);

        internal static bool AreSame(DurationRange x, DurationRange y) => (x is null) ? (y is null || Duration.AreSame(x.Min, null)) :
            (ReferenceEquals(x, y) || Duration.AreSame(x.Min, y?.Min) && Duration.AreSame(x.Max, y?.Max));
    }
}
