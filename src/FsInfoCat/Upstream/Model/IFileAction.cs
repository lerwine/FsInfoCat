using FsInfoCat.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Describes an action to be taken on a file.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="IEquatable{IFileAction}" />
    public interface IFileAction : IUpstreamDbEntity, IEquatable<IFileAction>
    {
        /// <summary>
        /// Gets the parent mitigation task.
        /// </summary>
        /// <value>The parent <see cref="IMitigationTask">mitigation task</see>.</value>
        [Display(Name = nameof(Properties.Resources.MitigationTask), ShortName = nameof(Properties.Resources.Task),
            Description = nameof(Properties.Resources.Description_FileActionTask), ResourceType = typeof(Properties.Resources))]
        IMitigationTask Task { get; }

        /// <summary>
        /// Gets the source file.
        /// </summary>
        /// <value>The source file that needs to be deleted, moved or copied.</value>
        [Display(Name = nameof(Properties.Resources.SourceFile), ShortName = nameof(Properties.Resources.Source),
            Description = nameof(Properties.Resources.Description_SourceFile), ResourceType = typeof(Properties.Resources))]
        IUpstreamFile Source { get; }

        /// <summary>
        /// Gets the destination subdirectory.
        /// </summary>
        /// <value>
        /// The destination <see cref="IUpstreamSubdirectory" /> or <see langword="null"/> if the file does not get copied or moved.
        /// </value>
        /// <remarks>
        /// The <see cref="FileCrawlOptions.FlaggedForDeletion" /> <see cref="IFileRow.Options">file options flag</see> and the <see cref="IFileRow.Status">file status</see> combined
        /// with the value of this field
        /// determines whether a file should be deleted, moved or copied, as follows:
        /// <list type="table">
        /// <item>
        ///     <description><strong>Destination</strong></description>
        ///     <description><strong>Options/Status</strong></description>
        ///     <description><strong>Action</strong></description>
        /// </item>
        /// <item>
        ///     <description><see langword="null"/></description>
        ///     <description><see cref="FileCrawlOptions.FlaggedForDeletion" /> <see cref="IFileRow.Options">options flag</see> is set or <see cref="IFileRow.Status" /> is
        ///         not <see cref="FileCorrelationStatus.Justified"/>.</description>
        ///     <description>Delete</description>
        /// </item>
        /// <item>
        ///     <description>Not <see langword="null"/></description>
        ///     <description><see cref="FileCrawlOptions.FlaggedForDeletion" /> <see cref="IFileRow.Options">options flag</see> is set or <see cref="IFileRow.Status" /> is
        ///         not <see cref="FileCorrelationStatus.Justified"/>.</description>
        ///     <description>Move</description>
        /// </item>
        /// <item>
        ///     <description>Not <see langword="null"/></description>
        ///     <description><see cref="IFileRow.Status" /> is <see cref="FileCorrelationStatus.Justified"/> and <see cref="IFileRow.Options">options flag</see> is not
        ///         set.</description>
        ///     <description>Copy</description>
        /// </item>
        /// </list></remarks>
        [Display(Name = nameof(Properties.Resources.DestinationSubdirectory), ShortName = nameof(Properties.Resources.Destination),
            Description = nameof(Properties.Resources.Description_DestinationSubdirectory), ResourceType = typeof(Properties.Resources))]
        IUpstreamSubdirectory Destination { get; }
    }
}
