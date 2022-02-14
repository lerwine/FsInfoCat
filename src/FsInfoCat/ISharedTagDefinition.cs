using System;
using System.Collections.Generic;

namespace FsInfoCat
{
    public interface ISharedTagDefinition : ITagDefinition, IEquatable<ISharedTagDefinition>
    {
        new IEnumerable<ISharedFileTag> FileTags { get; }

        new IEnumerable<ISharedSubdirectoryTag> SubdirectoryTags { get; }

        new IEnumerable<ISharedVolumeTag> VolumeTags { get; }
    }
}
