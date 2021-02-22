using System;
using System.Collections.Generic;
using System.Text;

namespace FsInfoCat.Test.Helpers
{
    public interface IInvocationResult : IInvocationInput
    {
        bool WasInvoked { get; }
    }

    public interface IInvocationResult<T> : IInvocationResult, IInvocationInput<T> { }

    public interface IInvocationResult<T1, T2> : IInvocationResult<T1>, IInvocationInput<T1, T2> { }

    public interface IInvocationResult<T1, T2, T3> : IInvocationResult<T1, T2>, IInvocationInput<T1, T2, T3> { }

    public interface IInvocationResult<T1, T2, T3, T4> : IInvocationResult<T1, T2, T3>, IInvocationInput<T1, T2, T3, T4> { }

    public interface IInvocationResult<T1, T2, T3, T4, T5> : IInvocationResult<T1, T2, T3, T4>, IInvocationInput<T1, T2, T3, T4, T5> { }

    public interface IInvocationResult<T1, T2, T3, T4, T5, T6> : IInvocationResult<T1, T2, T3, T4, T5>, IInvocationInput<T1, T2, T3, T4, T5, T6> { }

    public interface IInvocationResult<T1, T2, T3, T4, T5, T6, T7> : IInvocationResult<T1, T2, T3, T4, T5, T6>, IInvocationInput<T1, T2, T3, T4, T5, T6, T7> { }

    public interface IInvocationResult1<R> : IInvocationResult, IInvocationOutput<R> { }

    public interface IInvocationResult1<T, R> : IInvocationResult1<R>, IInvocationResult<T> { }

    public interface IInvocationResult1<T1, T2, R> : IInvocationResult1<T1, R>, IInvocationResult<T1, T2> { }

    public interface IInvocationResult1<T1, T2, T3, R> : IInvocationResult1<T1, T2, R>, IInvocationResult<T1, T2, T3> { }

    public interface IInvocationResult1<T1, T2, T3, T4, R> : IInvocationResult1<T1, T2, T3, R>, IInvocationResult<T1, T2, T3, T4> { }

    public interface IInvocationResult1<T1, T2, T3, T4, T5, R> : IInvocationResult1<T1, T2, T3, T4, R>, IInvocationResult<T1, T2, T3, T4, T5> { }

    public interface IInvocationResult1<T1, T2, T3, T4, T5, T6, R> : IInvocationResult1<T1, T2, T3, T4, T5, R>, IInvocationInput<T1, T2, T3, T4, T5, T6> { }

    public interface IInvocationResult2<R1, R2> : IInvocationResult1<R1>, IInvocationOutput<R1, R2> { }

    public interface IInvocationResult2<T, R1, R2> : IInvocationResult2<R1, R2>, IInvocationResult1<T, R1> { }

    public interface IInvocationResult2<T1, T2, R1, R2> : IInvocationResult2<T1, R1, R2>, IInvocationResult1<T1, T2, R1> { }

    public interface IInvocationResult2<T1, T2, T3, R1, R2> : IInvocationResult2<T1, T2, R1, R2>, IInvocationResult1<T1, T2, T3, R1> { }

    public interface IInvocationResult2<T1, T2, T3, T4, R1, R2> : IInvocationResult2<T1, T2, T3, R1, R2>, IInvocationResult1<T1, T2, T3, T4, R1> { }

    public interface IInvocationResult2<T1, T2, T3, T4, T5, R1, R2> : IInvocationResult2<T1, T2, T3, T4, R1, R2>, IInvocationResult1<T1, T2, T3, T4, T5, R1> { }

    public interface IInvocationResult3<R1, R2, R3> : IInvocationResult2<R1, R2>, IInvocationOutput<R1, R2, R3> { }

    public interface IInvocationResult3<T, R1, R2, R3> : IInvocationResult3<R1, R2, R3>, IInvocationResult2<T, R1, R2> { }

    public interface IInvocationResult3<T1, T2, R1, R2, R3> : IInvocationResult3<T1, R1, R2, R3>, IInvocationResult2<T1, T2, R1, R2> { }

    public interface IInvocationResult3<T1, T2, T3, R1, R2, R3> : IInvocationResult3<T1, T2, R1, R2, R3>, IInvocationResult2<T1, T2, T3, R1, R2> { }

    public interface IInvocationResult3<T1, T2, T3, T4, R1, R2, R3> : IInvocationResult3<T1, T2, T3, R1, R2, R3>, IInvocationResult2<T1, T2, T3, T4, R1, R2> { }

    public interface IInvocationResult4<R1, R2, R3, R4> : IInvocationResult3<R1, R2, R3>, IInvocationOutput<R1, R2, R3, R4> { }

    public interface IInvocationResult4<T, R1, R2, R3, R4> : IInvocationResult4<R1, R2, R3, R4>, IInvocationResult3<T, R1, R2, R3> { }

    public interface IInvocationResult4<T1, T2, R1, R2, R3, R4> : IInvocationResult4<T1, R1, R2, R3, R4>, IInvocationResult3<T1, T2, R1, R2, R3> { }

    public interface IInvocationResult4<T1, T2, T3, R1, R2, R3, R4> : IInvocationResult4<T1, T2, R1, R2, R3, R4>, IInvocationResult3<T1, T2, T3, R1, R2, R3> { }

    public interface IInvocationResult5<R1, R2, R3, R4, R5> : IInvocationResult4<R1, R2, R3, R4>, IInvocationOutput<R1, R2, R3, R4, R5> { }

    public interface IInvocationResult5<T, R1, R2, R3, R4, R5> : IInvocationResult5<R1, R2, R3, R4, R5>, IInvocationResult4<T, R1, R2, R3, R4> { }

    public interface IInvocationResult5<T1, T2, R1, R2, R3, R4, R5> : IInvocationResult5<T1, R1, R2, R3, R4, R5>, IInvocationResult4<T1, T2, R1, R2, R3, R4> { }

    public interface IInvocationResult6<R1, R2, R3, R4, R5, R6> : IInvocationResult5<R1, R2, R3, R4, R5>, IInvocationOutput<R1, R2, R3, R4, R5, R6> { }

    public interface IInvocationResult6<T, R1, R2, R3, R4, R5, R6> : IInvocationResult6<R1, R2, R3, R4, R5, R6>, IInvocationResult5<T, R1, R2, R3, R4, R5> { }

    public interface IInvocationResult7<R1, R2, R3, R4, R5, R6, R7> : IInvocationResult6<R1, R2, R3, R4, R5, R6>, IInvocationOutput<R1, R2, R3, R4, R5, R6, R7> { }
}
