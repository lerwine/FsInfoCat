using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat
{
    public static class Services
    {
        private static Task<IHost> _initializeTask;
        private static IHost _host;

        public const string DEFAULT_LOCAL_DB_FILENAME = "FsInfoCat.db";

        public static readonly Regex BackslashEscapablePattern = new(@"(?<l>[""\\])|[\0\a\b\f\n\r\t\v]|(\p{C}|(?! )(\s|\p{Z}))(?<x>[\da-fA-F])?", RegexOptions.Compiled);

        public static readonly Regex BackslashEscapableLBPattern = new(@"(?<l>[""\\])|(?<n>\r\n?|\n)|[\0\a\b\f\t\v]|(\p{C}|(?! )(\s|\p{Z}))(?<x>[\da-fA-F])?", RegexOptions.Compiled);

        public static IHost Host => _host is null
                    ? _initializeTask is null
                        ? throw new InvalidOperationException($"{nameof(Services)}.{nameof(Initialize)} has not been invoked.")
                        : _initializeTask.Result
                    : _host;

        public static IServiceProvider ServiceProvider => Host.Services;

        private static string GetAppDataPath(string path, CultureInfo cultureInfo)
        {
            string name;
            int index;
            if (cultureInfo is null || string.IsNullOrWhiteSpace(name = cultureInfo.Name) ||
                    ((index = name.IndexOf("/")) >= 0 && (name = name.Substring(0, index).Trim()).Length == 0))
                return path;
            path = Path.Combine(path, name);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }

        public static string GetAppDataPath(Assembly assembly) => GetAppDataPath(assembly, AppDataPathLevel.CurrentVersion);

        public static string GetAppDataPath(Assembly assembly, CultureInfo culture) => GetAppDataPath(GetAppDataPath(assembly, AppDataPathLevel.CurrentVersion), culture);

        public static string GetAppDataPath(Assembly assembly, AppDataPathLevel level)
        {
            AssemblyCompanyAttribute companyAttr = assembly.GetCustomAttribute<AssemblyCompanyAttribute>();
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), companyAttr.Company);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            if (level == AppDataPathLevel.Company)
                return path;
            AssemblyName assemblyName = assembly.GetName();
            path = Path.Combine(path, assemblyName.Name);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            if (level == AppDataPathLevel.Application)
                return path;
            path = Path.Combine(path, assemblyName.Version.ToString());
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            if (level == AppDataPathLevel.CurrentVersion)
                return path;
            return GetAppDataPath(path, assemblyName.CultureInfo);
        }

        //private static async Task<IHost> PrivateInitialize(string[] args)
        //{
        //    _host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
        //        .ConfigureHostConfiguration(builder => builder
        //            .SetBasePath(AppContext.BaseDirectory)
        //            .AddJsonFile(path: "hostsettings.json", optional: true, reloadOnChange: true)
        //        ).ConfigureAppConfiguration((context, builder) => builder
        //            .SetBasePath(AppContext.BaseDirectory)
        //            .AddJsonFile(path: "appsettings.json", optional: true, reloadOnChange: true)
        //            .AddJsonFile(path: $"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
        //        ).ConfigureServices(ServiceBuilderHandlerAttribute.InvokeHandlers)
        //        .ConfigureLogging((context, builder) =>
        //            builder.Configure(options => options.ActivityTrackingOptions = ActivityTrackingOptions.SpanId | ActivityTrackingOptions.TraceId | ActivityTrackingOptions.ParentId)
        //        ).Build();
        //    await _host.StartAsync();
        //    return _host;
        //}

        public static async Task<IHost> Initialize(params string[] args)
        {
            Thread.BeginCriticalRegion();
            if (_initializeTask is null)
                _initializeTask = Task.Run(async () =>
                {
                    _host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                        .ConfigureHostConfiguration(builder => builder
                            .SetBasePath(AppContext.BaseDirectory)
                            .AddJsonFile(path: "hostsettings.json", optional: true, reloadOnChange: true)
                        ).ConfigureAppConfiguration((context, builder) => builder
                            .SetBasePath(AppContext.BaseDirectory)
                            .AddJsonFile(path: "appsettings.json", optional: true, reloadOnChange: true)
                            .AddJsonFile(path: $"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                        ).ConfigureServices(ServiceBuilderHandlerAttribute.InvokeHandlers)
                        .ConfigureLogging((context, builder) =>
                            builder.Configure(options => options.ActivityTrackingOptions = ActivityTrackingOptions.SpanId | ActivityTrackingOptions.TraceId | ActivityTrackingOptions.ParentId)
                        ).Build();
                    await _host.StartAsync();
                    return _host;
                });
            Thread.EndCriticalRegion();
            return await _initializeTask;
        }

        //public static async Task Initialize_Obsolete(Action<IServiceCollection> configureServices, params string[] args)
        //{
        //    if (!(Host is null))
        //        throw new InvalidOperationException();
        //    _host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args).ConfigureHostConfiguration(builder =>
        //    {
        //        builder.SetBasePath(AppContext.BaseDirectory);
        //        builder.AddJsonFile(path: "hostsettings.json", optional: true, reloadOnChange: true);

        //    }).ConfigureAppConfiguration((context, builder) =>
        //    {
        //        builder.SetBasePath(AppContext.BaseDirectory);
        //        builder.AddJsonFile(path: "appsettings.json", optional: true, reloadOnChange: true);
        //        builder.AddJsonFile(path: $"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);

        //    }).ConfigureServices(services =>
        //    {
        //        configureServices?.Invoke(services);
        //    }).ConfigureLogging((context, builder) => builder.Configure(options =>
        //    {
        //        options.ActivityTrackingOptions = ActivityTrackingOptions.SpanId | ActivityTrackingOptions.TraceId | ActivityTrackingOptions.ParentId;
        //    })).Build();
        //    await Host.StartAsync();
        //}

        [Obsolete("Pass cancellation token")]
        public static async Task<IEnumerable<TProperty>> GetRelatedCollectionAsync<TEntity, TProperty>([NotNull] this EntityEntry<TEntity> entry, [NotNull] Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression)
            where TEntity : class
            where TProperty : class
        {
            CollectionEntry<TEntity, TProperty> collectionEntry = entry.Collection(propertyExpression);
            if (!collectionEntry.IsLoaded)
                await collectionEntry.LoadAsync();
            return collectionEntry.CurrentValue;
        }

        public static async Task<IEnumerable<TProperty>> GetRelatedCollectionAsync<TEntity, TProperty>([NotNull] this EntityEntry<TEntity> entry,
            [NotNull] Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression, CancellationToken cancellationToken)
            where TEntity : class
            where TProperty : class
        {
            cancellationToken.ThrowIfCancellationRequested();
            CollectionEntry<TEntity, TProperty> collectionEntry = entry.Collection(propertyExpression);
            if (!collectionEntry.IsLoaded)
                await collectionEntry.LoadAsync(cancellationToken);
            return collectionEntry.CurrentValue;
        }

        [Obsolete("Pass cancellation token")]
        public static async Task<TProperty> GetRelatedReferenceAsync<TEntity, TProperty>([NotNull] this EntityEntry<TEntity> entry, [NotNull] Expression<Func<TEntity, TProperty>> propertyExpression)
            where TEntity : class
            where TProperty : class
        {
            ReferenceEntry<TEntity, TProperty> referenceEntry = entry.Reference(propertyExpression);
            if (!referenceEntry.IsLoaded)
                await referenceEntry.LoadAsync();
            return referenceEntry.CurrentValue;
        }

        public static async Task<TProperty> GetRelatedReferenceAsync<TEntity, TProperty>([NotNull] this EntityEntry<TEntity> entry,
            [NotNull] Expression<Func<TEntity, TProperty>> propertyExpression, CancellationToken cancellationToken)
            where TEntity : class
            where TProperty : class
        {
            cancellationToken.ThrowIfCancellationRequested();
            ReferenceEntry<TEntity, TProperty> referenceEntry = entry.Reference(propertyExpression);
            if (!referenceEntry.IsLoaded)
                await referenceEntry.LoadAsync(cancellationToken);
            return referenceEntry.CurrentValue;
        }

        public static void RejectChanges<T>(this DbSet<T> dbSet, Func<T, EntityEntry<T>> getEntry) where T : class, IRevertibleChangeTracking
        {
            if (getEntry is null)
                throw new ArgumentNullException(nameof(getEntry));
            if (dbSet is null)
                return;
            T[] items = dbSet.Local.ToArray();
            foreach (T t in items)
            {
                EntityEntry<T> entry = getEntry(t);
                if (entry is null)
                    t.RejectChanges();
                else
                    RejectChanges(entry);
            }
        }

        public static void RejectChanges(this DbContext dbContext)
        {
            if (dbContext is null)
                return;
            EntityEntry[] entityEntries = dbContext.ChangeTracker.Entries().ToArray();
            foreach (EntityEntry entry in entityEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        dbContext.Remove(entry.Entity);
                        break;
                    case EntityState.Modified:
                        if (entry.Entity is IDbEntity dbEntity)
                            dbEntity.RejectChanges();
                        break;
                }
            }
        }

        public static void RejectChanges(this EntityEntry entry)
        {
            if (entry is null)
                return;
            switch (entry.State)
            {
                case EntityState.Added:
                    //if (entry.Entity is IRevertibleChangeTracking rct)
                    //    rct.RejectChanges();
                    entry.Context.Remove(entry.Entity);
                    break;
                case EntityState.Deleted:
                    if (entry.Entity is IRevertibleChangeTracking rct2)
                    {
                        rct2.RejectChanges();
                        entry.State = rct2.IsChanged ? EntityState.Modified : EntityState.Unchanged;
                    }
                    break;
                case EntityState.Unchanged:
                    break;
                default:
                    if (entry.Entity is IRevertibleChangeTracking rct3)
                        rct3.RejectChanges();
                    break;
            }
        }

        public static string AsNonNullTrimmed(this string text) => string.IsNullOrWhiteSpace(text) ? "" : text.Trim();

        public static string NullIfEmpty(this string text) => string.IsNullOrEmpty(text) ? null : text.Trim();

        public static string NullIfWhiteSpace(this string text) => string.IsNullOrWhiteSpace(text) ? null : text.Trim();

        public static readonly Regex NewLineRegex = new(@"\r\n?|\n", RegexOptions.Compiled);

        public static string[] SplitLines(this string text) => (text is null) ? Array.Empty<string>() : NewLineRegex.Split(text);

        public static string JoinWithNewLines(this IEnumerable<string> text) => (text is null || !text.Any()) ? null : string.Join(Environment.NewLine, text);

        public static IEnumerable<string> AsNonNullTrimmedValues(this IEnumerable<string> text) => text?.Select(AsNonNullTrimmed);

        public static IEnumerable<string> AsNonNullValues(this IEnumerable<string> text) => text?.Select(t => t ?? "");

        public static IEnumerable<string> AsOrderedDistinct(this IEnumerable<string> text) => text?.Select(t => t ?? "").Distinct().OrderBy(t => t);

        public static string EmptyIfNullOrWhiteSpace(this string source) => string.IsNullOrWhiteSpace(source) ? "" : source;

        public static IEnumerable<string> ValuesEmptyIfNullOrWhiteSpace(this IEnumerable<string> source) => source?.Select(EmptyIfNullOrWhiteSpace);

        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source) => (source is null) ? Array.Empty<T>() : source;

        public static IEnumerable<KeyValuePair<int, T>> ToIndexValuePairs<T>(this IEnumerable<T> source) => source?.Select((e, i) => new KeyValuePair<int, T>(i, e));

        public static IEnumerable<KeyValuePair<int, TResult>> ToIndexValuePairs<TElement, TResult>(this IEnumerable<TElement> source, Func<TElement, TResult> transform)
        {
            if (transform is null)
                throw new ArgumentNullException(nameof(transform));
            return source?.Select((e, i) => new KeyValuePair<int, TResult>(i, transform(e)));
        }

        public static IEnumerable<KeyValuePair<TKey, TValue>> ToKeyValuePairs<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, int, TKey> getKey, Func<TSource, int, TValue> getValue)
        {
            if (getKey is null)
                throw new ArgumentNullException(nameof(getKey));
            if (getValue is null)
                throw new ArgumentNullException(nameof(getValue));
            return source?.Select((e, i) => new KeyValuePair<TKey, TValue>(getKey(e, i), getValue(e, i)));
        }

        public static IEnumerable<KeyValuePair<TKey, TValue>> ToKeyValuePairs<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, int, TKey> getKey, Func<TSource, TValue> getValue)
        {
            if (getKey is null)
                throw new ArgumentNullException(nameof(getKey));
            if (getValue is null)
                throw new ArgumentNullException(nameof(getValue));
            return source?.Select((e, i) => new KeyValuePair<TKey, TValue>(getKey(e, i), getValue(e)));
        }

        public static IEnumerable<KeyValuePair<TKey, TValue>> ToKeyValuePairs<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> getKey, Func<TSource, int, TValue> getValue)
        {
            if (getKey is null)
                throw new ArgumentNullException(nameof(getKey));
            if (getValue is null)
                throw new ArgumentNullException(nameof(getValue));
            return source?.Select((e, i) => new KeyValuePair<TKey, TValue>(getKey(e), getValue(e, i)));
        }

        public static IEnumerable<KeyValuePair<TKey, TValue>> ToKeyValuePairs<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> getKey, Func<TSource, TValue> getValue)
        {
            if (getKey is null)
                throw new ArgumentNullException(nameof(getKey));
            if (getValue is null)
                throw new ArgumentNullException(nameof(getValue));
            return source?.Select(e => new KeyValuePair<TKey, TValue>(getKey(e), getValue(e)));
        }

        public static IEnumerable<KeyValuePair<TKey, TValue>> ToKeyValuePairs<TKey, TValue>(this IEnumerable<TValue> source, Func<TValue, int, TKey> getKey)
        {
            if (getKey is null)
                throw new ArgumentNullException(nameof(getKey));
            return source?.Select((e, i) => new KeyValuePair<TKey, TValue>(getKey(e, i), e));
        }

        public static IEnumerable<KeyValuePair<TKey, TValue>> ToKeyValuePairs<TKey, TValue>(this IEnumerable<TValue> source, Func<TValue, TKey> getKey)
        {
            if (getKey is null)
                throw new ArgumentNullException(nameof(getKey));
            return source?.Select(e => new KeyValuePair<TKey, TValue>(getKey(e), e));
        }

        public static bool IsNullableType(this Type type) => (type ?? throw new ArgumentNullException(nameof(type))).IsValueType && type.IsGenericType &&
            typeof(Nullable<>).Equals(type.GetGenericTypeDefinition());

        public static bool IsNullAssignable(this Type type) => !(type ?? throw new ArgumentNullException(nameof(type))).IsValueType || type.IsNullableType();

        public static string ToCsTypeName(this Type type, bool omitNamespaces = false)
        {
            if (type is null)
                return "null";
            if (type.IsGenericParameter)
                return type.Name;
            if (type.IsPointer)
                return $"{ToCsTypeName(type.GetElementType(), omitNamespaces)}*";
            if (type.IsByRef)
                return $"{ToCsTypeName(type.GetElementType(), omitNamespaces)}&";
            if (type.IsArray)
            {
                int rank = type.GetArrayRank();
                if (rank < 2)
                    return $"{ToCsTypeName(type.GetElementType(), omitNamespaces)}[]";
                if (rank == 2)
                    return $"{ToCsTypeName(type.GetElementType(), omitNamespaces)}[,]";
                return $"{ToCsTypeName(type.GetElementType(), omitNamespaces)}[{new string(',', rank - 1)}]";
            }
            if (type.IsNullableType())
                return $"{ToCsTypeName(Nullable.GetUnderlyingType(type), omitNamespaces)}?";

            if (type.IsValueType)
            {
                if (type.Equals(typeof(void)))
                    return "void";
                if (type.Equals(typeof(char)))
                    return "char";
                if (type.Equals(typeof(bool)))
                    return "bool";
                if (type.Equals(typeof(byte)))
                    return "byte";
                if (type.Equals(typeof(sbyte)))
                    return "sbyte";
                if (type.Equals(typeof(short)))
                    return "short";
                if (type.Equals(typeof(ushort)))
                    return "ushort";
                if (type.Equals(typeof(int)))
                    return "int";
                if (type.Equals(typeof(uint)))
                    return "uint";
                if (type.Equals(typeof(long)))
                    return "long";
                if (type.Equals(typeof(ulong)))
                    return "ulong";
                if (type.Equals(typeof(float)))
                    return "float";
                if (type.Equals(typeof(double)))
                    return "double";
                if (type.Equals(typeof(decimal)))
                    return "decimal";
            }
            else
            {
                if (type.Equals(typeof(string)))
                    return "string";
                if (type.Equals(typeof(object)))
                    return "object";
            }
            string n = type.Name;
            string ns;
            if (type.IsNested)
                ns = ToCsTypeName(type.DeclaringType, omitNamespaces);
            else if (omitNamespaces || (ns = type.Namespace) is null || ns == "System")
                ns = "";

            if (type.IsGenericType)
            {
                int i = n.IndexOf("`");
                if (i > 0)
                    n = n.Substring(0, i);
                if (ns.Length > 0)
                    return $"{ns}.{n}<{string.Join(",", type.GetGenericArguments().Select(a => a.ToCsTypeName(omitNamespaces)))}>";
                return $"{n}<{string.Join(",", type.GetGenericArguments().Select(a => a.ToCsTypeName(omitNamespaces)))}>";
            }
            return (ns.Length > 0) ? $"{ns}.{n}" : n;
        }

        public static string ToPseudoCsText(object obj)
        {
            if (obj is null)
                return "null";
            if (obj is string s)
                return $"\"{EscapeCsString(s)}\"";
            if (obj is char c)
                return c switch
                {
                    '\'' => "'\\''",
                    '"' => "'\"'",
                    _ => $"'{EscapeCsString(new string(new char[] { c }))}'",
                };
            if (obj is bool bv)
                return bv ? "true" : "false";
            if (obj is byte bn)
                return bn.ToString("X2");
            if (obj is sbyte sb)
                return $"(sbyte){sb:X2}";
            if (obj is short sv)
                return sv.ToString("X4");
            if (obj is ushort us)
                return $"(ushort){us:X4}";
            if (obj is int i)
                return i.ToString("X8");
            if (obj is uint ui)
                return $"{ui:X8}U";
            if (obj is long l)
                return l.ToString("X16");
            if (obj is ulong ul)
                return $"{ul:16}UL";
            if (obj is float fv)
                return $"{fv}f";
            if (obj is double d)
                return d.ToString();
            if (obj is decimal m)
                return $"{m}m";
            if (obj is DateTime dt)
                return dt.ToString();
            if (obj is DBNull)
                return "DBNull";
            if (obj is Type t)
                return t.ToCsTypeName();
            if (obj is IFormattable fm)
                fm.ToString();
            if (obj is IConvertible cv)
            {
                switch (cv.GetTypeCode())
                {
                    case TypeCode.Boolean:
                        return ToPseudoCsText(cv.ToBoolean(null));
                    case TypeCode.Byte:
                        return ToPseudoCsText(cv.ToByte(null));
                    case TypeCode.Char:
                        return ToPseudoCsText(cv.ToChar(null));
                    case TypeCode.DateTime:
                        return ToPseudoCsText(cv.ToDateTime(null));
                    case TypeCode.DBNull:
                        return "DBNull";
                    case TypeCode.Decimal:
                        return ToPseudoCsText(cv.ToDecimal(null));
                    case TypeCode.Double:
                        return ToPseudoCsText(cv.ToDouble(null));
                    case TypeCode.Int16:
                        return ToPseudoCsText(cv.ToInt16(null));
                    case TypeCode.Int32:
                        return ToPseudoCsText(cv.ToInt32(null));
                    case TypeCode.Int64:
                        return ToPseudoCsText(cv.ToInt64(null));
                    case TypeCode.SByte:
                        return ToPseudoCsText(cv.ToSByte(null));
                    case TypeCode.Single:
                        return ToPseudoCsText(cv.ToSingle(null));
                    case TypeCode.String:
                        return ToPseudoCsText(cv.ToString(null));
                    case TypeCode.UInt16:
                        return ToPseudoCsText(cv.ToUInt16(null));
                    case TypeCode.UInt32:
                        return ToPseudoCsText(cv.ToUInt32(null));
                    case TypeCode.UInt64:
                        return ToPseudoCsText(cv.ToUInt64(null));
                }
            }
            return obj.ToString();
        }

        public static string EscapeCsString(string source, bool keepLineBreaks = false)
        {
            if (string.IsNullOrEmpty(source) || !BackslashEscapablePattern.IsMatch(source))
                return source;
            if (keepLineBreaks)
                return BackslashEscapableLBPattern.Replace(source, m =>
                {
                    if (m.Groups["l"].Success)
                        return $"\\{m.Value}";
                    Group g = m.Groups["n"];
                    if (g.Success)
                        return g.Value switch
                        {
                            "\r" => "\\r\r",
                            "\n" => "\\n\n",
                            _ => "\\r\\n\r\n",
                        };
                    char c = m.Value[0];
                    switch (c)
                    {
                        case '\0':
                            return "\\0";
                        case '\a':
                            return "\\a";
                        case '\b':
                            return "\\b";
                        case '\f':
                            return "\\f";
                        case '\t':
                            return "\\t";
                        case '\v':
                            return "\\v";
                        default:
                            g = m.Groups["x"];
                            uint i = (uint)c;
                            if (g.Success)
                                return $"\\x{i:x4}{g.Value}";
                            return (i > 0xff) ? $"\\x{i:x4}" : $"\\x{i:x2}";
                    }
                });
            return BackslashEscapablePattern.Replace(source, m =>
            {
                if (m.Groups["l"].Success)
                    return $"\\{m.Value}";
                char c = m.Value[0];
                switch (c)
                {
                    case '\0':
                        return "\\0";
                    case '\a':
                        return "\\a";
                    case '\b':
                        return "\\b";
                    case '\f':
                        return "\\f";
                    case '\n':
                        return "\\n";
                    case '\r':
                        return "\\r";
                    case '\t':
                        return "\\t";
                    case '\v':
                        return "\\v";
                    default:
                        Group g = m.Groups["x"];
                        uint i = (uint)c;
                        if (g.Success)
                            return $"\\x{i:x4}{g.Value}";
                        return (i > 0xff) ? $"\\x{i:x4}" : $"\\x{i:x2}";
                }
            });
        }

        //private class DummyServiceProvider : IServiceProvider
        //{
        //    public object GetService(Type serviceType) => null;
        //}
    }
}
