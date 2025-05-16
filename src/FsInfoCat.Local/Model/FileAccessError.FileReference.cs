using FsInfoCat.Model;
using System;
using System.Threading;

namespace FsInfoCat.Local.Model;

public partial class FileAccessError
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    protected class FileReference : ForeignKeyReference<DbFile>, IForeignKeyReference<ILocalFile>, IForeignKeyReference<IFile>, IEquatable<ILocalFileAccessError>, IEquatable<IFileAccessError>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        internal FileReference(object syncRoot) : base(syncRoot) { }

        ILocalFile IForeignKeyReference<ILocalFile>.Entity => Entity;

        IFile IForeignKeyReference<IFile>.Entity => Entity;

        public bool Equals(ILocalFileAccessError other)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (Entity is null)
                {
                    if (TryGetId(out Guid i))
                        return other.TryGetId(out Guid id) && id.Equals(i);
                    return other.Target is null && !other.TryGetTargetId(out _);
                }
                if (Entity.TryGetId(out Guid g))
                    return other.TryGetId(out Guid id) && id.Equals(g);
                return other.Target is not null && Entity.Equals(other.Target);
            }
            finally { Monitor.Exit(SyncRoot); }
        }

        public bool Equals(IFileAccessError other)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (Entity is null)
                {
                    if (TryGetId(out Guid i))
                        return other.TryGetId(out Guid id) && id.Equals(i);
                    return other.Target is null && !other.TryGetTargetId(out _);
                }
                if (Entity.TryGetId(out Guid g))
                    return other.TryGetId(out Guid id) && id.Equals(g);
                return other.Target is not null && Entity.Equals(other.Target);
            }
            finally { Monitor.Exit(SyncRoot); }
        }

        bool IEquatable<IForeignKeyReference<ILocalFile>>.Equals(IForeignKeyReference<ILocalFile> other)
        {
            throw new NotImplementedException();
        }

        bool IEquatable<IForeignKeyReference<IFile>>.Equals(IForeignKeyReference<IFile> other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj) => obj is not null && ((obj is ForeignKeyReference<DbFile> other) ? Equals(other) : base.Equals(obj));
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
