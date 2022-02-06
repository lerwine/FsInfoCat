using FsInfoCat;
using FsInfoCat.Activities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;

namespace FsInfoCat.UnitTests
{
    [TestClass]
    public class AsyncActivityServiceTests
    {
#pragma warning disable IDE0052 // Remove unread private members
        private static TestContext _testContext;
#pragma warning restore IDE0052 // Remove unread private members

        [ClassInitialize]
        public static void OnClassInitialize(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestMethod, Priority(0)]
        public void GetAsyncActivityServiceTestMethod()
        {
            IAsyncActivityService asyncActivityService = Hosting.GetAsyncActivityService();
            Assert.IsNotNull(asyncActivityService);
            Assert.IsNotNull(asyncActivityService.ActivityStartedObservable);
        }

        [TestMethod("InvokeAsync(string activityDescription, string initialStatusMessage, Func<IActivityProgress, Task>)"), Priority(1)]
        public void InvokeAsyncTestMethod1()
        {
            IAsyncActivityService asyncActivityService = Hosting.GetAsyncActivityService();
            if (asyncActivityService is null) Assert.Inconclusive("Hosting.GetAsyncActivityService returned null");
            string activityDescription = "Example Activity";
            string initialStatusMessage = "Initial Status Example";
            using AutoResetEvent resetEvent = new(false);
            Task asyncMethodDelegate(IActivityProgress progress) => Task.Run(() =>
            {
                resetEvent.Set();
                resetEvent.WaitOne();
                progress.Token.ThrowIfCancellationRequested();
            });
            IAsyncAction<IActivityEvent> target = asyncActivityService.InvokeAsync(activityDescription, initialStatusMessage, asyncMethodDelegate);
            Assert.IsNotNull(target);
            Assert.IsTrue(asyncActivityService.Contains(target));
            Assert.AreEqual(activityDescription, target.ShortDescription);
            Assert.AreEqual(initialStatusMessage, target.StatusMessage);
            Assert.AreEqual(string.Empty, target.CurrentOperation);
            Assert.AreEqual(-1, target.PercentComplete);
            Assert.IsNull(target.ParentActivityId);
            resetEvent.WaitOne();
            Assert.AreEqual(ActivityStatus.Running, target.StatusValue);
            resetEvent.Set();
            target.Task.Wait();
            Assert.AreEqual(ActivityStatus.RanToCompletion, target.StatusValue);
            Assert.AreEqual(activityDescription, target.ShortDescription);
            Assert.AreEqual(initialStatusMessage, target.StatusMessage);
            Assert.AreEqual(string.Empty, target.CurrentOperation);
            Assert.AreEqual(-1, target.PercentComplete);
            Assert.IsNull(target.ParentActivityId);
            Assert.IsFalse(asyncActivityService.Contains(target));
        }

        [TestMethod("InvokeAsync<int>(string activityDescription, string initialStatusMessage, Func<IActivityProgress, Task<int>>)"), Priority(1)]
        public void InvokeAsyncTestMethod2()
        {
            IAsyncActivityService asyncActivityService = Hosting.GetAsyncActivityService();
            if (asyncActivityService is null) Assert.Inconclusive("Hosting.GetAsyncActivityService returned null");
            string activityDescription = "Example Activity";
            string initialStatusMessage = "Initial Status Example";
            int expectedResult = 7;
            using AutoResetEvent resetEvent = new(false);
            Task<int> asyncMethodDelegate(IActivityProgress progress) => Task.Run(() =>
            {
                resetEvent.Set();
                resetEvent.WaitOne();
                progress.Token.ThrowIfCancellationRequested();
                return expectedResult;
            });
            IAsyncFunc<IActivityEvent, int> target = asyncActivityService.InvokeAsync(activityDescription, initialStatusMessage, asyncMethodDelegate);
            Assert.IsNotNull(target);
            Assert.IsTrue(asyncActivityService.Contains(target));
            Assert.AreEqual(activityDescription, target.ShortDescription);
            Assert.AreEqual(initialStatusMessage, target.StatusMessage);
            Assert.AreEqual(string.Empty, target.CurrentOperation);
            Assert.AreEqual(-1, target.PercentComplete);
            Assert.IsNull(target.ParentActivityId);
            resetEvent.WaitOne();
            Assert.AreEqual(ActivityStatus.Running, target.StatusValue);
            resetEvent.Set();
            target.Task.Wait();
            Assert.AreEqual(target.Task.Result, expectedResult);
            Assert.AreEqual(ActivityStatus.RanToCompletion, target.StatusValue);
            Assert.AreEqual(activityDescription, target.ShortDescription);
            Assert.AreEqual(initialStatusMessage, target.StatusMessage);
            Assert.AreEqual(string.Empty, target.CurrentOperation);
            Assert.AreEqual(-1, target.PercentComplete);
            Assert.IsNull(target.ParentActivityId);
            Assert.IsFalse(asyncActivityService.Contains(target));
        }

        [TestMethod("InvokeAsync<string>(string activityDescription, string initialStatusMessage, Func<IActivityProgress<string>, Task>)"), Priority(1)]
        public void InvokeAsyncTestMethod3()
        {
            IAsyncActivityService asyncActivityService = Hosting.GetAsyncActivityService();
            if (asyncActivityService is null) Assert.Inconclusive("Hosting.GetAsyncActivityService returned null");
            string activityDescription = "Example Activity";
            string initialStatusMessage = "Initial Status Example";
            string state = "State value example";
            using AutoResetEvent resetEvent = new(false);
            Task asyncMethodDelegate(IActivityProgress<string> progress) => Task.Run(() =>
            {
                resetEvent.Set();
                resetEvent.WaitOne();
                progress.Token.ThrowIfCancellationRequested();
                Assert.AreEqual(state, progress.AsyncState);
            });
            IAsyncAction<IActivityEvent<string>, string> target = asyncActivityService.InvokeAsync(activityDescription, initialStatusMessage, state, asyncMethodDelegate);
            Assert.IsNotNull(target);
            Assert.IsTrue(asyncActivityService.Contains(target));
            Assert.AreEqual(activityDescription, target.ShortDescription);
            Assert.AreEqual(initialStatusMessage, target.StatusMessage);
            Assert.AreEqual(string.Empty, target.CurrentOperation);
            Assert.AreEqual(-1, target.PercentComplete);
            Assert.IsNull(target.ParentActivityId);
            Assert.AreEqual(state, target.AsyncState);
            Assert.AreEqual(state, target.Task.AsyncState);
            resetEvent.WaitOne();
            Assert.AreEqual(ActivityStatus.Running, target.StatusValue);
            resetEvent.Set();
            target.Task.Wait();
            Assert.AreEqual(ActivityStatus.RanToCompletion, target.StatusValue);
            Assert.AreEqual(activityDescription, target.ShortDescription);
            Assert.AreEqual(initialStatusMessage, target.StatusMessage);
            Assert.AreEqual(string.Empty, target.CurrentOperation);
            Assert.AreEqual(-1, target.PercentComplete);
            Assert.IsNull(target.ParentActivityId);
            Assert.AreEqual(state, target.AsyncState);
            Assert.IsFalse(asyncActivityService.Contains(target));
        }

        [TestMethod("InvokeAsync<string, int>(string activityDescription, string initialStatusMessage, Func<IActivityProgress<string>, Task<int>>)"), Priority(1)]
        public void InvokeAsyncTestMethod4()
        {
            IAsyncActivityService asyncActivityService = Hosting.GetAsyncActivityService();
            if (asyncActivityService is null) Assert.Inconclusive("Hosting.GetAsyncActivityService returned null");
            string activityDescription = "Example Activity";
            string initialStatusMessage = "Initial Status Example";
            int expectedResult = 7;
            string state = "State value example";
            using AutoResetEvent resetEvent = new(false);
            Task<int> asyncMethodDelegate(IActivityProgress<string> progress) => Task.Run(() =>
            {
                resetEvent.Set();
                resetEvent.WaitOne();
                progress.Token.ThrowIfCancellationRequested();
                Assert.AreEqual(state, progress.AsyncState);
                return expectedResult;
            });
            IAsyncFunc<IActivityEvent<string>, string, int> target = asyncActivityService.InvokeAsync(activityDescription, initialStatusMessage, state, asyncMethodDelegate);
            Assert.IsNotNull(target);
            Assert.IsTrue(asyncActivityService.Contains(target));
            Assert.AreEqual(activityDescription, target.ShortDescription);
            Assert.AreEqual(initialStatusMessage, target.StatusMessage);
            Assert.AreEqual(string.Empty, target.CurrentOperation);
            Assert.AreEqual(-1, target.PercentComplete);
            Assert.IsNull(target.ParentActivityId);
            Assert.AreEqual(state, target.AsyncState);
            Assert.AreEqual(state, target.Task.AsyncState);
            resetEvent.WaitOne();
            Assert.AreEqual(ActivityStatus.Running, target.StatusValue);
            resetEvent.Set();
            target.Task.Wait();
            Assert.AreEqual(target.Task.Result, expectedResult);
            Assert.AreEqual(ActivityStatus.RanToCompletion, target.StatusValue);
            Assert.AreEqual(activityDescription, target.ShortDescription);
            Assert.AreEqual(initialStatusMessage, target.StatusMessage);
            Assert.AreEqual(string.Empty, target.CurrentOperation);
            Assert.AreEqual(-1, target.PercentComplete);
            Assert.IsNull(target.ParentActivityId);
            Assert.AreEqual(state, target.AsyncState);
            Assert.IsFalse(asyncActivityService.Contains(target));
        }

        [TestMethod("InvokeTimedAsync(string activityDescription, string initialStatusMessage, Func<IActivityProgress, Task>)"), Priority(1)]
        public void InvokeTimedAsyncTestMethod1()
        {
            IAsyncActivityService asyncActivityService = Hosting.GetAsyncActivityService();
            if (asyncActivityService is null) Assert.Inconclusive("Hosting.GetAsyncActivityService returned null");
            string activityDescription = "Example Activity";
            string initialStatusMessage = "Initial Status Example";
            using AutoResetEvent resetEvent = new(false);
            Task asyncMethodDelegate(IActivityProgress progress) => Task.Run(() =>
            {
                resetEvent.Set();
                resetEvent.WaitOne();
                progress.Token.ThrowIfCancellationRequested();
            });
            ITimedAsyncAction<ITimedActivityEvent> target = asyncActivityService.InvokeTimedAsync(activityDescription, initialStatusMessage, asyncMethodDelegate);
            Assert.IsNotNull(target);
            Assert.IsTrue(asyncActivityService.Contains(target));
            Assert.AreEqual(activityDescription, target.ShortDescription);
            Assert.AreEqual(initialStatusMessage, target.StatusMessage);
            Assert.AreEqual(string.Empty, target.CurrentOperation);
            Assert.AreEqual(-1, target.PercentComplete);
            Assert.IsNull(target.ParentActivityId);
            resetEvent.WaitOne();
            Assert.AreEqual(ActivityStatus.Running, target.StatusValue);
            resetEvent.Set();
            target.Task.Wait();
            Assert.AreEqual(ActivityStatus.RanToCompletion, target.StatusValue);
            Assert.AreEqual(activityDescription, target.ShortDescription);
            Assert.AreEqual(initialStatusMessage, target.StatusMessage);
            Assert.AreEqual(string.Empty, target.CurrentOperation);
            Assert.AreEqual(-1, target.PercentComplete);
            Assert.IsNull(target.ParentActivityId);
            Assert.IsFalse(asyncActivityService.Contains(target));
        }

        [TestMethod("InvokeTimedAsync<int>(string activityDescription, string initialStatusMessage, Func<IActivityProgress, Task<int>>)"), Priority(1)]
        public void InvokeTimedAsyncTestMethod2()
        {
            IAsyncActivityService asyncActivityService = Hosting.GetAsyncActivityService();
            if (asyncActivityService is null) Assert.Inconclusive("Hosting.GetAsyncActivityService returned null");
            string activityDescription = "Example Activity";
            string initialStatusMessage = "Initial Status Example";
            int expectedResult = 7;
            using AutoResetEvent resetEvent = new(false);
            Task<int> asyncMethodDelegate(IActivityProgress progress) => Task.Run(() =>
            {
                resetEvent.Set();
                resetEvent.WaitOne();
                progress.Token.ThrowIfCancellationRequested();
                return expectedResult;
            });
            ITimedAsyncFunc<ITimedActivityEvent, int> target = asyncActivityService.InvokeTimedAsync(activityDescription, initialStatusMessage, asyncMethodDelegate);
            Assert.IsNotNull(target);
            Assert.IsTrue(asyncActivityService.Contains(target));
            Assert.AreEqual(activityDescription, target.ShortDescription);
            Assert.AreEqual(initialStatusMessage, target.StatusMessage);
            Assert.AreEqual(string.Empty, target.CurrentOperation);
            Assert.AreEqual(-1, target.PercentComplete);
            Assert.IsNull(target.ParentActivityId);
            resetEvent.WaitOne();
            Assert.AreEqual(ActivityStatus.Running, target.StatusValue);
            resetEvent.Set();
            target.Task.Wait();
            Assert.AreEqual(target.Task.Result, expectedResult);
            Assert.AreEqual(ActivityStatus.RanToCompletion, target.StatusValue);
            Assert.AreEqual(activityDescription, target.ShortDescription);
            Assert.AreEqual(initialStatusMessage, target.StatusMessage);
            Assert.AreEqual(string.Empty, target.CurrentOperation);
            Assert.AreEqual(-1, target.PercentComplete);
            Assert.IsNull(target.ParentActivityId);
            Assert.IsFalse(asyncActivityService.Contains(target));
        }

        [TestMethod("InvokeTimedAsync<string>(string activityDescription, string initialStatusMessage, Func<IActivityProgress<string>, Task>)"), Priority(1)]
        public void InvokeTimedAsyncTestMethod3()
        {
            IAsyncActivityService asyncActivityService = Hosting.GetAsyncActivityService();
            if (asyncActivityService is null) Assert.Inconclusive("Hosting.GetAsyncActivityService returned null");
            string activityDescription = "Example Activity";
            string initialStatusMessage = "Initial Status Example";
            string state = "State value example";
            using AutoResetEvent resetEvent = new(false);
            Task asyncMethodDelegate(IActivityProgress<string> progress) => Task.Run(() =>
            {
                resetEvent.Set();
                resetEvent.WaitOne();
                progress.Token.ThrowIfCancellationRequested();
                Assert.AreEqual(state, progress.AsyncState);
            });
            ITimedAsyncAction<ITimedActivityEvent<string>, string> target = asyncActivityService.InvokeTimedAsync(activityDescription, initialStatusMessage, state, asyncMethodDelegate);
            Assert.IsNotNull(target);
            Assert.IsTrue(asyncActivityService.Contains(target));
            Assert.AreEqual(activityDescription, target.ShortDescription);
            Assert.AreEqual(initialStatusMessage, target.StatusMessage);
            Assert.AreEqual(string.Empty, target.CurrentOperation);
            Assert.AreEqual(-1, target.PercentComplete);
            Assert.IsNull(target.ParentActivityId);
            Assert.AreEqual(state, target.AsyncState);
            Assert.AreEqual(state, target.Task.AsyncState);
            resetEvent.WaitOne();
            Assert.AreEqual(ActivityStatus.Running, target.StatusValue);
            resetEvent.Set();
            target.Task.Wait();
            Assert.AreEqual(ActivityStatus.RanToCompletion, target.StatusValue);
            Assert.AreEqual(activityDescription, target.ShortDescription);
            Assert.AreEqual(initialStatusMessage, target.StatusMessage);
            Assert.AreEqual(string.Empty, target.CurrentOperation);
            Assert.AreEqual(-1, target.PercentComplete);
            Assert.IsNull(target.ParentActivityId);
            Assert.AreEqual(state, target.AsyncState);
            Assert.IsFalse(asyncActivityService.Contains(target));
        }

        [TestMethod("InvokeTimedAsync<string, int>(string activityDescription, string initialStatusMessage, Func<IActivityProgress<string>, Task<int>>)"), Priority(1)]
        public void InvokeTimedAsyncTestMethod4()
        {
            IAsyncActivityService asyncActivityService = Hosting.GetAsyncActivityService();
            if (asyncActivityService is null) Assert.Inconclusive("Hosting.GetAsyncActivityService returned null");
            string activityDescription = "Example Activity";
            string initialStatusMessage = "Initial Status Example";
            int expectedResult = 7;
            string state = "State value example";
            using AutoResetEvent resetEvent = new(false);
            Task<int> asyncMethodDelegate(IActivityProgress<string> progress) => Task.Run(() =>
            {
                resetEvent.Set();
                resetEvent.WaitOne();
                progress.Token.ThrowIfCancellationRequested();
                Assert.AreEqual(state, progress.AsyncState);
                return expectedResult;
            });
            ITimedAsyncFunc<ITimedActivityEvent<string>, string, int> target = asyncActivityService.InvokeTimedAsync(activityDescription, initialStatusMessage, state, asyncMethodDelegate);
            Assert.IsNotNull(target);
            Assert.IsTrue(asyncActivityService.Contains(target));
            Assert.AreEqual(activityDescription, target.ShortDescription);
            Assert.AreEqual(initialStatusMessage, target.StatusMessage);
            Assert.AreEqual(string.Empty, target.CurrentOperation);
            Assert.AreEqual(-1, target.PercentComplete);
            Assert.IsNull(target.ParentActivityId);
            Assert.AreEqual(state, target.AsyncState);
            Assert.AreEqual(state, target.Task.AsyncState);
            resetEvent.WaitOne();
            Assert.AreEqual(ActivityStatus.Running, target.StatusValue);
            resetEvent.Set();
            target.Task.Wait();
            Assert.AreEqual(target.Task.Result, expectedResult);
            Assert.AreEqual(ActivityStatus.RanToCompletion, target.StatusValue);
            Assert.AreEqual(activityDescription, target.ShortDescription);
            Assert.AreEqual(initialStatusMessage, target.StatusMessage);
            Assert.AreEqual(string.Empty, target.CurrentOperation);
            Assert.AreEqual(-1, target.PercentComplete);
            Assert.IsNull(target.ParentActivityId);
            Assert.AreEqual(state, target.AsyncState);
            Assert.IsFalse(asyncActivityService.Contains(target));
        }

        [TestMethod]
        public void SubscribeChildActivityStartTestMethod()
        {
            IAsyncActivityService asyncActivityService = Hosting.GetAsyncActivityService();
            if (asyncActivityService is null) Assert.Inconclusive("Hosting.GetAsyncActivityService returned null");
            string activityDescription1 = "Example Activity #1";
            string initialStatusMessage1 = "Initial Status Example #1";
            using AutoResetEvent resetEvent = new(false);
            Task asyncMethodDelegate1(IActivityProgress progress) => Task.Run(() =>
            {
                resetEvent.Set();
                progress.Token.ThrowIfCancellationRequested();
                resetEvent.WaitOne();
                progress.Token.ThrowIfCancellationRequested();
            });
            IAsyncAction<IActivityEvent> asyncAction1 = asyncActivityService.InvokeAsync(activityDescription1, initialStatusMessage1, asyncMethodDelegate1);
            if (asyncActivityService is null) Assert.Inconclusive("IAsyncActivityService.InvokeAsync returned null");
            IDisposable subscription = ObserverHelper<IAsyncActivity>.SubscribeChildActivityStart(asyncActivityService, out ObserverHelper<IAsyncActivity> observer);
            Assert.IsNotNull(subscription);
            string activityDescription2 = "Example Activity #2";
            string initialStatusMessage2 = "Initial Status Example #2";
            Func<IActivityProgress, Task> asyncMethodDelegate2 = null;
            IAsyncAction<IActivityEvent> asyncAction2 = asyncActivityService.InvokeAsync(activityDescription2, initialStatusMessage2, asyncMethodDelegate2);
            if (asyncActivityService is null) Assert.Inconclusive("IAsyncActivityService.InvokeAsync returned null");
            Assert.Inconclusive("Test not fully implemented");
        }

        [TestMethod]
        public void SubscribeTestMethod()
        {
            IAsyncActivityService asyncActivityService = Hosting.GetAsyncActivityService();
            if (asyncActivityService is null) Assert.Inconclusive("Hosting.GetAsyncActivityService returned null");
            string activityDescription1 = "Example Activity #1";
            string initialStatusMessage1 = "Initial Status Example #1";
            using AutoResetEvent resetEvent = new(false);
            Task asyncMethodDelegate1(IActivityProgress progress) => Task.Run(() =>
            {
                resetEvent.Set();
                progress.Token.ThrowIfCancellationRequested();
                resetEvent.WaitOne();
                progress.Token.ThrowIfCancellationRequested();
            });
            IAsyncAction<IActivityEvent> asyncAction1 = asyncActivityService.InvokeAsync(activityDescription1, initialStatusMessage1, asyncMethodDelegate1);
            if (asyncActivityService is null) Assert.Inconclusive("IAsyncActivityService.InvokeAsync returned null");
            IDisposable subscription = ObserverHelper<bool>.Subscribe(asyncActivityService, out ObserverHelper<bool> observer);
            Assert.IsNotNull(subscription);
            string activityDescription2 = "Example Activity #2";
            string initialStatusMessage2 = "Initial Status Example #2";
            Func<IActivityProgress, Task> asyncMethodDelegate2 = null;
            IAsyncAction<IActivityEvent> asyncAction2 = asyncActivityService.InvokeAsync(activityDescription2, initialStatusMessage2, asyncMethodDelegate2);
            if (asyncActivityService is null) Assert.Inconclusive("IAsyncActivityService.InvokeAsync returned null");
            Assert.Inconclusive("Test not fully implemented");
        }

        enum ObserverStage
        {
            Observing,
            Observed,
            Completed
        }

        class ObserverHelper<T> : IObserver<T>
        {
            internal Queue<(T[] Items, ObserverStage Stage)> ObservingInvocations { get; } = new();

            internal Queue<(T Value, Exception Error, bool OnObservingInvoked)> ObserverInvocations { get; } = new();

            internal Queue<(T Value, Exception Error, bool IsCompleted)> PostCompletionInvocations { get; } = new();

            internal ObserverStage Stage { get; private set; }

            private ObserverHelper() { }

            internal static IDisposable Subscribe(IObservable<T> observable, out ObserverHelper<T> observer)
            {
                observer = new ObserverHelper<T> { Stage = ObserverStage.Observed };
                return observable.Subscribe(observer);
            }

            internal static IDisposable SubscribeChildActivityStart(IAsyncActivitySource asyncActivitySource, out ObserverHelper<IAsyncActivity> observer)
            {
                ObserverHelper<IAsyncActivity> observerHelper = new();
                observer = observerHelper;
                return asyncActivitySource.SubscribeChildActivityStart(observer, items =>
                {
                    Monitor.Enter(observerHelper.ObserverInvocations);
                    try
                    {
                        observerHelper.ObservingInvocations.Enqueue((items, observerHelper.Stage));
                        if (observerHelper.Stage == ObserverStage.Observing)
                            observerHelper.Stage = ObserverStage.Observed;
                    }
                    finally { Monitor.Exit(observerHelper.ObserverInvocations); }
                });
            }

            public void OnCompleted()
            {
                Monitor.Enter(ObserverInvocations);
                try
                {
                    if (Stage == ObserverStage.Completed)
                        PostCompletionInvocations.Enqueue(new(default, null, true));
                    else
                        Stage = ObserverStage.Completed;
                }
                finally { Monitor.Exit(ObserverInvocations); }
            }

            public void OnError(Exception error)
            {
                Monitor.Enter(ObserverInvocations);
                try
                {
                    if (Stage == ObserverStage.Completed)
                        PostCompletionInvocations.Enqueue(new(default, error, false));
                    else
                        ObserverInvocations.Enqueue(new(default, error, Stage == ObserverStage.Observed));
                }
                finally { Monitor.Exit(ObserverInvocations); }
            }

            public void OnNext(T value)
            {
                Monitor.Enter(ObserverInvocations);
                try
                {
                    if (Stage == ObserverStage.Completed)
                        PostCompletionInvocations.Enqueue(new(value, null, false));
                    else
                        ObserverInvocations.Enqueue(new(value, null, Stage == ObserverStage.Observed));
                }
                finally { Monitor.Exit(ObserverInvocations); }
            }
        }
    }
}
