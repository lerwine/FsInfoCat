using FsInfoCat.AsyncOps;
using FsInfoCat.Activities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.UnitTests
{
    [TestClass]
    public class BackgroundProgressServiceTests
    {
#pragma warning disable IDE0052 // Remove unread private members
        private static TestContext _testContext;
#pragma warning restore IDE0052 // Remove unread private members

        [ClassInitialize]
        public static void OnClassInitialize(TestContext testContext)
        {
            _testContext = testContext;
        }

        private static T GetRandomEnum<T>(Random random) where T : struct, Enum
        {
            T[] values = Enum.GetValues<T>();
            return values[random.Next(0, values.Length)];
        }

        public static IEnumerable<object[]> GetAsyncActionCompletedStateData()
        {
            Random random = new();
            yield return new object[]
            {
                new InitializeStateData("TestActivity", "Starting", random.Next(1, int.MaxValue)),
                new UpdateData("Working", "Item #1", GetRandomEnum<MessageCode>(random), (byte)random.Next(1, 99)),
                new CompleteData("Finished", GetRandomEnum<MessageCode>(random), false)
            };
            yield return new object[]
            {
                new InitializeStateData("TestActivity", "Starting", random.Next(1, int.MaxValue)),
                new UpdateData("Working", "Item #1", GetRandomEnum<MessageCode>(random), (byte)random.Next(1, 99)),
                new CompleteData("Finished", GetRandomEnum<MessageCode>(random), true)
            };
            yield return new object[]
            {
                new InitializeStateData("TestActivity", "Starting", random.Next(1, int.MaxValue)),
                new UpdateData("Working", "Item #1", GetRandomEnum<MessageCode>(random), (byte)random.Next(1, 99)),
                new ErrorCompleteData { StatusDescription = "Something went wrong", Code = GetRandomEnum<ErrorCode>(random) }
            };
        }

        public static IEnumerable<object[]> GetAsyncActionCompletedData()
        {
            Random random = new();
            yield return new object[]
            {
                new InitializeData("TestActivity", "Working"),
                new UpdateData(null, "First Item", GetRandomEnum<MessageCode>(random), (byte)random.Next(1, 99)),
                new CompleteData("Done", GetRandomEnum<MessageCode>(random), false)
            };
            yield return new object[]
            {
                new InitializeData("TestActivity", "Working"),
                new UpdateData(null, "First Item", GetRandomEnum<MessageCode>(random), (byte)random.Next(1, 99)),
                new CompleteData("Done", GetRandomEnum<MessageCode>(random), true)
            };
            yield return new object[]
            {
                new InitializeData("TestActivity", "Working"),
                new UpdateData(null, "First Item", GetRandomEnum<MessageCode>(random), (byte)random.Next(1, 99)),
                new ErrorCompleteData { StatusDescription = "Something went wrong", Code = GetRandomEnum<ErrorCode>(random) }
            };
        }

        public static IEnumerable<object[]> GetAsyncFuncResultStateData()
        {
            Random random = new();
            yield return new object[]
            {
                new InitializeStateData("TestActivity", "Starting", random.Next(1, int.MaxValue)),
                new UpdateData("Working", "", GetRandomEnum<MessageCode>(random), (byte)random.Next(1, 99)),
                new ResultData("Finished", GetRandomEnum<MessageCode>(random), false, Math.Round(random.NextDouble() * int.MaxValue, 2))
            };
            yield return new object[]
            {
                new InitializeStateData("TestActivity", "Starting", random.Next(1, int.MaxValue)),
                new UpdateData("Working", "", null, null),
                new ResultData("Finished", null, true, Math.Round(random.NextDouble() * int.MaxValue, 2))
            };
            yield return new object[]
            {
                new InitializeStateData("TestActivity", "Starting", random.Next(1, int.MaxValue)),
                new UpdateData("Working", "", GetRandomEnum<MessageCode>(random), (byte)random.Next(1, 99)),
                new ErrorResultData { StatusDescription = "Uh oh", Code = GetRandomEnum<ErrorCode>(random) }
            };
        }

        public static IEnumerable<object[]> GetAsyncFuncResultData()
        {
            Random random = new();
            yield return new object[]
            {
                new InitializeData("TestActivity", "Working"),
                new UpdateData("Working", "Item #2", GetRandomEnum<MessageCode>(random), (byte)random.Next(1, 99)),
                new ResultData("Done", GetRandomEnum<MessageCode>(random), false,  Math.Round(random.NextDouble() * int.MaxValue, 2))
            };
            yield return new object[]
            {
                new InitializeData("TestActivity", "Working"),
                new UpdateData("Working", "Item #2", GetRandomEnum<MessageCode>(random), null),
                new ResultData("Done", null, true,  Math.Round(random.NextDouble() * int.MaxValue, 2))
            };
            yield return new object[]
            {
                new InitializeData("TestActivity", "Working"),
                new UpdateData("Working", "Item #2", null, null),
                new ErrorResultData { StatusDescription = "Uh oh", Code = GetRandomEnum<ErrorCode>(random) }
            };
        }

        private static void ReportProgressUpdate<TEvent>(UpdateData updateData, IActivityProgress progress)
        {
            throw new NotImplementedException();
            //if (string.IsNullOrWhiteSpace(updateData.StatusDescription))
            //{
            //    if (updateData.Code.HasValue)
            //    {
            //        if (updateData.PercentComplete.HasValue)
            //            progress.ReportCurrentOperation(updateData.CurrentOperation, updateData.Code.Value, updateData.PercentComplete.Value);
            //        else
            //            progress.ReportCurrentOperation(updateData.CurrentOperation, updateData.Code.Value);
            //    }
            //    else if (updateData.PercentComplete.HasValue)
            //        progress.ReportCurrentOperation(updateData.CurrentOperation, updateData.PercentComplete.Value);
            //    else
            //        progress.ReportCurrentOperation(updateData.CurrentOperation);
            //}
            //else if (string.IsNullOrWhiteSpace(updateData.CurrentOperation))
            //{
            //    if (updateData.Code.HasValue)
            //    {
            //        if (updateData.PercentComplete.HasValue)
            //            progress.ReportStatusDescription(updateData.StatusDescription, updateData.Code.Value, updateData.PercentComplete.Value);
            //        else
            //            progress.ReportStatusDescription(updateData.StatusDescription, updateData.Code.Value);
            //    }
            //    else if (updateData.PercentComplete.HasValue)
            //        progress.ReportStatusDescription(updateData.StatusDescription, updateData.PercentComplete.Value);
            //    else
            //        progress.ReportStatusDescription(updateData.StatusDescription);
            //}
            //else if (updateData.Code.HasValue)
            //{
            //    if (updateData.PercentComplete.HasValue)
            //        progress.ReportStatusDescription(updateData.StatusDescription, updateData.CurrentOperation, updateData.Code.Value, updateData.PercentComplete.Value);
            //    else
            //        progress.ReportStatusDescription(updateData.StatusDescription, updateData.CurrentOperation, updateData.Code.Value);
            //}
            //else if (updateData.PercentComplete.HasValue)
            //    progress.ReportStatusDescription(updateData.StatusDescription, updateData.CurrentOperation, updateData.PercentComplete.Value);
            //else
            //    progress.ReportStatusDescription(updateData.StatusDescription, updateData.CurrentOperation);
        }

        [TestMethod("Hosting.GetBackgroundProgressService()")]
        [Priority(1)]
        public void GetRequiredServiceTest()
        {
            IAsyncActivityService service = Hosting.GetAsyncActivityService();
            Assert.IsNotNull(service);
        }

        [TestMethod()]
        [Priority(10)]
        public async Task IsActiveTest()
        {
            IAsyncActivityService service = Hosting.GetAsyncActivityService();
            if (service is null)
                throw new AssertInconclusiveException();
            Assert.IsNotNull(service);
            Assert.IsFalse(service.IsActive);
            using AutoResetEvent bgEvent = new(false);
            using AutoResetEvent fgEvent = new(false);
            Task<bool> asyncMethodDelegate(IActivityProgress progress) => Task.Run(() =>
            {
                bgEvent.Set();
                fgEvent.WaitOne();
                return true;
            });
            
            ITimedAsyncFunc<ITimedActivityEvent, bool> backgroundOperation = service.InvokeTimedAsync("Test timed func", "Starting", asyncMethodDelegate);
            bgEvent.WaitOne();
            Assert.IsTrue(service.IsActive);
            fgEvent.Set();
            await backgroundOperation.Task;
            Thread.Sleep(100);
            Assert.IsFalse(service.IsActive);
        }

        [TestMethod()]
        [Priority(10)]
        public async Task SubscribeTest()
        {
            IAsyncActivityService service = Hosting.GetAsyncActivityService();
            if (service is null)
                throw new AssertInconclusiveException();
            ObserverHelper<IAsyncActivity> observer1 = new();
            ObserverHelper<IAsyncActivity> observer2 = new();
            IDisposable subscription1 = service.ActivityStartedObservable.Subscribe(observer1);
            Assert.IsNotNull(subscription1);
            using IDisposable subscription2 = service.ActivityStartedObservable.Subscribe(observer2);
            Assert.IsNotNull(subscription2);
            using AutoResetEvent bgEvent = new(false);
            using AutoResetEvent fgEvent = new(false);
            Task<bool> asyncMethodDelegate(IActivityProgress progress) => Task.Run(() =>
            {
                bgEvent.Set();
                fgEvent.WaitOne();
                progress.Report("New Status");
                bgEvent.Set();
                fgEvent.WaitOne();
                return true;
            });
            ITimedAsyncFunc<ITimedActivityEvent, bool> backgroundOperation = service.InvokeTimedAsync("Test timed func", "Starting", asyncMethodDelegate);
            bgEvent.WaitOne();
            Assert.IsTrue(observer1.TryDequeue(out Observed<IAsyncActivity> observed1));
            Assert.IsNull(observed1.Error);
            Assert.IsFalse(observed1.IsComplete);
            Assert.IsTrue(observer2.TryDequeue(out Observed<IAsyncActivity> observed2));
            Assert.IsNull(observed2.Error);
            Assert.IsFalse(observed2.IsComplete);
            Assert.AreSame(observed1.Value, observed2.Value);
            using (subscription1)
            {
                fgEvent.Set();
                bgEvent.WaitOne();
                Assert.IsTrue(observer1.TryDequeue(out observed1));
                Assert.IsNull(observed1.Error);
                Assert.IsFalse(observed1.IsComplete);
                Assert.IsTrue(observer2.TryDequeue(out observed2));
                Assert.IsNull(observed2.Error);
                Assert.IsFalse(observed2.IsComplete);
                Assert.AreSame(observed1.Value, observed2.Value);
            }
            fgEvent.Set();
            await backgroundOperation.Task;
            Thread.Sleep(100);
            Assert.IsFalse(observer1.TryDequeue(out observed1));
            Assert.IsTrue(observer2.TryDequeue(out observed2));
            Assert.IsNull(observed2.Error);
            Assert.IsFalse(observed2.IsComplete);
        }

        private static Task<double> TimedTestFuncState(AutoResetEvent fgEvent, AutoResetEvent bgEvent, InitializeStateData initializeData, UpdateData updateData, IResultData completeData, IActivityProgress<int> progress)
        {
            Assert.IsNotNull(progress);
            throw new NotImplementedException();
            //return Task.Run(() =>
            //{
            //    bgEvent.Set(); // Signal that bg operation is being executed
            //    fgEvent.WaitOne(); // Wait until we've tested initial status properties
            //    progress.Token.ThrowIfCancellationRequested();
            //    Assert.AreEqual(initializeData.Activity, progress.Activity);
            //    Assert.AreEqual(initializeData.StatusDescription, progress.StatusDescription);
            //    Assert.AreEqual(string.Empty, progress.CurrentOperation);
            //    Assert.AreEqual(initializeData.State, progress.AsyncState);
            //    Assert.IsFalse(progress.PercentComplete.HasValue);
            //    ReportProgressUpdate(updateData, progress);
            //    Assert.AreEqual(initializeData.Activity, progress.Activity);
            //    Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription, progress.StatusDescription);
            //    Assert.AreEqual(updateData.CurrentOperation, progress.CurrentOperation);
            //    Assert.AreEqual(updateData.PercentComplete.HasValue, progress.PercentComplete.HasValue);
            //    if (updateData.PercentComplete.HasValue)
            //        Assert.AreEqual(updateData.PercentComplete.Value, progress.PercentComplete.Value);
            //    Assert.AreEqual(initializeData.State, progress.AsyncState);
            //    bgEvent.Set(); // Signal that status has been updated
            //    fgEvent.WaitOne(); // Wait until we've canceled the operation
            //    progress.Token.ThrowIfCancellationRequested();
            //    return completeData.OnCompleting(progress);
            //});
        }

        private static Task TimedTestActionState(AutoResetEvent fgEvent, AutoResetEvent bgEvent, InitializeStateData initializeData, UpdateData updateData, IActionCompleteData completeData, ITimedBackgroundProgress<int, ITimedBackgroundProgressEvent<int>> progress)
        {
            Assert.IsNotNull(progress);
            throw new NotImplementedException();
            //return Task.Run(() =>
            //{
            //    bgEvent.Set(); // Signal that bg operation is being executed
            //    fgEvent.WaitOne(); // Wait until we've tested initial status properties
            //    progress.Token.ThrowIfCancellationRequested();
            //    Assert.AreEqual(initializeData.Activity, progress.Activity);
            //    Assert.AreEqual(initializeData.StatusDescription, progress.StatusDescription);
            //    Assert.AreEqual(string.Empty, progress.CurrentOperation);
            //    Assert.AreEqual(initializeData.State, progress.AsyncState);
            //    Assert.IsFalse(progress.PercentComplete.HasValue);
            //    ReportProgressUpdate(updateData, progress);
            //    Assert.AreEqual(initializeData.Activity, progress.Activity);
            //    Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription, progress.StatusDescription);
            //    Assert.AreEqual(updateData.CurrentOperation, progress.CurrentOperation);
            //    Assert.AreEqual(updateData.PercentComplete.HasValue, progress.PercentComplete.HasValue);
            //    if (updateData.PercentComplete.HasValue)
            //        Assert.AreEqual(updateData.PercentComplete.Value, progress.PercentComplete.Value);
            //    Assert.AreEqual(initializeData.State, progress.AsyncState);
            //    bgEvent.Set(); // Signal that status has been updated
            //    fgEvent.WaitOne(); // Wait until we've canceled the operation
            //    progress.Token.ThrowIfCancellationRequested();
            //    completeData.OnCompleting(progress);
            //});
        }

        private static Task<double> TimedTestFunc(AutoResetEvent fgEvent, AutoResetEvent bgEvent, InitializeData initializeData, UpdateData updateData, IResultData completeData, IActivityProgress progress)
        {
            Assert.IsNotNull(progress);
            throw new NotImplementedException();
            //return Task.Run(() =>
            //{
            //    bgEvent.Set(); // Signal that bg operation is being executed
            //    fgEvent.WaitOne(); // Wait until we've tested initial status properties
            //    progress.Token.ThrowIfCancellationRequested();
            //    Assert.AreEqual(initializeData.Activity, progress.Activity);
            //    Assert.AreEqual(initializeData.StatusDescription, progress.StatusDescription);
            //    Assert.AreEqual(string.Empty, progress.CurrentOperation);
            //    Assert.IsFalse(progress.PercentComplete.HasValue);
            //    ReportProgressUpdate(updateData, progress);
            //    Assert.AreEqual(initializeData.Activity, progress.Activity);
            //    Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription, progress.StatusDescription);
            //    Assert.AreEqual(updateData.CurrentOperation, progress.CurrentOperation);
            //    Assert.AreEqual(updateData.PercentComplete.HasValue, progress.PercentComplete.HasValue);
            //    if (updateData.PercentComplete.HasValue)
            //        Assert.AreEqual(updateData.PercentComplete.Value, progress.PercentComplete.Value);
            //    bgEvent.Set(); // Signal that status has been updated
            //    fgEvent.WaitOne(); // Wait until we've canceled the operation
            //    progress.Token.ThrowIfCancellationRequested();
            //    return completeData.OnCompleting(progress);
            //});
        }

        private static Task TimedTestAction(AutoResetEvent fgEvent, AutoResetEvent bgEvent, InitializeData initializeData, UpdateData updateData, IActionCompleteData completeData, IActivityProgress progress)
        {
            Assert.IsNotNull(progress);
            throw new NotImplementedException();
            //return Task.Run(() =>
            //{
            //    bgEvent.Set(); // Signal that bg operation is being executed
            //    fgEvent.WaitOne(); // Wait until we've tested initial status properties
            //    progress.Token.ThrowIfCancellationRequested();
            //    Assert.AreEqual(initializeData.Activity, progress.Activity);
            //    Assert.AreEqual(initializeData.StatusDescription, progress.StatusDescription);
            //    Assert.AreEqual(string.Empty, progress.CurrentOperation);
            //    Assert.IsFalse(progress.PercentComplete.HasValue);
            //    ReportProgressUpdate(updateData, progress);
            //    Assert.AreEqual(initializeData.Activity, progress.Activity);
            //    Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription, progress.StatusDescription);
            //    Assert.AreEqual(updateData.CurrentOperation, progress.CurrentOperation);
            //    Assert.AreEqual(updateData.PercentComplete.HasValue, progress.PercentComplete.HasValue);
            //    if (updateData.PercentComplete.HasValue)
            //        Assert.AreEqual(updateData.PercentComplete.Value, progress.PercentComplete.Value);
            //    bgEvent.Set(); // Signal that status has been updated
            //    fgEvent.WaitOne(); // Wait until we've canceled the operation
            //    progress.Token.ThrowIfCancellationRequested();
            //    completeData.OnCompleting(progress);
            //});
        }

        private static Task<double> TestFuncState(AutoResetEvent fgEvent, AutoResetEvent bgEvent, InitializeStateData initializeData, UpdateData updateData, IResultData completeData, IActivityProgress<int> progress)
        {
            Assert.IsNotNull(progress);
            throw new NotImplementedException();
            //return Task.Run(() =>
            //{
            //    bgEvent.Set(); // Signal that bg operation is being executed
            //    fgEvent.WaitOne(); // Wait until we've tested initial status properties
            //    progress.Token.ThrowIfCancellationRequested();
            //    Assert.AreEqual(initializeData.Activity, progress.Activity);
            //    Assert.AreEqual(initializeData.StatusDescription, progress.StatusDescription);
            //    Assert.AreEqual(string.Empty, progress.CurrentOperation);
            //    Assert.AreEqual(initializeData.State, progress.AsyncState);
            //    Assert.IsFalse(progress.PercentComplete.HasValue);
            //    ReportProgressUpdate(updateData, progress);
            //    Assert.AreEqual(initializeData.Activity, progress.Activity);
            //    Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription, progress.StatusDescription);
            //    Assert.AreEqual(updateData.CurrentOperation, progress.CurrentOperation);
            //    Assert.AreEqual(updateData.PercentComplete.HasValue, progress.PercentComplete.HasValue);
            //    if (updateData.PercentComplete.HasValue)
            //        Assert.AreEqual(updateData.PercentComplete.Value, progress.PercentComplete.Value);
            //    Assert.AreEqual(initializeData.State, progress.AsyncState);
            //    bgEvent.Set(); // Signal that status has been updated
            //    fgEvent.WaitOne(); // Wait until we've canceled the operation
            //    progress.Token.ThrowIfCancellationRequested();
            //    return completeData.OnCompleting(progress);
            //});
        }

        private static Task TestActionState(AutoResetEvent fgEvent, AutoResetEvent bgEvent, InitializeStateData initializeData, UpdateData updateData, IActionCompleteData completeData, IActivityProgress<int> progress)
        {
            Assert.IsNotNull(progress);
            throw new NotImplementedException();
            //return Task.Run(() =>
            //{
            //    bgEvent.Set(); // Signal that bg operation is being executed
            //    fgEvent.WaitOne(); // Wait until we've tested initial status properties
            //    progress.Token.ThrowIfCancellationRequested();
            //    Assert.AreEqual(initializeData.Activity, progress.Activity);
            //    Assert.AreEqual(initializeData.StatusDescription, progress.StatusDescription);
            //    Assert.AreEqual(string.Empty, progress.CurrentOperation);
            //    Assert.AreEqual(initializeData.State, progress.AsyncState);
            //    Assert.IsFalse(progress.PercentComplete.HasValue);
            //    ReportProgressUpdate(updateData, progress);
            //    Assert.AreEqual(initializeData.Activity, progress.Activity);
            //    Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription, progress.StatusDescription);
            //    Assert.AreEqual(updateData.CurrentOperation, progress.CurrentOperation);
            //    Assert.AreEqual(updateData.PercentComplete.HasValue, progress.PercentComplete.HasValue);
            //    if (updateData.PercentComplete.HasValue)
            //        Assert.AreEqual(updateData.PercentComplete.Value, progress.PercentComplete.Value);
            //    Assert.AreEqual(initializeData.State, progress.AsyncState);
            //    bgEvent.Set(); // Signal that status has been updated
            //    fgEvent.WaitOne(); // Wait until we've canceled the operation
            //    progress.Token.ThrowIfCancellationRequested();
            //    completeData.OnCompleting(progress);
            //});
        }

        private static Task<double> TestFunc(AutoResetEvent fgEvent, AutoResetEvent bgEvent, InitializeData initializeData, UpdateData updateData, IResultData completeData, IActivityProgress progress)
        {
            Assert.IsNotNull(progress);
            throw new NotImplementedException();
            //return Task.Run(() =>
            //{
            //    bgEvent.Set(); // Signal that bg operation is being executed
            //    fgEvent.WaitOne(); // Wait until we've tested initial status properties
            //    progress.Token.ThrowIfCancellationRequested();
            //    Assert.AreEqual(initializeData.Activity, progress.Activity);
            //    Assert.AreEqual(initializeData.StatusDescription, progress.StatusDescription);
            //    Assert.AreEqual(string.Empty, progress.CurrentOperation);
            //    Assert.IsFalse(progress.PercentComplete.HasValue);
            //    ReportProgressUpdate(updateData, progress);
            //    Assert.AreEqual(initializeData.Activity, progress.Activity);
            //    Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription, progress.StatusDescription);
            //    Assert.AreEqual(updateData.CurrentOperation, progress.CurrentOperation);
            //    Assert.AreEqual(updateData.PercentComplete.HasValue, progress.PercentComplete.HasValue);
            //    if (updateData.PercentComplete.HasValue)
            //        Assert.AreEqual(updateData.PercentComplete.Value, progress.PercentComplete.Value);
            //    bgEvent.Set(); // Signal that status has been updated
            //    fgEvent.WaitOne(); // Wait until we've canceled the operation
            //    progress.Token.ThrowIfCancellationRequested();
            //    return completeData.OnCompleting(progress);
            //});
        }

        private static Task TestAction(AutoResetEvent fgEvent, AutoResetEvent bgEvent, InitializeData initializeData, UpdateData updateData, IActionCompleteData completeData, IActivityProgress<int> progress)
        {
            Assert.IsNotNull(progress);
            throw new NotImplementedException();
            //return Task.Run(() =>
            //{
            //    bgEvent.Set(); // Signal that bg operation is being executed
            //    fgEvent.WaitOne(); // Wait until we've tested initial status properties
            //    progress.Token.ThrowIfCancellationRequested();
            //    Assert.AreEqual(initializeData.Activity, progress.Activity);
            //    Assert.AreEqual(initializeData.StatusDescription, progress.StatusDescription);
            //    Assert.AreEqual(string.Empty, progress.CurrentOperation);
            //    Assert.IsFalse(progress.PercentComplete.HasValue);
            //    ReportProgressUpdate(updateData, progress);
            //    Assert.AreEqual(initializeData.Activity, progress.Activity);
            //    Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription, progress.StatusDescription);
            //    Assert.AreEqual(updateData.CurrentOperation, progress.CurrentOperation);
            //    Assert.AreEqual(updateData.PercentComplete.HasValue, progress.PercentComplete.HasValue);
            //    if (updateData.PercentComplete.HasValue)
            //        Assert.AreEqual(updateData.PercentComplete.Value, progress.PercentComplete.Value);
            //    bgEvent.Set(); // Signal that status has been updated
            //    fgEvent.WaitOne(); // Wait until we've canceled the operation
            //    progress.Token.ThrowIfCancellationRequested();
            //    completeData.OnCompleting(progress);
            //});
        }

        [DataTestMethod]
        [DynamicData(nameof(GetAsyncFuncResultStateData), DynamicDataSourceType.Method)]
        [Priority(10)]
        public async Task InvokeTimedAsyncFuncCompletedStateTokenTest(InitializeStateData initializeData, UpdateData updateData, IResultData completeData)
        {
            Assert.Inconclusive();
            //using AutoResetEvent fgEvent = new(false);
            //using AutoResetEvent bgEvent = new(false);
            //Task<double> asyncMethodDelegate(ITimedBackgroundProgress<int, ITimedBackgroundProgressEvent<int>> progress) =>
            //    TimedTestFuncState(fgEvent, bgEvent, initializeData, updateData, completeData, progress);
            //ITimedBackgroundOperationResultEvent<int, double> onCompleted(ITimedAsyncFunc<ITimedOperationEvent, int, double> backgroundOperation) =>
            //        new TimedBackgroundProcessResultEventArgs<int, double>(backgroundOperation, completeData.Code, null, backgroundOperation.Task.Result, string.IsNullOrWhiteSpace(completeData.StatusDescription) ? backgroundOperation.StatusDescription : completeData.StatusDescription); ;

            //IAsyncActivityService service = Hosting.GetAsyncActivityService();
            //if (service is null)
            //    throw new AssertInconclusiveException();
            //using CancellationTokenSource tokenSource = new();
            //ObserverHelper<ITimedBackgroundProgressEvent<int>> operationObserver = new();
            //ITimedAsyncFunc<ITimedOperationEvent, int, double> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, onCompleted, initializeData.Activity, initializeData.StatusDescription, initializeData.State, tokenSource.Token);

            //#region Test Initial Progress Properties

            //bgEvent.WaitOne(); // Wait until bg operation being executed
            //using IDisposable subscription = backgroundOperation.Subscribe(operationObserver);
            //Assert.IsNotNull(backgroundOperation);
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(initializeData.StatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            //fgEvent.Set(); // Signal that we've tested initial status properties

            //#endregion

            //#region Test Progress Update

            //bgEvent.WaitOne(); // Wait until status has been updated
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    backgroundOperation.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out Observed<ITimedBackgroundProgressEvent<int>> observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //ITimedBackgroundProgressEvent<int> progressEvent = observed.Value;
            //Assert.IsNotNull(progressEvent);
            //Assert.AreEqual(initializeData.Activity, progressEvent.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    progressEvent.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, progressEvent.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, progressEvent.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, progressEvent.PercentComplete.Value);
            //Assert.AreEqual(updateData.Code.HasValue, progressEvent.Code.HasValue);
            //if (updateData.Code.HasValue)
            //    Assert.AreEqual(updateData.Code.Value, progressEvent.Code.Value);
            //Assert.AreEqual(initializeData.State, progressEvent.AsyncState);
            //Assert.AreEqual(backgroundOperation.OperationId, progressEvent.OperationId);
            //Assert.IsFalse(progressEvent.ParentId.HasValue);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion

            //#region Test Operation Completion

            //string expectedStatusDescription;
            //string expectedCurrentOperation;
            //byte? expectedPercentComplete;
            //MessageCode? expectedCode;
            //if (completeData.Cancel)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = updateData.Code;
            //    tokenSource.Cancel();
            //    fgEvent.Set(); // Signal that we've canceled the operation
            //    await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => backgroundOperation.Task);
            //}
            //else if (completeData is ErrorResultData errorData)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = errorData.Code.ToMessageCode(MessageCode.UnexpectedError);
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await Assert.ThrowsExceptionAsync<AsyncOperationException>(() => backgroundOperation.Task);
            //}
            //else
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = string.Empty;
            //    expectedPercentComplete = updateData.PercentComplete.HasValue ? 100 : null;
            //    expectedCode = completeData.Code;
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    double actualResult = await backgroundOperation.Task;
            //    Assert.AreEqual(completeData.Result, actualResult);
            //}

            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(expectedStatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(expectedCurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(expectedPercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (expectedPercentComplete.HasValue)
            //    Assert.AreEqual(expectedPercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //if (observed.Value is ITimedBackgroundOperationCompletedEvent<int> completedEvent)
            //{
            //    if (completeData.Cancel)
            //    {
            //        Assert.IsNotInstanceOfType(completedEvent, typeof(ITimedBackgroundOperationResultEvent<int, double>));
            //        Assert.IsFalse(completedEvent.RanToCompletion);
            //    }
            //    else
            //    {
            //        Assert.IsTrue(completedEvent.RanToCompletion);
            //        if (completedEvent is ITimedBackgroundOperationResultEvent<int, double> resultEvent)
            //            Assert.AreEqual(completeData.Result, resultEvent.Result);
            //        else
            //            Assert.Fail("observed.Value is not ITimedBackgroundOperationResultEvent<int, double>");
            //    }
            //    Assert.IsNull(completedEvent.Error);
            //    Assert.AreEqual(initializeData.Activity, completedEvent.Activity);
            //    Assert.AreEqual(expectedStatusDescription, completedEvent.StatusDescription);
            //    Assert.AreEqual(expectedCurrentOperation, completedEvent.CurrentOperation);
            //    Assert.AreEqual(expectedPercentComplete.HasValue, completedEvent.PercentComplete.HasValue);
            //    if (expectedPercentComplete.HasValue)
            //        Assert.AreEqual(expectedPercentComplete.Value, completedEvent.PercentComplete.Value);
            //    Assert.AreEqual(expectedCode.HasValue, completedEvent.Code);
            //    if (expectedCode.HasValue)
            //        Assert.AreEqual(expectedCode.Value, completedEvent.Code.Value);
            //    Assert.AreEqual(initializeData.State, completedEvent.AsyncState);
            //    Assert.AreEqual(backgroundOperation.OperationId, completedEvent.OperationId);
            //    Assert.IsFalse(completedEvent.ParentId.HasValue);
            //}
            //else
            //    Assert.Fail("observed.Value is not ITimedBackgroundOperationCompletedEvent<int>");

            //#endregion

            //#region Test Observer Completion

            //Thread.Sleep(100);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsTrue(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //Assert.IsNull(observed.Value);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion
        }

        [DataTestMethod]
        [DynamicData(nameof(GetAsyncFuncResultStateData), DynamicDataSourceType.Method)]
        [Priority(10)]
        public async Task InvokeTimedAsyncFuncCompletedStateTest(InitializeStateData initializeData, UpdateData updateData, IResultData completeData)
        {
            Assert.Inconclusive();
            //using AutoResetEvent fgEvent = new(false);
            //using AutoResetEvent bgEvent = new(false);
            //Task<double> asyncMethodDelegate(ITimedBackgroundProgress<int, ITimedBackgroundProgressEvent<int>> progress) =>
            //    TimedTestFuncState(fgEvent, bgEvent, initializeData, updateData, completeData, progress);
            //ITimedBackgroundOperationResultEvent<int, double> onCompleted(ITimedAsyncFunc<ITimedOperationEvent, int, double> backgroundOperation) =>
            //        new TimedBackgroundProcessResultEventArgs<int, double>(backgroundOperation, completeData.Code, null, backgroundOperation.Task.Result, string.IsNullOrWhiteSpace(completeData.StatusDescription) ? backgroundOperation.StatusDescription : completeData.StatusDescription); ;

            //IAsyncActivityService service = Hosting.GetAsyncActivityService();
            //if (service is null)
            //    throw new AssertInconclusiveException();
            //ObserverHelper<ITimedBackgroundProgressEvent<int>> operationObserver = new();
            //ITimedAsyncFunc<ITimedOperationEvent, int, double> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, onCompleted, initializeData.Activity, initializeData.StatusDescription, initializeData.State);

            //#region Test Initial Progress Properties

            //bgEvent.WaitOne(); // Wait until bg operation being executed
            //using IDisposable subscription = backgroundOperation.Subscribe(operationObserver);
            //Assert.IsNotNull(backgroundOperation);
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(initializeData.StatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            //fgEvent.Set(); // Signal that we've tested initial status properties

            //#endregion

            //#region Test Progress Update

            //bgEvent.WaitOne(); // Wait until status has been updated
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    backgroundOperation.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out Observed<ITimedBackgroundProgressEvent<int>> observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //ITimedBackgroundProgressEvent<int> progressEvent = observed.Value;
            //Assert.IsNotNull(progressEvent);
            //Assert.AreEqual(initializeData.Activity, progressEvent.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    progressEvent.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, progressEvent.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, progressEvent.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, progressEvent.PercentComplete.Value);
            //Assert.AreEqual(updateData.Code.HasValue, progressEvent.Code.HasValue);
            //if (updateData.Code.HasValue)
            //    Assert.AreEqual(updateData.Code.Value, progressEvent.Code.Value);
            //Assert.AreEqual(initializeData.State, progressEvent.AsyncState);
            //Assert.AreEqual(backgroundOperation.OperationId, progressEvent.OperationId);
            //Assert.IsFalse(progressEvent.ParentId.HasValue);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion

            //#region Test Operation Completion

            //string expectedStatusDescription;
            //string expectedCurrentOperation;
            //byte? expectedPercentComplete;
            //MessageCode? expectedCode;
            //if (completeData.Cancel)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = updateData.Code;
            //    backgroundOperation.Cancel();
            //    fgEvent.Set(); // Signal that we've canceled the operation
            //    await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => backgroundOperation.Task);
            //}
            //else if (completeData is ErrorResultData errorData)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = errorData.Code.ToMessageCode(MessageCode.UnexpectedError);
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await Assert.ThrowsExceptionAsync<AsyncOperationException>(() => backgroundOperation.Task);
            //}
            //else
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = string.Empty;
            //    expectedPercentComplete = updateData.PercentComplete.HasValue ? 100 : null;
            //    expectedCode = completeData.Code;
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    double actualResult = await backgroundOperation.Task;
            //    Assert.AreEqual(completeData.Result, actualResult);
            //}

            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(expectedStatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(expectedCurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(expectedPercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (expectedPercentComplete.HasValue)
            //    Assert.AreEqual(expectedPercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //if (observed.Value is ITimedBackgroundOperationCompletedEvent<int> completedEvent)
            //{
            //    if (completeData.Cancel)
            //    {
            //        Assert.IsNotInstanceOfType(completedEvent, typeof(ITimedBackgroundOperationResultEvent<int, double>));
            //        Assert.IsFalse(completedEvent.RanToCompletion);
            //    }
            //    else
            //    {
            //        Assert.IsTrue(completedEvent.RanToCompletion);
            //        if (completedEvent is ITimedBackgroundOperationResultEvent<int, double> resultEvent)
            //            Assert.AreEqual(completeData.Result, resultEvent.Result);
            //        else
            //            Assert.Fail("observed.Value is not ITimedBackgroundOperationResultEvent<int, double>");
            //    }
            //    Assert.IsNull(completedEvent.Error);
            //    Assert.AreEqual(initializeData.Activity, completedEvent.Activity);
            //    Assert.AreEqual(expectedStatusDescription, completedEvent.StatusDescription);
            //    Assert.AreEqual(expectedCurrentOperation, completedEvent.CurrentOperation);
            //    Assert.AreEqual(expectedPercentComplete.HasValue, completedEvent.PercentComplete.HasValue);
            //    if (expectedPercentComplete.HasValue)
            //        Assert.AreEqual(expectedPercentComplete.Value, completedEvent.PercentComplete.Value);
            //    Assert.AreEqual(expectedCode.HasValue, completedEvent.Code);
            //    if (expectedCode.HasValue)
            //        Assert.AreEqual(expectedCode.Value, completedEvent.Code.Value);
            //    Assert.AreEqual(initializeData.State, completedEvent.AsyncState);
            //    Assert.AreEqual(backgroundOperation.OperationId, completedEvent.OperationId);
            //    Assert.IsFalse(completedEvent.ParentId.HasValue);
            //}
            //else
            //    Assert.Fail("observed.Value is not ITimedBackgroundOperationCompletedEvent<int>");

            //#endregion

            //#region Test Observer Completion

            //Thread.Sleep(100);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsTrue(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //Assert.IsNull(observed.Value);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion
        }

        [DataTestMethod]
        [DynamicData(nameof(GetAsyncFuncResultStateData), DynamicDataSourceType.Method)]
        [Priority(10)]
        public async Task InvokeTimedAsyncFuncStateTokenTest(InitializeStateData initializeData, UpdateData updateData, IResultData completeData)
        {
            Assert.Inconclusive();
            //using AutoResetEvent fgEvent = new(false);
            //using AutoResetEvent bgEvent = new(false);
            //Task<double> asyncMethodDelegate(ITimedBackgroundProgress<int, ITimedBackgroundProgressEvent<int>> progress) =>
            //    TimedTestFuncState(fgEvent, bgEvent, initializeData, updateData, completeData, progress);

            //IAsyncActivityService service = Hosting.GetAsyncActivityService();
            //if (service is null)
            //    throw new AssertInconclusiveException();
            //using CancellationTokenSource tokenSource = new();
            //ObserverHelper<ITimedBackgroundProgressEvent<int>> operationObserver = new();
            //ITimedAsyncFunc<ITimedOperationEvent, int, double> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, initializeData.Activity, initializeData.StatusDescription, initializeData.State, tokenSource.Token);

            //#region Test Initial Progress Properties

            //bgEvent.WaitOne(); // Wait until bg operation being executed
            //using IDisposable subscription = backgroundOperation.Subscribe(operationObserver);
            //Assert.IsNotNull(backgroundOperation);
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(initializeData.StatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            //fgEvent.Set(); // Signal that we've tested initial status properties

            //#endregion

            //#region Test Progress Update

            //bgEvent.WaitOne(); // Wait until status has been updated
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    backgroundOperation.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out Observed<ITimedBackgroundProgressEvent<int>> observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //ITimedBackgroundProgressEvent<int> progressEvent = observed.Value;
            //Assert.IsNotNull(progressEvent);
            //Assert.AreEqual(initializeData.Activity, progressEvent.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    progressEvent.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, progressEvent.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, progressEvent.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, progressEvent.PercentComplete.Value);
            //Assert.AreEqual(updateData.Code.HasValue, progressEvent.Code.HasValue);
            //if (updateData.Code.HasValue)
            //    Assert.AreEqual(updateData.Code.Value, progressEvent.Code.Value);
            //Assert.AreEqual(initializeData.State, progressEvent.AsyncState);
            //Assert.AreEqual(backgroundOperation.OperationId, progressEvent.OperationId);
            //Assert.IsFalse(progressEvent.ParentId.HasValue);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion

            //#region Test Operation Completion

            //string expectedStatusDescription;
            //string expectedCurrentOperation;
            //byte? expectedPercentComplete;
            //MessageCode? expectedCode;
            //if (completeData.Cancel)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = updateData.Code;
            //    tokenSource.Cancel();
            //    fgEvent.Set(); // Signal that we've canceled the operation
            //    await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => backgroundOperation.Task);
            //}
            //else if (completeData is ErrorResultData errorData)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = errorData.Code.ToMessageCode(MessageCode.UnexpectedError);
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await Assert.ThrowsExceptionAsync<AsyncOperationException>(() => backgroundOperation.Task);
            //}
            //else
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = string.Empty;
            //    expectedPercentComplete = updateData.PercentComplete.HasValue ? 100 : null;
            //    expectedCode = completeData.Code;
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    double actualResult = await backgroundOperation.Task;
            //    Assert.AreEqual(completeData.Result, actualResult);
            //}

            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(expectedStatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(expectedCurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(expectedPercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (expectedPercentComplete.HasValue)
            //    Assert.AreEqual(expectedPercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //if (observed.Value is ITimedBackgroundOperationCompletedEvent<int> completedEvent)
            //{
            //    if (completeData.Cancel)
            //    {
            //        Assert.IsNotInstanceOfType(completedEvent, typeof(ITimedBackgroundOperationResultEvent<int, double>));
            //        Assert.IsFalse(completedEvent.RanToCompletion);
            //    }
            //    else
            //    {
            //        Assert.IsTrue(completedEvent.RanToCompletion);
            //        if (completedEvent is ITimedBackgroundOperationResultEvent<int, double> resultEvent)
            //            Assert.AreEqual(completeData.Result, resultEvent.Result);
            //        else
            //            Assert.Fail("observed.Value is not ITimedBackgroundOperationResultEvent<int, double>");
            //    }
            //    Assert.IsNull(completedEvent.Error);
            //    Assert.AreEqual(initializeData.Activity, completedEvent.Activity);
            //    Assert.AreEqual(expectedStatusDescription, completedEvent.StatusDescription);
            //    Assert.AreEqual(expectedCurrentOperation, completedEvent.CurrentOperation);
            //    Assert.AreEqual(expectedPercentComplete.HasValue, completedEvent.PercentComplete.HasValue);
            //    if (expectedPercentComplete.HasValue)
            //        Assert.AreEqual(expectedPercentComplete.Value, completedEvent.PercentComplete.Value);
            //    Assert.AreEqual(expectedCode.HasValue, completedEvent.Code);
            //    if (expectedCode.HasValue)
            //        Assert.AreEqual(expectedCode.Value, completedEvent.Code.Value);
            //    Assert.AreEqual(initializeData.State, completedEvent.AsyncState);
            //    Assert.AreEqual(backgroundOperation.OperationId, completedEvent.OperationId);
            //    Assert.IsFalse(completedEvent.ParentId.HasValue);
            //}
            //else
            //    Assert.Fail("observed.Value is not ITimedBackgroundOperationCompletedEvent<int>");

            //#endregion

            //#region Test Observer Completion

            //Thread.Sleep(100);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsTrue(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //Assert.IsNull(observed.Value);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion
        }

        [DataTestMethod]
        [DynamicData(nameof(GetAsyncFuncResultStateData), DynamicDataSourceType.Method)]
        [Priority(10)]
        public async Task InvokeTimedAsyncFuncStateTest(InitializeStateData initializeData, UpdateData updateData, IResultData completeData)
        {
            Assert.Inconclusive();
            //using AutoResetEvent fgEvent = new(false);
            //using AutoResetEvent bgEvent = new(false);
            //Task<double> asyncMethodDelegate(ITimedBackgroundProgress<int, ITimedBackgroundProgressEvent<int>> progress) =>
            //    TimedTestFuncState(fgEvent, bgEvent, initializeData, updateData, completeData, progress);

            //IAsyncActivityService service = Hosting.GetAsyncActivityService();
            //if (service is null)
            //    throw new AssertInconclusiveException();
            //ObserverHelper<ITimedBackgroundProgressEvent<int>> operationObserver = new();
            //ITimedAsyncFunc<ITimedOperationEvent, int, double> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, initializeData.Activity, initializeData.StatusDescription, initializeData.State);

            //#region Test Initial Progress Properties

            //bgEvent.WaitOne(); // Wait until bg operation being executed
            //using IDisposable subscription = backgroundOperation.Subscribe(operationObserver);
            //Assert.IsNotNull(backgroundOperation);
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(initializeData.StatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            //fgEvent.Set(); // Signal that we've tested initial status properties

            //#endregion

            //#region Test Progress Update

            //bgEvent.WaitOne(); // Wait until status has been updated
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    backgroundOperation.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out Observed<ITimedBackgroundProgressEvent<int>> observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //ITimedBackgroundProgressEvent<int> progressEvent = observed.Value;
            //Assert.IsNotNull(progressEvent);
            //Assert.AreEqual(initializeData.Activity, progressEvent.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    progressEvent.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, progressEvent.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, progressEvent.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, progressEvent.PercentComplete.Value);
            //Assert.AreEqual(updateData.Code.HasValue, progressEvent.Code.HasValue);
            //if (updateData.Code.HasValue)
            //    Assert.AreEqual(updateData.Code.Value, progressEvent.Code.Value);
            //Assert.AreEqual(initializeData.State, progressEvent.AsyncState);
            //Assert.AreEqual(backgroundOperation.OperationId, progressEvent.OperationId);
            //Assert.IsFalse(progressEvent.ParentId.HasValue);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion

            //#region Test Operation Completion

            //string expectedStatusDescription;
            //string expectedCurrentOperation;
            //byte? expectedPercentComplete;
            //MessageCode? expectedCode;
            //if (completeData.Cancel)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = updateData.Code;
            //    backgroundOperation.Cancel();
            //    fgEvent.Set(); // Signal that we've canceled the operation
            //    await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => backgroundOperation.Task);
            //}
            //else if (completeData is ErrorResultData errorData)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = errorData.Code.ToMessageCode(MessageCode.UnexpectedError);
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await Assert.ThrowsExceptionAsync<AsyncOperationException>(() => backgroundOperation.Task);
            //}
            //else
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = string.Empty;
            //    expectedPercentComplete = updateData.PercentComplete.HasValue ? 100 : null;
            //    expectedCode = completeData.Code;
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    double actualResult = await backgroundOperation.Task;
            //    Assert.AreEqual(completeData.Result, actualResult);
            //}

            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(expectedStatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(expectedCurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(expectedPercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (expectedPercentComplete.HasValue)
            //    Assert.AreEqual(expectedPercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //if (observed.Value is ITimedBackgroundOperationCompletedEvent<int> completedEvent)
            //{
            //    if (completeData.Cancel)
            //    {
            //        Assert.IsNotInstanceOfType(completedEvent, typeof(ITimedBackgroundOperationResultEvent<int, double>));
            //        Assert.IsFalse(completedEvent.RanToCompletion);
            //    }
            //    else
            //    {
            //        Assert.IsTrue(completedEvent.RanToCompletion);
            //        if (completedEvent is ITimedBackgroundOperationResultEvent<int, double> resultEvent)
            //            Assert.AreEqual(completeData.Result, resultEvent.Result);
            //        else
            //            Assert.Fail("observed.Value is not ITimedBackgroundOperationResultEvent<int, double>");
            //    }
            //    Assert.IsNull(completedEvent.Error);
            //    Assert.AreEqual(initializeData.Activity, completedEvent.Activity);
            //    Assert.AreEqual(expectedStatusDescription, completedEvent.StatusDescription);
            //    Assert.AreEqual(expectedCurrentOperation, completedEvent.CurrentOperation);
            //    Assert.AreEqual(expectedPercentComplete.HasValue, completedEvent.PercentComplete.HasValue);
            //    if (expectedPercentComplete.HasValue)
            //        Assert.AreEqual(expectedPercentComplete.Value, completedEvent.PercentComplete.Value);
            //    Assert.AreEqual(expectedCode.HasValue, completedEvent.Code);
            //    if (expectedCode.HasValue)
            //        Assert.AreEqual(expectedCode.Value, completedEvent.Code.Value);
            //    Assert.AreEqual(initializeData.State, completedEvent.AsyncState);
            //    Assert.AreEqual(backgroundOperation.OperationId, completedEvent.OperationId);
            //    Assert.IsFalse(completedEvent.ParentId.HasValue);
            //}
            //else
            //    Assert.Fail("observed.Value is not ITimedBackgroundOperationCompletedEvent<int>");

            //#endregion

            //#region Test Observer Completion

            //Thread.Sleep(100);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsTrue(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //Assert.IsNull(observed.Value);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion
        }

        [DataTestMethod]
        [DynamicData(nameof(GetAsyncFuncResultData), DynamicDataSourceType.Method)]
        [Priority(10)]
        public async Task InvokeTimedAsyncFuncCompletedTokenTest(InitializeData initializeData, UpdateData updateData, IResultData completeData)
        {
            Assert.Inconclusive();
            //using AutoResetEvent fgEvent = new(false);
            //using AutoResetEvent bgEvent = new(false);
            //Task<double> asyncMethodDelegate(IActivityProgress progress) =>
            //    TimedTestFunc(fgEvent, bgEvent, initializeData, updateData, completeData, progress);
            //ITimedBackgroundOperationResultEvent<double> onCompleted(ITimedAsyncFunc<ITimedOperationEvent, double> backgroundOperation) =>
            //        new TimedBackgroundProcessResultEventArgs<double>(backgroundOperation, completeData.Code, null, backgroundOperation.Task.Result); ;

            //IAsyncActivityService service = Hosting.GetAsyncActivityService();
            //if (service is null)
            //    throw new AssertInconclusiveException();
            //using CancellationTokenSource tokenSource = new();
            //ObserverHelper<ITimedBackgroundProgressEvent> operationObserver = new();
            //ITimedAsyncFunc<ITimedOperationEvent, double> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, onCompleted, initializeData.Activity, initializeData.StatusDescription, tokenSource.Token);

            //#region Test Initial Progress Properties

            //bgEvent.WaitOne(); // Wait until bg operation being executed
            //using IDisposable subscription = backgroundOperation.Subscribe(operationObserver);
            //Assert.IsNotNull(backgroundOperation);
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(initializeData.StatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            //Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            //fgEvent.Set(); // Signal that we've tested initial status properties

            //#endregion

            //#region Test Progress Update

            //bgEvent.WaitOne(); // Wait until status has been updated
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    backgroundOperation.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out Observed<ITimedBackgroundProgressEvent> observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //ITimedBackgroundProgressEvent progressEvent = observed.Value;
            //Assert.IsNotNull(progressEvent);
            //Assert.AreEqual(initializeData.Activity, progressEvent.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    progressEvent.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, progressEvent.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, progressEvent.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, progressEvent.PercentComplete.Value);
            //Assert.AreEqual(updateData.Code.HasValue, progressEvent.Code.HasValue);
            //if (updateData.Code.HasValue)
            //    Assert.AreEqual(updateData.Code.Value, progressEvent.Code.Value);
            //Assert.AreEqual(backgroundOperation.OperationId, progressEvent.OperationId);
            //Assert.IsFalse(progressEvent.ParentId.HasValue);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion

            //#region Test Operation Completion

            //string expectedStatusDescription;
            //string expectedCurrentOperation;
            //byte? expectedPercentComplete;
            //MessageCode? expectedCode;
            //if (completeData.Cancel)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = updateData.Code;
            //    tokenSource.Cancel();
            //    fgEvent.Set(); // Signal that we've canceled the operation
            //    await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => backgroundOperation.Task);
            //}
            //else if (completeData is ErrorResultData errorData)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = errorData.Code.ToMessageCode(MessageCode.UnexpectedError);
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await Assert.ThrowsExceptionAsync<AsyncOperationException>(() => backgroundOperation.Task);
            //}
            //else
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = string.Empty;
            //    expectedPercentComplete = updateData.PercentComplete.HasValue ? 100 : null;
            //    expectedCode = completeData.Code;
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    double actualResult = await backgroundOperation.Task;
            //    Assert.AreEqual(completeData.Result, actualResult);
            //}

            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(expectedStatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(expectedCurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(expectedPercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (expectedPercentComplete.HasValue)
            //    Assert.AreEqual(expectedPercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //if (observed.Value is ITimedBackgroundOperationCompletedEvent completedEvent)
            //{
            //    if (completeData.Cancel)
            //    {
            //        Assert.IsNotInstanceOfType(completedEvent, typeof(ITimedBackgroundOperationResultEvent<double>));
            //        Assert.IsFalse(completedEvent.RanToCompletion);
            //    }
            //    else
            //    {
            //        Assert.IsTrue(completedEvent.RanToCompletion);
            //        if (completedEvent is ITimedBackgroundOperationResultEvent<double> resultEvent)
            //            Assert.AreEqual(completeData.Result, resultEvent.Result);
            //        else
            //            Assert.Fail("observed.Value is not ITimedBackgroundOperationResultEvent<double>");
            //    }
            //    Assert.IsNull(completedEvent.Error);
            //    Assert.AreEqual(initializeData.Activity, completedEvent.Activity);
            //    Assert.AreEqual(expectedStatusDescription, completedEvent.StatusDescription);
            //    Assert.AreEqual(expectedCurrentOperation, completedEvent.CurrentOperation);
            //    Assert.AreEqual(expectedPercentComplete.HasValue, completedEvent.PercentComplete.HasValue);
            //    if (expectedPercentComplete.HasValue)
            //        Assert.AreEqual(expectedPercentComplete.Value, completedEvent.PercentComplete.Value);
            //    Assert.AreEqual(expectedCode.HasValue, completedEvent.Code);
            //    if (expectedCode.HasValue)
            //        Assert.AreEqual(expectedCode.Value, completedEvent.Code.Value);
            //    Assert.AreEqual(backgroundOperation.OperationId, completedEvent.OperationId);
            //    Assert.IsFalse(completedEvent.ParentId.HasValue);
            //}
            //else
            //    Assert.Fail("observed.Value is not ITimedBackgroundOperationCompletedEvent");

            //#endregion

            //#region Test Observer Completion

            //Thread.Sleep(100);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsTrue(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //Assert.IsNull(observed.Value);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion
        }

        [DataTestMethod]
        [DynamicData(nameof(GetAsyncFuncResultData), DynamicDataSourceType.Method)]
        [Priority(10)]
        public async Task InvokeTimedAsyncFuncCompletedTest(InitializeData initializeData, UpdateData updateData, IResultData completeData)
        {
            Assert.Inconclusive();
            //using AutoResetEvent fgEvent = new(false);
            //using AutoResetEvent bgEvent = new(false);
            //Task<double> asyncMethodDelegate(IActivityProgress progress) =>
            //    TimedTestFunc(fgEvent, bgEvent, initializeData, updateData, completeData, progress);
            //ITimedBackgroundOperationResultEvent<double> onCompleted(ITimedAsyncFunc<ITimedOperationEvent, double> backgroundOperation) =>
            //        new TimedBackgroundProcessResultEventArgs<double>(backgroundOperation, completeData.Code, null, backgroundOperation.Task.Result); ;

            //IAsyncActivityService service = Hosting.GetAsyncActivityService();
            //if (service is null)
            //    throw new AssertInconclusiveException();
            //ObserverHelper<ITimedBackgroundProgressEvent> operationObserver = new();
            //ITimedAsyncFunc<ITimedOperationEvent, double> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, onCompleted, initializeData.Activity, initializeData.StatusDescription);

            //#region Test Initial Progress Properties

            //bgEvent.WaitOne(); // Wait until bg operation being executed
            //using IDisposable subscription = backgroundOperation.Subscribe(operationObserver);
            //Assert.IsNotNull(backgroundOperation);
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(initializeData.StatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            //Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            //fgEvent.Set(); // Signal that we've tested initial status properties

            //#endregion

            //#region Test Progress Update

            //bgEvent.WaitOne(); // Wait until status has been updated
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    backgroundOperation.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out Observed<ITimedBackgroundProgressEvent> observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //ITimedBackgroundProgressEvent progressEvent = observed.Value;
            //Assert.IsNotNull(progressEvent);
            //Assert.AreEqual(initializeData.Activity, progressEvent.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    progressEvent.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, progressEvent.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, progressEvent.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, progressEvent.PercentComplete.Value);
            //Assert.AreEqual(updateData.Code.HasValue, progressEvent.Code.HasValue);
            //if (updateData.Code.HasValue)
            //    Assert.AreEqual(updateData.Code.Value, progressEvent.Code.Value);
            //Assert.AreEqual(backgroundOperation.OperationId, progressEvent.OperationId);
            //Assert.IsFalse(progressEvent.ParentId.HasValue);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion

            //#region Test Operation Completion

            //string expectedStatusDescription;
            //string expectedCurrentOperation;
            //byte? expectedPercentComplete;
            //MessageCode? expectedCode;
            //if (completeData.Cancel)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = updateData.Code;
            //    backgroundOperation.Cancel();
            //    fgEvent.Set(); // Signal that we've canceled the operation
            //    await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => backgroundOperation.Task);
            //}
            //else if (completeData is ErrorResultData errorData)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = errorData.Code.ToMessageCode(MessageCode.UnexpectedError);
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await Assert.ThrowsExceptionAsync<AsyncOperationException>(() => backgroundOperation.Task);
            //}
            //else
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = string.Empty;
            //    expectedPercentComplete = updateData.PercentComplete.HasValue ? 100 : null;
            //    expectedCode = completeData.Code;
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    double actualResult = await backgroundOperation.Task;
            //    Assert.AreEqual(completeData.Result, actualResult);
            //}

            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(expectedStatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(expectedCurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(expectedPercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (expectedPercentComplete.HasValue)
            //    Assert.AreEqual(expectedPercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //if (observed.Value is ITimedBackgroundOperationCompletedEvent completedEvent)
            //{
            //    if (completeData.Cancel)
            //    {
            //        Assert.IsNotInstanceOfType(completedEvent, typeof(ITimedBackgroundOperationResultEvent<double>));
            //        Assert.IsFalse(completedEvent.RanToCompletion);
            //    }
            //    else
            //    {
            //        Assert.IsTrue(completedEvent.RanToCompletion);
            //        if (completedEvent is ITimedBackgroundOperationResultEvent<double> resultEvent)
            //            Assert.AreEqual(completeData.Result, resultEvent.Result);
            //        else
            //            Assert.Fail("observed.Value is not ITimedBackgroundOperationResultEvent<double>");
            //    }
            //    Assert.IsNull(completedEvent.Error);
            //    Assert.AreEqual(initializeData.Activity, completedEvent.Activity);
            //    Assert.AreEqual(expectedStatusDescription, completedEvent.StatusDescription);
            //    Assert.AreEqual(expectedCurrentOperation, completedEvent.CurrentOperation);
            //    Assert.AreEqual(expectedPercentComplete.HasValue, completedEvent.PercentComplete.HasValue);
            //    if (expectedPercentComplete.HasValue)
            //        Assert.AreEqual(expectedPercentComplete.Value, completedEvent.PercentComplete.Value);
            //    Assert.AreEqual(expectedCode.HasValue, completedEvent.Code);
            //    if (expectedCode.HasValue)
            //        Assert.AreEqual(expectedCode.Value, completedEvent.Code.Value);
            //    Assert.AreEqual(backgroundOperation.OperationId, completedEvent.OperationId);
            //    Assert.IsFalse(completedEvent.ParentId.HasValue);
            //}
            //else
            //    Assert.Fail("observed.Value is not ITimedBackgroundOperationCompletedEvent");

            //#endregion

            //#region Test Observer Completion

            //Thread.Sleep(100);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsTrue(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //Assert.IsNull(observed.Value);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion
        }

        [DataTestMethod]
        [DynamicData(nameof(GetAsyncFuncResultData), DynamicDataSourceType.Method)]
        [Priority(10)]
        public async Task InvokeTimedAsyncFuncTokenTest(InitializeData initializeData, UpdateData updateData, IResultData completeData)
        {
            Assert.Inconclusive();
            //using AutoResetEvent fgEvent = new(false);
            //using AutoResetEvent bgEvent = new(false);
            //Task<double> asyncMethodDelegate(IActivityProgress progress) =>
            //    TimedTestFunc(fgEvent, bgEvent, initializeData, updateData, completeData, progress);

            //IAsyncActivityService service = Hosting.GetAsyncActivityService();
            //if (service is null)
            //    throw new AssertInconclusiveException();
            //using CancellationTokenSource tokenSource = new();
            //ObserverHelper<ITimedBackgroundProgressEvent> operationObserver = new();
            //ITimedAsyncFunc<ITimedOperationEvent, double> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, initializeData.Activity, initializeData.StatusDescription, tokenSource.Token);

            //#region Test Initial Progress Properties

            //bgEvent.WaitOne(); // Wait until bg operation being executed
            //using IDisposable subscription = backgroundOperation.Subscribe(operationObserver);
            //Assert.IsNotNull(backgroundOperation);
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(initializeData.StatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            //Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            //fgEvent.Set(); // Signal that we've tested initial status properties

            //#endregion

            //#region Test Progress Update

            //bgEvent.WaitOne(); // Wait until status has been updated
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    backgroundOperation.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out Observed<ITimedBackgroundProgressEvent> observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //ITimedBackgroundProgressEvent progressEvent = observed.Value;
            //Assert.IsNotNull(progressEvent);
            //Assert.AreEqual(initializeData.Activity, progressEvent.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    progressEvent.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, progressEvent.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, progressEvent.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, progressEvent.PercentComplete.Value);
            //Assert.AreEqual(updateData.Code.HasValue, progressEvent.Code.HasValue);
            //if (updateData.Code.HasValue)
            //    Assert.AreEqual(updateData.Code.Value, progressEvent.Code.Value);
            //Assert.AreEqual(backgroundOperation.OperationId, progressEvent.OperationId);
            //Assert.IsFalse(progressEvent.ParentId.HasValue);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion

            //#region Test Operation Completion

            //string expectedStatusDescription;
            //string expectedCurrentOperation;
            //byte? expectedPercentComplete;
            //MessageCode? expectedCode;
            //if (completeData.Cancel)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = updateData.Code;
            //    tokenSource.Cancel();
            //    fgEvent.Set(); // Signal that we've canceled the operation
            //    await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => backgroundOperation.Task);
            //}
            //else if (completeData is ErrorResultData errorData)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = errorData.Code.ToMessageCode(MessageCode.UnexpectedError);
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await Assert.ThrowsExceptionAsync<AsyncOperationException>(() => backgroundOperation.Task);
            //}
            //else
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = string.Empty;
            //    expectedPercentComplete = updateData.PercentComplete.HasValue ? 100 : null;
            //    expectedCode = completeData.Code;
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    double actualResult = await backgroundOperation.Task;
            //    Assert.AreEqual(completeData.Result, actualResult);
            //}

            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(expectedStatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(expectedCurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(expectedPercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (expectedPercentComplete.HasValue)
            //    Assert.AreEqual(expectedPercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //if (observed.Value is ITimedBackgroundOperationCompletedEvent completedEvent)
            //{
            //    if (completeData.Cancel)
            //    {
            //        Assert.IsNotInstanceOfType(completedEvent, typeof(ITimedBackgroundOperationResultEvent<double>));
            //        Assert.IsFalse(completedEvent.RanToCompletion);
            //    }
            //    else
            //    {
            //        Assert.IsTrue(completedEvent.RanToCompletion);
            //        if (completedEvent is ITimedBackgroundOperationResultEvent<double> resultEvent)
            //            Assert.AreEqual(completeData.Result, resultEvent.Result);
            //        else
            //            Assert.Fail("observed.Value is not ITimedBackgroundOperationResultEvent<double>");
            //    }
            //    Assert.IsNull(completedEvent.Error);
            //    Assert.AreEqual(initializeData.Activity, completedEvent.Activity);
            //    Assert.AreEqual(expectedStatusDescription, completedEvent.StatusDescription);
            //    Assert.AreEqual(expectedCurrentOperation, completedEvent.CurrentOperation);
            //    Assert.AreEqual(expectedPercentComplete.HasValue, completedEvent.PercentComplete.HasValue);
            //    if (expectedPercentComplete.HasValue)
            //        Assert.AreEqual(expectedPercentComplete.Value, completedEvent.PercentComplete.Value);
            //    Assert.AreEqual(expectedCode.HasValue, completedEvent.Code);
            //    if (expectedCode.HasValue)
            //        Assert.AreEqual(expectedCode.Value, completedEvent.Code.Value);
            //    Assert.AreEqual(backgroundOperation.OperationId, completedEvent.OperationId);
            //    Assert.IsFalse(completedEvent.ParentId.HasValue);
            //}
            //else
            //    Assert.Fail("observed.Value is not ITimedBackgroundOperationCompletedEvent");

            //#endregion

            //#region Test Observer Completion

            //Thread.Sleep(100);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsTrue(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //Assert.IsNull(observed.Value);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion
        }

        [DataTestMethod]
        [DynamicData(nameof(GetAsyncFuncResultData), DynamicDataSourceType.Method)]
        [Priority(10)]
        public async Task InvokeTimedAsyncFuncTest(InitializeData initializeData, UpdateData updateData, IResultData completeData)
        {
            Assert.Inconclusive();
            //using AutoResetEvent fgEvent = new(false);
            //using AutoResetEvent bgEvent = new(false);
            //Task<double> asyncMethodDelegate(IActivityProgress progress) =>
            //    TimedTestFunc(fgEvent, bgEvent, initializeData, updateData, completeData, progress);

            //IAsyncActivityService service = Hosting.GetAsyncActivityService();
            //if (service is null)
            //    throw new AssertInconclusiveException();
            //ObserverHelper<ITimedBackgroundProgressEvent> operationObserver = new();
            //ITimedAsyncFunc<ITimedOperationEvent, double> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, initializeData.Activity, initializeData.StatusDescription);

            //#region Test Initial Progress Properties

            //bgEvent.WaitOne(); // Wait until bg operation being executed
            //using IDisposable subscription = backgroundOperation.Subscribe(operationObserver);
            //Assert.IsNotNull(backgroundOperation);
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(initializeData.StatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            //Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            //fgEvent.Set(); // Signal that we've tested initial status properties

            //#endregion

            //#region Test Progress Update

            //bgEvent.WaitOne(); // Wait until status has been updated
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    backgroundOperation.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out Observed<ITimedBackgroundProgressEvent> observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //ITimedBackgroundProgressEvent progressEvent = observed.Value;
            //Assert.IsNotNull(progressEvent);
            //Assert.AreEqual(initializeData.Activity, progressEvent.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    progressEvent.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, progressEvent.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, progressEvent.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, progressEvent.PercentComplete.Value);
            //Assert.AreEqual(updateData.Code.HasValue, progressEvent.Code.HasValue);
            //if (updateData.Code.HasValue)
            //    Assert.AreEqual(updateData.Code.Value, progressEvent.Code.Value);
            //Assert.AreEqual(backgroundOperation.OperationId, progressEvent.OperationId);
            //Assert.IsFalse(progressEvent.ParentId.HasValue);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion

            //#region Test Operation Completion

            //string expectedStatusDescription;
            //string expectedCurrentOperation;
            //byte? expectedPercentComplete;
            //MessageCode? expectedCode;
            //if (completeData.Cancel)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = updateData.Code;
            //    backgroundOperation.Cancel();
            //    fgEvent.Set(); // Signal that we've canceled the operation
            //    await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => backgroundOperation.Task);
            //}
            //else if (completeData is ErrorResultData errorData)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = errorData.Code.ToMessageCode(MessageCode.UnexpectedError);
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await Assert.ThrowsExceptionAsync<AsyncOperationException>(() => backgroundOperation.Task);
            //}
            //else
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = string.Empty;
            //    expectedPercentComplete = updateData.PercentComplete.HasValue ? 100 : null;
            //    expectedCode = completeData.Code;
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    double actualResult = await backgroundOperation.Task;
            //    Assert.AreEqual(completeData.Result, actualResult);
            //}

            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(expectedStatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(expectedCurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(expectedPercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (expectedPercentComplete.HasValue)
            //    Assert.AreEqual(expectedPercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //if (observed.Value is ITimedBackgroundOperationCompletedEvent completedEvent)
            //{
            //    if (completeData.Cancel)
            //    {
            //        Assert.IsNotInstanceOfType(completedEvent, typeof(ITimedBackgroundOperationResultEvent<double>));
            //        Assert.IsFalse(completedEvent.RanToCompletion);
            //    }
            //    else
            //    {
            //        Assert.IsTrue(completedEvent.RanToCompletion);
            //        if (completedEvent is ITimedBackgroundOperationResultEvent<double> resultEvent)
            //            Assert.AreEqual(completeData.Result, resultEvent.Result);
            //        else
            //            Assert.Fail("observed.Value is not ITimedBackgroundOperationResultEvent<double>");
            //    }
            //    Assert.IsNull(completedEvent.Error);
            //    Assert.AreEqual(initializeData.Activity, completedEvent.Activity);
            //    Assert.AreEqual(expectedStatusDescription, completedEvent.StatusDescription);
            //    Assert.AreEqual(expectedCurrentOperation, completedEvent.CurrentOperation);
            //    Assert.AreEqual(expectedPercentComplete.HasValue, completedEvent.PercentComplete.HasValue);
            //    if (expectedPercentComplete.HasValue)
            //        Assert.AreEqual(expectedPercentComplete.Value, completedEvent.PercentComplete.Value);
            //    Assert.AreEqual(expectedCode.HasValue, completedEvent.Code);
            //    if (expectedCode.HasValue)
            //        Assert.AreEqual(expectedCode.Value, completedEvent.Code.Value);
            //    Assert.AreEqual(backgroundOperation.OperationId, completedEvent.OperationId);
            //    Assert.IsFalse(completedEvent.ParentId.HasValue);
            //}
            //else
            //    Assert.Fail("observed.Value is not ITimedBackgroundOperationCompletedEvent");

            //#endregion

            //#region Test Observer Completion

            //Thread.Sleep(100);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsTrue(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //Assert.IsNull(observed.Value);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion
        }

        [DataTestMethod]
        [DynamicData(nameof(GetAsyncFuncResultStateData), DynamicDataSourceType.Method)]
        [Priority(10)]
        public async Task InvokeAsyncFuncCompletedStateTokenTest(InitializeStateData initializeData, UpdateData updateData, IResultData completeData)
        {
            Assert.Inconclusive();
            //using AutoResetEvent fgEvent = new(false);
            //using AutoResetEvent bgEvent = new(false);
            //Task<double> asyncMethodDelegate(IBackgroundProgress<int, IAsyncActivity<int>> progress) =>
            //    TestFuncState(fgEvent, bgEvent, initializeData, updateData, completeData, progress);
            //IBackgroundOperationResultEvent<int, double> onCompleted(IBackgroundFunc<int, double> backgroundOperation) =>
            //        new BackgroundProcessResultEventArgs<int, double>(backgroundOperation, completeData.Code, null, backgroundOperation.Task.Result, string.IsNullOrWhiteSpace(completeData.StatusDescription) ? backgroundOperation.StatusDescription : completeData.StatusDescription); ;

            //IAsyncActivityService service = Hosting.GetAsyncActivityService();
            //if (service is null)
            //    throw new AssertInconclusiveException();
            //using CancellationTokenSource tokenSource = new();
            //ObserverHelper<IAsyncActivity<int>> operationObserver = new();
            //IBackgroundFunc<int, double> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, onCompleted, initializeData.Activity, initializeData.StatusDescription, initializeData.State, tokenSource.Token);

            //#region Test Initial Progress Properties

            //bgEvent.WaitOne(); // Wait until bg operation being executed
            //using IDisposable subscription = backgroundOperation.Subscribe(operationObserver);
            //Assert.IsNotNull(backgroundOperation);
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(initializeData.StatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            //fgEvent.Set(); // Signal that we've tested initial status properties

            //#endregion

            //#region Test Progress Update

            //bgEvent.WaitOne(); // Wait until status has been updated
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    backgroundOperation.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out Observed<IAsyncActivity<int>> observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //IAsyncActivity<int> progressEvent = observed.Value;
            //Assert.IsNotNull(progressEvent);
            //Assert.AreEqual(initializeData.Activity, progressEvent.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    progressEvent.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, progressEvent.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, progressEvent.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, progressEvent.PercentComplete.Value);
            //Assert.AreEqual(updateData.Code.HasValue, progressEvent.Code.HasValue);
            //if (updateData.Code.HasValue)
            //    Assert.AreEqual(updateData.Code.Value, progressEvent.Code.Value);
            //Assert.AreEqual(initializeData.State, progressEvent.AsyncState);
            //Assert.AreEqual(backgroundOperation.OperationId, progressEvent.OperationId);
            //Assert.IsFalse(progressEvent.ParentId.HasValue);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion

            //#region Test Operation Completion

            //string expectedStatusDescription;
            //string expectedCurrentOperation;
            //byte? expectedPercentComplete;
            //MessageCode? expectedCode;
            //if (completeData.Cancel)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = updateData.Code;
            //    tokenSource.Cancel();
            //    fgEvent.Set(); // Signal that we've canceled the operation
            //    await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => backgroundOperation.Task);
            //}
            //else if (completeData is ErrorResultData errorData)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = errorData.Code.ToMessageCode(MessageCode.UnexpectedError);
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await Assert.ThrowsExceptionAsync<AsyncOperationException>(() => backgroundOperation.Task);
            //}
            //else
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = string.Empty;
            //    expectedPercentComplete = updateData.PercentComplete.HasValue ? 100 : null;
            //    expectedCode = completeData.Code;
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    double actualResult = await backgroundOperation.Task;
            //    Assert.AreEqual(completeData.Result, actualResult);
            //}

            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(expectedStatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(expectedCurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(expectedPercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (expectedPercentComplete.HasValue)
            //    Assert.AreEqual(expectedPercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //if (observed.Value is IBackgroundOperationCompletedEvent<int> completedEvent)
            //{
            //    if (completeData.Cancel)
            //    {
            //        Assert.IsNotInstanceOfType(completedEvent, typeof(IBackgroundOperationResultEvent<int, double>));
            //        Assert.IsFalse(completedEvent.RanToCompletion);
            //    }
            //    else
            //    {
            //        Assert.IsTrue(completedEvent.RanToCompletion);
            //        if (completedEvent is IBackgroundOperationResultEvent<int, double> resultEvent)
            //            Assert.AreEqual(completeData.Result, resultEvent.Result);
            //        else
            //            Assert.Fail("observed.Value is not IBackgroundOperationResultEvent<int, double>");
            //    }
            //    Assert.IsNull(completedEvent.Error);
            //    Assert.AreEqual(initializeData.Activity, completedEvent.Activity);
            //    Assert.AreEqual(expectedStatusDescription, completedEvent.StatusDescription);
            //    Assert.AreEqual(expectedCurrentOperation, completedEvent.CurrentOperation);
            //    Assert.AreEqual(expectedPercentComplete.HasValue, completedEvent.PercentComplete.HasValue);
            //    if (expectedPercentComplete.HasValue)
            //        Assert.AreEqual(expectedPercentComplete.Value, completedEvent.PercentComplete.Value);
            //    Assert.AreEqual(expectedCode.HasValue, completedEvent.Code);
            //    if (expectedCode.HasValue)
            //        Assert.AreEqual(expectedCode.Value, completedEvent.Code.Value);
            //    Assert.AreEqual(initializeData.State, completedEvent.AsyncState);
            //    Assert.AreEqual(backgroundOperation.OperationId, completedEvent.OperationId);
            //    Assert.IsFalse(completedEvent.ParentId.HasValue);
            //}
            //else
            //    Assert.Fail("observed.Value is not IBackgroundOperationCompletedEvent<int>");

            //#endregion

            //#region Test Observer Completion

            //Thread.Sleep(100);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsTrue(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //Assert.IsNull(observed.Value);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion
        }

        [DataTestMethod]
        [DynamicData(nameof(GetAsyncFuncResultStateData), DynamicDataSourceType.Method)]
        [Priority(10)]
        public async Task InvokeAsyncFuncCompletedStateTest(InitializeStateData initializeData, UpdateData updateData, IResultData completeData)
        {
            Assert.Inconclusive();
            //using AutoResetEvent fgEvent = new(false);
            //using AutoResetEvent bgEvent = new(false);
            //Task<double> asyncMethodDelegate(IBackgroundProgress<int, IAsyncActivity<int>> progress) =>
            //    TestFuncState(fgEvent, bgEvent, initializeData, updateData, completeData, progress);
            //IBackgroundOperationResultEvent<int, double> onCompleted(IBackgroundFunc<int, double> backgroundOperation) =>
            //        new BackgroundProcessResultEventArgs<int, double>(backgroundOperation, completeData.Code, null, backgroundOperation.Task.Result, string.IsNullOrWhiteSpace(completeData.StatusDescription) ? backgroundOperation.StatusDescription : completeData.StatusDescription); ;

            //IAsyncActivityService service = Hosting.GetAsyncActivityService();
            //if (service is null)
            //    throw new AssertInconclusiveException();
            //ObserverHelper<IAsyncActivity<int>> operationObserver = new();
            //IBackgroundFunc<int, double> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, onCompleted, initializeData.Activity, initializeData.StatusDescription, initializeData.State);

            //#region Test Initial Progress Properties

            //bgEvent.WaitOne(); // Wait until bg operation being executed
            //using IDisposable subscription = backgroundOperation.Subscribe(operationObserver);
            //Assert.IsNotNull(backgroundOperation);
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(initializeData.StatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            //fgEvent.Set(); // Signal that we've tested initial status properties

            //#endregion

            //#region Test Progress Update

            //bgEvent.WaitOne(); // Wait until status has been updated
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    backgroundOperation.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out Observed<IAsyncActivity<int>> observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //IAsyncActivity<int> progressEvent = observed.Value;
            //Assert.IsNotNull(progressEvent);
            //Assert.AreEqual(initializeData.Activity, progressEvent.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    progressEvent.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, progressEvent.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, progressEvent.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, progressEvent.PercentComplete.Value);
            //Assert.AreEqual(updateData.Code.HasValue, progressEvent.Code.HasValue);
            //if (updateData.Code.HasValue)
            //    Assert.AreEqual(updateData.Code.Value, progressEvent.Code.Value);
            //Assert.AreEqual(initializeData.State, progressEvent.AsyncState);
            //Assert.AreEqual(backgroundOperation.OperationId, progressEvent.OperationId);
            //Assert.IsFalse(progressEvent.ParentId.HasValue);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion

            //#region Test Operation Completion

            //string expectedStatusDescription;
            //string expectedCurrentOperation;
            //byte? expectedPercentComplete;
            //MessageCode? expectedCode;
            //if (completeData.Cancel)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = updateData.Code;
            //    backgroundOperation.Cancel();
            //    fgEvent.Set(); // Signal that we've canceled the operation
            //    await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => backgroundOperation.Task);
            //}
            //else if (completeData is ErrorResultData errorData)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = errorData.Code.ToMessageCode(MessageCode.UnexpectedError);
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await Assert.ThrowsExceptionAsync<AsyncOperationException>(() => backgroundOperation.Task);
            //}
            //else
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = string.Empty;
            //    expectedPercentComplete = updateData.PercentComplete.HasValue ? 100 : null;
            //    expectedCode = completeData.Code;
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    double actualResult = await backgroundOperation.Task;
            //    Assert.AreEqual(completeData.Result, actualResult);
            //}

            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(expectedStatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(expectedCurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(expectedPercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (expectedPercentComplete.HasValue)
            //    Assert.AreEqual(expectedPercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //if (observed.Value is IBackgroundOperationCompletedEvent<int> completedEvent)
            //{
            //    if (completeData.Cancel)
            //    {
            //        Assert.IsNotInstanceOfType(completedEvent, typeof(IBackgroundOperationResultEvent<int, double>));
            //        Assert.IsFalse(completedEvent.RanToCompletion);
            //    }
            //    else
            //    {
            //        Assert.IsTrue(completedEvent.RanToCompletion);
            //        if (completedEvent is IBackgroundOperationResultEvent<int, double> resultEvent)
            //            Assert.AreEqual(completeData.Result, resultEvent.Result);
            //        else
            //            Assert.Fail("observed.Value is not IBackgroundOperationResultEvent<int, double>");
            //    }
            //    Assert.IsNull(completedEvent.Error);
            //    Assert.AreEqual(initializeData.Activity, completedEvent.Activity);
            //    Assert.AreEqual(expectedStatusDescription, completedEvent.StatusDescription);
            //    Assert.AreEqual(expectedCurrentOperation, completedEvent.CurrentOperation);
            //    Assert.AreEqual(expectedPercentComplete.HasValue, completedEvent.PercentComplete.HasValue);
            //    if (expectedPercentComplete.HasValue)
            //        Assert.AreEqual(expectedPercentComplete.Value, completedEvent.PercentComplete.Value);
            //    Assert.AreEqual(expectedCode.HasValue, completedEvent.Code);
            //    if (expectedCode.HasValue)
            //        Assert.AreEqual(expectedCode.Value, completedEvent.Code.Value);
            //    Assert.AreEqual(initializeData.State, completedEvent.AsyncState);
            //    Assert.AreEqual(backgroundOperation.OperationId, completedEvent.OperationId);
            //    Assert.IsFalse(completedEvent.ParentId.HasValue);
            //}
            //else
            //    Assert.Fail("observed.Value is not IBackgroundOperationCompletedEvent<int>");

            //#endregion

            //#region Test Observer Completion

            //Thread.Sleep(100);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsTrue(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //Assert.IsNull(observed.Value);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion
        }

        [DataTestMethod]
        [DynamicData(nameof(GetAsyncFuncResultStateData), DynamicDataSourceType.Method)]
        [Priority(10)]
        public async Task InvokeAsyncFuncStateTokenTest(InitializeStateData initializeData, UpdateData updateData, IResultData completeData)
        {
            Assert.Inconclusive();
            //using AutoResetEvent fgEvent = new(false);
            //using AutoResetEvent bgEvent = new(false);
            //Task<double> asyncMethodDelegate(IBackgroundProgress<int, IAsyncActivity<int>> progress) =>
            //    TestFuncState(fgEvent, bgEvent, initializeData, updateData, completeData, progress);

            //IAsyncActivityService service = Hosting.GetAsyncActivityService();
            //if (service is null)
            //    throw new AssertInconclusiveException();
            //using CancellationTokenSource tokenSource = new();
            //ObserverHelper<IAsyncActivity<int>> operationObserver = new();
            //IBackgroundFunc<int, double> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, initializeData.Activity, initializeData.StatusDescription, initializeData.State, tokenSource.Token);

            //#region Test Initial Progress Properties

            //bgEvent.WaitOne(); // Wait until bg operation being executed
            //using IDisposable subscription = backgroundOperation.Subscribe(operationObserver);
            //Assert.IsNotNull(backgroundOperation);
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(initializeData.StatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            //fgEvent.Set(); // Signal that we've tested initial status properties

            //#endregion

            //#region Test Progress Update

            //bgEvent.WaitOne(); // Wait until status has been updated
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    backgroundOperation.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out Observed<IAsyncActivity<int>> observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //IAsyncActivity<int> progressEvent = observed.Value;
            //Assert.IsNotNull(progressEvent);
            //Assert.AreEqual(initializeData.Activity, progressEvent.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    progressEvent.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, progressEvent.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, progressEvent.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, progressEvent.PercentComplete.Value);
            //Assert.AreEqual(updateData.Code.HasValue, progressEvent.Code.HasValue);
            //if (updateData.Code.HasValue)
            //    Assert.AreEqual(updateData.Code.Value, progressEvent.Code.Value);
            //Assert.AreEqual(initializeData.State, progressEvent.AsyncState);
            //Assert.AreEqual(backgroundOperation.OperationId, progressEvent.OperationId);
            //Assert.IsFalse(progressEvent.ParentId.HasValue);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion

            //#region Test Operation Completion

            //string expectedStatusDescription;
            //string expectedCurrentOperation;
            //byte? expectedPercentComplete;
            //MessageCode? expectedCode;
            //if (completeData.Cancel)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = updateData.Code;
            //    tokenSource.Cancel();
            //    fgEvent.Set(); // Signal that we've canceled the operation
            //    await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => backgroundOperation.Task);
            //}
            //else if (completeData is ErrorResultData errorData)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = errorData.Code.ToMessageCode(MessageCode.UnexpectedError);
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await Assert.ThrowsExceptionAsync<AsyncOperationException>(() => backgroundOperation.Task);
            //}
            //else
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = string.Empty;
            //    expectedPercentComplete = updateData.PercentComplete.HasValue ? 100 : null;
            //    expectedCode = completeData.Code;
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    double actualResult = await backgroundOperation.Task;
            //    Assert.AreEqual(completeData.Result, actualResult);
            //}

            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(expectedStatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(expectedCurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(expectedPercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (expectedPercentComplete.HasValue)
            //    Assert.AreEqual(expectedPercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //if (observed.Value is IBackgroundOperationCompletedEvent<int> completedEvent)
            //{
            //    if (completeData.Cancel)
            //    {
            //        Assert.IsNotInstanceOfType(completedEvent, typeof(IBackgroundOperationResultEvent<int, double>));
            //        Assert.IsFalse(completedEvent.RanToCompletion);
            //    }
            //    else
            //    {
            //        Assert.IsTrue(completedEvent.RanToCompletion);
            //        if (completedEvent is IBackgroundOperationResultEvent<int, double> resultEvent)
            //            Assert.AreEqual(completeData.Result, resultEvent.Result);
            //        else
            //            Assert.Fail("observed.Value is not IBackgroundOperationResultEvent<int, double>");
            //    }
            //    Assert.IsNull(completedEvent.Error);
            //    Assert.AreEqual(initializeData.Activity, completedEvent.Activity);
            //    Assert.AreEqual(expectedStatusDescription, completedEvent.StatusDescription);
            //    Assert.AreEqual(expectedCurrentOperation, completedEvent.CurrentOperation);
            //    Assert.AreEqual(expectedPercentComplete.HasValue, completedEvent.PercentComplete.HasValue);
            //    if (expectedPercentComplete.HasValue)
            //        Assert.AreEqual(expectedPercentComplete.Value, completedEvent.PercentComplete.Value);
            //    Assert.AreEqual(expectedCode.HasValue, completedEvent.Code);
            //    if (expectedCode.HasValue)
            //        Assert.AreEqual(expectedCode.Value, completedEvent.Code.Value);
            //    Assert.AreEqual(initializeData.State, completedEvent.AsyncState);
            //    Assert.AreEqual(backgroundOperation.OperationId, completedEvent.OperationId);
            //    Assert.IsFalse(completedEvent.ParentId.HasValue);
            //}
            //else
            //    Assert.Fail("observed.Value is not IBackgroundOperationCompletedEvent<int>");

            //#endregion

            //#region Test Observer Completion

            //Thread.Sleep(100);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsTrue(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //Assert.IsNull(observed.Value);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion
        }

        [DataTestMethod]
        [DynamicData(nameof(GetAsyncFuncResultStateData), DynamicDataSourceType.Method)]
        [Priority(10)]
        public async Task InvokeAsyncFuncStateTest(InitializeStateData initializeData, UpdateData updateData, IResultData completeData)
        {
            Assert.Inconclusive();
            //using AutoResetEvent fgEvent = new(false);
            //using AutoResetEvent bgEvent = new(false);
            //Task<double> asyncMethodDelegate(IBackgroundProgress<int, IAsyncActivity<int>> progress) =>
            //    TestFuncState(fgEvent, bgEvent, initializeData, updateData, completeData, progress);

            //IAsyncActivityService service = Hosting.GetAsyncActivityService();
            //if (service is null)
            //    throw new AssertInconclusiveException();
            //ObserverHelper<IAsyncActivity<int>> operationObserver = new();
            //IBackgroundFunc<int, double> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, initializeData.Activity, initializeData.StatusDescription, initializeData.State);

            //#region Test Initial Progress Properties

            //bgEvent.WaitOne(); // Wait until bg operation being executed
            //using IDisposable subscription = backgroundOperation.Subscribe(operationObserver);
            //Assert.IsNotNull(backgroundOperation);
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(initializeData.StatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            //fgEvent.Set(); // Signal that we've tested initial status properties

            //#endregion

            //#region Test Progress Update

            //bgEvent.WaitOne(); // Wait until status has been updated
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    backgroundOperation.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out Observed<IAsyncActivity<int>> observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //IAsyncActivity<int> progressEvent = observed.Value;
            //Assert.IsNotNull(progressEvent);
            //Assert.AreEqual(initializeData.Activity, progressEvent.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    progressEvent.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, progressEvent.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, progressEvent.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, progressEvent.PercentComplete.Value);
            //Assert.AreEqual(updateData.Code.HasValue, progressEvent.Code.HasValue);
            //if (updateData.Code.HasValue)
            //    Assert.AreEqual(updateData.Code.Value, progressEvent.Code.Value);
            //Assert.AreEqual(initializeData.State, progressEvent.AsyncState);
            //Assert.AreEqual(backgroundOperation.OperationId, progressEvent.OperationId);
            //Assert.IsFalse(progressEvent.ParentId.HasValue);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion

            //#region Test Operation Completion

            //string expectedStatusDescription;
            //string expectedCurrentOperation;
            //byte? expectedPercentComplete;
            //MessageCode? expectedCode;
            //if (completeData.Cancel)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = updateData.Code;
            //    backgroundOperation.Cancel();
            //    fgEvent.Set(); // Signal that we've canceled the operation
            //    await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => backgroundOperation.Task);
            //}
            //else if (completeData is ErrorResultData errorData)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = errorData.Code.ToMessageCode(MessageCode.UnexpectedError);
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await Assert.ThrowsExceptionAsync<AsyncOperationException>(() => backgroundOperation.Task);
            //}
            //else
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = string.Empty;
            //    expectedPercentComplete = updateData.PercentComplete.HasValue ? 100 : null;
            //    expectedCode = completeData.Code;
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    double actualResult = await backgroundOperation.Task;
            //    Assert.AreEqual(completeData.Result, actualResult);
            //}

            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(expectedStatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(expectedCurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(expectedPercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (expectedPercentComplete.HasValue)
            //    Assert.AreEqual(expectedPercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //if (observed.Value is IBackgroundOperationCompletedEvent<int> completedEvent)
            //{
            //    if (completeData.Cancel)
            //    {
            //        Assert.IsNotInstanceOfType(completedEvent, typeof(IBackgroundOperationResultEvent<int, double>));
            //        Assert.IsFalse(completedEvent.RanToCompletion);
            //    }
            //    else
            //    {
            //        Assert.IsTrue(completedEvent.RanToCompletion);
            //        if (completedEvent is IBackgroundOperationResultEvent<int, double> resultEvent)
            //            Assert.AreEqual(completeData.Result, resultEvent.Result);
            //        else
            //            Assert.Fail("observed.Value is not IBackgroundOperationResultEvent<int, double>");
            //    }
            //    Assert.IsNull(completedEvent.Error);
            //    Assert.AreEqual(initializeData.Activity, completedEvent.Activity);
            //    Assert.AreEqual(expectedStatusDescription, completedEvent.StatusDescription);
            //    Assert.AreEqual(expectedCurrentOperation, completedEvent.CurrentOperation);
            //    Assert.AreEqual(expectedPercentComplete.HasValue, completedEvent.PercentComplete.HasValue);
            //    if (expectedPercentComplete.HasValue)
            //        Assert.AreEqual(expectedPercentComplete.Value, completedEvent.PercentComplete.Value);
            //    Assert.AreEqual(expectedCode.HasValue, completedEvent.Code);
            //    if (expectedCode.HasValue)
            //        Assert.AreEqual(expectedCode.Value, completedEvent.Code.Value);
            //    Assert.AreEqual(initializeData.State, completedEvent.AsyncState);
            //    Assert.AreEqual(backgroundOperation.OperationId, completedEvent.OperationId);
            //    Assert.IsFalse(completedEvent.ParentId.HasValue);
            //}
            //else
            //    Assert.Fail("observed.Value is not IBackgroundOperationCompletedEvent<int>");

            //#endregion

            //#region Test Observer Completion

            //Thread.Sleep(100);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsTrue(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //Assert.IsNull(observed.Value);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion
        }

        [DataTestMethod]
        [DynamicData(nameof(GetAsyncFuncResultData), DynamicDataSourceType.Method)]
        [Priority(10)]
        public async Task InvokeAsyncFuncCompletedTokenTest(InitializeData initializeData, UpdateData updateData, IResultData completeData)
        {
            Assert.Inconclusive();
            //using AutoResetEvent fgEvent = new(false);
            //using AutoResetEvent bgEvent = new(false);
            //Task<double> asyncMethodDelegate(IBackgroundProgress<IAsyncActivity> progress) =>
            //    TestFunc(fgEvent, bgEvent, initializeData, updateData, completeData, progress);
            //IBackgroundOperationResultEvent<double> onCompleted(IBackgroundFunc<double> backgroundOperation) =>
            //        new BackgroundProcessResultEventArgs<double>(backgroundOperation, completeData.Code, null, backgroundOperation.Task.Result); ;

            //IAsyncActivityService service = Hosting.GetAsyncActivityService();
            //if (service is null)
            //    throw new AssertInconclusiveException();
            //using CancellationTokenSource tokenSource = new();
            //ObserverHelper<IAsyncActivity> operationObserver = new();
            //IBackgroundFunc<double> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, onCompleted, initializeData.Activity, initializeData.StatusDescription, tokenSource.Token);

            //#region Test Initial Progress Properties

            //bgEvent.WaitOne(); // Wait until bg operation being executed
            //using IDisposable subscription = backgroundOperation.Subscribe(operationObserver);
            //Assert.IsNotNull(backgroundOperation);
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(initializeData.StatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            //Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            //fgEvent.Set(); // Signal that we've tested initial status properties

            //#endregion

            //#region Test Progress Update

            //bgEvent.WaitOne(); // Wait until status has been updated
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    backgroundOperation.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out Observed<IAsyncActivity> observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //IAsyncActivity progressEvent = observed.Value;
            //Assert.IsNotNull(progressEvent);
            //Assert.AreEqual(initializeData.Activity, progressEvent.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    progressEvent.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, progressEvent.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, progressEvent.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, progressEvent.PercentComplete.Value);
            //Assert.AreEqual(updateData.Code.HasValue, progressEvent.Code.HasValue);
            //if (updateData.Code.HasValue)
            //    Assert.AreEqual(updateData.Code.Value, progressEvent.Code.Value);
            //Assert.AreEqual(backgroundOperation.OperationId, progressEvent.OperationId);
            //Assert.IsFalse(progressEvent.ParentId.HasValue);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion

            //#region Test Operation Completion

            //string expectedStatusDescription;
            //string expectedCurrentOperation;
            //byte? expectedPercentComplete;
            //MessageCode? expectedCode;
            //if (completeData.Cancel)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = updateData.Code;
            //    tokenSource.Cancel();
            //    fgEvent.Set(); // Signal that we've canceled the operation
            //    await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => backgroundOperation.Task);
            //}
            //else if (completeData is ErrorResultData errorData)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = errorData.Code.ToMessageCode(MessageCode.UnexpectedError);
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await Assert.ThrowsExceptionAsync<AsyncOperationException>(() => backgroundOperation.Task);
            //}
            //else
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = string.Empty;
            //    expectedPercentComplete = updateData.PercentComplete.HasValue ? 100 : null;
            //    expectedCode = completeData.Code;
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    double actualResult = await backgroundOperation.Task;
            //    Assert.AreEqual(completeData.Result, actualResult);
            //}

            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(expectedStatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(expectedCurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(expectedPercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (expectedPercentComplete.HasValue)
            //    Assert.AreEqual(expectedPercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //if (observed.Value is IBackgroundOperationCompletedEvent completedEvent)
            //{
            //    if (completeData.Cancel)
            //    {
            //        Assert.IsNotInstanceOfType(completedEvent, typeof(IBackgroundOperationResultEvent<double>));
            //        Assert.IsFalse(completedEvent.RanToCompletion);
            //    }
            //    else
            //    {
            //        Assert.IsTrue(completedEvent.RanToCompletion);
            //        if (completedEvent is IBackgroundOperationResultEvent<double> resultEvent)
            //            Assert.AreEqual(completeData.Result, resultEvent.Result);
            //        else
            //            Assert.Fail("observed.Value is not IBackgroundOperationResultEvent<double>");
            //    }
            //    Assert.IsNull(completedEvent.Error);
            //    Assert.AreEqual(initializeData.Activity, completedEvent.Activity);
            //    Assert.AreEqual(expectedStatusDescription, completedEvent.StatusDescription);
            //    Assert.AreEqual(expectedCurrentOperation, completedEvent.CurrentOperation);
            //    Assert.AreEqual(expectedPercentComplete.HasValue, completedEvent.PercentComplete.HasValue);
            //    if (expectedPercentComplete.HasValue)
            //        Assert.AreEqual(expectedPercentComplete.Value, completedEvent.PercentComplete.Value);
            //    Assert.AreEqual(expectedCode.HasValue, completedEvent.Code);
            //    if (expectedCode.HasValue)
            //        Assert.AreEqual(expectedCode.Value, completedEvent.Code.Value);
            //    Assert.AreEqual(backgroundOperation.OperationId, completedEvent.OperationId);
            //    Assert.IsFalse(completedEvent.ParentId.HasValue);
            //}
            //else
            //    Assert.Fail("observed.Value is not IBackgroundOperationCompletedEvent");

            //#endregion

            //#region Test Observer Completion

            //Thread.Sleep(100);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsTrue(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //Assert.IsNull(observed.Value);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion
        }

        [DataTestMethod]
        [DynamicData(nameof(GetAsyncFuncResultData), DynamicDataSourceType.Method)]
        [Priority(10)]
        public async Task InvokeAsyncFuncCompletedTest(InitializeData initializeData, UpdateData updateData, IResultData completeData)
        {
            Assert.Inconclusive();
            //using AutoResetEvent fgEvent = new(false);
            //using AutoResetEvent bgEvent = new(false);
            //Task<double> asyncMethodDelegate(IBackgroundProgress<IAsyncActivity> progress) =>
            //    TestFunc(fgEvent, bgEvent, initializeData, updateData, completeData, progress);
            //IBackgroundOperationResultEvent<double> onCompleted(IBackgroundFunc<double> backgroundOperation) =>
            //        new BackgroundProcessResultEventArgs<double>(backgroundOperation, completeData.Code, null, backgroundOperation.Task.Result); ;

            //IAsyncActivityService service = Hosting.GetAsyncActivityService();
            //if (service is null)
            //    throw new AssertInconclusiveException();
            //ObserverHelper<IAsyncActivity> operationObserver = new();
            //IBackgroundFunc<double> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, onCompleted, initializeData.Activity, initializeData.StatusDescription);

            //#region Test Initial Progress Properties

            //bgEvent.WaitOne(); // Wait until bg operation being executed
            //using IDisposable subscription = backgroundOperation.Subscribe(operationObserver);
            //Assert.IsNotNull(backgroundOperation);
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(initializeData.StatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            //Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            //fgEvent.Set(); // Signal that we've tested initial status properties

            //#endregion

            //#region Test Progress Update

            //bgEvent.WaitOne(); // Wait until status has been updated
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    backgroundOperation.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out Observed<IAsyncActivity> observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //IAsyncActivity progressEvent = observed.Value;
            //Assert.IsNotNull(progressEvent);
            //Assert.AreEqual(initializeData.Activity, progressEvent.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    progressEvent.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, progressEvent.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, progressEvent.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, progressEvent.PercentComplete.Value);
            //Assert.AreEqual(updateData.Code.HasValue, progressEvent.Code.HasValue);
            //if (updateData.Code.HasValue)
            //    Assert.AreEqual(updateData.Code.Value, progressEvent.Code.Value);
            //Assert.AreEqual(backgroundOperation.OperationId, progressEvent.OperationId);
            //Assert.IsFalse(progressEvent.ParentId.HasValue);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion

            //#region Test Operation Completion

            //string expectedStatusDescription;
            //string expectedCurrentOperation;
            //byte? expectedPercentComplete;
            //MessageCode? expectedCode;
            //if (completeData.Cancel)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = updateData.Code;
            //    backgroundOperation.Cancel();
            //    fgEvent.Set(); // Signal that we've canceled the operation
            //    await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => backgroundOperation.Task);
            //}
            //else if (completeData is ErrorResultData errorData)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = errorData.Code.ToMessageCode(MessageCode.UnexpectedError);
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await Assert.ThrowsExceptionAsync<AsyncOperationException>(() => backgroundOperation.Task);
            //}
            //else
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = string.Empty;
            //    expectedPercentComplete = updateData.PercentComplete.HasValue ? 100 : null;
            //    expectedCode = completeData.Code;
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    double actualResult = await backgroundOperation.Task;
            //    Assert.AreEqual(completeData.Result, actualResult);
            //}

            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(expectedStatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(expectedCurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(expectedPercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (expectedPercentComplete.HasValue)
            //    Assert.AreEqual(expectedPercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //if (observed.Value is IBackgroundOperationCompletedEvent completedEvent)
            //{
            //    if (completeData.Cancel)
            //    {
            //        Assert.IsNotInstanceOfType(completedEvent, typeof(IBackgroundOperationResultEvent<double>));
            //        Assert.IsFalse(completedEvent.RanToCompletion);
            //    }
            //    else
            //    {
            //        Assert.IsTrue(completedEvent.RanToCompletion);
            //        if (completedEvent is IBackgroundOperationResultEvent<double> resultEvent)
            //            Assert.AreEqual(completeData.Result, resultEvent.Result);
            //        else
            //            Assert.Fail("observed.Value is not IBackgroundOperationResultEvent<double>");
            //    }
            //    Assert.IsNull(completedEvent.Error);
            //    Assert.AreEqual(initializeData.Activity, completedEvent.Activity);
            //    Assert.AreEqual(expectedStatusDescription, completedEvent.StatusDescription);
            //    Assert.AreEqual(expectedCurrentOperation, completedEvent.CurrentOperation);
            //    Assert.AreEqual(expectedPercentComplete.HasValue, completedEvent.PercentComplete.HasValue);
            //    if (expectedPercentComplete.HasValue)
            //        Assert.AreEqual(expectedPercentComplete.Value, completedEvent.PercentComplete.Value);
            //    Assert.AreEqual(expectedCode.HasValue, completedEvent.Code);
            //    if (expectedCode.HasValue)
            //        Assert.AreEqual(expectedCode.Value, completedEvent.Code.Value);
            //    Assert.AreEqual(backgroundOperation.OperationId, completedEvent.OperationId);
            //    Assert.IsFalse(completedEvent.ParentId.HasValue);
            //}
            //else
            //    Assert.Fail("observed.Value is not IBackgroundOperationCompletedEvent");

            //#endregion

            //#region Test Observer Completion

            //Thread.Sleep(100);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsTrue(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //Assert.IsNull(observed.Value);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion
        }

        [DataTestMethod]
        [DynamicData(nameof(GetAsyncFuncResultData), DynamicDataSourceType.Method)]
        [Priority(10)]
        public async Task InvokeAsyncFuncTokenTest(InitializeData initializeData, UpdateData updateData, IResultData completeData)
        {
            Assert.Inconclusive();
            //using AutoResetEvent fgEvent = new(false);
            //using AutoResetEvent bgEvent = new(false);
            //Task<double> asyncMethodDelegate(IBackgroundProgress<IAsyncActivity> progress) =>
            //    TestFunc(fgEvent, bgEvent, initializeData, updateData, completeData, progress);

            //IAsyncActivityService service = Hosting.GetAsyncActivityService();
            //if (service is null)
            //    throw new AssertInconclusiveException();
            //using CancellationTokenSource tokenSource = new();
            //ObserverHelper<IAsyncActivity> operationObserver = new();
            //IBackgroundFunc<double> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, initializeData.Activity, initializeData.StatusDescription, tokenSource.Token);

            //#region Test Initial Progress Properties

            //bgEvent.WaitOne(); // Wait until bg operation being executed
            //using IDisposable subscription = backgroundOperation.Subscribe(operationObserver);
            //Assert.IsNotNull(backgroundOperation);
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(initializeData.StatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            //Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            //fgEvent.Set(); // Signal that we've tested initial status properties

            //#endregion

            //#region Test Progress Update

            //bgEvent.WaitOne(); // Wait until status has been updated
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    backgroundOperation.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out Observed<IAsyncActivity> observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //IAsyncActivity progressEvent = observed.Value;
            //Assert.IsNotNull(progressEvent);
            //Assert.AreEqual(initializeData.Activity, progressEvent.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    progressEvent.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, progressEvent.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, progressEvent.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, progressEvent.PercentComplete.Value);
            //Assert.AreEqual(updateData.Code.HasValue, progressEvent.Code.HasValue);
            //if (updateData.Code.HasValue)
            //    Assert.AreEqual(updateData.Code.Value, progressEvent.Code.Value);
            //Assert.AreEqual(backgroundOperation.OperationId, progressEvent.OperationId);
            //Assert.IsFalse(progressEvent.ParentId.HasValue);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion

            //#region Test Operation Completion

            //string expectedStatusDescription;
            //string expectedCurrentOperation;
            //byte? expectedPercentComplete;
            //MessageCode? expectedCode;
            //if (completeData.Cancel)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = updateData.Code;
            //    tokenSource.Cancel();
            //    fgEvent.Set(); // Signal that we've canceled the operation
            //    await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => backgroundOperation.Task);
            //}
            //else if (completeData is ErrorResultData errorData)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = errorData.Code.ToMessageCode(MessageCode.UnexpectedError);
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await Assert.ThrowsExceptionAsync<AsyncOperationException>(() => backgroundOperation.Task);
            //}
            //else
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = string.Empty;
            //    expectedPercentComplete = updateData.PercentComplete.HasValue ? 100 : null;
            //    expectedCode = completeData.Code;
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    double actualResult = await backgroundOperation.Task;
            //    Assert.AreEqual(completeData.Result, actualResult);
            //}

            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(expectedStatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(expectedCurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(expectedPercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (expectedPercentComplete.HasValue)
            //    Assert.AreEqual(expectedPercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //if (observed.Value is IBackgroundOperationCompletedEvent completedEvent)
            //{
            //    if (completeData.Cancel)
            //    {
            //        Assert.IsNotInstanceOfType(completedEvent, typeof(IBackgroundOperationResultEvent<double>));
            //        Assert.IsFalse(completedEvent.RanToCompletion);
            //    }
            //    else
            //    {
            //        Assert.IsTrue(completedEvent.RanToCompletion);
            //        if (completedEvent is IBackgroundOperationResultEvent<double> resultEvent)
            //            Assert.AreEqual(completeData.Result, resultEvent.Result);
            //        else
            //            Assert.Fail("observed.Value is not IBackgroundOperationResultEvent<double>");
            //    }
            //    Assert.IsNull(completedEvent.Error);
            //    Assert.AreEqual(initializeData.Activity, completedEvent.Activity);
            //    Assert.AreEqual(expectedStatusDescription, completedEvent.StatusDescription);
            //    Assert.AreEqual(expectedCurrentOperation, completedEvent.CurrentOperation);
            //    Assert.AreEqual(expectedPercentComplete.HasValue, completedEvent.PercentComplete.HasValue);
            //    if (expectedPercentComplete.HasValue)
            //        Assert.AreEqual(expectedPercentComplete.Value, completedEvent.PercentComplete.Value);
            //    Assert.AreEqual(expectedCode.HasValue, completedEvent.Code);
            //    if (expectedCode.HasValue)
            //        Assert.AreEqual(expectedCode.Value, completedEvent.Code.Value);
            //    Assert.AreEqual(backgroundOperation.OperationId, completedEvent.OperationId);
            //    Assert.IsFalse(completedEvent.ParentId.HasValue);
            //}
            //else
            //    Assert.Fail("observed.Value is not IBackgroundOperationCompletedEvent");

            //#endregion

            //#region Test Observer Completion

            //Thread.Sleep(100);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsTrue(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //Assert.IsNull(observed.Value);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion
        }

        [DataTestMethod]
        [DynamicData(nameof(GetAsyncFuncResultData), DynamicDataSourceType.Method)]
        [Priority(10)]
        public async Task InvokeAsyncFuncTest(InitializeData initializeData, UpdateData updateData, IResultData completeData)
        {
            Assert.Inconclusive();
            //using AutoResetEvent fgEvent = new(false);
            //using AutoResetEvent bgEvent = new(false);
            //Task<double> asyncMethodDelegate(IBackgroundProgress<IAsyncActivity> progress) =>
            //    TestFunc(fgEvent, bgEvent, initializeData, updateData, completeData, progress);

            //IAsyncActivityService service = Hosting.GetAsyncActivityService();
            //if (service is null)
            //    throw new AssertInconclusiveException();
            //ObserverHelper<IAsyncActivity> operationObserver = new();
            //IBackgroundFunc<double> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, initializeData.Activity, initializeData.StatusDescription);

            //#region Test Initial Progress Properties

            //bgEvent.WaitOne(); // Wait until bg operation being executed
            //using IDisposable subscription = backgroundOperation.Subscribe(operationObserver);
            //Assert.IsNotNull(backgroundOperation);
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(initializeData.StatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            //Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            //fgEvent.Set(); // Signal that we've tested initial status properties

            //#endregion

            //#region Test Progress Update

            //bgEvent.WaitOne(); // Wait until status has been updated
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    backgroundOperation.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out Observed<IAsyncActivity> observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //IAsyncActivity progressEvent = observed.Value;
            //Assert.IsNotNull(progressEvent);
            //Assert.AreEqual(initializeData.Activity, progressEvent.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    progressEvent.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, progressEvent.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, progressEvent.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, progressEvent.PercentComplete.Value);
            //Assert.AreEqual(updateData.Code.HasValue, progressEvent.Code.HasValue);
            //if (updateData.Code.HasValue)
            //    Assert.AreEqual(updateData.Code.Value, progressEvent.Code.Value);
            //Assert.AreEqual(backgroundOperation.OperationId, progressEvent.OperationId);
            //Assert.IsFalse(progressEvent.ParentId.HasValue);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion

            //#region Test Operation Completion

            //string expectedStatusDescription;
            //string expectedCurrentOperation;
            //byte? expectedPercentComplete;
            //MessageCode? expectedCode;
            //if (completeData.Cancel)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = updateData.Code;
            //    backgroundOperation.Cancel();
            //    fgEvent.Set(); // Signal that we've canceled the operation
            //    await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => backgroundOperation.Task);
            //}
            //else if (completeData is ErrorResultData errorData)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = errorData.Code.ToMessageCode(MessageCode.UnexpectedError);
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await Assert.ThrowsExceptionAsync<AsyncOperationException>(() => backgroundOperation.Task);
            //}
            //else
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = string.Empty;
            //    expectedPercentComplete = updateData.PercentComplete.HasValue ? 100 : null;
            //    expectedCode = completeData.Code;
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    double actualResult = await backgroundOperation.Task;
            //    Assert.AreEqual(completeData.Result, actualResult);
            //}

            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(expectedStatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(expectedCurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(expectedPercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (expectedPercentComplete.HasValue)
            //    Assert.AreEqual(expectedPercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //if (observed.Value is IBackgroundOperationCompletedEvent completedEvent)
            //{
            //    if (completeData.Cancel)
            //    {
            //        Assert.IsNotInstanceOfType(completedEvent, typeof(IBackgroundOperationResultEvent<double>));
            //        Assert.IsFalse(completedEvent.RanToCompletion);
            //    }
            //    else
            //    {
            //        Assert.IsTrue(completedEvent.RanToCompletion);
            //        if (completedEvent is IBackgroundOperationResultEvent<double> resultEvent)
            //            Assert.AreEqual(completeData.Result, resultEvent.Result);
            //        else
            //            Assert.Fail("observed.Value is not IBackgroundOperationResultEvent<double>");
            //    }
            //    Assert.IsNull(completedEvent.Error);
            //    Assert.AreEqual(initializeData.Activity, completedEvent.Activity);
            //    Assert.AreEqual(expectedStatusDescription, completedEvent.StatusDescription);
            //    Assert.AreEqual(expectedCurrentOperation, completedEvent.CurrentOperation);
            //    Assert.AreEqual(expectedPercentComplete.HasValue, completedEvent.PercentComplete.HasValue);
            //    if (expectedPercentComplete.HasValue)
            //        Assert.AreEqual(expectedPercentComplete.Value, completedEvent.PercentComplete.Value);
            //    Assert.AreEqual(expectedCode.HasValue, completedEvent.Code);
            //    if (expectedCode.HasValue)
            //        Assert.AreEqual(expectedCode.Value, completedEvent.Code.Value);
            //    Assert.AreEqual(backgroundOperation.OperationId, completedEvent.OperationId);
            //    Assert.IsFalse(completedEvent.ParentId.HasValue);
            //}
            //else
            //    Assert.Fail("observed.Value is not IBackgroundOperationCompletedEvent");

            //#endregion

            //#region Test Observer Completion

            //Thread.Sleep(100);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsTrue(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //Assert.IsNull(observed.Value);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion
        }

        [DataTestMethod]
        [DynamicData(nameof(GetAsyncActionCompletedStateData), DynamicDataSourceType.Method)]
        [Priority(10)]
        public async Task InvokeTimedAsyncActionCompletedStateTokenTest(InitializeStateData initializeData, UpdateData updateData, IActionCompleteData completeData)
        {
            Assert.Inconclusive();
            //using AutoResetEvent fgEvent = new(false);
            //using AutoResetEvent bgEvent = new(false);
            //Task asyncMethodDelegate(ITimedBackgroundProgress<int, ITimedBackgroundProgressEvent<int>> progress) =>
            //    TimedTestActionState(fgEvent, bgEvent, initializeData, updateData, completeData, progress);
            //ITimedBackgroundOperationCompletedEvent<int> onCompleted(ITimedBackgroundOperation<int> backgroundOperation) =>
            //        new TimedBackgroundProcessCompletedEventArgs<int>(backgroundOperation, completeData.Code, null, !backgroundOperation.Task.IsCanceled, string.IsNullOrWhiteSpace(completeData.StatusDescription) ? backgroundOperation.StatusDescription : completeData.StatusDescription); ;

            //IAsyncActivityService service = Hosting.GetAsyncActivityService();
            //if (service is null)
            //    throw new AssertInconclusiveException();
            //using CancellationTokenSource tokenSource = new();
            //ObserverHelper<ITimedBackgroundProgressEvent<int>> operationObserver = new();
            //ITimedBackgroundOperation<int> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, onCompleted, initializeData.Activity, initializeData.StatusDescription, initializeData.State, tokenSource.Token);

            //#region Test Initial Progress Properties

            //bgEvent.WaitOne(); // Wait until bg operation being executed
            //using IDisposable subscription = backgroundOperation.Subscribe(operationObserver);
            //Assert.IsNotNull(backgroundOperation);
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(initializeData.StatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            //fgEvent.Set(); // Signal that we've tested initial status properties

            //#endregion

            //#region Test Progress Update

            //bgEvent.WaitOne(); // Wait until status has been updated
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    backgroundOperation.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out Observed<ITimedBackgroundProgressEvent<int>> observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //ITimedBackgroundProgressEvent<int> progressEvent = observed.Value;
            //Assert.IsNotNull(progressEvent);
            //Assert.AreEqual(initializeData.Activity, progressEvent.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    progressEvent.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, progressEvent.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, progressEvent.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, progressEvent.PercentComplete.Value);
            //Assert.AreEqual(updateData.Code.HasValue, progressEvent.Code.HasValue);
            //if (updateData.Code.HasValue)
            //    Assert.AreEqual(updateData.Code.Value, progressEvent.Code.Value);
            //Assert.AreEqual(initializeData.State, progressEvent.AsyncState);
            //Assert.AreEqual(backgroundOperation.OperationId, progressEvent.OperationId);
            //Assert.IsFalse(progressEvent.ParentId.HasValue);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion

            //#region Test Operation Completion

            //string expectedStatusDescription;
            //string expectedCurrentOperation;
            //byte? expectedPercentComplete;
            //MessageCode? expectedCode;
            //if (completeData.Cancel)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = updateData.Code;
            //    tokenSource.Cancel();
            //    fgEvent.Set(); // Signal that we've canceled the operation
            //    await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => backgroundOperation.Task);
            //}
            //else if (completeData is ErrorCompleteData errorData)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = errorData.Code.ToMessageCode(MessageCode.UnexpectedError);
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await Assert.ThrowsExceptionAsync<AsyncOperationException>(() => backgroundOperation.Task);
            //}
            //else
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = string.Empty;
            //    expectedPercentComplete = updateData.PercentComplete.HasValue ? 100 : null;
            //    expectedCode = completeData.Code;
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await backgroundOperation.Task;
            //}

            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(expectedStatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(expectedCurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(expectedPercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (expectedPercentComplete.HasValue)
            //    Assert.AreEqual(expectedPercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //if (observed.Value is ITimedBackgroundOperationCompletedEvent<int> completedEvent)
            //{
            //    if (completeData.Cancel)
            //        Assert.IsFalse(completedEvent.RanToCompletion);
            //    else
            //        Assert.IsTrue(completedEvent.RanToCompletion);
            //    Assert.IsNull(completedEvent.Error);
            //    Assert.AreEqual(initializeData.Activity, completedEvent.Activity);
            //    Assert.AreEqual(expectedStatusDescription, completedEvent.StatusDescription);
            //    Assert.AreEqual(expectedCurrentOperation, completedEvent.CurrentOperation);
            //    Assert.AreEqual(expectedPercentComplete.HasValue, completedEvent.PercentComplete.HasValue);
            //    if (expectedPercentComplete.HasValue)
            //        Assert.AreEqual(expectedPercentComplete.Value, completedEvent.PercentComplete.Value);
            //    Assert.AreEqual(expectedCode.HasValue, completedEvent.Code);
            //    if (expectedCode.HasValue)
            //        Assert.AreEqual(expectedCode.Value, completedEvent.Code.Value);
            //    Assert.AreEqual(initializeData.State, completedEvent.AsyncState);
            //    Assert.AreEqual(backgroundOperation.OperationId, completedEvent.OperationId);
            //    Assert.IsFalse(completedEvent.ParentId.HasValue);
            //}
            //else
            //    Assert.Fail("observed.Value is not ITimedBackgroundOperationCompletedEvent<int>");

            //#endregion

            //#region Test Observer Completion

            //Thread.Sleep(100);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsTrue(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //Assert.IsNull(observed.Value);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion
        }

        [DataTestMethod]
        [DynamicData(nameof(GetAsyncActionCompletedStateData), DynamicDataSourceType.Method)]
        [Priority(10)]
        public async Task InvokeTimedAsyncActionCompletedStateTest(InitializeStateData initializeData, UpdateData updateData, IActionCompleteData completeData)
        {
            //Assert.Inconclusive();
            //using AutoResetEvent fgEvent = new(false);
            //using AutoResetEvent bgEvent = new(false);
            //Task asyncMethodDelegate(ITimedBackgroundProgress<int, ITimedBackgroundProgressEvent<int>> progress) =>
            //    TimedTestActionState(fgEvent, bgEvent, initializeData, updateData, completeData, progress);
            //ITimedBackgroundOperationCompletedEvent<int> onCompleted(ITimedBackgroundOperation<int> backgroundOperation) =>
            //        new TimedBackgroundProcessCompletedEventArgs<int>(backgroundOperation, completeData.Code, null, !backgroundOperation.Task.IsCanceled, string.IsNullOrWhiteSpace(completeData.StatusDescription) ? backgroundOperation.StatusDescription : completeData.StatusDescription); ;

            //IAsyncActivityService service = Hosting.GetAsyncActivityService();
            //if (service is null)
            //    throw new AssertInconclusiveException();
            //ObserverHelper<ITimedBackgroundProgressEvent<int>> operationObserver = new();
            //ITimedBackgroundOperation<int> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, onCompleted, initializeData.Activity, initializeData.StatusDescription, initializeData.State);

            //#region Test Initial Progress Properties

            //bgEvent.WaitOne(); // Wait until bg operation being executed
            //using IDisposable subscription = backgroundOperation.Subscribe(operationObserver);
            //Assert.IsNotNull(backgroundOperation);
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(initializeData.StatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            //fgEvent.Set(); // Signal that we've tested initial status properties

            //#endregion

            //#region Test Progress Update

            //bgEvent.WaitOne(); // Wait until status has been updated
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    backgroundOperation.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out Observed<ITimedBackgroundProgressEvent<int>> observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //ITimedBackgroundProgressEvent<int> progressEvent = observed.Value;
            //Assert.IsNotNull(progressEvent);
            //Assert.AreEqual(initializeData.Activity, progressEvent.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    progressEvent.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, progressEvent.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, progressEvent.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, progressEvent.PercentComplete.Value);
            //Assert.AreEqual(updateData.Code.HasValue, progressEvent.Code.HasValue);
            //if (updateData.Code.HasValue)
            //    Assert.AreEqual(updateData.Code.Value, progressEvent.Code.Value);
            //Assert.AreEqual(initializeData.State, progressEvent.AsyncState);
            //Assert.AreEqual(backgroundOperation.OperationId, progressEvent.OperationId);
            //Assert.IsFalse(progressEvent.ParentId.HasValue);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion

            //#region Test Operation Completion

            //string expectedStatusDescription;
            //string expectedCurrentOperation;
            //byte? expectedPercentComplete;
            //MessageCode? expectedCode;
            //if (completeData.Cancel)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = updateData.Code;
            //    backgroundOperation.Cancel();
            //    fgEvent.Set(); // Signal that we've canceled the operation
            //    await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => backgroundOperation.Task);
            //}
            //else if (completeData is ErrorCompleteData errorData)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = errorData.Code.ToMessageCode(MessageCode.UnexpectedError);
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await Assert.ThrowsExceptionAsync<AsyncOperationException>(() => backgroundOperation.Task);
            //}
            //else
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = string.Empty;
            //    expectedPercentComplete = updateData.PercentComplete.HasValue ? 100 : null;
            //    expectedCode = completeData.Code;
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await backgroundOperation.Task;
            //}

            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(expectedStatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(expectedCurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(expectedPercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (expectedPercentComplete.HasValue)
            //    Assert.AreEqual(expectedPercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //if (observed.Value is ITimedBackgroundOperationCompletedEvent<int> completedEvent)
            //{
            //    if (completeData.Cancel)
            //        Assert.IsFalse(completedEvent.RanToCompletion);
            //    else
            //        Assert.IsTrue(completedEvent.RanToCompletion);
            //    Assert.IsNull(completedEvent.Error);
            //    Assert.AreEqual(initializeData.Activity, completedEvent.Activity);
            //    Assert.AreEqual(expectedStatusDescription, completedEvent.StatusDescription);
            //    Assert.AreEqual(expectedCurrentOperation, completedEvent.CurrentOperation);
            //    Assert.AreEqual(expectedPercentComplete.HasValue, completedEvent.PercentComplete.HasValue);
            //    if (expectedPercentComplete.HasValue)
            //        Assert.AreEqual(expectedPercentComplete.Value, completedEvent.PercentComplete.Value);
            //    Assert.AreEqual(expectedCode.HasValue, completedEvent.Code);
            //    if (expectedCode.HasValue)
            //        Assert.AreEqual(expectedCode.Value, completedEvent.Code.Value);
            //    Assert.AreEqual(initializeData.State, completedEvent.AsyncState);
            //    Assert.AreEqual(backgroundOperation.OperationId, completedEvent.OperationId);
            //    Assert.IsFalse(completedEvent.ParentId.HasValue);
            //}
            //else
            //    Assert.Fail("observed.Value is not ITimedBackgroundOperationCompletedEvent<int>");

            //#endregion

            //#region Test Observer Completion

            //Thread.Sleep(100);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsTrue(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //Assert.IsNull(observed.Value);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion
        }

        [DataTestMethod]
        [DynamicData(nameof(GetAsyncActionCompletedStateData), DynamicDataSourceType.Method)]
        [Priority(10)]
        public async Task InvokeTimedAsyncActionStateTokenTest(InitializeStateData initializeData, UpdateData updateData, IActionCompleteData completeData)
        {
            Assert.Inconclusive();
            //using AutoResetEvent fgEvent = new(false);
            //using AutoResetEvent bgEvent = new(false);
            //Task asyncMethodDelegate(ITimedBackgroundProgress<int, ITimedBackgroundProgressEvent<int>> progress) =>
            //    TimedTestActionState(fgEvent, bgEvent, initializeData, updateData, completeData, progress);

            //IAsyncActivityService service = Hosting.GetAsyncActivityService();
            //if (service is null)
            //    throw new AssertInconclusiveException();
            //using CancellationTokenSource tokenSource = new();
            //ObserverHelper<ITimedBackgroundProgressEvent<int>> operationObserver = new();
            //ITimedBackgroundOperation<int> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, initializeData.Activity, initializeData.StatusDescription, initializeData.State, tokenSource.Token);

            //#region Test Initial Progress Properties

            //bgEvent.WaitOne(); // Wait until bg operation being executed
            //using IDisposable subscription = backgroundOperation.Subscribe(operationObserver);
            //Assert.IsNotNull(backgroundOperation);
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(initializeData.StatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            //fgEvent.Set(); // Signal that we've tested initial status properties

            //#endregion

            //#region Test Progress Update

            //bgEvent.WaitOne(); // Wait until status has been updated
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    backgroundOperation.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out Observed<ITimedBackgroundProgressEvent<int>> observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //ITimedBackgroundProgressEvent<int> progressEvent = observed.Value;
            //Assert.IsNotNull(progressEvent);
            //Assert.AreEqual(initializeData.Activity, progressEvent.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    progressEvent.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, progressEvent.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, progressEvent.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, progressEvent.PercentComplete.Value);
            //Assert.AreEqual(updateData.Code.HasValue, progressEvent.Code.HasValue);
            //if (updateData.Code.HasValue)
            //    Assert.AreEqual(updateData.Code.Value, progressEvent.Code.Value);
            //Assert.AreEqual(initializeData.State, progressEvent.AsyncState);
            //Assert.AreEqual(backgroundOperation.OperationId, progressEvent.OperationId);
            //Assert.IsFalse(progressEvent.ParentId.HasValue);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion

            //#region Test Operation Completion

            //string expectedStatusDescription;
            //string expectedCurrentOperation;
            //byte? expectedPercentComplete;
            //MessageCode? expectedCode;
            //if (completeData.Cancel)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = updateData.Code;
            //    tokenSource.Cancel();
            //    fgEvent.Set(); // Signal that we've canceled the operation
            //    await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => backgroundOperation.Task);
            //}
            //else if (completeData is ErrorCompleteData errorData)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = errorData.Code.ToMessageCode(MessageCode.UnexpectedError);
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await Assert.ThrowsExceptionAsync<AsyncOperationException>(() => backgroundOperation.Task);
            //}
            //else
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = string.Empty;
            //    expectedPercentComplete = updateData.PercentComplete.HasValue ? 100 : null;
            //    expectedCode = completeData.Code;
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await backgroundOperation.Task;
            //}

            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(expectedStatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(expectedCurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(expectedPercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (expectedPercentComplete.HasValue)
            //    Assert.AreEqual(expectedPercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //if (observed.Value is ITimedBackgroundOperationCompletedEvent<int> completedEvent)
            //{
            //    if (completeData.Cancel)
            //        Assert.IsFalse(completedEvent.RanToCompletion);
            //    else
            //        Assert.IsTrue(completedEvent.RanToCompletion);
            //    Assert.IsNull(completedEvent.Error);
            //    Assert.AreEqual(initializeData.Activity, completedEvent.Activity);
            //    Assert.AreEqual(expectedStatusDescription, completedEvent.StatusDescription);
            //    Assert.AreEqual(expectedCurrentOperation, completedEvent.CurrentOperation);
            //    Assert.AreEqual(expectedPercentComplete.HasValue, completedEvent.PercentComplete.HasValue);
            //    if (expectedPercentComplete.HasValue)
            //        Assert.AreEqual(expectedPercentComplete.Value, completedEvent.PercentComplete.Value);
            //    Assert.AreEqual(expectedCode.HasValue, completedEvent.Code);
            //    if (expectedCode.HasValue)
            //        Assert.AreEqual(expectedCode.Value, completedEvent.Code.Value);
            //    Assert.AreEqual(initializeData.State, completedEvent.AsyncState);
            //    Assert.AreEqual(backgroundOperation.OperationId, completedEvent.OperationId);
            //    Assert.IsFalse(completedEvent.ParentId.HasValue);
            //}
            //else
            //    Assert.Fail("observed.Value is not ITimedBackgroundOperationCompletedEvent<int>");

            //#endregion

            //#region Test Observer Completion

            //Thread.Sleep(100);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsTrue(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //Assert.IsNull(observed.Value);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion
        }

        [DataTestMethod]
        [DynamicData(nameof(GetAsyncActionCompletedStateData), DynamicDataSourceType.Method)]
        [Priority(10)]
        public async Task InvokeTimedAsyncActionStateTest(InitializeStateData initializeData, UpdateData updateData, IActionCompleteData completeData)
        {
            Assert.Inconclusive();
            //using AutoResetEvent fgEvent = new(false);
            //using AutoResetEvent bgEvent = new(false);
            //Task asyncMethodDelegate(ITimedBackgroundProgress<int, ITimedBackgroundProgressEvent<int>> progress) =>
            //    TimedTestActionState(fgEvent, bgEvent, initializeData, updateData, completeData, progress);

            //IAsyncActivityService service = Hosting.GetAsyncActivityService();
            //if (service is null)
            //    throw new AssertInconclusiveException();
            //ObserverHelper<ITimedBackgroundProgressEvent<int>> operationObserver = new();
            //ITimedBackgroundOperation<int> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, initializeData.Activity, initializeData.StatusDescription, initializeData.State);

            //#region Test Initial Progress Properties

            //bgEvent.WaitOne(); // Wait until bg operation being executed
            //using IDisposable subscription = backgroundOperation.Subscribe(operationObserver);
            //Assert.IsNotNull(backgroundOperation);
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(initializeData.StatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            //fgEvent.Set(); // Signal that we've tested initial status properties

            //#endregion

            //#region Test Progress Update

            //bgEvent.WaitOne(); // Wait until status has been updated
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    backgroundOperation.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out Observed<ITimedBackgroundProgressEvent<int>> observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //ITimedBackgroundProgressEvent<int> progressEvent = observed.Value;
            //Assert.IsNotNull(progressEvent);
            //Assert.AreEqual(initializeData.Activity, progressEvent.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    progressEvent.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, progressEvent.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, progressEvent.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, progressEvent.PercentComplete.Value);
            //Assert.AreEqual(updateData.Code.HasValue, progressEvent.Code.HasValue);
            //if (updateData.Code.HasValue)
            //    Assert.AreEqual(updateData.Code.Value, progressEvent.Code.Value);
            //Assert.AreEqual(initializeData.State, progressEvent.AsyncState);
            //Assert.AreEqual(backgroundOperation.OperationId, progressEvent.OperationId);
            //Assert.IsFalse(progressEvent.ParentId.HasValue);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion

            //#region Test Operation Completion

            //string expectedStatusDescription;
            //string expectedCurrentOperation;
            //byte? expectedPercentComplete;
            //MessageCode? expectedCode;
            //if (completeData.Cancel)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = updateData.Code;
            //    backgroundOperation.Cancel();
            //    fgEvent.Set(); // Signal that we've canceled the operation
            //    await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => backgroundOperation.Task);
            //}
            //else if (completeData is ErrorCompleteData errorData)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = errorData.Code.ToMessageCode(MessageCode.UnexpectedError);
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await Assert.ThrowsExceptionAsync<AsyncOperationException>(() => backgroundOperation.Task);
            //}
            //else
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = string.Empty;
            //    expectedPercentComplete = updateData.PercentComplete.HasValue ? 100 : null;
            //    expectedCode = completeData.Code;
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await backgroundOperation.Task;
            //}

            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(expectedStatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(expectedCurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(expectedPercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (expectedPercentComplete.HasValue)
            //    Assert.AreEqual(expectedPercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //if (observed.Value is ITimedBackgroundOperationCompletedEvent<int> completedEvent)
            //{
            //    if (completeData.Cancel)
            //        Assert.IsFalse(completedEvent.RanToCompletion);
            //    else
            //        Assert.IsTrue(completedEvent.RanToCompletion);
            //    Assert.IsNull(completedEvent.Error);
            //    Assert.AreEqual(initializeData.Activity, completedEvent.Activity);
            //    Assert.AreEqual(expectedStatusDescription, completedEvent.StatusDescription);
            //    Assert.AreEqual(expectedCurrentOperation, completedEvent.CurrentOperation);
            //    Assert.AreEqual(expectedPercentComplete.HasValue, completedEvent.PercentComplete.HasValue);
            //    if (expectedPercentComplete.HasValue)
            //        Assert.AreEqual(expectedPercentComplete.Value, completedEvent.PercentComplete.Value);
            //    Assert.AreEqual(expectedCode.HasValue, completedEvent.Code);
            //    if (expectedCode.HasValue)
            //        Assert.AreEqual(expectedCode.Value, completedEvent.Code.Value);
            //    Assert.AreEqual(initializeData.State, completedEvent.AsyncState);
            //    Assert.AreEqual(backgroundOperation.OperationId, completedEvent.OperationId);
            //    Assert.IsFalse(completedEvent.ParentId.HasValue);
            //}
            //else
            //    Assert.Fail("observed.Value is not ITimedBackgroundOperationCompletedEvent<int>");

            //#endregion

            //#region Test Observer Completion

            //Thread.Sleep(100);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsTrue(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //Assert.IsNull(observed.Value);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion
        }

        [DataTestMethod]
        [DynamicData(nameof(GetAsyncActionCompletedData), DynamicDataSourceType.Method)]
        [Priority(10)]
        public async Task InvokeTimedAsyncActionCompletedTokenTest(InitializeData initializeData, UpdateData updateData, IActionCompleteData completeData)
        {
            Assert.Inconclusive();
            //using AutoResetEvent fgEvent = new(false);
            //using AutoResetEvent bgEvent = new(false);
            //Task asyncMethodDelegate(IActivityProgress progress) =>
            //    TimedTestAction(fgEvent, bgEvent, initializeData, updateData, completeData, progress);
            //ITimedBackgroundOperationCompletedEvent onCompleted(ITimedBackgroundOperation backgroundOperation) =>
            //        new TimedBackgroundProcessCompletedEventArgs(backgroundOperation, completeData.Code, null, !backgroundOperation.Task.IsCanceled); ;

            //IAsyncActivityService service = Hosting.GetAsyncActivityService();
            //if (service is null)
            //    throw new AssertInconclusiveException();
            //using CancellationTokenSource tokenSource = new();
            //ObserverHelper<ITimedBackgroundProgressEvent> operationObserver = new();
            //ITimedBackgroundOperation backgroundOperation = service.InvokeAsync(asyncMethodDelegate, onCompleted, initializeData.Activity, initializeData.StatusDescription, tokenSource.Token);

            //#region Test Initial Progress Properties

            //bgEvent.WaitOne(); // Wait until bg operation being executed
            //using IDisposable subscription = backgroundOperation.Subscribe(operationObserver);
            //Assert.IsNotNull(backgroundOperation);
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(initializeData.StatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            //Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            //fgEvent.Set(); // Signal that we've tested initial status properties

            //#endregion

            //#region Test Progress Update

            //bgEvent.WaitOne(); // Wait until status has been updated
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    backgroundOperation.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out Observed<ITimedBackgroundProgressEvent> observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //ITimedBackgroundProgressEvent progressEvent = observed.Value;
            //Assert.IsNotNull(progressEvent);
            //Assert.AreEqual(initializeData.Activity, progressEvent.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    progressEvent.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, progressEvent.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, progressEvent.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, progressEvent.PercentComplete.Value);
            //Assert.AreEqual(updateData.Code.HasValue, progressEvent.Code.HasValue);
            //if (updateData.Code.HasValue)
            //    Assert.AreEqual(updateData.Code.Value, progressEvent.Code.Value);
            //Assert.AreEqual(backgroundOperation.OperationId, progressEvent.OperationId);
            //Assert.IsFalse(progressEvent.ParentId.HasValue);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion

            //#region Test Operation Completion

            //string expectedStatusDescription;
            //string expectedCurrentOperation;
            //byte? expectedPercentComplete;
            //MessageCode? expectedCode;
            //if (completeData.Cancel)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = updateData.Code;
            //    tokenSource.Cancel();
            //    fgEvent.Set(); // Signal that we've canceled the operation
            //    await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => backgroundOperation.Task);
            //}
            //else if (completeData is ErrorCompleteData errorData)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = errorData.Code.ToMessageCode(MessageCode.UnexpectedError);
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await Assert.ThrowsExceptionAsync<AsyncOperationException>(() => backgroundOperation.Task);
            //}
            //else
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = string.Empty;
            //    expectedPercentComplete = updateData.PercentComplete.HasValue ? 100 : null;
            //    expectedCode = completeData.Code;
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await backgroundOperation.Task;
            //}

            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(expectedStatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(expectedCurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(expectedPercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (expectedPercentComplete.HasValue)
            //    Assert.AreEqual(expectedPercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //if (observed.Value is ITimedBackgroundOperationCompletedEvent completedEvent)
            //{
            //    if (completeData.Cancel)
            //        Assert.IsFalse(completedEvent.RanToCompletion);
            //    else
            //        Assert.IsTrue(completedEvent.RanToCompletion);
            //    Assert.IsNull(completedEvent.Error);
            //    Assert.AreEqual(initializeData.Activity, completedEvent.Activity);
            //    Assert.AreEqual(expectedStatusDescription, completedEvent.StatusDescription);
            //    Assert.AreEqual(expectedCurrentOperation, completedEvent.CurrentOperation);
            //    Assert.AreEqual(expectedPercentComplete.HasValue, completedEvent.PercentComplete.HasValue);
            //    if (expectedPercentComplete.HasValue)
            //        Assert.AreEqual(expectedPercentComplete.Value, completedEvent.PercentComplete.Value);
            //    Assert.AreEqual(expectedCode.HasValue, completedEvent.Code);
            //    if (expectedCode.HasValue)
            //        Assert.AreEqual(expectedCode.Value, completedEvent.Code.Value);
            //    Assert.AreEqual(backgroundOperation.OperationId, completedEvent.OperationId);
            //    Assert.IsFalse(completedEvent.ParentId.HasValue);
            //}
            //else
            //    Assert.Fail("observed.Value is not ITimedBackgroundOperationCompletedEvent");

            //#endregion

            //#region Test Observer Completion

            //Thread.Sleep(100);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsTrue(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //Assert.IsNull(observed.Value);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion
        }

        [DataTestMethod]
        [DynamicData(nameof(GetAsyncActionCompletedData), DynamicDataSourceType.Method)]
        [Priority(10)]
        public async Task InvokeTimedAsyncActionCompletedTest(InitializeData initializeData, UpdateData updateData, IActionCompleteData completeData)
        {
            Assert.Inconclusive();
            //using AutoResetEvent fgEvent = new(false);
            //using AutoResetEvent bgEvent = new(false);
            //Task asyncMethodDelegate(IActivityProgress progress) =>
            //    TimedTestAction(fgEvent, bgEvent, initializeData, updateData, completeData, progress);
            //ITimedBackgroundOperationCompletedEvent onCompleted(ITimedBackgroundOperation backgroundOperation) =>
            //        new TimedBackgroundProcessCompletedEventArgs(backgroundOperation, completeData.Code, null, !backgroundOperation.Task.IsCanceled); ;

            //IAsyncActivityService service = Hosting.GetAsyncActivityService();
            //if (service is null)
            //    throw new AssertInconclusiveException();
            //ObserverHelper<ITimedBackgroundProgressEvent> operationObserver = new();
            //ITimedBackgroundOperation backgroundOperation = service.InvokeAsync(asyncMethodDelegate, onCompleted, initializeData.Activity, initializeData.StatusDescription);

            //#region Test Initial Progress Properties

            //bgEvent.WaitOne(); // Wait until bg operation being executed
            //using IDisposable subscription = backgroundOperation.Subscribe(operationObserver);
            //Assert.IsNotNull(backgroundOperation);
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(initializeData.StatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            //Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            //fgEvent.Set(); // Signal that we've tested initial status properties

            //#endregion

            //#region Test Progress Update

            //bgEvent.WaitOne(); // Wait until status has been updated
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    backgroundOperation.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out Observed<ITimedBackgroundProgressEvent> observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //ITimedBackgroundProgressEvent progressEvent = observed.Value;
            //Assert.IsNotNull(progressEvent);
            //Assert.AreEqual(initializeData.Activity, progressEvent.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    progressEvent.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, progressEvent.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, progressEvent.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, progressEvent.PercentComplete.Value);
            //Assert.AreEqual(updateData.Code.HasValue, progressEvent.Code.HasValue);
            //if (updateData.Code.HasValue)
            //    Assert.AreEqual(updateData.Code.Value, progressEvent.Code.Value);
            //Assert.AreEqual(backgroundOperation.OperationId, progressEvent.OperationId);
            //Assert.IsFalse(progressEvent.ParentId.HasValue);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion

            //#region Test Operation Completion

            //string expectedStatusDescription;
            //string expectedCurrentOperation;
            //byte? expectedPercentComplete;
            //MessageCode? expectedCode;
            //if (completeData.Cancel)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = updateData.Code;
            //    backgroundOperation.Cancel();
            //    fgEvent.Set(); // Signal that we've canceled the operation
            //    await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => backgroundOperation.Task);
            //}
            //else if (completeData is ErrorCompleteData errorData)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = errorData.Code.ToMessageCode(MessageCode.UnexpectedError);
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await Assert.ThrowsExceptionAsync<AsyncOperationException>(() => backgroundOperation.Task);
            //}
            //else
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = string.Empty;
            //    expectedPercentComplete = updateData.PercentComplete.HasValue ? 100 : null;
            //    expectedCode = completeData.Code;
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await backgroundOperation.Task;
            //}

            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(expectedStatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(expectedCurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(expectedPercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (expectedPercentComplete.HasValue)
            //    Assert.AreEqual(expectedPercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //if (observed.Value is ITimedBackgroundOperationCompletedEvent completedEvent)
            //{
            //    if (completeData.Cancel)
            //        Assert.IsFalse(completedEvent.RanToCompletion);
            //    else
            //        Assert.IsTrue(completedEvent.RanToCompletion);
            //    Assert.IsNull(completedEvent.Error);
            //    Assert.AreEqual(initializeData.Activity, completedEvent.Activity);
            //    Assert.AreEqual(expectedStatusDescription, completedEvent.StatusDescription);
            //    Assert.AreEqual(expectedCurrentOperation, completedEvent.CurrentOperation);
            //    Assert.AreEqual(expectedPercentComplete.HasValue, completedEvent.PercentComplete.HasValue);
            //    if (expectedPercentComplete.HasValue)
            //        Assert.AreEqual(expectedPercentComplete.Value, completedEvent.PercentComplete.Value);
            //    Assert.AreEqual(expectedCode.HasValue, completedEvent.Code);
            //    if (expectedCode.HasValue)
            //        Assert.AreEqual(expectedCode.Value, completedEvent.Code.Value);
            //    Assert.AreEqual(backgroundOperation.OperationId, completedEvent.OperationId);
            //    Assert.IsFalse(completedEvent.ParentId.HasValue);
            //}
            //else
            //    Assert.Fail("observed.Value is not ITimedBackgroundOperationCompletedEvent");

            //#endregion

            //#region Test Observer Completion

            //Thread.Sleep(100);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsTrue(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //Assert.IsNull(observed.Value);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion
        }

        [DataTestMethod]
        [DynamicData(nameof(GetAsyncActionCompletedData), DynamicDataSourceType.Method)]
        [Priority(10)]
        public async Task InvokeTimedAsyncActionTokenTest(InitializeData initializeData, UpdateData updateData, IActionCompleteData completeData)
        {
            //Assert.Inconclusive();
            //using AutoResetEvent fgEvent = new(false);
            //using AutoResetEvent bgEvent = new(false);
            //Task asyncMethodDelegate(IActivityProgress progress) =>
            //    TimedTestAction(fgEvent, bgEvent, initializeData, updateData, completeData, progress);

            //IAsyncActivityService service = Hosting.GetAsyncActivityService();
            //if (service is null)
            //    throw new AssertInconclusiveException();
            //using CancellationTokenSource tokenSource = new();
            //ObserverHelper<ITimedBackgroundProgressEvent> operationObserver = new();
            //ITimedBackgroundOperation backgroundOperation = service.InvokeAsync(asyncMethodDelegate, initializeData.Activity, initializeData.StatusDescription, tokenSource.Token);

            //#region Test Initial Progress Properties

            //bgEvent.WaitOne(); // Wait until bg operation being executed
            //using IDisposable subscription = backgroundOperation.Subscribe(operationObserver);
            //Assert.IsNotNull(backgroundOperation);
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(initializeData.StatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            //Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            //fgEvent.Set(); // Signal that we've tested initial status properties

            //#endregion

            //#region Test Progress Update

            //bgEvent.WaitOne(); // Wait until status has been updated
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    backgroundOperation.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out Observed<ITimedBackgroundProgressEvent> observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //ITimedBackgroundProgressEvent progressEvent = observed.Value;
            //Assert.IsNotNull(progressEvent);
            //Assert.AreEqual(initializeData.Activity, progressEvent.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    progressEvent.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, progressEvent.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, progressEvent.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, progressEvent.PercentComplete.Value);
            //Assert.AreEqual(updateData.Code.HasValue, progressEvent.Code.HasValue);
            //if (updateData.Code.HasValue)
            //    Assert.AreEqual(updateData.Code.Value, progressEvent.Code.Value);
            //Assert.AreEqual(backgroundOperation.OperationId, progressEvent.OperationId);
            //Assert.IsFalse(progressEvent.ParentId.HasValue);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion

            //#region Test Operation Completion

            //string expectedStatusDescription;
            //string expectedCurrentOperation;
            //byte? expectedPercentComplete;
            //MessageCode? expectedCode;
            //if (completeData.Cancel)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = updateData.Code;
            //    tokenSource.Cancel();
            //    fgEvent.Set(); // Signal that we've canceled the operation
            //    await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => backgroundOperation.Task);
            //}
            //else if (completeData is ErrorCompleteData errorData)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = errorData.Code.ToMessageCode(MessageCode.UnexpectedError);
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await Assert.ThrowsExceptionAsync<AsyncOperationException>(() => backgroundOperation.Task);
            //}
            //else
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = string.Empty;
            //    expectedPercentComplete = updateData.PercentComplete.HasValue ? 100 : null;
            //    expectedCode = completeData.Code;
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await backgroundOperation.Task;
            //}

            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(expectedStatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(expectedCurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(expectedPercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (expectedPercentComplete.HasValue)
            //    Assert.AreEqual(expectedPercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //if (observed.Value is ITimedBackgroundOperationCompletedEvent completedEvent)
            //{
            //    if (completeData.Cancel)
            //        Assert.IsFalse(completedEvent.RanToCompletion);
            //    else
            //        Assert.IsTrue(completedEvent.RanToCompletion);
            //    Assert.IsNull(completedEvent.Error);
            //    Assert.AreEqual(initializeData.Activity, completedEvent.Activity);
            //    Assert.AreEqual(expectedStatusDescription, completedEvent.StatusDescription);
            //    Assert.AreEqual(expectedCurrentOperation, completedEvent.CurrentOperation);
            //    Assert.AreEqual(expectedPercentComplete.HasValue, completedEvent.PercentComplete.HasValue);
            //    if (expectedPercentComplete.HasValue)
            //        Assert.AreEqual(expectedPercentComplete.Value, completedEvent.PercentComplete.Value);
            //    Assert.AreEqual(expectedCode.HasValue, completedEvent.Code);
            //    if (expectedCode.HasValue)
            //        Assert.AreEqual(expectedCode.Value, completedEvent.Code.Value);
            //    Assert.AreEqual(backgroundOperation.OperationId, completedEvent.OperationId);
            //    Assert.IsFalse(completedEvent.ParentId.HasValue);
            //}
            //else
            //    Assert.Fail("observed.Value is not ITimedBackgroundOperationCompletedEvent");

            //#endregion

            //#region Test Observer Completion

            //Thread.Sleep(100);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsTrue(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //Assert.IsNull(observed.Value);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion
        }

        [DataTestMethod]
        [DynamicData(nameof(GetAsyncActionCompletedData), DynamicDataSourceType.Method)]
        [Priority(10)]
        public async Task InvokeTimedAsyncActionTest(InitializeData initializeData, UpdateData updateData, IActionCompleteData completeData)
        {
            //Assert.Inconclusive();
            //using AutoResetEvent fgEvent = new(false);
            //using AutoResetEvent bgEvent = new(false);
            //Task asyncMethodDelegate(IActivityProgress progress) =>
            //    TimedTestAction(fgEvent, bgEvent, initializeData, updateData, completeData, progress);

            //IAsyncActivityService service = Hosting.GetAsyncActivityService();
            //if (service is null)
            //    throw new AssertInconclusiveException();
            //ObserverHelper<ITimedBackgroundProgressEvent> operationObserver = new();
            //ITimedBackgroundOperation backgroundOperation = service.InvokeAsync(asyncMethodDelegate, initializeData.Activity, initializeData.StatusDescription);

            //#region Test Initial Progress Properties

            //bgEvent.WaitOne(); // Wait until bg operation being executed
            //using IDisposable subscription = backgroundOperation.Subscribe(operationObserver);
            //Assert.IsNotNull(backgroundOperation);
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(initializeData.StatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            //Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            //fgEvent.Set(); // Signal that we've tested initial status properties

            //#endregion

            //#region Test Progress Update

            //bgEvent.WaitOne(); // Wait until status has been updated
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    backgroundOperation.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out Observed<ITimedBackgroundProgressEvent> observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //ITimedBackgroundProgressEvent progressEvent = observed.Value;
            //Assert.IsNotNull(progressEvent);
            //Assert.AreEqual(initializeData.Activity, progressEvent.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    progressEvent.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, progressEvent.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, progressEvent.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, progressEvent.PercentComplete.Value);
            //Assert.AreEqual(updateData.Code.HasValue, progressEvent.Code.HasValue);
            //if (updateData.Code.HasValue)
            //    Assert.AreEqual(updateData.Code.Value, progressEvent.Code.Value);
            //Assert.AreEqual(backgroundOperation.OperationId, progressEvent.OperationId);
            //Assert.IsFalse(progressEvent.ParentId.HasValue);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion

            //#region Test Operation Completion

            //string expectedStatusDescription;
            //string expectedCurrentOperation;
            //byte? expectedPercentComplete;
            //MessageCode? expectedCode;
            //if (completeData.Cancel)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = updateData.Code;
            //    backgroundOperation.Cancel();
            //    fgEvent.Set(); // Signal that we've canceled the operation
            //    await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => backgroundOperation.Task);
            //}
            //else if (completeData is ErrorCompleteData errorData)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = errorData.Code.ToMessageCode(MessageCode.UnexpectedError);
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await Assert.ThrowsExceptionAsync<AsyncOperationException>(() => backgroundOperation.Task);
            //}
            //else
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = string.Empty;
            //    expectedPercentComplete = updateData.PercentComplete.HasValue ? 100 : null;
            //    expectedCode = completeData.Code;
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await backgroundOperation.Task;
            //}

            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(expectedStatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(expectedCurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(expectedPercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (expectedPercentComplete.HasValue)
            //    Assert.AreEqual(expectedPercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //if (observed.Value is ITimedBackgroundOperationCompletedEvent completedEvent)
            //{
            //    if (completeData.Cancel)
            //        Assert.IsFalse(completedEvent.RanToCompletion);
            //    else
            //        Assert.IsTrue(completedEvent.RanToCompletion);
            //    Assert.IsNull(completedEvent.Error);
            //    Assert.AreEqual(initializeData.Activity, completedEvent.Activity);
            //    Assert.AreEqual(expectedStatusDescription, completedEvent.StatusDescription);
            //    Assert.AreEqual(expectedCurrentOperation, completedEvent.CurrentOperation);
            //    Assert.AreEqual(expectedPercentComplete.HasValue, completedEvent.PercentComplete.HasValue);
            //    if (expectedPercentComplete.HasValue)
            //        Assert.AreEqual(expectedPercentComplete.Value, completedEvent.PercentComplete.Value);
            //    Assert.AreEqual(expectedCode.HasValue, completedEvent.Code);
            //    if (expectedCode.HasValue)
            //        Assert.AreEqual(expectedCode.Value, completedEvent.Code.Value);
            //    Assert.AreEqual(backgroundOperation.OperationId, completedEvent.OperationId);
            //    Assert.IsFalse(completedEvent.ParentId.HasValue);
            //}
            //else
            //    Assert.Fail("observed.Value is not ITimedBackgroundOperationCompletedEvent");

            //#endregion

            //#region Test Observer Completion

            //Thread.Sleep(100);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsTrue(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //Assert.IsNull(observed.Value);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion
        }

        [DataTestMethod]
        [DynamicData(nameof(GetAsyncActionCompletedStateData), DynamicDataSourceType.Method)]
        [Priority(10)]
        public async Task InvokeAsyncActionCompletedStateTokenTest(InitializeStateData initializeData, UpdateData updateData, IActionCompleteData completeData)
        {
            //Assert.Inconclusive();
            //using AutoResetEvent fgEvent = new(false);
            //using AutoResetEvent bgEvent = new(false);
            //Task asyncMethodDelegate(IBackgroundProgress<int, IAsyncActivity<int>> progress) =>
            //    TestActionState(fgEvent, bgEvent, initializeData, updateData, completeData, progress);
            //IBackgroundOperationCompletedEvent<int> onCompleted(IBackgroundOperation<int> backgroundOperation) =>
            //        new BackgroundProcessCompletedEventArgs<int>(backgroundOperation, completeData.Code, null, !backgroundOperation.Task.IsCanceled, string.IsNullOrWhiteSpace(completeData.StatusDescription) ? backgroundOperation.StatusDescription : completeData.StatusDescription); ;

            //IAsyncActivityService service = Hosting.GetAsyncActivityService();
            //if (service is null)
            //    throw new AssertInconclusiveException();
            //using CancellationTokenSource tokenSource = new();
            //ObserverHelper<IAsyncActivity<int>> operationObserver = new();
            //IBackgroundOperation<int> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, onCompleted, initializeData.Activity, initializeData.StatusDescription, initializeData.State, tokenSource.Token);

            //#region Test Initial Progress Properties

            //bgEvent.WaitOne(); // Wait until bg operation being executed
            //using IDisposable subscription = backgroundOperation.Subscribe(operationObserver);
            //Assert.IsNotNull(backgroundOperation);
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(initializeData.StatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            //fgEvent.Set(); // Signal that we've tested initial status properties

            //#endregion

            //#region Test Progress Update

            //bgEvent.WaitOne(); // Wait until status has been updated
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    backgroundOperation.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out Observed<IAsyncActivity<int>> observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //IAsyncActivity<int> progressEvent = observed.Value;
            //Assert.IsNotNull(progressEvent);
            //Assert.AreEqual(initializeData.Activity, progressEvent.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    progressEvent.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, progressEvent.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, progressEvent.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, progressEvent.PercentComplete.Value);
            //Assert.AreEqual(updateData.Code.HasValue, progressEvent.Code.HasValue);
            //if (updateData.Code.HasValue)
            //    Assert.AreEqual(updateData.Code.Value, progressEvent.Code.Value);
            //Assert.AreEqual(initializeData.State, progressEvent.AsyncState);
            //Assert.AreEqual(backgroundOperation.OperationId, progressEvent.OperationId);
            //Assert.IsFalse(progressEvent.ParentId.HasValue);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion

            //#region Test Operation Completion

            //string expectedStatusDescription;
            //string expectedCurrentOperation;
            //byte? expectedPercentComplete;
            //MessageCode? expectedCode;
            //if (completeData.Cancel)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = updateData.Code;
            //    tokenSource.Cancel();
            //    fgEvent.Set(); // Signal that we've canceled the operation
            //    await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => backgroundOperation.Task);
            //}
            //else if (completeData is ErrorCompleteData errorData)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = errorData.Code.ToMessageCode(MessageCode.UnexpectedError);
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await Assert.ThrowsExceptionAsync<AsyncOperationException>(() => backgroundOperation.Task);
            //}
            //else
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = string.Empty;
            //    expectedPercentComplete = updateData.PercentComplete.HasValue ? 100 : null;
            //    expectedCode = completeData.Code;
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await backgroundOperation.Task;
            //}

            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(expectedStatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(expectedCurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(expectedPercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (expectedPercentComplete.HasValue)
            //    Assert.AreEqual(expectedPercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //if (observed.Value is IBackgroundOperationCompletedEvent<int> completedEvent)
            //{
            //    if (completeData.Cancel)
            //        Assert.IsFalse(completedEvent.RanToCompletion);
            //    else
            //        Assert.IsTrue(completedEvent.RanToCompletion);
            //    Assert.IsNull(completedEvent.Error);
            //    Assert.AreEqual(initializeData.Activity, completedEvent.Activity);
            //    Assert.AreEqual(expectedStatusDescription, completedEvent.StatusDescription);
            //    Assert.AreEqual(expectedCurrentOperation, completedEvent.CurrentOperation);
            //    Assert.AreEqual(expectedPercentComplete.HasValue, completedEvent.PercentComplete.HasValue);
            //    if (expectedPercentComplete.HasValue)
            //        Assert.AreEqual(expectedPercentComplete.Value, completedEvent.PercentComplete.Value);
            //    Assert.AreEqual(expectedCode.HasValue, completedEvent.Code);
            //    if (expectedCode.HasValue)
            //        Assert.AreEqual(expectedCode.Value, completedEvent.Code.Value);
            //    Assert.AreEqual(initializeData.State, completedEvent.AsyncState);
            //    Assert.AreEqual(backgroundOperation.OperationId, completedEvent.OperationId);
            //    Assert.IsFalse(completedEvent.ParentId.HasValue);
            //}
            //else
            //    Assert.Fail("observed.Value is not IBackgroundOperationCompletedEvent<int>");

            //#endregion

            //#region Test Observer Completion

            //Thread.Sleep(100);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsTrue(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //Assert.IsNull(observed.Value);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion
        }

        [DataTestMethod]
        [DynamicData(nameof(GetAsyncActionCompletedStateData), DynamicDataSourceType.Method)]
        [Priority(10)]
        public async Task InvokeAsyncActionCompletedStateTest(InitializeStateData initializeData, UpdateData updateData, IActionCompleteData completeData)
        {
            Assert.Inconclusive();
            //using AutoResetEvent fgEvent = new(false);
            //using AutoResetEvent bgEvent = new(false);
            //Task asyncMethodDelegate(IBackgroundProgress<int, IAsyncActivity<int>> progress) =>
            //    TestActionState(fgEvent, bgEvent, initializeData, updateData, completeData, progress);
            //IBackgroundOperationCompletedEvent<int> onCompleted(IBackgroundOperation<int> backgroundOperation) =>
            //        new BackgroundProcessCompletedEventArgs<int>(backgroundOperation, completeData.Code, null, !backgroundOperation.Task.IsCanceled, string.IsNullOrWhiteSpace(completeData.StatusDescription) ? backgroundOperation.StatusDescription : completeData.StatusDescription); ;

            //IAsyncActivityService service = Hosting.GetAsyncActivityService();
            //if (service is null)
            //    throw new AssertInconclusiveException();
            //ObserverHelper<IAsyncActivity<int>> operationObserver = new();
            //IBackgroundOperation<int> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, onCompleted, initializeData.Activity, initializeData.StatusDescription, initializeData.State);

            //#region Test Initial Progress Properties

            //bgEvent.WaitOne(); // Wait until bg operation being executed
            //using IDisposable subscription = backgroundOperation.Subscribe(operationObserver);
            //Assert.IsNotNull(backgroundOperation);
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(initializeData.StatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            //fgEvent.Set(); // Signal that we've tested initial status properties

            //#endregion

            //#region Test Progress Update

            //bgEvent.WaitOne(); // Wait until status has been updated
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    backgroundOperation.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out Observed<IAsyncActivity<int>> observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //IAsyncActivity<int> progressEvent = observed.Value;
            //Assert.IsNotNull(progressEvent);
            //Assert.AreEqual(initializeData.Activity, progressEvent.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    progressEvent.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, progressEvent.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, progressEvent.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, progressEvent.PercentComplete.Value);
            //Assert.AreEqual(updateData.Code.HasValue, progressEvent.Code.HasValue);
            //if (updateData.Code.HasValue)
            //    Assert.AreEqual(updateData.Code.Value, progressEvent.Code.Value);
            //Assert.AreEqual(initializeData.State, progressEvent.AsyncState);
            //Assert.AreEqual(backgroundOperation.OperationId, progressEvent.OperationId);
            //Assert.IsFalse(progressEvent.ParentId.HasValue);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion

            //#region Test Operation Completion

            //string expectedStatusDescription;
            //string expectedCurrentOperation;
            //byte? expectedPercentComplete;
            //MessageCode? expectedCode;
            //if (completeData.Cancel)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = updateData.Code;
            //    backgroundOperation.Cancel();
            //    fgEvent.Set(); // Signal that we've canceled the operation
            //    await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => backgroundOperation.Task);
            //}
            //else if (completeData is ErrorCompleteData errorData)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = errorData.Code.ToMessageCode(MessageCode.UnexpectedError);
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await Assert.ThrowsExceptionAsync<AsyncOperationException>(() => backgroundOperation.Task);
            //}
            //else
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = string.Empty;
            //    expectedPercentComplete = updateData.PercentComplete.HasValue ? 100 : null;
            //    expectedCode = completeData.Code;
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await backgroundOperation.Task;
            //}

            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(expectedStatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(expectedCurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(expectedPercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (expectedPercentComplete.HasValue)
            //    Assert.AreEqual(expectedPercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //if (observed.Value is IBackgroundOperationCompletedEvent<int> completedEvent)
            //{
            //    if (completeData.Cancel)
            //        Assert.IsFalse(completedEvent.RanToCompletion);
            //    else
            //        Assert.IsTrue(completedEvent.RanToCompletion);
            //    Assert.IsNull(completedEvent.Error);
            //    Assert.AreEqual(initializeData.Activity, completedEvent.Activity);
            //    Assert.AreEqual(expectedStatusDescription, completedEvent.StatusDescription);
            //    Assert.AreEqual(expectedCurrentOperation, completedEvent.CurrentOperation);
            //    Assert.AreEqual(expectedPercentComplete.HasValue, completedEvent.PercentComplete.HasValue);
            //    if (expectedPercentComplete.HasValue)
            //        Assert.AreEqual(expectedPercentComplete.Value, completedEvent.PercentComplete.Value);
            //    Assert.AreEqual(expectedCode.HasValue, completedEvent.Code);
            //    if (expectedCode.HasValue)
            //        Assert.AreEqual(expectedCode.Value, completedEvent.Code.Value);
            //    Assert.AreEqual(initializeData.State, completedEvent.AsyncState);
            //    Assert.AreEqual(backgroundOperation.OperationId, completedEvent.OperationId);
            //    Assert.IsFalse(completedEvent.ParentId.HasValue);
            //}
            //else
            //    Assert.Fail("observed.Value is not IBackgroundOperationCompletedEvent<int>");

            //#endregion

            //#region Test Observer Completion

            //Thread.Sleep(100);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsTrue(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //Assert.IsNull(observed.Value);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion
        }

        [DataTestMethod]
        [DynamicData(nameof(GetAsyncActionCompletedStateData), DynamicDataSourceType.Method)]
        [Priority(10)]
        public async Task InvokeAsyncActionStateTokenTest(InitializeStateData initializeData, UpdateData updateData, IActionCompleteData completeData)
        {
            Assert.Inconclusive();
            //using AutoResetEvent fgEvent = new(false);
            //using AutoResetEvent bgEvent = new(false);
            //Task asyncMethodDelegate(IBackgroundProgress<int, IAsyncActivity<int>> progress) =>
            //    TestActionState(fgEvent, bgEvent, initializeData, updateData, completeData, progress);

            //IAsyncActivityService service = Hosting.GetAsyncActivityService();
            //if (service is null)
            //    throw new AssertInconclusiveException();
            //using CancellationTokenSource tokenSource = new();
            //ObserverHelper<IAsyncActivity<int>> operationObserver = new();
            //IBackgroundOperation<int> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, initializeData.Activity, initializeData.StatusDescription, initializeData.State, tokenSource.Token);

            //#region Test Initial Progress Properties

            //bgEvent.WaitOne(); // Wait until bg operation being executed
            //using IDisposable subscription = backgroundOperation.Subscribe(operationObserver);
            //Assert.IsNotNull(backgroundOperation);
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(initializeData.StatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            //fgEvent.Set(); // Signal that we've tested initial status properties

            //#endregion

            //#region Test Progress Update

            //bgEvent.WaitOne(); // Wait until status has been updated
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    backgroundOperation.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out Observed<IAsyncActivity<int>> observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //IAsyncActivity<int> progressEvent = observed.Value;
            //Assert.IsNotNull(progressEvent);
            //Assert.AreEqual(initializeData.Activity, progressEvent.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    progressEvent.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, progressEvent.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, progressEvent.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, progressEvent.PercentComplete.Value);
            //Assert.AreEqual(updateData.Code.HasValue, progressEvent.Code.HasValue);
            //if (updateData.Code.HasValue)
            //    Assert.AreEqual(updateData.Code.Value, progressEvent.Code.Value);
            //Assert.AreEqual(initializeData.State, progressEvent.AsyncState);
            //Assert.AreEqual(backgroundOperation.OperationId, progressEvent.OperationId);
            //Assert.IsFalse(progressEvent.ParentId.HasValue);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion

            //#region Test Operation Completion

            //string expectedStatusDescription;
            //string expectedCurrentOperation;
            //byte? expectedPercentComplete;
            //MessageCode? expectedCode;
            //if (completeData.Cancel)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = updateData.Code;
            //    tokenSource.Cancel();
            //    fgEvent.Set(); // Signal that we've canceled the operation
            //    await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => backgroundOperation.Task);
            //}
            //else if (completeData is ErrorCompleteData errorData)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = errorData.Code.ToMessageCode(MessageCode.UnexpectedError);
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await Assert.ThrowsExceptionAsync<AsyncOperationException>(() => backgroundOperation.Task);
            //}
            //else
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = string.Empty;
            //    expectedPercentComplete = updateData.PercentComplete.HasValue ? 100 : null;
            //    expectedCode = completeData.Code;
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await backgroundOperation.Task;
            //}

            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(expectedStatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(expectedCurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(expectedPercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (expectedPercentComplete.HasValue)
            //    Assert.AreEqual(expectedPercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //if (observed.Value is IBackgroundOperationCompletedEvent<int> completedEvent)
            //{
            //    if (completeData.Cancel)
            //        Assert.IsFalse(completedEvent.RanToCompletion);
            //    else
            //        Assert.IsTrue(completedEvent.RanToCompletion);
            //    Assert.IsNull(completedEvent.Error);
            //    Assert.AreEqual(initializeData.Activity, completedEvent.Activity);
            //    Assert.AreEqual(expectedStatusDescription, completedEvent.StatusDescription);
            //    Assert.AreEqual(expectedCurrentOperation, completedEvent.CurrentOperation);
            //    Assert.AreEqual(expectedPercentComplete.HasValue, completedEvent.PercentComplete.HasValue);
            //    if (expectedPercentComplete.HasValue)
            //        Assert.AreEqual(expectedPercentComplete.Value, completedEvent.PercentComplete.Value);
            //    Assert.AreEqual(expectedCode.HasValue, completedEvent.Code);
            //    if (expectedCode.HasValue)
            //        Assert.AreEqual(expectedCode.Value, completedEvent.Code.Value);
            //    Assert.AreEqual(initializeData.State, completedEvent.AsyncState);
            //    Assert.AreEqual(backgroundOperation.OperationId, completedEvent.OperationId);
            //    Assert.IsFalse(completedEvent.ParentId.HasValue);
            //}
            //else
            //    Assert.Fail("observed.Value is not IBackgroundOperationCompletedEvent<int>");

            //#endregion

            //#region Test Observer Completion

            //Thread.Sleep(100);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsTrue(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //Assert.IsNull(observed.Value);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion
        }

        [DataTestMethod]
        [DynamicData(nameof(GetAsyncActionCompletedStateData), DynamicDataSourceType.Method)]
        [Priority(10)]
        public async Task InvokeAsyncActionStateTest(InitializeStateData initializeData, UpdateData updateData, IActionCompleteData completeData)
        {
            Assert.Inconclusive();
            //using AutoResetEvent fgEvent = new(false);
            //using AutoResetEvent bgEvent = new(false);
            //Task asyncMethodDelegate(IBackgroundProgress<int, IAsyncActivity<int>> progress) =>
            //    TestActionState(fgEvent, bgEvent, initializeData, updateData, completeData, progress);

            //IAsyncActivityService service = Hosting.GetAsyncActivityService();
            //if (service is null)
            //    throw new AssertInconclusiveException();
            //ObserverHelper<IAsyncActivity<int>> operationObserver = new();
            //IBackgroundOperation<int> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, initializeData.Activity, initializeData.StatusDescription, initializeData.State);

            //#region Test Initial Progress Properties

            //bgEvent.WaitOne(); // Wait until bg operation being executed
            //using IDisposable subscription = backgroundOperation.Subscribe(operationObserver);
            //Assert.IsNotNull(backgroundOperation);
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(initializeData.StatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            //fgEvent.Set(); // Signal that we've tested initial status properties

            //#endregion

            //#region Test Progress Update

            //bgEvent.WaitOne(); // Wait until status has been updated
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    backgroundOperation.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out Observed<IAsyncActivity<int>> observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //IAsyncActivity<int> progressEvent = observed.Value;
            //Assert.IsNotNull(progressEvent);
            //Assert.AreEqual(initializeData.Activity, progressEvent.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    progressEvent.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, progressEvent.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, progressEvent.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, progressEvent.PercentComplete.Value);
            //Assert.AreEqual(updateData.Code.HasValue, progressEvent.Code.HasValue);
            //if (updateData.Code.HasValue)
            //    Assert.AreEqual(updateData.Code.Value, progressEvent.Code.Value);
            //Assert.AreEqual(initializeData.State, progressEvent.AsyncState);
            //Assert.AreEqual(backgroundOperation.OperationId, progressEvent.OperationId);
            //Assert.IsFalse(progressEvent.ParentId.HasValue);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion

            //#region Test Operation Completion

            //string expectedStatusDescription;
            //string expectedCurrentOperation;
            //byte? expectedPercentComplete;
            //MessageCode? expectedCode;
            //if (completeData.Cancel)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = updateData.Code;
            //    backgroundOperation.Cancel();
            //    fgEvent.Set(); // Signal that we've canceled the operation
            //    await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => backgroundOperation.Task);
            //}
            //else if (completeData is ErrorCompleteData errorData)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = errorData.Code.ToMessageCode(MessageCode.UnexpectedError);
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await Assert.ThrowsExceptionAsync<AsyncOperationException>(() => backgroundOperation.Task);
            //}
            //else
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = string.Empty;
            //    expectedPercentComplete = updateData.PercentComplete.HasValue ? 100 : null;
            //    expectedCode = completeData.Code;
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await backgroundOperation.Task;
            //}

            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(expectedStatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(expectedCurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(expectedPercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (expectedPercentComplete.HasValue)
            //    Assert.AreEqual(expectedPercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //if (observed.Value is IBackgroundOperationCompletedEvent<int> completedEvent)
            //{
            //    if (completeData.Cancel)
            //        Assert.IsFalse(completedEvent.RanToCompletion);
            //    else
            //        Assert.IsTrue(completedEvent.RanToCompletion);
            //    Assert.IsNull(completedEvent.Error);
            //    Assert.AreEqual(initializeData.Activity, completedEvent.Activity);
            //    Assert.AreEqual(expectedStatusDescription, completedEvent.StatusDescription);
            //    Assert.AreEqual(expectedCurrentOperation, completedEvent.CurrentOperation);
            //    Assert.AreEqual(expectedPercentComplete.HasValue, completedEvent.PercentComplete.HasValue);
            //    if (expectedPercentComplete.HasValue)
            //        Assert.AreEqual(expectedPercentComplete.Value, completedEvent.PercentComplete.Value);
            //    Assert.AreEqual(expectedCode.HasValue, completedEvent.Code);
            //    if (expectedCode.HasValue)
            //        Assert.AreEqual(expectedCode.Value, completedEvent.Code.Value);
            //    Assert.AreEqual(initializeData.State, completedEvent.AsyncState);
            //    Assert.AreEqual(backgroundOperation.OperationId, completedEvent.OperationId);
            //    Assert.IsFalse(completedEvent.ParentId.HasValue);
            //}
            //else
            //    Assert.Fail("observed.Value is not IBackgroundOperationCompletedEvent<int>");

            //#endregion

            //#region Test Observer Completion

            //Thread.Sleep(100);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsTrue(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //Assert.IsNull(observed.Value);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion
        }

        [DataTestMethod]
        [DynamicData(nameof(GetAsyncActionCompletedData), DynamicDataSourceType.Method)]
        [Priority(10)]
        public async Task InvokeAsyncActionCompletedTokenTest(InitializeData initializeData, UpdateData updateData, IActionCompleteData completeData)
        {
            Assert.Inconclusive();
            //using AutoResetEvent fgEvent = new(false);
            //using AutoResetEvent bgEvent = new(false);
            //Task asyncMethodDelegate(IBackgroundProgress<IAsyncActivity> progress) =>
            //    TestAction(fgEvent, bgEvent, initializeData, updateData, completeData, progress);
            //IBackgroundOperationCompletedEvent onCompleted(IBackgroundOperation backgroundOperation) =>
            //        new BackgroundProcessCompletedEventArgs(backgroundOperation, completeData.Code, null, !backgroundOperation.Task.IsCanceled); ;

            //IAsyncActivityService service = Hosting.GetAsyncActivityService();
            //if (service is null)
            //    throw new AssertInconclusiveException();
            //using CancellationTokenSource tokenSource = new();
            //ObserverHelper<IAsyncActivity> operationObserver = new();
            //IBackgroundOperation backgroundOperation = service.InvokeAsync(asyncMethodDelegate, onCompleted, initializeData.Activity, initializeData.StatusDescription, tokenSource.Token);

            //#region Test Initial Progress Properties

            //bgEvent.WaitOne(); // Wait until bg operation being executed
            //using IDisposable subscription = backgroundOperation.Subscribe(operationObserver);
            //Assert.IsNotNull(backgroundOperation);
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(initializeData.StatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            //Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            //fgEvent.Set(); // Signal that we've tested initial status properties

            //#endregion

            //#region Test Progress Update

            //bgEvent.WaitOne(); // Wait until status has been updated
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    backgroundOperation.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out Observed<IAsyncActivity> observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //IAsyncActivity progressEvent = observed.Value;
            //Assert.IsNotNull(progressEvent);
            //Assert.AreEqual(initializeData.Activity, progressEvent.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    progressEvent.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, progressEvent.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, progressEvent.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, progressEvent.PercentComplete.Value);
            //Assert.AreEqual(updateData.Code.HasValue, progressEvent.Code.HasValue);
            //if (updateData.Code.HasValue)
            //    Assert.AreEqual(updateData.Code.Value, progressEvent.Code.Value);
            //Assert.AreEqual(backgroundOperation.OperationId, progressEvent.OperationId);
            //Assert.IsFalse(progressEvent.ParentId.HasValue);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion

            //#region Test Operation Completion

            //string expectedStatusDescription;
            //string expectedCurrentOperation;
            //byte? expectedPercentComplete;
            //MessageCode? expectedCode;
            //if (completeData.Cancel)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = updateData.Code;
            //    tokenSource.Cancel();
            //    fgEvent.Set(); // Signal that we've canceled the operation
            //    await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => backgroundOperation.Task);
            //}
            //else if (completeData is ErrorCompleteData errorData)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = errorData.Code.ToMessageCode(MessageCode.UnexpectedError);
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await Assert.ThrowsExceptionAsync<AsyncOperationException>(() => backgroundOperation.Task);
            //}
            //else
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = string.Empty;
            //    expectedPercentComplete = updateData.PercentComplete.HasValue ? 100 : null;
            //    expectedCode = completeData.Code;
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await backgroundOperation.Task;
            //}

            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(expectedStatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(expectedCurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(expectedPercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (expectedPercentComplete.HasValue)
            //    Assert.AreEqual(expectedPercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //if (observed.Value is IBackgroundOperationCompletedEvent completedEvent)
            //{
            //    if (completeData.Cancel)
            //        Assert.IsFalse(completedEvent.RanToCompletion);
            //    else
            //        Assert.IsTrue(completedEvent.RanToCompletion);
            //    Assert.IsNull(completedEvent.Error);
            //    Assert.AreEqual(initializeData.Activity, completedEvent.Activity);
            //    Assert.AreEqual(expectedStatusDescription, completedEvent.StatusDescription);
            //    Assert.AreEqual(expectedCurrentOperation, completedEvent.CurrentOperation);
            //    Assert.AreEqual(expectedPercentComplete.HasValue, completedEvent.PercentComplete.HasValue);
            //    if (expectedPercentComplete.HasValue)
            //        Assert.AreEqual(expectedPercentComplete.Value, completedEvent.PercentComplete.Value);
            //    Assert.AreEqual(expectedCode.HasValue, completedEvent.Code);
            //    if (expectedCode.HasValue)
            //        Assert.AreEqual(expectedCode.Value, completedEvent.Code.Value);
            //    Assert.AreEqual(backgroundOperation.OperationId, completedEvent.OperationId);
            //    Assert.IsFalse(completedEvent.ParentId.HasValue);
            //}
            //else
            //    Assert.Fail("observed.Value is not IBackgroundOperationCompletedEvent");

            //#endregion

            //#region Test Observer Completion

            //Thread.Sleep(100);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsTrue(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //Assert.IsNull(observed.Value);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion
        }

        [DataTestMethod]
        [DynamicData(nameof(GetAsyncActionCompletedData), DynamicDataSourceType.Method)]
        [Priority(10)]
        public async Task InvokeAsyncActionCompletedTest(InitializeData initializeData, UpdateData updateData, IActionCompleteData completeData)
        {
            Assert.Inconclusive();
            //using AutoResetEvent fgEvent = new(false);
            //using AutoResetEvent bgEvent = new(false);
            //Task asyncMethodDelegate(IBackgroundProgress<IAsyncActivity> progress) =>
            //    TestAction(fgEvent, bgEvent, initializeData, updateData, completeData, progress);
            //IBackgroundOperationCompletedEvent onCompleted(IBackgroundOperation backgroundOperation) =>
            //        new BackgroundProcessCompletedEventArgs(backgroundOperation, completeData.Code, null, !backgroundOperation.Task.IsCanceled); ;

            //IAsyncActivityService service = Hosting.GetAsyncActivityService();
            //if (service is null)
            //    throw new AssertInconclusiveException();
            //ObserverHelper<IAsyncActivity> operationObserver = new();
            //IBackgroundOperation backgroundOperation = service.InvokeAsync(asyncMethodDelegate, onCompleted, initializeData.Activity, initializeData.StatusDescription);

            //#region Test Initial Progress Properties

            //bgEvent.WaitOne(); // Wait until bg operation being executed
            //using IDisposable subscription = backgroundOperation.Subscribe(operationObserver);
            //Assert.IsNotNull(backgroundOperation);
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(initializeData.StatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            //Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            //fgEvent.Set(); // Signal that we've tested initial status properties

            //#endregion

            //#region Test Progress Update

            //bgEvent.WaitOne(); // Wait until status has been updated
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    backgroundOperation.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out Observed<IAsyncActivity> observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //IAsyncActivity progressEvent = observed.Value;
            //Assert.IsNotNull(progressEvent);
            //Assert.AreEqual(initializeData.Activity, progressEvent.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    progressEvent.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, progressEvent.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, progressEvent.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, progressEvent.PercentComplete.Value);
            //Assert.AreEqual(updateData.Code.HasValue, progressEvent.Code.HasValue);
            //if (updateData.Code.HasValue)
            //    Assert.AreEqual(updateData.Code.Value, progressEvent.Code.Value);
            //Assert.AreEqual(backgroundOperation.OperationId, progressEvent.OperationId);
            //Assert.IsFalse(progressEvent.ParentId.HasValue);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion

            //#region Test Operation Completion

            //string expectedStatusDescription;
            //string expectedCurrentOperation;
            //byte? expectedPercentComplete;
            //MessageCode? expectedCode;
            //if (completeData.Cancel)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = updateData.Code;
            //    backgroundOperation.Cancel();
            //    fgEvent.Set(); // Signal that we've canceled the operation
            //    await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => backgroundOperation.Task);
            //}
            //else if (completeData is ErrorCompleteData errorData)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = errorData.Code.ToMessageCode(MessageCode.UnexpectedError);
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await Assert.ThrowsExceptionAsync<AsyncOperationException>(() => backgroundOperation.Task);
            //}
            //else
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = string.Empty;
            //    expectedPercentComplete = updateData.PercentComplete.HasValue ? 100 : null;
            //    expectedCode = completeData.Code;
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await backgroundOperation.Task;
            //}

            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(expectedStatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(expectedCurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(expectedPercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (expectedPercentComplete.HasValue)
            //    Assert.AreEqual(expectedPercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //if (observed.Value is IBackgroundOperationCompletedEvent completedEvent)
            //{
            //    if (completeData.Cancel)
            //        Assert.IsFalse(completedEvent.RanToCompletion);
            //    else
            //        Assert.IsTrue(completedEvent.RanToCompletion);
            //    Assert.IsNull(completedEvent.Error);
            //    Assert.AreEqual(initializeData.Activity, completedEvent.Activity);
            //    Assert.AreEqual(expectedStatusDescription, completedEvent.StatusDescription);
            //    Assert.AreEqual(expectedCurrentOperation, completedEvent.CurrentOperation);
            //    Assert.AreEqual(expectedPercentComplete.HasValue, completedEvent.PercentComplete.HasValue);
            //    if (expectedPercentComplete.HasValue)
            //        Assert.AreEqual(expectedPercentComplete.Value, completedEvent.PercentComplete.Value);
            //    Assert.AreEqual(expectedCode.HasValue, completedEvent.Code);
            //    if (expectedCode.HasValue)
            //        Assert.AreEqual(expectedCode.Value, completedEvent.Code.Value);
            //    Assert.AreEqual(backgroundOperation.OperationId, completedEvent.OperationId);
            //    Assert.IsFalse(completedEvent.ParentId.HasValue);
            //}
            //else
            //    Assert.Fail("observed.Value is not IBackgroundOperationCompletedEvent");

            //#endregion

            //#region Test Observer Completion

            //Thread.Sleep(100);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsTrue(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //Assert.IsNull(observed.Value);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion
        }

        [DataTestMethod]
        [DynamicData(nameof(GetAsyncActionCompletedData), DynamicDataSourceType.Method)]
        [Priority(10)]
        public async Task InvokeAsyncActionTokenTest(InitializeData initializeData, UpdateData updateData, IActionCompleteData completeData)
        {
            Assert.Inconclusive();
            //using AutoResetEvent fgEvent = new(false);
            //using AutoResetEvent bgEvent = new(false);
            //Task asyncMethodDelegate(IBackgroundProgress<IAsyncActivity> progress) =>
            //    TestAction(fgEvent, bgEvent, initializeData, updateData, completeData, progress);

            //IAsyncActivityService service = Hosting.GetAsyncActivityService();
            //if (service is null)
            //    throw new AssertInconclusiveException();
            //using CancellationTokenSource tokenSource = new();
            //ObserverHelper<IAsyncActivity> operationObserver = new();
            //IBackgroundOperation backgroundOperation = service.InvokeAsync(asyncMethodDelegate, initializeData.Activity, initializeData.StatusDescription, tokenSource.Token);

            //#region Test Initial Progress Properties

            //bgEvent.WaitOne(); // Wait until bg operation being executed
            //using IDisposable subscription = backgroundOperation.Subscribe(operationObserver);
            //Assert.IsNotNull(backgroundOperation);
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(initializeData.StatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            //Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            //fgEvent.Set(); // Signal that we've tested initial status properties

            //#endregion

            //#region Test Progress Update

            //bgEvent.WaitOne(); // Wait until status has been updated
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    backgroundOperation.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out Observed<IAsyncActivity> observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //IAsyncActivity progressEvent = observed.Value;
            //Assert.IsNotNull(progressEvent);
            //Assert.AreEqual(initializeData.Activity, progressEvent.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    progressEvent.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, progressEvent.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, progressEvent.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, progressEvent.PercentComplete.Value);
            //Assert.AreEqual(updateData.Code.HasValue, progressEvent.Code.HasValue);
            //if (updateData.Code.HasValue)
            //    Assert.AreEqual(updateData.Code.Value, progressEvent.Code.Value);
            //Assert.AreEqual(backgroundOperation.OperationId, progressEvent.OperationId);
            //Assert.IsFalse(progressEvent.ParentId.HasValue);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion

            //#region Test Operation Completion

            //string expectedStatusDescription;
            //string expectedCurrentOperation;
            //byte? expectedPercentComplete;
            //MessageCode? expectedCode;
            //if (completeData.Cancel)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = updateData.Code;
            //    tokenSource.Cancel();
            //    fgEvent.Set(); // Signal that we've canceled the operation
            //    await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => backgroundOperation.Task);
            //}
            //else if (completeData is ErrorCompleteData errorData)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = errorData.Code.ToMessageCode(MessageCode.UnexpectedError);
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await Assert.ThrowsExceptionAsync<AsyncOperationException>(() => backgroundOperation.Task);
            //}
            //else
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = string.Empty;
            //    expectedPercentComplete = updateData.PercentComplete.HasValue ? 100 : null;
            //    expectedCode = completeData.Code;
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await backgroundOperation.Task;
            //}

            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(expectedStatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(expectedCurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(expectedPercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (expectedPercentComplete.HasValue)
            //    Assert.AreEqual(expectedPercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //if (observed.Value is IBackgroundOperationCompletedEvent completedEvent)
            //{
            //    if (completeData.Cancel)
            //        Assert.IsFalse(completedEvent.RanToCompletion);
            //    else
            //        Assert.IsTrue(completedEvent.RanToCompletion);
            //    Assert.IsNull(completedEvent.Error);
            //    Assert.AreEqual(initializeData.Activity, completedEvent.Activity);
            //    Assert.AreEqual(expectedStatusDescription, completedEvent.StatusDescription);
            //    Assert.AreEqual(expectedCurrentOperation, completedEvent.CurrentOperation);
            //    Assert.AreEqual(expectedPercentComplete.HasValue, completedEvent.PercentComplete.HasValue);
            //    if (expectedPercentComplete.HasValue)
            //        Assert.AreEqual(expectedPercentComplete.Value, completedEvent.PercentComplete.Value);
            //    Assert.AreEqual(expectedCode.HasValue, completedEvent.Code);
            //    if (expectedCode.HasValue)
            //        Assert.AreEqual(expectedCode.Value, completedEvent.Code.Value);
            //    Assert.AreEqual(backgroundOperation.OperationId, completedEvent.OperationId);
            //    Assert.IsFalse(completedEvent.ParentId.HasValue);
            //}
            //else
            //    Assert.Fail("observed.Value is not IBackgroundOperationCompletedEvent");

            //#endregion

            //#region Test Observer Completion

            //Thread.Sleep(100);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsTrue(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //Assert.IsNull(observed.Value);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion
        }

        [DataTestMethod]
        [DynamicData(nameof(GetAsyncActionCompletedData), DynamicDataSourceType.Method)]
        [Priority(10)]
        public async Task InvokeAsyncActionTest(InitializeData initializeData, UpdateData updateData, IActionCompleteData completeData)
        {
            Assert.Inconclusive();
            //using AutoResetEvent fgEvent = new(false);
            //using AutoResetEvent bgEvent = new(false);
            //Task asyncMethodDelegate(IBackgroundProgress<IAsyncActivity> progress) =>
            //    TestAction(fgEvent, bgEvent, initializeData, updateData, completeData, progress);

            //IAsyncActivityService service = Hosting.GetAsyncActivityService();
            //if (service is null)
            //    throw new AssertInconclusiveException();
            //ObserverHelper<IAsyncActivity> operationObserver = new();
            //IBackgroundOperation backgroundOperation = service.InvokeAsync(asyncMethodDelegate, initializeData.Activity, initializeData.StatusDescription);

            //#region Test Initial Progress Properties

            //bgEvent.WaitOne(); // Wait until bg operation being executed
            //using IDisposable subscription = backgroundOperation.Subscribe(operationObserver);
            //Assert.IsNotNull(backgroundOperation);
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(initializeData.StatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            //Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            //fgEvent.Set(); // Signal that we've tested initial status properties

            //#endregion

            //#region Test Progress Update

            //bgEvent.WaitOne(); // Wait until status has been updated
            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    backgroundOperation.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out Observed<IAsyncActivity> observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //IAsyncActivity progressEvent = observed.Value;
            //Assert.IsNotNull(progressEvent);
            //Assert.AreEqual(initializeData.Activity, progressEvent.Activity);
            //Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,
            //    progressEvent.StatusDescription);
            //Assert.AreEqual(updateData.CurrentOperation, progressEvent.CurrentOperation);
            //Assert.AreEqual(updateData.PercentComplete.HasValue, progressEvent.PercentComplete.HasValue);
            //if (updateData.PercentComplete.HasValue)
            //    Assert.AreEqual(updateData.PercentComplete.Value, progressEvent.PercentComplete.Value);
            //Assert.AreEqual(updateData.Code.HasValue, progressEvent.Code.HasValue);
            //if (updateData.Code.HasValue)
            //    Assert.AreEqual(updateData.Code.Value, progressEvent.Code.Value);
            //Assert.AreEqual(backgroundOperation.OperationId, progressEvent.OperationId);
            //Assert.IsFalse(progressEvent.ParentId.HasValue);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion

            //#region Test Operation Completion

            //string expectedStatusDescription;
            //string expectedCurrentOperation;
            //byte? expectedPercentComplete;
            //MessageCode? expectedCode;
            //if (completeData.Cancel)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = updateData.Code;
            //    backgroundOperation.Cancel();
            //    fgEvent.Set(); // Signal that we've canceled the operation
            //    await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => backgroundOperation.Task);
            //}
            //else if (completeData is ErrorCompleteData errorData)
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = updateData.CurrentOperation;
            //    expectedPercentComplete = updateData.PercentComplete;
            //    expectedCode = errorData.Code.ToMessageCode(MessageCode.UnexpectedError);
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await Assert.ThrowsExceptionAsync<AsyncOperationException>(() => backgroundOperation.Task);
            //}
            //else
            //{
            //    expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?
            //        (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :
            //        completeData.StatusDescription;
            //    expectedCurrentOperation = string.Empty;
            //    expectedPercentComplete = updateData.PercentComplete.HasValue ? 100 : null;
            //    expectedCode = completeData.Code;
            //    fgEvent.Set(); // Signal that we're ready to complete
            //    await backgroundOperation.Task;
            //}

            //Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);
            //Assert.AreEqual(expectedStatusDescription, backgroundOperation.StatusDescription);
            //Assert.AreEqual(expectedCurrentOperation, backgroundOperation.CurrentOperation);
            //Assert.AreEqual(expectedPercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);
            //if (expectedPercentComplete.HasValue)
            //    Assert.AreEqual(expectedPercentComplete.Value, backgroundOperation.PercentComplete.Value);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsFalse(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //if (observed.Value is IBackgroundOperationCompletedEvent completedEvent)
            //{
            //    if (completeData.Cancel)
            //        Assert.IsFalse(completedEvent.RanToCompletion);
            //    else
            //        Assert.IsTrue(completedEvent.RanToCompletion);
            //    Assert.IsNull(completedEvent.Error);
            //    Assert.AreEqual(initializeData.Activity, completedEvent.Activity);
            //    Assert.AreEqual(expectedStatusDescription, completedEvent.StatusDescription);
            //    Assert.AreEqual(expectedCurrentOperation, completedEvent.CurrentOperation);
            //    Assert.AreEqual(expectedPercentComplete.HasValue, completedEvent.PercentComplete.HasValue);
            //    if (expectedPercentComplete.HasValue)
            //        Assert.AreEqual(expectedPercentComplete.Value, completedEvent.PercentComplete.Value);
            //    Assert.AreEqual(expectedCode.HasValue, completedEvent.Code);
            //    if (expectedCode.HasValue)
            //        Assert.AreEqual(expectedCode.Value, completedEvent.Code.Value);
            //    Assert.AreEqual(backgroundOperation.OperationId, completedEvent.OperationId);
            //    Assert.IsFalse(completedEvent.ParentId.HasValue);
            //}
            //else
            //    Assert.Fail("observed.Value is not IBackgroundOperationCompletedEvent");

            //#endregion

            //#region Test Observer Completion

            //Thread.Sleep(100);
            //Assert.IsTrue(operationObserver.TryDequeue(out observed));
            //Assert.IsTrue(observed.IsComplete);
            //Assert.IsNull(observed.Error);
            //Assert.IsNull(observed.Value);
            //Assert.IsFalse(operationObserver.TryDequeue(out observed));

            //#endregion
        }

        public record InitializeStateData(string Activity, string StatusDescription, int State);

        public record InitializeData(string Activity, string StatusDescription);

        public record UpdateData(string StatusDescription, string CurrentOperation, MessageCode? Code, byte? PercentComplete);

        public interface ICompleteData
        {
            string StatusDescription { get; }
            MessageCode? Code { get; }
            bool Cancel { get; }
        }

        public interface IResultData : ICompleteData
        {
            double Result { get; }
            Func<IBackgroundProgressInfo, double> OnCompleting { get; }
        }

        public interface IActionCompleteData : ICompleteData
        {
            Action<IBackgroundProgressInfo> OnCompleting { get; }
        }

        public record CompleteData(string StatusDescription, MessageCode? Code, bool Cancel) : IActionCompleteData
        {
            Action<IBackgroundProgressInfo> IActionCompleteData.OnCompleting => null;
        }

        public record ErrorCompleteData : IActionCompleteData
        {
            public string StatusDescription { get; init; }
            public ErrorCode Code { get; init; }
            Action<IBackgroundProgressInfo> IActionCompleteData.OnCompleting => OnCompleting;
            MessageCode? ICompleteData.Code => Code.ToMessageCode(MessageCode.UnexpectedError);
            bool ICompleteData.Cancel => false;
            public void OnCompleting(IBackgroundProgressInfo progressInfo) => throw new AsyncOperationException(progressInfo, Code, string.IsNullOrWhiteSpace(StatusDescription) ? progressInfo.StatusDescription : StatusDescription);
        }

        public record ResultData(string StatusDescription, MessageCode? Code, bool Cancel, double Result) : IResultData
        {
            Func<IBackgroundProgressInfo, double> IResultData.OnCompleting => p => Result;
        }

        public record ErrorResultData : IResultData
        {
            public string StatusDescription { get; init; }
            public ErrorCode Code { get; init; }
            MessageCode? ICompleteData.Code => Code.ToMessageCode(MessageCode.UnexpectedError);
            Func<IBackgroundProgressInfo, double> IResultData.OnCompleting => OnCompleting;
            double IResultData.Result => throw new InvalidOperationException();
            bool ICompleteData.Cancel => false;
            double OnCompleting(IBackgroundProgressInfo progressInfo) => throw new AsyncOperationException(progressInfo, Code, string.IsNullOrWhiteSpace(StatusDescription) ? progressInfo.StatusDescription : StatusDescription);
        }

        record Observed<T>(T Value, Exception Error, bool IsComplete);

        class ObserverHelper<T> : IObserver<T>
        {
            private readonly Queue<Observed<T>> _backingQueue = new();

            internal int Count => _backingQueue.Count;

            internal bool TryDequeue(out Observed<T> result) => _backingQueue.TryDequeue(out result);

            void IObserver<T>.OnCompleted() => _backingQueue.Enqueue(new(default, null, true));

            void IObserver<T>.OnError(Exception error) => _backingQueue.Enqueue(new(default, error, false));

            void IObserver<T>.OnNext(T value) => _backingQueue.Enqueue(new(value, null, false));
        }
    }
}
