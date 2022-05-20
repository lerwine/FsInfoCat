using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Desktop.ViewModel
{
    public class ItemTagViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : Model.DbEntity, Model.IItemTag
    {
        public ItemTagViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            // TODO: Create properties for related items
        }
    }
}
