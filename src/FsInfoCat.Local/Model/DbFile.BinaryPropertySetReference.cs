using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model;

public partial class DbFile
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    // CodeQL [cs/inconsistent-equals-and-gethashcode]: GetHashCode() of base class is sufficient
    protected class BinaryPropertySetReference : ForeignKeyReference<BinaryPropertySet>, IForeignKeyReference<ILocalBinaryPropertySet>, IForeignKeyReference<IBinaryPropertySet>
    {
        internal BinaryPropertySetReference(object syncRoot) : base(syncRoot) { }

        ILocalBinaryPropertySet IForeignKeyReference<ILocalBinaryPropertySet>.Entity => Entity;

        IBinaryPropertySet IForeignKeyReference<IBinaryPropertySet>.Entity => Entity;

        bool IEquatable<IForeignKeyReference<ILocalBinaryPropertySet>>.Equals(IForeignKeyReference<ILocalBinaryPropertySet> other)
        {
            throw new NotImplementedException();
        }

        bool IEquatable<IForeignKeyReference<IBinaryPropertySet>>.Equals(IForeignKeyReference<IBinaryPropertySet> other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj) => obj is not null && ((obj is ForeignKeyReference<BinaryPropertySet> other) ? Equals(other) : base.Equals(obj));
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
