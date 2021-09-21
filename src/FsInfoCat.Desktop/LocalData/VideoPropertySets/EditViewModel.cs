using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.LocalData.VideoPropertySets
{
    public class EditViewModel : VideoPropertySetDetailsViewModel<VideoPropertySet, FileWithBinaryPropertiesAndAncestorNames, FileWithBinaryPropertiesAndAncestorNamesViewModel>,
        INavigatedToNotifiable, INavigatingFromNotifiable
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
                IAsyncJob<VideoPropertiesListItem> job = jobFactory.StartNew("Saving changes", "Opening database", Entity, InvocationState, SaveChangesAsync);
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

        private static readonly DependencyPropertyKey ListItemPropertyKey = DependencyPropertyBuilder<EditViewModel, VideoPropertiesListItem>
            .Register(nameof(ListItem))
            .DefaultValue(null)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ListItem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ListItemProperty = ListItemPropertyKey.DependencyProperty;

        public VideoPropertiesListItem ListItem { get => (VideoPropertiesListItem)GetValue(ListItemProperty); private set => SetValue(ListItemPropertyKey, value); }

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

        public EditViewModel([DisallowNull] VideoPropertySet entity, VideoPropertiesListItem listItem) : base(entity)
        {
            SetValue(SaveChangesPropertyKey, new Commands.RelayCommand(OnSaveChangesCommand));
            SetValue(DiscardChangesPropertyKey, new Commands.RelayCommand(OnDiscardChangesCommand));
            IsNew = (ListItem = listItem) is null;
            UpstreamId = entity.UpstreamId;
            LastSynchronizedOn = entity.LastSynchronizedOn;
        }

        void INavigatedToNotifiable.OnNavigatedTo()
        {
            // TODO: Load option lists from database
        }

        private bool ApplyChanges()
        {
            if (Entity.IsChanged())
                Entity.RejectChanges();
            Entity.Compression = Compression;
            Entity.Director = Director;
            Entity.EncodingBitrate = EncodingBitrate;
            Entity.FrameHeight = FrameHeight;
            Entity.FrameRate = FrameRate;
            Entity.FrameWidth = FrameWidth;
            Entity.HorizontalAspectRatio = HorizontalAspectRatio;
            Entity.LastSynchronizedOn = LastSynchronizedOn;
            Entity.StreamName = StreamName;
            Entity.StreamNumber = StreamNumber;
            Entity.UpstreamId = UpstreamId;
            Entity.VerticalAspectRatio = VerticalAspectRatio;
            return Entity.IsChanged();
        }

        private void ReinitializeFromEntity()
        {
            Compression = Entity.Compression;
            Director = Entity.Director;
            EncodingBitrate = Entity.EncodingBitrate;
            FrameHeight = Entity.FrameHeight;
            FrameRate = Entity.FrameRate;
            FrameWidth = Entity.FrameWidth;
            HorizontalAspectRatio = Entity.HorizontalAspectRatio;
            LastSynchronizedOn = Entity.LastSynchronizedOn;
            StreamName = Entity.StreamName;
            StreamNumber = Entity.StreamNumber;
            UpstreamId = Entity.UpstreamId;
            VerticalAspectRatio = Entity.VerticalAspectRatio;
        }

        private static async Task<VideoPropertiesListItem> SaveChangesAsync(VideoPropertySet entity, object invocationState, IWindowsStatusListener statusListener)
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
                    entry = dbContext.VideoPropertySets.Add(entity);
                    isNew = true;
                    break;
                default:
                    isNew = false;
                    break;
            }
            if (entry.State == EntityState.Detached)
                entry = dbContext.VideoPropertySets.Add(entity);
            DispatcherOperation dispatcherOperation = statusListener.BeginSetMessage((entry.State == EntityState.Added) ?
                "Inserting new video properties record into database" : "Saving video properties record changes to database");
            await dbContext.SaveChangesAsync(statusListener.CancellationToken);
            await dispatcherOperation;
            if (isNew)
                return await dbContext.VideoPropertiesListing.FindAsync(entity.Id, statusListener.CancellationToken);
            return invocationState is VideoPropertiesListItem item ? item : null;
        }

        private void OnSaveTaskCompleted(Task<VideoPropertiesListItem> task)
        {
            if (task.IsCanceled)
                return;
            if (task.IsFaulted)
                _ = MessageBox.Show(Application.Current.MainWindow,
                    ((task.Exception.InnerException is AsyncOperationFailureException aExc) ? aExc.UserMessage.NullIfWhiteSpace() :
                        task.Exception.InnerExceptions.OfType<AsyncOperationFailureException>().Select(e => e.UserMessage)
                        .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                        "There was an unexpected error while loading items from the database.\n\nSee logs for further information",
                    "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (IsNew)
                RaiseItemInsertedResult(task.Result);
            else
                RaiseItemUpdatedResult();
        }

        protected override void RejectChanges()
        {
            base.RejectChanges();
            ReinitializeFromEntity();
        }

        void INavigatingFromNotifiable.OnNavigatingFrom(CancelEventArgs e)
        {
            if (ApplyChanges())
            {
                switch (MessageBox.Show(Application.Current.MainWindow, "There are unsaved changes. Do you wish to save them before continuing?", "Unsaved Changes",
                    MessageBoxButton.YesNoCancel, MessageBoxImage.Warning))
                {
                    case MessageBoxResult.Yes:
                        IWindowsAsyncJobFactoryService jobFactory = Services.GetRequiredService<IWindowsAsyncJobFactoryService>();
                        IAsyncJob<VideoPropertiesListItem> job = jobFactory.StartNew("Saving changes", "Opening database", Entity, InvocationState, SaveChangesAsync);
                        job.Task.ContinueWith(task => Dispatcher.Invoke(() => OnSaveTaskCompleted(task)));
                        e.Cancel = true;
                        break;
                    case MessageBoxResult.No:
                        break;
                    default:
                        e.Cancel = true;
                        break;
                }
            }
        }
    }
}
