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
using System.Collections.ObjectModel;

namespace FsInfoCat.UnitTests
{
    [TestClass]
    public class AsyncActivityServiceTests
    {
        public TestContext TestContext { get; set; }

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
            using ManualResetEvent runningEvent = new(false);
            using ManualResetEvent okToCompleteEvent = new(false);
            using ManualResetEvent completedEvent = new(false);
            Task asyncMethodDelegate(IActivityProgress progress) => Task.Run(() =>
            {
                runningEvent.Set();
                okToCompleteEvent.WaitOne();
                progress.Token.ThrowIfCancellationRequested();
            });
            IAsyncAction<IActivityEvent> target = asyncActivityService.InvokeAsync(activityDescription, initialStatusMessage, asyncMethodDelegate);
            Assert.IsNotNull(target);
            using ObserverHelper<IActivityEvent> observer = new();
            using IDisposable subscription = target.Subscribe(observer);
            Assert.AreEqual(activityDescription, target.ShortDescription);
            Assert.AreEqual(initialStatusMessage, target.StatusMessage);
            Assert.AreEqual(string.Empty, target.CurrentOperation);
            Assert.AreEqual(-1, target.PercentComplete);
            Assert.IsNull(target.ParentActivityId);
            Assert.IsTrue(runningEvent.WaitOne(1000));
            Assert.IsTrue(asyncActivityService.Contains(target));
            Assert.AreEqual(ActivityStatus.Running, target.StatusValue);
            okToCompleteEvent.Set();
            Assert.IsTrue(target.Task.Wait(1000));
            Assert.AreEqual(ActivityStatus.RanToCompletion, target.StatusValue);
            Assert.AreEqual(activityDescription, target.ShortDescription);
            Assert.AreEqual(initialStatusMessage, target.StatusMessage);
            Assert.AreEqual(string.Empty, target.CurrentOperation);
            Assert.AreEqual(-1, target.PercentComplete);
            Assert.IsNull(target.ParentActivityId);
            Assert.IsTrue(observer.WaitOne(1000));
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
            using ManualResetEvent runningEvent = new(false);
            using ManualResetEvent okToCompleteEvent = new(false);
            Task<int> asyncMethodDelegate(IActivityProgress progress) => Task.Run(() =>
            {
                runningEvent.Set();
                okToCompleteEvent.WaitOne();
                progress.Token.ThrowIfCancellationRequested();
                return expectedResult;
            });
            IAsyncFunc<IActivityEvent, int> target = asyncActivityService.InvokeAsync(activityDescription, initialStatusMessage, asyncMethodDelegate);
            Assert.IsNotNull(target);
            using ObserverHelper<IActivityEvent> observer = new();
            using IDisposable subscription = target.Subscribe(observer);
            Assert.IsTrue(asyncActivityService.Contains(target));
            Assert.AreEqual(activityDescription, target.ShortDescription);
            Assert.AreEqual(initialStatusMessage, target.StatusMessage);
            Assert.AreEqual(string.Empty, target.CurrentOperation);
            Assert.AreEqual(-1, target.PercentComplete);
            Assert.IsNull(target.ParentActivityId);
            Assert.IsTrue(runningEvent.WaitOne(1000));
            Assert.AreEqual(ActivityStatus.Running, target.StatusValue);
            okToCompleteEvent.Set();
            Assert.IsTrue(target.Task.Wait(1000));
            Assert.AreEqual(target.Task.Result, expectedResult);
            Assert.AreEqual(ActivityStatus.RanToCompletion, target.StatusValue);
            Assert.AreEqual(activityDescription, target.ShortDescription);
            Assert.AreEqual(initialStatusMessage, target.StatusMessage);
            Assert.AreEqual(string.Empty, target.CurrentOperation);
            Assert.AreEqual(-1, target.PercentComplete);
            Assert.IsNull(target.ParentActivityId);
            Assert.IsTrue(observer.WaitOne(1000));
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
            using ManualResetEvent runningEvent = new(false);
            using ManualResetEvent okToCompleteEvent = new(false);
            Task asyncMethodDelegate(IActivityProgress<string> progress) => Task.Run(() =>
            {
                runningEvent.Set();
                okToCompleteEvent.WaitOne();
                progress.Token.ThrowIfCancellationRequested();
                Assert.AreEqual(state, progress.AsyncState);
            });
            IAsyncAction<IActivityEvent<string>, string> target = asyncActivityService.InvokeAsync(activityDescription, initialStatusMessage, state, asyncMethodDelegate);
            Assert.IsNotNull(target);
            using ObserverHelper<IActivityEvent> observer = new();
            using IDisposable subscription = target.Subscribe(observer);
            Assert.IsTrue(asyncActivityService.Contains(target));
            Assert.AreEqual(activityDescription, target.ShortDescription);
            Assert.AreEqual(initialStatusMessage, target.StatusMessage);
            Assert.AreEqual(string.Empty, target.CurrentOperation);
            Assert.AreEqual(-1, target.PercentComplete);
            Assert.IsNull(target.ParentActivityId);
            Assert.AreEqual(state, target.AsyncState);
            Assert.AreEqual(state, target.Task.AsyncState);
            Assert.IsTrue(runningEvent.WaitOne(1000));
            Assert.AreEqual(ActivityStatus.Running, target.StatusValue);
            okToCompleteEvent.Set();
            Assert.IsTrue(target.Task.Wait(1000));
            Assert.AreEqual(ActivityStatus.RanToCompletion, target.StatusValue);
            Assert.AreEqual(activityDescription, target.ShortDescription);
            Assert.AreEqual(initialStatusMessage, target.StatusMessage);
            Assert.AreEqual(string.Empty, target.CurrentOperation);
            Assert.AreEqual(-1, target.PercentComplete);
            Assert.IsNull(target.ParentActivityId);
            Assert.AreEqual(state, target.AsyncState);
            Assert.IsTrue(observer.WaitOne(1000));
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
            using ManualResetEvent runningEvent = new(false);
            using ManualResetEvent okToCompleteEvent = new(false);
            Task<int> asyncMethodDelegate(IActivityProgress<string> progress) => Task.Run(() =>
            {
                runningEvent.Set();
                okToCompleteEvent.WaitOne();
                progress.Token.ThrowIfCancellationRequested();
                Assert.AreEqual(state, progress.AsyncState);
                return expectedResult;
            });
            IAsyncFunc<IActivityEvent<string>, string, int> target = asyncActivityService.InvokeAsync(activityDescription, initialStatusMessage, state, asyncMethodDelegate);
            Assert.IsNotNull(target);
            using ObserverHelper<IActivityEvent> observer = new();
            using IDisposable subscription = target.Subscribe(observer);
            Assert.IsTrue(asyncActivityService.Contains(target));
            Assert.AreEqual(activityDescription, target.ShortDescription);
            Assert.AreEqual(initialStatusMessage, target.StatusMessage);
            Assert.AreEqual(string.Empty, target.CurrentOperation);
            Assert.AreEqual(-1, target.PercentComplete);
            Assert.IsNull(target.ParentActivityId);
            Assert.AreEqual(state, target.AsyncState);
            Assert.AreEqual(state, target.Task.AsyncState);
            Assert.IsTrue(runningEvent.WaitOne(1000));
            Assert.AreEqual(ActivityStatus.Running, target.StatusValue);
            okToCompleteEvent.Set();
            Assert.IsTrue(target.Task.Wait(1000));
            Assert.AreEqual(target.Task.Result, expectedResult);
            Assert.AreEqual(ActivityStatus.RanToCompletion, target.StatusValue);
            Assert.AreEqual(activityDescription, target.ShortDescription);
            Assert.AreEqual(initialStatusMessage, target.StatusMessage);
            Assert.AreEqual(string.Empty, target.CurrentOperation);
            Assert.AreEqual(-1, target.PercentComplete);
            Assert.IsNull(target.ParentActivityId);
            Assert.AreEqual(state, target.AsyncState);
            Assert.IsTrue(observer.WaitOne(1000));
            Assert.IsFalse(asyncActivityService.Contains(target));
        }

        [TestMethod("InvokeTimedAsync(string activityDescription, string initialStatusMessage, Func<IActivityProgress, Task>)"), Priority(1)]
        public void InvokeTimedAsyncTestMethod1()
        {
            IAsyncActivityService asyncActivityService = Hosting.GetAsyncActivityService();
            if (asyncActivityService is null) Assert.Inconclusive("Hosting.GetAsyncActivityService returned null");
            string activityDescription = "Example Activity";
            string initialStatusMessage = "Initial Status Example";
            using ManualResetEvent runningEvent = new(false);
            using ManualResetEvent okToCompleteEvent = new(false);
            Task asyncMethodDelegate(IActivityProgress progress) => Task.Run(() =>
            {
                Thread.Sleep(10);
                runningEvent.Set();
                okToCompleteEvent.WaitOne();
                progress.Token.ThrowIfCancellationRequested();
            });
            ITimedAsyncAction<ITimedActivityEvent> target = asyncActivityService.InvokeTimedAsync(activityDescription, initialStatusMessage, asyncMethodDelegate);
            Assert.IsNotNull(target);
            using ObserverHelper<IActivityEvent> observer = new();
            using IDisposable subscription = target.Subscribe(observer);
            Assert.IsTrue(asyncActivityService.Contains(target));
            Assert.AreEqual(activityDescription, target.ShortDescription);
            Assert.AreEqual(initialStatusMessage, target.StatusMessage);
            Assert.AreEqual(string.Empty, target.CurrentOperation);
            Assert.AreEqual(-1, target.PercentComplete);
            Assert.IsNull(target.ParentActivityId);
            Assert.IsTrue(runningEvent.WaitOne(1000));
            Assert.AreEqual(ActivityStatus.Running, target.StatusValue);
            okToCompleteEvent.Set();
            Assert.IsTrue(target.Task.Wait(1000));
            TimeSpan duration = target.Duration;
            Assert.AreNotEqual(TimeSpan.Zero, duration);
            Assert.AreEqual(ActivityStatus.RanToCompletion, target.StatusValue);
            Assert.AreEqual(activityDescription, target.ShortDescription);
            Assert.AreEqual(initialStatusMessage, target.StatusMessage);
            Assert.AreEqual(string.Empty, target.CurrentOperation);
            Assert.AreEqual(-1, target.PercentComplete);
            Assert.IsNull(target.ParentActivityId);
            Assert.IsTrue(observer.WaitOne(1000));
            Assert.IsFalse(asyncActivityService.Contains(target));
            Thread.Sleep(100);
            Assert.AreEqual(duration, target.Duration);
        }

        [TestMethod("InvokeTimedAsync<int>(string activityDescription, string initialStatusMessage, Func<IActivityProgress, Task<int>>)"), Priority(1)]
        public void InvokeTimedAsyncTestMethod2()
        {
            IAsyncActivityService asyncActivityService = Hosting.GetAsyncActivityService();
            if (asyncActivityService is null) Assert.Inconclusive("Hosting.GetAsyncActivityService returned null");
            string activityDescription = "Example Activity";
            string initialStatusMessage = "Initial Status Example";
            int expectedResult = 7;
            using ManualResetEvent runningEvent = new(false);
            using ManualResetEvent okToCompleteEvent = new(false);
            Task<int> asyncMethodDelegate(IActivityProgress progress) => Task.Run(() =>
            {
                Thread.Sleep(10);
                runningEvent.Set();
                okToCompleteEvent.WaitOne();
                progress.Token.ThrowIfCancellationRequested();
                return expectedResult;
            });
            ITimedAsyncFunc<ITimedActivityEvent, int> target = asyncActivityService.InvokeTimedAsync(activityDescription, initialStatusMessage, asyncMethodDelegate);
            Assert.IsNotNull(target);
            using ObserverHelper<IActivityEvent> observer = new();
            using IDisposable subscription = target.Subscribe(observer);
            Assert.IsTrue(asyncActivityService.Contains(target));
            Assert.AreEqual(activityDescription, target.ShortDescription);
            Assert.AreEqual(initialStatusMessage, target.StatusMessage);
            Assert.AreEqual(string.Empty, target.CurrentOperation);
            Assert.AreEqual(-1, target.PercentComplete);
            Assert.IsNull(target.ParentActivityId);
            Assert.IsTrue(runningEvent.WaitOne(1000));
            Assert.AreEqual(ActivityStatus.Running, target.StatusValue);
            okToCompleteEvent.Set();
            Assert.IsTrue(target.Task.Wait(1000));
            TimeSpan duration = target.Duration;
            Assert.AreNotEqual(TimeSpan.Zero, duration);
            Assert.AreEqual(target.Task.Result, expectedResult);
            Assert.AreEqual(ActivityStatus.RanToCompletion, target.StatusValue);
            Assert.AreEqual(activityDescription, target.ShortDescription);
            Assert.AreEqual(initialStatusMessage, target.StatusMessage);
            Assert.AreEqual(string.Empty, target.CurrentOperation);
            Assert.AreEqual(-1, target.PercentComplete);
            Assert.IsNull(target.ParentActivityId);
            Assert.IsTrue(observer.WaitOne(1000));
            Assert.IsFalse(asyncActivityService.Contains(target));
            Thread.Sleep(100);
            Assert.AreEqual(duration, target.Duration);
        }

        [TestMethod("InvokeTimedAsync<string>(string activityDescription, string initialStatusMessage, Func<IActivityProgress<string>, Task>)"), Priority(1)]
        public void InvokeTimedAsyncTestMethod3()
        {
            IAsyncActivityService asyncActivityService = Hosting.GetAsyncActivityService();
            if (asyncActivityService is null) Assert.Inconclusive("Hosting.GetAsyncActivityService returned null");
            string activityDescription = "Example Activity";
            string initialStatusMessage = "Initial Status Example";
            string state = "State value example";
            using ManualResetEvent runningEvent = new(false);
            using ManualResetEvent okToCompleteEvent = new(false);
            Task asyncMethodDelegate(IActivityProgress<string> progress) => Task.Run(() =>
            {
                Thread.Sleep(10);
                runningEvent.Set();
                okToCompleteEvent.WaitOne();
                progress.Token.ThrowIfCancellationRequested();
                Assert.AreEqual(state, progress.AsyncState);
            });
            ITimedAsyncAction<ITimedActivityEvent<string>, string> target = asyncActivityService.InvokeTimedAsync(activityDescription, initialStatusMessage, state, asyncMethodDelegate);
            Assert.IsNotNull(target);
            using ObserverHelper<IActivityEvent> observer = new();
            using IDisposable subscription = target.Subscribe(observer);
            Assert.IsTrue(asyncActivityService.Contains(target));
            Assert.AreEqual(activityDescription, target.ShortDescription);
            Assert.AreEqual(initialStatusMessage, target.StatusMessage);
            Assert.AreEqual(string.Empty, target.CurrentOperation);
            Assert.AreEqual(-1, target.PercentComplete);
            Assert.IsNull(target.ParentActivityId);
            Assert.AreEqual(state, target.AsyncState);
            Assert.AreEqual(state, target.Task.AsyncState);
            Assert.IsTrue(runningEvent.WaitOne(1000));
            Assert.AreEqual(ActivityStatus.Running, target.StatusValue);
            okToCompleteEvent.Set();
            Assert.IsTrue(target.Task.Wait(1000));
            TimeSpan duration = target.Duration;
            Assert.AreNotEqual(TimeSpan.Zero, duration);
            Assert.AreEqual(ActivityStatus.RanToCompletion, target.StatusValue);
            Assert.AreEqual(activityDescription, target.ShortDescription);
            Assert.AreEqual(initialStatusMessage, target.StatusMessage);
            Assert.AreEqual(string.Empty, target.CurrentOperation);
            Assert.AreEqual(-1, target.PercentComplete);
            Assert.IsNull(target.ParentActivityId);
            Assert.AreEqual(state, target.AsyncState);
            Assert.IsTrue(observer.WaitOne(1000));
            Assert.IsFalse(asyncActivityService.Contains(target));
            Thread.Sleep(100);
            Assert.AreEqual(duration, target.Duration);
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
            using ManualResetEvent runningEvent = new(false);
            using ManualResetEvent okToCompleteEvent = new(false);
            Task<int> asyncMethodDelegate(IActivityProgress<string> progress) => Task.Run(() =>
            {
                Thread.Sleep(10);
                runningEvent.Set();
                okToCompleteEvent.WaitOne();
                progress.Token.ThrowIfCancellationRequested();
                Assert.AreEqual(state, progress.AsyncState);
                return expectedResult;
            });
            ITimedAsyncFunc<ITimedActivityEvent<string>, string, int> target = asyncActivityService.InvokeTimedAsync(activityDescription, initialStatusMessage, state, asyncMethodDelegate);
            Assert.IsNotNull(target);
            using ObserverHelper<IActivityEvent> observer = new();
            using IDisposable subscription = target.Subscribe(observer);
            Assert.IsTrue(asyncActivityService.Contains(target));
            Assert.AreEqual(activityDescription, target.ShortDescription);
            Assert.AreEqual(initialStatusMessage, target.StatusMessage);
            Assert.AreEqual(string.Empty, target.CurrentOperation);
            Assert.AreEqual(-1, target.PercentComplete);
            Assert.IsNull(target.ParentActivityId);
            Assert.AreEqual(state, target.AsyncState);
            Assert.AreEqual(state, target.Task.AsyncState);
            Assert.IsTrue(runningEvent.WaitOne(1000));
            Assert.AreEqual(ActivityStatus.Running, target.StatusValue);
            okToCompleteEvent.Set();
            Assert.IsTrue(target.Task.Wait(1000));
            TimeSpan duration = target.Duration;
            Assert.AreNotEqual(TimeSpan.Zero, duration);
            Assert.AreEqual(target.Task.Result, expectedResult);
            Assert.AreEqual(ActivityStatus.RanToCompletion, target.StatusValue);
            Assert.AreEqual(activityDescription, target.ShortDescription);
            Assert.AreEqual(initialStatusMessage, target.StatusMessage);
            Assert.AreEqual(string.Empty, target.CurrentOperation);
            Assert.AreEqual(-1, target.PercentComplete);
            Assert.IsNull(target.ParentActivityId);
            Assert.AreEqual(state, target.AsyncState);
            Assert.IsTrue(observer.WaitOne(1000));
            Assert.IsFalse(asyncActivityService.Contains(target));
            Thread.Sleep(100);
            Assert.AreEqual(duration, target.Duration);
        }

        [TestMethod, Priority(2)]
        public void ActivityProgressTestMethod()
        {
            IAsyncActivityService asyncActivityService = Hosting.GetAsyncActivityService();
            if (asyncActivityService is null) Assert.Inconclusive("Hosting.GetAsyncActivityService returned null");
            string activityDescription = "Example Activity";
            string initialStatusMessage = "Initial Status Example";
            string firstOperation = "First Operation Example";
            string progressStatusMessage = "Progress Status Example";
            int firstPercentComplete = 10;
            string nextOperation = "Next Operation Example";
            int nextPercentComplete = 50;
            int finalPercentComplete = 98;
            string finalStatusMessage = "Example Final Status";
            using AutoResetEvent proceedEvent = new(false);
            using AutoResetEvent confirmEvent = new(false);
            Task asyncMethodDelegate(IActivityProgress progress) => Task.Run(() =>
            {
                Assert.AreEqual(activityDescription, progress.ShortDescription);
                Assert.AreEqual(initialStatusMessage, progress.StatusMessage);
                Assert.AreEqual(string.Empty, progress.CurrentOperation);
                Assert.AreEqual(-1, progress.PercentComplete);
                confirmEvent.Set(); // Signal that task running

                proceedEvent.WaitOne(); // Wait till it's okay to set current operation
                progress.Token.ThrowIfCancellationRequested();
                progress.ReportCurrentOperation(firstOperation);
                Assert.AreEqual(initialStatusMessage, progress.StatusMessage);
                Assert.AreEqual(firstOperation, progress.CurrentOperation);
                Assert.AreEqual(-1, progress.PercentComplete);
                confirmEvent.Set(); // Signal that current operation was set

                proceedEvent.WaitOne(); // Wait till it's okay to set percent complete
                progress.Token.ThrowIfCancellationRequested();
                progress.Report(firstPercentComplete);
                Assert.AreEqual(initialStatusMessage, progress.StatusMessage);
                Assert.AreEqual(firstOperation, progress.CurrentOperation);
                Assert.AreEqual(firstPercentComplete, progress.PercentComplete);
                confirmEvent.Set(); // Signal that percent complete was set

                proceedEvent.WaitOne(); // Wait till it's okay to set progress message
                progress.Token.ThrowIfCancellationRequested();
                progress.Report(progressStatusMessage);
                Assert.AreEqual(progressStatusMessage, progress.StatusMessage);
                Assert.AreEqual(string.Empty, progress.CurrentOperation);
                Assert.AreEqual(firstPercentComplete, progress.PercentComplete);
                confirmEvent.Set(); // Signal that progress message was set.

                proceedEvent.WaitOne(); // Wait till it's okay to set next operation
                progress.Token.ThrowIfCancellationRequested();
                progress.ReportCurrentOperation(nextOperation, nextPercentComplete);
                Assert.AreEqual(progressStatusMessage, progress.StatusMessage);
                Assert.AreEqual(nextOperation, progress.CurrentOperation);
                Assert.AreEqual(nextPercentComplete, progress.PercentComplete);
                confirmEvent.Set(); // Signal that next operation was set.

                proceedEvent.WaitOne(); // Wait till it's okay to set final message
                progress.Token.ThrowIfCancellationRequested();
                progress.Report(finalStatusMessage);
                Assert.AreEqual(finalStatusMessage, progress.StatusMessage);
                Assert.AreEqual(string.Empty, progress.CurrentOperation);
                Assert.AreEqual(nextPercentComplete, progress.PercentComplete);
                confirmEvent.Set(); // Signal that final message was set.

                proceedEvent.WaitOne(); // Wait till it's okay to complete task
                progress.Report(finalPercentComplete);
            });

            IAsyncAction<IActivityEvent> target = asyncActivityService.InvokeAsync(activityDescription, initialStatusMessage, asyncMethodDelegate);
            Assert.IsNotNull(target);
            using ObserverHelper<IActivityEvent> observer = new();
            using IDisposable subscription = target.Subscribe(observer);
            Assert.AreEqual(activityDescription, target.ShortDescription);
            Assert.AreEqual(initialStatusMessage, target.StatusMessage);
            Assert.AreEqual(string.Empty, target.CurrentOperation);
            Assert.AreEqual(-1, target.PercentComplete);
            Assert.IsNull(target.ParentActivityId);
            Assert.IsTrue(confirmEvent.WaitOne(1000)); // Wait till task is running
            Assert.IsTrue(asyncActivityService.Contains(target));
            Assert.AreEqual(ActivityStatus.Running, target.StatusValue);

            proceedEvent.Set(); // Signal to set current operation and wait until it's set
            Assert.IsTrue(confirmEvent.WaitOne(1000));
            Assert.AreEqual(initialStatusMessage, target.StatusMessage);
            Assert.AreEqual(firstOperation, target.CurrentOperation);
            Assert.AreEqual(-1, target.PercentComplete);

            proceedEvent.Set(); // Signal to set percent complete and wait until it's set
            Assert.IsTrue(confirmEvent.WaitOne(1000));
            Assert.AreEqual(initialStatusMessage, target.StatusMessage);
            Assert.AreEqual(firstOperation, target.CurrentOperation);
            Assert.AreEqual(firstPercentComplete, target.PercentComplete);

            proceedEvent.Set(); // Signal to set progress message and wait until it's set.
            Assert.IsTrue(confirmEvent.WaitOne(1000));
            Assert.AreEqual(activityDescription, target.ShortDescription);
            Assert.AreEqual(progressStatusMessage, target.StatusMessage);
            Assert.AreEqual(string.Empty, target.CurrentOperation);
            Assert.AreEqual(firstPercentComplete, target.PercentComplete);

            proceedEvent.Set(); // Signal to set next operation and wait until it's set.
            Assert.IsTrue(confirmEvent.WaitOne(1000));
            Assert.AreEqual(activityDescription, target.ShortDescription);
            Assert.AreEqual(progressStatusMessage, target.StatusMessage);
            Assert.AreEqual(nextOperation, target.CurrentOperation);
            Assert.AreEqual(nextPercentComplete, target.PercentComplete);

            proceedEvent.Set(); // Signal to set final message and wait until it's set.
            Assert.IsTrue(confirmEvent.WaitOne(1000));
            Assert.AreEqual(activityDescription, target.ShortDescription);
            Assert.AreEqual(finalStatusMessage, target.StatusMessage);
            Assert.AreEqual(string.Empty, target.CurrentOperation);
            Assert.AreEqual(nextPercentComplete, target.PercentComplete);

            proceedEvent.Set(); // Signal to complete task and wait for completion.
            target.Task.Wait();
            Assert.IsTrue(observer.WaitOne(1000));
            Assert.AreEqual(ActivityStatus.RanToCompletion, target.StatusValue);
            Assert.AreEqual(activityDescription, target.ShortDescription);
            Assert.AreEqual(finalStatusMessage, target.StatusMessage);
            Assert.AreEqual(string.Empty, target.CurrentOperation);
            Assert.AreEqual(finalPercentComplete, target.PercentComplete);
            Assert.IsNull(target.ParentActivityId);
            Assert.IsFalse(asyncActivityService.Contains(target));
        }

        [TestMethod]
        public void SubscribeChildActivityStartTestMethod()
        {
            IAsyncActivityService asyncActivityService = Hosting.GetAsyncActivityService();
            if (asyncActivityService is null) Assert.Inconclusive("Hosting.GetAsyncActivityService returned null");
            using ManualResetEvent runningEvent1 = new(false);
            using ManualResetEvent okToCompleteEvent1 = new(false);
            Task asyncMethodDelegate1(IActivityProgress progress) => Task.Run(() =>
            {
                runningEvent1.Set();
                progress.Token.ThrowIfCancellationRequested();
                okToCompleteEvent1.WaitOne();
                progress.Token.ThrowIfCancellationRequested();
            });
            IAsyncAction<IActivityEvent> asyncAction1 = asyncActivityService.InvokeAsync("Example Activity #1", "Initial Status Example #1", asyncMethodDelegate1);
            if (asyncAction1 is null) Assert.Inconclusive("IAsyncActivityService.InvokeAsync returned null");
            if (!(runningEvent1.WaitOne(1000))) Assert.Inconclusive("First task not started");
            using ObserverHelper<IAsyncActivity> observer = new();
            using IDisposable subscription = asyncActivityService.SubscribeChildActivityStart(observer, observer.OnObserving);
            Assert.IsNotNull(subscription);
            Assert.AreEqual(1, observer.OnObservingInvocations.Count);
            Assert.IsTrue(observer.OnObservingInvocations[0].Items.Any(t => ReferenceEquals(t, asyncAction1)));
;
            using ManualResetEvent runningEvent2 = new(false);
            using ManualResetEvent okToCompleteEvent2 = new(false);
            Task asyncMethodDelegate2(IActivityProgress progress) => Task.Run(() =>
            {
                runningEvent2.Set();
                progress.Token.ThrowIfCancellationRequested();
                okToCompleteEvent2.WaitOne();
                progress.Token.ThrowIfCancellationRequested();
            });
            IAsyncAction<IActivityEvent> asyncAction2 = asyncActivityService.InvokeAsync("Example Activity #2", "Initial Status Example #2", asyncMethodDelegate2);
            if (asyncAction2 is null) Assert.Inconclusive("IAsyncActivityService.InvokeAsync returned null");
            if (!(runningEvent2.WaitOne(1000))) Assert.Inconclusive("Second task not started");
            Assert.IsFalse(observer.OnNextInvocations.Any(t => ReferenceEquals(t.Value, asyncAction1)));
            Assert.IsTrue(observer.OnNextInvocations.Any(t => ReferenceEquals(t.Value, asyncAction2)));

            using ObserverHelper<IActivityEvent> completion1 = new();
            using IDisposable cs1 = asyncAction1.Subscribe(completion1);
            if (cs1 is null) Assert.Inconclusive("Subscribe returned null");
            using ObserverHelper<IActivityEvent> completion2 = new();
            using IDisposable cs2 = asyncAction2.Subscribe(completion2);
            if (cs2 is null) Assert.Inconclusive("Subscribe returned null");

            observer.Reset();
            okToCompleteEvent1.Set();
            if (!completion1.WaitOne(1000)) Assert.Inconclusive("First task did not complete");
            Assert.AreEqual(0, observer.InvocationCount);

            okToCompleteEvent2.Set();
            if (!completion2.WaitOne(1000)) Assert.Inconclusive("Second task did not complete");
            Assert.AreEqual(0, observer.InvocationCount);
        }

        enum ObserverStage
        {
            Observing,
            Observed,
            Completed
        }

        class ObserverHelper<T> : IObserver<T>, IDisposable
        {
            private readonly object _syncRoot = new();
            private readonly ManualResetEvent _completedEvent = new(false);
            private bool _isDisposed;

            public int InvocationCount { get; private set; }

            public Collection<int> CompletionInvocations { get; } = new();

            public Collection<(Exception Error, int Index)> OnErrorInvocations { get; } = new();

            public Collection<(T[] Items, int Index)> OnObservingInvocations { get; } = new();

            public Collection<(T Value, int Index)> OnNextInvocations { get; } = new();

            internal IDisposable Subscribe<V>(IObservable<T> observable, Func<V> onSubscribe, out V result)
            {
                Monitor.Enter(_syncRoot);
                try
                {
                    result = onSubscribe();
                    return observable.Subscribe(this);
                }
                finally { Monitor.Exit(_syncRoot); }
            }

            internal void Reset()
            {
                Monitor.Enter(_syncRoot);
                try
                {
                    InvocationCount = 0;
                    CompletionInvocations.Clear();
                    OnErrorInvocations.Clear();
                    OnObservingInvocations.Clear();
                    OnNextInvocations.Clear();
                    _completedEvent.Reset();
                }
                finally { Monitor.Exit(_syncRoot); }
            }

            internal void Reset(out T[] values)
            {
                Monitor.Enter(_syncRoot);
                try
                {
                    InvocationCount = 0;
                    values = OnNextInvocations.Select(t => t.Value).ToArray();
                    CompletionInvocations.Clear();
                    OnErrorInvocations.Clear();
                    OnObservingInvocations.Clear();
                    OnNextInvocations.Clear();
                    _completedEvent.Reset();
                }
                finally { Monitor.Exit(_syncRoot); }
            }

            public void OnCompleted()
            {
                Monitor.Enter(_syncRoot);
                try
                {
                    CompletionInvocations.Add(InvocationCount++);
                    if (CompletionInvocations.Count > 1)
                        return;
                }
                finally { Monitor.Exit(_syncRoot); }
                _completedEvent.Set();
            }

            public void OnError(Exception error)
            {
                Monitor.Enter(_syncRoot);
                try { OnErrorInvocations.Add((error, InvocationCount++)); }
                finally { Monitor.Exit(_syncRoot); }
            }

            public void OnNext(T value)
            {
                Monitor.Enter(_syncRoot);
                try { OnNextInvocations.Add((value, InvocationCount++)); }
                finally { Monitor.Exit(_syncRoot); }
            }

            internal void OnObserving(T[] items)
            {
                Monitor.Enter(_syncRoot);
                try { OnObservingInvocations.Add((items, InvocationCount++)); }
                finally { Monitor.Exit(_syncRoot); }
            }

            internal bool WaitOne() => _completedEvent.WaitOne();

            internal bool WaitOne(int millisecondsTimeout) => _completedEvent.WaitOne(millisecondsTimeout);

            internal bool WaitOne(int millisecondsTimeout, bool exitContext) => _completedEvent.WaitOne(millisecondsTimeout, exitContext);

            internal bool WaitOne(TimeSpan timeout) => _completedEvent.WaitOne(timeout);

            internal bool WaitOne(TimeSpan timeout, bool exitContext) => _completedEvent.WaitOne(timeout, exitContext);

            protected virtual void Dispose(bool disposing)
            {
                if (!_isDisposed)
                {
                    if (disposing)
                        _completedEvent.Dispose();

                    // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                    // TODO: set large fields to null
                    _isDisposed = true;
                }
            }

            // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
            // ~ObserverHelper()
            // {
            //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            //     Dispose(disposing: false);
            // }

            public void Dispose()
            {
                // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }
        }
    }
}
