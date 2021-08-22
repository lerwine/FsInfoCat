using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public class EditRedundantSetVM : EditDbEntityVM<Volume>
    {
        #region BinaryProperties Property Members

        /// <summary>
        /// Identifies the <see cref="BinaryProperties"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BinaryPropertiesProperty = DependencyProperty.Register(nameof(BinaryProperties), typeof(BinaryPropertySetItemVM), typeof(EditRedundantSetVM),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as EditRedundantSetVM)?.OnBinaryPropertiesPropertyChanged((BinaryPropertySetItemVM)e.OldValue, (BinaryPropertySetItemVM)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public BinaryPropertySetItemVM BinaryProperties { get => (BinaryPropertySetItemVM)GetValue(BinaryPropertiesProperty); set => SetValue(BinaryPropertiesProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="BinaryProperties"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="BinaryProperties"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="BinaryProperties"/> property.</param>
        private void OnBinaryPropertiesPropertyChanged(BinaryPropertySetItemVM oldValue, BinaryPropertySetItemVM newValue)
        {
            // TODO: Implement OnBinaryPropertiesPropertyChanged Logic
        }

        #endregion
        #region Notes Property Members

        /// <summary>
        /// Identifies the <see cref="Notes"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NotesProperty = DependencyProperty.Register(nameof(Notes), typeof(string), typeof(EditRedundantSetVM),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as EditRedundantSetVM)?.OnNotesPropertyChanged(e.OldValue as string, e.NewValue as string)));

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
        private void OnNotesPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnNotesPropertyChanged Logic
        }

        #endregion
        #region FileOptions Property Members

        private readonly ObservableCollection<FileItemVM> _backingFileOptions = new();

        private static readonly DependencyPropertyKey FileOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(FileOptions), typeof(ReadOnlyObservableCollection<FileItemVM>), typeof(EditRedundantSetVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="FileOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FileOptionsProperty = FileOptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public ReadOnlyObservableCollection<FileItemVM> FileOptions => (ReadOnlyObservableCollection<FileItemVM>)GetValue(FileOptionsProperty);

/* TODO: Add Command to initialization code to constructor
   SetValue(FileOptionsPropertyKey, new ReadOnlyObservableCollection<FileItemVM>(_backingFileOptions)); */

#endregion
        #region Redundancies Property Members

        private static readonly DependencyPropertyKey RedundanciesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Redundancies), typeof(ObservableCollection<RedundancyItemVM>), typeof(EditRedundantSetVM),
                new PropertyMetadata(new ObservableCollection<RedundancyItemVM>()));

        /// <summary>
        /// Identifies the <see cref="Redundancies"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RedundanciesProperty = RedundanciesPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public ObservableCollection<RedundancyItemVM> Redundancies
        {
            get => (ObservableCollection<RedundancyItemVM>)GetValue(RedundanciesProperty);
            private set => SetValue(RedundanciesPropertyKey, value);
        }

        #endregion
        #region Reference Property Members

        /// <summary>
        /// Identifies the <see cref="Reference"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ReferenceProperty = DependencyProperty.Register(nameof(Reference), typeof(string), typeof(EditRedundantSetVM),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as EditRedundantSetVM)?.OnReferencePropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Reference { get => GetValue(ReferenceProperty) as string; set => SetValue(ReferenceProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Reference"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Reference"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Reference"/> property.</param>
        private void OnReferencePropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnReferencePropertyChanged Logic
        }

        #endregion

        public EditRedundantSetVM()
        {
        }

        protected override DbSet<Volume> GetDbSet([DisallowNull] LocalDbContext dbContext) => dbContext.Volumes;

        protected override void OnModelPropertyChanged(Volume oldValue, Volume newValue)
        {
            throw new NotImplementedException();
        }

        protected override bool OnBeforeSave()
        {
            throw new NotImplementedException();
        }
    }
}
