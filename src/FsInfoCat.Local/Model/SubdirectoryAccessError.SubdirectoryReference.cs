using FsInfoCat.Model;
using System;
using System.Threading;

namespace FsInfoCat.Local.Model;

public partial class SubdirectoryAccessError
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    protected class SubdirectoryReference : ForeignKeyReference<Subdirectory>, IForeignKeyReference<ILocalSubdirectory>, IForeignKeyReference<ISubdirectory>, IEquatable<ILocalSubdirectoryAccessError>, IEquatable<ISubdirectoryAccessError>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        internal SubdirectoryReference(object syncRoot) : base(syncRoot) { }

        ILocalSubdirectory IForeignKeyReference<ILocalSubdirectory>.Entity => Entity;

        ISubdirectory IForeignKeyReference<ISubdirectory>.Entity => Entity;

        public bool Equals(ILocalSubdirectoryAccessError other)
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

        public bool Equals(ISubdirectoryAccessError other)
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

        bool IEquatable<IForeignKeyReference<ILocalSubdirectory>>.Equals(IForeignKeyReference<ILocalSubdirectory> other)
        {
            throw new NotImplementedException();
        }

        bool IEquatable<IForeignKeyReference<ISubdirectory>>.Equals(IForeignKeyReference<ISubdirectory> other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj) => obj is not null && ((obj is ForeignKeyReference<Subdirectory> other) ? Equals(other) : base.Equals(obj));
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
