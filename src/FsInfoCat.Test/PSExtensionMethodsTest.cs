using System;
using NUnit.Framework;
using System.Management.Automation;
using FsInfoCat.Test.Helpers;
using FsInfoCat.PS;
using System.Collections.Generic;

namespace FsInfoCat.Test
{
    [TestFixture]
    public partial class PSExtensionMethodsTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetInvokeIfNotNullTest1Cases))]
        public IInvocationResult<Uri> InvokeIfNotNullTest1(Uri target)
        {
            InvocationMonitor<Uri> invocationMonitor = new InvocationMonitor<Uri>();
            target.InvokeIfNotNull(invocationMonitor.Apply);
            return invocationMonitor.ToResult();
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetInvokeIfNotNullTest2Cases))]
        public IInvocationResult<int> InvokeIfNotNullTest2(int? target)
        {
            InvocationMonitor<int> invocationMonitor = new InvocationMonitor<int>();
            target.InvokeIfNotNull(invocationMonitor.Apply);
            return invocationMonitor.ToResult();
        }

        [Test, Property("Priority", 1)]
        public void TryCastTestTest()
        {
            object inputObj = null;
            List<int?> ifSuccessResults = new List<int?>();
            Action<int?> ifSuccessHandler = i => ifSuccessResults.Add(i);
            int ifNullCallCount = 0;
            Func<int?> ifNullHandler = () =>
            {
                ifNullCallCount++;
                return 12;
            };
            bool nullReturnValue = false;
            bool returnValue = CoersionHelper.TryCast(inputObj, ifSuccessHandler, nullReturnValue, null, out int? result);
            Assert.That(returnValue, Is.False);
            Assert.That(ifSuccessResults.Count, Is.EqualTo(0));
            Assert.That(result, Is.Null);

            nullReturnValue = true;
            returnValue = CoersionHelper.TryCast(inputObj, ifSuccessHandler, nullReturnValue, null, out result);
            Assert.That(returnValue, Is.True);
            Assert.That(ifSuccessResults.Count, Is.EqualTo(1));
            Assert.That(result, Is.Null);
            Assert.That(ifSuccessResults[0], Is.Null);

            nullReturnValue = false;
            ifSuccessResults.Clear();
            returnValue = CoersionHelper.TryCast(inputObj, ifSuccessHandler, nullReturnValue, ifNullHandler, out result);
            Assert.That(returnValue, Is.False);
            Assert.That(ifSuccessResults.Count, Is.EqualTo(0));
            Assert.That(ifNullCallCount, Is.EqualTo(1));
            Assert.That(result, Is.EqualTo(12));

            nullReturnValue = true;
            ifNullCallCount = 0;
            returnValue = CoersionHelper.TryCast(inputObj, ifSuccessHandler, nullReturnValue, ifNullHandler, out result);
            Assert.That(returnValue, Is.True);
            Assert.That(ifSuccessResults.Count, Is.EqualTo(1));
            Assert.That(ifNullCallCount, Is.EqualTo(1));
            Assert.That(result, Is.EqualTo(12));
            Assert.That(ifSuccessResults[0], Is.EqualTo(12));

            inputObj = 7;
            ifNullCallCount = 0;
            ifSuccessResults.Clear();
            returnValue = CoersionHelper.TryCast(inputObj, ifSuccessHandler, nullReturnValue, ifNullHandler, out result);
            Assert.That(returnValue, Is.True);
            Assert.That(ifSuccessResults.Count, Is.EqualTo(1));
            Assert.That(ifNullCallCount, Is.EqualTo(0));
            Assert.That(result, Is.EqualTo(7));
            Assert.That(ifSuccessResults[0], Is.EqualTo(7));
        }


        [Test, Property("Priority", 1)]
        public void TryCoerceAsIntTest()
        {
            int ifNullCallCount = 0;
            int fallbackCallCount = 0;
            List<int> ifSuccessResults = new List<int>();
            TryCoerceHandler<object, int> fallbackHandler = (object o, out int i) =>
            {
                fallbackCallCount++;
                if (o is DateTime dateTime)
                {
                    i = dateTime.Year;
                    return true;
                }
                i = default;
                return false;
            };
            Func<int> ifNullHandler = () =>
            {
                ifNullCallCount++;
                return 12;
            };
            Action<int> ifSuccessHandler = i => ifSuccessResults.Add(i);
            bool returnValueIfNull = false;
            object inputObj = null;
            bool returnValue = CoersionHelper.TryCoerceAs(inputObj, ifNullHandler, returnValueIfNull, fallbackHandler, ifSuccessHandler, out int result);
            Assert.That(returnValue, Is.False);
            Assert.That(ifNullCallCount, Is.EqualTo(1));
            Assert.That(fallbackCallCount, Is.EqualTo(0));
            Assert.That(ifSuccessResults.Count, Is.EqualTo(0));
            Assert.That(result, Is.EqualTo(12));

            ifNullCallCount = 0;
            returnValueIfNull = true;
            returnValue = CoersionHelper.TryCoerceAs(inputObj, ifNullHandler, returnValueIfNull, fallbackHandler, ifSuccessHandler, out result);
            Assert.That(returnValue, Is.True);
            Assert.That(ifNullCallCount, Is.EqualTo(1));
            Assert.That(fallbackCallCount, Is.EqualTo(0));
            Assert.That(ifSuccessResults.Count, Is.EqualTo(1));
            Assert.That(result, Is.EqualTo(12));
            Assert.That(ifSuccessResults[0], Is.EqualTo(12));

            ifSuccessResults.Clear();
            ifNullCallCount = 0;
            inputObj = 3;
            returnValue = CoersionHelper.TryCoerceAs(inputObj, ifNullHandler, returnValueIfNull, fallbackHandler, ifSuccessHandler, out result);
            Assert.That(returnValue, Is.True);
            Assert.That(ifNullCallCount, Is.EqualTo(0));
            Assert.That(fallbackCallCount, Is.EqualTo(0));
            Assert.That(ifSuccessResults.Count, Is.EqualTo(1));
            Assert.That(result, Is.EqualTo(3));
            Assert.That(ifSuccessResults[0], Is.EqualTo(3));

            ifSuccessResults.Clear();
            inputObj = (long)int.MaxValue;
            returnValue = CoersionHelper.TryCoerceAs(inputObj, ifNullHandler, returnValueIfNull, fallbackHandler, ifSuccessHandler, out result);
            Assert.That(returnValue, Is.True);
            Assert.That(ifNullCallCount, Is.EqualTo(0));
            Assert.That(fallbackCallCount, Is.EqualTo(0));
            Assert.That(ifSuccessResults.Count, Is.EqualTo(1));
            Assert.That(result, Is.EqualTo(int.MaxValue));
            Assert.That(ifSuccessResults[0], Is.EqualTo(int.MaxValue));

            ifSuccessResults.Clear();
            inputObj = ((long)int.MaxValue) + 1L;
            returnValue = CoersionHelper.TryCoerceAs(inputObj, ifNullHandler, returnValueIfNull, fallbackHandler, ifSuccessHandler, out result);
            Assert.That(returnValue, Is.False);
            Assert.That(ifNullCallCount, Is.EqualTo(0));
            Assert.That(fallbackCallCount, Is.EqualTo(1));
            Assert.That(ifSuccessResults.Count, Is.EqualTo(0));
            Assert.That(result, Is.EqualTo(default(int)));

            DateTime d = DateTime.Now;
            inputObj = d;
            ifSuccessResults.Clear();
            fallbackCallCount = 0;
            returnValue = CoersionHelper.TryCoerceAs(inputObj, ifNullHandler, returnValueIfNull, fallbackHandler, ifSuccessHandler, out result);
            Assert.That(returnValue, Is.True);
            Assert.That(ifNullCallCount, Is.EqualTo(0));
            Assert.That(fallbackCallCount, Is.EqualTo(1));
            Assert.That(ifSuccessResults.Count, Is.EqualTo(1));
            Assert.That(result, Is.EqualTo(d.Year));
            Assert.That(ifSuccessResults[0], Is.EqualTo(d.Year));
        }

        /// <summary>
        /// Unit test for <see cref="CoersionHelper.TryCoerceTo{TInput, TResult}(TInput, Func{TInput, TResult}, Func{TResult}, out TResult)"/>.
        /// </summary>
        [Test, Property("Priority", 1), Ignore("Not enough time to work out data source")]
        [TestCaseSource(nameof(GetTryCoerceTo1TestCases))]
        public IFuncTestData3<IFuncInvocationResult<Uri>, IFuncInvocationResult<Uri>, Uri, bool> TryCoerceToTest1(string inputObj, Func<string, Uri> ifNotNull, Func<Uri> ifNull)
        {
            ifNotNull = ifNotNull.Monitor(out Func<IFuncInvocationResult<Uri>> getNotNullResult);
            ifNull = ifNull.Monitor(out Func<IFuncInvocationResult<Uri>> getNullResult);
            bool returnValue = inputObj.TryCoerceTo(ifNotNull, ifNull, out Uri result);
            return new FuncTestData3<IFuncInvocationResult<Uri>, IFuncInvocationResult<Uri>, Uri, bool>(returnValue, getNotNullResult(), getNullResult(), result);
        }

        /// <summary>
        /// Unit test for <see cref="CoersionHelper.TryCoerceTo{TInput, TResult}(TInput?, Func{TInput, TResult}, Func{TResult}, out TResult)"/>.
        /// </summary>
        [Test, Property("Priority", 1), Ignore("Not enough time to work out data source")]
        [TestCaseSource(nameof(GetTryCoerceTo3TestCases))]
        public IFuncTestData3<IFuncInvocationResult<string>, IFuncInvocationResult<string>, string, bool> TryCoerceToTest3(int? inputObj, Func<int, string> ifNotNull, Func<string> ifNull)
        {
            ifNotNull = ifNotNull.Monitor(out Func<IFuncInvocationResult<string>> getNotNullResult);
            ifNull = ifNull.Monitor(out Func<IFuncInvocationResult<string>> getNullResult);
            bool returnValue = inputObj.TryCoerceTo(ifNotNull, ifNull, out string result);
            return new FuncTestData3<IFuncInvocationResult<string>, IFuncInvocationResult<string>, string, bool>(returnValue, getNotNullResult(), getNullResult(), result);
        }

        /// <summary>
        /// Unit test for <see cref="CoersionHelper.TryCoerceTo{TInput, TResult}(TInput, Func{TInput, TResult}, out TResult)"/>.
        /// </summary>
        [Test, Property("Priority", 2), Ignore("Not enough time to work out data source")]
        [TestCaseSource(nameof(GetTryCoerceTo2TestCases))]
        public IFuncTestData2<IFuncInvocationResult<Uri>, Uri, bool> TryCoerceToTest2(string inputObj, Func<string, Uri> ifNotNull)
        {
            ifNotNull = ifNotNull.Monitor(out Func<IFuncInvocationResult<Uri>> getNotNullResult);
            bool returnValue = inputObj.TryCoerceTo(ifNotNull, out Uri result);
            return new FuncTestData2<IFuncInvocationResult<Uri>, Uri, bool>(returnValue, getNotNullResult(), result);
        }

        /// <summary>
        /// Unit test for <see cref="CoersionHelper.TryCoerceTo{TInput, TResult}(TInput?, Func{TInput, TResult}, out TResult)"/>.
        /// </summary>
        [Test, Property("Priority", 2), Ignore("Not enough time to work out data source")]
        [TestCaseSource(nameof(GetTryCoerceTo4TestCases))]
        public IFuncTestData2<IFuncInvocationResult<string>, string, bool> TryCoerceToTest4(int? inputObj, Func<int, string> ifNotNull)
        {
            ifNotNull = ifNotNull.Monitor(out Func<IFuncInvocationResult<string>> getNotNullResult);
            bool returnValue = inputObj.TryCoerceTo(ifNotNull, out string result);
            return new FuncTestData2<IFuncInvocationResult<string>, string, bool>(returnValue, getNotNullResult(), result);
        }

        /// <summary>
        /// Unit test for <seealso cref="CoersionHelper.TryCoerceAs{TResult}(object, Func{TResult}, bool, TryCoerceHandler{object, TResult}, Action{TResult}, out TResult)"/> using a reference type.
        /// </summary>
        /// <param name="inputObj">Value to be coerced.</param>
        /// <param name="ifNull">Produces the result value when <paramref name="inputObj"/> is null.</param>
        /// <param name="returnValueIfNull">The return value of the target method when <paramref name="inputObj"/> is null.</param>
        /// <param name="fallback">The fallback coersion handler that will be invoked if <paramref name="inputObj"/> is not null and cannot be converted.</param>
        /// <remarks>Other parameters used:
        /// <list type="bullet">
        ///     <item>
        ///         <term><seealso cref="Action{TResult}">Action</seealso><c>&lt;<seealso cref="Uri"/>&gt;</c> ifSuccess</term> Callback invoked upon successful coersion.
        ///     </item>
        ///     <item>
        ///         <term><c>out</c> result</term> <seealso cref="Uri"/>
        ///     </item>
        /// </list></remarks>
        /// <returns><seealso cref="IFuncTestData4{TOut1, TOut2, TOut3, TOut4, TResult}">IFuncTestData4</seealso><c>&lt;</c>
        /// <list type="number">
        ///     <item>
        ///         <term><seealso cref="IFuncInvocationResult{TResult}">IFuncInvocationResult</seealso><c>&lt;<seealso cref="Uri"/>&gt;</c> Output1</term>
        ///         <list type="bullet">
        ///             <item><term><c>bool</c> WasInvoked</term> <see langword="true"/> if the <paramref name="ifNull"/> delegate was invoked; otherwise, <see langword="false"/>.</item>
        ///             <item><term><seealso cref="Uri"/> ReturnValue</term> The value returned by the <paramref name="ifNull"/> delegate.</item>
        ///         </list>
        ///     </item>
        ///     <item>
        ///         <term><seealso cref="IFuncInvocationResult{TOut, TResult}">IFuncInvocationResult</seealso><c>&lt;<seealso cref="Uri"/>, bool&gt;</c>  Output2</term>
        ///         <list type="bullet">
        ///             <item><term><c>bool</c> WasInvoked</term> <see langword="true"/> if the <paramref name="fallback"/> delegate was invoked; otherwise, <see langword="false"/>.</item>
        ///             <item><term><c>bool</c> Input1</term> The value returned by <paramref name="fallback"/>.</item>
        ///             <item><term><seealso cref="Uri"/> Output1</term> The value of the output parameter of the <paramref name="fallback"/> delegate.</item>
        ///         </list>
        ///     </item>
        ///     <item>
        ///         <term><seealso cref="IInvocationResult{TOut}">IInvocationResult</seealso><c>&lt;<seealso cref="Uri"/>&gt;</c> Output3</term>
        ///         <list type="bullet">
        ///             <item><term><c>bool</c> WasInvoked</term> <see langword="true"/> if the <c>ifSuccess</c> callback was invoked; otherwise, <see langword="false"/>.</item>
        ///             <item><term><seealso cref="Uri"/> Output1</term> The value passed to the <c>ifSuccess</c> callback.</item>
        ///         </list>
        ///     </item>
        ///     <item>
        ///         <term><seealso cref="Uri"/> Output4</term> The value from the <c>return</c> parameter of
        ///             <seealso cref="CoersionHelper.TryCoerceAs{TResult}(object, Func{TResult}, bool, TryCoerceHandler{object, TResult}, Action{TResult}, out TResult)"/>.
        ///     </item>
        ///     <item>
        ///         <term><c>bool</c> ReturnValue</term> The value returned by
        ///         <seealso cref="CoersionHelper.TryCoerceAs{TResult}(object, Func{TResult}, bool, TryCoerceHandler{object, TResult}, Action{TResult}, out TResult)"/>.
        ///     </item>
        /// </list>
        /// <c>&gt;</c>
        /// </returns>
        [Test, Property("Priority", 1), Ignore("Not enough time to work out data source")]
        [TestCaseSource(nameof(GetTryCoerceAsUri1TestCases))]
        public IFuncTestData4<IFuncInvocationResult<Uri>, IFuncInvocationResult<Uri, bool>, IInvocationResult<Uri>, Uri, bool> TryCoerceAsUri1Test(object inputObj, Func<Uri> ifNull,
            bool returnValueIfNull, TryCoerceHandler<object, Uri> fallback)
        {
            InvocationMonitor<Uri> ifSuccessMontor = new InvocationMonitor<Uri>();
            fallback = fallback.Monitor(out Func<IFuncInvocationResult<Uri, bool>> getFallBackResult);
            ifNull = ifNull.Monitor(out Func<IFuncInvocationResult<Uri>> getIfNullResult);
            bool returnValue = CoersionHelper.TryCoerceAs(inputObj, ifNull, returnValueIfNull, fallback, ifSuccessMontor.Apply, out Uri result);
            return new FuncTestData4<IFuncInvocationResult<Uri>, IFuncInvocationResult<Uri, bool>, IInvocationResult<Uri>, Uri, bool>(returnValue, getIfNullResult(), getFallBackResult(),
                ifSuccessMontor.ToResult(), result);
        }
        /*
         Message: 
          Expected: <FuncTestData4`5{ ReturnValue = False, Output1 = FuncInvocationResult`1{ ReturnValue = https://www.erwinefamily.net/, WasInvoked = True }, Output2 = FuncInvocationResult`2{ ReturnValue = False, WasInvoked = True, Output1 =  }, Output3 = InvocationResult`1{ WasInvoked = False, Output1 =  }, Output4 = https://www.erwinefamily.net/ }>
          But was:  <FuncTestData4`5{ ReturnValue = False, Output1 = FuncInvocationResult`1{ ReturnValue = https://www.erwinefamily.net/, WasInvoked = True }, Output2 = FuncInvocationResult`2{ ReturnValue = False, WasInvoked = False, Output1 =  }, Output3 = InvocationResult`1{ WasInvoked = False, Output1 =  }, Output4 = https://www.erwinefamily.net/ }>
        
         */
        /// <summary>
        /// Unit test for <seealso cref="CoersionHelper.TryCoerceAs{TResult}(object, Func{TResult}, bool, TryCoerceHandler{object, TResult}, Action{TResult}, out TResult)"/> using a value type.
        /// </summary>
        /// <param name="inputObj">Value to be coerced.</param>
        /// <param name="ifNull">Produces the result value when <paramref name="inputObj"/> is null.</param>
        /// <param name="returnValueIfNull">The return value of the target method when <paramref name="inputObj"/> is null.</param>
        /// <param name="fallback">The fallback coersion handler that will be invoked if <paramref name="inputObj"/> is not null and cannot be converted.</param>
        /// <remarks>Other parameters used:
        /// <list type="bullet">
        ///     <item>
        ///         <term><seealso cref="Action{TResult}">Action</seealso><c>&lt;int&gt;</c> ifSuccess</term> Callback invoked upon successful coersion.
        ///     </item>
        ///     <item>
        ///         <term><c><c>out</c> TResult</c> result</term> The coerced <seealso cref="TResult"/> value.
        ///     </item>
        /// </list></remarks>
        /// <returns><seealso cref="IFuncTestData4{TOut1, TOut2, TOut3, TOut4, TResult}">IFuncTestData4</seealso><c>&lt;</c>
        /// <list type="number">
        ///     <item>
        ///         <term><seealso cref="IFuncInvocationResult{TResult}">IFuncInvocationResult</seealso><c>&lt;int&gt;</c> Output1</term>
        ///         <list type="bullet">
        ///             <item><term><c>bool</c> WasInvoked</term> <see langword="true"/> if the <paramref name="ifNull"/> delegate was invoked; otherwise, <see langword="false"/>.</item>
        ///             <item><term><c>int</c> ReturnValue</term> The value returned by the <paramref name="ifNull"/> delegate.</item>
        ///         </list>
        ///     </item>
        ///     <item>
        ///         <term><seealso cref="IFuncInvocationResult{TOut, TResult}">IFuncInvocationResult</seealso><c>&lt;int, bool&gt;</c>  Output2</term>
        ///         <list type="bullet">
        ///             <item><term><c>bool</c> WasInvoked</term> <see langword="true"/> if the <paramref name="fallback"/> delegate was invoked; otherwise, <see langword="false"/>.</item>
        ///             <item><term><c>bool</c> Input1</term> The value returned by <paramref name="fallback"/>.</item>
        ///             <item><term><c>int</c> Output1</term> The value of the output parameter of the <paramref name="fallback"/> delegate.</item>
        ///         </list>
        ///     </item>
        ///     <item>
        ///         <term><seealso cref="IInvocationResult{TOut}">IInvocationResult</seealso><c>&lt;int&gt;</c> Output3</term>
        ///         <list type="bullet">
        ///             <item><term><c>bool</c> WasInvoked</term> <see langword="true"/> if the <c>ifSuccess</c> callback was invoked; otherwise, <see langword="false"/>.</item>
        ///             <item><term><c>int</c> Output1</term> The value passed to the <c>ifSuccess</c> callback.</item>
        ///         </list>
        ///     </item>
        ///     <item>
        ///         <term><c>int</c> Output4</term> The value from the <c>return</c> parameter of
        ///             <seealso cref="CoersionHelper.TryCoerceAs{TResult}(object, Func{TResult}, bool, TryCoerceHandler{object, TResult}, Action{TResult}, out TResult)"/>.
        ///     </item>
        ///     <item>
        ///         <term><c>bool</c> ReturnValue</term> The value returned by
        ///         <seealso cref="CoersionHelper.TryCoerceAs{TResult}(object, Func{TResult}, bool, TryCoerceHandler{object, TResult}, Action{TResult}, out TResult)"/>.
        ///     </item>
        /// </list>
        /// <c>&gt;</c>
        /// </returns>
        [Test, Property("Priority", 1), Ignore("Not enough time to work out data source")]
        [TestCaseSource(nameof(GetTryCoerceAsInt1TestCases))]
        public IFuncTestData4<IFuncInvocationResult<int>, IFuncInvocationResult<int, bool>, IInvocationResult<int>, int, bool> TryCoerceAsInt1Test(object inputObj, Func<int> ifNull,
            bool returnValueIfNull, TryCoerceHandler<object, int> fallback)
        {
            InvocationMonitor<int> ifSuccessMontor = new InvocationMonitor<int>();
            fallback = fallback.Monitor(out Func<IFuncInvocationResult<int, bool>> getFallBackResult);
            ifNull = ifNull.Monitor(out Func<IFuncInvocationResult<int>> getIfNullResult);
            bool returnValue = CoersionHelper.TryCoerceAs(inputObj, ifNull, returnValueIfNull, fallback, ifSuccessMontor.Apply, out int result);
            return new FuncTestData4<IFuncInvocationResult<int>, IFuncInvocationResult<int, bool>, IInvocationResult<int>, int, bool>(returnValue, getIfNullResult(), getFallBackResult(),
                ifSuccessMontor.ToResult(), result);
        }

        /// <summary>
        /// Unit test for <seealso cref="CoersionHelper.TryCoerceAs{TResult}(object, Func{TResult}, bool, Action{TResult}, out TResult)"/> using a reference type.
        /// </summary>
        /// <param name="inputObj">Value to be coerced.</param>
        /// <param name="ifNull">Produces the result value when <paramref name="inputObj"/> is null.</param>
        /// <param name="returnValueIfNull">The return value of the target method when <paramref name="inputObj"/> is null.</param>
        /// <remarks>Other parameters used:
        /// <list type="bullet">
        ///     <item>
        ///         <term><seealso cref="Action{TResult}">Action</seealso><c>&lt;int&gt;</c> ifSuccess</term> Callback invoked upon successful coersion.
        ///     </item>
        ///     <item>
        ///         <term><c>out TResult</c> result</term> The coerced <seealso cref="TResult"/> value.
        ///     </item>
        /// </list></remarks>
        /// <returns><seealso cref="IFuncTestData4{TOut1, TOut2, TOut3, TOut4, TResult}">IFuncTestData4</seealso><c>&lt;<seealso cref="IFuncInvocationResult{TResult}">IFuncInvocationResult</seealso>&lt;<seealso cref="Uri"/>&gt;,
        ///     <seealso cref="IFuncInvocationResult{TOut, TResult}">IFuncInvocationResult</seealso>&lt;<seealso cref="Uri"/>, bool&gt;,
        ///     <seealso cref="IInvocationResult{TOut}">IInvocationResult</seealso>&lt;<seealso cref="Uri"/>&gt;,
        ///     <seealso cref="Uri"/>,
        ///     bool&gt;</c>
        /// </returns>
        [Test, Property("Priority", 2), Ignore("Not enough time to work out data source")]
        [TestCaseSource(nameof(GetTryCoerceAsUri2TestCases))]
        public IFuncTestData3<IFuncInvocationResult<Uri>, IInvocationResult<Uri>, Uri, bool> TryCoerceAsUri2Test(object inputObj, Func<Uri> ifNull, bool returnValueIfNull)
        {
            InvocationMonitor<Uri> ifSuccessMonitor = new InvocationMonitor<Uri>();
            ifNull = ifNull.Monitor(out Func<IFuncInvocationResult<Uri>> getIfNullResult);
            bool returnValue = CoersionHelper.TryCoerceAs(inputObj, ifNull, returnValueIfNull, ifSuccessMonitor.Apply, out Uri result);
            return new FuncTestData3<IFuncInvocationResult<Uri>, IInvocationResult<Uri>, Uri, bool>(returnValue, getIfNullResult(), ifSuccessMonitor.ToResult(), result);
        }

        /// <summary>
        /// Unit test for <seealso cref="CoersionHelper.TryCoerceAs{TResult}(object, Func{TResult}, bool, Action{TResult}, out TResult)"/> using a value type.
        /// </summary>
        /// <param name="inputObj">Value to be coerced.</param>
        /// <param name="ifNull">Produces the result value when <paramref name="inputObj"/> is null.</param>
        /// <param name="returnValueIfNull">The return value of the target method when <paramref name="inputObj"/> is null.</param>
        /// <remarks>Other parameters used:
        /// <list type="bullet">
        ///     <item>
        ///         <term><seealso cref="Action{TResult}">Action</seealso><c>&lt;int&gt;</c> ifSuccess</term> Callback invoked upon successful coersion.
        ///     </item>
        ///     <item>
        ///         <term><c>out TResult</c> result</term> The coerced <seealso cref="TResult"/> value.
        ///     </item>
        /// </list></remarks>
        /// <returns><seealso cref="IFuncTestData3{TOut1, TOut2, TOut3, TResult}">IFuncTestData3</seealso><c>&lt;<seealso cref="IFuncInvocationResult{TResult}">IFuncInvocationResult</seealso>&lt;int&gt;,
        ///     <seealso cref="IInvocationResult{TOut}">IInvocationResult</seealso>&lt;int&gt;,
        ///     int,
        ///     bool&gt;</c>
        /// </returns>
        [Test, Property("Priority", 2), Ignore("Not enough time to work out data source")]
        [TestCaseSource(nameof(GetTryCoerceAsInt2TestCases))]
        public IFuncTestData3<IFuncInvocationResult<int>, IInvocationResult<int>, int, bool> TryCoerceAsInt2Test(object inputObj, Func<int> ifNull, bool returnValueIfNull)
        {
            InvocationMonitor<int> ifSuccessMonitor = new InvocationMonitor<int>();
            ifNull = ifNull.Monitor(out Func<IFuncInvocationResult<int>> getIfNullResult);
            bool returnValue = CoersionHelper.TryCoerceAs(inputObj, ifNull, returnValueIfNull, ifSuccessMonitor.Apply, out int result);
            return new FuncTestData3<IFuncInvocationResult<int>, IInvocationResult<int>, int, bool>(returnValue, getIfNullResult(), ifSuccessMonitor.ToResult(), result);
        }

        /// <summary>
        /// Unit test for <seealso cref="CoersionHelper.TryCoerceAs{TResult}(object, Func{TResult}, TryCoerceHandler{object, TResult}, Action{TResult}, out TResult)"/> using a reference type.
        /// </summary>
        /// <param name="inputObj">Value to be coerced.</param>
        /// <param name="ifNull">Produces the result value when <paramref name="inputObj"/> is null.</param>
        /// <param name="fallback">The fallback coersion handler that will be invoked if <paramref name="inputObj"/> is not null and cannot be converted.</param>
        /// <remarks>Other parameters used:
        /// <list type="bullet">
        ///     <item>
        ///         <term><seealso cref="Action{TResult}">Action</seealso><c>&lt;int&gt;</c> ifSuccess</term> Callback invoked upon successful coersion.
        ///     </item>
        ///     <item>
        ///         <term><c>out TResult</c> result</term> The coerced <seealso cref="TResult"/> value.
        ///     </item>
        /// </list></remarks>
        /// <returns><seealso cref="IFuncTestData4{TOut1, TOut2, TOut3, TOut4, TResult}">IFuncTestData4</seealso><c>&lt;<seealso cref="IFuncInvocationResult{TResult}">IFuncInvocationResult</seealso>&lt;<seealso cref="Uri"/>&gt;,
        ///     <seealso cref="IFuncInvocationResult{TOut, TResult}">IFuncInvocationResult</seealso>&lt;<seealso cref="Uri"/>, bool&gt;,
        ///     <seealso cref="IInvocationResult{TOut}">IInvocationResult</seealso>&lt;<seealso cref="Uri"/>&gt;,
        ///     <seealso cref="Uri"/>,
        ///     bool&gt;</c>
        /// </returns>
        [Test, Property("Priority", 2), Ignore("Not enough time to work out data source")]
        [TestCaseSource(nameof(GetTryCoerceAsUri3TestCases))]
        public IFuncTestData4<IFuncInvocationResult<Uri>, IFuncInvocationResult<Uri, bool>, IInvocationResult<Uri>, Uri, bool> TryCoerceAsUri3Test(object inputObj, Func<Uri> ifNull,
            TryCoerceHandler<object, Uri> fallback)
        {
            InvocationMonitor<Uri> ifSuccessMonitor = new InvocationMonitor<Uri>();
            fallback = fallback.Monitor(out Func<IFuncInvocationResult<Uri, bool>> getFallBackResult);
            ifNull = ifNull.Monitor(out Func<IFuncInvocationResult<Uri>> getIfNullResult);
            bool returnValue = CoersionHelper.TryCoerceAs(inputObj, ifNull, fallback, ifSuccessMonitor.Apply, out Uri result);
            return new FuncTestData4<IFuncInvocationResult<Uri>, IFuncInvocationResult<Uri, bool>, IInvocationResult<Uri>, Uri, bool>(returnValue, getIfNullResult(), getFallBackResult(),
                ifSuccessMonitor.ToResult(), result);
        }

        /// <summary>
        /// Unit test for <seealso cref="CoersionHelper.TryCoerceAs{TResult}(object, Func{TResult}, TryCoerceHandler{object, TResult}, Action{TResult}, out TResult)"/> using a value type.
        /// </summary>
        /// <param name="inputObj">Value to be coerced.</param>
        /// <param name="ifNull">Produces the result value when <paramref name="inputObj"/> is null.</param>
        /// <param name="fallback">The fallback coersion handler that will be invoked if <paramref name="inputObj"/> is not null and cannot be converted.</param>
        /// <remarks>Other parameters used:
        /// <list type="bullet">
        ///     <item>
        ///         <term><seealso cref="Action{TResult}">Action</seealso><c>&lt;int&gt;</c> ifSuccess</term> Callback invoked upon successful coersion.
        ///     </item>
        ///     <item>
        ///         <term><c>out TResult</c> result</term> The coerced <seealso cref="TResult"/> value.
        ///     </item>
        /// </list></remarks>
        /// <returns><seealso cref="IFuncTestData4{TOut1, TOut2, TOut3, TOut4, TResult}">IFuncTestData4</seealso><c>&lt;<seealso cref="IFuncInvocationResult{TResult}">IFuncInvocationResult</seealso>&lt;int&gt;,
        ///     <seealso cref="IFuncInvocationResult{TOut, TResult}">&lt;int, bool&gt;</seealso>,
        ///     <seealso cref="IInvocationResult{TOut}">IInvocationResult</seealso>&lt;int&gt;,
        ///     int,
        ///     bool&gt;</c>
        /// </returns>
        [Test, Property("Priority", 2), Ignore("Not enough time to work out data source")]
        [TestCaseSource(nameof(GetTryCoerceAsInt3TestCases))]
        public IFuncTestData4<IFuncInvocationResult<int>, IFuncInvocationResult<int, bool>, IInvocationResult<int>, int, bool> TryCoerceAsInt3Test(object inputObj, Func<int> ifNull,
            TryCoerceHandler<object, int> fallback)
        {
            InvocationMonitor<int> ifSuccessMonitor = new InvocationMonitor<int>();
            fallback = fallback.Monitor(out Func<IFuncInvocationResult<int, bool>> getFallBackResult);
            ifNull = ifNull.Monitor(out Func<IFuncInvocationResult<int>> getIfNullResult);
            bool returnValue = CoersionHelper.TryCoerceAs(inputObj, ifNull, fallback, ifSuccessMonitor.Apply, out int result);
            return new FuncTestData4<IFuncInvocationResult<int>, IFuncInvocationResult<int, bool>, IInvocationResult<int>, int, bool>(returnValue, getIfNullResult(), getFallBackResult(), ifSuccessMonitor.ToResult(), result);
        }

        /// <summary>
        /// Unit test for <seealso cref="CoersionHelper.TryCoerceAs{TResult}(object, Func{TResult}, Action{TResult}, out TResult)"/> using a reference type.
        /// </summary>
        /// <param name="inputObj">Value to be coerced.</param>
        /// <param name="ifNull">Produces the result value when <paramref name="inputObj"/> is null.</param>
        /// <remarks>Other parameters used:
        /// <list type="bullet">
        ///     <item>
        ///         <term><seealso cref="Action{TResult}">Action</seealso><c>&lt;int&gt;</c> ifSuccess</term> Callback invoked upon successful coersion.
        ///     </item>
        ///     <item>
        ///         <term><c>out TResult</c> result</term> The coerced <seealso cref="TResult"/> value.
        ///     </item>
        /// </list></remarks>
        /// <returns><seealso cref="IFuncTestData3{TOut1, TOut2, TOut3, TResult}">IFuncTestData3</seealso><c>&lt;<seealso cref="IFuncInvocationResult{TResult}">IFuncInvocationResult</seealso>&lt;<seealso cref="Uri"/>&gt;,
        ///     <seealso cref="IInvocationResult{TOut}">IInvocationResult</seealso>&lt;<seealso cref="Uri"/>&gt;,
        ///     <seealso cref="Uri"/>,
        ///     bool&gt;</c>
        /// </returns>
        [Test, Property("Priority", 2), Ignore("Not enough time to work out data source")]
        [TestCaseSource(nameof(GetTryCoerceAsUri4TestCases))]
        public IFuncTestData3<IFuncInvocationResult<Uri>, IInvocationResult<Uri>, Uri, bool> TryCoerceAsUri4Test(object inputObj, Func<Uri> ifNull)
        {
            InvocationMonitor<Uri> ifSuccessMonitor = new InvocationMonitor<Uri>();
            ifNull = ifNull.Monitor(out Func<IFuncInvocationResult<Uri>> getResult);
            bool returnValue = CoersionHelper.TryCoerceAs(inputObj, ifNull, ifSuccessMonitor.Apply, out Uri result);
            return new FuncTestData3<IFuncInvocationResult<Uri>, IInvocationResult<Uri>, Uri, bool>(returnValue, getResult(), ifSuccessMonitor.ToResult(), result);
        }

        /// <summary>
        /// Unit test for <seealso cref="CoersionHelper.TryCoerceAs{TResult}(object, Func{TResult}, Action{TResult}, out TResult)"/> using a value type.
        /// </summary>
        /// <param name="inputObj">Value to be coerced.</param>
        /// <param name="ifNull">Produces the result value when <paramref name="inputObj"/> is null.</param>
        /// <remarks>Other parameters used:
        /// <list type="bullet">
        ///     <item>
        ///         <term><seealso cref="Action{TResult}">Action</seealso><c>&lt;int&gt;</c> ifSuccess</term> Callback invoked upon successful coersion.
        ///     </item>
        ///     <item>
        ///         <term><c>out TResult</c> result</term> The coerced <seealso cref="TResult"/> value.
        ///     </item>
        /// </list></remarks>
        /// <returns><seealso cref="IFuncTestData3{TOut1, TOut2, TOut3, TResult}">IFuncTestData3</seealso><c>&lt;<seealso cref="IFuncInvocationResult{TResult}">IFuncInvocationResult</seealso>&lt;int&gt;,
        ///     <seealso cref="IInvocationResult{TOut}">IInvocationResult</seealso>&lt;int&gt;,
        ///     int,
        ///     bool&gt;</c>
        /// </returns>
        [Test, Property("Priority", 2), Ignore("Not enough time to work out data source")]
        [TestCaseSource(nameof(GetTryCoerceAsInt4TestCases))]
        public IFuncTestData3<IFuncInvocationResult<int>, IInvocationResult<int>, int, bool> TryCoerceAsInt4Test(object inputObj, Func<int> ifNull)
        {
            InvocationMonitor<int> ifSuccessMonitor = new InvocationMonitor<int>();
            ifNull = ifNull.Monitor(out Func<IFuncInvocationResult<int>> getResult);
            bool returnValue = CoersionHelper.TryCoerceAs(inputObj, ifNull, ifSuccessMonitor.Apply, out int result);
            return new FuncTestData3<IFuncInvocationResult<int>, IInvocationResult<int>, int, bool>(returnValue, getResult(), ifSuccessMonitor.ToResult(), result);
        }

        /// <summary>
        /// Unit test for <seealso cref="CoersionHelper.TryCoerceAs{TResult}(object, bool, TryCoerceHandler{object, TResult}, Action{TResult}, out TResult)"/> using a value type.
        /// </summary>
        /// <param name="inputObj">Value to be coerced.</param>
        /// <param name="returnValueIfNull">The return value of the target method when <paramref name="inputObj"/> is null.</param>
        /// <param name="fallback">The fallback coersion handler that will be invoked if <paramref name="inputObj"/> is not null and cannot be converted.</param>
        /// <remarks>Other parameters used:
        /// <list type="bullet">
        ///     <item>
        ///         <term><seealso cref="Action{TResult}">Action</seealso><c>&lt;int&gt;</c> ifSuccess</term> Callback invoked upon successful coersion.
        ///     </item>
        ///     <item>
        ///         <term><c>out TResult</c> result</term> The coerced <seealso cref="TResult"/> value.
        ///     </item>
        /// </list></remarks>
        /// <returns><seealso cref="IFuncTestData3{TOut1, TOut2, TOut3, TResult}">IFuncTestData3</seealso><c>&lt;<seealso cref="IFuncInvocationResult{TOut, TResult}">&lt;int, bool&gt;</seealso>,
        ///     <seealso cref="IInvocationResult{TOut}">IInvocationResult</seealso>&lt;int&gt;,
        ///     int,
        ///     bool&gt;</c>
        /// </returns>
        [Test, Property("Priority", 2), Ignore("Not enough time to work out data source")]
        [TestCaseSource(nameof(GetTryCoerceAsInt5TestCases))]
        public IFuncTestData3<IFuncInvocationResult<int, bool>, IInvocationResult<int>, int, bool> TryCoerceAsInt5Test(object inputObj, bool returnValueIfNull,
            TryCoerceHandler<object, int> fallback)
        {
            InvocationMonitor<int> ifSuccessMonitor = new InvocationMonitor<int>();
            fallback = fallback.Monitor(out Func<IFuncInvocationResult<int, bool>> getResult);
            bool returnValue = CoersionHelper.TryCoerceAs(inputObj, returnValueIfNull, fallback, ifSuccessMonitor.Apply, out int result);
            return new FuncTestData3<IFuncInvocationResult<int, bool>, IInvocationResult<int>, int, bool>(returnValue, getResult(), ifSuccessMonitor.ToResult(), result);
        }

        /// <summary>
        /// Unit test for <seealso cref="CoersionHelper.TryCoerceAs{TResult}(object, bool, TryCoerceHandler{object, TResult}, Action{TResult}, out TResult)"/> using a reference type.
        /// </summary>
        /// <param name="inputObj">Value to be coerced.</param>
        /// <param name="returnValueIfNull">The return value of the target method when <paramref name="inputObj"/> is null.</param>
        /// <param name="fallback">The fallback coersion handler that will be invoked if <paramref name="inputObj"/> is not null and cannot be converted.</param>
        /// <remarks>Other parameters used:
        /// <list type="bullet">
        ///     <item>
        ///         <term><seealso cref="Action{TResult}">Action</seealso><c>&lt;int&gt;</c> ifSuccess</term> Callback invoked upon successful coersion.
        ///     </item>
        ///     <item>
        ///         <term><c>out TResult</c> result</term> The coerced <seealso cref="TResult"/> value.
        ///     </item>
        /// </list></remarks>
        /// <returns><seealso cref="IFuncTestData3{TOut1, TOut2, TOut3, TResult}">IFuncTestData3</seealso><c>&lt;<seealso cref="IFuncInvocationResult{TOut, TResult}">IFuncInvocationResult</seealso>&lt;<seealso cref="Uri"/>, bool&gt;,
        ///     <seealso cref="IInvocationResult{TOut}">IInvocationResult</seealso>&lt;<seealso cref="Uri"/>&gt;,
        ///     <seealso cref="Uri"/>,
        ///     bool&gt;</c>
        /// </returns>
        [Test, Property("Priority", 2), Ignore("Not enough time to work out data source")]
        [TestCaseSource(nameof(GetTryCoerceAsUri5TestCases))]
        public IFuncTestData3<IFuncInvocationResult<Uri, bool>, IInvocationResult<Uri>, Uri, bool> TryCoerceAsUri5Test(object inputObj, bool returnValueIfNull,
            TryCoerceHandler<object, Uri> fallback)
        {
            InvocationMonitor<Uri> ifSuccessMonitor = new InvocationMonitor<Uri>();
            fallback = fallback.Monitor(out Func<IFuncInvocationResult<Uri, bool>> getResult);
            bool returnValue = CoersionHelper.TryCoerceAs(inputObj, returnValueIfNull, fallback, ifSuccessMonitor.Apply, out Uri result);
            return new FuncTestData3<IFuncInvocationResult<Uri, bool>, IInvocationResult<Uri>, Uri, bool>(returnValue, getResult(), ifSuccessMonitor.ToResult(), result);
        }

        /// <summary>
        /// Unit test for <seealso cref="CoersionHelper.TryCoerceAs{TResult}(object, bool, Action{TResult}, out TResult)"/> using a reference type.
        /// </summary>
        /// <param name="inputObj">Value to be coerced.</param>
        /// <param name="returnValueIfNull">The return value of the target method when <paramref name="inputObj"/> is null.</param>
        /// <remarks>Other parameters used:
        /// <list type="bullet">
        ///     <item>
        ///         <term><seealso cref="Action{TResult}">Action</seealso><c>&lt;int&gt;</c> ifSuccess</term> Callback invoked upon successful coersion.
        ///     </item>
        ///     <item>
        ///         <term><c>out TResult</c> result</term> The coerced <seealso cref="TResult"/> value.
        ///     </item>
        /// </list></remarks>
        /// <returns><seealso cref="IFuncTestData2{TOut1, TOut2, TResult}">IFuncTestData2</seealso><c>&lt;<seealso cref="IFuncInvocationResult{TResult}">IFuncInvocationResult</seealso>&lt;<seealso cref="Uri"/>&gt;, <seealso cref="Uri"/>, bool&gt;</c>
        /// </returns>
        [Test, Property("Priority", 2), Ignore("Not enough time to work out data source")]
        [TestCaseSource(nameof(GetTryCoerceAsUri6TestCases))]
        public IFuncTestData2<IInvocationResult<Uri>, Uri, bool> TryCoerceAsUri6Test(object inputObj, bool returnValueIfNull)
        {
            InvocationMonitor<Uri> ifSuccessMonitor = new InvocationMonitor<Uri>();
            bool returnValue = CoersionHelper.TryCoerceAs(inputObj, returnValueIfNull, ifSuccessMonitor.Apply, out Uri result);
            return new FuncTestData2<IInvocationResult<Uri>, Uri, bool>(returnValue, ifSuccessMonitor.ToResult(), result);
        }

        /// <summary>
        /// Unit test for <seealso cref="CoersionHelper.TryCoerceAs{TResult}(object, bool, Action{TResult}, out TResult)"/> using a value type.
        /// </summary>
        /// <param name="inputObj">Value to be coerced.</param>
        /// <param name="returnValueIfNull">The return value of the target method when <paramref name="inputObj"/> is null.</param>
        /// <remarks>Other parameters used:
        /// <list type="bullet">
        ///     <item>
        ///         <term><seealso cref="Action{TResult}">Action</seealso><c>&lt;int&gt;</c> ifSuccess</term> Callback invoked upon successful coersion.
        ///     </item>
        ///     <item>
        ///         <term><c>out TResult</c> result</term> The coerced <seealso cref="TResult"/> value.
        ///     </item>
        /// </list></remarks>
        /// <returns><seealso cref="IFuncTestData2{TOut1, TOut2, TResult}">IFuncTestData2</seealso><c>&lt;<seealso cref="IFuncInvocationResult{TResult}">IFuncInvocationResult</seealso>&lt;int&gt;, int, bool&gt;</c>
        /// </returns>
        [Test, Property("Priority", 2), Ignore("Not enough time to work out data source")]
        [TestCaseSource(nameof(GetTryCoerceAsInt6TestCases))]
        public IFuncTestData2<IInvocationResult<int>, int, bool> TryCoerceAsInt6Test(object inputObj, bool returnValueIfNull)
        {
            InvocationMonitor<int> ifSuccessMonitor = new InvocationMonitor<int>();
            bool returnValue = CoersionHelper.TryCoerceAs(inputObj, returnValueIfNull, ifSuccessMonitor.Apply, out int result);
            return new FuncTestData2<IInvocationResult<int>, int, bool>(returnValue, ifSuccessMonitor.ToResult(), result);
        }

        /// <summary>
        /// Unit test for <seealso cref="CoersionHelper.TryCoerceAs{TResult}(object, TryCoerceHandler{object, TResult}, Action{TResult}, out TResult)"/> using a reference type.
        /// </summary>
        /// <param name="inputObj">Value to be coerced.</param>
        /// <param name="fallback">The fallback coersion handler that will be invoked if <paramref name="inputObj"/> is not null and cannot be converted.</param>
        /// <remarks>Other parameters used:
        /// <list type="bullet">
        ///     <item>
        ///         <term><seealso cref="Action{TResult}">Action</seealso><c>&lt;int&gt;</c> ifSuccess</term> Callback invoked upon successful coersion.
        ///     </item>
        ///     <item>
        ///         <term><c>out TResult</c> result</term> The coerced <seealso cref="TResult"/> value.
        ///     </item>
        /// </list></remarks>
        /// <returns><seealso cref="IFuncTestData3{TOut1, TOut2, TOut3, TResult}">IFuncTestData3</seealso><c>&lt;<seealso cref="IFuncInvocationResult{TOut, TResult}">IFuncInvocationResult</seealso>&lt;<seealso cref="Uri"/>, bool&gt;,
        ///     <seealso cref="IInvocationResult{TOut}">IInvocationResult</seealso>&lt;<seealso cref="Uri"/>&gt;,
        ///     <seealso cref="Uri"/>,
        ///     bool&gt;</c>
        /// </returns>
        [Test, Property("Priority", 2), Ignore("Not enough time to work out data source")]
        [TestCaseSource(nameof(GetTryCoerceAsUri7TestCases))]
        public IFuncTestData3<IFuncInvocationResult<Uri, bool>, IInvocationResult<Uri>, Uri, bool> TryCoerceAsUri7Test(object inputObj, TryCoerceHandler<object, Uri> fallback)
        {
            InvocationMonitor<Uri> ifSuccessMonitor = new InvocationMonitor<Uri>();
            fallback = fallback.Monitor(out Func<IFuncInvocationResult<Uri, bool>> getResult);
            bool returnValue = CoersionHelper.TryCoerceAs(inputObj, fallback, ifSuccessMonitor.Apply, out Uri result);
            return new FuncTestData3<IFuncInvocationResult<Uri, bool>, IInvocationResult<Uri>, Uri, bool>(returnValue, getResult(), ifSuccessMonitor.ToResult(), result);
        }

        /// <summary>
        /// Unit test for <seealso cref="CoersionHelper.TryCoerceAs{TResult}(object, TryCoerceHandler{object, TResult}, Action{TResult}, out TResult)"/> using a value type.
        /// </summary>
        /// <param name="inputObj">Value to be coerced.</param>
        /// <param name="fallback">The fallback coersion handler that will be invoked if <paramref name="inputObj"/> is not null and cannot be converted.</param>
        /// <remarks>Other parameters used:
        /// <list type="bullet">
        ///     <item>
        ///         <term><seealso cref="Action{TResult}">Action</seealso><c>&lt;int&gt;</c> ifSuccess</term> Callback invoked upon successful coersion.
        ///     </item>
        ///     <item>
        ///         <term><c>out TResult</c> result</term> The coerced <seealso cref="TResult"/> value.
        ///     </item>
        /// </list></remarks>
        /// <returns><seealso cref="IFuncTestData3{TOut1, TOut2, TOut3, TResult}">IFuncTestData3</seealso><c>&lt;<seealso cref="IFuncInvocationResult{TOut, TResult}">&lt;int, bool&gt;</seealso>,
        ///     <seealso cref="IInvocationResult{TOut}">IInvocationResult</seealso>&lt;int&gt;,
        ///     int,
        ///     bool&gt;</c>
        /// </returns>
        [Test, Property("Priority", 2), Ignore("Not enough time to work out data source")]
        [TestCaseSource(nameof(GetTryCoerceAsInt7TestCases))]
        public IFuncTestData3<IFuncInvocationResult<int, bool>, IInvocationResult<int>, int, bool> TryCoerceAsInt7Test(object inputObj, TryCoerceHandler<object, int> fallback)
        {
            InvocationMonitor<int> ifSuccessMonitor = new InvocationMonitor<int>();
            fallback = fallback.Monitor(out Func<IFuncInvocationResult<int, bool>> getResult);
            bool returnValue = CoersionHelper.TryCoerceAs(inputObj, fallback, ifSuccessMonitor.Apply, out int result);
            return new FuncTestData3<IFuncInvocationResult<int, bool>, IInvocationResult<int>, int, bool>(returnValue, getResult(), ifSuccessMonitor.ToResult(), result);
        }

        /// <summary>
        /// Unit test for <seealso cref="CoersionHelper.TryCoerceAs{TResult}(object, Action{TResult}, out TResult)"/> using a reference type.
        /// </summary>
        /// <param name="inputObj">Value to be coerced.</param>
        /// <remarks>Other parameters used:
        /// <list type="bullet">
        ///     <item>
        ///         <term><seealso cref="Action{TResult}">Action</seealso><c>&lt;int&gt;</c> ifSuccess</term> Callback invoked upon successful coersion.
        ///     </item>
        ///     <item>
        ///         <term><c>out TResult</c> result</term> The coerced <seealso cref="TResult"/> value.
        ///     </item>
        /// </list></remarks>
        /// <returns><seealso cref="IFuncTestData2{TOut1, TOut2, TResult}">IFuncTestData2</seealso><c>&lt;<seealso cref="IInvocationResult{TOut}">IInvocationResult</seealso>&lt;<seealso cref="Uri"/>&gt;,
        /// <seealso cref="Uri"/>, bool&gt;</c>
        /// </returns>
        [Test, Property("Priority", 2), Ignore("Not enough time to work out data source")]
        [TestCaseSource(nameof(GetTryCoerceAsUri8TestCases))]
        public IFuncTestData2<IInvocationResult<Uri>, Uri, bool> TryCoerceAsUri8Test(object inputObj)
        {
            InvocationMonitor<Uri> ifSuccessMonitor = new InvocationMonitor<Uri>();
            bool returnValue = CoersionHelper.TryCoerceAs(inputObj, ifSuccessMonitor.Apply, out Uri result);
            return new FuncTestData2<IInvocationResult<Uri>, Uri, bool>(returnValue, ifSuccessMonitor.ToResult(), result);
        }

        /// <summary>
        /// Unit test for <seealso cref="CoersionHelper.TryCoerceAs{TResult}(object, Action{TResult}, out TResult)"/> using a value type.
        /// </summary>
        /// <param name="inputObj">Value to be coerced.</param>
        /// <remarks>Other parameters used:
        /// <list type="bullet">
        ///     <item>
        ///         <term><seealso cref="Action{TResult}">Action</seealso><c>&lt;int&gt;</c> ifSuccess</term> Callback invoked upon successful coersion.
        ///     </item>
        ///     <item>
        ///         <term><c>out TResult</c> result</term> The coerced <seealso cref="TResult"/> value.
        ///     </item>
        /// </list></remarks>
        /// <returns><seealso cref="IFuncTestData2{TOut1, TOut2, TResult}">IFuncTestData2</seealso>&lt;<seealso cref="IInvocationResult{TOut}">IInvocationResult</seealso>&lt;int&gt;, int, bool&gt;</returns>
        [Test, Property("Priority", 2), Ignore("Not enough time to work out data source")]
        [TestCaseSource(nameof(GetTryCoerceAsInt8TestCases))]
        public IFuncTestData2<IInvocationResult<int>, int, bool> TryCoerceAsInt8Test(object inputObj)
        {
            InvocationMonitor<int> ifSuccessMonitor = new InvocationMonitor<int>();
            bool returnValue = CoersionHelper.TryCoerceAs(inputObj, ifSuccessMonitor.Apply, out int result);
            return new FuncTestData2<IInvocationResult<int>, int, bool>(returnValue, ifSuccessMonitor.ToResult(), result);
        }

        /// <summary>
        /// Unit test for <seealso cref="CoersionHelper.TryCoerceAs{TResult}(object, Func{TResult}, bool, TryCoerceHandler{object, TResult}, out TResult)"/> using a reference type.
        /// </summary>
        /// <param name="inputObj">Value to be coerced.</param>
        /// <param name="ifNull">Produces the result value when <paramref name="inputObj"/> is null.</param>
        /// <param name="returnValueIfNull">The return value of the target method when <paramref name="inputObj"/> is null.</param>
        /// <param name="fallback">The fallback coersion handler that will be invoked if <paramref name="inputObj"/> is not null and cannot be converted.</param>
        /// <remarks>Other parameters used:
        /// <list type="bullet">
        ///     <item>
        ///         <term><c>out TResult</c> result</term> The coerced <seealso cref="TResult"/> value.
        ///     </item>
        /// </list></remarks>
        /// <returns><seealso cref="IFuncTestData4{TOut1, TOut2, TOut3, TOut4, TResult}">IFuncTestData4</seealso><c>&lt;<seealso cref="IFuncInvocationResult{TResult}">IFuncInvocationResult</seealso>&lt;<seealso cref="Uri"/>&gt;,
        ///     <seealso cref="IFuncInvocationResult{TOut, TResult}">IFuncInvocationResult</seealso>&lt;<seealso cref="Uri"/>, bool&gt;,
        ///     <seealso cref="IInvocationResult{TOut}">IInvocationResult</seealso>&lt;<seealso cref="Uri"/>&gt;, int, bool&gt;</c></returns>
        [Test, Property("Priority", 2), Ignore("Not enough time to work out data source")]
        [TestCaseSource(nameof(GetTryCoerceAsUri9TestCases))]
        public IFuncTestData3<IFuncInvocationResult<Uri>, IFuncInvocationResult<Uri, bool>, Uri, bool> TryCoerceAsUri9Test(object inputObj, Func<Uri> ifNull, bool returnValueIfNull,
            TryCoerceHandler<object, Uri> fallback)
        {
            fallback = fallback.Monitor(out Func<IFuncInvocationResult<Uri, bool>> getFallBackResult);
            ifNull = ifNull.Monitor(out Func<IFuncInvocationResult<Uri>> getIfNullResult);
            bool returnValue = CoersionHelper.TryCoerceAs(inputObj, ifNull, returnValueIfNull, fallback, out Uri result);
            return new FuncTestData3<IFuncInvocationResult<Uri>, IFuncInvocationResult<Uri, bool>, Uri, bool>(returnValue, getIfNullResult(), getFallBackResult(), result);
        }

        /// <summary>
        /// Unit test for <seealso cref="CoersionHelper.TryCoerceAs{TResult}(object, Func{TResult}, bool, TryCoerceHandler{object, TResult}, out TResult)"/> using a value type.
        /// </summary>
        /// <param name="inputObj">Value to be coerced.</param>
        /// <param name="ifNull">Produces the result value when <paramref name="inputObj"/> is null.</param>
        /// <param name="returnValueIfNull">The return value of the target method when <paramref name="inputObj"/> is null.</param>
        /// <param name="fallback">The fallback coersion handler that will be invoked if <paramref name="inputObj"/> is not null and cannot be converted.</param>
        /// <remarks>Other parameters used:
        /// <list type="bullet">
        ///     <item>
        ///         <term><c>out TResult</c> result</term> The coerced <seealso cref="TResult"/> value.
        ///     </item>
        /// </list></remarks>
        /// <returns><seealso cref="IFuncTestData4{TOut1, TOut2, TOut3, TOut4, TResult}">IFuncTestData4</seealso><c>&lt;<seealso cref="IFuncInvocationResult{TResult}">IFuncInvocationResult</seealso>&lt;int&gt;,
        ///     <seealso cref="IFuncInvocationResult{TOut, TResult}">IFuncInvocationResult</seealso>&lt;int, bool&gt;,
        ///     <seealso cref="IInvocationResult{TOut}">IInvocationResult</seealso>&lt;int&gt;, int, bool&gt;</c>
        /// </returns>
        [Test, Property("Priority", 2), Ignore("Not enough time to work out data source")]
        [TestCaseSource(nameof(GetTryCoerceAsInt9TestCases))]
        public IFuncTestData3<IFuncInvocationResult<int>, IFuncInvocationResult<int, bool>, int, bool> TryCoerceAsInt9Test(object inputObj, Func<int> ifNull, bool returnValueIfNull,
            TryCoerceHandler<object, int> fallback)
        {
            fallback = fallback.Monitor(out Func<IFuncInvocationResult<int, bool>> getFallBackResult);
            ifNull = ifNull.Monitor(out Func<IFuncInvocationResult<int>> getIfNullResult);
            bool returnValue = CoersionHelper.TryCoerceAs(inputObj, ifNull, returnValueIfNull, fallback, out int result);
            return new FuncTestData3<IFuncInvocationResult<int>, IFuncInvocationResult<int, bool>, int, bool>(returnValue, getIfNullResult(), getFallBackResult(), result);
        }

        /// <summary>
        /// Unit test for <seealso cref="CoersionHelper.TryCoerceAs{TResult}(object, Func{TResult}, bool, out TResult)"/> using a reference type.
        /// </summary>
        /// <param name="inputObj">Value to be coerced.</param>
        /// <param name="ifNull">Produces the result value when <paramref name="inputObj"/> is null.</param>
        /// <param name="returnValueIfNull">The return value of the target method when <paramref name="inputObj"/> is null.</param>
        /// <remarks>Other parameters used:
        /// <list type="bullet">
        ///     <item>
        ///         <term><c>out TResult</c> result</term> The coerced <seealso cref="TResult"/> value.
        ///     </item>
        /// </list></remarks>
        /// <returns><seealso cref="IFuncTestData2{TOut1, TOut2, TResult}">IFuncTestData2</seealso><c>&lt;<seealso cref="IFuncInvocationResult{TResult}">IFuncInvocationResult</seealso>&lt;<seealso cref="Uri"/>&gt;,
        ///     <seealso cref="Uri"/>,
        ///     bool&gt;</c>
        /// </returns>
        [Test, Property("Priority", 2), Ignore("Not enough time to work out data source")]
        [TestCaseSource(nameof(GetTryCoerceAsUri10TestCases))]
        public IFuncTestData2<IFuncInvocationResult<Uri>, Uri, bool> TryCoerceAsUri10Test(object inputObj, Func<Uri> ifNull, bool returnValueIfNull)
        {
            ifNull = ifNull.Monitor(out Func<IFuncInvocationResult<Uri>> getIfNullResult);
            bool returnValue = CoersionHelper.TryCoerceAs(inputObj, ifNull, returnValueIfNull, out Uri result);
            return new FuncTestData2<IFuncInvocationResult<Uri>, Uri, bool>(returnValue, getIfNullResult(), result);
        }

        /// <summary>
        /// Unit test for <seealso cref="CoersionHelper.TryCoerceAs{TResult}(object, Func{TResult}, bool, out TResult)"/> using a value type.
        /// </summary>
        /// <param name="inputObj">Value to be coerced.</param>
        /// <param name="ifNull">Produces the result value when <paramref name="inputObj"/> is null.</param>
        /// <param name="returnValueIfNull">The return value of the target method when <paramref name="inputObj"/> is null.</param>
        /// <remarks>Other parameters used:
        /// <list type="bullet">
        ///     <item>
        ///         <term><c>out TResult</c> result</term> The coerced <seealso cref="TResult"/> value.
        ///     </item>
        /// </list></remarks>
        /// <returns><seealso cref="IFuncTestData2{TOut1, TOut2, TResult}">IFuncTestData2</seealso><c>&lt;<seealso cref="IFuncInvocationResult{TResult}">IFuncInvocationResult</seealso>&lt;int&gt;,
        ///     int,
        ///     bool&gt;</c>
        /// </returns>
        [Test, Property("Priority", 2), Ignore("Not enough time to work out data source")]
        [TestCaseSource(nameof(GetTryCoerceAsInt10TestCases))]
        public IFuncTestData2<IFuncInvocationResult<int>, int, bool> TryCoerceAsInt10Test(object inputObj, Func<int> ifNull, bool returnValueIfNull)
        {
            ifNull = ifNull.Monitor(out Func<IFuncInvocationResult<int>> getIfNullResult);
            bool returnValue = CoersionHelper.TryCoerceAs(inputObj, ifNull, returnValueIfNull, out int result);
            return new FuncTestData2<IFuncInvocationResult<int>, int, bool>(returnValue, getIfNullResult(), result);
        }

        /// <summary>
        /// Unit test for <seealso cref="CoersionHelper.TryCoerceAs{TResult}(object, Func{TResult}, TryCoerceHandler{object, TResult}, out TResult)"/> using a reference type.
        /// </summary>
        /// <param name="inputObj">Value to be coerced.</param>
        /// <param name="ifNull">Produces the result value when <paramref name="inputObj"/> is null.</param>
        /// <param name="fallback">The fallback coersion handler that will be invoked if <paramref name="inputObj"/> is not null and cannot be converted.</param>
        /// <remarks>Other parameters used:
        /// <list type="bullet">
        ///     <item>
        ///         <term><c>out TResult</c> result</term> The coerced <seealso cref="TResult"/> value.
        ///     </item>
        /// </list></remarks>
        /// <returns><seealso cref="IFuncTestData3{TOut1, TOut2, TOut3, TResult}">IFuncTestData3</seealso><c>&lt;<seealso cref="IFuncInvocationResult{TOut, TResult}">IFuncInvocationResult</seealso>&lt;<seealso cref="Uri"/>, bool&gt;,
        ///     <seealso cref="IInvocationResult{TOut}">IInvocationResult</seealso>&lt;<seealso cref="Uri"/>&gt;,
        ///     <seealso cref="Uri"/>,
        ///     bool&gt;</c>
        /// </returns>
        [Test, Property("Priority", 2), Ignore("Not enough time to work out data source")]
        [TestCaseSource(nameof(GetTryCoerceAsUri11TestCases))]
        public IFuncTestData3<IFuncInvocationResult<Uri>, IFuncInvocationResult<Uri, bool>, Uri, bool> TryCoerceAsUri11Test(object inputObj, Func<Uri> ifNull,
            TryCoerceHandler<object, Uri> fallback)
        {
            fallback = fallback.Monitor(out Func<IFuncInvocationResult<Uri, bool>> getFallBackResult);
            ifNull = ifNull.Monitor(out Func<IFuncInvocationResult<Uri>> getIfNullResult);
            bool returnValue = CoersionHelper.TryCoerceAs(inputObj, ifNull, fallback, out Uri result);
            return new FuncTestData3<IFuncInvocationResult<Uri>, IFuncInvocationResult<Uri, bool>, Uri, bool>(returnValue, getIfNullResult(), getFallBackResult(), result);
        }

        /// <summary>
        /// Unit test for <seealso cref="CoersionHelper.TryCoerceAs{TResult}(object, Func{TResult}, TryCoerceHandler{object, TResult}, out TResult)"/> using a value type.
        /// </summary>
        /// <param name="inputObj">Value to be coerced.</param>
        /// <param name="ifNull">Produces the result value when <paramref name="inputObj"/> is null.</param>
        /// <param name="fallback">The fallback coersion handler that will be invoked if <paramref name="inputObj"/> is not null and cannot be converted.</param>
        /// <remarks>Other parameters used:
        /// <list type="bullet">
        ///     <item>
        ///         <term><c>out TResult</c> result</term> The coerced <seealso cref="TResult"/> value.
        ///     </item>
        /// </list></remarks>
        /// <returns><seealso cref="IFuncTestData3{TOut1, TOut2, TOut3, TResult}">IFuncTestData3</seealso><c>&lt;<seealso cref="IFuncInvocationResult{TOut, TResult}">&lt;int, bool&gt;</seealso>,
        ///     <seealso cref="IInvocationResult{TOut}">IInvocationResult</seealso>&lt;int&gt;,
        ///     int,
        ///     bool&gt;</c>
        /// </returns>
        [Test, Property("Priority", 2), Ignore("Not enough time to work out data source")]
        [TestCaseSource(nameof(GetTryCoerceAsInt11TestCases))]
        public IFuncTestData3<IFuncInvocationResult<int>, IFuncInvocationResult<int, bool>, int, bool> TryCoerceAsInt11Test(object inputObj, Func<int> ifNull,
            TryCoerceHandler<object, int> fallback)
        {
            fallback = fallback.Monitor(out Func<IFuncInvocationResult<int, bool>> getFallBackResult);
            ifNull = ifNull.Monitor(out Func<IFuncInvocationResult<int>> getIfNullResult);
            bool returnValue = CoersionHelper.TryCoerceAs(inputObj, ifNull, fallback, out int result);
            return new FuncTestData3<IFuncInvocationResult<int>, IFuncInvocationResult<int, bool>, int, bool>(returnValue, getIfNullResult(), getFallBackResult(), result);
        }

        /// <summary>
        /// Unit test for <seealso cref="CoersionHelper.TryCoerceAs{TResult}(object, Func{TResult}, out TResult)"/> using a reference type.
        /// </summary>
        /// <param name="inputObj">Value to be coerced.</param>
        /// <param name="ifNull">Produces the result value when <paramref name="inputObj"/> is null.</param>
        /// <remarks>Other parameters used:
        /// <list type="bullet">
        ///     <item>
        ///         <term><c>out TResult</c> result</term> The coerced <seealso cref="TResult"/> value.
        ///     </item>
        /// </list></remarks>
        /// <returns><seealso cref="IFuncTestData2{TOut1, TOut2, TResult}">IFuncTestData2</seealso><c>&lt;<seealso cref="IFuncInvocationResult{TResult}">IFuncInvocationResult</seealso>&lt;<seealso cref="Uri"/>&gt;, <seealso cref="Uri"/>, bool&gt;</c>
        /// </returns>
        [Test, Property("Priority", 2), Ignore("Not enough time to work out data source")]
        [TestCaseSource(nameof(GetTryCoerceAsUri12TestCases))]
        public IFuncTestData2<IFuncInvocationResult<Uri>, Uri, bool> TryCoerceAsUri12Test(object inputObj, Func<Uri> ifNull)
        {
            ifNull = ifNull.Monitor(out Func<IFuncInvocationResult<Uri>> getResult);
            bool returnValue = CoersionHelper.TryCoerceAs(inputObj, ifNull, out Uri result);
            return new FuncTestData2<IFuncInvocationResult<Uri>, Uri, bool>(returnValue, getResult(), result);
        }

        /// <summary>
        /// Unit test for <seealso cref="CoersionHelper.TryCoerceAs{TResult}(object, Func{TResult}, out TResult)"/> using a value type.
        /// </summary>
        /// <param name="inputObj">Value to be coerced.</param>
        /// <param name="ifNull">Produces the result value when <paramref name="inputObj"/> is null.</param>
        /// <remarks>Other parameters used:
        /// <list type="bullet">
        ///     <item>
        ///         <term><c>out TResult</c> result</term> The coerced <seealso cref="TResult"/> value.
        ///     </item>
        /// </list></remarks>
        /// <returns><seealso cref="IFuncTestData2{TOut1, TOut2, TResult}">IFuncTestData2</seealso><c>&lt;<seealso cref="IFuncInvocationResult{TResult}">IFuncInvocationResult</seealso>&lt;int&gt;, int, bool&gt;</c>
        /// </returns>
        [Test, Property("Priority", 2), Ignore("Not enough time to work out data source")]
        [TestCaseSource(nameof(GetTryCoerceAsInt12TestCases))]
        public IFuncTestData2<IFuncInvocationResult<int>, int, bool> TryCoerceAsInt12Test(object inputObj, Func<int> ifNull)
        {
            ifNull = ifNull.Monitor(out Func<IFuncInvocationResult<int>> getResult);
            bool returnValue = CoersionHelper.TryCoerceAs(inputObj, ifNull, out int result);
            return new FuncTestData2<IFuncInvocationResult<int>, int, bool>(returnValue, getResult(), result);
        }

        /// <summary>
        /// Unit test for <seealso cref="CoersionHelper.TryCoerceAs{TResult}(object, bool, TryCoerceHandler{object, TResult}, out TResult)"/> using a reference type.
        /// </summary>
        /// <param name="inputObj">Value to be coerced.</param>
        /// <param name="returnValueIfNull">The return value of the target method when <paramref name="inputObj"/> is null.</param>
        /// <param name="fallback">The fallback coersion handler that will be invoked if <paramref name="inputObj"/> is not null and cannot be converted.</param>
        /// <remarks>Other parameters used:
        /// <list type="bullet">
        ///     <item>
        ///         <term><c>out TResult</c> result</term> The coerced <seealso cref="TResult"/> value.
        ///     </item>
        /// </list></remarks>
        /// <returns><seealso cref="IFuncTestData2{TOut1, TOut2, TResult}">IFuncTestData2</seealso><c>&lt;<seealso cref="IFuncInvocationResult{TOut, TResult}">IFuncInvocationResult</seealso>&lt;<seealso cref="Uri"/>, bool&gt;, <seealso cref="Uri"/>, bool&gt;</c>
        /// </returns>
        [Test, Property("Priority", 2), Ignore("Not enough time to work out data source")]
        [TestCaseSource(nameof(GetTryCoerceAsUri13TestCases))]
        public IFuncTestData2<IFuncInvocationResult<Uri, bool>, Uri, bool> TryCoerceAsUri13Test(object inputObj, bool returnValueIfNull, TryCoerceHandler<object, Uri> fallback)
        {
            fallback = fallback.Monitor(out Func<IFuncInvocationResult<Uri, bool>> getResult);
            bool returnValue = CoersionHelper.TryCoerceAs(inputObj, returnValueIfNull, fallback, out Uri result);
            return new FuncTestData2<IFuncInvocationResult<Uri, bool>, Uri, bool>(returnValue, getResult(), result);
        }

        /// <summary>
        /// Unit test for <seealso cref="CoersionHelper.TryCoerceAs{TResult}(object, bool, TryCoerceHandler{object, TResult}, out TResult)"/> using a value type.
        /// </summary>
        /// <param name="inputObj">Value to be coerced.</param>
        /// <param name="returnValueIfNull">The return value of the target method when <paramref name="inputObj"/> is null.</param>
        /// <param name="fallback">The fallback coersion handler that will be invoked if <paramref name="inputObj"/> is not null and cannot be converted.</param>
        /// <remarks>Other parameters used:
        /// <list type="bullet">
        ///     <item>
        ///         <term><c>out TResult</c> result</term> The coerced <seealso cref="TResult"/> value.
        ///     </item>
        /// </list></remarks>
        /// <returns><seealso cref="IFuncTestData2{TOut1, TOut2, TResult}">IFuncTestData2</seealso><c>&lt;<seealso cref="IFuncInvocationResult{TOut, TResult}">&lt;int, bool&gt;</seealso>, int, bool&gt;</c>
        /// </returns>
        [Test, Property("Priority", 2), Ignore("Not enough time to work out data source")]
        [TestCaseSource(nameof(GetTryCoerceAsInt13TestCases))]
        public IFuncTestData2<IFuncInvocationResult<int, bool>, int, bool> TryCoerceAsInt13Test(object inputObj, bool returnValueIfNull, TryCoerceHandler<object, int> fallback)
        {
            fallback = fallback.Monitor(out Func<IFuncInvocationResult<int, bool>> getResult);
            bool returnValue = CoersionHelper.TryCoerceAs(inputObj, returnValueIfNull, fallback, out int result);
            return new FuncTestData2<IFuncInvocationResult<int, bool>, int, bool>(returnValue, getResult(), result);
        }

        /// <summary>
        /// Unit test for <seealso cref="CoersionHelper.TryCoerceAs{TResult}(object, bool, out TResult)"/> using a reference type.
        /// </summary>
        /// <param name="inputObj">Value to be coerced.</param>
        /// <param name="returnValueIfNull">The return value of the target method when <paramref name="inputObj"/> is null.</param>
        /// <remarks>Other parameters used:
        /// <list type="bullet">
        ///     <item>
        ///         <term><c>out TResult</c> result</term> The coerced <seealso cref="TResult"/> value.
        ///     </item>
        /// </list></remarks>
        /// <returns><seealso cref="IFuncTestData1{TOut, TResult}">IFuncTestData1</seealso>&lt;<seealso cref="Uri"/>, bool&gt;</returns>
        [Test, Property("Priority", 2), Ignore("Not enough time to work out data source")]
        [TestCaseSource(nameof(GetTryCoerceAsUri14TestCases))]
        public IFuncTestData1<Uri, bool> TryCoerceAsUri14Test(object inputObj, bool returnValueIfNull)
        {
            bool returnValue = CoersionHelper.TryCoerceAs(inputObj, returnValueIfNull, out Uri result);
            return new FuncTestData1<Uri, bool>(returnValue, result);
        }

        /// <summary>
        /// Unit test for <seealso cref="CoersionHelper.TryCoerceAs{TResult}(object, bool, out TResult)"/> using a value type.
        /// </summary>
        /// <param name="inputObj">Value to be coerced.</param>
        /// <param name="returnValueIfNull">The return value of the target method when <paramref name="inputObj"/> is null.</param>
        /// <remarks>Other parameters used:
        /// <list type="bullet">
        ///     <item>
        ///         <term><c>out TResult</c> result</term> The coerced <seealso cref="TResult"/> value.
        ///     </item>
        /// </list></remarks>
        /// <returns><seealso cref="IFuncTestData1{TOut, TResult}">IFuncTestData1</seealso>&lt;int, bool&gt;</returns>
        [Test, Property("Priority", 2), Ignore("Not enough time to work out data source")]
        [TestCaseSource(nameof(GetTryCoerceAsInt14TestCases))]
        public IFuncTestData1<int, bool> TryCoerceAsInt14Test(object inputObj, bool returnValueIfNull)
        {
            bool returnValue = CoersionHelper.TryCoerceAs(inputObj, returnValueIfNull, out int result);
            return new FuncTestData1<int, bool>(returnValue, result);
        }

        /// <summary>
        /// Unit test for <seealso cref="CoersionHelper.TryCoerceAs{TResult}(object, TryCoerceHandler{object, TResult}, out TResult)"/> using a reference type.
        /// </summary>
        /// <param name="inputObj">Value to be coerced.</param>
        /// <param name="fallback">The fallback coersion handler that will be invoked if <paramref name="inputObj"/> is not null and cannot be converted.</param>
        /// <remarks>Other parameters used:
        /// <list type="bullet">
        ///     <item>
        ///         <term><c>out TResult</c> result</term> The coerced <seealso cref="TResult"/> value.
        ///     </item>
        /// </list></remarks>
        /// <returns><seealso cref="IFuncTestData2{TOut1, TOut2, TResult}">IFuncTestData2</seealso><c>&lt;<seealso cref="IFuncInvocationResult{TOut, TResult}">IFuncInvocationResult</seealso>&lt;<seealso cref="Uri"/>, bool&gt;,
        /// <seealso cref="Uri"/>, bool&gt;</c>
        /// </returns>
        [Test, Property("Priority", 2), Ignore("Not enough time to work out data source")]
        [TestCaseSource(nameof(GetTryCoerceAsUri15TestCases))]
        public IFuncTestData2<IFuncInvocationResult<Uri, bool>, Uri, bool> TryCoerceAsUri15Test(object inputObj, TryCoerceHandler<object, Uri> fallback)
        {
            fallback = fallback.Monitor(out Func<IFuncInvocationResult<Uri, bool>> getResult);
            bool returnValue = CoersionHelper.TryCoerceAs(inputObj, fallback, out Uri result);
            return new FuncTestData2<IFuncInvocationResult<Uri, bool>, Uri, bool>(returnValue, getResult(), result);
        }

        /// <summary>
        /// Unit test for <seealso cref="CoersionHelper.TryCoerceAs{TResult}(object, TryCoerceHandler{object, TResult}, out TResult)"/> using a value type.
        /// </summary>
        /// <param name="inputObj">Value to be coerced.</param>
        /// <param name="fallback">The fallback coersion handler that will be invoked if <paramref name="inputObj"/> is not null and cannot be converted.</param>
        /// <remarks>Other parameters used:
        /// <list type="bullet">
        ///     <item>
        ///         <term><c>out TResult</c> result</term> The coerced <seealso cref="TResult"/> value.
        ///     </item>
        /// </list></remarks>
        /// <returns><seealso cref="IFuncTestData2{TOut1, TOut2, TResult}">IFuncTestData2</seealso><c>&lt;<seealso cref="IFuncInvocationResult{TOut, TResult}">&lt;int, bool&gt;</seealso>,
        /// int, bool&gt;</c>
        /// </returns>
        [Test, Property("Priority", 2), Ignore("Not enough time to work out data source")]
        [TestCaseSource(nameof(GetTryCoerceAsInt15TestCases))]
        public IFuncTestData2<IFuncInvocationResult<int, bool>, int, bool> TryCoerceAsInt15Test(object inputObj, TryCoerceHandler<object, int> fallback)
        {
            fallback = fallback.Monitor(out Func<IFuncInvocationResult<int, bool>> getResult);
            bool returnValue = CoersionHelper.TryCoerceAs(inputObj, fallback, out int result);
            return new FuncTestData2<IFuncInvocationResult<int, bool>, int, bool>(returnValue, getResult(), result);
        }

        /// <summary>
        /// Unit test for <seealso cref="CoersionHelper.TryCoerceAs{TResult}(object, out TResult)"/> with a reference type.
        /// </summary>
        /// <param name="inputObj">Value to be coerced.</param>
        /// <remarks>Other parameters used:
        /// <list type="bullet">
        ///     <item>
        ///         <term><c>out TResult</c> result</term> The coerced <seealso cref="TResult"/> value.
        ///     </item>
        /// </list></remarks>
        /// <returns><seealso cref="IFuncTestData1{TOut, TResult}">IFuncTestData1</seealso>&lt;<seealso cref="Uri"/>, bool&gt;</returns>
        [Test, Property("Priority", 2), Ignore("Not enough time to work out data source")]
        [TestCaseSource(nameof(GetTryCoerceAsUri16TestCases))]
        public IFuncTestData1<Uri, bool> TryCoerceAsUri16Test(object inputObj)
        {
            bool returnValue = CoersionHelper.TryCoerceAs(inputObj, out Uri result);
            return new FuncTestData1<Uri, bool>(returnValue, result);
        }

        /// <summary>
        /// Unit test for <seealso cref="CoersionHelper.TryCoerceAs{TResult}(object, out TResult)"/> using a value type.
        /// </summary>
        /// <param name="inputObj">Value to be coerced.</param>
        /// <remarks>Other parameters used:
        /// <list type="bullet">
        ///     <item>
        ///         <term><c>out TResult</c> result</term> The coerced <seealso cref="TResult"/> value.
        ///     </item>
        /// </list></remarks>
        /// <returns><seealso cref="IFuncTestData1{TOut, TResult}">IFuncTestData1</seealso>&lt;int, bool&gt;</returns>
        [Test, Property("Priority", 1), Ignore("Not enough time to work out data source")]
        [TestCaseSource(nameof(GetTryCoerceAsInt16TestCases))]
        public IFuncTestData1<int, bool> TryCoerceAsInt16Test(object inputObj)
        {
            bool returnValue = CoersionHelper.TryCoerceAs(inputObj, out int result);
            return new FuncTestData1<int, bool>(returnValue, result);
        }
    }
}
