using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class ItemTagListItemViewModel<TEntity> : ItemTagRowViewModel<TEntity>
        where TEntity : DbEntity, IItemTagListItem
    {
        #region Name Property Members

        private static readonly DependencyPropertyKey NamePropertyKey = DependencyPropertyBuilder<ItemTagListItemViewModel<TEntity>, string>
            .Register(nameof(Name))
            .DefaultValue("")
            .CoerseWith(NormalizedOrEmptyStringCoersion.Default)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Name"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NameProperty = NamePropertyKey.DependencyProperty;

        public string Name { get => GetValue(NameProperty) as string; private set => SetValue(NamePropertyKey, value); }

        #endregion
        #region Description Property Members

        private static readonly DependencyPropertyKey DescriptionPropertyKey = DependencyPropertyBuilder<ItemTagListItemViewModel<TEntity>, string>
            .Register(nameof(Description))
            .DefaultValue("")
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Description"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DescriptionProperty = DescriptionPropertyKey.DependencyProperty;

        public string Description { get => GetValue(DescriptionProperty) as string; private set => SetValue(DescriptionPropertyKey, value); }

        #endregion

        public ItemTagListItemViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            Name = entity.Name;
            Description = entity.Description;
        }

        protected override void OnEntityPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            switch (args.PropertyName)
            {
                case nameof(IItemTagListItem.Name):
                    Name = Entity.Name;
                    break;
                case nameof(IItemTagListItem.Description):
                    Description = Entity.Description;
                    break;
                default:
                    base.OnEntityPropertyChanged(sender, args);
                    break;
            }
        }
    }
}
