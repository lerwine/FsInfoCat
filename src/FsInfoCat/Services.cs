using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
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

        [Obsolete("Use ExtensionMethods.NewLineRegex")]
        public static readonly Regex NewLineRegex = new(@"\r\n?|\n", RegexOptions.Compiled);

        [Obsolete("Use ExtensionMethods.AbnormalWsRegex")]
        public static readonly Regex AbnormalWsRegex = new(@" [\s\p{Z}\p{C}]+|(?! )[\s\p{Z}\p{C}]+", RegexOptions.Compiled);

        [Obsolete("Use ExtensionMethods.OuterWsRegex")]
        public static readonly Regex OuterWsRegex = new(@"^[\s\p{Z}\p{C}]+|[\s\p{Z}\p{C}]+$", RegexOptions.Compiled);

        [Obsolete("Use ExtensionMethods.BackslashEscapablePattern")]
        public static readonly Regex BackslashEscapablePattern = new(@"(?<l>[""\\])|[\0\a\b\f\n\r\t\v]|(\p{C}|(?! )(\s|\p{Z}))(?<x>[\da-fA-F])?", RegexOptions.Compiled);

        [Obsolete("Use ExtensionMethods.BackslashEscapableLBPattern")]
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

        [Obsolete("Use ExtensionMethods.ToCsTypeName(Type, bool)")]
        public static string ToCsTypeName(Type type, bool omitNamespaces = false)
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
    }
}
