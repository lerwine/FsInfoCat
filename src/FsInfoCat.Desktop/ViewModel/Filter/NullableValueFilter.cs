using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Filter
{
    public abstract class ValueSetFilter<TEntity, TValue> : Filter<TEntity>
        where TEntity : class
        where TValue : struct, IEquatable<TValue>
    {
    }
    public abstract class ValueFilter<TEntity, TValue>([DisallowNull] string propertyName) : Filter<TEntity>
        where TEntity : class
        where TValue : struct, IComparable<TValue>, IEquatable<TValue>
    {
        private readonly string _propertyName = propertyName;

        #region Operator Property Members

        /// <summary>
        /// Identifies the <see cref="Operator"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OperatorProperty = DependencyPropertyBuilder<ValueFilter<TEntity, TValue>, ValueComparisonOperator>
            .Register(nameof(Operator))
            .DefaultValue(ValueComparisonOperator.GreaterThan)
            .AsReadWrite();

        public ValueComparisonOperator Operator { get => (ValueComparisonOperator)GetValue(OperatorProperty); set => SetValue(OperatorProperty, value); }

        #endregion

        protected abstract TValue GetComparisonValue();

        protected abstract TValue GetMemberValue([DisallowNull] TEntity crawlConfiguration);

        public override BinaryExpression CreateExpression([DisallowNull] ParameterExpression parameterExpression) => CreateExpression(parameterExpression, _propertyName, Operator, GetComparisonValue());

        public override bool IsMatch(TEntity crawlConfiguration) => crawlConfiguration is not null && IsValueMatch(GetMemberValue(crawlConfiguration), Operator, GetComparisonValue());
    }
    public abstract class NullableValueFilter<TEntity, TValue>([DisallowNull] string propertyName) : Filter<TEntity>
        where TEntity : class
        where TValue : struct, IComparable<TValue>, IEquatable<TValue>
    {
        private readonly string _propertyName = propertyName;

        #region Operator Property Members

        /// <summary>
        /// Identifies the <see cref="Operator"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OperatorProperty = DependencyPropertyBuilder<NullableValueFilter<TEntity, TValue>, ObjectComparisonOperator>
            .Register(nameof(Operator))
            .DefaultValue(ObjectComparisonOperator.GreaterThan)
            .AsReadWrite();

        public ObjectComparisonOperator Operator { get => (ObjectComparisonOperator)GetValue(OperatorProperty); set => SetValue(OperatorProperty, value); }

        #endregion

        protected abstract TValue GetComparisonValue();

        protected abstract TValue? GetMemberValue([DisallowNull] TEntity crawlConfiguration);

        public override BinaryExpression CreateExpression([DisallowNull] ParameterExpression parameterExpression) => CreateExpressionForNullable(parameterExpression, _propertyName, Operator, GetComparisonValue());

        public override bool IsMatch(TEntity crawlConfiguration) => crawlConfiguration is not null && IsValueMatch(GetMemberValue(crawlConfiguration), Operator, GetComparisonValue());
    }
}
