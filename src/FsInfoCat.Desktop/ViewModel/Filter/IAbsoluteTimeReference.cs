using System;

namespace FsInfoCat.Desktop.ViewModel.Filter
{
    public interface IAbsoluteTimeReference : ITimeReference
    {
        DateTime Value { get; }
    }
}
