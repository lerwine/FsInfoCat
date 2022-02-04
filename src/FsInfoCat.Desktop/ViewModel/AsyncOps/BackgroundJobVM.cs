using FsInfoCat.Activities;
using FsInfoCat.AsyncOps;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    /// <summary>
    /// View model for a background job being tracked by a <see cref="JobServiceStatusViewModel"/>.
    /// </summary>
    /// <seealso cref="DependencyObject" />
    public partial class BackgroundJobVM : DependencyObject
    {
        private readonly ILogger<BackgroundJobVM> _logger;
        private IDisposable _currentActivitySubscription;
        private IDisposable _childActivitySubscription;

        internal CancellationToken Token { get; }

        #region ActivityId Property Members

        private static readonly DependencyPropertyKey ActivityIdPropertyKey = DependencyPropertyBuilder<BackgroundJobVM, Guid>
            .Register(nameof(ActivityId))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ActivityId"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ActivityIdProperty = ActivityIdPropertyKey.DependencyProperty;

        public Guid ActivityId => (Guid)GetValue(ActivityIdProperty);

        #endregion
        #region ShortDescription Property Members

        private static readonly DependencyPropertyKey ShortDescriptionPropertyKey = DependencyPropertyBuilder<BackgroundJobVM, string>
            .Register(nameof(ShortDescription))
            .DefaultValue("")
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ShortDescription"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShortDescriptionProperty = ShortDescriptionPropertyKey.DependencyProperty;

        public string ShortDescription => GetValue(ShortDescriptionProperty) as string;

        #endregion
        #region StatusMessage Property Members

        private static readonly DependencyPropertyKey StatusMessagePropertyKey = DependencyPropertyBuilder<BackgroundJobVM, string>
            .Register(nameof(StatusMessage))
            .DefaultValue("")
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="StatusMessage"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusMessageProperty = StatusMessagePropertyKey.DependencyProperty;

        public string StatusMessage { get => GetValue(StatusMessageProperty) as string; private set => SetValue(StatusMessagePropertyKey, value); }

        #endregion
        #region CurrentOperation Property Members

        private static readonly DependencyPropertyKey CurrentOperationPropertyKey = DependencyPropertyBuilder<BackgroundJobVM, string>
            .Register(nameof(CurrentOperation))
            .DefaultValue("")
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="CurrentOperation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CurrentOperationProperty = CurrentOperationPropertyKey.DependencyProperty;

        public string CurrentOperation { get => GetValue(CurrentOperationProperty) as string; private set => SetValue(CurrentOperationPropertyKey, value); }

        #endregion
        #region MessageLevel Property Members

        private static readonly DependencyPropertyKey MessageLevelPropertyKey = DependencyPropertyBuilder<BackgroundJobVM, StatusMessageLevel>
            .Register(nameof(MessageLevel))
            .DefaultValue(StatusMessageLevel.Information)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="MessageLevel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MessageLevelProperty = MessageLevelPropertyKey.DependencyProperty;

        public StatusMessageLevel MessageLevel { get => (StatusMessageLevel)GetValue(MessageLevelProperty); private set => SetValue(MessageLevelPropertyKey, value); }

        #endregion
        #region PercentComplete Property Members

        private static readonly DependencyPropertyKey PercentCompletePropertyKey = DependencyPropertyBuilder<BackgroundJobVM, int?>
            .Register(nameof(PercentComplete))
            .DefaultValue(null)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="PercentComplete"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PercentCompleteProperty = PercentCompletePropertyKey.DependencyProperty;

        public int? PercentComplete { get => (int?)GetValue(PercentCompleteProperty); private set => SetValue(PercentCompletePropertyKey, value); }

        #endregion
        #region Started Property Members

        private static readonly DependencyPropertyKey StartedPropertyKey = DependencyPropertyBuilder<BackgroundJobVM, DateTime?>
            .Register(nameof(Started))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Started"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StartedProperty = StartedPropertyKey.DependencyProperty;

        public DateTime? Started { get => (DateTime?)GetValue(StartedProperty); private set => SetValue(StartedPropertyKey, value); }

        #endregion
        #region Duration Property Members

        private static readonly DependencyPropertyKey DurationPropertyKey = DependencyPropertyBuilder<BackgroundJobVM, TimeSpan>
            .Register(nameof(Duration))
            .DefaultValue(TimeSpan.Zero)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Duration"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DurationProperty = DurationPropertyKey.DependencyProperty;

        public TimeSpan Duration { get => (TimeSpan)GetValue(DurationProperty); private set => SetValue(DurationPropertyKey, value); }

        #endregion
        #region Cancel Property Members

        private static readonly DependencyPropertyKey CancelPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Cancel),
            typeof(Commands.RelayCommand), typeof(BackgroundJobVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Cancel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CancelProperty = CancelPropertyKey.DependencyProperty;

        public Commands.RelayCommand Cancel => (Commands.RelayCommand)GetValue(CancelProperty);

        #endregion
        #region Items Property Members

        private readonly ObservableCollection<BackgroundJobVM> _backingItems = new();

        private static readonly DependencyPropertyKey ItemsPropertyKey = DependencyPropertyBuilder<BackgroundJobVM, ReadOnlyObservableCollection<BackgroundJobVM>>
            .Register(nameof(Items))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Items"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemsProperty = ItemsPropertyKey.DependencyProperty;

        public ReadOnlyObservableCollection<BackgroundJobVM> Items => (ReadOnlyObservableCollection<BackgroundJobVM>)GetValue(ItemsProperty);

        #endregion

        internal BackgroundJobVM([DisallowNull] IAsyncActivity activity)
        {
            if (activity is null) throw new ArgumentNullException(nameof(activity));
            _logger = App.GetLogger(this);
            SetValue(ActivityIdPropertyKey, activity.ActivityId);
            SetValue(ItemsPropertyKey, new ReadOnlyObservableCollection<BackgroundJobVM>(_backingItems));
            SetValue(ShortDescriptionPropertyKey, activity.ShortDescription);
            CurrentOperation = activity.CurrentOperation;
            StatusMessage = activity.StatusMessage;
            int p = activity.PercentComplete;
            PercentComplete = (p < 0) ? null : p;
            if (activity is ITimedAsyncAction<ITimedActivityEvent> timedAsyncAction)
            {
                Started = timedAsyncAction.Started;
                Duration = timedAsyncAction.Duration;
            }
            SetValue(CancelPropertyKey, new Commands.RelayCommand(parameter =>
            {
                if (!activity.TokenSource.IsCancellationRequested)
                    activity.TokenSource.Cancel(true);
            }));
        }

        private static BackgroundJobVM AppendItem<TEvent, TActivity>(ObservableCollection<BackgroundJobVM> backingCollection, TActivity asyncActivity, Func<BackgroundJobVM, IObserver<TEvent>> createObserver)
            where TActivity : IAsyncActivity, IObservable<TEvent>
        {
            if (backingCollection is null) throw new ArgumentNullException(nameof(backingCollection));
            if (asyncActivity is null) throw new ArgumentNullException(nameof(asyncActivity));
            BackgroundJobVM item = new BackgroundJobVM(asyncActivity);
            backingCollection.Add(item);
            item._currentActivitySubscription = asyncActivity.Subscribe(createObserver(item));
            item._childActivitySubscription = asyncActivity.SubscribeStateChange(new ChildOperationEventObserver(item), activeOps =>
            {
                foreach (IAsyncActivity activity in activeOps)
                    AppendItem(item._logger, activity, item._backingItems);
            });
            return item;
        }

        internal static void AppendItem([DisallowNull] ILogger logger, [DisallowNull] IAsyncActivity activity, [DisallowNull] ObservableCollection<BackgroundJobVM> backingCollection)
        {
            if (activity is null) throw new ArgumentNullException(nameof(activity));
            if (activity is ITimedAsyncAction<ITimedActivityEvent> timedAsyncAction)
            {
                BackgroundJobVM item = AppendItem(backingCollection, timedAsyncAction, vm => new TimedItemEventObserver(vm, () => backingCollection.Remove(vm)));
                item.Started = timedAsyncAction.Started;
                item.Duration = timedAsyncAction.Duration;
            }
            else if (activity is IAsyncAction<IActivityEvent> asyncAction)
                _ = AppendItem(backingCollection, asyncAction, vm => new ItemEventObserver(vm, () => backingCollection.Remove(vm)));
            else
                logger.LogError("{Type} is not a valid activity type.", activity.GetType().AssemblyQualifiedName);
        }

        //private static bool TryFindParentVM(Guid parentId, [DisallowNull] ObservableCollection<BackgroundJobVM> backingItems, out BackgroundJobVM vm)
        //{
        //    vm = backingItems.FirstOrDefault(i => i.OperationId == parentId);
        //    if (vm is not null)
        //        return true;
        //    foreach (BackgroundJobVM item in backingItems)
        //    {
        //        if (TryFindParentVM(parentId, item._backingItems, out vm))
        //            return true;
        //    }
        //    return false;
        //}
    }
}
