using FsInfoCat.PS;
using FsInfoCat.Test.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace FsInfoCat.Test
{
    public partial class PSExtensionMethodsTest
    {
        public static IEnumerable<TestCaseData> GetInvokeIfNotNullTest1Cases()
        {
            string methodName = $"{typeof(PS.ExtensionMethods).FullName}.{nameof(PS.ExtensionMethods.InvokeIfNotNull)}<{typeof(Uri).Name}>";
            yield return new TestCaseData(null).SetArgDisplayNames("target").Returns(new InvocationResult<Uri>());
            UriKind kind = UriKind.Relative;
            Uri target = new Uri("", kind);
            yield return new TestCaseData(target).SetArgDisplayNames("target").Returns(new InvocationResult<Uri>(target));
            target = new Uri("/dir/myfile.txt", kind);
            yield return new TestCaseData(target).SetArgDisplayNames("target").Returns(new InvocationResult<Uri>(target));
            kind = UriKind.Absolute;
            target = new Uri("file://myserver/dir/myfile.txt", kind);
            yield return new TestCaseData(target).SetArgDisplayNames("target").Returns(new InvocationResult<Uri>(target));
        }

        private static IEnumerable<int> GetTestIntValues() => new int[] { 0, int.MinValue, int.MaxValue };

        public static IEnumerable<TestCaseData> GetInvokeIfNotNullTest2Cases()
        {
            string methodName = $"{typeof(PS.ExtensionMethods).FullName}.{nameof(PS.ExtensionMethods.InvokeIfNotNull)}<{typeof(int).Name}?>";
            yield return new TestCaseData(null).SetArgDisplayNames("target").Returns(new InvocationResult<int>());
            foreach (int target in GetTestIntValues())
                yield return new TestCaseData(target).SetArgDisplayNames("target").Returns(new InvocationResult<int>(target));
        }

        private static IEnumerable<Uri> GetTestUris() => new Uri[]
        {
            new Uri("", UriKind.Relative),
            new Uri("/dir/myfile.txt", UriKind.Relative),
            new Uri("file://myserver/dir/myfile.txt", UriKind.Absolute)
        };

        public static IEnumerable<TestCaseData> GetTryCoerceTo1TestCases()
        {
            Func<string, Uri> ifNotNullFunc = s => (Uri.TryCreate(s, UriKind.RelativeOrAbsolute, out Uri u)) ? u : null;
            Uri ifNullResultUri = new Uri(".", UriKind.Relative);
            Func<Uri> ifNullFunc = () => ifNullResultUri;
            yield return new TestCaseData(null, ifNotNullFunc, ifNullFunc)
                    .SetArgDisplayNames("inputObj", "ifNotNull", "ifNull")
                .Returns(new FuncTestData3<IFuncInvocationResult<Uri>, IFuncInvocationResult<Uri>, Uri, bool>(false, new FuncInvocationResult<Uri>(),
                    new FuncInvocationResult<Uri>(ifNullResultUri), ifNullResultUri));
            foreach (Uri uri in GetTestUris())
                yield return new TestCaseData(uri.OriginalString, ifNotNullFunc, ifNullFunc)
                    .SetArgDisplayNames("inputObj", "ifNotNull", "ifNull")
                    .Returns(new FuncTestData3<IFuncInvocationResult<Uri>, IFuncInvocationResult<Uri>, Uri, bool>(true, new FuncInvocationResult<Uri>(uri),
                        new FuncInvocationResult<Uri>(), uri));
        }

        public static IEnumerable<TestCaseData> GetTryCoerceTo2TestCases()
        {
            Func<string, Uri> ifNotNullFunc = s => (Uri.TryCreate(s, UriKind.RelativeOrAbsolute, out Uri u)) ? u : null;
            yield return new TestCaseData(null, ifNotNullFunc)
                .SetArgDisplayNames("inputObj", "ifNotNull")
                .Returns(new FuncTestData2<IFuncInvocationResult<Uri>, Uri, bool>(false, new FuncInvocationResult<Uri>(), null));
            foreach (Uri uri in GetTestUris())
            {
                yield return new TestCaseData(uri.OriginalString, ifNotNullFunc)
                    .SetArgDisplayNames("inputObj", "ifNotNull")
                    .Returns(new FuncTestData2<IFuncInvocationResult<Uri>, Uri, bool>(true, new FuncInvocationResult<Uri>(uri), uri));
            }
        }

        public static IEnumerable<TestCaseData> GetTryCoerceTo3TestCases()
        {
            Func<int, string> ifNotNullFunc = i => i.ToString("X4");
            string ifNullString = "(none)";
            Func<string> ifNullFunc = () => ifNullString;
            yield return new TestCaseData(null, ifNotNullFunc, ifNullFunc)
                    .SetArgDisplayNames("inputObj", "ifNotNull", "ifNull")
                .Returns(new FuncTestData3<IFuncInvocationResult<string>, IFuncInvocationResult<string>, string, bool>(false, new FuncInvocationResult<string>(),
                    new FuncInvocationResult<string>(ifNullString), ifNullString));
            foreach (int i in GetTestIntValues())
                yield return new TestCaseData(i, ifNotNullFunc, ifNullFunc)
                    .SetArgDisplayNames("inputObj", "ifNotNull", "ifNull")
                    .Returns(new FuncTestData3<IFuncInvocationResult<string>, IFuncInvocationResult<string>, string, bool>(true, new FuncInvocationResult<string>(ifNotNullFunc(i)),
                        new FuncInvocationResult<string>(), ifNotNullFunc(i)));
        }

        public static IEnumerable<TestCaseData> GetTryCoerceTo4TestCases()
        {
            Func<int, string> ifNotNullFunc = i => i.ToString("X4");
            yield return new TestCaseData(null, ifNotNullFunc)
                    .SetArgDisplayNames("inputObj", "ifNotNull")
                .Returns(new FuncTestData2<IFuncInvocationResult<string>, string, bool>(false, new FuncInvocationResult<string>(), null));
            foreach (int i in GetTestIntValues())
                yield return new TestCaseData(i, ifNotNullFunc)
                    .SetArgDisplayNames("inputObj", "ifNotNull")
                    .Returns(new FuncTestData2<IFuncInvocationResult<string>, string, bool>(true, new FuncInvocationResult<string>(ifNotNullFunc(i)), ifNotNullFunc(i)));
        }

        /// <summary>
        /// Test data for <see cref="TryCoerceAsUri1Test(object, Func{Uri}, bool, TryCoerceHandler{object, Uri})"/>, targeting
        /// <seealso cref="ExtensionMethods.TryCoerceAs{TResult}(object, Func{TResult}, bool, TryCoerceHandler{object, TResult}, Action{TResult}, out TResult)"/> for a reference type.
        /// </summary>
        /// <remarks>Parameters in target method:
        /// <list type="number">
        ///     <item><term>this object inputObj</term> The value to be coerced.</item>
        ///     <item><term><seealso cref="Func{TResult}">Func</seealso><c>&lt;<seealso cref="Uri"/>&gt;</c> ifNull</term> </item>
        ///     <item><term>bool returnValueIfNull</term> </item>
        ///     <item><term><seealso cref="TryCoerceHandler{object, TResult}">TryCoerceHandler</seealso><c>&lt;object, out <seealso cref="Uri"/>&gt;</c> fallback</term> </item>
        ///     <item><term><seealso cref="Action{TResult}">Action</seealso><c>&lt;<seealso cref="Uri"/>&gt;</c> ifSuccess</term> </item>
        ///     <item><term><seealso cref="Uri"/> out result</term> </item>
        /// </list></remarks>
        public static IEnumerable<TestCaseData> GetTryCoerceAsUri1TestCases() => CoerceAsValueTestData.GetCoerceAsUriTestDataFNR().SelectMany(testData => (testData.InputObj is null) ? new CoerceAsValueTestDataFNR<Uri>[] { testData } : new CoerceAsValueTestDataFNR<Uri>[]
        {
            testData,
            new CoerceAsValueTestDataFNR<Uri>(PSObject.AsPSObject(testData.InputObj), testData.ReturnValue, testData.Result, testData.FallbackHandler, testData.FallbackResult,
                testData.IfNullCallback, testData.ReturnValueIfNull)
        }).Select(testData =>
            new TestCaseData(testData.InputObj, testData.IfNullCallback, testData.ReturnValueIfNull, testData.FallbackHandler)
                .SetArgDisplayNames("inputObj", "ifNull", "returnValueIfNull", "fallback")
                .Returns(new FuncTestData4<IFuncInvocationResult<Uri>, IFuncInvocationResult<Uri, bool>, IInvocationResult<Uri>, Uri, bool>(testData.ReturnValue, testData.IfNullResult,
                    testData.FallbackResult, testData.IfSuccessResult, testData.Result))
        );

        /// <summary>
        /// Test data for <see cref="TryCoerceAsInt1Test(object, Func{int}, bool, TryCoerceHandler{object, int})"/>, targeting
        /// <seealso cref="ExtensionMethods.TryCoerceAs{TResult}(object, Func{TResult}, bool, TryCoerceHandler{object, TResult}, Action{TResult}, out TResult)"/> with a value type.
        /// </summary>
        /// <remarks>Parameters in target method:
        /// <list type="number">
        ///     <item><term>this object inputObj</term> The value to be coerced.</item>
        ///     <item><term><seealso cref="Func{TResult}">Func</seealso><c>&lt;int&gt;</c> ifNull</term> </item>
        ///     <item><term>bool returnValueIfNull</term> </item>
        ///     <item><term><seealso cref="TryCoerceHandler{object, TResult}">TryCoerceHandler</seealso><c>&lt;object, out int&gt;</c> fallback</term> </item>
        ///     <item><term><seealso cref="Action{TResult}">Action</seealso><c>&lt;int&gt;</c> ifSuccess</term> </item>
        ///     <item><term><c>int</c> out result</term> </item>
        /// </list></remarks>
        public static IEnumerable<TestCaseData> GetTryCoerceAsInt1TestCases() => CoerceAsValueTestData.GetCoerceAsIntTestDataFNR().SelectMany(testData => new CoerceAsValueTestDataFNR<int>[]
        {
            testData,
            new CoerceAsValueTestDataFNR<int>(PSObject.AsPSObject(testData.InputObj), testData.ReturnValue, testData.Result, testData.FallbackHandler, testData.FallbackResult,
                testData.IfNullCallback, testData.ReturnValueIfNull)
        }).Select(testData =>
            new TestCaseData(testData.InputObj, testData.IfNullCallback, testData.ReturnValueIfNull, testData.FallbackHandler)
                .SetArgDisplayNames("inputObj")
                .Returns(new FuncTestData4<IFuncInvocationResult<int>, IFuncInvocationResult<int, bool>, IInvocationResult<int>, int, bool>(testData.ReturnValue,
                    testData.IfNullResult, testData.FallbackResult, testData.IfSuccessResult, testData.Result))
        );

        /// <summary>
        /// Test data for <see cref="TryCoerceAsUri2Test(object, Func{Uri}, bool)"/>, targeting
        /// <seealso cref="ExtensionMethods.TryCoerceAs{TResult}(object, Func{TResult}, bool, Action{TResult}, out TResult)"/> for a reference type.
        /// </summary>
        /// <remarks>Parameters in target method:
        /// <list type="number">
        ///     <item><term>this object inputObj</term> The value to be coerced.</item>
        ///     <item><term><seealso cref="Func{TResult}">Func</seealso><c>&lt;<seealso cref="Uri"/>&gt;</c> ifNull</term> </item>
        ///     <item><term>bool returnValueIfNull</term> </item>
        ///     <item><term><seealso cref="Action{TResult}">Action</seealso><c>&lt;<seealso cref="Uri"/>&gt;</c> ifSuccess</term> </item>
        ///     <item><term><seealso cref="Uri"/> out result</term> </item>
        /// </list></remarks>
        public static IEnumerable<TestCaseData> GetTryCoerceAsUri2TestCases() => CoerceAsValueTestData.GetCoerceAsUriTestDataNR().SelectMany(testData => new CoerceAsValueTestDataNR<Uri>[]
        {
            testData,
            new CoerceAsValueTestDataNR<Uri>(PSObject.AsPSObject(testData.InputObj), testData.ReturnValue, testData.Result, testData.IfNullCallback, testData.ReturnValueIfNull)
        }).Select(testData =>
            new TestCaseData(testData.InputObj, testData.IfNullCallback, testData.ReturnValueIfNull)
                .SetArgDisplayNames("inputObj")
                .Returns(new FuncTestData3<IFuncInvocationResult<Uri>, IInvocationResult<Uri>, Uri, bool>(testData.ReturnValue, testData.IfNullResult,
                    testData.IfSuccessResult, testData.Result))
        );

        /// <summary>
        /// Test data for <see cref="TryCoerceAsInt2Test(object, Func{int}, bool)"/>, targeting
        /// <seealso cref="ExtensionMethods.TryCoerceAs{TResult}(object, Func{TResult}, bool, Action{TResult}, out TResult)"/> with a value type.
        /// </summary>
        /// <remarks>Parameters in target method:
        /// <list type="number">
        ///     <item><term>this object inputObj</term> The value to be coerced.</item>
        ///     <item><term><seealso cref="Func{TResult}">Func</seealso><c>&lt;int&gt;</c> ifNull</term> </item>
        ///     <item><term>bool returnValueIfNull</term> </item>
        ///     <item><term><seealso cref="Action{TResult}">Action</seealso><c>&lt;int&gt;</c> ifSuccess</term> </item>
        ///     <item><term><c>int</c> out result</term> </item>
        /// </list></remarks>
        public static IEnumerable<TestCaseData> GetTryCoerceAsInt2TestCases() => CoerceAsValueTestData.GetCoerceAsIntTestDataNR().SelectMany(testData => new CoerceAsValueTestDataNR<int>[]
        {
            testData,
            new CoerceAsValueTestDataNR<int>(PSObject.AsPSObject(testData.InputObj), testData.ReturnValue, testData.Result, testData.IfNullCallback, testData.ReturnValueIfNull)
        }).Select(testData =>
            new TestCaseData(testData.InputObj, testData.IfNullCallback, testData.ReturnValueIfNull)
                .SetArgDisplayNames("inputObj")
                .Returns(new FuncTestData3<IFuncInvocationResult<int>, IInvocationResult<int>, int, bool>(testData.ReturnValue, testData.IfNullResult,
                    testData.IfSuccessResult, testData.Result))
        );

        /// <summary>
        /// Test data for <see cref="TryCoerceAsUri3Test(object, Func{Uri}, TryCoerceHandler{object, Uri})"/>, targeting
        /// <seealso cref="ExtensionMethods.TryCoerceAs{TResult}(object, Func{TResult}, TryCoerceHandler{object, TResult}, Action{TResult}, out TResult)"/> for a reference type.
        /// </summary>
        /// <remarks>Parameters in target method:
        /// <list type="number">
        ///     <item><term>this object inputObj</term> The value to be coerced.</item>
        ///     <item><term><seealso cref="Func{TResult}">Func</seealso><c>&lt;<seealso cref="Uri"/>&gt;</c> ifNull</term> </item>
        ///     <item><term><seealso cref="TryCoerceHandler{object, TResult}">TryCoerceHandler</seealso><c>&lt;object, out <seealso cref="Uri"/>&gt;</c> fallback</term> </item>
        ///     <item><term><seealso cref="Action{TResult}">Action</seealso><c>&lt;<seealso cref="Uri"/>&gt;</c> ifSuccess</term> </item>
        ///     <item><term><seealso cref="Uri"/> out result</term> </item>
        /// </list></remarks>
        public static IEnumerable<TestCaseData> GetTryCoerceAsUri3TestCases() => CoerceAsValueTestData.GetCoerceAsUriTestDataFN().SelectMany(testData => new CoerceAsValueTestDataFN<Uri>[]
        {
            testData,
            new CoerceAsValueTestDataFN<Uri>(PSObject.AsPSObject(testData.InputObj), testData.ReturnValue, testData.Result, testData.FallbackHandler, testData.FallbackResult,
                testData.IfNullCallback)
        }).Select(testData =>
            new TestCaseData(testData.InputObj, testData.IfNullCallback, testData.FallbackHandler)
                .SetArgDisplayNames("inputObj")
                .Returns(new FuncTestData4<IFuncInvocationResult<Uri>, IFuncInvocationResult<Uri, bool>, IInvocationResult<Uri>, Uri, bool>(testData.ReturnValue, testData.IfNullResult,
                    testData.FallbackResult, testData.IfSuccessResult, testData.Result))
        );

        /// <summary>
        /// Test data for <see cref="TryCoerceAsInt3Test(object, Func{int}, TryCoerceHandler{object, int})"/>, targeting
        /// <seealso cref="ExtensionMethods.TryCoerceAs{TResult}(object, Func{TResult}, TryCoerceHandler{object, TResult}, Action{TResult}, out TResult)"/> with a value type.
        /// </summary>
        /// <remarks>Parameters in target method:
        /// <list type="number">
        ///     <item><term>this object inputObj</term> The value to be coerced.</item>
        ///     <item><term><seealso cref="Func{TResult}">Func</seealso><c>&lt;int&gt;</c> ifNull</term> </item>
        ///     <item><term><seealso cref="TryCoerceHandler{object, TResult}">TryCoerceHandler</seealso><c>&lt;object, out int&gt;</c> fallback</term> </item>
        ///     <item><term><seealso cref="Action{TResult}">Action</seealso><c>&lt;int&gt;</c> ifSuccess</term> </item>
        ///     <item><term><c>int</c> out result</term> </item>
        /// </list></remarks>
        public static IEnumerable<TestCaseData> GetTryCoerceAsInt3TestCases() => CoerceAsValueTestData.GetCoerceAsIntTestDataFN().SelectMany(testData => new CoerceAsValueTestDataFN<int>[]
        {
            testData,
            new CoerceAsValueTestDataFN<int>(PSObject.AsPSObject(testData.InputObj), testData.ReturnValue, testData.Result, testData.FallbackHandler, testData.FallbackResult, testData.IfNullCallback)
        }).Select(testData =>
            new TestCaseData(testData.InputObj, testData.IfNullCallback, testData.FallbackHandler)
                .SetArgDisplayNames("inputObj")
                .Returns(new FuncTestData4<IFuncInvocationResult<int>, IFuncInvocationResult<int, bool>, IInvocationResult<int>, int, bool>(testData.ReturnValue, testData.IfNullResult,
                    testData.FallbackResult, testData.IfSuccessResult, testData.Result))
        );

        /// <summary>
        /// Test data for <see cref="TryCoerceAsUri4Test(object, Func{Uri})"/>, targeting
        /// <seealso cref="ExtensionMethods.TryCoerceAs{TResult}(object, Func{TResult}, Action{TResult}, out TResult)"/> for a reference type.
        /// </summary>
        /// <remarks>Parameters in target method:
        /// <list type="number">
        ///     <item><term>this object inputObj</term> The value to be coerced.</item>
        ///     <item><term><seealso cref="Func{TResult}">Func</seealso><c>&lt;<seealso cref="Uri"/>&gt;</c> ifNull</term> </item>
        ///     <item><term><seealso cref="Action{TResult}">Action</seealso><c>&lt;<seealso cref="Uri"/>&gt;</c> ifSuccess</term> </item>
        ///     <item><term><seealso cref="Uri"/> out result</term> </item>
        /// </list></remarks>
        public static IEnumerable<TestCaseData> GetTryCoerceAsUri4TestCases() => CoerceAsValueTestData.GetCoerceAsUriTestDataN().SelectMany(testData => new CoerceAsValueTestDataN<Uri>[]
        {
            testData,
            new CoerceAsValueTestDataN<Uri>(PSObject.AsPSObject(testData.InputObj), testData.ReturnValue, testData.Result, testData.IfNullCallback)
        }).Select(testData =>
            new TestCaseData(testData.InputObj, testData.IfNullCallback)
                .SetArgDisplayNames("inputObj")
                .Returns(new FuncTestData3<IFuncInvocationResult<Uri>, IInvocationResult<Uri>, Uri, bool>(testData.ReturnValue, testData.IfNullResult, testData.IfSuccessResult, testData.Result))
        );

        /// <summary>
        /// Test data for <see cref="TryCoerceAsInt4Test(object, Func{int})"/>, targeting
        /// <seealso cref="ExtensionMethods.TryCoerceAs{TResult}(object, Func{TResult}, Action{TResult}, out TResult)"/> with a value type.
        /// </summary>
        /// <remarks>Parameters in target method:
        /// <list type="number">
        ///     <item><term>this object inputObj</term> The value to be coerced.</item>
        ///     <item><term><seealso cref="Func{TResult}">Func</seealso><c>&lt;int&gt;</c> ifNull</term> </item>
        ///     <item><term><seealso cref="Action{TResult}">Action</seealso><c>&lt;int&gt;</c> ifSuccess</term> </item>
        ///     <item><term><c>int</c> out result</term> </item>
        /// </list></remarks>
        public static IEnumerable<TestCaseData> GetTryCoerceAsInt4TestCases() => CoerceAsValueTestData.GetCoerceAsIntTestDataN().SelectMany(testData => new CoerceAsValueTestDataN<int>[]
        {
            testData,
            new CoerceAsValueTestDataN<int>(PSObject.AsPSObject(testData.InputObj), testData.ReturnValue, testData.Result, testData.IfNullCallback)
        }).Select(testData =>
            new TestCaseData(testData.InputObj, testData.IfNullCallback)
                .SetArgDisplayNames("inputObj")
                .Returns(new FuncTestData3<IFuncInvocationResult<int>, IInvocationResult<int>, int, bool>(testData.ReturnValue, testData.IfNullResult, testData.IfSuccessResult, testData.Result))
        );

        /// <summary>
        /// Test data for <see cref="TryCoerceAsInt5Test(object, bool, TryCoerceHandler{object, int})"/>, targeting
        /// <seealso cref="ExtensionMethods.TryCoerceAs{TResult}(object, bool, TryCoerceHandler{object, TResult}, Action{TResult}, out TResult)"/> with a value type.
        /// </summary>
        /// <remarks>Parameters in target method:
        /// <list type="number">
        ///     <item><term>this object inputObj</term> The value to be coerced.</item>
        ///     <item><term>bool returnValueIfNull</term> </item>
        ///     <item><term><seealso cref="TryCoerceHandler{object, TResult}">TryCoerceHandler</seealso><c>&lt;object, out int&gt;</c> fallback</term> </item>
        ///     <item><term><seealso cref="Action{TResult}">Action</seealso><c>&lt;int&gt;</c> ifSuccess</term> </item>
        ///     <item><term><c>int</c> out result</term> </item>
        /// </list></remarks>
        public static IEnumerable<TestCaseData> GetTryCoerceAsInt5TestCases() => CoerceAsValueTestData.GetCoerceAsIntTestDataFR().SelectMany(testData => new CoerceAsValueTestDataFR<int>[]
        {
            testData,
            new CoerceAsValueTestDataFR<int>(PSObject.AsPSObject(testData.InputObj), testData.ReturnValue, testData.Result, testData.FallbackHandler, testData.FallbackResult, testData.ReturnValueIfNull)
        }).Select(testData =>
            new TestCaseData(testData.InputObj, testData.ReturnValueIfNull, testData.FallbackHandler)
                .SetArgDisplayNames("inputObj")
                .Returns(new FuncTestData3<IFuncInvocationResult<int, bool>, IInvocationResult<int>, int, bool>(testData.ReturnValue, testData.FallbackResult, testData.IfSuccessResult,
                    testData.Result))
        );

        /// <summary>
        /// Test data for <see cref="TryCoerceAsUri5Test(object, bool, TryCoerceHandler{object, Uri})"/>, targeting
        /// <seealso cref="ExtensionMethods.TryCoerceAs{TResult}(object, bool, TryCoerceHandler{object, TResult}, Action{TResult}, out TResult)"/> for a reference type.
        /// </summary>
        /// <remarks>Parameters in target method:
        /// <list type="number">
        ///     <item><term>this object inputObj</term> The value to be coerced.</item>
        ///     <item><term>bool returnValueIfNull</term> </item>
        ///     <item><term><seealso cref="TryCoerceHandler{object, TResult}">TryCoerceHandler</seealso><c>&lt;object, out <seealso cref="Uri"/>&gt;</c> fallback</term> </item>
        ///     <item><term><seealso cref="Action{TResult}">Action</seealso><c>&lt;<seealso cref="Uri"/>&gt;</c> ifSuccess</term> </item>
        ///     <item><term><seealso cref="Uri"/> out result</term> </item>
        /// </list></remarks>
        public static IEnumerable<TestCaseData> GetTryCoerceAsUri5TestCases() => CoerceAsValueTestData.GetCoerceAsUriTestDataFR().SelectMany(testData => new CoerceAsValueTestDataFR<Uri>[]
        {
            testData,
            new CoerceAsValueTestDataFR<Uri>(PSObject.AsPSObject(testData.InputObj), testData.ReturnValue, testData.Result, testData.FallbackHandler, testData.FallbackResult,
                testData.ReturnValueIfNull)
        }).Select(testData =>
            new TestCaseData(testData.InputObj, testData.ReturnValueIfNull, testData.FallbackHandler)
                .SetArgDisplayNames("inputObj")
                .Returns(new FuncTestData3<IFuncInvocationResult<Uri, bool>, IInvocationResult<Uri>, Uri, bool>(testData.ReturnValue, testData.FallbackResult, testData.IfSuccessResult,
                    testData.Result))
        );

        /// <summary>
        /// Test data for <see cref="TryCoerceAsUri6Test(object, bool)"/>, targeting
        /// <seealso cref="ExtensionMethods.TryCoerceAs{TResult}(object, bool, Action{TResult}, out TResult)"/> for a reference type.
        /// </summary>
        /// <remarks>Parameters in target method:
        /// <list type="number">
        ///     <item><term>this object inputObj</term> The value to be coerced.</item>
        ///     <item><term>bool returnValueIfNull</term> </item>
        ///     <item><term><seealso cref="Action{TResult}">Action</seealso><c>&lt;int&gt;</c> ifSuccess</term> </item>
        ///     <item><term><c>int</c> out result</term> </item>
        /// </list></remarks>
        public static IEnumerable<TestCaseData> GetTryCoerceAsUri6TestCases() => CoerceAsValueTestData.GetCoerceAsIntTestDataR().SelectMany(testData => new CoerceAsValueTestDataR<int>[]
        {
            testData,
            new CoerceAsValueTestDataR<int>(PSObject.AsPSObject(testData.InputObj), testData.ReturnValue, testData.Result, testData.ReturnValueIfNull)
        }).Select(testData =>
             new TestCaseData(testData.InputObj, testData.ReturnValueIfNull)
                 .SetArgDisplayNames("inputObj")
                 .Returns(new FuncTestData2<IInvocationResult<int>, int, bool>(testData.ReturnValue, testData.IfSuccessResult, testData.Result))
        );

        /// <summary>
        /// Test data for <see cref="TryCoerceAsInt6Test(object, bool)"/>, targeting
        /// <seealso cref="ExtensionMethods.TryCoerceAs{TResult}(object, bool, Action{TResult}, out TResult)"/> with a value type.
        /// </summary>
        /// <remarks>Parameters in target method:
        /// <list type="number">
        ///     <item><term>this object inputObj</term> The value to be coerced.</item>
        ///     <item><term>bool returnValueIfNull</term> </item>
        ///     <item><term><seealso cref="Action{TResult}">Action</seealso><c>&lt;<seealso cref="Uri"/>&gt;</c> ifSuccess</term> </item>
        ///     <item><term><seealso cref="Uri"/> out result</term> </item>
        /// </list></remarks>
        public static IEnumerable<TestCaseData> GetTryCoerceAsInt6TestCases() => CoerceAsValueTestData.GetCoerceAsUriTestData().SelectMany(testData => new CoerceAsValueTestData<Uri>[]
        {
            testData,
            new CoerceAsValueTestData<Uri>(PSObject.AsPSObject(testData.InputObj), testData.ReturnValue, testData.Result)
        }).Select(testData =>
            new TestCaseData(testData.InputObj)
                .SetArgDisplayNames("inputObj")
                .Returns(new FuncTestData2<IInvocationResult<Uri>, Uri, bool>(testData.ReturnValue, testData.IfSuccessResult, testData.Result))
        );

        /// <summary>
        /// Test data for <see cref="TryCoerceAsUri7Test(object, TryCoerceHandler{object, Uri})"/>, targeting
        /// <seealso cref="ExtensionMethods.TryCoerceAs{TResult}(object, TryCoerceHandler{object, TResult}, Action{TResult}, out TResult)"/> for a reference type.
        /// </summary>
        /// <remarks>Parameters in target method:
        /// <list type="number">
        ///     <item><term>this object inputObj</term> The value to be coerced.</item>
        ///     <item><term><seealso cref="TryCoerceHandler{object, TResult}">TryCoerceHandler</seealso><c>&lt;object, out <seealso cref="Uri"/>&gt;</c> fallback</term> </item>
        ///     <item><term><seealso cref="Action{TResult}">Action</seealso><c>&lt;<seealso cref="Uri"/>&gt;</c> ifSuccess</term> </item>
        ///     <item><term><seealso cref="Uri"/> out result</term> </item>
        /// </list></remarks>
        public static IEnumerable<TestCaseData> GetTryCoerceAsUri7TestCases() => CoerceAsValueTestData.GetCoerceAsUriTestDataF().SelectMany(testData => new CoerceAsValueTestDataF<Uri>[]
        {
            testData,
            new CoerceAsValueTestDataF<Uri>(PSObject.AsPSObject(testData.InputObj), testData.ReturnValue, testData.Result, testData.FallbackHandler, testData.FallbackResult)
        }).Select(testData =>
            new TestCaseData(testData.InputObj, testData.FallbackHandler)
                .SetArgDisplayNames("inputObj")
                .Returns(new FuncTestData3<IFuncInvocationResult<Uri, bool>, IInvocationResult<Uri>, Uri, bool>(testData.ReturnValue, testData.FallbackResult, testData.IfSuccessResult,
                    testData.Result))
        );

        /// <summary>
        /// Test data for <see cref="TryCoerceAsInt7Test(object, TryCoerceHandler{object, int})"/>, targeting
        /// <seealso cref="ExtensionMethods.TryCoerceAs{TResult}(object, TryCoerceHandler{object, TResult}, Action{TResult}, out TResult)"/> with a value type.
        /// </summary>
        /// <remarks>Parameters in target method:
        /// <list type="number">
        ///     <item><term>this object inputObj</term> The value to be coerced.</item>
        ///     <item><term><seealso cref="TryCoerceHandler{object, TResult}">TryCoerceHandler</seealso><c>&lt;object, out int&gt;</c> fallback</term> </item>
        ///     <item><term><seealso cref="Action{TResult}">Action</seealso><c>&lt;int&gt;</c> ifSuccess</term> </item>
        ///     <item><term><c>int</c> out result</term> </item>
        /// </list></remarks>
        public static IEnumerable<TestCaseData> GetTryCoerceAsInt7TestCases() => CoerceAsValueTestData.GetCoerceAsIntTestDataF().SelectMany(testData => new CoerceAsValueTestDataF<int>[]
        {
            testData,
            new CoerceAsValueTestDataF<int>(PSObject.AsPSObject(testData.InputObj), testData.ReturnValue, testData.Result, testData.FallbackHandler, testData.FallbackResult)
        }).Select(testData =>
            new TestCaseData(testData.InputObj, testData.FallbackHandler)
                .SetArgDisplayNames("inputObj")
                .Returns(new FuncTestData3<IFuncInvocationResult<int, bool>, IInvocationResult<int>, int, bool>(testData.ReturnValue, testData.FallbackResult, testData.IfSuccessResult,
                    testData.Result))
        );

        /// <summary>
        /// Test data for <see cref="TryCoerceAsUri8Test(object)"/>, targeting
        /// <seealso cref="ExtensionMethods.TryCoerceAs{TResult}(object, Action{TResult}, out TResult)"/> for a reference type.
        /// </summary>
        /// <remarks>Parameters in target method:
        /// <list type="number">
        ///     <item><term>this object inputObj</term> The value to be coerced.</item>
        ///     <item><term><seealso cref="Action{TResult}">Action</seealso><c>&lt;<seealso cref="Uri"/>&gt;</c> ifSuccess</term> </item>
        ///     <item><term><seealso cref="Uri"/> out result</term> </item>
        /// </list></remarks>
        public static IEnumerable<TestCaseData> GetTryCoerceAsUri8TestCases() => CoerceAsValueTestData.GetCoerceAsUriTestData().SelectMany(testData => new CoerceAsValueTestData<Uri>[]
        {
            testData,
            new CoerceAsValueTestData<Uri>(PSObject.AsPSObject(testData.InputObj), testData.ReturnValue, testData.Result)
        }).Select(testData =>
            new TestCaseData(testData.InputObj)
                .SetArgDisplayNames("inputObj")
                .Returns(new FuncTestData2<IInvocationResult<Uri>, Uri, bool>(testData.ReturnValue, testData.IfSuccessResult, testData.Result))
        );

        /// <summary>
        /// Test data for <see cref="TryCoerceAsInt8Test(object)"/>, targeting
        /// <seealso cref="ExtensionMethods.TryCoerceAs{TResult}(object, Action{TResult}, out TResult)"/> with a value type.
        /// </summary>
        /// <remarks>Parameters in target method:
        /// <list type="number">
        ///     <item><term>this object inputObj</term> The value to be coerced.</item>
        ///     <item><term><seealso cref="Action{TResult}">Action</seealso><c>&lt;int&gt;</c> ifSuccess</term> </item>
        ///     <item><term><c>int</c> out result</term> </item>
        /// </list></remarks>
        public static IEnumerable<TestCaseData> GetTryCoerceAsInt8TestCases() => CoerceAsValueTestData.GetCoerceAsIntTestData().SelectMany(testData => new CoerceAsValueTestData<int>[]
        {
            testData,
            new CoerceAsValueTestData<int>(PSObject.AsPSObject(testData.InputObj), testData.ReturnValue, testData.Result)
        }).Select(testData =>
            new TestCaseData(testData.InputObj)
                .SetArgDisplayNames("inputObj")
                .Returns(new FuncTestData2<IInvocationResult<int>, int, bool>(testData.ReturnValue, testData.IfSuccessResult, testData.Result))
        );

        /// <summary>
        /// Test data for <see cref="TryCoerceAsUri9Test(object, Func{Uri}, bool, TryCoerceHandler{object, Uri})"/>, targeting
        /// <seealso cref="ExtensionMethods.TryCoerceAs{TResult}(object, Func{TResult}, bool, TryCoerceHandler{object, TResult}, out TResult)"/> for a reference type.
        /// </summary>
        /// <remarks>Parameters in target method:
        /// <list type="number">
        ///     <item><term>this object inputObj</term> The value to be coerced.</item>
        ///     <item><term><seealso cref="Func{TResult}">Func</seealso><c>&lt;<seealso cref="Uri"/>&gt;</c> ifNull</term> </item>
        ///     <item><term>bool returnValueIfNull</term> </item>
        ///     <item><term><seealso cref="TryCoerceHandler{object, TResult}">TryCoerceHandler</seealso><c>&lt;object, out <seealso cref="Uri"/>&gt;</c> fallback</term> </item>
        ///     <item><term><seealso cref="Uri"/> out result</term> </item>
        /// </list></remarks>
        public static IEnumerable<TestCaseData> GetTryCoerceAsUri9TestCases() => CoerceAsValueTestData.GetCoerceAsUriTestDataFNR().SelectMany(testData => new CoerceAsValueTestDataFNR<Uri>[]
        {
            testData,
            new CoerceAsValueTestDataFNR<Uri>(PSObject.AsPSObject(testData.InputObj), testData.ReturnValue, testData.Result, testData.FallbackHandler, testData.FallbackResult,
                testData.IfNullCallback, testData.ReturnValueIfNull)
        }).Select(testData =>
            new TestCaseData(testData.InputObj, testData.IfNullCallback, testData.ReturnValueIfNull, testData.FallbackHandler)
                .SetArgDisplayNames("inputObj")
                .Returns(new FuncTestData4<IFuncInvocationResult<Uri>, IFuncInvocationResult<Uri, bool>, IInvocationResult<Uri>, Uri, bool>(testData.ReturnValue,
                    testData.IfNullResult, testData.FallbackResult, testData.IfSuccessResult, testData.Result))
        );

        /// <summary>
        /// Test data for <see cref="TryCoerceAsInt9Test(object, Func{int}, bool, TryCoerceHandler{object, int})"/>, targeting
        /// <seealso cref="ExtensionMethods.TryCoerceAs{TResult}(object, Func{TResult}, bool, TryCoerceHandler{object, TResult}, out TResult)"/> with a value type.
        /// </summary>
        /// <remarks>Parameters in target method:
        /// <list type="number">
        ///     <item><term>this object inputObj</term> The value to be coerced.</item>
        ///     <item><term><seealso cref="Func{TResult}">Func</seealso><c>&lt;int&gt;</c> ifNull</term> </item>
        ///     <item><term>bool returnValueIfNull</term> </item>
        ///     <item><term><seealso cref="TryCoerceHandler{object, TResult}">TryCoerceHandler</seealso><c>&lt;object, out int&gt;</c> fallback</term> </item>
        ///     <item><term><c>int</c> out result</term> </item>
        /// </list></remarks>
        public static IEnumerable<TestCaseData> GetTryCoerceAsInt9TestCases() => CoerceAsValueTestData.GetCoerceAsIntTestDataFNR().SelectMany(testData => new CoerceAsValueTestDataFNR<int>[]
        {
            testData,
            new CoerceAsValueTestDataFNR<int>(PSObject.AsPSObject(testData.InputObj), testData.ReturnValue, testData.Result, testData.FallbackHandler, testData.FallbackResult,
                testData.IfNullCallback, testData.ReturnValueIfNull)
        }).Select(testData =>
            new TestCaseData(testData.InputObj, testData.IfNullCallback, testData.ReturnValueIfNull, testData.FallbackHandler)
                .SetArgDisplayNames("inputObj")
                .Returns(new FuncTestData3<IFuncInvocationResult<int>, IFuncInvocationResult<int, bool>, int, bool>(testData.ReturnValue, testData.IfNullResult, testData.FallbackResult,
                    testData.Result))
        );

        /// <summary>
        /// Test data for <see cref="TryCoerceAsUri10Test(object, Func{Uri}, bool)"/>, targeting
        /// <seealso cref="ExtensionMethods.TryCoerceAs{TResult}(object, Func{TResult}, bool, out TResult)"/> for a reference type.
        /// </summary>
        /// <remarks>Parameters in target method:
        /// <list type="number">
        ///     <item><term>this object inputObj</term> The value to be coerced.</item>
        ///     <item><term><seealso cref="Func{TResult}">Func</seealso><c>&lt;<seealso cref="Uri"/>&gt;</c> ifNull</term> </item>
        ///     <item><term>bool returnValueIfNull</term> </item>
        ///     <item><term><seealso cref="Uri"/> out result</term> </item>
        /// </list></remarks>
        public static IEnumerable<TestCaseData> GetTryCoerceAsUri10TestCases() => CoerceAsValueTestData.GetCoerceAsUriTestDataNR().SelectMany(testData => new CoerceAsValueTestDataNR<Uri>[]
        {
            testData,
            new CoerceAsValueTestDataNR<Uri>(PSObject.AsPSObject(testData.InputObj), testData.ReturnValue, testData.Result, testData.IfNullCallback, testData.ReturnValueIfNull)
        }).Select(testData =>
            new TestCaseData(testData.InputObj, testData.IfNullCallback, testData.ReturnValueIfNull)
                .SetArgDisplayNames("inputObj")
                .Returns(new FuncTestData2<IFuncInvocationResult<Uri>, Uri, bool>(testData.ReturnValue, testData.IfNullResult, testData.Result))
        );

        /// <summary>
        /// Test data for <see cref="TryCoerceAsInt10Test(object, Func{int}, bool)"/>, targeting
        /// <seealso cref="ExtensionMethods.TryCoerceAs{TResult}(object, Func{TResult}, bool, out TResult)"/> with a value type.
        /// </summary>
        /// <remarks>Parameters in target method:
        /// <list type="number">
        ///     <item><term>this object inputObj</term> The value to be coerced.</item>
        ///     <item><term><seealso cref="Func{TResult}">Func</seealso><c>&lt;int&gt;</c> ifNull</term> </item>
        ///     <item><term>bool returnValueIfNull</term> </item>
        ///     <item><term><c>int</c> out result</term> </item>
        /// </list></remarks>
        public static IEnumerable<TestCaseData> GetTryCoerceAsInt10TestCases() => CoerceAsValueTestData.GetCoerceAsIntTestDataNR().SelectMany(testData => new CoerceAsValueTestDataNR<int>[]
        {
            testData,
            new CoerceAsValueTestDataNR<int>(PSObject.AsPSObject(testData.InputObj), testData.ReturnValue, testData.Result, testData.IfNullCallback, testData.ReturnValueIfNull)
        }).Select(testData =>
            new TestCaseData(testData.InputObj, testData.IfNullCallback, testData.ReturnValueIfNull)
                .SetArgDisplayNames("inputObj")
                .Returns(new FuncTestData2<IFuncInvocationResult<int>, int, bool>(testData.ReturnValue, testData.IfNullResult, testData.Result))
        );

        /// <summary>
        /// Test data for <see cref="TryCoerceAsUri11Test(object, Func{Uri}, TryCoerceHandler{object, Uri})"/>, targeting
        /// <seealso cref="ExtensionMethods.TryCoerceAs{TResult}(object, Func{TResult}, TryCoerceHandler{object, TResult}, out TResult)"/> for a reference type.
        /// </summary>
        /// <remarks>Parameters in target method:
        /// <list type="number">
        ///     <item><term>this object inputObj</term> The value to be coerced.</item>
        ///     <item><term><seealso cref="Func{TResult}">Func</seealso><c>&lt;<seealso cref="Uri"/>&gt;</c> ifNull</term> </item>
        ///     <item><term><seealso cref="TryCoerceHandler{object, TResult}">TryCoerceHandler</seealso><c>&lt;object, out <seealso cref="Uri"/>&gt;</c> fallback</term> </item>
        ///     <item><term><seealso cref="Uri"/> out result</term> </item>
        /// </list></remarks>
        public static IEnumerable<TestCaseData> GetTryCoerceAsUri11TestCases() => CoerceAsValueTestData.GetCoerceAsUriTestDataFN().SelectMany(testData => new CoerceAsValueTestDataFN<Uri>[]
        {
            testData,
            new CoerceAsValueTestDataFN<Uri>(PSObject.AsPSObject(testData.InputObj), testData.ReturnValue, testData.Result, testData.FallbackHandler, testData.FallbackResult, testData.IfNullCallback)
        }).Select(testData =>
            new TestCaseData(testData.InputObj, testData.IfNullCallback, testData.FallbackHandler)
                .SetArgDisplayNames("inputObj")
                .Returns(new FuncTestData3<IFuncInvocationResult<Uri>, IFuncInvocationResult<Uri, bool>, Uri, bool>(testData.ReturnValue, testData.IfNullResult, testData.FallbackResult, testData.Result))
        );

        /// <summary>
        /// Test data for <see cref="TryCoerceAsInt11Test(object, Func{int}, TryCoerceHandler{object, int})"/>, targeting
        /// <seealso cref="ExtensionMethods.TryCoerceAs{TResult}(object, Func{TResult}, TryCoerceHandler{object, TResult}, out TResult)"/> with a value type.
        /// </summary>
        /// <remarks>Parameters in target method:
        /// <list type="number">
        ///     <item><term>this object inputObj</term> The value to be coerced.</item>
        ///     <item><term><seealso cref="Func{TResult}">Func</seealso><c>&lt;int&gt;</c> ifNull</term> </item>
        ///     <item><term><seealso cref="TryCoerceHandler{object, TResult}">TryCoerceHandler</seealso><c>&lt;object, out int&gt;</c> fallback</term> </item>
        ///     <item><term><c>int</c> out result</term> </item>
        /// </list></remarks>
        public static IEnumerable<TestCaseData> GetTryCoerceAsInt11TestCases() => CoerceAsValueTestData.GetCoerceAsIntTestDataFN().SelectMany(testData => new CoerceAsValueTestDataFN<int>[]
        {
            testData,
            new CoerceAsValueTestDataFN<int>(PSObject.AsPSObject(testData.InputObj), testData.ReturnValue, testData.Result, testData.FallbackHandler, testData.FallbackResult, testData.IfNullCallback)
        }).Select(testData =>
            new TestCaseData(testData.InputObj, testData.IfNullCallback, testData.FallbackHandler)
                .SetArgDisplayNames("inputObj")
                .Returns(new FuncTestData3<IFuncInvocationResult<int>, IFuncInvocationResult<int, bool>, int, bool>(testData.ReturnValue, testData.IfNullResult, testData.FallbackResult, testData.Result))
        );

        /// <summary>
        /// Test data for <see cref="TryCoerceAsUri12Test(object, Func{Uri})"/>, targeting
        /// <seealso cref="ExtensionMethods.TryCoerceAs{TResult}(object, Func{TResult}, out TResult)"/> for a reference type.
        /// </summary>
        /// <remarks>Parameters in target method:
        /// <list type="number">
        ///     <item><term>this object inputObj</term> The value to be coerced.</item>
        ///     <item><term><seealso cref="Func{TResult}">Func</seealso><c>&lt;<seealso cref="Uri"/>&gt;</c> ifNull</term> </item>
        ///     <item><term><seealso cref="Uri"/> out result</term> </item>
        /// </list></remarks>
        public static IEnumerable<TestCaseData> GetTryCoerceAsUri12TestCases() => CoerceAsValueTestData.GetCoerceAsUriTestDataN().SelectMany(testData => new CoerceAsValueTestDataN<Uri>[]
        {
            testData,
            new CoerceAsValueTestDataN<Uri>(PSObject.AsPSObject(testData.InputObj), testData.ReturnValue, testData.Result, testData.IfNullCallback)
        }).Select(testData =>
            new TestCaseData(testData.InputObj, testData.IfNullCallback)
                .SetArgDisplayNames("inputObj")
                .Returns(new FuncTestData2<IFuncInvocationResult<Uri>, Uri, bool>(testData.ReturnValue, testData.IfNullResult, testData.Result))
        );

        /// <summary>
        /// Test data for <see cref="TryCoerceAsInt12Test(object, Func{int})"/>, targeting
        /// <seealso cref="ExtensionMethods.TryCoerceAs{TResult}(object, Func{TResult}, out TResult)"/> with a value type.
        /// </summary>
        /// <remarks>Parameters in target method:
        /// <list type="number">
        ///     <item><term>this object inputObj</term> The value to be coerced.</item>
        ///     <item><term><seealso cref="Func{TResult}">Func</seealso><c>&lt;int&gt;</c> ifNull</term> </item>
        ///     <item><term><c>int</c> out result</term> </item>
        /// </list></remarks>
        public static IEnumerable<TestCaseData> GetTryCoerceAsInt12TestCases() => CoerceAsValueTestData.GetCoerceAsIntTestDataN().SelectMany(testData => new CoerceAsValueTestDataN<int>[]
        {
            testData,
            new CoerceAsValueTestDataN<int>(PSObject.AsPSObject(testData.InputObj), testData.ReturnValue, testData.Result, testData.IfNullCallback)
        }).Select(testData =>
            new TestCaseData(testData.InputObj, testData.IfNullCallback)
                .SetArgDisplayNames("inputObj")
                .Returns(new FuncTestData2<IFuncInvocationResult<int>, int, bool>(testData.ReturnValue, testData.IfNullResult, testData.Result))
        );

        /// <summary>
        /// Test data for <see cref="TryCoerceAsUri13Test(object, bool, TryCoerceHandler{object, Uri})"/>, targeting
        /// <seealso cref="ExtensionMethods.TryCoerceAs{TResult}(object, bool, TryCoerceHandler{object, TResult}, out TResult)"/> for a reference type.
        /// </summary>
        /// <remarks>Parameters in target method:
        /// <list type="number">
        ///     <item><term>this object inputObj</term> The value to be coerced.</item>
        ///     <item><term>bool returnValueIfNull</term> </item>
        ///     <item><term><seealso cref="TryCoerceHandler{object, TResult}">TryCoerceHandler</seealso><c>&lt;object, out <seealso cref="Uri"/>&gt;</c> fallback</term> </item>
        ///     <item><term><seealso cref="Uri"/> out result</term> </item>
        /// </list></remarks>
        public static IEnumerable<TestCaseData> GetTryCoerceAsUri13TestCases() => CoerceAsValueTestData.GetCoerceAsUriTestDataFR().SelectMany(testData => new CoerceAsValueTestDataFR<Uri>[]
        {
            testData,
            new CoerceAsValueTestDataFR<Uri>(PSObject.AsPSObject(testData.InputObj), testData.ReturnValue, testData.Result, testData.FallbackHandler, testData.FallbackResult, testData.ReturnValueIfNull)
        }).Select(testData =>
            new TestCaseData(testData.InputObj, testData.ReturnValueIfNull, testData.FallbackHandler)
                .SetArgDisplayNames("inputObj")
                .Returns(new FuncTestData2<IFuncInvocationResult<Uri, bool>, Uri, bool>(testData.ReturnValue, testData.FallbackResult, testData.Result))
        );

        /// <summary>
        /// Test data for <see cref="TryCoerceAsInt13Test(object, bool, TryCoerceHandler{object, int})"/>, targeting
        /// <seealso cref="ExtensionMethods.TryCoerceAs{TResult}(object, bool, TryCoerceHandler{object, TResult}, out TResult)"/> with a value type.
        /// </summary>
        /// <remarks>Parameters in target method:
        /// <list type="number">
        ///     <item><term>this object inputObj</term> The value to be coerced.</item>
        ///     <item><term>bool returnValueIfNull</term> </item>
        ///     <item><term><seealso cref="TryCoerceHandler{object, TResult}">TryCoerceHandler</seealso><c>&lt;object, out int&gt;</c> fallback</term> </item>
        ///     <item><term><c>int</c> out result</term> </item>
        /// </list></remarks>
        public static IEnumerable<TestCaseData> GetTryCoerceAsInt13TestCases() => CoerceAsValueTestData.GetCoerceAsIntTestDataFR().SelectMany(testData => new CoerceAsValueTestDataFR<int>[]
        {
            testData,
            new CoerceAsValueTestDataFR<int>(PSObject.AsPSObject(testData.InputObj), testData.ReturnValue, testData.Result, testData.FallbackHandler, testData.FallbackResult, testData.ReturnValueIfNull)
        }).Select(testData =>
            new TestCaseData(testData.InputObj, testData.ReturnValueIfNull, testData.FallbackHandler)
                .SetArgDisplayNames("inputObj")
                .Returns(new FuncTestData2<IFuncInvocationResult<int, bool>, int, bool>(testData.ReturnValue, testData.FallbackResult, testData.Result))
        );

        /// <summary>
        /// Test data for <see cref="TryCoerceAsUri14Test(object, bool)"/>, targeting
        /// <seealso cref="ExtensionMethods.TryCoerceAs{TResult}(object, bool, out TResult)"/> for a reference type.
        /// </summary>
        /// <remarks>Parameters in target method:
        /// <list type="number">
        ///     <item><term>this object inputObj</term> The value to be coerced.</item>
        ///     <item><term>bool returnValueIfNull</term> </item>
        ///     <item><term><seealso cref="Uri"/> out result</term> </item>
        /// </list></remarks>
        public static IEnumerable<TestCaseData> GetTryCoerceAsUri14TestCases() => CoerceAsValueTestData.GetCoerceAsUriTestDataR().SelectMany(testData => new CoerceAsValueTestDataR<Uri>[]
         {
            testData,
            new CoerceAsValueTestDataR<Uri>(PSObject.AsPSObject(testData.InputObj), testData.ReturnValue, testData.Result, testData.ReturnValueIfNull)
         }).Select(testData =>
             new TestCaseData(testData.InputObj, testData.ReturnValueIfNull)
                 .SetArgDisplayNames("inputObj")
                 .Returns(new FuncTestData1<Uri, bool>(testData.ReturnValue, testData.Result))
        );

        /// <summary>
        /// Test data for <see cref="TryCoerceAsInt14Test(object, bool)"/>, targeting
        /// <seealso cref="ExtensionMethods.TryCoerceAs{TResult}(object, bool, out TResult)"/> with a value type.
        /// </summary>
        /// <remarks>Parameters in target method:
        /// <list type="number">
        ///     <item><term>this object inputObj</term> The value to be coerced.</item>
        ///     <item><term>bool returnValueIfNull</term> </item>
        ///     <item><term><c>int</c> out result</term> </item>
        /// </list></remarks>
        public static IEnumerable<TestCaseData> GetTryCoerceAsInt14TestCases() => CoerceAsValueTestData.GetCoerceAsIntTestDataR().SelectMany(testData => new CoerceAsValueTestDataR<int>[]
        {
            testData,
            new CoerceAsValueTestDataR<int>(PSObject.AsPSObject(testData.InputObj), testData.ReturnValue, testData.Result, testData.ReturnValueIfNull)
        }).Select(testData =>
             new TestCaseData(testData.InputObj, testData.ReturnValueIfNull)
                 .SetArgDisplayNames("inputObj")
                 .Returns(new FuncTestData1<int, bool>(testData.ReturnValue, testData.Result))
        );


        /// <summary>
        /// Test data for <see cref="TryCoerceAsUri15Test(object, TryCoerceHandler{object, Uri})"/>, targeting
        /// <seealso cref="ExtensionMethods.TryCoerceAs{TResult}(object, TryCoerceHandler{object, TResult}, out TResult)"/> for a reference type.
        /// </summary>
        /// <remarks>Parameters in target method:
        /// <list type="number">
        ///     <item><term>this object inputObj</term> The value to be coerced.</item>
        ///     <item><term><seealso cref="TryCoerceHandler{object, TResult}">TryCoerceHandler</seealso><c>&lt;object, out <seealso cref="Uri"/>&gt;</c> fallback</term> </item>
        ///     <item><term><seealso cref="Uri"/> out result</term> </item>
        /// </list></remarks>
        public static IEnumerable<TestCaseData> GetTryCoerceAsUri15TestCases() => CoerceAsValueTestData.GetCoerceAsUriTestDataF().SelectMany(testData => new CoerceAsValueTestDataF<Uri>[]
        {
            testData,
            new CoerceAsValueTestDataF<Uri>(PSObject.AsPSObject(testData.InputObj), testData.ReturnValue, testData.Result, testData.FallbackHandler, testData.FallbackResult)
        }).Select(testData =>
            new TestCaseData(testData.InputObj, testData.FallbackHandler)
                .SetArgDisplayNames("inputObj")
                .Returns(new FuncTestData2<IFuncInvocationResult<Uri, bool>, Uri, bool>(testData.ReturnValue, testData.FallbackResult, testData.Result))
        );

        /// <summary>
        /// Test data for <see cref="TryCoerceAsInt15Test(object, TryCoerceHandler{object, int})"/>, targeting
        /// <seealso cref="ExtensionMethods.TryCoerceAs{TResult}(object, TryCoerceHandler{object, TResult}, out TResult)"/> with a value type.
        /// </summary>
        /// <remarks>Parameters in target method:
        /// <list type="number">
        ///     <item><term>this object inputObj</term> The value to be coerced.</item>
        ///     <item><term><seealso cref="TryCoerceHandler{object, TResult}">TryCoerceHandler</seealso><c>&lt;object, out int&gt;</c> fallback</term> </item>
        ///     <item><term><c>int</c> out result</term> </item>
        /// </list></remarks>
        public static IEnumerable<TestCaseData> GetTryCoerceAsInt15TestCases() => CoerceAsValueTestData.GetCoerceAsIntTestDataF().SelectMany(testData => new CoerceAsValueTestDataF<int>[]
        {
            testData,
            new CoerceAsValueTestDataF<int>(PSObject.AsPSObject(testData.InputObj), testData.ReturnValue, testData.Result, testData.FallbackHandler, testData.FallbackResult)
        }).Select(testData =>
            new TestCaseData(testData.InputObj, testData.FallbackHandler)
                .SetArgDisplayNames("inputObj")
                .Returns(new FuncTestData2<IFuncInvocationResult<int, bool>, int, bool>(testData.ReturnValue, testData.FallbackResult, testData.Result))
        );

        /// <summary>
        /// Test data for <see cref="TryCoerceAsUri16Test(object)"/>, targeting
        /// <seealso cref="ExtensionMethods.TryCoerceAs{TResult}(object, out TResult)"/> with a reference type.
        /// </summary>
        /// <remarks>Parameters in target method:
        /// <list type="number">
        ///     <item><term>this object inputObj</term> The value to be coerced.</item>
        ///     <item><term><seealso cref="Uri"/> out result</term> </item>
        /// </list></remarks>
        public static IEnumerable<TestCaseData> GetTryCoerceAsUri16TestCases() => CoerceAsValueTestData.GetCoerceAsUriTestData().SelectMany(testData => new CoerceAsValueTestData<Uri>[]
        {
            testData,
            new CoerceAsValueTestData<Uri>(PSObject.AsPSObject(testData.InputObj), testData.ReturnValue, testData.Result)
        }).Select(testData =>
            new TestCaseData(testData.InputObj)
                .SetArgDisplayNames("inputObj")
                .Returns(new FuncTestData1<Uri, bool>(testData.ReturnValue, testData.Result))
        );

        /// <summary>
        /// Test data for <see cref="TryCoerceAsInt16Test(object)"/>, targeting
        /// <seealso cref="ExtensionMethods.TryCoerceAs{TResult}(object, out TResult)"/> with a value type.
        /// </summary>
        /// <remarks>Parameters in target method:
        /// <list type="number">
        ///     <item><term>this object inputObj</term> The value to be coerced.</item>
        ///     <item><term><c>int</c> out result</term> </item>
        /// </list></remarks>
        public static IEnumerable<TestCaseData> GetTryCoerceAsInt16TestCases() => CoerceAsValueTestData.GetCoerceAsIntTestData().SelectMany(testData => new CoerceAsValueTestData<int>[]
        {
            testData,
            new CoerceAsValueTestData<int>(PSObject.AsPSObject(testData.InputObj), testData.ReturnValue, testData.Result)
        }).Select(testData =>
            new TestCaseData(testData.InputObj)
                .SetArgDisplayNames("inputObj")
                .Returns(new FuncTestData1<int, bool>(testData.ReturnValue, testData.Result))
        );

        public static IEnumerable<TestCaseData> GetTryCast1TestCases()
        {
            foreach (var testData in new[]
            {
                new
                {
                    InputObj = (object)null,
                    ReturnValue = false,
                    SuccessResult = new InvocationResult<int?>(),
                    Result = (int?)null
                },
                new
                {
                    InputObj = (object)1,
                    ReturnValue = true,
                    SuccessResult = new InvocationResult<int?>(1),
                    Result = (int?)1
                },
                new
                {
                    InputObj = (object)"1",
                    ReturnValue = true,
                    SuccessResult = new InvocationResult<int?>(1),
                    Result = (int?)1
                },
                new
                {
                    InputObj = (object)" ",
                    ReturnValue = false,
                    SuccessResult = new InvocationResult<int?>(),
                    Result = (int?)null
                },
                new
                {
                    InputObj = (object)PSObject.AsPSObject(1),
                    ReturnValue = true,
                    SuccessResult = new InvocationResult<int?>(1),
                    Result = (int?)1
                },
                new
                {
                    InputObj = (object)PSObject.AsPSObject("1"),
                    ReturnValue = true,
                    SuccessResult = new InvocationResult<int?>(1),
                    Result = (int?)1
                }
            })
                yield return new TestCaseData(testData.InputObj)
                    .SetArgDisplayNames("inputObj")
                    .Returns(new FuncTestData2<IInvocationResult<int?>, int?, bool>(testData.ReturnValue, testData.SuccessResult, testData.Result));
        }

        public static IEnumerable<TestCaseData> GetTryCast2TestCases()
        {
            yield return new TestCaseData(null)
                .SetArgDisplayNames("inputObj")
                .Returns(new FuncTestData1<int?, bool>(false, null));
            yield return new TestCaseData("1")
                .SetArgDisplayNames("inputObj")
                .Returns(new FuncTestData1<int?, bool>(false, null));
            yield return new TestCaseData(1)
                .SetArgDisplayNames("inputObj")
                .Returns(new FuncTestData1<int?, bool>(true, 1));
            yield return new TestCaseData(PSObject.AsPSObject(1))
                .SetArgDisplayNames("inputObj")
                .Returns(new FuncTestData1<int?, bool>(true, 1));
        }

        public static IEnumerable<TestCaseData> GetTryCast3TestCases()
        {
            string nullString = "(none)";
            Func<string> ifNull = () => nullString;
            foreach (var testData in new[]
            {
                new
                {
                    InputObj = (object)null,
                    NullReturnValue = false,
                    ReturnValue = false,
                    IfSuccessResult = new InvocationResult<string>(),
                    IfNullResult = new FuncInvocationResult<string>(nullString),
                    Result = nullString
                },
                new
                {
                    InputObj = (object)null,
                    NullReturnValue = true,
                    ReturnValue = true,
                    IfSuccessResult = new InvocationResult<string>(),
                    IfNullResult = new FuncInvocationResult<string>(nullString),
                    Result = nullString
                },
                new
                {
                    InputObj = (object)1,
                    NullReturnValue = false,
                    ReturnValue = false,
                    IfSuccessResult = new InvocationResult<string>(),
                    IfNullResult = new FuncInvocationResult<string>(),
                    Result = (string)null
                },
                new
                {
                    InputObj = (object)1,
                    NullReturnValue = true,
                    ReturnValue = false,
                    IfSuccessResult = new InvocationResult<string>(),
                    IfNullResult = new FuncInvocationResult<string>(),
                    Result = (string)null
                },
                new
                {
                    InputObj = (object)"1",
                    NullReturnValue = false,
                    ReturnValue = true,
                    IfSuccessResult = new InvocationResult<string>("1"),
                    IfNullResult = new FuncInvocationResult<string>(),
                    Result = "1"
                },
                new
                {
                    InputObj = (object)"1",
                    NullReturnValue = true,
                    ReturnValue = true,
                    IfSuccessResult = new InvocationResult<string>("1"),
                    IfNullResult = new FuncInvocationResult<string>(),
                    Result = "1"
                },
                new
                {
                    InputObj = (object)PSObject.AsPSObject("1"),
                    NullReturnValue = true,
                    ReturnValue = true,
                    IfSuccessResult = new InvocationResult<string>("1"),
                    IfNullResult = new FuncInvocationResult<string>(),
                    Result = "1"
                },
                new
                {
                    InputObj = (object)PSObject.AsPSObject("1"),
                    NullReturnValue = false,
                    ReturnValue = true,
                    IfSuccessResult = new InvocationResult<string>("1"),
                    IfNullResult = new FuncInvocationResult<string>(),
                    Result = "1"
                }
            })
                yield return new TestCaseData(testData.InputObj, testData.NullReturnValue, ifNull)
                    .SetArgDisplayNames("inputObj", "nullReturnValue", "ifNull")
                    .Returns(new FuncTestData3<IInvocationResult<string>, IFuncInvocationResult<string>, string, bool>(testData.ReturnValue,
                        testData.IfSuccessResult, testData.IfNullResult, testData.Result));
        }

        public static IEnumerable<TestCaseData> GetTryCast4TestCases()
        {
            TryCoerceHandler<object, string> fallback = (object o, out string r) =>
            {
                if (o is int i)
                {
                    r = i.ToString("X4");
                    return true;
                }
                r = null;
                return false;
            };
            foreach (var testData in new[]
            {
                new
                {
                    InputObj = (object)null,
                    NullReturnValue = false,
                    ReturnValue = false,
                    IfSuccessResult = new InvocationResult<string>(),
                    Result = (string)null
                },
                new
                {
                    InputObj = (object)null,
                    NullReturnValue = true,
                    ReturnValue = true,
                    IfSuccessResult = new InvocationResult<string>(),
                    Result = (string)null
                },
                new
                {
                    InputObj = (object)1,
                    NullReturnValue = false,
                    ReturnValue = true,
                    IfSuccessResult = new InvocationResult<string>(),
                    Result = (string)null
                },
                new
                {
                    InputObj = (object)true,
                    NullReturnValue = false,
                    ReturnValue = false,
                    IfSuccessResult = new InvocationResult<string>(),
                    Result = (string)null
                },
                new
                {
                    InputObj = (object)"1",
                    NullReturnValue = true,
                    ReturnValue = true,
                    IfSuccessResult = new InvocationResult<string>("1"),
                    Result = "1"
                },
                new
                {
                    InputObj = (object)"1",
                    NullReturnValue = false,
                    ReturnValue = true,
                    IfSuccessResult = new InvocationResult<string>("1"),
                    Result = "1"
                },
                new
                {
                    InputObj = (object)PSObject.AsPSObject(1),
                    NullReturnValue = true,
                    ReturnValue = false,
                    IfSuccessResult = new InvocationResult<string>(),
                    Result = (string)null
                },
                new
                {
                    InputObj = (object)PSObject.AsPSObject(1),
                    NullReturnValue = false,
                    ReturnValue = false,
                    IfSuccessResult = new InvocationResult<string>(),
                    Result = (string)null
                },
                new
                {
                    InputObj = (object)PSObject.AsPSObject("1"),
                    NullReturnValue = true,
                    ReturnValue = false,
                    IfSuccessResult = new InvocationResult<string>("1"),
                    Result = "1"
                },
                new
                {
                    InputObj = (object)PSObject.AsPSObject("1"),
                    NullReturnValue = false,
                    ReturnValue = false,
                    IfSuccessResult = new InvocationResult<string>("1"),
                    Result = "1"
                }
            })
                yield return new TestCaseData(testData.InputObj, testData.NullReturnValue, fallback)
                    .SetArgDisplayNames("inputObj", "nullReturnValue")
                    .Returns(new FuncTestData2<IInvocationResult<string>, string, bool>(testData.ReturnValue,
                        testData.IfSuccessResult, testData.Result));
        }

        public static IEnumerable<TestCaseData> GetTryCast5TestCases()
        {
            string nullString = "(none)";
            Func<string> ifNull = () => nullString;
            foreach (var testData in new[]
            {
                new
                {
                    InputObj = (object)null,
                    ReturnValue = false,
                    IfSuccessResult = new InvocationResult<string>(),
                    IfNullResult = new FuncInvocationResult<string>(nullString),
                    Result = nullString
                },
                new
                {
                    InputObj = (object)1,
                    ReturnValue = false,
                    IfSuccessResult = new InvocationResult<string>(),
                    IfNullResult = new FuncInvocationResult<string>(),
                    Result = (string)null
                },
                new
                {
                    InputObj = (object)true,
                    ReturnValue = false,
                    IfSuccessResult = new InvocationResult<string>(),
                    IfNullResult = new FuncInvocationResult<string>(),
                    Result = (string)null
                },
                new
                {
                    InputObj = (object)"1",
                    ReturnValue = true,
                    IfSuccessResult = new InvocationResult<string>("1"),
                    IfNullResult = new FuncInvocationResult<string>(),
                    Result = "1"
                },
                new
                {
                    InputObj = (object)PSObject.AsPSObject(1),
                    ReturnValue = false,
                    IfSuccessResult = new InvocationResult<string>(),
                    IfNullResult = new FuncInvocationResult<string>(),
                    Result = (string)null
                },
                new
                {
                    InputObj = (object)PSObject.AsPSObject("1"),
                    ReturnValue = true,
                    IfSuccessResult = new InvocationResult<string>("1"),
                    IfNullResult = new FuncInvocationResult<string>(),
                    Result = "1"
                }
            })
                yield return new TestCaseData(testData.InputObj, ifNull)
                    .SetArgDisplayNames("inputObj", "ifNull")
                    .Returns(new FuncTestData3<IInvocationResult<string>, IFuncInvocationResult<string>, string, bool>(testData.ReturnValue,
                        testData.IfSuccessResult, testData.IfNullResult, testData.Result));
        }

        public static IEnumerable<TestCaseData> GetTryCast6TestCases()
        {
            TryCoerceHandler<object, string> fallback = (object o, out string r) =>
            {
                if (o is int i)
                {
                    r = i.ToString("X4");
                    return true;
                }
                r = null;
                return false;
            };
            foreach (var testData in new[]
            {
                new
                {
                    InputObj = (object)null,
                    ReturnValue = false,
                    IfSuccessResult = new InvocationResult<string>(),
                    Result = (string)null
                },
                new
                {
                    InputObj = (object)null,
                    ReturnValue = false,
                    IfSuccessResult = new InvocationResult<string>(),
                    Result = (string)null
                },
                new
                {
                    InputObj = (object)1,
                    ReturnValue = false,
                    IfSuccessResult = new InvocationResult<string>(),
                    Result = (string)null
                },
                new
                {
                    InputObj = (object)"1",
                    ReturnValue = true,
                    IfSuccessResult = new InvocationResult<string>("1"),
                    Result = "1"
                },
                new
                {
                    InputObj = (object)PSObject.AsPSObject(1),
                    ReturnValue = false,
                    IfSuccessResult = new InvocationResult<string>(),
                    Result = (string)null
                },
                new
                {
                    InputObj = (object)PSObject.AsPSObject("1"),
                    ReturnValue = true,
                    IfSuccessResult = new InvocationResult<string>("1"),
                    Result = "1"
                }
            })
                yield return new TestCaseData(testData.InputObj)
                    .SetArgDisplayNames("inputObj", "fallback")
                    .Returns(new FuncTestData2<IInvocationResult<string>, string, bool>(testData.ReturnValue, testData.IfSuccessResult, testData.Result));
        }

        public static IEnumerable<TestCaseData> GetTryCast7TestCases()
        {
            string nullString = "(none)";
            Func<string> ifNull = () => nullString;
            foreach (var testData in new[]
            {
                new
                {
                    InputObj = (object)null,
                    NullReturnValue = false,
                    ReturnValue = false,
                    IfNullResult = new FuncInvocationResult<string>(nullString),
                    Result = nullString
                },
                new
                {
                    InputObj = (object)null,
                    NullReturnValue = true,
                    ReturnValue = true,
                    IfNullResult = new FuncInvocationResult<string>(nullString),
                    Result = nullString
                },
                new
                {
                    InputObj = (object)1,
                    NullReturnValue = false,
                    ReturnValue = false,
                    IfNullResult = new FuncInvocationResult<string>(null),
                    Result = (string)null
                },
                new
                {
                    InputObj = (object)true,
                    NullReturnValue = false,
                    ReturnValue = false,
                    IfNullResult = new FuncInvocationResult<string>(),
                    Result = (string)null
                },
                new
                {
                    InputObj = (object)"1",
                    NullReturnValue = false,
                    ReturnValue = true,
                    IfNullResult = new FuncInvocationResult<string>(),
                    Result = "1"
                },
                new
                {
                    InputObj = (object)PSObject.AsPSObject(1),
                    NullReturnValue = false,
                    ReturnValue = false,
                    IfNullResult = new FuncInvocationResult<string>(null),
                    Result = (string)null
                },
                new
                {
                    InputObj = (object)PSObject.AsPSObject("1"),
                    NullReturnValue = false,
                    ReturnValue = true,
                    IfNullResult = new FuncInvocationResult<string>(),
                    Result = "1"
                },
            })
                yield return new TestCaseData(testData.InputObj, testData.NullReturnValue, ifNull)
                    .SetArgDisplayNames("inputObj", "nullReturnValue", "ifNull")
                    .Returns(new FuncTestData2<IFuncInvocationResult<string>, string, bool>(testData.ReturnValue, testData.IfNullResult, testData.Result));
        }

        public static IEnumerable<TestCaseData> GetTryCast8TestCases()
        {
            foreach (var testData in new[]
            {
                new
                {
                    InputObj = (object)null,
                    NullReturnValue = false,
                    ReturnValue = false,
                    Result = (string)null
                },
                new
                {
                    InputObj = (object)null,
                    NullReturnValue = true,
                    ReturnValue = true,
                    Result = (string)null
                },
                new
                {
                    InputObj = (object)1,
                    NullReturnValue = true,
                    ReturnValue = false,
                    Result = (string)null
                },
                new
                {
                    InputObj = (object)true,
                    NullReturnValue = false,
                    ReturnValue = false,
                    Result = (string)null
                },
                new
                {
                    InputObj = (object)"1",
                    NullReturnValue = false,
                    ReturnValue = true,
                    Result = "1"
                },
                new
                {
                    InputObj = (object)PSObject.AsPSObject(1),
                    NullReturnValue = true,
                    ReturnValue = false,
                    Result = (string)null
                },
                new
                {
                    InputObj = (object)PSObject.AsPSObject(1),
                    NullReturnValue = false,
                    ReturnValue = false,
                    Result = (string)null
                },
                new
                {
                    InputObj = (object)PSObject.AsPSObject("1"),
                    NullReturnValue = false,
                    ReturnValue = true,
                    Result = "1"
                }
            })
                yield return new TestCaseData(testData.InputObj, testData.NullReturnValue)
                    .SetArgDisplayNames("inputObj", "nullReturnValue")
                    .Returns(new FuncTestData1<string, bool>(testData.ReturnValue, testData.Result));
        }

        public static IEnumerable<TestCaseData> GetTryCast9TestCases()
        {
            string nullString = "(none)";
            Func<string> ifNull = () => nullString;
            foreach (var testData in new[]
            {
                new
                {
                    InputObj = (object)null,
                    ReturnValue = false,
                    IfNullResult = new FuncInvocationResult<string>(nullString),
                    Result = nullString
                },
                new
                {
                    InputObj = (object)1,
                    ReturnValue = false,
                    IfNullResult = new FuncInvocationResult<string>(),
                    Result = (string)null
                },
                new
                {
                    InputObj = (object)true,
                    ReturnValue = false,
                    IfNullResult = new FuncInvocationResult<string>(),
                    Result = (string)null
                },
                new
                {
                    InputObj = (object)"1",
                    ReturnValue = true,
                    IfNullResult = new FuncInvocationResult<string>(),
                    Result = "1"
                },
                new
                {
                    InputObj = (object)PSObject.AsPSObject(1),
                    ReturnValue = false,
                    IfNullResult = new FuncInvocationResult<string>(),
                    Result = (string)null
                },
                new
                {
                    InputObj = (object)PSObject.AsPSObject("1"),
                    ReturnValue = true,
                    IfNullResult = new FuncInvocationResult<string>(),
                    Result = "1"
                }
            })
                yield return new TestCaseData(testData.InputObj, ifNull)
                    .SetArgDisplayNames("inputObj", "ifNull")
                    .Returns(new FuncTestData2<IFuncInvocationResult<string>, string, bool>(testData.ReturnValue,
                        testData.IfNullResult, testData.Result));
        }

        public static IEnumerable<TestCaseData> GetTryCast10TestCases()
        {
            TryCoerceHandler<object, string> fallback = (object o, out string r) =>
            {
                if (o is int i)
                {
                    r = i.ToString("X4");
                    return true;
                }
                r = null;
                return false;
            };
            foreach (var testData in new[]
            {
                new
                {
                    InputObj = (object)null,
                    ReturnValue = false,
                    Result = (string)null
                },
                new
                {
                    InputObj = (object)null,
                    ReturnValue = false,
                    Result = (string)null
                },
                new
                {
                    InputObj = (object)1,
                    ReturnValue = false,
                    Result = (string)null
                },
                new
                {
                    InputObj = (object)"1",
                    ReturnValue = true,
                    Result = "1"
                },
                new
                {
                    InputObj = (object)PSObject.AsPSObject(1),
                    ReturnValue = false,
                    Result = (string)null
                },
                new
                {
                    InputObj = (object)PSObject.AsPSObject("1"),
                    ReturnValue = true,
                    Result = "1"
                }
            })
                yield return new TestCaseData(testData.InputObj)
                    .SetArgDisplayNames("inputObj", "fallback")
                    .Returns(new FuncTestData1<string, bool>(testData.ReturnValue, testData.Result));
        }

        public static IEnumerable<TestCaseData> GetTryGetErrorMessage1TestCases()
        {
#warning GetTryGetErrorMessage1TestCases not implemented
            return new TestCaseData[0];
        }

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
                    Assert.That(other, Is.Null, "");
                else
                {
                    Assert.That(other, Is.Not.Null, "");
                    Assert.That(_errorRecord.FullyQualifiedErrorId, Is.EqualTo(other.FullyQualifiedErrorId), "");
                    Assert.That(_errorRecord.TargetObject, Is.EqualTo(other.TargetObject), "");
                    ErrorCategoryInfo categoryInfo = _errorRecord.CategoryInfo;
                    Assert.That(categoryInfo, Is.Not.Null, "");
                    AssertEquals(categoryInfo, other.CategoryInfo);
                    AssertEquals(_errorRecord.ErrorDetails, other.ErrorDetails);
                    AssertMessageTypeAndParamNameEquals(_errorRecord.Exception, other.Exception);
                }
            }

            private static void AssertMessageTypeAndParamNameEquals([System.Diagnostics.CodeAnalysis.AllowNull] Exception x, [System.Diagnostics.CodeAnalysis.AllowNull] Exception y)
            {
                if (x is null)
                    Assert.That(y, Is.Null, "");
                {
                    Assert.That(y, Is.Not.Null, "");
                    Assert.That(x.Message, Is.EqualTo(y.Message), "");
                    Assert.That(x.GetType(), Is.EqualTo(y.GetType()), "");
                    if (x is ArgumentException argumentException)
                        Assert.That(argumentException.ParamName, Is.EqualTo(((ArgumentException)y).ParamName), "");
                }
            }

            private static void AssertEquals([System.Diagnostics.CodeAnalysis.AllowNull] ErrorDetails x, [System.Diagnostics.CodeAnalysis.AllowNull] ErrorDetails y)
            {
                if (x is null)
                    Assert.That(y, Is.Null, "");
                {
                    Assert.That(y, Is.Not.Null, "");
                    Assert.That(x.Message, Is.EqualTo(y.Message), "");
                    Assert.That(x.RecommendedAction, Is.EqualTo(y.RecommendedAction), "");
                }
            }

            private static void AssertEquals([System.Diagnostics.CodeAnalysis.NotNull] ErrorCategoryInfo x, [System.Diagnostics.CodeAnalysis.AllowNull] ErrorCategoryInfo y)
            {
                Assert.That(y, Is.Not.Null, "");
                Assert.That(x.Category, Is.EqualTo(y.Category), "");
                Assert.That(x.Activity, Is.EqualTo(y.Activity), "");
                Assert.That(x.Reason, Is.EqualTo(y.Reason), "");
                Assert.That(x.TargetName, Is.EqualTo(y.TargetName), "");
                Assert.That(x.TargetType, Is.EqualTo(y.TargetType), "");
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
    }
}
