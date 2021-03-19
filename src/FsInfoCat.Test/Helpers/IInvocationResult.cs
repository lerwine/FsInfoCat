using System;

namespace FsInfoCat.Test.Helpers
{
    /// <summary>
    /// Represents the results of an <seealso cref="Action"/> invocation.
    /// </summary>
    public interface IInvocationResult : IDelegateTestData, IEquatable<IInvocationResult>
    {
        /// <summary>
        /// This gets set to true if the associated delegate was invoked.
        /// </summary>
        bool WasInvoked { get; }
    }

    /// <summary>
    /// Data object containaing at least 1 output value.
    /// </summary>
    /// <typeparam name="TOut1">The type of the output value or output parameter result.</typeparam>
    /// <remarks>This can be used to represent the results of an invocation that matches the signature of the <seealso cref="ActionWithOutput{TOut}"/> delegate.</remarks>
    public interface IInvocationResult<TOut> : IInvocationResult, IDelegateTestDataOut1<TOut>, IEquatable<IInvocationResult<TOut>> { }

    /// <summary>
    /// Data object containaing at least 2 output values.
    /// </summary>
    /// <typeparam name="TOut1">The type of the first output value or output parameter result.</typeparam>
    /// <typeparam name="TOut2">The type of the second output value or output parameter result.</typeparam>
    /// <remarks>This can be used to represent the results of an invocation that matches the signature of the <seealso cref="ActionWithOutput{TOut1, TOut2}"/>
    /// delegate.</remarks>
    public interface IInvocationResult<TOut1, TOut2> : IInvocationResult<TOut1>, IDelegateTestDataOut2<TOut1, TOut2>, IEquatable<IInvocationResult<TOut1, TOut2>> { }

    /// <summary>
    /// Data object containaing at least 3 output values.
    /// </summary>
    /// <typeparam name="TOut1">The type of the first output value or output parameter result.</typeparam>
    /// <typeparam name="TOut2">The type of the second output value or output parameter result.</typeparam>
    /// <typeparam name="TOut3">The type of the third output value or output parameter result.</typeparam>
    /// <remarks>This can be used to represent the results of an invocation that matches the signature of the <seealso cref="ActionWithOutput{TOut1, TOut2, TOut3}"/>
    /// delegate.</remarks>
    public interface IInvocationResult<TOut1, TOut2, TOut3> : IInvocationResult<TOut1, TOut2>, IDelegateTestDataOut3<TOut1, TOut2, TOut3>,
        IEquatable<IInvocationResult<TOut1, TOut2, TOut3>>
    { }

    /// <summary>
    /// Data object containaing at least 4 output values.
    /// </summary>
    /// <typeparam name="TOut1">The type of the first output value or output parameter result.</typeparam>
    /// <typeparam name="TOut2">The type of the second output value or output parameter result.</typeparam>
    /// <typeparam name="TOut3">The type of the third output value or output parameter result.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth output value or output parameter result.</typeparam>
    /// <remarks>This can be used to represent the results of an invocation that matches the signature of the <seealso cref="ActionWithOutput{TOut1, TOut2, TOut3, TOut4}"/>
    /// delegate.</remarks>
    public interface IInvocationResult<TOut1, TOut2, TOut3, TOut4> : IInvocationResult<TOut1, TOut2, TOut3>, IDelegateTestDataOut4<TOut1, TOut2, TOut3, TOut4>,
        IEquatable<IInvocationResult<TOut1, TOut2, TOut3, TOut4>>
    { }

    /// <summary>
    /// Data object containaing at least 5 output values.
    /// </summary>
    /// <typeparam name="TOut1">The type of the first output value or output parameter result.</typeparam>
    /// <typeparam name="TOut2">The type of the second output value or output parameter result.</typeparam>
    /// <typeparam name="TOut3">The type of the third output value or output parameter result.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth output value or output parameter result.</typeparam>
    /// <typeparam name="TOut5">The type of the fifth output value or output parameter result.</typeparam>
    /// <remarks>This can be used to represent the results of an invocation that matches the signature of the
    /// <seealso cref="ActionWithOutput{TOut1, TOut2, TOut3, TOut4, TOut5}"/> delegate.</remarks>
    public interface IInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5> : IInvocationResult<TOut1, TOut2, TOut3, TOut4>,
        IDelegateTestDataOut5<TOut1, TOut2, TOut3, TOut4, TOut5>, IEquatable<IInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5>>
    { }

    /// <summary>
    /// Data object containaing at least 6 output values.
    /// </summary>
    /// <typeparam name="TOut1">The type of the first output value or output parameter result.</typeparam>
    /// <typeparam name="TOut2">The type of the second output value or output parameter result.</typeparam>
    /// <typeparam name="TOut3">The type of the third output value or output parameter result.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth output value or output parameter result.</typeparam>
    /// <typeparam name="TOut5">The type of the fifth output value or output parameter result.</typeparam>
    /// <typeparam name="TOut6">The type of the sixth output value or output parameter result.</typeparam>
    /// <remarks>This can be used to represent the results of an invocation that matches the signature of the
    /// <seealso cref="ActionWithOutput{TOut1, TOut2, TOut3, TOut4, TOut5, TOut6}"/> delegate.</remarks>
    public interface IInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> : IInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5>,
        IDelegateTestDataOut6<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>, IEquatable<IInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>
    { }

    /// <summary>
    /// Data object containaing at least 7 output values.
    /// </summary>
    /// <typeparam name="TOut1">The type of the first output value or output parameter result.</typeparam>
    /// <typeparam name="TOut2">The type of the second output value or output parameter result.</typeparam>
    /// <typeparam name="TOut3">The type of the third output value or output parameter result.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth output value or output parameter result.</typeparam>
    /// <typeparam name="TOut5">The type of the fifth output value or output parameter result.</typeparam>
    /// <typeparam name="TOut6">The type of the sixth output value or output parameter result.</typeparam>
    /// <typeparam name="TOut7">The type of the seventh output value or output parameter result.</typeparam>
    /// <remarks>This can be used to represent the results of an invocation that matches the signature of the
    /// <seealso cref="ActionWithOutput{TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7}"/> delegate.</remarks>
    public interface IInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7> : IInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>,
        IDelegateTestDataOut7<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>, IEquatable<IInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>
    { }
}
