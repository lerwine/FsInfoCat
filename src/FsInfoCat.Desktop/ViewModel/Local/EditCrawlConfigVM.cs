using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public class EditCrawlConfigVM : EditDbEntityVM<CrawlConfiguration>, IHasSubdirectoryEntity
    {
        ///// <summary>
        ///// Instantiates a new <see cref="View.EditCrawlConfigWindow"/> to edit the properties of an existing <see cref="CrawlConfiguration"/>.
        ///// </summary>
        ///// <param name="model">The <see cref="CrawlConfiguration"/> to be modified.</param>
        ///// <returns><see langword="true"/> if modifications were successfully saved to the databaase; otherwise <see langword="false"/> to indicate the user cancelled or there was
        ///// an error that prohibited successful initialization.</returns>
        //internal static bool Edit([DisallowNull] CrawlConfiguration model)
        //{
        //    View.Local.EditCrawlConfigWindow window = new();
        //    EditCrawlConfigVM vm = (EditCrawlConfigVM)window.DataContext;
        //    if (vm is null)
        //    {
        //        vm = new();
        //        window.DataContext = vm;
        //    }
        //    vm.Initialize(model);
        //    vm.CloseCancel += new EventHandler((sender, e) => window.DialogResult = false);
        //    vm.CloseSuccess += new EventHandler((sender, e) => window.DialogResult = true);
        //    return window.ShowDialog() ?? false;
        //}

        ///// <summary>
        ///// Instantiates a new <see cref="View.EditCrawlConfigWindow"/> to edit the properties of an existing <see cref="CrawlConfiguration"/> for a specified path. If no configuration
        ///// exists for the specified path, a new entity will be created when the user saves changes.
        ///// </summary>
        ///// <param name="crawlRoot">The root path of the crawl configuration.</param>
        ///// <param name="model">Returns the <see cref="CrawlConfiguration"/> that was saved to the database.</param>
        ///// <param name="isNew">Returns <see langword="true"/> if a new entity was saved to the database; otherwise, <see langword="false"/>.</param>
        ///// <returns><see langword="true"/> if a new record or modifications to an existing one were successfully saved to the databaase;
        ///// otherwise <see langword="false"/> to indicate the user cancelled or there was an error that prohibited successful initialization.</returns>
        //internal static bool Edit(string crawlRoot, out CrawlConfiguration model, out bool isNew)
        //{
        //    View.Local.EditCrawlConfigWindow window = new();
        //    EditCrawlConfigVM vm = (EditCrawlConfigVM)window.DataContext;
        //    if (vm is null)
        //    {
        //        vm = new();
        //        window.DataContext = vm;
        //    }

        //    EventHandler closeCancelHandler = new((sender, e) =>
        //    {
        //        window.DialogResult = false;
        //    });
        //    vm.CloseCancel += closeCancelHandler;
        //    vm.OpAggregate.OperationCancelRequested += closeCancelHandler;
        //    vm.CloseSuccess += new EventHandler((sender, e) => window.DialogResult = true);
        //    window.Loaded += new RoutedEventHandler((sender, e) =>
        //    {
        //        //AsyncOps.AsyncFuncOpViewModel<string, ConfigurationRootAndPath> lookupCrawlConfig = vm.OpAggregate.FromAsync("Getting Crawl Configuration", "Connecting to database", crawlRoot,
        //        //    vm.LookupCrawlConfigOpMgr, LookupCrawlConfigAsync);
        //        //lookupCrawlConfig.GetTask().ContinueWith(task =>
        //        //{
        //        //    vm.Dispatcher.Invoke(() =>
        //        //    {
        //        //        if (task.IsCompletedSuccessfully)
        //        //        {
        //        //            vm.OpAggregate.CancelOperation -= closeCancelHandler;
        //        //            vm.Initialize(task.Result.Configuration, task.Result.Root, task.Result.Path);
        //        //            vm.LookupCrawlConfigOpMgr.RemoveOperation(lookupCrawlConfig);
        //        //        }
        //        //    });
        //        //});
        //    });
        //    if (window.ShowDialog() ?? false)
        //    {
        //        isNew = vm.IsNew;
        //        model = vm.Model;
        //        return true;
        //    }
        //    model = null;
        //    isNew = false;
        //    return false;
        //}

        //internal void Initialize([DisallowNull] CrawlConfiguration crawlConfiguration)
        //{
        //    Root = crawlConfiguration.Root;
        //    DisplayName = crawlConfiguration.DisplayName;
        //    MaxRecursionDepth = crawlConfiguration.MaxRecursionDepth;
        //    ulong? mti = crawlConfiguration.MaxTotalItems;
        //    LimitTotalItems = mti.HasValue;
        //    MaxTotalItems = mti ?? int.MaxValue;
        //    long? seconds = crawlConfiguration.TTL;
        //    if (seconds.HasValue)
        //    {
        //        LimitTTL = true;
        //        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds.Value);
        //        TtlDays = timeSpan.Days;
        //        TtlHours = timeSpan.Hours;
        //        TtlMinutes = timeSpan.Minutes;
        //    }
        //    else
        //        LimitTTL = false;
        //    StatusValue = crawlConfiguration.StatusValue;
        //    LastCrawlEnd = crawlConfiguration.LastCrawlEnd;
        //    LastCrawlStart = crawlConfiguration.LastCrawlStart;
        //    Notes = crawlConfiguration.Notes;
        //    RescheduleAfterFail = crawlConfiguration.RescheduleAfterFail;
        //    RescheduleFromJobEnd = crawlConfiguration.RescheduleFromJobEnd;
        //    seconds = crawlConfiguration.RescheduleInterval;
        //    DateTime? nextScheduledStart = crawlConfiguration.NextScheduledStart;
        //    if (seconds.HasValue)
        //    {
        //        AutoReschedule = true;
        //        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds.Value);
        //        RescheduleDays = timeSpan.Days;
        //        RescheduleHours = timeSpan.Hours;
        //        RescheduleMinutes = timeSpan.Minutes;
        //    }
        //    else
        //        OneTimeSchedule = nextScheduledStart.HasValue;
        //    DateTime dateTime = nextScheduledStart ?? DateTime.Now.AddHours(8.0);
        //    NextScheduledStartDate = dateTime.Date;
        //    NextScheduledStartHour = dateTime.Hour;
        //    NextScheduledStartMinute = dateTime.Minute;
        //}

        #region SelectRootCommand Property Members

        private static readonly DependencyPropertyKey SelectRootCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SelectRootCommand),
            typeof(Commands.RelayCommand), typeof(EditCrawlConfigVM), new PropertyMetadata(null));

        public static readonly DependencyProperty SelectRootCommandProperty = SelectRootCommandPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets a bindable command for selecting the root path.
        /// </summary>
        /// <value>The bindable <see cref="Commands.RelayCommand">RelayCommand</see> for selecting a new root path for the crawl configuration..</value>
        public Commands.RelayCommand SelectRootCommand => (Commands.RelayCommand)GetValue(SelectRootCommandProperty);

        private static Task<DirectoryInfo> GetDirectoryAsync(string path, IWindowsStatusListener listener) => string.IsNullOrEmpty(path) ? Task.FromResult<DirectoryInfo>(null) :
            Task.Run(() =>
            {
                listener.CancellationToken.ThrowIfCancellationRequested();
                if (System.IO.Path.IsPathFullyQualified(path))
                {
                    DirectoryInfo directoryInfo = new(path);
                    if (directoryInfo.Exists)
                        return directoryInfo;
                }
                return null;
            });

        private void OnSelectRootExecute(object parameter)
        {
            BgOps.FromAsync("Initializing", "Getting start directory", Path, GetDirectoryAsync).ContinueWith(task => Dispatcher.Invoke(() =>
                ShowFolderBrowserDialog((task.IsCompletedSuccessfully && task.Result is not null) ? task.Result.FullName : Environment.GetFolderPath(Environment.SpecialFolder.UserProfile))));
        }

        private static async Task<Subdirectory> CheckPathAsync(string path, IWindowsStatusListener listener)
        {
            using IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            Subdirectory subdirectory = await Subdirectory.FindByFullNameAsync(path, listener.CancellationToken, dbContext);
            if (subdirectory is not null && subdirectory.CrawlConfiguration is null)
                subdirectory.CrawlConfiguration = await dbContext.Entry(subdirectory).GetRelatedReferenceAsync(d => d.CrawlConfiguration, listener.CancellationToken);
            return subdirectory;
        }

        internal static Task<(string, Subdirectory)?> BrowseForFolderAsync(string newPath, AsyncOps.AsyncBgModalVM bgOpManager)
        {
            using (System.Windows.Forms.FolderBrowserDialog dialog = new()
            {
                Description = FsInfoCat.Properties.Resources.Description_SelectCrawlRootFolder,
                SelectedPath = newPath
            })
            {
                if (dialog.ShowDialog(new WindowOwner()) != System.Windows.Forms.DialogResult.OK)
                    return null;
                newPath = dialog.SelectedPath;
            }

            return bgOpManager.FromAsync("Checking Availability", "Checking whether selected subdirectory is already configured", newPath, CheckPathAsync).ContinueWith<(string, Subdirectory)?>(task =>
                (newPath, task.Result), TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        private void ShowFolderBrowserDialog(string newPath)
        {
            IEnumerable<string> errors = Validation.GetErrors(nameof(Path));
            Validation.ClearErrorMessages(nameof(Path));
            BrowseForFolderAsync(newPath, BgOps).ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    if (task.Result?.Item2?.CrawlConfiguration is null)
                        Dispatcher.Invoke(() =>
                        {
                            Path = newPath;
                            Root = task.Result?.Item2;
                        });
                    else
                        Dispatcher.Invoke(() =>
                        {
                            MessageBox.Show(Application.Current.MainWindow, "Not Available", "That subdirectory already has a crawl configuration.", MessageBoxButton.OK, MessageBoxImage.Error);
                            if (errors?.Any() ?? false)
                                Validation.SetErrorMessage(nameof(Path), errors.First(), errors.Skip(1).ToArray());
                        });
                }
                else
                    Dispatcher.Invoke(() =>
                    {
                        if (errors?.Any() ?? false)
                            Validation.SetErrorMessage(nameof(Path), errors.First(), errors.Skip(1).ToArray());
                    });
            });
        }

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

            ChangeTracker.SetChangeState(nameof(NextScheduledStartMinute), newValue != Model?.NextScheduledStart?.Minute);
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

        private static readonly DependencyPropertyKey RootPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Root), typeof(ISimpleIdentityReference<Subdirectory>), typeof(EditCrawlConfigVM),
            new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnRootPropertyChanged((ISimpleIdentityReference<Subdirectory>)e.OldValue, (ISimpleIdentityReference<Subdirectory>)e.NewValue)));

        public static readonly DependencyProperty RootProperty = RootPropertyKey.DependencyProperty;

        public ISimpleIdentityReference<Subdirectory> Root
        {
            get => (ISimpleIdentityReference<Subdirectory>)GetValue(RootProperty);
            private set => SetValue(RootPropertyKey, value);
        }

        protected virtual void OnRootPropertyChanged(ISimpleIdentityReference<Subdirectory> oldValue, ISimpleIdentityReference<Subdirectory> newValue)
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
            internal set => SetValue(PathPropertyKey, value);
        }

        #endregion

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

        public EditCrawlConfigVM()
        {
            SetValue(SelectRootCommandPropertyKey, new Commands.RelayCommand(OnSelectRootExecute));
        }

        protected override DbSet<CrawlConfiguration> GetDbSet([DisallowNull] LocalDbContext dbContext) => dbContext.CrawlConfigurations;

        protected override void OnModelPropertyChanged(CrawlConfiguration oldValue, CrawlConfiguration newValue)
        {
            if (newValue is null)
            {
                DisplayName = Notes = "";
                LimitTotalItems = LimitTTL = NextScheduledStartIsPm = RescheduleAfterFail = RescheduleFromJobEnd = false;
                MaxTotalItems = 0UL;
                MaxRecursionDepth = DbConstants.DbColDefaultValue_MaxRecursionDepth;
                StatusValue = CrawlStatus.NotRunning;
                LastCrawlStart = LastCrawlEnd = NextScheduledStartDate = null;
                TtlDays = TtlHours = TtlMinutes = RescheduleDays = RescheduleHours = RescheduleMinutes = null;
                NoReschedule = true;
                NextScheduledStartHour = NextScheduledStartMinute = null;
                return;
            }

            //if ((Root = newValue.Root) is null || AttachedProperties.GetFullNameId(this) == newValue.Root.Id)
            //    Path = AttachedProperties.GetFullName(this) ?? "";
            //else
            //    AttachedProperties.GetFullName(this, BgOps, p => Path = p ?? "");
            // TODO: Calculate full name
            DisplayName = newValue.DisplayName;
            LimitTotalItems = newValue.MaxTotalItems.HasValue;
            MaxTotalItems = newValue.MaxTotalItems ?? 0UL;
            MaxRecursionDepth = newValue.MaxRecursionDepth;
            StatusValue = newValue.StatusValue;
            LastCrawlStart = newValue.LastCrawlStart;
            LastCrawlEnd = newValue.LastCrawlEnd;
            Notes = newValue.Notes;
            RescheduleAfterFail = newValue.RescheduleAfterFail;
            RescheduleFromJobEnd = newValue.RescheduleFromJobEnd;
            TimeSpan? timeSpan = newValue.GetTTLAsTimeSpan();
            if (timeSpan.HasValue)
            {
                LimitTTL = true;
                TtlDays = timeSpan.Value.Days;
                TtlHours = timeSpan.Value.Hours;
                TtlMinutes = timeSpan.Value.Minutes;
            }
            else
            {
                LimitTTL = false;
                TtlDays = TtlHours = TtlMinutes = null;
            }
            DateTime? dateTime = newValue.NextScheduledStart;
            NextScheduledStartDate = dateTime?.Date;
            timeSpan = newValue.GetRescheduleIntervalAsTimeSpan();
            if (timeSpan.HasValue)
            {
                AutoReschedule = true;
                RescheduleDays = timeSpan.Value.Days;
                RescheduleHours = timeSpan.Value.Hours;
                RescheduleMinutes = timeSpan.Value.Minutes;
            }
            else
            {
                RescheduleDays = RescheduleHours = RescheduleMinutes = null;
                if (dateTime.HasValue)
                    OneTimeSchedule = true;
                else
                    NoReschedule = true;
            }
            if (dateTime.HasValue)
            {
                NextScheduledStartMinute = dateTime.Value.Minute;
                switch (dateTime.Value.Hour)
                {
                    case 0:
                        NextScheduledStartHour = 12;
                        NextScheduledStartIsPm = false;
                        break;
                    case 12:
                        NextScheduledStartHour = 12;
                        NextScheduledStartIsPm = true;
                        break;
                    default:
                        if (dateTime.Value.Hour < 12)
                        {
                            NextScheduledStartHour = dateTime.Value.Hour;
                            NextScheduledStartIsPm = false;
                        }
                        else
                        {
                            NextScheduledStartHour = dateTime.Value.Hour - 12;
                            NextScheduledStartIsPm = true;
                        }
                        break;
                }
            }
            else
            {
                NextScheduledStartHour = NextScheduledStartMinute = null;
                NextScheduledStartIsPm = false;
            }
        }

        protected override bool OnBeforeSave()
        {
            CrawlConfiguration model = Model;
            model.RootId = Root.Id;
            model.DisplayName = DisplayName.AsWsNormalizedOrEmpty();
            if (LimitTotalItems)
                model.MaxTotalItems = MaxTotalItems;
            else
                model.MaxTotalItems = null;
            model.MaxRecursionDepth = MaxRecursionDepth;
            model.StatusValue = StatusValue;
            model.Notes = Notes.EmptyIfNullOrWhiteSpace();
            model.RescheduleAfterFail = RescheduleAfterFail;
            model.RescheduleFromJobEnd = RescheduleFromJobEnd;
            if (LimitTTL)
                model.TTL = Convert.ToInt64(new TimeSpan(TtlDays ?? 0, TtlHours ?? 0, TtlMinutes ?? 0, 0).TotalSeconds);
            else
                model.TTL = null;
            DateTime? dateTime = model.NextScheduledStart;
            model.NextScheduledStart = dateTime;
            if (dateTime.HasValue && AutoReschedule)
                model.RescheduleInterval = Convert.ToInt64(new TimeSpan(RescheduleDays ?? 0, RescheduleHours ?? 0, RescheduleMinutes ?? 0, 0).TotalSeconds);
            else
                model.RescheduleInterval = null;
            return true;
        }

        protected override async Task<bool> OnSaveChangesAsync([DisallowNull] EntityEntry<CrawlConfiguration> entry, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IWindowsStatusListener statusListener, bool force = false)
        {
            if (entry.Entity.Root is null)
            {
                entry.Entity.Root = (await Subdirectory.ImportBranchAsync(new DirectoryInfo(Dispatcher.Invoke(() => Path)), dbContext, statusListener.CancellationToken, true))?.Entity;
                if (entry.Entity.Root is null)
                {
                    Dispatcher.Invoke(() => MessageBox.Show(Application.Current.MainWindow, "Import Failure", "Unable to import teh subdirectory specified.", MessageBoxButton.OK, MessageBoxImage.Error));
                    return false;
                }
            }
            return await base.OnSaveChangesAsync(entry, dbContext, statusListener, force);
        }

        ISimpleIdentityReference<Subdirectory> IHasSubdirectoryEntity.GetSubdirectoryEntity() => CheckAccess() ? Root : Dispatcher.Invoke(() => Root);

        public async Task<ISimpleIdentityReference<Subdirectory>> GetSubdirectoryEntityAsync([DisallowNull] IWindowsStatusListener statusListener)
        {
            return await Dispatcher.InvokeAsync(() => Root);
        }

        public record ConfigurationAndRoot(CrawlConfiguration Configuration, Subdirectory Root);

        public record ConfigurationRootAndPath(CrawlConfiguration Configuration, Subdirectory Root, string Path);
    }
}
