using System;
using System.Collections;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows;

namespace FsInfoCat.ExpressionFilter
{
    public class DurationRange : NotifyDataErrorInfo, IFilter
    {
        private readonly IPropertyChangeTracker<Duration> _min;
        private readonly IPropertyChangeTracker<Duration> _max;

        public Duration Min { get => _min.GetValue(); set => _min.SetValue(value); }

        public Duration Max { get => _max.GetValue(); set => _max.SetValue(value); }

        public DurationRange()
        {
            _min = AddChangeTracker<Duration>(nameof(Min), null);
            _max = AddChangeTracker<Duration>(nameof(Max), null);
        }

        public bool IsInRange(long? seconds) => IsInRange(seconds.HasValue ? TimeSpan.FromSeconds(seconds.Value) : null);

        public bool IsInRange(TimeSpan? value)
        {
            Duration duration = Min;
            if (value.HasValue)
                return (duration is null || (duration.IsExclusive ? duration.CompareTo(value) < 0 : duration.CompareTo(value) <= 0)) && ((duration = Max) is null || (duration.IsExclusive ? duration.CompareTo(value) > 0 : duration.CompareTo(value) >= 0));
            return (duration is null) ? ((duration = Max) is null || duration.IncludeNull) : (duration.IncludeNull || ((duration = Max) is not null && duration.IncludeNull));
        }

        public BinaryExpression GetExpression(MemberExpression memberExpression)
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

        internal static bool AreSame(DurationRange x, DurationRange y) => (x is null) ? (y is null || Duration.AreSame(y.Min, null)) :
            (ReferenceEquals(x, y) || Duration.AreSame(x.Min, y?.Min) && Duration.AreSame(x.Max, y?.Max));
    }
}
