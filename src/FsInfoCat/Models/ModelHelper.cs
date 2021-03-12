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
        public const string ROLE_NAME_VIEWER = "viewer";
        public const string ROLE_NAME_USER = "user";
        public const string ROLE_NAME_APP_CONTRIB = "app-contrib";
        public const string ROLE_NAME_ADMIN = "admin";

        public const string PATTERN_LOGIN_NAME_VALIDATION = @"(?i)^\s*([a-z][a-z\d_]*(\.[a-z][a-z\d_]*)*)\s*$";
        public static readonly Regex LOGIN_NAME_VALIDATION_REGEX = new Regex(PATTERN_LOGIN_NAME_VALIDATION, RegexOptions.Compiled);

        public const string PATTERN_BASE64 = @"^\s*(?i)(([A-Za-z\d+/]{4})\s*)?$";
        public static readonly Regex Base64Regex = new Regex(@"^\s*(?i)(([A-Za-z\d+/]{2}([A-Za-z\d+/]([A-Za-z\d+/]|=)|==))\s*)+$", RegexOptions.Compiled);

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
