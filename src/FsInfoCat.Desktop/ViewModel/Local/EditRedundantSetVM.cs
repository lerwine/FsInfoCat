using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    /// <summary>
    /// View Model for <see cref="View.Local.EditRedundantSetWindow"/>.
    /// </summary>
    public class EditRedundantSetVM : EditDbEntityVM<RedundantSet>
    {
        #region BinaryProperties Property Members

        private static readonly DependencyPropertyKey BinaryPropertiesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(BinaryProperties), typeof(BinaryPropertySet), typeof(EditRedundantSetVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="BinaryProperties"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BinaryPropertiesProperty = BinaryPropertiesPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public BinaryPropertySet BinaryProperties { get => (BinaryPropertySet)GetValue(BinaryPropertiesProperty); private set => SetValue(BinaryPropertiesPropertyKey, value); }

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
        protected void OnReferencePropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnReferencePropertyChanged Logic
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
        protected void OnNotesPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnNotesPropertyChanged Logic
        }

        #endregion
        #region Redundancies Property Members

        private readonly ObservableCollection<Redundancy> _backingRedundancies = new();

        private static readonly DependencyPropertyKey RedundanciesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Redundancies), typeof(ReadOnlyObservableCollection<Redundancy>), typeof(EditRedundantSetVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Redundancies"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RedundanciesProperty = RedundanciesPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public ReadOnlyObservableCollection<Redundancy> Redundancies => (ReadOnlyObservableCollection<Redundancy>)GetValue(RedundanciesProperty);

        #endregion
        
        public EditRedundantSetVM()
        {
            SetValue(RedundanciesPropertyKey, new ReadOnlyObservableCollection<Redundancy>(_backingRedundancies));
        }

        protected override DbSet<RedundantSet> GetDbSet([DisallowNull] LocalDbContext dbContext)
        {
            throw new NotImplementedException();
        }

        protected override bool OnBeforeSave()
        {
            throw new NotImplementedException();
        }

        protected override void OnModelPropertyChanged(RedundantSet oldValue, RedundantSet newValue)
        {
            BinaryProperties = newValue.BinaryProperties;
            Notes = newValue.Notes;
            // TODO: Initialize Redundancies
            //Redundancies = new ReadOnlyCollection<Redundancy>(new Collection<Redundancy>(newValue.Redundancies));
            Reference = newValue.Reference;
        }
    }
}
