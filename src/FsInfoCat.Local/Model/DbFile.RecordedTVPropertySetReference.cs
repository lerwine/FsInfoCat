using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model;

public partial class DbFile
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    // CodeQL [cs/inconsistent-equals-and-gethashcode]: GetHashCode() of base class is sufficient
    protected class RecordedTVPropertySetReference : ForeignKeyReference<RecordedTVPropertySet>, IForeignKeyReference<ILocalRecordedTVPropertySet>, IForeignKeyReference<IRecordedTVPropertySet>
    {
        internal RecordedTVPropertySetReference(object syncRoot) : base(syncRoot) { }

        ILocalRecordedTVPropertySet IForeignKeyReference<ILocalRecordedTVPropertySet>.Entity => Entity;

        IRecordedTVPropertySet IForeignKeyReference<IRecordedTVPropertySet>.Entity => Entity;

        bool IEquatable<IForeignKeyReference<IRecordedTVPropertySet>>.Equals(IForeignKeyReference<IRecordedTVPropertySet> other)
        {
            throw new NotImplementedException();
        }

        bool IEquatable<IForeignKeyReference<ILocalRecordedTVPropertySet>>.Equals(IForeignKeyReference<ILocalRecordedTVPropertySet> other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj) => obj is not null && ((obj is ForeignKeyReference<RecordedTVPropertySet> other) ? Equals(other) : base.Equals(obj));
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
