using System;
using System.Collections.Generic;
using System.Text;

namespace FsInfoCat.Test.Helpers
{
    public class FuncTestData<TResult> : IFuncTestData<TResult>
    {
        public TResult ReturnValue { get; }
        object IFuncTestData.ReturnValue { get; }
        public FuncTestData(TResult returnValue)
        {
            ReturnValue = returnValue;
        }
        public static FuncTestData<TResult> FromInvocation(Func<TResult> func) => new FuncTestData<TResult>(func());
    }
    public class FuncTestData<TParam, TResult> : FuncTestData<TResult>, IFuncTestData<TParam, TResult>
    {
        public TParam Input1 { get; }
        public FuncTestData(TParam arg1, TResult returnValue)
            : base(returnValue)
        {
            Input1 = arg1;
        }
        public static FuncTestData<TParam, TResult> FromInvocation(Func<TParam, TResult> func, TParam arg1) => new FuncTestData<TParam, TResult>(arg1, func(arg1));
    }

    public class FuncTestData<TParam1, TParam2, TResult> : FuncTestData<TParam1, TResult>, IFuncTestData<TParam1, TParam2, TResult>
    {
        public TParam2 Input2 { get; }
        public FuncTestData(TParam1 arg1, TParam2 arg2, TResult returnValue)
            : base(arg1, returnValue)
        {
            Input2 = arg2;
        }
        public static FuncTestData<TParam1, TParam2, TResult> FromInvocation(Func<TParam1, TParam2, TResult> func,
                TParam1 arg1, TParam2 arg2)
            => new FuncTestData<TParam1, TParam2, TResult>(arg1, arg2, func(arg1, arg2));
    }

    public class FuncTestData<TParam1, TParam2, TParam3, TResult> : FuncTestData<TParam1, TParam2, TResult>, IFuncTestData<TParam1, TParam2, TParam3, TResult>
    {
        public TParam3 Input3 { get; }
        public FuncTestData(TParam1 arg1, TParam2 arg2, TParam3 arg3, TResult returnValue)
            : base(arg1, arg2, returnValue)
        {
            Input3 = arg3;
        }
        public static FuncTestData<TParam1, TParam2, TParam3, TResult> FromInvocation(Func<TParam1, TParam2, TParam3, TResult> func,
                TParam1 arg1, TParam2 arg2, TParam3 arg3)
            => new FuncTestData<TParam1, TParam2, TParam3, TResult>(arg1, arg2, arg3, func(arg1, arg2, arg3));
    }

    public class FuncTestData<TParam1, TParam2, TParam3, TParam4, TResult> : FuncTestData<TParam1, TParam2, TParam3, TResult>,
        IFuncTestData<TParam1, TParam2, TParam3, TParam4, TResult>
    {
        public TParam4 Input4 { get; }
        public FuncTestData(TParam1 arg1, TParam2 arg2, TParam3 arg3, TParam4 arg4, TResult returnValue)
            : base(arg1, arg2, arg3, returnValue)
        {
            Input4 = arg4;
        }
        public static FuncTestData<TParam1, TParam2, TParam3, TParam4, TResult> FromInvocation(Func<TParam1, TParam2, TParam3, TParam4, TResult> func,
                TParam1 arg1, TParam2 arg2, TParam3 arg3, TParam4 arg4)
            => new FuncTestData<TParam1, TParam2, TParam3, TParam4, TResult>(arg1, arg2, arg3, arg4, func(arg1, arg2, arg3, arg4));
    }

    public class FuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TResult> : FuncTestData<TParam1, TParam2, TParam3, TParam4, TResult>,
        IFuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TResult>
    {
        public TParam5 Input5 { get; }
        public FuncTestData(TParam1 arg1, TParam2 arg2, TParam3 arg3, TParam4 arg4, TParam5 arg5, TResult returnValue)
            : base(arg1, arg2, arg3, arg4, returnValue)
        {
            Input5 = arg5;
        }
        public static FuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TResult> FromInvocation(Func<TParam1, TParam2, TParam3, TParam4, TParam5, TResult> func,
                TParam1 arg1, TParam2 arg2, TParam3 arg3, TParam4 arg4, TParam5 arg5)
            => new FuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TResult>(arg1, arg2, arg3, arg4, arg5, func(arg1, arg2, arg3, arg4, arg5));
    }

    public class FuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TResult> : FuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TResult>,
        IFuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TResult>
    {
        public TParam6 Input6 { get; }
        public FuncTestData(TParam1 arg1, TParam2 arg2, TParam3 arg3, TParam4 arg4, TParam5 arg5, TParam6 arg6, TResult returnValue)
            : base(arg1, arg2, arg3, arg4, arg5, returnValue)
        {
            Input6 = arg6;
        }
        public static FuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TResult> FromInvocation(Func<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TResult> func,
                TParam1 arg1, TParam2 arg2, TParam3 arg3, TParam4 arg4, TParam5 arg5, TParam6 arg6)
            => new FuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TResult>(arg1, arg2, arg3, arg4, arg5, arg6, func(arg1, arg2, arg3, arg4, arg5, arg6));
    }

    public class FuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TResult> :
        FuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TResult>, IFuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TResult>
    {
        public TParam7 Input7 { get; }
        public FuncTestData(TParam1 arg1, TParam2 arg2, TParam3 arg3, TParam4 arg4, TParam5 arg5, TParam6 arg6, TParam7 arg7, TResult returnValue)
            : base(arg1, arg2, arg3, arg4, arg5, arg6, returnValue)
        {
            Input7 = arg7;
        }
        public static FuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TResult> FromInvocation(Func<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TResult> func,
                TParam1 arg1, TParam2 arg2, TParam3 arg3, TParam4 arg4, TParam5 arg5, TParam6 arg6, TParam7 arg7)
            => new FuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TResult>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, func(arg1, arg2, arg3, arg4, arg5, arg6, arg7));
    }

    public class FuncTestData1<TOut, TResult> : FuncTestData<TResult>, IFuncTestData1<TOut, TResult>
    {
        public TOut Output1 { get; }
        public FuncTestData1(TResult returnValue, TOut o)
            : base(returnValue)
        {
            Output1 = o;
        }
        public static FuncTestData1<TOut, TResult> FromInvocation(FuncWithOutput<TOut, TResult> func) => new FuncTestData1<TOut, TResult>(func(out TOut o), o);
    }

    public class FuncTestData1<TParam, TOut, TResult> : FuncTestData1<TOut, TResult>, IFuncTestData1<TParam, TOut, TResult>
    {
        public TParam Input1 { get; }
        public FuncTestData1(TParam arg1, TResult returnValue, TOut o)
            : base(returnValue, o)
        {
            Input1 = arg1;
        }
        public static FuncTestData1<TParam, TOut, TResult> FromInvocation(FuncWithOutput<TParam, TOut, TResult> func, TParam arg1)
            => new FuncTestData1<TParam, TOut, TResult>(arg1, func(arg1, out TOut o), o);
    }

    public class FuncTestData1<TParam1, TParam2, TOut, TResult> : FuncTestData1<TParam1, TOut, TResult>, IFuncTestData1<TParam1, TParam2, TOut, TResult>
    {
        public TParam2 Input2 { get; }
        public FuncTestData1(TParam1 arg1, TParam2 arg2, TResult returnValue, TOut o)
            : base(arg1, returnValue, o)
        {
            Input2 = arg2;
        }
        public static FuncTestData1<TParam1, TParam2, TOut, TResult> FromInvocation(FuncWithOutput<TParam1, TParam2, TOut, TResult> func,
                TParam1 arg1, TParam2 arg2)
            => new FuncTestData1<TParam1, TParam2, TOut, TResult>(arg1, arg2, func(arg1, arg2, out TOut o), o);
    }

    public class FuncTestData1<TParam1, TParam2, TParam3, TOut, TResult> : FuncTestData1<TParam1, TParam2, TOut, TResult>, IFuncTestData1<TParam1, TParam2, TParam3, TOut, TResult>
    {
        public TParam3 Input3 { get; }
        public FuncTestData1(TParam1 arg1, TParam2 arg2, TParam3 arg3, TResult returnValue, TOut o)
            : base(arg1, arg2, returnValue, o)
        {
            Input3 = arg3;
        }
        public static FuncTestData1<TParam1, TParam2, TParam3, TOut, TResult> FromInvocation(FuncWithOutput<TParam1, TParam2, TParam3, TOut, TResult> func,
                TParam1 arg1, TParam2 arg2, TParam3 arg3)
            => new FuncTestData1<TParam1, TParam2, TParam3, TOut, TResult>(arg1, arg2, arg3, func(arg1, arg2, arg3, out TOut o), o);
    }

    public class FuncTestData1<TParam1, TParam2, TParam3, TParam4, TOut, TResult> : FuncTestData1<TParam1, TParam2, TParam3, TOut, TResult>,
        IFuncTestData1<TParam1, TParam2, TParam3, TParam4, TOut, TResult>
    {
        public TParam4 Input4 { get; }
        public FuncTestData1(TParam1 arg1, TParam2 arg2, TParam3 arg3, TParam4 arg4, TResult returnValue, TOut o)
            : base(arg1, arg2, arg3, returnValue, o)
        {
            Input4 = arg4;
        }
        public static FuncTestData1<TParam1, TParam2, TParam3, TParam4, TOut, TResult> FromInvocation(FuncWithOutput<TParam1, TParam2, TParam3, TParam4, TOut, TResult> func,
                TParam1 arg1, TParam2 arg2, TParam3 arg3, TParam4 arg4)
            => new FuncTestData1<TParam1, TParam2, TParam3, TParam4, TOut, TResult>(arg1, arg2, arg3, arg4, func(arg1, arg2, arg3, arg4, out TOut o), o);
    }

    public class FuncTestData1<TParam1, TParam2, TParam3, TParam4, TParam5, TOut, TResult> : FuncTestData1<TParam1, TParam2, TParam3, TParam4, TOut, TResult>,
        IFuncTestData1<TParam1, TParam2, TParam3, TParam4, TParam5, TOut, TResult>
    {
        public TParam5 Input5 { get; }
        public FuncTestData1(TParam1 arg1, TParam2 arg2, TParam3 arg3, TParam4 arg4, TParam5 arg5, TResult returnValue, TOut o)
            : base(arg1, arg2, arg3, arg4, returnValue, o)
        {
            Input5 = arg5;
        }
        public static FuncTestData1<TParam1, TParam2, TParam3, TParam4, TParam5, TOut, TResult> FromInvocation(FuncWithOutput<TParam1, TParam2, TParam3, TParam4, TParam5, TOut, TResult> func,
                TParam1 arg1, TParam2 arg2, TParam3 arg3, TParam4 arg4, TParam5 arg5)
            => new FuncTestData1<TParam1, TParam2, TParam3, TParam4, TParam5, TOut, TResult>(arg1, arg2, arg3, arg4, arg5, func(arg1, arg2, arg3, arg4, arg5, out TOut o), o);
    }

    public class FuncTestData1<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TOut, TResult> : FuncTestData1<TParam1, TParam2, TParam3, TParam4, TParam5, TOut, TResult>,
        IFuncTestData1<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TOut, TResult>
    {
        public TParam6 Input6 { get; }
        public FuncTestData1(TParam1 arg1, TParam2 arg2, TParam3 arg3, TParam4 arg4, TParam5 arg5, TParam6 arg6, TResult returnValue, TOut o)
            : base(arg1, arg2, arg3, arg4, arg5, returnValue, o)
        {
            Input6 = arg6;
        }
        public static FuncTestData1<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TOut, TResult> FromInvocation(FuncWithOutput<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TOut, TResult> func,
                TParam1 arg1, TParam2 arg2, TParam3 arg3, TParam4 arg4, TParam5 arg5, TParam6 arg6)
            => new FuncTestData1<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TOut, TResult>(arg1, arg2, arg3, arg4, arg5, arg6, func(arg1, arg2, arg3, arg4, arg5, arg6,
                out TOut o), o);
    }

    public class FuncTestData2<TOut1, TOut2, TResult> : FuncTestData1<TOut1, TResult>, IFuncTestData2<TOut1, TOut2, TResult>
    {
        public TOut2 Output2 { get; }
        public FuncTestData2(TResult returnValue, TOut1 o1, TOut2 o2)
            : base(returnValue, o1)
        {
            Output2 = o2;
        }
        public static FuncTestData2<TOut1, TOut2, TResult> FromInvocation(FuncWithOutput2<TOut1, TOut2, TResult> func) =>
            new FuncTestData2<TOut1, TOut2, TResult>(func(out TOut1 o1, out TOut2 o2), o1, o2);
    }

    public class FuncTestData2<TParam, TOut1, TOut2, TResult> : FuncTestData2<TOut1, TOut2, TResult>, IFuncTestData2<TParam, TOut1, TOut2, TResult>
    {
        public TParam Input1 { get; }
        public FuncTestData2(TParam arg1, TResult returnValue, TOut1 o1, TOut2 o2)
            : base(returnValue, o1, o2)
        {
            Input1 = arg1;
        }
        public static FuncTestData2<TParam, TOut1, TOut2, TResult> FromInvocation(FuncWithOutput2<TParam, TOut1, TOut2, TResult> func, TParam arg1)
            => new FuncTestData2<TParam, TOut1, TOut2, TResult>(arg1, func(arg1, out TOut1 o1, out TOut2 o2), o1, o2);
    }

    public class FuncTestData2<TParam1, TParam2, TOut1, TOut2, TResult> : FuncTestData2<TParam1, TOut1, TOut2, TResult>, IFuncTestData2<TParam1, TParam2, TOut1, TOut2, TResult>
    {
        public TParam2 Input2 { get; }
        public FuncTestData2(TParam1 arg1, TParam2 arg2, TResult returnValue, TOut1 o1, TOut2 o2)
            : base(arg1, returnValue, o1, o2)
        {
            Input2 = arg2;
        }
        public static FuncTestData2<TParam1, TParam2, TOut1, TOut2, TResult> FromInvocation(FuncWithOutput2<TParam1, TParam2, TOut1, TOut2, TResult> func,
                TParam1 arg1, TParam2 arg2)
            => new FuncTestData2<TParam1, TParam2, TOut1, TOut2, TResult>(arg1, arg2, func(arg1, arg2, out TOut1 o1, out TOut2 o2), o1, o2);
    }

    public class FuncTestData2<TParam1, TParam2, TParam3, TOut1, TOut2, TResult> : FuncTestData2<TParam1, TParam2, TOut1, TOut2, TResult>,
        IFuncTestData2<TParam1, TParam2, TParam3, TOut1, TOut2, TResult>
    {
        public TParam3 Input3 { get; }
        public FuncTestData2(TParam1 arg1, TParam2 arg2, TParam3 arg3, TResult returnValue, TOut1 o1, TOut2 o2)
            : base(arg1, arg2, returnValue, o1, o2)
        {
            Input3 = arg3;
        }
        public static FuncTestData2<TParam1, TParam2, TParam3, TOut1, TOut2, TResult> FromInvocation(FuncWithOutput2<TParam1, TParam2, TParam3, TOut1, TOut2, TResult> func,
                TParam1 arg1, TParam2 arg2, TParam3 arg3)
            => new FuncTestData2<TParam1, TParam2, TParam3, TOut1, TOut2, TResult>(arg1, arg2, arg3, func(arg1, arg2, arg3, out TOut1 o1, out TOut2 o2), o1, o2);
    }

    public class FuncTestData2<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TResult> : FuncTestData2<TParam1, TParam2, TParam3, TOut1, TOut2, TResult>,
        IFuncTestData2<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TResult>
    {
        public TParam4 Input4 { get; }
        public FuncTestData2(TParam1 arg1, TParam2 arg2, TParam3 arg3, TParam4 arg4, TResult returnValue, TOut1 o1, TOut2 o2)
            : base(arg1, arg2, arg3, returnValue, o1, o2)
        {
            Input4 = arg4;
        }
        public static FuncTestData2<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TResult> FromInvocation(FuncWithOutput2<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TResult> func,
                TParam1 arg1, TParam2 arg2, TParam3 arg3, TParam4 arg4)
            => new FuncTestData2<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TResult>(arg1, arg2, arg3, arg4, func(arg1, arg2, arg3, arg4,
                out TOut1 o1, out TOut2 o2), o1, o2);
    }

    public class FuncTestData2<TParam1, TParam2, TParam3, TParam4, TParam5, TOut1, TOut2, TResult> :
        FuncTestData2<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TResult>, IFuncTestData2<TParam1, TParam2, TParam3, TParam4, TParam5, TOut1, TOut2, TResult>
    {
        public TParam5 Input5 { get; }
        public FuncTestData2(TParam1 arg1, TParam2 arg2, TParam3 arg3, TParam4 arg4, TParam5 arg5, TResult returnValue, TOut1 o1, TOut2 o2)
            : base(arg1, arg2, arg3, arg4, returnValue, o1, o2)
        {
            Input5 = arg5;
        }
        public static FuncTestData2<TParam1, TParam2, TParam3, TParam4, TParam5, TOut1, TOut2, TResult> FromInvocation(FuncWithOutput2<TParam1, TParam2, TParam3, TParam4, TParam5, TOut1, TOut2, TResult> func,
                TParam1 arg1, TParam2 arg2, TParam3 arg3, TParam4 arg4, TParam5 arg5)
            => new FuncTestData2<TParam1, TParam2, TParam3, TParam4, TParam5, TOut1, TOut2, TResult>(arg1, arg2, arg3, arg4, arg5, func(arg1, arg2, arg3, arg4, arg5,
                out TOut1 o1, out TOut2 o2), o1, o2);
    }

    public class FuncTestData3<TOut1, TOut2, TOut3, TResult> : FuncTestData2<TOut1, TOut2, TResult>, IFuncTestData3<TOut1, TOut2, TOut3, TResult>
    {
        public TOut3 Output3 { get; }
        public FuncTestData3(TResult returnValue, TOut1 o1, TOut2 o2, TOut3 o3)
            : base(returnValue, o1, o2)
        {
            Output3 = o3;
        }
        public static FuncTestData3<TOut1, TOut2, TOut3, TResult> FromInvocation(FuncWithOutput3<TOut1, TOut2, TOut3, TResult> func)
            => new FuncTestData3<TOut1, TOut2, TOut3, TResult>(func(out TOut1 o1, out TOut2 o2, out TOut3 o3), o1, o2, o3);
    }

    public class FuncTestData3<TParam, TOut1, TOut2, TOut3, TResult> : FuncTestData3<TOut1, TOut2, TOut3, TResult>, IFuncTestData3<TParam, TOut1, TOut2, TOut3, TResult>
    {
        public TParam Input1 { get; }
        public FuncTestData3(TParam arg1, TResult returnValue, TOut1 o1, TOut2 o2, TOut3 o3)
            : base(returnValue, o1, o2, o3)
        {
            Input1 = arg1;
        }
        public static FuncTestData3<TParam, TOut1, TOut2, TOut3, TResult> FromInvocation(FuncWithOutput3<TParam, TOut1, TOut2, TOut3, TResult> func,
                TParam arg1)
            => new FuncTestData3<TParam, TOut1, TOut2, TOut3, TResult>(arg1, func(arg1, out TOut1 o1, out TOut2 o2, out TOut3 o3), o1, o2, o3);
    }

    public class FuncTestData3<TParam1, TParam2, TOut1, TOut2, TOut3, TResult> : FuncTestData3<TParam1, TOut1, TOut2, TOut3, TResult>,
        IFuncTestData3<TParam1, TParam2, TOut1, TOut2, TOut3, TResult>
    {
        public TParam2 Input2 { get; }
        public FuncTestData3(TParam1 arg1, TParam2 arg2, TResult returnValue, TOut1 o1, TOut2 o2, TOut3 o3)
            : base(arg1, returnValue, o1, o2, o3)
        {
            Input2 = arg2;
        }
        public static FuncTestData3<TParam1, TParam2, TOut1, TOut2, TOut3, TResult> FromInvocation(FuncWithOutput3<TParam1, TParam2, TOut1, TOut2, TOut3, TResult> func,
                TParam1 arg1, TParam2 arg2)
            => new FuncTestData3<TParam1, TParam2, TOut1, TOut2, TOut3, TResult>(arg1, arg2, func(arg1, arg2, out TOut1 o1, out TOut2 o2, out TOut3 o3), o1, o2, o3);
    }

    public class FuncTestData3<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TResult> : FuncTestData3<TParam1, TParam2, TOut1, TOut2, TOut3, TResult>,
        IFuncTestData3<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TResult>
    {
        public TParam3 Input3 { get; }
        public FuncTestData3(TParam1 arg1, TParam2 arg2, TParam3 arg3, TResult returnValue, TOut1 o1, TOut2 o2, TOut3 o3)
            : base(arg1, arg2, returnValue, o1, o2, o3)
        {
            Input3 = arg3;
        }
        public static FuncTestData3<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TResult> FromInvocation(FuncWithOutput3<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TResult> func,
                TParam1 arg1, TParam2 arg2, TParam3 arg3)
            => new FuncTestData3<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TResult>(arg1, arg2, arg3, func(arg1, arg2, arg3, out TOut1 o1, out TOut2 o2, out TOut3 o3), o1, o2, o3);
    }

    public class FuncTestData3<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TOut3, TResult> :
        FuncTestData3<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TResult>, IFuncTestData3<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TOut3, TResult>
    {
        public TParam4 Input4 { get; }
        public FuncTestData3(TParam1 arg1, TParam2 arg2, TParam3 arg3, TParam4 arg4, TResult returnValue, TOut1 o1, TOut2 o2, TOut3 o3)
            : base(arg1, arg2, arg3, returnValue, o1, o2, o3)
        {
            Input4 = arg4;
        }
        public static FuncTestData3<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TOut3, TResult> FromInvocation(FuncWithOutput3<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TOut3, TResult> func,
                TParam1 arg1, TParam2 arg2, TParam3 arg3, TParam4 arg4)
            => new FuncTestData3<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TOut3, TResult>(arg1, arg2, arg3, arg4, func(arg1, arg2, arg3, arg4, out TOut1 o1, out TOut2 o2,
                out TOut3 o3), o1, o2, o3);
    }

    public class FuncTestData4<TOut1, TOut2, TOut3, TOut4, TResult> : FuncTestData3<TOut1, TOut2, TOut3, TResult>, IFuncTestData4<TOut1, TOut2, TOut3, TOut4, TResult>
    {
        public TOut4 Output4 { get; }
        public FuncTestData4(TResult returnValue, TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4)
            : base(returnValue, o1, o2, o3)
        {
            Output4 = o4;
        }
        public static FuncTestData4<TOut1, TOut2, TOut3, TOut4, TResult> FromInvocation(FuncWithOutput4<TOut1, TOut2, TOut3, TOut4, TResult> func)
            => new FuncTestData4<TOut1, TOut2, TOut3, TOut4, TResult>(func(out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4), o1, o2, o3, o4);
    }

    public class FuncTestData4<TParam, TOut1, TOut2, TOut3, TOut4, TResult> : FuncTestData4<TOut1, TOut2, TOut3, TOut4, TResult>,
        IFuncTestData4<TParam, TOut1, TOut2, TOut3, TOut4, TResult>
    {
        public TParam Input1 { get; }
        public FuncTestData4(TParam arg1, TResult returnValue, TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4) : base(returnValue, o1, o2, o3, o4)
        {
            Input1 = arg1;
        }
        public static FuncTestData4<TParam, TOut1, TOut2, TOut3, TOut4, TResult> FromInvocation(FuncWithOutput4<TParam, TOut1, TOut2, TOut3, TOut4, TResult> func,
                TParam arg1)
            => new FuncTestData4<TParam, TOut1, TOut2, TOut3, TOut4, TResult>(arg1, func(arg1, out TOut1 o1, out TOut2 o2, out TOut3 o3,
                out TOut4 o4), o1, o2, o3, o4);
    }

    public class FuncTestData4<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TResult> : FuncTestData4<TParam1, TOut1, TOut2, TOut3, TOut4, TResult>,
        IFuncTestData4<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TResult>
    {
        public TParam2 Input2 { get; }
        public FuncTestData4(TParam1 arg1, TParam2 arg2, TResult returnValue, TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4) : base(arg1, returnValue, o1, o2, o3, o4)
        {
            Input2 = arg2;
        }
        public static FuncTestData4<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TResult> FromInvocation(FuncWithOutput4<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TResult> func,
                TParam1 arg1, TParam2 arg2)
            => new FuncTestData4<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TResult>(arg1, arg2, func(arg1, arg2, out TOut1 o1, out TOut2 o2, out TOut3 o3,
                out TOut4 o4), o1, o2, o3, o4);
    }

    public class FuncTestData4<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TOut4, TResult> : FuncTestData4<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TResult>,
        IFuncTestData4<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TOut4, TResult>
    {
        public TParam3 Input3 { get; }
        public FuncTestData4(TParam1 arg1, TParam2 arg2, TParam3 arg3, TResult returnValue, TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4) : base(arg1, arg2, returnValue, o1, o2, o3, o4)
        {
            Input3 = arg3;
        }
        public static FuncTestData4<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TOut4, TResult> FromInvocation(FuncWithOutput4<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TOut4, TResult> func,
                TParam1 arg1, TParam2 arg2, TParam3 arg3)
            => new FuncTestData4<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TOut4, TResult>(arg1, arg2, arg3, func(arg1, arg2, arg3, out TOut1 o1, out TOut2 o2, out TOut3 o3,
                out TOut4 o4), o1, o2, o3, o4);
    }

    public class FuncTestData5<TOut1, TOut2, TOut3, TOut4, TOut5, TResult> : FuncTestData4<TOut1, TOut2, TOut3, TOut4, TResult>,
        IFuncTestData5<TOut1, TOut2, TOut3, TOut4, TOut5, TResult>
    {
        public TOut5 Output5 { get; }
        public FuncTestData5(TResult returnValue, TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4, TOut5 o5)
            : base(returnValue, o1, o2, o3, o4)
        {
            Output5 = o5;
        }
        public static FuncTestData5<TOut1, TOut2, TOut3, TOut4, TOut5, TResult> FromInvocation(FuncWithOutput5<TOut1, TOut2, TOut3, TOut4, TOut5, TResult> func)
            => new FuncTestData5<TOut1, TOut2, TOut3, TOut4, TOut5, TResult>(func(out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4, out TOut5 o5),
                o1, o2, o3, o4, o5);
    }

    public class FuncTestData5<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TResult> : FuncTestData5<TOut1, TOut2, TOut3, TOut4, TOut5, TResult>,
        IFuncTestData5<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TResult>
    {
        public TParam Input1 { get; }
        public FuncTestData5(TParam arg1, TResult returnValue, TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4, TOut5 o5) : base(returnValue, o1, o2, o3, o4, o5)
        {
            Input1 = arg1;
        }
        public static FuncTestData5<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TResult> FromInvocation(FuncWithOutput5<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TResult> func,
                TParam arg1)
            => new FuncTestData5<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TResult>(arg1, func(arg1, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4, out TOut5 o5),
                o1, o2, o3, o4, o5);
    }

    public class FuncTestData5<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TOut5, TResult> : FuncTestData5<TParam1, TOut1, TOut2, TOut3, TOut4, TOut5, TResult>,
        IFuncTestData5<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TOut5, TResult>
    {
        public TParam2 Input2 { get; }
        public FuncTestData5(TParam1 arg1, TParam2 arg2, TResult returnValue, TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4, TOut5 o5) : base(arg1, returnValue, o1, o2, o3, o4, o5)
        {
            Input2 = arg2;
        }
        public static FuncTestData5<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TOut5, TResult> FromInvocation(FuncWithOutput5<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TOut5, TResult> func,
                TParam1 arg1, TParam2 arg2)
            => new FuncTestData5<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TOut5, TResult>(arg1, arg2, func(arg1, arg2, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4,
                out TOut5 o5), o1, o2, o3, o4, o5);
    }

    public class FuncTestData6<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult> : FuncTestData5<TOut1, TOut2, TOut3, TOut4, TOut5, TResult>,
        IFuncTestData6<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult>
    {
        public TOut6 Output6 { get; }
        public FuncTestData6(TResult returnValue, TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4, TOut5 o5, TOut6 o6)
            : base(returnValue, o1, o2, o3, o4, o5)
        {
            Output6 = o6;
        }
        public static FuncTestData6<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult> FromInvocation(FuncWithOutput6<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult> func)
            => new FuncTestData6<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult>(func(out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4, out TOut5 o5, out TOut6 o6),
                o1, o2, o3, o4, o5, o6);
    }

    public class FuncTestData6<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult> : FuncTestData6<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult>,
        IFuncTestData6<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult>
    {
        public TParam Input1 { get; }
        public FuncTestData6(TParam arg1, TResult returnValue, TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4, TOut5 o5, TOut6 o6) : base(returnValue, o1, o2, o3, o4, o5, o6)
        {
            Input1 = arg1;
        }
        public static FuncTestData6<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult> FromInvocation(FuncWithOutput6<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult> func, TParam p)
            => new FuncTestData6<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult>(p, func(p, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4, out TOut5 o5, out TOut6 o6),
                o1, o2, o3, o4, o5, o6);
    }

    public class FuncTestData7<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult> : FuncTestData6<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult>,
        IFuncTestData7<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult>
    {
        public TOut7 Output7 { get; }
        public FuncTestData7(TResult returnValue, TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4, TOut5 o5, TOut6 o6, TOut7 o7)
            : base(returnValue, o1, o2, o3, o4, o5, o6)
        {
            Output7 = o7;
        }
        public static FuncTestData7<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult> FromInvocation(FuncWithOutput7<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult> func)
            => new FuncTestData7<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult>(func(out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4, out TOut5 o5, out TOut6 o6, out TOut7 o7),
                o1, o2, o3, o4, o5, o6, o7);
    }
}
