using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model;

public partial class Redundancy
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    // CodeQL [cs/inconsistent-equals-and-gethashcode]: GetHashCode() of base class is sufficient
    protected class RedundantSetReference : ForeignKeyReference<RedundantSet>, IForeignKeyReference<ILocalRedundantSet>, IForeignKeyReference<IRedundantSet>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        internal RedundantSetReference(object syncRoot) : base(syncRoot) { }

        ILocalRedundantSet IForeignKeyReference<ILocalRedundantSet>.Entity => Entity;

        IRedundantSet IForeignKeyReference<IRedundantSet>.Entity => Entity;

        bool IEquatable<IForeignKeyReference<IRedundantSet>>.Equals(IForeignKeyReference<IRedundantSet> other)
        {
            throw new NotImplementedException();
        }

        bool IEquatable<IForeignKeyReference<ILocalRedundantSet>>.Equals(IForeignKeyReference<ILocalRedundantSet> other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj) => obj is not null && ((obj is ForeignKeyReference<RedundantSet> other) ? Equals(other) : base.Equals(obj));
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
