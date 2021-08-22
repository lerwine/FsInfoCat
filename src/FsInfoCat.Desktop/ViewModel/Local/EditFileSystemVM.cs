using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public class EditFileSystemVM : EditDbEntityVM<FileSystem>
    {
        #region DisplayName Property Members

        public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register(nameof(DisplayName), typeof(string), typeof(EditFileSystemVM),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditFileSystemVM).OnDisplayNamePropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string DisplayName
        {
            get => GetValue(DisplayNameProperty) as string;
            set => SetValue(DisplayNameProperty, value);
        }

        protected virtual void OnDisplayNamePropertyChanged(string oldValue, string newValue)
        {
            ChangeTracker.SetChangeState(nameof(DisplayName), (newValue = newValue.AsWsNormalizedOrEmpty()) == Model?.DisplayName);
            if (newValue.Length == 0)
                Validation.SetErrorMessage(nameof(DisplayName), FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameRequired);
            else if (newValue.Length > DbConstants.DbColMaxLen_LongName)
                Validation.SetErrorMessage(nameof(DisplayName), FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameLength);
            else
                Validation.ClearErrorMessages(nameof(DisplayName));
        }

        #endregion
        #region Notes Property Members

        public static readonly DependencyProperty NotesProperty = DependencyProperty.Register(nameof(Notes), typeof(string), typeof(EditFileSystemVM),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditFileSystemVM).OnNotesPropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string Notes
        {
            get => GetValue(NotesProperty) as string;
            set => SetValue(NotesProperty, value);
        }

        protected virtual void OnNotesPropertyChanged(string oldValue, string newValue)
        {
            ChangeTracker.SetChangeState(nameof(Notes), Model?.Notes != newValue);
        }

        #endregion
        #region DriveTypeOptions Property Members

        private static readonly DependencyPropertyKey DriveTypeOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(DriveTypeOptions),
            typeof(ReadOnlyObservableCollection<DriveType>), typeof(EditFileSystemVM), new PropertyMetadata(null));

        public static readonly DependencyProperty DriveTypeOptionsProperty = DriveTypeOptionsPropertyKey.DependencyProperty;

        public ReadOnlyObservableCollection<DriveType> DriveTypeOptions => (ReadOnlyObservableCollection<DriveType>)GetValue(DriveTypeOptionsProperty);

        #endregion
        #region SelectedDriveType Property Members

        public static readonly DependencyProperty SelectedDriveTypeProperty = DependencyProperty.Register(nameof(SelectedDriveType), typeof(DriveType), typeof(EditFileSystemVM),
                new PropertyMetadata(DriveType.Unknown, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditFileSystemVM).OnSelectedDriveTypePropertyChanged((DriveType)e.OldValue, (DriveType)e.NewValue)));

        public DriveType SelectedDriveType
        {
            get => (DriveType)GetValue(SelectedDriveTypeProperty);
            set => SetValue(SelectedDriveTypeProperty, value);
        }

        protected virtual void OnSelectedDriveTypePropertyChanged(DriveType oldValue, DriveType newValue)
        {
            // TODO: Implement OnSelectedDriveTypePropertyChanged Logic
        }

        #endregion
        #region HasDefaultDriveType Property Members

        public static readonly DependencyProperty HasDefaultDriveTypeProperty = DependencyProperty.Register(nameof(HasDefaultDriveType), typeof(bool), typeof(EditFileSystemVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditFileSystemVM).OnHasDefaultDriveTypePropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool HasDefaultDriveType
        {
            get => (bool)GetValue(HasDefaultDriveTypeProperty);
            set => SetValue(HasDefaultDriveTypeProperty, value);
        }

        protected virtual void OnHasDefaultDriveTypePropertyChanged(bool oldValue, bool newValue)
        {
            // TODO: Implement OnHasDefaultDriveTypePropertyChanged Logic
        }

        #endregion
        #region IsInactive Property Members

        public static readonly DependencyProperty IsInactiveProperty = DependencyProperty.Register(nameof(IsInactive), typeof(bool), typeof(EditFileSystemVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditFileSystemVM).OnIsInactivePropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool IsInactive
        {
            get => (bool)GetValue(IsInactiveProperty);
            set => SetValue(IsInactiveProperty, value);
        }

        protected virtual void OnIsInactivePropertyChanged(bool oldValue, bool newValue)
        {
            // TODO: Implement OnIsInactivePropertyChanged Logic
        }

        #endregion
        #region MaxNameLength Property Members

        public static readonly DependencyProperty MaxNameLengthProperty = DependencyProperty.Register(nameof(MaxNameLength), typeof(uint), typeof(EditFileSystemVM),
                new PropertyMetadata((uint)DbConstants.DbColDefaultValue_MaxNameLength, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditFileSystemVM).OnMaxNameLengthPropertyChanged((uint)e.OldValue, (uint)e.NewValue)));

        public uint MaxNameLength
        {
            get => (uint)GetValue(MaxNameLengthProperty);
            set => SetValue(MaxNameLengthProperty, value);
        }

        protected virtual void OnMaxNameLengthPropertyChanged(uint oldValue, uint newValue)
        {
            // TODO: Implement OnMaxNameLengthPropertyChanged Logic
        }

        #endregion
        #region ReadOnly Property Members

        public static readonly DependencyProperty ReadOnlyProperty = DependencyProperty.Register(nameof(ReadOnly), typeof(bool), typeof(EditFileSystemVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditFileSystemVM).OnReadOnlyPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool ReadOnly
        {
            get => (bool)GetValue(ReadOnlyProperty);
            set => SetValue(ReadOnlyProperty, value);
        }

        protected virtual void OnReadOnlyPropertyChanged(bool oldValue, bool newValue)
        {
            // TODO: Implement OnReadOnlyPropertyChanged Logic
        }

        #endregion
        public EditFileSystemVM()
        {
            SetValue(DriveTypeOptionsPropertyKey, new ReadOnlyObservableCollection<DriveType>(new ObservableCollection<DriveType>(Enum.GetValues<DriveType>())));
        }

        protected override DbSet<FileSystem> GetDbSet([DisallowNull] LocalDbContext dbContext) => dbContext.FileSystems;

        protected override void OnModelPropertyChanged(FileSystem oldValue, FileSystem newValue)
        {
            if (newValue is null)
            {
                Notes = DisplayName = "";
                SelectedDriveType = DriveType.Unknown;
                ReadOnly = IsInactive = HasDefaultDriveType = false;
                MaxNameLength = DbConstants.DbColDefaultValue_MaxNameLength;
            }
            else
            {
                DisplayName = newValue.DisplayName;
                Notes = newValue.Notes;
                SelectedDriveType = newValue.DefaultDriveType ?? DriveType.Unknown;
                HasDefaultDriveType = newValue.DefaultDriveType.HasValue;
                IsInactive = newValue.IsInactive;
                MaxNameLength = newValue.MaxNameLength;
                ReadOnly = newValue.ReadOnly;
            }
        }

        protected override bool OnBeforeSave()
        {
            FileSystem model = Model;
            if (model is null)
                return false;
            model.DisplayName = DisplayName;
            model.Notes = Notes;
            model.DefaultDriveType = HasDefaultDriveType ? SelectedDriveType : null;
            model.IsInactive = IsInactive;
            model.MaxNameLength = MaxNameLength;
            model.ReadOnly = ReadOnly;
            return true;
        }
    }
}
