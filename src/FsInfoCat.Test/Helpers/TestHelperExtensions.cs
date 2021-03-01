using FsInfoCat.PS;
using System;

namespace FsInfoCat.Test.Helpers
{
    public static class TestHelperExtensions
    {
        public static Func<T1, T2, T3, TResult> Monitor<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> target, out Func<IFuncInvocationResult<TResult>> getResult)
        {
            FunctionInvocationMonitor<TResult> monitor = new FunctionInvocationMonitor<TResult>();
            getResult = monitor.ToResult;
            return monitor.CreateProxy(target);
        }

        public static Action<T1, T2, T3> Monitor<T1, T2, T3>(this Action<T1, T2, T3> target, out Func<IInvocationResult<T1, T2, T3>> getResult)
        {
            InvocationMonitor<T1, T2, T3> monitor = new InvocationMonitor<T1, T2, T3>();
            getResult = monitor.ToResult;
            return monitor.CreateProxy(target);
        }


        public static TryCoerceHandler<T, TOut> Monitor<T, TOut>(this TryCoerceHandler<T, TOut> target, out Func<IFuncInvocationResult<TOut, bool>> getResult)
        {
            FunctionInvocationMonitor<TOut, bool> monitor = new FunctionInvocationMonitor<TOut, bool>();
            getResult = monitor.ToResult;
            return (T t, out TOut o) =>
            {
                bool result = target(t, out o);
                monitor.Apply(result, o);
                return result;
            };
        }

        public static Func<T1, T2, TResult> Monitor<T1, T2, TResult>(this Func<T1, T2, TResult> target, out Func<IFuncInvocationResult<TResult>> getResult)
        {
            FunctionInvocationMonitor<TResult> monitor = new FunctionInvocationMonitor<TResult>();
            getResult = monitor.ToResult;
            return monitor.CreateProxy(target);
        }

        public static Action<T1, T2> Monitor<T1, T2>(this Action<T1, T2> target, out Func<IInvocationResult<T1, T2>> getResult)
        {
            InvocationMonitor<T1, T2> monitor = new InvocationMonitor<T1, T2>();
            getResult = monitor.ToResult;
            return monitor.CreateProxy(target);
        }

        public static Func<T, TResult> Monitor<T, TResult>(this Func<T, TResult> target, out Func<IFuncInvocationResult<TResult>> getResult)
        {
            FunctionInvocationMonitor<TResult> monitor = new FunctionInvocationMonitor<TResult>();
            getResult = monitor.ToResult;
            return monitor.CreateProxy(target);
        }

        public static Action<T> Monitor<T>(this Action<T> target, out Func<IInvocationResult<T>> getResult)
        {
            InvocationMonitor<T> monitor = new InvocationMonitor<T>();
            getResult = monitor.ToResult;
            return monitor.CreateProxy(target);
        }

        public static Func<TResult> Monitor<TResult>(this Func<TResult> target, out Func<IFuncInvocationResult<TResult>> getResult)
        {
            FunctionInvocationMonitor<TResult> monitor = new FunctionInvocationMonitor<TResult>();
            getResult = monitor.ToResult;
            return monitor.CreateProxy(target);
        }

        public static Action Monitor(this Action target, out Func<IInvocationResult> getResult)
        {
            InvocationMonitor monitor = new InvocationMonitor();
            getResult = monitor.ToResult;
            return monitor.CreateProxy(target);
        }
    }
}
