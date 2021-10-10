using System;

namespace FsInfoCat.ExpressionFilter
{
    public interface ITimeReference : IFilter, IComparable<DateTime?>
    {
    }
}
