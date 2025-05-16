using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model;

public partial class DbFile
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    // CodeQL [cs/inconsistent-equals-and-gethashcode]: GetHashCode() of base class is sufficient
    protected class SummaryPropertySetReference : ForeignKeyReference<SummaryPropertySet>, IForeignKeyReference<ILocalSummaryPropertySet>, IForeignKeyReference<ISummaryPropertySet>
    {
        internal SummaryPropertySetReference(object syncRoot) : base(syncRoot) { }

        ILocalSummaryPropertySet IForeignKeyReference<ILocalSummaryPropertySet>.Entity => Entity;

        ISummaryPropertySet IForeignKeyReference<ISummaryPropertySet>.Entity => Entity;

        bool IEquatable<IForeignKeyReference<ILocalSummaryPropertySet>>.Equals(IForeignKeyReference<ILocalSummaryPropertySet> other)
        {
            throw new NotImplementedException();
        }

        bool IEquatable<IForeignKeyReference<ISummaryPropertySet>>.Equals(IForeignKeyReference<ISummaryPropertySet> other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj) => obj is not null && ((obj is ForeignKeyReference<SummaryPropertySet> other) ? Equals(other) : base.Equals(obj));
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
