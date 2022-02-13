using FsInfoCat.Activities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    public partial class BackgroundJobVM
    {
        class ChildOperationStartedObserver : IObserver<IAsyncActivity>
        {
            private readonly BackgroundJobVM _target;
            private readonly IAsyncActivity _activity;

            internal static ChildOperationStartedObserver Create([DisallowNull] ObservableCollection<BackgroundJobVM> backingCollection, [DisallowNull] IAsyncActivity asyncActivity, [DisallowNull] Dispatcher dispatcher, out BackgroundJobVM item)
            {
                item = dispatcher.Invoke(() =>
                {
                    BackgroundJobVM vm = new BackgroundJobVM(asyncActivity);
                    backingCollection.Add(vm);
                    return vm;
                });
                return new ChildOperationStartedObserver(item, asyncActivity);
            }

            private ChildOperationStartedObserver([DisallowNull] BackgroundJobVM target, [DisallowNull] IAsyncActivity asyncActivity) => (_target, _activity) = (target ?? throw new ArgumentNullException(nameof(target)), asyncActivity ?? throw new ArgumentNullException(nameof(asyncActivity)));

            //internal ChildOperationStartedObserver([DisallowNull] BackgroundJobVM target) => _target = target ?? throw new ArgumentNullException(nameof(target));

            void IObserver<IAsyncActivity>.OnCompleted()
            {
                _target._logger.LogDebug(@"Activity Completed: Type={Type}; ActivityId={ActivityId}; ParentActivityId={ParentActivityId}; StatusValue={StatusValue}; PercentComplete={PercentComplete}
ShortDescription={ShortDescription}
StatusMessage={StatusMessage}
CurrentOperation={CurrentOperation}", _activity.GetType(), _activity.ActivityId, _activity.ParentActivityId, _activity.StatusValue, _activity.PercentComplete, _activity.ShortDescription, _activity.StatusMessage, _activity.CurrentOperation);
                _target._activityStartedSubscription?.Dispose();
                _target.Dispatcher.BeginInvoke(() => _target._backingItems.Clear(), DispatcherPriority.Background);
            }

            void IObserver<IAsyncActivity>.OnError([DisallowNull] Exception error)
            {
                if (error is ActivityException asyncOpError)
                    _target._logger.LogError(asyncOpError.Code.ToEventId(), asyncOpError, "Background progress error observed: Error Code={Code}; Error Message={Message}; Activity={Activity}; StatusMessage={StatusMessage}; Current Operation={CurrentOperation}; Activity ID={ActivityId}",
                        asyncOpError.Code, asyncOpError.Message, asyncOpError.Operation?.ShortDescription, asyncOpError.Operation?.StatusMessage, asyncOpError.Operation?.CurrentOperation, asyncOpError.Operation?.ActivityId);
                else
                    _target._logger.LogError(ErrorCode.Unexpected.ToEventId(), error, "Unexpected background progress error observed: Error Message={Message}", error.Message);
            }

            void IObserver<IAsyncActivity>.OnNext([DisallowNull] IAsyncActivity value)
            {
                _target._logger.LogDebug($"{nameof(BackgroundJobVM)}.{nameof(ChildOperationStartedObserver)}.{nameof(IObserver<IAsyncActivity>.OnNext)} invoked: ActivityId={{ActivityId}}; ShortDescription={{ShortDescription}}; StatusMessage={{StatusMessage}}; ParentActivityId={{ParentActivityId}}",
                    value.ActivityId, value.ShortDescription, value.StatusMessage, value.ParentActivityId);
                OnActivityStarted(_target._logger, value, _target.Dispatcher, _target._backingItems);
            }
        }
    }
}
