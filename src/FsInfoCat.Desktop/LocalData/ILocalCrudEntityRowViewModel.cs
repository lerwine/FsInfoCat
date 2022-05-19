using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;

namespace FsInfoCat.Desktop.LocalData
{
    public interface ILocalCrudEntityRowViewModel : ICrudEntityRowViewModel, ILocalDbEntityRowViewModel { }

    public interface ILocalCrudEntityRowViewModel<TEntity> : ICrudEntityRowViewModel<TEntity>, ILocalDbEntityRowViewModel<TEntity>, ILocalCrudEntityRowViewModel where TEntity : LocalDbEntity { }
}
