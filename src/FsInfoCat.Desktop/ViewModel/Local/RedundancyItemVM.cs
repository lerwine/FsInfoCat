using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public class RedundancyItemVM : DbEntityItemVM<Redundancy>
    {
        #region File Property Members

        private static readonly DependencyPropertyKey FilePropertyKey = DependencyProperty.RegisterReadOnly(nameof(File), typeof(DbFile), typeof(RedundancyItemVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="File"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FileProperty = FilePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public DbFile File { get => (DbFile)GetValue(FileProperty); private set => SetValue(FilePropertyKey, value); }

        #endregion
        #region Notes Property Members

        private static readonly DependencyPropertyKey NotesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Notes), typeof(string), typeof(RedundancyItemVM), new PropertyMetadata(""));

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
        #region RedundantSet Property Members

        private static readonly DependencyPropertyKey RedundantSetPropertyKey = DependencyProperty.RegisterReadOnly(nameof(RedundantSet), typeof(RedundantSet), typeof(RedundancyItemVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="RedundantSet"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RedundantSetProperty = RedundantSetPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public RedundantSet RedundantSet { get => (RedundantSet)GetValue(RedundantSetProperty); private set => SetValue(RedundantSetPropertyKey, value); }

        #endregion
        #region Reference Property Members

        private static readonly DependencyPropertyKey ReferencePropertyKey = DependencyProperty.RegisterReadOnly(nameof(Reference), typeof(string), typeof(RedundancyItemVM), new PropertyMetadata(""));

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

        public RedundancyItemVM(Redundancy model)
            : base(model)
        {
            File = model.File;
            Notes = model.Notes;
            RedundantSet = model.RedundantSet;
            Reference = model.Reference;
        }

        protected override void OnModelPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Redundancy.File):
                    File = Model?.File;
                    break;
                case nameof(Redundancy.Notes):
                    Notes = Model?.Notes;
                    break;
                case nameof(Redundancy.RedundantSet):
                    RedundantSet = Model?.RedundantSet;
                    break;
                case nameof(Redundancy.Reference):
                    Reference = Model?.Reference;
                    break;
            }
        }

        protected override DbSet<Redundancy> GetDbSet(LocalDbContext dbContext) => dbContext.Redundancies;
    }
}
