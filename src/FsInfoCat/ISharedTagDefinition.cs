using System.Collections.Generic;

namespace FsInfoCat
{
    public interface ISharedTagDefinition : ITagDefinition
    {
        new IEnumerable<ISharedFileTag> FileTags { get; }

        new IEnumerable<ISharedSubdirectoryTag> SubdirectoryTags { get; }

        new IEnumerable<ISharedVolumeTag> VolumeTags { get; }
    }
}
