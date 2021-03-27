namespace DevHelperGUI
{
    public delegate bool TryGetter<TResult>(out TResult result);
    public delegate bool TryGetter<TArg, TResult>(TArg arg, out TResult result);
    public delegate bool TryGetter<TArg1, TArg2, TResult>(TArg1 arg1, TArg2 arg2, out TResult result);
    public delegate bool TryGetter<TArg1, TArg2, TArg3, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, out TResult result);
    public delegate bool TryGetter<TArg1, TArg2, TArg3, TArg4, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, out TResult result);
    public delegate bool TryGetter<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, out TResult result);
    public delegate bool TryGetter<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, out TResult result);
    public delegate bool TryGetter<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, out TResult result);
}
