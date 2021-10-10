using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;


namespace FsInfoCat.ExpressionFilter
{
    public abstract class Filter<T> : NotifyDataErrorInfo, IFilter
    {
        public abstract bool IsMatch(ICrawlConfigReportItem item);

        public abstract BinaryExpression CreateExpression([DisallowNull] ParameterExpression parameterExpression);
    }
}
