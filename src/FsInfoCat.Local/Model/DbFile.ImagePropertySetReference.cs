using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model;

public partial class DbFile
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    // CodeQL [cs/inconsistent-equals-and-gethashcode]: GetHashCode() of base class is sufficient
    protected class ImagePropertySetReference : ForeignKeyReference<ImagePropertySet>, IForeignKeyReference<ILocalImagePropertySet>, IForeignKeyReference<IImagePropertySet>
    {
        internal ImagePropertySetReference(object syncRoot) : base(syncRoot) { }

        ILocalImagePropertySet IForeignKeyReference<ILocalImagePropertySet>.Entity => Entity;

        IImagePropertySet IForeignKeyReference<IImagePropertySet>.Entity => Entity;

        bool IEquatable<IForeignKeyReference<IImagePropertySet>>.Equals(IForeignKeyReference<IImagePropertySet> other)
        {
            throw new NotImplementedException();
        }

        bool IEquatable<IForeignKeyReference<ILocalImagePropertySet>>.Equals(IForeignKeyReference<ILocalImagePropertySet> other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj) => obj is not null && ((obj is ForeignKeyReference<ImagePropertySet> other) ? Equals(other) : base.Equals(obj));
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
