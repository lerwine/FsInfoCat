using System;

namespace FsInfoCat
{
    public interface ITagDefinitionListItem : ITagDefinitionRow, IEquatable<ITagDefinitionListItem>
    {
        long FileTagCount { get; }

        long SubdirectoryTagCount { get; }

        long VolumeTagCount { get; }
    }
}
