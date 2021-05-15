namespace FsInfoCat
{

    public delegate TReturn FuncOut<TResult, TReturn>(out TResult result);

    public delegate TReturn FuncOut<TArg, TResult, TReturn>(TArg arg, out TResult result);

    public delegate TReturn FuncOut<TArg1, TArg2, TResult, TReturn>(TArg1 arg1, TArg2 arg2, out TResult result);

    public delegate TReturn FuncOut<TArg1, TArg2, TArg3, TResult, TReturn>(TArg1 arg1, TArg2 arg2, TArg3 arg3, out TResult result);
}
