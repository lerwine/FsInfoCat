using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using LinqExpression = System.Linq.Expressions.Expression;

namespace FsInfoCat.Desktop.ViewModel.Filter
{
    public abstract class OrFilter<TEntity> : Filter<TEntity>
        where TEntity : class
    {
        #region Filters Property Members

        /// <summary>
        /// Identifies the <see cref="Filters"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FiltersProperty = DependencyPropertyBuilder<OrFilter<TEntity>, ObservableCollection<Filter<TEntity>>>
            .Register(nameof(Filters))
            .CoerseWith(baseValue => (baseValue as ObservableCollection<Filter<TEntity>>) ?? new())
            .AsReadWrite();

        public ObservableCollection<Filter<TEntity>> Filters { get => (ObservableCollection<Filter<TEntity>>)GetValue(FiltersProperty); set => SetValue(FiltersProperty, value); }

        #endregion

        protected OrFilter()
        {
            Filters = new();
        }

        public override BinaryExpression CreateExpression([DisallowNull] ParameterExpression parameterExpression)
        {
            ObservableCollection<Filter<TEntity>> filters = Filters;
            return filters?.Where(f => f is not null).Select(f => f.CreateExpression(parameterExpression)).Where(e => e is not null).Aggregate(LinqExpression.AndAlso);
        }

        public override bool IsMatch(TEntity entity)
        {
            ObservableCollection<Filter<TEntity>> filters = Filters;
            return filters is not null && filters.All(f => f is null || f.IsMatch(entity));
        }
    }
}
