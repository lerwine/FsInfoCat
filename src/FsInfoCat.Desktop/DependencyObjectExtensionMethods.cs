using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace FsInfoCat.Desktop
{
    public static class DependencyObjectExtensionMethods
    {
        public static void CheckInvoke<TArg1, TArg2, TArg3>(this Dispatcher dispatcher, TArg1 arg1, TArg2 arg2, TArg3 arg3, Action<TArg1, TArg2, TArg3> callback, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (dispatcher.CheckAccess())
                callback(arg1, arg2, arg3);
            else
                dispatcher.Invoke(() => callback(arg1, arg2, arg3), priority);
        }
        public static void CheckInvoke<TArg1, TArg2>(this Dispatcher dispatcher, TArg1 arg1, TArg2 arg2, Action<TArg1, TArg2> callback, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (dispatcher.CheckAccess())
                callback(arg1, arg2);
            else
                dispatcher.Invoke(() => callback(arg1, arg2), priority);
        }
        public static void CheckInvoke<TArg>(this Dispatcher dispatcher, TArg arg, Action<TArg> callback, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (dispatcher.CheckAccess())
                callback(arg);
            else
                dispatcher.Invoke(() => callback(arg), priority);
        }
        public static void CheckInvoke(this Dispatcher dispatcher, Action action, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (dispatcher.CheckAccess())
                action();
            else
                dispatcher.Invoke(action, priority);
        }
        public static void CheckInvoke<TArg1, TArg2, TArg3>(this Dispatcher dispatcher, TArg1 arg1, TArg2 arg2, TArg3 arg3, CancellationToken cancellationToken, Action<TArg1, TArg2, TArg3> callback, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (dispatcher.CheckAccess())
            {
                cancellationToken.ThrowIfCancellationRequested();
                callback(arg1, arg2, arg3);
            }
            else
                dispatcher.Invoke(() => callback(arg1, arg2, arg3), priority, cancellationToken);
        }
        public static void CheckInvoke<TArg1, TArg2>(this Dispatcher dispatcher, TArg1 arg1, TArg2 arg2, CancellationToken cancellationToken, Action<TArg1, TArg2> callback, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (dispatcher.CheckAccess())
            {
                cancellationToken.ThrowIfCancellationRequested();
                callback(arg1, arg2);
            }
            else
                dispatcher.Invoke(() => callback(arg1, arg2), priority, cancellationToken);
        }
        public static void CheckInvoke<TArg>(this Dispatcher dispatcher, TArg arg, CancellationToken cancellationToken, Action<TArg> callback, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (dispatcher.CheckAccess())
            {
                cancellationToken.ThrowIfCancellationRequested();
                callback(arg);
            }
            else
                dispatcher.Invoke(() => callback(arg), priority, cancellationToken);
        }
        public static void CheckInvoke(this Dispatcher dispatcher, CancellationToken cancellationToken, Action callback, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (dispatcher.CheckAccess())
            {
                cancellationToken.ThrowIfCancellationRequested();
                callback();
            }
            else
                dispatcher.Invoke(callback, priority, cancellationToken);
        }
        public static void CheckInvoke<TArg1, TArg2, TArg3>(this Dispatcher dispatcher, TArg1 arg1, TArg2 arg2, TArg3 arg3, CancellationToken cancellationToken, TimeSpan timeout, Action<TArg1, TArg2, TArg3> callback, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (dispatcher.CheckAccess())
            {
                cancellationToken.ThrowIfCancellationRequested();
                callback(arg1, arg2, arg3);
            }
            else
                dispatcher.Invoke(() => callback(arg1, arg2, arg3), priority, cancellationToken, timeout);
        }
        public static void CheckInvoke<TArg1, TArg2>(this Dispatcher dispatcher, TArg1 arg1, TArg2 arg2, CancellationToken cancellationToken, TimeSpan timeout, Action<TArg1, TArg2> callback, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (dispatcher.CheckAccess())
            {
                cancellationToken.ThrowIfCancellationRequested();
                callback(arg1, arg2);
            }
            else
                dispatcher.Invoke(() => callback(arg1, arg2), priority, cancellationToken, timeout);
        }
        public static void CheckInvoke<TArg>(this Dispatcher dispatcher, TArg arg, CancellationToken cancellationToken, TimeSpan timeout, Action<TArg> callback, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (dispatcher.CheckAccess())
            {
                cancellationToken.ThrowIfCancellationRequested();
                callback(arg);
            }
            else
                dispatcher.Invoke(() => callback(arg), priority, cancellationToken, timeout);
        }
        public static void CheckInvoke(this Dispatcher dispatcher, CancellationToken cancellationToken, TimeSpan timeout, Action callback, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (dispatcher.CheckAccess())
            {
                cancellationToken.ThrowIfCancellationRequested();
                callback();
            }
            else
                dispatcher.Invoke(callback, priority, cancellationToken, timeout);
        }
        public static TResult CheckInvoke<TArg1, TArg2, TArg3, TResult>(this Dispatcher dispatcher, TArg1 arg1, TArg2 arg2, TArg3 arg3, CancellationToken cancellationToken, TimeSpan timeout, Func<TArg1, TArg2, TArg3, TResult> callback, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (dispatcher.CheckAccess())
            {
                cancellationToken.ThrowIfCancellationRequested();
                return callback(arg1, arg2, arg3);
            }
            return dispatcher.Invoke(() => callback(arg1, arg2, arg3), priority, cancellationToken, timeout);
        }
        public static TResult CheckInvoke<TArg1, TArg2, TResult>(this Dispatcher dispatcher, TArg1 arg1, TArg2 arg2, CancellationToken cancellationToken, TimeSpan timeout, Func<TArg1, TArg2, TResult> callback, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (dispatcher.CheckAccess())
            {
                cancellationToken.ThrowIfCancellationRequested();
                return callback(arg1, arg2);
            }
            return dispatcher.Invoke(() => callback(arg1, arg2), priority, cancellationToken, timeout);
        }
        public static TResult CheckInvoke<TArg, TResult>(this Dispatcher dispatcher, TArg arg, CancellationToken cancellationToken, TimeSpan timeout, Func<TArg, TResult> callback, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (dispatcher.CheckAccess())
            {
                cancellationToken.ThrowIfCancellationRequested();
                return callback(arg);
            }
            return dispatcher.Invoke(() => callback(arg), priority, cancellationToken, timeout);
        }
        public static TResult CheckInvoke<TResult>(this Dispatcher dispatcher, CancellationToken cancellationToken, TimeSpan timeout, Func<TResult> callback, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (dispatcher.CheckAccess())
            {
                cancellationToken.ThrowIfCancellationRequested();
                return callback();
            }
            return dispatcher.Invoke(callback, priority, cancellationToken, timeout);
        }
        public static TResult CheckInvoke<TArg1, TArg2, TArg3, TResult>(this Dispatcher dispatcher, TArg1 arg1, TArg2 arg2, TArg3 arg3, Func<TArg1, TArg2, TArg3, TResult> callback, CancellationToken cancellationToken, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (dispatcher.CheckAccess())
            {
                cancellationToken.ThrowIfCancellationRequested();
                return callback(arg1, arg2, arg3);
            }
            return dispatcher.Invoke(() => callback(arg1, arg2, arg3), priority, cancellationToken);
        }
        public static TResult CheckInvoke<TArg1, TArg2, TResult>(this Dispatcher dispatcher, TArg1 arg1, TArg2 arg2, Func<TArg1, TArg2, TResult> callback, CancellationToken cancellationToken, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (dispatcher.CheckAccess())
            {
                cancellationToken.ThrowIfCancellationRequested();
                return callback(arg1, arg2);
            }
            return dispatcher.Invoke(() => callback(arg1, arg2), priority, cancellationToken);
        }
        public static TResult CheckInvoke<TArg, TResult>(this Dispatcher dispatcher, TArg arg, Func<TArg, TResult> callback, CancellationToken cancellationToken, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (dispatcher.CheckAccess())
            {
                cancellationToken.ThrowIfCancellationRequested();
                return callback(arg);
            }
            return dispatcher.Invoke(() => callback(arg), priority, cancellationToken);
        }
        public static TResult CheckInvoke<TResult>(this Dispatcher dispatcher, CancellationToken cancellationToken, Func<TResult> callback, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (dispatcher.CheckAccess())
            {
                cancellationToken.ThrowIfCancellationRequested();
                return callback();
            }
            return dispatcher.Invoke(callback, priority, cancellationToken);
        }
        public static TResult CheckInvoke<TResult>(this Dispatcher dispatcher, Func<TResult> callback, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (dispatcher.CheckAccess())
                return callback();
            return dispatcher.Invoke(callback, priority);
        }
        public static TResult CheckInvoke<TArg1, TArg2, TArg3, TResult>(this Dispatcher dispatcher, TArg1 arg1, TArg2 arg2, TArg3 arg3, Func<TArg1, TArg2, TArg3, TResult> callback, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (dispatcher.CheckAccess())
                return callback(arg1, arg2, arg3);
            return dispatcher.Invoke(() => callback(arg1, arg2, arg3), priority);
        }
        public static TResult CheckInvoke<TArg1, TArg2, TResult>(this Dispatcher dispatcher, TArg1 arg1, TArg2 arg2, Func<TArg1, TArg2, TResult> callback, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (dispatcher.CheckAccess())
                return callback(arg1, arg2);
            return dispatcher.Invoke(() => callback(arg1, arg2), priority);
        }
        public static TResult CheckInvoke<TArg, TResult>(this Dispatcher dispatcher, TArg arg, Func<TArg, TResult> callback, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (dispatcher.CheckAccess())
                return callback(arg);
            return dispatcher.Invoke(() => callback(arg), priority);
        }
        public static TryGetResult<TResult> CheckInvoke<TArg1, TArg2, TArg3, TResult>(this Dispatcher dispatcher, TArg1 arg1, TArg2 arg2, TArg3 arg3, CancellationToken cancellationToken, TimeSpan timeout, TryGetDelegate<TArg1, TArg2, TArg3, TResult> callback, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (dispatcher.CheckAccess())
            {
                cancellationToken.ThrowIfCancellationRequested();
                return new(callback(arg1, arg2, arg3, out TResult result), result);
            }
            return dispatcher.Invoke(() => new TryGetResult<TResult>(callback(arg1, arg2, arg3, out TResult result), result), priority, cancellationToken, timeout);
        }
        public static TryGetResult<TResult> CheckInvoke<TArg1, TArg2, TResult>(this Dispatcher dispatcher, TArg1 arg1, TArg2 arg2, CancellationToken cancellationToken, TimeSpan timeout, TryGetDelegate<TArg1, TArg2, TResult> callback, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (dispatcher.CheckAccess())
            {
                cancellationToken.ThrowIfCancellationRequested();
                return new(callback(arg1, arg2, out TResult result), result);
            }
            return dispatcher.Invoke(() => new TryGetResult<TResult>(callback(arg1, arg2, out TResult result), result), priority, cancellationToken, timeout);
        }
        public static TryGetResult<TResult> CheckInvoke<TArg, TResult>(this Dispatcher dispatcher, TArg arg, CancellationToken cancellationToken, TimeSpan timeout, TryGetDelegate<TArg, TResult> callback, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (dispatcher.CheckAccess())
            {
                cancellationToken.ThrowIfCancellationRequested();
                return new(callback(arg, out TResult result), result);
            }
            return dispatcher.Invoke(() => new TryGetResult<TResult>(callback(arg, out TResult result), result), priority, cancellationToken, timeout);
        }
        public static TryGetResult<TResult> CheckInvoke<TResult>(this Dispatcher dispatcher, CancellationToken cancellationToken, TimeSpan timeout, TryGetDelegate<TResult> callback, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (dispatcher.CheckAccess())
            {
                cancellationToken.ThrowIfCancellationRequested();
                return new(callback(out TResult result), result);
            }
            return dispatcher.Invoke(() => new TryGetResult<TResult>(callback(out TResult result), result), priority, cancellationToken, timeout);
        }
        public static TryGetResult<TResult> CheckInvoke<TArg1, TArg2, TArg3, TResult>(this Dispatcher dispatcher, TArg1 arg1, TArg2 arg2, TArg3 arg3, CancellationToken cancellationToken, TryGetDelegate<TArg1, TArg2, TArg3, TResult> callback, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (dispatcher.CheckAccess())
            {
                cancellationToken.ThrowIfCancellationRequested();
                return new(callback(arg1, arg2, arg3, out TResult result), result);
            }
            return dispatcher.Invoke(() => new TryGetResult<TResult>(callback(arg1, arg2, arg3, out TResult result), result), priority, cancellationToken);
        }
        public static TryGetResult<TResult> CheckInvoke<TArg1, TArg2, TResult>(this Dispatcher dispatcher, TArg1 arg1, TArg2 arg2, CancellationToken cancellationToken, TryGetDelegate<TArg1, TArg2, TResult> callback, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (dispatcher.CheckAccess())
            {
                cancellationToken.ThrowIfCancellationRequested();
                return new(callback(arg1, arg2, out TResult result), result);
            }
            return dispatcher.Invoke(() => new TryGetResult<TResult>(callback(arg1, arg2, out TResult result), result), priority, cancellationToken);
        }
        public static TryGetResult<TResult> CheckInvoke<TArg, TResult>(this Dispatcher dispatcher, TArg arg, CancellationToken cancellationToken, TryGetDelegate<TArg, TResult> callback, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (dispatcher.CheckAccess())
            {
                cancellationToken.ThrowIfCancellationRequested();
                return new(callback(arg, out TResult result), result);
            }
            return dispatcher.Invoke(() => new TryGetResult<TResult>(callback(arg, out TResult result), result), priority, cancellationToken);
        }
        public static TryGetResult<TResult> CheckInvoke<TResult>(this Dispatcher dispatcher, CancellationToken cancellationToken, TryGetDelegate<TResult> callback, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (dispatcher.CheckAccess())
            {
                cancellationToken.ThrowIfCancellationRequested();
                return new(callback(out TResult result), result);
            }
            return dispatcher.Invoke(() => new TryGetResult<TResult>(callback(out TResult result), result), priority, cancellationToken);
        }
        public static TryGetResult<TResult> CheckInvoke<TArg1, TArg2, TArg3, TResult>(this Dispatcher dispatcher, TArg1 arg1, TArg2 arg2, TArg3 arg3, TryGetDelegate<TArg1, TArg2, TArg3, TResult> callback, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (dispatcher.CheckAccess())
                return new(callback(arg1, arg2, arg3, out TResult result), result);
            return dispatcher.Invoke(() => new TryGetResult<TResult>(callback(arg1, arg2, arg3, out TResult result), result), priority);
        }
        public static TryGetResult<TResult> CheckInvoke<TArg1, TArg2, TResult>(this Dispatcher dispatcher, TArg1 arg1, TArg2 arg2, TryGetDelegate<TArg1, TArg2, TResult> callback, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (dispatcher.CheckAccess())
                return new(callback(arg1, arg2, out TResult result), result);
            return dispatcher.Invoke(() => new TryGetResult<TResult>(callback(arg1, arg2, out TResult result), result), priority);
        }
        public static TryGetResult<TResult> CheckInvoke<TArg, TResult>(this Dispatcher dispatcher, TArg arg, TryGetDelegate<TArg, TResult> callback, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (dispatcher.CheckAccess())
                return new(callback(arg, out TResult result), result);
            return dispatcher.Invoke(() => new TryGetResult<TResult>(callback(arg, out TResult result), result), priority);
        }
        public static TryGetResult<TResult> CheckInvoke<TResult>(this Dispatcher dispatcher, TryGetDelegate<TResult> callback, DispatcherPriority priority = DispatcherPriority.Background)
        {
            if (dispatcher.CheckAccess())
                return new(callback(out TResult result), result);
            return dispatcher.Invoke(() => new TryGetResult<TResult>(callback(out TResult result), result), priority);
        }
        public static DispatcherOperation InvokeAsync<TArg1, TArg2, TArg3>(this Dispatcher dispatcher, TArg1 arg1, TArg2 arg2, TArg3 arg3, CancellationToken cancellationToken, Action<TArg1, TArg2, TArg3> callback, DispatcherPriority priority = DispatcherPriority.Background) =>
            dispatcher.InvokeAsync(() => callback(arg1, arg2, arg3), priority, cancellationToken);
        public static DispatcherOperation InvokeAsync<TArg1, TArg2>(this Dispatcher dispatcher, TArg1 arg1, TArg2 arg2, CancellationToken cancellationToken, Action<TArg1, TArg2> callback, DispatcherPriority priority = DispatcherPriority.Background) =>
            dispatcher.InvokeAsync(() => callback(arg1, arg2), priority, cancellationToken);
        public static DispatcherOperation InvokeAsync<TArg>(this Dispatcher dispatcher, TArg arg, CancellationToken cancellationToken, Action<TArg> callback, DispatcherPriority priority = DispatcherPriority.Background) =>
            dispatcher.InvokeAsync(() => callback(arg), priority, cancellationToken);
        public static DispatcherOperation<TResult> InvokeAsync<TArg1, TArg2, TArg3, TResult>(this Dispatcher dispatcher, TArg1 arg1, TArg2 arg2, TArg3 arg3, CancellationToken cancellationToken, Func<TArg1, TArg2, TArg3, TResult> callback, DispatcherPriority priority = DispatcherPriority.Background) =>
            dispatcher.InvokeAsync(() => callback(arg1, arg2, arg3), priority, cancellationToken);
        public static DispatcherOperation<TResult> InvokeAsync<TArg1, TArg2, TResult>(this Dispatcher dispatcher, TArg1 arg1, TArg2 arg2, CancellationToken cancellationToken, Func<TArg1, TArg2, TResult> callback, DispatcherPriority priority = DispatcherPriority.Background) =>
            dispatcher.InvokeAsync(() => callback(arg1, arg2), priority, cancellationToken);
        public static DispatcherOperation<TResult> InvokeAsync<TArg, TResult>(this Dispatcher dispatcher, TArg arg, CancellationToken cancellationToken, Func<TArg, TResult> callback, DispatcherPriority priority = DispatcherPriority.Background) =>
            dispatcher.InvokeAsync(() => callback(arg), priority, cancellationToken);
        public static DispatcherOperation<TryGetResult<TResult>> InvokeAsync<TArg1, TArg2, TArg3, TResult>(this Dispatcher dispatcher, TArg1 arg1, TArg2 arg2, TArg3 arg3, CancellationToken cancellationToken, TryGetDelegate<TArg1, TArg2, TArg3, TResult> callback, DispatcherPriority priority = DispatcherPriority.Background) =>
            dispatcher.InvokeAsync(() => new TryGetResult<TResult>(callback(arg1, arg2, arg3, out TResult result), result), priority, cancellationToken);
        public static DispatcherOperation<TryGetResult<TResult>> InvokeAsync<TArg1, TArg2, TResult>(this Dispatcher dispatcher, TArg1 arg1, TArg2 arg2, CancellationToken cancellationToken, TryGetDelegate<TArg1, TArg2, TResult> callback, DispatcherPriority priority = DispatcherPriority.Background) =>
            dispatcher.InvokeAsync(() => new TryGetResult<TResult>(callback(arg1, arg2, out TResult result), result), priority, cancellationToken);
        public static DispatcherOperation<TryGetResult<TResult>> InvokeAsync<TArg, TResult>(this Dispatcher dispatcher, TArg arg, CancellationToken cancellationToken, TryGetDelegate<TArg, TResult> callback, DispatcherPriority priority = DispatcherPriority.Background) =>
            dispatcher.InvokeAsync(() => new TryGetResult<TResult>(callback(arg, out TResult result), result), priority, cancellationToken);
        public static DispatcherOperation<TryGetResult<TResult>> InvokeAsync<TResult>(this Dispatcher dispatcher, CancellationToken cancellationToken, TryGetDelegate<TResult> callback, DispatcherPriority priority = DispatcherPriority.Background) =>
            dispatcher.InvokeAsync(() => new TryGetResult<TResult>(callback(out TResult result), result), priority, cancellationToken);
    }

    public record TryGetResult<T>(bool Success, T Result);
    public delegate bool TryGetDelegate<TArg1, TArg2, TArg3, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, out TResult result);
    public delegate bool TryGetDelegate<TArg1, TArg2, TResult>(TArg1 arg1, TArg2 arg2, out TResult result);
    public delegate bool TryGetDelegate<TArg, TResult>(TArg arg, out TResult result);
    public delegate bool TryGetDelegate<TResult>(out TResult result);
}
