using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public class RedundancyItemVM : DbEntityItemVM<Redundancy>
    {
        #region File Property Members

        private static readonly DependencyPropertyKey FilePropertyKey = DependencyProperty.RegisterReadOnly(nameof(File), typeof(FileItemVM), typeof(RedundancyItemVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="File"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FileProperty = FilePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public FileItemVM File { get => (FileItemVM)GetValue(FileProperty); private set => SetValue(FilePropertyKey, value); }

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

        private static readonly DependencyPropertyKey RedundantSetPropertyKey = DependencyProperty.RegisterReadOnly(nameof(RedundantSet), typeof(RedundantSetItemVM), typeof(RedundancyItemVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="RedundantSet"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RedundantSetProperty = RedundantSetPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public RedundantSetItemVM RedundantSet { get => (RedundantSetItemVM)GetValue(RedundantSetProperty); private set => SetValue(RedundantSetPropertyKey, value); }

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
            File = model.File.ToItemViewModel();
            Notes = model.Notes;
            RedundantSet = model.RedundantSet.ToItemViewModel();
            Reference = model.Reference;
        }

        protected override void OnNestedModelPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Redundancy.File):
                    Dispatcher.CheckInvoke(() => File = Model?.File.ToItemViewModel());
                    break;
                case nameof(Redundancy.Notes):
                    Dispatcher.CheckInvoke(() => Notes = Model?.Notes ?? "");
                    break;
                case nameof(Redundancy.RedundantSet):
                    Dispatcher.CheckInvoke(() => RedundantSet = Model?.RedundantSet.ToItemViewModel());
                    break;
                case nameof(Redundancy.Reference):
                    Dispatcher.CheckInvoke(() => Reference = Model?.Reference);
                    break;
            }
        }

        protected override DbSet<Redundancy> GetDbSet(LocalDbContext dbContext) => dbContext.Redundancies;
    }
}
