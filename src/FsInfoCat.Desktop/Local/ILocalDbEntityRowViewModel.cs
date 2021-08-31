using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System;

namespace FsInfoCat.Desktop.Local
{
    public interface ILocalDbEntityRowViewModel : IDbEntityRowViewModel
    {
        DateTime? LastSynchronizedOn { get; }
    }

    public interface ILocalDbEntityRowViewModel<TEntity> : IDbEntityRowViewModel<TEntity>, ILocalDbEntityRowViewModel where TEntity : LocalDbEntity { }
}
