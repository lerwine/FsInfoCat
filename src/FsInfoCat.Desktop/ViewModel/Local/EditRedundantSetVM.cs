using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public class EditRedundantSetVM : EditDbEntityVM<Volume>
    {
        #region BinaryPropertiesOptions Property Members

        private readonly ObservableCollection<BinaryPropertySetItemVM> _backingBinaryPropertiesOptions = new();

        private static readonly DependencyPropertyKey BinaryPropertiesOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(BinaryPropertiesOptions), typeof(ReadOnlyObservableCollection<BinaryPropertySetItemVM>), typeof(EditRedundantSetVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="BinaryPropertiesOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BinaryPropertiesOptionsProperty = BinaryPropertiesOptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public ReadOnlyObservableCollection<BinaryPropertySetItemVM> BinaryPropertiesOptions => (ReadOnlyObservableCollection<BinaryPropertySetItemVM>)GetValue(BinaryPropertiesOptionsProperty);

        #endregion
        #region SelectedBinaryProperties Property Members

        /// <summary>
        /// Identifies the <see cref="SelectedBinaryProperties"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedBinaryPropertiesProperty = DependencyProperty.Register(nameof(SelectedBinaryProperties), typeof(BinaryPropertySetItemVM), typeof(EditRedundantSetVM),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as EditRedundantSetVM)?.OnSelectedBinaryPropertiesPropertyChanged((BinaryPropertySetItemVM)e.OldValue, (BinaryPropertySetItemVM)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public BinaryPropertySetItemVM SelectedBinaryProperties { get => (BinaryPropertySetItemVM)GetValue(SelectedBinaryPropertiesProperty); set => SetValue(SelectedBinaryPropertiesProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="SelectedBinaryProperties"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="SelectedBinaryProperties"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="SelectedBinaryProperties"/> property.</param>
        private void OnSelectedBinaryPropertiesPropertyChanged(BinaryPropertySetItemVM oldValue, BinaryPropertySetItemVM newValue)
        {
            // TODO: Implement OnSelectedBinaryPropertiesPropertyChanged Logic
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
            SetValue(BinaryPropertiesOptionsPropertyKey, new ReadOnlyObservableCollection<BinaryPropertySetItemVM>(_backingBinaryPropertiesOptions));
        }

        protected override DbSet<Volume> GetDbSet(LocalDbContext dbContext)
        {
            throw new NotImplementedException();
        }

        protected override Volume InitializeNewModel()
        {
            throw new NotImplementedException();
        }

        protected override void UpdateModelForSave(Volume model, bool isNew)
        {
            throw new NotImplementedException();
        }
    }
}
