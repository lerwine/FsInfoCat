using FsInfoCat.Desktop.ViewModel.AsyncOps;
using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public class SymbolicNamesPageVM : DbEntityListingPageVM<SymbolicName, SymbolicNameItemVM>
    {
        #region IsEditingViewOptions Property Members

        private static readonly DependencyPropertyKey IsEditingViewOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsEditingViewOptions), typeof(bool), typeof(SymbolicNamesPageVM),
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
            typeof(Commands.RelayCommand), typeof(SymbolicNamesPageVM), new PropertyMetadata(null));

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
            typeof(Commands.RelayCommand), typeof(SymbolicNamesPageVM), new PropertyMetadata(null));

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

        private static readonly DependencyPropertyKey ShowViewOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ShowViewOptions), typeof(Commands.RelayCommand), typeof(SymbolicNamesPageVM), new PropertyMetadata(null));

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

        private static readonly DependencyPropertyKey ViewOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewOptions), typeof(ThreeStateViewModel), typeof(SymbolicNamesPageVM),
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

        private static readonly DependencyPropertyKey EditingViewOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(EditingViewOptions), typeof(ThreeStateViewModel), typeof(SymbolicNamesPageVM),
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

        internal Task<int> LoadAsync(bool? isInactive)
        {
            return BgOps.FromAsync("Loading items", "Connecting to database...", isInactive, LoadItemsAsync);
        }

        private async Task<int> LoadItemsAsync(bool? isInactive, AsyncOps.IStatusListener statusListener)
        {
            statusListener.CancellationToken.ThrowIfCancellationRequested();
            IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            IQueryable<SymbolicName> items;
            if (isInactive.HasValue)
            {
                if (isInactive.Value)
                    items = from s in dbContext.SymbolicNames where s.IsInactive select s;
                else
                    items = from s in dbContext.SymbolicNames where !s.IsInactive select s;
            }
            else
                items = from s in dbContext.SymbolicNames select s;
            return await OnEntitiesLoaded(items, statusListener, entity => new SymbolicNameItemVM(entity));
        }

        protected override DbSet<SymbolicName> GetDbSet(LocalDbContext dbContext) => dbContext.SymbolicNames;

        protected override SymbolicName InitializeNewEntity()
        {
            SymbolicName entity = base.InitializeNewEntity();
            bool? isInactive = ViewOptions.Value;
            if (isInactive.HasValue)
                entity.IsInactive = isInactive.Value;
            return entity;
        }
        protected override string GetDeleteProgressTitle(SymbolicNameItemVM item)
        {
            throw new NotImplementedException();
        }

        protected override string GetSaveExistingProgressTitle(SymbolicNameItemVM item)
        {
            throw new NotImplementedException();
        }

        protected override string GetSaveNewProgressTitle(SymbolicNameItemVM item)
        {
            throw new NotImplementedException();
        }

        protected override bool PromptItemDeleting(SymbolicNameItemVM item, object parameter)
        {
            throw new NotImplementedException();
        }

        protected override bool ShowModalItemEditWindow(SymbolicNameItemVM item, object parameter)
        {
            throw new NotImplementedException();
        }

        protected override Func<IStatusListener, Task<int>> GetItemsLoaderFactory()
        {
            throw new NotImplementedException();
        }
    }
}
