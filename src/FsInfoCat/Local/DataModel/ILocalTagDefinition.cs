using System.Collections.Generic;

namespace FsInfoCat.Local
{
    public interface ILocalTagDefinitionRow : ILocalDbEntity, ITagDefinitionRow { }

    public interface ILocalTagDefinitionListItem : ITagDefinitionListItem, ILocalTagDefinitionRow { }

    public interface ILocalTagDefinition : ILocalTagDefinitionRow, ITagDefinition
    {
        new IEnumerable<ILocalFileTag> FileTags { get; }

        new IEnumerable<ILocalSubdirectoryTag> SubdirectoryTags { get; }

        new IEnumerable<ILocalVolumeTag> VolumeTags { get; }
    }
}
