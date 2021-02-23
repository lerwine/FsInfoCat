using System;

namespace FsInfoCat.Test.Helpers
{

    /// <summary>
    /// Base data object interface for test operations that may have zero or more input values.
    /// </summary>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="Action"/> delegate.</remarks>
    public interface ITestInputData { }

    /// <summary>
    /// Defines test data for operations include at least 1 input value.
    /// </summary>
    /// <typeparam name="TParam">The type of the parameter.</typeparam>
    /// <remarks>Can be used with invocations that match the (ie. <seealso cref="Action{T}"/> delegate.</remarks>
    public interface ITestInputData<TParam> : ITestInputData
    {
        /// <summary>
        /// The first input value.
        /// </summary>
        TParam Input1 { get; }
    }

    /// <summary>
    /// Defines test data for operations that include at least 2 input values.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="Action{TParam1, TParam2}"/> delegate.</remarks>
    public interface ITestInputData<TParam1, TParam2> : ITestInputData<TParam1>
    {
        /// <summary>
        /// The second input value.
        /// </summary>
        TParam2 Input2 { get; }
    }

    /// <summary>
    /// Defines test data for operations that include at least 3 input values.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third input value or parameter.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="Action{TParam1, TParam2, TParam3}"/> delegate.</remarks>
    public interface ITestInputData<TParam1, TParam2, TParam3> : ITestInputData<TParam1, TParam2>
    {
        /// <summary>
        /// The third input value.
        /// </summary>
        TParam3 Input3 { get; }
    }

    /// <summary>
    /// Defines test data for operations that include at least 4 input values.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third input value or parameter.</typeparam>
    /// <typeparam name="TParam4">The type of the fourth input value or parameter.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="Action{TParam1, TParam2, TParam3, TParam4}"/> delegate.</remarks>
    public interface ITestInputData<TParam1, TParam2, TParam3, TParam4> : ITestInputData<TParam1, TParam2, TParam3>
    {
        /// <summary>
        /// The fourth input value.
        /// </summary>
        TParam4 Input4 { get; }
    }

    /// <summary>
    /// Defines test data for operations that include at least 5 input values.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third input value or parameter.</typeparam>
    /// <typeparam name="TParam4">The type of the fourth input value or parameter.</typeparam>
    /// <typeparam name="TParam5">The type of the fifth input value or parameter.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="Action{TParam1, TParam2, TParam3, TParam4, TParam5}"/> delegate.</remarks>
    public interface ITestInputData<TParam1, TParam2, TParam3, TParam4, TParam5> : ITestInputData<TParam1, TParam2, TParam3, TParam4>
    {
        /// <summary>
        /// The fifth input value.
        /// </summary>
        TParam5 Input5 { get; }
    }

    /// <summary>
    /// Defines test data for operations that include at least 6 input values.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first input value or parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second input value or parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third input value or parameter.</typeparam>
    /// <typeparam name="TParam4">The type of the fourth input value or parameter.</typeparam>
    /// <typeparam name="TParam5">The type of the fifth input value or parameter.</typeparam>
    /// <typeparam name="TParam6">The type of the sixth input value or parameter.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="Action{TParam1, TParam2, TParam3, TParam4, TParam5, TParam6}"/> delegate.</remarks>
    public interface ITestInputData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6> : ITestInputData<TParam1, TParam2, TParam3, TParam4, TParam5>
    {
        /// <summary>
        /// The sixth input value.
        /// </summary>
        TParam6 Input6 { get; }
    }

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
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="Action{TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7}"/> delegate.</remarks>
    public interface ITestInputData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7> : ITestInputData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6>
    {
        /// <summary>
        /// The seventh input value.
        /// </summary>
        TParam7 Input7 { get; }
    }
}
