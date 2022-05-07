using System.Collections.Generic;

namespace FsInfoCat.Local
{
    public interface ILocalSharedTagDefinition : ILocalTagDefinition, ISharedTagDefinition
    {
        new IEnumerable<ILocalSharedFileTag> FileTags { get; }

        new IEnumerable<ILocalSharedSubdirectoryTag> SubdirectoryTags { get; }

        new IEnumerable<ILocalSharedVolumeTag> VolumeTags { get; }
    }
}
