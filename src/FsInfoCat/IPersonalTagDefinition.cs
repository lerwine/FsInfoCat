using System;
using System.Collections.Generic;

namespace FsInfoCat
{
    public interface IPersonalTagDefinition : ITagDefinition, IEquatable<IPersonalTagDefinition>
    {
        new IEnumerable<IPersonalFileTag> FileTags { get; }

        new IEnumerable<IPersonalSubdirectoryTag> SubdirectoryTags { get; }

        new IEnumerable<IPersonalVolumeTag> VolumeTags { get; }
    }
}
