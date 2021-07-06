using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>
    /// Indicates status of files represented within the <see cref="IDbContext">local database</see>, as it pertains to local file processing.
    /// </summary>
    public enum FileCorrelationStatus : byte
    {
        /// <summary>
        /// File has been added to the database or modified and needs to be analyzed.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_RemediationStatus_Dissociated), Description = nameof(Properties.Resources.Description_RemediationStatus_Dissociated), ResourceType = typeof(Properties.Resources))]
        Dissociated = 0,

        /// <summary>
        /// Acknowledged; Validation in progress.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_RemediationStatus_PendingValidation), Description = nameof(Properties.Resources.Description_RemediationStatus_PendingValidation), ResourceType = typeof(Properties.Resources))]
        PendingValidation = 1,

        /// <summary>
        /// Final determination deferred.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_RemediationStatus_Deferred), Description = nameof(Properties.Resources.Description_RemediationStatus_Deferred), ResourceType = typeof(Properties.Resources))]
        Deferred = 2,

        /// <summary>
        /// Correlations to other files are being established.
        /// </summary>
        /// <remarks>This status value is only for new files and those which have changed. Results of comparisons will be stored in the <see cref="IComparison"/> table, where the <see cref="IComparison.Correlative"/>
        /// is the new or changed file, and the <see cref="IComparison.Baseline"/> is the file that it is being compared to.</remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_RemediationStatus_Analyzing), Description = nameof(Properties.Resources.Description_RemediationStatus_Analyzing), ResourceType = typeof(Properties.Resources))]
        Analyzing = 3,

        /// <summary>
        /// File has been compared with all other files with the same Hash. Final disposition pending.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_RemediationStatus_Correlated), Description = nameof(Properties.Resources.Description_RemediationStatus_Correlated), ResourceType = typeof(Properties.Resources))]
        Correlated = 4,

        /// <summary>
        /// File duplicaton is justified.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_RemediationStatus_Justified), Description = nameof(Properties.Resources.Description_RemediationStatus_Justified), ResourceType = typeof(Properties.Resources))]
        Justified = 5,

        /// <summary>
        /// Indicates that the files should be deleted.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_RemediationStatus_Insupportable), Description = nameof(Properties.Resources.Description_RemediationStatus_Insupportable), ResourceType = typeof(Properties.Resources))]
        Insupportable = 6,

        /// <summary>
        /// Elevated status for unauthorized copies of files that need further attention.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_RemediationStatus_Violation), Description = nameof(Properties.Resources.Description_RemediationStatus_Violation), ResourceType = typeof(Properties.Resources))]
        Violation = 7,

        /// <summary>
        /// Indicates that the file has been scheduled/tasked to be deleted, but not yet confirmed.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_RemediationStatus_Attrition), Description = nameof(Properties.Resources.Description_RemediationStatus_Attrition), ResourceType = typeof(Properties.Resources))]
        Attrition = 8,

        /// <summary>
        /// The file has been deleted.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_RemediationStatus_FileDeleted), Description = nameof(Properties.Resources.Description_RemediationStatus_FileDeleted), ResourceType = typeof(Properties.Resources))]
        Deleted = 9
    }
}
