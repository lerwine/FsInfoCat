using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class FileSystemRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>, IFileSystemRowViewModel
        where TEntity : DbEntity, IFileSystemRow
    {
        #region Notes Property Members

        /// <summary>
        /// Identifies the <see cref="Notes"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NotesProperty = ColumnPropertyBuilder<string, FileSystemRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IFileSystemRow.Notes))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as FileSystemRowViewModel<TEntity>)?.OnNotesPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string Notes { get => GetValue(NotesProperty) as string; set => SetValue(NotesProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Notes"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Notes"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Notes"/> property.</param>
        protected virtual void OnNotesPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region IsInactive Property Members

        /// <summary>
        /// Identifies the <see cref="IsInactive"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsInactiveProperty = ColumnPropertyBuilder<bool, FileSystemRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IFileSystemRow.IsInactive))
            .DefaultValue(false)
            .OnChanged((d, oldValue, newValue) => (d as FileSystemRowViewModel<TEntity>)?.OnIsInactivePropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public bool IsInactive { get => (bool)GetValue(IsInactiveProperty); set => SetValue(IsInactiveProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="IsInactive"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="IsInactive"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="IsInactive"/> property.</param>
        protected virtual void OnIsInactivePropertyChanged(bool oldValue, bool newValue) { }

        #endregion
        #region DisplayName Property Members

        /// <summary>
        /// Identifies the <see cref="DisplayName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DisplayNameProperty = ColumnPropertyBuilder<string, FileSystemRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IFileSystemRow.DisplayName))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as FileSystemRowViewModel<TEntity>)?.OnDisplayNamePropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string DisplayName { get => GetValue(DisplayNameProperty) as string; set => SetValue(DisplayNameProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="DisplayName"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="DisplayName"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="DisplayName"/> property.</param>
        protected virtual void OnDisplayNamePropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region ReadOnly Property Members

        /// <summary>
        /// Identifies the <see cref="ReadOnly"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ReadOnlyProperty = ColumnPropertyBuilder<bool, FileSystemRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IFileSystemRow.ReadOnly))
            .DefaultValue(false)
            .OnChanged((d, oldValue, newValue) => (d as FileSystemRowViewModel<TEntity>)?.OnReadOnlyPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public bool ReadOnly { get => (bool)GetValue(ReadOnlyProperty); set => SetValue(ReadOnlyProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ReadOnly"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ReadOnly"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ReadOnly"/> property.</param>
        protected virtual void OnReadOnlyPropertyChanged(bool oldValue, bool newValue) { }

        #endregion
        #region MaxNameLength Property Members

        /// <summary>
        /// Identifies the <see cref="MaxNameLength"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxNameLengthProperty = ColumnPropertyBuilder<uint, FileSystemRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IFileSystemRow.MaxNameLength))
            .DefaultValue(DbConstants.DbColDefaultValue_MaxNameLength)
            .OnChanged((d, oldValue, newValue) => (d as FileSystemRowViewModel<TEntity>)?.OnMaxNameLengthPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public uint MaxNameLength { get => (uint)GetValue(MaxNameLengthProperty); set => SetValue(MaxNameLengthProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="MaxNameLength"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="MaxNameLength"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="MaxNameLength"/> property.</param>
        protected virtual void OnMaxNameLengthPropertyChanged(uint oldValue, uint newValue) { }

        #endregion
        #region DefaultDriveType Property Members

        /// <summary>
        /// Identifies the <see cref="DefaultDriveType"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DefaultDriveTypeProperty = ColumnPropertyBuilder<DriveType?, FileSystemRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IFileSystemRow.DefaultDriveType))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as FileSystemRowViewModel<TEntity>)?.OnDefaultDriveTypePropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public DriveType? DefaultDriveType { get => (DriveType?)GetValue(DefaultDriveTypeProperty); set => SetValue(DefaultDriveTypeProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="DefaultDriveType"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="DefaultDriveType"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="DefaultDriveType"/> property.</param>
        protected virtual void OnDefaultDriveTypePropertyChanged(DriveType? oldValue, DriveType? newValue) { }

        #endregion

        IFileSystemRow IFileSystemRowViewModel.Entity => Entity;

        public FileSystemRowViewModel([DisallowNull] TEntity entity) : base(entity)
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
