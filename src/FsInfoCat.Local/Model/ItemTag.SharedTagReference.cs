using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model;

public abstract partial class ItemTag
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    protected class SharedTagReference : ForeignKeyReference<SharedTagDefinition>, IForeignKeyReference<ILocalSharedTagDefinition>, IForeignKeyReference<ISharedTagDefinition>, IForeignKeyReference<ILocalTagDefinition>, IForeignKeyReference<ITagDefinition>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        internal SharedTagReference(object syncRoot) : base(syncRoot) { }

        ILocalSharedTagDefinition IForeignKeyReference<ILocalSharedTagDefinition>.Entity => Entity;

        ISharedTagDefinition IForeignKeyReference<ISharedTagDefinition>.Entity => Entity;

        ILocalTagDefinition IForeignKeyReference<ILocalTagDefinition>.Entity => Entity;

        ITagDefinition IForeignKeyReference<ITagDefinition>.Entity => Entity;

        bool IEquatable<IForeignKeyReference<ITagDefinition>>.Equals(IForeignKeyReference<ITagDefinition> other)
        {
            throw new NotImplementedException();
        }

        bool IEquatable<IForeignKeyReference<ILocalTagDefinition>>.Equals(IForeignKeyReference<ILocalTagDefinition> other)
        {
            throw new NotImplementedException();
        }

        bool IEquatable<IForeignKeyReference<ISharedTagDefinition>>.Equals(IForeignKeyReference<ISharedTagDefinition> other)
        {
            throw new NotImplementedException();
        }

        bool IEquatable<IForeignKeyReference<ILocalSharedTagDefinition>>.Equals(IForeignKeyReference<ILocalSharedTagDefinition> other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj) => obj is not null && ((obj is ForeignKeyReference<SharedTagDefinition> other) ? Equals(other) : base.Equals(obj));
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
