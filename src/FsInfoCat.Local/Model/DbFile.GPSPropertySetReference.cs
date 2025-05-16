using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model;

public partial class DbFile
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    protected class GPSPropertySetReference : ForeignKeyReference<GPSPropertySet>, IForeignKeyReference<ILocalGPSPropertySet>, IForeignKeyReference<IGPSPropertySet>
    {
        internal GPSPropertySetReference(object syncRoot) : base(syncRoot) { }

        ILocalGPSPropertySet IForeignKeyReference<ILocalGPSPropertySet>.Entity => Entity;

        IGPSPropertySet IForeignKeyReference<IGPSPropertySet>.Entity => Entity;

        bool IEquatable<IForeignKeyReference<IGPSPropertySet>>.Equals(IForeignKeyReference<IGPSPropertySet> other)
        {
            throw new NotImplementedException();
        }

        bool IEquatable<IForeignKeyReference<ILocalGPSPropertySet>>.Equals(IForeignKeyReference<ILocalGPSPropertySet> other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj) => obj is not null && ((obj is ForeignKeyReference<GPSPropertySet> other) ? Equals(other) : base.Equals(obj));
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
