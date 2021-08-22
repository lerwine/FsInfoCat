using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    public interface ITagDefinition : IDbEntity, IHasSimpleIdentifier
    {
        /// <summary>Gets the name of item tag.</summary>
        /// <value>The name of the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Name), ResourceType = typeof(Properties.Resources))]
        string Name { get; }

        /// <summary>Gets the description of the current tag.</summary>
        /// <value>The custom description of the current tag.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Description), ResourceType = typeof(Properties.Resources))]
        string Description { get; }

        /// <summary>Gets a value indicating whether the tag is inactive (archived).</summary>
        /// <value><see langword="true"/> if the curren tag is inactive; otherwise, <see langword="false"/>.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Description), ResourceType = typeof(Properties.Resources))]
        bool IsInactive { get; }

        IEnumerable<IFileTag> FileTags { get; }

        IEnumerable<ISubdirectoryTag> SubdirectoryTags { get; }

        IEnumerable<IVolumeTag> VolumeTags { get; }
    }
}
