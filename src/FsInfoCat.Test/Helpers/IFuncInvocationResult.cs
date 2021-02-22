namespace FsInfoCat.Test.Helpers
{
    public interface IFuncInvocationResult<TResult> : IInvocationResult, IFuncInvocation<TResult> { }

    public interface IFuncInvocationResult<T, TResult> : IFuncInvocationResult<TResult>, IInvocationResult<T>, IFuncInvocation<T, TResult> { }

    public interface IFuncInvocationResult<T1, T2, TResult> : IFuncInvocationResult<T1, TResult>, IInvocationResult<T1, T2>, IFuncInvocation<T1, T2, TResult> { }

    public interface IFuncInvocationResult<T1, T2, T3, TResult> : IFuncInvocationResult<T1, T2, TResult>, IInvocationResult<T1, T2, T3>, IFuncInvocation<T1, T2, T3, TResult> { }

    public interface IFuncInvocationResult<T1, T2, T3, T4, TResult> : IFuncInvocationResult<T1, T2, T3, TResult>, IInvocationResult<T1, T2, T3, T4>, IFuncInvocation<T1, T2, T3, T4, TResult> { }

    public interface IFuncInvocationResult<T1, T2, T3, T4, T5, TResult> : IFuncInvocationResult<T1, T2, T3, T4, TResult>, IInvocationResult<T1, T2, T3, T4, T5>, IFuncInvocation<T1, T2, T3, T4, T5, TResult> { }

    public interface IFuncInvocationResult<T1, T2, T3, T4, T5, T6, TResult> : IFuncInvocationResult<T1, T2, T3, T4, T5, TResult>, IInvocationResult<T1, T2, T3, T4, T5, T6>, IFuncInvocation<T1, T2, T3, T4, T5, T6, TResult> { }

    public interface IFuncInvocationResult<T1, T2, T3, T4, T5, T6, T7, TResult> : IFuncInvocationResult<T1, T2, T3, T4, T5, T6, TResult>, IInvocationResult<T1, T2, T3, T4, T5, T6, T7>,
        IFuncInvocation<T1, T2, T3, T4, T5, T6, T7, TResult> { }

    public interface IFuncInvocationResult1<R, TResult> : IFuncInvocationResult<TResult>, IFuncInvocation1<R, TResult> { }

    public interface IFuncInvocationResult1<T, R, TResult> : IFuncInvocationResult1<R, TResult>, IFuncInvocationResult<T, TResult>, IFuncInvocation1<T, R, TResult> { }

    public interface IFuncInvocationResult1<T1, T2, R, TResult> : IFuncInvocationResult1<T1, R, TResult>, IFuncInvocationResult<T1, T2, TResult>, IFuncInvocation1<T1, T2, R, TResult> { }

    public interface IFuncInvocationResult1<T1, T2, T3, R, TResult> : IFuncInvocationResult1<T1, T2, R, TResult>, IFuncInvocationResult<T1, T2, T3, TResult>, IFuncInvocation1<T1, T2, T3, R, TResult> { }

    public interface IFuncInvocationResult1<T1, T2, T3, T4, R, TResult> : IFuncInvocationResult1<T1, T2, T3, R, TResult>, IFuncInvocationResult<T1, T2, T3, T4, TResult>, IFuncInvocation1<T1, T2, T3, T4, R, TResult> { }

    public interface IFuncInvocationResult1<T1, T2, T3, T4, T5, R, TResult> : IFuncInvocationResult1<T1, T2, T3, T4, R, TResult>, IFuncInvocationResult<T1, T2, T3, T4, T5, TResult>, IFuncInvocation1<T1, T2, T3, T4, T5, R, TResult> { }

    public interface IFuncInvocationResult1<T1, T2, T3, T4, T5, T6, R, TResult> : IFuncInvocationResult1<T1, T2, T3, T4, T5, R, TResult>, IFuncInvocationResult<T1, T2, T3, T4, T5, T6, TResult>,
        IFuncInvocation1<T1, T2, T3, T4, T5, T6, R, TResult> { }

    public interface IFuncInvocationResult2<R1, R2, TResult> : IFuncInvocationResult1<R1, TResult>, IFuncInvocation2<R1, R2, TResult> { }

    public interface IFuncInvocationResult2<T, R1, R2, TResult> : IFuncInvocationResult2<R1, R2, TResult>, IFuncInvocationResult1<T, R1, TResult>,IFuncInvocation2<T, R1, R2, TResult> { }

    public interface IFuncInvocationResult2<T1, T2, R1, R2, TResult> : IFuncInvocationResult2<T1, R1, R2, TResult>, IFuncInvocationResult1<T1, T2, R1, TResult>, IFuncInvocation2<T1, T2, R1, R2, TResult> { }

    public interface IFuncInvocationResult2<T1, T2, T3, R1, R2, TResult> : IFuncInvocationResult2<T1, T2, R1, R2, TResult>, IFuncInvocationResult1<T1, T2, T3, R1, TResult>, IFuncInvocation2<T1, T2, T3, R1, R2, TResult> { }

    public interface IFuncInvocationResult2<T1, T2, T3, T4, R1, R2, TResult> : IFuncInvocationResult2<T1, T2, T3, R1, R2, TResult>, IFuncInvocationResult1<T1, T2, T3, T4, R1, TResult>,
        IFuncInvocation2<T1, T2, T3, T4, R1, R2, TResult> { }

    public interface IFuncInvocationResult2<T1, T2, T3, T4, T5, R1, R2, TResult> : IFuncInvocationResult2<T1, T2, T3, T4, R1, R2, TResult>, IFuncInvocationResult1<T1, T2, T3, T4, T5, R1, TResult>,
        IFuncInvocation2<T1, T2, T3, T4, T5, R1, R2, TResult> { }

    public interface IFuncInvocationResult3<R1, R2, R3, TResult> : IFuncInvocationResult2<R1, R2, TResult>, IFuncInvocation3<R1, R2, R3, TResult> { }

    public interface IFuncInvocationResult3<T, R1, R2, R3, TResult> : IFuncInvocationResult3<R1, R2, R3, TResult>, IFuncInvocationResult2<T, R1, R2, TResult>, IFuncInvocation3<T, R1, R2, R3, TResult> { }

    public interface IFuncInvocationResult3<T1, T2, R1, R2, R3, TResult> : IFuncInvocationResult3<T1, R1, R2, R3, TResult>, IFuncInvocationResult2<T1, T2, R1, R2, TResult>, IFuncInvocation3<T1, T2, R1, R2, R3, TResult> { }

    public interface IFuncInvocationResult3<T1, T2, T3, R1, R2, R3, TResult> : IFuncInvocationResult3<T1, T2, R1, R2, R3, TResult>, IFuncInvocationResult2<T1, T2, T3, R1, R2, TResult>,
        IFuncInvocation3<T1, T2, T3, R1, R2, R3, TResult> { }

    public interface IFuncInvocationResult3<T1, T2, T3, T4, R1, R2, R3, TResult> : IFuncInvocationResult3<T1, T2, T3, R1, R2, R3, TResult>, IFuncInvocationResult2<T1, T2, T3, T4, R1, R2, TResult>,
        IFuncInvocation3<T1, T2, T3, T4, R1, R2, R3, TResult> { }

    public interface IFuncInvocationResult4<R1, R2, R3, R4, TResult> : IFuncInvocationResult3<R1, R2, R3, TResult>, IFuncInvocation4<R1, R2, R3, R4, TResult> { }

    public interface IFuncInvocationResult4<T, R1, R2, R3, R4, TResult> : IFuncInvocationResult4<R1, R2, R3, R4, TResult>, IFuncInvocationResult3<T, R1, R2, R3, TResult>, IFuncInvocation4<T, R1, R2, R3, R4, TResult> { }

    public interface IFuncInvocationResult4<T1, T2, R1, R2, R3, R4, TResult> : IFuncInvocationResult4<T1, R1, R2, R3, R4, TResult>, IFuncInvocationResult3<T1, T2, R1, R2, R3, TResult>,
        IFuncInvocation4<T1, T2, R1, R2, R3, R4, TResult> { }

    public interface IFuncInvocationResult4<T1, T2, T3, R1, R2, R3, R4, TResult> : IFuncInvocationResult4<T1, T2, R1, R2, R3, R4, TResult>, IFuncInvocationResult3<T1, T2, T3, R1, R2, R3, TResult>,
        IFuncInvocation4<T1, T2, T3, R1, R2, R3, R4, TResult> { }

    public interface IFuncInvocationResult5<R1, R2, R3, R4, R5, TResult> : IFuncInvocationResult4<R1, R2, R3, R4, TResult>, IFuncInvocation5<R1, R2, R3, R4, R5, TResult> { }

    public interface IFuncInvocationResult5<T, R1, R2, R3, R4, R5, TResult> : IFuncInvocationResult5<R1, R2, R3, R4, R5, TResult>, IFuncInvocationResult4<T, R1, R2, R3, R4, TResult>, IFuncInvocation5<T, R1, R2, R3, R4, R5, TResult> { }

    public interface IFuncInvocationResult5<T1, T2, R1, R2, R3, R4, R5, TResult> : IFuncInvocationResult5<T1, R1, R2, R3, R4, R5, TResult>, IFuncInvocationResult4<T1, T2, R1, R2, R3, R4, TResult>,
        IFuncInvocation5<T1, T2, R1, R2, R3, R4, R5, TResult> { }

    public interface IFuncInvocationResult6<R1, R2, R3, R4, R5, R6, TResult> : IFuncInvocationResult5<R1, R2, R3, R4, R5, TResult>, IFuncInvocation6<R1, R2, R3, R4, R5, R6, TResult> { }

    public interface IFuncInvocationResult6<T, R1, R2, R3, R4, R5, R6, TResult> : IFuncInvocationResult6<R1, R2, R3, R4, R5, R6, TResult>, IFuncInvocationResult5<T, R1, R2, R3, R4, R5, TResult>,
        IFuncInvocation6<T, R1, R2, R3, R4, R5, R6, TResult> { }

    public interface IFuncInvocationResult7<R1, R2, R3, R4, R5, R6, R7, TResult> : IFuncInvocationResult6<R1, R2, R3, R4, R5, R6, TResult>, IFuncInvocation7<R1, R2, R3, R4, R5, R6, R7, TResult> { }
}
