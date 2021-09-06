using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class TagDefinitionRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : DbEntity, ITagDefinitionRow
    {
        #region Name Property Members

        /// <summary>
        /// Identifies the <see cref="Name"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NameProperty = ColumnPropertyBuilder<string, TagDefinitionRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ITagDefinitionRow.Name))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as TagDefinitionRowViewModel<TEntity>)?.OnNamePropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string Name { get => GetValue(NameProperty) as string; set => SetValue(NameProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Name"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Name"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Name"/> property.</param>
        protected virtual void OnNamePropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region Description Property Members

        /// <summary>
        /// Identifies the <see cref="Description"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DescriptionProperty = ColumnPropertyBuilder<string, TagDefinitionRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ITagDefinitionRow.Description))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as TagDefinitionRowViewModel<TEntity>)?.OnDescriptionPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string Description { get => GetValue(DescriptionProperty) as string; set => SetValue(DescriptionProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Description"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Description"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Description"/> property.</param>
        protected virtual void OnDescriptionPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region IsInactive Property Members

        /// <summary>
        /// Identifies the <see cref="IsInactive"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsInactiveProperty = ColumnPropertyBuilder<bool, TagDefinitionRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ITagDefinitionRow.IsInactive))
            .DefaultValue(false)
            .OnChanged((d, oldValue, newValue) => (d as TagDefinitionRowViewModel<TEntity>)?.OnIsInactivePropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public bool IsInactive { get => (bool)GetValue(IsInactiveProperty); set => SetValue(IsInactiveProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="IsInactive"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="IsInactive"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="IsInactive"/> property.</param>
        protected virtual void OnIsInactivePropertyChanged(bool oldValue, bool newValue) { }

        #endregion

        public TagDefinitionRowViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            Name = entity.Name;
            Description = entity.Description;
            IsInactive = entity.IsInactive;
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(ITagDefinitionRow.Name):
                    Dispatcher.CheckInvoke(() => Name = Entity.Name);
                    break;
                case nameof(ITagDefinitionRow.Description):
                    Dispatcher.CheckInvoke(() => Description = Entity.Description);
                    break;
                case nameof(ITagDefinitionRow.IsInactive):
                    Dispatcher.CheckInvoke(() => IsInactive = Entity.IsInactive);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
