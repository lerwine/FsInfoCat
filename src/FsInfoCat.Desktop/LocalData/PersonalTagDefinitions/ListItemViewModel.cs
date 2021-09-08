using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.LocalData.PersonalTagDefinitions
{
    public class ListItemViewModel : TagDefinitionListItemViewModel<PersonalTagDefinitionListItem>, ILocalCrudEntityRowViewModel<PersonalTagDefinitionListItem>
    {
        #region SynchronizeNow Command Property Members

        /// <summary>
        /// Occurs when the <see cref="SynchronizeNow"/> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> SynchronizeNowCommand;

        private static readonly DependencyPropertyKey SynchronizeNowPropertyKey = DependencyPropertyBuilder<ListItemViewModel, Commands.RelayCommand>
            .Register(nameof(SynchronizeNow))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="SynchronizeNow"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SynchronizeNowProperty = SynchronizeNowPropertyKey.DependencyProperty;

        public Commands.RelayCommand SynchronizeNow => (Commands.RelayCommand)GetValue(SynchronizeNowProperty);

        /// <summary>
        /// Called when the SynchronizeNow event is raised by <see cref="SynchronizeNow" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="SynchronizeNow" />.</param>
        protected void RaiseSynchronizeNowCommand(object parameter) => SynchronizeNowCommand?.Invoke(this, new(parameter));
        // {
        //   try { OnSynchronizeNowCommand(parameter); }
        //   finally { SynchronizeNowCommand?.Invoke(this, new(parameter)); }
        // }

        /// <summary>
        /// Called when the <see cref="SynchronizeNow">SynchronizeNow Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="SynchronizeNow" />.</param>
        protected virtual void OnSynchronizeNowCommand(object parameter)
        {
            // TODO: Implement OnSynchronizeNowCommand Logic
        }

        #endregion
        #region ShowTaggedVolumes Command Property Members

        /// <summary>
        /// Occurs when the <see cref="ShowTaggedVolumes"/> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> ShowTaggedVolumesCommand;

        private static readonly DependencyPropertyKey ShowTaggedVolumesPropertyKey = DependencyPropertyBuilder<ListItemViewModel, Commands.RelayCommand>
            .Register(nameof(ShowTaggedVolumes))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ShowTaggedVolumes"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowTaggedVolumesProperty = ShowTaggedVolumesPropertyKey.DependencyProperty;

        public Commands.RelayCommand ShowTaggedVolumes => (Commands.RelayCommand)GetValue(ShowTaggedVolumesProperty);

        /// <summary>
        /// Called when the ShowTaggedVolumes event is raised by <see cref="ShowTaggedVolumes" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="ShowTaggedVolumes" />.</param>
        protected void RaiseShowTaggedVolumesCommand(object parameter) => ShowTaggedVolumesCommand?.Invoke(this, new(parameter));
        // {
        //   try { OnShowTaggedVolumesCommand(parameter); }
        //   finally { ShowTaggedVolumesCommand?.Invoke(this, new(parameter)); }
        // }

        /// <summary>
        /// Called when the <see cref="ShowTaggedVolumes">ShowTaggedVolumes Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="ShowTaggedVolumes" />.</param>
        protected virtual void OnShowTaggedVolumesCommand(object parameter)
        {
            // TODO: Implement OnShowTaggedVolumesCommand Logic
        }

        #endregion
        #region ShowTaggedSubdirectories Command Property Members

        /// <summary>
        /// Occurs when the <see cref="ShowTaggedSubdirectories"/> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> ShowTaggedSubdirectoriesCommand;

        private static readonly DependencyPropertyKey ShowTaggedSubdirectoriesPropertyKey = DependencyPropertyBuilder<ListItemViewModel, Commands.RelayCommand>
            .Register(nameof(ShowTaggedSubdirectories))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ShowTaggedSubdirectories"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowTaggedSubdirectoriesProperty = ShowTaggedSubdirectoriesPropertyKey.DependencyProperty;

        public Commands.RelayCommand ShowTaggedSubdirectories => (Commands.RelayCommand)GetValue(ShowTaggedSubdirectoriesProperty);

        /// <summary>
        /// Called when the ShowTaggedSubdirectories event is raised by <see cref="ShowTaggedSubdirectories" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="ShowTaggedSubdirectories" />.</param>
        protected void RaiseShowTaggedSubdirectoriesCommand(object parameter) => ShowTaggedSubdirectoriesCommand?.Invoke(this, new(parameter));
        // {
        //   try { OnShowTaggedSubdirectoriesCommand(parameter); }
        //   finally { ShowTaggedSubdirectoriesCommand?.Invoke(this, new(parameter)); }
        // }

        /// <summary>
        /// Called when the <see cref="ShowTaggedSubdirectories">ShowTaggedSubdirectories Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="ShowTaggedSubdirectories" />.</param>
        protected virtual void OnShowTaggedSubdirectoriesCommand(object parameter)
        {
            // TODO: Implement OnShowTaggedSubdirectoriesCommand Logic
        }

        #endregion
        #region ShowTaggedFiles Command Property Members

        /// <summary>
        /// Occurs when the <see cref="ShowTaggedFiles"/> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> ShowTaggedFilesCommand;

        private static readonly DependencyPropertyKey ShowTaggedFilesPropertyKey = DependencyPropertyBuilder<ListItemViewModel, Commands.RelayCommand>
            .Register(nameof(ShowTaggedFiles))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ShowTaggedFiles"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowTaggedFilesProperty = ShowTaggedFilesPropertyKey.DependencyProperty;

        public Commands.RelayCommand ShowTaggedFiles => (Commands.RelayCommand)GetValue(ShowTaggedFilesProperty);

        /// <summary>
        /// Called when the ShowTaggedFiles event is raised by <see cref="ShowTaggedFiles" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="ShowTaggedFiles" />.</param>
        protected void RaiseShowTaggedFilesCommand(object parameter) => ShowTaggedFilesCommand?.Invoke(this, new(parameter));
        // {
        //   try { OnShowTaggedFilesCommand(parameter); }
        //   finally { ShowTaggedFilesCommand?.Invoke(this, new(parameter)); }
        // }

        /// <summary>
        /// Called when the <see cref="ShowTaggedFiles">ShowTaggedFiles Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="ShowTaggedFiles" />.</param>
        protected virtual void OnShowTaggedFilesCommand(object parameter)
        {
            // TODO: Implement OnShowTaggedFilesCommand Logic
        }

        #endregion
        #region UpstreamId Property Members

        private static readonly DependencyPropertyKey UpstreamIdPropertyKey = DependencyProperty.RegisterReadOnly(nameof(UpstreamId), typeof(Guid?),
            typeof(ListItemViewModel), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="UpstreamId"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty UpstreamIdProperty = UpstreamIdPropertyKey.DependencyProperty;

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

        public ListItemViewModel([DisallowNull] PersonalTagDefinitionListItem entity) : base(entity)
        {
            SetValue(SynchronizeNowPropertyKey, new Commands.RelayCommand(RaiseSynchronizeNowCommand));
            SetValue(ShowTaggedVolumesPropertyKey, new Commands.RelayCommand(RaiseShowTaggedVolumesCommand));
            SetValue(ShowTaggedSubdirectoriesPropertyKey, new Commands.RelayCommand(RaiseShowTaggedSubdirectoriesCommand));
            SetValue(ShowTaggedFilesPropertyKey, new Commands.RelayCommand(RaiseShowTaggedFilesCommand));
            UpstreamId = entity.UpstreamId;
            LastSynchronizedOn = entity.LastSynchronizedOn;
        }
    }
}
