using System;

namespace FsInfoCat.Desktop.ViewModel.Filter
{
    public interface ITimeReference : IFilter, IComparable<DateTime?>
    {
    }
}
