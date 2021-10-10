using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

namespace FsInfoCat.ExpressionFilter
{
    public class StatusFilter : NotifyDataErrorInfo, IFilter
    {
        private readonly IPropertyChangeTracker<bool> _isExclusive;
        private readonly IPropertyChangeTracker<bool> _allottedTimeElapsed;
        private readonly IPropertyChangeTracker<bool> _canceled;
        private readonly IPropertyChangeTracker<bool> _completed;
        private readonly IPropertyChangeTracker<bool> _disabled;
        private readonly IPropertyChangeTracker<bool> _failed;
        private readonly IPropertyChangeTracker<bool> _inProgress;
        private readonly IPropertyChangeTracker<bool> _maxItemCountReached;
        private readonly IPropertyChangeTracker<bool> _notRunning;

        public bool IsExclusive { get => _isExclusive.GetValue(); set => _isExclusive.SetValue(value); }

        public bool AllottedTimeElapsed { get => _allottedTimeElapsed.GetValue(); set => _allottedTimeElapsed.SetValue(value); }

        public bool Canceled { get => _canceled.GetValue(); set => _canceled.SetValue(value); }

        public bool Completed { get => _completed.GetValue(); set => _completed.SetValue(value); }

        public bool Disabled { get => _disabled.GetValue(); set => _disabled.SetValue(value); }

        public bool Failed { get => _failed.GetValue(); set => _failed.SetValue(value); }

        public bool InProgress { get => _inProgress.GetValue(); set => _inProgress.SetValue(value); }

        public bool MaxItemCountReached { get => _maxItemCountReached.GetValue(); set => _maxItemCountReached.SetValue(value); }

        public bool NotRunning { get => _notRunning.GetValue(); set => _notRunning.SetValue(value); }

        public StatusFilter()
        {
            _isExclusive = AddChangeTracker(nameof(IsExclusive), false);
            _allottedTimeElapsed = AddChangeTracker(nameof(AllottedTimeElapsed), false);
            _canceled = AddChangeTracker(nameof(Canceled), false);
            _completed = AddChangeTracker(nameof(Completed), false);
            _disabled = AddChangeTracker(nameof(Disabled), false);
            _failed = AddChangeTracker(nameof(Failed), false);
            _inProgress = AddChangeTracker(nameof(InProgress), false);
            _maxItemCountReached = AddChangeTracker(nameof(MaxItemCountReached), false);
            _notRunning = AddChangeTracker(nameof(NotRunning), false);
        }

        public static IEnumerable<(CrawlStatus Value, bool Option)> GetEmptyOptions()
        {
            yield return (CrawlStatus.NotRunning, false);
            yield return (CrawlStatus.InProgress, false);
            yield return (CrawlStatus.Completed, false);
            yield return (CrawlStatus.AllottedTimeElapsed, false);
            yield return (CrawlStatus.MaxItemCountReached, false);
            yield return (CrawlStatus.Canceled, false);
            yield return (CrawlStatus.Failed, false);
            yield return (CrawlStatus.Disabled, false);
        }

        public IEnumerable<CrawlStatus> GetSelectedOptions()
        {
            if (NotRunning)
            yield return CrawlStatus.NotRunning;
            if (InProgress)
                yield return CrawlStatus.InProgress;
            if (Completed)
                yield return CrawlStatus.Completed;
            if (AllottedTimeElapsed)
                yield return CrawlStatus.AllottedTimeElapsed;
            if (MaxItemCountReached)
                yield return CrawlStatus.MaxItemCountReached;
            if (Canceled)
                yield return CrawlStatus.Canceled;
            if (Failed)
                yield return CrawlStatus.Failed;
            if (Disabled)
                yield return CrawlStatus.Disabled;
        }

        public static bool AreSame(StatusFilter x, StatusFilter y) => (x is null) ? (y is null || !y.GetSelectedOptions().Any()) : (y is null) ? !x.GetSelectedOptions().Any() :
            y is not null && (ReferenceEquals(x, y) || (x.GetSelectedOptions().Any() ? (x.IsExclusive == y.IsExclusive && x.GetSelectedOptions().SequenceEqual(y.GetSelectedOptions())) : !y.GetSelectedOptions().Any()));

        public bool IsMatch(CrawlStatus statusValue) => IsExclusive ? !GetSelectedOptions().Contains(statusValue) : GetSelectedOptions().Contains(statusValue);

        public BinaryExpression CreateExpression([DisallowNull] ParameterExpression parameterExpression) => IsExclusive ?
            GetSelectedOptions().Select(s => Expression.NotEqual(Expression.Property(parameterExpression, nameof(ICrawlConfigReportItem.StatusValue)), Expression.Constant(s))).Aggregate(Expression.AndAlso) :
            GetSelectedOptions().Select(s => Expression.Equal(Expression.Property(parameterExpression, nameof(ICrawlConfigReportItem.StatusValue)), Expression.Constant(s))).Aggregate(Expression.OrElse);
    }
}
