using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.Local.PersonalTagDefinitions
{
    public class ListingViewModel : ListingViewModel<PersonalTagDefinitionListItem, ListItemViewModel, bool?>, INotifyNavigatedTo
    {
        #region ViewOptions Property Members

        private static readonly DependencyPropertyKey ViewOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewOptions), typeof(ThreeStateViewModel), typeof(ListingViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ViewOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewOptionsProperty = ViewOptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the view model for the listing view options.
        /// </summary>
        /// <value>The view model that indicates what items to load into the <see cref="Items"/> collection.</value>
        public ThreeStateViewModel ViewOptions => (ThreeStateViewModel)GetValue(ViewOptionsProperty);

        #endregion
        #region EditingOptions Property Members

        private static readonly DependencyPropertyKey EditingOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(EditingOptions), typeof(ThreeStateViewModel), typeof(ListingViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="EditingOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EditingOptionsProperty = EditingOptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public ThreeStateViewModel EditingOptions => (ThreeStateViewModel)GetValue(EditingOptionsProperty);

        #endregion

        public ListingViewModel()
        {
            ThreeStateViewModel viewOptions = new(true);
            SetValue(ViewOptionsPropertyKey, viewOptions);
            viewOptions.ValuePropertyChanged += (sender, e) => ReloadAsync(e.NewValue as bool?);
            SetValue(EditingOptionsPropertyKey, new ThreeStateViewModel(viewOptions.Value));
        }

        void INotifyNavigatedTo.OnNavigatedTo() => ReloadAsync(ViewOptions.Value);

        protected override IQueryable<PersonalTagDefinitionListItem> GetQueryableListing(bool? options, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IWindowsStatusListener statusListener)
        {
            statusListener.SetMessage("Reading personal tags from database");
            return options.HasValue ? (options.Value ? dbContext.PersonalTagDefinitionListing.Where(f => !f.IsInactive) :
                dbContext.PersonalTagDefinitionListing.Where(f => f.IsInactive)) : dbContext.PersonalTagDefinitionListing;
        }

        protected override ListItemViewModel CreateItemViewModel([DisallowNull] PersonalTagDefinitionListItem entity) => new ListItemViewModel(entity);

        protected override void OnApplyFilterOptionsCommand(object parameter)
        {
            ViewOptions.Value = EditingOptions.Value;
        }

        protected override void OnCancelFilterOptionsCommand(object parameter)
        {
            EditingOptions.Value = ViewOptions.Value;
            base.OnCancelFilterOptionsCommand(parameter);
        }

        protected override void OnRefreshCommand(object parameter) => ReloadAsync(ViewOptions.Value);

        protected override void OnItemEditCommand([DisallowNull] ListItemViewModel item, object parameter)
        {
            throw new System.NotImplementedException();
        }

        protected override bool ConfirmItemDelete(ListItemViewModel item, object parameter)
        {
            string message;
            switch (item.FileTagCount)
            {
                case 0:
                    switch (item.SubdirectoryTagCount)
                    {
                        case 0:
                            message = item.VolumeTagCount switch
                            {
                                0 => $" ",
                                1 => $", including 1 volume tag, ",
                                _ => $", including all {item.VolumeTagCount} volume tags, ",
                            };
                            break;
                        case 1:
                            message = item.VolumeTagCount switch
                            {
                                0 => $", including 1 sub-directory tag, ",
                                1 => $", including 1 sub-directory tag and 1 volume tag, ",
                                _ => $", including 1 sub-directory tag and all {item.VolumeTagCount} volume tags, ",
                            };
                            break;
                        default:
                            message = item.VolumeTagCount switch
                            {
                                0 => $", including all {item.SubdirectoryTagCount} sub-directory tags, ",
                                1 => $", including all {item.SubdirectoryTagCount} sub-directory tags and 1 volume tag, ",
                                _ => $", including all {item.SubdirectoryTagCount} sub-directory and {item.VolumeTagCount} volume tags, ",
                            };
                            break;
                    }
                    break;
                case 1:
                    switch (item.SubdirectoryTagCount)
                    {
                        case 0:
                            message = item.VolumeTagCount switch
                            {
                                0 => $", including 1 file tag, ",
                                1 => $", including 1 file tag and 1 volume tag, ",
                                _ => $", including 1 file tag and all {item.VolumeTagCount} volume tags, ",
                            };
                            break;
                        case 1:
                            message = item.VolumeTagCount switch
                            {
                                0 => $", including 1 file tag and 1 sub-directory tag, ",
                                1 => $", including 1 file tag, 1 sub-directory tag and 1 volume tag, ",
                                _ => $", including 1 file tag, 1 sub-directory tag and all {item.VolumeTagCount} volume tags, ",
                            };
                            break;
                        default:
                            message = item.VolumeTagCount switch
                            {
                                0 => $", including 1 file tag and all {item.SubdirectoryTagCount} sub-directory tags, ",
                                1 => $", including 1 file tag and all {item.SubdirectoryTagCount} sub-directory tags and 1 volume tag, ",
                                _ => $", including 1 file tag and all {item.SubdirectoryTagCount} sub-directory and {item.VolumeTagCount} volume tags, ",
                            };
                            break;
                    }
                    break;
                default:
                    switch (item.SubdirectoryTagCount)
                    {
                        case 0:
                            message = item.VolumeTagCount switch
                            {
                                0 => $", including all {item.FileTagCount} file tags, ",
                                1 => $", including all {item.FileTagCount} file tags and 1 volume tag, ",
                                _ => $", including all {item.FileTagCount} file tags and {item.VolumeTagCount} volume tags, ",
                            };
                            break;
                        case 1:
                            message = item.VolumeTagCount switch
                            {
                                0 => $", including all {item.FileTagCount} file tags and 1 sub-directory tag, ",
                                1 => $", including all {item.FileTagCount} file tags, 1 sub-directory tag and 1 volume tag, ",
                                _ => $", including all {item.FileTagCount} file tags, 1 sub-directory tag and {item.VolumeTagCount} volume tags, ",
                            };
                            break;
                        default:
                            message = item.VolumeTagCount switch
                            {
                                0 => $", including all {item.FileTagCount} file and {item.SubdirectoryTagCount} sub-directory tags, ",
                                1 => $", including all {item.FileTagCount} file tags, {item.SubdirectoryTagCount} sub-directory tags and 1 volume tag, ",
                                _ => $", including all {item.FileTagCount} file tags, {item.SubdirectoryTagCount} sub-directory tags and {item.VolumeTagCount} volume tags, ",
                            };
                            break;
                    }
                    break;
            }
            return MessageBox.Show(App.Current.MainWindow, $"This action cannot be undone!\n\nAre you sure you want to remove this personal tag{message}from the database?",
                "Delete Personal Tag", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes;
        }

        protected override async Task<int> DeleteEntityFromDbContextAsync([DisallowNull] PersonalTagDefinitionListItem entity, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IWindowsStatusListener statusListener)
        {
            PersonalTagDefinition target = await dbContext.PersonalTagDefinitions.FindAsync(new object[] { entity.Id }, statusListener.CancellationToken);
            return (target is null) ? 0 : await PersonalTagDefinition.DeleteAsync(target, dbContext, statusListener);
        }

        protected override void OnAddNewItemCommand(object parameter)
        {
            throw new System.NotImplementedException();
        }
    }
}
