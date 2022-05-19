using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class TagDefinitionListItemViewModel<TEntity> : TagDefinitionRowViewModel<TEntity>, ICrudEntityRowViewModel<TEntity>
        where TEntity : Model.DbEntity, Model.ITagDefinitionListItem
    {
        #region Open Command Property Members

        /// <summary>
        /// Occurs when the <see cref="Open"/> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> OpenCommand;

        private static readonly DependencyPropertyKey OpenPropertyKey = DependencyPropertyBuilder<TagDefinitionListItemViewModel<TEntity>, Commands.RelayCommand>
            .Register(nameof(Open))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Open"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OpenProperty = OpenPropertyKey.DependencyProperty;

        public Commands.RelayCommand Open => (Commands.RelayCommand)GetValue(OpenProperty);

        /// <summary>
        /// Called when the Open event is raised by <see cref="Open" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="Open" />.</param>
        protected void RaiseOpenCommand(object parameter) => OpenCommand?.Invoke(this, new(parameter));

        #endregion
        #region Edit Property Members

        /// <summary>
        /// Occurs when the <see cref="Edit">Edit Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> EditCommand;

        private static readonly DependencyPropertyKey EditPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Edit),
            typeof(Commands.RelayCommand), typeof(TagDefinitionListItemViewModel<TEntity>), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Edit"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EditProperty = EditPropertyKey.DependencyProperty;

        public Commands.RelayCommand Edit => (Commands.RelayCommand)GetValue(EditProperty);

        /// <summary>
        /// Called when the Edit event is raised by <see cref="Edit" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="Edit" />.</param>
        protected virtual void RaiseEditCommand(object parameter) => EditCommand?.Invoke(this, new(parameter));

        #endregion
        #region Delete Property Members

        /// <summary>
        /// Occurs when the <see cref="Delete">Delete Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> DeleteCommand;

        private static readonly DependencyPropertyKey DeletePropertyKey = DependencyProperty.RegisterReadOnly(nameof(Delete),
            typeof(Commands.RelayCommand), typeof(TagDefinitionListItemViewModel<TEntity>), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Delete"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DeleteProperty = DeletePropertyKey.DependencyProperty;

        public Commands.RelayCommand Delete => (Commands.RelayCommand)GetValue(DeleteProperty);

        /// <summary>
        /// Called when the Delete event is raised by <see cref="Delete" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="Delete" />.</param>
        protected virtual void RaiseDeleteCommand(object parameter) => DeleteCommand?.Invoke(this, new(parameter));

        #endregion
        #region FileTagCount Property Members

        private static readonly DependencyPropertyKey FileTagCountPropertyKey = ColumnPropertyBuilder<long, TagDefinitionListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.ITagDefinitionListItem.FileTagCount))
            .DefaultValue(0L)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="FileTagCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FileTagCountProperty = FileTagCountPropertyKey.DependencyProperty;

        public long FileTagCount { get => (long)GetValue(FileTagCountProperty); private set => SetValue(FileTagCountPropertyKey, value); }

        #endregion
        #region SubdirectoryTagCount Property Members

        private static readonly DependencyPropertyKey SubdirectoryTagCountPropertyKey = ColumnPropertyBuilder<long, TagDefinitionListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.ITagDefinitionListItem.SubdirectoryTagCount))
            .DefaultValue(0L)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="SubdirectoryTagCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SubdirectoryTagCountProperty = SubdirectoryTagCountPropertyKey.DependencyProperty;

        public long SubdirectoryTagCount { get => (long)GetValue(SubdirectoryTagCountProperty); private set => SetValue(SubdirectoryTagCountPropertyKey, value); }

        #endregion
        #region VolumeTagCount Property Members

        private static readonly DependencyPropertyKey VolumeTagCountPropertyKey = ColumnPropertyBuilder<long, TagDefinitionListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.ITagDefinitionListItem.VolumeTagCount))
            .DefaultValue(0L)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="VolumeTagCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VolumeTagCountProperty = VolumeTagCountPropertyKey.DependencyProperty;

        public long VolumeTagCount { get => (long)GetValue(VolumeTagCountProperty); private set => SetValue(VolumeTagCountPropertyKey, value); }

        #endregion

        public TagDefinitionListItemViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            SetValue(OpenPropertyKey, new Commands.RelayCommand(RaiseOpenCommand));
            SetValue(EditPropertyKey, new Commands.RelayCommand(RaiseEditCommand));
            SetValue(DeletePropertyKey, new Commands.RelayCommand(RaiseDeleteCommand));
            FileTagCount = entity.FileTagCount;
            SubdirectoryTagCount = entity.SubdirectoryTagCount;
            VolumeTagCount = entity.VolumeTagCount;
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Model.ITagDefinitionListItem.FileTagCount):
                    Dispatcher.CheckInvoke(() => FileTagCount = Entity.FileTagCount);
                    break;
                case nameof(Model.ITagDefinitionListItem.SubdirectoryTagCount):
                    Dispatcher.CheckInvoke(() => SubdirectoryTagCount = Entity.SubdirectoryTagCount);
                    break;
                case nameof(Model.ITagDefinitionListItem.VolumeTagCount):
                    Dispatcher.CheckInvoke(() => VolumeTagCount = Entity.VolumeTagCount);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
