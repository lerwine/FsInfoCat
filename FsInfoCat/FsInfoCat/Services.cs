using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FsInfoCat
{
    public static class Services
    {
        public const string DEFAULT_LOCAL_DB_FILENAME = "FsInfoCat.db";

        public static IServiceProvider ServiceProvider { get; private set; }

        private static string GetAppDataPath(string path, CultureInfo cultureInfo, bool doNotCreate)
        {
            string name;
            int index;
            if (cultureInfo is null || string.IsNullOrWhiteSpace(name = cultureInfo.Name) ||
                    ((index = name.IndexOf("/")) >= 0 && (name = name.Substring(0, index).Trim()).Length == 0))
                return path;
            path = Path.Combine(path, name);
            if (!(doNotCreate || Directory.Exists(path)))
                Directory.CreateDirectory(path);
            return path;
        }

        public static string GetAppDataPath(Assembly assembly, bool doNotCreate = false) => GetAppDataPath(assembly, AppDataPathLevel.CurrentVersion, doNotCreate);

        public static string GetAppDataPath(Assembly assembly, CultureInfo culture, bool doNotCreate = false) =>
            GetAppDataPath(GetAppDataPath(assembly, AppDataPathLevel.CurrentVersion, doNotCreate), culture, doNotCreate);

        public static string GetAppDataPath(Assembly assembly, AppDataPathLevel level, bool doNotCreate = false)
        {
            AssemblyCompanyAttribute companyAttr = assembly.GetCustomAttribute<AssemblyCompanyAttribute>();
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), companyAttr.Company);
            if (!(doNotCreate || Directory.Exists(path)))
                Directory.CreateDirectory(path);
            if (level == AppDataPathLevel.Company)
                return path;
            AssemblyName assemblyName = assembly.GetName();
            path = Path.Combine(path, assemblyName.Name);
            if (!(doNotCreate || Directory.Exists(path)))
                Directory.CreateDirectory(path);
            if (level == AppDataPathLevel.Application)
                return path;
            path = Path.Combine(path, assemblyName.Version.ToString());
            if (!(doNotCreate || Directory.Exists(path)))
                Directory.CreateDirectory(path);
            if (level == AppDataPathLevel.CurrentVersion)
                return path;
            return GetAppDataPath(path, assemblyName.CultureInfo, doNotCreate);
        }

        public static void Initialize(Action<IServiceCollection> configureServices)
        {
            if (!(ServiceProvider is null))
                throw new InvalidOperationException();
            //ServiceProvider = new DummyServiceProvider();
            ServiceCollection services = new();
            services.AddLogging(b => b.AddDebug());
            ServiceProvider = services.BuildServiceProvider();
            configureServices?.Invoke(services);
            ServiceProvider = services.BuildServiceProvider();
        }

        public static string AsNonNullTrimmed(this string text) => string.IsNullOrWhiteSpace(text) ? "" : text.Trim();

        public static IEnumerable<string> AsNonNullTrimmedValues(this IEnumerable<string> text) => (text is null) ? null : text.Select(AsNonNullTrimmed);

        public static IEnumerable<string> AsNonNullValues(this IEnumerable<string> text) => (text is null) ? null : text.Select(t => t ?? "");

        public static IEnumerable<string> AsOrderedDistinct(this IEnumerable<string> text) => (text is null) ? null : text.Select(t => t ?? "").Distinct().OrderBy(t => t);

        public static string EmptyIfNullOrWhiteSpace(this string source) => string.IsNullOrWhiteSpace(source) ? "" : source;

        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source) => (source is null) ? Array.Empty<T>() : source;

        public static IEnumerable<KeyValuePair<int, T>> ToIndexValuePairs<T>(this IEnumerable<T> source) => (source is null) ? null : source.Select((e, i) => new KeyValuePair<int, T>(i, e));

        public static IEnumerable<KeyValuePair<int, TResult>> ToIndexValuePairs<TElement, TResult>(this IEnumerable<TElement> source, Func<TElement, TResult> transform)
        {
            if (transform is null)
                throw new ArgumentNullException(nameof(transform));
            return (source is null) ? null : source.Select((e, i) => new KeyValuePair<int, TResult>(i, transform(e)));
        }

        public static IEnumerable<KeyValuePair<TKey, TValue>> ToKeyValuePairs<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, int, TKey> getKey, Func<TSource, int, TValue> getValue)
        {
            if (getKey is null)
                throw new ArgumentNullException(nameof(getKey));
            if (getValue is null)
                throw new ArgumentNullException(nameof(getValue));
            return (source is null) ? null : source.Select((e, i) => new KeyValuePair<TKey, TValue>(getKey(e, i), getValue(e, i)));
        }

        public static IEnumerable<KeyValuePair<TKey, TValue>> ToKeyValuePairs<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, int, TKey> getKey, Func<TSource, TValue> getValue)
        {
            if (getKey is null)
                throw new ArgumentNullException(nameof(getKey));
            if (getValue is null)
                throw new ArgumentNullException(nameof(getValue));
            return (source is null) ? null : source.Select((e, i) => new KeyValuePair<TKey, TValue>(getKey(e, i), getValue(e)));
        }

        public static IEnumerable<KeyValuePair<TKey, TValue>> ToKeyValuePairs<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> getKey, Func<TSource, int, TValue> getValue)
        {
            if (getKey is null)
                throw new ArgumentNullException(nameof(getKey));
            if (getValue is null)
                throw new ArgumentNullException(nameof(getValue));
            return (source is null) ? null : source.Select((e, i) => new KeyValuePair<TKey, TValue>(getKey(e), getValue(e, i)));
        }

        public static IEnumerable<KeyValuePair<TKey, TValue>> ToKeyValuePairs<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> getKey, Func<TSource, TValue> getValue)
        {
            if (getKey is null)
                throw new ArgumentNullException(nameof(getKey));
            if (getValue is null)
                throw new ArgumentNullException(nameof(getValue));
            return (source is null) ? null : source.Select(e => new KeyValuePair<TKey, TValue>(getKey(e), getValue(e)));
        }

        public static IEnumerable<KeyValuePair<TKey, TValue>> ToKeyValuePairs<TKey, TValue>(this IEnumerable<TValue> source, Func<TValue, int, TKey> getKey)
        {
            if (getKey is null)
                throw new ArgumentNullException(nameof(getKey));
            return (source is null) ? null : source.Select((e, i) => new KeyValuePair<TKey, TValue>(getKey(e, i), e));
        }

        public static IEnumerable<KeyValuePair<TKey, TValue>> ToKeyValuePairs<TKey, TValue>(this IEnumerable<TValue> source, Func<TValue, TKey> getKey)
        {
            if (getKey is null)
                throw new ArgumentNullException(nameof(getKey));
            return (source is null) ? null : source.Select(e => new KeyValuePair<TKey, TValue>(getKey(e), e));
        }

        //private class DummyServiceProvider : IServiceProvider
        //{
        //    public object GetService(Type serviceType) => null;
        //}
    }
    //public interface ILoggingService
    //{
    //    ILogger<T> CreateLogger<T>();
    //}
    //internal class LoggingService : ILoggingService
    //{
    //    private static readonly LoggerFactory _loggerFactory;

    //    static LoggingService()
    //    {
    //        _loggerFactory = new LoggerFactory();
    //        _loggerFactory.AddProvider(new DebugLoggerProvider());
    //    }

    //    public ILogger<T> CreateLogger<T>() => _loggerFactory.CreateLogger<T>();
    //}
}
