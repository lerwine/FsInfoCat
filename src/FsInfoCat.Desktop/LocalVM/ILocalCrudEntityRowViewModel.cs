using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;

namespace FsInfoCat.Desktop.LocalVM
{
    public interface ILocalCrudEntityRowViewModel : ICrudEntityRowViewModel, ILocalDbEntityRowViewModel { }

    public interface ILocalCrudEntityRowViewModel<TEntity> : ICrudEntityRowViewModel<TEntity>, ILocalDbEntityRowViewModel<TEntity>, ILocalCrudEntityRowViewModel where TEntity : LocalDbEntity { }
}
