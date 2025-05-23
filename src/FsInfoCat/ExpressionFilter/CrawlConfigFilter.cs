using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace FsInfoCat.ExpressionFilter
{
    // TODO: Document CrawlConfigFilter class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class CrawlConfigFilter : Filter<Model.ICrawlConfigReportItem>
    {
        private readonly IPropertyChangeTracker<string> _displayText;
        private readonly IPropertyChangeTracker<Historical.Range> _crawlEnd;
        private readonly IPropertyChangeTracker<Scheduled.Range> _nextCrawlStart;
        private readonly IPropertyChangeTracker<StatusFilter> _status;
        private readonly IPropertyChangeTracker<DurationRange> _averageDuration;
        private readonly IPropertyChangeTracker<DurationRange> _maxDuration;
        private readonly IPropertyChangeTracker<bool?> _hasCancel;
        private readonly IPropertyChangeTracker<bool?> _hasFail;
        private readonly IPropertyChangeTracker<bool?> _anyReachedItemLimit;
        private readonly IPropertyChangeTracker<bool?> _anySucceeded;
        private readonly IPropertyChangeTracker<bool?> _anyTimedOut;

        public string DisplayText { get => _displayText.GetValue(); set => _displayText.SetValue(value); }

        public Historical.Range CrawlEnd { get => _crawlEnd.GetValue(); set => _crawlEnd.SetValue(value); }

        public Scheduled.Range NextCrawlStart { get => _nextCrawlStart.GetValue(); set => _nextCrawlStart.SetValue(value); }

        public StatusFilter Status { get => _status.GetValue(); set => _status.SetValue(value); }

        public DurationRange AverageDuration { get => _averageDuration.GetValue(); set => _averageDuration.SetValue(value); }

        public DurationRange MaxDuration { get => _maxDuration.GetValue(); set => _maxDuration.SetValue(value); }

        public bool? HasCancel { get => _hasCancel.GetValue(); set => _hasCancel.SetValue(value); }

        public bool? HasFail { get => _hasFail.GetValue(); set => _hasFail.SetValue(value); }

        public bool? AnyReachedItemLimit { get => _anyReachedItemLimit.GetValue(); set => _anyReachedItemLimit.SetValue(value); }

        public bool? AnySucceeded { get => _anySucceeded.GetValue(); set => _anySucceeded.SetValue(value); }

        public bool? AnyTimedOut { get => _anyTimedOut.GetValue(); set => _anyTimedOut.SetValue(value); }

        public CrawlConfigFilter()
        {
            _displayText = AddChangeTracker(nameof(DisplayText), "");
            _crawlEnd = AddChangeTracker<Historical.Range>(nameof(CrawlEnd), null);
            _nextCrawlStart = AddChangeTracker<Scheduled.Range>(nameof(NextCrawlStart), null);
            _status = AddChangeTracker<StatusFilter>(nameof(Status), null);
            _averageDuration = AddChangeTracker<DurationRange>(nameof(AverageDuration), null);
            _maxDuration = AddChangeTracker<DurationRange>(nameof(MaxDuration), null);
            _hasCancel = AddChangeTracker<bool?>(nameof(HasCancel), null);
            _hasFail = AddChangeTracker<bool?>(nameof(HasFail), null);
            _anyReachedItemLimit = AddChangeTracker<bool?>(nameof(AnyReachedItemLimit), null);
            _anySucceeded = AddChangeTracker<bool?>(nameof(AnySucceeded), null);
            _anyTimedOut = AddChangeTracker<bool?>(nameof(AnyTimedOut), null);
        }

        record RangeValue<T>(T Value, bool IsExclusive);

        record RangePair<T>(RangeValue<T> Start, RangeValue<T> End, bool IncludeNull);

        public override BinaryExpression CreateExpression([DisallowNull] ParameterExpression parameterExpression)
        {
            BinaryExpression binaryExpression = CrawlEnd?.GetExpression(Expression.Property(parameterExpression, nameof(Model.ICrawlConfigReportItem.LastCrawlEnd)));
            BinaryExpression expr = NextCrawlStart?.GetExpression(Expression.Property(parameterExpression, nameof(Model.ICrawlConfigReportItem.NextScheduledStart)));
            if (expr is not null)
                binaryExpression = (binaryExpression is null) ? expr : Expression.AndAlso(binaryExpression, expr);
            if ((expr = AverageDuration?.GetExpression(Expression.Property(parameterExpression, nameof(Model.ICrawlConfigReportItem.AverageDuration)))) is not null)
                binaryExpression = (binaryExpression is null) ? expr : Expression.AndAlso(binaryExpression, expr);
            if ((expr = MaxDuration?.GetExpression(Expression.Property(parameterExpression, nameof(Model.ICrawlConfigReportItem.MaxDuration)))) is not null)
                binaryExpression = (binaryExpression is null) ? expr : Expression.AndAlso(binaryExpression, expr);
            bool? b = AnySucceeded;
            if (b.HasValue)
            {
                expr = b.Value ? Expression.GreaterThan(Expression.Property(parameterExpression, nameof(Model.ICrawlConfigReportItem.SucceededCount)),
                    Expression.Constant(0)) :
                    Expression.Equal(Expression.Property(parameterExpression, nameof(Model.ICrawlConfigReportItem.SucceededCount)),
                    Expression.Constant(0));
                binaryExpression = (binaryExpression is null) ? expr : Expression.AndAlso(binaryExpression, expr);
            }
            if ((b = AnyReachedItemLimit).HasValue)
            {
                expr = b.Value ? Expression.GreaterThan(Expression.Property(parameterExpression, nameof(Model.ICrawlConfigReportItem.ItemLimitReachedCount)),
                    Expression.Constant(0)) :
                    Expression.Equal(Expression.Property(parameterExpression, nameof(Model.ICrawlConfigReportItem.ItemLimitReachedCount)),
                    Expression.Constant(0));
                binaryExpression = (binaryExpression is null) ? expr : Expression.AndAlso(binaryExpression, expr);
            }
            if ((b = AnyTimedOut).HasValue)
            {
                expr = b.Value ? Expression.GreaterThan(Expression.Property(parameterExpression, nameof(Model.ICrawlConfigReportItem.TimedOutCount)),
                    Expression.Constant(0)) :
                    Expression.Equal(Expression.Property(parameterExpression, nameof(Model.ICrawlConfigReportItem.TimedOutCount)),
                    Expression.Constant(0));
                binaryExpression = (binaryExpression is null) ? expr : Expression.AndAlso(binaryExpression, expr);
            }
            if ((b = HasCancel).HasValue)
            {
                expr = b.Value ? Expression.GreaterThan(Expression.Property(parameterExpression, nameof(Model.ICrawlConfigReportItem.CanceledCount)),
                    Expression.Constant(0)) :
                    Expression.Equal(Expression.Property(parameterExpression, nameof(Model.ICrawlConfigReportItem.CanceledCount)),
                    Expression.Constant(0));
                binaryExpression = (binaryExpression is null) ? expr : Expression.AndAlso(binaryExpression, expr);
            }
            if ((b = HasFail).HasValue)
            {
                expr = b.Value ? Expression.GreaterThan(Expression.Property(parameterExpression, nameof(Model.ICrawlConfigReportItem.FailedCount)),
                    Expression.Constant(0)) :
                    Expression.Equal(Expression.Property(parameterExpression, nameof(Model.ICrawlConfigReportItem.FailedCount)),
                    Expression.Constant(0));
                binaryExpression = (binaryExpression is null) ? expr : Expression.AndAlso(binaryExpression, expr);
            }

            expr = Status?.CreateExpression(parameterExpression);
            return (expr is null) ? binaryExpression : (binaryExpression is null) ? expr : Expression.AndAlso(binaryExpression, expr);
        }

        internal static bool AreSame(CrawlConfigFilter x, CrawlConfigFilter y)
        {
            if (x is null)
                return y is null;
            if (y is null)
                return false;
            if (ReferenceEquals(x, y))
                return true;
            return Historical.Range.AreSame(x.CrawlEnd, y.CrawlEnd) && Scheduled.Range.AreSame(x.NextCrawlStart, y.NextCrawlStart) && DurationRange.AreSame(x.AverageDuration, y.AverageDuration) && DurationRange.AreSame(x.MaxDuration, y.MaxDuration) &&
                x.HasCancel == y.HasCancel && x.HasFail == y.HasFail && x.AnyReachedItemLimit == y.AnyReachedItemLimit && x.AnySucceeded == y.AnySucceeded && x.AnyTimedOut == y.AnyTimedOut &&
                StatusFilter.AreSame(x.Status, y.Status);
        }

        public override bool IsMatch(Model.ICrawlConfigReportItem item)
        {
            if (item is null)
                return false;
            Historical.Range crawlEnd = CrawlEnd;
            Scheduled.Range nextStart = NextCrawlStart;
            if ((crawlEnd is not null && !crawlEnd.IsInRange(item.LastCrawlEnd)) || (nextStart is not null && !nextStart.IsInRange(item.NextScheduledStart)))
                return false;
            StatusFilter crawlStatus = Status;
            DurationRange dr = AverageDuration;
            if ((crawlStatus is not null && !crawlStatus.IsMatch(item.StatusValue)) || (dr is not null && !dr.IsInRange(item.AverageDuration)))
                return false;
            dr = MaxDuration;
            bool? b = HasCancel;
            if ((dr is null || dr.IsInRange(item.MaxDuration)) && (!b.HasValue || b.Value == item.CanceledCount > 0))
            {
                b = HasFail;
                if (!b.HasValue || b.Value == item.FailedCount > 0)
                {
                    b = AnyReachedItemLimit;
                    if (!b.HasValue || b.Value == item.ItemLimitReachedCount > 0)
                    {
                        b = AnySucceeded;
                        if (!b.HasValue || b.Value == item.SucceededCount > 0)
                        {
                            b = AnyTimedOut;
                            return !b.HasValue || b.Value == item.TimedOutCount > 0;
                        }
                    }
                }
            }
            return false;
        }
    }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
}
