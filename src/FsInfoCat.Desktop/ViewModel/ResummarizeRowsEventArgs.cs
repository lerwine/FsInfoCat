using System.Collections.Generic;

namespace FsInfoCat.Desktop.ViewModel
{
    public class ResummarizeRowsEventArgs(ColumnProperty[] toSummarize)
    {
        public IReadOnlyList<ColumnProperty> ToSummarize { get; } = toSummarize;
    }
}
