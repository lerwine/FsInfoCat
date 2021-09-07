using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System;

namespace FsInfoCat.Desktop.LocalData
{
    public interface ILocalDbEntityRowViewModel : IDbEntityRowViewModel
    {
        DateTime? LastSynchronizedOn { get; }
    }

    public interface ILocalDbEntityRowViewModel<TEntity> : IDbEntityRowViewModel<TEntity>, ILocalDbEntityRowViewModel where TEntity : LocalDbEntity { }
}
