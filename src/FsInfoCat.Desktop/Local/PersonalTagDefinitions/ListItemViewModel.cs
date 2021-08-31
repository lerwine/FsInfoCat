using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System;
using System.Windows;

namespace FsInfoCat.Desktop.Local.PersonalTagDefinitions
{
    public class ListItemViewModel : TagDefinitionListItemViewModel<PersonalTagDefinitionListItem>
    {
        #region EditButtonClick Property Members

        /// <summary>
        /// Occurs when the <see cref="EditButtonClick">EditButtonClick Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> EditButtonClicked;

        private static readonly DependencyPropertyKey EditButtonClickPropertyKey = DependencyProperty.RegisterReadOnly(nameof(EditButtonClick),
            typeof(Commands.RelayCommand), typeof(ListItemViewModel), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="EditButtonClick"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EditButtonClickProperty = EditButtonClickPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand EditButtonClick => (Commands.RelayCommand)GetValue(EditButtonClickProperty);

        /// <summary>
        /// Called when the EditButtonClick event is raised by <see cref="EditButtonClick" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="EditButtonClick" />.</param>
        private void RaiseEditButtonClicked(object parameter)
        {
            try { OnEditButtonClicked(parameter); }
            finally { EditButtonClicked?.Invoke(this, new(parameter)); }
        }

        /// <summary>
        /// Called when the <see cref="EditButtonClick">EditButtonClick Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="EditButtonClick" />.</param>
        protected virtual void OnEditButtonClicked(object parameter)
        {
            // TODO: Implement OnEditButtonClicked Logic
        }

        #endregion
        #region DeleteButtonClick Property Members

        /// <summary>
        /// Occurs when the <see cref="DeleteButtonClick">DeleteButtonClick Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> DeleteButtonClicked;

        private static readonly DependencyPropertyKey DeleteButtonClickPropertyKey = DependencyProperty.RegisterReadOnly(nameof(DeleteButtonClick),
            typeof(Commands.RelayCommand), typeof(ListItemViewModel), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="DeleteButtonClick"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DeleteButtonClickProperty = DeleteButtonClickPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand DeleteButtonClick => (Commands.RelayCommand)GetValue(DeleteButtonClickProperty);

        /// <summary>
        /// Called when the DeleteButtonClick event is raised by <see cref="DeleteButtonClick" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="DeleteButtonClick" />.</param>
        private void RaiseDeleteButtonClicked(object parameter)
        {
            try { OnDeleteButtonClicked(parameter); }
            finally { DeleteButtonClicked?.Invoke(this, new(parameter)); }
        }

        /// <summary>
        /// Called when the <see cref="DeleteButtonClick">DeleteButtonClick Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="DeleteButtonClick" />.</param>
        protected virtual void OnDeleteButtonClicked(object parameter)
        {
            // TODO: Implement OnDeleteButtonClicked Logic
        }

        #endregion
        #region ShowTaggedVolumesButtonClick Property Members

        /// <summary>
        /// Occurs when the <see cref="ShowTaggedVolumesButtonClick">ShowTaggedVolumesButtonClick Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> ShowTaggedVolumesButtonClicked;

        private static readonly DependencyPropertyKey ShowTaggedVolumesButtonClickPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ShowTaggedVolumesButtonClick),
            typeof(Commands.RelayCommand), typeof(ListItemViewModel), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ShowTaggedVolumesButtonClick"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowTaggedVolumesButtonClickProperty = ShowTaggedVolumesButtonClickPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand ShowTaggedVolumesButtonClick => (Commands.RelayCommand)GetValue(ShowTaggedVolumesButtonClickProperty);

        /// <summary>
        /// Called when the ShowTaggedVolumesButtonClick event is raised by <see cref="ShowTaggedVolumesButtonClick" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="ShowTaggedVolumesButtonClick" />.</param>
        private void RaiseShowTaggedVolumesButtonClicked(object parameter)
        {
            try { OnShowTaggedVolumesButtonClicked(parameter); }
            finally { ShowTaggedVolumesButtonClicked?.Invoke(this, new(parameter)); }
        }

        /// <summary>
        /// Called when the <see cref="ShowTaggedVolumesButtonClick">ShowTaggedVolumesButtonClick Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="ShowTaggedVolumesButtonClick" />.</param>
        protected virtual void OnShowTaggedVolumesButtonClicked(object parameter)
        {
            // TODO: Implement OnShowTaggedVolumesButtonClicked Logic
        }

        #endregion
        #region ShowTaggedFoldersButtonClick Property Members

        /// <summary>
        /// Occurs when the <see cref="ShowTaggedFoldersButtonClick">ShowTaggedFoldersButtonClick Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> ShowTaggedFoldersButtonClicked;

        private static readonly DependencyPropertyKey ShowTaggedFoldersButtonClickPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ShowTaggedFoldersButtonClick),
            typeof(Commands.RelayCommand), typeof(ListItemViewModel), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ShowTaggedFoldersButtonClick"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowTaggedFoldersButtonClickProperty = ShowTaggedFoldersButtonClickPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand ShowTaggedFoldersButtonClick => (Commands.RelayCommand)GetValue(ShowTaggedFoldersButtonClickProperty);

        /// <summary>
        /// Called when the ShowTaggedFoldersButtonClick event is raised by <see cref="ShowTaggedFoldersButtonClick" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="ShowTaggedFoldersButtonClick" />.</param>
        private void RaiseShowTaggedFoldersButtonClicked(object parameter)
        {
            try { OnShowTaggedFoldersButtonClicked(parameter); }
            finally { ShowTaggedFoldersButtonClicked?.Invoke(this, new(parameter)); }
        }

        /// <summary>
        /// Called when the <see cref="ShowTaggedFoldersButtonClick">ShowTaggedFoldersButtonClick Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="ShowTaggedFoldersButtonClick" />.</param>
        protected virtual void OnShowTaggedFoldersButtonClicked(object parameter)
        {
            // TODO: Implement OnShowTaggedFoldersButtonClicked Logic
        }

        #endregion
        #region ShowTaggedFilesButtonClick Property Members

        /// <summary>
        /// Occurs when the <see cref="ShowTaggedFilesButtonClick">ShowTaggedFilesButtonClick Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> ShowTaggedFilesButtonClicked;

        private static readonly DependencyPropertyKey ShowTaggedFilesButtonClickPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ShowTaggedFilesButtonClick),
            typeof(Commands.RelayCommand), typeof(ListItemViewModel), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ShowTaggedFilesButtonClick"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowTaggedFilesButtonClickProperty = ShowTaggedFilesButtonClickPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand ShowTaggedFilesButtonClick => (Commands.RelayCommand)GetValue(ShowTaggedFilesButtonClickProperty);

        /// <summary>
        /// Called when the ShowTaggedFilesButtonClick event is raised by <see cref="ShowTaggedFilesButtonClick" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="ShowTaggedFilesButtonClick" />.</param>
        private void RaiseShowTaggedFilesButtonClicked(object parameter)
        {
            try { OnShowTaggedFilesButtonClicked(parameter); }
            finally { ShowTaggedFilesButtonClicked?.Invoke(this, new(parameter)); }
        }

        /// <summary>
        /// Called when the <see cref="ShowTaggedFilesButtonClick">ShowTaggedFilesButtonClick Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="ShowTaggedFilesButtonClick" />.</param>
        protected virtual void OnShowTaggedFilesButtonClicked(object parameter)
        {
            // TODO: Implement OnShowTaggedFilesButtonClicked Logic
        }

        #endregion
        #region UpstreamId Property Members

        private static readonly DependencyPropertyKey UpstreamIdPropertyKey = DependencyProperty.RegisterReadOnly(nameof(UpstreamId), typeof(Guid?),
            typeof(ListItemViewModel), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="UpstreamId"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty UpstreamIdProperty = UpstreamIdPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public Guid? UpstreamId { get => (Guid?)GetValue(UpstreamIdProperty); private set => SetValue(UpstreamIdPropertyKey, value); }

        #endregion
        #region LastSynchronizedOn Property Members

        private static readonly DependencyPropertyKey LastSynchronizedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastSynchronizedOn), typeof(DateTime?),
            typeof(ListItemViewModel), new PropertyMetadata(null));

        /// <summary>
        /// Gets the value of the primary key for the corresponding <see cref="Upstream.IUpstreamDbEntity">upstream (remote) database entity</see>.
        /// </summary>
        /// <value>
        /// The value of the primary key of the corresponding <see cref="Upstream.IUpstreamDbEntity">upstream (remote) database entity</see>;
        /// otherwise, <see langword="null" /> if there is no corresponding entity.
        /// </value>
        /// <remarks>
        /// If this value is <see langword="null" />, then <see cref="LastSynchronizedOn" /> should also be <see langword="null" />.
        /// Likewise, if this is not <see langword="null" />, then <see cref="LastSynchronizedOn" /> should not be <see langword="null" />, either.
        /// </remarks>
        public static readonly DependencyProperty LastSynchronizedOnProperty = LastSynchronizedOnPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the date and time when the current entity was sychronized with the corresponding <see cref="Upstream.IUpstreamDbEntity">upstream (remote) database entity</see>.
        /// </summary>
        /// <value>
        /// date and time when the current entity was sychronized with the corresponding <see cref="Upstream.IUpstreamDbEntity">upstream (remote) database entity</see>;
        /// otherwise, <see langword="null" /> if there is no corresponding entity.
        /// </value>
        /// <remarks>
        /// If this value is <see langword="null" />, then <see cref="UpstreamId" /> should also be <see langword="null" />.
        /// Likewise, if this is not <see langword="null" />, then <see cref="UpstreamId" /> should not be <see langword="null" />, either.
        /// </remarks>
        public DateTime? LastSynchronizedOn { get => (DateTime?)GetValue(LastSynchronizedOnProperty); private set => SetValue(LastSynchronizedOnPropertyKey, value); }

        #endregion

        public ListItemViewModel(PersonalTagDefinitionListItem entity) : base(entity)
        {
            SetValue(EditButtonClickPropertyKey, new Commands.RelayCommand(RaiseEditButtonClicked));
            SetValue(DeleteButtonClickPropertyKey, new Commands.RelayCommand(RaiseDeleteButtonClicked));
            SetValue(ShowTaggedVolumesButtonClickPropertyKey, new Commands.RelayCommand(RaiseShowTaggedVolumesButtonClicked));
            SetValue(ShowTaggedFoldersButtonClickPropertyKey, new Commands.RelayCommand(RaiseShowTaggedFoldersButtonClicked));
            SetValue(ShowTaggedFilesButtonClickPropertyKey, new Commands.RelayCommand(RaiseShowTaggedFilesButtonClicked));
            UpstreamId = entity.UpstreamId;
            LastSynchronizedOn = entity.LastSynchronizedOn;
        }
    }
}
