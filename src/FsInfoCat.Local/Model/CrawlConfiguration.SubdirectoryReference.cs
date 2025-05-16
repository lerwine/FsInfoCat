using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model;

public partial class CrawlConfiguration
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    // CodeQL [cs/inconsistent-equals-and-gethashcode]: GetHashCode() of base class is sufficient
    protected class SubdirectoryReference : ForeignKeyReference<Subdirectory>, IForeignKeyReference<ILocalSubdirectory>, IForeignKeyReference<ISubdirectory>
    {
        internal SubdirectoryReference(object syncRoot) : base(syncRoot) { }

        ILocalSubdirectory IForeignKeyReference<ILocalSubdirectory>.Entity => Entity;

        ISubdirectory IForeignKeyReference<ISubdirectory>.Entity => Entity;

        bool IEquatable<IForeignKeyReference<ILocalSubdirectory>>.Equals(IForeignKeyReference<ILocalSubdirectory> other)
        {
            throw new NotImplementedException();
        }

        bool IEquatable<IForeignKeyReference<ISubdirectory>>.Equals(IForeignKeyReference<ISubdirectory> other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj) => obj is not null && ((obj is ForeignKeyReference<Subdirectory> other) ? Equals(other) : base.Equals(obj));
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
