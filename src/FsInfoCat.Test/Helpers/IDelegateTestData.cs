using System;

namespace FsInfoCat.Test.Helpers
{
    /// <summary>
    /// Base data object interface for test operations that may have zero or more input values.
    /// </summary>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="Action"/> delegate.</remarks>
    public interface IDelegateTestData : ITestInputData, ITestOutputData { }

    /// <summary>
    /// Defines test data for operations include at least 1 input value.
    /// </summary>
    /// <typeparam name="TParam">The type of the parameter.</typeparam>
    /// <remarks>Can be used with invocations that match the (ie. <seealso cref="Action{TParam}"/> delegate.</remarks>
    public interface IDelegateTestData<TParam> : IDelegateTestData, ITestInputData<TParam> { }

    /// <summary>
    /// Defines test data for operations that include at least 2 input values.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="Action{TParam1, TParam2}"/> delegate.</remarks>
    public interface IDelegateTestData<TParam1, TParam2> : IDelegateTestData<TParam1>, ITestInputData<TParam1, TParam2> { }

    /// <summary>
    /// Defines test data for operations that include at least 3 input values.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third input value or parameter.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="Action{TParam1, TParam2, TParam3}"/> delegate.</remarks>
    public interface IDelegateTestData<TParam1, TParam2, TParam3> : IDelegateTestData<TParam1, TParam2>, ITestInputData<TParam1, TParam2, TParam3> { }

    /// <summary>
    /// Defines test data for operations that include at least 4 input values.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third input value or parameter.</typeparam>
    /// <typeparam name="TParam4">The type of the fourth input value or parameter.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="Action{TParam1, TParam2, TParam3, TParam4}"/> delegate.</remarks>
    public interface IDelegateTestData<TParam1, TParam2, TParam3, TParam4> : IDelegateTestData<TParam1, TParam2, TParam3>,
        ITestInputData<TParam1, TParam2, TParam3, TParam4> { }

    /// <summary>
    /// Defines test data for operations that include at least 5 input values.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third input value or parameter.</typeparam>
    /// <typeparam name="TParam4">The type of the fourth input value or parameter.</typeparam>
    /// <typeparam name="TParam5">The type of the fifth input value or parameter.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="Action{TParam1, TParam2, TParam3, TParam4, TParam5}"/> delegate.</remarks>
    public interface IDelegateTestData<TParam1, TParam2, TParam3, TParam4, TParam5> : IDelegateTestData<TParam1, TParam2, TParam3, TParam4>,
        ITestInputData<TParam1, TParam2, TParam3, TParam4, TParam5> { }

    /// <summary>
    /// Defines test data for operations that include at least 6 input values.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third input value or parameter.</typeparam>
    /// <typeparam name="TParam4">The type of the fourth input value or parameter.</typeparam>
    /// <typeparam name="TParam5">The type of the fifth input value or parameter.</typeparam>
    /// <typeparam name="TParam6">The type of the sixth input value or parameter.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="Action{TParam1, TParam2, TParam3, TParam4, TParam5, TParam6}"/>
    /// delegate.</remarks>
    public interface IDelegateTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6> : IDelegateTestData<TParam1, TParam2, TParam3, TParam4, TParam5>,
        ITestInputData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6> { }

    /// <summary>
    /// Defines test data for operations that include at least 7 input values.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third input value or parameter.</typeparam>
    /// <typeparam name="TParam4">The type of the fourth input value or parameter.</typeparam>
    /// <typeparam name="TParam5">The type of the fifth input value or parameter.</typeparam>
    /// <typeparam name="TParam6">The type of the sixth input value or parameter.</typeparam>
    /// <typeparam name="TParam7">The type of the seventh input value or parameter.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="Action{TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7}"/>
    /// delegate.</remarks>
    public interface IDelegateTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7> :
        IDelegateTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6>, ITestInputData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7> { }

    /// <summary>
    /// Defines test data for operations that produce at least 1 output value.
    /// </summary>
    /// <typeparam name="TOut">The type of the output parameter or additional output value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="ActionWithOutput{TOut}"/> delegate.</remarks>
    public interface IDelegateTestDataOut1<TOut> : IDelegateTestData, ITestOutputData<TOut> { }

    /// <summary>
    /// Defines test data for operations that include at least 1 input value and produce at least 1 output value.
    /// </summary>
    /// <typeparam name="TParam">The type of the parameter.</typeparam>
    /// <typeparam name="TOut">The type of the output parameter or additional output value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="ActionWithOutput{TParam, TOut}"/> delegate.</remarks>
    public interface IDelegateTestDataOut1<TParam, TOut> : IDelegateTestDataOut1<TOut>, IDelegateTestData<TParam> { }

    /// <summary>
    /// Defines test data for operations that include at least 2 input values and produce at least 1 output value.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TOut">The type of the output parameter or additional output value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="ActionWithOutput{TParam1, TParam2, TOut}"/> delegate.</remarks>
    public interface IDelegateTestDataOut1<TParam1, TParam2, TOut> : IDelegateTestDataOut1<TParam1, TOut>, IDelegateTestData<TParam1, TParam2> { }

    /// <summary>
    /// Defines test data for operations that include at least 3 input values and produce at least 1 output value.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third input value or parameter.</typeparam>
    /// <typeparam name="TOut">The type of the output parameter or additional output value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="ActionWithOutput{TParam1, TParam2, TParam3, TOut}"/> delegate.</remarks>
    public interface IDelegateTestDataOut1<TParam1, TParam2, TParam3, TOut> : IDelegateTestDataOut1<TParam1, TParam2, TOut>, IDelegateTestData<TParam1, TParam2, TParam3> { }

    /// <summary>
    /// Defines test data for operations that include at least 4 input values and produce at least 1 output value.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third input value or parameter.</typeparam>
    /// <typeparam name="TParam4">The type of the fourth input value or parameter.</typeparam>
    /// <typeparam name="TOut">The type of the output parameter or additional output value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="ActionWithOutput{TParam1, TParam2, TParam3, TParam4, TOut}"/>
    /// delegate.</remarks>
    public interface IDelegateTestDataOut1<TParam1, TParam2, TParam3, TParam4, TOut> : IDelegateTestDataOut1<TParam1, TParam2, TParam3, TOut>,
        IDelegateTestData<TParam1, TParam2, TParam3, TParam4> { }

    /// <summary>
    /// Defines test data for operations that include at least 5 input values and produce at least 1 output value.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third input value or parameter.</typeparam>
    /// <typeparam name="TParam4">The type of the fourth input value or parameter.</typeparam>
    /// <typeparam name="TParam5">The type of the fifth input value or parameter.</typeparam>
    /// <typeparam name="TOut">The type of the output parameter or additional output value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="ActionWithOutput{TParam1, TParam2, TParam3, TParam4, TParam5, TOut}"/>
    /// delegate.</remarks>
    public interface IDelegateTestDataOut1<TParam1, TParam2, TParam3, TParam4, TParam5, TOut> : IDelegateTestDataOut1<TParam1, TParam2, TParam3, TParam4, TOut>,
        IDelegateTestData<TParam1, TParam2, TParam3, TParam4, TParam5> { }

    /// <summary>
    /// Defines test data for operations that include at least 5 input values and produce at least 1 output value.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third input value or parameter.</typeparam>
    /// <typeparam name="TParam4">The type of the fourth input value or parameter.</typeparam>
    /// <typeparam name="TParam5">The type of the fifth input value or parameter.</typeparam>
    /// <typeparam name="TParam6">The type of the sixth input value or parameter.</typeparam>
    /// <typeparam name="TOut">The type of the output parameter or additional output value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="ActionWithOutput{TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TOut}"/>
    /// delegate.</remarks>
    public interface IDelegateTestDataOut1<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TOut> :
        IDelegateTestDataOut1<TParam1, TParam2, TParam3, TParam4, TParam5, TOut>, IDelegateTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6> { }

    /// <summary>
    /// Defines test data for operations that produce at least 2 output values.
    /// </summary>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="ActionWithOutput{TOut1, TOut2}"/> delegate.</remarks>
    public interface IDelegateTestDataOut2<TOut1, TOut2> : IDelegateTestDataOut1<TOut1>, ITestOutputData<TOut1, TOut2> { }

    /// <summary>
    /// Defines test data for operations that include at least 1 input value and produce at least 2 output output values.
    /// </summary>
    /// <typeparam name="TParam">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="ActionWithOutput{TParam, TOut1, TOut2}"/> delegate.</remarks>
    public interface IDelegateTestDataOut2<TParam, TOut1, TOut2> : IDelegateTestDataOut2<TOut1, TOut2>, IDelegateTestDataOut1<TParam, TOut1> { }

    /// <summary>
    /// Defines test data for operations that include at least 2 input values and produce at least 2 output output values.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="ActionWithOutput{TParam1, TParam2, TOut1, TOut2}"/> delegate.</remarks>
    public interface IDelegateTestDataOut2<TParam1, TParam2, TOut1, TOut2> : IDelegateTestDataOut2<TParam1, TOut1, TOut2>, IDelegateTestDataOut1<TParam1, TParam2, TOut1> { }

    /// <summary>
    /// Defines test data for operations that include at least 3 input values and produce at least 2 output output values.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third input value or parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="ActionWithOutput{TParam1, TParam2, TParam3, TOut1, TOut2}"/> delegate.</remarks>
    public interface IDelegateTestDataOut2<TParam1, TParam2, TParam3, TOut1, TOut2> :
        IDelegateTestDataOut2<TParam1, TParam2, TOut1, TOut2>, IDelegateTestDataOut1<TParam1, TParam2, TParam3, TOut1> { }

    /// <summary>
    /// Defines test data for operations that include at least 4 input values and produce at least 2 output output values.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third input value or parameter.</typeparam>
    /// <typeparam name="TParam4">The type of the fourth input value or parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="ActionWithOutput{TParam1, TParam2, TParam3, TParam4, TOut1, TOut2}"/>
    /// delegate.</remarks>
    public interface IDelegateTestDataOut2<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2> : IDelegateTestDataOut2<TParam1, TParam2, TParam3, TOut1, TOut2>,
        IDelegateTestDataOut1<TParam1, TParam2, TParam3, TParam4, TOut1> { }

    /// <summary>
    /// Defines test data for operations that include at least 5 input values and produce at least 2 output output values.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third input value or parameter.</typeparam>
    /// <typeparam name="TParam4">The type of the fourth input value or parameter.</typeparam>
    /// <typeparam name="TParam5">The type of the fifth input value or parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="ActionWithOutput{TParam1, TParam2, TParam3, TParam4, TParam5, TOut1, TOut2}"/>
    /// delegate.</remarks>
    public interface IDelegateTestDataOut2<TParam1, TParam2, TParam3, TParam4, TParam5, TOut1, TOut2> :
        IDelegateTestDataOut2<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2>, IDelegateTestDataOut1<TParam1, TParam2, TParam3, TParam4, TParam5, TOut1> { }

    /// <summary>
    /// Defines test data for operations that produce at least 3 output values.
    /// </summary>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter or additional output value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="ActionWithOutput{TOut1, TOut2, TOut3}"/> delegate.</remarks>
    public interface IDelegateTestDataOut3<TOut1, TOut2, TOut3> : IDelegateTestDataOut2<TOut1, TOut2>, ITestOutputData<TOut1, TOut2, TOut3> { }

    /// <summary>
    /// Defines test data for operations that include at least 1 input value and produce at least 3 output output values.
    /// </summary>
    /// <typeparam name="TParam">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter or additional output value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="ActionWithOutput{TParam, TOut1, TOut2, TOut3}"/> delegate.</remarks>
    public interface IDelegateTestDataOut3<TParam, TOut1, TOut2, TOut3> : IDelegateTestDataOut3<TOut1, TOut2, TOut3>, IDelegateTestDataOut2<TParam, TOut1, TOut2> { }

    /// <summary>
    /// Defines test data for operations that include at least 2 input values and produce at least 3 output output values.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter or additional output value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="ActionWithOutput{TParam1, TParam2, TOut1, TOut2, TOut3}"/> delegate.</remarks>
    public interface IDelegateTestDataOut3<TParam1, TParam2, TOut1, TOut2, TOut3> : IDelegateTestDataOut3<TParam1, TOut1, TOut2, TOut3>,
        IDelegateTestDataOut2<TParam1, TParam2, TOut1, TOut2> { }

    /// <summary>
    /// Defines test data for operations that include at least 3 input values and produce at least 3 output output values.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third input value or parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter or additional output value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="ActionWithOutput{TParam1, TParam2, TParam3, TOut1, TOut2, TOut3}"/>
    /// delegate.</remarks>
    public interface IDelegateTestDataOut3<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3> : IDelegateTestDataOut3<TParam1, TParam2, TOut1, TOut2, TOut3>,
        IDelegateTestDataOut2<TParam1, TParam2, TParam3, TOut1, TOut2> { }

    /// <summary>
    /// Defines test data for operations that include at least 4 input values and produce at least 3 output output values.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third input value or parameter.</typeparam>
    /// <typeparam name="TParam4">The type of the fourth input value or parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter or additional output value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="ActionWithOutput{TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TOut3}"/>
    /// delegate.</remarks>
    public interface IDelegateTestDataOut3<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TOut3> : IDelegateTestDataOut3<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3>,
        IDelegateTestDataOut2<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2> { }

    /// <summary>
    /// Defines test data for operations that produce at least 4 output values.
    /// </summary>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth output parameter or additional output value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="ActionWithOutput{TOut1, TOut2, TOut3, TOut4}"/> delegate.</remarks>
    public interface IDelegateTestDataOut4<TOut1, TOut2, TOut3, TOut4> : IDelegateTestDataOut3<TOut1, TOut2, TOut3>, ITestOutputData<TOut1, TOut2, TOut3, TOut4> { }

    /// <summary>
    /// Defines test data for operations that include at least 1 input value and produce at least 4 output output values.
    /// </summary>
    /// <typeparam name="TParam">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth output parameter or additional output value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="ActionWithOutput{TParam, TOut1, TOut2, TOut3, TOut44}"/> delegate.</remarks>
    public interface IDelegateTestDataOut4<TParam, TOut1, TOut2, TOut3, TOut4> : IDelegateTestDataOut4<TOut1, TOut2, TOut3, TOut4>,
       IDelegateTestDataOut3<TParam, TOut1, TOut2, TOut3> { }

    /// <summary>
    /// Defines test data for operations that include at least 2 input values and produce at least 4 output output values.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>=
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth output parameter or additional output value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="ActionWithOutput{TParam1, TParam2, TOut1, TOut2, TOut3, TOut4}"/>
    /// delegate.</remarks>
    public interface IDelegateTestDataOut4<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4> : IDelegateTestDataOut4<TParam1, TOut1, TOut2, TOut3, TOut4>,
        IDelegateTestDataOut3<TParam1, TParam2, TOut1, TOut2, TOut3> { }

    /// <summary>
    /// Defines test data for operations that include at least 3 input values and produce at least 4 output output values.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third input value or parameter.</typeparam>=
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth output parameter or additional output value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="ActionWithOutput{TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TOut4}"/>
    /// delegate.</remarks>
    public interface IDelegateTestDataOut4<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TOut4> : IDelegateTestDataOut4<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4>,
        IDelegateTestDataOut3<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3> { }

    /// <summary>
    /// Defines test data for operations that produce at least 5 output values.
    /// </summary>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut5">The type of the fifth output parameter or additional output value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="ActionWithOutput{TOut1, TOut2, TOut3, TOut4, TOut5}"/> delegate.</remarks>
    public interface IDelegateTestDataOut5<TOut1, TOut2, TOut3, TOut4, TOut5> : IDelegateTestDataOut4<TOut1, TOut2, TOut3, TOut4>,
        ITestOutputData<TOut1, TOut2, TOut3, TOut4, TOut5> { }

    /// <summary>
    /// Defines test data for operations that include at least 1 input value and produce at least 5 output output values.
    /// </summary>
    /// <typeparam name="TParam">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut5">The type of the fifth output parameter or additional output value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="ActionWithOutput{TOut1, TOut2, TOut3, TOut4}"/> delegate.</remarks>
    public interface IDelegateTestDataOut5<TParam, TOut1, TOut2, TOut3, TOut4, TOut5> : IDelegateTestDataOut5<TOut1, TOut2, TOut3, TOut4, TOut5>,
        IDelegateTestDataOut4<TParam, TOut1, TOut2, TOut3, TOut4> { }

    /// <summary>
    /// Defines test data for operations that include at least 2 input values and produce at least 5 output output values.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut5">The type of the fifth output parameter or additional output value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="ActionWithOutput{TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TOut5>}"/>
    /// delegate.</remarks>
    public interface IDelegateTestDataOut5<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TOut5> : IDelegateTestDataOut5<TParam1, TOut1, TOut2, TOut3, TOut4, TOut5>,
        IDelegateTestDataOut4<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4> { }

    /// <summary>
    /// Defines test data for operations that produce at least 6 output values.
    /// </summary>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut5">The type of the fifth output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut6">The type of the sixth output parameter or additional output value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="ActionWithOutput{TOut1, TOut2, TOut3, TOut4, TOut5, TOut6}"/> delegate.</remarks>
    public interface IDelegateTestDataOut6<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> : IDelegateTestDataOut5<TOut1, TOut2, TOut3, TOut4, TOut5>,
        ITestOutputData<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> { }

    /// <summary>
    /// Defines test data for operations that include at least 1 input value and produce at least 6 output output values.
    /// </summary>
    /// <typeparam name="TParam">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut5">The type of the fifth output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut6">The type of the sixth output parameter or additional output value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="ActionWithOutput{TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6}"/>
    /// delegate.</remarks>
    public interface IDelegateTestDataOut6<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> : IDelegateTestDataOut6<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>,
        IDelegateTestDataOut5<TParam, TOut1, TOut2, TOut3, TOut4, TOut5> { }

    /// <summary>
    /// Defines test data for operations that produce at least 7 output values.
    /// </summary>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut5">The type of the fifth output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut6">The type of the sixth output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut7">The type of the seventh output parameter or additional output value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the
    /// <seealso cref="ActionWithOutput{TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7}"/> delegate.</remarks>
    public interface IDelegateTestDataOut7<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7> : IDelegateTestDataOut6<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>,
        ITestOutputData<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7> { }
}
