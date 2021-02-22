using System;
using System.Collections.Generic;
using System.Text;

namespace FsInfoCat.Test.Helpers
{
    /// <summary>
    /// Defines test data to represent results from a <seealso cref="Func{object}"/> invocation.
    /// </summary>
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
    public interface IFuncInvocation<T, TResult> : IFuncInvocation<TResult>, IInvocationInput<T> { }

    /// <summary>
    /// Defines test data to represent results from a <seealso cref="Func{T1, T2, TResult}"/> invocation.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    public interface IFuncInvocation<T1, T2, TResult> : IFuncInvocation<T1, TResult>, IInvocationInput<T1, T2> { }

    /// <summary>
    /// Defines test data to represent results from a <seealso cref="Func{T1, T2, T3, TResult}"/> invocation.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    public interface IFuncInvocation<T1, T2, T3, TResult> : IFuncInvocation<T1, T2, TResult>, IInvocationInput<T1, T2, T3> { }

    /// <summary>
    /// Defines test data to represent results from a <seealso cref="Func{T1, T2, T3, T4, TResult}"/> invocation.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
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
    public interface IFuncInvocation<T1, T2, T3, T4, T5, T6, T7, TResult> : IFuncInvocation<T1, T2, T3, T4, T5, T6, TResult>, IInvocationInput<T1, T2, T3, T4, T5, T6, T7> { }

    /// <summary>
    /// Defines test data to represent results from a <seealso cref="FuncWithOutput{R, TResult}"/> invocation.
    /// </summary>
    /// <typeparam name="R">The type of the output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    public interface IFuncInvocation1<R, TResult> : IFuncInvocation<TResult>, IInvocationOutput<R> { }

    /// <summary>
    /// Defines test data to represent results from a <seealso cref="FuncWithOutput{T, R, TResult}"/> invocation.
    /// </summary>
    /// <typeparam name="T">The type of the first parameter.</typeparam>
    /// <typeparam name="R">The type of the output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    public interface IFuncInvocation1<T, R, TResult> : IFuncInvocation1<R, TResult>, IFuncInvocation<T, TResult> { }

    /// <summary>
    /// Defines test data to represent results from a <seealso cref="FuncWithOutput{T1, T2, R, TResult}"/> invocation.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="R">The type of the output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    public interface IFuncInvocation1<T1, T2, R, TResult> : IFuncInvocation1<T1, R, TResult>, IFuncInvocation<T1, T2, TResult> { }

    /// <summary>
    /// Defines test data to represent results from a <seealso cref="FuncWithOutput{T1, T2, T3, R, TResult}"/> invocation.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="R">The type of the output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    public interface IFuncInvocation1<T1, T2, T3, R, TResult> : IFuncInvocation1<T1, T2, R, TResult>, IFuncInvocation<T1, T2, T3, TResult> { }

    /// <summary>
    /// Defines test data to represent results from a <seealso cref="FuncWithOutput{T1, T2, T3, T4, R, TResult}"/> invocation.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="R">The type of the output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    public interface IFuncInvocation1<T1, T2, T3, T4, R, TResult> : IFuncInvocation1<T1, T2, T3, R, TResult>, IFuncInvocation<T1, T2, T3, T4, TResult> { }

    /// <summary>
    /// Defines test data to represent results from a <seealso cref="FuncWithOutput{T1, T2, T3, T4, T5, R, TResult}"/> invocation.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter.</typeparam>
    /// <typeparam name="R">The type of the output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    public interface IFuncInvocation1<T1, T2, T3, T4, T5, R, TResult> : IFuncInvocation1<T1, T2, T3, T4, R, TResult>, IFuncInvocation<T1, T2, T3, T4, T5, TResult> { }

    /// <summary>
    /// Defines test data to represent results from a <seealso cref="FuncWithOutput{T1, T2, T3, T4, T5, T6, R, TResult}"/> invocation.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter.</typeparam>
    /// <typeparam name="R">The type of the output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    public interface IFuncInvocation1<T1, T2, T3, T4, T5, T6, R, TResult> : IFuncInvocation1<T1, T2, T3, T4, T5, R, TResult>, IFuncInvocation<T1, T2, T3, T4, T5, T6, TResult> { }

    /// <summary>
    /// Defines test data to represent results from a <seealso cref="FuncWithOutput2{R1, R2, TResult}"/> invocation.
    /// </summary>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    public interface IFuncInvocation2<R1, R2, TResult> : IFuncInvocation1<R1, TResult>, IInvocationOutput<R1, R2> { }

    /// <summary>
    /// Defines test data to represent results from a <seealso cref="FuncWithOutput2{T, R1, R2, TResult}"/> invocation.
    /// </summary>
    /// <typeparam name="T">The type of the first parameter.</typeparam>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    public interface IFuncInvocation2<T, R1, R2, TResult> : IFuncInvocation2<R1, R2, TResult>, IFuncInvocation1<T, R1, TResult> { }

    /// <summary>
    /// Defines test data to represent results from a <seealso cref="FuncWithOutput2{T1, T2, R1, R2, TResult}"/> invocation.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    public interface IFuncInvocation2<T1, T2, R1, R2, TResult> : IFuncInvocation2<T1, R1, R2, TResult>, IFuncInvocation1<T1, T2, R1, TResult> { }

    /// <summary>
    /// Defines test data to represent results from a <seealso cref="FuncWithOutput2{T1, T2, T3, R1, R2, TResult}"/> invocation.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    public interface IFuncInvocation2<T1, T2, T3, R1, R2, TResult> : IFuncInvocation2<T1, T2, R1, R2, TResult>, IFuncInvocation1<T1, T2, T3, R1, TResult> { }

    /// <summary>
    /// Defines test data to represent results from a <seealso cref="FuncWithOutput2{T1, T2, T3, T4, R1, R2, TResult}"/> invocation.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    public interface IFuncInvocation2<T1, T2, T3, T4, R1, R2, TResult> : IFuncInvocation2<T1, T2, T3, R1, R2, TResult>, IFuncInvocation1<T1, T2, T3, T4, R1, TResult> { }

    /// <summary>
    /// Defines test data to represent results from a <seealso cref="FuncWithOutput2{T1, T2, T3, T4, T5, R1, R2, TResult}"/> invocation.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter.</typeparam>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    public interface IFuncInvocation2<T1, T2, T3, T4, T5, R1, R2, TResult> : IFuncInvocation2<T1, T2, T3, T4, R1, R2, TResult>, IFuncInvocation1<T1, T2, T3, T4, T5, R1, TResult> { }

    /// <summary>
    /// Defines test data to represent results from a <seealso cref="FuncWithOutput3{R1, R2, R3, TResult}"/> invocation.
    /// </summary>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="R3">The type of the third output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    public interface IFuncInvocation3<R1, R2, R3, TResult> : IFuncInvocation2<R1, R2, TResult>, IInvocationOutput<R1, R2, R3> { }

    /// <summary>
    /// Defines test data to represent results from a <seealso cref="FuncWithOutput3{T, R1, R2, R3, TResult}"/> invocation.
    /// </summary>
    /// <typeparam name="T">The type of the first parameter.</typeparam>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="R3">The type of the third output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    public interface IFuncInvocation3<T, R1, R2, R3, TResult> : IFuncInvocation3<R1, R2, R3, TResult>, IFuncInvocation2<T, R1, R2, TResult> { }

    /// <summary>
    /// Defines test data to represent results from a <seealso cref="FuncWithOutput3{T1, T2, R1, R2, R3, TResult}"/> invocation.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="R3">The type of the third output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    public interface IFuncInvocation3<T1, T2, R1, R2, R3, TResult> : IFuncInvocation3<T1, R1, R2, R3, TResult>, IFuncInvocation2<T1, T2, R1, R2, TResult> { }

    /// <summary>
    /// Defines test data to represent results from a <seealso cref="FuncWithOutput3{T1, T2, T3, R1, R2, R3, TResult}"/> invocation.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="R3">The type of the third output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    public interface IFuncInvocation3<T1, T2, T3, R1, R2, R3, TResult> : IFuncInvocation3<T1, T2, R1, R2, R3, TResult>, IFuncInvocation2<T1, T2, T3, R1, R2, TResult> { }

    /// <summary>
    /// Defines test data to represent results from a <seealso cref="FuncWithOutput3{T1, T2, T3, T4, R1, R2, R3, TResult}"/> invocation.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter.</typeparam>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="R3">The type of the third output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    public interface IFuncInvocation3<T1, T2, T3, T4, R1, R2, R3, TResult> : IFuncInvocation3<T1, T2, T3, R1, R2, R3, TResult>, IFuncInvocation2<T1, T2, T3, T4, R1, R2, TResult> { }

    /// <summary>
    /// Defines test data to represent results from a <seealso cref="FuncWithOutput4{R1, R2, R3, R4, TResult}"/> invocation.
    /// </summary>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="R3">The type of the third output parameter.</typeparam>
    /// <typeparam name="R4">The type of the fourth output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    public interface IFuncInvocation4<R1, R2, R3, R4, TResult> : IFuncInvocation3<R1, R2, R3, TResult>, IInvocationOutput<R1, R2, R3, R4> { }

    /// <summary>
    /// Defines test data to represent results from a <seealso cref="FuncWithOutput4{T, R1, R2, R3, R4, TResult}"/> invocation.
    /// </summary>
    /// <typeparam name="T">The type of the first parameter.</typeparam>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="R3">The type of the third output parameter.</typeparam>
    /// <typeparam name="R4">The type of the fourth output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    public interface IFuncInvocation4<T, R1, R2, R3, R4, TResult> : IFuncInvocation4<R1, R2, R3, R4, TResult>, IFuncInvocation3<T, R1, R2, R3, TResult> { }

    /// <summary>
    /// Defines test data to represent results from a <seealso cref="FuncWithOutput4{T1, T2, R1, R2, R3, R4, TResult}"/> invocation.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="R3">The type of the third output parameter.</typeparam>
    /// <typeparam name="R4">The type of the fourth output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    public interface IFuncInvocation4<T1, T2, R1, R2, R3, R4, TResult> : IFuncInvocation4<T1, R1, R2, R3, R4, TResult>, IFuncInvocation3<T1, T2, R1, R2, R3, TResult> { }

    /// <summary>
    /// Defines test data to represent results from a <seealso cref="FuncWithOutput4{T1, T2, T3, R1, R2, R3, R4, TResult}"/> invocation.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="R3">The type of the third output parameter.</typeparam>
    /// <typeparam name="R4">The type of the fourth output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    public interface IFuncInvocation4<T1, T2, T3, R1, R2, R3, R4, TResult> : IFuncInvocation4<T1, T2, R1, R2, R3, R4, TResult>, IFuncInvocation3<T1, T2, T3, R1, R2, R3, TResult> { }

    /// <summary>
    /// Defines test data to represent results from a <seealso cref="FuncWithOutput5R1, R2, R3, R4, R5, TResult}"/> invocation.
    /// </summary>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="R3">The type of the third output parameter.</typeparam>
    /// <typeparam name="R4">The type of the fourth output parameter.</typeparam>
    /// <typeparam name="R5">The type of the fifth output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    public interface IFuncInvocation5<R1, R2, R3, R4, R5, TResult> : IFuncInvocation4<R1, R2, R3, R4, TResult>, IInvocationOutput<R1, R2, R3, R4, R5> { }

    /// <summary>
    /// Defines test data to represent results from a <seealso cref="FuncWithOutput5{T, R1, R2, R3, R4, R5, TResult}"/> invocation.
    /// </summary>
    /// <typeparam name="T">The type of the first parameter.</typeparam>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="R3">The type of the third output parameter.</typeparam>
    /// <typeparam name="R4">The type of the fourth output parameter.</typeparam>
    /// <typeparam name="R5">The type of the fifth output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    public interface IFuncInvocation5<T, R1, R2, R3, R4, R5, TResult> : IFuncInvocation5<R1, R2, R3, R4, R5, TResult>, IFuncInvocation4<T, R1, R2, R3, R4, TResult> { }

    /// <summary>
    /// Defines test data to represent results from a <seealso cref="FuncWithOutput5{T1, T2, R1, R2, R3, R4, R5, TResult}"/> invocation.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="R3">The type of the third output parameter.</typeparam>
    /// <typeparam name="R4">The type of the fourth output parameter.</typeparam>
    /// <typeparam name="R5">The type of the fifth output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    public interface IFuncInvocation5<T1, T2, R1, R2, R3, R4, R5, TResult> : IFuncInvocation5<T1, R1, R2, R3, R4, R5, TResult>, IFuncInvocation4<T1, T2, R1, R2, R3, R4, TResult> { }

    /// <summary>
    /// Defines test data to represent results from a <seealso cref="FuncWithOutput6{R1, R2, R3, R4, R5, R6, TResult}"/> invocation.
    /// </summary>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="R3">The type of the third output parameter.</typeparam>
    /// <typeparam name="R4">The type of the fourth output parameter.</typeparam>
    /// <typeparam name="R5">The type of the fifth output parameter.</typeparam>
    /// <typeparam name="R6">The type of the sixth output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    public interface IFuncInvocation6<R1, R2, R3, R4, R5, R6, TResult> : IFuncInvocation5<R1, R2, R3, R4, R5, TResult>, IInvocationOutput<R1, R2, R3, R4, R5, R6> { }

    /// <summary>
    /// Defines test data to represent results from a <seealso cref="FuncWithOutput6{T, R1, R2, R3, R4, R5, R6, TResult}"/> invocation.
    /// </summary>
    /// <typeparam name="T">The type of the first parameter.</typeparam>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="R3">The type of the third output parameter.</typeparam>
    /// <typeparam name="R4">The type of the fourth output parameter.</typeparam>
    /// <typeparam name="R5">The type of the fifth output parameter.</typeparam>
    /// <typeparam name="R6">The type of the sixth output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    public interface IFuncInvocation6<T, R1, R2, R3, R4, R5, R6, TResult> : IFuncInvocation6<R1, R2, R3, R4, R5, R6, TResult>, IFuncInvocation5<T, R1, R2, R3, R4, R5, TResult> { }

    /// <summary>
    /// Defines test data to represent results from a <seealso cref="FuncWithOutput7{R1, R2, R3, R4, R5, R6, R7, TResult}"/> invocation.
    /// </summary>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="R3">The type of the third output parameter.</typeparam>
    /// <typeparam name="R4">The type of the fourth output parameter.</typeparam>
    /// <typeparam name="R5">The type of the fifth output parameter.</typeparam>
    /// <typeparam name="R6">The type of the sixth output parameter.</typeparam>
    /// <typeparam name="R7">The type of the seventh output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    public interface IFuncInvocation7<R1, R2, R3, R4, R5, R6, R7, TResult> : IFuncInvocation6<R1, R2, R3, R4, R5, R6, TResult>, IInvocationOutput<R1, R2, R3, R4, R5, R6, R7> { }
}
