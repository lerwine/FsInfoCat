using System;
using System.Linq;
using System.Threading.Tasks;

namespace DevHelperGUI
{
    public static class BinaryAlternate
    {
        public static BinaryAlternate<TPrimary, TSecondary> GetResult<TPrimary, TSecondary>(Task<TPrimary> task, Func<Exception, TSecondary> onException,
            Func<TSecondary> onCanceled)
        {
            if (task.IsCanceled)
                return BinaryAlternate<TPrimary, TSecondary>.AsSecondary(onCanceled());
            if (task.IsFaulted)
                return BinaryAlternate<TPrimary, TSecondary>.AsSecondary(onException(task.Exception));
            try { return BinaryAlternate<TPrimary, TSecondary>.AsPrimary(task.Result); }
            catch (Exception exception)
            {
                if (task.IsCanceled)
                    return BinaryAlternate<TPrimary, TSecondary>.AsSecondary(onCanceled());
                return BinaryAlternate<TPrimary, TSecondary>.AsSecondary(onException(exception));
            }
        }

        public static BinaryAlternate<TPrimary, TSecondary> GetResult<TPrimary, TSecondary>(Task<TPrimary> task, Func<Exception, TSecondary> onException)
        {
            if (task.IsFaulted)
                return BinaryAlternate<TPrimary, TSecondary>.AsSecondary(onException(task.Exception));
            try { return BinaryAlternate<TPrimary, TSecondary>.AsPrimary(task.Result); }
            catch (Exception exception)
            {
                return BinaryAlternate<TPrimary, TSecondary>.AsSecondary(onException(exception));
            }
        }

        public static BinaryAlternate<TPrimary, Exception> GetResult<TPrimary>(Task<TPrimary> task)
        {
            if (task.IsFaulted)
                return BinaryAlternate<TPrimary, Exception>.AsSecondary(task.Exception);
            try { return BinaryAlternate<TPrimary, Exception>.AsPrimary(task.Result); }
            catch (Exception exception)
            {
                return BinaryAlternate<TPrimary, Exception>.AsSecondary(exception);
            }
        }

        public static BinaryAlternate<TPrimary, Exception> GetResult<TPrimary>(Task<BinaryAlternate<TPrimary, Exception>> task)
        {
            if (task.IsFaulted)
                return BinaryAlternate<TPrimary, Exception>.AsSecondary(task.Exception);
            try { return task.Result; }
            catch (Exception exception)
            {
                return BinaryAlternate<TPrimary, Exception>.AsSecondary(exception);
            }
        }

        public static BinaryAlternate<TPrimary, TSecondary> MapResult<TPrimary, TSecondary>(Task<BinaryAlternate<TPrimary, TSecondary>> task,
            Func<Exception, TSecondary> ifError, Func<TSecondary> ifCanceled)
        {
            if (task.IsCanceled)
                return BinaryAlternate<TPrimary, TSecondary>.AsSecondary(ifCanceled());
            if (task.IsFaulted)
                return BinaryAlternate<TPrimary, TSecondary>.AsSecondary(ifError(task.Exception));
            try { return task.Result; }
            catch (Exception exception)
            {
                if (task.IsCanceled)
                    return BinaryAlternate<TPrimary, TSecondary>.AsSecondary(ifCanceled());
                return BinaryAlternate<TPrimary, TSecondary>.AsSecondary(ifError(exception));
            }
        }

        public static BinaryAlternate<TPrimary, TSecondary> MapResult<TPrimary, TSecondary>(Task<BinaryAlternate<TPrimary, TSecondary>> task,
            Func<Exception, TSecondary> ifError)
        {
            if (task.IsFaulted)
                return BinaryAlternate<TPrimary, TSecondary>.AsSecondary(ifError(task.Exception));
            try { return task.Result; }
            catch (Exception exception)
            {
                return BinaryAlternate<TPrimary, TSecondary>.AsSecondary(ifError(exception));
            }
        }

        public static BinaryAlternate<TPrimary, TSecondary> Map<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TPrimary, TSecondary>(TArg1 arg1, TArg2 arg2,
            TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TryGetter<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TPrimary> tryGetter,
            TSecondary ifFalse) =>
            tryGetter(arg1, arg2, arg3, arg4, arg5, arg6, arg7, out TPrimary primary) ? BinaryAlternate<TPrimary, TSecondary>.AsPrimary(primary) :
            BinaryAlternate<TPrimary, TSecondary>.AsSecondary(ifFalse);

        public static BinaryAlternate<TPrimary, TSecondary> Map<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TPrimary, TSecondary>(TArg1 arg1, TArg2 arg2,
            TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TryGetter<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TPrimary> tryGetter,
            Func<TSecondary> ifFalse) =>
            tryGetter(arg1, arg2, arg3, arg4, arg5, arg6, arg7, out TPrimary primary) ? BinaryAlternate<TPrimary, TSecondary>.AsPrimary(primary) :
            BinaryAlternate<TPrimary, TSecondary>.AsSecondary(ifFalse());

        public static BinaryAlternate<TPrimary, TSecondary> Map<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TPrimary, TSecondary>(TArg1 arg1, TArg2 arg2, TArg3 arg3,
            TArg4 arg4, TArg5 arg5, TArg6 arg6, TryGetter<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TPrimary> tryGetter, TSecondary ifFalse) =>
            tryGetter(arg1, arg2, arg3, arg4, arg5, arg6, out TPrimary primary) ? BinaryAlternate<TPrimary, TSecondary>.AsPrimary(primary) :
            BinaryAlternate<TPrimary, TSecondary>.AsSecondary(ifFalse);

        public static BinaryAlternate<TPrimary, TSecondary> Map<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TPrimary, TSecondary>(TArg1 arg1, TArg2 arg2, TArg3 arg3,
            TArg4 arg4, TArg5 arg5, TArg6 arg6, TryGetter<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TPrimary> tryGetter, Func<TSecondary> ifFalse) =>
            tryGetter(arg1, arg2, arg3, arg4, arg5, arg6, out TPrimary primary) ? BinaryAlternate<TPrimary, TSecondary>.AsPrimary(primary) :
            BinaryAlternate<TPrimary, TSecondary>.AsSecondary(ifFalse());

        public static BinaryAlternate<TPrimary, TSecondary> Map<TArg1, TArg2, TArg3, TArg4, TArg5, TPrimary, TSecondary>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4,
            TArg5 arg5, TryGetter<TArg1, TArg2, TArg3, TArg4, TArg5, TPrimary> tryGetter, TSecondary ifFalse) =>
            tryGetter(arg1, arg2, arg3, arg4, arg5, out TPrimary primary) ? BinaryAlternate<TPrimary, TSecondary>.AsPrimary(primary) :
            BinaryAlternate<TPrimary, TSecondary>.AsSecondary(ifFalse);

        public static BinaryAlternate<TPrimary, TSecondary> Map<TArg1, TArg2, TArg3, TArg4, TArg5, TPrimary, TSecondary>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4,
            TArg5 arg5, TryGetter<TArg1, TArg2, TArg3, TArg4, TArg5, TPrimary> tryGetter, Func<TSecondary> ifFalse) =>
            tryGetter(arg1, arg2, arg3, arg4, arg5, out TPrimary primary) ? BinaryAlternate<TPrimary, TSecondary>.AsPrimary(primary) :
            BinaryAlternate<TPrimary, TSecondary>.AsSecondary(ifFalse());

        public static BinaryAlternate<TPrimary, TSecondary> Map<TArg1, TArg2, TArg3, TArg4, TPrimary, TSecondary>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4,
            TryGetter<TArg1, TArg2, TArg3, TArg4, TPrimary> tryGetter, TSecondary ifFalse) =>
            tryGetter(arg1, arg2, arg3, arg4, out TPrimary primary) ? BinaryAlternate<TPrimary, TSecondary>.AsPrimary(primary) :
            BinaryAlternate<TPrimary, TSecondary>.AsSecondary(ifFalse);

        public static BinaryAlternate<TPrimary, TSecondary> Map<TArg1, TArg2, TArg3, TArg4, TPrimary, TSecondary>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4,
            TryGetter<TArg1, TArg2, TArg3, TArg4, TPrimary> tryGetter, Func<TSecondary> ifFalse) =>
            tryGetter(arg1, arg2, arg3, arg4, out TPrimary primary) ? BinaryAlternate<TPrimary, TSecondary>.AsPrimary(primary) :
            BinaryAlternate<TPrimary, TSecondary>.AsSecondary(ifFalse());

        public static BinaryAlternate<TPrimary, TSecondary> Map<TArg1, TArg2, TArg3, TPrimary, TSecondary>(TArg1 arg1, TArg2 arg2, TArg3 arg3,
            TryGetter<TArg1, TArg2, TArg3, TPrimary> tryGetter, TSecondary ifFalse) =>
            tryGetter(arg1, arg2, arg3, out TPrimary primary) ? BinaryAlternate<TPrimary, TSecondary>.AsPrimary(primary) :
            BinaryAlternate<TPrimary, TSecondary>.AsSecondary(ifFalse);

        public static BinaryAlternate<TPrimary, TSecondary> Map<TArg1, TArg2, TArg3, TPrimary, TSecondary>(TArg1 arg1, TArg2 arg2, TArg3 arg3,
            TryGetter<TArg1, TArg2, TArg3, TPrimary> tryGetter, Func<TSecondary> ifFalse) =>
            tryGetter(arg1, arg2, arg3, out TPrimary primary) ? BinaryAlternate<TPrimary, TSecondary>.AsPrimary(primary) :
            BinaryAlternate<TPrimary, TSecondary>.AsSecondary(ifFalse());

        public static BinaryAlternate<TPrimary, TSecondary> Map<TArg1, TArg2, TPrimary, TSecondary>(TArg1 arg1, TArg2 arg2, TryGetter<TArg1, TArg2, TPrimary> tryGetter,
            TSecondary ifFalse) =>
            tryGetter(arg1, arg2, out TPrimary primary) ? BinaryAlternate<TPrimary, TSecondary>.AsPrimary(primary) :
            BinaryAlternate<TPrimary, TSecondary>.AsSecondary(ifFalse);

        public static BinaryAlternate<TPrimary, TSecondary> Map<TArg1, TArg2, TPrimary, TSecondary>(TArg1 arg1, TArg2 arg2, TryGetter<TArg1, TArg2, TPrimary> tryGetter,
            Func<TSecondary> ifFalse) =>
            tryGetter(arg1, arg2, out TPrimary primary) ? BinaryAlternate<TPrimary, TSecondary>.AsPrimary(primary) :
            BinaryAlternate<TPrimary, TSecondary>.AsSecondary(ifFalse());

        public static BinaryAlternate<TPrimary, TSecondary> Map<TArg, TPrimary, TSecondary>(TArg arg, TryGetter<TArg, TPrimary> tryGetter, TSecondary ifFalse) =>
            tryGetter(arg, out TPrimary primary) ? BinaryAlternate<TPrimary, TSecondary>.AsPrimary(primary) :
            BinaryAlternate<TPrimary, TSecondary>.AsSecondary(ifFalse);

        public static BinaryAlternate<TPrimary, TSecondary> Map<TArg, TPrimary, TSecondary>(TArg arg, TryGetter<TArg, TPrimary> tryGetter, Func<TSecondary> ifFalse) =>
            tryGetter(arg, out TPrimary primary) ? BinaryAlternate<TPrimary, TSecondary>.AsPrimary(primary) :
            BinaryAlternate<TPrimary, TSecondary>.AsSecondary(ifFalse());

        public static BinaryAlternate<TPrimary, TSecondary> Map<TPrimary, TSecondary>(TryGetter<TPrimary> tryGetter, TSecondary ifFalse) =>
            tryGetter(out TPrimary primary) ? BinaryAlternate<TPrimary, TSecondary>.AsPrimary(primary) : BinaryAlternate<TPrimary, TSecondary>.AsSecondary(ifFalse);

        public static BinaryAlternate<TPrimary, TSecondary> Map<TPrimary, TSecondary>(TryGetter<TPrimary> tryGetter, Func<TSecondary> ifFalse) =>
            tryGetter(out TPrimary primary) ? BinaryAlternate<TPrimary, TSecondary>.AsPrimary(primary) : BinaryAlternate<TPrimary, TSecondary>.AsSecondary(ifFalse());

        public static BinaryAlternate<TPrimary, TSecondary> TryInvoke<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TPrimary, TSecondary>(TArg1 arg1, TArg2 arg2,
            TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TPrimary> func, Func<Exception, TSecondary> ifError)
        {
            try { return BinaryAlternate<TPrimary, TSecondary>.AsPrimary(func(arg1, arg2, arg3, arg4, arg5, arg6, arg7)); }
            catch (Exception exception) { return BinaryAlternate<TPrimary, TSecondary>.AsSecondary(ifError(exception)); }
        }

        public static BinaryAlternate<TPrimary, TSecondary> TryInvoke<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TPrimary, TSecondary>(TArg1 arg1, TArg2 arg2,
            TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TPrimary> func, Func<Exception, TSecondary> ifError)
        {
            try { return BinaryAlternate<TPrimary, TSecondary>.AsPrimary(func(arg1, arg2, arg3, arg4, arg5, arg6)); }
            catch (Exception exception) { return BinaryAlternate<TPrimary, TSecondary>.AsSecondary(ifError(exception)); }
        }

        public static BinaryAlternate<TPrimary, Exception> TryInvoke<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TPrimary>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4,
            TArg5 arg5, TArg6 arg6, Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TPrimary> func)
        {
            try { return BinaryAlternate<TPrimary, Exception>.AsPrimary(func(arg1, arg2, arg3, arg4, arg5, arg6)); }
            catch (Exception exception) { return BinaryAlternate<TPrimary, Exception>.AsSecondary(exception); }
        }

        public static BinaryAlternate<TPrimary, TSecondary> TryInvoke<TArg1, TArg2, TArg3, TArg4, TArg5, TPrimary, TSecondary>(TArg1 arg1, TArg2 arg2, TArg3 arg3,
            TArg4 arg4, TArg5 arg5, Func<TArg1, TArg2, TArg3, TArg4, TArg5, TPrimary> func, Func<Exception, TSecondary> ifError)
        {
            try { return BinaryAlternate<TPrimary, TSecondary>.AsPrimary(func(arg1, arg2, arg3, arg4, arg5)); }
            catch (Exception exception) { return BinaryAlternate<TPrimary, TSecondary>.AsSecondary(ifError(exception)); }
        }

        public static BinaryAlternate<TPrimary, Exception> TryInvoke<TArg1, TArg2, TArg3, TArg4, TArg5, TPrimary>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4,
            TArg5 arg5, Func<TArg1, TArg2, TArg3, TArg4, TArg5, TPrimary> func)
        {
            try { return BinaryAlternate<TPrimary, Exception>.AsPrimary(func(arg1, arg2, arg3, arg4, arg5)); }
            catch (Exception exception) { return BinaryAlternate<TPrimary, Exception>.AsSecondary(exception); }
        }

        public static BinaryAlternate<TPrimary, TSecondary> TryInvoke<TArg1, TArg2, TArg3, TArg4, TPrimary, TSecondary>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4,
            Func<TArg1, TArg2, TArg3, TArg4, TPrimary> func, Func<Exception, TSecondary> ifError)
        {
            try { return BinaryAlternate<TPrimary, TSecondary>.AsPrimary(func(arg1, arg2, arg3, arg4)); }
            catch (Exception exception) { return BinaryAlternate<TPrimary, TSecondary>.AsSecondary(ifError(exception)); }
        }

        public static BinaryAlternate<TPrimary, Exception> TryInvoke<TArg1, TArg2, TArg3, TArg4, TPrimary>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4,
            Func<TArg1, TArg2, TArg3, TArg4, TPrimary> func)
        {
            try { return BinaryAlternate<TPrimary, Exception>.AsPrimary(func(arg1, arg2, arg3, arg4)); }
            catch (Exception exception) { return BinaryAlternate<TPrimary, Exception>.AsSecondary(exception); }
        }

        public static BinaryAlternate<TPrimary, TSecondary> TryInvoke<TArg1, TArg2, TArg3, TPrimary, TSecondary>(TArg1 arg1, TArg2 arg2, TArg3 arg3,
            Func<TArg1, TArg2, TArg3, TPrimary> func, Func<Exception, TSecondary> ifError)
        {
            try { return BinaryAlternate<TPrimary, TSecondary>.AsPrimary(func(arg1, arg2, arg3)); }
            catch (Exception exception) { return BinaryAlternate<TPrimary, TSecondary>.AsSecondary(ifError(exception)); }
        }

        public static BinaryAlternate<TPrimary, Exception> TryInvoke<TArg1, TArg2, TArg3, TPrimary>(TArg1 arg1, TArg2 arg2, TArg3 arg3,
            Func<TArg1, TArg2, TArg3, TPrimary> func)
        {
            try { return BinaryAlternate<TPrimary, Exception>.AsPrimary(func(arg1, arg2, arg3)); }
            catch (Exception exception) { return BinaryAlternate<TPrimary, Exception>.AsSecondary(exception); }
        }

        public static BinaryAlternate<TPrimary, TSecondary> TryInvoke<TArg1, TArg2, TPrimary, TSecondary>(TArg1 arg1, TArg2 arg2, Func<TArg1, TArg2, TPrimary> func,
            Func<Exception, TSecondary> ifError)
        {
            try { return BinaryAlternate<TPrimary, TSecondary>.AsPrimary(func(arg1, arg2)); }
            catch (Exception exception) { return BinaryAlternate<TPrimary, TSecondary>.AsSecondary(ifError(exception)); }
        }

        public static BinaryAlternate<TPrimary, Exception> TryInvoke<TArg1, TArg2, TPrimary>(TArg1 arg1, TArg2 arg2, Func<TArg1, TArg2, TPrimary> func)
        {
            try { return BinaryAlternate<TPrimary, Exception>.AsPrimary(func(arg1, arg2)); }
            catch (Exception exception) { return BinaryAlternate<TPrimary, Exception>.AsSecondary(exception); }
        }

        public static BinaryAlternate<TPrimary, TSecondary> TryInvoke<TArg, TPrimary, TSecondary>(TArg arg, Func<TArg, TPrimary> func,
            Func<Exception, TSecondary> ifError)
        {
            try { return BinaryAlternate<TPrimary, TSecondary>.AsPrimary(func(arg)); }
            catch (Exception exception) { return BinaryAlternate<TPrimary, TSecondary>.AsSecondary(ifError(exception)); }
        }

        public static BinaryAlternate<TPrimary, Exception> TryInvoke<TArg, TPrimary>(TArg arg, Func<TArg, TPrimary> func)
        {
            try { return BinaryAlternate<TPrimary, Exception>.AsPrimary(func(arg)); }
            catch (Exception exception) { return BinaryAlternate<TPrimary, Exception>.AsSecondary(exception); }
        }

        public static BinaryAlternate<TPrimary, TSecondary> TryInvoke<TPrimary, TSecondary>(Func<TPrimary> func, Func<Exception, TSecondary> ifError)
        {
            try { return BinaryAlternate<TPrimary, TSecondary>.AsPrimary(func()); }
            catch (Exception exception) { return BinaryAlternate<TPrimary, TSecondary>.AsSecondary(ifError(exception)); }
        }

        public static BinaryAlternate<TPrimary, Exception> TryInvoke<TPrimary>(Func<TPrimary> func)
        {
            try { return BinaryAlternate<TPrimary, Exception>.AsPrimary(func()); }
            catch (Exception exception) { return BinaryAlternate<TPrimary, Exception>.AsSecondary(exception); }
        }
    }

    public class BinaryAlternate<TPrimary, TSecondary>
    {
        private readonly TPrimary _primaryValue;
        private readonly TSecondary _secondaryValue;
        public bool IsPrimary { get; }
        public TPrimary PrimaryValue => IsPrimary ? _primaryValue : throw new InvalidOperationException();
        public TSecondary SecondaryValue => IsPrimary ? throw new InvalidOperationException() : _secondaryValue;

        private BinaryAlternate(TPrimary primary, TSecondary secondary, bool isPrimary)
        {
            IsPrimary = isPrimary;
            _primaryValue = primary;
            _secondaryValue = secondary;
        }

        public bool Test(Func<TPrimary, bool> ifPrimary, Func<TSecondary, bool> ifSecondary) => IsPrimary ? ifPrimary(_primaryValue) : ifSecondary(_secondaryValue);

        public bool Test(Func<TPrimary, bool> ifPrimary) => IsPrimary && ifPrimary(_primaryValue);

        public bool TryGetPrimary(out TPrimary result)
        {
            result = _primaryValue;
            return IsPrimary;
        }

        public bool TryGetSecondary(out TPrimary result)
        {
            result = _primaryValue;
            return IsPrimary;
        }

        public TPrimary ToPrimaryValue(Func<TSecondary, TPrimary> ifSecondary) => IsPrimary ? _primaryValue : ifSecondary(_secondaryValue);

        public TSecondary ToSecondaryValue(Func<TPrimary, TSecondary> ifPrimary) => IsPrimary ? _secondaryValue : ifPrimary(_primaryValue);

        public bool IfPrimary(Action<TPrimary> action)
        {
            if (IsPrimary)
            {
                action(_primaryValue);
                return true;
            }
            return false;
        }

        public bool IfSecondary(Action<TSecondary> action)
        {
            if (IsPrimary)
                return false;
            action(_secondaryValue);
            return true;
        }

        public BinaryAlternate<TSecondary, TPrimary> CreateInverse() => new BinaryAlternate<TSecondary, TPrimary>(_secondaryValue, _primaryValue, !IsPrimary);

        public bool Apply(Action<TPrimary> ifPrimary, Action<TSecondary> ifSecondary)
        {
            if (IsPrimary)
                ifPrimary(_primaryValue);
            else
                ifSecondary(_secondaryValue);
            return IsPrimary;
        }

        public BinaryAlternate<TPrimary, TSecondary> Filter(Predicate<TPrimary> predicateIfPrimary, Predicate<TSecondary> predicateIfSecondary,
            Func<TSecondary> ifPrimaryFalse, Func<TPrimary> ifSecondaryFalse) => IsPrimary ?
                (predicateIfPrimary(_primaryValue) ? this : new BinaryAlternate<TPrimary, TSecondary>(default, ifPrimaryFalse(), false)) :
                (predicateIfSecondary(_secondaryValue) ? this : new BinaryAlternate<TPrimary, TSecondary>(ifSecondaryFalse(), default, false));

        public BinaryAlternate<TPrimary, TSecondary> Filter(Predicate<TPrimary> predicateIfPrimary, Predicate<TSecondary> predicateIfSecondary,
            TSecondary ifPrimaryFalse, TPrimary ifSecondaryFalse) => IsPrimary ?
                (predicateIfPrimary(_primaryValue) ? this : new BinaryAlternate<TPrimary, TSecondary>(default, ifPrimaryFalse, false)) :
                (predicateIfSecondary(_secondaryValue) ? this : new BinaryAlternate<TPrimary, TSecondary>(ifSecondaryFalse, default, false));

        public BinaryAlternate<TPrimary, TSecondary> Filter(Predicate<TPrimary> predicateIfPrimary, Func<TSecondary> ifFalse) =>
            (IsPrimary && predicateIfPrimary(_primaryValue)) ? this : new BinaryAlternate<TPrimary, TSecondary>(default, ifFalse(), false);

        public BinaryAlternate<TPrimary, TSecondary> Filter(Predicate<TPrimary> predicateIfPrimary, TSecondary ifFalse) =>
            (IsPrimary && predicateIfPrimary(_primaryValue)) ? this : new BinaryAlternate<TPrimary, TSecondary>(default, ifFalse, false);

        public TResult Flatten<TResult>(Func<TPrimary, TResult> ifPrimary, Func<TSecondary, TResult> ifSecondary) =>
            IsPrimary ? ifPrimary(_primaryValue) : ifSecondary(_secondaryValue);

        public TPrimary FlattenAsPrimary(Func<TPrimary, bool> conditional, Func<TPrimary> ifSecondaryOrFalse) => (IsPrimary && conditional(_primaryValue)) ?
            _primaryValue : ifSecondaryOrFalse();

        public TPrimary FlattenAsPrimary(Func<TSecondary, TPrimary> ifSecondary) => IsPrimary ? _primaryValue : ifSecondary(_secondaryValue);

        public TSecondary FlattenAsSecondary(Func<TSecondary, bool> conditional, Func<TSecondary> ifPrimaryOrFalse) => (IsPrimary || !conditional(_secondaryValue)) ?
            ifPrimaryOrFalse() : _secondaryValue;

        public TSecondary FlattenAsSecondary(Func<TPrimary, TSecondary> ifPrimary) => IsPrimary ? ifPrimary(_primaryValue) : _secondaryValue;

        public BinaryAlternate<TAltPrimary, TAltSecondary> Map<TAltPrimary, TAltSecondary>(Func<TPrimary, TAltPrimary> ifPrimary,
            Func<TSecondary, TAltSecondary> ifSecondary) =>
            IsPrimary ? new BinaryAlternate<TAltPrimary, TAltSecondary>(ifPrimary(_primaryValue), default, true) :
            new BinaryAlternate<TAltPrimary, TAltSecondary>(default, ifSecondary(_secondaryValue), false);

        public BinaryAlternate<TAltPrimary, TSecondary> Map<TAltPrimary>(Func<TPrimary, TAltPrimary> ifPrimary) =>
            IsPrimary ? new BinaryAlternate<TAltPrimary, TSecondary>(ifPrimary(_primaryValue), default, true) :
            new BinaryAlternate<TAltPrimary, TSecondary>(default, _secondaryValue, false);

        public BinaryAlternate<TPrimary, TNewSecondary> PositiveMerge<TAltSecondary, TNewSecondary>(BinaryAlternate<TPrimary, TAltSecondary> other,
            Func<TPrimary, TPrimary, TPrimary> mergePrimaryFunc, Func<TSecondary, TAltSecondary, TNewSecondary> mergeSecondaryFunc)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));
            if (mergePrimaryFunc is null)
                throw new ArgumentNullException(nameof(mergePrimaryFunc));
            if (mergeSecondaryFunc is null)
                throw new ArgumentNullException(nameof(mergeSecondaryFunc));
            if (IsPrimary)
                return new BinaryAlternate<TPrimary, TNewSecondary>(other.IsPrimary ? mergePrimaryFunc(_primaryValue, other._primaryValue) : _primaryValue, default, true);
            if (other.IsPrimary)
                return new BinaryAlternate<TPrimary, TNewSecondary>(other._primaryValue, default, true);
            return new BinaryAlternate<TPrimary, TNewSecondary>(default, mergeSecondaryFunc(_secondaryValue, other._secondaryValue), false);
        }

        public BinaryAlternate<TPrimary, TSecondary> PositiveMerge(BinaryAlternate<TPrimary, TSecondary> other, Func<TPrimary, TPrimary, TPrimary> mergePrimaryFunc)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));
            if (mergePrimaryFunc is null)
                throw new ArgumentNullException(nameof(mergePrimaryFunc));
            if (IsPrimary)
                return new BinaryAlternate<TPrimary, TSecondary>(other.IsPrimary ? mergePrimaryFunc(_primaryValue, other._primaryValue) : _primaryValue, default, true);
            if (other.IsPrimary)
                return other;
            return this;
        }

        public BinaryAlternate<TPrimary, TSecondary> OrFirstPrimary(params BinaryAlternate<TPrimary, TSecondary>[] other)
        {
            if (IsPrimary || other is null)
                return this;
            return other.Where(o => !(o is null) && o.IsPrimary).DefaultIfEmpty(this).First();
        }

        public BinaryAlternate<TPrimary, TSecondary> AndNoneSecondary(params BinaryAlternate<TPrimary, TSecondary>[] other)
        {
            if (!IsPrimary || other is null)
                return this;
            return other.Where(o => !(o is null || o.IsPrimary)).DefaultIfEmpty(this).First();
        }

        public BinaryAlternate<TNewPrimary, TSecondary> NegativeMerge<TAltPrimary, TNewPrimary>(BinaryAlternate<TAltPrimary, TSecondary> other,
            Func<TPrimary, TAltPrimary, TNewPrimary> mergePrimaryFunc, Func<TSecondary, TSecondary, TSecondary> mergeSecondaryFunc)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));
            if (mergePrimaryFunc is null)
                throw new ArgumentNullException(nameof(mergePrimaryFunc));
            if (mergeSecondaryFunc is null)
                throw new ArgumentNullException(nameof(mergeSecondaryFunc));
            if (IsPrimary)
            {
                if (other.IsPrimary)
                    return new BinaryAlternate<TNewPrimary, TSecondary>(mergePrimaryFunc(_primaryValue, other._primaryValue), default, true);
                return new BinaryAlternate<TNewPrimary, TSecondary>(default, other._secondaryValue, false);
            }
            return new BinaryAlternate<TNewPrimary, TSecondary>(default, _secondaryValue, false);
        }

        public static BinaryAlternate<TPrimary, TSecondary> AsPrimary(TPrimary value) => new BinaryAlternate<TPrimary, TSecondary>(value, default, true);

        public static BinaryAlternate<TPrimary, TSecondary> AsSecondary(TSecondary value) => new BinaryAlternate<TPrimary, TSecondary>(default, value, false);
    }

}
