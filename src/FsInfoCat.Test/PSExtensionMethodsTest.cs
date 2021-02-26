using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Management.Automation;
using System.Collections;
using FsInfoCat.Test.Helpers;
using FsInfoCat.PS;
//using static FsInfoCat.PS.ExtensionMethods;

namespace FsInfoCat.Test
{
    [TestFixture]
    public class PSExtensionMethodsTest
    {
        [SetUp]
        public void Setup()
        {
        }

        public static IEnumerable<TestCaseData> GetInvokeIfNotNull1TestCases()
        {
            string methodName = $"{typeof(PS.ExtensionMethods).FullName}.{nameof(PS.ExtensionMethods.InvokeIfNotNull)}<{typeof(Uri).Name}>";
            yield return new TestCaseData(null).SetDescription($"{methodName}(null)").Returns(new InvocationResult<Uri>());
            UriKind kind = UriKind.Relative;
            Uri target = new Uri("", kind);
            yield return new TestCaseData(target).SetDescription($"{methodName}(new Uri(\"{target.OriginalString}\", UriKind.{kind}))").Returns(new InvocationResult<Uri>(target));
            target = new Uri("/dir/myfile.txt", kind);
            yield return new TestCaseData(target).SetDescription($"{methodName}(new Uri(\"{target.OriginalString}\", UriKind.{kind}))").Returns(new InvocationResult<Uri>(target));
            kind = UriKind.Absolute;
            target = new Uri("file://myserver/dir/myfile.txt", kind);
            yield return new TestCaseData(target).SetDescription($"{methodName}(new Uri(\"{target.OriginalString}\", UriKind.{kind}))").Returns(new InvocationResult<Uri>(target));
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetInvokeIfNotNull1TestCases))]
        public IInvocationResult<Uri> InvokeIfNotNull1Test(Uri target)
        {
            InvocationMonitor<Uri> invocationMonitor = new InvocationMonitor<Uri>();
            target.InvokeIfNotNull(invocationMonitor.Apply);
            return invocationMonitor.ToResult();
        }

        public static IEnumerable<TestCaseData> GetInvokeIfNotNull2TestCases()
        {
            string methodName = $"{typeof(PS.ExtensionMethods).FullName}.{nameof(PS.ExtensionMethods.InvokeIfNotNull)}<{typeof(int).Name}?>";
            yield return new TestCaseData(null).SetDescription($"{methodName}(null)").Returns(new InvocationResult<int>());
            int target = 0;
            yield return new TestCaseData(target).SetDescription($"{methodName}({target})").Returns(new InvocationResult<int>(target));
            target = int.MaxValue;
            yield return new TestCaseData(target).SetDescription($"{methodName}({target})").Returns(new InvocationResult<int>(target));
            target = int.MinValue;
            yield return new TestCaseData(target).SetDescription($"{methodName}({target})").Returns(new InvocationResult<int>(target));
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetInvokeIfNotNull2TestCases))]
        public IInvocationResult<int> InvokeIfNotNull2Test(int? target)
        {
            InvocationMonitor<int> invocationMonitor = new InvocationMonitor<int>();
            target.InvokeIfNotNull(invocationMonitor.Apply);
            return invocationMonitor.ToResult();
        }

        public static IEnumerable<TestCaseData> GetTryCoerceValue3TestCases()
        {
            string args = typeof(Uri).Name;
            string methodName = $"<{typeof(string).Name}, {args}>";
            args = $"Func<{methodName}> ifNotNull, Func<{args}> ifNull, out {args} result";
            methodName = $"{typeof(PS.ExtensionMethods).FullName}.{nameof(PS.ExtensionMethods.TryCoerceValue)}<{methodName}>";
            Func<string, Uri> ifNotNullFunc = s => (Uri.TryCreate(s, UriKind.RelativeOrAbsolute, out Uri u)) ? u : null;
            Uri resultUri = new Uri(".", UriKind.Relative);
            Func<Uri> ifNullFunc = () => resultUri;
            FuncInvocationResult<Uri> ifNotNullResult = new FuncInvocationResult<Uri>();
            FuncInvocationResult<Uri> ifNullResult = new FuncInvocationResult<Uri>(resultUri);
            yield return new TestCaseData(null, ifNotNullFunc, ifNullFunc)
                .SetDescription($"{methodName}(null, {args})")
                .Returns(new FuncTestData3<IFuncInvocationResult<Uri>, IFuncInvocationResult<Uri>, Uri, bool>(false, ifNotNullResult, ifNullResult, resultUri));
            string inputObj = "";
            resultUri = new Uri(inputObj, UriKind.Relative);
            ifNotNullResult = new FuncInvocationResult<Uri>(resultUri);
            ifNullResult = new FuncInvocationResult<Uri>();
            yield return new TestCaseData(inputObj, ifNotNullFunc, ifNullFunc)
                .SetDescription($"{methodName}(\"{inputObj})\", {args}")
                .Returns(new FuncTestData3<IFuncInvocationResult<Uri>, IFuncInvocationResult<Uri>, Uri, bool>(true, ifNotNullResult, ifNullResult, resultUri));
            inputObj = "/dir/myfile.txt";
            resultUri = new Uri(inputObj, UriKind.Relative);
            ifNotNullResult = new FuncInvocationResult<Uri>(resultUri);
            yield return new TestCaseData(inputObj, ifNotNullFunc, ifNullFunc)
                .SetDescription($"{methodName}(\"{inputObj})\", {args}")
                .Returns(new FuncTestData3<IFuncInvocationResult<Uri>, IFuncInvocationResult<Uri>, Uri, bool>(true, ifNotNullResult, ifNullResult, resultUri));
            inputObj = "file://myserver/dir/myfile.txt";
            resultUri = new Uri(inputObj, UriKind.Absolute);
            ifNotNullResult = new FuncInvocationResult<Uri>(resultUri);
            yield return new TestCaseData(inputObj, ifNotNullFunc, ifNullFunc)
                .SetDescription($"{methodName}(\"{inputObj})\", {args}")
                .Returns(new FuncTestData3<IFuncInvocationResult<Uri>, IFuncInvocationResult<Uri>, Uri, bool>(true, ifNotNullResult, ifNullResult, resultUri));
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryCoerceValue3TestCases))]
        public IFuncTestData3<IFuncInvocationResult<Uri>, IFuncInvocationResult<Uri>, Uri, bool> TryCoerceValue3Test(string inputObj, Func<string, Uri> ifNotNull, Func<Uri> ifNull)
        {
            FunctionInvocationMonitor<Uri> notNullMonitor = new FunctionInvocationMonitor<Uri>();
            FunctionInvocationMonitor<Uri> ifNullMonitor = new FunctionInvocationMonitor<Uri>();
            return FuncTestData3<IFuncInvocationResult<Uri>, IFuncInvocationResult<Uri>, Uri, bool>.FromInvocation((out IFuncInvocationResult<Uri> notNullResult,
                out IFuncInvocationResult<Uri> ifNullResult, out Uri uri) =>
            {
                bool result = inputObj.TryCoerceValue(notNullMonitor.CreateProxy(ifNotNull), ifNullMonitor.CreateProxy(ifNull), out uri);
                notNullResult = notNullMonitor.ToResult();
                ifNullResult = ifNullMonitor.ToResult();
                return result;
            });
        }

        public static IEnumerable<TestCaseData> GetTryCoerceValue4TestCases()
        {
            string args = typeof(Uri).Name;
            string methodName = $"<{typeof(string).Name}, {args}>";
            args = $"Func<{methodName}> ifNotNull, Func<{args}> ifNull, out {args} result";
            methodName = $"{typeof(PS.ExtensionMethods).FullName}.{nameof(PS.ExtensionMethods.TryCoerceValue)}<{methodName}>";
            Func<string, Uri> ifNotNullFunc = s => (Uri.TryCreate(s, UriKind.RelativeOrAbsolute, out Uri u)) ? u : null;
            FuncInvocationResult<Uri> ifNotNullResult = new FuncInvocationResult<Uri>();
            yield return new TestCaseData(null, ifNotNullFunc)
                .SetDescription($"{methodName}(null, {args})")
                .Returns(new FuncTestData2<IFuncInvocationResult<Uri>, Uri, bool>(false, ifNotNullResult, null));
            string inputObj = "";
            Uri resultUri = new Uri(inputObj, UriKind.Relative);
            ifNotNullResult = new FuncInvocationResult<Uri>(resultUri);
            yield return new TestCaseData(inputObj, ifNotNullFunc)
                .SetDescription($"{methodName}(\"{inputObj})\", {args}")
                .Returns(new FuncTestData2<IFuncInvocationResult<Uri>, Uri, bool>(true, ifNotNullResult, resultUri));
            inputObj = "/dir/myfile.txt";
            resultUri = new Uri(inputObj, UriKind.Relative);
            ifNotNullResult = new FuncInvocationResult<Uri>(resultUri);
            yield return new TestCaseData(inputObj, ifNotNullFunc)
                .SetDescription($"{methodName}(\"{inputObj})\", {args}")
                .Returns(new FuncTestData2<IFuncInvocationResult<Uri>, Uri, bool>(true, ifNotNullResult, resultUri));
            inputObj = "file://myserver/dir/myfile.txt";
            resultUri = new Uri(inputObj, UriKind.Absolute);
            ifNotNullResult = new FuncInvocationResult<Uri>(resultUri);
            yield return new TestCaseData(inputObj, ifNotNullFunc)
                .SetDescription($"{methodName}(\"{inputObj})\", {args}")
                .Returns(new FuncTestData2<IFuncInvocationResult<Uri>, Uri, bool>(true, ifNotNullResult, resultUri));
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryCoerceValue4TestCases))]
        public FuncTestData2<IFuncInvocationResult<Uri>, Uri, bool> TryCoerceValue4Test(string inputObj, Func<string, Uri> ifNotNull)
        {
            FunctionInvocationMonitor<Uri> notNullMonitor = new FunctionInvocationMonitor<Uri>();
            return FuncTestData2<IFuncInvocationResult<Uri>, Uri, bool>.FromInvocation((out IFuncInvocationResult<Uri> notNullResult, out Uri uri) =>
            {
                bool result = inputObj.TryCoerceValue(notNullMonitor.CreateProxy(ifNotNull), out uri);
                notNullResult = notNullMonitor.ToResult();
                return result;
            });
        }

        public static IEnumerable<TestCaseData> GetTryCoerceValue5TestCases()
        {
            string args = typeof(Uri).Name;
            string methodName = $"<{typeof(string).Name}, {args}>";
            args = $"Func<{methodName}> ifNotNull, Func<{args}> ifNull, out {args} result";
            methodName = $"{typeof(PS.ExtensionMethods).FullName}.{nameof(PS.ExtensionMethods.TryCoerceValue)}<{methodName}>";
            Func<int, string> ifNotNullFunc = i => i.ToString("X4");
            string resultString = "(None)";
            Func<string> ifNullFunc = () => resultString;
            FuncInvocationResult<string> ifNotNullResult = new FuncInvocationResult<string>();
            FuncInvocationResult<string> ifNullResult = new FuncInvocationResult<string>(resultString);
            yield return new TestCaseData(null, ifNotNullFunc, ifNullFunc)
                .SetDescription($"{methodName}(null, {args})")
                .Returns(new FuncTestData3<IFuncInvocationResult<string>, IFuncInvocationResult<string>, string, bool>(false, ifNotNullResult, ifNullResult, resultString));
            int inputObj = 0;
            resultString = "0000";
            ifNotNullResult = new FuncInvocationResult<string>(resultString);
            ifNullResult = new FuncInvocationResult<string>();
            yield return new TestCaseData(inputObj, ifNotNullFunc, ifNullFunc)
                .SetDescription($"{methodName}(\"{inputObj})\", {args}")
                .Returns(new FuncTestData3<IFuncInvocationResult<string>, IFuncInvocationResult<string>, string, bool>(true, ifNotNullResult, ifNullResult, resultString));
            inputObj = int.MinValue;
            resultString = inputObj.ToString("X4");
            ifNotNullResult = new FuncInvocationResult<string>(resultString);
            yield return new TestCaseData(inputObj, ifNotNullFunc, ifNullFunc)
                .SetDescription($"{methodName}(\"{inputObj})\", {args}")
                .Returns(new FuncTestData3<IFuncInvocationResult<string>, IFuncInvocationResult<string>, string, bool>(true, ifNotNullResult, ifNullResult, resultString));
            inputObj = int.MaxValue;
            resultString = inputObj.ToString("X4");
            ifNotNullResult = new FuncInvocationResult<string>(resultString);
            yield return new TestCaseData(inputObj, ifNotNullFunc, ifNullFunc)
                .SetDescription($"{methodName}(\"{inputObj})\", {args}")
                .Returns(new FuncTestData3<IFuncInvocationResult<string>, IFuncInvocationResult<string>, string, bool>(true, ifNotNullResult, ifNullResult, resultString));
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryCoerceValue5TestCases))]
        public FuncTestData3<IFuncInvocationResult<string>, IFuncInvocationResult<string>, string, bool> TryCoerceValue5Test(int? inputObj, Func<int, string> ifNotNull, Func<string> ifNull)
        {
            FunctionInvocationMonitor<string> notNullMonitor = new FunctionInvocationMonitor<string>();
            FunctionInvocationMonitor<string> ifNullMonitor = new FunctionInvocationMonitor<string>();
            return FuncTestData3<IFuncInvocationResult<string>, IFuncInvocationResult<string>, string, bool>.FromInvocation((out IFuncInvocationResult<string> notNullResult,
                out IFuncInvocationResult<string> ifNullResult, out string value) =>
            {
                bool result = inputObj.TryCoerceValue(notNullMonitor.CreateProxy(ifNotNull), ifNullMonitor.CreateProxy(ifNull), out value);
                notNullResult = notNullMonitor.ToResult();
                ifNullResult = ifNullMonitor.ToResult();
                return result;
            });
        }

        public static IEnumerable<TestCaseData> GetTryCoerceValue6TestCases()
        {
            string args = typeof(Uri).Name;
            string methodName = $"<{typeof(string).Name}, {args}>";
            args = $"Func<{methodName}> ifNotNull, Func<{args}> ifNull, out {args} result";
            methodName = $"{typeof(PS.ExtensionMethods).FullName}.{nameof(PS.ExtensionMethods.TryCoerceValue)}<{methodName}>";
            Func<int, string> ifNotNullFunc = i => i.ToString("X4");
            FuncInvocationResult<string> ifNotNullResult = new FuncInvocationResult<string>();
            yield return new TestCaseData(null, ifNotNullFunc)
                .SetDescription($"{methodName}(null, {args})")
                .Returns(new FuncTestData2<IFuncInvocationResult<string>, string, bool>(false, ifNotNullResult, null));
            int inputObj = 0;
            string resultString = "0000";
            ifNotNullResult = new FuncInvocationResult<string>(resultString);
            yield return new TestCaseData(inputObj, ifNotNullFunc)
                .SetDescription($"{methodName}(\"{inputObj})\", {args}")
                .Returns(new FuncTestData2<IFuncInvocationResult<string>, string, bool>(true, ifNotNullResult, resultString));
            inputObj = int.MinValue;
            resultString = inputObj.ToString("X4");
            ifNotNullResult = new FuncInvocationResult<string>(resultString);
            yield return new TestCaseData(inputObj, ifNotNullFunc)
                .SetDescription($"{methodName}(\"{inputObj})\", {args}")
                .Returns(new FuncTestData2<IFuncInvocationResult<string>, string, bool>(true, ifNotNullResult, resultString));
            inputObj = int.MaxValue;
            resultString = inputObj.ToString("X4");
            ifNotNullResult = new FuncInvocationResult<string>(resultString);
            yield return new TestCaseData(inputObj, ifNotNullFunc)
                .SetDescription($"{methodName}(\"{inputObj})\", {args}")
                .Returns(new FuncTestData2<IFuncInvocationResult<string>, string, bool>(true, ifNotNullResult, resultString));
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryCoerceValue6TestCases))]
        public FuncTestData2<IFuncInvocationResult<string>, string, bool> TryCoerceValue6Test(int? inputObj, Func<int, string> ifNotNull)
        {
            FunctionInvocationMonitor<string> notNullMonitor = new FunctionInvocationMonitor<string>();
            return FuncTestData2<IFuncInvocationResult<string>, string, bool>.FromInvocation((out IFuncInvocationResult<string> notNullResult, out string value) =>
            {
                bool result = inputObj.TryCoerceValue(notNullMonitor.CreateProxy(ifNotNull), out value);
                notNullResult = notNullMonitor.ToResult();
                return result;
            });
        }

        /*
        public static IEnumerable<TestCaseData> GetTryCast1TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryCast1TestCases))]
        public FuncTestData3<IInvocationResult<int?>, IFuncInvocationResult<int?, bool>, int?, bool> TryCast1Test(object inputObj, Action<int?> ifSuccess, TryCoerceHandler<object, int?> fallback, bool asRawValueIfFail)
        {
            InvocationMonitor<int?> ifSuccessMonitor = new InvocationMonitor<int?>();
            FunctionInvocationMonitor<int?, bool> fallbackMonitor = new FunctionInvocationMonitor<int?, bool>();
            FuncWithOutput<object, int?, bool> funcWithOutput = fallbackMonitor.CreateProxy((object obj, out int? i) => fallback(inputObj, out i));
            return FuncTestData3<IInvocationResult<int?>, IFuncInvocationResult<int?, bool>, int?, bool>.FromInvocation((out IInvocationResult<int?> ifSuccessResult,
                out IFuncInvocationResult<int?, bool> ifNullResult, out int? value) =>
            {
                bool result = inputObj.TryCast(ifSuccessMonitor.CreateProxy(ifSuccess), (object obj, out int? i) => funcWithOutput(inputObj, out i), asRawValueIfFail, out value);
                ifSuccessResult = ifSuccessMonitor.ToResult();
                ifNullResult = fallbackMonitor.ToResult();
                return result;
            });
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
        */
    }
}
