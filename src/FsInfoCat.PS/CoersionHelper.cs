using FsInfoCat.Models.Crawl;
using FsInfoCat.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;

namespace FsInfoCat.PS
{
    public static class CoersionHelper
    {
        #region PSObject conversion

        public static bool TryFindFirstBaseObject<T>(this IEnumerable<PSObject> collection, Predicate<T> predicate, out PSObject psObject, out T result)
        {
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            if (collection is null)
            {
                psObject = null;
                result = default;
                return false;
            }

            foreach (PSObject p in collection)
            {
                if (!(p is null) && p.BaseObject is T t && predicate(t))
                {
                    result = t;
                    psObject = p;
                    return true;
                }
            }
            psObject = null;
            result = default;
            return false;
        }

        public static IEnumerable<PSObject> WhereBaseObjectOf<T>(this IEnumerable<PSObject> collection, Predicate<T> predicate = null)
        {
            if (collection is null)
                return new PSObject[0];
            if (predicate is null)
                return collection.Where(o => !(o is null) && ((o is PSObject p) ? p.BaseObject : o) is T);
            return collection.Where(o => !(o is null) && ((o is PSObject p) ? p.BaseObject : o) is T t && predicate(t));
        }

        public static IEnumerable<object> WhereBaseObjectOf<T>(this IEnumerable<object> collection, Predicate<T> predicate = null)
        {
            if (collection is null)
                return new object[0];
            if (predicate is null)
                return collection.Where(o => !(o is null) && ((o is PSObject p) ? p.BaseObject : o) is T);
            return collection.Where(o => !(o is null) && ((o is PSObject p) ? p.BaseObject : o) is T t && predicate(t));
        }

        public static IEnumerable<T> BaseObjectOfType<T>(this IEnumerable<PSObject> collection)
        {
            if (collection is null)
                return new T[0];
            return collection.Select(o => o?.BaseObject).OfType<T>();
        }

        public static IEnumerable<T> BaseObjectOfType<T>(this IEnumerable<object> collection)
        {
            if (collection is null)
                return new T[0];
            return collection.Select(o => (o is PSObject p) ? p.BaseObject : o).OfType<T>();
        }

        #endregion

        #region InvokeIfNotNull

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

        #endregion

        #region TryCoerceTo

        public static bool TryCoerceToFileUri(object obj, bool assumeLocalPath, out FileUri result)
        {
            object o = (obj is PSObject psObject) ? psObject.BaseObject : obj;
            if (o is FileUri fileUri)
                result = fileUri;
            else if (o is FileSystemInfo fsi)
                result = new FileUri(fsi);
            else if (o is Uri uri)
            {
                if (uri.IsAbsoluteUri && uri.Scheme == System.Uri.UriSchemeFile && string.IsNullOrEmpty(uri.Query) && string.IsNullOrEmpty(uri.Fragment))
                    result = new FileUri(uri.AbsoluteUri);
                else
                {
                    result = null;
                    return false;
                }
            }
            else if (TryCoerceAs(obj, out string uriString))
            {
                if (string.IsNullOrEmpty(uriString))
                    result = new FileUri("");
                else
                {
                    try
                    {
                        if (assumeLocalPath)
                        {
                            if (File.Exists(uriString))
                                result = new FileUri(new FileInfo(uriString));
                            else if (Directory.Exists(uriString) || string.IsNullOrEmpty(Path.GetExtension(uriString)))
                                result = new FileUri(new DirectoryInfo(uriString));
                            else
                                result = new FileUri(new FileInfo(uriString));
                        }
                        else
                            result = new FileUri(uriString);
                    }
                    catch
                    {
                        result = null;
                        return false;
                    }
                }
            }
            else
            {
                result = null;
                return false;
            }
            return true;
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
                result = (ifNull is null) ? default : ifNull();
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
            result = (ifNull is null) ? default : ifNull();
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

        #endregion

        #region TryCoerceAs

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
        public static bool TryCoerceAs<TResult>(object inputObj, Func<TResult> ifNull, bool returnValueIfNull, TryCoerceHandler<object, TResult> fallback, Action<TResult> ifSuccess, out TResult result)
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
        public static bool TryCoerceAs<TResult>(object inputObj, Func<TResult> ifNull, bool returnValueIfNull, Action<TResult> ifSuccess, out TResult result) =>
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
        public static bool TryCoerceAs<TResult>(object inputObj, Func<TResult> ifNull, TryCoerceHandler<object, TResult> fallback, Action<TResult> ifSuccess, out TResult result) =>
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
        public static bool TryCoerceAs<TResult>(object inputObj, Func<TResult> ifNull, Action<TResult> ifSuccess, out TResult result) =>
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
        public static bool TryCoerceAs<TResult>(object inputObj, bool returnValueIfNull, TryCoerceHandler<object, TResult> fallback, Action<TResult> ifSuccess, out TResult result) =>
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
        public static bool TryCoerceAs<TResult>(object inputObj, bool returnValueIfNull, Action<TResult> ifSuccess, out TResult result) =>
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
        public static bool TryCoerceAs<TResult>(object inputObj, TryCoerceHandler<object, TResult> fallback, Action<TResult> ifSuccess, out TResult result) =>
            TryCoerceAs(inputObj, null, fallback, ifSuccess, out result);

        /// <summary>
        /// Attempts to coerce an object as a specific type.
        /// </summary>
        /// <typeparam name="TResult">The result value type.</typeparam>
        /// <param name="inputObj">The input value.</param>
        /// <param name="ifSuccess">The callback that will be invoked if <paramref name="inputObj"/> was successfully converted converted to type <typeparamref name="TResult"/>.</param>
        /// <param name="result">The coerced <typeparamref name="TResult"/> value.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> could be coerced; otherwise, <c>false</c>.</returns>
        public static bool TryCoerceAs<TResult>(object inputObj, Action<TResult> ifSuccess, out TResult result) => TryCoerceAs(inputObj, (Func<TResult>)null, ifSuccess, out result);

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
        public static bool TryCoerceAs<TResult>(object inputObj, Func<TResult> ifNull, bool returnValueIfNull, TryCoerceHandler<object, TResult> fallback, out TResult result) =>
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
        public static bool TryCoerceAs<TResult>(object inputObj, Func<TResult> ifNull, bool returnValueIfNull, out TResult result) =>
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
        public static bool TryCoerceAs<TResult>(object inputObj, Func<TResult> ifNull, TryCoerceHandler<object, TResult> fallback, out TResult result) =>
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
        public static bool TryCoerceAs<TResult>(object inputObj, Func<TResult> ifNull, out TResult result) => TryCoerceAs(inputObj, ifNull, null, out result);

        /// <summary>
        /// Attempts to coerce an object as a specific type.
        /// </summary>
        /// <typeparam name="TResult">The result value type.</typeparam>
        /// <param name="inputObj">The input value.</param>
        /// <param name="returnValueIfNull">The return value of this method when <paramref name="inputObj"/> is null.</param>
        /// <param name="fallback">The fallback conversion handler that will be invoked if <paramref name="inputObj"/> is not null and cannot be converted to type <typeparamref name="TResult"/>.</param>
        /// <param name="result">The coerced <typeparamref name="TResult"/> value.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> could be coerced; otherwise, <c>false</c>.</returns>
        public static bool TryCoerceAs<TResult>(object inputObj, bool returnValueIfNull, TryCoerceHandler<object, TResult> fallback, out TResult result) =>
            TryCoerceAs(inputObj, null, returnValueIfNull, fallback, out result);

        /// <summary>
        /// Attempts to coerce an object as a specific type.
        /// </summary>
        /// <typeparam name="TResult">The result value type.</typeparam>
        /// <param name="inputObj">The input value.</param>
        /// <param name="returnValueIfNull">The return value of this method when <paramref name="inputObj"/> is null.</param>
        /// <param name="result">The coerced <typeparamref name="TResult"/> value.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> could be coerced; otherwise, <c>false</c>.</returns>
        public static bool TryCoerceAs<TResult>(object inputObj, bool returnValueIfNull, out TResult result) => TryCoerceAs(inputObj, null, returnValueIfNull, out result);

        /// <summary>
        /// Attempts to coerce an object as a specific type.
        /// </summary>
        /// <typeparam name="TResult">The result value type.</typeparam>
        /// <param name="inputObj">The input value.</param>
        /// <param name="fallback">The fallback conversion handler that will be invoked if <paramref name="inputObj"/> is not null and cannot be converted to type <typeparamref name="TResult"/>.</param>
        /// <param name="result">The coerced <typeparamref name="TResult"/> value.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> could be coerced; otherwise, <c>false</c>.</returns>
        public static bool TryCoerceAs<TResult>(object inputObj, TryCoerceHandler<object, TResult> fallback, out TResult result) =>
            TryCoerceAs(inputObj, null, fallback, out result);

        /// <summary>
        /// Attempts to coerce an object as a specific type.
        /// </summary>
        /// <typeparam name="TResult">The result value type.</typeparam>
        /// <param name="inputObj">The input value.</param>
        /// <param name="result">The coerced <typeparamref name="TResult"/> value.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> could be coerced; otherwise, <c>false</c>.</returns>
        public static bool TryCoerceAs<TResult>(object inputObj, out TResult result) => TryCoerceAs(inputObj, (Func<TResult>)null, out result);

        #endregion

        #region TryCast overloads

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
        public static bool TryCast<TResult>(object inputObj, Action<TResult> ifSuccess, bool nullReturnValue, Func<TResult> ifNull, out TResult result)
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
        public static bool TryCast<T>(object inputObj, Action<T> ifSuccess, bool nullReturnValue, out T result) => TryCast(inputObj, ifSuccess, nullReturnValue, out result);

        /// <summary>
        /// Attempts to cast an object to a specific type.
        /// </summary>
        /// <typeparam name="T">The type to be cast as.</typeparam>
        /// <param name="inputObj">The source object.</param>
        /// <param name="ifSuccess">The <seealso cref="Action{T}"/> to invoke if <paramref name="inputObj"/> was null or could be cast as type <typeparamref name="T"/>.</param>
        /// <param name="ifNull">Gets the <paramref name="result"/>value if <paramref name="inputObj"/> is null.</param>
        /// <param name="result">The value of <paramref name="inputObj"/> cast or coerced as type <typeparamref name="T"/>.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> could be cast or coerced as type <typeparamref name="T"/>; otherwise, <c>false</c>.</returns>
        public static bool TryCast<T>(object inputObj, Action<T> ifSuccess, Func<T> ifNull, out T result) => TryCast(inputObj, ifSuccess, false, ifNull, out result);

        /// <summary>
        /// Attempts to cast an object to a specific type.
        /// </summary>
        /// <typeparam name="T">The type to be cast as.</typeparam>
        /// <param name="inputObj">The source object.</param>
        /// <param name="ifSuccess">The <seealso cref="Action{T}"/> to invoke if <paramref name="inputObj"/> was null or could be cast as type <typeparamref name="T"/>.</param>
        /// <param name="result">The value of <paramref name="inputObj"/> cast or coerced as type <typeparamref name="T"/>.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> could be cast or coerced as type <typeparamref name="T"/>; otherwise, <c>false</c>.</returns>
        public static bool TryCast<T>(object inputObj, Action<T> ifSuccess, out T result) => TryCast(inputObj, ifSuccess, null, out result);

        /// <summary>
        /// Attempts to cast an object to a specific type.
        /// </summary>
        /// <typeparam name="T">The type to be cast as.</typeparam>
        /// <param name="inputObj">The source object.</param>
        /// <param name="nullReturnValue">The return value of the method if <paramref name="inputObj"/> is null.</param>
        /// <param name="ifNull">Gets the <paramref name="result"/>value if <paramref name="inputObj"/> is null.</param>
        /// <param name="result">The value of <paramref name="inputObj"/> cast or coerced as type <typeparamref name="T"/>.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> could be cast or coerced as type <typeparamref name="T"/>; otherwise, <c>false</c>.</returns>
        public static bool TryCast<T>(object inputObj, bool nullReturnValue, Func<T> ifNull, out T result) => TryCast(inputObj, null, nullReturnValue, ifNull, out result);

        /// <summary>
        /// Attempts to cast an object to a specific type.
        /// </summary>
        /// <typeparam name="T">The type to be cast as.</typeparam>
        /// <param name="inputObj">The source object.</param>
        /// <param name="nullReturnValue">The return value of the method if <paramref name="inputObj"/> is null.</param>
        /// <param name="result">The value of <paramref name="inputObj"/> cast or coerced as type <typeparamref name="T"/>.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> could be cast or coerced as type <typeparamref name="T"/>; otherwise, <c>false</c>.</returns>
        public static bool TryCast<T>(object inputObj, bool nullReturnValue, out T result) => TryCast(inputObj, (Action<T>)null, nullReturnValue, out result);

        /// <summary>
        /// Attempts to cast an object to a specific type.
        /// </summary>
        /// <typeparam name="T">The type to be cast as.</typeparam>
        /// <param name="inputObj">The source object.</param>
        /// <param name="ifNull">Gets the <paramref name="result"/>value if <paramref name="inputObj"/> is null.</param>
        /// <param name="result">The value of <paramref name="inputObj"/> cast or coerced as type <typeparamref name="T"/>.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> could be cast or coerced as type <typeparamref name="T"/>; otherwise, <c>false</c>.</returns>
        public static bool TryCast<T>(object inputObj, Func<T> ifNull, out T result) => TryCast(inputObj, null, ifNull, out result);

        /// <summary>
        /// Attempts to cast an object to a specific type.
        /// </summary>
        /// <typeparam name="T">The type to be cast as.</typeparam>
        /// <param name="inputObj">The source object.</param>
        /// <param name="result">The value of <paramref name="inputObj"/> cast or coerced as type <typeparamref name="T"/>.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> could be cast or coerced as type <typeparamref name="T"/>; otherwise, <c>false</c>.</returns>
        public static bool TryCast<T>(object inputObj, out T result) => TryCast(inputObj, (Action<T>)null, out result);

        #endregion

        public static bool TryGetErrorId(this ErrorCategory errorCategory, out MessageId errorId)
        {
            // TODO: Implement TryGetErrorId
            throw new NotImplementedException();
        }

        public static bool TryGetTargetObject(this Exception exception, out string targetName, out object targetObject)
        {
            // TODO: Implement TryGetTargetObject
            throw new NotImplementedException();
        }

        public static ErrorRecord ToArgumentOutOfRangeError(this MessageId messageId, string message, ErrorCategory errorCategory, string paramName, object actualValue)
        {
            if (string.IsNullOrWhiteSpace(message))
                message = messageId.GetDescription();
            return messageId.ToErrorRecord(new PSArgumentOutOfRangeException(paramName, actualValue, message), errorCategory, paramName, actualValue);
        }

        public static ErrorRecord ToArgumentOutOfRangeError(this MessageId messageId, ErrorCategory errorCategory, string paramName, object actualValue)
        {
            return messageId.ToArgumentOutOfRangeError(null, errorCategory, paramName, actualValue);
        }

        public static ErrorRecord ToArgumentOutOfRangeError(this MessageId messageId, string message, string paramName, object actualValue)
        {
            return messageId.ToArgumentOutOfRangeError(message, ErrorCategory.InvalidArgument, paramName, actualValue);
        }

        public static ErrorRecord ToArgumentOutOfRangeError(this MessageId messageId, string paramName, object actualValue)
        {
            return messageId.ToArgumentOutOfRangeError(null, paramName, actualValue);
        }

        public static ErrorRecord ToArgumentNullError(this MessageId messageId, string message, ErrorCategory errorCategory, string paramName)
        {
            if (string.IsNullOrWhiteSpace(message))
                return messageId.ToErrorRecord(new PSArgumentNullException(paramName), errorCategory, paramName, null);
            return messageId.ToErrorRecord(new PSArgumentNullException(paramName, message), errorCategory, paramName, null);
        }

        public static ErrorRecord ToArgumentNullError(this MessageId messageId, ErrorCategory errorCategory, string paramName)
        {
            return messageId.ToArgumentNullError(null, errorCategory, paramName);
        }

        public static ErrorRecord ToArgumentNullError(this MessageId messageId, string message, string paramName)
        {
            return messageId.ToArgumentNullError(message, ErrorCategory.InvalidArgument, paramName);
        }

        public static ErrorRecord ToArgumentNullError(this MessageId messageId, string paramName)
        {
            return messageId.ToArgumentNullError(null, paramName);
        }

        public static bool TryGetErrorCategory(this Exception exception, out ErrorCategory errorCategory)
        {
            if (TryGetErrorRecord(exception, out ErrorRecord errorRecord))
                errorCategory = errorRecord.CategoryInfo.Category;
            else if (exception is System.IndexOutOfRangeException || exception is ArgumentException)
                errorCategory = ErrorCategory.InvalidArgument;
            else if (exception is InvalidOperationException)
                errorCategory = ErrorCategory.InvalidOperation;
            else if (exception is NotImplementedException)
                errorCategory = ErrorCategory.NotImplemented;
            else if (exception is NotSupportedException)
                errorCategory = ErrorCategory.NotInstalled;
            else if (exception is EndOfStreamException || exception is PathTooLongException)
                errorCategory = ErrorCategory.LimitsExceeded;
            else if (exception is InvalidDataException || exception is FormatException)
                errorCategory = ErrorCategory.InvalidData;
            else if (exception is DirectoryNotFoundException || exception is System.IO.DriveNotFoundException || exception is ItemNotFoundException || exception is FileNotFoundException)
                errorCategory = ErrorCategory.ObjectNotFound;
            else if (exception is System.AccessViolationException)
                errorCategory = ErrorCategory.PermissionDenied;
            else if (exception is System.FormatException)
                errorCategory = ErrorCategory.SyntaxError;
            else if (exception is FileLoadException)
                errorCategory = ErrorCategory.OpenError;
            else if (exception is System.TimeoutException)
                errorCategory = ErrorCategory.OperationTimeout;
            else if (exception is OperationCanceledException)
                errorCategory = ErrorCategory.OperationStopped;
            else if (exception is ParseException)
                errorCategory = ErrorCategory.ParserError;
            else
            {
                errorCategory = ErrorCategory.NotSpecified;
                return false;
            }
            return true;
        }

        public static ErrorRecord ToErrorRecord(this MessageId errorId, string message, Exception exception, ErrorCategory errorCategory, string targetName, object targetObject)
        {
            if (string.IsNullOrWhiteSpace(targetName))
            {
                if (string.IsNullOrWhiteSpace(message))
                    return new ErrorRecord(exception, errorId.GetName(), errorCategory, targetObject);
                return new ErrorRecord(exception, errorId.GetName(), errorCategory, targetObject)
                {
                    ErrorDetails = new ErrorDetails(message)
                };
            }
            ErrorRecord result = (string.IsNullOrWhiteSpace(message)) ? new ErrorRecord(exception, errorId.GetName(), errorCategory, targetObject) :
                new ErrorRecord(exception, errorId.GetName(), errorCategory, targetObject)
                {
                    ErrorDetails = new ErrorDetails(message)
                };
            result.CategoryInfo.TargetName = targetName;
            return result;
        }

        // 5
        public static ErrorRecord ToErrorRecord(this MessageId errorId, string message, Exception exception, string targetName, object targetObject)
        {
            if (!errorId.TryGetAmbientValue(out ErrorCategory errorCategory))
                TryGetErrorCategory(exception, out errorCategory);
            return ToErrorRecord(errorId, message, exception, errorCategory, targetName, targetObject);
            //if (exception is ArgumentException argumentException)
            //    return new ErrorRecord(exception, errorId, ErrorCategory.InvalidArgument, targetObject ?? argumentException.ParamName).SetReason(reason);
            //if (exception is FileLoadException fileLoadException)
            //    return new ErrorRecord(exception, errorId, ErrorCategory.OpenError, targetObject ?? fileLoadException.FileName).SetReason(reason);
            //if (exception is FileNotFoundException fileNotFoundException)
            //    return new ErrorRecord(exception, errorId, ErrorCategory.ObjectNotFound, targetObject ?? fileNotFoundException.FileName).SetReason(reason);
        }

        public static ErrorRecord ToErrorRecord(this MessageId errorId, Exception exception, ErrorCategory errorCategory, string targetName, object targetObject)
        {
            return ToErrorRecord(errorId, null, exception, errorCategory, targetName, targetObject);
        }

        public static ErrorRecord ToErrorRecord(this Exception exception, string message, ErrorCategory errorCategory, string targetName, object targetObject)
        {
            TryGetErrorId(errorCategory, out MessageId errorId);
            return ToErrorRecord(errorId, message, exception, errorCategory, targetName, targetObject);
        }

        public static ErrorRecord ToErrorRecord(this MessageId errorId, string message, Exception exception, ErrorCategory errorCategory)
        {
            TryGetTargetObject(exception, out string targetName, out object targetObject);
            return ToErrorRecord(errorId, message, exception, errorCategory, targetName, targetObject);
        }

        public static ErrorRecord ToErrorRecord(this MessageId errorId, Exception exception, string targetName, object targetObject)
        {
            return ToErrorRecord(errorId, null, exception, targetName, targetObject);
        }

        public static ErrorRecord ToErrorRecord(this Exception exception, string message, string targetName, object targetObject)
        {
            TryGetErrorCategory(exception, out ErrorCategory errorCategory);
            return ToErrorRecord(exception, message, errorCategory, targetName, targetObject);
        }

        public static ErrorRecord ToErrorRecord(this Exception exception, ErrorCategory errorCategory, string targetName, object targetObject)
        {
            return ToErrorRecord(exception, null, errorCategory, targetName, targetObject);
        }

        public static ErrorRecord ToErrorRecord(this MessageId errorId, string message, Exception exception)
        {
            if (!errorId.TryGetAmbientValue(out ErrorCategory errorCategory))
                TryGetErrorCategory(exception, out errorCategory);
            return ToErrorRecord(errorId, message, exception, errorCategory);
        }

        public static ErrorRecord ToErrorRecord(this MessageId errorId, Exception exception, ErrorCategory errorCategory)
        {
            return ToErrorRecord(errorId, null, exception, errorCategory);
        }

        public static ErrorRecord ToErrorRecord(this MessageId errorId, string targetName, object targetObject)
        {
            return ToErrorRecord(errorId, null, targetName, targetObject);
        }

        public static ErrorRecord ToErrorRecord(this Exception exception, string message, ErrorCategory errorCategory)
        {
            TryGetTargetObject(exception, out string targetName, out object targetObject);
            return ToErrorRecord(exception, message, errorCategory, targetName, targetObject);
        }

        public static ErrorRecord ToErrorRecord(this Exception exception, string targetName, object targetObject)
        {
            TryGetErrorCategory(exception, out ErrorCategory errorCategory);
            return ToErrorRecord(exception, errorCategory, targetName, targetObject);
        }

        public static ErrorRecord ToErrorRecord(this MessageId errorId, Exception exception)
        {
            if (!errorId.TryGetAmbientValue(out ErrorCategory errorCategory))
                TryGetErrorCategory(exception, out errorCategory);
            return ToErrorRecord(errorId, exception, errorCategory);
        }

        public static ErrorRecord ToErrorRecord(this Exception exception, string message)
        {
            TryGetErrorCategory(exception, out ErrorCategory errorCategory);
            return ToErrorRecord(exception, message, errorCategory);
        }

        public static ErrorRecord ToErrorRecord(this Exception exception, ErrorCategory errorCategory)
        {
            TryGetErrorId(errorCategory, out MessageId errorId);
            return ToErrorRecord(errorId, exception, errorCategory);
        }

        public static ErrorRecord ToErrorRecord(this Exception exception, bool forceCreateNew = false)
        {
            if (!forceCreateNew && TryGetErrorRecord(exception, out ErrorRecord errorRecord))
                return errorRecord;
            return ToErrorRecord(exception, null);
        }

        /// <summary>
        /// Attempts to get a <seealso cref="ErrorRecord"/> from an input value.
        /// </summary>
        /// <param name="inputObj">The input value.</param>
        /// <param name="errorRecord">The <seealso cref="ErrorRecord"/> object cast or obtained from <paramref name="inputObj"/>.</param>
        /// <returns><c>true</c> if <paramref name="inputObj"/> was an <seealso cref="ErrorRecord"/> object or an object which contained an <seealso cref="ErrorRecord"/>;
        /// otherwise, <c>false</c>.</returns>
        public static bool TryGetErrorRecord(object inputObj, out ErrorRecord errorRecord) => TryCoerceAs(inputObj, (object o, out ErrorRecord e) =>
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
        public static bool TryGetException(object inputObj, out Exception exception) => TryCoerceAs(inputObj, (object o, out Exception e) =>
        {
            if (o is ErrorRecord r || (o is IContainsErrorRecord c && !((r = c.ErrorRecord) is null)))
                return !((e = r.Exception) is null);
            e = null;
            return false;
        }, out exception);

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
