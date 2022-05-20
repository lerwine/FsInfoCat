using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class RedundantSetListItemViewModel<TEntity> : RedundantSetRowViewModel<TEntity>, ICrudEntityRowViewModel<TEntity>
        where TEntity : Model.DbEntity, Model.IRedundantSetListItem
    {
        #region Open Command Property Members

        /// <summary>
        /// Occurs when the <see cref="Open"/> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> OpenCommand;

        private static readonly DependencyPropertyKey OpenPropertyKey = DependencyPropertyBuilder<RedundantSetListItemViewModel<TEntity>, Commands.RelayCommand>
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
            typeof(Commands.RelayCommand), typeof(RedundantSetListItemViewModel<TEntity>), new PropertyMetadata(null));

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
            typeof(Commands.RelayCommand), typeof(RedundantSetListItemViewModel<TEntity>), new PropertyMetadata(null));

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

        private static readonly DependencyPropertyKey LengthPropertyKey = ColumnPropertyBuilder<long, RedundantSetListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IRedundantSetListItem.Length))
            .DefaultValue(0L)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Length"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LengthProperty = LengthPropertyKey.DependencyProperty;

        public long Length { get => (long)GetValue(LengthProperty); private set => SetValue(LengthPropertyKey, value); }

        #endregion
        #region Hash Property Members

        private static readonly DependencyPropertyKey HashPropertyKey = ColumnPropertyBuilder<Model.MD5Hash?, RedundantSetListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IRedundantSetListItem.Hash))
            .DefaultValue(null)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Hash"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HashProperty = HashPropertyKey.DependencyProperty;

        public Model.MD5Hash? Hash { get => (Model.MD5Hash?)GetValue(HashProperty); private set => SetValue(HashPropertyKey, value); }

        #endregion
        #region Status Property Members

        /// <summary>
        /// Identifies the <see cref="Status"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusProperty = DependencyPropertyBuilder<RedundantSetListItemViewModel<TEntity>, RedundancyRemediationStatus>
            .Register(nameof(Status))
            .DefaultValue(RedundancyRemediationStatus.Unconfirmed)
            .AsReadWrite();

        public RedundancyRemediationStatus Status { get => (RedundancyRemediationStatus)GetValue(StatusProperty); set => SetValue(StatusProperty, value); }

        #endregion
        #region RedundancyCount Property Members

        private static readonly DependencyPropertyKey RedundancyCountPropertyKey = ColumnPropertyBuilder<long, RedundantSetListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IRedundantSetListItem.RedundancyCount))
            .DefaultValue(0L)
            .OnChanged((d, oldValue, newValue) => (d as RedundantSetListItemViewModel<TEntity>)?.OnRedundancyCountPropertyChanged(newValue))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="RedundancyCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RedundancyCountProperty = RedundancyCountPropertyKey.DependencyProperty;

        public long RedundancyCount { get => (long)GetValue(RedundancyCountProperty); private set => SetValue(RedundancyCountPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="RedundancyCount"/> dependency property has changed.
        /// </summary>
        /// <param name="newValue">The new value of the <see cref="RedundancyCount"/> property.</param>
        ///
        private void OnRedundancyCountPropertyChanged(long newValue) => Delete.IsEnabled = newValue > 0L;

        #endregion

        public RedundantSetListItemViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            SetValue(OpenPropertyKey, new Commands.RelayCommand(RaiseOpenCommand));
            SetValue(EditPropertyKey, new Commands.RelayCommand(RaiseEditCommand));
            SetValue(DeletePropertyKey, new Commands.RelayCommand(RaiseDeleteCommand));
            Length = entity.Length;
            Hash = entity.Hash;
            Status = entity.Status;
            RedundancyCount = entity.RedundancyCount;
        }
    }
}
