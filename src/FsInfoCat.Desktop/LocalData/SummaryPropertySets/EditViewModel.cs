using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
namespace FsInfoCat.Desktop.LocalData.SummaryPropertySets
{
    public class EditViewModel : SummaryPropertySetDetailsViewModel<SummaryPropertySet, FileWithBinaryPropertiesAndAncestorNames, FileWithBinaryPropertiesAndAncestorNamesViewModel>,
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
                IAsyncJob<SummaryPropertiesListItem> job = jobFactory.StartNew("Saving changes", "Opening database", Entity, InvocationState, SaveChangesAsync);
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

        private static readonly DependencyPropertyKey ListItemPropertyKey = DependencyPropertyBuilder<EditViewModel, SummaryPropertiesListItem>
            .Register(nameof(ListItem))
            .DefaultValue(null)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ListItem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ListItemProperty = ListItemPropertyKey.DependencyProperty;

        public SummaryPropertiesListItem ListItem { get => (SummaryPropertiesListItem)GetValue(ListItemProperty); private set => SetValue(ListItemPropertyKey, value); }

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

        public EditViewModel([DisallowNull] SummaryPropertySet entity, SummaryPropertiesListItem listItem) : base(entity)
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
            Entity.ApplicationName = ApplicationName;
            Entity.Author = new(Author);
            Entity.Comment = Comment;
            Entity.Company = Company;
            Entity.ContentType = ContentType;
            Entity.Copyright = Copyright;
            Entity.ItemAuthors = new(ItemAuthors);
            Entity.ItemType = ItemType;
            Entity.ItemTypeText = ItemTypeText;
            Entity.Keywords = new(Keywords);
            Entity.Kind = new(Kind);
            Entity.LastSynchronizedOn = LastSynchronizedOn;
            Entity.MIMEType = MIMEType;
            Entity.ParentalRating = ParentalRating;
            Entity.ParentalRatingReason = ParentalRatingReason;
            Entity.ParentalRatingsOrganization = ParentalRatingsOrganization;
            Entity.ProductName = ProductName;
            Entity.Rating = Rating;
            Entity.Sensitivity = Sensitivity;
            Entity.SensitivityText = SensitivityText;
            Entity.SimpleRating = SimpleRating;
            Entity.Subject = Subject;
            Entity.Title = Title;
            Entity.Trademarks = Trademarks;
            Entity.UpstreamId = UpstreamId;
            return Entity.IsChanged();
        }

        private void ReinitializeFromEntity()
        {
            ApplicationName = Entity.ApplicationName;
            Comment = Entity.Comment;
            Company = Entity.Company;
            ContentType = Entity.ContentType;
            Copyright = Entity.Copyright;
            ItemType = Entity.ItemType;
            ItemTypeText = Entity.ItemTypeText;
            LastSynchronizedOn = Entity.LastSynchronizedOn;
            MIMEType = Entity.MIMEType;
            ParentalRating = Entity.ParentalRating;
            ParentalRatingReason = Entity.ParentalRatingReason;
            ParentalRatingsOrganization = Entity.ParentalRatingsOrganization;
            ProductName = Entity.ProductName;
            Rating = Entity.Rating;
            Sensitivity = Entity.Sensitivity;
            SensitivityText = Entity.SensitivityText;
            SimpleRating = Entity.SimpleRating;
            Subject = Entity.Subject;
            Title = Entity.Title;
            Trademarks = Entity.Trademarks;
            UpstreamId = Entity.UpstreamId;
            Author.Clear();
            ReadOnlyCollection<string> source = Entity.Author;
            if (source is not null)
                foreach (string s in source)
                    Author.Add(s ?? "");
            source = Entity.ItemAuthors;
            ItemAuthors.Clear();
            if (source is not null)
                foreach (string s in source)
                    ItemAuthors.Add(s ?? "");
            Keywords.Clear();
            source = Entity.Keywords;
            if (source is not null)
                foreach (string s in source)
                    Keywords.Add(s ?? "");
            Kind.Clear();
            source = Entity.Kind;
            if (source is not null)
                foreach (string s in source)
                    Kind.Add(s ?? "");
        }

        private static async Task<SummaryPropertiesListItem> SaveChangesAsync(SummaryPropertySet entity, object invocationState, IWindowsStatusListener statusListener)
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
                    entry = dbContext.SummaryPropertySets.Add(entity);
                    isNew = true;
                    break;
                default:
                    isNew = false;
                    break;
            }
            if (entry.State == EntityState.Detached)
                entry = dbContext.SummaryPropertySets.Add(entity);
            DispatcherOperation dispatcherOperation = statusListener.BeginSetMessage((entry.State == EntityState.Added) ?
                "Inserting new summary properties record into database" : "Saving summary properties record changes to database");
            await dbContext.SaveChangesAsync(statusListener.CancellationToken);
            await dispatcherOperation;
            if (isNew)
                return await dbContext.SummaryPropertiesListing.FindAsync(entity.Id, statusListener.CancellationToken);
            return invocationState is SummaryPropertiesListItem item ? item : null;
        }

        private void OnSaveTaskCompleted(Task<SummaryPropertiesListItem> task)
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
                        IAsyncJob<SummaryPropertiesListItem> job = jobFactory.StartNew("Saving changes", "Opening database", Entity, InvocationState, SaveChangesAsync);
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
