using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

namespace FsInfoCat.ExpressionFilter
{
    // TODO: Document StatusFilter class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
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

        public static IEnumerable<(Model.CrawlStatus Value, bool Option)> GetEmptyOptions()
        {
            yield return (Model.CrawlStatus.NotRunning, false);
            yield return (Model.CrawlStatus.InProgress, false);
            yield return (Model.CrawlStatus.Completed, false);
            yield return (Model.CrawlStatus.AllottedTimeElapsed, false);
            yield return (Model.CrawlStatus.MaxItemCountReached, false);
            yield return (Model.CrawlStatus.Canceled, false);
            yield return (Model.CrawlStatus.Failed, false);
            yield return (Model.CrawlStatus.Disabled, false);
        }

        public IEnumerable<Model.CrawlStatus> GetSelectedOptions()
        {
            if (NotRunning)
                yield return Model.CrawlStatus.NotRunning;
            if (InProgress)
                yield return Model.CrawlStatus.InProgress;
            if (Completed)
                yield return Model.CrawlStatus.Completed;
            if (AllottedTimeElapsed)
                yield return Model.CrawlStatus.AllottedTimeElapsed;
            if (MaxItemCountReached)
                yield return Model.CrawlStatus.MaxItemCountReached;
            if (Canceled)
                yield return Model.CrawlStatus.Canceled;
            if (Failed)
                yield return Model.CrawlStatus.Failed;
            if (Disabled)
                yield return Model.CrawlStatus.Disabled;
        }

        public static bool AreSame(StatusFilter x, StatusFilter y)
        {
            if (x is null)
                return y is null || !y.GetSelectedOptions().Any();
            if (y is null)
                return !x.GetSelectedOptions().Any();
            return ReferenceEquals(x, y) || (x.GetSelectedOptions().Any() ? (x.IsExclusive == y.IsExclusive && x.GetSelectedOptions().SequenceEqual(y.GetSelectedOptions())) : !y.GetSelectedOptions().Any());
        }

        public bool IsMatch(Model.CrawlStatus statusValue) => IsExclusive ? !GetSelectedOptions().Contains(statusValue) : GetSelectedOptions().Contains(statusValue);

        public BinaryExpression CreateExpression([DisallowNull] ParameterExpression parameterExpression) => IsExclusive ?
            GetSelectedOptions().Select(s => Expression.NotEqual(Expression.Property(parameterExpression, nameof(Model.ICrawlConfigReportItem.StatusValue)), Expression.Constant(s))).Aggregate(Expression.AndAlso) :
            GetSelectedOptions().Select(s => Expression.Equal(Expression.Property(parameterExpression, nameof(Model.ICrawlConfigReportItem.StatusValue)), Expression.Constant(s))).Aggregate(Expression.OrElse);
    }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
}
