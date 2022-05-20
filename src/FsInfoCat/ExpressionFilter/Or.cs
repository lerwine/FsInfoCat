using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;


namespace FsInfoCat.ExpressionFilter
{
    // TODO: Document Or class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class Or<T> : Filter<T>
    {
        private readonly IPropertyChangeTracker<ObservableCollection<Filter<T>>> _filters;

        public ObservableCollection<Filter<T>> Filters { get => _filters.GetValue(); set => _filters.SetValue(value); }

        public Or()
        {
            _filters = AddChangeTracker(nameof(Filters), new ObservableCollection<Filter<T>>());
        }

        public override bool IsMatch(Model.ICrawlConfigReportItem item)
        {
            if (item is null)
                return false;
            IEnumerable<Filter<T>> filters = Filters.Where(f => f is not null).Distinct();
            return filters.Any(f => f.IsMatch(item)) || !filters.Any();
        }

        public override BinaryExpression CreateExpression([DisallowNull] ParameterExpression parameterExpression) => Filters.Where(f => f is not null).Distinct().Select(f => f.CreateExpression(parameterExpression))
            .Where(e => e is not null).Aggregate(Expression.OrElse);
    }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
}
