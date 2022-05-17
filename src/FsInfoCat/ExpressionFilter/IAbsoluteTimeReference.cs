using System;

namespace FsInfoCat.ExpressionFilter
{
    // TODO: Document IAbsoluteTimeReference interface
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface IAbsoluteTimeReference : ITimeReference
    {
        DateTime Value { get; }
    }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
}
