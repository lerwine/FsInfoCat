using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model;

public partial class DbFile
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    protected class VideoPropertySetReference : ForeignKeyReference<VideoPropertySet>, IForeignKeyReference<ILocalVideoPropertySet>, IForeignKeyReference<IVideoPropertySet>
    {
        internal VideoPropertySetReference(object syncRoot) : base(syncRoot) { }

        ILocalVideoPropertySet IForeignKeyReference<ILocalVideoPropertySet>.Entity => Entity;

        IVideoPropertySet IForeignKeyReference<IVideoPropertySet>.Entity => Entity;

        bool IEquatable<IForeignKeyReference<IVideoPropertySet>>.Equals(IForeignKeyReference<IVideoPropertySet> other)
        {
            throw new NotImplementedException();
        }

        bool IEquatable<IForeignKeyReference<ILocalVideoPropertySet>>.Equals(IForeignKeyReference<ILocalVideoPropertySet> other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj) => obj is not null && ((obj is ForeignKeyReference<VideoPropertySet> other) ? Equals(other) : base.Equals(obj));
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
