using System;

namespace FsInfoCat.Test.Helpers
{
    public class InvocationMonitor
    {
        public bool WasInvoked { get; private set; }

        public void Apply() { WasInvoked = true; }

        public Action CreateProxy(Action action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            return () => Invoke(action);
        }

        public Action<TParam> CreateProxy<TParam>(Action<TParam> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            return p => Invoke(action, p);
        }

        public Action<TParam1, TParam2> CreateProxy<TParam1, TParam2>(Action<TParam1, TParam2> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            return (p1, p2) => Invoke(action, p1, p2);
        }

        public Action<TParam1, TParam2, TParam3> CreateProxy<TParam1, TParam2, TParam3>(Action<TParam1, TParam2, TParam3> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            return (p1, p2, p3) => Invoke(action, p1, p2, p3);
        }

        public Action<TParam1, TParam2, TParam3, TParam4> CreateProxy<TParam1, TParam2, TParam3, TParam4>(Action<TParam1, TParam2, TParam3, TParam4> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            return (p1, p2, p3, p4) => Invoke(action, p1, p2, p3, p4);
        }

        public Action<TParam1, TParam2, TParam3, TParam4, TParam5> CreateProxy<TParam1, TParam2, TParam3, TParam4, TParam5>(Action<TParam1, TParam2, TParam3, TParam4, TParam5> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            return (p1, p2, p3, p4, p5) => Invoke(action, p1, p2, p3, p4, p5);
        }

        public Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6> CreateProxy<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6>(Action<TParam1, TParam2, TParam3, TParam4,
                TParam5, TParam6> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            return (p1, p2, p3, p4, p5, p6) => Invoke(action, p1, p2, p3, p4, p5, p6);
        }

        public Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7> CreateProxy<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7>(Action<TParam1, TParam2,
                TParam3, TParam4, TParam5, TParam6, TParam7> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            return (p1, p2, p3, p4, p5, p6, p7) => Invoke(action, p1, p2, p3, p4, p5, p6, p7);
        }

        public void Invoke(Action action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action();
            WasInvoked = true;
        }

        public void Invoke<TParam>(Action<TParam> action, TParam p)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(p);
            WasInvoked = true;
        }

        public void Invoke<TParam1, TParam2>(Action<TParam1, TParam2> action, TParam1 p1, TParam2 p2)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(p1, p2);
            WasInvoked = true;
        }

        public void Invoke<TParam1, TParam2, TParam3>(Action<TParam1, TParam2, TParam3> action, TParam1 p1, TParam2 p2, TParam3 p3)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(p1, p2, p3);
            WasInvoked = true;
        }

        public void Invoke<TParam1, TParam2, TParam3, TParam4>(Action<TParam1, TParam2, TParam3, TParam4> action, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(p1, p2, p3, p4);
            WasInvoked = true;
        }

        public void Invoke<TParam1, TParam2, TParam3, TParam4, TParam5>(Action<TParam1, TParam2, TParam3, TParam4, TParam5> action, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(p1, p2, p3, p4, p5);
            WasInvoked = true;
        }

        public void Invoke<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6>(Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6> action, TParam1 p1, TParam2 p2, TParam3 p3,
            TParam4 p4, TParam5 p5, TParam6 p6)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(p1, p2, p3, p4, p5, p6);
            WasInvoked = true;
        }

        public void Invoke<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7>(Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7> action, TParam1 p1,
            TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6, TParam7 p7)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(p1, p2, p3, p4, p5, p6, p7);
            WasInvoked = true;
        }

        public IInvocationResult ToResult() => new InvocationResult(WasInvoked);
    }

    public class InvocationMonitor<TOut>
    {
        public bool WasInvoked { get; set; }
        public TOut Output1 { get; set; }

        public void Apply(TOut obj)
        {
            WasInvoked = true;
            Output1 = obj;
        }

        public Action<TOut> CreateProxy(Action<TOut> action) => o => Invoke(action, o);

        public ActionWithOutput<TOut> CreateProxy(ActionWithOutput<TOut> action) => (out TOut o1) => Invoke(action, out o1);

        public ActionWithOutput<TParam, TOut> CreateProxy<TParam>(ActionWithOutput<TParam, TOut> action)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return (TParam p, out TOut o1) => Invoke(action, p, out o1);
        }

        public ActionWithOutput<TParam1, TParam2, TOut> CreateProxy<TParam1, TParam2>(ActionWithOutput<TParam1, TParam2, TOut> action)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return (TParam1 p1, TParam2 p2, out TOut o1) => Invoke(action, p1, p2, out o1);
        }

        public ActionWithOutput<TParam1, TParam2, TParam3, TOut> CreateProxy<TParam1, TParam2, TParam3>(ActionWithOutput<TParam1, TParam2, TParam3, TOut> action)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return (TParam1 p1, TParam2 p2, TParam3 p3, out TOut o1) => Invoke(action, p1, p2, p3, out o1);
        }

        public ActionWithOutput<TParam1, TParam2, TParam3, TParam4, TOut> CreateProxy<TParam1, TParam2, TParam3, TParam4>(ActionWithOutput<TParam1, TParam2, TParam3, TParam4, TOut> action)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return (TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, out TOut o1) => Invoke(action, p1, p2, p3, p4, out o1);
        }

        public ActionWithOutput<TParam1, TParam2, TParam3, TParam4, TParam5, TOut> CreateProxy<TParam1, TParam2, TParam3, TParam4, TParam5>(ActionWithOutput<TParam1, TParam2, TParam3,
                TParam4, TParam5, TOut> action)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return (TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, out TOut o1) => Invoke(action, p1, p2, p3, p4, p5, out o1);
        }

        public ActionWithOutput<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TOut> CreateProxy<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6>(ActionWithOutput<TParam1,
                TParam2, TParam3, TParam4, TParam5, TParam6, TOut> action)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return (TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6, out TOut o1) => Invoke(action, p1, p2, p3, p4, p5, p6, out o1);
        }

        private void Invoke(Action<TOut> action, TOut o)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(o);
            Apply(o);
        }

        public void Invoke(ActionWithOutput<TOut> action, out TOut result)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(out result);
            Apply(result);
        }

        public void Invoke<TParam>(ActionWithOutput<TParam, TOut> action, TParam p, out TOut result)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(p, out result);
            Apply(result);
        }

        public void Invoke<TParam1, TParam2>(ActionWithOutput<TParam1, TParam2, TOut> action, TParam1 p1, TParam2 p2, out TOut result)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(p1, p2, out result);
            Apply(result);
        }

        public void Invoke<TParam1, TParam2, TParam3>(ActionWithOutput<TParam1, TParam2, TParam3, TOut> action, TParam1 p1, TParam2 p2, TParam3 p3, out TOut result)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(p1, p2, p3, out result);
            Apply(result);
        }

        public void Invoke<TParam1, TParam2, TParam3, TParam4>(ActionWithOutput<TParam1, TParam2, TParam3, TParam4, TOut> action, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4,
            out TOut result)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(p1, p2, p3, p4, out result);
            Apply(result);
        }

        public void Invoke<TParam1, TParam2, TParam3, TParam4, TParam5>(ActionWithOutput<TParam1, TParam2, TParam3, TParam4, TParam5, TOut> action, TParam1 p1, TParam2 p2, TParam3 p3,
            TParam4 p4, TParam5 p5, out TOut result)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(p1, p2, p3, p4, p5, out result);
            Apply(result);
        }

        public void Invoke<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6>(ActionWithOutput<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TOut> action, TParam1 p1, TParam2 p2,
            TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6, out TOut result)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(p1, p2, p3, p4, p5, p6, out result);
            Apply(result);
        }

        public IInvocationResult<TOut> ToResult() => (WasInvoked) ? new InvocationResult<TOut>(Output1) : new InvocationResult<TOut>();
    }

    public class InvocationMonitor<TOut1, TOut2>
    {
        public bool WasInvoked { get; set; }
        public TOut1 Output1 { get; set; }
        public TOut2 Output2 { get; set; }

        public void Apply(TOut1 arg1, TOut2 arg2)
        {
            WasInvoked = true;
            Output1 = arg1;
            Output2 = arg2;
        }

        public Action<TOut1, TOut2> CreateProxy(Action<TOut1, TOut2> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            return (TOut1 o1, TOut2 o2) => Invoke(action, o1, o2);
        }

        public ActionWithOutput2<TOut1, TOut2> CreateProxy(ActionWithOutput2<TOut1, TOut2> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            return (out TOut1 o1, out TOut2 o2) => Invoke(action, out o1, out o2);
        }

        public ActionWithOutput2<TParam, TOut1, TOut2> CreateProxy<TParam>(ActionWithOutput2<TParam, TOut1, TOut2> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            return (TParam p, out TOut1 o1, out TOut2 o2) => Invoke(action, p, out o1, out o2);
        }

        public ActionWithOutput2<TParam1, TParam2, TOut1, TOut2> CreateProxy<TParam1, TParam2>(ActionWithOutput2<TParam1, TParam2, TOut1, TOut2> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            return (TParam1 p1, TParam2 p2, out TOut1 o1, out TOut2 o2) => Invoke(action, p1, p2, out o1, out o2);
        }

        public ActionWithOutput2<TParam1, TParam2, TParam3, TOut1, TOut2> CreateProxy<TParam1, TParam2, TParam3>(ActionWithOutput2<TParam1, TParam2, TParam3, TOut1, TOut2> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            return (TParam1 p1, TParam2 p2, TParam3 p3, out TOut1 o1, out TOut2 o2) => Invoke(action, p1, p2, p3, out o1, out o2);
        }

        public ActionWithOutput2<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2> CreateProxy<TParam1, TParam2, TParam3, TParam4>(ActionWithOutput2<TParam1, TParam2, TParam3, TParam4,
                TOut1, TOut2> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            return (TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, out TOut1 o1, out TOut2 o2) => Invoke(action, p1, p2, p3, p4, out o1, out o2);
        }

        public ActionWithOutput2<TParam1, TParam2, TParam3, TParam4, TParam5, TOut1, TOut2> CreateProxy<TParam1, TParam2, TParam3, TParam4, TParam5>(ActionWithOutput2<TParam1, TParam2,
                TParam3, TParam4, TParam5, TOut1, TOut2> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            return (TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, out TOut1 o1, out TOut2 o2) => Invoke(action, p1, p2, p3, p4, p5, out o1, out o2);
        }

        public void Invoke(Action<TOut1, TOut2> action, TOut1 o1, TOut2 o2)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(o1, o2);
            Apply(o1, o2);
        }

        public void Invoke(ActionWithOutput2<TOut1, TOut2> action, out TOut1 o1, out TOut2 o2)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(out o1, out o2);
            Apply(o1, o2);
        }

        public void Invoke<TParam>(ActionWithOutput2<TParam, TOut1, TOut2> action, TParam p, out TOut1 o1, out TOut2 o2)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(p, out o1, out o2);
            Apply(o1, o2);
        }

        public void Invoke<TParam1, TParam2>(ActionWithOutput2<TParam1, TParam2, TOut1, TOut2> action, TParam1 p1, TParam2 p2, out TOut1 o1, out TOut2 o2)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(p1, p2, out o1, out o2);
            Apply(o1, o2);
        }

        public void Invoke<TParam1, TParam2, TParam3>(ActionWithOutput2<TParam1, TParam2, TParam3, TOut1, TOut2> action, TParam1 p1, TParam2 p2, TParam3 p3, out TOut1 o1, out TOut2 o2)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(p1, p2, p3, out o1, out o2);
            Apply(o1, o2);
        }

        public void Invoke<TParam1, TParam2, TParam3, TParam4>(ActionWithOutput2<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2> action, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4,
            out TOut1 o1, out TOut2 o2)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(p1, p2, p3, p4, out o1, out o2);
            Apply(o1, o2);
        }

        public void Invoke<TParam1, TParam2, TParam3, TParam4, TParam5>(ActionWithOutput2<TParam1, TParam2, TParam3, TParam4, TParam5, TOut1, TOut2> action, TParam1 p1, TParam2 p2,
            TParam3 p3, TParam4 p4, TParam5 p5, out TOut1 o1, out TOut2 o2)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(p1, p2, p3, p4, p5, out o1, out o2);
            Apply(o1, o2);
        }

        public IInvocationResult<TOut1, TOut2> ToResult() => (WasInvoked) ? new InvocationResult<TOut1, TOut2>(Output1, Output2) : new InvocationResult<TOut1, TOut2>();
    }

    public class InvocationMonitor<TOut1, TOut2, TOut3>
    {
        public bool WasInvoked { get; set; }
        public TOut1 Output1 { get; set; }
        public TOut2 Output2 { get; set; }
        public TOut3 Output3 { get; set; }

        public void Apply(TOut1 arg1, TOut2 arg2, TOut3 arg3)
        {
            WasInvoked = true;
            Output1 = arg1;
            Output2 = arg2;
            Output3 = arg3;
        }

        public Action<TOut1, TOut2, TOut3> CreateProxy(Action<TOut1, TOut2, TOut3> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            return (TOut1 o1, TOut2 o2, TOut3 o3) => Invoke(action, o1, o2, o3);
        }

        public ActionWithOutput3<TOut1, TOut2, TOut3> CreateProxy(ActionWithOutput3<TOut1, TOut2, TOut3> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            return (out TOut1 o1, out TOut2 o2, out TOut3 o3) => Invoke(action, out o1, out o2, out o3);
        }

        public ActionWithOutput3<TParam, TOut1, TOut2, TOut3> CreateProxy<TParam>(ActionWithOutput3<TParam, TOut1, TOut2, TOut3> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            return (TParam p, out TOut1 o1, out TOut2 o2, out TOut3 o3) => Invoke(action, p, out o1, out o2, out o3);
        }

        public ActionWithOutput3<TParam1, TParam2, TOut1, TOut2, TOut3> CreateProxy<TParam1, TParam2>(ActionWithOutput3<TParam1, TParam2, TOut1, TOut2, TOut3> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            return (TParam1 p1, TParam2 p2, out TOut1 o1, out TOut2 o2, out TOut3 o3) => Invoke(action, p1, p2, out o1, out o2, out o3);
        }

        public ActionWithOutput3<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3> CreateProxy<TParam1, TParam2, TParam3>(ActionWithOutput3<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            return (TParam1 p1, TParam2 p2, TParam3 p3, out TOut1 o1, out TOut2 o2, out TOut3 o3) => Invoke(action, p1, p2, p3, out o1, out o2, out o3);
        }

        public ActionWithOutput3<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TOut3> CreateProxy<TParam1, TParam2, TParam3, TParam4>(ActionWithOutput3<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TOut3> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            return (TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, out TOut1 o1, out TOut2 o2, out TOut3 o3) => Invoke(action, p1, p2, p3, p4, out o1, out o2, out o3);
        }

        public void Invoke(Action<TOut1, TOut2, TOut3> action, TOut1 o1, TOut2 o2, TOut3 o3)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(o1, o2, o3);
            Apply(o1, o2, o3);
        }

        public void Invoke(ActionWithOutput3<TOut1, TOut2, TOut3> action, out TOut1 o1, out TOut2 o2, out TOut3 o3)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(out o1, out o2, out o3);
            Apply(o1, o2, o3);
        }

        public void Invoke<TParam>(ActionWithOutput3<TParam, TOut1, TOut2, TOut3> action, TParam p, out TOut1 o1, out TOut2 o2, out TOut3 o3)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(p, out o1, out o2, out o3);
            Apply(o1, o2, o3);
        }

        public void Invoke<TParam1, TParam2>(ActionWithOutput3<TParam1, TParam2, TOut1, TOut2, TOut3> action, TParam1 p1, TParam2 p2, out TOut1 o1, out TOut2 o2, out TOut3 o3)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(p1, p2, out o1, out o2, out o3);
            Apply(o1, o2, o3);
        }

        public void Invoke<TParam1, TParam2, TParam3>(ActionWithOutput3<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3> action, TParam1 p1, TParam2 p2,
            TParam3 p3, out TOut1 o1, out TOut2 o2, out TOut3 o3)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(p1, p2, p3, out o1, out o2, out o3);
            Apply(o1, o2, o3);
        }

        public void Invoke<TParam1, TParam2, TParam3, TParam4>(ActionWithOutput3<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TOut3> action, TParam1 p1, TParam2 p2,
            TParam3 p3, TParam4 p4, out TOut1 o1, out TOut2 o2, out TOut3 o3)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(p1, p2, p3, p4, out o1, out o2, out o3);
            Apply(o1, o2, o3);
        }

        public IInvocationResult<TOut1, TOut2, TOut3> ToResult() => (WasInvoked) ? new InvocationResult<TOut1, TOut2, TOut3>(Output1, Output2, Output3) : new InvocationResult<TOut1, TOut2, TOut3>();
    }

    public class InvocationMonitor<TOut1, TOut2, TOut3, TOut4>
    {
        public bool WasInvoked { get; set; }
        public TOut1 Output1 { get; set; }
        public TOut2 Output2 { get; set; }
        public TOut3 Output3 { get; set; }
        public TOut4 Output4 { get; set; }

        public void Apply(TOut1 arg1, TOut2 arg2, TOut3 arg3, TOut4 arg4)
        {
            WasInvoked = true;
            Output1 = arg1;
            Output2 = arg2;
            Output3 = arg3;
            Output4 = arg4;
        }

        public Action<TOut1, TOut2, TOut3, TOut4> CreateProxy(Action<TOut1, TOut2, TOut3, TOut4> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            return (TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4) => Invoke(action, o1, o2, o3, o4);
        }

        public ActionWithOutput4<TOut1, TOut2, TOut3, TOut4> CreateProxy(ActionWithOutput4<TOut1, TOut2, TOut3, TOut4> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            return (out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4) => Invoke(action, out o1, out o2, out o3, out o4);
        }

        public ActionWithOutput4<TParam, TOut1, TOut2, TOut3, TOut4> CreateProxy<TParam>(ActionWithOutput4<TParam, TOut1, TOut2, TOut3, TOut4> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            return (TParam p, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4) => Invoke(action, p, out o1, out o2, out o3, out o4);
        }

        public ActionWithOutput4<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4> CreateProxy<TParam1, TParam2>(ActionWithOutput4<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            return (TParam1 p1, TParam2 p2, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4) => Invoke(action, p1, p2, out o1, out o2, out o3, out o4);
        }

        public ActionWithOutput4<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TOut4> CreateProxy<TParam1, TParam2, TParam3>(ActionWithOutput4<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TOut4> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            return (TParam1 p1, TParam2 p2, TParam3 p3, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4) => Invoke(action, p1, p2, p3, out o1, out o2, out o3, out o4);
        }

        public void Invoke(Action<TOut1, TOut2, TOut3, TOut4> action, TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(o1, o2, o3, o4);
            Apply(o1, o2, o3, o4);
        }

        public void Invoke(ActionWithOutput4<TOut1, TOut2, TOut3, TOut4> action, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(out o1, out o2, out o3, out o4);
            Apply(o1, o2, o3, o4);
        }

        public void Invoke<TParam>(ActionWithOutput4<TParam, TOut1, TOut2, TOut3, TOut4> action, TParam p, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(p, out o1, out o2, out o3, out o4);
            Apply(o1, o2, o3, o4);
        }

        public void Invoke<TParam1, TParam2>(ActionWithOutput4<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4> action, TParam1 p1, TParam2 p2, out TOut1 o1, out TOut2 o2, out TOut3 o3,
            out TOut4 o4)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(p1, p2, out o1, out o2, out o3, out o4);
            Apply(o1, o2, o3, o4);
        }

        public void Invoke<TParam1, TParam2, TParam3>(ActionWithOutput4<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TOut4> action, TParam1 p1, TParam2 p2,
            TParam3 p3, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(p1, p2, p3, out o1, out o2, out o3, out o4);
            Apply(o1, o2, o3, o4);
        }

        public IInvocationResult<TOut1, TOut2, TOut3, TOut4> ToResult() => (WasInvoked) ? new InvocationResult<TOut1, TOut2, TOut3, TOut4>(Output1, Output2, Output3, Output4) :
            new InvocationResult<TOut1, TOut2, TOut3, TOut4>();
    }

    public class InvocationMonitor<TOut1, TOut2, TOut3, TOut4, TOut5>
    {
        public bool WasInvoked { get; set; }
        public TOut1 Output1 { get; set; }
        public TOut2 Output2 { get; set; }
        public TOut3 Output3 { get; set; }
        public TOut4 Output4 { get; set; }
        public TOut5 Output5 { get; set; }

        public void Apply(TOut1 arg1, TOut2 arg2, TOut3 arg3, TOut4 arg4, TOut5 arg5)
        {
            WasInvoked = true;
            Output1 = arg1;
            Output2 = arg2;
            Output3 = arg3;
            Output4 = arg4;
            Output5 = arg5;
        }

        public Action<TOut1, TOut2, TOut3, TOut4, TOut5> CreateProxy(Action<TOut1, TOut2, TOut3, TOut4, TOut5> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            return (TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4, TOut5 o5) => Invoke(action, o1, o2, o3, o4, o5);
        }

        public ActionWithOutput5<TOut1, TOut2, TOut3, TOut4, TOut5> CreateProxy(ActionWithOutput5<TOut1, TOut2, TOut3, TOut4, TOut5> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            return (out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4, out TOut5 o5) => Invoke(action, out o1, out o2, out o3, out o4, out o5);
        }

        public ActionWithOutput5<TParam, TOut1, TOut2, TOut3, TOut4, TOut5> CreateProxy<TParam>(ActionWithOutput5<TParam, TOut1, TOut2, TOut3, TOut4, TOut5> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            return (TParam p, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4, out TOut5 o5) => Invoke(action, p, out o1, out o2, out o3, out o4, out o5);
        }

        public ActionWithOutput5<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TOut5> CreateProxy<TParam1, TParam2>(ActionWithOutput5<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TOut5> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            return (TParam1 p1, TParam2 p2, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4, out TOut5 o5) => Invoke(action, p1, p2, out o1, out o2, out o3, out o4, out o5);
        }

        public void Invoke(Action<TOut1, TOut2, TOut3, TOut4, TOut5> action, TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4, TOut5 o5)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(o1, o2, o3, o4, o5);
            Apply(o1, o2, o3, o4, o5);
        }

        public void Invoke(ActionWithOutput5<TOut1, TOut2, TOut3, TOut4, TOut5> action, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4, out TOut5 o5)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(out o1, out o2, out o3, out o4, out o5);
            Apply(o1, o2, o3, o4, o5);
        }

        public void Invoke<TParam>(ActionWithOutput5<TParam, TOut1, TOut2, TOut3, TOut4, TOut5> action, TParam p, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4, out TOut5 o5)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(p, out o1, out o2, out o3, out o4, out o5);
            Apply(o1, o2, o3, o4, o5);
        }

        public void Invoke<TParam1, TParam2>(ActionWithOutput5<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TOut5> action, TParam1 p1, TParam2 p2, out TOut1 o1, out TOut2 o2, out TOut3 o3,
            out TOut4 o4, out TOut5 o5)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(p1, p2, out o1, out o2, out o3, out o4, out o5);
            Apply(o1, o2, o3, o4, o5);
        }

        public IInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5> ToResult() => (WasInvoked) ?
            new InvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5>(Output1, Output2, Output3, Output4, Output5) : new InvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5>();
    }

    public class InvocationMonitor<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>
    {
        public bool WasInvoked { get; set; }
        public TOut1 Output1 { get; set; }
        public TOut2 Output2 { get; set; }
        public TOut3 Output3 { get; set; }
        public TOut4 Output4 { get; set; }
        public TOut5 Output5 { get; set; }
        public TOut6 Output6 { get; set; }

        public void Apply(TOut1 arg1, TOut2 arg2, TOut3 arg3, TOut4 arg4, TOut5 arg5, TOut6 arg6)
        {
            WasInvoked = true;
            Output1 = arg1;
            Output2 = arg2;
            Output3 = arg3;
            Output4 = arg4;
            Output5 = arg5;
            Output6 = arg6;
        }

        public Action<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> CreateProxy(Action<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            return (TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4, TOut5 o5, TOut6 o6) => Invoke(action, o1, o2, o3, o4, o5, o6);
        }

        public ActionWithOutput6<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> CreateProxy(ActionWithOutput6<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            return (out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4, out TOut5 o5, out TOut6 o6) => Invoke(action, out o1, out o2, out o3, out o4, out o5, out o6);
        }

        public ActionWithOutput6<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> CreateProxy<TParam>(ActionWithOutput6<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            return (TParam p, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4, out TOut5 o5, out TOut6 o6) => Invoke(action, p, out o1, out o2, out o3, out o4, out o5, out o6);
        }

        public void Invoke(Action<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> action, TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4, TOut5 o5, TOut6 o6)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(o1, o2, o3, o4, o5, o6);
            Apply(o1, o2, o3, o4, o5, o6);
        }

        public void Invoke(ActionWithOutput6<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> action, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4,
            out TOut5 o5, out TOut6 o6)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(out o1, out o2, out o3, out o4, out o5, out o6);
            Apply(o1, o2, o3, o4, o5, o6);
        }

        public void Invoke<TParam>(ActionWithOutput6<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> action, TParam p, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4,
            out TOut5 o5, out TOut6 o6)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(p, out o1, out o2, out o3, out o4, out o5, out o6);
            Apply(o1, o2, o3, o4, o5, o6);
        }

        public IInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> ToResult() => (WasInvoked) ?
            new InvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(Output1, Output2, Output3, Output4, Output5, Output6) :
            new InvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>();
    }

    public class InvocationMonitor<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>
    {
        public bool WasInvoked { get; set; }
        public TOut1 Output1 { get; set; }
        public TOut2 Output2 { get; set; }
        public TOut3 Output3 { get; set; }
        public TOut4 Output4 { get; set; }
        public TOut5 Output5 { get; set; }
        public TOut6 Output6 { get; set; }
        public TOut7 Output7 { get; set; }

        public void Apply(TOut1 arg1, TOut2 arg2, TOut3 arg3, TOut4 arg4, TOut5 arg5, TOut6 arg6, TOut7 arg7)
        {
            WasInvoked = true;
            Output1 = arg1;
            Output2 = arg2;
            Output3 = arg3;
            Output4 = arg4;
            Output5 = arg5;
            Output6 = arg6;
            Output7 = arg7;
        }

        public Action<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7> CreateProxy(Action<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            return (TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4, TOut5 o5, TOut6 o6, TOut7 o7) => Invoke(action, o1, o2, o3, o4, o5, o6, o7);
        }

        public ActionWithOutput7<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7> CreateProxy(ActionWithOutput7<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            return (out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4, out TOut5 o5, out TOut6 o6, out TOut7 o7) => Invoke(action, out o1, out o2, out o3, out o4, out o5, out o6, out o7);
        }

        public void Invoke(Action<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7> action, TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4, TOut5 o5, TOut6 o6, TOut7 o7)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(o1, o2, o3, o4, o5, o6, o7);
            Apply(o1, o2, o3, o4, o5, o6, o7);
        }

        public void Invoke(ActionWithOutput7<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7> action, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4, out TOut5 o5,
            out TOut6 o6, out TOut7 o7)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            action(out o1, out o2, out o3, out o4, out o5, out o6, out o7);
            Apply(o1, o2, o3, o4, o5, o6, o7);
        }

        public IInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7> ToResult() => (WasInvoked) ?
            new InvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(Output1, Output2, Output3, Output4, Output5, Output6, Output7) :
            new InvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>();
    }
}
