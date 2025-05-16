using FsInfoCat.Model;
using System;
using System.Threading;

namespace FsInfoCat.Local.Model;

public partial class VolumeAccessError
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    protected class VolumeReference : ForeignKeyReference<Volume>, IForeignKeyReference<ILocalVolume>, IForeignKeyReference<IVolume>, IEquatable<ILocalVolumeAccessError>,
        IEquatable<IVolumeAccessError>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        internal VolumeReference(object syncRoot) : base(syncRoot) { }

        ILocalVolume IForeignKeyReference<ILocalVolume>.Entity => Entity;

        IVolume IForeignKeyReference<IVolume>.Entity => Entity;

        public bool Equals(ILocalVolumeAccessError other)
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

        public bool Equals(IVolumeAccessError other)
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

        bool IEquatable<IForeignKeyReference<IVolume>>.Equals(IForeignKeyReference<IVolume> other)
        {
            throw new NotImplementedException();
        }

        bool IEquatable<IForeignKeyReference<ILocalVolume>>.Equals(IForeignKeyReference<ILocalVolume> other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj) => obj is not null && ((obj is ForeignKeyReference<Volume> other) ? Equals(other) : base.Equals(obj));
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
