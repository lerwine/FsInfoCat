using FsInfoCat.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace FsInfoCat
{
    /// <summary>Values for application error codes.</summary>
    public enum ErrorCode : int
    {
        /// <summary>An unexpected error has occurred.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_UnexpectedError), Description = nameof(Properties.Resources.ErrorMessage_UnexpectedError), ResourceType = typeof(Properties.Resources))]
        Unexpected = 0,

        /// <summary>An <see cref="System.IO.IOException" /> has occurred.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_IOError), Description = nameof(Properties.Resources.ErrorMessage_IOError), ResourceType = typeof(Properties.Resources))]
        IOError = 1,

        /// <summary>An <see cref="System.IO.UnauthorizedAccessException" /> has occurred.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_UnauthorizedAccessError), Description = nameof(Properties.Resources.DisplayName_UnauthorizedAccessError), ResourceType = typeof(Properties.Resources))]
        UnauthorizedAccess = 2,

        /// <summary>An <see cref="System.IO.SecurityException" /> has occurred.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_SecurityException), Description = nameof(Properties.Resources.ErrorMessage_SecurityException), ResourceType = typeof(Properties.Resources))]
        SecurityException = 3,

        /// <summary>The name of one or more files or subdirectories in the path contain a invalid character(s).</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_InvalidPathError), Description = nameof(Properties.Resources.ErrorMessage_InvalidPathError), ResourceType = typeof(Properties.Resources))]
        InvalidPath = 4,

        /// <summary>A <see cref="System.IO.PathTooLongException" /> has occurred, indicating that hhe specified path, file name, or both exceed the system-defined maximum length.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_PathTooLongError), Description = nameof(Properties.Resources.ErrorMessage_PathTooLongError), ResourceType = typeof(Properties.Resources))]
        PathTooLong = 5,

        /// <summary>Crawl operation has failed and terminated before completion.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_CrawlOperationFailed), Description = nameof(Properties.Resources.ErrorMessage_CrawlOperationFailed), ResourceType = typeof(Properties.Resources))]
        CrawlOperationFailed = 6
    }

    /// <summary>Represents file system access error codes.</summary>
    public enum AccessErrorCode : byte
    {
        /// <summary>An unknown/unspecified error has occurred.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_UnexpectedError), ResourceType = typeof(Properties.Resources))]
        Unspecified = 0,

        /// <summary>An <see cref="System.IO.IOException" /> has occurred.</summary>
        [AmbientValue(ErrorCode.IOError)]
        [Display(Name = nameof(Properties.Resources.DisplayName_IOError), ResourceType = typeof(Properties.Resources))]
        IOError = 1,

        /// <summary>An <see cref="System.UnauthorizedAccessException" /> has occurred.</summary>
        [AmbientValue(ErrorCode.UnauthorizedAccess)]
        [Display(Name = nameof(Properties.Resources.DisplayName_UnauthorizedAccessError), ResourceType = typeof(Properties.Resources))]
        UnauthorizedAccess = 2,

        /// <summary>A <see cref="System.Security.SecurityException" /> has occurred.</summary>
        [AmbientValue(ErrorCode.SecurityException)]
        [Display(Name = nameof(Properties.Resources.DisplayName_SecurityException), ResourceType = typeof(Properties.Resources))]
        SecurityException = 3,

        /// <summary>The name of one or more files or subdirectories in the path contain a invalid character(s).</summary>
        [AmbientValue(ErrorCode.InvalidPath)]
        [Display(Name = nameof(Properties.Resources.DisplayName_InvalidPathError), ResourceType = typeof(Properties.Resources))]
        InvalidPath = 4,

        /// <summary>A <see cref="System.IO.PathTooLongException" /> has occurred, indicating that hhe specified path, file name, or both exceed the system-defined maximum length.</summary>
        [AmbientValue(ErrorCode.PathTooLong)]
        [Display(Name = nameof(Properties.Resources.DisplayName_PathTooLongError), ResourceType = typeof(Properties.Resources))]
        PathTooLong = 5
    }

    /// <summary>Represents a volume status.</summary>
    public enum VolumeStatus : byte
    {
        /// <summary>The status of the volume is uknown or unspecified.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_VolumeStatus_Unknown), Description = nameof(Properties.Resources.Description_VolumeStatus_Unknown), ResourceType = typeof(Properties.Resources))]
        Unknown = 0,

        /// <summary>Volume is under corporate control / ownership.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_VolumeStatus_Controlled), Description = nameof(Properties.Resources.Description_VolumeStatus_Controlled), ResourceType = typeof(Properties.Resources))]
        Controlled = 1,

        /// <summary>An error occurred while trying to access the volume.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_AccessError), Description = nameof(Properties.Resources.Description_VolumeStatus_AccessError), ResourceType = typeof(Properties.Resources))]
        AccessError = 2,

        /// <summary>Volume is being tracked, but is not under corporate control / ownership.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_VolumeStatus_Uncontrolled), Description = nameof(Properties.Resources.Description_VolumeStatus_Uncontrolled), ResourceType = typeof(Properties.Resources))]
        Uncontrolled = 3,

        /// <summary>Volume is offline or unavailable.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_VolumeStatus_Offline), Description = nameof(Properties.Resources.Description_VolumeStatus_Offline), ResourceType = typeof(Properties.Resources))]
        Offline = 4,

        /// <summary>Ownership / control of volume has been relinquished to another entity.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_VolumeStatus_Relinquished), Description = nameof(Properties.Resources.Description_VolumeStatus_Relinquished), ResourceType = typeof(Properties.Resources))]
        Relinquished = 5,

        /// <summary>Volume has been destroyed.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_VolumeStatus_Destroyed), Description = nameof(Properties.Resources.Description_VolumeStatus_Destroyed), ResourceType = typeof(Properties.Resources))]
        Destroyed = 6
    }

    /// <summary>Directory-specific crawl option flags.</summary>
    [Flags]
    public enum DirectoryCrawlOptions : byte
    {
        /// <summary>No options selected.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_NoOptionsSelected), ResourceType = typeof(Properties.Resources))]
        None = 0,

        /// <summary>Do not crawl nested <see cref="ISubdirectory">sub-directories</see>.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_DirectoryCrawlOptions_SkipSubdirectories), ResourceType = typeof(Properties.Resources))]
        SkipSubdirectories = 1,

        /// <summary>Do not crawl the current <see cref="ISubdirectory" />.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_DirectoryCrawlOptions_Skip), ResourceType = typeof(Properties.Resources))]
        Skip = 2,

        /// <summary>Do not compare files for redundancies.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_DirectoryCrawlOptions_DoNotCompareFiles), ResourceType = typeof(Properties.Resources))]
        DoNotCompareFiles = 4,

        /// <summary>Do not show <see cref="ISubdirectory" /> in normal result displays.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_DirectoryCrawlOptions_DoNotShow), ResourceType = typeof(Properties.Resources))]
        DoNotShow = 8,

        /// <summary>Mark <see cref="ISubdirectory" /> for deletion.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_FlaggedForDeletion), ResourceType = typeof(Properties.Resources))]
        FlaggedForDeletion = 16,

        /// <summary>Mark <see cref="ISubdirectory" /> for re-scan.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_FlaggedForRescan), ResourceType = typeof(Properties.Resources))]
        FlaggedForRescan = 32
    }

    /// <summary>Indicates crawl status for the a <see cref="ISubdirectory" /></summary>
    public enum DirectoryStatus : byte
    {
        /// <summary>Not all items in the current <see cref="ISubdirectory" /> have been crawled.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_DirectoryStatus_Incomplete), ResourceType = typeof(Properties.Resources))]
        Incomplete = 0,

        /// <summary>All items in the current <see cref="ISubdirectory" /> have been crawled.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_DirectoryStatus_Complete), ResourceType = typeof(Properties.Resources))]
        Complete = 1,

        /// <summary>An error occurred while trying to enumerate items in the current <see cref="ISubdirectory" />.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_AccessError), ResourceType = typeof(Properties.Resources))]
        AccessError = 2,

        /// <summary>The current <see cref="ISubdirectory" /> has been deleted.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_Deleted), ResourceType = typeof(Properties.Resources))]
        Deleted = 3
    }

    /// <summary>File-specific crawl options.</summary>
    [Flags]
    public enum FileCrawlOptions : byte
    {
        /// <summary>No options selected.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_NoOptionsSelected), ResourceType = typeof(Properties.Resources))]
        None = 0,

        /// <summary>Do not compare the current <see cref="IFile" /> for redundancies.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_FileCrawlOptions_DoNotCompare), ResourceType = typeof(Properties.Resources))]
        DoNotCompare = 1,

        /// <summary>Ignore the current <see cref="IFile" />.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_FileCrawlOptions_Ignore), ResourceType = typeof(Properties.Resources))]
        Ignore = 2,

        /// <summary>Mark <see cref="IFile" /> for deletion.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_FlaggedForDeletion), ResourceType = typeof(Properties.Resources))]
        FlaggedForDeletion = 4,

        /// <summary>Mark <see cref="IFile" /> for re-scan.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_FlaggedForRescan), ResourceType = typeof(Properties.Resources))]
        FlaggedForRescan = 8
    }

    /// <summary></summary>
    public enum FileCorrelationStatus : byte
    {
        /// <summary>File has been added to the database or modified and needs to be analyzed.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_FileCorrelationStatus_Dissociated), Description = nameof(Properties.Resources.Description_FileCorrelationStatus_Dissociated), ResourceType = typeof(Properties.Resources))]
        Dissociated = 0,

        /// <summary>Acknowledged; Validation in progress.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_FileCorrelationStatus_PendingValidation), Description = nameof(Properties.Resources.Description_FileCorrelationStatus_PendingValidation), ResourceType = typeof(Properties.Resources))]
        PendingValidation = 1,

        /// <summary>Final determination deferred.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_FileCorrelationStatus_Deferred), Description = nameof(Properties.Resources.Description_FileCorrelationStatus_Deferred), ResourceType = typeof(Properties.Resources))]
        Deferred = 2,

        /// <summary>Correlations to other files are being established.</summary>
        /// <remarks>
        /// This status value is only for new files and those which have changed. Results of comparisons will be stored in the <see cref="IComparison" /> table, where the <see cref="IComparison.Correlative" />
        /// is the new or changed file, and the <see cref="IComparison.Baseline" /> is the file that it is being compared to.
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_FileCorrelationStatus_Analyzing), Description = nameof(Properties.Resources.Description_FileCorrelationStatus_Analyzing), ResourceType = typeof(Properties.Resources))]
        Analyzing = 3,

        /// <summary>File has been compared with all other files with the same Hash. Final disposition pending.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_FileCorrelationStatus_Correlated), Description = nameof(Properties.Resources.Description_FileCorrelationStatus_Correlated), ResourceType = typeof(Properties.Resources))]
        Correlated = 4,

        /// <summary>File duplicaton is justified.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_FileCorrelationStatus_Justified), Description = nameof(Properties.Resources.Description_FileCorrelationStatus_Justified), ResourceType = typeof(Properties.Resources))]
        Justified = 5,

        /// <summary>Indicates that the files should be deleted.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_FileCorrelationStatus_Insupportable), Description = nameof(Properties.Resources.Description_FileCorrelationStatus_Insupportable), ResourceType = typeof(Properties.Resources))]
        Insupportable = 6,

        /// <summary>Elevated status for unauthorized copies of files that need further attention.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_FileCorrelationStatus_Violation), Description = nameof(Properties.Resources.Description_FileCorrelationStatus_Violation), ResourceType = typeof(Properties.Resources))]
        Violation = 7,

        /// <summary>Indicates that the file has been scheduled/tasked to be deleted, but not yet confirmed.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_FileCorrelationStatus_Attrition), Description = nameof(Properties.Resources.Description_FileCorrelationStatus_Attrition), ResourceType = typeof(Properties.Resources))]
        Attrition = 8,

        /// <summary>The file has been deleted.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_Deleted), Description = nameof(Properties.Resources.Description_FileCorrelationStatus_FileDeleted), ResourceType = typeof(Properties.Resources))]
        Deleted = 9
    }

    /// <summary>Values to represent the file crawl status.</summary>
    public enum CrawlStatus : byte
    {
        /// <summary>File system crawl is not running.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_CrawlStatus_NotRunning), ResourceType = typeof(Properties.Resources))]
        NotRunning = 0,

        /// <summary>File system crawl is in progress.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_CrawlStatus_InProgress), ResourceType = typeof(Properties.Resources))]
        InProgress = 1,

        /// <summary>File system crawl ranto completion.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_CrawlStatus_Completed), ResourceType = typeof(Properties.Resources))]
        Completed = 2,

        /// <summary>File system was stopped prior to completion because the alotted execution duration had been reached.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_CrawlStatus_AllottedTimeElapsed), ResourceType = typeof(Properties.Resources))]
        AllottedTimeElapsed = 3,

        /// <summary>File system was stopped prior to completion because the maximum number of items had been processed.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_CrawlStatus_MaxItemCountReached), ResourceType = typeof(Properties.Resources))]
        MaxItemCountReached = 4,

        /// <summary>File system crawl was manually canceled before completion.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_CrawlStatus_Canceled), ResourceType = typeof(Properties.Resources))]
        Canceled = 5,

        /// <summary>File system crawl was aborted due to a failure.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_CrawlStatus_Failed), ResourceType = typeof(Properties.Resources))]
        Failed = 6,

        /// <summary>File system crawl configuration is disabled.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_CrawlStatus_Disabled), ResourceType = typeof(Properties.Resources))]
        Disabled = 7
    }

    /// <summary>
    /// Base interface for all database entity objects which track the creation and modification dates as well as implementing the
    /// <see cref="IValidatableObject" /> and <see cref="IRevertibleChangeTracking" /> interfaces.
    /// </summary>
    /// <seealso cref="IValidatableObject" />
    /// <seealso cref="IRevertibleChangeTracking" />
    public interface IDbEntity : IValidatableObject, IRevertibleChangeTracking
    {
        /// <summary>Gets or sets the database entity creation date/time.</summary>
        /// <value>The date and time when the database entity was created.</value>
        /// <remarks>
        /// For local databases, this value is the system-<see cref="DateTimeKind.Local" /> date and time. For upstream (remote) databases, this is the
        /// <see cref="DateTimeKind.Utc">UTC</see> date and time.
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_CreatedOn), ResourceType = typeof(Properties.Resources))]
        DateTime CreatedOn { get; set; }

        /// <summary>Gets or sets the database entity modification date/time.</summary>
        /// <value>The date and time when the database entity was last modified.</value>
        /// <remarks>
        /// For local databases, this value is the system-<see cref="DateTimeKind.Local" /> date and time. For upstream (remote) databases, this is the
        /// <see cref="DateTimeKind.Utc">UTC</see> date and time.
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_ModifiedOn), ResourceType = typeof(Properties.Resources))]
        DateTime ModifiedOn { get; set; }
    }

    /// <summary>Generic interface for access error entities.</summary>
    /// <seealso cref="IDbEntity" />
    public interface IAccessError : IDbEntity
    {
        /// <summary>Gets the primary key value.</summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the current entity's primary key the database.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Id), ResourceType = typeof(Properties.Resources))]
        Guid Id { get; }

        /// <summary>Gets the error code.</summary>
        /// <value>The <see cref="AccessErrorCode" /> value that represents the numeric error code.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_ErrorCode), ResourceType = typeof(Properties.Resources))]
        AccessErrorCode ErrorCode { get; }

        /// <summary>Gets the brief error message.</summary>
        /// <value>The brief error message.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Message), ResourceType = typeof(Properties.Resources))]
        string Message { get; }

        /// <summary>Gets the error detail text.</summary>
        /// <value>The error detail text.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Details), ResourceType = typeof(Properties.Resources))]
        string Details { get; }

        /// <summary>Gets the target entity to which the access error applies.</summary>
        /// <value>The <see cref="IDbEntity" /> object that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Target), ResourceType = typeof(Properties.Resources))]
        IDbEntity Target { get; }
    }

    /// <summary>Generic interface for access error entities.</summary>
    /// <typeparam name="TTarget">The target entity type.</typeparam>
    /// <seealso cref="IAccessError" />
    public interface IAccessError<TTarget> : IAccessError
    {
        /// <summary>Gets the target entity to which the access error applies.</summary>
        /// <value>The <see cref="IDbEntity" /> object that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Target), ResourceType = typeof(Properties.Resources))]
        new IDbEntity Target { get; }
    }

    /// <summary>Interface for entities which represent a specific file system type.</summary>
    /// <seealso cref="IDbEntity" />
    public interface IFileSystem : IDbEntity
    {
        /// <summary>Gets the primary key value.</summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the current entity's primary key the database.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Id), ResourceType = typeof(Properties.Resources))]
        Guid Id { get; }

        /// <summary>Gets the display name.</summary>
        /// <value>The display name of the file system.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_DisplayName), ResourceType = typeof(Properties.Resources))]
        string DisplayName { get; }

        /// <summary>Gets a value indicating whether file name searches are case-sensitive.</summary>
        /// <value><see langword="true" /> if file name searches are case-sensitive; otherwise, <see langword="false" />.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_CaseSensitiveSearch), ResourceType = typeof(Properties.Resources))]
        bool CaseSensitiveSearch { get; }

        /// <summary>Gets a value indicating whether this is a read-only file system type.</summary>
        /// <value><see langword="true" /> if this is a read-only file system type; otherwise, <see langword="false" />.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_ReadOnly), ResourceType = typeof(Properties.Resources))]
        bool ReadOnly { get; }

        /// <summary>Gets the maximum length of file system name components.</summary>
        /// <value>The maximum length of file system name components.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_MaxNameLength), ResourceType = typeof(Properties.Resources))]
        uint MaxNameLength { get; }

        /// <summary>Gets the default drive type for this file system.</summary>
        /// <value>The default drive type for this file system.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_DefaultDriveType), ResourceType = typeof(Properties.Resources))]
        DriveType? DefaultDriveType { get; }

        /// <summary>Gets the custom notes for this file system type.</summary>
        /// <value>The custom notes to associate with this file system type.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; }

        /// <summary>Gets a value indicating whether this file system type is inactive.</summary>
        /// <value><see langword="true" /> if this file system type is inactive; otherwise, <see langword="false" />.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_IsInactive), ResourceType = typeof(Properties.Resources))]
        bool IsInactive { get; }

        /// <summary>Gets the volumes that share this file system type.</summary>
        /// <value>The volumes that share this file system type.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Volumes), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IVolume> Volumes { get; }

        /// <summary>Gets the symbolic names for the current file system type.</summary>
        /// <value>The symbolic names that are used to identify the current file system type.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_SymbolicNames), ResourceType = typeof(Properties.Resources))]
        IEnumerable<ISymbolicName> SymbolicNames { get; }
    }

    /// <summary>Interface for entities that represent a symbolic name for a file system type.</summary>
    /// <seealso cref="IDbEntity" />
    public interface ISymbolicName : IDbEntity
    {
        /// <summary>Gets the primary key value.</summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the current entity's primary key the database.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Id), ResourceType = typeof(Properties.Resources))]
        Guid Id { get; }

        /// <summary>Gets the symbolic name.</summary>
        /// <value>The symbolic name which refers to a file system type..</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Name), ResourceType = typeof(Properties.Resources))]
        string Name { get; }

        /// <summary>Gets the custom notes for the current symbolic name.</summary>
        /// <value>The custom notes to associate with the current symblic name.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; }

        /// <summary>Gets the priority for this symbolic name.</summary>
        /// <value>The priority of this symbolic name in relation to other symbolic names that refer to the same file system type, with lower values being higher priority.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Priority), ResourceType = typeof(Properties.Resources))]
        int Priority { get; }

        /// <summary>Gets a value indicating whether this symbolic name is inactive.</summary>
        /// <value><see langword="true" /> if this symbolic name  is inactive; otherwise, <see langword="false" />.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_IsInactive), ResourceType = typeof(Properties.Resources))]
        bool IsInactive { get; }

        /// <summary>Gets the file system that this symbolic name refers to.</summary>
        /// <value>The file system entity that represents the file system type that this symbolic name refers to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_FileSystem), ResourceType = typeof(Properties.Resources))]
        IFileSystem FileSystem { get; }
    }

    /// <summary>Interface for entities which represent a logical file system volume.</summary>
    /// <seealso cref="IDbEntity" />
    public interface IVolume : IDbEntity
    {
        /// <summary>Gets the primary key value.</summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the current entity's primary key the database.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Id), ResourceType = typeof(Properties.Resources))]
        Guid Id { get; }

        /// <summary>Gets the display name of the volume.</summary>
        /// <value>The display name of the volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_DisplayName), ResourceType = typeof(Properties.Resources))]
        string DisplayName { get; }

        /// <summary>Gets the name of the volume.</summary>
        /// <value>The name of the volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_VolumeName), ResourceType = typeof(Properties.Resources))]
        string VolumeName { get; }

        /// <summary>Gets the unique volume identifier.</summary>
        /// <value>The system-independent unique identifier, which identifies the volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Identifier), ResourceType = typeof(Properties.Resources))]
        VolumeIdentifier Identifier { get; }

        /// <summary>Gets a value indicating whether file name searches are case-sensitive.</summary>
        /// <value><see langword="true" /> if file name searches are case-sensitive; <see langword="false" /> if they are case-insensitive; otherwise, <see langword="null" /> to assume the same value as defined by the <see cref="IFileSystem.CaseSensitiveSearch">file system type</see>.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_CaseSensitiveSearch), ResourceType = typeof(Properties.Resources))]
        bool? CaseSensitiveSearch { get; }

        /// <summary>Gets a value indicating whether the current volume is read-only.</summary>
        /// <value><see langword="true" /> if the current volume is read-only; <see langword="false" /> if it is read/write; otherwise, <see langword="null" /> to assume the same value as defined by the <see cref="IFileSystem.ReadOnly">file system type</see>.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_ReadOnly), ResourceType = typeof(Properties.Resources))]
        bool? ReadOnly { get; }

        /// <summary>Gets the maximum length of file system name components.</summary>
        /// <value>The maximum length of file system name components or <see langword="null" /> to assume the same value as defined by the <see cref="IFileSystem.MaxNameLength">file system type</see>.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_MaxNameLength), ResourceType = typeof(Properties.Resources))]
        uint? MaxNameLength { get; }

        /// <summary>Gets the drive type for this volume.</summary>
        /// <value>The drive type for this volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Type), ResourceType = typeof(Properties.Resources))]
        DriveType Type { get; }

        /// <summary>Gets the custom notes for this volume.</summary>
        /// <value>The custom notes to associate with this volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; }

        /// <summary>Gets the volume status.</summary>
        /// <value>The volume status value.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Status), ResourceType = typeof(Properties.Resources))]
        VolumeStatus Status { get; }

        /// <summary>Gets the root directory of this volume.</summary>
        /// <value>The root directory of this volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_RootDirectory), ResourceType = typeof(Properties.Resources))]
        ISubdirectory RootDirectory { get; }

        /// <summary>Gets the file system type.</summary>
        /// <value>The file system type for this volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_FileSystem), ResourceType = typeof(Properties.Resources))]
        IFileSystem FileSystem { get; }

        /// <summary>Gets the access errors for the current file system item.</summary>
        /// <value>The access errors for the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IVolumeAccessError> AccessErrors { get; }
    }

    /// <summary>Generic interface for volume access error entities.</summary>
    /// <typeparam name="TTarget">The target entity type.</typeparam>
    /// <seealso cref="IAccessError&lt;IVolume&gt;" />
    public interface IVolumeAccessError : IAccessError<IVolume>
    {
        /// <summary>Gets the target volume to which the access error applies.</summary>
        /// <value>The <typeparamref name="IVolume" /> entity that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Target), ResourceType = typeof(Properties.Resources))]
        new IVolume Target { get; }
    }

    /// <summary>Specifies the settings for a file system crawl.</summary>
    public interface ICrawlSettings
    {
        /// <summary>Gets the maximum recursion depth.</summary>
        /// <value>
        /// The maximum sub-folder recursion depth, where a value less than <c>1</c> only crawls the <see cref="Root" /> <see cref="ISubdirectory" />,
        /// a value will crawl 1 sub-folder deep, and so on.
        /// </value>
        [Display(Name = nameof(Properties.Resources.DisplayName_MaxRecursionDepth), ResourceType = typeof(Properties.Resources))]
        ushort MaxRecursionDepth { get; }

        /// <summary>Gets the maximum total items to crawl.</summary>
        /// <value>The maximum total items to crawl, including both files and subdirectories.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_MaxTotalItems), ResourceType = typeof(Properties.Resources))]
        ulong MaxTotalItems { get; }

        /// <summary>Gets the maximum duration of the crawl.</summary>
        /// <value>The maximum duration of the crawl, in seconds. This value should never be less than <c>1</c>.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_TTL), ResourceType = typeof(Properties.Resources))]
        long? TTL { get; }
    }

    /// <summary>Specifies the configuration of a file system crawl.</summary>
    /// <seealso cref="IDbEntity" />
    /// <seealso cref="ICrawlSettings" />
    public interface ICrawlConfiguration : IDbEntity, ICrawlSettings
    {
        /// <summary>Gets the primary key value.</summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the current entity's primary key the database.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Id), ResourceType = typeof(Properties.Resources))]
        Guid Id { get; }

        /// <summary>Gets the display name.</summary>
        /// <value>The display name for the current crawl configuration.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_DisplayName), ResourceType = typeof(Properties.Resources))]
        string DisplayName { get; }

        /// <summary>Gets the custom notes.</summary>
        /// <value>The custom notes to associate with the current crawl configuration.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; }

        /// <summary>Gets a value indicating whether the current crawl configuration has been deactivated.</summary>
        /// <value>
        /// <see langword="true" /> if the current crawl configuration has been deactivated; otherwise, <see langword="false" /> to indicate that is
        /// available for use.
        /// </value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Status), ResourceType = typeof(Properties.Resources))]
        CrawlStatus StatusValue { get; }

        /// <summary>Gets the date and time when the last crawl was started.</summary>
        /// <value>The date and time when the last crawl was started or <see langword="null" /> if no crawl hhas ever been started for this configuration.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_LastCrawlStart), ResourceType = typeof(Properties.Resources))]
        DateTime? LastCrawlStart { get; }

        /// <summary>Gets the date and time when the last crawl was finshed.</summary>
        /// <value>
        /// The date and time when the last crawl was finshed; otherwise, <see langword="null" /> if the current crawl is still active or if
        /// no crawl has ever been started for this configuration.
        /// </value>
        [Display(Name = nameof(Properties.Resources.DisplayName_LastCrawlEnd), ResourceType = typeof(Properties.Resources))]
        DateTime? LastCrawlEnd { get; }

        /// <summary>Gets the date and time when the next true is to begin.</summary>
        /// <value>The date and time when the next crawl is to begin or <see langword="null" /> if there is no scheduled crawl.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_NextScheduledStart), ResourceType = typeof(Properties.Resources))]
        DateTime? NextScheduledStart { get; }

        /// <summary>Gets the length of time between automatic crawl re-scheduling.</summary>
        /// <value>The length of time between automatic crawl re-scheduling, in seconds or <cref langword="null" /> to disable automatic re-scheduling.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_RescheduleInterval), ResourceType = typeof(Properties.Resources))]
        long? RescheduleInterval { get; }

        /// <summary>Gets a value indicating whether automatic rescheduling is calculated from the completion time of the previous job, versus the start time.</summary>
        /// <value>
        /// <see langword="true" /> if crawl jobs are automatically scheduled <see cref="RescheduleInterval" /> seconds from the completion of the previous job;
        /// otherwise, <see langword="false" /> if crawl jobs are automatically scheduled <see cref="RescheduleInterval" /> seconds from the value
        /// of <see cref="NextScheduledStart" /> at the time the job is started.
        /// </value>
        [Display(Name = nameof(Properties.Resources.DisplayName_RescheduleFromJobEnd), ResourceType = typeof(Properties.Resources))]
        bool RescheduleFromJobEnd { get; }

        /// <summary>Gets a value indicating whether crawl jobs are automatically rescheduled even if the previous job failed.</summary>
        /// <value>
        /// <see langword="true" /> if crawl jobs are always automatically re-scheduled; otherwise, <see langword="false" /> if crawl jobs are automatically
        /// re-scheduled only if the preceding job did not fail.
        /// </value>
        [Display(Name = nameof(Properties.Resources.DisplayName_RescheduleAfterFail), ResourceType = typeof(Properties.Resources))]
        bool RescheduleAfterFail { get; }

        /// <summary>Gets the starting subdirectory for the configured subdirectory crawl.</summary>
        /// <value>The root subdirectory of the configured subdirectory crawl.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Root), ResourceType = typeof(Properties.Resources))]
        ISubdirectory Root { get; }

        /// <summary>Gets the crawl log entries.</summary>
        /// <value>The crawl log entries.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Logs), ResourceType = typeof(Properties.Resources))]
        IEnumerable<ICrawlJobLog> Logs { get; }
    }

    /// <summary>Log of crawl job results.</summary>
    /// <seealso cref="IDbEntity" />
    /// <seealso cref="ICrawlSettings" />
    public interface ICrawlJobLog : IDbEntity, ICrawlSettings
    {
        /// <summary>Gets the primary key value.</summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the current entity's primary key the database.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Id), ResourceType = typeof(Properties.Resources))]
        Guid Id { get; }

        /// <summary>Gets root path of the crawl.</summary>
        /// <value>The root path of the crawl.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_RootPath), ResourceType = typeof(Properties.Resources))]
        string RootPath { get; }

        /// <summary>Gets a value indicating whether the current crawl configuration has been deactivated.</summary>
        /// <value>
        /// <see langword="true" /> if the current crawl configuration has been deactivated; otherwise, <see langword="false" /> to indicate that is
        /// available for use.
        /// </value>
        [Display(Name = nameof(Properties.Resources.DisplayName_StatusCode), ResourceType = typeof(Properties.Resources))]
        CrawlStatus StatusCode { get; }

        /// <summary>Gets the date and time when the crawl was started.</summary>
        /// <value>The date and time when the crawl was started.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_CrawlStart), ResourceType = typeof(Properties.Resources))]
        DateTime CrawlStart { get; }

        /// <summary>Gets the date and time when the crawl was finshed.</summary>
        /// <value>The date and time when the crawl was finshed or <see langword="null" /> if the current crawl is still active.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_CrawlEnd), ResourceType = typeof(Properties.Resources))]
        DateTime? CrawlEnd { get; }

        /// <summary>Gets the status message.</summary>
        /// <value>The crawl status message.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Message), ResourceType = typeof(Properties.Resources))]
        string StatusMessage { get; }

        /// <summary>Gets the status details.</summary>
        /// <value>The status details.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Details), ResourceType = typeof(Properties.Resources))]
        string StatusDetail { get; }

        /// <summary>Gets the configuration source for the file system crawl.</summary>
        /// <value>The configuration for the file system crawl.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Configuration), ResourceType = typeof(Properties.Resources))]
        ICrawlConfiguration Configuration { get; }
    }

    /// <summary>Base interface for a database entity that represents a file system node.</summary>
    /// <seealso cref="IDbEntity" />
    public interface IDbFsItem : IDbEntity
    {
        /// <summary>Gets the primary key value.</summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the current entity's primary key the database.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Id), ResourceType = typeof(Properties.Resources))]
        Guid Id { get; }

        /// <summary>Gets the name of the current file system item.</summary>
        /// <value>The name of the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Name), ResourceType = typeof(Properties.Resources))]
        string Name { get; }

        /// <summary>Gets the date and time last accessed.</summary>
        /// <value>The last accessed for the purposes of this application.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_LastAccessed), ResourceType = typeof(Properties.Resources))]
        DateTime LastAccessed { get; }

        /// <summary>Gets custom notes to be associated with the current file system item.</summary>
        /// <value>The custom notes to associate with the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; }

        /// <summary>Gets the file's creation time.</summary>
        /// <value>The creation time as reported by the host file system.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_CreationTime), ResourceType = typeof(Properties.Resources))]
        DateTime CreationTime { get; }

        /// <summary>Gets the date and time the file system item was last written nto.</summary>
        /// <value>The last write time as reported by the host file system.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_LastWriteTime), ResourceType = typeof(Properties.Resources))]
        DateTime LastWriteTime { get; }

        /// <summary>Gets the parent subdirectory of the current file system item.</summary>
        /// <value>The parent <see cref="ISubdirectory" /> of the current file system item or <see langword="null" /> if this is the root subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Parent), ResourceType = typeof(Properties.Resources))]
        ISubdirectory Parent { get; }

        /// <summary>Gets the access errors for the current file system item.</summary>
        /// <value>The access errors for the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IAccessError> AccessErrors { get; }
    }

    /// <summary>Interface for entities that represent a subdirectory node within a file system.</summary>
    /// <seealso cref="IDbFsItem" />
    public interface ISubdirectory : IDbFsItem
    {
        /// <summary>Gets the crawl options for the current subdirectory.</summary>
        /// <value>The crawl options for the current subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Options), ResourceType = typeof(Properties.Resources))]
        DirectoryCrawlOptions Options { get; }

        /// <summary>Gets the status of the current subdirectory.</summary>
        /// <value>The status value for the current subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Status), ResourceType = typeof(Properties.Resources))]
        DirectoryStatus Status { get; }

        /// <summary>Gets the parent volume.</summary>
        /// <value>The parent volume (if this is the root subdirectory or <see langword="null" /> if this is a subdirectory.</value>
        /// <remarks>If this is <see langword="null" />, then <see cref="ISubdirectory.Parent" /> should not be null, and vice-versa.</remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Volume), ResourceType = typeof(Properties.Resources))]
        IVolume Volume { get; }

        /// <summary>Gets the crawl configuration that starts with the current subdirectory.</summary>
        /// <value>The crawl configuration that starts with the current subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_CrawlConfiguration), ResourceType = typeof(Properties.Resources))]
        ICrawlConfiguration CrawlConfiguration { get; }

        /// <summary>Gets the files directly contained within this subdirectory.</summary>
        /// <value>The files directly contained within this subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Files), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IFile> Files { get; }

        /// <summary>Gets the nested subdirectories directly contained within this subdirectory.</summary>
        /// <value>The nested subdirectories directly contained within this subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_SubDirectories), ResourceType = typeof(Properties.Resources))]
        IEnumerable<ISubdirectory> SubDirectories { get; }

        /// <summary>Gets the access errors for the current file system item.</summary>
        /// <value>The access errors for the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ISubdirectoryAccessError> AccessErrors { get; }
    }

    /// <summary>Generic interface for subdirectory access error entities.</summary>
    /// <typeparam name="TTarget">The target entity type.</typeparam>
    /// <seealso cref="IAccessError&lt;ISubdirectory&gt;" />
    public interface ISubdirectoryAccessError : IAccessError<ISubdirectory>
    {
        /// <summary>Gets the target subdirectory to which the access error applies.</summary>
        /// <value>The <typeparamref name="ISubdirectory" /> entity that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Target), ResourceType = typeof(Properties.Resources))]
        new ISubdirectory Target { get; }
    }

    /// <summary>Represents a set of files that have the same file size and cryptographic hash.</summary>
    /// <seealso cref="IDbEntity" />
    public interface IBinaryPropertySet : IDbEntity
    {
        /// <summary>Gets the primary key value.</summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the current entity's primary key the database.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Id), ResourceType = typeof(Properties.Resources))]
        Guid Id { get; }

        /// <summary>Gets the file length.</summary>
        /// <value>The file length in bytes.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Length), ResourceType = typeof(Properties.Resources))]
        long Length { get; }

        /// <summary>Gets the MD5 hash of the file's contents.</summary>
        /// <value>The MD5 hash of the file's contents or <see langword="null" /> if the hash has not yet been calculated.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Hash), ResourceType = typeof(Properties.Resources))]
        MD5Hash? Hash { get; }

        /// <summary>Gets the files which have the same length and cryptographic hash.</summary>
        /// <value>The files which have the same length and cryptographic hash..</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Files), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IFile> Files { get; }

        /// <summary>Gets the sets of files which were determined to be duplicates.</summary>
        /// <value>The sets of files which were determined to be duplicates.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_RedundantSets), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IRedundantSet> RedundantSets { get; }
    }

    /// <summary>Represents extended file summary properties.</summary>
    public interface ISummaryProperties
    {
        /// <summary>Gets the Application Name.</summary>
        /// <value>The name of the application that created this file or item.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <para>Do not use version numbers to identify the application's specific version.</para><list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Application Name</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{F29F85E0-4FF9-1068-AB91-08002B27B3D9} (SummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>18</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-applicationname">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_ApplicationName), ResourceType = typeof(Properties.Resources))]
        string ApplicationName { get; }

        /// <summary>Gets the Author.</summary>
        /// <value>The author or authors of the document.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Author</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{F29F85E0-4FF9-1068-AB91-08002B27B3D9} (SummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>4</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-author">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Author), ResourceType = typeof(Properties.Resources))]
        MultiStringValue Author { get; }

        /// <summary>Gets the comments.</summary>
        /// <value>The comment attached to a file, typically added by a user.</value>
        /// <remarks>
        /// This value should be trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Comments</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{F29F85E0-4FF9-1068-AB91-08002B27B3D9} (SummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>6</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-comment">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Comment), ResourceType = typeof(Properties.Resources))]
        string Comment { get; }

        /// <summary>Gets the keywords for the item.</summary>
        /// <value>The set of keywords (also known as "tags") assigned to the item.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Tags</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{F29F85E0-4FF9-1068-AB91-08002B27B3D9} (SummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>5</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-keywords">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Keywords), ResourceType = typeof(Properties.Resources))]
        MultiStringValue Keywords { get; }

        /// <summary>Gets the Subject.</summary>
        /// <value>The subject of a document.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <para>This property maps to the OLE document property Subject.</para><list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Subject</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{F29F85E0-4FF9-1068-AB91-08002B27B3D9} (SummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>3</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-subject">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Subject), ResourceType = typeof(Properties.Resources))]
        string Subject { get; }

        /// <summary>Gets the Title of the item.</summary>
        /// <value>The title of the item.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Title</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{F29F85E0-4FF9-1068-AB91-08002B27B3D9} (SummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>2</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-title">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Title), ResourceType = typeof(Properties.Resources))]
        string Title { get; }

        /// <summary>Gets the company or publisher.</summary>
        /// <value>The company or publisher.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Company Name</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{D5CDD502-2E9C-101B-9397-08002B2CF9AE} (DocumentSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>15</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-company">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Company), ResourceType = typeof(Properties.Resources))]
        string Company { get; }

        /// <summary>Gets the Content Type.</summary>
        /// <value>The content type.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Content Type</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{D5CDD502-2E9C-101B-9397-08002B2CF9AE} (DocumentSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>26</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-contenttype">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_ContentType), ResourceType = typeof(Properties.Resources))]
        string ContentType { get; }

        /// <summary>Gets the Copyright.</summary>
        /// <value>The copyright information stored as a string.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Copyright</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>11</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-copyright">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Copyright), ResourceType = typeof(Properties.Resources))]
        string Copyright { get; }

        /// <summary>Gets the Parental Rating.</summary>
        /// <value>The parental rating stored in a format typically determined by the organization named in System.ParentalRatingsOrganization.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Parental Rating</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>21</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-parentalrating">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_ParentalRating), ResourceType = typeof(Properties.Resources))]
        string ParentalRating { get; }

        /// <summary>Indicates the users preference rating of an item on a scale of 1-99</summary>
        /// <value>1-12 = One Star, 13-37 = Two Stars, 38-62 = Three Stars, 63-87 = Four Stars, 88-99 = Five Stars.</value>
        /// <remarks>
        /// This is the rating system used by the Windows Vista Shell.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Rating</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>9</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-rating">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Rating), ResourceType = typeof(Properties.Resources))]
        uint? Rating { get; }

        /// <summary>This is the generic list of authors associated with an item.</summary>
        /// <value>Generic list of authors associated with an item.</value>
        /// <remarks>
        /// For example, the artist name for a music track is the item author.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Creators</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{D0A04F0A-462A-48A4-BB2F-3706E88DBD7D} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-itemauthors">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_ItemAuthors), ResourceType = typeof(Properties.Resources))]
        MultiStringValue ItemAuthors { get; }

        /// <summary>Gets the canonical item type.</summary>
        /// <value>The canonical type of the item, intended to be programmatically parsed.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <para>
        ///     If there is no canonical type, the value is VT_EMPTY. If the item is a file (ie, System.FileName is not VT_EMPTY), the value is the same as
        ///     System.FileExtension.Use System.ItemTypeText when you want to display the type to end users in a view. (If the item is a file, passing the System.ItemType value
        ///     to PSFormatForDisplay will result in the same value as System.ItemTypeText.)
        /// </para><list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Item Type</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{28636AA6-953D-11D2-B5D6-00C04FD918D0} (ShellDetails)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>11</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-itemtype">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_ItemType), ResourceType = typeof(Properties.Resources))]
        string ItemType { get; }

        /// <summary>Gets the item type name.</summary>
        /// <value>This is the user friendly type name of the item.</value>
        /// <remarks>
        /// This value should be trimmed, with white-space-only converted to <see langword="null" />.
        /// <para>
        ///     This is not intended to be programmatically parsed. If System.ItemType is VT_EMPTY, the value of this property is also VT_EMPTY.
        ///     If the item is a file, the value of this property is the same as if you passed the file's System.ItemType value to PSFormatForDisplay.
        ///     This property should not be confused with System.Kind, where System.Kind is a high-level user friendly kind name.
        /// </para><list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Item Type</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{B725F130-47EF-101A-A5F1-02608C9EEBAC} (Storage)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>4</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-itemtypetext">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_ItemType), ResourceType = typeof(Properties.Resources))]
        string ItemTypeText { get; }

        /// <summary>Search folder extension mappings.</summary>
        /// <value>System.Kind values that are used to map extensions to various Search folders.</value>
        /// <remarks>
        /// Extensions are mapped to Kinds at HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\Explorer\KindMap The list of kinds is not extensible.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>File Kind</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{1E3EE840-BC2B-476C-8237-2ACD1A839B22} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>3</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-kind">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Kind), ResourceType = typeof(Properties.Resources))]
        MultiStringValue Kind { get; }

        /// <summary>Gets the MIME type.</summary>
        /// <value>The MIME type.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <para>Eg, for EML files: 'message/rfc822'.</para><list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>MIME-Type</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{0B63E350-9CCC-11D0-BCDB-00805FCCCE04} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>5</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-mimetype">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_MIMEType), ResourceType = typeof(Properties.Resources))]
        string MIMEType { get; }

        /// <summary>Gets the Parental Rating Reason.</summary>
        /// <value>Explains file ratings.</value>
        /// <remarks>
        /// This value should be trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Parental Rating Reason</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{10984E0A-F9F2-4321-B7EF-BAF195AF4319} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-parentalratingreason">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_ParentalRatingReason), ResourceType = typeof(Properties.Resources))]
        string ParentalRatingReason { get; }

        /// <summary>Gets the Parental Ratings Organization.</summary>
        /// <value>The name of the organization whose rating system is used for System.ParentalRating.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <para>ParentalRating.</para><list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Parental Ratings Organization</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{A7FE0840-1344-46F0-8D37-52ED712A4BF9} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-parentalratingsorganization">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_ParentalRatingsOrganization), ResourceType = typeof(Properties.Resources))]
        string ParentalRatingsOrganization { get; }

        /// <summary>Gets the Sensitivity.</summary>
        /// <value>The Sensitivity value.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Sensitivity</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{F8D3F6AC-4874-42CB-BE59-AB454B30716A} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-sensitivity">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Sensitivity), ResourceType = typeof(Properties.Resources))]
        ushort? Sensitivity { get; }

        /// <summary>Gets the user-friendly Sensitivity value.</summary>
        /// <value>The user-friendly form of System.Sensitivity.</value>
        /// <remarks>
        /// This value should be trimmed, with white-space-only converted to <see langword="null" />.
        /// <para>This value is not intended to be parsed programmatically.</para><list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Sensitivity</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{D0C7F054-3F72-4725-8527-129A577CB269} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-sensitivitytext">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Sensitivity), ResourceType = typeof(Properties.Resources))]
        string SensitivityText { get; }

        /// <summary>Indicates the users preference rating of an item on a scale of 0-5</summary>
        /// <value>0=unrated, 1=One Star, 2=Two Stars, 3=Three Stars, 4=Four Stars, 5=Five Stars</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Simple Rating</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{A09F084E-AD41-489F-8076-AA5BE3082BCA} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-simplerating">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_SimpleRating), ResourceType = typeof(Properties.Resources))]
        uint? SimpleRating { get; }

        /// <summary>Gets the Legal Trademarks.</summary>
        /// <value>The trademark associated with the item, in a string format.</value>
        /// <remarks>
        /// This value should be trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Legal Trademarks</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{0CEF7D53-FA64-11D1-A203-0000F81FEDEE} (VERSION)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>9</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-trademarks">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Trademarks), ResourceType = typeof(Properties.Resources))]
        string Trademarks { get; }

        /// <summary>Gets the Product Name.</summary>
        /// <value>The product name.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Product Name</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{0CEF7D53-FA64-11D1-A203-0000F81FEDEE} (VERSION)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>7</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-software-productname">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_ProductName), ResourceType = typeof(Properties.Resources))]
        string ProductName { get; }
    }

    /// <summary>Represents extended file properties for document files.</summary>
    public interface IDocumentProperties
    {
        /// <summary>Gets the Client ID.</summary>
        /// <value>The Client ID.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Client ID</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{276D7BB0-5B34-4FB0-AA4B-158ED12A1809} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-document-clientid">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_ClientID), ResourceType = typeof(Properties.Resources))]
        string ClientID { get; }

        /// <summary>Gets the Contributor.</summary>
        /// <value>The document contributor.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Contributor</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{F334115E-DA1B-4509-9B3D-119504DC7ABB} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-document-contributor">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Contributor), ResourceType = typeof(Properties.Resources))]
        MultiStringValue Contributor { get; }

        /// <summary>Gets the Date Created.</summary>
        /// <value>The date and time that a document was created.</value>
        /// <remarks>
        /// This property is stored in the document, not obtained from the file system.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Date Created</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{F29F85E0-4FF9-1068-AB91-08002B27B3D9} (SummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>12</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-document-datecreated">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_DateCreated), ResourceType = typeof(Properties.Resources))]
        DateTime? DateCreated { get; }

        /// <summary>Gets the Last Author.</summary>
        /// <value>The last person to save the document, as stored in the document.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Last Author</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{F29F85E0-4FF9-1068-AB91-08002B27B3D9} (SummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>8</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-document-lastauthor">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_LastAuthor), ResourceType = typeof(Properties.Resources))]
        string LastAuthor { get; }

        /// <summary>Gets the Revision Number.</summary>
        /// <value>The revision number.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Revision Number</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{F29F85E0-4FF9-1068-AB91-08002B27B3D9} (SummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>9</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-document-revisionnumber">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_RevisionNumber), ResourceType = typeof(Properties.Resources))]
        string RevisionNumber { get; }

        /// <summary>Access control information, from SummaryInfo propset.</summary>
        /// <value>Access control information, from SummaryInfo propset.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Security</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{F29F85E0-4FF9-1068-AB91-08002B27B3D9} (SummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>19</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-document-security">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Security), ResourceType = typeof(Properties.Resources))]
        int? Security { get; }

        /// <summary>Gets the Division.</summary>
        /// <value>The Division</value>
        /// <remarks>
        /// This value should be trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Division</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{1E005EE6-BF27-428B-B01C-79676ACD2870} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-document-division">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Division), ResourceType = typeof(Properties.Resources))]
        string Division { get; }

        /// <summary>Gets the Document ID.</summary>
        /// <value>The Document ID</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Document ID</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{E08805C8-E395-40DF-80D2-54F0D6C43154} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-document-documentid">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_DocumentID), ResourceType = typeof(Properties.Resources))]
        string DocumentID { get; }

        /// <summary>Gets the Manager.</summary>
        /// <value>The Manager</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Manager</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{D5CDD502-2E9C-101B-9397-08002B2CF9AE} (DocumentSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>14</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-document-manager">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Manager), ResourceType = typeof(Properties.Resources))]
        string Manager { get; }

        /// <summary>Gets the Presentation Format.</summary>
        /// <value>The Presentation Format</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Presentation Format</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{D5CDD502-2E9C-101B-9397-08002B2CF9AE} (DocumentSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>3</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-document-presentationformat">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_PresentationFormat), ResourceType = typeof(Properties.Resources))]
        string PresentationFormat { get; }

        /// <summary>Gets the Version.</summary>
        /// <value>The Version</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Version Number</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{D5CDD502-2E9C-101B-9397-08002B2CF9AE} (DocumentSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>29</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-document-version">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Version), ResourceType = typeof(Properties.Resources))]
        string Version { get; }
    }

    /// <summary>Represents extended file properties for audio files.</summary>
    public interface IAudioProperties
    {
        /// <summary>Gets the Compression Method.</summary>
        /// <value>Indicates the audio compression used on the audio file.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Compression Method</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440490-4C8B-11D1-8B70-080036B11A03} (AudioSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>10</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-audio-compression">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Compression), ResourceType = typeof(Properties.Resources))]
        string Compression { get; }

        /// <summary>Indicates the average data rate in Hz for the audio file in "bits per second".</summary>
        /// <value>Indicates the average data rate in Hertz (Hz) for the audio file in bits per second.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Encoding Bitrate</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440490-4C8B-11D1-8B70-080036B11A03} (AudioSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>4</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-audio-encodingbitrate">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_EncodingBitrate), ResourceType = typeof(Properties.Resources))]
        uint? EncodingBitrate { get; }

        /// <summary>Indicates the format of the audio file.</summary>
        /// <value>Indicates the format of the audio file.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Format</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440490-4C8B-11D1-8B70-080036B11A03} (AudioSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>2</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-audio-format">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Format), ResourceType = typeof(Properties.Resources))]
        string Format { get; }

        /// <summary>Indicates whether Bit Rate of the audio is variable.</summary>
        /// <value>Indicates whether the audio file had a variable or constant bit rate.</value>
        /// <remarks>
        /// <see langword="true" /> if the bit rate of the audio is variable; <see langword="false" /> if the bit rate is constant; otherwise, <see langword="null" /> if this value is not specified.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Is Variable Bitrate</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{E6822FEE-8C17-4D62-823C-8E9CFCBD1D5C} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-audio-isvariablebitrate">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_IsVariableBitrate), ResourceType = typeof(Properties.Resources))]
        bool? IsVariableBitrate { get; }

        /// <summary>Indicates the audio sample rate for the audio file in "samples per second".</summary>
        /// <value>Indicates the sample rate for the audio file in samples per second.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Sample Rate</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440490-4C8B-11D1-8B70-080036B11A03} (AudioSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>5</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-audio-samplerate">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_SampleRate), ResourceType = typeof(Properties.Resources))]
        uint? SampleRate { get; }

        /// <summary>Indicates the audio sample size for the audio file in "bits per sample".</summary>
        /// <value>Indicates the sample size for the audio file in bits per sample.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Sample Size</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440490-4C8B-11D1-8B70-080036B11A03} (AudioSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>6</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-audio-samplesize">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_SampleSize), ResourceType = typeof(Properties.Resources))]
        uint? SampleSize { get; }

        /// <summary>Gets the Stream Name.</summary>
        /// <value>Identifies the name of the stream for the audio file.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Stream Name</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440490-4C8B-11D1-8B70-080036B11A03} (AudioSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>9</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-audio-streamname">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_StreamName), ResourceType = typeof(Properties.Resources))]
        string StreamName { get; }

        /// <summary>Gets the Stream Number.</summary>
        /// <value>Identifies the stream number of the audio file.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Stream Number</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440490-4C8B-11D1-8B70-080036B11A03} (AudioSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>8</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-audio-streamnumber">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_StreamNumber), ResourceType = typeof(Properties.Resources))]
        ushort? StreamNumber { get; }
    }

    /// <summary>Represents extended file properties for DRM information.</summary>
    public interface IDRMProperties
    {
        /// <summary>Indicates when play expires for digital rights management.</summary>
        /// <value>Indicates when play rights expire.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Date Play Expires</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{AEAC19E4-89AE-4508-B9B7-BB867ABEE2ED} (DRM)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>6</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-drm-dateplayexpires">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_DatePlayExpires), ResourceType = typeof(Properties.Resources))]
        DateTime? DatePlayExpires { get; }

        /// <summary>Indicates when play starts for digital rights management.</summary>
        /// <value>Indicates when play rights begin.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Date Play Starts</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{AEAC19E4-89AE-4508-B9B7-BB867ABEE2ED} (DRM)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>5</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-drm-dateplaystarts">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_DatePlayStarts), ResourceType = typeof(Properties.Resources))]
        DateTime? DatePlayStarts { get; }

        /// <summary>Displays the description for digital rights management.</summary>
        /// <value>Displays the description for Digital Rights Management (DRM).</value>
        /// <remarks>
        /// This value should be trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>License Description</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{AEAC19E4-89AE-4508-B9B7-BB867ABEE2ED} (DRM)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>3</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-drm-description">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Description), ResourceType = typeof(Properties.Resources))]
        string Description { get; }

        /// <summary>Indicates whether the content is protected.</summary>
        /// <value><see langword="true" /> if the content of the file is protected; <see langword="false" /> if the file content is unprotected; otherwise, <see langword="null" /> if this value is not specified.</value>
        /// <remarks>
        /// Indicates whether the file is protected under Digital Rights Management (DRM).
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Is Protected</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{AEAC19E4-89AE-4508-B9B7-BB867ABEE2ED} (DRM)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>2</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-drm-isprotected">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_IsProtected), ResourceType = typeof(Properties.Resources))]
        bool? IsProtected { get; }

        /// <summary>Indicates the play count for digital rights management.</summary>
        /// <value>Indicates the number of times the file has been played.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Plays Remaining</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{AEAC19E4-89AE-4508-B9B7-BB867ABEE2ED} (DRM)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>4</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-drm-playcount">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_PlayCount), ResourceType = typeof(Properties.Resources))]
        uint? PlayCount { get; }
    }

    /// <summary>Represents extended file properties for GPS information.</summary>
    public interface IGPSProperties
    {
        /// <summary>Gets the name of the GPS area.</summary>
        /// <value>The name of the GPS area.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Area Information</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{972E333E-AC7E-49F1-8ADF-A70D07A9BCAB} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-areainformation">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_AreaInformation), ResourceType = typeof(Properties.Resources))]
        string AreaInformation { get; }

        /// <summary>Indicates the latitude degrees.</summary>
        /// <value>This is the value at index 0 from an array of three values.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Latitude Degrees</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{8727CFFF-4868-4EC6-AD5B-81B98521D1AB} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-latitudedegrees">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_LatitudeDegrees), ResourceType = typeof(Properties.Resources))]
        double? LatitudeDegrees { get; }

        /// <summary>Indicates the latitude minutes.</summary>
        /// <value>This is the value at index 1 from an array of three values.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Latitude Minutes</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{8727CFFF-4868-4EC6-AD5B-81B98521D1AB} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-latitudeminutes">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_LatitudeMinutes), ResourceType = typeof(Properties.Resources))]
        double? LatitudeMinutes { get; }

        /// <summary>Indicates the latitude seconds.</summary>
        /// <value>This is the value at index 2 from an array of three values.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Latitude Seconds</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{8727CFFF-4868-4EC6-AD5B-81B98521D1AB} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-latitude">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_LatitudeSeconds), ResourceType = typeof(Properties.Resources))]
        double? LatitudeSeconds { get; }

        /// <summary>Indicates whether latitude is north or south latitude.</summary>
        /// <value>Indicates whether latitude is north or south.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Latitude Reference</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{029C0252-5B86-46C7-ACA0-2769FFC8E3D4} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-latituderef">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_LatitudeRef), ResourceType = typeof(Properties.Resources))]
        string LatitudeRef { get; }

        /// <summary>Indicates the longitude degrees.</summary>
        /// <value>This is the value at index 0 from an array of three values.</value>
        /// <remarks>
        /// Indicates whether latitude is north or south.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Longitude Degrees</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{C4C4DBB2-B593-466B-BBDA-D03D27D5E43A} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-longitudedegrees">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_LongitudeDegrees), ResourceType = typeof(Properties.Resources))]
        double? LongitudeDegrees { get; }

        /// <summary>Indicates the longitude minutes.</summary>
        /// <value>This is the value at index 1 from an array of three values.</value>
        /// <remarks>
        /// Indicates whether latitude is north or south.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Longitude Minutes</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{C4C4DBB2-B593-466B-BBDA-D03D27D5E43A} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-longitudeminutes">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_LongitudeMinutes), ResourceType = typeof(Properties.Resources))]
        double? LongitudeMinutes { get; }

        /// <summary>Indicates the longitude seconds.</summary>
        /// <value>This is the value at index 2 from an array of three values.</value>
        /// <remarks>
        /// Indicates whether latitude is north or south.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Longitude Seconds</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{C4C4DBB2-B593-466B-BBDA-D03D27D5E43A} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-longitudeseconds">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_LongitudeSeconds), ResourceType = typeof(Properties.Resources))]
        double? LongitudeSeconds { get; }

        /// <summary>Indicates whether longitude is east or west longitude.</summary>
        /// <value>Indicates whether longitude is east or west.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Longitude Reference</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{33DCF22B-28D5-464C-8035-1EE9EFD25278} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-longituderef">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_LongitudeRef), ResourceType = typeof(Properties.Resources))]
        string LongitudeRef { get; }

        /// <summary>Indicates the GPS measurement mode.</summary>
        /// <value>eg: 2-dimensional, 3-dimensional Indicates the GPS measurement mode (for example, two-dimensional, three-dimensional).</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Measure Mode</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{A015ED5D-AAEA-4D58-8A86-3C586920EA0B} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-measuremode">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_MeasureMode), ResourceType = typeof(Properties.Resources))]
        string MeasureMode { get; }

        /// <summary>Indicates the name of the method used for location finding.</summary>
        /// <value>Indicates the name of the method used for finding locations.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Processing Method</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{59D49E61-840F-4AA9-A939-E2099B7F6399} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-processingmethod">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_ProcessingMethod), ResourceType = typeof(Properties.Resources))]
        string ProcessingMethod { get; }

        /// <summary>Indicates the version of the GPS information.</summary>
        /// <value>Indicates the version of the GPS information.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Version ID</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{22704DA4-C6B2-4A99-8E56-F16DF8C92599} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-gps-versionid">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_VersionID), ResourceType = typeof(Properties.Resources))]
        ByteValues VersionID { get; }
    }

    /// <summary>Represents extended file properties for image files.</summary>
    public interface IImageProperties
    {
        /// <summary>Gets the Bit Depth.</summary>
        /// <value>Indicates how many bits are used in each pixel of the image.</value>
        /// <remarks>
        /// (Usually 8, 16, 24, or 32).
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Bit Depth</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{6444048F-4C8B-11D1-8B70-080036B11A03} (ImageSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>7</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-image-bitdepth">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_BitDepth), ResourceType = typeof(Properties.Resources))]
        uint? BitDepth { get; }

        /// <summary>Gets the Color Space.</summary>
        /// <value>PropertyTagExifColorSpace The colorspace embedded in the image.</value>
        /// <remarks>
        /// Taken from the Exchangeable Image File (EXIF) information.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Color Representation</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{14B81DA1-0135-4D31-96D9-6CBFC9671A99} (ImageProperties)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>40961</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-image-colorspace">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_ColorSpace), ResourceType = typeof(Properties.Resources))]
        ushort? ColorSpace { get; }

        /// <summary>Gets the Compressed Bits-per-Pixel.</summary>
        /// <value>Calculated from PKEY_Image_CompressedBitsPerPixelNumerator and PKEY_Image_CompressedBitsPerPixelDenominator.</value>
        /// <remarks>
        /// Indicates the image compression level.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Compressed Bits-per-Pixel</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{364B6FA9-37AB-482A-BE2B-AE02F60D4318} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-image-compressedbitsperpixel">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_CompressedBitsPerPixel), ResourceType = typeof(Properties.Resources))]
        double? CompressedBitsPerPixel { get; }

        /// <summary>Indicates the image compression level.</summary>
        /// <value>PropertyTagCompression The algorithm used to compress the image.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Compression</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{14B81DA1-0135-4D31-96D9-6CBFC9671A99} (ImageProperties)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>259</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-image-compression">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Compression), ResourceType = typeof(Properties.Resources))]
        ushort? Compression { get; }

        /// <summary>This is the user-friendly form of System.Image.Compression.</summary>
        /// <value>Not intended to be parsed programmatically.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <para>The user-friendly form of System.Image.Compression. Not intended to be parsed programmatically.</para><list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Compression</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{3F08E66F-2F44-4BB9-A682-AC35D2562322} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-image-compressiontext">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Compression), ResourceType = typeof(Properties.Resources))]
        string CompressionText { get; }

        /// <summary>Gets the Horizontal Resolution.</summary>
        /// <value>Indicates the number of pixels per resolution unit in the image width.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Horizontal Resolution</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{6444048F-4C8B-11D1-8B70-080036B11A03} (ImageSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>5</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-image-horizontalresolution">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_HorizontalResolution), ResourceType = typeof(Properties.Resources))]
        double? HorizontalResolution { get; }

        /// <summary>Gets the Horizontal Size.</summary>
        /// <value>The horizontal size of the image, in pixels.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Width</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{6444048F-4C8B-11D1-8B70-080036B11A03} (ImageSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>3</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-image-horizontalsize">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_HorizontalSize), ResourceType = typeof(Properties.Resources))]
        uint? HorizontalSize { get; }

        /// <summary>Gets the Image ID.</summary>
        /// <value>The Image ID.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Image ID</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{10DABE05-32AA-4C29-BF1A-63E2D220587F} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-image-imageid">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_ImageID), ResourceType = typeof(Properties.Resources))]
        string ImageID { get; }

        /// <summary>Gets the Resolution Unit.</summary>
        /// <value>Indicates the resolution units.</value>
        /// <remarks>
        /// Used for images with a non-square aspect ratio, but without meaningful absolute dimensions. 1 = No absolute unit of measurement. 2 = Inches. 3 = Centimeters. The default value is 2 (Inches).
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Resolution Unit</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{19B51FA6-1F92-4A5C-AB48-7DF0ABD67444} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-image-resolutionunit">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_ResolutionUnit), ResourceType = typeof(Properties.Resources))]
        short? ResolutionUnit { get; }

        /// <summary>Gets the Vertical Resolution.</summary>
        /// <value>Indicates the number of pixels per resolution unit in the image height.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Vertical Resolution</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{6444048F-4C8B-11D1-8B70-080036B11A03} (ImageSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>6</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-image-verticalresolution">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_VerticalResolution), ResourceType = typeof(Properties.Resources))]
        double? VerticalResolution { get; }

        /// <summary>Gets the Vertical Size.</summary>
        /// <value>The vertical size of the image, in pixels.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Height</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{6444048F-4C8B-11D1-8B70-080036B11A03} (ImageSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>4</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-image-verticalsize">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_VerticalSize), ResourceType = typeof(Properties.Resources))]
        uint? VerticalSize { get; }
    }

    /// <summary>Represents extended file properties for media files.</summary>
    public interface IMediaProperties
    {
        /// <summary>Gets the Content Distributor.</summary>
        /// <value>The Content Distributor.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Content Distributor</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>18</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-contentdistributor">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_ContentDistributor), ResourceType = typeof(Properties.Resources))]
        string ContentDistributor { get; }

        /// <summary>Gets the Creator Application.</summary>
        /// <value>The creator application.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Creator Application/Tool</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>27</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-creatorapplication">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_CreatorApplication), ResourceType = typeof(Properties.Resources))]
        string CreatorApplication { get; }

        /// <summary>Gets the Creator Application Version.</summary>
        /// <value>The creator application version.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Creator Application/Tool Version</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>28</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-creatorapplicationversion">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_CreatorApplicationVersion), ResourceType = typeof(Properties.Resources))]
        string CreatorApplicationVersion { get; }

        /// <summary>Gets the Date Released.</summary>
        /// <value>The release data.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Date Released</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{DE41CC29-6971-4290-B472-F59F2E2F31E2} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-datereleased">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_DateReleased), ResourceType = typeof(Properties.Resources))]
        string DateReleased { get; }

        /// <summary>Gets the duration.</summary>
        /// <value>100ns units, not milliseconds The actual play time of a media file and is measured in 100ns units, not milliseconds.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Duration</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440490-4C8B-11D1-8B70-080036B11A03} (AudioSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>3</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-duration">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Duration), ResourceType = typeof(Properties.Resources))]
        ulong? Duration { get; }

        /// <summary>Gets the DVD ID.</summary>
        /// <value>The DVD ID.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>DVD ID</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>15</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-dvdid">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_DVDID), ResourceType = typeof(Properties.Resources))]
        string DVDID { get; }

        /// <summary>Indicates the frame count for the image.</summary>
        /// <value>Indicates the frame count for the image.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Frame Count</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{6444048F-4C8B-11D1-8B70-080036B11A03} (ImageSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>12</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-framecount">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_FrameCount), ResourceType = typeof(Properties.Resources))]
        uint? FrameCount { get; }

        /// <summary>Gets the Producer.</summary>
        /// <value>The producer.</value>
        /// <remarks>
        /// Media.Producer
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Producer</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>22</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-producer">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Producer), ResourceType = typeof(Properties.Resources))]
        MultiStringValue Producer { get; }

        /// <summary>Gets the Protection Type.</summary>
        /// <value>If media is protected, how is it protected? Describes the type of media protection.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Protection Type</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>38</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-protectiontype">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_ProtectionType), ResourceType = typeof(Properties.Resources))]
        string ProtectionType { get; }

        /// <summary>Gets the Provider Rating.</summary>
        /// <value>Rating value ranges from 0 to 99, supplied by metadata provider The rating (0 - 99) supplied by metadata provider.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Provider Rating</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>39</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-providerrating">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_ProviderRating), ResourceType = typeof(Properties.Resources))]
        string ProviderRating { get; }

        /// <summary>Style of music or video.</summary>
        /// <value>Supplied by metadata provider The style of music or video, supplied by metadata provider.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Provider Style</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>40</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-providerstyle">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_ProviderStyle), ResourceType = typeof(Properties.Resources))]
        string ProviderStyle { get; }

        /// <summary>Gets the Publisher.</summary>
        /// <value>The Publisher.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Publisher</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>30</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-publisher">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Publisher), ResourceType = typeof(Properties.Resources))]
        string Publisher { get; }

        /// <summary>Gets the Subtitle.</summary>
        /// <value>The sub-title.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Subtitle</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{56A3372E-CE9C-11D2-9F0E-006097C686F6} (MUSIC)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>38</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-subtitle">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Subtitle), ResourceType = typeof(Properties.Resources))]
        string Subtitle { get; }

        /// <summary>Gets the Writer.</summary>
        /// <value>The writer.</value>
        /// <remarks>
        /// Media.Writer
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Writer</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>23</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-writer">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Writer), ResourceType = typeof(Properties.Resources))]
        MultiStringValue Writer { get; }

        /// <summary>Gets the Publication Year.</summary>
        /// <value>The publication year.</value>
        /// <remarks>
        /// Media.Year
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Publication Year</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{56A3372E-CE9C-11D2-9F0E-006097C686F6} (MUSIC)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>5</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-media-year">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Year), ResourceType = typeof(Properties.Resources))]
        uint? Year { get; }
    }

    /// <summary>Represents extended file properties for music files.</summary>
    public interface IMusicProperties
    {
        /// <summary>Gets the Album Artist.</summary>
        /// <value>The Album Artist</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Album Artist</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{56A3372E-CE9C-11D2-9F0E-006097C686F6} (MUSIC)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>13</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-albumartist">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_AlbumArtist), ResourceType = typeof(Properties.Resources))]
        string AlbumArtist { get; }

        /// <summary>Gets the Album Title.</summary>
        /// <value>The Album Title</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Album Title</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{56A3372E-CE9C-11D2-9F0E-006097C686F6} (MUSIC)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>4</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-albumtitle">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_AlbumTitle), ResourceType = typeof(Properties.Resources))]
        string AlbumTitle { get; }

        /// <summary>Gets the Contributing Artist.</summary>
        /// <value>The Contributing Artist</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Contributing Artist</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{56A3372E-CE9C-11D2-9F0E-006097C686F6} (MUSIC)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>2</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-artist">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Artist), ResourceType = typeof(Properties.Resources))]
        MultiStringValue Artist { get; }

        /// <summary>Gets the Channel Count.</summary>
        /// <value>Indicates the channel count for the audio file. Possible values are 1 for mono and 2 for stereo.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Channel Count</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440490-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>2</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-artist">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_ChannelCount), ResourceType = typeof(Properties.Resources))]
        uint? ChannelCount { get; }

        /// <summary>Gets the Composer.</summary>
        /// <value>The Composer</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Composer</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>19</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-composer">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Composer), ResourceType = typeof(Properties.Resources))]
        MultiStringValue Composer { get; }

        /// <summary>Gets the Conductor.</summary>
        /// <value>The Conductor</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Conductor</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{56A3372E-CE9C-11D2-9F0E-006097C686F6} (MUSIC)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>36</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-conductor">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Conductor), ResourceType = typeof(Properties.Resources))]
        MultiStringValue Conductor { get; }

        /// <summary>Gets the Album Artist (best match of relevant properties).</summary>
        /// <value>The best representation of Album Artist for a given music file based upon AlbumArtist, ContributingArtist and compilation info.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <para>This property returns the best representation of the album artist for a specific music file based upon System.Music.AlbumArtist, System.Music.Artist, and System.Music.IsCompilation information.</para><list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Display Artist</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{FD122953-FA93-4EF7-92C3-04C946B2F7C8} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-displayartist">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_DisplayArtist), ResourceType = typeof(Properties.Resources))]
        string DisplayArtist { get; }

        /// <summary>Gets the Genre.</summary>
        /// <value>The Genre</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Genre</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{56A3372E-CE9C-11D2-9F0E-006097C686F6} (MUSIC)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>11</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-genre">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Genre), ResourceType = typeof(Properties.Resources))]
        MultiStringValue Genre { get; }

        /// <summary>Gets the Part of the Set.</summary>
        /// <value>The Part of the Set</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Part of Set</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{56A3372E-CE9C-11D2-9F0E-006097C686F6} (MUSIC)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>37</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-partofset">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_PartOfSet), ResourceType = typeof(Properties.Resources))]
        string PartOfSet { get; }

        /// <summary>Gets the Period.</summary>
        /// <value>The Period</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Period</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>31</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-period">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Period), ResourceType = typeof(Properties.Resources))]
        string Period { get; }

        /// <summary>Gets the Track Number.</summary>
        /// <value>The Track Number</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>#</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{56A3372E-CE9C-11D2-9F0E-006097C686F6} (MUSIC)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>7</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-tracknumber">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_TrackNumber), ResourceType = typeof(Properties.Resources))]
        uint? TrackNumber { get; }
    }

    /// <summary>Represents extended file properties for photo files.</summary>
    public interface IPhotoProperties
    {
        /// <summary>Gets the Camera Manufacturer.</summary>
        /// <value>The manufacturer name of the camera that took the photo, in a string format.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Camera Manufacturer</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{14B81DA1-0135-4D31-96D9-6CBFC9671A99} (ImageProperties)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>271</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-photo-cameramanufacturer">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_CameraManufacturer), ResourceType = typeof(Properties.Resources))]
        string CameraManufacturer { get; }

        /// <summary>Gets the Camera Model.</summary>
        /// <value>The model name of the camera that shot the photo, in string form.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Camera Model</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{14B81DA1-0135-4D31-96D9-6CBFC9671A99} (ImageProperties)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>272</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-photo-cameramodel">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_CameraModel), ResourceType = typeof(Properties.Resources))]
        string CameraModel { get; }

        /// <summary>Gets the Date Taken.</summary>
        /// <value>The date when the photo was taken, as read from the camera in the file's Exchangeable Image File (EXIF) tag.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Date Taken</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{14B81DA1-0135-4D31-96D9-6CBFC9671A99} (ImageProperties)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>36867</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-photo-datetaken">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_DateTaken), ResourceType = typeof(Properties.Resources))]
        DateTime? DateTaken { get; }

        /// <summary>Return the event at which the photo was taken.</summary>
        /// <value>The event where the photo was taken.</value>
        /// <remarks>
        /// The end-user provides this value.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Event Name</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{14B81DA1-0135-4D31-96D9-6CBFC9671A99} (ImageProperties)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>18248</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-photo-event">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Event), ResourceType = typeof(Properties.Resources))]
        MultiStringValue Event { get; }

        /// <summary>Returns the EXIF version.</summary>
        /// <value>The Exchangeable Image File (EXIF) version.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>EXIF Version</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{D35F743A-EB2E-47F2-A286-844132CB1427} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-photo-exifversion">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_EXIFVersion), ResourceType = typeof(Properties.Resources))]
        string EXIFVersion { get; }

        /// <summary>Gets the Orientation.</summary>
        /// <value>The orientation of the photo when it was taken, as specified in the Exchangeable Image File (EXIF) information and in terms of rows and columns.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Orientation</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{14B81DA1-0135-4D31-96D9-6CBFC9671A99} (ImageProperties)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>274</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-photo-orientation">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Orientation), ResourceType = typeof(Properties.Resources))]
        ushort? Orientation { get; }

        /// <summary>Gets the user-friendly form of System.Photo.Orientation.</summary>
        /// <value>The user-friendly form of System.Photo.Orientation.</value>
        /// <remarks>
        /// This value should be trimmed, with white-space-only converted to <see langword="null" />.
        /// <para>Not intended to be parsed programmatically.</para><list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Orientation</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{A9EA193C-C511-498A-A06B-58E2776DCC28} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-photo-orientationtext">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Orientation), ResourceType = typeof(Properties.Resources))]
        string OrientationText { get; }

        /// <summary>The people tags on an image.</summary>
        /// <value>The people tags on an image.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>People Tags</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{E8309B6E-084C-49B4-B1FC-90A80331B638} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-photo-peoplenames">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_PeopleNames), ResourceType = typeof(Properties.Resources))]
        MultiStringValue PeopleNames { get; }
    }

    /// <summary>Represents extended file properties for recorded TV files.</summary>
    public interface IRecordedTVProperties
    {
        /// <summary>Gets the Channel Number.</summary>
        /// <value>Example: 42 The recorded TV channels.</value>
        /// <remarks>
        /// For example, 42, 5, 53.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Channel Number</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{6D748DE2-8D38-4CC3-AC60-F009B057C557} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>7</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-recordedtv-channelnumber">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_ChannelNumber), ResourceType = typeof(Properties.Resources))]
        uint? ChannelNumber { get; }

        /// <summary>Gets the Episode Name.</summary>
        /// <value>Example: "Nowhere to Hyde" The names of recorded TV episodes.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <para>For example, "Nowhere to Hyde".</para><list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Episode Name</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{6D748DE2-8D38-4CC3-AC60-F009B057C557} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>2</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-recordedtv-episodename">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_EpisodeName), ResourceType = typeof(Properties.Resources))]
        string EpisodeName { get; }

        /// <summary>Indicates whether the video is DTV.</summary>
        /// <value><see langword="true" /> if the video is DTV; <see langword="false" /> if not DTV; otherwise, <see langword="null" /> if not specified.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Is DTV Content</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{6D748DE2-8D38-4CC3-AC60-F009B057C557} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>17</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-recordedtv-isdtvcontent">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_IsDTVContent), ResourceType = typeof(Properties.Resources))]
        bool? IsDTVContent { get; }

        /// <summary>Indicates whether the video is HDTV.</summary>
        /// <value><see langword="true" /> if the video is HDTV; <see langword="false" /> if not HDTV; otherwise, <see langword="null" /> if not specified.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Is HDTV Content</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{6D748DE2-8D38-4CC3-AC60-F009B057C557} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>18</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-recordedtv-ishdcontent">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_IsHDContent), ResourceType = typeof(Properties.Resources))]
        bool? IsHDContent { get; }

        /// <summary>Gets the Network Affiliation.</summary>
        /// <value>The Network Affiliation</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>TV Network Affiliation</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{2C53C813-FB63-4E22-A1AB-0B331CA1E273} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-recordedtv-networkaffiliation">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_NetworkAffiliation), ResourceType = typeof(Properties.Resources))]
        string NetworkAffiliation { get; }

        /// <summary>Gets the Original Broadcast Date.</summary>
        /// <value>The Original Broadcast Date</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Original Broadcast Date</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{4684FE97-8765-4842-9C13-F006447B178C} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-recordedtv-originalbroadcastdate">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_OriginalBroadcastDate), ResourceType = typeof(Properties.Resources))]
        DateTime? OriginalBroadcastDate { get; }

        /// <summary>Gets the Program Description.</summary>
        /// <value>The Program Description</value>
        /// <remarks>
        /// This value should be trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Program Description</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{6D748DE2-8D38-4CC3-AC60-F009B057C557} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>3</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-recordedtv-programdescription">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_ProgramDescription), ResourceType = typeof(Properties.Resources))]
        string ProgramDescription { get; }

        /// <summary>Gets the Station Call Sign.</summary>
        /// <value>Example: "TOONP" Any recorded station call signs.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <para>For example, "TOONP".</para><list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Station Call Sign</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{6D748DE2-8D38-4CC3-AC60-F009B057C557} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>5</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-recordedtv-stationcallsign">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_StationCallSign), ResourceType = typeof(Properties.Resources))]
        string StationCallSign { get; }

        /// <summary>Gets the Station Name.</summary>
        /// <value>The  name of the broadcast station or <see langword="null" /> if this value is not specified.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Station Name</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{1B5439E7-EBA1-4AF8-BDD7-7AF1D4549493} (Format)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-recordedtv-stationname">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_StationName), ResourceType = typeof(Properties.Resources))]
        string StationName { get; }
    }

    /// <summary>Represents extended file properties for video files.</summary>
    public interface IVideoProperties
    {
        /// <summary>Indicates the level of compression for the video stream.</summary>
        /// <value>Specifies the video compression format.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Compression</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440491-4C8B-11D1-8B70-080036B11A03} (VideoSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>10</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-video-compression">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Compression), ResourceType = typeof(Properties.Resources))]
        string Compression { get; }

        /// <summary>Gets the Director.</summary>
        /// <value>Indicates the person who directed the video.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Director</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>20</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-video-director">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Director), ResourceType = typeof(Properties.Resources))]
        MultiStringValue Director { get; }

        /// <summary>Indicates the data rate in "bits per second" for the video stream.</summary>
        /// <value>Indicates the data rate in "bits per second" for the video stream.</value>
        /// <remarks>
        /// "DataRate".
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Encoding Data Rate</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440491-4C8B-11D1-8B70-080036B11A03} (VideoSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>8</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-video-encodingbitrate">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_EncodingBitrate), ResourceType = typeof(Properties.Resources))]
        uint? EncodingBitrate { get; }

        /// <summary>Indicates the frame height for the video stream.</summary>
        /// <value>Indicates the frame height for the video stream.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Frame Height</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440491-4C8B-11D1-8B70-080036B11A03} (VideoSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>4</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-video-frameheight">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_FrameHeight), ResourceType = typeof(Properties.Resources))]
        uint? FrameHeight { get; }

        /// <summary>Indicates the frame rate in "frames per millisecond" for the video stream.</summary>
        /// <value>Indicates the frame rate for the video stream, in frames per 1000 seconds.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Frame Rate</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440491-4C8B-11D1-8B70-080036B11A03} (VideoSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>6</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-video-framerate">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_FrameRate), ResourceType = typeof(Properties.Resources))]
        uint? FrameRate { get; }

        /// <summary>Indicates the frame width for the video stream.</summary>
        /// <value>Indicates the frame width for the video stream.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Frame Width</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440491-4C8B-11D1-8B70-080036B11A03} (VideoSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>3</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-video-framewidth">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_FrameWidth), ResourceType = typeof(Properties.Resources))]
        uint? FrameWidth { get; }

        /// <summary>Indicates the horizontal portion of the aspect ratio.</summary>
        /// <value>The X portion of XX:YY, like 16:9.</value>
        /// <remarks>
        /// Indicates the horizontal portion of the pixel aspect ratio. The X portion of XX:YY. For example, 10 is the X portion of 10:11.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Horizontal Aspect Ratio</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440491-4C8B-11D1-8B70-080036B11A03} (VideoSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>42</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-video-horizontalaspectratio">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_HorizontalAspectRatio), ResourceType = typeof(Properties.Resources))]
        uint? HorizontalAspectRatio { get; }

        /// <summary>Gets the Stream Number.</summary>
        /// <value>Indicates the ordinal number of the stream being played.</value>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Stream Number</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440491-4C8B-11D1-8B70-080036B11A03} (VideoSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>11</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-video-streamnumber">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_StreamNumber), ResourceType = typeof(Properties.Resources))]
        ushort? StreamNumber { get; }

        /// <summary>Gets the name for the video stream..</summary>
        /// <value>The name for the video stream.</value>
        /// <remarks>
        /// This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null" />.
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Stream Name</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440491-4C8B-11D1-8B70-080036B11A03} (VideoSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>2</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-video-streamname">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_StreamName), ResourceType = typeof(Properties.Resources))]
        string StreamName { get; }

        /// <summary>Indicates the vertical portion of the aspect ratio.</summary>
        /// <value>The Y portion of XX:YY, like 16:9.</value>
        /// <remarks>
        /// Indicates the horizontal portion of the pixel aspect ratio. The Y portion of XX:YY. For example, 11 is the Y portion of 10:11 .
        /// <list type="bullet">
        ///     <item>
        ///         <term>Name</term>
        ///         <description>Vertical Aspect Ratio</description>
        ///     </item>
        ///     <item>
        ///         <term>Format ID</term>
        ///         <description>{64440491-4C8B-11D1-8B70-080036B11A03} (VideoSummaryInformation)</description>
        ///     </item>
        ///     <item>
        ///         <term>Property ID</term>
        ///         <description>45</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-video-verticalaspectratio">[Reference Link]</a>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_VerticalAspectRatio), ResourceType = typeof(Properties.Resources))]
        uint? VerticalAspectRatio { get; }
    }

    /// <summary>Base interface for entities that represent a grouping of extended file properties.</summary>
    /// <seealso cref="IDbEntity" />
    public interface IPropertySet : IDbEntity
    {
        /// <summary>Gets the primary key value.</summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the current entity's primary key the database.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Id), ResourceType = typeof(Properties.Resources))]
        Guid Id { get; }

        /// <summary>Gets the files that share the same property values as this property set.</summary>
        /// <value>The <see cref="IFile">files</see> that share the same property values as this property set.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Files), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IFile> Files { get; }
    }

    /// <summary>Interface for database objects that contain extended file summary property values.</summary>
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="ISummaryProperties" />
    public interface ISummaryPropertySet : IPropertySet, ISummaryProperties
    {
    }

    /// <summary>Interface for database objects that contain extended file property values of document files.</summary>
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="IDocumentProperties" />
    public interface IDocumentPropertySet : IPropertySet, IDocumentProperties
    {
    }

    /// <summary>Interface for database objects that contain extended file property values of audio files.</summary>
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="IAudioProperties" />
    public interface IAudioPropertySet : IPropertySet, IAudioProperties
    {
    }

    /// <summary>Interface for database objects that contain extended file DRM property values.</summary>
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="IDRMProperties" />
    public interface IDRMPropertySet : IPropertySet, IDRMProperties
    {
    }

    /// <summary>Interface for database objects that contain extended file GPS property values.</summary>
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="IGPSProperties" />
    public interface IGPSPropertySet : IPropertySet, IGPSProperties
    {
    }

    /// <summary>Interface for database objects that contain extended file property values of image files.</summary>
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="IImageProperties" />
    public interface IImagePropertySet : IPropertySet, IImageProperties
    {
    }

    /// <summary>Interface for database objects that contain extended file property values of media files.</summary>
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="IMediaProperties" />
    public interface IMediaPropertySet : IPropertySet, IMediaProperties
    {
    }

    /// <summary>Interface for database objects that contain extended file property values of music files.</summary>
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="IMusicProperties" />
    public interface IMusicPropertySet : IPropertySet, IMusicProperties
    {
    }

    /// <summary>Interface for database objects that contain extended file property values of photo files.</summary>
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="IPhotoProperties" />
    public interface IPhotoPropertySet : IPropertySet, IPhotoProperties
    {
    }

    /// <summary>Interface for database objects that contain extended file property values of recorded TV files.</summary>
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="IRecordedTVProperties" />
    public interface IRecordedTVPropertySet : IPropertySet, IRecordedTVProperties
    {
    }

    /// <summary>Interface for database objects that contain extended file property values of video files.</summary>
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="IVideoProperties" />
    public interface IVideoPropertySet : IPropertySet, IVideoProperties
    {
    }

    /// <summary>Represents a structural instance of file.</summary>
    /// <seealso cref="IDbFsItem" />
    public interface IFile : IDbFsItem
    {
        /// <summary>Gets the visibility and crawl options for the current file.</summary>
        /// <value>A <see cref="FileCrawlOptions" /> value that contains the crawl options for the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Options), ResourceType = typeof(Properties.Resources))]
        FileCrawlOptions Options { get; }

        /// <summary>Gets the correlative status of the current file.</summary>
        /// <value>A <see cref="FileCorrelationStatus" /> value that indicates the file's correlation status.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Status), ResourceType = typeof(Properties.Resources))]
        FileCorrelationStatus Status { get; }

        /// <summary>Gets the date and time that the <see cref="MD5Hash">MD5 hash</see> was calculated for the current file.</summary>
        /// <value>The date and time that the <see cref="MD5Hash">MD5 hash</see> was calculated for the current file or <see langword="null" /> if no <see cref="MD5Hash">MD5 hash</see> has been calculated, yet.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_LastHashCalculation), ResourceType = typeof(Properties.Resources))]
        DateTime? LastHashCalculation { get; }

        /// <summary>Gets the binary properties for the current file.</summary>
        /// <value>The generic <see cref="IBinaryPropertySet" /> that contains the file size and optionally, the <see cref="MD5Hash">MD5 hash</see> value of its binary contents.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_BinaryProperties), ResourceType = typeof(Properties.Resources))]
        IBinaryPropertySet BinaryProperties { get; }

        /// <summary>Gets the summary properties for the current file.</summary>
        /// <value>The generic <see cref="IBinaryPropertySet" /> that contains the summary properties for the current file or <see langword="null" /> if no summary properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_SummaryProperties), ResourceType = typeof(Properties.Resources))]
        ISummaryPropertySet SummaryProperties { get; }

        /// <summary>Gets the document properties for the current file.</summary>
        /// <value>The generic <see cref="IDocumentPropertySet" /> that contains the document properties for the current file or <see langword="null" /> if no document properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_DocumentProperties), ResourceType = typeof(Properties.Resources))]
        IDocumentPropertySet DocumentProperties { get; }

        /// <summary>Gets the audio properties for the current file.</summary>
        /// <value>The generic <see cref="IAudioPropertySet" /> that contains the audio properties for the current file or <see langword="null" /> if no audio properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AudioProperties), ResourceType = typeof(Properties.Resources))]
        IAudioPropertySet AudioProperties { get; }

        /// <summary>Gets the DRM properties for the current file.</summary>
        /// <value>The generic <see cref="IDRMPropertySet" /> that contains the DRM properties for the current file or <see langword="null" /> if no DRM properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_DRMProperties), ResourceType = typeof(Properties.Resources))]
        IDRMPropertySet DRMProperties { get; }

        /// <summary>Gets the GPS properties for the current file.</summary>
        /// <value>The generic <see cref="IGPSPropertySet" /> that contains the GPS properties for the current file or <see langword="null" /> if no GPS properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_GPSProperties), ResourceType = typeof(Properties.Resources))]
        IGPSPropertySet GPSProperties { get; }

        /// <summary>Gets the image properties for the current file.</summary>
        /// <value>The generic <see cref="IImagePropertySet" /> that contains the image properties for the current file or <see langword="null" /> if no image properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_ImageProperties), ResourceType = typeof(Properties.Resources))]
        IImagePropertySet ImageProperties { get; }

        /// <summary>Gets the media properties for the current file.</summary>
        /// <value>The generic <see cref="IMediaPropertySet" /> that contains the media properties for the current file or <see langword="null" /> if no media properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_MediaProperties), ResourceType = typeof(Properties.Resources))]
        IMediaPropertySet MediaProperties { get; }

        /// <summary>Gets the music properties for the current file.</summary>
        /// <value>The generic <see cref="IMusicPropertySet" /> that contains the music properties for the current file or <see langword="null" /> if no music properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_MusicProperties), ResourceType = typeof(Properties.Resources))]
        IMusicPropertySet MusicProperties { get; }

        /// <summary>Gets the photo properties for the current file.</summary>
        /// <value>The generic <see cref="IPhotoPropertySet" /> that contains the photo properties for the current file or <see langword="null" /> if no photo properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_PhotoProperties), ResourceType = typeof(Properties.Resources))]
        IPhotoPropertySet PhotoProperties { get; }

        /// <summary>Gets the recorded tv properties for the current file.</summary>
        /// <value>The generic <see cref="IRecordedTVPropertySet" /> that contains the recorded TV properties for the current file or <see langword="null" /> if no recorded TV properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_RecordedTVProperties), ResourceType = typeof(Properties.Resources))]
        IRecordedTVPropertySet RecordedTVProperties { get; }

        /// <summary>Gets the video properties for the current file.</summary>
        /// <value>The generic <see cref="IVideoPropertySet" /> that contains the video properties for the current file or <see langword="null" /> if no video properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_VideoProperties), ResourceType = typeof(Properties.Resources))]
        IVideoPropertySet VideoProperties { get; }

        /// <summary>Gets the redundancy item that indicates the membership of a collection of redundant files.</summary>
        /// <value>
        /// A <see cref="IRedundancy" /> object that indicates the current file is an exact copy of other files that belong to the same <see cref="IRedundancy.RedundantSet" />
        /// or <see langword="null" /> if this file has not been identified as being redundant with any other.
        /// </value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Redundancy), ResourceType = typeof(Properties.Resources))]
        IRedundancy Redundancy { get; }

        /// <summary>Gets the comparisons where the current file was the <see cref="IComparison.Baseline" />.</summary>
        /// <value>The <see cref="IComparison" /> entities where the current file is the <see cref="IComparison.Baseline" />.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_BaselineComparisons), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IComparison> BaselineComparisons { get; }

        /// <summary>Gets the comparisons where the current file was the <see cref="IComparison.Correlative" /> being compared to a separate <see cref="IComparison.Baseline" /> file.</summary>
        /// <value>The <see cref="IComparison" /> entities where the current file is the <see cref="IComparison.Correlative" />.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_CorrelativeComparisons), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IComparison> CorrelativeComparisons { get; }

        /// <summary>Gets the access errors for the current file system item.</summary>
        /// <value>The access errors for the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<IFileAccessError> AccessErrors { get; }
    }

    /// <summary>Generic interface for file access error entities.</summary>
    /// <typeparam name="TTarget">The target entity type.</typeparam>
    /// <seealso cref="IAccessError&lt;IFile&gt;" />
    public interface IFileAccessError : IAccessError<IFile>
    {
        /// <summary>Gets the target file to which the access error applies.</summary>
        /// <value>The <typeparamref name="IFile" /> entity that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Target), ResourceType = typeof(Properties.Resources))]
        new IFile Target { get; }
    }

    /// <summary>Represents a set of files that have the same size, Hash and remediation status.</summary>
    /// <seealso cref="IDbEntity" />
    public interface IRedundantSet : IDbEntity
    {
        /// <summary>Gets the primary key value.</summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the current entity's primary key the database.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Id), ResourceType = typeof(Properties.Resources))]
        Guid Id { get; }

        /// <summary>Gets the custom reference value.</summary>
        /// <value>The custom reference value which can be used to refer to external information regarding redundancy remediation, such as a ticket number.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Reference), ResourceType = typeof(Properties.Resources))]
        string Reference { get; }

        /// <summary>Gets custom notes to be associated with the current set of redunant files.</summary>
        /// <value>The custom notes to associate with the current set of redunant files.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; }

        /// <summary>Gets the binary properties in common with all files in the current redundant set.</summary>
        /// <value>The binary properties in common with all files in the current redundant set.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_BinaryProperties), ResourceType = typeof(Properties.Resources))]
        IBinaryPropertySet BinaryProperties { get; }

        /// <summary>Gets the redundancy entities which represent links to redundant files.</summary>
        /// <value>The redundancy entities which represent links to redundant files.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Redundancies), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IRedundancy> Redundancies { get; }
    }

    /// <summary>.</summary>
    /// <seealso cref="IDbEntity" />
    public interface IRedundancy : IDbEntity
    {
        /// <summary>Gets the custom reference value.</summary>
        /// <value>The custom reference value which can be used to refer to external information regarding redundancy remediation, such as a ticket number.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Reference), ResourceType = typeof(Properties.Resources))]
        string Reference { get; }

        /// <summary>Gets custom notes to be associated with the current redundancy.</summary>
        /// <value>The custom notes to associate with the current redundancy.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; }

        /// <summary>Gets the primary key of the file in the that belongs to the redundancy set.</summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the foreign key that refers to the <see cref="IRedundancy.File" /><see cref="IFile">entity</see>.</value>
        /// <remarks>This is also part of this entity's compound primary key.</remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_FileId), ResourceType = typeof(Properties.Resources))]
        Guid FileId { get; }

        /// <summary>Gets the file that belongs to the redundancy set.</summary>
        /// <value>The file that belongs to the redundancy set.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_File), ResourceType = typeof(Properties.Resources))]
        IFile File { get; }

        /// <summary>Gets the primary key of the redundancy set file belongs to.</summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the foreign key that refers to the <see cref="IRedundancy.RedundantSet" /><see cref="IRedundantSet">entity</see>.</value>
        /// <remarks>This is also part of this entity's compound primary key.</remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_RedundantSetId), ResourceType = typeof(Properties.Resources))]
        Guid RedundantSetId { get; }

        /// <summary>Gets the redundancy set.</summary>
        /// <value>The redundancy set.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_RedundantSet), ResourceType = typeof(Properties.Resources))]
        IRedundantSet RedundantSet { get; }
    }

    /// <summary>The results of a byte-for-byte comparison of 2 files.</summary>
    /// <seealso cref="IDbEntity" />
    public interface IComparison : IDbEntity
    {
        /// <summary>Gets a value indicating whether the <see cref="IComparison.Baseline" /> and <see cref="IComparison.Correlative" /> are identical byte-for-byte.</summary>
        /// <value><see langword="true" /> if <see cref="IComparison.Baseline" /> and <see cref="IComparison.Correlative" /> are identical byte-for-byte; otherwise, <see langword="false" />.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AreEqual), ResourceType = typeof(Properties.Resources))]
        bool AreEqual { get; }

        /// <summary>Gets the date and time when the files were compared.</summary>
        /// <value>The date and time when <see cref="IComparison.Baseline" /> was compared to <see cref="IComparison.Correlative" />.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_ComparedOn), ResourceType = typeof(Properties.Resources))]
        DateTime ComparedOn { get; }

        /// <summary>Gets the primary key of the baseline file in the comparison.</summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the foreign key that refers to the <see cref="IComparison.Baseline" /><see cref="IFile">file entity</see>.</value>
        /// <remarks>This is also part of this entity's compound primary key.</remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_BaselineId), ResourceType = typeof(Properties.Resources))]
        Guid BaselineId { get; }

        /// <summary>Gets the baseline file in the comparison.</summary>
        /// <value>The generic <see cref="IFile" /> that represents the baseline file in the comparison.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Baseline), ResourceType = typeof(Properties.Resources))]
        IFile Baseline { get; }

        /// <summary>Gets the primary key of the correlative file in the comparison.</summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the foreign key that refers to the <see cref="IComparison.Correlative" /><see cref="IFile">file entity</see>.</value>
        /// <remarks>This is also part of this entity's compound primary key.</remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_CorrelativeId), ResourceType = typeof(Properties.Resources))]
        Guid CorrelativeId { get; }

        /// <summary>Gets the correlative file in the comparison.</summary>
        /// <value>The generic <see cref="IFile" /> that represents the correlative file, which is the new or changed file in the comparison.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Correlative), ResourceType = typeof(Properties.Resources))]
        IFile Correlative { get; }
    }

    public static class DbConstants
    {
        public const int DbColMaxLen_SimpleName = 256;
        public const int DbColMaxLen_LongName = 1024;
        public const int DbColMaxLen_ShortName = 128;
        public const int DbColMaxLen_Identifier = 1024;
        public const int DbColMaxLen_FileName = 1024;
        public const uint DbColDefaultValue_MaxNameLength = 255;
        public const ushort DbColDefaultValue_MaxRecursionDepth = 256;
        public const ulong DbColDefaultValue_MaxTotalItems = ulong.MaxValue;
    }

}

namespace FsInfoCat.Local
{
    /// <summary>Base interface for all database entity objects for the database which is hosted on the local machine.</summary>
    /// <seealso cref="IDbEntity" />
    public interface ILocalDbEntity : IDbEntity
    {
        /// <summary>Gets the value of the primary key for the corresponding <see cref="Upstream.IUpstreamDbEntity">upstream (remote) database entity</see>.</summary>
        /// <value>
        /// The value of the primary key of the corresponding <see cref="Upstream.IUpstreamDbEntity">upstream (remote) database entity</see>;
        /// otherwise, <see langword="null" /> if there is no corresponding entity.
        /// </value>
        /// <remarks>
        /// If this value is <see langword="null" />, then <see cref="LastSynchronizedOn" /> should also be <see langword="null" />.
        /// Likewise, if this is not <see langword="null" />, then <see cref="LastSynchronizedOn" /> should not be <see langword="null" />, either.
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_UpstreamId), ResourceType = typeof(Properties.Resources))]
        Guid? UpstreamId { get; }

        /// <summary>Gets the value of the primary key for the corresponding <see cref="Upstream.IUpstreamDbEntity">upstream (remote) database entity</see>.</summary>
        /// <value>
        /// The value of the primary key of the corresponding <see cref="Upstream.IUpstreamDbEntity">upstream (remote) database entity</see>;
        /// otherwise, <see langword="null" /> if there is no corresponding entity.
        /// </value>
        /// <remarks>
        /// If this value is <see langword="null" />, then <see cref="LastSynchronizedOn" /> should also be <see langword="null" />.
        /// Likewise, if this is not <see langword="null" />, then <see cref="LastSynchronizedOn" /> should not be <see langword="null" />, either.
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_LastSynchronizedOn), ResourceType = typeof(Properties.Resources))]
        DateTime? LastSynchronizedOn { get; }
    }

    /// <summary></summary>
    /// <typeparam name="TTarget">The target entity type.</typeparam>
    /// <seealso cref="IAccessError&lt;ILocalDbEntity&gt;" />
    /// <seealso cref="IDbEntity" />
    public interface ILocalAccessError : IAccessError<ILocalDbEntity>, IDbEntity
    {
        /// <summary>Gets the target entity to which the access error applies.</summary>
        /// <value>The <see cref="ILocalDbEntity" /> object that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Target), ResourceType = typeof(Properties.Resources))]
        new ILocalDbEntity Target { get; }
    }

    /// <summary>Interface for entities which represent a specific file system type.</summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="IFileSystem" />
    public interface ILocalFileSystem : ILocalDbEntity, IFileSystem
    {
        /// <summary>Gets the volumes that share this file system type.</summary>
        /// <value>The volumes that share this file system type.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Volumes), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ILocalVolume> Volumes { get; }

        /// <summary>Gets the symbolic names for the current file system type.</summary>
        /// <value>The symbolic names that are used to identify the current file system type.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_SymbolicNames), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ILocalSymbolicName> SymbolicNames { get; }
    }

    /// <summary>Interface for entities that represent a symbolic name for a file system type.</summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="ISymbolicName" />
    public interface ILocalSymbolicName : ILocalDbEntity, ISymbolicName
    {
        /// <summary>Gets the file system that this symbolic name refers to.</summary>
        /// <value>The file system entity that represents the file system type that this symbolic name refers to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_FileSystem), ResourceType = typeof(Properties.Resources))]
        new ILocalFileSystem FileSystem { get; }
    }

    /// <summary></summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="IVolume" />
    public interface ILocalVolume : ILocalDbEntity, IVolume
    {
        /// <summary>Gets the root directory of this volume.</summary>
        /// <value>The root directory of this volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_RootDirectory), ResourceType = typeof(Properties.Resources))]
        new ILocalSubdirectory RootDirectory { get; }

        /// <summary>Gets the file system type.</summary>
        /// <value>The file system type for this volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_FileSystem), ResourceType = typeof(Properties.Resources))]
        new ILocalFileSystem FileSystem { get; }

        /// <summary>Gets the access errors for the current file system item.</summary>
        /// <value>The access errors for the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ILocalVolumeAccessError> AccessErrors { get; }
    }

    /// <summary>Generic interface for volume access error entities.</summary>
    /// <typeparam name="TTarget">The target entity type.</typeparam>
    /// <seealso cref="ILocalAccessError" />
    /// <seealso cref="IVolumeAccessError" />
    /// <seealso cref="IAccessError&lt;ILocalVolume&gt;" />
    public interface ILocalVolumeAccessError : ILocalAccessError, IVolumeAccessError, IAccessError<ILocalVolume>
    {
        /// <summary>Gets the target volume to which the access error applies.</summary>
        /// <value>The <typeparamref name="IVolume" /> entity that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Target), ResourceType = typeof(Properties.Resources))]
        new ILocalVolume Target { get; }
    }

    /// <summary>Specifies the configuration of a file system crawl.</summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="ICrawlConfiguration" />
    public interface ILocalCrawlConfiguration : ILocalDbEntity, ICrawlConfiguration
    {
        /// <summary>Gets the starting subdirectory for the configured subdirectory crawl.</summary>
        /// <value>The root subdirectory of the configured subdirectory crawl.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Root), ResourceType = typeof(Properties.Resources))]
        new ILocalSubdirectory Root { get; }

        /// <summary>Gets the crawl log entries.</summary>
        /// <value>The crawl log entries.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Logs), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ILocalCrawlJobLog> Logs { get; }
    }

    /// <summary>Log of crawl job results.</summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="ICrawlJobLog" />
    public interface ILocalCrawlJobLog : ILocalDbEntity, ICrawlJobLog
    {
        /// <summary>Gets the configuration source for the file system crawl.</summary>
        /// <value>The configuration for the file system crawl.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Configuration), ResourceType = typeof(Properties.Resources))]
        new ILocalCrawlConfiguration Configuration { get; }
    }

    /// <summary></summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="IDbFsItem" />
    public interface ILocalDbFsItem : ILocalDbEntity, IDbFsItem
    {
        /// <summary>Gets the parent subdirectory.</summary>
        /// <value>The parent <see cref="ILocalSubdirectory" /> or <see langword="null" /> if this is the root <see cref="ILocalSubdirectory" />.</value>
        /// <remarks>
        /// If the current entity is a <see cref="ILocalSubdirectory" /> and this is <see langword="null" />,
        /// then <see cref="ILocalSubdirectory.Volume" /> should not be <see langword="null" />, and vice-versa;
        /// otherwise, if the current entity is a <see cref="ILocalFile" />, then this should never be <see langword="null" />.
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Parent), ResourceType = typeof(Properties.Resources))]
        new ILocalSubdirectory Parent { get; }

        /// <summary>Gets the access errors for the current file system item.</summary>
        /// <value>The access errors for the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ILocalAccessError> AccessErrors { get; }
    }

    /// <summary></summary>
    /// <seealso cref="ILocalDbFsItem" />
    /// <seealso cref="ISubdirectory" />
    public interface ILocalSubdirectory : ILocalDbFsItem, ISubdirectory
    {
        /// <summary>Gets the parent volume.</summary>
        /// <value>The parent volume (if this is the root subdirectory or <see langword="null" /> if this is a subdirectory.</value>
        /// <remarks>If this is <see langword="null" />, then <see cref="ISubdirectory.Parent" /> should not be null, and vice-versa.</remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Volume), ResourceType = typeof(Properties.Resources))]
        new ILocalVolume Volume { get; }

        /// <summary>Gets the crawl configuration that starts with the current subdirectory.</summary>
        /// <value>The crawl configuration that starts with the current subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_CrawlConfiguration), ResourceType = typeof(Properties.Resources))]
        new ILocalCrawlConfiguration CrawlConfiguration { get; }

        /// <summary>Gets the files directly contained within this subdirectory.</summary>
        /// <value>The files directly contained within this subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Files), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ILocalFile> Files { get; }

        /// <summary>Gets the nested subdirectories directly contained within this subdirectory.</summary>
        /// <value>The nested subdirectories directly contained within this subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_SubDirectories), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ILocalSubdirectory> SubDirectories { get; }

        /// <summary>Gets the access errors for the current file system item.</summary>
        /// <value>The access errors for the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ILocalSubdirectoryAccessError> AccessErrors { get; }
    }

    /// <summary></summary>
    /// <typeparam name="TTarget">The target entity type.</typeparam>
    /// <seealso cref="ILocalAccessError" />
    /// <seealso cref="ISubdirectoryAccessError" />
    /// <seealso cref="IAccessError&lt;ILocalSubdirectory&gt;" />
    public interface ILocalSubdirectoryAccessError : ILocalAccessError, ISubdirectoryAccessError, IAccessError<ILocalSubdirectory>
    {
        /// <summary>Gets the target subdirectory to which the access error applies.</summary>
        /// <value>The <typeparamref name="ISubdirectory" /> entity that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Target), ResourceType = typeof(Properties.Resources))]
        new ILocalSubdirectory Target { get; }
    }

    /// <summary></summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="IBinaryPropertySet" />
    public interface ILocalBinaryPropertySet : ILocalDbEntity, IBinaryPropertySet
    {
        /// <summary>Gets the files which have the same length and cryptographic hash.</summary>
        /// <value>The files which have the same length and cryptographic hash..</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Files), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ILocalFile> Files { get; }

        /// <summary>Gets the sets of files which were determined to be duplicates.</summary>
        /// <value>The sets of files which were determined to be duplicates.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_RedundantSets), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ILocalRedundantSet> RedundantSets { get; }
    }

    /// <summary></summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="IPropertySet" />
    public interface ILocalPropertySet : ILocalDbEntity, IPropertySet
    {
        /// <summary>Gets the files that share the same property values as this property set.</summary>
        /// <value>The <see cref="ILocalFile">files</see> that share the same property values as this property set.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Files), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ILocalFile> Files { get; }
    }

    /// <summary></summary>
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="ISummaryPropertySet" />
    public interface ILocalSummaryPropertySet : ILocalPropertySet, ISummaryPropertySet
    {
    }

    /// <summary></summary>
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="IDocumentPropertySet" />
    public interface ILocalDocumentPropertySet : ILocalPropertySet, IDocumentPropertySet
    {
    }

    /// <summary></summary>
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="IAudioPropertySet" />
    public interface ILocalAudioPropertySet : ILocalPropertySet, IAudioPropertySet
    {
    }

    /// <summary></summary>
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="IDRMPropertySet" />
    public interface ILocalDRMPropertySet : ILocalPropertySet, IDRMPropertySet
    {
    }

    /// <summary></summary>
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="IGPSPropertySet" />
    public interface ILocalGPSPropertySet : ILocalPropertySet, IGPSPropertySet
    {
    }

    /// <summary></summary>
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="IImagePropertySet" />
    public interface ILocalImagePropertySet : ILocalPropertySet, IImagePropertySet
    {
    }

    /// <summary></summary>
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="IMediaPropertySet" />
    public interface ILocalMediaPropertySet : ILocalPropertySet, IMediaPropertySet
    {
    }

    /// <summary></summary>
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="IMusicPropertySet" />
    public interface ILocalMusicPropertySet : ILocalPropertySet, IMusicPropertySet
    {
    }

    /// <summary></summary>
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="IPhotoPropertySet" />
    public interface ILocalPhotoPropertySet : ILocalPropertySet, IPhotoPropertySet
    {
    }

    /// <summary></summary>
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="IRecordedTVPropertySet" />
    public interface ILocalRecordedTVPropertySet : ILocalPropertySet, IRecordedTVPropertySet
    {
    }

    /// <summary></summary>
    /// <seealso cref="ILocalPropertySet" />
    /// <seealso cref="IVideoPropertySet" />
    public interface ILocalVideoPropertySet : ILocalPropertySet, IVideoPropertySet
    {
    }

    /// <summary></summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="IRedundantSet" />
    public interface ILocalRedundantSet : ILocalDbEntity, IRedundantSet
    {
        /// <summary>Gets the binary properties in common with all files in the current redundant set.</summary>
        /// <value>The binary properties in common with all files in the current redundant set.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_BinaryProperties), ResourceType = typeof(Properties.Resources))]
        new ILocalBinaryPropertySet BinaryProperties { get; }

        /// <summary>Gets the redundancy entities which represent links to redundant files.</summary>
        /// <value>The redundancy entities which represent links to redundant files.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Redundancies), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ILocalRedundancy> Redundancies { get; }
    }

    /// <summary></summary>
    /// <seealso cref="ILocalDbFsItem" />
    /// <seealso cref="IFile" />
    public interface ILocalFile : ILocalDbFsItem, IFile
    {
        /// <summary>Gets the binary properties for the current file.</summary>
        /// <value>The generic <see cref="ILocalBinaryPropertySet" /> that contains the file size and optionally, the <see cref="MD5Hash">MD5 hash</see> value of its binary contents.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_BinaryProperties), ResourceType = typeof(Properties.Resources))]
        new ILocalBinaryPropertySet BinaryProperties { get; }

        /// <summary>Gets the summary properties for the current file.</summary>
        /// <value>The generic <see cref="ILocaBinaryPropertySet" /> that contains the summary properties for the current file or <see langword="null" /> if no summary properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_SummaryProperties), ResourceType = typeof(Properties.Resources))]
        new ILocalSummaryPropertySet SummaryProperties { get; }

        /// <summary>Gets the document properties for the current file.</summary>
        /// <value>The generic <see cref="ILocaDocumentPropertySet" /> that contains the document properties for the current file or <see langword="null" /> if no document properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_DocumentProperties), ResourceType = typeof(Properties.Resources))]
        new ILocalDocumentPropertySet DocumentProperties { get; }

        /// <summary>Gets the audio properties for the current file.</summary>
        /// <value>The generic <see cref="ILocaAudioPropertySet" /> that contains the audio properties for the current file or <see langword="null" /> if no audio properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AudioProperties), ResourceType = typeof(Properties.Resources))]
        new ILocalAudioPropertySet AudioProperties { get; }

        /// <summary>Gets the DRM properties for the current file.</summary>
        /// <value>The generic <see cref="ILocaDRMPropertySet" /> that contains the DRM properties for the current file or <see langword="null" /> if no DRM properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_DRMProperties), ResourceType = typeof(Properties.Resources))]
        new ILocalDRMPropertySet DRMProperties { get; }

        /// <summary>Gets the GPS properties for the current file.</summary>
        /// <value>The generic <see cref="ILocaGPSPropertySet" /> that contains the GPS properties for the current file or <see langword="null" /> if no GPS properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_GPSProperties), ResourceType = typeof(Properties.Resources))]
        new ILocalGPSPropertySet GPSProperties { get; }

        /// <summary>Gets the image properties for the current file.</summary>
        /// <value>The generic <see cref="ILocaImagePropertySet" /> that contains the image properties for the current file or <see langword="null" /> if no image properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_ImageProperties), ResourceType = typeof(Properties.Resources))]
        new ILocalImagePropertySet ImageProperties { get; }

        /// <summary>Gets the media properties for the current file.</summary>
        /// <value>The generic <see cref="ILocaMediaPropertySet" /> that contains the media properties for the current file or <see langword="null" /> if no media properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_MediaProperties), ResourceType = typeof(Properties.Resources))]
        new ILocalMediaPropertySet MediaProperties { get; }

        /// <summary>Gets the music properties for the current file.</summary>
        /// <value>The generic <see cref="ILocaMusicPropertySet" /> that contains the music properties for the current file or <see langword="null" /> if no music properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_MusicProperties), ResourceType = typeof(Properties.Resources))]
        new ILocalMusicPropertySet MusicProperties { get; }

        /// <summary>Gets the photo properties for the current file.</summary>
        /// <value>The generic <see cref="ILocaPhotoPropertySet" /> that contains the photo properties for the current file or <see langword="null" /> if no photo properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_PhotoProperties), ResourceType = typeof(Properties.Resources))]
        new ILocalPhotoPropertySet PhotoProperties { get; }

        /// <summary>Gets the recorded tv properties for the current file.</summary>
        /// <value>The generic <see cref="ILocaRecordedTVPropertySet" /> that contains the recorded TV properties for the current file or <see langword="null" /> if no recorded TV properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_RecordedTVProperties), ResourceType = typeof(Properties.Resources))]
        new ILocalRecordedTVPropertySet RecordedTVProperties { get; }

        /// <summary>Gets the video properties for the current file.</summary>
        /// <value>The generic <see cref="ILocaVideoPropertySet" /> that contains the video properties for the current file or <see langword="null" /> if no video properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_VideoProperties), ResourceType = typeof(Properties.Resources))]
        new ILocalVideoPropertySet VideoProperties { get; }

        /// <summary>Gets the redundancy item that indicates the membership of a collection of redundant files.</summary>
        /// <value>
        /// A <see cref="ILocaRedundancy" /> object that indicates the current file is an exact copy of other files that belong to the same <see cref="IRedundancy.RedundantSet" />
        /// or <see langword="null" /> if this file has not been identified as being redundant with any other.
        /// </value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Redundancy), ResourceType = typeof(Properties.Resources))]
        new ILocalRedundancy Redundancy { get; }

        /// <summary>Gets the comparisons where the current file was the <see cref="IComparison.Baseline" />.</summary>
        /// <value>The <see cref="ILocaComparison" /> entities where the current file is the <see cref="IComparison.Baseline" />.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_BaselineComparisons), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ILocalComparison> BaselineComparisons { get; }

        /// <summary>Gets the comparisons where the current file was the <see cref="IComparison.Correlative" /> being compared to a separate <see cref="IComparison.Baseline" /> file.</summary>
        /// <value>The <see cref="ILocaComparison" /> entities where the current file is the <see cref="IComparison.Correlative" />.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_CorrelativeComparisons), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ILocalComparison> CorrelativeComparisons { get; }

        /// <summary>Gets the access errors for the current file system item.</summary>
        /// <value>The access errors for the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ILocalFileAccessError> AccessErrors { get; }
    }

    /// <summary></summary>
    /// <typeparam name="TTarget">The target entity type.</typeparam>
    /// <seealso cref="ILocalAccessError" />
    /// <seealso cref="IFileAccessError" />
    /// <seealso cref="IAccessError&lt;ILocalFile&gt;" />
    public interface ILocalFileAccessError : ILocalAccessError, IFileAccessError, IAccessError<ILocalFile>
    {
        /// <summary>Gets the target file to which the access error applies.</summary>
        /// <value>The <typeparamref name="ILocalFile" /> entity that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Target), ResourceType = typeof(Properties.Resources))]
        new ILocalFile Target { get; }
    }

    /// <summary></summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="IRedundancy" />
    public interface ILocalRedundancy : ILocalDbEntity, IRedundancy
    {
        /// <summary>Gets the file that belongs to the redundancy set.</summary>
        /// <value>The file that belongs to the redundancy set.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_File), ResourceType = typeof(Properties.Resources))]
        new ILocalFile File { get; }

        /// <summary>Gets the redundancy set.</summary>
        /// <value>The redundancy set.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_RedundantSet), ResourceType = typeof(Properties.Resources))]
        new ILocalRedundantSet RedundantSet { get; }
    }

    /// <summary></summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="IComparison" />
    public interface ILocalComparison : ILocalDbEntity, IComparison
    {
        /// <summary>Gets the baseline file in the comparison.</summary>
        /// <value>The generic <see cref="ILocalFile" /> that represents the baseline file in the comparison.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Baseline), ResourceType = typeof(Properties.Resources))]
        new ILocalFile Baseline { get; }

        /// <summary>Gets the correlative file in the comparison.</summary>
        /// <value>The generic <see cref="ILocalFile" /> that represents the correlative file, which is the new or changed file in the comparison.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Correlative), ResourceType = typeof(Properties.Resources))]
        new ILocalFile Correlative { get; }
    }
}

namespace FsInfoCat.Upstream
{
    /// <summary></summary>
    [Flags]
    public enum UserRole : byte
    {
        /// <summary></summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_UserRole_None), ResourceType = typeof(Properties.Resources))]
        None = 0,

        /// <summary></summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_UserRole_Reader), ResourceType = typeof(Properties.Resources))]
        Reader = 1,

        /// <summary></summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_UserRole_Auditor), ResourceType = typeof(Properties.Resources))]
        Auditor = 2,

        /// <summary></summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_UserRole_Contributor), ResourceType = typeof(Properties.Resources))]
        Contributor = 4,

        /// <summary></summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_UserRole_ITSupport), ResourceType = typeof(Properties.Resources))]
        ITSupport = 8,

        /// <summary></summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_UserRole_ChangeAdministrator), ResourceType = typeof(Properties.Resources))]
        ChangeAdministrator = 16,

        /// <summary></summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_UserRole_AppAdministrator), ResourceType = typeof(Properties.Resources))]
        AppAdministrator = 32,

        /// <summary></summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_UserRole_SystemAdmin), ResourceType = typeof(Properties.Resources))]
        SystemAdmin = 64
    }

    /// <summary></summary>
    public enum TaskStatus : byte
    {
        /// <summary></summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_TaskStatus_New), ResourceType = typeof(Properties.Resources))]
        New = 0,

        /// <summary></summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_TaskStatus_Open), ResourceType = typeof(Properties.Resources))]
        Open = 1,

        /// <summary></summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_TaskStatus_Active), ResourceType = typeof(Properties.Resources))]
        Active = 2,

        /// <summary></summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_TaskStatus_Pending), ResourceType = typeof(Properties.Resources))]
        Pending = 3,

        /// <summary></summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_TaskStatus_Canceled), ResourceType = typeof(Properties.Resources))]
        Canceled = 4,

        /// <summary></summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_TaskStatus_Completed), ResourceType = typeof(Properties.Resources))]
        Completed = 5
    }

    /// <summary></summary>
    public enum PriorityLevel : byte
    {
        /// <summary></summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_PriorityLevel_Deferred), ResourceType = typeof(Properties.Resources))]
        Deferred = 0,

        /// <summary></summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_PriorityLevel_Critical), ResourceType = typeof(Properties.Resources))]
        Critical = 1,

        /// <summary></summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_PriorityLevel_High), ResourceType = typeof(Properties.Resources))]
        High = 2,

        /// <summary></summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_PriorityLevel_Normal), ResourceType = typeof(Properties.Resources))]
        Normal = 3,

        /// <summary></summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_PriorityLevel_Low), ResourceType = typeof(Properties.Resources))]
        Low = 4
    }

    /// <summary></summary>
    public enum PlatformType : byte
    {
        /// <summary></summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_PlatformType_Unknown), ResourceType = typeof(Properties.Resources))]
        Unknown = 0,

        /// <summary></summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_PlatformType_Windows), ResourceType = typeof(Properties.Resources))]
        Windows = 1,

        /// <summary></summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_PlatformType_Linux), ResourceType = typeof(Properties.Resources))]
        Linux = 2,

        /// <summary></summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_PlatformType_OSX), ResourceType = typeof(Properties.Resources))]
        OSX = 3,

        /// <summary></summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_PlatformType_Android), ResourceType = typeof(Properties.Resources))]
        Android = 4,

        /// <summary></summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_PlatformType_IOS), ResourceType = typeof(Properties.Resources))]
        IOS = 5,

        /// <summary></summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_PlatformType_Other), ResourceType = typeof(Properties.Resources))]
        Other = 255
    }

    /// <summary>Base interface for all database entity objects for the database which is hosted on the local machine.</summary>
    /// <seealso cref="IDbEntity" />
    public interface IUpstreamDbEntity : IDbEntity
    {
        /// <summary>Gets the user who created the current record.</summary>
        /// <value>The user who created the current record.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_CreatedBy), ResourceType = typeof(Properties.Resources))]
        IUserProfile CreatedBy { get; }

        /// <summary>Gets the user who last modified the current record.</summary>
        /// <value>The user who last modified the current record.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_ModifiedBy), ResourceType = typeof(Properties.Resources))]
        IUserProfile ModifiedBy { get; }
    }

    /// <summary></summary>
    /// <typeparam name="TTarget">The target entity type.</typeparam>
    /// <seealso cref="IAccessError&lt;IUpstreamDbEntity&gt;" />
    /// <seealso cref="IDbEntity" />
    public interface IUpstreamAccessError : IAccessError<IUpstreamDbEntity>, IDbEntity
    {
        /// <summary>Gets the target entity to which the access error applies.</summary>
        /// <value>The <see cref="IUpstreamDbEntity" /> object that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Target), ResourceType = typeof(Properties.Resources))]
        new IUpstreamDbEntity Target { get; }
    }

    /// <summary>Base interface for user profile database entities.</summary>
    /// <seealso cref="IUpstreamDbEntity" />
    public interface IUserProfile : IUpstreamDbEntity
    {
        /// <summary>Gets the user's display name</summary>
        /// <value>The user's display name</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_DisplayName), ResourceType = typeof(Properties.Resources))]
        string DisplayName { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_FirstName), ResourceType = typeof(Properties.Resources))]
        string FirstName { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_LastName), ResourceType = typeof(Properties.Resources))]
        string LastName { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_MI), ResourceType = typeof(Properties.Resources))]
        string MI { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Suffix), ResourceType = typeof(Properties.Resources))]
        string Suffix { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Title), ResourceType = typeof(Properties.Resources))]
        string Title { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_DbPrincipalId), ResourceType = typeof(Properties.Resources))]
        int? DbPrincipalId { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_SID), ResourceType = typeof(Properties.Resources))]
        ByteValues SID { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_IsInactive), ResourceType = typeof(Properties.Resources))]
        bool IsInactive { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_MemberOf), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IGroupMembership> MemberOf { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Tasks), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IMitigationTask> Tasks { get; }
    }

    /// <summary></summary>
    /// <seealso cref="IUpstreamDbEntity" />
    public interface IUserGroup : IUpstreamDbEntity
    {
        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Name), ResourceType = typeof(Properties.Resources))]
        string Name { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Roles), ResourceType = typeof(Properties.Resources))]
        UserRole Roles { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Members), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IGroupMembership> Members { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Tasks), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IMitigationTask> Tasks { get; }
    }

    /// <summary></summary>
    /// <seealso cref="IUpstreamDbEntity" />
    public interface IGroupMembership : IUpstreamDbEntity
    {
        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_IsGroupAdmin), ResourceType = typeof(Properties.Resources))]
        bool IsGroupAdmin { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Group), ResourceType = typeof(Properties.Resources))]
        IUserGroup Group { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_User), ResourceType = typeof(Properties.Resources))]
        IUserProfile User { get; }
    }

    /// <summary></summary>
    /// <seealso cref="IUpstreamDbEntity" />
    public interface IMitigationTask : IUpstreamDbEntity
    {
        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_ShortDescription), ResourceType = typeof(Properties.Resources))]
        string ShortDescription { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Status), ResourceType = typeof(Properties.Resources))]
        TaskStatus Status { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Priority), ResourceType = typeof(Properties.Resources))]
        PriorityLevel Priority { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AssignmentGroup), ResourceType = typeof(Properties.Resources))]
        IUserGroup AssignmentGroup { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AssignedTo), ResourceType = typeof(Properties.Resources))]
        IUserProfile AssignedTo { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_FileActions), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IFileAction> FileActions { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_SubdirectoryActions), ResourceType = typeof(Properties.Resources))]
        IEnumerable<ISubdirectoryAction> SubdirectoryActions { get; }
    }

    /// <summary></summary>
    /// <seealso cref="IUpstreamDbEntity" />
    public interface IHostPlatform : IUpstreamDbEntity
    {
        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_DisplayName), ResourceType = typeof(Properties.Resources))]
        string DisplayName { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Type), ResourceType = typeof(Properties.Resources))]
        PlatformType Type { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_IsInactive), ResourceType = typeof(Properties.Resources))]
        bool IsInactive { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_DefaultFsType), ResourceType = typeof(Properties.Resources))]
        IUpstreamFileSystem DefaultFsType { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_HostDevices), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IHostDevice> HostDevices { get; }
    }

    /// <summary></summary>
    /// <seealso cref="IUpstreamDbEntity" />
    public interface IHostDevice : IUpstreamDbEntity
    {
        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_DisplayName), ResourceType = typeof(Properties.Resources))]
        string DisplayName { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_MachineIdentifer), ResourceType = typeof(Properties.Resources))]
        string MachineIdentifer { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_MachineName), ResourceType = typeof(Properties.Resources))]
        string MachineName { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_IsInactive), ResourceType = typeof(Properties.Resources))]
        bool IsInactive { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Platform), ResourceType = typeof(Properties.Resources))]
        IHostPlatform Platform { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Volumes), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IUpstreamVolume> Volumes { get; }
    }

    /// <summary>Interface for entities which represent a specific file system type.</summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="IFileSystem" />
    public interface IUpstreamFileSystem : IUpstreamDbEntity, IFileSystem
    {
        /// <summary>Gets the volumes that share this file system type.</summary>
        /// <value>The volumes that share this file system type.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Volumes), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<IUpstreamVolume> Volumes { get; }

        /// <summary>Gets the symbolic names for the current file system type.</summary>
        /// <value>The symbolic names that are used to identify the current file system type.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_SymbolicNames), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<IUpstreamSymbolicName> SymbolicNames { get; }
    }

    /// <summary>Interface for entities that represent a symbolic name for a file system type.</summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="ISymbolicName" />
    public interface IUpstreamSymbolicName : IUpstreamDbEntity, ISymbolicName
    {
        /// <summary>Gets the file system that this symbolic name refers to.</summary>
        /// <value>The file system entity that represents the file system type that this symbolic name refers to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_FileSystem), ResourceType = typeof(Properties.Resources))]
        new IUpstreamFileSystem FileSystem { get; }
    }

    /// <summary></summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="IVolume" />
    public interface IUpstreamVolume : IUpstreamDbEntity, IVolume
    {
        /// <summary>Gets the device that hosts the current volume.</summary>
        /// <value>The device that hosts the current volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_HostDevice), ResourceType = typeof(Properties.Resources))]
        IHostDevice HostDevice { get; }

        /// <summary>Gets the root directory of this volume.</summary>
        /// <value>The root directory of this volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_RootDirectory), ResourceType = typeof(Properties.Resources))]
        new IUpstreamSubdirectory RootDirectory { get; }

        /// <summary>Gets the file system type.</summary>
        /// <value>The file system type for this volume.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_FileSystem), ResourceType = typeof(Properties.Resources))]
        new IUpstreamFileSystem FileSystem { get; }

        /// <summary>Gets the access errors for the current file system item.</summary>
        /// <value>The access errors for the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<IUpstreamVolumeAccessError> AccessErrors { get; }
    }

    /// <summary>Generic interface for volume access error entities.</summary>
    /// <typeparam name="TTarget">The target entity type.</typeparam>
    /// <seealso cref="IUpstreamAccessError" />
    /// <seealso cref="IVolumeAccessError" />
    /// <seealso cref="IAccessError&lt;IUpstreamVolume&gt;" />
    public interface IUpstreamVolumeAccessError : IUpstreamAccessError, IVolumeAccessError, IAccessError<IUpstreamVolume>
    {
        /// <summary>Gets the target volume to which the access error applies.</summary>
        /// <value>The <typeparamref name="IUpstreamVolume" /> entity that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Target), ResourceType = typeof(Properties.Resources))]
        new IUpstreamVolume Target { get; }
    }

    /// <summary>Specifies the configuration of a file system crawl.</summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="ICrawlConfiguration" />
    public interface IUpstreamCrawlConfiguration : IUpstreamDbEntity, ICrawlConfiguration
    {
        /// <summary>Gets the starting subdirectory for the configured subdirectory crawl.</summary>
        /// <value>The root subdirectory of the configured subdirectory crawl.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Root), ResourceType = typeof(Properties.Resources))]
        new IUpstreamSubdirectory Root { get; }

        /// <summary>Gets the crawl log entries.</summary>
        /// <value>The crawl log entries.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Logs), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<IUpstreamCrawlJobLog> Logs { get; }
    }

    /// <summary>Log of crawl job results.</summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="ICrawlJobLog" />
    public interface IUpstreamCrawlJobLog : IUpstreamDbEntity, ICrawlJobLog
    {
        /// <summary>Gets the configuration source for the file system crawl.</summary>
        /// <value>The configuration for the file system crawl.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Configuration), ResourceType = typeof(Properties.Resources))]
        new IUpstreamCrawlConfiguration Configuration { get; }
    }

    /// <summary></summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="IDbFsItem" />
    public interface IUpstreamDbFsItem : IUpstreamDbEntity, IDbFsItem
    {
        /// <summary>Gets the parent subdirectory.</summary>
        /// <value>The parent <see cref="IUpstreamSubdirectory" /> or <see langword="null" /> if this is the root <see cref="IUpstreamSubdirectory" />.</value>
        /// <remarks>
        /// If the current entity is a <see cref="IUpstreamSubdirectory" /> and this is <see langword="null" />,
        /// then <see cref="IUpstreamSubdirectory.Volume" /> should not be <see langword="null" />, and vice-versa;
        /// otherwise, if the current entity is a <see cref="IUpstreamFile" />, then this should never be <see langword="null" />.
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Parent), ResourceType = typeof(Properties.Resources))]
        new IUpstreamSubdirectory Parent { get; }

        /// <summary>Gets the access errors for the current file system item.</summary>
        /// <value>The access errors for the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<IUpstreamAccessError> AccessErrors { get; }
    }

    /// <summary></summary>
    /// <seealso cref="IUpstreamDbFsItem" />
    /// <seealso cref="ISubdirectory" />
    public interface IUpstreamSubdirectory : IUpstreamDbFsItem, ISubdirectory
    {
        /// <summary>Gets the parent volume.</summary>
        /// <value>The parent volume (if this is the root subdirectory or <see langword="null" /> if this is a subdirectory.</value>
        /// <remarks>If this is <see langword="null" />, then <see cref="IUpstreamSubdirectory.Parent" /> should not be null, and vice-versa.</remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Volume), ResourceType = typeof(Properties.Resources))]
        new IUpstreamVolume Volume { get; }

        /// <summary>Gets the crawl configuration that starts with the current subdirectory.</summary>
        /// <value>The crawl configuration that starts with the current subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_CrawlConfiguration), ResourceType = typeof(Properties.Resources))]
        new IUpstreamCrawlConfiguration CrawlConfiguration { get; }

        /// <summary>Gets the files directly contained within this subdirectory.</summary>
        /// <value>The files directly contained within this subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Files), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<IUpstreamFile> Files { get; }

        /// <summary>Gets the nested subdirectories directly contained within this subdirectory.</summary>
        /// <value>The nested subdirectories directly contained within this subdirectory.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_SubDirectories), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<IUpstreamSubdirectory> SubDirectories { get; }

        /// <summary>Gets the access errors for the current file system item.</summary>
        /// <value>The access errors for the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<IUpstreamSubdirectoryAccessError> AccessErrors { get; }
    }

    /// <summary></summary>
    /// <typeparam name="TTarget">The target entity type.</typeparam>
    /// <seealso cref="IUpstreamAccessError" />
    /// <seealso cref="ISubdirectoryAccessError" />
    /// <seealso cref="IAccessError&lt;IUpstreamSubdirectory&gt;" />
    public interface IUpstreamSubdirectoryAccessError : IUpstreamAccessError, ISubdirectoryAccessError, IAccessError<IUpstreamSubdirectory>
    {
        /// <summary>Gets the target subdirectory to which the access error applies.</summary>
        /// <value>The <typeparamref name="IUpstreamSubdirectory" /> entity that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Target), ResourceType = typeof(Properties.Resources))]
        new IUpstreamSubdirectory Target { get; }
    }

    /// <summary></summary>
    /// <seealso cref="IUpstreamDbEntity" />
    public interface ISubdirectoryAction : IUpstreamDbEntity
    {
        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Task), ResourceType = typeof(Properties.Resources))]
        IMitigationTask Task { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Source), ResourceType = typeof(Properties.Resources))]
        IUpstreamSubdirectory Source { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Target), ResourceType = typeof(Properties.Resources))]
        IUpstreamSubdirectory Target { get; }
    }

    /// <summary></summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="IBinaryPropertySet" />
    public interface IUpstreamBinaryPropertySet : IUpstreamDbEntity, IBinaryPropertySet
    {
        /// <summary>Gets the files which have the same length and cryptographic hash.</summary>
        /// <value>The files which have the same length and cryptographic hash..</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Files), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<IUpstreamFile> Files { get; }

        /// <summary>Gets the sets of files which were determined to be duplicates.</summary>
        /// <value>The sets of files which were determined to be duplicates.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_RedundantSets), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<IUpstreamRedundantSet> RedundantSets { get; }
    }

    /// <summary></summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="IPropertySet" />
    public interface IUpstreamPropertySet : IUpstreamDbEntity, IPropertySet
    {
        /// <summary>Gets the files that share the same property values as this property set.</summary>
        /// <value>The <see cref="IUpstreamFile">files</see> that share the same property values as this property set.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Files), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<IUpstreamFile> Files { get; }
    }

    /// <summary></summary>
    /// <seealso cref="IUpstreamPropertySet" />
    /// <seealso cref="ISummaryPropertySet" />
    public interface IUpstreamSummaryPropertySet : IUpstreamPropertySet, ISummaryPropertySet
    {
    }

    /// <summary></summary>
    /// <seealso cref="IUpstreamPropertySet" />
    /// <seealso cref="IDocumentPropertySet" />
    public interface IUpstreamDocumentPropertySet : IUpstreamPropertySet, IDocumentPropertySet
    {
    }

    /// <summary></summary>
    /// <seealso cref="IUpstreamPropertySet" />
    /// <seealso cref="IAudioPropertySet" />
    public interface IUpstreamAudioPropertySet : IUpstreamPropertySet, IAudioPropertySet
    {
    }

    /// <summary></summary>
    /// <seealso cref="IUpstreamPropertySet" />
    /// <seealso cref="IDRMPropertySet" />
    public interface IUpstreamDRMPropertySet : IUpstreamPropertySet, IDRMPropertySet
    {
    }

    /// <summary></summary>
    /// <seealso cref="IUpstreamPropertySet" />
    /// <seealso cref="IGPSPropertySet" />
    public interface IUpstreamGPSPropertySet : IUpstreamPropertySet, IGPSPropertySet
    {
    }

    /// <summary></summary>
    /// <seealso cref="IUpstreamPropertySet" />
    /// <seealso cref="IImagePropertySet" />
    public interface IUpstreamImagePropertySet : IUpstreamPropertySet, IImagePropertySet
    {
    }

    /// <summary></summary>
    /// <seealso cref="IUpstreamPropertySet" />
    /// <seealso cref="IMediaPropertySet" />
    public interface IUpstreamMediaPropertySet : IUpstreamPropertySet, IMediaPropertySet
    {
    }

    /// <summary></summary>
    /// <seealso cref="IUpstreamPropertySet" />
    /// <seealso cref="IMusicPropertySet" />
    public interface IUpstreamMusicPropertySet : IUpstreamPropertySet, IMusicPropertySet
    {
    }

    /// <summary></summary>
    /// <seealso cref="IUpstreamPropertySet" />
    /// <seealso cref="IPhotoPropertySet" />
    public interface IUpstreamPhotoPropertySet : IUpstreamPropertySet, IPhotoPropertySet
    {
    }

    /// <summary></summary>
    /// <seealso cref="IUpstreamPropertySet" />
    /// <seealso cref="IRecordedTVPropertySet" />
    public interface IUpstreamRecordedTVPropertySet : IUpstreamPropertySet, IRecordedTVPropertySet
    {
    }

    /// <summary></summary>
    /// <seealso cref="IUpstreamPropertySet" />
    /// <seealso cref="IVideoPropertySet" />
    public interface IUpstreamVideoPropertySet : IUpstreamPropertySet, IVideoPropertySet
    {
    }

    /// <summary></summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="IRedundantSet" />
    public interface IUpstreamRedundantSet : IUpstreamDbEntity, IRedundantSet
    {
        /// <summary>Gets the binary properties in common with all files in the current redundant set.</summary>
        /// <value>The binary properties in common with all files in the current redundant set.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_BinaryProperties), ResourceType = typeof(Properties.Resources))]
        new IUpstreamBinaryPropertySet BinaryProperties { get; }

        /// <summary>Gets the redundancy entities which represent links to redundant files.</summary>
        /// <value>The redundancy entities which represent links to redundant files.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Redundancies), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<IUpstreamRedundancy> Redundancies { get; }
    }

    /// <summary></summary>
    /// <seealso cref="IUpstreamDbFsItem" />
    /// <seealso cref="IFile" />
    public interface IUpstreamFile : IUpstreamDbFsItem, IFile
    {
        /// <summary>Gets the binary properties for the current file.</summary>
        /// <value>The generic <see cref="IUpstreamBinaryPropertySet" /> that contains the file size and optionally, the <see cref="MD5Hash">MD5 hash</see> value of its binary contents.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_BinaryProperties), ResourceType = typeof(Properties.Resources))]
        new IUpstreamBinaryPropertySet BinaryProperties { get; }

        /// <summary>Gets the summary properties for the current file.</summary>
        /// <value>The generic <see cref="IUpstreamBinaryPropertySet" /> that contains the summary properties for the current file or <see langword="null" /> if no summary properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_SummaryProperties), ResourceType = typeof(Properties.Resources))]
        new IUpstreamSummaryPropertySet SummaryProperties { get; }

        /// <summary>Gets the document properties for the current file.</summary>
        /// <value>The generic <see cref="IUpstreamDocumentPropertySet" /> that contains the document properties for the current file or <see langword="null" /> if no document properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_DocumentProperties), ResourceType = typeof(Properties.Resources))]
        new IUpstreamDocumentPropertySet DocumentProperties { get; }

        /// <summary>Gets the audio properties for the current file.</summary>
        /// <value>The generic <see cref="IUpstreamAudioPropertySet" /> that contains the audio properties for the current file or <see langword="null" /> if no audio properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AudioProperties), ResourceType = typeof(Properties.Resources))]
        new IUpstreamAudioPropertySet AudioProperties { get; }

        /// <summary>Gets the DRM properties for the current file.</summary>
        /// <value>The generic <see cref="IUpstreamDRMPropertySet" /> that contains the DRM properties for the current file or <see langword="null" /> if no DRM properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_DRMProperties), ResourceType = typeof(Properties.Resources))]
        new IUpstreamDRMPropertySet DRMProperties { get; }

        /// <summary>Gets the GPS properties for the current file.</summary>
        /// <value>The generic <see cref="IUpstreamGPSPropertySet" /> that contains the GPS properties for the current file or <see langword="null" /> if no GPS properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_GPSProperties), ResourceType = typeof(Properties.Resources))]
        new IUpstreamGPSPropertySet GPSProperties { get; }

        /// <summary>Gets the image properties for the current file.</summary>
        /// <value>The generic <see cref="IUpstreamImagePropertySet" /> that contains the image properties for the current file or <see langword="null" /> if no image properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_ImageProperties), ResourceType = typeof(Properties.Resources))]
        new IUpstreamImagePropertySet ImageProperties { get; }

        /// <summary>Gets the media properties for the current file.</summary>
        /// <value>The generic <see cref="IUpstreamMediaPropertySet" /> that contains the media properties for the current file or <see langword="null" /> if no media properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_MediaProperties), ResourceType = typeof(Properties.Resources))]
        new IUpstreamMediaPropertySet MediaProperties { get; }

        /// <summary>Gets the music properties for the current file.</summary>
        /// <value>The generic <see cref="IUpstreamMusicPropertySet" /> that contains the music properties for the current file or <see langword="null" /> if no music properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_MusicProperties), ResourceType = typeof(Properties.Resources))]
        new IUpstreamMusicPropertySet MusicProperties { get; }

        /// <summary>Gets the photo properties for the current file.</summary>
        /// <value>The generic <see cref="IUpstreamPhotoPropertySet" /> that contains the photo properties for the current file or <see langword="null" /> if no photo properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_PhotoProperties), ResourceType = typeof(Properties.Resources))]
        new IUpstreamPhotoPropertySet PhotoProperties { get; }

        /// <summary>Gets the recorded tv properties for the current file.</summary>
        /// <value>The generic <see cref="IUpstreamRecordedTVPropertySet" /> that contains the recorded TV properties for the current file or <see langword="null" /> if no recorded TV properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_RecordedTVProperties), ResourceType = typeof(Properties.Resources))]
        new IUpstreamRecordedTVPropertySet RecordedTVProperties { get; }

        /// <summary>Gets the video properties for the current file.</summary>
        /// <value>The generic <see cref="IUpstreamVideoPropertySet" /> that contains the video properties for the current file or <see langword="null" /> if no video properties are defined on the current file.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_VideoProperties), ResourceType = typeof(Properties.Resources))]
        new IUpstreamVideoPropertySet VideoProperties { get; }

        /// <summary>Gets the redundancy item that indicates the membership of a collection of redundant files.</summary>
        /// <value>
        /// A <see cref="IUpstreamRedundancy" /> object that indicates the current file is an exact copy of other files that belong to the same <see cref="IRedundancy.RedundantSet" />
        /// or <see langword="null" /> if this file has not been identified as being redundant with any other.
        /// </value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Redundancy), ResourceType = typeof(Properties.Resources))]
        new IUpstreamRedundancy Redundancy { get; }

        /// <summary>Gets the comparisons where the current file was the <see cref="IComparison.Baseline" />.</summary>
        /// <value>The <see cref="IUpstreamComparison" /> entities where the current file is the <see cref="IComparison.Baseline" />.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_BaselineComparisons), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<IUpstreamComparison> BaselineComparisons { get; }

        /// <summary>Gets the comparisons where the current file was the <see cref="IComparison.Correlative" /> being compared to a separate <see cref="IComparison.Baseline" /> file.</summary>
        /// <value>The <see cref="IUpstreamComparison" /> entities where the current file is the <see cref="IComparison.Correlative" />.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_CorrelativeComparisons), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<IUpstreamComparison> CorrelativeComparisons { get; }

        /// <summary>Gets the access errors for the current file system item.</summary>
        /// <value>The access errors for the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<IUpstreamFileAccessError> AccessErrors { get; }
    }

    /// <summary></summary>
    /// <seealso cref="IUpstreamDbEntity" />
    public interface IFileAction : IUpstreamDbEntity
    {
        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Task), ResourceType = typeof(Properties.Resources))]
        IMitigationTask Task { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Source), ResourceType = typeof(Properties.Resources))]
        IUpstreamFile Source { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Target), ResourceType = typeof(Properties.Resources))]
        IUpstreamSubdirectory Target { get; }
    }

    /// <summary></summary>
    /// <typeparam name="TTarget">The target entity type.</typeparam>
    /// <seealso cref="IUpstreamAccessError" />
    /// <seealso cref="IFileAccessError" />
    /// <seealso cref="IAccessError&lt;IUpstreamFile&gt;" />
    public interface IUpstreamFileAccessError : IUpstreamAccessError, IFileAccessError, IAccessError<IUpstreamFile>
    {
        /// <summary>Gets the target file to which the access error applies.</summary>
        /// <value>The <typeparamref name="IUpstreamFile" /> entity that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Target), ResourceType = typeof(Properties.Resources))]
        new IUpstreamFile Target { get; }
    }

    /// <summary></summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="IRedundancy" />
    public interface IUpstreamRedundancy : IUpstreamDbEntity, IRedundancy
    {
        /// <summary>Gets the file that belongs to the redundancy set.</summary>
        /// <value>The file that belongs to the redundancy set.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_File), ResourceType = typeof(Properties.Resources))]
        new IUpstreamFile File { get; }

        /// <summary>Gets the redundancy set.</summary>
        /// <value>The redundancy set.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_RedundantSet), ResourceType = typeof(Properties.Resources))]
        new IUpstreamRedundantSet RedundantSet { get; }
    }

    /// <summary></summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="IComparison" />
    public interface IUpstreamComparison : IUpstreamDbEntity, IComparison
    {
        /// <summary>Gets the baseline file in the comparison.</summary>
        /// <value>The generic <see cref="IUpstreamFile" /> that represents the baseline file in the comparison.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Baseline), ResourceType = typeof(Properties.Resources))]
        new IUpstreamFile Baseline { get; }

        /// <summary>Gets the correlative file in the comparison.</summary>
        /// <value>The generic <see cref="IUpstreamFile" /> that represents the correlative file, which is the new or changed file in the comparison.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Correlative), ResourceType = typeof(Properties.Resources))]
        new IUpstreamFile Correlative { get; }
    }
}

