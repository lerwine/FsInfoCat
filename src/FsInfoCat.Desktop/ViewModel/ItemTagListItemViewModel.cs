using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class ItemTagListItemViewModel<TEntity> : ItemTagRowViewModel<TEntity>
        where TEntity : DbEntity, IItemTagListItem
    {
        #region Name Property Members

        private static readonly DependencyPropertyKey NamePropertyKey = DependencyPropertyBuilder<ItemTagRowViewModel<TEntity>, string>
            .Register(nameof(Name))
            .DefaultValue("")
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Name"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NameProperty = NamePropertyKey.DependencyProperty;

        public string Name { get => GetValue(NameProperty) as string; private set => SetValue(NamePropertyKey, value); }

        #endregion

        public ItemTagListItemViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            Name = entity.Name;
        }

        protected override void OnEntityPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == nameof(Name))
                Dispatcher.CheckInvoke(() => Name = Entity.Notes);
            else
                base.OnEntityPropertyChanged(sender, args);
        }
    }
}
