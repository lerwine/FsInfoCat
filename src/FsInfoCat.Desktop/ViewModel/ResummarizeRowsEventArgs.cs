using System.Collections.Generic;

namespace FsInfoCat.Desktop.ViewModel
{
    public class ResummarizeRowsEventArgs
    {
        public IReadOnlyList<ColumnProperty> ToSummarize { get; }

        public ResummarizeRowsEventArgs(ColumnProperty[] toSummarize)
        {
            ToSummarize = toSummarize;
        }
    }
}
