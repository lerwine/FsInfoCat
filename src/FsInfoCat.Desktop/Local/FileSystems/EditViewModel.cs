using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.Local.FileSystems
{
    public class EditViewModel : FileSystemDetailsViewModel<FileSystem, VolumeListItem, VolumeListItemViewModel, SymbolicNameRow, SymbolicNameListItemViewModel>,
        INotifyNavigatedTo
    {
        #region UpstreamId Property Members

        private static readonly DependencyPropertyKey UpstreamIdPropertyKey = DependencyProperty.RegisterReadOnly(nameof(UpstreamId), typeof(Guid?), typeof(EditViewModel),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as EditViewModel).OnUpstreamIdPropertyChanged((Guid?)e.OldValue, (Guid?)e.NewValue)));

        /// <summary>
        /// Identifies the <see cref="UpstreamId"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty UpstreamIdProperty = UpstreamIdPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public Guid? UpstreamId { get => (Guid?)GetValue(UpstreamIdProperty); private set => SetValue(UpstreamIdPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="UpstreamId"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="UpstreamId"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="UpstreamId"/> property.</param>
        private void OnUpstreamIdPropertyChanged(Guid? oldValue, Guid? newValue)
        {
            // TODO: Implement OnUpstreamIdPropertyChanged Logic
        }

        #endregion
        #region LastSynchronizedOn Property Members

        private static readonly DependencyPropertyKey LastSynchronizedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastSynchronizedOn), typeof(DateTime?), typeof(EditViewModel),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as EditViewModel).OnLastSynchronizedOnPropertyChanged((DateTime?)e.OldValue, (DateTime?)e.NewValue)));

        /// <summary>
        /// Identifies the <see cref="LastSynchronizedOn"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LastSynchronizedOnProperty = LastSynchronizedOnPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public DateTime? LastSynchronizedOn { get => (DateTime?)GetValue(LastSynchronizedOnProperty); private set => SetValue(LastSynchronizedOnPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="LastSynchronizedOn"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="LastSynchronizedOn"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="LastSynchronizedOn"/> property.</param>
        private void OnLastSynchronizedOnPropertyChanged(DateTime? oldValue, DateTime? newValue)
        {
            // TODO: Implement OnLastSynchronizedOnPropertyChanged Logic
        }

        #endregion

        public EditViewModel(FileSystem entity) : base(entity)
        {
            UpstreamId = entity.UpstreamId;
            LastSynchronizedOn = entity.LastSynchronizedOn;
        }

        void INotifyNavigatedTo.OnNavigatedTo()
        {
            throw new NotImplementedException();
        }
    }
}