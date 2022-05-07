using System.Collections.Generic;

namespace FsInfoCat.Upstream
{
    public interface IUpstreamTagDefinitionRow : IUpstreamDbEntity, ITagDefinitionRow { }

    public interface IUpstreamTagDefinitionListItem : ITagDefinitionListItem, IUpstreamTagDefinitionRow { }

    public interface IUpstreamTagDefinition : IUpstreamTagDefinitionRow, ITagDefinition
    {
        new IEnumerable<IUpstreamFileTag> FileTags { get; }

        new IEnumerable<IUpstreamSubdirectoryTag> SubdirectoryTags { get; }

        new IEnumerable<IUpstreamVolumeTag> VolumeTags { get; }
    }
}
