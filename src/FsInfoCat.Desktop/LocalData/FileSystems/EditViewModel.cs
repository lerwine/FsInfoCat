using FsInfoCat.Activities;
using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;
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

namespace FsInfoCat.Desktop.LocalData.FileSystems
{
    public class EditViewModel : FileSystemDetailsViewModel<FileSystem, VolumeListItem, VolumeListItemViewModel, SymbolicNameRow, SymbolicNameListItemViewModel>,
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
            // TODO: Implement FileSystems.EditViewModel.OnSaveChangesCommand
            //if (ApplyChanges() || IsNew)
            //{
            //    throw new NotImplementedException();
            //    //IWindowsAsyncJobFactoryService jobFactory = Hosting.GetRequiredService<IWindowsAsyncJobFactoryService>();
            //    //IAsyncJob<FileSystemListItem> job = jobFactory.StartNew("Saving changes", "Opening database", Entity, InvocationState, SaveChangesAsync);
            //    //job.Task.ContinueWith(task => Dispatcher.Invoke(() => OnSaveTaskCompleted(task)));
            //}
            //else
            //    RaiseItemUnmodifiedResult();
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

        private static readonly DependencyPropertyKey ListItemPropertyKey = DependencyPropertyBuilder<EditViewModel, FileSystemListItem>
            .Register(nameof(ListItem))
            .DefaultValue(null)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ListItem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ListItemProperty = ListItemPropertyKey.DependencyProperty;

        public FileSystemListItem ListItem { get => (FileSystemListItem)GetValue(ListItemProperty); private set => SetValue(ListItemPropertyKey, value); }

        #endregion
        #region UpstreamId Property Members

        private static readonly DependencyPropertyKey UpstreamIdPropertyKey = DependencyProperty.RegisterReadOnly(nameof(UpstreamId), typeof(Guid?), typeof(EditViewModel),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as EditViewModel).OnUpstreamIdPropertyChanged((Guid?)e.OldValue, (Guid?)e.NewValue)));

        /// <summary>
        /// Identifies the <see cref="UpstreamId"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty UpstreamIdProperty = UpstreamIdPropertyKey.DependencyProperty;

        public Guid? UpstreamId { get => (Guid?)GetValue(UpstreamIdProperty); private set => SetValue(UpstreamIdPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="UpstreamId"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="UpstreamId"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="UpstreamId"/> property.</param>
        private void OnUpstreamIdPropertyChanged(Guid? oldValue, Guid? newValue) { }

        #endregion
        #region LastSynchronizedOn Property Members

        private static readonly DependencyPropertyKey LastSynchronizedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastSynchronizedOn), typeof(DateTime?), typeof(EditViewModel),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as EditViewModel).OnLastSynchronizedOnPropertyChanged((DateTime?)e.OldValue, (DateTime?)e.NewValue)));

        /// <summary>
        /// Identifies the <see cref="LastSynchronizedOn"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LastSynchronizedOnProperty = LastSynchronizedOnPropertyKey.DependencyProperty;

        public DateTime? LastSynchronizedOn { get => (DateTime?)GetValue(LastSynchronizedOnProperty); private set => SetValue(LastSynchronizedOnPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="LastSynchronizedOn"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="LastSynchronizedOn"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="LastSynchronizedOn"/> property.</param>
        private void OnLastSynchronizedOnPropertyChanged(DateTime? oldValue, DateTime? newValue) { }

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

        public EditViewModel([DisallowNull] FileSystem entity, FileSystemListItem listItem) : base(entity)
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

        private void ApplyChanges()
        {
            Entity.DefaultDriveType = DefaultDriveType;
            Entity.DisplayName = DisplayName;
            Entity.IsInactive = IsInactive;
            Entity.LastSynchronizedOn = LastSynchronizedOn;
            Entity.MaxNameLength = MaxNameLength;
            Entity.Notes = Notes;
            Entity.ReadOnly = ReadOnly;
            Entity.UpstreamId = UpstreamId;
        }

        private void ReinitializeFromEntity()
        {
            DefaultDriveType = Entity.DefaultDriveType;
            DisplayName = Entity.DisplayName;
            IsInactive = Entity.IsInactive;
            LastSynchronizedOn = Entity.LastSynchronizedOn;
            MaxNameLength = Entity.MaxNameLength;
            Notes = Entity.Notes;
            ReadOnly = Entity.ReadOnly;
            UpstreamId = Entity.UpstreamId;
        }

        private static async Task<FileSystemListItem> SaveChangesAsync(FileSystem entity, object invocationState, IActivityProgress progress)
        {
            using IServiceScope scope = Hosting.CreateScope();
            using LocalDbContext dbContext = scope.ServiceProvider.GetRequiredService<LocalDbContext>();
            EntityEntry entry = dbContext.Entry(entity);
            bool isNew;
            switch (entry.State)
            {
                case EntityState.Added:
                    isNew = true;
                    break;
                case EntityState.Detached:
                    entry = dbContext.FileSystems.Add(entity);
                    isNew = true;
                    break;
                default:
                    isNew = false;
                    break;
            }
            if (entry.State == EntityState.Detached)
                entry = dbContext.FileSystems.Add(entity);
            progress.Report((entry.State == EntityState.Added) ?
                "Inserting new file system record into database" : "Saving file system record changes to database");
            await dbContext.SaveChangesAsync(progress.Token);
            if (isNew)
                return await dbContext.FileSystemListing.FindAsync(entity.Id, progress.Token);
            return invocationState is FileSystemListItem item ? item : null;
        }

        private void OnSaveTaskCompleted(Task<FileSystemListItem> task)
        {
            if (task.IsCanceled)
                return;
            if (task.IsFaulted)
                _ = MessageBox.Show(Application.Current.MainWindow,
                    ((task.Exception.InnerException is ActivityException aExc) ? aExc.ToString().NullIfWhiteSpace() :
                        task.Exception.InnerExceptions.OfType<ActivityException>().Select(e => e.ToString())
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
            // TODO: Implement FileSystems.EditViewModel.OnNavigatingFrom
            //if (ApplyChanges())
            //{
            //    switch (MessageBox.Show(Application.Current.MainWindow, "There are unsaved changes. Do you wish to save them before continuing?", "Unsaved Changes",
            //        MessageBoxButton.YesNoCancel, MessageBoxImage.Warning))
            //    {
            //        case MessageBoxResult.Yes:
            //            throw new NotImplementedException();
            //            //IWindowsAsyncJobFactoryService jobFactory = Hosting.GetRequiredService<IWindowsAsyncJobFactoryService>();
            //            //IAsyncJob<FileSystemListItem> job = jobFactory.StartNew("Saving changes", "Opening database", Entity, InvocationState, SaveChangesAsync);
            //            //job.Task.ContinueWith(task => Dispatcher.Invoke(() => OnSaveTaskCompleted(task)));
            //            //e.Cancel = true;
            //            //break;
            //        case MessageBoxResult.No:
            //            break;
            //        default:
            //            e.Cancel = true;
            //            break;
            //    }
            //}
        }
    }
}
