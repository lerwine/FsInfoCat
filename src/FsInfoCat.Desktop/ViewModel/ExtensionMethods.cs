using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel
{
    public static class ExtensionMethods
    {
        public static readonly Regex NormalizableWsRegex = new Regex(@" \s+|(?! )\s+", RegexOptions.Compiled);

        public static void CheckInvoke<T1, T2>(this Dispatcher dispatcher, Action<T1, T2> action, T1 arg1, T2 arg2)
        {
            if (dispatcher is null)
                throw new ArgumentNullException(nameof(dispatcher));
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            if (dispatcher.CheckAccess())
                action(arg1, arg2);
            else
                dispatcher.Invoke(action, arg1, arg2);
        }

        public static TResult CheckInvoke<T1, T2, TResult>(this Dispatcher dispatcher, Func<T1, T2, TResult> function, T1 arg1, T2 arg2)
        {
            if (dispatcher is null)
                throw new ArgumentNullException(nameof(dispatcher));
            if (function is null)
                throw new ArgumentNullException(nameof(function));
            if (dispatcher.CheckAccess())
                return function(arg1, arg2);
            return (TResult)dispatcher.Invoke(function, arg1, arg2);
        }

        public static void CheckInvoke<T>(this Dispatcher dispatcher, Action<T> action, T arg)
        {
            if (dispatcher is null)
                throw new ArgumentNullException(nameof(dispatcher));
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            if (dispatcher.CheckAccess())
                action(arg);
            else
                dispatcher.Invoke(action, arg);
        }

        public static TResult CheckInvoke<T, TResult>(this Dispatcher dispatcher, Func<T, TResult> function, T arg)
        {
            if (dispatcher is null)
                throw new ArgumentNullException(nameof(dispatcher));
            if (function is null)
                throw new ArgumentNullException(nameof(function));
            if (dispatcher.CheckAccess())
                return function(arg);
            return (TResult)dispatcher.Invoke(function, arg);
        }

        public static void CheckInvoke(this Dispatcher dispatcher, Action action)
        {
            if (dispatcher is null)
                throw new ArgumentNullException(nameof(dispatcher));
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            if (dispatcher.CheckAccess())
                action();
            else
                dispatcher.Invoke(action);
        }

        public static TResult CheckInvoke<TResult>(this Dispatcher dispatcher, Func<TResult> function)
        {
            if (dispatcher is null)
                throw new ArgumentNullException(nameof(dispatcher));
            if (function is null)
                throw new ArgumentNullException(nameof(function));
            if (dispatcher.CheckAccess())
                return function();
            return (TResult)dispatcher.Invoke(function);
        }

        public static CoerceValueCallback ToCoerceValueCallback<TResult>(TResult defaultValue) => (DependencyObject d, object baseValue) =>
            (baseValue is TResult result) ? result : defaultValue;

        public static CoerceValueCallback ToCoerceValueCallback<TResult>(Func<TResult> getDefaultValue) => (DependencyObject d, object baseValue) =>
            (baseValue is TResult result) ? result : getDefaultValue();

        public static CoerceValueCallback ToCoerceValueCallback<TResult>(Func<object, TResult> func) => (DependencyObject d, object baseValue) =>
            (baseValue is TResult result) ? result : func(baseValue);

        public static CoerceValueCallback ToCoerceValueCallback<TResult>(TResult defaultValue, Func<object, TResult> func) => (DependencyObject d, object baseValue) =>
            (baseValue is null) ? defaultValue : ((baseValue is TResult result) ? result : func(baseValue));

        public static CoerceValueCallback ToCoerceValueCallback<TResult>(Func<TResult> getDefaultValue, Func<object, TResult> func) => (DependencyObject d, object baseValue) =>
            (baseValue is null) ? getDefaultValue() : ((baseValue is TResult result) ? result : func(baseValue));

        public static string CoerceAsNonNullString(object baseValue) => (baseValue is string s) ? s : ((baseValue is null) ? "" : baseValue.ToString());

        public static string CoerceAsNonNullTrimmedString(object baseValue) => (baseValue is string s) ? s.Trim() : ((baseValue is null) ? "" : baseValue.ToString().Trim());

        public static string CoerceAsTrimmedString(object baseValue) => (baseValue is string s) ? s.Trim() : ((baseValue is null) ? null : baseValue.ToString().Trim());

        public static string CoerceAsNonNullWsNormalizedString(object baseValue)
        {
            if (baseValue is null)
                return "";
            return (((baseValue is string s) ? s : (s = baseValue.ToString())).Length > 0 && (s = s.Trim()).Length > 0 && NormalizableWsRegex.IsMatch(s)) ?
                NormalizableWsRegex.Replace(s, " ") : s;
        }

        public static string CoerceAsWsNormalizedString(object baseValue)
        {
            if (baseValue is null)
                return null;
            return (((baseValue is string s) ? s : (s = baseValue.ToString())).Length > 0 && (s = s.Trim()).Length > 0 && NormalizableWsRegex.IsMatch(s)) ?
                NormalizableWsRegex.Replace(s, " ") : s;
        }

        public static readonly DependencyProperty BindToLoginViewModel = DependencyProperty.RegisterAttached(nameof(BindToLoginViewModel), typeof(LoginViewModel), typeof(LoginViewModel),
            new PropertyMetadata(null, OnBindToModelPropertyChanged));

        public static LoginViewModel GetBindToLoginViewModel(DependencyObject obj) => (obj is null) ? null : (LoginViewModel)obj.GetValue(BindToLoginViewModel);

        public static void SetBindToLoginViewModel(DependencyObject obj, LoginViewModel newValue) => obj.SetValue(BindToLoginViewModel, newValue);

        private static void OnBindToModelPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is PasswordBox passwordBox)
            {
                if (e.OldValue is LoginViewModel wasBound)
                    passwordBox.PasswordChanged -= wasBound.OnPasswordChanged;
                if (e.NewValue is LoginViewModel needToBind)
                    passwordBox.PasswordChanged += needToBind.OnPasswordChanged;
            }
        }

        public static Action ToInvocationAction(this Dispatcher dispatcher, Action action, bool checkAccess = false)
        {
            if (action is null)
                return null;
            if (checkAccess)
                return () =>
                {
                    if (dispatcher.CheckAccess())
                        action();
                    else
                        dispatcher.Invoke(action);
                };
            return () => dispatcher.Invoke(action);
        }

        public static Action ToInvocationAction(this DependencyObject obj, Action action, bool checkAccess = false) =>
            ToInvocationAction((obj ?? throw new ArgumentNullException(nameof(obj))).Dispatcher, action, checkAccess);

        public static Action<T> ToInvocationAction<T>(this Dispatcher dispatcher, Action<T> action, bool checkAccess = false)
        {
            if (action is null)
                return null;
            if (checkAccess)
                return arg =>
                {
                    if (dispatcher.CheckAccess())
                        action(arg);
                    else
                        dispatcher.Invoke(action, arg);
                };
            return arg => dispatcher.Invoke(action, arg);
        }

        public static Action<T> ToInvocationAction<T>(this DependencyObject obj, Action<T> action, bool checkAccess = false) =>
            ToInvocationAction((obj ?? throw new ArgumentNullException(nameof(obj))).Dispatcher, action, checkAccess);

        public static Action<T1, T2> ToInvocationAction<T1, T2>(this Dispatcher dispatcher, Action<T1, T2> action, bool checkAccess = false)
        {
            if (action is null)
                return null;
            if (checkAccess)
                return (arg1, arg2) =>
                {
                    if (dispatcher.CheckAccess())
                        action(arg1, arg2);
                    else
                        dispatcher.Invoke(action, arg1, arg2);
                };
            return (arg1, arg2) => dispatcher.Invoke(action, arg1, arg2);
        }

        public static Action<T1, T2> ToInvocationAction<T1, T2>(this DependencyObject obj, Action<T1, T2> action, bool checkAccess = false) =>
            ToInvocationAction((obj ?? throw new ArgumentNullException(nameof(obj))).Dispatcher, action, checkAccess);

        public static Action<T1, T2, T3> ToInvocationAction<T1, T2, T3>(this Dispatcher dispatcher, Action<T1, T2, T3> action, bool checkAccess = false)
        {
            if (action is null)
                return null;
            if (checkAccess)
                return (arg1, arg2, arg3) =>
                {
                    if (dispatcher.CheckAccess())
                        action(arg1, arg2, arg3);
                    else
                        dispatcher.Invoke(action, arg1, arg2, arg3);
                };
            return (arg1, arg2, arg3) => dispatcher.Invoke(action, arg1, arg2, arg3);
        }

        public static Action<T1, T2, T3> ToInvocationAction<T1, T2, T3>(this DependencyObject obj, Action<T1, T2, T3> action, bool checkAccess = false) =>
            ToInvocationAction((obj ?? throw new ArgumentNullException(nameof(obj))).Dispatcher, action, checkAccess);

        public static Action<T1, T2, T3, T4> ToInvocationAction<T1, T2, T3, T4>(this Dispatcher dispatcher, Action<T1, T2, T3, T4> action, bool checkAccess = false)
        {
            if (action is null)
                return null;
            if (checkAccess)
                return (arg1, arg2, arg3, arg4) =>
                {
                    if (dispatcher.CheckAccess())
                        action(arg1, arg2, arg3, arg4);
                    else
                        dispatcher.Invoke(action, arg1, arg2, arg3, arg4);
                };
            return (arg1, arg2, arg3, arg4) => dispatcher.Invoke(action, arg1, arg2, arg3, arg4);
        }

        public static Action<T1, T2, T3, T4> ToInvocationAction<T1, T2, T3, T4>(this DependencyObject obj, Action<T1, T2, T3, T4> action, bool checkAccess = false) =>
            ToInvocationAction((obj ?? throw new ArgumentNullException(nameof(obj))).Dispatcher, action, checkAccess);

        public static Action<T1, T2, T3, T4, T5> ToInvocationAction<T1, T2, T3, T4, T5>(this Dispatcher dispatcher, Action<T1, T2, T3, T4, T5> action, bool checkAccess = false)
        {
            if (action is null)
                return null;
            if (checkAccess)
                return (arg1, arg2, arg3, arg4, arg5) =>
                {
                    if (dispatcher.CheckAccess())
                        action(arg1, arg2, arg3, arg4, arg5);
                    else
                        dispatcher.Invoke(action, arg1, arg2, arg3, arg4, arg5);
                };
            return (arg1, arg2, arg3, arg4, arg5) => dispatcher.Invoke(action, arg1, arg2, arg3, arg4, arg5);
        }

        public static Action<T1, T2, T3, T4, T5> ToInvocationAction<T1, T2, T3, T4, T5>(this DependencyObject obj, Action<T1, T2, T3, T4, T5> action, bool checkAccess = false) =>
            ToInvocationAction((obj ?? throw new ArgumentNullException(nameof(obj))).Dispatcher, action, checkAccess);

        public static Action AsBeginInvocationAction(this Dispatcher dispatcher, Action action)
        {
            if (action is null)
                return null;
            return () => dispatcher.BeginInvoke(action);
        }

        public static Action AsBeginInvocationAction(this DependencyObject obj, Action action) =>
            AsBeginInvocationAction((obj ?? throw new ArgumentNullException(nameof(obj))).Dispatcher, action);

        public static Action<T> AsBeginInvocationAction<T>(this Dispatcher dispatcher, Action<T> action)
        {
            if (action is null)
                return null;
            return arg => dispatcher.BeginInvoke(action, arg);
        }

        public static Action<T> AsBeginInvocationAction<T>(this DependencyObject obj, Action<T> action) =>
            AsBeginInvocationAction((obj ?? throw new ArgumentNullException(nameof(obj))).Dispatcher, action);

        public static Action<T1, T2> AsBeginInvocationAction<T1, T2>(this Dispatcher dispatcher, Action<T1, T2> action)
        {
            if (action is null)
                return null;
            return (arg1, arg2) => dispatcher.BeginInvoke(action, arg1, arg2);
        }

        public static Action<T1, T2> AsBeginInvocationAction<T1, T2>(this DependencyObject obj, Action<T1, T2> action) =>
            AsBeginInvocationAction((obj ?? throw new ArgumentNullException(nameof(obj))).Dispatcher, action);

        public static Action<T1, T2, T3> AsBeginInvocationAction<T1, T2, T3>(this Dispatcher dispatcher, Action<T1, T2, T3> action)
        {
            if (action is null)
                return null;
            return (arg1, arg2, arg3) => dispatcher.BeginInvoke(action, arg1, arg2, arg3);
        }

        public static Action<T1, T2, T3> AsBeginInvocationAction<T1, T2, T3>(this DependencyObject obj, Action<T1, T2, T3> action) =>
            AsBeginInvocationAction((obj ?? throw new ArgumentNullException(nameof(obj))).Dispatcher, action);

        public static Action<T1, T2, T3, T4> AsBeginInvocationAction<T1, T2, T3, T4>(this Dispatcher dispatcher, Action<T1, T2, T3, T4> action)
        {
            if (action is null)
                return null;
            return (arg1, arg2, arg3, arg4) => dispatcher.BeginInvoke(action, arg1, arg2, arg3, arg4);
        }

        public static Action<T1, T2, T3, T4> AsBeginInvocationAction<T1, T2, T3, T4>(this DependencyObject obj, Action<T1, T2, T3, T4> action) =>
            AsBeginInvocationAction((obj ?? throw new ArgumentNullException(nameof(obj))).Dispatcher, action);

        public static Action<T1, T2, T3, T4, T5> AsBeginInvocationAction<T1, T2, T3, T4, T5>(this Dispatcher dispatcher, Action<T1, T2, T3, T4, T5> action)
        {
            if (action is null)
                return null;
            return (arg1, arg2, arg3, arg4, arg5) => dispatcher.BeginInvoke(action, arg1, arg2, arg3, arg4, arg5);
        }

        public static Action<T1, T2, T3, T4, T5> AsBeginInvocationAction<T1, T2, T3, T4, T5>(this DependencyObject obj, Action<T1, T2, T3, T4, T5> action) =>
            AsBeginInvocationAction((obj ?? throw new ArgumentNullException(nameof(obj))).Dispatcher, action);

        public static Func<TResult> ToInvocationFunction<TResult>(this Dispatcher dispatcher, Func<TResult> function, bool checkAccess = false)
        {
            if (function is null)
                return null;
            if (checkAccess)
                return () =>
                {
                    if (dispatcher.CheckAccess())
                        return function();
                    return dispatcher.Invoke(function);
                };
            return () => dispatcher.Invoke(function);
        }

        public static Func<TResult> ToInvocationFunction<TResult>(this DependencyObject obj, Func<TResult> function, bool checkAccess = false) =>
            ToInvocationFunction((obj ?? throw new ArgumentNullException(nameof(obj))).Dispatcher, function, checkAccess);

        public static Func<T, TResult> ToInvocationFunction<T, TResult>(this Dispatcher dispatcher, Func<T, TResult> function, bool checkAccess = false)
        {
            if (function is null)
                return null;
            if (checkAccess)
                return arg =>
                {
                    if (dispatcher.CheckAccess())
                        return function(arg);
                    return (TResult)dispatcher.Invoke(function, arg);
                };
            return arg => (TResult)dispatcher.Invoke(function, arg);
        }

        public static Func<T, TResult> ToInvocationFunction<T, TResult>(this DependencyObject obj, Func<T, TResult> function, bool checkAccess = false) =>
            ToInvocationFunction((obj ?? throw new ArgumentNullException(nameof(obj))).Dispatcher, function, checkAccess);

        public static Func<T1, T2, TResult> ToInvocationFunction<T1, T2, TResult>(this Dispatcher dispatcher, Func<T1, T2, TResult> function, bool checkAccess = false)
        {
            if (function is null)
                return null;
            if (checkAccess)
                return (arg1, arg2) =>
                {
                    if (dispatcher.CheckAccess())
                        return function(arg1, arg2);
                    return (TResult)dispatcher.Invoke(function, arg1, arg2);
                };
            return (arg1, arg2) => (TResult)dispatcher.Invoke(function, arg1, arg2);
        }

        public static Func<T1, T2, TResult> ToInvocationFunction<T1, T2, TResult>(this DependencyObject obj, Func<T1, T2, TResult> function, bool checkAccess = false) =>
            ToInvocationFunction((obj ?? throw new ArgumentNullException(nameof(obj))).Dispatcher, function, checkAccess);

        public static Func<T1, T2, T3, TResult> ToInvocationFunction<T1, T2, T3, TResult>(this Dispatcher dispatcher, Func<T1, T2, T3, TResult> function, bool checkAccess = false)
        {
            if (function is null)
                return null;
            if (checkAccess)
                return (arg1, arg2, arg3) =>
                {
                    if (dispatcher.CheckAccess())
                        return function(arg1, arg2, arg3);
                    return (TResult)dispatcher.Invoke(function, arg1, arg2, arg3);
                };
            return (arg1, arg2, arg3) => (TResult)dispatcher.Invoke(function, arg1, arg2, arg3);
        }

        public static Func<T1, T2, T3, TResult> ToInvocationFunction<T1, T2, T3, TResult>(this DependencyObject obj, Func<T1, T2, T3, TResult> function, bool checkAccess = false) =>
            ToInvocationFunction((obj ?? throw new ArgumentNullException(nameof(obj))).Dispatcher, function, checkAccess);

        public static Func<T1, T2, T3, T4, TResult> ToInvocationFunction<T1, T2, T3, T4, TResult>(this Dispatcher dispatcher, Func<T1, T2, T3, T4, TResult> function, bool checkAccess = false)
        {
            if (function is null)
                return null;
            if (checkAccess)
                return (arg1, arg2, arg3, arg4) =>
                {
                    if (dispatcher.CheckAccess())
                        return function(arg1, arg2, arg3, arg4);
                    return (TResult)dispatcher.Invoke(function, arg1, arg2, arg3, arg4);
                };
            return (arg1, arg2, arg3, arg4) => (TResult)dispatcher.Invoke(function, arg1, arg2, arg3, arg4);
        }

        public static Func<T1, T2, T3, T4, TResult> ToInvocationFunction<T1, T2, T3, T4, TResult>(this DependencyObject obj, Func<T1, T2, T3, T4, TResult> function, bool checkAccess = false) =>
            ToInvocationFunction((obj ?? throw new ArgumentNullException(nameof(obj))).Dispatcher, function, checkAccess);

        public static Func<T1, T2, T3, T4, T5, TResult> ToInvocationFunction<T1, T2, T3, T4, T5, TResult>(this Dispatcher dispatcher, Func<T1, T2, T3, T4, T5, TResult> function, bool checkAccess = false)
        {
            if (function is null)
                return null;
            if (checkAccess)
                return (arg1, arg2, arg3, arg4, arg5) =>
                {
                    if (dispatcher.CheckAccess())
                        return function(arg1, arg2, arg3, arg4, arg5);
                    return (TResult)dispatcher.Invoke(function, arg1, arg2, arg3, arg4, arg5);
                };
            return (arg1, arg2, arg3, arg4, arg5) => (TResult)dispatcher.Invoke(function, arg1, arg2, arg3, arg4, arg5);
        }

        public static Func<T1, T2, T3, T4, T5, TResult> ToInvocationFunction<T1, T2, T3, T4, T5, TResult>(this DependencyObject obj, Func<T1, T2, T3, T4, T5, TResult> function, bool checkAccess = false) =>
            ToInvocationFunction((obj ?? throw new ArgumentNullException(nameof(obj))).Dispatcher, function, checkAccess);
    }
}
