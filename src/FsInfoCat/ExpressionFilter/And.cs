using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;


namespace FsInfoCat.ExpressionFilter
{
    // TODO: Document And class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class And<T> : Filter<T>
    {
        private readonly IPropertyChangeTracker<ObservableCollection<Filter<T>>> _filters;

        public ObservableCollection<Filter<T>> Filters { get => _filters.GetValue(); set => _filters.SetValue(value); }

        public And()
        {
            _filters = AddChangeTracker(nameof(Filters), new ObservableCollection<Filter<T>>());
        }

        public override bool IsMatch(Model.ICrawlConfigReportItem item) => item is not null && Filters.Where(f => f is not null).Distinct().All(f => f.IsMatch(item));

        public override BinaryExpression CreateExpression([DisallowNull] ParameterExpression parameterExpression) => Filters.Where(f => f is not null).Distinct().Select(f => f.CreateExpression(parameterExpression))
            .Where(e => e is not null).Aggregate(Expression.AndAlso);
    }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
}
