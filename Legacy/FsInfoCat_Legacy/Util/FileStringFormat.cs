using FsInfoCat.Models;
using System;

namespace FsInfoCat
{
    /// <summary>
    /// Represents the format of a string representing a file path.
    /// </summary>
    public enum FileStringFormat : byte
    {
        /// <summary>
        /// A relative filesystem path that is not well-formed, but can be used with the current filesystem type.
        /// </summary>
        RelativeLocalPath = ModelHelper.FILE_STRING_FORMAT_FLAG_NONE,

        /// <summary>
        /// A well-formed relative filesystem path that is compatible with the current filesystem type.
        /// </summary>
        WellFormedRelativeLocalPath = ModelHelper.FILE_STRING_FORMAT_FLAG_WELL_FORMED,

        /// <summary>
        /// A relative filesystem path that is not well-formed, but can be used with the filesystem type that is alternative to the current.
        /// </summary>
        RelativeAltPath = ModelHelper.FILE_STRING_FORMAT_FLAG_ALTERNATIVE,

        /// <summary>
        /// A well-formed relative filesystem path that is compatible with the filesystem type that is alternative to the current.
        /// </summary>
        WellFormedRelativeAltPath = ModelHelper.FILE_STRING_FORMAT_FLAG_ALTERNATIVE | ModelHelper.FILE_STRING_FORMAT_FLAG_WELL_FORMED,

        /// <summary>
        /// An absolute filesystem path that is not well-formed, but can be used with the current filesystem type.
        /// </summary>
        AbsoluteLocalPath = ModelHelper.FILE_STRING_FORMAT_FLAG_ABSOLUTE,

        /// <summary>
        /// A well-formed absolute filesystem path that is compatible with the current filesystem type.
        /// </summary>
        WellFormedAbsoluteLocalPath = ModelHelper.FILE_STRING_FORMAT_FLAG_ABSOLUTE | ModelHelper.FILE_STRING_FORMAT_FLAG_WELL_FORMED,

        /// <summary>
        /// An absolute filesystem path that is not well-formed, but can be used with the filesystem type that is alternative to the current.
        /// </summary>
        AbsoluteAltPath = ModelHelper.FILE_STRING_FORMAT_FLAG_ABSOLUTE | ModelHelper.FILE_STRING_FORMAT_FLAG_ALTERNATIVE,

        /// <summary>
        /// A well-formed absolute filesystem path that is compatible with the filesystem type that is alternative to the current.
        /// </summary>
        WellFormedAbsoluteAltPath = ModelHelper.FILE_STRING_FORMAT_FLAG_ABSOLUTE | ModelHelper.FILE_STRING_FORMAT_FLAG_ALTERNATIVE | ModelHelper.FILE_STRING_FORMAT_FLAG_WELL_FORMED,

        /// <summary>
        /// A relative URI path string that is not well-formed, but can be used to reference filesystem locations for the current filesystem type.
        /// </summary>
        RelativeLocalUri = ModelHelper.FILE_STRING_FORMAT_FLAG_FILE_URI,

        /// <summary>
        /// A well-formed relative URI-encoded path string that can be used as the <seealso cref="Uri.AbsolutePath">path</seealso> component of a <seealso cref="Uri.UriSchemeFile">file</seealso> URL,
        /// and is compatible with the current filesystem type.
        /// </summary>
        WellformedRelativeLocalUri = ModelHelper.FILE_STRING_FORMAT_FLAG_FILE_URI | ModelHelper.FILE_STRING_FORMAT_FLAG_WELL_FORMED,

        /// <summary>
        /// A relative URI path string that is not well-formed, but can be used to reference filesystem locations for the filesystem type that is alternative to the current.
        /// </summary>
        RelativeAltUri = ModelHelper.FILE_STRING_FORMAT_FLAG_FILE_URI | ModelHelper.FILE_STRING_FORMAT_FLAG_ALTERNATIVE,

        /// <summary>
        /// A well-formed relative URI-encoded path string that can be used as the <seealso cref="Uri.AbsolutePath">path</seealso> component of a <seealso cref="Uri.UriSchemeFile">file</seealso> URL,
        /// and is compatible with the filesystem type that is alternative to the current.
        /// </summary>
        WellformedRelativeAltUri = ModelHelper.FILE_STRING_FORMAT_FLAG_FILE_URI | ModelHelper.FILE_STRING_FORMAT_FLAG_ALTERNATIVE | ModelHelper.FILE_STRING_FORMAT_FLAG_WELL_FORMED,

        /// <summary>
        /// An absolute <seealso cref="Uri.UriSchemeFile">file</seealso> URL string that is not well-formed but can be used to reference absolute paths for the current filesystem type.
        /// </summary>
        AbsoluteLocalUrl = ModelHelper.FILE_STRING_FORMAT_FLAG_FILE_URI | ModelHelper.FILE_STRING_FORMAT_FLAG_ABSOLUTE,

        /// <summary>
        /// A well-formed absolute <seealso cref="Uri.UriSchemeFile">file</seealso> URL string that is compatible with the current filesystem type.
        /// </summary>
        WellformedAbsoluteLocalUrl = ModelHelper.FILE_STRING_FORMAT_FLAG_FILE_URI | ModelHelper.FILE_STRING_FORMAT_FLAG_ABSOLUTE | ModelHelper.FILE_STRING_FORMAT_FLAG_WELL_FORMED,

        /// <summary>
        /// An absolute <seealso cref="Uri.UriSchemeFile">file</seealso> URL string that is not well-formed but can be used to reference absolute paths for the filesystem type that is alternative to the current.
        /// </summary>
        AbsoluteAltUrl = ModelHelper.FILE_STRING_FORMAT_FLAG_FILE_URI | ModelHelper.FILE_STRING_FORMAT_FLAG_ALTERNATIVE | ModelHelper.FILE_STRING_FORMAT_FLAG_ABSOLUTE,

        /// <summary>
        /// A well-formed absolute <seealso cref="Uri.UriSchemeFile">file</seealso> URL string that is compatible with the filesystem type that is alternative to the current.
        /// </summary>
        WellformedAbsoluteAltUrl = ModelHelper.FILE_STRING_FORMAT_FLAG_FILE_URI | ModelHelper.FILE_STRING_FORMAT_FLAG_ALTERNATIVE | ModelHelper.FILE_STRING_FORMAT_FLAG_ABSOLUTE | ModelHelper.FILE_STRING_FORMAT_FLAG_WELL_FORMED,

        /// <summary>
        /// A <c>UNC</c> (Universal Naming Convention) path string that is not well-formed, but can be used to reference filesystem locations for the current filesystem type.
        /// </summary>
        LocalUnc = ModelHelper.FILE_STRING_FORMAT_FLAG_UNC | ModelHelper.FILE_STRING_FORMAT_FLAG_ABSOLUTE,

        /// <summary>
        /// A well-formemd <c>UNC</c> (Universal Naming Convention) path string that is compatible with the current filesystem type.
        /// </summary>
        WellFormedLocalUnc = ModelHelper.FILE_STRING_FORMAT_FLAG_UNC | ModelHelper.FILE_STRING_FORMAT_FLAG_WELL_FORMED | ModelHelper.FILE_STRING_FORMAT_FLAG_ABSOLUTE,

        /// <summary>>
        /// A <c>UNC</c> (Universal Naming Convention) path string that is not well-formed, but can be used to reference filesystem locations for the filesystem type that is alternative to the current.
        /// </summary>
        AltUnc = ModelHelper.FILE_STRING_FORMAT_FLAG_UNC | ModelHelper.FILE_STRING_FORMAT_FLAG_ALTERNATIVE | ModelHelper.FILE_STRING_FORMAT_FLAG_ABSOLUTE,

        /// <summary>
        /// A well-formemd <c>UNC</c> (Universal Naming Convention) path string that is compatible with the filesystem type that is alternative to the current.
        /// </summary>
        WellFormedAltUnc = ModelHelper.FILE_STRING_FORMAT_FLAG_UNC | ModelHelper.FILE_STRING_FORMAT_FLAG_ALTERNATIVE | ModelHelper.FILE_STRING_FORMAT_FLAG_WELL_FORMED | ModelHelper.FILE_STRING_FORMAT_FLAG_ABSOLUTE,

        /// <summary>
        /// A relative URI string that is not well-formed and cannot be used as the <seealso cref="Uri.AbsolutePath">path</seealso> component of a <seealso cref="Uri.UriSchemeFile">file</seealso> URL string.
        /// </summary>
        RelativeNonFileUri = ModelHelper.FILE_STRING_FORMAT_FLAG_NON_FILE,

        /// <summary>
        /// A well-formed relative URI string that cannot be used as the <seealso cref="Uri.AbsolutePath">path</seealso> component of a <seealso cref="Uri.UriSchemeFile">file</seealso> URL string.
        /// </summary>
        WellFormedRelativeNonFileUri = ModelHelper.FILE_STRING_FORMAT_FLAG_NON_FILE | ModelHelper.FILE_STRING_FORMAT_FLAG_WELL_FORMED,

        /// <summary>
        /// An absolute URI string that is neither well-formed nor specifies the <seealso cref="Uri.UriSchemeFile">file</seealso> <seealso cref="Uri.Scheme"/>.
        /// </summary>
        AbsoluteNonFileUri = ModelHelper.FILE_STRING_FORMAT_FLAG_NON_FILE | ModelHelper.FILE_STRING_FORMAT_FLAG_ABSOLUTE,

        /// <summary>
        /// A well-formed absolute URI string with a <seealso cref="Uri.Scheme"/> is not <seealso cref="Uri.UriSchemeFile">file</seealso> .
        /// </summary>
        WellFormedAbsoluteNonFileUri = ModelHelper.FILE_STRING_FORMAT_FLAG_NON_FILE | ModelHelper.FILE_STRING_FORMAT_FLAG_WELL_FORMED | ModelHelper.FILE_STRING_FORMAT_FLAG_ABSOLUTE,

        /// <summary>
        /// Contains invalid filesystem characters or sequences.
        /// </summary>
        Invalid = ModelHelper.FILE_STRING_FORMAT_FLAG_INVALID
    }
}
