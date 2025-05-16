using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model;

public partial class DbFile
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    // CodeQL [cs/inconsistent-equals-and-gethashcode]: GetHashCode() of base class is sufficient
    protected class DocumentPropertySetReference : ForeignKeyReference<DocumentPropertySet>, IForeignKeyReference<ILocalDocumentPropertySet>, IForeignKeyReference<IDocumentPropertySet>
    {
        internal DocumentPropertySetReference(object syncRoot) : base(syncRoot) { }

        ILocalDocumentPropertySet IForeignKeyReference<ILocalDocumentPropertySet>.Entity => Entity;

        IDocumentPropertySet IForeignKeyReference<IDocumentPropertySet>.Entity => Entity;

        bool IEquatable<IForeignKeyReference<IDocumentPropertySet>>.Equals(IForeignKeyReference<IDocumentPropertySet> other)
        {
            throw new NotImplementedException();
        }

        bool IEquatable<IForeignKeyReference<ILocalDocumentPropertySet>>.Equals(IForeignKeyReference<ILocalDocumentPropertySet> other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj) => obj is not null && ((obj is ForeignKeyReference<DocumentPropertySet> other) ? Equals(other) : base.Equals(obj));
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
