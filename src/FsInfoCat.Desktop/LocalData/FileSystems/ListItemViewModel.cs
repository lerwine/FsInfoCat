using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.LocalData.FileSystems
{
    public class ListItemViewModel : FileSystemListItemViewModel<FileSystemListItem>, ILocalCrudEntityRowViewModel<FileSystemListItem>
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
        #region OpenListingButtonClick Property Members

        /// <summary>
        /// Occurs when the <see cref="OpenListingButtonClick">OpenListingButtonClick Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> OpenListingButtonClicked;

        private static readonly DependencyPropertyKey OpenListingButtonClickPropertyKey = DependencyProperty.RegisterReadOnly(nameof(OpenListingButtonClick),
            typeof(Commands.RelayCommand), typeof(ListItemViewModel), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="OpenListingButtonClick"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OpenListingButtonClickProperty = OpenListingButtonClickPropertyKey.DependencyProperty;

        public Commands.RelayCommand OpenListingButtonClick => (Commands.RelayCommand)GetValue(OpenListingButtonClickProperty);

        /// <summary>
        /// Called when the OpenListingButtonClick event is raised by <see cref="OpenListingButtonClick" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="OpenListingButtonClick" />.</param>
        private void RaiseOpenListingButtonClicked(object parameter)
        {
            try { OnOpenListingButtonClicked(parameter); }
            finally { OpenListingButtonClicked?.Invoke(this, new(parameter)); }
        }

        /// <summary>
        /// Called when the <see cref="OpenListingButtonClick">OpenListingButtonClick Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="OpenListingButtonClick" />.</param>
        protected virtual void OnOpenListingButtonClicked(object parameter)
        {
            // TODO: Implement OnOpenListingButtonClicked Logic
        }

        #endregion
        #region SynchronizeNowButtonClick Property Members

        /// <summary>
        /// Occurs when the <see cref="SynchronizeNowButtonClick">SynchronizeNowButtonClick Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> SynchronizeNowButtonClicked;

        private static readonly DependencyPropertyKey SynchronizeNowButtonClickPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SynchronizeNowButtonClick),
            typeof(Commands.RelayCommand), typeof(ListItemViewModel), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="SynchronizeNowButtonClick"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SynchronizeNowButtonClickProperty = SynchronizeNowButtonClickPropertyKey.DependencyProperty;

        public Commands.RelayCommand SynchronizeNowButtonClick => (Commands.RelayCommand)GetValue(SynchronizeNowButtonClickProperty);

        /// <summary>
        /// Called when the SynchronizeNowButtonClick event is raised by <see cref="SynchronizeNowButtonClick" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="SynchronizeNowButtonClick" />.</param>
        private void RaiseSynchronizeNowButtonClicked(object parameter)
        {
            try { OnSynchronizeNowButtonClicked(parameter); }
            finally { SynchronizeNowButtonClicked?.Invoke(this, new(parameter)); }
        }

        /// <summary>
        /// Called when the <see cref="SynchronizeNowButtonClick">SynchronizeNowButtonClick Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="SynchronizeNowButtonClick" />.</param>
        protected virtual void OnSynchronizeNowButtonClicked(object parameter)
        {
            // TODO: Implement OnSynchronizeNowButtonClicked Logic
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
        public Guid? UpstreamId { get => (Guid?)GetValue(UpstreamIdProperty); private set => SetValue(UpstreamIdPropertyKey, value); }

        #endregion
        #region LastSynchronizedOn Property Members

        private static readonly DependencyPropertyKey LastSynchronizedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastSynchronizedOn), typeof(DateTime?),
            typeof(ListItemViewModel), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="LastSynchronizedOn"/> dependency property.
        /// </summary>
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

        public ListItemViewModel([DisallowNull] FileSystemListItem entity)
            : base(entity)
        {
            SetValue(EditButtonClickPropertyKey, new Commands.RelayCommand(RaiseEditButtonClicked));
            SetValue(DeleteButtonClickPropertyKey, new Commands.RelayCommand(RaiseDeleteButtonClicked));
            SetValue(OpenListingButtonClickPropertyKey, new Commands.RelayCommand(RaiseOpenListingButtonClicked));
            SetValue(SynchronizeNowButtonClickPropertyKey, new Commands.RelayCommand(RaiseSynchronizeNowButtonClicked));
            UpstreamId = entity.UpstreamId;
            LastSynchronizedOn = entity.LastSynchronizedOn;
        }
    }
}
