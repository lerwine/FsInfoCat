using System;
using NUnit.Framework;
using FsInfoCat.PS;
using System.Collections.Generic;
using System.Management.Automation;
using System.Collections;
using FsInfoCat.Test.Helpers;

namespace FsInfoCat.Test
{
    [TestFixture]
    public class PSExtensionMethodsTest
    {
        [SetUp]
        public void Setup()
        {
        }

        public class ActionInvokeTestHelper<T1, T2>
        {
            public bool WasActionInvoked { get; protected set; }
            public T2 ActionInvokedValue { get; protected set; }
            public bool ExpectActionInvoke { get; }
            public T1 InputObj { get; }
            public ActionInvokeTestHelper(T1 inputObj, bool expectActionInvoke)
            {
                InputObj = inputObj;
                ExpectActionInvoke = expectActionInvoke;
            }
            public void ActionHandler(T2 value)
            {
                WasActionInvoked = true;
                ActionInvokedValue = value;
            }

            public Tuple<T2, R> GetReturnValue<R>(R result)
            {
                Assert.That(ExpectActionInvoke, Is.EqualTo(WasActionInvoked), (WasActionInvoked) ? "Callback invocation not expected" : "Callback invocation not expected");
                return new Tuple<T2, R>(ActionInvokedValue, result);
            }
        }

        public class CoerceTestHelper<T1, T2, C> : ActionInvokeTestHelper<T1, T2>
        {
            private readonly Func<T2, C> _coersionFunc;
            public bool ExpectCoersionInvoke { get; }
            public bool WasCoersionInvoked { get; private set; }
            public T2 CoersionInvokedValue { get; private set; }
            public bool WasSuccessActionInvoked { get; protected set; }
            public C SuccessActionInvokedValue { get; protected set; }
            public bool ExpectSuccessActionInvoke { get; }

            public CoerceTestHelper(T1 inputObj, Func<T2, C> coersionFunc, bool expectCoersionInvoke, bool expectActionInvoke) : base(inputObj, expectActionInvoke)
            {
                _coersionFunc = coersionFunc;
                ExpectCoersionInvoke = expectCoersionInvoke;
            }
            public C CoersionHandler(T2 value)
            {
                WasCoersionInvoked = true;
                CoersionInvokedValue = value;
                return _coersionFunc(value);
            }
            public void SuccessActionHandler(C value)
            {
                WasSuccessActionInvoked = true;
                SuccessActionInvokedValue = value;
            }
            public Tuple<T2, C, R> GetReturnValue<R>(R result, C coercedValue)
            {
                Assert.That(ExpectActionInvoke, Is.EqualTo(WasActionInvoked), (WasActionInvoked) ? "Action callback invocation not expected" : "Action callback invocation not expected");
                Assert.That(ExpectCoersionInvoke, Is.EqualTo(WasCoersionInvoked), (WasCoersionInvoked) ? "Coersiosn callback invocation not expected" : "Coersiosn callback invocation not expected");
                return new Tuple<T2, C, R>(ActionInvokedValue, coercedValue, result);
            }
        }

        public class TryCoerceTestHelper<T1, T2, C> : ActionInvokeTestHelper<T1, T2>
        {
            private readonly ExtensionMethods.TryCoerceHandler<T2, C> _coersionFunc;
            public bool ExpectCoersionInvoke { get; }
            public bool WasCoersionInvoked { get; private set; }
            public T2 CoersionInvokedValue { get; private set; }
            public bool WasSuccessActionInvoked { get; protected set; }
            public C SuccessActionInvokedValue { get; protected set; }
            public bool ExpectSuccessActionInvoke { get; }

            public TryCoerceTestHelper(T1 inputObj, ExtensionMethods.TryCoerceHandler<T2, C> coersionFunc, bool expectCoersionInvoke, bool expectActionInvoke) : base(inputObj, expectActionInvoke)
            {
                _coersionFunc = coersionFunc;
                ExpectCoersionInvoke = expectCoersionInvoke;
            }
            public bool CoersionHandler(T2 value, out C result)
            {
                WasCoersionInvoked = true;
                CoersionInvokedValue = value;
                return _coersionFunc(value, out result);
            }
            public void SuccessActionHandler(C value)
            {
                WasSuccessActionInvoked = true;
                SuccessActionInvokedValue = value;
            }
            public Tuple<T2, C, R> GetReturnValue<R>(R result, C coercedValue)
            {
                Assert.That(ExpectActionInvoke, Is.EqualTo(WasActionInvoked), (WasActionInvoked) ? "Action callback invocation not expected" : "Action callback invocation not expected");
                Assert.That(ExpectCoersionInvoke, Is.EqualTo(WasCoersionInvoked), (WasCoersionInvoked) ? "Coersiosn callback invocation not expected" : "Coersiosn callback invocation not expected");
                return new Tuple<T2, C, R>(ActionInvokedValue, coercedValue, result);
            }
        }

        public static IEnumerable<TestCaseData> GetInvokeIfNotNull1TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetInvokeIfNotNull1TestCases))]
        public IFunctionInvocationResult<Uri, bool> InvokeIfNotNull1Test(Uri target)
        {
            return InvocationListener<Uri>.Apply(a => target.InvokeIfNotNull(a));
        }

        public static IEnumerable<TestCaseData> GetInvokeIfNotNull2TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetInvokeIfNotNull2TestCases))]
        public IFunctionInvocationResult<int, bool> InvokeIfNotNull2Test(int? target)
        {
            return InvocationListener<int>.Apply(a => target.InvokeIfNotNull(a));
        }

        public static IEnumerable<TestCaseData> GetTryCoerceValue3TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryCoerceValue3TestCases))]
        public FunctionInvocationResult<Uri, bool> TryCoerceValue3Test(string inputObj, Func<string, Uri> ifNotNull, Func<Uri> ifNull)
        {
            IFunctionResult<IFunctionInvocationResult<string, Uri>, IFunctionInvocationResult<Uri>, Uri, bool>
            //
            // Func<string, Uri> ifNotNull => IFunctionInvocationResult<string, Uri>
            // Func<Uri> ifNull => IFunctionInvocationResult<Uri>
            // IFunctionInvocationResult<IFunctionInvocationResult<string, Uri>, IFunctionInvocationResult<Uri>, bool>
            // bool TryCoerceValue<V, T>(this V inputObj, Func<V, T> ifNotNull, Func<T> ifNull, out R result)
        }

        public static IEnumerable<TestCaseData> GetTryCoerceValue4TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryCoerceValue4TestCases))]
        public Tuple<string, Uri, bool> TryCoerceValue4Test(CoerceTestHelper<string, string, Uri> helper)
        {
            return helper.GetReturnValue(helper.InputObj.TryCoerceValue(helper.CoersionHandler, out Uri result), result);
        }

        public static IEnumerable<TestCaseData> GetTryCoerceValue5TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryCoerceValue5TestCases))]
        public Tuple<int, string, bool> TryCoerceValue5Test(CoerceTestHelper<int?, int, string> helper, Func<string> ifNull)
        {
            return helper.GetReturnValue(helper.InputObj.TryCoerceValue(helper.CoersionHandler, ifNull, out string result), result);
        }

        public static IEnumerable<TestCaseData> GetTryCoerceValue6TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryCoerceValue6TestCases))]
        public Tuple<int, string, bool> TryCoerceValue6Test(CoerceTestHelper<int?, int, string> helper)
        {
            return helper.GetReturnValue(helper.InputObj.TryCoerceValue(helper.CoersionHandler, out string result), result);
        }

        public static IEnumerable<TestCaseData> GetTryCast1TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryCast1TestCases))]
        public Tuple<object, int?, bool> TryCast1Test(TryCoerceTestHelper<object, object, int?> helper, bool asRawValueIfFail)
        {
            return helper.GetReturnValue(helper.InputObj.TryCast(helper.SuccessActionHandler, helper.CoersionHandler, asRawValueIfFail, out int? result), result);
        }

        public static IEnumerable<TestCaseData> GetTryCast2TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryCast2TestCases))]
        public Tuple<object, int?, bool> TryCast2Test(TryCoerceTestHelper<object, object, int?> helper)
        {
            return helper.GetReturnValue(helper.InputObj.TryCast(helper.SuccessActionHandler, helper.CoersionHandler, out int? result), result);
        }

        public static IEnumerable<TestCaseData> GetTryCast3TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryCast3TestCases))]
        public Tuple<object, int?, bool> TryCast3Test(TryCoerceTestHelper<object, object, int?> helper)
        {
            return helper.GetReturnValue(helper.InputObj.TryCast(helper.SuccessActionHandler, out int? result), result);
        }

        public static IEnumerable<TestCaseData> GetTryCast4TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryCast4TestCases))]
        public Tuple<object, int?, bool> TryCast4Test(TryCoerceTestHelper<object, object, int?> helper, bool asRawValueIfFail)
        {
            return helper.GetReturnValue(helper.InputObj.TryCast(helper.CoersionHandler, asRawValueIfFail, out int? result), result);
        }

        public static IEnumerable<TestCaseData> GetTryCast5TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryCast5TestCases))]
        public Tuple<object, int?, bool> TryCast5Test(TryCoerceTestHelper<object, object, int?> helper)
        {
            return helper.GetReturnValue(helper.InputObj.TryCast(helper.CoersionHandler, out int? result), result);
        }

        public static IEnumerable<TestCaseData> GetTryCast6TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryCast6TestCases))]
        public Tuple<bool, int?> TryCast6Test(object inputObj)
        {
            return new Tuple<bool, int?>(inputObj.TryCast(out int? result), result);
        }

        public static IEnumerable<TestCaseData> GetTryCast7TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryCast7TestCases))]
        public Tuple<object, string, bool> TryCast7Test(TryCoerceTestHelper<object, object, string> helper, bool nullReturnValue, Func<string> ifNull, bool asRawValueIfFail)
        {
            return helper.GetReturnValue(helper.InputObj.TryCast(helper.SuccessActionHandler, nullReturnValue, ifNull, helper.CoersionHandler, asRawValueIfFail, out string result), result);
        }

        public static IEnumerable<TestCaseData> GetTryCast8TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryCast8TestCases))]
        public Tuple<object, string, bool> TryCast8Test(TryCoerceTestHelper<object, object, string> helper, bool nullReturnValue, Func<string> ifNull)
        {
            return helper.GetReturnValue(helper.InputObj.TryCast(helper.SuccessActionHandler, nullReturnValue, ifNull, helper.CoersionHandler, out string result), result);
        }

        public static IEnumerable<TestCaseData> GetTryCast9TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryCast9TestCases))]
        public Tuple<object, string, bool> TryCast9Test(TryCoerceTestHelper<object, object, string> helper, bool nullReturnValue, Func<string> ifNull)
        {
            return helper.GetReturnValue(helper.InputObj.TryCast(helper.SuccessActionHandler, nullReturnValue, ifNull, out string result), result);
        }

        public static IEnumerable<TestCaseData> GetTryCast10TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryCast10TestCases))]
        public Tuple<object, string, bool> TryCast10Test(TryCoerceTestHelper<object, object, string> helper, Action<string> ifSuccess, bool nullReturnValue, bool asRawValueIfFail)
        {
            return helper.GetReturnValue(helper.InputObj.TryCast(helper.SuccessActionHandler, nullReturnValue, helper.CoersionHandler, asRawValueIfFail, out string result), result);
        }

        public static IEnumerable<TestCaseData> GetTryCast11TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryCast11TestCases))]
        public Tuple<object, string, bool> TryCast11Test(TryCoerceTestHelper<object, object, string> helper, Action<string> ifSuccess, bool nullReturnValue)
        {
            return helper.GetReturnValue(helper.InputObj.TryCast(helper.SuccessActionHandler, nullReturnValue, helper.CoersionHandler, out string result), result);
        }

        public static IEnumerable<TestCaseData> GetTryCast12TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryCast12TestCases))]
        public Tuple<object, string, bool> TryCast12Test(TryCoerceTestHelper<object, object, string> helper, Action<string> ifSuccess, bool nullReturnValue)
        {
            return helper.GetReturnValue(helper.InputObj.TryCast(helper.SuccessActionHandler, nullReturnValue, out string result), result);
        }

        public static IEnumerable<TestCaseData> GetTryCast13TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryCast13TestCases))]
        public Tuple<object, string, bool> TryCast13Test(TryCoerceTestHelper<object, object, string> helper, Action<string> ifSuccess, Func<string> ifNull, bool asRawValueIfFail)
        {
            return helper.GetReturnValue(helper.InputObj.TryCast(helper.SuccessActionHandler, ifNull, helper.CoersionHandler, asRawValueIfFail, out string result), result);
        }

        public static IEnumerable<TestCaseData> GetTryCast14TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryCast14TestCases))]
        public Tuple<object, string, bool> TryCast14Test(TryCoerceTestHelper<object, object, string> helper, Action<string> ifSuccess, Func<string> ifNull)
        {
            return helper.GetReturnValue(helper.InputObj.TryCast(helper.SuccessActionHandler, ifNull, helper.CoersionHandler, out string result), result);
        }

        public static IEnumerable<TestCaseData> GetTryCast15TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryCast15TestCases))]
        public Tuple<object, string, bool> TryCast15Test(TryCoerceTestHelper<object, object, string> helper, Action<string> ifSuccess, Func<string> ifNull)
        {
            return helper.GetReturnValue(helper.InputObj.TryCast(helper.SuccessActionHandler, ifNull, out string result), result);
        }

        public static IEnumerable<TestCaseData> GetTryCast16TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryCast16TestCases))]
        public Tuple<object, string, bool> TryCast16Test(TryCoerceTestHelper<object, object, string> helper, Action<string> ifSuccess, bool asRawValueIfFail)
        {
            return helper.GetReturnValue(helper.InputObj.TryCast(helper.SuccessActionHandler, helper.CoersionHandler, asRawValueIfFail, out string result), result);
        }

        public static IEnumerable<TestCaseData> GetTryCast17TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryCast17TestCases))]
        public Tuple<object, string, bool> TryCast17Test(TryCoerceTestHelper<object, object, string> helper, Action<string> ifSuccess)
        {
            return helper.GetReturnValue(helper.InputObj.TryCast(helper.SuccessActionHandler, helper.CoersionHandler, out string result), result);
        }

        public static IEnumerable<TestCaseData> GetTryCast18TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryCast18TestCases))]
        public Tuple<object, string, bool> TryCast18Test(TryCoerceTestHelper<object, object, string> helper, Action<string> ifSuccess)
        {
            return helper.GetReturnValue(helper.InputObj.TryCast(helper.SuccessActionHandler, out string result), result);
        }

        public static IEnumerable<TestCaseData> GetTryCast19TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryCast19TestCases))]
        public Tuple<object, string, bool> TryCast19Test(TryCoerceTestHelper<object, object, string> helper, bool nullReturnValue, Func<string> ifNull, bool asRawValueIfFail)
        {
            return helper.GetReturnValue(helper.InputObj.TryCast(nullReturnValue, ifNull, helper.CoersionHandler, asRawValueIfFail, out string result), result);
        }

        public static IEnumerable<TestCaseData> GetTryCast20TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryCast20TestCases))]
        public Tuple<object, string, bool> TryCast20Test(TryCoerceTestHelper<object, object, string> helper, bool nullReturnValue, Func<string> ifNull)
        {
            return helper.GetReturnValue(helper.InputObj.TryCast(nullReturnValue, ifNull, helper.CoersionHandler, out string result), result);
        }

        public static IEnumerable<TestCaseData> GetTryCast21TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryCast21TestCases))]
        public Tuple<bool, string> TryCast21Test(object inputObj, bool nullReturnValue, Func<string> ifNull)
        {
            return new Tuple<bool, string>(inputObj.TryCast(nullReturnValue, ifNull, out string result), result);
        }

        public static IEnumerable<TestCaseData> GetTryCast22TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryCast22TestCases))]
        public Tuple<object, string, bool> TryCast22Test(TryCoerceTestHelper<object, object, string> helper, bool nullReturnValue, bool asRawValueIfFail)
        {
            return helper.GetReturnValue(helper.InputObj.TryCast(nullReturnValue, helper.CoersionHandler, asRawValueIfFail, out string result), result);
        }

        public static IEnumerable<TestCaseData> GetTryCast23TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryCast23TestCases))]
        public Tuple<object, string, bool> TryCast23Test(TryCoerceTestHelper<object, object, string> helper, bool nullReturnValue)
        {
            return helper.GetReturnValue(helper.InputObj.TryCast(nullReturnValue, helper.CoersionHandler, out string result), result);
        }

        public static IEnumerable<TestCaseData> GetTryCast24TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryCast24TestCases))]
        public Tuple<bool, string> TryCast24Test(object inputObj, bool nullReturnValue)
        {
            return new Tuple<bool, string>(inputObj.TryCast(nullReturnValue, out string result), result);
        }

        public static IEnumerable<TestCaseData> GetTryCast25TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryCast25TestCases))]
        public Tuple<object, string, bool> TryCast25Test(TryCoerceTestHelper<object, object, string> helper, Func<string> ifNull, bool asRawValueIfFail)
        {
            return helper.GetReturnValue(helper.InputObj.TryCast(ifNull, helper.CoersionHandler, asRawValueIfFail, out string result), result);
        }

        public static IEnumerable<TestCaseData> GetTryCast26TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryCast6TestCases))]
        public Tuple<object, string, bool> TryCast26Test(TryCoerceTestHelper<object, object, string> helper, Func<string> ifNull)
        {
            return helper.GetReturnValue(helper.InputObj.TryCast(ifNull, helper.CoersionHandler, out string result), result);
        }

        public static IEnumerable<TestCaseData> GetTryCast27TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryCast7TestCases))]
        public Tuple<bool, string> TryCast27Test(object inputObj, Func<string> ifNull)
        {
            return new Tuple<bool, string>(inputObj.TryCast(ifNull, out string result), result);
        }

        public static IEnumerable<TestCaseData> GetTryCast28TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryCast8TestCases))]
        public Tuple<object, string, bool> TryCast28Test(TryCoerceTestHelper<object, object, string> helper, bool asRawValueIfFail)
        {
            return helper.GetReturnValue(helper.InputObj.TryCast(helper.CoersionHandler, asRawValueIfFail, out string result), result);
        }

        public static IEnumerable<TestCaseData> GetTryCast29TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryCast9TestCases))]
        public Tuple<object, string, bool> TryCast29Test(TryCoerceTestHelper<object, object, string> helper)
        {
            return helper.GetReturnValue(helper.InputObj.TryCast(helper.CoersionHandler, out string result), result);
        }

        public static IEnumerable<TestCaseData> GetTryCast30TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryCast30TestCases))]
        public Tuple<bool, string> TryCast30Test(object inputObj)
        {
            return new Tuple<bool, string>(inputObj.TryCast(out string result), result);
        }

        public static IEnumerable<TestCaseData> GetTryGetErrorMessage1TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryGetErrorMessage1TestCases))]
        public string TryGetErrorMessage1Test(IContainsErrorRecord source, ErrorCategory defaultCategory, string defaultErrorId, string defaultReason, object defaultTargetObject)
        {
            // TODO: Write test for bool TryGetErrorMessage(this IContainsErrorRecord source, ErrorCategory defaultCategory, string defaultErrorId, string defaultReason, object defaultTargetObject, out string message, out ErrorCategory category, out string errorId, out string reason, out object targetObject)
            Assert.Inconclusive();
            throw new NotImplementedException();
        }

        public static IEnumerable<TestCaseData> GetTryGetErrorMessage2TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryGetErrorMessage2TestCases))]
        public Tuple<bool, string, ErrorCategory, string, object> TryGetErrorMessage2Test(IContainsErrorRecord source, ErrorCategory defaultCategory, string defaultErrorId, object defaultTargetObject)
        {
            return new Tuple<bool, string, ErrorCategory, string, object>(source.TryGetErrorMessage(defaultCategory, defaultErrorId, defaultTargetObject, out string message, out ErrorCategory category,
                out string errorId, out object targetObject), message, category, errorId, targetObject);
        }

        public static IEnumerable<TestCaseData> GetTryGetErrorMessage3TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryGetErrorMessage3TestCases))]
        public Tuple<bool, string> TryGetError3MessageTest(IContainsErrorRecord source)
        {
            return new Tuple<bool, string>(source.TryGetErrorMessage(out string message), message);
        }

        public static IEnumerable<TestCaseData> GetTryGetErrorMessage4TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryGetErrorMessage4TestCases))]
        public Tuple<bool, string, ErrorCategory, string, string, object> TryGetError4MessageTest(Exception exception, ErrorCategory defaultCategory, string defaultErrorId, string defaultReason, object defaultTargetObject)
        {
            return new Tuple<bool, string, ErrorCategory, string, string, object>(exception.TryGetErrorMessage(defaultCategory, defaultErrorId, defaultReason, defaultTargetObject,
                out string message, out ErrorCategory category, out string errorId, out string reason, out object targetObject), message, category, errorId, reason, targetObject);
        }

        public static IEnumerable<TestCaseData> GetTryGetErrorMessage5TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryGetErrorMessage5TestCases))]
        public Tuple<bool, string, ErrorCategory, string, object> TryGetErrorMessage5Test(Exception exception, ErrorCategory defaultCategory, string defaultErrorId, object defaultTargetObject)
        {
            return new Tuple<bool, string, ErrorCategory, string, object>(exception.TryGetErrorMessage(defaultCategory, defaultErrorId, defaultTargetObject,
                out string message, out ErrorCategory category, out string errorId, out object targetObject), message, category, errorId, targetObject);
        }

        public static IEnumerable<TestCaseData> GetTryGetErrorMessage6TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryGetErrorMessage6TestCases))]
        public Tuple<bool, string> TryGetErrorMessage6Test(Exception exception)
        {
            return new Tuple<bool, string>(exception.TryGetErrorMessage(out string message), message);
        }

        public static IEnumerable<TestCaseData> GetTryGetErrorCategory1TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryGetErrorCategory1TestCases))]
        public Tuple<bool, ErrorCategory, string, string, string, object> TryGetErrorCategory1Test(ErrorRecord errorRecord)
        {
            return new Tuple<bool, ErrorCategory, string, string, string, object>(errorRecord.TryGetErrorCategory(out ErrorCategory category, out string message, out string errorId,
                out string reason, out object targetObject), category, message, errorId, reason, targetObject);
        }

        public static IEnumerable<TestCaseData> GetTryGetErrorCategory2TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryGetErrorCategory2TestCases))]
        public Tuple<bool, ErrorCategory, string, string, object> TryGetErrorCategory2Test(ErrorRecord errorRecord)
        {
            return new Tuple<bool, ErrorCategory, string, string, object>(errorRecord.TryGetErrorCategory(out ErrorCategory category, out string message, out string errorId, out object targetObject),
                category, message, errorId, targetObject);
        }

        public static IEnumerable<TestCaseData> GetTryGetErrorCategory3TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryGetErrorCategory3TestCases))]
        public Tuple<bool, ErrorCategory, string, string, string, object> TryGetErrorCategory3Test(IContainsErrorRecord source)
        {
            return new Tuple<bool, ErrorCategory, string, string, string, object>(source.TryGetErrorCategory(out ErrorCategory category, out string message, out string errorId,
                out string reason, out object targetObject), category, message, errorId, reason, targetObject);
        }

        public static IEnumerable<TestCaseData> GetTryGetErrorCategory4TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryGetErrorCategory4TestCases))]
        public Tuple<bool, ErrorCategory, string, string, object> TryGetErrorCategory4Test(IContainsErrorRecord source)
        {
            return new Tuple<bool, ErrorCategory, string, string, object>(source.TryGetErrorCategory(out ErrorCategory category, out string message, out string errorId, out object targetObject),
                category, message, errorId, targetObject);
        }

        public static IEnumerable<TestCaseData> GetTryGetErrorCategory5TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryGetErrorCategory5TestCases))]
        public Tuple<bool, ErrorCategory, string, string, string, object> TryGetErrorCategory5Test(Exception exception)
        {
            return new Tuple<bool, ErrorCategory, string, string, string, object>(exception.TryGetErrorCategory(out ErrorCategory category, out string message, out string errorId,
                out string reason, out object targetObject), category, message, errorId, reason, targetObject);
        }

        public static IEnumerable<TestCaseData> GetTryGetErrorCategory6TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryGetErrorCategory6TestCases))]
        public Tuple<bool, ErrorCategory, string, string, object> TryGetErrorCategory6Test(Exception exception)
        {
            return new Tuple<bool, ErrorCategory, string, string, object>(exception.TryGetErrorCategory(out ErrorCategory category, out string message, out string errorId, out object targetObject),
                category, message, errorId, targetObject);
        }

        public static IEnumerable<TestCaseData> GetTryGetTargetObject1TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryGetTargetObject1TestCases))]
        public Tuple<bool, object, string, string> TryGetTargetObject1Test(ErrorRecord errorRecord)
        {
            return new Tuple<bool, object, string, string>(errorRecord.TryGetTargetObject(out object targetObject, out string errorId, out string reason), targetObject, errorId, reason);
        }

        public static IEnumerable<TestCaseData> GetTryGetTargetObject2TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryGetTargetObject2TestCases))]
        public Tuple<bool, object, string, string> TryGetTargetObject2Test(IContainsErrorRecord source)
        {
            return new Tuple<bool, object, string, string>(source.TryGetTargetObject(out object targetObject, out string errorId, out string reason), targetObject, errorId, reason);
        }

        public static IEnumerable<TestCaseData> GetTryGetTargetObject3TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryGetTargetObject3TestCases))]
        public Tuple<bool, object, string, string> TryGetTargetObject3Test(Exception exception)
        {
            return new Tuple<bool, object, string, string>(exception.TryGetTargetObject(out object targetObject, out string errorId, out string reason), targetObject, errorId, reason);
        }

        public static IEnumerable<TestCaseData> GetTryGetErrorRecordTestCases() => throw new InconclusiveException("Test data not implemented");


        public class ErrorRecordComparable
        {
            private readonly ErrorRecord _errorRecord;
            private readonly string _description;
            public ErrorRecordComparable([System.Diagnostics.CodeAnalysis.AllowNull] ErrorRecord errorRecord, string description)
            {
                _errorRecord = errorRecord;
            }
            public void AssertEquals([System.Diagnostics.CodeAnalysis.AllowNull] ErrorRecord other)
            {
                if (_errorRecord is null)
                    Assert.That(other, Is.Null, "")
                    return other is null;
                if (other is null)
                    return false;
                if (ReferenceEquals(_errorRecord, other) || (Equals(other.CategoryInfo) && Equals(other.ErrorDetails) && ReferenceEquals(_errorRecord.Exception, other.Exception) &&
                    _errorRecord.FullyQualifiedErrorId == other.FullyQualifiedErrorId))
                {
                    if (_errorRecord.TargetObject is null)
                        return other.TargetObject is null;
                    if (other.TargetObject is null)
                        return false;
                    Type a = _errorRecord.TargetObject.GetType();
                    Type b = other.TargetObject.GetType();
                    if (a.Equals(b))
                    {
                        if (a.IsValueType)
                            return ((IEqualityComparer)(typeof(EqualityComparer<>)).MakeGenericType(a).GetProperty("Default").GetValue(null)).Equals(_errorRecord.TargetObject, other.TargetObject);
                        return ReferenceEquals(_errorRecord.TargetObject, other.TargetObject);
                    }
                }
                throw new NotImplementedException();
            }

            public bool Equals([System.Diagnostics.CodeAnalysis.AllowNull] ErrorCategoryInfo other)
            {
                ErrorCategoryInfo errorCategoryInfo;
                if (_errorRecord is null || (errorCategoryInfo = _errorRecord.CategoryInfo) is null)
                    return other is null;
                if (other is null)
                    return false;
                return ReferenceEquals(errorCategoryInfo, other) || (errorCategoryInfo.Activity == other.Activity && errorCategoryInfo.Category == other.Category && errorCategoryInfo.Reason == other.Reason &&
                    errorCategoryInfo.TargetName == other.TargetName && errorCategoryInfo.TargetType == other.TargetType);
            }

            public bool Equals([System.Diagnostics.CodeAnalysis.AllowNull] ErrorDetails other)
            {
                ErrorDetails errorDetails;
                if (_errorRecord is null || (errorDetails = _errorRecord.ErrorDetails) is null)
                    return other is null;
                if (other is null)
                    return false;
                return ReferenceEquals(errorDetails, other) || (errorDetails.Message == other.Message && errorDetails.RecommendedAction == other.RecommendedAction);
            }
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryGetErrorRecordTestCases))]
        public Tuple<bool, ErrorRecordComparable> TryGetErrorRecordTest(object inputObj)
        {
            // TODO: Write test for bool TryGetErrorRecord(this object inputObj, out ErrorRecord errorRecord)
            Assert.Inconclusive();
            throw new NotImplementedException();
        }

        public static IEnumerable<TestCaseData> GetTryGetExceptionTestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryGetExceptionTestCases))]
        public string TryGetExceptionTest(object inputObj)
        {
            // TODO: Write test for bool TryGetException(this object inputObj, out Exception exception)
            Assert.Inconclusive();
            throw new NotImplementedException();
        }

        public static IEnumerable<TestCaseData> GetToErrorRecordTestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetToErrorRecordTestCases))]
        public string ToErrorRecordTest(Exception exception)
        {
            // TODO: Write test for ErrorRecord ToErrorRecord(this Exception exception)
            Assert.Inconclusive();
            throw new NotImplementedException();
        }

        public static IEnumerable<TestCaseData> GetSetReasonTestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetSetReasonTestCases))]
        public string SetReasonTest(ErrorRecord errorRecord, string reason)
        {
            // TODO: Write test for ErrorRecord SetReason(this ErrorRecord errorRecord, string reason)
            Assert.Inconclusive();
            throw new NotImplementedException();
        }
    }
}
