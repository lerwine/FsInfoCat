using System;

namespace FsInfoCat.Test.Helpers
{

    /// <summary>
    /// Defines test data to represent results from an <seealso cref="Action"/> invocation.
    /// </summary>
    public interface IInvocationInput { }

    /// <summary>
    /// Defines test data to represent results from an <seealso cref="Action{T}"/> invocation.
    /// </summary>
    /// <typeparam name="T">The type of the parameter.</typeparam>
    /// <param name="t">The parameter.</param>
    public interface IInvocationInput<T> : IInvocationInput
    {
        T Input1 { get; }
    }

    /// <summary>
    /// Defines test data to represent results from an <seealso cref="Action{T1, T2}"/> invocation.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <param name="t1">The first parameter.</param>
    /// <param name="t2">The second parameter.</param>
    public interface IInvocationInput<T1, T2> : IInvocationInput<T1>
    {
        T2 Input2 { get; }
    }

    /// <summary>
    /// Defines test data to represent results from an <seealso cref="Action{T1, T2, T3}"/> invocation.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <param name="t1">The first parameter.</param>
    /// <param name="t2">The second parameter.</param>
    /// <param name="t3">The third parameter.</param>
    public interface IInvocationInput<T1, T2, T3> : IInvocationInput<T1, T2>
    {
        T3 Input3 { get; }
    }

    /// <summary>
    /// Defines test data to represent results from an <seealso cref="Action{T1, T2, T3, T4}"/> invocation.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <param name="t1">The first parameter.</param>
    /// <param name="t2">The second parameter.</param>
    /// <param name="t3">The third parameter.</param>
    /// <param name="t4">The fourth parameter.</param>
    public interface IInvocationInput<T1, T2, T3, T4> : IInvocationInput<T1, T2, T3>
    {
        T4 Input4 { get; }
    }

    /// <summary>
    /// Defines test data to represent results from an <seealso cref="Action{T1, T2, T3, T4, T5}"/> invocation.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter.</typeparam>
    /// <param name="t1">The first parameter.</param>
    /// <param name="t2">The second parameter.</param>
    /// <param name="t3">The third parameter.</param>
    /// <param name="t4">The fourth parameter.</param>
    /// <param name="t5">The fifth parameter.</param>
    public interface IInvocationInput<T1, T2, T3, T4, T5> : IInvocationInput<T1, T2, T3, T4>
    {
        T5 Input5 { get; }
    }

    /// <summary>
    /// Defines test data to represent results from an <seealso cref="Action{T1, T2, T3, T4, T5, T6}"/> invocation.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter.</typeparam>
    /// <param name="t1">The first parameter.</param>
    /// <param name="t2">The second parameter.</param>
    /// <param name="t3">The third parameter.</param>
    /// <param name="t4">The fourth parameter.</param>
    /// <param name="t5">The fifth parameter.</param>
    /// <param name="t6">The sixth parameter.</param>
    public interface IInvocationInput<T1, T2, T3, T4, T5, T6> : IInvocationInput<T1, T2, T3, T4, T5>
    {
        T6 Input6 { get; }
    }

    /// <summary>
    /// Defines test data to represent results from an <seealso cref="Action{T1, T2, T3, T4, T5, T6, T7}"/> invocation.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter.</typeparam>
    /// <param name="t1">The first parameter.</param>
    /// <param name="t2">The second parameter.</param>
    /// <param name="t3">The third parameter.</param>
    /// <param name="t4">The fourth parameter.</param>
    /// <param name="t5">The fifth parameter.</param>
    /// <param name="t6">The sixth parameter.</param>
    /// <param name="t7">The seventh parameter.</param>
    public interface IInvocationInput<T1, T2, T3, T4, T5, T6, T7> : IInvocationInput<T1, T2, T3, T4, T5, T6>
    {
        T7 Input7 { get; }
    }
}
