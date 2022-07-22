using FsInfoCat.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Describes an action to be taken on a sub-directory.
    /// </summary>
    /// <seealso cref="ISubdirectoryActionRow" />
    /// <seealso cref="IEquatable{ISubdirectoryAction}" />
    public interface ISubdirectoryAction : ISubdirectoryActionRow, IEquatable<ISubdirectoryAction>
    {
        /// <summary>
        /// Gets the parent mitigation task.
        /// </summary>
        /// <value>The parent <see cref="IMitigationTask">mitigation task</see>.</value>
        [Display(Name = nameof(Properties.Resources.MitigationTask), ShortName = nameof(Properties.Resources.Task),
            Description = nameof(Properties.Resources.Description_SubdirectoryActionTask), ResourceType = typeof(Properties.Resources))]
        IMitigationTask Task { get; }

        /// <summary>
        /// Gets the source sub-directory.
        /// </summary>
        /// <value>The source subdirectory to be deleted, moved or copied.</value>
        [Display(Name = nameof(Properties.Resources.SourceSubdirectory), ShortName = nameof(Properties.Resources.Source),
            Description = nameof(Properties.Resources.Description_SourceSubdirectory), ResourceType = typeof(Properties.Resources))]
        IUpstreamSubdirectory Source { get; }

        /// <summary>
        /// Gets the destination subdirectory.
        /// </summary>
        /// <value>The destination <see cref="IUpstreamSubdirectory"/> or <see langword="null"/> if the subdirectory does not get copied or moved.</value>
        /// <remarks>
        /// The <see cref="DirectoryCrawlOptions.FlaggedForDeletion" /> <see cref="ISubdirectoryRow.Options">file options flag</see> combined with the value of this field determines
        /// whether a file should be deleted, moved or copied, as follows:
        /// <list type="table">
        /// <item>
        ///     <description><strong>Destination</strong></description>
        ///     <description><strong>Options</strong></description>
        ///     <description><strong>Action</strong></description>
        /// </item>
        /// <item>
        ///     <description><see langword="null"/></description>
        ///     <description><see cref="DirectoryCrawlOptions.FlaggedForDeletion" /> <see cref="ISubdirectoryRow.Options">options flag</see> is set:</description>
        ///     <description>Delete</description>
        /// </item>
        /// <item>
        ///     <description>Not <see langword="null"/></description>
        ///     <description><see cref="DirectoryCrawlOptions.FlaggedForDeletion" /> <see cref="ISubdirectoryRow.Options">options flag</see> is set:</description>
        ///     <description>Move</description>
        /// </item>
        /// <item>
        ///     <description>Not <see langword="null"/></description>
        ///     <description><see cref="DirectoryCrawlOptions.FlaggedForDeletion" /> <see cref="ISubdirectoryRow.Options">options flag</see> is not set.</description>
        ///     <description>Copy</description>
        /// </item>
        /// </list></remarks>
        [Display(Name = nameof(Properties.Resources.DestinationSubdirectory), ShortName = nameof(Properties.Resources.Destination),
            Description = nameof(Properties.Resources.Description_SubdirectoryActionDestination), ResourceType = typeof(Properties.Resources))]
        IUpstreamSubdirectory Destination { get; }
    }
}
