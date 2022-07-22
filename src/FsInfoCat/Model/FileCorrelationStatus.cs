using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Represents correlation status of a file.
    /// </summary>
    public enum FileCorrelationStatus : byte
    {
        /// <summary>
        /// File has been added to the database or modified and needs to be analyzed.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.Dissociated), ShortName = nameof(Properties.Resources.Dissociated),
            Description = nameof(Properties.Resources.FileDissociated), ResourceType = typeof(Properties.Resources))]
        Dissociated = 0,

        /// <summary>
        /// Acknowledged; Validation in progress.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.PendingValidation), ShortName = nameof(Properties.Resources.Pending),
            Description = nameof(Properties.Resources.FilePendingValidation), ResourceType = typeof(Properties.Resources))]
        PendingValidation = 1,

        /// <summary>
        /// Final determination deferred.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.ValidationDeferred), ShortName = nameof(Properties.Resources.Deferred),
            Description = nameof(Properties.Resources.FileValidationDeferred), ResourceType = typeof(Properties.Resources))]
        Deferred = 2,

        /// <summary>
        /// Correlations to other files are being established.
        /// </summary>
        /// <remarks>
        /// This status value is only for new files and those which have changed. Results of comparisons will be stored in the <see cref="IComparison" /> table,
        /// where the <see cref="IComparison.Correlative" /> is the new or changed file, and the <see cref="IComparison.Baseline" /> is the file that it is being compared to.
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.Analyzing), ShortName = nameof(Properties.Resources.Analyzing),
            Description = nameof(Properties.Resources.AnalyzingFile), ResourceType = typeof(Properties.Resources))]
        Analyzing = 3,

        /// <summary>
        /// File has been compared with all other files with the same Hash. Final disposition pending.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.Correlated), ShortName = nameof(Properties.Resources.Correlated),
            Description = nameof(Properties.Resources.FileCorrelated), ResourceType = typeof(Properties.Resources))]
        Correlated = 4,

        /// <summary>
        /// File duplicaton is justified.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DuplicationJustified), ShortName = nameof(Properties.Resources.Justified),
            Description = nameof(Properties.Resources.FileDuplicationJustified), ResourceType = typeof(Properties.Resources))]
        Justified = 5,

        /// <summary>
        /// Indicates that the files should be deleted.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.Insupportable), ShortName = nameof(Properties.Resources.Insupportable),
            Description = nameof(Properties.Resources.FilesInsupportable), ResourceType = typeof(Properties.Resources))]
        Insupportable = 6,

        /// <summary>
        /// Elevated status for unauthorized copies of files that need further attention.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.Violation_Unauthorized), ShortName = nameof(Properties.Resources.Violation),
            Description = nameof(Properties.Resources.Description_ViolationUnauthorized), ResourceType = typeof(Properties.Resources))]
        Violation = 7,

        /// <summary>
        /// Indicates that the file has been scheduled/tasked to be deleted, but not yet confirmed.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.Attrition), ShortName = nameof(Properties.Resources.ToDelete),
            Description = nameof(Properties.Resources.FilesMarkedForAttrition), ResourceType = typeof(Properties.Resources))]
        Attrition = 8,

        /// <summary>
        /// The file has been deleted.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.Deleted), ShortName = nameof(Properties.Resources.Deleted),
            Description = nameof(Properties.Resources.FileDeleted), ResourceType = typeof(Properties.Resources))]
        Deleted = 9,

        // TODO: Document FileCorrelationStatus field
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        AccessError = 10
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}

