using FsInfoCat.Models.Crawl;
using FsInfoCat.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Text;

namespace FsInfoCat.PS
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Invokes an <seealso cref="Action{T}"/> if a value is not <c>null</c>.
        /// </summary>
        /// <typeparam name="T">The target type.</typeparam>
        /// <param name="inputObj">The target value.</param>
        /// <param name="ifNotNull">The <seealso cref="Action{T}"/> to invoke if <paramref name="inputObj"/> is not null.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> was not null; otherwise, <c>false</c>.</returns>
        public static bool InvokeIfNotNull<T>(this T inputObj, Action<T> ifNotNull)
            where T : class
        {
            if (ifNotNull is null)
                throw new ArgumentNullException(nameof(ifNotNull));
            if (inputObj is null)
                return false;
            ifNotNull.Invoke(inputObj);
            return true;
        }

        /// <summary>
        /// Invokes an <seealso cref="Action{T}"/> if a value is not <c>null</c>.
        /// </summary>
        /// <typeparam name="T">The target type.</typeparam>
        /// <param name="inputObj">The target value.</param>
        /// <param name="ifNotNull">The <seealso cref="Action{T}"/> to invoke if <paramref name="inputObj"/> is not null.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> was not null; otherwise, <c>false</c>.</returns>
        public static bool InvokeIfNotNull<T>(this T? inputObj, Action<T> ifNotNull)
            where T : struct
        {
            if (ifNotNull is null)
                throw new ArgumentNullException(nameof(ifNotNull));
            if (inputObj.HasValue)
            {
                ifNotNull.Invoke(inputObj.Value);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Attempts to coerce a value to another type.
        /// </summary>
        /// <typeparam name="TInput">The input value type.</typeparam>
        /// <typeparam name="TResult">The result value type.</typeparam>
        /// <param name="inputObj">The input value.</param>
        /// <param name="ifNotNull">The <seealso cref="Func{T, TResult}"/> that produces the result value if <paramref name="inputObj"/> is not null.</param>
        /// <param name="ifNull">The <seealso cref="Func{TResult}"/> that produces the result value if <paramref name="inputObj"/> is null.
        /// If this parameter is null, then the result value will be the default value of <typeparamref name="TResult"/> if <paramref name="inputObj"/> is null.</param>
        /// <param name="result">The result value.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> was null; otherwise, <c>false</c>.</returns>
        public static bool TryCoerceTo<TInput, TResult>(this TInput inputObj, Func<TInput, TResult> ifNotNull, Func<TResult> ifNull, out TResult result)
            where TInput : class
        {
            if (ifNotNull is null)
                throw new ArgumentNullException(nameof(ifNotNull));
            if (inputObj is null)
            {
                result = (ifNull is null) ? default(TResult) : ifNull();
                return false;
            }
            result = ifNotNull(inputObj);
            return true;
        }

        /// <summary>
        /// Attempts to coerce one value from another.
        /// </summary>
        /// <typeparam name="TInput">The input value type.</typeparam>
        /// <typeparam name="TResult">The result value type.</typeparam>
        /// <param name="inputObj">The input value.</param>
        /// <param name="ifNotNull">The <seealso cref="Func{T, TResult}"/> that produces the result value if <paramref name="inputObj"/> is not null.</param>
        /// <param name="result">The result value from <paramref name="ifNotNull"/> or the default value of <typeparamref name="TResult"/> if <paramref name="inputObj"/> was null.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> was null; otherwise, <c>false</c>.</returns>
        public static bool TryCoerceTo<TInput, TResult>(this TInput inputObj, Func<TInput, TResult> ifNotNull, out TResult result) where TInput : class => TryCoerceTo(inputObj, ifNotNull, null, out result);

        /// <summary>
        /// Attempts to coerce a value to another type.
        /// </summary>
        /// <typeparam name="TInput">The input value type.</typeparam>
        /// <typeparam name="TResult">The result value type.</typeparam>
        /// <param name="inputObj">The input value.</param>
        /// <param name="ifNotNull">The <seealso cref="Func{T, TResult}"/> that produces the result value if <paramref name="inputObj"/> is not null.</param>
        /// <param name="ifNull">The <seealso cref="Func{TResult}"/> that produces the result value if <paramref name="inputObj"/> is null.
        /// If this parameter is null, then the result value will be the default value of <typeparamref name="TResult"/> if <paramref name="inputObj"/> is null.</param>
        /// <param name="result">The result value.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> was null; otherwise, <c>false</c>.</returns>
        public static bool TryCoerceTo<TInput, TResult>(this TInput? inputObj, Func<TInput, TResult> ifNotNull, Func<TResult> ifNull, out TResult result)
            where TInput : struct
        {
            if (ifNotNull is null)
                throw new ArgumentNullException(nameof(ifNotNull));
            if (inputObj.HasValue)
            {
                result = ifNotNull(inputObj.Value);
                return true;
            }
            result = (ifNull is null) ? default(TResult) : ifNull();
            return false;
        }

        /// <summary>
        /// Attempts to coerce a value to another type.
        /// </summary>
        /// <typeparam name="TInput">The input value type.</typeparam>
        /// <typeparam name="TResult">The result value type.</typeparam>
        /// <param name="inputObj">The input value.</param>
        /// <param name="ifNotNull">The <seealso cref="Func{T, TResult}"/> that produces the result value if <paramref name="inputObj"/> is not null.</param>
        /// <param name="result">The result value from <paramref name="ifNotNull"/> or the default value of <typeparamref name="TResult"/> if <paramref name="inputObj"/> was null.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> was null; otherwise, <c>false</c>.</returns>
        public static bool TryCoerceTo<TInput, TResult>(this TInput? inputObj, Func<TInput, TResult> ifNotNull, out TResult result) where TInput : struct => TryCoerceTo(inputObj, ifNotNull, null, out result);

        /// <summary>
        /// Attempts to coerce an object as a specific type.
        /// </summary>
        /// <typeparam name="TResult">The result value type.</typeparam>
        /// <param name="inputObj">The input value.</param>
        /// <param name="ifNull">The <seealso cref="Func{TResult}"/> that produces the <paramref name="result"/> value if <paramref name="inputObj"/> is null.
        /// If this parameter is null, then the <paramref name="result"/> value will be the default value of <typeparamref name="TResult"/> if <paramref name="inputObj"/> is null.</param>
        /// <param name="returnValueIfNull">The return value of this method when <paramref name="inputObj"/> is null.</param>
        /// <param name="fallback">The fallback coersion handler that will be invoked if <paramref name="inputObj"/> is not null and cannot be converted.</param>
        /// <param name="ifSuccess">The callback that will be invoked if <paramref name="inputObj"/> was successfully converted converted to type <typeparamref name="TResult"/>.</param>
        /// <param name="result">The coerced <typeparamref name="TResult"/> value.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> could be coerced; otherwise, <c>false</c>.</returns>
        public static bool TryCoerceAs<TResult>(this object inputObj, Func<TResult> ifNull, bool returnValueIfNull, TryCoerceHandler<object, TResult> fallback, Action<TResult> ifSuccess, out TResult result)
        {
            if (inputObj is null)
            {
                result = (ifNull is null) ? default : ifNull();
                if (returnValueIfNull)
                    ifSuccess?.Invoke(result);
                return returnValueIfNull;
            }
            if (inputObj is TResult r1)
                result = r1;
            else
            {
                object o = inputObj;
                if (o is PSObject psObject && (o = psObject.BaseObject) is TResult r2)
                    result = r2;
                else
                {
                    try { result = (TResult)o; }
                    catch
                    {
                        try { result = (TResult)Convert.ChangeType(o, typeof(TResult)); }
                        catch
                        {
                            if (fallback is null || !fallback(o, out result))
                            {
                                result = default;
                                return false;
                            }
                        }
                    }
                }
            }
            ifSuccess?.Invoke(result);
            return true;
        }

        /// <summary>
        /// Attempts to coerce an object as a specific type.
        /// </summary>
        /// <typeparam name="TResult">The result value type.</typeparam>
        /// <param name="inputObj">The input value.</param>
        /// <param name="ifNull">The <seealso cref="Func{TResult}"/> that produces the <paramref name="result"/> value if <paramref name="inputObj"/> is null.
        /// If this parameter is null, then the <paramref name="result"/> value will be the default value of <typeparamref name="TResult"/> if <paramref name="inputObj"/> is null.</param>
        /// <param name="returnValueIfNull">The return value of this method when <paramref name="inputObj"/> is null.</param>
        /// <param name="ifSuccess">The callback that will be invoked if <paramref name="inputObj"/> was successfully converted converted to type <typeparamref name="TResult"/>.</param>
        /// <param name="result">The coerced <typeparamref name="TResult"/> value.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> could be coerced; otherwise, <c>false</c>.</returns>
        public static bool TryCoerceAs<TResult>(this object inputObj, Func<TResult> ifNull, bool returnValueIfNull, Action<TResult> ifSuccess, out TResult result) =>
            TryCoerceAs(inputObj, ifNull, returnValueIfNull, null, ifSuccess, out result);

        /// <summary>
        /// Attempts to coerce an object as a specific type.
        /// </summary>
        /// <typeparam name="TResult">The result value type.</typeparam>
        /// <param name="inputObj">The input value.</param>
        /// <param name="ifNull">The <seealso cref="Func{TResult}"/> that produces the <paramref name="result"/> value if <paramref name="inputObj"/> is null.
        /// If this parameter is null, then the <paramref name="result"/> value will be the default value of <typeparamref name="TResult"/> if <paramref name="inputObj"/> is null.</param>
        /// <param name="fallback">The fallback coersion handler that will be invoked if <paramref name="inputObj"/> is not null and cannot be converted.</param>
        /// <param name="ifSuccess">The callback that will be invoked if <paramref name="inputObj"/> was successfully converted converted to type <typeparamref name="TResult"/>.</param>
        /// <param name="result">The coerced <typeparamref name="TResult"/> value.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> could be coerced; otherwise, <c>false</c>.</returns>
        public static bool TryCoerceAs<TResult>(this object inputObj, Func<TResult> ifNull, TryCoerceHandler<object, TResult> fallback, Action<TResult> ifSuccess, out TResult result) =>
            TryCoerceAs(inputObj, ifNull, false, fallback, ifSuccess, out result);

        /// <summary>
        /// Attempts to coerce an object as a specific type.
        /// </summary>
        /// <typeparam name="TResult">The result value type.</typeparam>
        /// <param name="inputObj">The input value.</param>
        /// <param name="ifNull">The <seealso cref="Func{TResult}"/> that produces the <paramref name="result"/> value if <paramref name="inputObj"/> is null.
        /// If this parameter is null, then the <paramref name="result"/> value will be the default value of <typeparamref name="TResult"/> if <paramref name="inputObj"/> is null.</param>
        /// <param name="ifSuccess">The callback that will be invoked if <paramref name="inputObj"/> was successfully converted converted to type <typeparamref name="TResult"/>.</param>
        /// <param name="result">The coerced <typeparamref name="TResult"/> value.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> could be coerced; otherwise, <c>false</c>.</returns>
        public static bool TryCoerceAs<TResult>(this object inputObj, Func<TResult> ifNull, Action<TResult> ifSuccess, out TResult result) =>
            TryCoerceAs(inputObj, ifNull, null, ifSuccess, out result);

        /// <summary>
        /// Attempts to coerce an object as a specific type.
        /// </summary>
        /// <typeparam name="TResult">The result value type.</typeparam>
        /// <param name="inputObj">The input value.</param>
        /// <param name="returnValueIfNull">The return value of this method when <paramref name="inputObj"/> is null.</param>
        /// <param name="fallback">The fallback coersion handler that will be invoked if <paramref name="inputObj"/> is not null and cannot be converted.</param>
        /// <param name="ifSuccess">The callback that will be invoked if <paramref name="inputObj"/> was successfully converted converted to type <typeparamref name="TResult"/>.</param>
        /// <param name="result">The coerced <typeparamref name="TResult"/> value.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> could be coerced; otherwise, <c>false</c>.</returns>
        public static bool TryCoerceAs<TResult>(this object inputObj, bool returnValueIfNull, TryCoerceHandler<object, TResult> fallback, Action<TResult> ifSuccess, out TResult result) =>
            TryCoerceAs(inputObj, null, returnValueIfNull, fallback, ifSuccess, out result);

        /// <summary>
        /// Attempts to coerce an object as a specific type.
        /// </summary>
        /// <typeparam name="TResult">The result value type.</typeparam>
        /// <param name="inputObj">The input value.</param>
        /// <param name="returnValueIfNull">The return value of this method when <paramref name="inputObj"/> is null.</param>
        /// <param name="ifSuccess">The callback that will be invoked if <paramref name="inputObj"/> was successfully converted converted to type <typeparamref name="TResult"/>.</param>
        /// <param name="result">The coerced <typeparamref name="TResult"/> value.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> could be coerced; otherwise, <c>false</c>.</returns>
        public static bool TryCoerceAs<TResult>(this object inputObj, bool returnValueIfNull, Action<TResult> ifSuccess, out TResult result) =>
            TryCoerceAs(inputObj, null, returnValueIfNull, ifSuccess, out result);

        /// <summary>
        /// Attempts to coerce an object as a specific type.
        /// </summary>
        /// <typeparam name="TResult">The result value type.</typeparam>
        /// <param name="inputObj">The input value.</param>
        /// <param name="fallback">The fallback coersion handler that will be invoked if <paramref name="inputObj"/> is not null and cannot be converted.</param>
        /// <param name="ifSuccess">The callback that will be invoked if <paramref name="inputObj"/> was successfully converted converted to type <typeparamref name="TResult"/>.</param>
        /// <param name="result">The coerced <typeparamref name="TResult"/> value.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> could be coerced; otherwise, <c>false</c>.</returns>
        public static bool TryCoerceAs<TResult>(this object inputObj, TryCoerceHandler<object, TResult> fallback, Action<TResult> ifSuccess, out TResult result) =>
            TryCoerceAs(inputObj, null, fallback, ifSuccess, out result);

        /// <summary>
        /// Attempts to coerce an object as a specific type.
        /// </summary>
        /// <typeparam name="TResult">The result value type.</typeparam>
        /// <param name="inputObj">The input value.</param>
        /// <param name="ifSuccess">The callback that will be invoked if <paramref name="inputObj"/> was successfully converted converted to type <typeparamref name="TResult"/>.</param>
        /// <param name="result">The coerced <typeparamref name="TResult"/> value.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> could be coerced; otherwise, <c>false</c>.</returns>
        public static bool TryCoerceAs<TResult>(this object inputObj, Action<TResult> ifSuccess, out TResult result) => TryCoerceAs(inputObj, (Func<TResult>)null, ifSuccess, out result);

        /// <summary>
        /// Attempts to coerce an object as a specific type.
        /// </summary>
        /// <typeparam name="TResult">The result value type.</typeparam>
        /// <param name="inputObj">The input value.</param>
        /// <param name="ifNull">The <seealso cref="Func{TResult}"/> that produces the <paramref name="result"/> value if <paramref name="inputObj"/> is null.
        /// If this parameter is null, then the <paramref name="result"/> value will be the default value of <typeparamref name="TResult"/> if <paramref name="inputObj"/> is null.</param>
        /// <param name="returnValueIfNull">The return value of this method when <paramref name="inputObj"/> is null.</param>
        /// <param name="fallback">The fallback coersion handler that will be invoked if <paramref name="inputObj"/> is not null and cannot be converted.</param>
        /// <param name="result">The coerced <typeparamref name="TResult"/> value.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> could be coerced; otherwise, <c>false</c>.</returns>
        public static bool TryCoerceAs<TResult>(this object inputObj, Func<TResult> ifNull, bool returnValueIfNull, TryCoerceHandler<object, TResult> fallback, out TResult result) =>
            TryCoerceAs(inputObj, ifNull, returnValueIfNull, fallback, null, out result);

        /// <summary>
        /// Attempts to coerce an object as a specific type.
        /// </summary>
        /// <typeparam name="TResult">The result value type.</typeparam>
        /// <param name="inputObj">The input value.</param>
        /// <param name="ifNull">The <seealso cref="Func{TResult}"/> that produces the <paramref name="result"/> value if <paramref name="inputObj"/> is null.
        /// If this parameter is null, then the <paramref name="result"/> value will be the default value of <typeparamref name="TResult"/> if <paramref name="inputObj"/> is null.</param>
        /// <param name="returnValueIfNull">The return value of this method when <paramref name="inputObj"/> is null.</param>
        /// <param name="result">The coerced <typeparamref name="TResult"/> value.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> could be coerced; otherwise, <c>false</c>.</returns>
        public static bool TryCoerceAs<TResult>(this object inputObj, Func<TResult> ifNull, bool returnValueIfNull, out TResult result) =>
            TryCoerceAs(inputObj, ifNull, returnValueIfNull, null, out result);

        /// <summary>
        /// Attempts to coerce an object as a specific type.
        /// </summary>
        /// <typeparam name="TResult">The result value type.</typeparam>
        /// <param name="inputObj">The input value.</param>
        /// <param name="ifNull">The <seealso cref="Func{TResult}"/> that produces the <paramref name="result"/> value if <paramref name="inputObj"/> is null.
        /// If this parameter is null, then the <paramref name="result"/> value will be the default value of <typeparamref name="TResult"/> if <paramref name="inputObj"/> is null.</param>
        /// <param name="fallback">The fallback conversion handler that will be invoked if <paramref name="inputObj"/> is not null and cannot be converted to type <typeparamref name="TResult"/>.</param>
        /// <param name="result">The coerced <typeparamref name="TResult"/> value.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> could be coerced; otherwise, <c>false</c>.</returns>
        public static bool TryCoerceAs<TResult>(this object inputObj, Func<TResult> ifNull, TryCoerceHandler<object, TResult> fallback, out TResult result) =>
            TryCoerceAs(inputObj, ifNull, false, fallback, out result);

        /// <summary>
        /// Attempts to coerce an object as a specific type.
        /// </summary>
        /// <typeparam name="TResult">The result value type.</typeparam>
        /// <param name="inputObj">The input value.</param>
        /// <param name="ifNull">The <seealso cref="Func{TResult}"/> that produces the <paramref name="result"/> value if <paramref name="inputObj"/> is null.
        /// If this parameter is null, then the <paramref name="result"/> value will be the default value of <typeparamref name="TResult"/> if <paramref name="inputObj"/> is null.</param>
        /// <param name="result">The coerced <typeparamref name="TResult"/> value.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> could be coerced; otherwise, <c>false</c>.</returns>
        public static bool TryCoerceAs<TResult>(this object inputObj, Func<TResult> ifNull, out TResult result) => TryCoerceAs(inputObj, ifNull, null, out result);

        /// <summary>
        /// Attempts to coerce an object as a specific type.
        /// </summary>
        /// <typeparam name="TResult">The result value type.</typeparam>
        /// <param name="inputObj">The input value.</param>
        /// <param name="returnValueIfNull">The return value of this method when <paramref name="inputObj"/> is null.</param>
        /// <param name="fallback">The fallback conversion handler that will be invoked if <paramref name="inputObj"/> is not null and cannot be converted to type <typeparamref name="TResult"/>.</param>
        /// <param name="result">The coerced <typeparamref name="TResult"/> value.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> could be coerced; otherwise, <c>false</c>.</returns>
        public static bool TryCoerceAs<TResult>(this object inputObj, bool returnValueIfNull, TryCoerceHandler<object, TResult> fallback, out TResult result) =>
            TryCoerceAs(inputObj, null, returnValueIfNull, fallback, out result);

        /// <summary>
        /// Attempts to coerce an object as a specific type.
        /// </summary>
        /// <typeparam name="TResult">The result value type.</typeparam>
        /// <param name="inputObj">The input value.</param>
        /// <param name="returnValueIfNull">The return value of this method when <paramref name="inputObj"/> is null.</param>
        /// <param name="result">The coerced <typeparamref name="TResult"/> value.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> could be coerced; otherwise, <c>false</c>.</returns>
        public static bool TryCoerceAs<TResult>(this object inputObj, bool returnValueIfNull, out TResult result) => TryCoerceAs(inputObj, null, returnValueIfNull, out result);

        /// <summary>
        /// Attempts to coerce an object as a specific type.
        /// </summary>
        /// <typeparam name="TResult">The result value type.</typeparam>
        /// <param name="inputObj">The input value.</param>
        /// <param name="fallback">The fallback conversion handler that will be invoked if <paramref name="inputObj"/> is not null and cannot be converted to type <typeparamref name="TResult"/>.</param>
        /// <param name="result">The coerced <typeparamref name="TResult"/> value.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> could be coerced; otherwise, <c>false</c>.</returns>
        public static bool TryCoerceAs<TResult>(this object inputObj, TryCoerceHandler<object, TResult> fallback, out TResult result) =>
            TryCoerceAs(inputObj, null, fallback, out result);

        /// <summary>
        /// Attempts to coerce an object as a specific type.
        /// </summary>
        /// <typeparam name="TResult">The result value type.</typeparam>
        /// <param name="inputObj">The input value.</param>
        /// <param name="result">The coerced <typeparamref name="TResult"/> value.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> could be coerced; otherwise, <c>false</c>.</returns>
        public static bool TryCoerceAs<TResult>(this object inputObj, out TResult result) => TryCoerceAs(inputObj, (Func<TResult>)null, out result);

        /// <summary>
        /// Attempts to cast an object to a specific type.
        /// </summary>
        /// <typeparam name="TResult">The type to be cast as.</typeparam>
        /// <param name="inputObj">The source object.</param>
        /// <param name="ifSuccess">The <seealso cref="Action{T}"/> to invoke if <paramref name="inputObj"/> was null or could be cast as type <typeparamref name="TResult"/>.</param>
        /// <param name="nullReturnValue">The return value of the method if <paramref name="inputObj"/> is null.</param>
        /// <param name="ifNull">Gets the <paramref name="result"/>value if <paramref name="inputObj"/> is null.</param>
        /// <param name="result">The value of <paramref name="inputObj"/> cast as type <typeparamref name="TResult"/>.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> could be cast as type <typeparamref name="TResult"/>; otherwise, <c>false</c>.</returns>
        public static bool TryCast<TResult>(this object inputObj, Action<TResult> ifSuccess, bool nullReturnValue, Func<TResult> ifNull, out TResult result)
        {
            if (inputObj is null)
            {
                result = (ifNull is null) ? default : ifNull();
                if (!nullReturnValue)
                    return nullReturnValue;
            }
            else if (inputObj is TResult r1)
                result = r1;
            else if (inputObj is PSObject psObject && psObject.BaseObject is TResult r2)
                result = r2;
            else
            {
                result = default;
                return false;
            }
            ifSuccess?.Invoke(result);
            return true;
        }

        /// <summary>
        /// Attempts to cast an object to a specific type.
        /// </summary>
        /// <typeparam name="T">The type to be cast as.</typeparam>
        /// <param name="inputObj">The source object.</param>
        /// <param name="ifSuccess">The <seealso cref="Action{T}"/> to invoke if <paramref name="inputObj"/> was null or could be cast as type <typeparamref name="T"/>.</param>
        /// <param name="nullReturnValue">The return value of the method if <paramref name="inputObj"/> is null.</param>
        /// <param name="result">The value of <paramref name="inputObj"/> cast or coerced as type <typeparamref name="T"/>.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> could be cast or coerced as type <typeparamref name="T"/>; otherwise, <c>false</c>.</returns>
        public static bool TryCast<T>(this object inputObj, Action<T> ifSuccess, bool nullReturnValue, out T result)  => TryCast(inputObj, ifSuccess, nullReturnValue, out result);

        /// <summary>
        /// Attempts to cast an object to a specific type.
        /// </summary>
        /// <typeparam name="T">The type to be cast as.</typeparam>
        /// <param name="inputObj">The source object.</param>
        /// <param name="ifSuccess">The <seealso cref="Action{T}"/> to invoke if <paramref name="inputObj"/> was null or could be cast as type <typeparamref name="T"/>.</param>
        /// <param name="ifNull">Gets the <paramref name="result"/>value if <paramref name="inputObj"/> is null.</param>
        /// <param name="result">The value of <paramref name="inputObj"/> cast or coerced as type <typeparamref name="T"/>.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> could be cast or coerced as type <typeparamref name="T"/>; otherwise, <c>false</c>.</returns>
        public static bool TryCast<T>(this object inputObj, Action<T> ifSuccess, Func<T> ifNull, out T result)  => TryCast(inputObj, ifSuccess, false, ifNull, out result);

        /// <summary>
        /// Attempts to cast an object to a specific type.
        /// </summary>
        /// <typeparam name="T">The type to be cast as.</typeparam>
        /// <param name="inputObj">The source object.</param>
        /// <param name="ifSuccess">The <seealso cref="Action{T}"/> to invoke if <paramref name="inputObj"/> was null or could be cast as type <typeparamref name="T"/>.</param>
        /// <param name="result">The value of <paramref name="inputObj"/> cast or coerced as type <typeparamref name="T"/>.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> could be cast or coerced as type <typeparamref name="T"/>; otherwise, <c>false</c>.</returns>
        public static bool TryCast<T>(this object inputObj, Action<T> ifSuccess, out T result)  => TryCast(inputObj, ifSuccess, null, out result);

        /// <summary>
        /// Attempts to cast an object to a specific type.
        /// </summary>
        /// <typeparam name="T">The type to be cast as.</typeparam>
        /// <param name="inputObj">The source object.</param>
        /// <param name="nullReturnValue">The return value of the method if <paramref name="inputObj"/> is null.</param>
        /// <param name="ifNull">Gets the <paramref name="result"/>value if <paramref name="inputObj"/> is null.</param>
        /// <param name="result">The value of <paramref name="inputObj"/> cast or coerced as type <typeparamref name="T"/>.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> could be cast or coerced as type <typeparamref name="T"/>; otherwise, <c>false</c>.</returns>
        public static bool TryCast<T>(this object inputObj, bool nullReturnValue, Func<T> ifNull, out T result)  => TryCast(inputObj, null, nullReturnValue, ifNull, out result);

        /// <summary>
        /// Attempts to cast an object to a specific type.
        /// </summary>
        /// <typeparam name="T">The type to be cast as.</typeparam>
        /// <param name="inputObj">The source object.</param>
        /// <param name="nullReturnValue">The return value of the method if <paramref name="inputObj"/> is null.</param>
        /// <param name="result">The value of <paramref name="inputObj"/> cast or coerced as type <typeparamref name="T"/>.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> could be cast or coerced as type <typeparamref name="T"/>; otherwise, <c>false</c>.</returns>
        public static bool TryCast<T>(this object inputObj, bool nullReturnValue, out T result)  => TryCast(inputObj, (Action<T>)null, nullReturnValue, out result);

        /// <summary>
        /// Attempts to cast an object to a specific type.
        /// </summary>
        /// <typeparam name="T">The type to be cast as.</typeparam>
        /// <param name="inputObj">The source object.</param>
        /// <param name="ifNull">Gets the <paramref name="result"/>value if <paramref name="inputObj"/> is null.</param>
        /// <param name="result">The value of <paramref name="inputObj"/> cast or coerced as type <typeparamref name="T"/>.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> could be cast or coerced as type <typeparamref name="T"/>; otherwise, <c>false</c>.</returns>
        public static bool TryCast<T>(this object inputObj, Func<T> ifNull, out T result)  => TryCast(inputObj, null, ifNull, out result);

        /// <summary>
        /// Attempts to cast an object to a specific type.
        /// </summary>
        /// <typeparam name="T">The type to be cast as.</typeparam>
        /// <param name="inputObj">The source object.</param>
        /// <param name="result">The value of <paramref name="inputObj"/> cast or coerced as type <typeparamref name="T"/>.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> could be cast or coerced as type <typeparamref name="T"/>; otherwise, <c>false</c>.</returns>
        public static bool TryCast<T>(this object inputObj, out T result)  => TryCast(inputObj, (Action<T>)null, out result);

        /// <summary>
        /// Attempts to get an error message and related infromation from an <seealso cref="IContainsErrorRecord"/> object.
        /// </summary>
        /// <param name="source">The source <seealso cref="IContainsErrorRecord"/> object.</param>
        /// <param name="defaultCategory">The <seealso cref="ErrorCategory"/> to return in the <paramref name="category"/> parameter if the <paramref name="source" /> didn't
        /// contain a category value.</param>
        /// <param name="defaultErrorId">The error id to return in the <paramref name="errorId"/> parameter if the <paramref name="source" /> didn't
        /// contain a error id.</param>
        /// <param name="defaultReason">The reason text to return in the <paramref name="reason"/> parameter if the <paramref name="source"/> didn't contain a reason value.</param>
        /// <param name="defaultTargetObject">The object to return in the <paramref name="targetObject"/> parameter if the <paramref name="source"/> didn't contain a target object value.</param>
        /// <param name="message">The message text coerced from the <paramref name="source"/> object.</param>
        /// <param name="category">The <seealso cref="ErrorCategory"/> coerced from the <paramref name="source"/> object.</param>
        /// <param name="errorId">The error id coerced from the <paramref name="source"/> object.</param>
        /// <param name="reason">The reason text coerced from the <paramref name="source"/> object.</param>
        /// <param name="targetObject">The target object instance coerced from the <paramref name="source"/> object.</param>
        /// <returns><c>true</c> if a message could be coerced from the <paramref name="source"/> object; otherwise, <c>false</c>.</returns>
        public static bool TryGetErrorMessage(this IContainsErrorRecord source, ErrorCategory defaultCategory, string defaultErrorId, string defaultReason, object defaultTargetObject,
            out string message, out ErrorCategory category, out string errorId, out string reason, out object targetObject)
        {
            if (!(source is null))
            {
                if (TryGetErrorCategory(source.ErrorRecord, out category, out message, out errorId, out reason, out targetObject))
                    return true;
                if (source is Exception exc && !((message = exc.Message) is null))
                {
                    category = defaultCategory;
                    errorId = defaultErrorId;
                    targetObject = defaultTargetObject;
                    reason = defaultReason;
                    return true;
                }
            }
            category = default(ErrorCategory);
            targetObject = message = errorId = reason = null;
            return false;
        }

        /// <summary>
        /// Attempts to get an error message and related infromation from an <seealso cref="IContainsErrorRecord"/> object.
        /// </summary>
        /// <param name="source">The source <seealso cref="IContainsErrorRecord"/> object.</param>
        /// <param name="defaultCategory">The <seealso cref="ErrorCategory"/> to return in the <paramref name="category"/> parameter if the <paramref name="source" /> didn't
        /// contain a category value.</param>
        /// <param name="defaultErrorId">The error id to return in the <paramref name="errorId"/> parameter if the <paramref name="source" /> didn't
        /// contain a error id.</param>
        /// <param name="defaultTargetObject">The object to return in the <paramref name="targetObject"/> parameter if the <paramref name="source"/>
        /// didn't contain a target object value.</param>
        /// <param name="message">The message text coerced from the <paramref name="source"/> object.</param>
        /// <param name="category">The <seealso cref="ErrorCategory"/> coerced from the <paramref name="source"/> object.</param>
        /// <param name="errorId">The error id coerced from the <paramref name="source"/> object.</param>
        /// <param name="targetObject">The target object instance coerced from the <paramref name="source"/> object.</param>
        /// <returns><c>true</c> if a message could be coerced from the <paramref name="source"/> object; otherwise, <c>false</c>.</returns>
        public static bool TryGetErrorMessage(this IContainsErrorRecord source, ErrorCategory defaultCategory, string defaultErrorId, object defaultTargetObject, out string message,
            out ErrorCategory category, out string errorId, out object targetObject)
        {
            if (!(source is null))
            {
                if (TryGetErrorCategory(source.ErrorRecord, out category, out message, out errorId, out targetObject))
                    return true;
                if (source is Exception exc && !((message = exc.Message) is null))
                {
                    category = defaultCategory;
                    errorId = defaultErrorId;
                    targetObject = defaultTargetObject;
                    return true;
                }
            }
            category = default(ErrorCategory);
            targetObject = message = errorId = null;
            return false;
        }

        /// <summary>
        /// Attempts to get an error message and related infromation from an <seealso cref="IContainsErrorRecord"/> object.
        /// </summary>
        /// <param name="source">The source <seealso cref="IContainsErrorRecord"/> object.</param>
        /// <param name="message">The message text coerced from the <paramref name="source"/> object.</param>
        /// <returns><c>true</c> if a message could be coerced from the <paramref name="source"/> object; otherwise, <c>false</c>.</returns>
        public static bool TryGetErrorMessage(this IContainsErrorRecord source, out string message)
        {
            if (!(source is null))
            {
                ErrorRecord errorRecord = source.ErrorRecord;
                if (!(errorRecord is null || string.IsNullOrWhiteSpace(message = errorRecord.ToString())) || (source is Exception e && !string.IsNullOrWhiteSpace(message = e.Message)))
                    return true;
            }
            message = null;
            return false;
        }

        /// <summary>
        /// Attempts to get an error message and related infromation from an <seealso cref="Exception"/> object.
        /// </summary>
        /// <param name="exception">The source <seealso cref="Exception"/> object.</param>
        /// <param name="defaultCategory">The <seealso cref="ErrorCategory"/> to return in the <paramref name="category"/> parameter if the <paramref name="source" /> didn't
        /// contain a category value.</param>
        /// <param name="defaultErrorId">The error id to return in the <paramref name="errorId"/> parameter if the <paramref name="source" /> didn't
        /// contain a error id.</param>
        /// <param name="defaultReason">The reason text to return in the <paramref name="reason"/> parameter if the <paramref name="source"/> didn't contain a reason value.</param>
        /// <param name="defaultTargetObject">The object to return in the <paramref name="targetObject"/> parameter if the <paramref name="source"/>
        /// didn't contain a target object value.</param>
        /// <param name="message">The message text coerced from the <paramref name="exception"/>.</param>
        /// <param name="category">The <seealso cref="ErrorCategory"/> coerced from the <paramref name="exception"/>.</param>
        /// <param name="errorId">The error id coerced from the <paramref name="exception"/>.</param>
        /// <param name="reason">The reason text coerced from the <paramref name="exception"/>.</param>
        /// <param name="targetObject">The target object instance coerced from the <paramref name="exception"/>.</param>
        /// <returns><c>true</c> if a message could be coerced from the <paramref name="exception"/> object; otherwise, <c>false</c>.</returns>
        public static bool TryGetErrorMessage(this Exception exception, ErrorCategory defaultCategory, string defaultErrorId, string defaultReason, object defaultTargetObject,
            out string message, out ErrorCategory category, out string errorId, out string reason, out object targetObject)
        {
            if (!(exception is null))
            {
                if (exception is IContainsErrorRecord c && TryGetErrorCategory(c.ErrorRecord, out category, out message, out errorId, out reason, out targetObject))
                    return true;
                if (!string.IsNullOrWhiteSpace(message = exception.Message))
                {
                    category = defaultCategory;
                    errorId = defaultErrorId;
                    targetObject = defaultTargetObject;
                    reason = defaultReason;
                    return true;
                }
            }
            category = default(ErrorCategory);
            targetObject = message = errorId = reason = null;
            return false;
        }

        /// <summary>
        /// Attempts to get an error message and related infromation from an <seealso cref="Exception"/> object.
        /// </summary>
        /// <param name="exception">The source <seealso cref="Exception"/> object.</param>
        /// <param name="defaultCategory">The <seealso cref="ErrorCategory"/> to return in the <paramref name="category"/> parameter if the <paramref name="source" /> didn't
        /// contain a category value.</param>
        /// <param name="defaultErrorId">The error id to return in the <paramref name="errorId"/> parameter if the <paramref name="source" /> didn't
        /// contain a error id.</param>
        /// <param name="defaultTargetObject">The object to return in the <paramref name="targetObject"/> parameter if the <paramref name="source"/>
        /// didn't contain a target object value.</param>
        /// <param name="message">The message text coerced from the <paramref name="exception"/>.</param>
        /// <param name="category">The <seealso cref="ErrorCategory"/> coerced from the <paramref name="exception"/>.</param>
        /// <param name="errorId">The error id coerced from the <paramref name="exception"/>.</param>
        /// <param name="targetObject">The target object instance coerced from the <paramref name="exception"/>.</param>
        /// <returns><c>true</c> if a message could be coerced from the <paramref name="exception"/> object; otherwise, <c>false</c>.</returns>
        public static bool TryGetErrorMessage(this Exception exception, ErrorCategory defaultCategory, string defaultErrorId, object defaultTargetObject, out string message,
            out ErrorCategory category, out string errorId, out object targetObject)
        {
            if (!(exception is null))
            {
                if (exception is IContainsErrorRecord c && TryGetErrorCategory(c.ErrorRecord, out category, out message, out errorId, out targetObject))
                    return true;
                if (!string.IsNullOrWhiteSpace(message = exception.Message))
                {
                    category = defaultCategory;
                    errorId = defaultErrorId;
                    targetObject = defaultTargetObject;
                    return true;
                }
            }
            category = default(ErrorCategory);
            targetObject = message = errorId = null;
            return false;
        }

        /// <summary>
        /// Attempts to get an error message and related infromation from an <seealso cref="Exception"/> object.
        /// </summary>
        /// <param name="exception">The source <seealso cref="Exception"/> object.</param>
        /// <param name="message">The message text coerced from the <paramref name="exception"/> object.</param>
        /// <returns><c>true</c> if a message could be coerced from the <paramref name="exception"/> object; otherwise, <c>false</c>.</returns>
        public static bool TryGetErrorMessage(this Exception exception, out string message)
        {
            if (exception is null)
                message = null;
            else
            {
                if (!string.IsNullOrWhiteSpace(message = exception.Message))
                    return true;
                if (exception is IContainsErrorRecord c)
                {
                    ErrorRecord e = c.ErrorRecord;
                    if (!(e is null || string.IsNullOrWhiteSpace(message = e.ToString())))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Attempts to get an <seealso cref="ErrorCategory"/> and related infromation from an <seealso cref="ErrorRecord"/> object.
        /// </summary>
        /// <param name="errorRecord">The source <seealso cref="ErrorRecord"/> object.</param>
        /// <param name="category">The <seealso cref="ErrorCategory"/> coerced from the <paramref name="errorRecord"/>.</param>
        /// <param name="message">The message text coerced from the <paramref name="errorRecord"/>.</param>
        /// <param name="errorId">The error id coerced from the <paramref name="errorRecord"/>.</param>
        /// <param name="reason">The reason text coerced from the <paramref name="errorRecord"/>.</param>
        /// <param name="targetObject">The target object instance coerced from the <paramref name="errorRecord"/>.</param>
        /// <returns><c>true</c> if an <seealso cref="ErrorCategory"/> could be coerced from the <paramref name="errorRecord"/> object; otherwise, <c>false</c>.</returns>
        public static bool TryGetErrorCategory(this ErrorRecord errorRecord, out ErrorCategory category, out string message, out string errorId, out string reason,
            out object targetObject)
        {
            if (TryGetErrorCategory(errorRecord, out category, out message, out errorId, out targetObject))
            {
                reason = errorRecord.CategoryInfo.Reason;
                return true;
            }
            reason = null;
            return false;
        }

        /// <summary>
        /// Attempts to get an <seealso cref="ErrorCategory"/> and related infromation from an <seealso cref="ErrorRecord"/> object.
        /// </summary>
        /// <param name="errorRecord">The source <seealso cref="ErrorRecord"/> object.</param>
        /// <param name="category">The <seealso cref="ErrorCategory"/> coerced from the <paramref name="errorRecord"/>.</param>
        /// <param name="message">The message text coerced from the <paramref name="errorRecord"/>.</param>
        /// <param name="errorId">The error id coerced from the <paramref name="errorRecord"/>.</param>
        /// <param name="targetObject">The target object instance coerced from the <paramref name="errorRecord"/>.</param>
        /// <returns><c>true</c> if an <seealso cref="ErrorCategory"/> could be coerced from the <paramref name="errorRecord"/> object; otherwise, <c>false</c>.</returns>
        public static bool TryGetErrorCategory(this ErrorRecord errorRecord, out ErrorCategory category, out string message, out string errorId, out object targetObject)
        {
            if (errorRecord is null)
            {
                category = default(ErrorCategory);
                targetObject = message = errorId = null;
                return false;
            }
            category = errorRecord.CategoryInfo.Category;
            message = errorRecord.ToString();
            errorId = errorRecord.FullyQualifiedErrorId;
            targetObject = errorRecord.TargetObject;
            return true;
        }

        /// <summary>
        /// Attempts to get an <seealso cref="ErrorCategory"/> and related infromation from an <seealso cref="ErrorRecord"/> object.
        /// </summary>
        /// <param name="source">The source <seealso cref="IContainsErrorRecord"/> object.</param>
        /// <param name="category">The <seealso cref="ErrorCategory"/> coerced from the <paramref name="source"/> object.</param>
        /// <param name="message">The message text coerced from the <paramref name="source"/> object.</param>
        /// <param name="errorId">The error id coerced from the <paramref name="source"/> object.</param>
        /// <param name="reason">The reason text coerced from the <paramref name="source"/> object.</param>
        /// <param name="targetObject">The target object instance coerced from the <paramref name="source"/>.</param>
        /// <returns><c>true</c> if an <seealso cref="ErrorCategory"/> could be coerced from the <paramref name="source"/> object; otherwise, <c>false</c>.</returns>
        public static bool TryGetErrorCategory(this IContainsErrorRecord source, out ErrorCategory category, out string message, out string errorId, out string reason,
            out object targetObject)
        {
            if (source is null)
            {
                category = default(ErrorCategory);
                targetObject = message = errorId = reason = null;
                return false;
            }
            return TryGetErrorCategory(source.ErrorRecord, out category, out message, out errorId, out reason, out targetObject);
        }

        /// <summary>
        /// Attempts to get an <seealso cref="ErrorCategory"/> and related infromation from an <seealso cref="ErrorRecord"/> object.
        /// </summary>
        /// <param name="source">The source <seealso cref="IContainsErrorRecord"/> object.</param>
        /// <param name="category">The <seealso cref="ErrorCategory"/> coerced from the <paramref name="source"/> object.</param>
        /// <param name="message">The message text coerced from the <paramref name="source"/> object.</param>
        /// <param name="errorId">The error id coerced from the <paramref name="source"/> object.</param>
        /// <param name="targetObject">The target object instance coerced from the <paramref name="source"/>.</param>
        /// <returns><c>true</c> if an <seealso cref="ErrorCategory"/> could be coerced from the <paramref name="source"/> object; otherwise, <c>false</c>.</returns>
        public static bool TryGetErrorCategory(this IContainsErrorRecord source, out ErrorCategory category, out string message, out string errorId, out object targetObject)
        {
            if (source is null)
            {
                category = default(ErrorCategory);
                targetObject = message = errorId = null;
                return false;
            }
            return TryGetErrorCategory(source.ErrorRecord, out category, out message, out errorId, out targetObject);
        }

        /// <summary>
        /// Attempts to get an <seealso cref="ErrorCategory"/> and related infromation from an <seealso cref="ErrorRecord"/> object.
        /// </summary>
        /// <param name="source">The source <seealso cref="Exception"/> object.</param>
        /// <param name="category">The <seealso cref="ErrorCategory"/> coerced from the <paramref name="exception"/>.</param>
        /// <param name="message">The message text coerced from the <paramref name="exception"/>.</param>
        /// <param name="errorId">The error id coerced from the <paramref name="exception"/>.</param>
        /// <param name="reason">The reason text coerced from the <paramref name="exception"/>.</param>
        /// <param name="targetObject">The target object instance coerced from the <paramref name="exception"/>.</param>
        /// <returns><c>true</c> if an <seealso cref="ErrorCategory"/> could be coerced from the <paramref name="source"/> object; otherwise, <c>false</c>.</returns>
        public static bool TryGetErrorCategory(this Exception exception, out ErrorCategory category, out string message, out string errorId, out string reason, out object targetObject)
        {
            if (exception is IContainsErrorRecord c)
                return TryGetErrorCategory(c.ErrorRecord, out category, out message, out errorId, out reason, out targetObject);
            category = default(ErrorCategory);
            targetObject = message = errorId = reason = null;
            return false;
        }

        /// <summary>
        /// Attempts to get an <seealso cref="ErrorCategory"/> and related infromation from an <seealso cref="Exception"/> object.
        /// </summary>
        /// <param name="source">The source <seealso cref="Exception"/> object.</param>
        /// <param name="category">The <seealso cref="ErrorCategory"/> coerced from the <paramref name="exception"/>.</param>
        /// <param name="message">The message text coerced from the <paramref name="exception"/>.</param>
        /// <param name="errorId">The error id coerced from the <paramref name="exception"/>.</param>
        /// <param name="targetObject">The target object instance coerced from the <paramref name="exception"/>.</param>
        /// <returns><c>true</c> if an <seealso cref="ErrorCategory"/> could be coerced from the <paramref name="source"/> object; otherwise, <c>false</c>.</returns>
        public static bool TryGetErrorCategory(this Exception exception, out ErrorCategory category, out string message, out string errorId, out object targetObject)
        {
            if (exception is IContainsErrorRecord c)
                return TryGetErrorCategory(c.ErrorRecord, out category, out message, out errorId, out targetObject);
            category = default(ErrorCategory);
            targetObject = message = errorId = null;
            return false;
        }

        /// <summary>
        /// Attempts to get the target object and related infromation from an <seealso cref="ErrorRecord"/> object.
        /// </summary>
        /// <param name="errorRecord">The source <seealso cref="ErrorRecord"/> object.</param>
        /// <param name="targetObject">The target object instance coerced from the <paramref name="errorRecord"/>.</param>
        /// <param name="errorId">The error id coerced from the <paramref name="errorRecord"/>.</param>
        /// <param name="reason">The reason text coerced from the <paramref name="errorRecord"/>.</param>
        /// <returns><c>true</c> if the target object could be coerced from the <paramref name="errorRecord"/> object; otherwise, <c>false</c>.</returns>
        public static bool TryGetTargetObject(this ErrorRecord errorRecord, out object targetObject, out string errorId, out string reason)
        {
            if (errorRecord is null)
            {
                targetObject = errorId = reason = null;
                return false;
            }
            errorId = errorRecord.FullyQualifiedErrorId;
            targetObject = errorRecord.TargetObject;
            reason = errorRecord.CategoryInfo.Reason;
            return true;
        }

        /// <summary>
        /// Attempts to get the target object and related infromation from an <seealso cref="ErrorRecord"/> object.
        /// </summary>
        /// <param name="source">The source <seealso cref="IContainsErrorRecord"/> object.</param>
        /// <param name="targetObject">The target object instance coerced from the <paramref name="source"/> object.</param>
        /// <param name="errorId">The error id coerced from the <paramref name="source"/> object.</param>
        /// <param name="reason">The reason text coerced from the <paramref name="source"/> object.</param>
        /// <returns><c>true</c> if the target object could be coerced from the <paramref name="source"/> object; otherwise, <c>false</c>.</returns>
        public static bool TryGetTargetObject(this IContainsErrorRecord source, out object targetObject, out string errorId, out string reason)
        {
            if (source is null)
            {
                targetObject = errorId = reason = null;
                return false;
            }
            return TryGetTargetObject(source.ErrorRecord, out targetObject, out errorId, out reason);
        }

        /// <summary>
        /// Attempts to get the target object and related infromation from an <seealso cref="Exception"/> object.
        /// </summary>
        /// <param name="source">The source <seealso cref="Exception"/> object.</param>
        /// <param name="targetObject">The target object instance coerced from the <paramref name="exception"/>.</param>
        /// <param name="errorId">The error id coerced from the <paramref name="exception"/>.</param>
        /// <param name="reason">The reason text coerced from the <paramref name="exception"/>.</param>
        /// <returns><c>true</c> if the target object could be coerced from the <paramref name="exception"/>; otherwise, <c>false</c>.</returns>
        public static bool TryGetTargetObject(this Exception exception, out object targetObject, out string errorId, out string reason)
        {
            if (exception is IContainsErrorRecord c)
                return TryGetTargetObject(c.ErrorRecord, out targetObject, out errorId, out reason);
            targetObject = errorId = reason = null;
            return false;
        }

        /// <summary>
        /// Attempts to get a <seealso cref="ErrorRecord"/> from an input value.
        /// </summary>
        /// <param name="inputObj">The input value.</param>
        /// <param name="errorRecord">The <seealso cref="ErrorRecord"/> object cast or obtained from <paramref name="inputObj"/>.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> was an <seealso cref="ErrorRecord"/> object or an object which contained an <seealso cref="ErrorRecord"/>;
        /// otherwise, <c>false</c>.</returns>
        public static bool TryGetErrorRecord(this object inputObj, out ErrorRecord errorRecord) => TryCoerceAs(inputObj, (object o, out ErrorRecord e) =>
        {
            if (o is IContainsErrorRecord c)
                return !((e = c.ErrorRecord) is null);
            e = null;
            return false;
        }, out errorRecord);

        /// <summary>
        /// Attempts to get a <seealso cref="Exception"/> from an input value.
        /// </summary>
        /// <param name="inputObj">The input value.</param>
        /// <param name="exception">The <seealso cref="Exception"/> object cast or obtained from <paramref name="inputObj"/>.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> was an <seealso cref="Exception"/> object or an object which contained an <seealso cref="ErrorRecord"/>;
        /// otherwise, <c>false</c>.</returns>
        public static bool TryGetException(this object inputObj, out Exception exception) => TryCoerceAs(inputObj, (object o, out Exception e) =>
        {
            if (o is ErrorRecord r || (o is IContainsErrorRecord c && !((r = c.ErrorRecord) is null)))
                return !((e = r.Exception) is null);
            e = null;
            return false;
        }, out exception);

        /// <summary>
        /// Gets or creates an <seealso cref="ErrorRecord"/> associated with an <seealso cref="Exception"/>.
        /// </summary>
        /// <param name="exception">The target <seealso cref="Exception"/>.</param>
        /// <returns>The <seealso cref="ErrorRecord"/> created or obtained from the target <seealso cref="Exception"/>.</returns>
        public static ErrorRecord ToErrorRecord(this Exception exception)
        {
            if (exception is null)
                return null;
            if (exception is IContainsErrorRecord c)
            {
                ErrorRecord errorRecord = c.ErrorRecord;
                if (!(errorRecord is null))
                    return errorRecord;
            }
            if (!TryGetTargetObject(exception, out object targetObject, out string errorId, out string reason))
                reason = MessageId.UnexpectedError.GetDescription(out errorId);
            if (exception is ArgumentException argumentException)
                return new ErrorRecord(exception, errorId, ErrorCategory.InvalidArgument, targetObject ?? argumentException.ParamName).SetReason(reason);
            if (exception is FileLoadException fileLoadException)
                return new ErrorRecord(exception, errorId, ErrorCategory.OpenError, targetObject ?? fileLoadException.FileName).SetReason(reason);
            if (exception is FileNotFoundException fileNotFoundException)
                return new ErrorRecord(exception, errorId, ErrorCategory.ObjectNotFound, targetObject ?? fileNotFoundException.FileName).SetReason(reason);

            ErrorCategory category;
            if (exception is InvalidOperationException)
                category = ErrorCategory.InvalidOperation;
            else if (exception is NotImplementedException)
                category = ErrorCategory.NotImplemented;
            else if (exception is NotSupportedException)
                category = ErrorCategory.NotInstalled;
            else if (exception is EndOfStreamException || exception is PathTooLongException)
                category = ErrorCategory.LimitsExceeded;
            else if (exception is InvalidDataException || exception is FormatException)
                category = ErrorCategory.InvalidData;
            else if (exception is DirectoryNotFoundException || exception is System.IO.DriveNotFoundException || exception is ItemNotFoundException)
                category = ErrorCategory.ObjectNotFound;
            else if (exception is System.AccessViolationException)
                category = ErrorCategory.PermissionDenied;
            else if (exception is System.FormatException)
                category = ErrorCategory.SyntaxError;
            else if (exception is System.IndexOutOfRangeException)
                category = ErrorCategory.InvalidArgument;
            else if (exception is System.TimeoutException)
                category = ErrorCategory.OperationTimeout;
            else if (exception is OperationCanceledException)
                category = ErrorCategory.OperationStopped;
            else if (exception is ParseException)
                category = ErrorCategory.ParserError;
            else
                category = ErrorCategory.NotSpecified;
            return new ErrorRecord(exception, errorId, category, targetObject).SetReason(reason);
        }

        /// <summary>
        /// Sets the reason message for an <seealso cref="ErrorRecord"/>.
        /// </summary>
        /// <param name="errorRecord">The target <seealso cref="ErrorRecord"/>.</param>
        /// <param name="reason">The text for the category reason.</param>
        /// <returns>The target <seealso cref="ErrorRecord"/> (for chaining).</returns>
        public static ErrorRecord SetReason(this ErrorRecord errorRecord, string reason)
        {
            errorRecord.CategoryInfo.Reason = reason;
            return errorRecord;
        }
    }

    /// <summary>
    /// Handler callback to attempt to coerce one value from another.
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <typeparam name="R">The result type.</typeparam>
    /// <param name="inputObj">The input value.</param>
    /// <param name="result">The input value of type <typeparamref name="T"/> cast or coerced as type <typeparamref name="R"/>.</param>
    /// <returns><c>true</c> if the input type <typeparamref name="T"/> was succesfully coerced as type <typeparamref name="R"/>; otherwise, <c>false</c>.</returns>
    public delegate bool TryCoerceHandler<T, R>(T inputObj, out R result);
}
