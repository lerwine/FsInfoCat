using System;
using System.Collections.Generic;
using System.Text;

namespace FsInfoCat.Test.Helpers
{
    /// <summary>
    /// Defines test data to represent results from a <seealso cref="Func{TResult}"/> invocation.
    /// </summary>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    public interface IFuncInvocation : IInvocationInput
    {
        object ReturnValue { get; }
    }

    /// <summary>
    /// Defines test data to represent results from a <seealso cref="Func{TResult}"/> invocation.
    /// </summary>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    public interface IFuncInvocation<TResult> : IFuncInvocation
    {
        new TResult ReturnValue { get; }
    }

    /// <summary>
    /// Defines test data to represent results from a <seealso cref="Func{T, TResult}"/> invocation.
    /// </summary>
    /// <typeparam name="T">The type of the parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <param name="t">The parameter.</param>
    public interface IFuncInvocation<T, TResult> : IFuncInvocation<TResult>, IInvocationInput<T> { }

    /// <summary>
    /// Defines test data to represent results from a <seealso cref="Func{T1, T2, TResult}"/> invocation.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <param name="t1">The first parameter.</param>
    /// <param name="t2">The second parameter.</param>
    public interface IFuncInvocation<T1, T2, TResult> : IFuncInvocation<T1, TResult>, IInvocationInput<T1, T2> { }

    /// <summary>
    /// Defines test data to represent results from a <seealso cref="Func{T1, T2, T3, TResult}"/> invocation.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <param name="t1">The first parameter.</param>
    /// <param name="t2">The second parameter.</param>
    /// <param name="t3">The third parameter.</param>
    public interface IFuncInvocation<T1, T2, T3, TResult> : IFuncInvocation<T1, T2, TResult>, IInvocationInput<T1, T2, T3> { }

    /// <summary>
    /// Defines test data to represent results from a <seealso cref="Func{T1, T2, T3, T4, TResult}"/> invocation.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <param name="t1">The first parameter.</param>
    /// <param name="t2">The second parameter.</param>
    /// <param name="t3">The third parameter.</param>
    /// <param name="t4">The fourth parameter.</param>
    public interface IFuncInvocation<T1, T2, T3, T4, TResult> : IFuncInvocation<T1, T2, T3, TResult>, IInvocationInput<T1, T2, T3, T4> { }

    /// <summary>
    /// Defines test data to represent results from a <seealso cref="Func{T1, T2, T3, T4, T5, TResult}"/> invocation.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <param name="t1">The first parameter.</param>
    /// <param name="t2">The second parameter.</param>
    /// <param name="t3">The third parameter.</param>
    /// <param name="t4">The fourth parameter.</param>
    /// <param name="t5">The fifth parameter.</param>
    public interface IFuncInvocation<T1, T2, T3, T4, T5, TResult> : IFuncInvocation<T1, T2, T3, T4, TResult>, IInvocationInput<T1, T2, T3, T4, T5> { }

    /// <summary>
    /// Defines test data to represent results from a <seealso cref="Func{T1, T2, T3, T4, T5, T6, TResult}"/> invocation.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <param name="t1">The first parameter.</param>
    /// <param name="t2">The second parameter.</param>
    /// <param name="t3">The third parameter.</param>
    /// <param name="t4">The fourth parameter.</param>
    /// <param name="t5">The fifth parameter.</param>
    /// <param name="t6">The sixth parameter.</param>
    public interface IFuncInvocation<T1, T2, T3, T4, T5, T6, TResult> : IFuncInvocation<T1, T2, T3, T4, T5, TResult>, IInvocationInput<T1, T2, T3, T4, T5, T6> { }

    /// <summary>
    /// Defines test data to represent results from a <seealso cref="Func{T1, T2, T3, T4, T5, T6, T7, TResult}"/> invocation.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <param name="t1">The first parameter.</param>
    /// <param name="t2">The second parameter.</param>
    /// <param name="t3">The third parameter.</param>
    /// <param name="t4">The fourth parameter.</param>
    /// <param name="t5">The fifth parameter.</param>
    /// <param name="t6">The sixth parameter.</param>
    /// <param name="t7">The seventh parameter.</param>
    public interface IFuncInvocation<T1, T2, T3, T4, T5, T6, T7, TResult> : IFuncInvocation<T1, T2, T3, T4, T5, T6, TResult>, IInvocationInput<T1, T2, T3, T4, T5, T6, T7> { }

    public interface IFuncInvocation1<R, TResult> : IFuncInvocation<TResult>, IFuncInvocation { }

    public interface IFuncInvocation1<T, R, TResult> : IFuncInvocation1<R, TResult>, IFuncInvocation<T, TResult> { }
}
