using FsInfoCat.AsyncOps;
using FsInfoCat.Services;
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

        private const string STATUS_FIRST = "First Status";
        private const string STATUS_SECOND = "Status #2";
        private const string OPERATION_FIRST = "First Operation";
        private const string OPERATION_SECOND = "Second Operation";
        private const string OPERATION_THIRD = "Operation #3";

        [TestMethod("Hosting.GetRequiredService<IBackgroundProgressService>()")]
        [Priority(1)]
        public void GetRequiredServiceTest()
        {
            IBackgroundProgressService service = Hosting.GetRequiredService<IBackgroundProgressService>();
            Assert.IsNotNull(service);
            Assert.IsFalse(service.IsActive);
        }

        [TestMethod("InvokeAsync(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task>, string, string)")]
        [Priority(1)]
        public void InvokeAsyncActionStringStringTestX()
        {
            IBackgroundProgressService service = Hosting.GetRequiredService<IBackgroundProgressService>();
            Assert.IsNotNull(service);
            Assert.IsFalse(service.IsActive);

            ObserverHelper<IBackgroundProgressEvent> serviceStateObserver = new();
            ObserverHelper<bool> serviceActiveObserver = new();
            using IDisposable serviceStateSubscription = (service ?? throw new AssertInconclusiveException()).Subscribe(serviceStateObserver);
            using IDisposable serviceActiveSubscription = service.Subscribe(serviceActiveObserver);
            using AutoResetEvent syncEvent = new(false);
            Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate = (IBackgroundProgress<IBackgroundProgressEvent> progress) => Task.Run(() =>
            {
                syncEvent.Set();
                syncEvent.WaitOne();
                progress.ReportCurrentOperation(OPERATION_FIRST);
                syncEvent.Set();
                syncEvent.WaitOne();
                progress.ReportStatusDescription(STATUS_FIRST);
                syncEvent.Set();
                syncEvent.WaitOne();
                progress.ReportStatusDescription(STATUS_SECOND, OPERATION_SECOND);
                syncEvent.Set();
                syncEvent.WaitOne();
                progress.ReportCurrentOperation(OPERATION_THIRD);
                syncEvent.Set();
                syncEvent.WaitOne();
            });
            string activity = "Test Activity";
            string statusDescription = "Initial Description";
            ObserverHelper<IBackgroundProgressEvent> operationObserver = new();
            IBackgroundOperation backgroundOperation = service.InvokeAsync(asyncMethodDelegate, activity, statusDescription);
            Assert.IsNotNull(backgroundOperation);

            syncEvent.WaitOne();
            Assert.IsTrue(service.IsActive);
            backgroundOperation.Subscribe(operationObserver);
            Assert.IsNotNull(backgroundOperation);
            Assert.AreEqual(backgroundOperation.Activity, activity);
            Assert.AreEqual(backgroundOperation.StatusDescription, statusDescription);
            Assert.AreEqual(backgroundOperation.CurrentOperation, string.Empty);
            Assert.IsTrue(serviceStateObserver.TryDequeue(out Observed<IBackgroundProgressEvent> observedProgress));
            Assert.IsFalse(observedProgress.IsComplete);
            Assert.IsNull(observedProgress.Error);
            if (observedProgress.Value is IBackgroundProgressStartedEvent startedEvent)
            {
                Assert.AreEqual(startedEvent.Activity, activity);
                Assert.AreEqual(startedEvent.StatusDescription, statusDescription);
                Assert.AreEqual(startedEvent.CurrentOperation, string.Empty);
                Assert.AreEqual(startedEvent.OperationId, backgroundOperation.OperationId);
                Assert.IsFalse(startedEvent.Code.HasValue);
                Assert.IsFalse(startedEvent.ParentId.HasValue);
                Assert.IsFalse(startedEvent.PercentComplete.HasValue);
            }
            else
                Assert.Fail("Observed is not IBackgroundProgressStartedEvent");
            Assert.IsFalse(serviceStateObserver.TryDequeue(out observedProgress));
            Assert.IsTrue(serviceActiveObserver.TryDequeue(out Observed<bool> observedActive));
            Assert.IsFalse(observedActive.IsComplete);
            Assert.IsNull(observedActive.Error);
            Assert.IsTrue(observedActive.Value);
            Assert.IsFalse(serviceActiveObserver.TryDequeue(out observedActive));

            syncEvent.Set();
            syncEvent.WaitOne();
            Assert.AreEqual(backgroundOperation.Activity, activity);
            Assert.AreEqual(backgroundOperation.StatusDescription, statusDescription);
            Assert.AreEqual(backgroundOperation.CurrentOperation, OPERATION_FIRST);
            Assert.IsTrue(serviceStateObserver.TryDequeue(out observedProgress));
            Assert.IsFalse(observedProgress.IsComplete);
            Assert.IsNull(observedProgress.Error);
            Assert.IsNotNull(observedProgress.Value);
            Assert.AreEqual(observedProgress.Value.Activity, activity);
            Assert.AreEqual(observedProgress.Value.StatusDescription, statusDescription);
            Assert.AreEqual(observedProgress.Value.CurrentOperation, OPERATION_FIRST);
            Assert.AreEqual(observedProgress.Value.OperationId, backgroundOperation.OperationId);
            Assert.IsFalse(observedProgress.Value.Code.HasValue);
            Assert.IsFalse(observedProgress.Value.ParentId.HasValue);
            Assert.IsFalse(observedProgress.Value.PercentComplete.HasValue);
            Assert.IsFalse(serviceStateObserver.TryDequeue(out observedProgress));
            Assert.IsFalse(serviceActiveObserver.TryDequeue(out observedActive));

            syncEvent.Set();
            syncEvent.WaitOne();
            Assert.AreEqual(backgroundOperation.Activity, activity);
            Assert.AreEqual(backgroundOperation.StatusDescription, STATUS_FIRST);
            Assert.AreEqual(backgroundOperation.CurrentOperation, string.Empty);
            Assert.IsTrue(serviceStateObserver.TryDequeue(out observedProgress));
            Assert.IsFalse(observedProgress.IsComplete);
            Assert.IsNull(observedProgress.Error);
            Assert.IsNotNull(observedProgress.Value);
            Assert.AreEqual(observedProgress.Value.Activity, activity);
            Assert.AreEqual(observedProgress.Value.StatusDescription, STATUS_FIRST);
            Assert.AreEqual(observedProgress.Value.CurrentOperation, string.Empty);
            Assert.AreEqual(observedProgress.Value.OperationId, backgroundOperation.OperationId);
            Assert.IsFalse(observedProgress.Value.Code.HasValue);
            Assert.IsFalse(observedProgress.Value.ParentId.HasValue);
            Assert.IsFalse(observedProgress.Value.PercentComplete.HasValue);
            Assert.IsFalse(serviceStateObserver.TryDequeue(out observedProgress));
            Assert.IsFalse(serviceActiveObserver.TryDequeue(out observedActive));

            syncEvent.Set();
            syncEvent.WaitOne();
            Assert.AreEqual(backgroundOperation.Activity, activity);
            Assert.AreEqual(backgroundOperation.StatusDescription, STATUS_SECOND);
            Assert.AreEqual(backgroundOperation.CurrentOperation, OPERATION_SECOND);
            Assert.IsTrue(serviceStateObserver.TryDequeue(out observedProgress));
            Assert.IsFalse(observedProgress.IsComplete);
            Assert.IsNull(observedProgress.Error);
            Assert.IsNotNull(observedProgress.Value);
            Assert.AreEqual(observedProgress.Value.Activity, activity);
            Assert.AreEqual(observedProgress.Value.StatusDescription, STATUS_SECOND);
            Assert.AreEqual(observedProgress.Value.CurrentOperation, OPERATION_SECOND);
            Assert.AreEqual(observedProgress.Value.OperationId, backgroundOperation.OperationId);
            Assert.IsFalse(observedProgress.Value.Code.HasValue);
            Assert.IsFalse(observedProgress.Value.ParentId.HasValue);
            Assert.IsFalse(observedProgress.Value.PercentComplete.HasValue);
            Assert.IsFalse(serviceStateObserver.TryDequeue(out observedProgress));
            Assert.IsFalse(serviceActiveObserver.TryDequeue(out observedActive));

            syncEvent.Set();
            syncEvent.WaitOne();
            Assert.AreEqual(backgroundOperation.Activity, activity);
            Assert.AreEqual(backgroundOperation.StatusDescription, STATUS_SECOND);
            Assert.AreEqual(backgroundOperation.CurrentOperation, OPERATION_THIRD);
            Assert.IsTrue(serviceStateObserver.TryDequeue(out observedProgress));
            Assert.IsFalse(observedProgress.IsComplete);
            Assert.IsNull(observedProgress.Error);
            Assert.IsNotNull(observedProgress.Value);
            Assert.AreEqual(observedProgress.Value.Activity, activity);
            Assert.AreEqual(observedProgress.Value.StatusDescription, STATUS_SECOND);
            Assert.AreEqual(observedProgress.Value.CurrentOperation, OPERATION_THIRD);
            Assert.AreEqual(observedProgress.Value.OperationId, backgroundOperation.OperationId);
            Assert.IsFalse(observedProgress.Value.Code.HasValue);
            Assert.IsFalse(observedProgress.Value.ParentId.HasValue);
            Assert.IsFalse(observedProgress.Value.PercentComplete.HasValue);
            Assert.IsFalse(serviceStateObserver.TryDequeue(out observedProgress));
            Assert.IsFalse(serviceActiveObserver.TryDequeue(out observedActive));

            syncEvent.Set();
            backgroundOperation.Task.Wait();
            Assert.AreEqual(backgroundOperation.Activity, activity);
            Assert.AreEqual(backgroundOperation.StatusDescription, statusDescription);
            Assert.AreEqual(backgroundOperation.CurrentOperation, string.Empty);
            Assert.IsTrue(serviceStateObserver.TryDequeue(out observedProgress));
            Assert.IsFalse(observedProgress.IsComplete);
            Assert.IsNull(observedProgress.Error);
            if (observedProgress.Value is IBackgroundOperationCompletedEvent completedEvent)
            {
                Assert.AreEqual(completedEvent.Activity, activity);
                Assert.AreEqual(completedEvent.StatusDescription, statusDescription);
                Assert.AreEqual(completedEvent.CurrentOperation, string.Empty);
                Assert.AreEqual(completedEvent.OperationId, backgroundOperation.OperationId);
                Assert.IsFalse(completedEvent.Code.HasValue);
                Assert.IsFalse(completedEvent.ParentId.HasValue);
                Assert.IsFalse(completedEvent.PercentComplete.HasValue);
                Assert.IsTrue(completedEvent.RanToCompletion);
                Assert.IsNull(completedEvent.Error);
            }
            else
                Assert.Fail("Observed is not IBackgroundOperationCompletedEvent");
            Assert.IsFalse(serviceStateObserver.TryDequeue(out observedProgress));
            Assert.IsTrue(serviceActiveObserver.TryDequeue(out observedActive));
            Assert.IsFalse(observedActive.IsComplete);
            Assert.IsNull(observedActive.Error);
            Assert.IsFalse(observedActive.Value);
            Assert.IsFalse(serviceActiveObserver.TryDequeue(out observedActive));
            Assert.IsFalse(service.IsActive);
        }

        private static Task TestActionAsync(IBackgroundProgress<IBackgroundProgressEvent> progress, string expectedActivity, string expectedStatusDescription, AutoResetEvent syncEvent,
            params (string StatusDescription, string CurrentOperation, Exception Error)[] events) => Task.Run(() =>
            {
                Assert.AreEqual(expectedActivity, progress.Activity);
                Assert.AreEqual(expectedStatusDescription, progress.StatusDescription);
                Assert.AreEqual(string.Empty, progress.CurrentOperation);
                foreach ((string StatusDescription, string CurrentOperation, Exception Error) evt in events)
                {
                    syncEvent.Set();
                    syncEvent.WaitOne();
                    if (evt.Error is null)
                    {
                        if (evt.StatusDescription is null)
                        {
                            progress.ReportCurrentOperation(evt.CurrentOperation);
                            Assert.AreEqual(evt.CurrentOperation, progress.CurrentOperation);
                        }
                        else
                        {
                            expectedStatusDescription = evt.StatusDescription;
                            if (evt.CurrentOperation is null)
                            {
                                progress.ReportStatusDescription(expectedStatusDescription);
                                Assert.AreEqual(string.Empty, progress.CurrentOperation);
                            }
                            else
                            {
                                progress.ReportStatusDescription(expectedStatusDescription, evt.CurrentOperation);
                                Assert.AreEqual(evt.CurrentOperation, progress.CurrentOperation);
                            }
                        }
                    }
                    else if (evt.StatusDescription is null)
                    {
                        progress.ReportException(evt.Error);
                        Assert.AreEqual(string.Empty, progress.CurrentOperation);
                    }
                    else
                    {
                        expectedStatusDescription = evt.StatusDescription;
                        if (evt.CurrentOperation is null)
                        {
                            progress.ReportException(evt.Error, evt.StatusDescription);
                            Assert.AreEqual(string.Empty, progress.CurrentOperation);
                        }
                        else
                        {
                            progress.ReportException(evt.Error, evt.StatusDescription, evt.CurrentOperation);
                            Assert.AreEqual(evt.CurrentOperation, progress.CurrentOperation);
                        }
                    }
                    Assert.AreEqual(expectedStatusDescription, progress.StatusDescription);
                }
                syncEvent.Set();
                syncEvent.WaitOne();
            });

        private static Task TestActionStateAsync<TState>(IBackgroundProgress<TState, IBackgroundProgressEvent<TState>> progress, string expectedActivity, string expectedStatusDescription, TState expectedState,
            AutoResetEvent syncEvent, params (string StatusDescription, string CurrentOperation, Exception Error)[] events) => Task.Run(() =>
            {
                Assert.AreEqual(expectedActivity, progress.Activity);
                Assert.AreEqual(expectedStatusDescription, progress.StatusDescription);
                Assert.AreEqual(string.Empty, progress.CurrentOperation);
                Assert.AreEqual(expectedState, progress.AsyncState);
                foreach ((string StatusDescription, string CurrentOperation, Exception Error) evt in events)
                {
                    syncEvent.Set();
                    syncEvent.WaitOne();
                    if (evt.Error is null)
                    {
                        if (evt.StatusDescription is null)
                        {
                            progress.ReportCurrentOperation(evt.CurrentOperation);
                            Assert.AreEqual(evt.CurrentOperation, progress.CurrentOperation);
                        }
                        else
                        {
                            expectedStatusDescription = evt.StatusDescription;
                            if (evt.CurrentOperation is null)
                            {
                                progress.ReportStatusDescription(expectedStatusDescription);
                                Assert.AreEqual(string.Empty, progress.CurrentOperation);
                            }
                            else
                            {
                                progress.ReportStatusDescription(expectedStatusDescription, evt.CurrentOperation);
                                Assert.AreEqual(evt.CurrentOperation, progress.CurrentOperation);
                            }
                        }
                    }
                    else if (evt.StatusDescription is null)
                    {
                        progress.ReportException(evt.Error);
                        Assert.AreEqual(string.Empty, progress.CurrentOperation);
                    }
                    else
                    {
                        expectedStatusDescription = evt.StatusDescription;
                        if (evt.CurrentOperation is null)
                        {
                            progress.ReportException(evt.Error, evt.StatusDescription);
                            Assert.AreEqual(string.Empty, progress.CurrentOperation);
                        }
                        else
                        {
                            progress.ReportException(evt.Error, evt.StatusDescription, evt.CurrentOperation);
                            Assert.AreEqual(evt.CurrentOperation, progress.CurrentOperation);
                        }
                    }
                    Assert.AreEqual(expectedStatusDescription, progress.StatusDescription);
                }
                syncEvent.Set();
                syncEvent.WaitOne();
            });

        private static Task TestTimedActionAsync(ITimedBackgroundProgress<ITimedBackgroundProgressEvent> progress, string expectedActivity, string expectedStatusDescription, AutoResetEvent syncEvent,
            params (string StatusDescription, string CurrentOperation, Exception Error)[] events) => Task.Run(() =>
            {
                Assert.AreEqual(expectedActivity, progress.Activity);
                Assert.AreEqual(expectedStatusDescription, progress.StatusDescription);
                Assert.AreEqual(string.Empty, progress.CurrentOperation);
                foreach ((string StatusDescription, string CurrentOperation, Exception Error) evt in events)
                {
                    syncEvent.Set();
                    syncEvent.WaitOne();
                    if (evt.Error is null)
                    {
                        if (evt.StatusDescription is null)
                        {
                            progress.ReportCurrentOperation(evt.CurrentOperation);
                            Assert.AreEqual(evt.CurrentOperation, progress.CurrentOperation);
                        }
                        else
                        {
                            expectedStatusDescription = evt.StatusDescription;
                            if (evt.CurrentOperation is null)
                            {
                                progress.ReportStatusDescription(expectedStatusDescription);
                                Assert.AreEqual(string.Empty, progress.CurrentOperation);
                            }
                            else
                            {
                                progress.ReportStatusDescription(expectedStatusDescription, evt.CurrentOperation);
                                Assert.AreEqual(evt.CurrentOperation, progress.CurrentOperation);
                            }
                        }
                    }
                    else if (evt.StatusDescription is null)
                    {
                        progress.ReportException(evt.Error);
                        Assert.AreEqual(string.Empty, progress.CurrentOperation);
                    }
                    else
                    {
                        expectedStatusDescription = evt.StatusDescription;
                        if (evt.CurrentOperation is null)
                        {
                            progress.ReportException(evt.Error, evt.StatusDescription);
                            Assert.AreEqual(string.Empty, progress.CurrentOperation);
                        }
                        else
                        {
                            progress.ReportException(evt.Error, evt.StatusDescription, evt.CurrentOperation);
                            Assert.AreEqual(evt.CurrentOperation, progress.CurrentOperation);
                        }
                    }
                    Assert.AreEqual(expectedStatusDescription, progress.StatusDescription);
                }
                syncEvent.Set();
                syncEvent.WaitOne();
            });

        private static Task TestTimedActionStateAsync<TState>(ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>> progress, string expectedActivity, string expectedStatusDescription, TState expectedState,
            AutoResetEvent syncEvent, params (string StatusDescription, string CurrentOperation, Exception Error)[] events) => Task.Run(() =>
            {
                Assert.AreEqual(expectedActivity, progress.Activity);
                Assert.AreEqual(expectedStatusDescription, progress.StatusDescription);
                Assert.AreEqual(string.Empty, progress.CurrentOperation);
                Assert.AreEqual(expectedState, progress.AsyncState);
                foreach ((string StatusDescription, string CurrentOperation, Exception Error) evt in events)
                {
                    syncEvent.Set();
                    syncEvent.WaitOne();
                    if (evt.Error is null)
                    {
                        if (evt.StatusDescription is null)
                        {
                            progress.ReportCurrentOperation(evt.CurrentOperation);
                            Assert.AreEqual(evt.CurrentOperation, progress.CurrentOperation);
                        }
                        else
                        {
                            expectedStatusDescription = evt.StatusDescription;
                            if (evt.CurrentOperation is null)
                            {
                                progress.ReportStatusDescription(expectedStatusDescription);
                                Assert.AreEqual(string.Empty, progress.CurrentOperation);
                            }
                            else
                            {
                                progress.ReportStatusDescription(expectedStatusDescription, evt.CurrentOperation);
                                Assert.AreEqual(evt.CurrentOperation, progress.CurrentOperation);
                            }
                        }
                    }
                    else if (evt.StatusDescription is null)
                    {
                        progress.ReportException(evt.Error);
                        Assert.AreEqual(string.Empty, progress.CurrentOperation);
                    }
                    else
                    {
                        expectedStatusDescription = evt.StatusDescription;
                        if (evt.CurrentOperation is null)
                        {
                            progress.ReportException(evt.Error, evt.StatusDescription);
                            Assert.AreEqual(string.Empty, progress.CurrentOperation);
                        }
                        else
                        {
                            progress.ReportException(evt.Error, evt.StatusDescription, evt.CurrentOperation);
                            Assert.AreEqual(evt.CurrentOperation, progress.CurrentOperation);
                        }
                    }
                    Assert.AreEqual(expectedStatusDescription, progress.StatusDescription);
                }
                syncEvent.Set();
                syncEvent.WaitOne();
            });

        private static Task<TResult> TestFuncAsync<TResult>(IBackgroundProgress<IBackgroundProgressEvent> progress, string expectedActivity, string expectedStatusDescription, TResult resultValue, AutoResetEvent syncEvent,
            params (string StatusDescription, string CurrentOperation, Exception Error)[] events) => Task.Run(() =>
            {
                Assert.AreEqual(expectedActivity, progress.Activity);
                Assert.AreEqual(expectedStatusDescription, progress.StatusDescription);
                Assert.AreEqual(string.Empty, progress.CurrentOperation);
                foreach ((string StatusDescription, string CurrentOperation, Exception Error) evt in events)
                {
                    syncEvent.Set();
                    syncEvent.WaitOne();
                    if (evt.Error is null)
                    {
                        if (evt.StatusDescription is null)
                        {
                            progress.ReportCurrentOperation(evt.CurrentOperation);
                            Assert.AreEqual(evt.CurrentOperation, progress.CurrentOperation);
                        }
                        else
                        {
                            expectedStatusDescription = evt.StatusDescription;
                            if (evt.CurrentOperation is null)
                            {
                                progress.ReportStatusDescription(expectedStatusDescription);
                                Assert.AreEqual(string.Empty, progress.CurrentOperation);
                            }
                            else
                            {
                                progress.ReportStatusDescription(expectedStatusDescription, evt.CurrentOperation);
                                Assert.AreEqual(evt.CurrentOperation, progress.CurrentOperation);
                            }
                        }
                    }
                    else if (evt.StatusDescription is null)
                    {
                        progress.ReportException(evt.Error);
                        Assert.AreEqual(string.Empty, progress.CurrentOperation);
                    }
                    else
                    {
                        expectedStatusDescription = evt.StatusDescription;
                        if (evt.CurrentOperation is null)
                        {
                            progress.ReportException(evt.Error, evt.StatusDescription);
                            Assert.AreEqual(string.Empty, progress.CurrentOperation);
                        }
                        else
                        {
                            progress.ReportException(evt.Error, evt.StatusDescription, evt.CurrentOperation);
                            Assert.AreEqual(evt.CurrentOperation, progress.CurrentOperation);
                        }
                    }
                    Assert.AreEqual(expectedStatusDescription, progress.StatusDescription);
                }
                syncEvent.Set();
                syncEvent.WaitOne();
                return resultValue;
            });

        private static Task<TResult> TestFuncStateAsync<TState, TResult>(IBackgroundProgress<TState, IBackgroundProgressEvent<TState>> progress, string expectedActivity, string expectedStatusDescription, TState expectedState,
            TResult resultValue, AutoResetEvent syncEvent, params (string StatusDescription, string CurrentOperation, Exception Error)[] events) => Task.Run(() =>
            {
                Assert.AreEqual(expectedActivity, progress.Activity);
                Assert.AreEqual(expectedStatusDescription, progress.StatusDescription);
                Assert.AreEqual(string.Empty, progress.CurrentOperation);
                Assert.AreEqual(expectedState, progress.AsyncState);
                foreach ((string StatusDescription, string CurrentOperation, Exception Error) evt in events)
                {
                    syncEvent.Set();
                    syncEvent.WaitOne();
                    if (evt.Error is null)
                    {
                        if (evt.StatusDescription is null)
                        {
                            progress.ReportCurrentOperation(evt.CurrentOperation);
                            Assert.AreEqual(evt.CurrentOperation, progress.CurrentOperation);
                        }
                        else
                        {
                            expectedStatusDescription = evt.StatusDescription;
                            if (evt.CurrentOperation is null)
                            {
                                progress.ReportStatusDescription(expectedStatusDescription);
                                Assert.AreEqual(string.Empty, progress.CurrentOperation);
                            }
                            else
                            {
                                progress.ReportStatusDescription(expectedStatusDescription, evt.CurrentOperation);
                                Assert.AreEqual(evt.CurrentOperation, progress.CurrentOperation);
                            }
                        }
                    }
                    else if (evt.StatusDescription is null)
                    {
                        progress.ReportException(evt.Error);
                        Assert.AreEqual(string.Empty, progress.CurrentOperation);
                    }
                    else
                    {
                        expectedStatusDescription = evt.StatusDescription;
                        if (evt.CurrentOperation is null)
                        {
                            progress.ReportException(evt.Error, evt.StatusDescription);
                            Assert.AreEqual(string.Empty, progress.CurrentOperation);
                        }
                        else
                        {
                            progress.ReportException(evt.Error, evt.StatusDescription, evt.CurrentOperation);
                            Assert.AreEqual(evt.CurrentOperation, progress.CurrentOperation);
                        }
                    }
                    Assert.AreEqual(expectedStatusDescription, progress.StatusDescription);
                }
                syncEvent.Set();
                syncEvent.WaitOne();
                return resultValue;
            });

        private static Task<TResult> TestTimedFuncAsync<TResult>(ITimedBackgroundProgress<ITimedBackgroundProgressEvent> progress, string expectedActivity, string expectedStatusDescription, TResult resultValue,
            AutoResetEvent syncEvent, params (string StatusDescription, string CurrentOperation, Exception Error)[] events) => Task.Run(() =>
            {
                Assert.AreEqual(expectedActivity, progress.Activity);
                Assert.AreEqual(expectedStatusDescription, progress.StatusDescription);
                Assert.AreEqual(string.Empty, progress.CurrentOperation);
                foreach ((string StatusDescription, string CurrentOperation, Exception Error) evt in events)
                {
                    syncEvent.Set();
                    syncEvent.WaitOne();
                    if (evt.Error is null)
                    {
                        if (evt.StatusDescription is null)
                        {
                            progress.ReportCurrentOperation(evt.CurrentOperation);
                            Assert.AreEqual(evt.CurrentOperation, progress.CurrentOperation);
                        }
                        else
                        {
                            expectedStatusDescription = evt.StatusDescription;
                            if (evt.CurrentOperation is null)
                            {
                                progress.ReportStatusDescription(expectedStatusDescription);
                                Assert.AreEqual(string.Empty, progress.CurrentOperation);
                            }
                            else
                            {
                                progress.ReportStatusDescription(expectedStatusDescription, evt.CurrentOperation);
                                Assert.AreEqual(evt.CurrentOperation, progress.CurrentOperation);
                            }
                        }
                    }
                    else if (evt.StatusDescription is null)
                    {
                        progress.ReportException(evt.Error);
                        Assert.AreEqual(string.Empty, progress.CurrentOperation);
                    }
                    else
                    {
                        expectedStatusDescription = evt.StatusDescription;
                        if (evt.CurrentOperation is null)
                        {
                            progress.ReportException(evt.Error, evt.StatusDescription);
                            Assert.AreEqual(string.Empty, progress.CurrentOperation);
                        }
                        else
                        {
                            progress.ReportException(evt.Error, evt.StatusDescription, evt.CurrentOperation);
                            Assert.AreEqual(evt.CurrentOperation, progress.CurrentOperation);
                        }
                    }
                    Assert.AreEqual(expectedStatusDescription, progress.StatusDescription);
                }
                syncEvent.Set();
                syncEvent.WaitOne();
                return resultValue;
            });

        private static Task<TResult> TestTimedFuncStateAsync<TState, TResult>(ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>> progress, string expectedActivity, string expectedStatusDescription,
            TState expectedState, TResult resultValue, AutoResetEvent syncEvent, params (string StatusDescription, string CurrentOperation, Exception Error)[] events) => Task.Run<TResult>(() =>
        {
            Assert.AreEqual(expectedActivity, progress.Activity);
            Assert.AreEqual(expectedStatusDescription, progress.StatusDescription);
            Assert.AreEqual(string.Empty, progress.CurrentOperation);
            Assert.AreEqual(expectedState, progress.AsyncState);
            foreach ((string StatusDescription, string CurrentOperation, Exception Error) evt in events)
            {
                syncEvent.Set();
                syncEvent.WaitOne();
                if (evt.Error is null)
                {
                    if (evt.StatusDescription is null)
                    {
                        progress.ReportCurrentOperation(evt.CurrentOperation);
                        Assert.AreEqual(evt.CurrentOperation, progress.CurrentOperation);
                    }
                    else
                    {
                        expectedStatusDescription = evt.StatusDescription;
                        if (evt.CurrentOperation is null)
                        {
                            progress.ReportStatusDescription(expectedStatusDescription);
                            Assert.AreEqual(string.Empty, progress.CurrentOperation);
                        }
                        else
                        {
                            progress.ReportStatusDescription(expectedStatusDescription, evt.CurrentOperation);
                            Assert.AreEqual(evt.CurrentOperation, progress.CurrentOperation);
                        }
                    }
                }
                else if (evt.StatusDescription is null)
                {
                    progress.ReportException(evt.Error);
                    Assert.AreEqual(string.Empty, progress.CurrentOperation);
                }
                else
                {
                    expectedStatusDescription = evt.StatusDescription;
                    if (evt.CurrentOperation is null)
                    {
                        progress.ReportException(evt.Error, evt.StatusDescription);
                        Assert.AreEqual(string.Empty, progress.CurrentOperation);
                    }
                    else
                    {
                        progress.ReportException(evt.Error, evt.StatusDescription, evt.CurrentOperation);
                        Assert.AreEqual(evt.CurrentOperation, progress.CurrentOperation);
                    }
                }
                Assert.AreEqual(expectedStatusDescription, progress.StatusDescription);
            }
            syncEvent.Set();
            syncEvent.WaitOne();
            return resultValue;
        });

        [TestMethod("InvokeAsync(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task>, string, string)")]
        public void InvokeAsyncActionStringStringTest()
        {
            IBackgroundProgressService service = Hosting.GetRequiredService<IBackgroundProgressService>();
            using AutoResetEvent syncEvent = new(false);
            string activity = "Test Activity";
            string statusDescription = "Initial Description";
            Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate = async progress => await TestActionAsync(progress, activity, statusDescription, syncEvent,
                (null, OPERATION_FIRST, null),
                (STATUS_FIRST, null, null),
                (STATUS_SECOND, OPERATION_SECOND, null),
                (null, OPERATION_THIRD, null));
            IBackgroundOperation backgroundOperation = service.InvokeAsync(asyncMethodDelegate, activity, statusDescription);
            Assert.IsNotNull(backgroundOperation);
            ObserverHelper<IBackgroundProgressEvent> observer = new();
            using IDisposable subscription = backgroundOperation.Subscribe(observer);
            Assert.IsNotNull(subscription);

            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            Assert.IsFalse(backgroundOperation.IsCancellationRequested);
            Assert.IsFalse(backgroundOperation.ParentId.HasValue);
            Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_FIRST, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_FIRST, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_SECOND, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_THIRD, backgroundOperation.CurrentOperation);

            Assert.IsTrue(observer.TryDequeue(out Observed<IBackgroundProgressEvent> observed));
            Assert.IsFalse(observed.IsComplete);
            Assert.IsNull(observed.Error);
            Assert.IsNotNull(observed.Value);
            Assert.AreEqual(activity, observed.Value.Activity);
            Assert.AreEqual(statusDescription, observed.Value.StatusDescription);
            Assert.AreEqual(OPERATION_FIRST, observed.Value.CurrentOperation);
            Assert.IsFalse(observed.Value.Code.HasValue);
            Assert.IsFalse(observed.Value.ParentId.HasValue);
            Assert.IsFalse(observed.Value.PercentComplete.HasValue);

            Assert.IsTrue(observer.TryDequeue(out observed));
            Assert.IsFalse(observed.IsComplete);
            Assert.IsNull(observed.Error);
            Assert.IsNotNull(observed.Value);
            Assert.AreEqual(activity, observed.Value.Activity);
            Assert.AreEqual(STATUS_FIRST, observed.Value.StatusDescription);
            Assert.AreEqual(string.Empty, observed.Value.CurrentOperation);
            Assert.IsFalse(observed.Value.Code.HasValue);
            Assert.IsFalse(observed.Value.ParentId.HasValue);
            Assert.IsFalse(observed.Value.PercentComplete.HasValue);

            Assert.IsTrue(observer.TryDequeue(out observed));
            Assert.IsFalse(observed.IsComplete);
            Assert.IsNull(observed.Error);
            Assert.IsNotNull(observed.Value);
            Assert.AreEqual(activity, observed.Value.Activity);
            Assert.AreEqual(STATUS_SECOND, observed.Value.StatusDescription);
            Assert.AreEqual(OPERATION_SECOND, observed.Value.CurrentOperation);
            Assert.IsFalse(observed.Value.Code.HasValue);
            Assert.IsFalse(observed.Value.ParentId.HasValue);
            Assert.IsFalse(observed.Value.PercentComplete.HasValue);

            Assert.IsTrue(observer.TryDequeue(out observed));
            Assert.IsFalse(observed.IsComplete);
            Assert.IsNull(observed.Error);
            Assert.IsNotNull(observed.Value);
            Assert.AreEqual(activity, observed.Value.Activity);
            Assert.AreEqual(STATUS_SECOND, observed.Value.StatusDescription);
            Assert.AreEqual(OPERATION_THIRD, observed.Value.CurrentOperation);
            Assert.IsFalse(observed.Value.Code.HasValue);
            Assert.IsFalse(observed.Value.ParentId.HasValue);
            Assert.IsFalse(observed.Value.PercentComplete.HasValue);

            syncEvent.Set();
            backgroundOperation.Task.Wait();

            Assert.IsTrue(observer.TryDequeue(out observed));
            Assert.IsFalse(observed.IsComplete);
            Assert.IsNull(observed.Error);
            if (observed.Value is IBackgroundOperationCompletedEvent completedEvent)
            {
                Assert.AreEqual(activity, completedEvent.Activity);
                Assert.AreEqual(STATUS_SECOND, completedEvent.StatusDescription);
                Assert.AreEqual(OPERATION_THIRD, completedEvent.CurrentOperation);
                Assert.IsTrue(completedEvent.RanToCompletion);
                Assert.IsNull(completedEvent.Error);
                Assert.IsFalse(completedEvent.Code.HasValue);
                Assert.IsFalse(completedEvent.ParentId.HasValue);
                Assert.IsFalse(completedEvent.PercentComplete.HasValue);
            }
            else
                Assert.Fail("Final event is not IBackgroundOperationCompletedEvent");
            Assert.IsFalse(observer.TryDequeue(out observed));

            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_THIRD, backgroundOperation.CurrentOperation);
        }

        [TestMethod("InvokeAsync(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task>, string, string, CancellationToken[])")]
        public void InvokeAsyncActionStringStringCancellationTest()
        {
            IBackgroundProgressService service = Hosting.GetRequiredService<IBackgroundProgressService>();
            using AutoResetEvent syncEvent = new(false);
            string activity = "Test Activity";
            string statusDescription = "Initial Description";
            using CancellationTokenSource tokenSource = new();
            Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate = async progress => await TestActionAsync(progress, activity, statusDescription, syncEvent,
                (null, OPERATION_FIRST, null),
                (STATUS_FIRST, null, null),
                (STATUS_SECOND, OPERATION_SECOND, null),
                (null, OPERATION_THIRD, null));
            IBackgroundOperation backgroundOperation = service.InvokeAsync(asyncMethodDelegate, activity, statusDescription, tokenSource.Token);
            Assert.IsNotNull(backgroundOperation);
            ObserverHelper<IBackgroundProgressEvent> observer = new();
            using IDisposable subscription = backgroundOperation.Subscribe(observer);
            Assert.IsNotNull(subscription);

            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            Assert.IsFalse(backgroundOperation.IsCancellationRequested);
            Assert.IsFalse(backgroundOperation.ParentId.HasValue);
            Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_FIRST, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_FIRST, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_SECOND, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_THIRD, backgroundOperation.CurrentOperation);
        }

        [TestMethod("InvokeAsync(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task>, Func<IBackgroundOperation, IBackgroundOperationCompletedEvent>, string, string)")]
        public void InvokeAsyncActionCompletedStringStringTest()
        {
            IBackgroundProgressService service = Hosting.GetRequiredService<IBackgroundProgressService>();
            using AutoResetEvent syncEvent = new(false);
            string activity = "Test Activity";
            string statusDescription = "Initial Description";
            Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate = async progress => await TestActionAsync(progress, activity, statusDescription, syncEvent,
                (null, OPERATION_FIRST, null),
                (STATUS_FIRST, null, null),
                (STATUS_SECOND, OPERATION_SECOND, null),
                (null, OPERATION_THIRD, null));
            Func<IBackgroundOperation, IBackgroundOperationCompletedEvent> onCompleted = op => throw new AssertInconclusiveException();
            IBackgroundOperation backgroundOperation = service.InvokeAsync(asyncMethodDelegate, onCompleted, activity, statusDescription);
            Assert.IsNotNull(backgroundOperation);
            ObserverHelper<IBackgroundProgressEvent> observer = new();
            using IDisposable subscription = backgroundOperation.Subscribe(observer);
            Assert.IsNotNull(subscription);

            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            Assert.IsFalse(backgroundOperation.IsCancellationRequested);
            Assert.IsFalse(backgroundOperation.ParentId.HasValue);
            Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_FIRST, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_FIRST, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_SECOND, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_THIRD, backgroundOperation.CurrentOperation);
        }

        [TestMethod("InvokeAsync(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task>, Func<IBackgroundOperation, IBackgroundOperationCompletedEvent>, string, string, CancellationToken[])")]
        public void InvokeAsyncActionCompletedStringStringCancellationTest()
        {
            IBackgroundProgressService service = Hosting.GetRequiredService<IBackgroundProgressService>();
            using AutoResetEvent syncEvent = new(false);
            string activity = "Test Activity";
            string statusDescription = "Initial Description";
            using CancellationTokenSource tokenSource = new();
            Func<IBackgroundProgress<IBackgroundProgressEvent>, Task> asyncMethodDelegate = async progress => await TestActionAsync(progress, activity, statusDescription, syncEvent,
                (null, OPERATION_FIRST, null),
                (STATUS_FIRST, null, null),
                (STATUS_SECOND, OPERATION_SECOND, null),
                (null, OPERATION_THIRD, null));
            Func<IBackgroundOperation, IBackgroundOperationCompletedEvent> onCompleted = op => throw new AssertInconclusiveException();
            IBackgroundOperation backgroundOperation = service.InvokeAsync(asyncMethodDelegate, onCompleted, activity, statusDescription, tokenSource.Token);
            Assert.IsNotNull(backgroundOperation);
            ObserverHelper<IBackgroundProgressEvent> observer = new();
            using IDisposable subscription = backgroundOperation.Subscribe(observer);
            Assert.IsNotNull(subscription);

            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            Assert.IsFalse(backgroundOperation.IsCancellationRequested);
            Assert.IsFalse(backgroundOperation.ParentId.HasValue);
            Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_FIRST, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_FIRST, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_SECOND, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_THIRD, backgroundOperation.CurrentOperation);
        }

        [TestMethod("InvokeAsync(Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task>, string, string)")]
        public void InvokeAsyncTimedActionStringStringTest()
        {
            IBackgroundProgressService service = Hosting.GetRequiredService<IBackgroundProgressService>();
            using AutoResetEvent syncEvent = new(false);
            string activity = "Test Activity";
            string statusDescription = "Initial Description";
            Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task> asyncMethodDelegate = async progress => await TestTimedActionAsync(progress, activity, statusDescription, syncEvent,
                (null, OPERATION_FIRST, null),
                (STATUS_FIRST, null, null),
                (STATUS_SECOND, OPERATION_SECOND, null),
                (null, OPERATION_THIRD, null));
            ITimedBackgroundOperation backgroundOperation = service.InvokeAsync(asyncMethodDelegate, activity, statusDescription);
            Assert.IsNotNull(backgroundOperation);
            ObserverHelper<ITimedBackgroundProgressEvent> observer = new();
            using IDisposable subscription = backgroundOperation.Subscribe(observer);
            Assert.IsNotNull(subscription);

            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            Assert.IsFalse(backgroundOperation.IsCancellationRequested);
            Assert.IsFalse(backgroundOperation.ParentId.HasValue);
            Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_FIRST, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_FIRST, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_SECOND, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_THIRD, backgroundOperation.CurrentOperation);
        }

        [TestMethod("InvokeAsync(Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task>, string, string, CancellationToken[])")]
        public void InvokeAsyncTimedActionStringStringCancellationTest()
        {
            IBackgroundProgressService service = Hosting.GetRequiredService<IBackgroundProgressService>();
            using AutoResetEvent syncEvent = new(false);
            string activity = "Test Activity";
            string statusDescription = "Initial Description";
            using CancellationTokenSource tokenSource = new();
            Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task> asyncMethodDelegate = async progress => await TestTimedActionAsync(progress, activity, statusDescription, syncEvent,
                (null, OPERATION_FIRST, null),
                (STATUS_FIRST, null, null),
                (STATUS_SECOND, OPERATION_SECOND, null),
                (null, OPERATION_THIRD, null));
            ITimedBackgroundOperation backgroundOperation = service.InvokeAsync(asyncMethodDelegate, activity, statusDescription, tokenSource.Token);
            Assert.IsNotNull(backgroundOperation);
            ObserverHelper<ITimedBackgroundProgressEvent> observer = new();
            using IDisposable subscription = backgroundOperation.Subscribe(observer);
            Assert.IsNotNull(subscription);

            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            Assert.IsFalse(backgroundOperation.IsCancellationRequested);
            Assert.IsFalse(backgroundOperation.ParentId.HasValue);
            Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_FIRST, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_FIRST, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_SECOND, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_THIRD, backgroundOperation.CurrentOperation);
        }

        [TestMethod("InvokeAsync(Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task>, Func<ITimedBackgroundOperation, ITimedBackgroundOperationCompletedEvent>, string, string)")]
        public void InvokeAsyncTimedActionCompletedStringStringTest()
        {
            IBackgroundProgressService service = Hosting.GetRequiredService<IBackgroundProgressService>();
            using AutoResetEvent syncEvent = new(false);
            string activity = "Test Activity";
            string statusDescription = "Initial Description";
            Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task> asyncMethodDelegate = async progress => await TestTimedActionAsync(progress, activity, statusDescription, syncEvent,
                (null, OPERATION_FIRST, null),
                (STATUS_FIRST, null, null),
                (STATUS_SECOND, OPERATION_SECOND, null),
                (null, OPERATION_THIRD, null));
            Func<ITimedBackgroundOperation, ITimedBackgroundOperationCompletedEvent> onCompleted = op => throw new AssertInconclusiveException();
            ITimedBackgroundOperation backgroundOperation = service.InvokeAsync(asyncMethodDelegate, onCompleted, activity, statusDescription);
            Assert.IsNotNull(backgroundOperation);
            ObserverHelper<ITimedBackgroundProgressEvent> observer = new();
            using IDisposable subscription = backgroundOperation.Subscribe(observer);
            Assert.IsNotNull(subscription);

            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            Assert.IsFalse(backgroundOperation.IsCancellationRequested);
            Assert.IsFalse(backgroundOperation.ParentId.HasValue);
            Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_FIRST, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_FIRST, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_SECOND, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_THIRD, backgroundOperation.CurrentOperation);
        }

        [TestMethod("InvokeAsync(Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task>, Func<ITimedBackgroundOperation, ITimedBackgroundOperationCompletedEvent>, string, string, CancellationToken[])")]
        public void InvokeAsyncTimedActionCompletedStringStringCancellationTest()
        {
            IBackgroundProgressService service = Hosting.GetRequiredService<IBackgroundProgressService>();
            using AutoResetEvent syncEvent = new(false);
            string activity = "Test Activity";
            string statusDescription = "Initial Description";
            using CancellationTokenSource tokenSource = new();
            Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task> asyncMethodDelegate = async progress => await TestTimedActionAsync(progress, activity, statusDescription, syncEvent,
                (null, OPERATION_FIRST, null),
                (STATUS_FIRST, null, null),
                (STATUS_SECOND, OPERATION_SECOND, null),
                (null, OPERATION_THIRD, null));
            Func<ITimedBackgroundOperation, ITimedBackgroundOperationCompletedEvent> onCompleted = op => throw new AssertInconclusiveException();
            ITimedBackgroundOperation backgroundOperation = service.InvokeAsync(asyncMethodDelegate, onCompleted, activity, statusDescription, tokenSource.Token);
            Assert.IsNotNull(backgroundOperation);
            ObserverHelper<ITimedBackgroundProgressEvent> observer = new();
            using IDisposable subscription = backgroundOperation.Subscribe(observer);
            Assert.IsNotNull(subscription);

            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            Assert.IsFalse(backgroundOperation.IsCancellationRequested);
            Assert.IsFalse(backgroundOperation.ParentId.HasValue);
            Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_FIRST, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_FIRST, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_SECOND, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_THIRD, backgroundOperation.CurrentOperation);
        }

        [TestMethod("InvokeAsync<TResult>(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>>, string, string)")]
        public void InvokeAsyncFuncStringStringTest()
        {
            IBackgroundProgressService service = Hosting.GetRequiredService<IBackgroundProgressService>();
            using AutoResetEvent syncEvent = new(false);
            string activity = "Test Activity";
            string statusDescription = "Initial Description";
            int expectedResult = 7; 
            Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<int>> asyncMethodDelegate = async progress => await TestFuncAsync(progress, activity, statusDescription, expectedResult, syncEvent,
                (null, OPERATION_FIRST, null),
                (STATUS_FIRST, null, null),
                (STATUS_SECOND, OPERATION_SECOND, null),
                (null, OPERATION_THIRD, null));
            IBackgroundFunc<int> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, activity, statusDescription);
            Assert.IsNotNull(backgroundOperation);
            ObserverHelper<IBackgroundProgressEvent> observer = new();
            using IDisposable subscription = backgroundOperation.Subscribe(observer);
            Assert.IsNotNull(subscription);

            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            Assert.IsFalse(backgroundOperation.IsCancellationRequested);
            Assert.IsFalse(backgroundOperation.ParentId.HasValue);
            Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_FIRST, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_FIRST, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_SECOND, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_THIRD, backgroundOperation.CurrentOperation);
        }

        [TestMethod("InvokeAsync<TResult>(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>>, string, string, CancellationToken[])")]
        public void InvokeAsyncFuncStringStringCancellationTest()
        {
            IBackgroundProgressService service = Hosting.GetRequiredService<IBackgroundProgressService>();
            using AutoResetEvent syncEvent = new(false);
            string activity = "Test Activity";
            string statusDescription = "Initial Description";
            using CancellationTokenSource tokenSource = new();
            int expectedResult = 7;
            Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<int>> asyncMethodDelegate = async progress => await TestFuncAsync(progress, activity, statusDescription, expectedResult, syncEvent,
                (null, OPERATION_FIRST, null),
                (STATUS_FIRST, null, null),
                (STATUS_SECOND, OPERATION_SECOND, null),
                (null, OPERATION_THIRD, null));
            IBackgroundFunc<int> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, activity, statusDescription, tokenSource.Token);
            Assert.IsNotNull(backgroundOperation);
            ObserverHelper<IBackgroundProgressEvent> observer = new();
            using IDisposable subscription = backgroundOperation.Subscribe(observer);
            Assert.IsNotNull(subscription);

            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            Assert.IsFalse(backgroundOperation.IsCancellationRequested);
            Assert.IsFalse(backgroundOperation.ParentId.HasValue);
            Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_FIRST, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_FIRST, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_SECOND, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_THIRD, backgroundOperation.CurrentOperation);
        }

        [TestMethod("InvokeAsync<TResult>(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>>, Func<IBackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>>, string, string)")]
        public void InvokeAsyncFuncCompletedStringStringTest()
        {
            IBackgroundProgressService service = Hosting.GetRequiredService<IBackgroundProgressService>();
            using AutoResetEvent syncEvent = new(false);
            string activity = "Test Activity";
            string statusDescription = "Initial Description";
            int expectedResult = 7;
            Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<int>> asyncMethodDelegate = async progress => await TestFuncAsync(progress, activity, statusDescription, expectedResult, syncEvent,
                (null, OPERATION_FIRST, null),
                (STATUS_FIRST, null, null),
                (STATUS_SECOND, OPERATION_SECOND, null),
                (null, OPERATION_THIRD, null));
            Func<IBackgroundFunc<int>, IBackgroundOperationResultEvent<int>> onCompleted = op => throw new AssertInconclusiveException();
            IBackgroundFunc<int> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, onCompleted, activity, statusDescription);
            Assert.IsNotNull(backgroundOperation);
            ObserverHelper<IBackgroundProgressEvent> observer = new();
            using IDisposable subscription = backgroundOperation.Subscribe(observer);
            Assert.IsNotNull(subscription);

            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            Assert.IsFalse(backgroundOperation.IsCancellationRequested);
            Assert.IsFalse(backgroundOperation.ParentId.HasValue);
            Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_FIRST, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_FIRST, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_SECOND, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_THIRD, backgroundOperation.CurrentOperation);
        }

        [TestMethod("InvokeAsync<TResult>(Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<TResult>>, Func<IBackgroundFunc<TResult>, IBackgroundOperationResultEvent<TResult>>, string, string, CancellationToken[])")]
        public void InvokeAsyncFuncCompletedStringStringCancellationTest()
        {
            IBackgroundProgressService service = Hosting.GetRequiredService<IBackgroundProgressService>();
            using AutoResetEvent syncEvent = new(false);
            string activity = "Test Activity";
            string statusDescription = "Initial Description";
            using CancellationTokenSource tokenSource = new();
            int expectedResult = 7;
            Func<IBackgroundProgress<IBackgroundProgressEvent>, Task<int>> asyncMethodDelegate = async progress => await TestFuncAsync(progress, activity, statusDescription, expectedResult, syncEvent,
                (null, OPERATION_FIRST, null),
                (STATUS_FIRST, null, null),
                (STATUS_SECOND, OPERATION_SECOND, null),
                (null, OPERATION_THIRD, null));
            Func<IBackgroundFunc<int>, IBackgroundOperationResultEvent<int>> onCompleted = op => throw new AssertInconclusiveException();
            IBackgroundFunc<int> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, onCompleted, activity, statusDescription, tokenSource.Token);
            Assert.IsNotNull(backgroundOperation);
            ObserverHelper<IBackgroundProgressEvent> observer = new();
            using IDisposable subscription = backgroundOperation.Subscribe(observer);
            Assert.IsNotNull(subscription);

            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            Assert.IsFalse(backgroundOperation.IsCancellationRequested);
            Assert.IsFalse(backgroundOperation.ParentId.HasValue);
            Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_FIRST, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_FIRST, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_SECOND, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_THIRD, backgroundOperation.CurrentOperation);
        }

        [TestMethod("InvokeAsync<TResult>(Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<TResult>>, string, string)")]
        public void InvokeAsyncTimedFuncStringStringTest()
        {
            IBackgroundProgressService service = Hosting.GetRequiredService<IBackgroundProgressService>();
            using AutoResetEvent syncEvent = new(false);
            string activity = "Test Activity";
            string statusDescription = "Initial Description";
            int expectedResult = 7;
            Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<int>> asyncMethodDelegate = async progress => await TestTimedFuncAsync(progress, activity, statusDescription, expectedResult, syncEvent,
                (null, OPERATION_FIRST, null),
                (STATUS_FIRST, null, null),
                (STATUS_SECOND, OPERATION_SECOND, null),
                (null, OPERATION_THIRD, null));
            ITimedBackgroundFunc<int> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, activity, statusDescription);
            Assert.IsNotNull(backgroundOperation);
            ObserverHelper<ITimedBackgroundProgressEvent> observer = new();
            using IDisposable subscription = backgroundOperation.Subscribe(observer);
            Assert.IsNotNull(subscription);

            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            Assert.IsFalse(backgroundOperation.IsCancellationRequested);
            Assert.IsFalse(backgroundOperation.ParentId.HasValue);
            Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_FIRST, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_FIRST, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_SECOND, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_THIRD, backgroundOperation.CurrentOperation);
        }

        [TestMethod("InvokeAsync<TResult>(Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<TResult>>, string, string, CancellationToken[])")]
        public void InvokeAsyncTimedFuncStringStringCancellationTest()
        {
            IBackgroundProgressService service = Hosting.GetRequiredService<IBackgroundProgressService>();
            using AutoResetEvent syncEvent = new(false);
            string activity = "Test Activity";
            string statusDescription = "Initial Description";
            using CancellationTokenSource tokenSource = new();
            int expectedResult = 7;
            Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<int>> asyncMethodDelegate = async progress => await TestTimedFuncAsync(progress, activity, statusDescription, expectedResult, syncEvent,
                (null, OPERATION_FIRST, null),
                (STATUS_FIRST, null, null),
                (STATUS_SECOND, OPERATION_SECOND, null),
                (null, OPERATION_THIRD, null));
            ITimedBackgroundFunc<int> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, activity, statusDescription, tokenSource.Token);
            Assert.IsNotNull(backgroundOperation);
            ObserverHelper<ITimedBackgroundProgressEvent> observer = new();
            using IDisposable subscription = backgroundOperation.Subscribe(observer);
            Assert.IsNotNull(subscription);

            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            Assert.IsFalse(backgroundOperation.IsCancellationRequested);
            Assert.IsFalse(backgroundOperation.ParentId.HasValue);
            Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_FIRST, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_FIRST, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_SECOND, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_THIRD, backgroundOperation.CurrentOperation);
        }

        [TestMethod("InvokeAsync<TResult>(Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<TResult>>, Func<ITimedBackgroundFunc<TResult>, ITimedBackgroundOperationResultEvent<TResult>>, string, string)")]
        public void InvokeAsyncTimedFuncCompletedStringStringTest()
        {
            IBackgroundProgressService service = Hosting.GetRequiredService<IBackgroundProgressService>();
            using AutoResetEvent syncEvent = new(false);
            string activity = "Test Activity";
            string statusDescription = "Initial Description";
            int expectedResult = 7;
            Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<int>> asyncMethodDelegate = async progress => await TestTimedFuncAsync(progress, activity, statusDescription, expectedResult, syncEvent,
                (null, OPERATION_FIRST, null),
                (STATUS_FIRST, null, null),
                (STATUS_SECOND, OPERATION_SECOND, null),
                (null, OPERATION_THIRD, null));
            Func<ITimedBackgroundFunc<int>, ITimedBackgroundOperationResultEvent<int>> onCompleted = op => throw new AssertInconclusiveException();
            ITimedBackgroundFunc<int> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, onCompleted, activity, statusDescription);
            Assert.IsNotNull(backgroundOperation);
            ObserverHelper<ITimedBackgroundProgressEvent> observer = new();
            using IDisposable subscription = backgroundOperation.Subscribe(observer);
            Assert.IsNotNull(subscription);

            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            Assert.IsFalse(backgroundOperation.IsCancellationRequested);
            Assert.IsFalse(backgroundOperation.ParentId.HasValue);
            Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_FIRST, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_FIRST, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_SECOND, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_THIRD, backgroundOperation.CurrentOperation);
        }

        [TestMethod("InvokeAsync<TResult>(Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<TResult>>, Func<ITimedBackgroundFunc<TResult>, ITimedBackgroundOperationResultEvent<TResult>>, string, string, CancellationToken[])")]
        public void InvokeAsyncTimedFuncCompletedStringStringCancellationTest()
        {
            IBackgroundProgressService service = Hosting.GetRequiredService<IBackgroundProgressService>();
            using AutoResetEvent syncEvent = new(false);
            string activity = "Test Activity";
            string statusDescription = "Initial Description";
            using CancellationTokenSource tokenSource = new();
            int expectedResult = 7;
            Func<ITimedBackgroundProgress<ITimedBackgroundProgressEvent>, Task<int>> asyncMethodDelegate = async progress => await TestTimedFuncAsync(progress, activity, statusDescription, expectedResult, syncEvent,
                (null, OPERATION_FIRST, null),
                (STATUS_FIRST, null, null),
                (STATUS_SECOND, OPERATION_SECOND, null),
                (null, OPERATION_THIRD, null));
            Func<ITimedBackgroundFunc<int>, ITimedBackgroundOperationResultEvent<int>> onCompleted = op => throw new AssertInconclusiveException();
            ITimedBackgroundFunc<int> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, onCompleted, activity, statusDescription, tokenSource.Token);
            Assert.IsNotNull(backgroundOperation);
            ObserverHelper<ITimedBackgroundProgressEvent> observer = new();
            using IDisposable subscription = backgroundOperation.Subscribe(observer);
            Assert.IsNotNull(subscription);

            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            Assert.IsFalse(backgroundOperation.IsCancellationRequested);
            Assert.IsFalse(backgroundOperation.ParentId.HasValue);
            Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_FIRST, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_FIRST, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_SECOND, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_THIRD, backgroundOperation.CurrentOperation);
        }

        [TestMethod("InvokeAsync<TState, TResult>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>>, string, string, TState)")]
        public void InvokeAsyncFuncStringStringStateTest()
        {
            IBackgroundProgressService service = Hosting.GetRequiredService<IBackgroundProgressService>();
            using AutoResetEvent syncEvent = new(false);
            string activity = "Test Activity";
            string statusDescription = "Initial Description";
            int state = 12;
            char expectedResult = 'b';
            Func<IBackgroundProgress<int, IBackgroundProgressEvent<int>>, Task<char>> asyncMethodDelegate = async progress => await TestFuncStateAsync(progress, activity, statusDescription, state, expectedResult, syncEvent,
                (null, OPERATION_FIRST, null),
                (STATUS_FIRST, null, null),
                (STATUS_SECOND, OPERATION_SECOND, null),
                (null, OPERATION_THIRD, null));
            IBackgroundFunc<int, char> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, activity, statusDescription, state);
            Assert.IsNotNull(backgroundOperation);
            ObserverHelper<IBackgroundProgressEvent<int>> observer = new();
            using IDisposable subscription = backgroundOperation.Subscribe(observer);
            Assert.IsNotNull(subscription);

            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            Assert.IsFalse(backgroundOperation.IsCancellationRequested);
            Assert.IsFalse(backgroundOperation.ParentId.HasValue);
            Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_FIRST, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_FIRST, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_SECOND, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_THIRD, backgroundOperation.CurrentOperation);
        }

        [TestMethod("InvokeAsync<TState, TResult>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>>, string, string, TState, CancellationToken[])")]
        public void InvokeAsyncFuncStringStringStateCancellationTest()
        {
            IBackgroundProgressService service = Hosting.GetRequiredService<IBackgroundProgressService>();
            using AutoResetEvent syncEvent = new(false);
            string activity = "Test Activity";
            string statusDescription = "Initial Description";
            int state = 12;
            using CancellationTokenSource tokenSource = new();
            char expectedResult = 'b';
            Func<IBackgroundProgress<int, IBackgroundProgressEvent<int>>, Task<char>> asyncMethodDelegate = async progress => await TestFuncStateAsync(progress, activity, statusDescription, state, expectedResult, syncEvent,
                (null, OPERATION_FIRST, null),
                (STATUS_FIRST, null, null),
                (STATUS_SECOND, OPERATION_SECOND, null),
                (null, OPERATION_THIRD, null));
            IBackgroundFunc<int, char> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, activity, statusDescription, state, tokenSource.Token);
            Assert.IsNotNull(backgroundOperation);
            ObserverHelper<IBackgroundProgressEvent<int>> observer = new();
            using IDisposable subscription = backgroundOperation.Subscribe(observer);
            Assert.IsNotNull(subscription);

            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            Assert.IsFalse(backgroundOperation.IsCancellationRequested);
            Assert.IsFalse(backgroundOperation.ParentId.HasValue);
            Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_FIRST, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_FIRST, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_SECOND, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_THIRD, backgroundOperation.CurrentOperation);
        }

        [TestMethod("InvokeAsync<TState, TResult>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>>, Func<IBackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>>, string, string, TState)")]
        public void InvokeAsyncFuncCompletedStringStringStateTest()
        {
            IBackgroundProgressService service = Hosting.GetRequiredService<IBackgroundProgressService>();
            using AutoResetEvent syncEvent = new(false);
            string activity = "Test Activity";
            string statusDescription = "Initial Description";
            int state = 12;
            char expectedResult = 'b';
            Func<IBackgroundProgress<int, IBackgroundProgressEvent<int>>, Task<char>> asyncMethodDelegate = async progress => await TestFuncStateAsync(progress, activity, statusDescription, state, expectedResult, syncEvent,
                (null, OPERATION_FIRST, null),
                (STATUS_FIRST, null, null),
                (STATUS_SECOND, OPERATION_SECOND, null),
                (null, OPERATION_THIRD, null));
            Func<IBackgroundFunc<int, char>, IBackgroundOperationResultEvent<int, char>> onCompleted = op => throw new AssertInconclusiveException();
            IBackgroundFunc<int, char> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, onCompleted, activity, statusDescription, state);
            Assert.IsNotNull(backgroundOperation);
            ObserverHelper<IBackgroundProgressEvent<int>> observer = new();
            using IDisposable subscription = backgroundOperation.Subscribe(observer);
            Assert.IsNotNull(subscription);

            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            Assert.IsFalse(backgroundOperation.IsCancellationRequested);
            Assert.IsFalse(backgroundOperation.ParentId.HasValue);
            Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_FIRST, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_FIRST, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_SECOND, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_THIRD, backgroundOperation.CurrentOperation);
        }

        [TestMethod("InvokeAsync<TState, TResult>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task<TResult>>, Func<IBackgroundFunc<TState, TResult>, IBackgroundOperationResultEvent<TState, TResult>>, string, string, TState, CancellationToken[])")]
        public void InvokeAsyncFuncCompletedStringStringStateCancellationTest()
        {
            IBackgroundProgressService service = Hosting.GetRequiredService<IBackgroundProgressService>();
            using AutoResetEvent syncEvent = new(false);
            string activity = "Test Activity";
            string statusDescription = "Initial Description";
            int state = 12;
            using CancellationTokenSource tokenSource = new();
            char expectedResult = 'b';
            Func<IBackgroundProgress<int, IBackgroundProgressEvent<int>>, Task<char>> asyncMethodDelegate = async progress => await TestFuncStateAsync(progress, activity, statusDescription, state, expectedResult, syncEvent,
                (null, OPERATION_FIRST, null),
                (STATUS_FIRST, null, null),
                (STATUS_SECOND, OPERATION_SECOND, null),
                (null, OPERATION_THIRD, null));
            Func<IBackgroundFunc<int, char>, IBackgroundOperationResultEvent<int, char>> onCompleted = op => throw new AssertInconclusiveException();
            IBackgroundFunc<int, char> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, onCompleted, activity, statusDescription, state, tokenSource.Token);
            Assert.IsNotNull(backgroundOperation);
            ObserverHelper<IBackgroundProgressEvent<int>> observer = new();
            using IDisposable subscription = backgroundOperation.Subscribe(observer);
            Assert.IsNotNull(subscription);

            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            Assert.IsFalse(backgroundOperation.IsCancellationRequested);
            Assert.IsFalse(backgroundOperation.ParentId.HasValue);
            Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_FIRST, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_FIRST, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_SECOND, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_THIRD, backgroundOperation.CurrentOperation);
        }

        [TestMethod("InvokeAsync<TState, TResult>(Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task<TResult>>, string, string, TState)")]
        public void InvokeAsyncTimedFuncStringStringStateTest()
        {
            IBackgroundProgressService service = Hosting.GetRequiredService<IBackgroundProgressService>();
            using AutoResetEvent syncEvent = new(false);
            string activity = "Test Activity";
            string statusDescription = "Initial Description";
            int state = 12;
            char expectedResult = 'b';
            Func<ITimedBackgroundProgress<int, ITimedBackgroundProgressEvent<int>>, Task<char>> asyncMethodDelegate = async progress => await TestTimedFuncStateAsync(progress, activity, statusDescription, state, expectedResult,
                syncEvent,
                (null, OPERATION_FIRST, null),
                (STATUS_FIRST, null, null),
                (STATUS_SECOND, OPERATION_SECOND, null),
                (null, OPERATION_THIRD, null));
            ITimedBackgroundFunc<int, char> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, activity, statusDescription, state);
            Assert.IsNotNull(backgroundOperation);
            ObserverHelper<ITimedBackgroundProgressEvent<int>> observer = new();
            using IDisposable subscription = backgroundOperation.Subscribe(observer);
            Assert.IsNotNull(subscription);

            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            Assert.IsFalse(backgroundOperation.IsCancellationRequested);
            Assert.IsFalse(backgroundOperation.ParentId.HasValue);
            Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_FIRST, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_FIRST, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_SECOND, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_THIRD, backgroundOperation.CurrentOperation);
        }

        [TestMethod("InvokeAsync<TState, TResult>(Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task<TResult>>, string, string, TState, CancellationToken[])")]
        public void InvokeAsyncTimedFuncStringStringStateCancellationTest()
        {
            IBackgroundProgressService service = Hosting.GetRequiredService<IBackgroundProgressService>();
            using AutoResetEvent syncEvent = new(false);
            string activity = "Test Activity";
            string statusDescription = "Initial Description";
            int state = 12;
            using CancellationTokenSource tokenSource = new();
            char expectedResult = 'b';
            Func<ITimedBackgroundProgress<int, ITimedBackgroundProgressEvent<int>>, Task<char>> asyncMethodDelegate = async progress => await TestTimedFuncStateAsync(progress, activity, statusDescription, state, expectedResult,
                syncEvent,
                (null, OPERATION_FIRST, null),
                (STATUS_FIRST, null, null),
                (STATUS_SECOND, OPERATION_SECOND, null),
                (null, OPERATION_THIRD, null));
            ITimedBackgroundFunc<int, char> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, activity, statusDescription, state, tokenSource.Token);
            Assert.IsNotNull(backgroundOperation);
            ObserverHelper<ITimedBackgroundProgressEvent<int>> observer = new();
            using IDisposable subscription = backgroundOperation.Subscribe(observer);
            Assert.IsNotNull(subscription);

            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            Assert.IsFalse(backgroundOperation.IsCancellationRequested);
            Assert.IsFalse(backgroundOperation.ParentId.HasValue);
            Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_FIRST, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_FIRST, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_SECOND, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_THIRD, backgroundOperation.CurrentOperation);
        }

        [TestMethod("InvokeAsync<TState, TResult>(Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task<TResult>>, Func<ITimedBackgroundFunc<TState, TResult>, ITimedBackgroundOperationResultEvent<TState, TResult>>, string, string, TState)")]
        public void InvokeAsyncTimedFuncCompletedStringStringStateTest()
        {
            IBackgroundProgressService service = Hosting.GetRequiredService<IBackgroundProgressService>();
            using AutoResetEvent syncEvent = new(false);
            string activity = "Test Activity";
            string statusDescription = "Initial Description";
            int state = 12;
            char expectedResult = 'b';
            Func<ITimedBackgroundProgress<int, ITimedBackgroundProgressEvent<int>>, Task<char>> asyncMethodDelegate = async progress => await TestTimedFuncStateAsync(progress, activity, statusDescription, state, expectedResult,
                syncEvent,
                (null, OPERATION_FIRST, null),
                (STATUS_FIRST, null, null),
                (STATUS_SECOND, OPERATION_SECOND, null),
                (null, OPERATION_THIRD, null));
            Func<ITimedBackgroundFunc<int, char>, ITimedBackgroundOperationResultEvent<int, char>> onCompleted = op => throw new AssertInconclusiveException();
            ITimedBackgroundFunc<int, char> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, onCompleted, activity, statusDescription, state);
            Assert.IsNotNull(backgroundOperation);
            ObserverHelper<ITimedBackgroundProgressEvent<int>> observer = new();
            using IDisposable subscription = backgroundOperation.Subscribe(observer);
            Assert.IsNotNull(subscription);

            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            Assert.IsFalse(backgroundOperation.IsCancellationRequested);
            Assert.IsFalse(backgroundOperation.ParentId.HasValue);
            Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_FIRST, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_FIRST, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_SECOND, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_THIRD, backgroundOperation.CurrentOperation);
        }

        [TestMethod("InvokeAsync<TState, TResult>(Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task<TResult>>, Func<ITimedBackgroundFunc<TState, TResult>, ITimedBackgroundOperationResultEvent<TState, TResult>>, string, string, TState, CancellationToken[])")]
        public void InvokeAsyncTimedFuncCompletedStringStringStateCancellationTest()
        {
            IBackgroundProgressService service = Hosting.GetRequiredService<IBackgroundProgressService>();
            using AutoResetEvent syncEvent = new(false);
            string activity = "Test Activity";
            string statusDescription = "Initial Description";
            int state = 12;
            using CancellationTokenSource tokenSource = new();
            char expectedResult = 'b';
            Func<ITimedBackgroundProgress<int, ITimedBackgroundProgressEvent<int>>, Task<char>> asyncMethodDelegate = async progress => await TestTimedFuncStateAsync(progress, activity, statusDescription, state, expectedResult,
                syncEvent,
                (null, OPERATION_FIRST, null),
                (STATUS_FIRST, null, null),
                (STATUS_SECOND, OPERATION_SECOND, null),
                (null, OPERATION_THIRD, null));
            Func<ITimedBackgroundFunc<int, char>, ITimedBackgroundOperationResultEvent<int, char>> onCompleted = op => throw new AssertInconclusiveException();
            ITimedBackgroundFunc<int, char> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, onCompleted, activity, statusDescription, state, tokenSource.Token);
            Assert.IsNotNull(backgroundOperation);
            ObserverHelper<ITimedBackgroundProgressEvent<int>> observer = new();
            using IDisposable subscription = backgroundOperation.Subscribe(observer);
            Assert.IsNotNull(subscription);

            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            Assert.IsFalse(backgroundOperation.IsCancellationRequested);
            Assert.IsFalse(backgroundOperation.ParentId.HasValue);
            Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_FIRST, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_FIRST, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_SECOND, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_THIRD, backgroundOperation.CurrentOperation);
        }

        [TestMethod("InvokeAsync<TState>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task>, string, string, TState)")]
        public void InvokeAsyncActionStringStringStateTest()
        {
            IBackgroundProgressService service = Hosting.GetRequiredService<IBackgroundProgressService>();
            using AutoResetEvent syncEvent = new(false);
            string activity = "Test Activity";
            string statusDescription = "Initial Description";
            int state = 12;
            Func<IBackgroundProgress<int, IBackgroundProgressEvent<int>>, Task> asyncMethodDelegate = async progress => await TestActionStateAsync(progress, activity, statusDescription, state, syncEvent,
                (null, OPERATION_FIRST, null),
                (STATUS_FIRST, null, null),
                (STATUS_SECOND, OPERATION_SECOND, null),
                (null, OPERATION_THIRD, null));
            IBackgroundOperation<int> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, activity, statusDescription, state);
            Assert.IsNotNull(backgroundOperation);
            ObserverHelper<IBackgroundProgressEvent<int>> observer = new();
            using IDisposable subscription = backgroundOperation.Subscribe(observer);
            Assert.IsNotNull(subscription);

            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            Assert.IsFalse(backgroundOperation.IsCancellationRequested);
            Assert.IsFalse(backgroundOperation.ParentId.HasValue);
            Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_FIRST, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_FIRST, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_SECOND, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_THIRD, backgroundOperation.CurrentOperation);
        }

        [TestMethod("InvokeAsync<TState>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task>, string, string, TState, CancellationToken[])")]
        public void InvokeAsyncActionStringStringStateCancellationTest()
        {
            IBackgroundProgressService service = Hosting.GetRequiredService<IBackgroundProgressService>();
            using AutoResetEvent syncEvent = new(false);
            string activity = "Test Activity";
            string statusDescription = "Initial Description";
            int state = 12;
            using CancellationTokenSource tokenSource = new();
            Func<IBackgroundProgress<int, IBackgroundProgressEvent<int>>, Task> asyncMethodDelegate = async progress => await TestActionStateAsync(progress, activity, statusDescription, state, syncEvent,
                (null, OPERATION_FIRST, null),
                (STATUS_FIRST, null, null),
                (STATUS_SECOND, OPERATION_SECOND, null),
                (null, OPERATION_THIRD, null));
            IBackgroundOperation<int> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, activity, statusDescription, state, tokenSource.Token);
            Assert.IsNotNull(backgroundOperation);
            ObserverHelper<IBackgroundProgressEvent<int>> observer = new();
            using IDisposable subscription = backgroundOperation.Subscribe(observer);
            Assert.IsNotNull(subscription);

            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            Assert.IsFalse(backgroundOperation.IsCancellationRequested);
            Assert.IsFalse(backgroundOperation.ParentId.HasValue);
            Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_FIRST, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_FIRST, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_SECOND, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_THIRD, backgroundOperation.CurrentOperation);
        }

        [TestMethod("InvokeAsync<TState>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task>, Func<IBackgroundOperation<TState>, IBackgroundOperationCompletedEvent<TState>>, string, string, TState)")]
        public void InvokeAsyncActionCompletedStringStringStateTest()
        {
            IBackgroundProgressService service = Hosting.GetRequiredService<IBackgroundProgressService>();
            using AutoResetEvent syncEvent = new(false);
            string activity = "Test Activity";
            string statusDescription = "Initial Description";
            int state = 12;
            Func<IBackgroundProgress<int, IBackgroundProgressEvent<int>>, Task> asyncMethodDelegate = async progress => await TestActionStateAsync(progress, activity, statusDescription, state, syncEvent,
                (null, OPERATION_FIRST, null),
                (STATUS_FIRST, null, null),
                (STATUS_SECOND, OPERATION_SECOND, null),
                (null, OPERATION_THIRD, null));
            Func<IBackgroundOperation<int>, IBackgroundOperationCompletedEvent<int>> onCompleted = op => throw new AssertInconclusiveException();
            IBackgroundOperation<int> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, onCompleted, activity, statusDescription, state);
            Assert.IsNotNull(backgroundOperation);
            ObserverHelper<IBackgroundProgressEvent<int>> observer = new();
            using IDisposable subscription = backgroundOperation.Subscribe(observer);
            Assert.IsNotNull(subscription);

            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            Assert.IsFalse(backgroundOperation.IsCancellationRequested);
            Assert.IsFalse(backgroundOperation.ParentId.HasValue);
            Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_FIRST, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_FIRST, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_SECOND, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_THIRD, backgroundOperation.CurrentOperation);
        }

        [TestMethod("InvokeAsync<TState>(Func<IBackgroundProgress<TState, IBackgroundProgressEvent<TState>>, Task>, Func<IBackgroundOperation<TState>, IBackgroundOperationCompletedEvent<TState>>, string, string, TState, CancellationToken[])")]
        public void InvokeAsyncActionCompletedStringStringStateCancellationTest()
        {
            IBackgroundProgressService service = Hosting.GetRequiredService<IBackgroundProgressService>();
            using AutoResetEvent syncEvent = new(false);
            string activity = "Test Activity";
            string statusDescription = "Initial Description";
            int state = 12;
            using CancellationTokenSource tokenSource = new();
            Func<IBackgroundProgress<int, IBackgroundProgressEvent<int>>, Task> asyncMethodDelegate = async progress => await TestActionStateAsync(progress, activity, statusDescription, state, syncEvent,
                (null, OPERATION_FIRST, null),
                (STATUS_FIRST, null, null),
                (STATUS_SECOND, OPERATION_SECOND, null),
                (null, OPERATION_THIRD, null));
            Func<IBackgroundOperation<int>, IBackgroundOperationCompletedEvent<int>> onCompleted = op => throw new AssertInconclusiveException();
            IBackgroundOperation<int> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, onCompleted, activity, statusDescription, state, tokenSource.Token);
            Assert.IsNotNull(backgroundOperation);
            ObserverHelper<IBackgroundProgressEvent<int>> observer = new();
            using IDisposable subscription = backgroundOperation.Subscribe(observer);
            Assert.IsNotNull(subscription);

            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            Assert.IsFalse(backgroundOperation.IsCancellationRequested);
            Assert.IsFalse(backgroundOperation.ParentId.HasValue);
            Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_FIRST, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_FIRST, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_SECOND, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_THIRD, backgroundOperation.CurrentOperation);
        }

        [TestMethod("InvokeAsync<TState>(Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task>, string, string, TState)")]
        public void InvokeAsyncTimedActionStringStringStateTest()
        {
            IBackgroundProgressService service = Hosting.GetRequiredService<IBackgroundProgressService>();
            using AutoResetEvent syncEvent = new(false);
            string activity = "Test Activity";
            string statusDescription = "Initial Description";
            int state = 12;
            Func<ITimedBackgroundProgress<int, ITimedBackgroundProgressEvent<int>>, Task> asyncMethodDelegate = async progress => await TestTimedActionStateAsync(progress, activity, statusDescription, state, syncEvent,
                (null, OPERATION_FIRST, null),
                (STATUS_FIRST, null, null),
                (STATUS_SECOND, OPERATION_SECOND, null),
                (null, OPERATION_THIRD, null));
            ITimedBackgroundOperation<int> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, activity, statusDescription, state);
            Assert.IsNotNull(backgroundOperation);
            ObserverHelper<ITimedBackgroundProgressEvent<int>> observer = new();
            using IDisposable subscription = backgroundOperation.Subscribe(observer);
            Assert.IsNotNull(subscription);

            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            Assert.IsFalse(backgroundOperation.IsCancellationRequested);
            Assert.IsFalse(backgroundOperation.ParentId.HasValue);
            Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_FIRST, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_FIRST, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_SECOND, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_THIRD, backgroundOperation.CurrentOperation);
        }

        [TestMethod("InvokeAsync<TState>(Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task>, string, string, TState, CancellationToken[])")]
        public void InvokeAsyncTimedActionStringStringStateCancellationTest()
        {
            IBackgroundProgressService service = Hosting.GetRequiredService<IBackgroundProgressService>();
            using AutoResetEvent syncEvent = new(false);
            string activity = "Test Activity";
            string statusDescription = "Initial Description";
            int state = 12;
            using CancellationTokenSource tokenSource = new();
            Func<ITimedBackgroundProgress<int, ITimedBackgroundProgressEvent<int>>, Task> asyncMethodDelegate = async progress => await TestTimedActionStateAsync(progress, activity, statusDescription, state, syncEvent,
                (null, OPERATION_FIRST, null),
                (STATUS_FIRST, null, null),
                (STATUS_SECOND, OPERATION_SECOND, null),
                (null, OPERATION_THIRD, null));
            ITimedBackgroundOperation<int> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, activity, statusDescription, state, tokenSource.Token);
            Assert.IsNotNull(backgroundOperation);
            ObserverHelper<ITimedBackgroundProgressEvent<int>> observer = new();
            using IDisposable subscription = backgroundOperation.Subscribe(observer);
            Assert.IsNotNull(subscription);

            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            Assert.IsFalse(backgroundOperation.IsCancellationRequested);
            Assert.IsFalse(backgroundOperation.ParentId.HasValue);
            Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_FIRST, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_FIRST, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_SECOND, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_THIRD, backgroundOperation.CurrentOperation);
        }

        [TestMethod("InvokeAsync<TState>(Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task>, Func<ITimedBackgroundOperation<TState>, ITimedBackgroundOperationCompletedEvent<TState>>, string, string, TState)")]
        public void InvokeTimedAsyncActionCompletedStringStringStateTest()
        {
            IBackgroundProgressService service = Hosting.GetRequiredService<IBackgroundProgressService>();
            using AutoResetEvent syncEvent = new(false);
            string activity = "Test Activity";
            string statusDescription = "Initial Description";
            int state = 12;
            Func<ITimedBackgroundProgress<int, ITimedBackgroundProgressEvent<int>>, Task> asyncMethodDelegate = async progress => await TestTimedActionStateAsync(progress, activity, statusDescription, state, syncEvent,
                (null, OPERATION_FIRST, null),
                (STATUS_FIRST, null, null),
                (STATUS_SECOND, OPERATION_SECOND, null),
                (null, OPERATION_THIRD, null));
            Func<ITimedBackgroundOperation<int>, ITimedBackgroundOperationCompletedEvent<int>> onCompleted = op => throw new AssertInconclusiveException();
            ITimedBackgroundOperation<int> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, onCompleted, activity, statusDescription, state);
            Assert.IsNotNull(backgroundOperation);
            ObserverHelper<ITimedBackgroundProgressEvent<int>> observer = new();
            using IDisposable subscription = backgroundOperation.Subscribe(observer);
            Assert.IsNotNull(subscription);

            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            Assert.IsFalse(backgroundOperation.IsCancellationRequested);
            Assert.IsFalse(backgroundOperation.ParentId.HasValue);
            Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_FIRST, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_FIRST, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_SECOND, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_THIRD, backgroundOperation.CurrentOperation);
        }

        [TestMethod("InvokeAsync<TState>(Func<ITimedBackgroundProgress<TState, ITimedBackgroundProgressEvent<TState>>, Task>, Func<ITimedBackgroundOperation<TState>, ITimedBackgroundOperationCompletedEvent<TState>>, string, string, TState, CancellationToken[])")]
        public void InvokeAsyncTimedActionCompletedStringStringStateCancellationTest()
        {
            IBackgroundProgressService service = Hosting.GetRequiredService<IBackgroundProgressService>();
            using AutoResetEvent syncEvent = new(false);
            string activity = "Test Activity";
            string statusDescription = "Initial Description";
            int state = 12;
            using CancellationTokenSource tokenSource = new();
            Func<ITimedBackgroundProgress<int, ITimedBackgroundProgressEvent<int>>, Task> asyncMethodDelegate = async progress => await TestTimedActionStateAsync(progress, activity, statusDescription, state, syncEvent,
                (null, OPERATION_FIRST, null),
                (STATUS_FIRST, null, null),
                (STATUS_SECOND, OPERATION_SECOND, null),
                (null, OPERATION_THIRD, null));
            Func<ITimedBackgroundOperation<int>, ITimedBackgroundOperationCompletedEvent<int>> onCompleted = op => throw new AssertInconclusiveException();
            ITimedBackgroundOperation<int> backgroundOperation = service.InvokeAsync(asyncMethodDelegate, onCompleted, activity, statusDescription, state, tokenSource.Token);
            Assert.IsNotNull(backgroundOperation);
            ObserverHelper<ITimedBackgroundProgressEvent<int>> observer = new();
            using IDisposable subscription = backgroundOperation.Subscribe(observer);
            Assert.IsNotNull(subscription);

            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            Assert.IsFalse(backgroundOperation.IsCancellationRequested);
            Assert.IsFalse(backgroundOperation.ParentId.HasValue);
            Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(statusDescription, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_FIRST, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_FIRST, backgroundOperation.StatusDescription);
            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_SECOND, backgroundOperation.CurrentOperation);
            syncEvent.Set();
            if (!syncEvent.WaitOne(100))
            {
                backgroundOperation.Task.Wait();
                Assert.Fail("Sync event not signaled");
            }
            Assert.AreEqual(activity, backgroundOperation.Activity);
            Assert.AreEqual(STATUS_SECOND, backgroundOperation.StatusDescription);
            Assert.AreEqual(OPERATION_THIRD, backgroundOperation.CurrentOperation);
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
