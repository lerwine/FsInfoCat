using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public class BinaryPropertySetItemVM : DbEntityItemVM<BinaryPropertySet>
    {
        #region Hash Property Members

        private static readonly DependencyPropertyKey HashPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Hash), typeof(MD5Hash?), typeof(BinaryPropertySetItemVM),
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

        private static readonly DependencyPropertyKey LengthPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Length), typeof(long), typeof(BinaryPropertySetItemVM),
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

        public BinaryPropertySetItemVM([DisallowNull] BinaryPropertySet model)
            : base(model)
        {
            Hash = model.Hash;
            Length = model.Length;
        }

        protected override void OnNestedModelPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(BinaryPropertySet.Hash):
                    Dispatcher.CheckInvoke(() => Hash = Model?.Hash);
                    break;
                case nameof(BinaryPropertySet.Length):
                    Dispatcher.CheckInvoke(() => Length = Model?.Length ?? 0L);
                    break;
            }
        }

        protected override DbSet<BinaryPropertySet> GetDbSet(LocalDbContext dbContext) => dbContext.BinaryPropertySets;
    }
}
