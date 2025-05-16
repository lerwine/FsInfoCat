using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model;

public partial class Volume
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    protected class FileSystemReference : ForeignKeyReference<FileSystem>, IForeignKeyReference<ILocalFileSystem>, IForeignKeyReference<IFileSystem>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        internal FileSystemReference(object syncRoot) : base(syncRoot) { }

        ILocalFileSystem IForeignKeyReference<ILocalFileSystem>.Entity => Entity;

        IFileSystem IForeignKeyReference<IFileSystem>.Entity => Entity;

        bool IEquatable<IForeignKeyReference<ILocalFileSystem>>.Equals(IForeignKeyReference<ILocalFileSystem> other)
        {
            throw new NotImplementedException();
        }

        bool IEquatable<IForeignKeyReference<IFileSystem>>.Equals(IForeignKeyReference<IFileSystem> other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj) => obj is not null && ((obj is ForeignKeyReference<FileSystem> other) ? Equals(other) : base.Equals(obj));
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
