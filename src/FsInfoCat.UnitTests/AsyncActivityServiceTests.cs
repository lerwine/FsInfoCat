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
            using CompletionObserverHelper<IActivityEvent> observer = new();
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
            using CompletionObserverHelper<IActivityEvent> observer = new();
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
            using CompletionObserverHelper<IActivityEvent> observer = new();
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
            using CompletionObserverHelper<IActivityEvent> observer = new();
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
            using CompletionObserverHelper<IActivityEvent> observer = new();
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
            using CompletionObserverHelper<IActivityEvent> observer = new();
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
            using CompletionObserverHelper<IActivityEvent> observer = new();
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
            using CompletionObserverHelper<IActivityEvent> observer = new();
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
            using CompletionObserverHelper<IActivityEvent> observer = new();
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
            string activityDescription1 = "Example Activity #1";
            string initialStatusMessage1 = "Initial Status Example #1";
            using ManualResetEvent runningEvent = new(false);
            using ManualResetEvent okToCompleteEvent = new(false);
            Task asyncMethodDelegate1(IActivityProgress progress) => Task.Run(() =>
            {
                runningEvent.Set();
                progress.Token.ThrowIfCancellationRequested();
                okToCompleteEvent.WaitOne();
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
            using ManualResetEvent runningEvent = new(false);
            using ManualResetEvent okToCompleteEvent = new(false);
            Task asyncMethodDelegate1(IActivityProgress progress) => Task.Run(() =>
            {
                runningEvent.Set();
                progress.Token.ThrowIfCancellationRequested();
                okToCompleteEvent.WaitOne();
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

        class CompletionObserverHelper<T> : IObserver<T>, IDisposable
        {
            private readonly object _syncRoot = new();
            private readonly ManualResetEvent _completedEvent = new(false);
            private bool _isDisposed;

            public int InvocationCount { get; private set; }

            public Queue<int> CompletionInvocations { get; } = new();

            public Queue<(Exception Error, int Index)> OnErrorInvocations { get; } = new();

            public Queue<(T[] Items, int Index)> OnObservingInvocations { get; } = new();

            public Queue<(T Value, int Index)> OnNextInvocations { get; } = new();

            public void OnCompleted()
            {
                Monitor.Enter(_syncRoot);
                try
                {
                    CompletionInvocations.Enqueue(InvocationCount++);
                    if (CompletionInvocations.Count > 1)
                        return;
                }
                finally { Monitor.Exit(_syncRoot); }
                _completedEvent.Set();
            } 

            public void OnError(Exception error)
            {
                Monitor.Enter(_syncRoot);
                try { OnErrorInvocations.Enqueue((error, InvocationCount++)); }
                finally { Monitor.Exit(_syncRoot); }
            }

            public void OnNext(T value)
            {
                Monitor.Enter(_syncRoot);
                try { OnNextInvocations.Enqueue((value, InvocationCount++)); }
                finally { Monitor.Exit(_syncRoot); }
            }

            internal void OnObserving(T[] items)
            {
                Monitor.Enter(_syncRoot);
                try { OnObservingInvocations.Enqueue((items, InvocationCount++)); }
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
            // ~CompletionObserverHelper()
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
