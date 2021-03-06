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

        //public const string PATTERN_PATH_OR_URL = @"(?i)^([a-z]:[\\/]$|file:///[a-z]:/$|([a-z]:|[\\/]{2}([a-z]+(-[a-z\d]+)*(\.[a-z]+(-[a-z\d]+)*)*|(([01]\d?|[3-9])\d?|2(5[0-5]?|[0-4]?\d?)?)(\.(([01]\d?|[3-9])\d?|2(5[0-5]?|[0-4]?\d?)?)){3}))([\\/][^\\/:""<>|*?\x00-\x19]+)+[\\/]?$|file://(/[a-z]:|[a-z]+(-[a-z\d]+)*(\.[a-z]+(-[a-z\d]+)*)*|(([01]\d?|[3-9])\d?|2(5[0-5]?|[0-4]?\d?)?)(\.(([01]\d?|[3-9])\d?|2(5[0-5]?|[0-4]?\d?)?)){3})?([\\/][^\\/:""<>|*?\x00-\x19]+)+/?$)";

        public const string PATTERN_LOGIN_NAME_VALIDATION = @"(?i)^\s*([a-z][a-z\d_]*(\.[a-z][a-z\d_]*)*)\s*$";
        public static readonly Regex LOGIN_NAME_VALIDATION_REGEX = new Regex(PATTERN_LOGIN_NAME_VALIDATION, RegexOptions.Compiled);

        public const string PATTERN_MACHINE_NAME = @"(?i)^\s*((?=((2(5[0-5]?|[0-4]?\d?)?|[01]?\d\d?)(\.|(?![.\d]))){4})\d+(\.\d+){3}(?![.\d])|(?=\[[a-f\d:]+\]|[a-f\d]*:)\[?(:(:[a-f\d]{1,4}){1,7}|(?=([a-f\d]+:)+((:[a-f\d]+)+|:|[a-f\d]+))([a-f\d]{0,4}:){1,7}[a-f\d]{0,4})\]?|(?=\S{1,255}\s*$)(?=[\d.]*[a-z_-])[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?)\s*$";
        public static readonly Regex MACHINE_NAME_REGEX = new Regex(PATTERN_MACHINE_NAME, RegexOptions.Compiled);

        public const string PATTERN_BASE64 = @"^\s*(([A-Za-z\d+/])\s*)?$";
        public static readonly Regex Base64Regex = new Regex(PATTERN_BASE64, RegexOptions.Compiled);

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
                throw new ArgumentNullException("target");
            if (modifiedBy is null)
                throw new ArgumentNullException("modifiedBy");
            target.ModifiedOn = DateTime.Now;
            target.ModifiedBy = modifiedBy.AccountID;
            if (isCreate)
            {
                target.CreatedOn = target.ModifiedOn;
                target.CreatedBy = target.ModifiedBy;
            }
            return target.ValidateAll();
        }

        /// <summary>
        /// Finds the <seealso cref="IVolumeInfo"/> that contains the specified <seealso cref="DirectoryInfo"/>.
        /// </summary>
        /// <typeparam name="T">The <seealso cref="IVolumeInfo"/> type.</typeparam>
        /// <param name="volumes">The collection of <seealso cref="IVolumeInfo"/> objects.</param>
        /// <param name="directoryInfo">The target <seealso cref="DirectoryInfo"/>.</param>
        /// <param name="result">The <seealso cref="IVolumeInfo"/> of type <typeparamref name="T"/> to which <paramref name="directoryInfo"/> or <see langword="null"/>
        /// if no match was found.</param>
        /// <returns><see langword="true"/> if <paramref name="directoryInfo"/> belongs to one of the items in <paramref name="volumes"/>;
        /// otherwise, <see langword="false"/>.</returns>
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

        public static T FindByChildItem<T>(this IEnumerable<T> source, FileUri fileUri)
            where T : class, IVolumeInfo
        {
            if (source is null || fileUri is null)
                return null;

            // Compare name. If matches, go up to parent volume and compare there
            throw new NotImplementedException();
        }

        public static IEnumerable<T> FindByIdentifier<T>(this IEnumerable<T> source, VolumeIdentifier volumeIdentifier)
            where T : class, IVolumeInfo
        {
            if (source is null)
                return new T[0];
            return source.Where(v => !(v is null) && v.Identifier.Equals(volumeIdentifier));
        }

        public static IEnumerable<T> FindByRootUri<T>(this IEnumerable<T> source, FileUri fileUri)
            where T : class, IVolumeInfo
        {
            if (source is null || fileUri is null)
                return new T[0];
            return source.Where(v => !(v is null) && fileUri.Equals(v.RootUri, v.PathComparer));
        }

        public static IEnumerable<T> FindByVolumeName<T>(this IEnumerable<T> source, string volumeName)
            where T : class, IVolumeInfo
        {
            if (source is null)
                return new T[0];
            if (string.IsNullOrEmpty(volumeName))
                return source.Where(v => !(v is null) && string.IsNullOrEmpty(v.VolumeName));
            StringComparer comparer = StringComparer.InvariantCultureIgnoreCase;
            return source.Where(v => !(v is null) && comparer.Equals(volumeName, v.VolumeName));
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
