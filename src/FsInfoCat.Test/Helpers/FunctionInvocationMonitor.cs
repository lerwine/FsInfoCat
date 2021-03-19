using System;

namespace FsInfoCat.Test.Helpers
{
    public class FunctionInvocationMonitor<TResult>
    {
        public bool WasInvoked { get; set; }
        public TResult ReturnValue { get; set; }

        public void Apply(TResult result)
        {
            ReturnValue = result;
            WasInvoked = true;
        }

        public Func<TResult> CreateProxy(Func<TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return () => Invoke(func);
        }

        public Func<TParam, TResult> CreateProxy<TParam>(Func<TParam, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return p => Invoke(func, p);
        }

        public Func<TParam1, TParam2, TResult> CreateProxy<TParam1, TParam2>(Func<TParam1, TParam2, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return (p1, p2) => Invoke(func, p1, p2);
        }

        public Func<TParam1, TParam2, TParam3, TResult> CreateProxy<TParam1, TParam2, TParam3>(Func<TParam1, TParam2, TParam3, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return (p1, p2, p3) => Invoke(func, p1, p2, p3);
        }

        public Func<TParam1, TParam2, TParam3, TParam4, TResult> CreateProxy<TParam1, TParam2, TParam3, TParam4>(Func<TParam1, TParam2, TParam3, TParam4, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return (p1, p2, p3, p4) => Invoke(func, p1, p2, p3, p4);
        }

        public Func<TParam1, TParam2, TParam3, TParam4, TParam5, TResult> CreateProxy<TParam1, TParam2, TParam3, TParam4, TParam5>(Func<TParam1, TParam2, TParam3, TParam4, TParam5, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return (p1, p2, p3, p4, p5) => Invoke(func, p1, p2, p3, p4, p5);
        }

        public Func<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TResult> CreateProxy<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6>(Func<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return (p1, p2, p3, p4, p5, p6) => Invoke(func, p1, p2, p3, p4, p5, p6);
        }

        public Func<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TResult> CreateProxy<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7>(Func<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return (p1, p2, p3, p4, p5, p6, p7) => Invoke(func, p1, p2, p3, p4, p5, p6, p7);
        }

        public TResult Invoke(Func<TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func();
            Apply(result);
            return result;
        }

        public TResult Invoke<TParam>(Func<TParam, TResult> func, TParam p)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(p);
            Apply(result);
            return result;
        }

        public TResult Invoke<TParam1, TParam2>(Func<TParam1, TParam2, TResult> func, TParam1 p1, TParam2 p2)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(p1, p2);
            Apply(result);
            return result;
        }

        public TResult Invoke<TParam1, TParam2, TParam3>(Func<TParam1, TParam2, TParam3, TResult> func, TParam1 p1, TParam2 p2, TParam3 p3)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(p1, p2, p3);
            Apply(result);
            return result;
        }

        public TResult Invoke<TParam1, TParam2, TParam3, TParam4>(Func<TParam1, TParam2, TParam3, TParam4, TResult> func, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(p1, p2, p3, p4);
            Apply(result);
            return result;
        }

        public TResult Invoke<TParam1, TParam2, TParam3, TParam4, TParam5>(Func<TParam1, TParam2, TParam3, TParam4, TParam5, TResult> func, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4,
            TParam5 p5)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(p1, p2, p3, p4, p5);
            Apply(result);
            return result;
        }

        public TResult Invoke<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6>(Func<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TResult> func, TParam1 p1,
            TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(p1, p2, p3, p4, p5, p6);
            Apply(result);
            return result;
        }

        public TResult Invoke<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7>(Func<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TResult> func, TParam1 p1,
            TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6, TParam7 p7)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(p1, p2, p3, p4, p5, p6, p7);
            Apply(result);
            return result;
        }

        public IFuncInvocationResult<TResult> ToResult() => (WasInvoked) ? new FuncInvocationResult<TResult>(ReturnValue) : new FuncInvocationResult<TResult>();
    }

    public class FunctionInvocationMonitor<TOut, TResult>
    {
        public bool WasInvoked { get; set; }
        public TOut Output1 { get; set; }
        public TResult ReturnValue { get; set; }

        public void Apply(TResult result, TOut o)
        {
            ReturnValue = result;
            Output1 = o;
            WasInvoked = true;
        }

        public Func<TOut, TResult> CreateProxy(Func<TOut, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return o1 => Invoke(func, o1);
        }

        public FuncWithOutput<TOut, TResult> CreateProxy(FuncWithOutput<TOut, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return (out TOut o1) => Invoke(func, out o1);
        }

        public FuncWithOutput<TParam, TOut, TResult> CreateProxy<TParam>(FuncWithOutput<TParam, TOut, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return (TParam p, out TOut o1) => Invoke(func, p, out o1);
        }

        public FuncWithOutput<TParam1, TParam2, TOut, TResult> CreateProxy<TParam1, TParam2>(FuncWithOutput<TParam1, TParam2, TOut, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return (TParam1 p1, TParam2 p2, out TOut o1) => Invoke(func, p1, p2, out o1);
        }

        public FuncWithOutput<TParam1, TParam2, TParam3, TOut, TResult> CreateProxy<TParam1, TParam2, TParam3>(FuncWithOutput<TParam1, TParam2, TParam3, TOut, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return (TParam1 p1, TParam2 p2, TParam3 p3, out TOut o1) => Invoke(func, p1, p2, p3, out o1);
        }

        public FuncWithOutput<TParam1, TParam2, TParam3, TParam4, TOut, TResult> CreateProxy<TParam1, TParam2, TParam3, TParam4>(FuncWithOutput<TParam1, TParam2, TParam3, TParam4, TOut, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return (TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, out TOut o1) => Invoke(func, p1, p2, p3, p4, out o1);
        }

        public FuncWithOutput<TParam1, TParam2, TParam3, TParam4, TParam5, TOut, TResult> CreateProxy<TParam1, TParam2, TParam3, TParam4, TParam5>(FuncWithOutput<TParam1, TParam2, TParam3, TParam4, TParam5, TOut, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return (TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, out TOut o1) => Invoke(func, p1, p2, p3, p4, p5, out o1);
        }

        public FuncWithOutput<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TOut, TResult> CreateProxy<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6>(FuncWithOutput<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TOut, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return (TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6, out TOut o1) => Invoke(func, p1, p2, p3, p4, p5, p6, out o1);
        }

        public TResult Invoke(Func<TOut, TResult> func, TOut o)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(o);
            Apply(result, o);
            return result;
        }

        public TResult Invoke(FuncWithOutput<TOut, TResult> func, out TOut o)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(out o);
            Apply(result, o);
            return result;
        }

        public TResult Invoke<TParam>(FuncWithOutput<TParam, TOut, TResult> func, TParam p, out TOut o)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(p, out o);
            Apply(result, o);
            return result;
        }

        public TResult Invoke<TParam1, TParam2>(FuncWithOutput<TParam1, TParam2, TOut, TResult> func, TParam1 p1, TParam2 p2, out TOut o)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(p1, p2, out o);
            Apply(result, o);
            return result;
        }

        public TResult Invoke<TParam1, TParam2, TParam3>(FuncWithOutput<TParam1, TParam2, TParam3, TOut, TResult> func, TParam1 p1, TParam2 p2, TParam3 p3, out TOut o)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(p1, p2, p3, out o);
            Apply(result, o);
            return result;
        }

        public TResult Invoke<TParam1, TParam2, TParam3, TParam4>(FuncWithOutput<TParam1, TParam2, TParam3, TParam4, TOut, TResult> func, TParam1 p1,
            TParam2 p2, TParam3 p3, TParam4 p4, out TOut o)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(p1, p2, p3, p4, out o);
            Apply(result, o);
            return result;
        }

        public TResult Invoke<TParam1, TParam2, TParam3, TParam4, TParam5>(FuncWithOutput<TParam1, TParam2, TParam3, TParam4, TParam5, TOut, TResult> func, TParam1 p1,
            TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, out TOut o)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(p1, p2, p3, p4, p5, out o);
            Apply(result, o);
            return result;
        }

        public TResult Invoke<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6>(FuncWithOutput<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TOut, TResult> func, TParam1 p1,
            TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6, out TOut o)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(p1, p2, p3, p4, p5, p6, out o);
            Apply(result, o);
            return result;
        }

        public IFuncInvocationResult<TOut, TResult> ToResult() => (WasInvoked) ? new FuncInvocationResult<TOut, TResult>(ReturnValue, Output1) : new FuncInvocationResult<TOut, TResult>();
    }

    public class FunctionInvocationMonitor<TOut1, TOut2, TResult>
    {
        public bool WasInvoked { get; set; }
        public TOut1 Output1 { get; set; }
        public TOut2 Output2 { get; set; }
        public TResult ReturnValue { get; set; }

        public void Apply(TResult result, TOut1 o1, TOut2 o2)
        {
            ReturnValue = result;
            Output1 = o1;
            Output2 = o2;
            WasInvoked = true;
        }

        public Func<TOut1, TOut2, TResult> CreateProxy(Func<TOut1, TOut2, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return (TOut1 o1, TOut2 o2) => Invoke(func, o1, o2);
        }

        public FuncWithOutput2<TOut1, TOut2, TResult> CreateProxy(FuncWithOutput2<TOut1, TOut2, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return (out TOut1 o1, out TOut2 o2) => Invoke(func, out o1, out o2);
        }

        public FuncWithOutput2<TParam, TOut1, TOut2, TResult> CreateProxy<TParam>(FuncWithOutput2<TParam, TOut1, TOut2, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return (TParam p, out TOut1 o1, out TOut2 o2) => Invoke(func, p, out o1, out o2);
        }

        public FuncWithOutput2<TParam1, TParam2, TOut1, TOut2, TResult> CreateProxy<TParam1, TParam2>(FuncWithOutput2<TParam1, TParam2, TOut1, TOut2, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return (TParam1 p1, TParam2 p2, out TOut1 o1, out TOut2 o2) => Invoke(func, p1, p2, out o1, out o2);
        }

        public FuncWithOutput2<TParam1, TParam2, TParam3, TOut1, TOut2, TResult> CreateProxy<TParam1, TParam2, TParam3>(FuncWithOutput2<TParam1, TParam2, TParam3, TOut1, TOut2, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return (TParam1 p1, TParam2 p2, TParam3 p3, out TOut1 o1, out TOut2 o2) => Invoke(func, p1, p2, p3, out o1, out o2);
        }

        public FuncWithOutput2<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TResult> CreateProxy<TParam1, TParam2, TParam3, TParam4>(FuncWithOutput2<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return (TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, out TOut1 o1, out TOut2 o2) => Invoke(func, p1, p2, p3, p4, out o1, out o2);
        }

        public FuncWithOutput2<TParam1, TParam2, TParam3, TParam4, TParam5, TOut1, TOut2, TResult> CreateProxy<TParam1, TParam2, TParam3, TParam4, TParam5>(FuncWithOutput2<TParam1, TParam2, TParam3, TParam4, TParam5, TOut1, TOut2, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return (TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, out TOut1 o1, out TOut2 o2) => Invoke(func, p1, p2, p3, p4, p5, out o1, out o2);
        }

        public TResult Invoke(Func<TOut1, TOut2, TResult> func, TOut1 o1, TOut2 o2)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(o1, o2);
            Apply(result, o1, o2);
            return result;
        }

        public TResult Invoke(FuncWithOutput2<TOut1, TOut2, TResult> func, out TOut1 o1, out TOut2 o2)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(out o1, out o2);
            Apply(result, o1, o2);
            return result;
        }

        public TResult Invoke<TParam>(FuncWithOutput2<TParam, TOut1, TOut2, TResult> func, TParam p, out TOut1 o1, out TOut2 o2)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(p, out o1, out o2);
            Apply(result, o1, o2);
            return result;
        }

        public TResult Invoke<TParam1, TParam2>(FuncWithOutput2<TParam1, TParam2, TOut1, TOut2, TResult> func, TParam1 p1,
            TParam2 p2, out TOut1 o1, out TOut2 o2)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(p1, p2, out o1, out o2);
            Apply(result, o1, o2);
            return result;
        }

        public TResult Invoke<TParam1, TParam2, TParam3>(FuncWithOutput2<TParam1, TParam2, TParam3, TOut1, TOut2, TResult> func, TParam1 p1,
            TParam2 p2, TParam3 p3, out TOut1 o1, out TOut2 o2)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(p1, p2, p3, out o1, out o2);
            Apply(result, o1, o2);
            return result;
        }

        public TResult Invoke<TParam1, TParam2, TParam3, TParam4>(FuncWithOutput2<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TResult> func, TParam1 p1,
            TParam2 p2, TParam3 p3, TParam4 p4, out TOut1 o1, out TOut2 o2)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(p1, p2, p3, p4, out o1, out o2);
            Apply(result, o1, o2);
            return result;
        }

        public TResult Invoke<TParam1, TParam2, TParam3, TParam4, TParam5>(FuncWithOutput2<TParam1, TParam2, TParam3, TParam4, TParam5, TOut1, TOut2, TResult> func, TParam1 p1,
            TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, out TOut1 o1, out TOut2 o2)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(p1, p2, p3, p4, p5, out o1, out o2);
            Apply(result, o1, o2);
            return result;
        }

        public IFuncInvocationResult<TOut1, TOut2, TResult> ToResult() => (WasInvoked) ? new FuncInvocationResult<TOut1, TOut2, TResult>(ReturnValue, Output1, Output2) :
            new FuncInvocationResult<TOut1, TOut2, TResult>();
    }

    public class FunctionInvocationMonitor<TOut1, TOut2, TOut3, TResult>
    {
        public bool WasInvoked { get; set; }
        public TOut1 Output1 { get; set; }
        public TOut2 Output2 { get; set; }
        public TOut3 Output3 { get; set; }
        public TResult ReturnValue { get; set; }

        public void Apply(TResult result, TOut1 o1, TOut2 o2, TOut3 o3)
        {
            ReturnValue = result;
            Output1 = o1;
            Output2 = o2;
            Output3 = o3;
            WasInvoked = true;
        }

        public Func<TOut1, TOut2, TOut3, TResult> CreateProxy(Func<TOut1, TOut2, TOut3, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return (TOut1 o1, TOut2 o2, TOut3 o3) => Invoke(func, o1, o2, o3);
        }

        public FuncWithOutput3<TOut1, TOut2, TOut3, TResult> CreateProxy(FuncWithOutput3<TOut1, TOut2, TOut3, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return (out TOut1 o1, out TOut2 o2, out TOut3 o3) => Invoke(func, out o1, out o2, out o3);
        }

        public FuncWithOutput3<TParam, TOut1, TOut2, TOut3, TResult> CreateProxy<TParam>(FuncWithOutput3<TParam, TOut1, TOut2, TOut3, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return (TParam p, out TOut1 o1, out TOut2 o2, out TOut3 o3) => Invoke(func, p, out o1, out o2, out o3);
        }

        public FuncWithOutput3<TParam1, TParam2, TOut1, TOut2, TOut3, TResult> CreateProxy<TParam1, TParam2>(FuncWithOutput3<TParam1, TParam2, TOut1, TOut2, TOut3, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return (TParam1 p1, TParam2 p2, out TOut1 o1, out TOut2 o2, out TOut3 o3) => Invoke(func, p1, p2, out o1, out o2, out o3);
        }

        public FuncWithOutput3<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TResult> CreateProxy<TParam1, TParam2, TParam3>(FuncWithOutput3<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return (TParam1 p1, TParam2 p2, TParam3 p3, out TOut1 o1, out TOut2 o2, out TOut3 o3) => Invoke(func, p1, p2, p3, out o1, out o2, out o3);
        }

        public FuncWithOutput3<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TOut3, TResult> CreateProxy<TParam1, TParam2, TParam3, TParam4>(FuncWithOutput3<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TOut3, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return (TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, out TOut1 o1, out TOut2 o2, out TOut3 o3) => Invoke(func, p1, p2, p3, p4, out o1, out o2, out o3);
        }

        public TResult Invoke(Func<TOut1, TOut2, TOut3, TResult> func, TOut1 o1, TOut2 o2, TOut3 o3)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(o1, o2, o3);
            Apply(result, o1, o2, o3);
            return result;
        }

        public TResult Invoke(FuncWithOutput3<TOut1, TOut2, TOut3, TResult> func, out TOut1 o1, out TOut2 o2, out TOut3 o3)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(out o1, out o2, out o3);
            Apply(result, o1, o2, o3);
            return result;
        }

        public TResult Invoke<TParam>(FuncWithOutput3<TParam, TOut1, TOut2, TOut3, TResult> func, TParam p, out TOut1 o1, out TOut2 o2, out TOut3 o3)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(p, out o1, out o2, out o3);
            Apply(result, o1, o2, o3);
            return result;
        }

        public TResult Invoke<TParam1, TParam2>(FuncWithOutput3<TParam1, TParam2, TOut1, TOut2, TOut3, TResult> func, TParam1 p1, TParam2 p2, out TOut1 o1, out TOut2 o2, out TOut3 o3)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(p1, p2, out o1, out o2, out o3);
            Apply(result, o1, o2, o3);
            return result;
        }

        public TResult Invoke<TParam1, TParam2, TParam3>(FuncWithOutput3<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TResult> func, TParam1 p1, TParam2 p2, TParam3 p3, out TOut1 o1, out TOut2 o2, out TOut3 o3)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(p1, p2, p3, out o1, out o2, out o3);
            Apply(result, o1, o2, o3);
            return result;
        }

        public TResult Invoke<TParam1, TParam2, TParam3, TParam4>(FuncWithOutput3<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TOut3, TResult> func, TParam1 p1,
            TParam2 p2, TParam3 p3, TParam4 p4, out TOut1 o1, out TOut2 o2, out TOut3 o3)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(p1, p2, p3, p4, out o1, out o2, out o3);
            Apply(result, o1, o2, o3);
            return result;
        }

        public IFuncInvocationResult<TOut1, TOut2, TOut3, TResult> ToResult() => (WasInvoked) ?
            new FuncInvocationResult<TOut1, TOut2, TOut3, TResult>(ReturnValue, Output1, Output2, Output3) : new FuncInvocationResult<TOut1, TOut2, TOut3, TResult>();
    }

    public class FunctionInvocationMonitor<TOut1, TOut2, TOut3, TOut4, TResult>
    {
        public bool WasInvoked { get; set; }
        public TOut1 Output1 { get; set; }
        public TOut2 Output2 { get; set; }
        public TOut3 Output3 { get; set; }
        public TOut4 Output4 { get; set; }
        public TResult ReturnValue { get; set; }

        public void Apply(TResult result, TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4)
        {
            ReturnValue = result;
            Output1 = o1;
            Output2 = o2;
            Output3 = o3;
            Output4 = o4;
            WasInvoked = true;
        }

        public Func<TOut1, TOut2, TOut3, TOut4, TResult> CreateProxy(Func<TOut1, TOut2, TOut3, TOut4, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return (TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4) => Invoke(func, o1, o2, o3, o4);
        }

        public FuncWithOutput4<TOut1, TOut2, TOut3, TOut4, TResult> CreateProxy(FuncWithOutput4<TOut1, TOut2, TOut3, TOut4, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return (out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4) => Invoke(func, out o1, out o2, out o3, out o4);
        }

        public FuncWithOutput4<TParam, TOut1, TOut2, TOut3, TOut4, TResult> CreateProxy<TParam>(FuncWithOutput4<TParam, TOut1, TOut2, TOut3, TOut4, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return (TParam p, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4) => Invoke(func, p, out o1, out o2, out o3, out o4);
        }

        public FuncWithOutput4<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TResult> CreateProxy<TParam1, TParam2>(FuncWithOutput4<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return (TParam1 p1, TParam2 p2, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4) => Invoke(func, p1, p2, out o1, out o2, out o3, out o4);
        }

        public FuncWithOutput4<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TOut4, TResult> CreateProxy<TParam1, TParam2, TParam3>(FuncWithOutput4<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TOut4, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return (TParam1 p1, TParam2 p2, TParam3 p3, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4) => Invoke(func, p1, p2, p3, out o1, out o2, out o3, out o4);
        }

        public TResult Invoke(Func<TOut1, TOut2, TOut3, TOut4, TResult> func, TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(o1, o2, o3, o4);
            Apply(result, o1, o2, o3, o4);
            return result;
        }

        public TResult Invoke(FuncWithOutput4<TOut1, TOut2, TOut3, TOut4, TResult> func, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(out o1, out o2, out o3, out o4);
            Apply(result, o1, o2, o3, o4);
            return result;
        }

        public TResult Invoke<TParam>(FuncWithOutput4<TParam, TOut1, TOut2, TOut3, TOut4, TResult> func, TParam p, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(p, out o1, out o2, out o3, out o4);
            Apply(result, o1, o2, o3, o4);
            return result;
        }

        public TResult Invoke<TParam1, TParam2>(FuncWithOutput4<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TResult> func, TParam1 p1, TParam2 p2, out TOut1 o1, out TOut2 o2,
            out TOut3 o3, out TOut4 o4)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(p1, p2, out o1, out o2, out o3, out o4);
            Apply(result, o1, o2, o3, o4);
            return result;
        }

        public TResult Invoke<TParam1, TParam2, TParam3>(FuncWithOutput4<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TOut4, TResult> func, TParam1 p1,
            TParam2 p2, TParam3 p3, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(p1, p2, p3, out o1, out o2, out o3, out o4);
            Apply(result, o1, o2, o3, o4);
            return result;
        }

        public IFuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TResult> ToResult() => (WasInvoked) ?
            new FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TResult>(ReturnValue, Output1, Output2, Output3, Output4) : new FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TResult>();
    }

    public class FunctionInvocationMonitor<TOut1, TOut2, TOut3, TOut4, TOut5, TResult>
    {
        public bool WasInvoked { get; set; }
        public TOut1 Output1 { get; set; }
        public TOut2 Output2 { get; set; }
        public TOut3 Output3 { get; set; }
        public TOut4 Output4 { get; set; }
        public TOut5 Output5 { get; set; }
        public TResult ReturnValue { get; set; }

        public void Apply(TResult result, TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4, TOut5 o5)
        {
            ReturnValue = result;
            Output1 = o1;
            Output2 = o2;
            Output3 = o3;
            Output4 = o4;
            Output5 = o5;
            WasInvoked = true;
        }

        public Func<TOut1, TOut2, TOut3, TOut4, TOut5, TResult> CreateProxy(Func<TOut1, TOut2, TOut3, TOut4, TOut5, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return (TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4, TOut5 o5) => Invoke(func, o1, o2, o3, o4, o5);
        }

        public FuncWithOutput5<TOut1, TOut2, TOut3, TOut4, TOut5, TResult> CreateProxy(FuncWithOutput5<TOut1, TOut2, TOut3, TOut4, TOut5, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return (out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4, out TOut5 o5) => Invoke(func, out o1, out o2, out o3, out o4, out o5);
        }

        public FuncWithOutput5<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TResult> CreateProxy<TParam>(FuncWithOutput5<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return (TParam p, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4, out TOut5 o5) => Invoke(func, p, out o1, out o2, out o3, out o4, out o5);
        }

        public FuncWithOutput5<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TOut5, TResult> CreateProxy<TParam1, TParam2>(FuncWithOutput5<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4,
            TOut5, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return (TParam1 p1, TParam2 p2, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4, out TOut5 o5) => Invoke(func, p1, p2, out o1, out o2, out o3, out o4, out o5);
        }

        public TResult Invoke(Func<TOut1, TOut2, TOut3, TOut4, TOut5, TResult> func, TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4, TOut5 o5)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(o1, o2, o3, o4, o5);
            Apply(result, o1, o2, o3, o4, o5);
            return result;
        }

        public TResult Invoke(FuncWithOutput5<TOut1, TOut2, TOut3, TOut4, TOut5, TResult> func, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4, out TOut5 o5)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(out o1, out o2, out o3, out o4, out o5);
            Apply(result, o1, o2, o3, o4, o5);
            return result;
        }

        public TResult Invoke<TParam>(FuncWithOutput5<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TResult> func, TParam p, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4, out TOut5 o5)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(p, out o1, out o2, out o3, out o4, out o5);
            Apply(result, o1, o2, o3, o4, o5);
            return result;
        }

        public TResult Invoke<TParam1, TParam2>(FuncWithOutput5<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TOut5, TResult> func, TParam1 p1,
            TParam2 p2, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4, out TOut5 o5)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(p1, p2, out o1, out o2, out o3, out o4, out o5);
            Apply(result, o1, o2, o3, o4, o5);
            return result;
        }

        public IFuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TResult> ToResult() => (WasInvoked) ?
            new FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TResult>(ReturnValue, Output1, Output2, Output3, Output4, Output5) :
            new FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TResult>();
    }

    public class FunctionInvocationMonitor<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult>
    {
        public bool WasInvoked { get; set; }
        public TOut1 Output1 { get; set; }
        public TOut2 Output2 { get; set; }
        public TOut3 Output3 { get; set; }
        public TOut4 Output4 { get; set; }
        public TOut5 Output5 { get; set; }
        public TOut6 Output6 { get; set; }
        public TResult ReturnValue { get; set; }

        public void Apply(TResult result, TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4, TOut5 o5, TOut6 o6)
        {
            ReturnValue = result;
            Output1 = o1;
            Output2 = o2;
            Output3 = o3;
            Output4 = o4;
            Output5 = o5;
            Output6 = o6;
            WasInvoked = true;
        }

        public Func<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult> Func(Func<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return (TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4, TOut5 o5, TOut6 o6) => Invoke(func, o1, o2, o3, o4, o5, o6);
        }

        public FuncWithOutput6<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult> CreateProxy(FuncWithOutput6<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return (out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4, out TOut5 o5, out TOut6 o6) => Invoke(func, out o1, out o2, out o3, out o4, out o5, out o6);
        }

        public FuncWithOutput6<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult> CreateProxy<TParam>(FuncWithOutput6<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return (TParam p, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4, out TOut5 o5, out TOut6 o6) => Invoke(func, p, out o1, out o2, out o3, out o4, out o5, out o6);
        }

        public TResult Invoke(Func<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult> func, TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4, TOut5 o5, TOut6 o6)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(o1, o2, o3, o4, o5, o6);
            Apply(result, o1, o2, o3, o4, o5, o6);
            return result;
        }

        public TResult Invoke(FuncWithOutput6<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult> func, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4,
            out TOut5 o5, out TOut6 o6)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(out o1, out o2, out o3, out o4, out o5, out o6);
            Apply(result, o1, o2, o3, o4, o5, o6);
            return result;
        }

        public TResult Invoke<TParam>(FuncWithOutput6<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult> func, TParam p, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4,
            out TOut5 o5, out TOut6 o6)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(p, out o1, out o2, out o3, out o4, out o5, out o6);
            Apply(result, o1, o2, o3, o4, o5, o6);
            return result;
        }

        public IFuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult> ToResult() => (WasInvoked) ?
            new FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult>(ReturnValue, Output1, Output2, Output3, Output4, Output5, Output6) :
            new FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult>();
    }

    public class FunctionInvocationMonitor<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult>
    {
        public bool WasInvoked { get; set; }
        public TOut1 Output1 { get; set; }
        public TOut2 Output2 { get; set; }
        public TOut3 Output3 { get; set; }
        public TOut4 Output4 { get; set; }
        public TOut5 Output5 { get; set; }
        public TOut6 Output6 { get; set; }
        public TOut7 Output7 { get; set; }
        public TResult ReturnValue { get; set; }

        public void Apply(TResult result, TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4, TOut5 o5, TOut6 o6, TOut7 o7)
        {
            ReturnValue = result;
            Output1 = o1;
            Output2 = o2;
            Output3 = o3;
            Output4 = o4;
            Output5 = o5;
            Output6 = o6;
            Output7 = o7;
            WasInvoked = true;
        }

        public Func<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult> CreateProxy(Func<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return (TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4, TOut5 o5, TOut6 o6, TOut7 o7) => Invoke(func, o1, o2, o3, o4, o5, o6, o7);
        }

        public FuncWithOutput7<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult> CreateProxy(FuncWithOutput7<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            return (out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4, out TOut5 o5, out TOut6 o6, out TOut7 o7) => Invoke(func, out o1, out o2, out o3, out o4, out o5, out o6, out o7);
        }

        public TResult Invoke(Func<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult> func, TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4, TOut5 o5, TOut6 o6, TOut7 o7)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(o1, o2, o3, o4, o5, o6, o7);
            Apply(result, o1, o2, o3, o4, o5, o6, o7);
            return result;
        }

        public TResult Invoke(FuncWithOutput7<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult> func, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4,
            out TOut5 o5, out TOut6 o6, out TOut7 o7)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            TResult result = func(out o1, out o2, out o3, out o4, out o5, out o6, out o7);
            Apply(result, o1, o2, o3, o4, o5, o6, o7);
            return result;
        }

        public IFuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult> ToResult() => (WasInvoked) ?
            new FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult>(ReturnValue, Output1, Output2, Output3, Output4, Output5, Output6, Output7) :
            new FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult>();
    }
}
