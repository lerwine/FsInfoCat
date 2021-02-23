namespace FsInfoCat.Test.Helpers
{
    /// <summary>
    /// Encapsulates a method that has 1 output parameter and returns a value.
    /// </summary>
    /// <typeparam name="TOut">The type of the output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <param name="o">The output parameter.</param>
    public delegate TResult FuncWithOutput<TOut, TResult>(out TOut o);

    /// <summary>
    /// Encapsulates a method that has 1 parameter, 1 output parameter, and returns a value.
    /// </summary>
    /// <typeparam name="TParam">The type of the first parameter.</typeparam>
    /// <typeparam name="TOut">The type of the output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <param name="p">The first parameter.</param>
    /// <param name="o">The output parameter.</param>
    public delegate TResult FuncWithOutput<TParam, TOut, TResult>(TParam p, out TOut o);

    /// <summary>
    /// Encapsulates a method that has 2 parameters, 1 output parameter, and returns a value.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second parameter.</typeparam>
    /// <typeparam name="TOut">The type of the output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <param name="p1">The first parameter.</param>
    /// <param name="p2">The second parameter.</param>
    /// <param name="o">The output parameter.</param>
    public delegate TResult FuncWithOutput<TParam1, TParam2, TOut, TResult>(TParam1 p1, TParam2 p2, out TOut o);

    /// <summary>
    /// Encapsulates a method that has 3 parameters, 1 output parameter, and returns a value.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third parameter.</typeparam>
    /// <typeparam name="TOut">The type of the output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <param name="p1">The first parameter.</param>
    /// <param name="p2">The second parameter.</param>
    /// <param name="p3">The third parameter.</param>
    /// <param name="o">The output parameter.</param>
    public delegate TResult FuncWithOutput<TParam1, TParam2, TParam3, TOut, TResult>(TParam1 p1, TParam2 p2, TParam3 p3, out TOut o);

    /// <summary>
    /// Encapsulates a method that has 4 parameters, 1 output parameter, and returns a value.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third parameter.</typeparam>
    /// <typeparam name="TParam4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="TOut">The type of the output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <param name="p1">The first parameter.</param>
    /// <param name="p2">The second parameter.</param>
    /// <param name="p3">The third parameter.</param>
    /// <param name="p4">The fourth parameter.</param>
    /// <param name="o">The output parameter.</param>
    public delegate TResult FuncWithOutput<TParam1, TParam2, TParam3, TParam4, TOut, TResult>(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, out TOut o);

    /// <summary>
    /// Encapsulates a method that has 5 parameters, 1 output parameter, and returns a value.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third parameter.</typeparam>
    /// <typeparam name="TParam4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="TParam5">The type of the fifth parameter.</typeparam>
    /// <typeparam name="TOut">The type of the output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <param name="p1">The first parameter.</param>
    /// <param name="p2">The second parameter.</param>
    /// <param name="p3">The third parameter.</param>
    /// <param name="p4">The fourth parameter.</param>
    /// <param name="p5">The fifth parameter.</param>
    /// <param name="o">The output parameter.</param>
    public delegate TResult FuncWithOutput<TParam1, TParam2, TParam3, TParam4, TParam5, TOut, TResult>(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, out TOut o);

    /// <summary>
    /// Encapsulates a method that has 6 parameters, 1 output parameter, and returns a value.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third parameter.</typeparam>
    /// <typeparam name="TParam4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="TParam5">The type of the fifth parameter.</typeparam>
    /// <typeparam name="TParam6">The type of the sixth parameter.</typeparam>
    /// <typeparam name="TOut">The type of the output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <param name="p1">The first parameter.</param>
    /// <param name="p2">The second parameter.</param>
    /// <param name="p3">The third parameter.</param>
    /// <param name="p4">The fourth parameter.</param>
    /// <param name="p5">The fifth parameter.</param>
    /// <param name="p6">The sixth parameter.</param>
    /// <param name="o">The output parameter.</param>
    public delegate TResult FuncWithOutput<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TOut, TResult>(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6, out TOut o);

    /// <summary>
    /// Encapsulates a method that has 2 output parameters and returns a value.
    /// </summary>
    /// <typeparam name="TOut1">The type of the first output parameter.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <param name="o1">The first output parameter.</param>
    /// <param name="o2">The second output parameter.</param>
    public delegate TResult FuncWithOutput2<TOut1, TOut2, TResult>(out TOut1 o1, out TOut2 o2);

    /// <summary>
    /// Encapsulates a method that has 1 parameter, 2 output parameters, and returns a value.
    /// </summary>
    /// <typeparam name="TParam">The type of the first parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <param name="p">The first parameter.</param>
    /// <param name="o1">The first output parameter.</param>
    /// <param name="o2">The second output parameter.</param>
    public delegate TResult FuncWithOutput2<TParam, TOut1, TOut2, TResult>(TParam p, out TOut1 o1, out TOut2 o2);

    /// <summary>
    /// Encapsulates a method that has 2 parameters, 2 output parameters, and returns a value.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <param name="p1">The first parameter.</param>
    /// <param name="p2">The second parameter.</param>
    /// <param name="o1">The first output parameter.</param>
    /// <param name="o2">The second output parameter.</param>
    public delegate TResult FuncWithOutput2<TParam1, TParam2, TOut1, TOut2, TResult>(TParam1 p1, TParam2 p2, out TOut1 o1, out TOut2 o2);

    /// <summary>
    /// Encapsulates a method that has 3 parameters, 2 output parameters, and returns a value.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <param name="p1">The first parameter.</param>
    /// <param name="p2">The second parameter.</param>
    /// <param name="p3">The third parameter.</param>
    /// <param name="o1">The first output parameter.</param>
    /// <param name="o2">The second output parameter.</param>
    public delegate TResult FuncWithOutput2<TParam1, TParam2, TParam3, TOut1, TOut2, TResult>(TParam1 p1, TParam2 p2, TParam3 p3, out TOut1 o1, out TOut2 o2);

    /// <summary>
    /// Encapsulates a method that has 4 parameters, 2 output parameters, and returns a value.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third parameter.</typeparam>
    /// <typeparam name="TParam4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <param name="p1">The first parameter.</param>
    /// <param name="p2">The second parameter.</param>
    /// <param name="p3">The third parameter.</param>
    /// <param name="p4">The fourth parameter.</param>
    /// <param name="o1">The first output parameter.</param>
    /// <param name="o2">The second output parameter.</param>
    public delegate TResult FuncWithOutput2<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TResult>(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, out TOut1 o1, out TOut2 o2);

    /// <summary>
    /// Encapsulates a method that has 5 parameters, 2 output parameters, and returns a value.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third parameter.</typeparam>
    /// <typeparam name="TParam4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="TParam5">The type of the fifth parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <param name="p1">The first parameter.</param>
    /// <param name="p2">The second parameter.</param>
    /// <param name="p3">The third parameter.</param>
    /// <param name="p4">The fourth parameter.</param>
    /// <param name="p5">The fifth parameter.</param>
    /// <param name="o1">The first output parameter.</param>
    /// <param name="o2">The second output parameter.</param>
    public delegate TResult FuncWithOutput2<TParam1, TParam2, TParam3, TParam4, TParam5, TOut1, TOut2, TResult>(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, out TOut1 o1, out TOut2 o2);

    /// <summary>
    /// Encapsulates a method that has 3 output parameters and returns a value.
    /// </summary>
    /// <typeparam name="TOut1">The type of the first output parameter.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <param name="o1">The first output parameter.</param>
    /// <param name="o2">The second output parameter.</param>
    /// <param name="o3">The third output parameter.</param>
    public delegate TResult FuncWithOutput3<TOut1, TOut2, TOut3, TResult>(out TOut1 o1, out TOut2 o2, out TOut3 o3);

    /// <summary>
    /// Encapsulates a method that has 1 parameter, 3 output parameters, and returns a value.
    /// </summary>
    /// <typeparam name="TParam">The type of the first parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <param name="p">The first parameter.</param>
    /// <param name="o1">The first output parameter.</param>
    /// <param name="o2">The second output parameter.</param>
    /// <param name="o3">The third output parameter.</param>
    public delegate TResult FuncWithOutput3<TParam, TOut1, TOut2, TOut3, TResult>(TParam p, out TOut1 o1, out TOut2 o2, out TOut3 o3);

    /// <summary>
    /// Encapsulates a method that has 2 parameters, 3 output parameters, and returns a value.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <param name="p1">The first parameter.</param>
    /// <param name="p2">The second parameter.</param>
    /// <param name="o1">The first output parameter.</param>
    /// <param name="o2">The second output parameter.</param>
    /// <param name="o3">The third output parameter.</param>
    public delegate TResult FuncWithOutput3<TParam1, TParam2, TOut1, TOut2, TOut3, TResult>(TParam1 p1, TParam2 p2, out TOut1 o1, out TOut2 o2, out TOut3 o3);

    /// <summary>
    /// Encapsulates a method that has 3 parameters, 3 output parameters, and returns a value.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <param name="p1">The first parameter.</param>
    /// <param name="p2">The second parameter.</param>
    /// <param name="p3">The third parameter.</param>
    /// <param name="o1">The first output parameter.</param>
    /// <param name="o2">The second output parameter.</param>
    /// <param name="o3">The third output parameter.</param>
    public delegate TResult FuncWithOutput3<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TResult>(TParam1 p1, TParam2 p2, TParam3 p3, out TOut1 o1, out TOut2 o2, out TOut3 o3);

    /// <summary>
    /// Encapsulates a method that has 4 parameters, 3 output parameters, and returns a value.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third parameter.</typeparam>
    /// <typeparam name="TParam4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <param name="p1">The first parameter.</param>
    /// <param name="p2">The second parameter.</param>
    /// <param name="p3">The third parameter.</param>
    /// <param name="p4">The fourth parameter.</param>
    /// <param name="o1">The first output parameter.</param>
    /// <param name="o2">The second output parameter.</param>
    /// <param name="o3">The third output parameter.</param>
    public delegate TResult FuncWithOutput3<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TOut3, TResult>(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, out TOut1 o1, out TOut2 o2, out TOut3 o3);

    /// <summary>
    /// Encapsulates a method that has 4 output parameters and returns a value.
    /// </summary>
    /// <typeparam name="TOut1">The type of the first output parameter.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <param name="o1">The first output parameter.</param>
    /// <param name="o2">The second output parameter.</param>
    /// <param name="o3">The third output parameter.</param>
    /// <param name="o4">The fourth output parameter.</param>
    public delegate TResult FuncWithOutput4<TOut1, TOut2, TOut3, TOut4, TResult>(out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4);

    /// <summary>
    /// Encapsulates a method that has 1 parameter, 4 output parameters, and returns a value.
    /// </summary>
    /// <typeparam name="TParam">The type of the first parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <param name="p">The first parameter.</param>
    /// <param name="o1">The first output parameter.</param>
    /// <param name="o2">The second output parameter.</param>
    /// <param name="o3">The third output parameter.</param>
    /// <param name="o4">The fourth output parameter.</param>
    public delegate TResult FuncWithOutput4<TParam, TOut1, TOut2, TOut3, TOut4, TResult>(TParam p, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4);

    /// <summary>
    /// Encapsulates a method that has 2 parameters, 4 output parameters, and returns a value.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <param name="p1">The first parameter.</param>
    /// <param name="p2">The second parameter.</param>
    /// <param name="o1">The first output parameter.</param>
    /// <param name="o2">The second output parameter.</param>
    /// <param name="o3">The third output parameter.</param>
    /// <param name="o4">The fourth output parameter.</param>
    public delegate TResult FuncWithOutput4<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TResult>(TParam1 p1, TParam2 p2, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4);

    /// <summary>
    /// Encapsulates a method that has 3 parameters, 4 output parameters, and returns a value.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second parameter.</typeparam>
    /// <typeparam name="TParam3">The type of the third parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <param name="p1">The first parameter.</param>
    /// <param name="p2">The second parameter.</param>
    /// <param name="p3">The third parameter.</param>
    /// <param name="o1">The first output parameter.</param>
    /// <param name="o2">The second output parameter.</param>
    /// <param name="o3">The third output parameter.</param>
    /// <param name="o4">The fourth output parameter.</param>
    public delegate TResult FuncWithOutput4<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TOut4, TResult>(TParam1 p1, TParam2 p2, TParam3 p3, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4);

    /// <summary>
    /// Encapsulates a method that has 5 output parameters and returns a value.
    /// </summary>
    /// <typeparam name="TOut1">The type of the first output parameter.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth output parameter.</typeparam>
    /// <typeparam name="TOut5">The type of the fifth output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <param name="o1">The first output parameter.</param>
    /// <param name="o2">The second output parameter.</param>
    /// <param name="o3">The third output parameter.</param>
    /// <param name="o4">The fourth output parameter.</param>
    /// <param name="o5">The fifth output parameter.</param>
    public delegate TResult FuncWithOutput5<TOut1, TOut2, TOut3, TOut4, TOut5, TResult>(out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4, out TOut5 o5);

    /// <summary>
    /// Encapsulates a method that has 1 parameter, 5 output parameters, and returns a value.
    /// </summary>
    /// <typeparam name="TParam">The type of the first parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth output parameter.</typeparam>
    /// <typeparam name="TOut5">The type of the fifth output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <param name="p">The first parameter.</param>
    /// <param name="o1">The first output parameter.</param>
    /// <param name="o2">The second output parameter.</param>
    /// <param name="o3">The third output parameter.</param>
    /// <param name="o4">The fourth output parameter.</param>
    /// <param name="o5">The fifth output parameter.</param>
    public delegate TResult FuncWithOutput5<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TResult>(TParam p, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4, out TOut5 o5);

    /// <summary>
    /// Encapsulates a method that has 2 parameters, 5 output parameters, and returns a value.
    /// </summary>
    /// <typeparam name="TParam1">The type of the first parameter.</typeparam>
    /// <typeparam name="TParam2">The type of the second parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth output parameter.</typeparam>
    /// <typeparam name="TOut5">The type of the fifth output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <param name="p1">The first parameter.</param>
    /// <param name="p2">The second parameter.</param>
    /// <param name="o1">The first output parameter.</param>
    /// <param name="o2">The second output parameter.</param>
    /// <param name="o3">The third output parameter.</param>
    /// <param name="o4">The fourth output parameter.</param>
    /// <param name="o5">The fifth output parameter.</param>
    public delegate TResult FuncWithOutput5<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TOut5, TResult>(TParam1 p1, TParam2 p2, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4, out TOut5 o5);

    /// <summary>
    /// Encapsulates a method that has 6 output parameters and returns a value.
    /// </summary>
    /// <typeparam name="TOut1">The type of the first output parameter.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth output parameter.</typeparam>
    /// <typeparam name="TOut5">The type of the fifth output parameter.</typeparam>
    /// <typeparam name="TOut6">The type of the sixth output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <param name="o1">The first output parameter.</param>
    /// <param name="o2">The second output parameter.</param>
    /// <param name="o3">The third output parameter.</param>
    /// <param name="o4">The fourth output parameter.</param>
    /// <param name="o5">The fifth output parameter.</param>
    /// <param name="o6">The sixth output parameter.</param>
    public delegate TResult FuncWithOutput6<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult>(out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4, out TOut5 o5, out TOut6 o6);

    /// <summary>
    /// Encapsulates a method that has 1 parameter, 6 output parameters, and returns a value.
    /// </summary>
    /// <typeparam name="TParam">The type of the first parameter.</typeparam>
    /// <typeparam name="TOut1">The type of the first output parameter.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth output parameter.</typeparam>
    /// <typeparam name="TOut5">The type of the fifth output parameter.</typeparam>
    /// <typeparam name="TOut6">The type of the sixth output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <param name="p">The first parameter.</param>
    /// <param name="o1">The first output parameter.</param>
    /// <param name="o2">The second output parameter.</param>
    /// <param name="o3">The third output parameter.</param>
    /// <param name="o4">The fourth output parameter.</param>
    /// <param name="o5">The fifth output parameter.</param>
    /// <param name="o6">The sixth output parameter.</param>
    public delegate TResult FuncWithOutput6<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult>(TParam p, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4, out TOut5 o5, out TOut6 o6);

    /// <summary>
    /// Encapsulates a method that has 7 output parameters and returns a value.
    /// </summary>
    /// <typeparam name="TOut1">The type of the first output parameter.</typeparam>
    /// <typeparam name="TOut2">The type of the second output parameter.</typeparam>
    /// <typeparam name="TOut3">The type of the third output parameter.</typeparam>
    /// <typeparam name="TOut4">The type of the fourth output parameter.</typeparam>
    /// <typeparam name="TOut5">The type of the fifth output parameter.</typeparam>
    /// <typeparam name="TOut6">The type of the sixth output parameter.</typeparam>
    /// <typeparam name="TOut7">The type of the seventh output parameter.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <param name="o1">The first output parameter.</param>
    /// <param name="o2">The second output parameter.</param>
    /// <param name="o3">The third output parameter.</param>
    /// <param name="o4">The fourth output parameter.</param>
    /// <param name="o5">The fifth output parameter.</param>
    /// <param name="o6">The sixth output parameter.</param>
    /// <param name="o7">The seventh output parameter.</param>
    public delegate TResult FuncWithOutput7<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult>(out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4, out TOut5 o5, out TOut6 o6, out TOut7 o7);
}
