using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.LocalData.RedundantSets
{
    public class EditViewModel : RedundantSetDetailsViewModel<RedundantSet, BinaryPropertySet, BinaryPropertySetRowViewModel<BinaryPropertySet>, Redundancy, RedundancyRowViewModel<Redundancy>>, INavigatedToNotifiable, INavigatingFromNotifiable
    {
        #region SaveChanges Command Property Members

        private static readonly DependencyPropertyKey SaveChangesPropertyKey = DependencyPropertyBuilder<EditViewModel, Commands.RelayCommand>
            .Register(nameof(SaveChanges))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="SaveChanges"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SaveChangesProperty = SaveChangesPropertyKey.DependencyProperty;

        public Commands.RelayCommand SaveChanges => (Commands.RelayCommand)GetValue(SaveChangesProperty);

        /// <summary>
        /// Called when the <see cref="SaveChanges">SaveChanges Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="SaveChanges" />.</param>
        protected virtual void OnSaveChangesCommand(object parameter)
        {
            if (ApplyChanges() || IsNew)
            {
                IWindowsAsyncJobFactoryService jobFactory = Services.GetRequiredService<IWindowsAsyncJobFactoryService>();
                IAsyncJob<RedundantSetListItem> job = jobFactory.StartNew("Saving changes", "Opening database", Entity, InvocationState, SaveChangesAsync);
                job.Task.ContinueWith(task => Dispatcher.Invoke(() => OnSaveTaskCompleted(task)));
            }
            else
                RaiseItemUnmodifiedResult();
        }

        #endregion
        #region DiscardChanges Command Property Members

        private static readonly DependencyPropertyKey DiscardChangesPropertyKey = DependencyPropertyBuilder<EditViewModel, Commands.RelayCommand>
            .Register(nameof(DiscardChanges))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="DiscardChanges"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DiscardChangesProperty = DiscardChangesPropertyKey.DependencyProperty;

        public Commands.RelayCommand DiscardChanges => (Commands.RelayCommand)GetValue(DiscardChangesProperty);

        /// <summary>
        /// Called when the <see cref="DiscardChanges">DiscardChanges Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="DiscardChanges" />.</param>
        protected virtual void OnDiscardChangesCommand(object parameter)
        {
            RejectChanges();
            RaiseItemUnmodifiedResult();
        }

        #endregion
        #region ListItem Property Members

        private static readonly DependencyPropertyKey ListItemPropertyKey = DependencyPropertyBuilder<EditViewModel, RedundantSetListItem>
            .Register(nameof(ListItem))
            .DefaultValue(null)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ListItem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ListItemProperty = ListItemPropertyKey.DependencyProperty;

        public RedundantSetListItem ListItem { get => (RedundantSetListItem)GetValue(ListItemProperty); private set => SetValue(ListItemPropertyKey, value); }

        #endregion
        #region UpstreamId Property Members

        private static readonly DependencyPropertyKey UpstreamIdPropertyKey = DependencyPropertyBuilder<EditViewModel, Guid?>
            .Register(nameof(UpstreamId))
            .DefaultValue(null)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="UpstreamId"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty UpstreamIdProperty = UpstreamIdPropertyKey.DependencyProperty;

        public Guid? UpstreamId { get => (Guid?)GetValue(UpstreamIdProperty); private set => SetValue(UpstreamIdPropertyKey, value); }

        #endregion
        #region LastSynchronizedOn Property Members

        private static readonly DependencyPropertyKey LastSynchronizedOnPropertyKey = DependencyPropertyBuilder<EditViewModel, DateTime?>
            .Register(nameof(LastSynchronizedOn))
            .DefaultValue(null)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="LastSynchronizedOn"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LastSynchronizedOnProperty = LastSynchronizedOnPropertyKey.DependencyProperty;

        public DateTime? LastSynchronizedOn { get => (DateTime?)GetValue(LastSynchronizedOnProperty); private set => SetValue(LastSynchronizedOnPropertyKey, value); }

        #endregion
        #region IsNew Property Members

        private static readonly DependencyPropertyKey IsNewPropertyKey = DependencyPropertyBuilder<EditViewModel, bool>
            .Register(nameof(IsNew))
            .DefaultValue(false)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="IsNew"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsNewProperty = IsNewPropertyKey.DependencyProperty;

        public bool IsNew { get => (bool)GetValue(IsNewProperty); private set => SetValue(IsNewPropertyKey, value); }

        #endregion

        public EditViewModel(RedundantSet entity, RedundantSetListItem listItem) : base(entity)
        {
            SetValue(SaveChangesPropertyKey, new Commands.RelayCommand(OnSaveChangesCommand));
            SetValue(DiscardChangesPropertyKey, new Commands.RelayCommand(OnDiscardChangesCommand));
            IsNew = (ListItem = listItem) is null;
            UpstreamId = entity.UpstreamId;
            LastSynchronizedOn = entity.LastSynchronizedOn;
            // TODO: Implement EditViewModel
        }

        void INavigatedToNotifiable.OnNavigatedTo()
        {
            // TODO: Load option lists from database
        }

        void INavigatingFromNotifiable.OnNavigatingFrom(CancelEventArgs e)
        {
            // TODO: Implement OnNavigatingFrom
        }

        private static async Task<RedundantSetListItem> SaveChangesAsync(RedundantSet entity, object invocationState, IWindowsStatusListener statusListener)
        {
            using IServiceScope scope = Services.CreateScope();
            using LocalDbContext dbContext = scope.ServiceProvider.GetRequiredService<LocalDbContext>();
            EntityEntry entry = dbContext.Entry(entity);
            bool isNew = entry.State == EntityState.Detached;
            switch (entry.State)
            {
                case EntityState.Added:
                    isNew = true;
                    break;
                case EntityState.Detached:
                    entry = dbContext.RedundantSets.Add(entity);
                    isNew = true;
                    break;
                default:
                    isNew = false;
                    break;
            }
            if (entry.State == EntityState.Detached)
                entry = dbContext.RedundantSets.Add(entity);
            DispatcherOperation dispatcherOperation = statusListener.BeginSetMessage((entry.State == EntityState.Added) ?
                "Inserting new redundancy set into database" : "Saving redundancy set changes to database");
            await dbContext.SaveChangesAsync(statusListener.CancellationToken);
            await dispatcherOperation;
            if (isNew)
                return await dbContext.RedundantSetListing.FindAsync(entity.Id, statusListener.CancellationToken);
            return invocationState is RedundantSetListItem item ? item : null;
        }

        private void OnSaveTaskCompleted(Task<RedundantSetListItem> task)
        {
            // TODO: Implement OnSaveTaskCompleted
            throw new NotImplementedException("OnSaveTaskCompleted not implemented");
        }

        private bool ApplyChanges()
        {
            // TODO: Implement ApplyChanges
            throw new NotImplementedException("ApplyChanges not implemented");
        }
    }
}
