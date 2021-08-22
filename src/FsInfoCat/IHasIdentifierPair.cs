using System;

namespace FsInfoCat
{
    public interface IHasIdentifierPair : IHasCompoundIdentifier
    {
        new ValueTuple<Guid, Guid> Id { get; }
    }
}
