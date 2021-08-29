using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.Local.FileSystems
{
    public class ListItemViewModel : DbEntityRowViewModel<FileSystemListItem>
    {
        public ListItemViewModel([DisallowNull] FileSystemListItem entity)
            : base(entity)
        {
            // TODO: Initialize property values
        }
    }
    public class RowItemViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : FileSystemRow
    {
        public RowItemViewModel([DisallowNull] TEntity entity)
            : base(entity)
        {
            // TODO: Initialize property values
        }
    }
}
