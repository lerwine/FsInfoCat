using FsInfoCat.Desktop.ViewModel.AsyncOps;
using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public class PersonalTagDefinitionsPageVM : DbEntityListingPageVM<PersonalTagDefinition, PersonalTagDefinitionItemVM>
    {
        #region IsEditingViewOptions Property Members

        private static readonly DependencyPropertyKey IsEditingViewOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsEditingViewOptions), typeof(bool), typeof(PersonalTagDefinitionsPageVM),
                new PropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="IsEditingViewOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsEditingViewOptionsProperty = IsEditingViewOptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public bool IsEditingViewOptions { get => (bool)GetValue(IsEditingViewOptionsProperty); private set => SetValue(IsEditingViewOptionsPropertyKey, value); }

        #endregion
        #region ViewOptionsOkClick Command Property Members

        private static readonly DependencyPropertyKey ViewOptionsOkClickPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewOptionsOkClick),
            typeof(Commands.RelayCommand), typeof(PersonalTagDefinitionsPageVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ViewOptionsOkClick"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewOptionsOkClickProperty = ViewOptionsOkClickPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand ViewOptionsOkClick => (Commands.RelayCommand)GetValue(ViewOptionsOkClickProperty);

        #endregion
        #region ViewOptionCancelClick Command Property Members

        private static readonly DependencyPropertyKey ViewOptionCancelClickPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewOptionCancelClick),
            typeof(Commands.RelayCommand), typeof(PersonalTagDefinitionsPageVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ViewOptionCancelClick"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewOptionCancelClickProperty = ViewOptionCancelClickPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand ViewOptionCancelClick => (Commands.RelayCommand)GetValue(ViewOptionCancelClickProperty);

        #endregion
        #region ShowViewOptions Command Property Members

        private static readonly DependencyPropertyKey ShowViewOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ShowViewOptions), typeof(Commands.RelayCommand), typeof(PersonalTagDefinitionsPageVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ShowViewOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowViewOptionsProperty = ShowViewOptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand ShowViewOptions => (Commands.RelayCommand)GetValue(ShowViewOptionsProperty);

        #endregion
        #region ViewOptions Property Members

        private static readonly DependencyPropertyKey ViewOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewOptions), typeof(ThreeStateViewModel), typeof(PersonalTagDefinitionsPageVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ViewOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewOptionsProperty = ViewOptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public ThreeStateViewModel ViewOptions => (ThreeStateViewModel)GetValue(ViewOptionsProperty);

        #endregion
        #region EditingViewOptions Property Members

        private static readonly DependencyPropertyKey EditingViewOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(EditingViewOptions), typeof(ThreeStateViewModel), typeof(PersonalTagDefinitionsPageVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="EditingViewOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EditingViewOptionsProperty = EditingViewOptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public ThreeStateViewModel EditingViewOptions => (ThreeStateViewModel)GetValue(EditingViewOptionsProperty);

        #endregion
        #region SelectedItem Property Members

        /// <summary>
        /// Identifies the <see cref="SelectedItem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(nameof(SelectedItem), typeof(PersonalTagDefinitionItemVM), typeof(PersonalTagDefinitionsPageVM),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as PersonalTagDefinitionsPageVM)?.OnSelectedItemPropertyChanged((PersonalTagDefinitionItemVM)e.OldValue, (PersonalTagDefinitionItemVM)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public PersonalTagDefinitionItemVM SelectedItem { get => (PersonalTagDefinitionItemVM)GetValue(SelectedItemProperty); set => SetValue(SelectedItemProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="SelectedItem"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="SelectedItem"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="SelectedItem"/> property.</param>
        private void OnSelectedItemPropertyChanged(PersonalTagDefinitionItemVM oldValue, PersonalTagDefinitionItemVM newValue)
        {
            // TODO: Implement OnSelectedItemPropertyChanged Logic
        }

        #endregion

        public PersonalTagDefinitionsPageVM()
        {
            ThreeStateViewModel viewOptions = new(true);
            SetValue(ViewOptionsPropertyKey, viewOptions);
            SetValue(EditingViewOptionsPropertyKey, new ThreeStateViewModel(viewOptions.Value));
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
            viewOptions.ValuePropertyChanged += (s, e) => LoadItemsAsync();
            SetValue(ShowViewOptionsPropertyKey, new Commands.RelayCommand(() => IsEditingViewOptions = true));
            SetValue(ViewOptionsOkClickPropertyKey, new Commands.RelayCommand(() =>
            {
                IsEditingViewOptions = false;
                ViewOptions.Value = EditingViewOptions.Value;
            }));
            SetValue(ViewOptionCancelClickPropertyKey, new Commands.RelayCommand(() =>
            {
                EditingViewOptions.Value = ViewOptions.Value;
                IsEditingViewOptions = false;
            }));
        }

        protected override PersonalTagDefinitionItemVM CreateItem(PersonalTagDefinition entity) => new(entity);

        protected override DbSet<PersonalTagDefinition> GetDbSet(LocalDbContext dbContext) => dbContext.PersonalTagDefinitions;

        protected override string GetDeleteProgressTitle(PersonalTagDefinitionItemVM item) => $"Deleting Personal Tag Definition \"{item.Name}\"";

        protected override Func<IStatusListener, Task<int>> GetItemsLoaderFactory()
        {
            bool? viewOptions = ViewOptions.Value;
            return listener => Task.Run(async () => await LoadItemsAsync(viewOptions, listener));
        }

        private async Task<int> LoadItemsAsync(bool? showActive, IStatusListener statusListener)
        {
            statusListener.CancellationToken.ThrowIfCancellationRequested();
            IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            IQueryable<EntityAndCounts> items;
            if (showActive.HasValue)
            {
                if (showActive.Value)
                    items = from f in dbContext.PersonalTagDefinitions where !f.IsInactive select new EntityAndCounts(f, f.VolumeTags.Count(), f.SubdirectoryTags.Count(), f.FileTags.Count());
                else
                    items = from f in dbContext.PersonalTagDefinitions where f.IsInactive select new EntityAndCounts(f, f.VolumeTags.Count(), f.SubdirectoryTags.Count(), f.FileTags.Count());
            }
            else
                items = from f in dbContext.PersonalTagDefinitions select new EntityAndCounts(f, f.VolumeTags.Count(), f.SubdirectoryTags.Count(), f.FileTags.Count());
            return await OnEntitiesLoaded(items, statusListener, r => new PersonalTagDefinitionItemVM(r));
        }

        protected override string GetSaveExistingProgressTitle(PersonalTagDefinitionItemVM item) => $"Saving Personal Tag Definition \"{item.Name}\"";

        protected override string GetSaveNewProgressTitle(PersonalTagDefinitionItemVM item) => $"Adding new Personal Tag Definition \"{item.Name}\"";

        protected override PersonalTagDefinition InitializeNewEntity() => new() { CreatedOn = DateTime.Now };

        protected override bool PromptItemDeleting(PersonalTagDefinitionItemVM item, object parameter)
        {
            throw new NotImplementedException();
        }

        protected override bool ShowModalItemEditWindow(PersonalTagDefinitionItemVM item, object parameter)
        {
            throw new NotImplementedException();
        }

        public record EntityAndCounts(PersonalTagDefinition Entity, int VolumeCount, int SubdirectoryCount, int FileCount);
    }
}
