using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Filter
{
    public abstract class StringValueFilter<TEntity> : Filter<TEntity>
        where TEntity : class
    {
        private readonly string _propertyName;

        #region Operator Property Members

        /// <summary>
        /// Identifies the <see cref="Operator"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OperatorProperty = DependencyPropertyBuilder<StringValueFilter<TEntity>, ValueEqualityOperator>
            .Register(nameof(Operator))
            .DefaultValue(ValueEqualityOperator.EqualTo)
            .AsReadWrite();

        public ValueEqualityOperator Operator { get => (ValueEqualityOperator)GetValue(OperatorProperty); set => SetValue(OperatorProperty, value); }

        #endregion
        #region Value Property Members

        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyPropertyBuilder<StringValueFilter<TEntity>, string>
            .Register(nameof(Value))
            .DefaultValue("")
            .CoerseWith((DependencyObject d, object baseValue) => (baseValue as string) ?? "")
            .AsReadWrite();

        public string Value { get => GetValue(ValueProperty) as string; set => SetValue(ValueProperty, value); }

        #endregion
        protected StringValueFilter([DisallowNull] string propertyName)
        {
            _propertyName = propertyName;
        }

        protected abstract string GetMemberValue([DisallowNull] TEntity entity);

        public override BinaryExpression CreateExpression([DisallowNull] ParameterExpression parameterExpression) => CreateExpressionForString(parameterExpression, _propertyName, Operator, Value);

        public override bool IsMatch(TEntity entity) => entity is not null && IsStringMatch(GetMemberValue(entity), Operator, Value);
    }
}
