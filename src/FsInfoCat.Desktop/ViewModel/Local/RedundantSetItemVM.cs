using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public class RedundantSetItemVM : DbEntityItemVM<RedundantSet>
    {
        #region Notes Property Members

        private static readonly DependencyPropertyKey NotesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Notes), typeof(string), typeof(RedundantSetItemVM), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="Notes"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NotesProperty = NotesPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Notes { get => GetValue(NotesProperty) as string; private set => SetValue(NotesPropertyKey, value); }

        #endregion
        #region Reference Property Members

        private static readonly DependencyPropertyKey ReferencePropertyKey = DependencyProperty.RegisterReadOnly(nameof(Reference), typeof(string), typeof(RedundantSetItemVM), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="Reference"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ReferenceProperty = ReferencePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Reference { get => GetValue(ReferenceProperty) as string; private set => SetValue(ReferencePropertyKey, value); }

        #endregion
        #region BinaryProperties Property Members

        private static readonly DependencyPropertyKey BinaryPropertiesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(BinaryProperties), typeof(BinaryPropertySet), typeof(RedundantSetItemVM),
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

        internal RedundantSetItemVM([DisallowNull] RedundantSet model)
            : base(model)
        {
            Notes = model.Notes;
            // TODO: Initialize properties
        }

        protected override void OnModelPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(RedundantSet.BinaryProperties):
                    Dispatcher.CheckInvoke(() => BinaryProperties = Model?.BinaryProperties);
                    break;
                case nameof(RedundantSet.Notes):
                    Dispatcher.CheckInvoke(() => Notes = Model?.Notes);
                    break;
                case nameof(RedundantSet.Reference):
                    Dispatcher.CheckInvoke(() => Reference = Model?.Reference);
                    break;
            }
        }

        protected override DbSet<RedundantSet> GetDbSet(LocalDbContext dbContext) => dbContext.RedundantSets;
    }
}
