using System;

namespace FsInfoCat
{
    public interface IItemTagListItem : IItemTagRow, IEquatable<IItemTagListItem>
    {
        string Name { get; }

        string Description { get; }
    }
}
