using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.LocalData.AudioPropertySets
{
    public class ListItemViewModel : AudioPropertiesListItemViewModel<AudioPropertiesListItem>, ILocalCrudEntityRowViewModel<AudioPropertiesListItem>
    {
        #region LastSynchronizedOn Property Members

        private static readonly DependencyPropertyKey LastSynchronizedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastSynchronizedOn), typeof(DateTime?), typeof(ListItemViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="LastSynchronizedOn"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LastSynchronizedOnProperty = LastSynchronizedOnPropertyKey.DependencyProperty;

        public DateTime? LastSynchronizedOn { get => (DateTime?)GetValue(LastSynchronizedOnProperty); private set => SetValue(LastSynchronizedOnPropertyKey, value); }

        #endregion
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
        #region ShowFileListing Command Property Members

        /// <summary>
        /// Occurs when the <see cref="ShowFileListing"/> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> ShowFileListingCommand;

        private static readonly DependencyPropertyKey ShowFileListingPropertyKey = DependencyPropertyBuilder<ListItemViewModel, Commands.RelayCommand>
            .Register(nameof(ShowFileListing))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ShowFileListing"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowFileListingProperty = ShowFileListingPropertyKey.DependencyProperty;

        public Commands.RelayCommand ShowFileListing => (Commands.RelayCommand)GetValue(ShowFileListingProperty);

        /// <summary>
        /// Called when the ShowFileListing event is raised by <see cref="ShowFileListing" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="ShowFileListing" />.</param>
        protected void RaiseShowFileListingCommand(object parameter) => ShowFileListingCommand?.Invoke(this, new(parameter));
        // {
        //   try { OnShowFileListingCommand(parameter); }
        //   finally { ShowFileListingCommand?.Invoke(this, new(parameter)); }
        // }

        /// <summary>
        /// Called when the <see cref="ShowFileListing">ShowFileListing Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="ShowFileListing" />.</param>
        protected virtual void OnShowFileListingCommand(object parameter)
        {
            // TODO: Implement OnShowFileListingCommand Logic
        }

        #endregion

        public ListItemViewModel([DisallowNull] AudioPropertiesListItem entity) : base(entity)
        {
            SetValue(SynchronizeNowPropertyKey, new Commands.RelayCommand(RaiseSynchronizeNowCommand));
            SetValue(ShowFileListingPropertyKey, new Commands.RelayCommand(RaiseShowFileListingCommand));
            LastSynchronizedOn = entity.LastSynchronizedOn;
        }
    }
}
