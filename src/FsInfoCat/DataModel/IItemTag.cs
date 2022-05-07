using System;

namespace FsInfoCat
{
    public interface IItemTag : IItemTagRow
    {
        IDbEntity Tagged { get; }

        ITagDefinition Definition { get; }

        bool TryGetTaggedId(out Guid taggedId);

        bool TryGetDefinitionId(out Guid definitionId);
    }
}
