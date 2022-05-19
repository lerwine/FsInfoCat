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

        protected override void OnEntityPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            // TODO: Handle property changes
            switch (args.PropertyName)
            {
                case nameof(Model.IItemTag.Tagged):
                    //Dispatcher.CheckInvoke(() => Notes = Entity.Notes);
                    break;
                case nameof(Model.IItemTag.Definition):
                    //Dispatcher.CheckInvoke(() => Notes = Entity.Notes);
                    break;
                default:
                    base.OnEntityPropertyChanged(sender, args);
                    break;
            }
        }
    }
}
