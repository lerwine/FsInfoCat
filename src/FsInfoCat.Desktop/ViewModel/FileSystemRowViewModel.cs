using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class FileSystemRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : DbEntity, IFileSystemRow
    {
#pragma warning disable IDE0060 // Remove unused parameter
        #region Notes Property Members

        /// <summary>
        /// Identifies the <see cref="Notes"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NotesProperty = DependencyProperty.Register(nameof(Notes), typeof(string), typeof(FileSystemRowViewModel<TEntity>),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as FileSystemRowViewModel<TEntity>)?.OnNotesPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Notes { get => GetValue(NotesProperty) as string; set => SetValue(NotesProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Notes"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Notes"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Notes"/> property.</param>
        protected void OnNotesPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnNotesPropertyChanged Logic
        }

        #endregion
        #region IsInactive Property Members

        /// <summary>
        /// Identifies the <see cref="IsInactive"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsInactiveProperty = DependencyProperty.Register(nameof(IsInactive), typeof(bool),
            typeof(FileSystemRowViewModel<TEntity>), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as FileSystemRowViewModel<TEntity>)?.OnIsInactivePropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool IsInactive { get => (bool)GetValue(IsInactiveProperty); set => SetValue(IsInactiveProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="IsInactive"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="IsInactive"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="IsInactive"/> property.</param>
        protected void OnIsInactivePropertyChanged(bool oldValue, bool newValue)
        {
            // TODO: Implement OnIsInactivePropertyChanged Logic
        }

        #endregion
        #region DisplayName Property Members

        /// <summary>
        /// Identifies the <see cref="DisplayName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register(nameof(DisplayName), typeof(string),
            typeof(FileSystemRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as FileSystemRowViewModel<TEntity>)?.OnDisplayNamePropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string DisplayName { get => GetValue(DisplayNameProperty) as string; set => SetValue(DisplayNameProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="DisplayName"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="DisplayName"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="DisplayName"/> property.</param>
        protected void OnDisplayNamePropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnDisplayNamePropertyChanged Logic
        }

        #endregion
        #region ReadOnly Property Members

        /// <summary>
        /// Identifies the <see cref="ReadOnly"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ReadOnlyProperty = DependencyProperty.Register(nameof(ReadOnly), typeof(bool),
            typeof(FileSystemRowViewModel<TEntity>), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as FileSystemRowViewModel<TEntity>)?.OnReadOnlyPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool ReadOnly { get => (bool)GetValue(ReadOnlyProperty); set => SetValue(ReadOnlyProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ReadOnly"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ReadOnly"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ReadOnly"/> property.</param>
        protected void OnReadOnlyPropertyChanged(bool oldValue, bool newValue)
        {
            // TODO: Implement OnReadOnlyPropertyChanged Logic
        }

        #endregion
        #region MaxNameLength Property Members

        /// <summary>
        /// Identifies the <see cref="MaxNameLength"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxNameLengthProperty = DependencyProperty.Register(nameof(MaxNameLength), typeof(uint),
            typeof(FileSystemRowViewModel<TEntity>), new PropertyMetadata(DbConstants.DbColDefaultValue_MaxNameLength, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as FileSystemRowViewModel<TEntity>)?.OnMaxNameLengthPropertyChanged((uint)e.OldValue, (uint)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public uint MaxNameLength { get => (uint)GetValue(MaxNameLengthProperty); set => SetValue(MaxNameLengthProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="MaxNameLength"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="MaxNameLength"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="MaxNameLength"/> property.</param>
        protected void OnMaxNameLengthPropertyChanged(uint oldValue, uint newValue)
        {
            // TODO: Implement OnMaxNameLengthPropertyChanged Logic
        }

        #endregion
        #region DefaultDriveType Property Members

        /// <summary>
        /// Identifies the <see cref="DefaultDriveType"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DefaultDriveTypeProperty = DependencyProperty.Register(nameof(DefaultDriveType), typeof(DriveType?),
            typeof(FileSystemRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as FileSystemRowViewModel<TEntity>)?.OnDefaultDriveTypePropertyChanged((DriveType?)e.OldValue, (DriveType?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public DriveType? DefaultDriveType { get => (DriveType?)GetValue(DefaultDriveTypeProperty); set => SetValue(DefaultDriveTypeProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="DefaultDriveType"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="DefaultDriveType"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="DefaultDriveType"/> property.</param>
        protected void OnDefaultDriveTypePropertyChanged(DriveType? oldValue, DriveType? newValue)
        {
            // TODO: Implement OnDefaultDriveTypePropertyChanged Logic
        }

        #endregion
#pragma warning restore IDE0060 // Remove unused parameter

        public FileSystemRowViewModel(TEntity entity) : base(entity)
        {
            Notes = entity.Notes;
            IsInactive = entity.IsInactive;
            DisplayName = entity.DisplayName;
            ReadOnly = entity.ReadOnly;
            MaxNameLength = entity.MaxNameLength;
            DefaultDriveType = entity.DefaultDriveType;
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IFileSystemRow.Notes):
                    Dispatcher.CheckInvoke(() => Notes = Entity.Notes);
                    break;
                case nameof(IFileSystemRow.IsInactive):
                    Dispatcher.CheckInvoke(() => IsInactive = Entity.IsInactive);
                    break;
                case nameof(IFileSystemRow.DisplayName):
                    Dispatcher.CheckInvoke(() => DisplayName = Entity.DisplayName);
                    break;
                case nameof(IFileSystemRow.ReadOnly):
                    Dispatcher.CheckInvoke(() => ReadOnly = Entity.ReadOnly);
                    break;
                case nameof(IFileSystemRow.MaxNameLength):
                    Dispatcher.CheckInvoke(() => MaxNameLength = Entity.MaxNameLength);
                    break;
                case nameof(IFileSystemRow.DefaultDriveType):
                    Dispatcher.CheckInvoke(() => DefaultDriveType = Entity.DefaultDriveType);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
