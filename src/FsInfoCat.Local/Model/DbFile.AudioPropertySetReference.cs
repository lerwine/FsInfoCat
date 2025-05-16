using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model;

public partial class DbFile
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    protected class AudioPropertySetReference : ForeignKeyReference<AudioPropertySet>, IForeignKeyReference<ILocalAudioPropertySet>, IForeignKeyReference<IAudioPropertySet>
    {
        internal AudioPropertySetReference(object syncRoot) : base(syncRoot) { }

        ILocalAudioPropertySet IForeignKeyReference<ILocalAudioPropertySet>.Entity => Entity;

        IAudioPropertySet IForeignKeyReference<IAudioPropertySet>.Entity => Entity;

        bool IEquatable<IForeignKeyReference<IAudioPropertySet>>.Equals(IForeignKeyReference<IAudioPropertySet> other)
        {
            throw new NotImplementedException();
        }

        bool IEquatable<IForeignKeyReference<ILocalAudioPropertySet>>.Equals(IForeignKeyReference<ILocalAudioPropertySet> other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj) => obj is not null && ((obj is ForeignKeyReference<AudioPropertySet> other) ? Equals(other) : base.Equals(obj));
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
