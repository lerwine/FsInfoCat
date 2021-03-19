using System;

namespace FsInfoCat.Test.Helpers
{
    /// <summary>
    /// Base data object interface for test operations that has a return value.
    /// </summary>
    public interface IFuncTestData : IDelegateTestData
    {
        /// <summary>
        /// The first test operation return value.
        /// </summary>
        object ReturnValue { get; }
    }

    /// <summary>
    /// Defines test data for operations that return a value.
    /// </summary>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="Func{TResult}"/> delegate.</remarks>
    public interface IFuncTestData<TResult> : IFuncTestData
    {
        /// <summary>
        /// The first test operation return value.
        /// </summary>
        new TResult ReturnValue { get; }
    }

    /// <summary>
    /// Defines test data for operations that include at least 1 input value and return a value.
    /// </summary>
    /// <typeparam name="TParam">The type of the parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="Func{TParam, TResult}"/> delegate.</remarks>
    public interface IFuncTestData<TParam, TResult> : IFuncTestData<TResult>, IDelegateTestData<TParam> { }

    /// <summary>
    /// Defines test data for operations that include at least 2 input values and return a value.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="Func{TParam1, TParam2, TResult}"/> delegate.</remarks>
    public interface IFuncTestData<TParam1, TParam2, TResult> : IFuncTestData<TParam1, TResult>, IDelegateTestData<TParam1, TParam2> { }

    /// <summary>
    /// Defines test data for operations that include at least 3 input values and return a value.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third input value or parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="Func{TParam1, TParam2, TParam3, TResult}"/> delegate.</remarks>
    public interface IFuncTestData<TParam1, TParam2, TParam3, TResult> : IFuncTestData<TParam1, TParam2, TResult>, IDelegateTestData<TParam1, TParam2, TParam3> { }

    /// <summary>
    /// Defines test data for operations that include at least 4 input values and return a value.
    /// </summary>~
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third input value or parameter.</typeparam>
    /// <typeparam name="TParam4">The type of the fourth input value or parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="Func{TParam1, TParam2, TParam3, TParam4, TResult}"/> delegate.</remarks>
    public interface IFuncTestData<TParam1, TParam2, TParam3, TParam4, TResult> : IFuncTestData<TParam1, TParam2, TParam3, TResult>,
        IDelegateTestData<TParam1, TParam2, TParam3, TParam4>
    { }

    /// <summary>
    /// Defines test data for operations that include at least 5 input values and return a value.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third input value or parameter.</typeparam>
    /// <typeparam name="TParam4">The type of the fourth input value or parameter.</typeparam>
    /// <typeparam name="TParam5">The type of the fifth input value or parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="Func{TParam1, TParam2, TParam3, TParam4, TParam5, TResult}"/> delegate.</remarks>
    public interface IFuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TResult> : IFuncTestData<TParam1, TParam2, TParam3, TParam4, TResult>,
        IDelegateTestData<TParam1, TParam2, TParam3, TParam4, TParam5>
    { }

    /// <summary>
    /// Defines test data for operations that include at least 6 input values and return a value.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third input value or parameter.</typeparam>
    /// <typeparam name="TParam4">The type of the fourth input value or parameter.</typeparam>
    /// <typeparam name="TParam5">The type of the fifth input value or parameter.</typeparam>
    /// <typeparam name="TParam6">The type of the sixth input value or parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="Func{TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TResult}"/>
    /// delegate.</remarks>
    public interface IFuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TResult> : IFuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TResult>,
        IDelegateTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6>
    { }

    /// <summary>
    /// Defines test data for operations that include at least 7 input values and return a value.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third input value or parameter.</typeparam>
    /// <typeparam name="TParam4">The type of the fourth input value or parameter.</typeparam>
    /// <typeparam name="TParam5">The type of the fifth input value or parameter.</typeparam>
    /// <typeparam name="TParam6">The type of the sixth input value or parameter.</typeparam>
    /// <typeparam name="TParam7">The type of the seventh input value or parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="Func{TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TResult}"/>
    /// delegate.</remarks>
    public interface IFuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TResult> :
        IFuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TResult>, IDelegateTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7>
    { }

    /// <summary>
    /// Defines test data for operations that return a value and include at least 1 additional output value as well.
    /// </summary>
    /// <typeparam name="TOut">The type of the output parameter or additional output value.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="FuncWithOutput{TOut, TResult}"/> delegate.</remarks>
    public interface IFuncTestData1<TOut, TResult> : IFuncTestData<TResult>, IDelegateTestDataOut1<TOut> { }

    /// <summary>
    /// Defines test data for operations that include at least 1 input value and return a value as well as 1 additional output value.
    /// </summary>
    /// <typeparam name="TParam">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TOut">The type of the output parameter or additional output value.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="FuncWithOutput{TParam, TOut, TResult}"/> delegate.</remarks>
    public interface IFuncTestData1<TParam, TOut, TResult> : IFuncTestData1<TOut, TResult>, IFuncTestData<TParam, TResult>, IDelegateTestDataOut1<TParam, TOut> { }

    /// <summary>
    /// Defines test data for operations that include at least 2 input values and return a value as well as 1 additional output value.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TOut">The type of the output parameter or additional output value.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="FuncWithOutput{TParam1, TParam2, TOut, TResult}"/> delegate.</remarks>
    public interface IFuncTestData1<TParam1, TParam2, TOut, TResult> : IFuncTestData1<TParam1, TOut, TResult>, IFuncTestData<TParam1, TParam2, TResult>,
        IDelegateTestDataOut1<TParam1, TParam2, TOut>
    { }

    /// <summary>
    /// Defines test data for operations that include at least 3 input values and return a value as well as 1 additional output value.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third input value or parameter.</typeparam>
    /// <typeparam name="TOut">The type of the output parameter or additional output value.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="FuncWithOutput{TParam1, TParam2, TParam3, TOut, TResult}"/> delegate.</remarks>
    public interface IFuncTestData1<TParam1, TParam2, TParam3, TOut, TResult> : IFuncTestData1<TParam1, TParam2, TOut, TResult>,
        IFuncTestData<TParam1, TParam2, TParam3, TResult>, IDelegateTestDataOut1<TParam1, TParam2, TParam3, TOut>
    { }

    /// <summary>
    /// Defines test data for operations that include at least 4 input values and return a value as well as 1 additional output value.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third input value or parameter.</typeparam>
    /// <typeparam name="TParam4">The type of the fourth input value or parameter.</typeparam>
    /// <typeparam name="TOut">The type of the output parameter or additional output value.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="FuncWithOutput{TParam1, TParam2, TParam3, TParam4, TOut, TResult}"/>
    /// delegate.</remarks>
    public interface IFuncTestData1<TParam1, TParam2, TParam3, TParam4, TOut, TResult> : IFuncTestData1<TParam1, TParam2, TParam3, TOut, TResult>,
        IFuncTestData<TParam1, TParam2, TParam3, TParam4, TResult>, IDelegateTestDataOut1<TParam1, TParam2, TParam3, TParam4, TOut>
    { }

    /// <summary>
    /// Defines test data for operations that include at least 5 input values and return a value as well as 1 additional output value.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third input value or parameter.</typeparam>
    /// <typeparam name="TParam4">The type of the fourth input value or parameter.</typeparam>
    /// <typeparam name="TParam5">The type of the fifth input value or parameter.</typeparam>
    /// <typeparam name="TOut">The type of the output parameter or additional output value.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="FuncWithOutput{TParam1, TParam2, TParam3, TParam4, TParam5, TOut, TResult}"/>
    /// delegate.</remarks>
    public interface IFuncTestData1<TParam1, TParam2, TParam3, TParam4, TParam5, TOut, TResult> : IFuncTestData1<TParam1, TParam2, TParam3, TParam4, TOut, TResult>,
        IFuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TResult>, IDelegateTestDataOut1<TParam1, TParam2, TParam3, TParam4, TParam5, TOut>
    { }

    /// <summary>
    /// Defines test data for operations that include at least 6 input values and return a value as well as 1 additional output value.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third input value or parameter.</typeparam>
    /// <typeparam name="TParam4">The type of the fourth input value or parameter.</typeparam>
    /// <typeparam name="TParam5">The type of the fifth input value or parameter.</typeparam>
    /// <typeparam name="TParam6">The type of the sixth input value or parameter.</typeparam>
    /// <typeparam name="TOut">The type of the output parameter or additional output value.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the
    /// <seealso cref="FuncWithOutput{TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TOut, TResult}"/> delegate.</remarks>
    public interface IFuncTestData1<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TOut, TResult> :
        IFuncTestData1<TParam1, TParam2, TParam3, TParam4, TParam5, TOut, TResult>, IFuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TResult>,
        IDelegateTestDataOut1<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TOut>
    { }

    /// <summary>
    /// Defines test data for operations that return a value and include at least 2 additional output values as well.
    /// </summary>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="FuncWithOutput2{TOut1, TOut2, TResult}"/> delegate.</remarks>
    public interface IFuncTestData2<TOut1, TOut2, TResult> : IFuncTestData1<TOut1, TResult>, ITestOutputData<TOut1, TOut2>, IDelegateTestDataOut2<TOut1, TOut2> { }

    /// <summary>
    /// Defines test data for operations that include at least 1 input value and return a value as well as at least 2 additional output values.
    /// </summary>
    /// <typeparam name="TParam">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="FuncWithOutput2{TParam, TOut1, TOut2, TResult}"/> delegate.</remarks>
    public interface IFuncTestData2<TParam, TOut1, TOut2, TResult> : IFuncTestData2<TOut1, TOut2, TResult>, IFuncTestData1<TParam, TOut1, TResult>,
        IDelegateTestDataOut2<TParam, TOut1, TOut2>
    { }

    /// <summary>
    /// Defines test data for operations that include at least 2 input values and return a value as well as at least 2 additional output values.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="FuncWithOutput2{TParam1, TParam2, TOut1, TOut2, TResult}"/> delegate.</remarks>
    public interface IFuncTestData2<TParam1, TParam2, TOut1, TOut2, TResult> : IFuncTestData2<TParam1, TOut1, TOut2, TResult>,
        IFuncTestData1<TParam1, TParam2, TOut1, TResult>, IDelegateTestDataOut2<TParam1, TParam2, TOut1, TOut2>
    { }

    /// <summary>
    /// Defines test data for operations that include at least 3 input values and return a value as well as at least 2 additional output values.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third input value or parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="FuncWithOutput2{TParam1, TParam2, TParam3, TOut1, TOut2, TResult}"/>
    /// delegate.</remarks>
    public interface IFuncTestData2<TParam1, TParam2, TParam3, TOut1, TOut2, TResult> : IFuncTestData2<TParam1, TParam2, TOut1, TOut2, TResult>,
        IFuncTestData1<TParam1, TParam2, TParam3, TOut1, TResult>, IDelegateTestDataOut2<TParam1, TParam2, TParam3, TOut1, TOut2>
    { }

    /// <summary>
    /// Defines test data for operations that include at least 4 input values and return a value as well as at least 2 additional output values.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third input value or parameter.</typeparam>
    /// <typeparam name="TParam4">The type of the fourth input value or parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="FuncWithOutput2{TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TResult}"/
    /// delegate.</remarks>
    public interface IFuncTestData2<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TResult> : IFuncTestData2<TParam1, TParam2, TParam3, TOut1, TOut2, TResult>,
        IFuncTestData1<TParam1, TParam2, TParam3, TParam4, TOut1, TResult>, IDelegateTestDataOut2<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2>
    { }

    /// <summary>
    /// Defines test data for operations that include at least 5 input values and return a value as well as at least 2 additional output values.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third input value or parameter.</typeparam>
    /// <typeparam name="TParam4">The type of the fourth input value or parameter.</typeparam>
    /// <typeparam name="TParam5">The type of the fifth input value or parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the
    /// <seealso cref="FuncWithOutput2{TParam1, TParam2, TParam3, TParam4, TParam5, TOut1, TOut2, TResult}"/> delegate.</remarks>
    public interface IFuncTestData2<TParam1, TParam2, TParam3, TParam4, TParam5, TOut1, TOut2, TResult> :
        IFuncTestData2<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TResult>, IFuncTestData1<TParam1, TParam2, TParam3, TParam4, TParam5, TOut1, TResult>,
        IDelegateTestDataOut2<TParam1, TParam2, TParam3, TParam4, TParam5, TOut1, TOut2>
    { }

    /// <summary>
    /// Defines test data for operations that return a value and at least 3 additional output values as well.
    /// </summary>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter or additional output value.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="FuncWithOutput3{TOut1, TOut2, TOut3, TResult}"/> delegate.</remarks>
    public interface IFuncTestData3<TOut1, TOut2, TOut3, TResult> : IFuncTestData2<TOut1, TOut2, TResult>, ITestOutputData<TOut1, TOut2, TOut3>,
        IDelegateTestDataOut3<TOut1, TOut2, TOut3>
    { }

    /// <summary>
    /// Defines test data for operations that include at least 1 input value and return a value as well as at least 3 additional output values.
    /// </summary>
    /// <typeparam name="TParam">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter or additional output value.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="FuncWithOutput3{TParam, TOut1, TOut2, TOut3, TResult}"/> delegate.</remarks>
    public interface IFuncTestData3<TParam, TOut1, TOut2, TOut3, TResult> : IFuncTestData3<TOut1, TOut2, TOut3, TResult>, IFuncTestData2<TParam, TOut1, TOut2, TResult>,
        IDelegateTestDataOut3<TParam, TOut1, TOut2, TOut3>
    { }

    /// <summary>
    /// Defines test data for operations that include at least 2 input values and return a value as well as at least 3 additional output values.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter or additional output value.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="FuncWithOutput3{TParam1, TParam2, TOut1, TOut2, TOut3, TResult}"/>
    /// delegate.</remarks>
    public interface IFuncTestData3<TParam1, TParam2, TOut1, TOut2, TOut3, TResult> : IFuncTestData3<TParam1, TOut1, TOut2, TOut3, TResult>,
        IFuncTestData2<TParam1, TParam2, TOut1, TOut2, TResult>, IDelegateTestDataOut3<TParam1, TParam2, TOut1, TOut2, TOut3>
    { }

    /// <summary>
    /// Defines test data for operations that include at least 3 input values and return a value as well as at least 3 additional output values.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third input value or parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter or additional output value.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="FuncWithOutput3{TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TResult}"/>
    /// delegate.</remarks>
    public interface IFuncTestData3<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TResult> : IFuncTestData3<TParam1, TParam2, TOut1, TOut2, TOut3, TResult>,
        IFuncTestData2<TParam1, TParam2, TParam3, TOut1, TOut2, TResult>, IDelegateTestDataOut3<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3>
    { }

    /// <summary>
    /// Defines test data for operations that include at least 4 input values and return a value as well as at least 3 additional output values.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third input value or parameter.</typeparam>
    /// <typeparam name="TParam4">The type of the fourth input value or parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter or additional output value.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the
    /// <seealso cref="FuncWithOutput3{TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TOut3, TResult}"/> delegate.</remarks>
    public interface IFuncTestData3<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TOut3, TResult> :
        IFuncTestData3<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TResult>, IFuncTestData2<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TResult>,
        IDelegateTestDataOut3<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TOut3>
    { }

    /// <summary>
    /// Defines test data for operations that return a value and include at least 4 additional output values as well.
    /// </summary>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth output parameter or additional output value.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="FuncWithOutput4{TOut1, TOut2, TOut3, TOut4, TResult}"/> delegate.</remarks>
    public interface IFuncTestData4<TOut1, TOut2, TOut3, TOut4, TResult> : IFuncTestData3<TOut1, TOut2, TOut3, TResult>, ITestOutputData<TOut1, TOut2, TOut3, TOut4>,
        IDelegateTestDataOut4<TOut1, TOut2, TOut3, TOut4>
    { }

    /// <summary>
    /// Defines test data for operations that include at least 1 input value and return a value as well as at least 4 additional output values.
    /// </summary>
    /// <typeparam name="TParam">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth output parameter or additional output value.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="FuncWithOutput4{TParam, TOut1, TOut2, TOut3, TOut4, TResult}"/>
    /// delegate.</remarks>
    public interface IFuncTestData4<TParam, TOut1, TOut2, TOut3, TOut4, TResult> : IFuncTestData4<TOut1, TOut2, TOut3, TOut4, TResult>,
        IFuncTestData3<TParam, TOut1, TOut2, TOut3, TResult>, IDelegateTestDataOut4<TParam, TOut1, TOut2, TOut3, TOut4>
    { }

    /// <summary>
    /// Defines test data for operations that include at least 2 input values and return a value as well as at least 4 additional output values.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth output parameter or additional output value.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="FuncWithOutput4{TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TResult}"/>
    /// delegate.</remarks>
    public interface IFuncTestData4<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TResult> : IFuncTestData4<TParam1, TOut1, TOut2, TOut3, TOut4, TResult>,
        IFuncTestData3<TParam1, TParam2, TOut1, TOut2, TOut3, TResult>, IDelegateTestDataOut4<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4>
    { }

    /// <summary>
    /// Defines test data for operations that include at least 3 input values and return a value as well as at least 4 additional output values.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third input value or parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth output parameter or additional output value.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the
    /// <seealso cref="FuncWithOutput4{TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TOut4, TResult}"/> delegate.</remarks>
    public interface IFuncTestData4<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TOut4, TResult> : IFuncTestData4<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TResult>,
        IFuncTestData3<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TResult>, IDelegateTestDataOut4<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TOut4>
    { }

    /// <summary>
    /// Defines test data for operations that return a value and include at least 5 additional output values as well.
    /// </summary>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut5">The type of the fifth output parameter or additional output value.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="FuncWithOutput5{TOut1, TOut2, TOut3, TOut4, TOut5, TResult}"/>
    /// delegate.</remarks>
    public interface IFuncTestData5<TOut1, TOut2, TOut3, TOut4, TOut5, TResult> : IFuncTestData4<TOut1, TOut2, TOut3, TOut4, TResult>,
        ITestOutputData<TOut1, TOut2, TOut3, TOut4, TOut5>, IDelegateTestDataOut5<TOut1, TOut2, TOut3, TOut4, TOut5>
    { }

    /// <summary>
    /// Defines test data for operations that include at least 1 input value and return a value as well as at least 5 additional output values.
    /// </summary>
    /// <typeparam name="TParam">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut5">The type of the fifth output parameter or additional output value.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="FuncWithOutput5{TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TResult}"/>
    /// delegate.</remarks>
    public interface IFuncTestData5<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TResult> : IFuncTestData5<TOut1, TOut2, TOut3, TOut4, TOut5, TResult>,
        IFuncTestData4<TParam, TOut1, TOut2, TOut3, TOut4, TResult>, IDelegateTestDataOut5<TParam, TOut1, TOut2, TOut3, TOut4, TOut5>
    { }

    /// <summary>
    /// Defines test data for operations that include at least 2 input values and return a value as well as at least 5 additional output values.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut5">The type of the fifth output parameter or additional output value.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="FuncWithOutput5{TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TOut5, TResult}"/>
    /// delegate.</remarks>
    public interface IFuncTestData5<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TOut5, TResult> : IFuncTestData5<TParam1, TOut1, TOut2, TOut3, TOut4, TOut5, TResult>,
        IFuncTestData4<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TResult>, IDelegateTestDataOut5<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TOut5>
    { }

    /// <summary>
    /// Defines test data for operations that return a value and include at least 6 additional output values as well.
    /// </summary>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut5">The type of the fifth output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut6">The type of the sixth output parameter or additional output value.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="FuncWithOutput6{TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult}"/>
    /// delegate.</remarks>
    public interface IFuncTestData6<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult> : IFuncTestData5<TOut1, TOut2, TOut3, TOut4, TOut5, TResult>,
        ITestOutputData<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>, IDelegateTestDataOut6<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>
    { }

    /// <summary>
    /// Defines test data for operations that include at least 1 input value and return a value as well as at least 6 additional output values.
    /// </summary>
    /// <typeparam name="TParam">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut5">The type of the fifth output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut6">The type of the sixth output parameter or additional output value.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="FuncWithOutput6{TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult}"/>
    /// delegate.</remarks>
    public interface IFuncTestData6<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult> : IFuncTestData6<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult>,
        IFuncTestData5<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TResult>, IDelegateTestDataOut6<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>
    { }

    /// <summary>
    /// Defines test data for operations that return a value and include at least 7 additional output values as well.
    /// </summary>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut5">The type of the fifth output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut6">The type of the sixth output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut7">The type of the seventh output parameter or additional output value.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="FuncWithOutput7{TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult}"/>
    /// delegate.</remarks>
    public interface IFuncTestData7<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult> : IFuncTestData6<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult>,
        ITestOutputData<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>, IDelegateTestDataOut7<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>
    { }
}
