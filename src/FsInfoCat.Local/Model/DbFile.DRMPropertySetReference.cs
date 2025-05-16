using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model;

public partial class DbFile
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    protected class DRMPropertySetReference : ForeignKeyReference<DRMPropertySet>, IForeignKeyReference<ILocalDRMPropertySet>, IForeignKeyReference<IDRMPropertySet>
    {
        internal DRMPropertySetReference(object syncRoot) : base(syncRoot) { }

        ILocalDRMPropertySet IForeignKeyReference<ILocalDRMPropertySet>.Entity => Entity;

        IDRMPropertySet IForeignKeyReference<IDRMPropertySet>.Entity => Entity;

        bool IEquatable<IForeignKeyReference<ILocalDRMPropertySet>>.Equals(IForeignKeyReference<ILocalDRMPropertySet> other)
        {
            throw new NotImplementedException();
        }

        bool IEquatable<IForeignKeyReference<IDRMPropertySet>>.Equals(IForeignKeyReference<IDRMPropertySet> other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj) => obj is not null && ((obj is ForeignKeyReference<DRMPropertySet> other) ? Equals(other) : base.Equals(obj));
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
