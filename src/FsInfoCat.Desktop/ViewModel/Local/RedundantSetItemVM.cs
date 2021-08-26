using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    /// <summary>
    /// View model for <see cref="DbEntityListingPageVM{TDbEntity, TItemVM}.Items"/> in the <see cref="RedundantSetsPageVM"/> view model.
    /// </summary>
    public class RedundantSetItemVM : DbEntityItemVM<RedundantSetListItem>
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
        #region Hash Property Members

        private static readonly DependencyPropertyKey HashPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Hash), typeof(MD5Hash?), typeof(RedundantSetItemVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Hash"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HashProperty = HashPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public MD5Hash? Hash { get => (MD5Hash?)GetValue(HashProperty); private set => SetValue(HashPropertyKey, value); }

        #endregion
        #region Length Property Members

        private static readonly DependencyPropertyKey LengthPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Length), typeof(long), typeof(RedundantSetItemVM),
                new PropertyMetadata(0L));

        /// <summary>
        /// Identifies the <see cref="Length"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LengthProperty = LengthPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public long Length { get => (long)GetValue(LengthProperty); private set => SetValue(LengthPropertyKey, value); }

        #endregion
        #region RedundancyCount Property Members

        private static readonly DependencyPropertyKey RedundancyCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(RedundancyCount), typeof(long), typeof(RedundantSetItemVM),
                new PropertyMetadata(0L));

        /// <summary>
        /// Identifies the <see cref="RedundancyCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RedundancyCountProperty = RedundancyCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public long RedundancyCount { get => (long)GetValue(RedundancyCountProperty); private set => SetValue(RedundancyCountPropertyKey, value); }

        #endregion

        internal RedundantSetItemVM([DisallowNull] RedundantSetListItem model)
            : base(model)
        {
            Notes = model.Notes;
            Hash = model.Hash;
            Length = model.Length;
            RedundancyCount = model.RedundancyCount;
            Reference = model.Reference;
        }

        protected override void OnNestedModelPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(RedundantSetListItem.Reference):
                    Dispatcher.CheckInvoke(() => Reference = Model?.Reference);
                    break;
                case nameof(RedundantSetListItem.Hash):
                    Dispatcher.CheckInvoke(() => Hash = Model?.Hash);
                    break;
                case nameof(RedundantSetListItem.Length):
                    Dispatcher.CheckInvoke(() => Length = Model?.Length ?? 0L);
                    break;
                case nameof(RedundantSetListItem.RedundancyCount):
                    Dispatcher.CheckInvoke(() => RedundancyCount = Model?.RedundancyCount ?? 0L);
                    break;
                case nameof(RedundantSetListItem.Notes):
                    Dispatcher.CheckInvoke(() => Notes = Model?.Notes);
                    break;
            }
        }

        protected override DbSet<RedundantSetListItem> GetDbSet(LocalDbContext dbContext) => dbContext.RedundantSetListing;
    }
}
