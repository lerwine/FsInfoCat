using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;

namespace FsInfoCat.ExpressionFilter
{
    // TODO: Document TimeRange class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public abstract class TimeRange<T> : NotifyDataErrorInfo, ITimeRange
        where T : TimeReference
    {
        private readonly IPropertyChangeTracker<T> _start;
        private readonly IPropertyChangeTracker<T> _end;

        public T Start { get => _start.GetValue(); set => _start.SetValue(value); }

        TimeReference ITimeRange.Start => Start;

        public T End { get => _end.GetValue(); set => _end.SetValue(value); }

        TimeReference ITimeRange.End => End;

        public TimeRange()
        {
            _start = AddChangeTracker<T>(nameof(Start), null);
            _end = AddChangeTracker<T>(nameof(End), null);
        }

        public static (DateTime? StartTime, bool StartExclusive, DateTime? EndTime, bool EndExclusive, bool IncludesNull) GetRangeValues(TimeRange<T> range)
        {
            if (range is not null)
            {
                T start = range.Start;
                T end = range.End;
                if (start is null)
                {
                    if (end is not null)
                        return (null, false, end.ToDateTime(), end.IsExclusive, end.IncludeNull);
                }
                else
                {
                    if (end is null)
                        return (start.ToDateTime(), start.IsExclusive, null, false, start.IncludeNull);
                    return (start.ToDateTime(), start.IsExclusive, end.ToDateTime(), end.IsExclusive, start.IncludeNull || end.IncludeNull);
                }
            }
            return (null, false, null, false, true);
        }

        public bool IsInRange(DateTime? value)
        {
            T time = Start;
            if (value.HasValue)
                return (time is null || (time.IsExclusive ? time.CompareTo(value) < 0 : time.CompareTo(value) <= 0)) && ((time = End) is null || (time.IsExclusive ? time.CompareTo(value) > 0 : time.CompareTo(value) >= 0));
            return (time is null) ? ((time = End) is null || time.IncludeNull) : (time.IncludeNull || ((time = End) is not null && time.IncludeNull));
        }

        public BinaryExpression GetExpression(MemberExpression memberExpression)
        {
            T start = Start;
            T end = End;

            ConstantExpression endExp;
            BinaryExpression binaryExpression;
            ConstantExpression nc = System.Linq.Expressions.Expression.Constant(null);
            if (start is null)
            {
                if (end is null)
                    return null;
                endExp = System.Linq.Expressions.Expression.Constant(end.ToDateTime());
                binaryExpression = end.IsExclusive ? System.Linq.Expressions.Expression.LessThan(memberExpression, endExp) : System.Linq.Expressions.Expression.LessThanOrEqual(memberExpression, endExp);
                return end.IncludeNull ? System.Linq.Expressions.Expression.OrElse(System.Linq.Expressions.Expression.Equal(memberExpression, nc), binaryExpression) :
                    System.Linq.Expressions.Expression.AndAlso(System.Linq.Expressions.Expression.NotEqual(memberExpression, nc), binaryExpression);
            }
            ConstantExpression startExp = System.Linq.Expressions.Expression.Constant(start.ToDateTime());
            binaryExpression = start.IsExclusive ? System.Linq.Expressions.Expression.GreaterThan(memberExpression, startExp) : System.Linq.Expressions.Expression.GreaterThanOrEqual(memberExpression, startExp);
            if (end is null)
                return start.IncludeNull ? System.Linq.Expressions.Expression.OrElse(System.Linq.Expressions.Expression.Equal(memberExpression, nc), binaryExpression) :
                    System.Linq.Expressions.Expression.AndAlso(System.Linq.Expressions.Expression.NotEqual(memberExpression, nc), binaryExpression);
            endExp = System.Linq.Expressions.Expression.Constant(end.ToDateTime());
            binaryExpression = System.Linq.Expressions.Expression.AndAlso(binaryExpression, end.IsExclusive ? System.Linq.Expressions.Expression.LessThan(memberExpression, endExp) :
                System.Linq.Expressions.Expression.LessThanOrEqual(memberExpression, endExp));
            return (start.IncludeNull || end.IncludeNull) ? System.Linq.Expressions.Expression.OrElse(System.Linq.Expressions.Expression.Equal(memberExpression, nc), binaryExpression) :
                    System.Linq.Expressions.Expression.AndAlso(System.Linq.Expressions.Expression.NotEqual(memberExpression, nc), binaryExpression);
        }
    }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
}
