using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Filter
{
    public abstract class NullableStringValueFilter<TEntity>([DisallowNull] string propertyName) : Filter<TEntity>
        where TEntity : class
    {
        private readonly string _propertyName = propertyName;

        #region Operator Property Members

        /// <summary>
        /// Identifies the <see cref="Operator"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OperatorProperty = DependencyPropertyBuilder<NullableStringValueFilter<TEntity>, ObjectEqualityOperator>
            .Register(nameof(Operator))
            .DefaultValue(ObjectEqualityOperator.EqualTo)
            .AsReadWrite();

        public ObjectEqualityOperator Operator { get => (ObjectEqualityOperator)GetValue(OperatorProperty); set => SetValue(OperatorProperty, value); }

        #endregion
        #region Value Property Members

        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyPropertyBuilder<NullableStringValueFilter<TEntity>, string>
            .Register(nameof(Value))
            .DefaultValue("")
            .CoerseWith((DependencyObject d, object baseValue) => (baseValue as string) ?? "")
            .AsReadWrite();

        public string Value { get => GetValue(ValueProperty) as string; set => SetValue(ValueProperty, value); }

        #endregion

        protected abstract string GetMemberValue([DisallowNull] TEntity crawlConfiguration);

        public override BinaryExpression CreateExpression([DisallowNull] ParameterExpression parameterExpression) => CreateExpression(parameterExpression, _propertyName, Operator, Value);

        public override bool IsMatch(TEntity crawlConfiguration) => crawlConfiguration is not null && IsObjectMatch(GetMemberValue(crawlConfiguration), Operator, Value);
    }
}
