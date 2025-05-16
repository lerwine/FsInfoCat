using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model;

public partial class DbFile
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    // CodeQL [cs/inconsistent-equals-and-gethashcode]: GetHashCode() of base class is sufficient
    protected class MediaPropertySetReference : ForeignKeyReference<MediaPropertySet>, IForeignKeyReference<ILocalMediaPropertySet>, IForeignKeyReference<IMediaPropertySet>
    {
        internal MediaPropertySetReference(object syncRoot) : base(syncRoot) { }

        ILocalMediaPropertySet IForeignKeyReference<ILocalMediaPropertySet>.Entity => Entity;

        IMediaPropertySet IForeignKeyReference<IMediaPropertySet>.Entity => Entity;

        bool IEquatable<IForeignKeyReference<IMediaPropertySet>>.Equals(IForeignKeyReference<IMediaPropertySet> other)
        {
            throw new NotImplementedException();
        }

        bool IEquatable<IForeignKeyReference<ILocalMediaPropertySet>>.Equals(IForeignKeyReference<ILocalMediaPropertySet> other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj) => obj is not null && ((obj is ForeignKeyReference<MediaPropertySet> other) ? Equals(other) : base.Equals(obj));
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
