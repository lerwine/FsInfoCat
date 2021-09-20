using FsInfoCat.Local;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Windows;
using LinqExpression = System.Linq.Expressions.Expression;

namespace FsInfoCat.Desktop.ViewModel.Filter.CrawlConfig
{
    public sealed class StatusValue : Filter<CrawlConfigReportItem>
    {
        #region Value Property Members

        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyPropertyBuilder<StatusValue, CrawlStatus>
            .Register(nameof(Value))
            .DefaultValue(CrawlStatus.NotRunning)
            .AsReadWrite();

        public CrawlStatus Value { get => (CrawlStatus)GetValue(ValueProperty); set => SetValue(ValueProperty, value); }

        #endregion
        #region Operator Property Members

        /// <summary>
        /// Identifies the <see cref="Operator"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OperatorProperty = DependencyPropertyBuilder<StatusValue, ValueEqualityOperator>
            .Register(nameof(Operator))
            .DefaultValue(ValueEqualityOperator.EqualTo)
            .AsReadWrite();

        public ValueEqualityOperator Operator { get => (ValueEqualityOperator)GetValue(OperatorProperty); set => SetValue(OperatorProperty, value); }

        #endregion
        public override BinaryExpression CreateExpression([DisallowNull] ParameterExpression parameterExpression) => (Operator == ValueEqualityOperator.EqualTo) ?
            LinqExpression.Equal(LinqExpression.Property(parameterExpression, nameof(ICrawlConfigReportItem.StatusValue)), LinqExpression.Constant(Value)) :
            LinqExpression.NotEqual(LinqExpression.Property(parameterExpression, nameof(ICrawlConfigReportItem.StatusValue)), LinqExpression.Constant(Value));

        public override bool IsMatch(CrawlConfigReportItem crawlConfiguration) => crawlConfiguration is not null && crawlConfiguration.StatusValue == Value;
    }
}
