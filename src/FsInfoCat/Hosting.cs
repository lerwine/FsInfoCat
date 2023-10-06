using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat
{
    // TODO: Document Hosting class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class Hosting
    {
        private static Task<IHost> _initializeTask;
        private static IHost _host;

        public const string DEFAULT_LOCAL_DB_FILENAME = "FsInfoCat.db";

        public static IHost Host => _host is null
                    ? _initializeTask is null
                        ? throw new InvalidOperationException($"{nameof(Hosting)}.{nameof(Initialize)} has not been invoked.")
                        : _initializeTask.Result
                    : _host;

        public static IServiceProvider ServiceProvider => Host.Services;

        public static Activities.IAsyncActivityService GetAsyncActivityService() => (Activities.IAsyncActivityService)Host.Services.GetRequiredService<IHostedService>();

        public static IServiceScope CreateScope() => Host.Services.CreateScope();

        public static T GetRequiredService<T>() => Host.Services.GetRequiredService<T>();

        private static string GetAppDataPath(string path, CultureInfo cultureInfo)
        {
            string name;
            int index;
            if (cultureInfo is null || string.IsNullOrWhiteSpace(name = cultureInfo.Name) ||
                    ((index = name.IndexOf("/")) >= 0 && (name = name.Substring(0, index).Trim()).Length == 0))
                return path;
            path = Path.Combine(path, name);
            if (!Directory.Exists(path))
                _ = Directory.CreateDirectory(path);
            return path;
        }

        public static Regex FileNameEncodedSequenceRegex = new(@"__0x[\da-f]{4}__", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static string GetAppDataPath(Assembly assembly) => GetAppDataPath(assembly, AppDataPathLevel.CurrentVersion);

        public static string GetAppDataPath(Assembly assembly, CultureInfo culture) => GetAppDataPath(GetAppDataPath(assembly, AppDataPathLevel.CurrentVersion), culture);

        public static string GetAppDataPath(Assembly assembly, AppDataPathLevel level)
        {
            AssemblyCompanyAttribute companyAttr = assembly.GetCustomAttribute<AssemblyCompanyAttribute>();
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), companyAttr?.Company ?? "UnknownCompany");
            if (!Directory.Exists(path))
                _ = Directory.CreateDirectory(path);
            if (level == AppDataPathLevel.Company)
                return path;
            path = Path.Combine(path, assembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product.NullIfWhiteSpace() ?? nameof(FsInfoCat));
            if (!Directory.Exists(path))
                _ = Directory.CreateDirectory(path);
            if (level == AppDataPathLevel.Product)
                return path;
            AssemblyName assemblyName = assembly.GetName();
            path = Path.Combine(path, assemblyName.Name ?? "UnknownAssembly");
            if (!Directory.Exists(path))
                _ = Directory.CreateDirectory(path);
            if (level == AppDataPathLevel.Assembly)
                return path;
            Version version = assemblyName.Version;
            if (version is not null)
            {
                path = Path.Combine(path, version.ToString());
                if (!Directory.Exists(path))
                    _ = Directory.CreateDirectory(path);
            }
            if (level == AppDataPathLevel.CurrentVersion)
                return path;
            return GetAppDataPath(path, assemblyName.CultureInfo);
        }

        /// <summary>
        /// Initializes the application <see cref="Microsoft.Extensions.Hosting.Host"/>.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static async Task<IHost> Initialize(string[] args, params Assembly[] assemblies)
        {
            Thread.BeginCriticalRegion();
            if (_initializeTask is null)
            {
                _initializeTask = Task.Run(async () =>
                {
                    System.Diagnostics.Debug.WriteLine($"Invoked {typeof(Hosting).FullName}.{nameof(Initialize)} initialize task started");
                    _host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                        .UseDefaultServiceProvider(opt => { }) // workaround?
                        .ConfigureHostConfiguration(builder => builder
                            .SetBasePath(AppContext.BaseDirectory)
                            .AddJsonFile(path: "hostsettings.json", optional: true, reloadOnChange: true)
                        ).ConfigureAppConfiguration((context, builder) =>
                        {
                            builder.SetBasePath(AppContext.BaseDirectory)
                                .AddJsonFile(path: "appsettings.json", optional: true, reloadOnChange: true)
                                .AddJsonFile(path: $"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
                            ConfigurationBuilderHandlerAttribute.InvokeHandlers(context, builder, assemblies);
                            FsInfoCatOptions.Configure(args, builder, context, assemblies);
                        }).ConfigureServices((hostContext, services) =>
                        {
                            services.AddLogging();
                            services.Configure<FsInfoCatOptions>(hostContext.Configuration.GetSection(FsInfoCatOptions.FsInfoCat));
                            ServiceBuilderHandlerAttribute.InvokeHandlers(hostContext, services, assemblies);
                        })
                        .ConfigureLogging((context, builder) =>
                            builder.Configure(options => options.ActivityTrackingOptions = ActivityTrackingOptions.SpanId | ActivityTrackingOptions.TraceId | ActivityTrackingOptions.ParentId)
                        ).Build();
                    System.Diagnostics.Debug.WriteLine($"{typeof(Hosting).FullName}.{nameof(Initialize)} host built");
                    await _host.StartAsync();
                    System.Diagnostics.Debug.WriteLine($"{typeof(Hosting).FullName}.{nameof(Initialize)} host started");
                    return _host;
                });
            }
            else
                System.Diagnostics.Debug.WriteLine($"{typeof(Hosting).FullName}.{nameof(Initialize)} initialize task already started");
            Thread.EndCriticalRegion();
            return await _initializeTask;
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
