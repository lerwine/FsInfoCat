namespace FsInfoCat.Test.Helpers
{
    /// <summary>
    /// Base data object interface for test operations that may have zero or more output values.
    /// </summary>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="Action"/> delegate.</remarks>
    public interface ITestOutputData { }

    /// <summary>
    /// Defines test data for operations that produce at least 1 output value.
    /// </summary>
    /// <typeparam name="TOut">The type of the output parameter or additional output value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="ActionWithOutput{TOut}"/> delegate.</remarks>
    public interface ITestOutputData<TOut> : ITestOutputData
    {
        /// <summary>
        /// The first output value.
        /// </summary>
        TOut Output1 { get; }
    }

    /// <summary>
    /// Defines test data for operations that produce at least 2 output values.
    /// </summary>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="ActionWithOutput2{TOut1, TOut2}"/> delegate.</remarks>
    public interface ITestOutputData<TOut1, TOut2> : ITestOutputData<TOut1>
    {
        /// <summary>
        /// The second output value.
        /// </summary>
        TOut2 Output2 { get; }
    }

    /// <summary>
    /// Defines test data for operations that produce at least 3 output values.
    /// </summary>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter or additional output value.</typeparam>
    /// <param name="r1">The first output parameter.</param>
    /// <param name="r2">The second output parameter.</param>
    /// <param name="r3">The third output parameter.</param>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="ActionWithOutput3{TOut1, TOut2, TOut3}"/> delegate.</remarks>
    public interface ITestOutputData<TOut1, TOut2, TOut3> : ITestOutputData<TOut1, TOut2>
    {
        /// <summary>
        /// The third output value.
        /// </summary>
        TOut3 Output3 { get; }
    }

    /// <summary>
    /// Defines test data for operations that produce at least 4 output values.
    /// </summary>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth output parameter or additional output value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="ActionWithOutput4{TOut1, TOut2, TOut3, TOut4}}"/> delegate.</remarks>
    public interface ITestOutputData<TOut1, TOut2, TOut3, TOut4> : ITestOutputData<TOut1, TOut2, TOut3>
    {
        /// <summary>
        /// The fourth output value.
        /// </summary>
        TOut4 Output4 { get; }
    }

    /// <summary>
    /// Defines test data for operations that produce at least 5 output values.
    /// </summary>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut5">The type of the fifth output parameter or additional output value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="ActionWithOutput5{TOut1, TOut2, TOut3, TOut4, TOut5}"/> delegate.</remarks>
    public interface ITestOutputData<TOut1, TOut2, TOut3, TOut4, TOut5> : ITestOutputData<TOut1, TOut2, TOut3, TOut4>
    {
        /// <summary>
        /// The fifth output value.
        /// </summary>
        TOut5 Output5 { get; }
    }

    /// <summary>
    /// Defines test data for operations that produce at least 6 output values.
    /// </summary>
    /// <typeparam name="TOut1">The type of the first output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut5">The type of the fifth output parameter or additional output value.</typeparam>
    /// <typeparam name="TOut6">The type of the sixth output parameter or additional output value.</typeparam>
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="ActionWithOutput6{TOut1, TOut2, TOut3, TOut4, TOut5, TOut6}"/> delegate.</remarks>
    public interface ITestOutputData<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> : ITestOutputData<TOut1, TOut2, TOut3, TOut4, TOut5>
    {
        /// <summary>
        /// The sixth output value.
        /// </summary>
        TOut6 Output6 { get; }
    }

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
    /// <remarks>Can be used with invocations that match the signature of the <seealso cref="ActionWithOutput7{TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7}"/> delegate.</remarks>
    public interface ITestOutputData<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7> : ITestOutputData<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>
    {
        /// <summary>
        /// The seventhS output value.
        /// </summary>
        TOut7 Output7 { get; }
    }
}
