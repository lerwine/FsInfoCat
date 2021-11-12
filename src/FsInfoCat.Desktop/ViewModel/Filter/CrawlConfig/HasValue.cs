using FsInfoCat.Local;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Windows;
using LinqExpression = System.Linq.Expressions.Expression;

namespace FsInfoCat.Desktop.ViewModel.Filter.CrawlConfig
{

    public sealed class HasValue : Filter<CrawlConfigReportItem>
    {
        #region IsExclusive Property Members

        /// <summary>
        /// Identifies the <see cref="IsExclusive"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsExclusiveProperty = DependencyPropertyBuilder<HasValue, bool>
            .Register(nameof(IsExclusive))
            .DefaultValue(false)
            .AsReadWrite();

        public bool IsExclusive { get => (bool)GetValue(IsExclusiveProperty); set => SetValue(IsExclusiveProperty, value); }

        #endregion
        #region LastCrawlStart Property Members

        /// <summary>
        /// Identifies the <see cref="LastCrawlStart"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LastCrawlStartProperty = DependencyPropertyBuilder<HasValue, bool?>
            .Register(nameof(LastCrawlStart))
            .DefaultValue(null)
            .AsReadWrite();

        public bool? LastCrawlStart { get => (bool?)GetValue(LastCrawlStartProperty); set => SetValue(LastCrawlStartProperty, value); }

        #endregion
        #region LastCrawlEnd Property Members

        /// <summary>
        /// Identifies the <see cref="LastCrawlEnd"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LastCrawlEndProperty = DependencyPropertyBuilder<HasValue, bool?>
            .Register(nameof(LastCrawlEnd))
            .DefaultValue(null)
            .AsReadWrite();

        public bool? LastCrawlEnd { get => (bool?)GetValue(LastCrawlEndProperty); set => SetValue(LastCrawlEndProperty, value); }

        #endregion
        #region NextScheduledStart Property Members

        /// <summary>
        /// Identifies the <see cref="NextScheduledStart"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NextScheduledStartProperty = DependencyPropertyBuilder<HasValue, bool?>
            .Register(nameof(NextScheduledStart))
            .DefaultValue(null)
            .AsReadWrite();

        public bool? NextScheduledStart { get => (bool?)GetValue(NextScheduledStartProperty); set => SetValue(NextScheduledStartProperty, value); }

        #endregion
        #region RescheduleInterval Property Members

        /// <summary>
        /// Identifies the <see cref="RescheduleInterval"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RescheduleIntervalProperty = DependencyPropertyBuilder<HasValue, bool?>
            .Register(nameof(RescheduleInterval))
            .DefaultValue(null)
            .AsReadWrite();

        public bool? RescheduleInterval { get => (bool?)GetValue(RescheduleIntervalProperty); set => SetValue(RescheduleIntervalProperty, value); }

        #endregion
        #region AggregateDurations Property Members

        /// <summary>
        /// Identifies the <see cref="AggregateDurations"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AggregateDurationsProperty = DependencyPropertyBuilder<HasValue, bool?>
            .Register(nameof(AggregateDurations))
            .DefaultValue(null)
            .AsReadWrite();

        public bool? AggregateDurations { get => (bool?)GetValue(AggregateDurationsProperty); set => SetValue(AggregateDurationsProperty, value); }

        #endregion

        public override BinaryExpression CreateExpression([DisallowNull] ParameterExpression parameterExpression)
        {
            bool isExclusive = IsExclusive;
            bool? hasValue = LastCrawlStart.HasValue;
            BinaryExpression binaryExpression = hasValue.HasValue ? (hasValue.Value ? LinqExpression.NotEqual(LinqExpression.Property(parameterExpression, nameof(ICrawlConfigReportItem.LastCrawlStart)), LinqExpression.Constant(null)) :
                LinqExpression.Equal(LinqExpression.Property(parameterExpression, nameof(ICrawlConfigReportItem.LastCrawlStart)), LinqExpression.Constant(null))) : null;
            hasValue = LastCrawlEnd.HasValue;
            BinaryExpression be = hasValue.HasValue ? (hasValue.Value ? LinqExpression.NotEqual(LinqExpression.Property(parameterExpression, nameof(ICrawlConfigReportItem.LastCrawlEnd)), LinqExpression.Constant(null)) :
                LinqExpression.Equal(LinqExpression.Property(parameterExpression, nameof(ICrawlConfigReportItem.LastCrawlEnd)), LinqExpression.Constant(null))) : null;
            if (be is not null)
                binaryExpression = (binaryExpression is null) ? be : isExclusive ? LinqExpression.AndAlso(binaryExpression, be) : LinqExpression.OrElse(binaryExpression, be);
            hasValue = NextScheduledStart.HasValue;
            be = hasValue.HasValue ? (hasValue.Value ? LinqExpression.NotEqual(LinqExpression.Property(parameterExpression, nameof(ICrawlConfigReportItem.NextScheduledStart)), LinqExpression.Constant(null)) :
                LinqExpression.Equal(LinqExpression.Property(parameterExpression, nameof(ICrawlConfigReportItem.NextScheduledStart)), LinqExpression.Constant(null))) : null;
            if (be is not null)
                binaryExpression = (binaryExpression is null) ? be : isExclusive ? LinqExpression.AndAlso(binaryExpression, be) : LinqExpression.OrElse(binaryExpression, be);
            hasValue = RescheduleInterval.HasValue;
            be = hasValue.HasValue ? (hasValue.Value ? LinqExpression.NotEqual(LinqExpression.Property(parameterExpression, nameof(ICrawlConfigReportItem.RescheduleInterval)), LinqExpression.Constant(null)) :
                LinqExpression.Equal(LinqExpression.Property(parameterExpression, nameof(ICrawlConfigReportItem.RescheduleInterval)), LinqExpression.Constant(null))) : null;
            if (be is not null)
                binaryExpression = (binaryExpression is null) ? be : isExclusive ? LinqExpression.AndAlso(binaryExpression, be) : LinqExpression.OrElse(binaryExpression, be);
            hasValue = AggregateDurations.HasValue;
            be = hasValue.HasValue ? (hasValue.Value ? LinqExpression.NotEqual(LinqExpression.Property(parameterExpression, nameof(ICrawlConfigReportItem.AverageDuration)), LinqExpression.Constant(null)) :
                LinqExpression.Equal(LinqExpression.Property(parameterExpression, nameof(ICrawlConfigReportItem.AverageDuration)), LinqExpression.Constant(null))) : null;
            return (be is null) ? binaryExpression : (binaryExpression is null) ? be : isExclusive ? LinqExpression.AndAlso(binaryExpression, be) : LinqExpression.OrElse(binaryExpression, be);
        }

        public override bool IsMatch(CrawlConfigReportItem crawlConfiguration)
        {
            if (crawlConfiguration is null)
                return false;

            bool? b = LastCrawlStart;
            if (b.HasValue && crawlConfiguration.LastCrawlStart.HasValue != b.Value)
                return false;
            b = LastCrawlEnd;
            if (b.HasValue && crawlConfiguration.LastCrawlEnd.HasValue != b.Value)
                return false;
            b = NextScheduledStart;
            if (b.HasValue && crawlConfiguration.NextScheduledStart.HasValue != b.Value)
                return false;
            b = RescheduleInterval;
            if (b.HasValue && crawlConfiguration.RescheduleInterval.HasValue != b.Value)
                return false;
            b = AggregateDurations;
            return b.HasValue && crawlConfiguration.AverageDuration.HasValue != b.Value;
        }
    }
}
