using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model;

public partial class DbFile
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    protected class MusicPropertySetReference : ForeignKeyReference<MusicPropertySet>, IForeignKeyReference<ILocalMusicPropertySet>, IForeignKeyReference<IMusicPropertySet>
    {
        internal MusicPropertySetReference(object syncRoot) : base(syncRoot) { }

        ILocalMusicPropertySet IForeignKeyReference<ILocalMusicPropertySet>.Entity => Entity;

        IMusicPropertySet IForeignKeyReference<IMusicPropertySet>.Entity => Entity;

        bool IEquatable<IForeignKeyReference<IMusicPropertySet>>.Equals(IForeignKeyReference<IMusicPropertySet> other)
        {
            throw new NotImplementedException();
        }

        bool IEquatable<IForeignKeyReference<ILocalMusicPropertySet>>.Equals(IForeignKeyReference<ILocalMusicPropertySet> other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj) => obj is not null && ((obj is ForeignKeyReference<MusicPropertySet> other) ? Equals(other) : base.Equals(obj));
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
