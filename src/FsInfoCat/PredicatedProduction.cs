namespace FsInfoCat
{
    /// <summary>
    /// Encapsulates a method that produces a result value and returns a boolean value to indicate whether the invocation was successful.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="result">The result.</param>
    /// <returns></returns>
    public delegate bool PredicatedProduction<TResult>(out TResult result);

    /// <summary>
    /// Encapsulates a method that produces a result value from a single argument and returns a boolean value to indicate whether the invocation was successful.
    /// </summary>
    /// <typeparam name="T">The type of input parameter.</typeparam>
    /// <typeparam name="TResult">The type of the result parameter.</typeparam>
    /// <param name="arg">The input argument.</param>
    /// <param name="result">The result value produced by the method.</param>
    /// <returns><see langword="true"/> to indicate successful method invocation; otherwise, false.</returns>
    public delegate bool PredicatedProduction<in T, TResult>(T arg, out TResult result);

    /// <summary>
    /// Encapsulates a method that produces a result value from 2 arguments and returns a boolean value to indicate whether the invocation was successful.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument.</typeparam>
    /// <typeparam name="T2">The type of the second argument.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="arg1">The first argument.</param>
    /// <param name="arg2">The second argument.</param>
    /// <param name="result">The result value produced by the method.</param>>
    /// <returns><see langword="true"/> to indicate successful method invocation; otherwise, false.</returns>
    public delegate bool PredicatedProduction<in T1, in T2, TResult>(T1 arg1, T2 arg2, out TResult result);

    /// <summary>
    /// Encapsulates a method that produces a result value from 3 arguments and returns a boolean value to indicate whether the invocation was successful.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument.</typeparam>
    /// <typeparam name="T2">The type of the second argument.</typeparam>
    /// <typeparam name="T3">The type of the third argument.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="arg1">The first argument.</param>
    /// <param name="arg2">The second argument.</param>
    /// <param name="arg3">The third argument.</param>
    /// <param name="result">The result value produced by the method.</param>>
    /// <returns><see langword="true"/> to indicate successful method invocation; otherwise, false.</returns>
    public delegate bool PredicatedProduction<in T1, in T2, in T3, TResult>(T1 arg1, T2 arg2, T3 arg3, out TResult result);

    /// <summary>
    /// Encapsulates a method that produces a result value from 4 arguments and returns a boolean value to indicate whether the invocation was successful.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument.</typeparam>
    /// <typeparam name="T2">The type of the second argument.</typeparam>
    /// <typeparam name="T3">The type of the third argument.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="arg1">The first argument.</param>
    /// <param name="arg2">The second argument.</param>
    /// <param name="arg3">The third argument.</param>
    /// <param name="arg4">The fourth argument.</param>
    /// <param name="result">The result value produced by the method.</param>>
    /// <returns><see langword="true"/> to indicate successful method invocation; otherwise, false.</returns>
    public delegate bool PredicatedProduction<in T1, in T2, in T3, in T4, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, out TResult result);

    /// <summary>
    /// Encapsulates a method that produces a result value from 5 arguments and returns a boolean value to indicate whether the invocation was successful.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument.</typeparam>
    /// <typeparam name="T2">The type of the second argument.</typeparam>
    /// <typeparam name="T3">The type of the third argument.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="arg1">The first argument.</param>
    /// <param name="arg2">The second argument.</param>
    /// <param name="arg3">The third argument.</param>
    /// <param name="arg4">The fourth argument.</param>
    /// <param name="arg5">The fifth arg5.</param>
    /// <param name="result">The result value produced by the method.</param>>
    /// <returns><see langword="true"/> to indicate successful method invocation; otherwise, false.</returns>
    public delegate bool PredicatedProduction<in T1, in T2, in T3, in T4, in T5, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, out TResult result);

    /// <summary>
    /// Encapsulates a method that produces a result value from 6 arguments and returns a boolean value to indicate whether the invocation was successful.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument.</typeparam>
    /// <typeparam name="T2">The type of the second argument.</typeparam>
    /// <typeparam name="T3">The type of the third argument.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="arg1">The first argument.</param>
    /// <param name="arg2">The second argument.</param>
    /// <param name="arg3">The third argument.</param>
    /// <param name="arg4">The fourth argument.</param>
    /// <param name="arg5">The fifth arg5.</param>
    /// <param name="arg6">The sixth argument.</param>
    /// <param name="result">The result value produced by the method.</param>>
    /// <returns><see langword="true"/> to indicate successful method invocation; otherwise, false.</returns>
    public delegate bool PredicatedProduction<in T1, in T2, in T3, in T4, in T5, in T6, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, out TResult result);

    /// <summary>
    /// Encapsulates a method that produces a result value from 7 arguments and returns a boolean value to indicate whether the invocation was successful.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument.</typeparam>
    /// <typeparam name="T2">The type of the second argument.</typeparam>
    /// <typeparam name="T3">The type of the third argument.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="arg1">The first argument.</param>
    /// <param name="arg2">The second argument.</param>
    /// <param name="arg3">The third argument.</param>
    /// <param name="arg4">The fourth argument.</param>
    /// <param name="arg5">The fifth arg5.</param>
    /// <param name="arg6">The sixth argument.</param>
    /// <param name="arg7">The seventh argument.</param>
    /// <param name="result">The result value produced by the method.</param>>
    /// <returns><see langword="true"/> to indicate successful method invocation; otherwise, false.</returns>
    public delegate bool PredicatedProduction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7,
        out TResult result);
}
