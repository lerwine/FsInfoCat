using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class TagDefinitionEditViewModel<TEntity, TItem> : TagDefinitionRowViewModel<TEntity>
        where TEntity : DbEntity, ITagDefinition
        where TItem : DbEntity, ITagDefinitionRow
    {
        #region ListItem Property Members

        private static readonly DependencyPropertyKey ListItemPropertyKey = DependencyPropertyBuilder<TagDefinitionEditViewModel<TEntity, TItem>, TItem>
            .Register(nameof(ListItem))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ListItem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ListItemProperty = ListItemPropertyKey.DependencyProperty;

        public TItem ListItem { get => (TItem)GetValue(ListItemProperty); private set => SetValue(ListItemPropertyKey, value); }

        #endregion
        #region IsNew Property Members

        private static readonly DependencyPropertyKey IsNewPropertyKey = DependencyPropertyBuilder<TagDefinitionEditViewModel<TEntity, TItem>, bool>
            .Register(nameof(IsNew))
            .DefaultValue(false)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="IsNew"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsNewProperty = IsNewPropertyKey.DependencyProperty;

        public bool IsNew { get => (bool)GetValue(IsNewProperty); private set => SetValue(IsNewPropertyKey, value); }

        #endregion

        public TagDefinitionEditViewModel([DisallowNull] TEntity entity, TItem listItem) : base(entity)
        {
            IsNew = (ListItem = listItem) is null;
        }
    }
}
