using FsInfoCat.Models.DB;
using FsInfoCat.Models.Volumes;
using FsInfoCat.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace FsInfoCat.Models
{
    public static class ModelHelper
    {
        /// <summary>
        /// Zero flag byte value for <seealso cref="FileStringFormat"/> enumeration values.
        /// </summary>
        public const byte FILE_STRING_FORMAT_FLAG_NONE =        0b000_0000;

        /// <summary>
        /// Flag bit value for <seealso cref="FileStringFormat"/> enumeration values representing well-formed strings.
        /// </summary>
        public const byte FILE_STRING_FORMAT_FLAG_WELL_FORMED = 0b000_0001;

        /// <summary>
        /// Flag bit value for <seealso cref="FileStringFormat"/> enumeration values representing strings that are compatible with the filesystem type that is alternative to the current.
        /// </summary>
        public const byte FILE_STRING_FORMAT_FLAG_ALTERNATIVE = 0b000_0010;

        /// <summary>
        /// Flag bit value for <seealso cref="FileStringFormat"/> enumeration values representing absolute path/URI references.
        /// </summary>
        public const byte FILE_STRING_FORMAT_FLAG_ABSOLUTE =    0b000_0100;

        /// <summary>
        /// Flag bit value for <seealso cref="FileStringFormat"/> enumeration values representing absolute <seealso cref="Uri.UriSchemeFile">file</seealso> URLs and relative URIs that can be used as the
        /// <seealso cref="Uri.AbsolutePath">path</seealso> component of a <seealso cref="Uri.UriSchemeFile">file</seealso> URL.
        /// </summary>
        public const byte FILE_STRING_FORMAT_FLAG_FILE_URI =    0b000_1000;

        /// <summary>
        /// Flag bit value for <seealso cref="FileStringFormat"/> enumeration values representing <c>UNC</c> (Universal Naming Convention) path strings.
        /// </summary>
        public const byte FILE_STRING_FORMAT_FLAG_UNC =         0b001_0000;

        /// <summary>
        /// Flag bit value for <seealso cref="FileStringFormat"/> enumeration values representing non-file URI strings.
        /// </summary>
        public const byte FILE_STRING_FORMAT_FLAG_NON_FILE =    0b010_0000;

        /// <summary>
        /// Flag bit value for <seealso cref="FileStringFormat"/> enumeration values representing strings that contain characters or character sequences which cannot be used to reference file system locations.
        /// </summary>
        public const byte FILE_STRING_FORMAT_FLAG_INVALID =     0b100_0000;
        private const byte FILE_STRING_FORMAT_FLAGS_NON_FS_PATH = FILE_STRING_FORMAT_FLAG_FILE_URI | FILE_STRING_FORMAT_FLAG_UNC | FILE_STRING_FORMAT_FLAG_NON_FILE | FILE_STRING_FORMAT_FLAG_INVALID;
        private const byte FILE_STRING_FORMAT_FLAGS_WELL_FORMED_ABSOLUTE = FILE_STRING_FORMAT_FLAG_ABSOLUTE | FILE_STRING_FORMAT_FLAG_WELL_FORMED;
        private const byte FILE_STRING_FORMAT_FLAGS_URI = FILE_STRING_FORMAT_FLAG_FILE_URI | FILE_STRING_FORMAT_FLAG_NON_FILE;

        /// <summary>
        /// Name of role for users that have view rights.
        /// </summary>
        public const string ROLE_NAME_VIEWER = "viewer";

        /// <summary>
        /// Name of role for users that have contributor rights.
        /// </summary>
        public const string ROLE_NAME_CONTRIBUTOR = "contributor";

        /// <summary>
        /// Name of role for users that have elevated content administration rights.
        /// </summary>
        public const string ROLE_NAME_CONTENT_ADMIN = "content-admin";

        /// <summary>
        /// Name of role for users that have overall application administration rights.
        /// </summary>
        public const string ROLE_NAME_APP_ADMIN = "app-admin";

        /// <summary>
        /// Regular expression pattern for user login name validation.
        /// </summary>
        public const string PATTERN_LOGIN_NAME_VALIDATION = @"(?i)^\s*([a-z][a-z\d_]*(\.[a-z][a-z\d_]*)*)\s*$";

        /// <summary>
        /// Regular expression for validating user login names.
        /// </summary>
        public static readonly Regex LOGIN_NAME_VALIDATION_REGEX = new Regex(@"^([a-z][a-z\d_]*(\.[a-z][a-z\d_]*)*)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Regular expression pattern for base64-encoded text validation.
        /// </summary>
        public const string PATTERN_BASE64 = @"^\s*(?i)(([a-z\d+/]{2}([a-z\d+/]([a-z\d+/]|=)|==))\s*)+$";

        /// <summary>
        /// Regular expression for validating ubase64-encoded text.
        /// </summary>
        public static readonly Regex Base64Regex = new Regex(@"^(([a-z\d+/]{2}([a-z\d+/]([a-z\d+/]|=)|==))\s*)+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static bool IsAbsolute(this FileStringFormat format) => ((byte)format & FILE_STRING_FORMAT_FLAG_ABSOLUTE) != 0;

        public static bool IsWellFormed(this FileStringFormat format) => ((byte)format & FILE_STRING_FORMAT_FLAG_WELL_FORMED) != 0;

        public static bool IsWellFormedAbsolute(this FileStringFormat format) => ((byte)format & FILE_STRING_FORMAT_FLAGS_WELL_FORMED_ABSOLUTE) == FILE_STRING_FORMAT_FLAGS_WELL_FORMED_ABSOLUTE;

        public static bool IsAlternative(this FileStringFormat format) => ((byte)format & FILE_STRING_FORMAT_FLAG_ALTERNATIVE) != 0;

        public static bool IsFileURI(this FileStringFormat format) => ((byte)format & FILE_STRING_FORMAT_FLAG_FILE_URI) != 0;

        public static bool IsNonFileURI(this FileStringFormat format) => ((byte)format & FILE_STRING_FORMAT_FLAG_NON_FILE) != 0;

        public static bool IsFileSystemPath(this FileStringFormat format) => ((byte)format & FILE_STRING_FORMAT_FLAGS_NON_FS_PATH) == 0;

        public static bool IsUNC(this FileStringFormat format) => ((byte)format & FILE_STRING_FORMAT_FLAG_UNC) != 0;

        public static bool IsURI(this FileStringFormat format) => ((byte)format & FILE_STRING_FORMAT_FLAGS_URI) != 0;

        public static bool IsUnixLike(this PlatformType platform)
        {
            switch (platform)
            {
                case PlatformType.Windows:
                case PlatformType.Xbox:
                    return false;
                default:
                    return true;
            }
        }

        /// <summary>
        /// Validates an <seealso cref="IModficationAuditable"/>.
        /// </summary>
        /// <param name="target">The <seealso cref="IModficationAuditable"/> to validate.</param>
        /// <param name="modifiedBy">The <seealso cref="IModficationAuditable.ModifiedBy"/> to apply to the <paramref name="target"/> <seealso cref="IModficationAuditable"/>.</param>
        /// <param name="isCreate">Set to <see langword="true"/> if this is being validated for a database insert operation.</param>
        /// <returns>An <seealso cref="IList{T}">IList</seealso><c>&lt;<see cref="ValidationResult"/>&gt;</c> that contains the property validation results for
        /// the <paramref name="target"/> <seealso cref="IModficationAuditable"/></returns>
        public static IList<ValidationResult> ValidateForSave(this IModficationAuditable target, Account modifiedBy, bool isCreate)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (modifiedBy is null)
                throw new ArgumentNullException(nameof(modifiedBy));
            target.ModifiedOn = DateTime.Now;
            target.ModifiedBy = modifiedBy.AccountID;
            if (isCreate)
            {
                target.CreatedOn = target.ModifiedOn;
                target.CreatedBy = target.ModifiedBy;
            }
            return target.ValidateAll();
        }

        [Obsolete("Use IVolumeSetProvider, instead")]
        public static bool TryFindVolume<T>(this IEnumerable<T> volumes, DirectoryInfo directoryInfo, out T result)
            where T : class, IVolumeInfo
        {
            if (volumes is null || directoryInfo is null)
            {
                result = null;
                return false;
            }
            FileUri fileUri = new FileUri(directoryInfo);
            result = volumes.FirstOrDefault(v => v.RootUri.Equals(fileUri));
            if (result is null)
                return TryFindVolume(volumes, directoryInfo.Parent, out result);
            return true;
        }

        [Obsolete("Use IVolumeSetProvider, instead")]
        public static bool TryFindVolume<TSource, TItem>(this IEnumerable<TSource> volumes, Func<TSource, TItem> mapper, DirectoryInfo directoryInfo, out TSource result)
            where TItem : class, IVolumeInfo
            where TSource : class
        {
            if (volumes is null || directoryInfo is null)
            {
                result = null;
                return false;
            }
            FileUri fileUri = new FileUri(directoryInfo);
            result = volumes.FirstOrDefault(v => mapper(v).RootUri.Equals(fileUri));
            if (result is null)
                return TryFindVolume(volumes, mapper, directoryInfo.Parent, out result);
            return true;
        }

        [Obsolete("Use IVolumeSetProvider, instead")]
        public static T FindByChildItem<T>(this IEnumerable<T> source, FileUri fileUri)
            where T : class, IVolumeInfo
        {
            if (source is null || fileUri is null)
                return null;

            Stack<FileUri> stack = new Stack<FileUri>();
            stack.Push(fileUri);
            while (!((fileUri = fileUri.Parent) is null))
                stack.Push(fileUri);

            while (!(fileUri is null))
            {
                T result = source.FirstOrDefault(v => v.RootUri.Equals(fileUri));
                if (result is null)
                    fileUri = fileUri.Parent;
                else
                {
                    for (FileUri baseUri = result.RootUri.Parent; !(baseUri is null); baseUri = baseUri.Parent)
                    {
                        if ((fileUri = fileUri.Parent) is null)
                            return result;
                        T b1 = FindByChildItem(source, baseUri);
                        T b2 = FindByChildItem(source, fileUri);
                        if (b2 is null)
                        {
                            if (b1 is null)
                                return result;

                        }
                    }
                }
            }
            // TODO: Compare name. If matches, go up to parent volume and compare there
            throw new NotImplementedException();
        }

        [Obsolete("Use IVolumeSetProvider, instead")]
        public static TSource FindByChildItem<TSource, TItem>(this IEnumerable<TSource> source, Func<TSource, TItem> mapper, FileUri fileUri)
            where TItem : class, IVolumeInfo
            where TSource : class
        {
            if (source is null || fileUri is null)
                return null;

            // TODO: Compare name. If matches, go up to parent volume and compare there
            throw new NotImplementedException();
        }

        [Obsolete("Use IVolumeSetProvider, instead")]
        public static IEnumerable<T> FindByIdentifier<T>(this IEnumerable<T> source, VolumeIdentifier volumeIdentifier)
            where T : class, IVolumeInfo
        {
            if (source is null)
                return Array.Empty<T>();
            return source.Where(v => !(v is null) && v.Identifier.Equals(volumeIdentifier));
        }

        [Obsolete("Use IVolumeSetProvider, instead")]
        public static IEnumerable<TSource> FindByIdentifier<TSource, TItem>(this IEnumerable<TSource> source, Func<TSource, TItem> mapper, VolumeIdentifier volumeIdentifier)
            where TItem : class, IVolumeInfo
            where TSource : class
        {
            if (source is null)
                return Array.Empty<TSource>();
            return source.Where(v => !(v is null) && mapper(v).Identifier.Equals(volumeIdentifier));
        }

        [Obsolete("Use IVolumeSetProvider, instead")]
        public static IEnumerable<T> FindByRootUri<T>(this IEnumerable<T> source, FileUri fileUri)
            where T : class, IVolumeInfo
        {
            if (source is null || fileUri is null)
                return Array.Empty<T>();
            return source.Where(v => !(v is null) && fileUri.Equals(v.RootUri, v.GetPathComparer()));
        }

        [Obsolete("Use IVolumeSetProvider, instead")]
        public static IEnumerable<TSource> FindByRootUri<TSource, TItem>(this IEnumerable<TSource> source, Func<TSource, TItem> mapper, FileUri fileUri)
            where TItem : class, IVolumeInfo
            where TSource : class
        {
            if (source is null || fileUri is null)
                return Array.Empty<TSource>();
            return source.Where(s =>
            {
                if (s is null)
                    return false;
                TItem v = mapper(s);
                return fileUri.Equals(v.RootUri, v.GetPathComparer());
            });
        }

        [Obsolete("Use IVolumeSetProvider, instead")]
        public static IEnumerable<T> FindByVolumeName<T>(this IEnumerable<T> source, string volumeName)
            where T : class, IVolumeInfo
        {
            if (source is null)
                return Array.Empty<T>();
            if (string.IsNullOrEmpty(volumeName))
                return source.Where(v => !(v is null) && string.IsNullOrEmpty(v.VolumeName));
            StringComparer comparer = StringComparer.InvariantCultureIgnoreCase;
            return source.Where(v => !(v is null) && comparer.Equals(volumeName, v.VolumeName));
        }

        [Obsolete("Use IVolumeSetProvider, instead")]
        public static IEnumerable<TSource> FindByVolumeName<TSource, TItem>(this IEnumerable<TSource> source, Func<TSource, TItem> mapper, string volumeName)
            where TItem : class, IVolumeInfo
            where TSource : class
        {
            if (source is null)
                return Array.Empty<TSource>();
            if (string.IsNullOrEmpty(volumeName))
                return source.Where(v => !(v is null) && string.IsNullOrEmpty(mapper(v).VolumeName));
            StringComparer comparer = StringComparer.InvariantCultureIgnoreCase;
            return source.Where(v => !(v is null) && comparer.Equals(volumeName, mapper(v).VolumeName));
        }

        public static PlatformType GetPlatformType(this HostDevices.IHostDevice host) => (host is null) ? FileUriConverter.CURRENT_FACTORY.FsPlatform : host.Platform;

        public static PlatformType GetPlatformType(this IVolume volume) => (volume?.Host).GetPlatformType();

        public static Accounts.UserRole ToUserRole(byte value)
        {
            return Enum.GetValues(typeof(Accounts.UserRole)).Cast<Accounts.UserRole>().Where(t => t.Equals(value)).FirstOrDefault();
        }

        public static PlatformType ToPlatformType(byte value)
        {
            return Enum.GetValues(typeof(PlatformType)).Cast<PlatformType>().Where(t => t.Equals(value)).FirstOrDefault();
        }
    }
}
