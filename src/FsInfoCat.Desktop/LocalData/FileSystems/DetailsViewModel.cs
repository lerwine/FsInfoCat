using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace FsInfoCat.Desktop.LocalData.FileSystems
{
    public class DetailsViewModel : FileSystemDetailsViewModel<FileSystem, VolumeListItem, VolumeListItemViewModel, SymbolicNameRow, SymbolicNameListItemViewModel>,
        INavigatedToNotifiable, INavigatingFromNotifiable
    {
        /// <summary>
        /// Occurs when the <see cref="SaveChanges"/> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> ChangesSaved;

        private void RaiseChangesSaved(object args) => ChangesSaved?.Invoke(this, new Commands.CommandEventArgs(args));

        #region ListItem Property Members

        private static readonly DependencyPropertyKey ListItemPropertyKey = DependencyPropertyBuilder<DetailsViewModel, FileSystemListItem>
            .Register(nameof(ListItem))
            .DefaultValue(null)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ListItem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ListItemProperty = ListItemPropertyKey.DependencyProperty;

        public FileSystemListItem ListItem { get => (FileSystemListItem)GetValue(ListItemProperty); private set => SetValue(ListItemPropertyKey, value); }

        #endregion
        #region UpstreamId Property Members

        private static readonly DependencyPropertyKey UpstreamIdPropertyKey = DependencyProperty.RegisterReadOnly(nameof(UpstreamId), typeof(Guid?), typeof(DetailsViewModel),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as DetailsViewModel).OnUpstreamIdPropertyChanged((Guid?)e.OldValue, (Guid?)e.NewValue)));

        /// <summary>
        /// Identifies the <see cref="UpstreamId"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty UpstreamIdProperty = UpstreamIdPropertyKey.DependencyProperty;

        public Guid? UpstreamId { get => (Guid?)GetValue(UpstreamIdProperty); private set => SetValue(UpstreamIdPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="UpstreamId"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="UpstreamId"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="UpstreamId"/> property.</param>
        private void OnUpstreamIdPropertyChanged(Guid? oldValue, Guid? newValue) { }

        #endregion
        #region LastSynchronizedOn Property Members

        private static readonly DependencyPropertyKey LastSynchronizedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastSynchronizedOn), typeof(DateTime?), typeof(DetailsViewModel),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as DetailsViewModel).OnLastSynchronizedOnPropertyChanged((DateTime?)e.OldValue, (DateTime?)e.NewValue)));

        /// <summary>
        /// Identifies the <see cref="LastSynchronizedOn"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LastSynchronizedOnProperty = LastSynchronizedOnPropertyKey.DependencyProperty;

        public DateTime? LastSynchronizedOn { get => (DateTime?)GetValue(LastSynchronizedOnProperty); private set => SetValue(LastSynchronizedOnPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="LastSynchronizedOn"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="LastSynchronizedOn"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="LastSynchronizedOn"/> property.</param>
        private void OnLastSynchronizedOnPropertyChanged(DateTime? oldValue, DateTime? newValue) { }

        #endregion

        public DetailsViewModel([DisallowNull] FileSystem entity, FileSystemListItem listItem) : base(entity)
        {
            ListItem = listItem;
            UpstreamId = entity.UpstreamId;
            LastSynchronizedOn = entity.LastSynchronizedOn;
        }

        public static void AddNewItem(ReturnEventHandler<FileSystem> onReturn = null)
        {
            // TODO: Implement AddNewItem
        }

        void INavigatedToNotifiable.OnNavigatedTo()
        {
            // TODO: Load option lists from database
            throw new NotImplementedException();
        }

        void INavigatingFromNotifiable.OnNavigatingFrom(CancelEventArgs e)
        {
            // TODO: Prompt to lose changes if not saved
            throw new NotImplementedException();
        }
    }
}
