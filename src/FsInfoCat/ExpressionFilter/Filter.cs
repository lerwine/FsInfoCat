using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;


namespace FsInfoCat.ExpressionFilter
{
    // TODO: Document Filter class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public abstract class Filter<T> : NotifyDataErrorInfo, IFilter
    {
        public abstract bool IsMatch(ICrawlConfigReportItem item);

        public abstract BinaryExpression CreateExpression([DisallowNull] ParameterExpression parameterExpression);
    }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
}
