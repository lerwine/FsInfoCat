using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class FileWithBinaryPropertiesViewModel<TEntity> : FileRowViewModel<TEntity>, ICrudEntityRowViewModel<TEntity>
        where TEntity : DbEntity, IFileListItemWithBinaryProperties
    {
        #region Edit Property Members

        /// <summary>
        /// Occurs when the <see cref="Edit">Edit Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> EditCommand;

        private static readonly DependencyPropertyKey EditPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Edit),
            typeof(Commands.RelayCommand), typeof(FileWithBinaryPropertiesViewModel<TEntity>), new PropertyMetadata(null));

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
            typeof(Commands.RelayCommand), typeof(FileWithBinaryPropertiesViewModel<TEntity>), new PropertyMetadata(null));

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
        #region Length Property Members

        private static readonly DependencyPropertyKey LengthPropertyKey = ColumnPropertyBuilder<long, FileWithBinaryPropertiesViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IFileListItemWithBinaryProperties.Length))
            .DefaultValue(0L)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Length"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LengthProperty = LengthPropertyKey.DependencyProperty;

        public long Length { get => (long)GetValue(LengthProperty); private set => SetValue(LengthPropertyKey, value); }

        #endregion
        #region Hash Property Members

        private static readonly DependencyPropertyKey HashPropertyKey = ColumnPropertyBuilder<MD5Hash?, FileWithBinaryPropertiesViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IFileListItemWithBinaryProperties.Hash))
            .DefaultValue(null)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Hash"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HashProperty = HashPropertyKey.DependencyProperty;

        public MD5Hash? Hash { get => (MD5Hash?)GetValue(HashProperty); private set => SetValue(HashPropertyKey, value); }

        #endregion
        #region RedundancyCount Property Members

        private static readonly DependencyPropertyKey RedundancyCountPropertyKey = ColumnPropertyBuilder<long, FileWithBinaryPropertiesViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IFileListItemWithBinaryProperties.RedundancyCount))
            .DefaultValue(0L)
            .OnChanged((d, oldValue, newValue) => (d as FileWithBinaryPropertiesViewModel<TEntity>)?.OnRedundancyCountPropertyChanged(oldValue, newValue))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="RedundancyCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RedundancyCountProperty = RedundancyCountPropertyKey.DependencyProperty;

        public long RedundancyCount { get => (long)GetValue(RedundancyCountProperty); private set => SetValue(RedundancyCountPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="RedundancyCount"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="RedundancyCount"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="RedundancyCount"/> property.</param>
        protected virtual void OnRedundancyCountPropertyChanged(long oldValue, long newValue) { }

        #endregion
        #region ComparisonCount Property Members

        private static readonly DependencyPropertyKey ComparisonCountPropertyKey = ColumnPropertyBuilder<long, FileWithBinaryPropertiesViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IFileListItemWithBinaryProperties.ComparisonCount))
            .DefaultValue(0L)
            .OnChanged((d, oldValue, newValue) => (d as FileWithBinaryPropertiesViewModel<TEntity>)?.OnComparisonCountPropertyChanged(oldValue, newValue))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ComparisonCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ComparisonCountProperty = ComparisonCountPropertyKey.DependencyProperty;

        public long ComparisonCount { get => (long)GetValue(ComparisonCountProperty); private set => SetValue(ComparisonCountPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="ComparisonCount"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ComparisonCount"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ComparisonCount"/> property.</param>
        protected virtual void OnComparisonCountPropertyChanged(long oldValue, long newValue) { }

        #endregion
        #region AccessErrorCount Property Members

        private static readonly DependencyPropertyKey AccessErrorCountPropertyKey = ColumnPropertyBuilder<long, FileWithBinaryPropertiesViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IFileListItemWithBinaryProperties.AccessErrorCount))
            .DefaultValue(0L)
            .OnChanged((d, oldValue, newValue) => (d as FileWithBinaryPropertiesViewModel<TEntity>)?.OnAccessErrorCountPropertyChanged(oldValue, newValue))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="AccessErrorCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AccessErrorCountProperty = AccessErrorCountPropertyKey.DependencyProperty;

        public long AccessErrorCount { get => (long)GetValue(AccessErrorCountProperty); private set => SetValue(AccessErrorCountPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="AccessErrorCount"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="AccessErrorCount"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="AccessErrorCount"/> property.</param>
        protected virtual void OnAccessErrorCountPropertyChanged(long oldValue, long newValue) { }

        #endregion
        #region PersonalTagCount Property Members

        private static readonly DependencyPropertyKey PersonalTagCountPropertyKey = ColumnPropertyBuilder<long, FileWithBinaryPropertiesViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IFileListItemWithBinaryProperties.PersonalTagCount))
            .DefaultValue(0L)
            .OnChanged((d, oldValue, newValue) => (d as FileWithBinaryPropertiesViewModel<TEntity>)?.OnPersonalTagCountPropertyChanged(oldValue, newValue))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="PersonalTagCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PersonalTagCountProperty = PersonalTagCountPropertyKey.DependencyProperty;

        public long PersonalTagCount { get => (long)GetValue(PersonalTagCountProperty); private set => SetValue(PersonalTagCountPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="PersonalTagCount"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="PersonalTagCount"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="PersonalTagCount"/> property.</param>
        protected virtual void OnPersonalTagCountPropertyChanged(long oldValue, long newValue) { }

        #endregion
        #region SharedTagCount Property Members

        private static readonly DependencyPropertyKey SharedTagCountPropertyKey = ColumnPropertyBuilder<long, FileWithBinaryPropertiesViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IFileListItemWithBinaryProperties.SharedTagCount))
            .DefaultValue(0L)
            .OnChanged((d, oldValue, newValue) => (d as FileWithBinaryPropertiesViewModel<TEntity>)?.OnSharedTagCountPropertyChanged(oldValue, newValue))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="SharedTagCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SharedTagCountProperty = SharedTagCountPropertyKey.DependencyProperty;

        public long SharedTagCount { get => (long)GetValue(SharedTagCountProperty); private set => SetValue(SharedTagCountPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="SharedTagCount"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="SharedTagCount"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="SharedTagCount"/> property.</param>
        protected virtual void OnSharedTagCountPropertyChanged(long oldValue, long newValue) { }

        #endregion

        public FileWithBinaryPropertiesViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            SetValue(EditPropertyKey, new Commands.RelayCommand(RaiseEditCommand));
            SetValue(DeletePropertyKey, new Commands.RelayCommand(RaiseDeleteCommand));
            Length = entity.Length;
            Hash = entity.Hash;
            RedundancyCount = entity.RedundancyCount;
            ComparisonCount = entity.ComparisonCount;
            AccessErrorCount = entity.AccessErrorCount;
            PersonalTagCount = entity.PersonalTagCount;
            SharedTagCount = entity.SharedTagCount;
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IFileListItemWithBinaryProperties.Length):
                    Dispatcher.CheckInvoke(() => Length = Entity.Length);
                    break;
                case nameof(IFileListItemWithBinaryProperties.Hash):
                    Dispatcher.CheckInvoke(() => Hash = Entity.Hash);
                    break;
                case nameof(IFileListItemWithBinaryProperties.RedundancyCount):
                    Dispatcher.CheckInvoke(() => RedundancyCount = Entity.RedundancyCount);
                    break;
                case nameof(IFileListItemWithBinaryProperties.ComparisonCount):
                    Dispatcher.CheckInvoke(() => ComparisonCount = Entity.ComparisonCount);
                    break;
                case nameof(IFileListItemWithBinaryProperties.AccessErrorCount):
                    Dispatcher.CheckInvoke(() => AccessErrorCount = Entity.AccessErrorCount);
                    break;
                case nameof(IFileListItemWithBinaryProperties.PersonalTagCount):
                    Dispatcher.CheckInvoke(() => PersonalTagCount = Entity.PersonalTagCount);
                    break;
                case nameof(IFileListItemWithBinaryProperties.SharedTagCount):
                    Dispatcher.CheckInvoke(() => SharedTagCount = Entity.SharedTagCount);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
