using System;

namespace FsInfoCat.Test.Helpers
{
    /// <summary>
    /// Data object containing the return value.
    /// </summary>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>This can be used to represent the results of an invocation that matches the signature of the <seealso cref="Func{TResult}"/> delegate.</remarks>
    public interface IFuncInvocationResult<TResult> : IInvocationResult, IFuncTestData<TResult> { }

    /// <summary>
    /// Data object containaing the return value and at least 1 additional output value.
    /// </summary>
    /// <typeparam name="TOut">The type of the output parameter or additional output value.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>This can be used to represent the results of an invocation that matches the signature of the <seealso cref="FuncWithOutput{TOut, TResult}"/>
    /// delegate.</remarks>
    public interface IFuncInvocationResult<TOut, TResult> : IInvocationResult<TOut>, IFuncTestData1<TOut, TResult> { }

    /// <summary>
    /// Data object containaing the return value and at least 2 additional output values.
    /// </summary>
    /// <typeparam name="TOut1">The type of the first additional value or output parameter.</typeparam>
    /// <typeparam name="TOut2">The type of the second additional value or output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>This can be used to represent the results of an invocation that matches the signature of the <seealso cref="FuncWithOutput2{TOut1, TOut2, TResult}"/>
    /// delegate.</remarks>
    public interface IFuncInvocationResult<TOut1, TOut2, TResult> : IInvocationResult<TOut1, TOut2>, IFuncTestData2<TOut1, TOut2, TResult> { }

    /// <summary>
    /// Data object containaing the return value and at least 3 additional output values.
    /// </summary>
    /// <typeparam name="TOut1">The type of the first additional value or output parameter.</typeparam>
    /// <typeparam name="TOut2">The type of the second additional value or output parameter.</typeparam>
    /// <typeparam name="TOut3">The type of the third additional value or output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>This can be used to represent the results of an invocation that matches the signature of the <seealso cref="FuncWithOutput3{TOut1, TOut2, TOut3, TResult}"/>
    /// delegate.</remarks>
    public interface IFuncInvocationResult<TOut1, TOut2, TOut3, TResult> : IInvocationResult<TOut1, TOut2, TOut3>, IFuncTestData3<TOut1, TOut2, TOut3, TResult> { }

    /// <summary>
    /// Data object containaing the return value and at least 4 additional output values.
    /// </summary>
    /// <typeparam name="TOut1">The type of the first additional value or output parameter.</typeparam>
    /// <typeparam name="TOut2">The type of the second additional value or output parameter.</typeparam>
    /// <typeparam name="TOut3">The type of the third additional value or output parameter.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth additional value or output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>This can be used to represent the results of an invocation that matches the signature of the
    /// <seealso cref="FuncWithOutput4{TOut1, TOut2, TOut3, TOut4, TResult}"/> delegate.</remarks>
    public interface IFuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TResult> : IInvocationResult<TOut1, TOut2, TOut3, TOut4>,
        IFuncTestData4<TOut1, TOut2, TOut3, TOut4, TResult> { }

    /// <summary>
    /// Data object containaing the return value and at least 5 additional output values.
    /// </summary>
    /// <typeparam name="TOut1">The type of the first additional value or output parameter.</typeparam>
    /// <typeparam name="TOut2">The type of the second additional value or output parameter.</typeparam>
    /// <typeparam name="TOut3">The type of the third additional value or output parameter.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth additional value or output parameter.</typeparam>
    /// <typeparam name="TOut5">The type of the fifth additional value or output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>This can be used to represent the results of an invocation that matches the signature of the
    /// <seealso cref="FuncWithOutput5{TOut1, TOut2, TOut3, TOut4, TOut5, TResult}"/> delegate.</remarks>
    public interface IFuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TResult> : IInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5>,
        IFuncTestData5<TOut1, TOut2, TOut3, TOut4, TOut5, TResult> { }

    /// <summary>
    /// Data object containaing the return value and at least 6 additional output values.
    /// </summary>
    /// <typeparam name="TOut1">The type of the first additional value or output parameter.</typeparam>
    /// <typeparam name="TOut2">The type of the second additional value or output parameter.</typeparam>
    /// <typeparam name="TOut3">The type of the third additional value or output parameter.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth additional value or output parameter.</typeparam>
    /// <typeparam name="TOut5">The type of the fifth additional value or output parameter.</typeparam>
    /// <typeparam name="TOut6">The type of the sixth additional value or output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>This can be used to represent the results of an invocation that matches the signature of the
    /// <seealso cref="FuncWithOutput6{TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult}"/> delegate.</remarks>
    public interface IFuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult> : IInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>,
        IFuncTestData6<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult> { }

    /// <summary>
    /// Data object containaing the return value and at least 7 additional output values.
    /// </summary>
    /// <typeparam name="TOut1">The type of the first additional value or output parameter.</typeparam>
    /// <typeparam name="TOut2">The type of the second additional value or output parameter.</typeparam>
    /// <typeparam name="TOut3">The type of the third additional value or output parameter.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth additional value or output parameter.</typeparam>
    /// <typeparam name="TOut5">The type of the fifth additional value or output parameter.</typeparam>
    /// <typeparam name="TOut6">The type of the sixth additional value or output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <remarks>This can be used to represent the results of an invocation that matches the signature of the
    /// <seealso cref="FuncWithOutput7{TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult}"/> delegate.</remarks>
    public interface IFuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult> : IInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>,
        IFuncTestData7<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult> { }
}
