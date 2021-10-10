using System;

namespace FsInfoCat.ExpressionFilter
{
    public interface IAbsoluteTimeReference : ITimeReference
    {
        DateTime Value { get; }
    }
}
