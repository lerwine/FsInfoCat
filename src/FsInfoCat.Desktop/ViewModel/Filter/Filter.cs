using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using LinqExpression = System.Linq.Expressions.Expression;

namespace FsInfoCat.Desktop.ViewModel.Filter
{
    public static class Filter
    {
        public const ushort ComparisonFlag_EqualTo = 0b0000_0001;
        public const ushort ComparisonFlag_LessThan = 0b0000_0010;
        public const ushort ComparisonFlag_GreaterThan = 0b0000_0100;
        public const ushort ComparisonFlag_StartsWith = 0b0000_1000;
        public const ushort ComparisonFlag_EndsWith = 0b0001_0000;
        public const ushort ComparisonFlag_Contains = 0b0010_0000;
        public const ushort ComparisonFlag_Negate = 0b0100_0000;
        public const ushort ComparisonFlag_NotNull = 0b1000_0000;
        public const ushort ComparisonFlags_Comparable = ComparisonFlag_EqualTo | ComparisonFlag_LessThan | ComparisonFlag_GreaterThan;
        public const ushort ComparisonFlags_String = ComparisonFlag_EqualTo | ComparisonFlag_StartsWith | ComparisonFlag_EndsWith | ComparisonFlag_Contains | ComparisonFlag_Negate;
        public const ushort ComparisonValue_NotNullOrEqualTo = ComparisonFlag_NotNull; // 0b1000_0000
        public const ushort ComparisonValue_NotNullAndEqualTo = ComparisonFlag_EqualTo | ComparisonFlag_NotNull; // 0b1000_0001
        public const ushort ComparisonValue_NotNullAndLessThan = ComparisonFlag_LessThan | ComparisonFlag_NotNull; // 0b1000_0010
        public const ushort ComparisonValue_NotNullAndLessThanOrEqualTo = ComparisonValue_NotNullAndLessThan | ComparisonValue_NotNullAndEqualTo; // 0b1000_0011
        public const ushort ComparisonValue_NotNullAndGreaterThan = ComparisonFlag_GreaterThan | ComparisonFlag_NotNull; // 0b1000_0100
        public const ushort ComparisonValue_NotNullAndGreaterThanOrEqualTo = ComparisonValue_NotNullAndGreaterThan | ComparisonValue_NotNullAndEqualTo; // 0b1000_0101
        public const ushort ComparisonValue_NotNullAndStartsWith = ComparisonFlag_StartsWith | ComparisonFlag_NotNull; // 0b1000_1000
        public const ushort ComparisonValue_NotNullAndEndsWith = ComparisonFlag_EndsWith | ComparisonFlag_NotNull; // 0b1001_0000
        public const ushort ComparisonValue_NotNullAndStartsOrEndsWith = ComparisonValue_NotNullAndStartsWith | ComparisonValue_NotNullAndEndsWith; // 0b0000_1000 | 0b0001_0000
        public const ushort ComparisonValue_NotNullAndContains = ComparisonFlag_Contains | ComparisonValue_NotNullAndStartsOrEndsWith; // 0b1011_1000
        public const ushort ComparisonValue_NullOrNotEqualTo = 0b0000_0000;
        public const ushort ComparisonValue_NotNullOrStartsWith = ComparisonValue_NotNullAndStartsWith | ComparisonFlag_Negate; // 0b1100_1000
        public const ushort ComparisonValue_NotNullOrEndsWith = ComparisonValue_NotNullAndEndsWith | ComparisonFlag_Negate; // 0b1101_0000
        public const ushort ComparisonValue_NotNullStartsOrEndsWith = ComparisonValue_NotNullAndStartsOrEndsWith | ComparisonFlag_Negate; // 0b1101_1000
        public const ushort ComparisonValue_NotNullOrContains = ComparisonValue_NotNullAndContains | ComparisonFlag_Negate; // 0b1111_1000
        public const ushort ComparisonValue_NullOrEqualTo = ComparisonFlag_EqualTo; // 0b0000_0001
        public const ushort ComparisonValue_NullOrLessThan = ComparisonFlag_LessThan; // 0b0000_0010
        public const ushort ComparisonValue_NullLessThanOrEqualTo = ComparisonValue_NullOrEqualTo | ComparisonValue_NullOrLessThan; // 0b0000_0011
        public const ushort ComparisonValue_NullOrGreaterThan = ComparisonFlag_GreaterThan; // 0b0000_0100
        public const ushort ComparisonValue_NullGreaterThanOrEqualTo = ComparisonValue_NullOrEqualTo | ComparisonValue_NullOrGreaterThan; // 0b0000_0101
        public const ushort ComparisonValue_NullOrStartsWith = ComparisonFlag_StartsWith; // 0b0000_1000
        public const ushort ComparisonValue_NullOrEndsWith = ComparisonFlag_EndsWith; // 0b0001_0000
        public const ushort ComparisonValue_NullStartsOrEndsWith = ComparisonValue_NullOrStartsWith | ComparisonValue_NullOrEndsWith; // 0b0000_1000 | 0b0001_0000
        public const ushort ComparisonValue_NullOrContains = ComparisonFlag_Contains | ComparisonValue_NullStartsOrEndsWith; // 0b0011_1000
        public const ushort ComparisonValue_NullOrDoesNotStartWith = ComparisonValue_NullOrStartsWith | ComparisonFlag_Negate; // 0b0100_1000
        public const ushort ComparisonValue_NullOrDoesNotEndWith = ComparisonValue_NullOrEndsWith | ComparisonFlag_Negate; // 0b0101_0000
        public const ushort ComparisonValue_NullOrDoesNotStartOrEndWith = ComparisonValue_NullOrDoesNotStartWith | ComparisonValue_NullOrDoesNotEndWith; // 0b0101_1000
        public const ushort ComparisonValue_NullOrDoesNotContain = ComparisonValue_NullOrContains | ComparisonValue_NullOrDoesNotStartOrEndWith; // 0b0111_1000
    }
    public abstract class Filter<TEntity> : DependencyObject, IFilter
        where TEntity : class
    {
        protected DataErrorInfo ErrorInfo { get; } = new();

        /// <summary>
        /// Occurs when the value of the <see cref="HasErrors"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler HasErrorsPropertyChanged;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        #region HasErrors Property Members

        private static readonly DependencyPropertyKey HasErrorsPropertyKey = DependencyPropertyBuilder<Filter<TEntity>, bool>
            .Register(nameof(HasErrors))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as Filter<TEntity>)?.RaiseHasErrorsPropertyChanged(e))
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

        public Filter()
        {
            ErrorInfo.ErrorsChanged += ErrorInfo_ErrorsChanged;
        }

        protected static MemberExpression CreateMemberExpressions<T>([DisallowNull] ParameterExpression parameterExpression, [DisallowNull] string propertyName, [DisallowNull] T value, out ConstantExpression constantExpression)
        {
            constantExpression = LinqExpression.Constant(value);
            return LinqExpression.Property(parameterExpression, propertyName);
        }

        protected static MemberExpression CreateMemberExpressions<T>([DisallowNull] ParameterExpression parameterExpression, [DisallowNull] string propertyName, T value, out MemberExpression property, out ConstantExpression constantExpression)
            where T : struct
        {
            property = CreateMemberExpressions(parameterExpression, propertyName, value, out constantExpression);
            return LinqExpression.Property(property, nameof(Nullable<T>.Value)); // parameter.{propertyName}.Value
        }

        protected static BinaryExpression CreateEqualityExpression([DisallowNull] MemberExpression memberExpression, ValueEqualityOperator @operator, [DisallowNull] ConstantExpression constantExpression) =>
            (@operator == ValueEqualityOperator.EqualTo) ? LinqExpression.Equal(memberExpression, constantExpression) : LinqExpression.NotEqual(memberExpression, constantExpression);

        protected static BinaryExpression CreateComparisonExpression([DisallowNull] MemberExpression memberExpression, ValueComparisonOperator @operator, [DisallowNull] ConstantExpression constantExpression) =>
            @operator.HasFlag(ValueComparisonOperator.EqualTo) ?
                (@operator.HasFlag(ValueComparisonOperator.GreaterThan) ?
                    LinqExpression.GreaterThanOrEqual(memberExpression, constantExpression) // parameter.{propertyName}.Value >= dateTime
                    : (@operator.HasFlag(ValueComparisonOperator.LessThan) ?
                        LinqExpression.LessThanOrEqual(memberExpression, constantExpression) // parameter.{propertyName}.Value <= dateTime
                        : LinqExpression.Equal(memberExpression, constantExpression) // parameter.{propertyName}.Value == dateTime
                    )
                )
                : (@operator.HasFlag(ValueComparisonOperator.GreaterThan) ?
                    LinqExpression.GreaterThan(memberExpression, constantExpression) // parameter.{propertyName}.Value > dateTime
                    : (@operator.HasFlag(ValueComparisonOperator.LessThan) ?
                        LinqExpression.LessThan(memberExpression, constantExpression) // parameter.{propertyName}.Value < dateTime
                        : LinqExpression.NotEqual(memberExpression, constantExpression) // parameter.{propertyName}.Value != dateTime
                    )
                );

        protected static BinaryExpression CreateComparisonExpression([DisallowNull] MemberExpression memberExpression, ObjectComparisonOperator @operator, [DisallowNull] ConstantExpression constantExpression) =>
            @operator.HasFlag(ObjectComparisonOperator.NotNullOrEqualTo) ?
            LinqExpression.AndAlso(LinqExpression.NotEqual(memberExpression, LinqExpression.Constant(null)), CreateComparisonExpression(memberExpression, (ValueComparisonOperator)((uint)@operator & Filter.ComparisonFlags_Comparable), constantExpression))
            : LinqExpression.OrElse(LinqExpression.Equal(memberExpression, LinqExpression.Constant(null)), CreateComparisonExpression(memberExpression, (ValueComparisonOperator)((uint)@operator & Filter.ComparisonFlags_Comparable), constantExpression));

        protected static BinaryExpression CreateExpressionForNullable<T>([DisallowNull] ParameterExpression parameterExpression, [DisallowNull] string propertyName, ObjectEqualityOperator @operator, T value)
            where T : struct
        {
            MemberExpression memberExpression;
            ConstantExpression valueConstant;
            BinaryExpression expression = @operator switch
            {
                ObjectEqualityOperator.NotEqualTo or ObjectEqualityOperator.NotNullOrEqualTo =>
                    LinqExpression.NotEqual(CreateMemberExpressions(parameterExpression, propertyName, value, out memberExpression, out valueConstant), valueConstant),
                _ => LinqExpression.Equal(CreateMemberExpressions(parameterExpression, propertyName, value, out memberExpression, out valueConstant), valueConstant),
            };
            return @operator.HasFlag(ObjectEqualityOperator.NotNullOrEqualTo) ?
                LinqExpression.AndAlso(LinqExpression.NotEqual(memberExpression, LinqExpression.Constant(null)), expression)  // parameter.{propertyName} != null && {expression}
                : LinqExpression.OrElse(LinqExpression.Equal(memberExpression, LinqExpression.Constant(null)), expression); // parameter.{propertyName} == null || {expression}
        }

        protected static BinaryExpression CreateExpression<T>([DisallowNull] ParameterExpression parameterExpression, [DisallowNull] string propertyName, ValueEqualityOperator @operator, T value) where T : struct =>
            (@operator == ValueEqualityOperator.EqualTo) ?
                LinqExpression.Equal(CreateMemberExpressions(parameterExpression, propertyName, value, out ConstantExpression valueConstant), valueConstant)
                : LinqExpression.NotEqual(CreateMemberExpressions(parameterExpression, propertyName, value, out valueConstant), valueConstant);

        protected static BinaryExpression CreateExpression<T>([DisallowNull] ParameterExpression parameterExpression, [DisallowNull] string propertyName, ObjectEqualityOperator @operator, T value) where T : class
        {
            MemberExpression memberExpression = CreateMemberExpressions(parameterExpression, propertyName, value, out ConstantExpression valueConstant);
            BinaryExpression expression = @operator switch
            {
                ObjectEqualityOperator.NotEqualTo or ObjectEqualityOperator.NotNullOrEqualTo => LinqExpression.NotEqual(memberExpression, valueConstant),
                _ => LinqExpression.Equal(memberExpression, valueConstant),
            };
            return @operator.HasFlag(ObjectEqualityOperator.NotNullOrEqualTo) ?
                LinqExpression.AndAlso(LinqExpression.NotEqual(memberExpression, LinqExpression.Constant(null)), expression) // parameter.{propertyName} != null && {expression}
                : LinqExpression.OrElse(LinqExpression.Equal(memberExpression, LinqExpression.Constant(null)), expression);  // parameter.{propertyName} == null || {expression}
        }

        protected static BinaryExpression CreateExpressionForString([DisallowNull] ParameterExpression parameterExpression, [DisallowNull] string propertyName, ValueEqualityOperator @operator, [DisallowNull] string value) =>
            (@operator == ValueEqualityOperator.EqualTo) ?
                LinqExpression.Equal(CreateMemberExpressions(parameterExpression, propertyName, value, out ConstantExpression valueConstant), valueConstant)
                : LinqExpression.NotEqual(CreateMemberExpressions(parameterExpression, propertyName, value, out valueConstant), valueConstant);

        protected static BinaryExpression CreateExpressionForNullable<T>([DisallowNull] ParameterExpression parameterExpression, [DisallowNull] string propertyName, ObjectComparisonOperator @operator, T value)
            where T : struct
        {
            BinaryExpression expression = @operator.HasFlag(ObjectComparisonOperator.NullOrEqualTo) ?
                (@operator.HasFlag(ObjectComparisonOperator.NullOrGreaterThan) ?
                    LinqExpression.GreaterThanOrEqual(CreateMemberExpressions(parameterExpression, propertyName, value, out MemberExpression memberExpression, out ConstantExpression valueConstant), valueConstant) // parameter.{propertyName}.Value >= dateTime
                    : (@operator.HasFlag(ObjectComparisonOperator.NullOrLessThan) ?
                        LinqExpression.LessThanOrEqual(CreateMemberExpressions(parameterExpression, propertyName, value, out memberExpression, out valueConstant), valueConstant) // parameter.{propertyName}.Value <= dateTime
                        : LinqExpression.Equal(CreateMemberExpressions(parameterExpression, propertyName, value, out memberExpression, out valueConstant), valueConstant) // parameter.{propertyName}.Value == dateTime
                    )
                )
                : (@operator.HasFlag(ObjectComparisonOperator.NullOrGreaterThan) ?
                    LinqExpression.GreaterThan(CreateMemberExpressions(parameterExpression, propertyName, value, out memberExpression, out valueConstant), valueConstant) // parameter.{propertyName}.Value > dateTime
                    : (@operator.HasFlag(ObjectComparisonOperator.NullOrLessThan) ?
                        LinqExpression.LessThan(CreateMemberExpressions(parameterExpression, propertyName, value, out memberExpression, out valueConstant), valueConstant) // parameter.{propertyName}.Value < dateTime
                        : LinqExpression.NotEqual(CreateMemberExpressions(parameterExpression, propertyName, value, out memberExpression, out valueConstant), valueConstant) // parameter.{propertyName}.Value != dateTime
                    )
                );
            return @operator.HasFlag(ObjectComparisonOperator.NotNullOrEqualTo) ?
                LinqExpression.AndAlso(LinqExpression.NotEqual(memberExpression, LinqExpression.Constant(null)), expression)  // parameter.{propertyName} != null && {expression}
                : LinqExpression.OrElse(LinqExpression.Equal(memberExpression, LinqExpression.Constant(null)), expression); // parameter.{propertyName} == null || {expression}
        }

        protected static BinaryExpression CreateExpression<T>([DisallowNull] ParameterExpression parameterExpression, [DisallowNull] string propertyName, ObjectComparisonOperator @operator, [DisallowNull] T value)
            where T : class
        {
            MemberExpression memberExpression = CreateMemberExpressions(parameterExpression, propertyName, value, out ConstantExpression valueConstant);
            BinaryExpression expression = @operator.HasFlag(ObjectComparisonOperator.NullOrEqualTo) ?
                (@operator.HasFlag(ObjectComparisonOperator.NullOrGreaterThan) ?
                    LinqExpression.GreaterThanOrEqual(memberExpression, valueConstant) // parameter.{propertyName}.Value >= dateTime
                    : (@operator.HasFlag(ObjectComparisonOperator.NullOrLessThan) ?
                        LinqExpression.LessThanOrEqual(memberExpression, valueConstant) // parameter.{propertyName}.Value <= dateTime
                        : LinqExpression.Equal(memberExpression, valueConstant) // parameter.{propertyName}.Value == dateTime
                    )
                )
                : (@operator.HasFlag(ObjectComparisonOperator.NullOrGreaterThan) ?
                    LinqExpression.GreaterThan(memberExpression, valueConstant) // parameter.{propertyName}.Value > dateTime
                    : (@operator.HasFlag(ObjectComparisonOperator.NullOrLessThan) ?
                        LinqExpression.LessThan(memberExpression, valueConstant) // parameter.{propertyName}.Value < dateTime
                        : LinqExpression.NotEqual(memberExpression, valueConstant) // parameter.{propertyName}.Value != dateTime
                    )
                );
            return @operator.HasFlag(ObjectComparisonOperator.NotNullOrEqualTo) ?
                LinqExpression.AndAlso(LinqExpression.NotEqual(memberExpression, LinqExpression.Constant(null)), expression) // parameter.{propertyName} != null && {expression}
                : LinqExpression.OrElse(LinqExpression.Equal(memberExpression, LinqExpression.Constant(null)), expression);  // parameter.{propertyName} == null || {expression}
        }

        protected static BinaryExpression CreateExpressionForValues<T>([DisallowNull] ParameterExpression parameterExpression, [DisallowNull] string propertyName, ValueEqualityOperator @operator, [DisallowNull] IEnumerable<T> values)
            where T : struct
        {
            IEnumerable<BinaryExpression> expressions = values.Select(v => CreateExpression(parameterExpression, propertyName, @operator, v));
            return (@operator == ValueEqualityOperator.EqualTo) ? expressions.Aggregate(LinqExpression.OrElse) : expressions.Aggregate(LinqExpression.AndAlso);
        }

        protected static BinaryExpression CreateExpressionForNullableValues<T>([DisallowNull] ParameterExpression parameterExpression, [DisallowNull] string propertyName, ObjectEqualityOperator @operator, [DisallowNull] IEnumerable<T> values)
            where T : struct
        {
            MemberExpression memberExpression = LinqExpression.Property(parameterExpression, propertyName);
            MemberExpression valueExpression = LinqExpression.Property(memberExpression, nameof(Nullable<T>.Value));
            BinaryExpression binaryExpression = @operator switch
            {
                ObjectEqualityOperator.NotEqualTo or ObjectEqualityOperator.NotNullOrEqualTo => values.Select(v => CreateEqualityExpression(valueExpression, ValueEqualityOperator.NotEqualTo, LinqExpression.Constant(v))).Aggregate(LinqExpression.OrElse),
                _ => values.Select(v => CreateEqualityExpression(valueExpression, ValueEqualityOperator.EqualTo, LinqExpression.Constant(v))).Aggregate(LinqExpression.AndAlso),
            };
            return @operator switch
            {
                ObjectEqualityOperator.NullOrEqualTo or ObjectEqualityOperator.NotEqualTo => (binaryExpression is null) ? LinqExpression.Equal(memberExpression, LinqExpression.Constant(null)) :
                                       LinqExpression.OrElse(LinqExpression.Equal(memberExpression, LinqExpression.Constant(null)), binaryExpression),
                _ => (binaryExpression is null) ? LinqExpression.NotEqual(memberExpression, LinqExpression.Constant(null)) :
LinqExpression.AndAlso(LinqExpression.NotEqual(memberExpression, LinqExpression.Constant(null)), binaryExpression),
            };
        }

        protected static BinaryExpression CreateExpressionForStringValues([DisallowNull] ParameterExpression parameterExpression, [DisallowNull] string propertyName, ValueEqualityOperator @operator, [DisallowNull] IEnumerable<string> values)
        {
            IEnumerable<BinaryExpression> expressions = values.Select(v => CreateExpressionForString(parameterExpression, propertyName, @operator, v));
            return (@operator == ValueEqualityOperator.EqualTo) ? expressions.Aggregate(LinqExpression.OrElse) : expressions.Aggregate(LinqExpression.AndAlso);
        }

        protected static BinaryExpression CreateExpressionForValues<T>([DisallowNull] ParameterExpression parameterExpression, [DisallowNull] string propertyName, ObjectEqualityOperator @operator, [DisallowNull] IEnumerable<T> values)
            where T : class
        {
            MemberExpression memberExpression = LinqExpression.Property(parameterExpression, propertyName);
            BinaryExpression binaryExpression = @operator switch
            {
                ObjectEqualityOperator.NotEqualTo or ObjectEqualityOperator.NotNullOrEqualTo => values.Select(v => CreateEqualityExpression(memberExpression, ValueEqualityOperator.NotEqualTo, LinqExpression.Constant(v))).Aggregate(LinqExpression.OrElse),
                _ => values.Select(v => CreateEqualityExpression(memberExpression, ValueEqualityOperator.EqualTo, LinqExpression.Constant(v))).Aggregate(LinqExpression.AndAlso),
            };
            return @operator switch
            {
                ObjectEqualityOperator.NullOrEqualTo or ObjectEqualityOperator.NotEqualTo => (binaryExpression is null) ? LinqExpression.Equal(memberExpression, LinqExpression.Constant(null)) :
                                       LinqExpression.OrElse(LinqExpression.Equal(memberExpression, LinqExpression.Constant(null)), binaryExpression),
                _ => (binaryExpression is null) ? LinqExpression.NotEqual(memberExpression, LinqExpression.Constant(null)) :
LinqExpression.AndAlso(LinqExpression.NotEqual(memberExpression, LinqExpression.Constant(null)), binaryExpression),
            };
        }

        protected static BinaryExpression CreateExpression<T>([DisallowNull] ParameterExpression parameterExpression, [DisallowNull] string propertyName, ValueComparisonOperator @operator, [DisallowNull] T value) where T : struct =>
            @operator.HasFlag(ValueComparisonOperator.EqualTo) ?
                (@operator.HasFlag(ValueComparisonOperator.GreaterThan) ?
                    LinqExpression.GreaterThanOrEqual(CreateMemberExpressions(parameterExpression, propertyName, value, out ConstantExpression valueConstant), valueConstant) // parameter.{propertyName} >= dateTime
                    : (@operator.HasFlag(ValueComparisonOperator.LessThan) ?
                        LinqExpression.LessThanOrEqual(CreateMemberExpressions(parameterExpression, propertyName, value, out valueConstant), valueConstant) // parameter.{propertyName} <= dateTime
                        : LinqExpression.Equal(CreateMemberExpressions(parameterExpression, propertyName, value, out valueConstant), valueConstant) // parameter.{propertyName} == dateTime
                    )
                )
                : (@operator.HasFlag(ValueComparisonOperator.GreaterThan) ?
                    LinqExpression.GreaterThan(CreateMemberExpressions(parameterExpression, propertyName, value, out valueConstant), valueConstant) // parameter.{propertyName} > dateTime
                    : (@operator.HasFlag(ValueComparisonOperator.LessThan) ?
                        LinqExpression.LessThan(CreateMemberExpressions(parameterExpression, propertyName, value, out valueConstant), valueConstant) // parameter.{propertyName} < dateTime
                        : LinqExpression.NotEqual(CreateMemberExpressions(parameterExpression, propertyName, value, out valueConstant), valueConstant) // parameter.{propertyName} != dateTime
                    )
                );

        protected static bool IsValueMatch<T>(T x, ValueEqualityOperator @operator, T y) where T : struct, IEquatable<T> => @operator == ValueEqualityOperator.EqualTo == x.Equals(y);

        protected static bool IsStringMatch(string x, ValueEqualityOperator @operator, [DisallowNull] string y) => @operator == ValueEqualityOperator.EqualTo == (x ?? "").Equals(y);

        protected static bool IsValueMatch<T>(T? x, ObjectEqualityOperator @operator, T y) where T : struct, IEquatable<T> => x.HasValue ? @operator switch
        {
            ObjectEqualityOperator.NotEqualTo or ObjectEqualityOperator.NotNullOrEqualTo => !x.Value.Equals(y),
            _ => x.Value.Equals(y),
        } : !@operator.HasFlag(ObjectEqualityOperator.NotNullOrEqualTo);

        protected static bool IsValueMatch<T>(T x, ValueComparisonOperator @operator, T y) where T : struct, IComparable<T>, IEquatable<T> => @operator switch
        {
            ValueComparisonOperator.NotEqualTo => !x.Equals(y),
            ValueComparisonOperator.LessThan => x.CompareTo(y) < 0,
            ValueComparisonOperator.GreaterThan => x.CompareTo(y) > 0,
            ValueComparisonOperator.LessThanOrEqualTo => x.CompareTo(y) <= 0,
            ValueComparisonOperator.GreaterThanOrEqualTo => x.CompareTo(y) >= 0,
            _ => x.Equals(y),
        };

        protected static bool IsValueMatch<T>(T? x, ObjectComparisonOperator @operator, T y) where T : struct, IComparable<T>, IEquatable<T> => x.HasValue ? @operator switch
        {
            ObjectComparisonOperator.NotEqualTo or ObjectComparisonOperator.NotNullOrEqualTo => !x.Value.Equals(y),
            ObjectComparisonOperator.LessThan or ObjectComparisonOperator.NullOrLessThan => x.Value.CompareTo(y) < 0,
            ObjectComparisonOperator.GreaterThan or ObjectComparisonOperator.NullOrGreaterThan => x.Value.CompareTo(y) > 0,
            ObjectComparisonOperator.LessThanOrEqualTo or ObjectComparisonOperator.NullLessThanOrEqualTo => x.Value.CompareTo(y) <= 0,
            ObjectComparisonOperator.GreaterThanOrEqualTo or ObjectComparisonOperator.NullGreaterThanOrEqualTo => x.Value.CompareTo(y) >= 0,
            _ => x.Value.Equals(y),
        } : !@operator.HasFlag(ObjectComparisonOperator.NotNullOrEqualTo);

        protected static bool IsObjectMatch<T>(T x, ObjectEqualityOperator @operator, T y) where T : class, IEquatable<T> => (x is null) ? !@operator.HasFlag(ObjectEqualityOperator.NotNullOrEqualTo) : @operator switch
        {
            ObjectEqualityOperator.NotEqualTo or ObjectEqualityOperator.NotNullOrEqualTo => !x.Equals(y),
            _ => x.Equals(y),
        };

        protected static bool IsObjectMatch<T>(T x, ObjectComparisonOperator @operator, T y) where T : class, IComparable<T>, IEquatable<T> => (x is null) ? !@operator.HasFlag(ObjectComparisonOperator.NotNullOrEqualTo) : @operator switch
        {
            ObjectComparisonOperator.NotEqualTo or ObjectComparisonOperator.NotNullOrEqualTo => !x.Equals(y),
            ObjectComparisonOperator.LessThan or ObjectComparisonOperator.NullOrLessThan => x.CompareTo(y) < 0,
            ObjectComparisonOperator.GreaterThan or ObjectComparisonOperator.NullOrGreaterThan => x.CompareTo(y) > 0,
            ObjectComparisonOperator.LessThanOrEqualTo or ObjectComparisonOperator.NullLessThanOrEqualTo => x.CompareTo(y) <= 0,
            ObjectComparisonOperator.GreaterThanOrEqualTo or ObjectComparisonOperator.NullGreaterThanOrEqualTo => x.CompareTo(y) >= 0,
            _ => x.Equals(y),
        };

        public abstract BinaryExpression CreateExpression([DisallowNull] ParameterExpression parameterExpression);

        public abstract bool IsMatch(TEntity entity);

        private void ErrorInfo_ErrorsChanged(object sender, DataErrorsChangedEventArgs e) => ErrorsChanged?.Invoke(this, e);

        public IEnumerable GetErrors(string propertyName) => ErrorInfo.GetErrors(propertyName);
    }
}
