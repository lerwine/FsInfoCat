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

        public static readonly Regex NewLineRegex = new(@"\r\n?|\n", RegexOptions.Compiled);

        public static readonly Regex AbnormalWsRegex = new(@" [\s\p{Z}\p{C}]+|(?! )[\s\p{Z}\p{C}]+", RegexOptions.Compiled);

        public static readonly Regex OuterWsRegex = new(@"^[\s\p{Z}\p{C}]+|[\s\p{Z}\p{C}]+$", RegexOptions.Compiled);

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

        public static bool TryGetAmbientValue<TEnum, TResult>(this TEnum value, out TResult result)
            where TEnum : struct, Enum
        {
            AmbientValueAttribute attribute = typeof(TEnum).GetField(Enum.GetName(value)).GetCustomAttribute<AmbientValueAttribute>();
            if (attribute is not null && attribute.Value is TResult r)
            {
                result = r;
                return false;
            }
            result = default;
            return false;
        }

        public static TResult GetAmbientValue<TEnum, TResult>(this TEnum value, TResult defaultValue = default)
            where TEnum : struct, Enum
        {
            AmbientValueAttribute attribute = typeof(TEnum).GetField(Enum.GetName(value)).GetCustomAttribute<AmbientValueAttribute>();
            if (attribute is not null && attribute.Value is TResult r)
                return r;
            return defaultValue;
        }

        public static IEnumerable<TEnum> GetFlagValues<TEnum>(this TEnum value)
            where TEnum : struct, Enum
        {
            Type type = typeof(TEnum);
            if (type.GetCustomAttribute<FlagsAttribute>() is null)
                return new[] { value };
            return Enum.GetValues<TEnum>().Where(v => value.HasFlag(v));
        }

        public static ISummaryProperties NullIfEmpty(this ISummaryProperties properties) => properties.IsNullOrEmpty() ? null : properties;

        public static bool IsNullOrEmpty(this ISummaryProperties properties) => properties is null || (string.IsNullOrWhiteSpace(properties.ApplicationName) &&
            string.IsNullOrWhiteSpace(properties.Comment) && string.IsNullOrWhiteSpace(properties.Subject) &&
            string.IsNullOrWhiteSpace(properties.Title) && string.IsNullOrWhiteSpace(properties.Company) &&
            string.IsNullOrWhiteSpace(properties.ContentType) && string.IsNullOrWhiteSpace(properties.Copyright) &&
            string.IsNullOrWhiteSpace(properties.ParentalRating) && string.IsNullOrWhiteSpace(properties.ItemType) &&
            string.IsNullOrWhiteSpace(properties.MIMEType) && string.IsNullOrWhiteSpace(properties.ItemTypeText) &&
            string.IsNullOrWhiteSpace(properties.ParentalRatingsOrganization) && string.IsNullOrWhiteSpace(properties.ParentalRatingReason) &&
            string.IsNullOrWhiteSpace(properties.SensitivityText) && string.IsNullOrWhiteSpace(properties.Trademarks) && string.IsNullOrWhiteSpace(properties.ProductName) &&
            !(properties.Rating.HasValue || properties.Sensitivity.HasValue || properties.SimpleRating.HasValue) &&
            properties.Author.ElementsNotNullOrWhiteSpace().NullIfEmpty() is null && properties.Keywords.ElementsNotNullOrWhiteSpace().NullIfEmpty() is null &&
            properties.ItemAuthors.ElementsNotNullOrWhiteSpace().NullIfEmpty() is null &&  properties.Kind.ElementsNotNullOrWhiteSpace().NullIfEmpty() is null);

        public static IAudioProperties NullIfEmpty(this IAudioProperties properties) => properties.IsNullOrEmpty() ? null : properties;

        public static bool IsNullOrEmpty(this IAudioProperties properties) => properties is null || (string.IsNullOrWhiteSpace(properties.Compression) &&
            string.IsNullOrWhiteSpace(properties.Format) && string.IsNullOrWhiteSpace(properties.StreamName) && !(properties.EncodingBitrate.HasValue ||
            properties.IsVariableBitrate.HasValue || properties.SampleRate.HasValue || properties.SampleSize.HasValue || properties.StreamNumber.HasValue));

        public static IDocumentProperties NullIfEmpty(this IDocumentProperties properties) => properties.IsNullOrEmpty() ? null : properties;

        public static bool IsNullOrEmpty(this IDocumentProperties properties) => properties is null || (string.IsNullOrWhiteSpace(properties.ClientID) &&
            string.IsNullOrWhiteSpace(properties.LastAuthor) && string.IsNullOrWhiteSpace(properties.RevisionNumber) &&
            string.IsNullOrWhiteSpace(properties.Division) && string.IsNullOrWhiteSpace(properties.DocumentID) && string.IsNullOrWhiteSpace(properties.Manager) &&
            string.IsNullOrWhiteSpace(properties.PresentationFormat) && string.IsNullOrWhiteSpace(properties.Version) && !(properties.DateCreated.HasValue ||
            properties.Security.HasValue) && properties.Contributor.ElementsNotNullOrWhiteSpace().NullIfEmpty() is null);

        public static IDRMProperties NullIfEmpty(this IDRMProperties properties) => properties.IsNullOrEmpty() ? null : properties;

        public static bool IsNullOrEmpty(this IDRMProperties properties) => properties is null || (string.IsNullOrWhiteSpace(properties.Description) &&
            !(properties.DatePlayExpires.HasValue || properties.DatePlayStarts.HasValue || properties.IsProtected.HasValue || properties.PlayCount.HasValue));

        public static IGPSProperties NullIfEmpty(this IGPSProperties properties) => properties.IsNullOrEmpty() ? null : properties;

        public static bool IsNullOrEmpty(this IGPSProperties properties) => properties is null || (string.IsNullOrWhiteSpace(properties.AreaInformation) &&
            string.IsNullOrWhiteSpace(properties.LatitudeRef) && string.IsNullOrWhiteSpace(properties.LongitudeRef) &&
            string.IsNullOrWhiteSpace(properties.MeasureMode) && string.IsNullOrWhiteSpace(properties.ProcessingMethod) &&
            !(properties.LatitudeDegrees.HasValue || properties.LatitudeMinutes.HasValue || properties.LatitudeSeconds.HasValue ||
            properties.LongitudeDegrees.HasValue || properties.LongitudeMinutes.HasValue || properties.LongitudeSeconds.HasValue) &&
            (properties.VersionID is null || properties.VersionID.Count == 0));

        public static IImageProperties NullIfEmpty(this IImageProperties properties) => properties.IsNullOrEmpty() ? null : properties;

        public static bool IsNullOrEmpty(this IImageProperties properties) => properties is null || (string.IsNullOrWhiteSpace(properties.CompressionText) &&
            string.IsNullOrWhiteSpace(properties.ImageID) && !(properties.BitDepth.HasValue || properties.ColorSpace.HasValue ||
            properties.CompressedBitsPerPixel.HasValue || properties.Compression.HasValue || properties.HorizontalResolution.HasValue ||
            properties.HorizontalSize.HasValue || properties.ResolutionUnit.HasValue || properties.VerticalResolution.HasValue || properties.VerticalSize.HasValue));

        public static IMediaProperties NullIfEmpty(this IMediaProperties properties) => properties.IsNullOrEmpty() ? null : properties;

        public static bool IsNullOrEmpty(this IMediaProperties properties) => properties is null || (string.IsNullOrWhiteSpace(properties.ContentDistributor) &&
            string.IsNullOrWhiteSpace(properties.CreatorApplication) && string.IsNullOrWhiteSpace(properties.CreatorApplicationVersion) &&
            string.IsNullOrWhiteSpace(properties.DateReleased) && string.IsNullOrWhiteSpace(properties.DVDID) && string.IsNullOrWhiteSpace(properties.ProtectionType) &&
            string.IsNullOrWhiteSpace(properties.ProviderRating) && string.IsNullOrWhiteSpace(properties.ProviderStyle) &&
            string.IsNullOrWhiteSpace(properties.Publisher) && string.IsNullOrWhiteSpace(properties.Subtitle) && !(properties.Duration.HasValue ||
            properties.FrameCount.HasValue || properties.Year.HasValue) && properties.Producer.ElementsNotNullOrWhiteSpace().NullIfEmpty() is null &&
            properties.Writer.ElementsNotNullOrWhiteSpace().NullIfEmpty() is null);

        public static IMusicProperties NullIfEmpty(this IMusicProperties properties) => properties.IsNullOrEmpty() ? null : properties;

        public static bool IsNullOrEmpty(this IMusicProperties properties) => properties is null || (string.IsNullOrWhiteSpace(properties.AlbumArtist) &&
            string.IsNullOrWhiteSpace(properties.AlbumTitle) && string.IsNullOrWhiteSpace(properties.DisplayArtist) && string.IsNullOrWhiteSpace(properties.PartOfSet) &&
            string.IsNullOrWhiteSpace(properties.Period) && !properties.TrackNumber.HasValue && properties.Artist.ElementsNotNullOrWhiteSpace().NullIfEmpty() is null &&
            properties.Composer.ElementsNotNullOrWhiteSpace().NullIfEmpty() is null && properties.Conductor.ElementsNotNullOrWhiteSpace().NullIfEmpty() is null &&
            properties.Genre.ElementsNotNullOrWhiteSpace().NullIfEmpty() is null);

        public static IPhotoProperties NullIfEmpty(this IPhotoProperties properties) => properties.IsNullOrEmpty() ? null : properties;

        public static bool IsNullOrEmpty(this IPhotoProperties properties) => properties is null || (string.IsNullOrWhiteSpace(properties.CameraManufacturer) &&
            string.IsNullOrWhiteSpace(properties.CameraModel) && string.IsNullOrWhiteSpace(properties.EXIFVersion) &&
            string.IsNullOrWhiteSpace(properties.OrientationText) && !(properties.DateTaken.HasValue || properties.Orientation.HasValue) &&
            properties.Event.ElementsNotNullOrWhiteSpace().NullIfEmpty() is null && properties.PeopleNames.ElementsNotNullOrWhiteSpace().NullIfEmpty() is null);

        public static IRecordedTVProperties NullIfEmpty(this IRecordedTVProperties properties) => properties.IsNullOrEmpty() ? null : properties;

        public static bool IsNullOrEmpty(this IRecordedTVProperties properties) => properties is null || (string.IsNullOrWhiteSpace(properties.EpisodeName) &&
            string.IsNullOrWhiteSpace(properties.NetworkAffiliation) && string.IsNullOrWhiteSpace(properties.ProgramDescription) &&
            string.IsNullOrWhiteSpace(properties.StationCallSign) && string.IsNullOrWhiteSpace(properties.StationName) &&
            !(properties.ChannelNumber.HasValue || properties.IsDTVContent.HasValue || properties.IsHDContent.HasValue || properties.OriginalBroadcastDate.HasValue));

        public static IVideoProperties NullIfEmpty(this IVideoProperties properties) => properties.IsNullOrEmpty() ? null : properties;

        public static bool IsNullOrEmpty(this IVideoProperties properties) => properties is null || (string.IsNullOrWhiteSpace(properties.Compression) &&
            string.IsNullOrWhiteSpace(properties.StreamName) && !(properties.EncodingBitrate.HasValue || properties.FrameHeight.HasValue ||
            properties.FrameRate.HasValue || properties.FrameWidth.HasValue || properties.HorizontalAspectRatio.HasValue || properties.StreamNumber.HasValue ||
            properties.VerticalAspectRatio.HasValue) && properties.Director.ElementsNotNullOrWhiteSpace().NullIfEmpty() is null);

        public static ErrorCode ToErrorCode(this AccessErrorCode errorCode) => errorCode.GetAmbientValue(ErrorCode.Unexpected);

        public static AccessErrorCode ToAccessErrorCode(this ErrorCode errorCode) =>
            Enum.GetValues<AccessErrorCode>().Where(e => e.ToErrorCode() == errorCode).DefaultIfEmpty(AccessErrorCode.Unspecified).First();

        public static EventId ToEventId(this AccessErrorCode errorCode) => errorCode.ToErrorCode().ToEventId();

        public static EventId ToEventId(this ErrorCode errorCode) =>
            new((byte)errorCode, errorCode.TryGetAmbientValue(out string name) ? name : Enum.GetName(errorCode));

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

        public static T DefaultIf<T>(T inputValue, Func<T, bool> predicate, T defaultValue) => predicate(inputValue) ? defaultValue : inputValue;


        public static TResult DefaultIf<TInput, TResult>(TInput inputValue, PredicatedProduction<TInput, TResult> producer, TResult defaultValue) =>
            producer(inputValue, out TResult result) ? result : defaultValue;

        public static T GetDefaultIf<T>(T inputValue, Func<T, bool> predicate, Func<T> defaultValueFunc) => predicate(inputValue) ? defaultValueFunc() : inputValue;

        public static TResult GetDefaultIf<TInput, TResult>(TInput inputValue, PredicatedProduction<TInput, TResult> producer, Func<TInput, TResult> defaultValueFunc) =>
            producer(inputValue, out TResult result) ? result : defaultValueFunc(inputValue);

        public static string AsWsNormalizedOrEmpty(this string text) => (text is null || text.Length == 0 || (text = OuterWsRegex.Replace(text, "")).Length == 0) ? "" :
            (AbnormalWsRegex.IsMatch(text) ? AbnormalWsRegex.Replace(text, " ") : text);

        public static string DefaultIfNullOrEmpty(this string text, string defaultValue) => DefaultIf(text, string.IsNullOrEmpty, defaultValue);

        public static string DefaultIfNullOrWhiteSpace(this string text, string defaultValue) => DefaultIf(text, string.IsNullOrWhiteSpace, defaultValue);

        public static string GetDefaultIfNullOrEmpty(this string text, Func<string> getDefaultValue) => GetDefaultIf(text, string.IsNullOrEmpty, getDefaultValue);

        public static string GetDefaultIfNullOrWhiteSpace(this string text, Func<string> getDefaultValue) =>
            GetDefaultIf(text, string.IsNullOrWhiteSpace, getDefaultValue);

        public static string AsNonNullTrimmed(this string text) => string.IsNullOrWhiteSpace(text) ? "" : text.Trim();

        public static string TrimmedOrNullIfEmpty(this string text) => string.IsNullOrEmpty(text) ? null : text.Trim();

        public static string TrimmedOrNullIfWhiteSpace(this string text) => (text is null || (text = text.Trim()).Length == 0) ? null : text;

        public static string[] SplitLines(this string text) => (text is null) ? Array.Empty<string>() : NewLineRegex.Split(text);

        public static string JoinWithNewLines(this IEnumerable<string> text) => (text is null || !text.Any()) ? null : string.Join(Environment.NewLine, text);

        public static IEnumerable<T> NullIfEmpty<T>(this IEnumerable<T> source) => (source is null || !source.Any()) ? null : source;

        public static IEnumerable<T> NonNullElements<T>(this IEnumerable<T> source) where T : class => source?.Where(s => s is not null);

        public static IEnumerable<string> ElementsNotNullOrEmpty(this IEnumerable<string> text) => text?.Where(s => !string.IsNullOrEmpty(s));

        public static IEnumerable<string> ElementsNotNullOrWhiteSpace(this IEnumerable<string> text) => text.Where(s => !string.IsNullOrWhiteSpace(s));

        public static bool AllNullOrWhiteSpace(this IEnumerable<string> text) => text is null || !text.Any() || text.All(string.IsNullOrWhiteSpace);

        public static IEnumerable<string> NonEmptyTrimmedElements(this IEnumerable<string> text) => text?.Where(s => s is not null).Select(s => s.Trim())
            .Where(s => s.Length > 0);

        public static IEnumerable<string> AsNonNullTrimmedValues(this IEnumerable<string> text) => text?.Select(AsNonNullTrimmed);

        public static IEnumerable<string> AsNonNullValues(this IEnumerable<string> text) => text?.Select(t => t ?? "");

        public static IEnumerable<string> AsOrderedDistinct(this IEnumerable<string> text) => text?.Select(t => t ?? "").Distinct().OrderBy(t => t);

        public static string EmptyIfNullOrWhiteSpace(this string source) => string.IsNullOrWhiteSpace(source) ? "" : source;

        public static IEnumerable<string> ValuesEmptyIfNullOrWhiteSpace(this IEnumerable<string> source) => source?.Select(EmptyIfNullOrWhiteSpace);

        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source) => (source is null) ? Array.Empty<T>() : source;

        public static IEnumerable<IndexedValue<T>> ToIndexValuePairs<T>(this IEnumerable<T> source) => source?.Select((e, i) => new IndexedValue<T>(i, e));

        public static IEnumerable<IndexedValue<TResult>> ToIndexValuePairs<TElement, TResult>(this IEnumerable<TElement> source, Func<TElement, TResult> transform)
        {
            if (transform is null)
                throw new ArgumentNullException(nameof(transform));
            return source?.Select((e, i) => new IndexedValue<TResult>(i, transform(e)));
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

    public record IndexedValue<T>(int Index, T Value);
}
