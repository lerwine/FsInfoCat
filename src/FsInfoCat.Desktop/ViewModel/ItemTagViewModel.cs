using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Desktop.ViewModel
{
    public class ItemTagViewModel<TEntity>([DisallowNull] TEntity entity) : DbEntityRowViewModel<TEntity>(entity)
        where TEntity : Model.DbEntity, Model.IItemTag
    {
    }
}
