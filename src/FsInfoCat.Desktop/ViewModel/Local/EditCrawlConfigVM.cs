using FsInfoCat.Desktop.ViewModel.AsyncOps;
using FsInfoCat.Local;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public class EditCrawlConfigVM : DependencyObject, INotifyDataErrorInfo
    {
        private (Subdirectory Root, string Path)? _validatedPath;

        /// <summary>
        /// Occurs when the window should be closed by setting <see cref="Window.DialogResult"/> to <see langword="true"/>.
        /// </summary>
        public event EventHandler CloseSuccess;

        /// <summary>
        /// Occurs when the window should be closed by setting <see cref="Window.DialogResult"/> to <see langword="false"/>.
        /// </summary>
        public event EventHandler CloseCancel;

        /// <summary>
        /// Occurs when modal popup button is clicked, usually to cancel a background operation or acknowledge the results of a background operation.
        /// </summary>
        public event EventHandler ModalPopupConfirmClick;

        /// <summary>
        /// Occurs when the validation errors have changed for a property or for the entire entity.
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        #region Initialization Members

        public EditCrawlConfigVM()
        {
            SetValue(SelectRootCommandPropertyKey, new Commands.RelayCommand(OnSelectRootExecute));
            SetValue(SaveCommandPropertyKey, new Commands.RelayCommand(OnSaveExecute));
            SetValue(CancelCommandPropertyKey, new Commands.RelayCommand(OnCancelExecute));
            ValidationMessageTracker validation = new();
            SetValue(ValidationPropertyKey, validation);
            ChangeStateTracker changeTracker = new();
            SetValue(ChangeTrackerPropertyKey, changeTracker);
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
#endif
            validation.ErrorsChanged += Validation_ErrorsChanged;
            validation.AnyInvalidPropertyChanged += OnValidationStateChanged;
            changeTracker.AnyInvalidPropertyChanged += OnValidationStateChanged;
            // DEFERRED: Figure out why this crashes designer
            SetValue(LookupCrawlConfigOpMgrPropertyKey, new LookupCrawlConfigAsyncOpManager());
            SetValue(GetSubdirectoryFullNameOpMgrPropertyKey, new GetSubdirectoryFullNameAsyncOpManager());
            SetValue(SaveChangesOpMgrPropertyKey, new SaveCrawlConfigAsyncOpManager());
            SetValue(OpAggregatePropertyKey, new AsyncOpAggregate());
        }

        /// <summary>
        /// This get invoked by a <see cref="Microsoft.Xaml.Behaviors.Core.CallMethodAction">CallMethodAction</see> from the <see cref="View.Local.EditCrawlConfigWindow">EditCrawlConfigWindow</see>
        /// using an <see cref="Microsoft.Xaml.Behaviors.EventTrigger">EventTrigger</see> bound to the <see cref="Window.Closing"/> event.
        /// </summary>
        public void OnWindowClosing()
        {
            try { LookupCrawlConfigOpMgr.CancelAll(); }
            finally
            {
                try { LookupCrawlConfigOpMgr.CancelAll(); }
                finally { SaveChangesOpMgr.CancelAll(); }
            }
        }

        /// <summary>
        /// Instantiates a new <see cref="View.EditCrawlConfigWindow"/> to edit the properties of an existing <see cref="CrawlConfiguration"/>.
        /// </summary>
        /// <param name="model">The <see cref="CrawlConfiguration"/> to be modified.</param>
        /// <returns><see langword="true"/> if modifications were successfully saved to the databaase; otherwise <see langword="false"/> to indicate the user cancelled or there was
        /// an error that prohibited successful initialization.</returns>
        internal static bool Edit([DisallowNull] CrawlConfiguration model)
        {
            View.Local.EditCrawlConfigWindow window = new();
            EditCrawlConfigVM vm = (EditCrawlConfigVM)window.DataContext;
            if (vm is null)
            {
                vm = new();
                window.DataContext = vm;
            }
            vm.Initialize(model);
            vm.CloseCancel += new EventHandler((sender, e) => window.DialogResult = false);
            vm.CloseSuccess += new EventHandler((sender, e) => window.DialogResult = true);
            return window.ShowDialog() ?? false;
        }

        /// <summary>
        /// Instantiates a new <see cref="View.EditCrawlConfigWindow"/> to edit the properties of an existing <see cref="CrawlConfiguration"/> for a specified path. If no configuration
        /// exists for the specified path, a new entity will be created when the user saves changes.
        /// </summary>
        /// <param name="crawlRoot">The root path of the crawl configuration.</param>
        /// <param name="model">Returns the <see cref="CrawlConfiguration"/> that was saved to the database.</param>
        /// <param name="isNew">Returns <see langword="true"/> if a new entity was saved to the database; otherwise, <see langword="false"/>.</param>
        /// <returns><see langword="true"/> if a new record or modifications to an existing one were successfully saved to the databaase;
        /// otherwise <see langword="false"/> to indicate the user cancelled or there was an error that prohibited successful initialization.</returns>
        internal static bool Edit(string crawlRoot, out CrawlConfiguration model, out bool isNew)
        {
            View.Local.EditCrawlConfigWindow window = new();
            EditCrawlConfigVM vm = (EditCrawlConfigVM)window.DataContext;
            if (vm is null)
            {
                vm = new();
                window.DataContext = vm;
            }

            EventHandler closeCancelHandler = new((sender, e) =>
            {
                window.DialogResult = false;
            });
            vm.CloseCancel += closeCancelHandler;
            vm.ModalPopupConfirmClick += closeCancelHandler;
            vm.CloseSuccess += new EventHandler((sender, e) => window.DialogResult = true);
            window.Loaded += new RoutedEventHandler((sender, e) =>
            {
                AsyncFuncOpViewModel<string, (CrawlConfiguration Configuration, Subdirectory Root, string ValidatedPath)> lookupCrawlConfig = vm.LookupCrawlConfigAsync(crawlRoot);
                //lookupCrawlConfig.AsyncOpStatusPropertyChanged += vm.BgOp_AsyncOpStatusPropertyChanged;
                //lookupCrawlConfig.StatusMessagePropertyChanged += vm.BgOp_StatusMessagePropertyChanged;
                //lookupCrawlConfig.MessageLevelPropertyChanged += vm.BgOp_MessageLevelPropertyChanged;
                //lookupCrawlConfig.DurationPropertyChanged += vm.BgOp_DurationPropertyChanged;
                //vm.BgOpStatusMessage = lookupCrawlConfig.StatusMessage;
                //vm.BgOpMessageLevel = lookupCrawlConfig.MessageLevel;
                //vm.BgOpStatus = lookupCrawlConfig.AsyncOpStatus;
                lookupCrawlConfig.GetTask().ContinueWith(task =>
                {
                    //lookupCrawlConfig.AsyncOpStatusPropertyChanged -= vm.BgOp_AsyncOpStatusPropertyChanged;
                    //lookupCrawlConfig.StatusMessagePropertyChanged -= vm.BgOp_StatusMessagePropertyChanged;
                    //lookupCrawlConfig.MessageLevelPropertyChanged -= vm.BgOp_MessageLevelPropertyChanged;
                    //lookupCrawlConfig.DurationPropertyChanged -= vm.BgOp_DurationPropertyChanged;
                    vm.Dispatcher.Invoke(() =>
                    {
                        //vm.BgOpStatusMessage = lookupCrawlConfig.StatusMessage;
                        //vm.BgOpMessageLevel = lookupCrawlConfig.MessageLevel;
                        //vm.BgOpStatus = lookupCrawlConfig.AsyncOpStatus;
                        if (task.IsCanceled)
                        {
                            // TODO: Log cancellation.
                        }
                        else if (task.IsFaulted)
                        {
                            // TODO: Log failure.
                        }
                        else
                        {
                            vm.ModalPopupConfirmClick -= closeCancelHandler;
                            vm.Initialize(task.Result.Configuration, task.Result.Root, task.Result.ValidatedPath);
                            vm.LookupCrawlConfigOpMgr.RemoveOperation(lookupCrawlConfig);
                        }
                    });
                });
            });
            if (window.ShowDialog() ?? false)
            {
                isNew = vm.IsNew;
                model = vm.Model;
                return true;
            }
            model = null;
            isNew = false;
            return false;
        }

        private void Initialize(CrawlConfiguration crawlConfiguration, Subdirectory subdirectory, string fullPathName)
        {
            _validatedPath = (subdirectory, fullPathName);
            if (crawlConfiguration is not null)
                Initialize(crawlConfiguration);
            else
            {
                IsNew = true;
                Root = subdirectory;
                CreatedOn = ModifiedOn = DateTime.Now;
                MaxTotalItems = int.MaxValue;
                TimeSpan timeSpan = TimeSpan.FromDays(1.0);
                TtlDays = timeSpan.Days;
                TtlHours = timeSpan.Hours;
                TtlMinutes = timeSpan.Minutes;
                timeSpan = TimeSpan.FromDays(1.0);
                RescheduleDays = timeSpan.Days;
                RescheduleHours = timeSpan.Hours;
                RescheduleMinutes = timeSpan.Minutes;
                DateTime dateTime = DateTime.Now.AddHours(8.0);
                NextScheduledStartDate = dateTime.Date;
                NextScheduledStartHour = dateTime.Hour;
                NextScheduledStartMinute = dateTime.Minute;
            }
        }

        internal void Initialize([DisallowNull] CrawlConfiguration crawlConfiguration)
        {
            IsNew = false;
            Model = crawlConfiguration;
            Root = crawlConfiguration.Root;
            CreatedOn = crawlConfiguration.CreatedOn;
            ModifiedOn = crawlConfiguration.ModifiedOn;
            DisplayName = crawlConfiguration.DisplayName;
            MaxRecursionDepth = crawlConfiguration.MaxRecursionDepth;
            ulong? mti = crawlConfiguration.MaxTotalItems;
            LimitTotalItems = mti.HasValue;
            MaxTotalItems = mti ?? int.MaxValue;
            long? seconds = crawlConfiguration.TTL;
            if (seconds.HasValue)
            {
                LimitTTL = true;
                TimeSpan timeSpan = TimeSpan.FromSeconds(seconds.Value);
                TtlDays = timeSpan.Days;
                TtlHours = timeSpan.Hours;
                TtlMinutes = timeSpan.Minutes;
            }
            else
                LimitTTL = false;
            StatusValue = crawlConfiguration.StatusValue;
            LastCrawlEnd = crawlConfiguration.LastCrawlEnd;
            LastCrawlStart = crawlConfiguration.LastCrawlStart;
            Notes = crawlConfiguration.Notes;
            RescheduleAfterFail = crawlConfiguration.RescheduleAfterFail;
            RescheduleFromJobEnd = crawlConfiguration.RescheduleFromJobEnd;
            seconds = crawlConfiguration.RescheduleInterval;
            DateTime? nextScheduledStart = crawlConfiguration.NextScheduledStart;
            if (seconds.HasValue)
            {
                AutoReschedule = true;
                TimeSpan timeSpan = TimeSpan.FromSeconds(seconds.Value);
                RescheduleDays = timeSpan.Days;
                RescheduleHours = timeSpan.Hours;
                RescheduleMinutes = timeSpan.Minutes;
            }
            else
                OneTimeSchedule = nextScheduledStart.HasValue;
            DateTime dateTime = nextScheduledStart ?? DateTime.Now.AddHours(8.0);
            NextScheduledStartDate = dateTime.Date;
            NextScheduledStartHour = dateTime.Hour;
            NextScheduledStartMinute = dateTime.Minute;
            LastSynchronizedOn = crawlConfiguration.LastSynchronizedOn;
            WindowTitle = "Edit Crawl Configuration";
        }

        #endregion

        #region Command Members

        #region SelectRootCommand Property Members

        private static readonly DependencyPropertyKey SelectRootCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SelectRootCommand),
            typeof(Commands.RelayCommand), typeof(EditCrawlConfigVM), new PropertyMetadata(null));

        public static readonly DependencyProperty SelectRootCommandProperty = SelectRootCommandPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets a bindable command for selecting the root path.
        /// </summary>
        /// <value>The bindable <see cref="Commands.RelayCommand">RelayCommand</see> for selecting a new root path for the crawl configuration..</value>
        public Commands.RelayCommand SelectRootCommand => (Commands.RelayCommand)GetValue(SelectRootCommandProperty);

        private void OnSelectRootExecute(object parameter)
        {
            string newPath;
            using (System.Windows.Forms.FolderBrowserDialog dialog = new()
            {
                Description = FsInfoCat.Properties.Resources.Description_SelectCrawlRootFolder,
                SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
            })
            {
                if (dialog.ShowDialog(new WindowOwner()) != System.Windows.Forms.DialogResult.OK)
                    return;
                newPath = dialog.SelectedPath;
            }

            ReadOnlyCollection<string> errors = Validation.GetErrors(nameof(Path));
            Validation.ClearErrorMessages(nameof(Path));
            if (_validatedPath.HasValue && ReferenceEquals(_validatedPath.Value.Path, newPath))
                Path = _validatedPath.Value.Path;
            else
            {
                AsyncFuncOpViewModel<string, (CrawlConfiguration Configuration, Subdirectory Root, string ValidatedPath)> lookupCrawlConfig = LookupCrawlConfigAsync(newPath);
                //lookupCrawlConfig.AsyncOpStatusPropertyChanged += BgOp_AsyncOpStatusPropertyChanged;
                //lookupCrawlConfig.StatusMessagePropertyChanged += BgOp_StatusMessagePropertyChanged;
                //lookupCrawlConfig.MessageLevelPropertyChanged += BgOp_MessageLevelPropertyChanged;
                //lookupCrawlConfig.DurationPropertyChanged += BgOp_DurationPropertyChanged;
                //BgOpStatusMessage = lookupCrawlConfig.StatusMessage;
                //BgOpMessageLevel = lookupCrawlConfig.MessageLevel;
                //BgOpStatus = lookupCrawlConfig.AsyncOpStatus;
                lookupCrawlConfig.GetTask().ContinueWith(task =>
                {
                    //lookupCrawlConfig.AsyncOpStatusPropertyChanged -= BgOp_AsyncOpStatusPropertyChanged;
                    //lookupCrawlConfig.StatusMessagePropertyChanged -= BgOp_StatusMessagePropertyChanged;
                    //lookupCrawlConfig.MessageLevelPropertyChanged -= BgOp_MessageLevelPropertyChanged;
                    //lookupCrawlConfig.DurationPropertyChanged -= BgOp_DurationPropertyChanged;
                    Dispatcher.Invoke(() =>
                    {
                        if (task.IsCanceled)
                        {
                            // TODO: Log cancellation.
                        }
                        else if (task.IsFaulted)
                        {
                            // TODO: Log error.
                        }
                        else if (task.Result.Configuration is null)
                        {
                            _validatedPath = (task.Result.Root, task.Result.ValidatedPath);
                            Path = task.Result.ValidatedPath;
                            Root = task.Result.Root;
                        }
                        else if (Path != task.Result.ValidatedPath)
                        {
                            // TODO: Log error.
                            //BgOpStatusMessage = "That path already has its own craw configuration.";
                            //BgOpMessageLevel = StatusMessageLevel.Error;
                            //BgOpStatus = AsyncOpStatusCode.Faulted;
                        }
                    });
                });
            }
        }

        #endregion

        #region SaveCommand Property Members

        private static readonly DependencyPropertyKey SaveCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SaveCommand),
            typeof(Commands.RelayCommand), typeof(EditCrawlConfigVM), new PropertyMetadata(null));

        public static readonly DependencyProperty SaveCommandProperty = SaveCommandPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the bindable "Save" command.
        /// </summary>
        /// <value>The bindable command for saving changes and closing the edit window.</value>
        // TODO: Make sure save command is disabled when there are validation errors or no changes.
        public Commands.RelayCommand SaveCommand => (Commands.RelayCommand)GetValue(SaveCommandProperty);

        private void OnSaveExecute(object parameter)
        {
            CrawlConfiguration model = Model;
            if (model is null)
            {
                model = new()
                {
                    Id = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    DisplayName = DisplayName,
                    MaxRecursionDepth = MaxRecursionDepth,
                    Notes = Notes,
                    Root = Root,
                    StatusValue = IsEnabled ? CrawlStatus.NotRunning : CrawlStatus.Disabled
                };
                model.ModifiedOn = model.CreatedOn;
            }
            else
            {
                model.ModifiedOn = DateTime.Now;
                model.DisplayName = DisplayName;
                model.MaxRecursionDepth = MaxRecursionDepth;
                model.Notes = Notes;
                model.Root = Root;
                if (!IsEnabled)
                    model.StatusValue = CrawlStatus.Disabled;
            }
            if (LimitTotalItems)
                model.MaxTotalItems = MaxTotalItems;
            else
                model.MaxTotalItems = null;
            if (LimitTTL)
                model.TTL = ((((TtlDays.Value * 24L) + TtlHours.Value) * 60L) + TtlMinutes.Value) * 60L;
            else
                model.TTL = null;
            if (AutoReschedule)
            {
                model.RescheduleAfterFail = RescheduleAfterFail;
                model.RescheduleFromJobEnd = RescheduleFromJobEnd;
                model.RescheduleInterval = ((((RescheduleDays.Value * 24L) + RescheduleHours.Value) * 60L) + RescheduleMinutes.Value) * 60L;
            }
            else
            {
                model.RescheduleAfterFail = false;
                model.RescheduleFromJobEnd = false;
                model.RescheduleInterval = null;
            }
            if (NoReschedule)
                model.NextScheduledStart = null;
            else
                model.NextScheduledStart = new DateTime(NextScheduledStartDate.Value.Year, NextScheduledStartDate.Value.Month, NextScheduledStartDate.Value.Day,
                    (NextScheduledStartHour.Value == 12) ? (NextScheduledStartIsPm ? 12 : 0) : (NextScheduledStartIsPm ? NextScheduledStartHour.Value + 12 :
                        NextScheduledStartHour.Value), NextScheduledStartMinute.Value, 0, DateTimeKind.Local);
            AsyncFuncOpViewModel<(CrawlConfiguration, EditCrawlConfigVM), (CrawlConfiguration Configuration, Subdirectory Root)> asyncOp = SaveChangesAsync(model);
            //asyncOp.AsyncOpStatusPropertyChanged += BgOp_AsyncOpStatusPropertyChanged;
            //asyncOp.StatusMessagePropertyChanged += BgOp_StatusMessagePropertyChanged;
            //asyncOp.MessageLevelPropertyChanged += BgOp_MessageLevelPropertyChanged;
            //asyncOp.DurationPropertyChanged += BgOp_DurationPropertyChanged;
            //BgOpStatusMessage = asyncOp.StatusMessage;
            //BgOpMessageLevel = asyncOp.MessageLevel;
            //BgOpStatus = asyncOp.AsyncOpStatus;
            asyncOp.GetTask().ContinueWith(task =>
            {
                //asyncOp.AsyncOpStatusPropertyChanged -= BgOp_AsyncOpStatusPropertyChanged;
                //asyncOp.StatusMessagePropertyChanged -= BgOp_StatusMessagePropertyChanged;
                //asyncOp.MessageLevelPropertyChanged -= BgOp_MessageLevelPropertyChanged;
                //asyncOp.DurationPropertyChanged -= BgOp_DurationPropertyChanged;
                Dispatcher.Invoke(() =>
                {
                    //BgOpStatusMessage = asyncOp.StatusMessage;
                    //BgOpMessageLevel = asyncOp.MessageLevel;
                    //BgOpStatus = asyncOp.AsyncOpStatus;
                    //SaveChangesOpMgr.RemoveOperation(asyncOp);
                    if (task.IsCanceled)
                    {
                        // TODO: Log cancellation.
                    }
                    else if (task.IsFaulted)
                    {
                        // TODO: Log error.
                    }
                    else
                        CloseSuccess?.Invoke(this, EventArgs.Empty);
                });
            });
        }

        #endregion

        #region CancelCommand Property Members

        private static readonly DependencyPropertyKey CancelCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CancelCommand),
            typeof(Commands.RelayCommand), typeof(EditCrawlConfigVM), new PropertyMetadata(null));

        public static readonly DependencyProperty CancelCommandProperty = CancelCommandPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the bindable "Cancel" command.
        /// </summary>
        /// <value>The bindable command for discarding changes and closing the edit window.</value>
        public Commands.RelayCommand CancelCommand => (Commands.RelayCommand)GetValue(CancelCommandProperty);

        private void OnCancelExecute(object parameter) => CloseCancel?.Invoke(this, EventArgs.Empty);

        #endregion

        //#region PopupButtonClickCommand Property Members

        //private static readonly DependencyPropertyKey PopupButtonClickCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(PopupButtonClickCommand),
        //    typeof(Commands.RelayCommand), typeof(EditCrawlConfigVM), new PropertyMetadata(null));

        //public static readonly DependencyProperty PopupButtonClickCommandProperty = PopupButtonClickCommandPropertyKey.DependencyProperty;

        ///// <summary>
        ///// Gets the bindable modal popup cancel / confirmation command.
        ///// </summary>
        ///// <value>The bindable command for cancelling an active modal background operator or to confirm the results of a background operation.</value>
        //public Commands.RelayCommand PopupButtonClickCommand => (Commands.RelayCommand)GetValue(PopupButtonClickCommandProperty);

        //private void OnPopupButtonClickExecute(object parameter)
        //{
        //    switch (BgOpStatus)
        //    {
        //        case AsyncOpStatusCode.RanToCompletion:
        //        case AsyncOpStatusCode.CancellationPending:
        //            break;
        //        case AsyncOpStatusCode.Faulted:
        //        case AsyncOpStatusCode.Canceled:
        //            BgOpStatus = AsyncOpStatusCode.NotStarted;
        //            ModalPopupConfirmClick?.Invoke(this, EventArgs.Empty);
        //            break;
        //        default:
        //            LookupCrawlConfigOpMgr.CancelAll();
        //            GetSubdirectoryFullNameOpMgr.CancelAll();
        //            SaveChangesOpMgr.CancelAll();
        //            break;
        //    }
        //}

        //#endregion

        #endregion

        #region Background Operation Properties

        private static readonly DependencyPropertyKey OpAggregatePropertyKey = DependencyProperty.RegisterReadOnly(nameof(OpAggregate), typeof(AsyncOpAggregate), typeof(EditCrawlConfigVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty OpAggregateProperty = OpAggregatePropertyKey.DependencyProperty;

        public AsyncOpAggregate OpAggregate => (AsyncOpAggregate)GetValue(OpAggregateProperty);

        //#region BgOpStatus Property Members

        //private static readonly DependencyPropertyKey BgOpStatusPropertyKey = DependencyProperty.RegisterReadOnly(nameof(BgOpStatus), typeof(AsyncOpStatusCode),
        //    typeof(EditCrawlConfigVM), new PropertyMetadata(AsyncOpStatusCode.NotStarted));

        //public static readonly DependencyProperty BgOpStatusProperty = BgOpStatusPropertyKey.DependencyProperty;

        ///// <summary>
        ///// Gets the status of the current background operation.
        ///// </summary>
        ///// <value>The background operation status. If this is <see cref="AsyncOpStatusCode.NotStarted"/>, then that indicates that no modal popup should be visible.</value>
        //public AsyncOpStatusCode BgOpStatus
        //{
        //    get => (AsyncOpStatusCode)GetValue(BgOpStatusProperty);
        //    private set => SetValue(BgOpStatusPropertyKey, value);
        //}

        //#endregion

        //#region BgOpMessageLevel Property Members

        //private static readonly DependencyPropertyKey BgOpMessageLevelPropertyKey = DependencyProperty.RegisterReadOnly(nameof(BgOpMessageLevel), typeof(StatusMessageLevel),
        //    typeof(EditCrawlConfigVM), new PropertyMetadata(StatusMessageLevel.Information));

        //public static readonly DependencyProperty BgOpMessageLevelProperty = BgOpMessageLevelPropertyKey.DependencyProperty;

        //public StatusMessageLevel BgOpMessageLevel
        //{
        //    get => (StatusMessageLevel)GetValue(BgOpMessageLevelProperty);
        //    private set => SetValue(BgOpMessageLevelPropertyKey, value);
        //}

        //#endregion

        //#region BgOpStatusMessage Property Members

        //private static readonly DependencyPropertyKey BgOpStatusMessagePropertyKey = DependencyProperty.RegisterReadOnly(nameof(BgOpStatusMessage), typeof(string),
        //    typeof(EditCrawlConfigVM), new PropertyMetadata(""));

        //public static readonly DependencyProperty BgOpStatusMessageProperty = BgOpStatusMessagePropertyKey.DependencyProperty;

        //public string BgOpStatusMessage
        //{
        //    get => GetValue(BgOpStatusMessageProperty) as string;
        //    private set => SetValue(BgOpStatusMessagePropertyKey, value);
        //}

        //#endregion

        //#region BgOpDuration Property Members

        //private static readonly DependencyPropertyKey BgOpDurationPropertyKey = DependencyProperty.RegisterReadOnly(nameof(BgOpDuration), typeof(TimeSpan), typeof(EditCrawlConfigVM),
        //        new PropertyMetadata(TimeSpan.Zero));

        //public static readonly DependencyProperty BgOpDurationProperty = BgOpDurationPropertyKey.DependencyProperty;

        //public TimeSpan BgOpDuration
        //{
        //    get => (TimeSpan)GetValue(BgOpDurationProperty);
        //    private set => SetValue(BgOpDurationPropertyKey, value);
        //}

        //#endregion

        #region LookupCrawlConfigOpMgr Property

        private static readonly DependencyPropertyKey LookupCrawlConfigOpMgrPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LookupCrawlConfigOpMgr),
            typeof(LookupCrawlConfigAsyncOpManager), typeof(EditCrawlConfigVM), new PropertyMetadata(null));

        public static readonly DependencyProperty LookupCrawlConfigOpMgrProperty = LookupCrawlConfigOpMgrPropertyKey.DependencyProperty;

        public LookupCrawlConfigAsyncOpManager LookupCrawlConfigOpMgr => (LookupCrawlConfigAsyncOpManager)GetValue(LookupCrawlConfigOpMgrProperty);

        #endregion

        #region GetSubdirectoryFullNameOpMgr Property

        private static readonly DependencyPropertyKey GetSubdirectoryFullNameOpMgrPropertyKey = DependencyProperty.RegisterReadOnly(nameof(GetSubdirectoryFullNameOpMgr),
            typeof(GetSubdirectoryFullNameAsyncOpManager), typeof(EditCrawlConfigVM), new PropertyMetadata(null));

        public static readonly DependencyProperty GetSubdirectoryFullNameOpMgrProperty = GetSubdirectoryFullNameOpMgrPropertyKey.DependencyProperty;

        public GetSubdirectoryFullNameAsyncOpManager GetSubdirectoryFullNameOpMgr => (GetSubdirectoryFullNameAsyncOpManager)GetValue(GetSubdirectoryFullNameOpMgrProperty);

        #endregion

        private static readonly DependencyPropertyKey SaveChangesOpMgrPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SaveChangesOpMgr), typeof(SaveCrawlConfigAsyncOpManager), typeof(EditCrawlConfigVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty SaveChangesOpMgrProperty = SaveChangesOpMgrPropertyKey.DependencyProperty;

        public SaveCrawlConfigAsyncOpManager SaveChangesOpMgr => (SaveCrawlConfigAsyncOpManager)GetValue(SaveChangesOpMgrProperty);

        //private void BgOp_DurationPropertyChanged(object sender, DependencyPropertyChangedEventArgs e) => BgOpDuration = (e.NewValue as TimeSpan?) ?? TimeSpan.Zero;

        //private void BgOp_MessageLevelPropertyChanged(object sender, DependencyPropertyChangedEventArgs e) => BgOpMessageLevel = (StatusMessageLevel)e.NewValue;

        //private void BgOp_StatusMessagePropertyChanged(object sender, DependencyPropertyChangedEventArgs e) => BgOpStatusMessage = (e.NewValue as string) ?? "";

        //private void BgOp_AsyncOpStatusPropertyChanged(object sender, DependencyPropertyChangedEventArgs e) => BgOpStatus = (AsyncOpStatusCode)e.NewValue;

        #endregion

        #region Change Tracking / Validation Members

        internal bool HasErrors => Validation.HasErrors;

        bool INotifyDataErrorInfo.HasErrors => Validation.HasErrors;

        #region IsNew Property Members

        private static readonly DependencyPropertyKey IsNewPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsNew), typeof(bool), typeof(EditCrawlConfigVM),
                new PropertyMetadata(true));

        public static readonly DependencyProperty IsNewProperty = IsNewPropertyKey.DependencyProperty;

        public bool IsNew
        {
            get => (bool)GetValue(IsNewProperty);
            private set => SetValue(IsNewPropertyKey, value);
        }

        #endregion

        #region Model Property Members

        private static readonly DependencyPropertyKey ModelPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Model), typeof(CrawlConfiguration), typeof(EditCrawlConfigVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ModelProperty = ModelPropertyKey.DependencyProperty;

        public CrawlConfiguration Model
        {
            get => (CrawlConfiguration)GetValue(ModelProperty);
            private set => SetValue(ModelPropertyKey, value);
        }

        #endregion

        #region Validation ChangeTracker Members

        private static readonly DependencyPropertyKey ChangeTrackerPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ChangeTracker), typeof(ChangeStateTracker), typeof(EditCrawlConfigVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ChangeTrackerProperty = ChangeTrackerPropertyKey.DependencyProperty;

        public ChangeStateTracker ChangeTracker => (ChangeStateTracker)GetValue(ChangeTrackerProperty);

        #endregion

        #region Validation Property Members

        private static readonly DependencyPropertyKey ValidationPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Validation), typeof(ValidationMessageTracker), typeof(EditCrawlConfigVM),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ValidationProperty = ValidationPropertyKey.DependencyProperty;

        public ValidationMessageTracker Validation => (ValidationMessageTracker)GetValue(ValidationProperty);

        private void Validation_ErrorsChanged(object sender, DataErrorsChangedEventArgs e) => ErrorsChanged?.Invoke(this, e);

        #endregion

        private void OnValidationStateChanged(object sender, EventArgs e)
        {

        }

        public IEnumerable GetErrors(string propertyName) => Validation.GetErrors(propertyName);

        #endregion

        #region TTL Members

        private void OnTtlChanged(int days, int hours, int minutes)
        {
            if (days == 0)
            {
                if (hours == 0 && minutes < DbConstants.DbColMinValue_TTL_TotalMinutes)
                    Validation.SetErrorMessage(nameof(TtlMinutes), $"Crawl duration limit cannot be less than {DbConstants.DbColMinValue_TTL_TotalMinutes} minutes.");
                else if (!Validation.IsValid(nameof(TtlMinutes)))
                    CheckTtlMinutes(minutes);
            }
            else if (days == TimeSpan.MaxValue.Days)
            {
                if (hours > TimeSpan.MaxValue.Hours)
                    Validation.SetErrorMessage(nameof(TtlHours), $"Hours cannot be greater than {TimeSpan.MaxValue.Hours} when days is set to {TimeSpan.MaxValue.Days}.");
                else
                {
                    if (!Validation.IsValid(nameof(TtlHours)))
                        CheckTtlHours(hours);
                    if (hours == TimeSpan.MaxValue.Hours)
                    {
                        if (minutes > TimeSpan.MaxValue.Minutes)
                            Validation.SetErrorMessage(nameof(TtlMinutes), $"Minutes cannot be greater than {TimeSpan.MaxValue.Minutes} when days is set to {TimeSpan.MaxValue.Days} and hours is set to {TimeSpan.MaxValue.Hours}.");
                        else if (!Validation.IsValid(nameof(TtlMinutes)))
                            CheckTtlMinutes(minutes);
                    }
                }
            }
        }

        #region LimitTTL Property Members

        public static readonly DependencyProperty LimitTTLProperty = DependencyProperty.Register(nameof(LimitTTL), typeof(bool), typeof(EditCrawlConfigVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as EditCrawlConfigVM).OnLimitTTLPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool LimitTTL
        {
            get => (bool)GetValue(LimitTTLProperty);
            set => SetValue(LimitTTLProperty, value);
        }

        protected virtual void OnLimitTTLPropertyChanged(bool oldValue, bool newValue)
        {
            if (newValue)
            {
                int days = TtlDays ?? 0;
                int hours = TtlHours ?? 0;
                int minutes = TtlMinutes ?? 0;
                bool isValid = CheckTtlDays(days);
                if (!CheckTtlHours(hours))
                    isValid = false;
                if (CheckTtlMinutes(minutes) && isValid)
                    OnTtlChanged(days, hours, minutes);
            }
            else
            {
                bool isChanged = Model?.GetTTLAsTimeSpan() is not null;
                ChangeTracker.SetChangeState(nameof(TtlDays), isChanged);
                ChangeTracker.SetChangeState(nameof(TtlHours), isChanged);
                ChangeTracker.SetChangeState(nameof(TtlMinutes), isChanged);
                Validation.ClearErrorMessages(nameof(TtlDays));
                Validation.ClearErrorMessages(nameof(TtlHours));
                Validation.ClearErrorMessages(nameof(TtlMinutes));
            }
        }

        #endregion

        #region TtlDays Property Members

        public static readonly DependencyProperty TtlDaysProperty = DependencyProperty.Register(nameof(TtlDays), typeof(int?), typeof(EditCrawlConfigVM),
                new PropertyMetadata(0, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnTtlDaysPropertyChanged((int?)e.OldValue, (int?)e.NewValue)));

        public int? TtlDays
        {
            get => (int?)GetValue(TtlDaysProperty);
            set => SetValue(TtlDaysProperty, value);
        }

        private bool CheckTtlDays(int days)
        {
            ChangeTracker.SetChangeState(nameof(TtlDays), days != Model?.GetTTLAsTimeSpan()?.Days);

            if (days < 0)
                Validation.SetErrorMessage(nameof(TtlDays), "Days cannot be negative");
            else if (days > TimeSpan.MaxValue.Days)
                Validation.SetErrorMessage(nameof(TtlDays), $"Days cannot be greater than {TimeSpan.MaxValue.Days}");
            else
            {
                Validation.ClearErrorMessages(nameof(TtlDays));
                return true;
            }
            return false;
        }

        protected virtual void OnTtlDaysPropertyChanged(int? oldValue, int? newValue)
        {
            if (!LimitTTL)
                return;
            int days = newValue ?? 0;
            if (CheckTtlDays(days) && Validation.IsValid(nameof(TtlHours)) && Validation.IsValid(nameof(TtlMinutes)))
                OnTtlChanged(days, TtlHours ?? 0, TtlMinutes ?? 0);
        }

        #endregion

        #region TtlHours Property Members

        public static readonly DependencyProperty TtlHoursProperty = DependencyProperty.Register(nameof(TtlHours), typeof(int?), typeof(EditCrawlConfigVM),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnTtlHoursPropertyChanged((int?)e.OldValue, (int?)e.NewValue)));

        public int? TtlHours
        {
            get => (int?)GetValue(TtlHoursProperty);
            set => SetValue(TtlHoursProperty, value);
        }

        private bool CheckTtlHours(int hours)
        {
            ChangeTracker.SetChangeState(nameof(TtlHours), hours != Model?.GetTTLAsTimeSpan()?.Hours);
            if (hours < 0)
                Validation.SetErrorMessage(nameof(TtlHours), "Hours cannot be negative");
            else if (hours > 23)
                Validation.SetErrorMessage(nameof(TtlHours), "Hours cannot be greater than 23");
            else
            {
                Validation.ClearErrorMessages(nameof(TtlHours));
                return true;
            }
            return false;
        }

        protected virtual void OnTtlHoursPropertyChanged(int? oldValue, int? newValue)
        {
            if (!LimitTTL)
                return;
            int hours = newValue ?? 0;
            if (CheckTtlHours(hours) && Validation.IsValid(nameof(TtlDays)) && Validation.IsValid(nameof(TtlMinutes)))
                OnTtlChanged(TtlDays ?? 0, hours, TtlMinutes ?? 0);
        }

        #endregion

        #region TtlMinutes Property Members

        public static readonly DependencyProperty TtlMinutesProperty = DependencyProperty.Register(nameof(TtlMinutes), typeof(int?), typeof(EditCrawlConfigVM),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnTtlMinutesPropertyChanged((int?)e.OldValue, (int?)e.NewValue)));

        public int? TtlMinutes
        {
            get => (int?)GetValue(TtlMinutesProperty);
            set => SetValue(TtlMinutesProperty, value);
        }

        private bool CheckTtlMinutes(int minutes)
        {
            ChangeTracker.SetChangeState(nameof(TtlMinutes), minutes != Model?.GetTTLAsTimeSpan()?.Minutes);
            if (minutes < 0)
                Validation.SetErrorMessage(nameof(TtlMinutes), "Minutes cannot be negative");
            else if (minutes > 59)
                Validation.SetErrorMessage(nameof(TtlMinutes), "Minutes cannot be greater than 59");
            else
            {
                Validation.ClearErrorMessages(nameof(TtlMinutes));
                return true;
            }
            return false;
        }

        protected virtual void OnTtlMinutesPropertyChanged(int? oldValue, int? newValue)
        {
            if (!LimitTTL)
                return;
            int minutes = newValue ?? 0;
            if (CheckTtlMinutes(minutes) && Validation.IsValid(nameof(TtlDays)) && Validation.IsValid(nameof(TtlHours)))
                OnTtlChanged(TtlDays ?? 0, TtlHours ?? 0, minutes);
        }

        #endregion

        #endregion

        #region Scheduling Members

        #region AutoReschedule Property Members

        public static readonly DependencyProperty AutoRescheduleProperty = DependencyProperty.Register(nameof(AutoReschedule), typeof(bool), typeof(EditCrawlConfigVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnAutoReschedulePropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool AutoReschedule
        {
            get => (bool)GetValue(AutoRescheduleProperty);
            set => SetValue(AutoRescheduleProperty, value);
        }

        private void OnAutoReschedulePropertyChanged(bool oldValue, bool newValue)
        {
            if (newValue)
            {
                OneTimeSchedule = false;
                NoReschedule = false;
                ChangeTracker.SetChangeState(nameof(RescheduleAfterFail), Model?.RescheduleAfterFail != RescheduleAfterFail);
                ChangeTracker.SetChangeState(nameof(RescheduleFromJobEnd), Model?.RescheduleFromJobEnd != RescheduleFromJobEnd);
                int days = TtlDays ?? 0;
                int hours = TtlHours ?? 0;
                int minutes = TtlMinutes ?? 0;
                bool isValid = CheckTtlDays(days);
                if (!CheckRescheduleHours(hours))
                    isValid = false;
                if (CheckRescheduleMinutes(minutes) && isValid)
                    OnRescheduleIntervalChanged(days, hours, minutes);
                if (!NextScheduledStartDate.HasValue)
                    Validation.SetErrorMessage(nameof(NextScheduledStartDate), "No initial crawl start date selected.");
                else
                    Validation.ClearErrorMessages(nameof(NextScheduledStartDate));
                OnNextScheduledStartHourPropertyChanged(null, NextScheduledStartHour);
                OnNextScheduledStartMinutePropertyChanged(null, NextScheduledStartMinute);
            }
            else
            {
                bool isChanged = Model?.GetTTLAsTimeSpan() is not null;
                ChangeTracker.SetChangeState(nameof(TtlDays), isChanged);
                ChangeTracker.SetChangeState(nameof(TtlHours), isChanged);
                ChangeTracker.SetChangeState(nameof(TtlMinutes), isChanged);
                Validation.ClearErrorMessages(nameof(TtlDays));
                Validation.ClearErrorMessages(nameof(TtlHours));
                Validation.ClearErrorMessages(nameof(TtlMinutes));
                ChangeTracker.SetChangeState(nameof(RescheduleAfterFail), false);
                ChangeTracker.SetChangeState(nameof(RescheduleFromJobEnd), false);
            }
        }

        #endregion

        #region OneTimeSchedule Property Members

        public static readonly DependencyProperty OneTimeScheduleProperty = DependencyProperty.Register(nameof(OneTimeSchedule), typeof(bool), typeof(EditCrawlConfigVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as EditCrawlConfigVM).OnOneTimeSchedulePropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool OneTimeSchedule
        {
            get => (bool)GetValue(OneTimeScheduleProperty);
            set => SetValue(OneTimeScheduleProperty, value);
        }

        protected virtual void OnOneTimeSchedulePropertyChanged(bool oldValue, bool newValue)
        {
            if (newValue)
            {
                AutoReschedule = false;
                NoReschedule = false;
                if (!NextScheduledStartDate.HasValue)
                    Validation.SetErrorMessage(nameof(NextScheduledStartDate), "No crawl start date selected.");
                OnNextScheduledStartHourPropertyChanged(null, NextScheduledStartHour);
                OnNextScheduledStartMinutePropertyChanged(null, NextScheduledStartMinute);
            }
        }

        #endregion

        #region NoReschedule Property Members

        public static readonly DependencyProperty NoRescheduleProperty = DependencyProperty.Register(nameof(NoReschedule), typeof(bool), typeof(EditCrawlConfigVM),
                new PropertyMetadata(true, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnNoReschedulePropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool NoReschedule
        {
            get => (bool)GetValue(NoRescheduleProperty);
            set => SetValue(NoRescheduleProperty, value);
        }

        protected virtual void OnNoReschedulePropertyChanged(bool oldValue, bool newValue)
        {
            if (newValue)
            {
                AutoReschedule = false;
                OneTimeSchedule = false;
                Validation.ClearErrorMessages(nameof(NextScheduledStartDate));
                Validation.ClearErrorMessages(nameof(NextScheduledStartHour));
                Validation.ClearErrorMessages(nameof(NextScheduledStartMinute));
            }
        }

        #endregion

        #region RescheduleInterval Members

        private void OnRescheduleIntervalChanged(int days, int hours, int minutes)
        {
            if (days == 0)
            {
                if (hours == 0 && minutes < DbConstants.DbColMinValue_RescheduleInterval_TotalMinutes)
                    Validation.SetErrorMessage(nameof(RescheduleMinutes), $"Re-schedule interval cannot be less than {DbConstants.DbColMinValue_RescheduleInterval_TotalMinutes} minutes.");
                else if (!Validation.IsValid(nameof(RescheduleMinutes)))
                    CheckRescheduleMinutes(minutes);
            }
            else if (days == TimeSpan.MaxValue.Days)
            {
                if (hours > TimeSpan.MaxValue.Hours)
                    Validation.SetErrorMessage(nameof(RescheduleHours), $"Hours cannot be greater than {TimeSpan.MaxValue.Hours} when days is set to {TimeSpan.MaxValue.Days}.");
                else
                {
                    if (!Validation.IsValid(nameof(RescheduleHours)))
                        CheckRescheduleHours(hours);
                    if (hours == TimeSpan.MaxValue.Hours)
                    {
                        if (minutes > TimeSpan.MaxValue.Minutes)
                            Validation.SetErrorMessage(nameof(RescheduleMinutes), $"Minutes cannot be greater than {TimeSpan.MaxValue.Minutes} when days is set to {TimeSpan.MaxValue.Days} and hours is set to {TimeSpan.MaxValue.Hours}.");
                        else if (!Validation.IsValid(nameof(RescheduleMinutes)))
                            CheckRescheduleMinutes(minutes);
                    }
                }
            }
        }

        #region RescheduleDays Property Members

        public static readonly DependencyProperty RescheduleDaysProperty = DependencyProperty.Register(nameof(RescheduleDays), typeof(int?), typeof(EditCrawlConfigVM),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as EditCrawlConfigVM).OnRescheduleDaysPropertyChanged((int?)e.OldValue, (int?)e.NewValue)));

        public int? RescheduleDays
        {
            get => (int?)GetValue(RescheduleDaysProperty);
            set => SetValue(RescheduleDaysProperty, value);
        }

        private bool CheckRescheduleDays(int days)
        {
            ChangeTracker.SetChangeState(nameof(RescheduleDays), days != Model?.GetRescheduleIntervalAsTimeSpan()?.Days);

            if (days < 0)
                Validation.SetErrorMessage(nameof(RescheduleDays), "Days cannot be negative");
            else if (days > TimeSpan.MaxValue.Days)
                Validation.SetErrorMessage(nameof(RescheduleDays), $"Days cannot be greater than {TimeSpan.MaxValue.Days}");
            else
            {
                Validation.ClearErrorMessages(nameof(RescheduleDays));
                return true;
            }
            return false;
        }

        protected virtual void OnRescheduleDaysPropertyChanged(int? oldValue, int? newValue)
        {
            if (!AutoReschedule)
                return;
            int days = newValue ?? 0;
            if (CheckRescheduleDays(days) && Validation.IsValid(nameof(RescheduleHours)) && Validation.IsValid(nameof(RescheduleMinutes)))
                OnRescheduleIntervalChanged(days, RescheduleHours ?? 0, RescheduleMinutes ?? 0);
        }

        #endregion

        #region RescheduleHours Property Members

        public static readonly DependencyProperty RescheduleHoursProperty = DependencyProperty.Register(nameof(RescheduleHours), typeof(int?), typeof(EditCrawlConfigVM),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as EditCrawlConfigVM).OnRescheduleHoursPropertyChanged((int?)e.OldValue, (int?)e.NewValue)));

        public int? RescheduleHours
        {
            get => (int?)GetValue(RescheduleHoursProperty);
            set => SetValue(RescheduleHoursProperty, value);
        }

        private bool CheckRescheduleHours(int hours)
        {
            ChangeTracker.SetChangeState(nameof(RescheduleHours), hours != Model?.GetRescheduleIntervalAsTimeSpan()?.Hours);
            if (hours < 0)
                Validation.SetErrorMessage(nameof(RescheduleHours), "Hours cannot be negative");
            else if (hours > 23)
                Validation.SetErrorMessage(nameof(RescheduleHours), "Hours cannot be greater than 23");
            else
            {
                Validation.ClearErrorMessages(nameof(RescheduleHours));
                return true;
            }
            return false;
        }

        protected virtual void OnRescheduleHoursPropertyChanged(int? oldValue, int? newValue)
        {
            if (!AutoReschedule)
                return;
            int hours = newValue ?? 0;
            if (CheckRescheduleHours(hours) && Validation.IsValid(nameof(RescheduleDays)) && Validation.IsValid(nameof(RescheduleMinutes)))
                OnTtlChanged(RescheduleDays ?? 0, hours, RescheduleMinutes ?? 0);
        }

        #endregion

        #region RescheduleMinutes Property Members

        public static readonly DependencyProperty RescheduleMinutesProperty = DependencyProperty.Register(nameof(RescheduleMinutes), typeof(int?), typeof(EditCrawlConfigVM),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as EditCrawlConfigVM).OnRescheduleMinutesPropertyChanged((int?)e.OldValue, (int?)e.NewValue)));

        public int? RescheduleMinutes
        {
            get => (int?)GetValue(RescheduleMinutesProperty);
            set => SetValue(RescheduleMinutesProperty, value);
        }

        private bool CheckRescheduleMinutes(int minutes)
        {
            ChangeTracker.SetChangeState(nameof(RescheduleMinutes), minutes != Model?.GetRescheduleIntervalAsTimeSpan()?.Minutes);
            if (minutes < 0)
                Validation.SetErrorMessage(nameof(RescheduleMinutes), "Minutes cannot be negative");
            else if (minutes > 59)
                Validation.SetErrorMessage(nameof(RescheduleMinutes), "Minutes cannot be greater than 59");
            else
            {
                Validation.ClearErrorMessages(nameof(RescheduleMinutes));
                return true;
            }
            return false;
        }

        protected virtual void OnRescheduleMinutesPropertyChanged(int? oldValue, int? newValue)
        {
            if (!AutoReschedule)
                return;
            int minutes = newValue ?? 0;
            if (CheckRescheduleMinutes(minutes) && Validation.IsValid(nameof(RescheduleDays)) && Validation.IsValid(nameof(RescheduleHours)))
                OnTtlChanged(RescheduleDays ?? 0, RescheduleHours ?? 0, minutes);
        }

        #endregion

        #region RescheduleAfterFail Property Members

        public static readonly DependencyProperty RescheduleAfterFailProperty = DependencyProperty.Register(nameof(RescheduleAfterFail), typeof(bool), typeof(EditCrawlConfigVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnRescheduleAfterFailPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool RescheduleAfterFail
        {
            get => (bool)GetValue(RescheduleAfterFailProperty);
            set => SetValue(RescheduleAfterFailProperty, value);
        }

        private void OnRescheduleAfterFailPropertyChanged(bool oldValue, bool newValue)
        {
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this) || Model is null)
                return;
#else
            if (Model is null)
                return;
#endif
            ChangeTracker.SetChangeState(nameof(RescheduleAfterFail), Model?.RescheduleAfterFail != newValue);
        }

        #endregion

        #region RescheduleFromJobEnd Property Members

        public static readonly DependencyProperty RescheduleFromJobEndProperty = DependencyProperty.Register(nameof(RescheduleFromJobEnd), typeof(bool), typeof(EditCrawlConfigVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnRescheduleFromJobEndPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool RescheduleFromJobEnd
        {
            get => (bool)GetValue(RescheduleFromJobEndProperty);
            set => SetValue(RescheduleFromJobEndProperty, value);
        }

        private void OnRescheduleFromJobEndPropertyChanged(bool oldValue, bool newValue)
        {
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this) || Model is null)
                return;
#else
            if (Model is null)
                return;
#endif
            ChangeTracker.SetChangeState(nameof(RescheduleFromJobEnd), Model?.RescheduleFromJobEnd != newValue);
        }

        #endregion

        #endregion

        #region NextScheduledStart Members

        #region NextScheduledStartDate Property Members

        public static readonly DependencyProperty NextScheduledStartDateProperty = DependencyProperty.Register(nameof(NextScheduledStartDate), typeof(DateTime?), typeof(EditCrawlConfigVM),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnNextScheduledStartDatePropertyChanged((DateTime?)e.OldValue, (DateTime?)e.NewValue)));

        public DateTime? NextScheduledStartDate
        {
            get => (DateTime?)GetValue(NextScheduledStartDateProperty);
            set => SetValue(NextScheduledStartDateProperty, value);
        }

        protected virtual void OnNextScheduledStartDatePropertyChanged(DateTime? oldValue, DateTime? newValue)
        {
            if (!NoReschedule)
            {
                if (!NextScheduledStartDate.HasValue)
                    Validation.SetErrorMessage(nameof(NextScheduledStartDate), "No initial crawl start date selected.");
                else
                    Validation.ClearErrorMessages(nameof(NextScheduledStartDate));
            }
        }

        #endregion

        #region NextScheduledStartHour Property Members

        public static readonly DependencyProperty NextScheduledStartHourProperty = DependencyProperty.Register(nameof(NextScheduledStartHour), typeof(int?), typeof(EditCrawlConfigVM),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnNextScheduledStartHourPropertyChanged((int?)e.OldValue, (int?)e.NewValue)));

        public int? NextScheduledStartHour
        {
            get => (int?)GetValue(NextScheduledStartHourProperty);
            set => SetValue(NextScheduledStartHourProperty, value);
        }

        protected virtual void OnNextScheduledStartHourPropertyChanged(int? oldValue, int? newValue)
        {
            if (NoReschedule)
                return;

            OnNextScheduledStartHourChanged(newValue, NextScheduledStartIsPm);
            if (newValue.HasValue)
            {
                if (newValue.Value < 1)
                    Validation.SetErrorMessage(nameof(NextScheduledStartHour), "Hours cannot be less than 1");
                else if (newValue.Value > 12)
                    Validation.SetErrorMessage(nameof(NextScheduledStartHour), "Hours cannot be greater than 12");
                else
                    Validation.ClearErrorMessages(nameof(NextScheduledStartHour));
            }
            else
                Validation.SetErrorMessage(nameof(NextScheduledStartHour), "Hours not specified");
        }

        private void OnNextScheduledStartHourChanged(int? newValue, bool isPm)
        {
            ChangeTracker.SetChangeState(nameof(NextScheduledStartHour), ((newValue.Value == 12) ? (isPm ? 12 : 0) : (isPm ? newValue.Value + 12 : newValue.Value)) != Model?.NextScheduledStart?.Hour);
        }

        #endregion

        #region NextScheduledStartMinute Property Members

        public static readonly DependencyProperty NextScheduledStartMinuteProperty = DependencyProperty.Register(nameof(NextScheduledStartMinute), typeof(int?), typeof(EditCrawlConfigVM),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnNextScheduledStartMinutePropertyChanged((int?)e.OldValue, (int?)e.NewValue)));

        public int? NextScheduledStartMinute
        {
            get => (int?)GetValue(NextScheduledStartMinuteProperty);
            set => SetValue(NextScheduledStartMinuteProperty, value);
        }

        protected virtual void OnNextScheduledStartMinutePropertyChanged(int? oldValue, int? newValue)
        {
            if (NoReschedule)
                return;

            ChangeTracker.SetChangeState(nameof(NextScheduledStartMinute), newValue != Model.NextScheduledStart?.Minute);
            if (newValue.HasValue)
            {
                if (newValue.Value < 0)
                    Validation.SetErrorMessage(nameof(NextScheduledStartMinute), "Minutes cannot be negative");
                else if (newValue.Value > 59)
                    Validation.SetErrorMessage(nameof(NextScheduledStartMinute), "Minutes cannot be greater than 59");
                else
                    Validation.ClearErrorMessages(nameof(NextScheduledStartMinute));
            }
            else
                Validation.SetErrorMessage(nameof(NextScheduledStartMinute), "Minutes not specified");
        }

        #endregion

        #region NextScheduledStartIsPm Property Members

        public static readonly DependencyProperty NextScheduledStartIsPmProperty = DependencyProperty.Register(nameof(NextScheduledStartIsPm), typeof(bool), typeof(EditCrawlConfigVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnNextScheduledStartIsPmPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool NextScheduledStartIsPm
        {
            get => (bool)GetValue(NextScheduledStartIsPmProperty);
            set => SetValue(NextScheduledStartIsPmProperty, value);
        }

        protected virtual void OnNextScheduledStartIsPmPropertyChanged(bool oldValue, bool newValue)
        {
            if (!NoReschedule)
                OnNextScheduledStartHourChanged(NextScheduledStartHour, newValue);
        }

        #endregion

        #endregion

        #endregion

        #region Other Property Members

        #region DisplayName Property Members

        public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register(nameof(DisplayName), typeof(string), typeof(EditCrawlConfigVM),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnDisplayNamePropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string DisplayName
        {
            get => GetValue(DisplayNameProperty) as string;
            set => SetValue(DisplayNameProperty, value);
        }

        protected virtual void OnDisplayNamePropertyChanged(string oldValue, string newValue)
        {
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this) || Model is null)
                return;
#else
            if (Model is null)
                return;
#endif
            ChangeTracker.SetChangeState(nameof(DisplayName), (newValue = newValue.AsWsNormalizedOrEmpty()) == Model?.DisplayName);
            if (newValue.Length == 0)
                Validation.SetErrorMessage(nameof(DisplayName), FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameRequired);
            else if (newValue.Length > DbConstants.DbColMaxLen_LongName)
                Validation.SetErrorMessage(nameof(DisplayName), FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameLength);
            else
                Validation.ClearErrorMessages(nameof(DisplayName));
        }

        #endregion

        #region MaxTotalItems Members

        #region LimitTotalItems Property Members

        public static readonly DependencyProperty LimitTotalItemsProperty = DependencyProperty.Register(nameof(LimitTotalItems), typeof(bool), typeof(EditCrawlConfigVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnLimitTotalItemsPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool LimitTotalItems
        {
            get => (bool)GetValue(LimitTotalItemsProperty);
            set => SetValue(LimitTotalItemsProperty, value);
        }

        protected virtual void OnLimitTotalItemsPropertyChanged(bool oldValue, bool newValue)
        {
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this) || Model is null)
                return;
#else
            if (Model is null)
                return;
#endif
            OnMaxTotalItemsChanged(MaxTotalItems, newValue);
        }

        #endregion

        #region MaxTotalItems Property Members

        public static readonly DependencyProperty MaxTotalItemsProperty = DependencyProperty.Register(nameof(MaxTotalItems), typeof(ulong), typeof(EditCrawlConfigVM),
                new PropertyMetadata(0UL, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnMaxTotalItemsPropertyChanged((ulong)e.OldValue, (ulong)e.NewValue)));

        public ulong MaxTotalItems
        {
            get => (ulong)GetValue(MaxTotalItemsProperty);
            set => SetValue(MaxTotalItemsProperty, value);
        }

        protected virtual void OnMaxTotalItemsPropertyChanged(ulong oldValue, ulong newValue)
        {
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this) || Model is null)
                return;
#else
            if (Model is null)
                return;
#endif
            OnMaxTotalItemsChanged(newValue, LimitTotalItems);
        }

        private void OnMaxTotalItemsChanged(ulong value, bool isEnabled)
        {
            if (isEnabled)
            {
                ChangeTracker.SetChangeState(nameof(MaxTotalItems), Model?.MaxTotalItems != value);
                if (value < DbConstants.DbColMinValue_TTL_TotalSeconds)
                    Validation.SetErrorMessage(nameof(MaxTotalItems), FsInfoCat.Properties.Resources.ErrorMessage_TTLInvalid);
                else
                    Validation.ClearErrorMessages(nameof(MaxTotalItems));
            }
            else
            {
                Validation.ClearErrorMessages(nameof(MaxTotalItems));
                ChangeTracker.SetChangeState(nameof(MaxTotalItems), Model?.MaxTotalItems is not null);
            }
        }

        #endregion

        #endregion

        #region Root Directory Members

        #region Root Property Members

        private static readonly DependencyPropertyKey RootPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Root), typeof(Subdirectory), typeof(EditCrawlConfigVM),
            new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnRootPropertyChanged((Subdirectory)e.OldValue, (Subdirectory)e.NewValue)));

        public static readonly DependencyProperty RootProperty = RootPropertyKey.DependencyProperty;

        public Subdirectory Root
        {
            get => (Subdirectory)GetValue(RootProperty);
            private set => SetValue(RootPropertyKey, value);
        }

        protected virtual void OnRootPropertyChanged(Subdirectory oldValue, Subdirectory newValue)
        {
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
#endif
            ChangeTracker.SetChangeState(nameof(Path), Model?.RootId != newValue?.Id);
        }

        #endregion

        #region Path Property Members

        private static readonly DependencyPropertyKey PathPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Path), typeof(string), typeof(EditCrawlConfigVM), new PropertyMetadata(""));

        public static readonly DependencyProperty PathProperty = PathPropertyKey.DependencyProperty;

        public string Path
        {
            get => GetValue(PathProperty) as string;
            private set => SetValue(PathPropertyKey, value);
        }

        #endregion

        private AsyncFuncOpViewModel<string, (CrawlConfiguration Configuration, Subdirectory Root, string Path)> LookupCrawlConfigAsync(string path) =>
            OpAggregate.FromAsync("Getting Crawl Configuration", path, LookupCrawlConfigOpMgr, LookupCrawlConfigAsyncOpManager.LookupCrawlConfigAsync);

        //private AsyncFuncOpViewModel<string, (CrawlConfiguration Configuration, Subdirectory Root, string Path)> LookupCrawlConfigAsync2(string path) =>
        //    AsyncFuncOpViewModel<string, (CrawlConfiguration Configuration, Subdirectory Root, string Path)>.FromAsync(path, LookupCrawlConfigOpMgr, LookupCrawlConfigAsyncOpManager.LookupCrawlConfigAsync);

        //private AsyncFuncOpViewModel<Subdirectory, (CrawlConfiguration Configuration, Subdirectory Root, string Path)> GetSubdirectoryFullNameAsync(Subdirectory subdirectory) =>
        //    AsyncFuncOpViewModel<Subdirectory, (CrawlConfiguration Configuration, Subdirectory Root, string Path)>.FromAsync(subdirectory, GetSubdirectoryFullNameOpMgr,
        //        GetSubdirectoryFullNameOpMgr.LookupFullNameAsync);

        private AsyncFuncOpViewModel<(CrawlConfiguration, EditCrawlConfigVM), (CrawlConfiguration Configuration, Subdirectory Root)> SaveChangesAsync(CrawlConfiguration crawlConfiguration) =>
            OpAggregate.FromAsync("Saving Changes", (crawlConfiguration, this), SaveChangesOpMgr, SaveChangesOpMgr.SaveChangesAsync);

        //private AsyncFuncOpViewModel<(CrawlConfiguration, EditCrawlConfigVM), (CrawlConfiguration Configuration, Subdirectory Root)> SaveChangesAsync2(CrawlConfiguration crawlConfiguration) =>
        //    AsyncFuncOpViewModel<(CrawlConfiguration, EditCrawlConfigVM), (CrawlConfiguration Configuration, Subdirectory Root)>.FromAsync((crawlConfiguration, this), SaveChangesOpMgr, SaveChangesOpMgr.SaveChangesAsync);

        #endregion

        #region WindowTitle Property Members

        private static readonly DependencyPropertyKey WindowTitlePropertyKey = DependencyProperty.RegisterReadOnly(nameof(WindowTitle), typeof(string), typeof(EditCrawlConfigVM), new PropertyMetadata("Edit New Crawl Configuration"));

        public static readonly DependencyProperty WindowTitleProperty = WindowTitlePropertyKey.DependencyProperty;

        public string WindowTitle
        {
            get { return GetValue(WindowTitleProperty) as string; }
            private set { SetValue(WindowTitlePropertyKey, value); }
        }

        #endregion

        #region MaxRecursionDepth Property Members

        public static readonly DependencyProperty MaxRecursionDepthProperty = DependencyProperty.Register(nameof(MaxRecursionDepth), typeof(ushort), typeof(EditCrawlConfigVM),
                new PropertyMetadata(DbConstants.DbColDefaultValue_MaxRecursionDepth, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnMaxRecursionDepthPropertyChanged((ushort)e.OldValue, (ushort)e.NewValue)));

        public ushort MaxRecursionDepth
        {
            get => (ushort)GetValue(MaxRecursionDepthProperty);
            set => SetValue(MaxRecursionDepthProperty, value);
        }

        protected virtual void OnMaxRecursionDepthPropertyChanged(ushort oldValue, ushort newValue)
        {
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this) || Model is null)
                return;
#else
            if (Model is null)
                return;
#endif
            ChangeTracker.SetChangeState(nameof(MaxRecursionDepth), Model?.MaxRecursionDepth != newValue);
        }

        #endregion

        #region Status Members

        #region StatusValue Property Members

        private static readonly DependencyPropertyKey StatusValuePropertyKey = DependencyProperty.RegisterReadOnly(nameof(StatusValue), typeof(CrawlStatus), typeof(EditCrawlConfigVM),
                new PropertyMetadata(CrawlStatus.NotRunning, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnStatusValuePropertyChanged((CrawlStatus)e.OldValue, (CrawlStatus)e.NewValue)));

        public static readonly DependencyProperty StatusValueProperty = StatusValuePropertyKey.DependencyProperty;

        public CrawlStatus StatusValue
        {
            get => (CrawlStatus)GetValue(StatusValueProperty);
            private set => SetValue(StatusValuePropertyKey, value);
        }

        protected virtual void OnStatusValuePropertyChanged(CrawlStatus oldValue, CrawlStatus newValue)
        {
            IsEnabled = newValue != CrawlStatus.Disabled;
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this) || Model is null)
                return;
#else
            if (Model is null)
                return;
#endif
            ChangeTracker.SetChangeState(nameof(StatusValue), Model?.StatusValue != newValue);
        }

        #endregion

        #region EnabledStatus Property Members

        private static readonly DependencyPropertyKey EnabledStatusPropertyKey = DependencyProperty.RegisterReadOnly(nameof(EnabledStatus), typeof(CrawlStatus), typeof(EditCrawlConfigVM),
                new PropertyMetadata(CrawlStatus.NotRunning));

        public static readonly DependencyProperty EnabledStatusProperty = EnabledStatusPropertyKey.DependencyProperty;

        public CrawlStatus EnabledStatus
        {
            get => (CrawlStatus)GetValue(EnabledStatusProperty);
            private set => SetValue(EnabledStatusPropertyKey, value);
        }

        #endregion

        #region IsEnabled Property Members

        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register(nameof(IsEnabled), typeof(bool), typeof(EditCrawlConfigVM),
                new PropertyMetadata(true, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnIsEnabledPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool IsEnabled
        {
            get => (bool)GetValue(IsEnabledProperty);
            set => SetValue(IsEnabledProperty, value);
        }

        protected virtual void OnIsEnabledPropertyChanged(bool oldValue, bool newValue) => StatusValue = newValue ? EnabledStatus : CrawlStatus.Disabled;

        #endregion

        #endregion

        #region LastCrawlStart Property Members

        private static readonly DependencyPropertyKey LastCrawlStartPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastCrawlStart), typeof(DateTime?), typeof(EditCrawlConfigVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty LastCrawlStartProperty = LastCrawlStartPropertyKey.DependencyProperty;

        public DateTime? LastCrawlStart
        {
            get => (DateTime?)GetValue(LastCrawlStartProperty);
            private set => SetValue(LastCrawlStartPropertyKey, value);
        }

        #endregion

        #region LastCrawlEnd Property Members

        private static readonly DependencyPropertyKey LastCrawlEndPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastCrawlEnd), typeof(DateTime?), typeof(EditCrawlConfigVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty LastCrawlEndProperty = LastCrawlEndPropertyKey.DependencyProperty;

        public DateTime? LastCrawlEnd
        {
            get => (DateTime?)GetValue(LastCrawlEndProperty);
            private set => SetValue(LastCrawlEndPropertyKey, value);
        }

        #endregion

        #region Notes Property Members

        public static readonly DependencyProperty NotesProperty = DependencyProperty.Register(nameof(Notes), typeof(string), typeof(EditCrawlConfigVM),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnNotesPropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string Notes
        {
            get => GetValue(NotesProperty) as string;
            set => SetValue(NotesProperty, value);
        }

        protected virtual void OnNotesPropertyChanged(string oldValue, string newValue)
        {
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this) || Model is null)
                return;
#else
            if (Model is null)
                return;
#endif
            ChangeTracker.SetChangeState(nameof(Notes), Model?.Notes != newValue);
        }

        #endregion

        #region CreatedOn Property Members

        private static readonly DependencyPropertyKey CreatedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CreatedOn), typeof(DateTime), typeof(EditCrawlConfigVM),
                new PropertyMetadata(default));

        public static readonly DependencyProperty CreatedOnProperty = CreatedOnPropertyKey.DependencyProperty;

        public DateTime CreatedOn
        {
            get => (DateTime)GetValue(CreatedOnProperty);
            private set => SetValue(CreatedOnPropertyKey, value);
        }

        #endregion

        #region ModifiedOn Property Members

        private static readonly DependencyPropertyKey ModifiedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ModifiedOn), typeof(DateTime), typeof(EditCrawlConfigVM),
                new PropertyMetadata(default));

        public static readonly DependencyProperty ModifiedOnProperty = ModifiedOnPropertyKey.DependencyProperty;

        public DateTime ModifiedOn
        {
            get => (DateTime)GetValue(ModifiedOnProperty);
            private set => SetValue(ModifiedOnPropertyKey, value);
        }

        #endregion

        #region LastSynchronizedOn Property Members

        private static readonly DependencyPropertyKey LastSynchronizedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastSynchronizedOn), typeof(DateTime?), typeof(EditCrawlConfigVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty LastSynchronizedOnProperty = LastSynchronizedOnPropertyKey.DependencyProperty;

        public DateTime? LastSynchronizedOn
        {
            get => (DateTime?)GetValue(LastSynchronizedOnProperty);
            private set => SetValue(LastSynchronizedOnPropertyKey, value);
        }

        #endregion

        #endregion
    }
}
