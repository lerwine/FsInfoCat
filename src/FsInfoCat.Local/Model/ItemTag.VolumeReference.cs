using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model;

public abstract partial class ItemTag
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    protected class VolumeReference : ForeignKeyReference<Volume>, IForeignKeyReference<ILocalVolume>, IForeignKeyReference<IVolume>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        internal VolumeReference(object syncRoot) : base(syncRoot) { }

        ILocalVolume IForeignKeyReference<ILocalVolume>.Entity => Entity;

        IVolume IForeignKeyReference<IVolume>.Entity => Entity;

        bool IEquatable<IForeignKeyReference<IVolume>>.Equals(IForeignKeyReference<IVolume> other)
        {
            throw new NotImplementedException();
        }

        bool IEquatable<IForeignKeyReference<ILocalVolume>>.Equals(IForeignKeyReference<ILocalVolume> other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj) => obj is not null && ((obj is ForeignKeyReference<Volume> other) ? Equals(other) : base.Equals(obj));
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
