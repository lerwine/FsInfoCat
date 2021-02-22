namespace FsInfoCat.Test.Helpers
{
    /// <summary>
    /// Defines test data to represent results from an <seealso cref="Action"/> invocation.
    /// </summary>
    public interface IInvocationOutput { }

    /// <summary>
    /// Defines test data to represent results from an <seealso cref="ActionWithOutput{R}"/> invocation.
    /// </summary>
    /// <typeparam name="R">The type of the output parameter.</typeparam>
    /// <param name="r">The output parameter.</param>
    public interface IInvocationOutput<R> : IInvocationOutput
    {
        R Output1 { get; }
    }

    /// <summary>
    /// Defines test data to represent results from an <seealso cref="ActionWithOutput2{R1, R2}"/> invocation.
    /// </summary>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <param name="r1">The first output parameter.</param>
    /// <param name="r2">The second output parameter.</param>
    public interface IInvocationOutput<R1, R2> : IInvocationOutput<R1>
    {
        R2 Output2 { get; }
    }

    /// <summary>
    /// Defines test data to represent results from an <seealso cref="ActionWithOutput3{R1, R2, R3}"/> invocation.
    /// </summary>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="R3">The type of the third output parameter.</typeparam>
    /// <param name="r1">The first output parameter.</param>
    /// <param name="r2">The second output parameter.</param>
    /// <param name="r3">The third output parameter.</param>
    public interface IInvocationOutput<R1, R2, R3> : IInvocationOutput<R1, R2>
    {
        R3 Output3 { get; }
    }

    /// <summary>
    /// Defines test data to represent results from an <seealso cref="ActionWithOutput4{R1, R2, R3, R4}"/> invocation.
    /// </summary>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="R3">The type of the third output parameter.</typeparam>
    /// <typeparam name="R4">The type of the fourth output parameter.</typeparam>
    /// <param name="r1">The first output parameter.</param>
    /// <param name="r2">The second output parameter.</param>
    /// <param name="r3">The third output parameter.</param>
    /// <param name="r4">The fourth output parameter.</param>
    public interface IInvocationOutput<R1, R2, R3, R4> : IInvocationOutput<R1, R2, R3>
    {
        R4 Output4 { get; }
    }

    /// <summary>
    /// Defines test data to represent results from an <seealso cref="ActionWithOutput5{R1, R2, R3, R4, R5}"/> invocation.
    /// </summary>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="R3">The type of the third output parameter.</typeparam>
    /// <typeparam name="R4">The type of the fourth output parameter.</typeparam>
    /// <typeparam name="R5">The type of the fifth output parameter.</typeparam>
    /// <param name="r1">The first output parameter.</param>
    /// <param name="r2">The second output parameter.</param>
    /// <param name="r3">The third output parameter.</param>
    /// <param name="r4">The fourth output parameter.</param>
    /// <param name="r5">The fifth output parameter.</param>
    public interface IInvocationOutput<R1, R2, R3, R4, R5> : IInvocationOutput<R1, R2, R3, R4>
    {
        R5 Output5 { get; }
    }

    /// <summary>
    /// Defines test data to represent results from an <seealso cref="ActionWithOutput6{R1, R2, R3, R4, R5, R6}"/> invocation.
    /// </summary>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="R3">The type of the third output parameter.</typeparam>
    /// <typeparam name="R4">The type of the fourth output parameter.</typeparam>
    /// <typeparam name="R5">The type of the fifth output parameter.</typeparam>
    /// <typeparam name="R6">The type of the sixth output parameter.</typeparam>
    /// <param name="r1">The first output parameter.</param>
    /// <param name="r2">The second output parameter.</param>
    /// <param name="r3">The third output parameter.</param>
    /// <param name="r4">The fourth output parameter.</param>
    /// <param name="r5">The fifth output parameter.</param>
    /// <param name="r6">The sixth output parameter.</param>
    public interface IInvocationOutput<R1, R2, R3, R4, R5, R6> : IInvocationOutput<R1, R2, R3, R4, R5>
    {
        R6 Output6 { get; }
    }

    /// <summary>
    /// Defines test data to represent results from an <seealso cref="ActionWithOutput7{R1, R2, R3, R4, R5, R6, R7}"/> invocation.
    /// </summary>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="R3">The type of the third output parameter.</typeparam>
    /// <typeparam name="R4">The type of the fourth output parameter.</typeparam>
    /// <typeparam name="R5">The type of the fifth output parameter.</typeparam>
    /// <typeparam name="R6">The type of the sixth output parameter.</typeparam>
    /// <typeparam name="R7">The type of the seventh output parameter.</typeparam>
    /// <param name="r1">The first output parameter.</param>
    /// <param name="r2">The second output parameter.</param>
    /// <param name="r3">The third output parameter.</param>
    /// <param name="r4">The fourth output parameter.</param>
    /// <param name="r5">The fifth output parameter.</param>
    /// <param name="r6">The sixth output parameter.</param>
    /// <param name="r7">The seventh output parameter.</param>
    public interface IInvocationOutput<R1, R2, R3, R4, R5, R6, R7> : IInvocationOutput<R1, R2, R3, R4, R5, R6>
    {
        R7 Output7 { get; }
    }
}
