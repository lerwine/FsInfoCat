using M = FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model
{
    // TODO: Document ItemTag class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public abstract class ItemTag : ItemTagRow, ILocalItemTag
    {
        protected abstract ILocalDbEntity GetTagged();

        protected abstract ILocalTagDefinition GetDefinition();

        public abstract bool TryGetDefinitionId(out Guid definitionId);

        public abstract bool TryGetTaggedId(out Guid taggedId);

        M.IDbEntity M.IItemTag.Tagged => GetTagged();

        M.ITagDefinition M.IItemTag.Definition => GetDefinition();

        ILocalDbEntity ILocalItemTag.Tagged => GetTagged();

        ILocalTagDefinition ILocalItemTag.Definition => GetDefinition();

        protected class FileReference : ForeignKeyReference<DbFile>, IForeignKeyReference<ILocalFile>, IForeignKeyReference<M.IFile>
        {
            internal FileReference(object syncRoot) : base(syncRoot) { }

            ILocalFile IForeignKeyReference<ILocalFile>.Entity => Entity;

            M.IFile IForeignKeyReference<M.IFile>.Entity => Entity;

            bool IEquatable<IForeignKeyReference<ILocalFile>>.Equals(IForeignKeyReference<ILocalFile> other)
            {
                throw new NotImplementedException();
            }

            bool IEquatable<IForeignKeyReference<M.IFile>>.Equals(IForeignKeyReference<M.IFile> other)
            {
                throw new NotImplementedException();
            }
        }

        protected class SubdirectoryReference : ForeignKeyReference<Subdirectory>, IForeignKeyReference<ILocalSubdirectory>, IForeignKeyReference<M.ISubdirectory>
        {
            internal SubdirectoryReference(object syncRoot) : base(syncRoot) { }

            ILocalSubdirectory IForeignKeyReference<ILocalSubdirectory>.Entity => Entity;

            M.ISubdirectory IForeignKeyReference<M.ISubdirectory>.Entity => Entity;

            bool IEquatable<IForeignKeyReference<M.ISubdirectory>>.Equals(IForeignKeyReference<M.ISubdirectory> other)
            {
                throw new NotImplementedException();
            }

            bool IEquatable<IForeignKeyReference<ILocalSubdirectory>>.Equals(IForeignKeyReference<ILocalSubdirectory> other)
            {
                throw new NotImplementedException();
            }
        }

        protected class VolumeReference : ForeignKeyReference<Volume>, IForeignKeyReference<ILocalVolume>, IForeignKeyReference<M.IVolume>
        {
            internal VolumeReference(object syncRoot) : base(syncRoot) { }

            ILocalVolume IForeignKeyReference<ILocalVolume>.Entity => Entity;

            M.IVolume IForeignKeyReference<M.IVolume>.Entity => Entity;

            bool IEquatable<IForeignKeyReference<M.IVolume>>.Equals(IForeignKeyReference<M.IVolume> other)
            {
                throw new NotImplementedException();
            }

            bool IEquatable<IForeignKeyReference<ILocalVolume>>.Equals(IForeignKeyReference<ILocalVolume> other)
            {
                throw new NotImplementedException();
            }
        }

        protected class PersonalTagReference : ForeignKeyReference<PersonalTagDefinition>, IForeignKeyReference<ILocalPersonalTagDefinition>, IForeignKeyReference<M.IPersonalTagDefinition>, IForeignKeyReference<ILocalTagDefinition>, IForeignKeyReference<M.ITagDefinition>
        {
            internal PersonalTagReference(object syncRoot) : base(syncRoot) { }

            ILocalPersonalTagDefinition IForeignKeyReference<ILocalPersonalTagDefinition>.Entity => Entity;

            M.IPersonalTagDefinition IForeignKeyReference<M.IPersonalTagDefinition>.Entity => Entity;

            ILocalTagDefinition IForeignKeyReference<ILocalTagDefinition>.Entity => Entity;

            M.ITagDefinition IForeignKeyReference<M.ITagDefinition>.Entity => Entity;

            bool IEquatable<IForeignKeyReference<M.ITagDefinition>>.Equals(IForeignKeyReference<M.ITagDefinition> other)
            {
                throw new NotImplementedException();
            }

            bool IEquatable<IForeignKeyReference<ILocalTagDefinition>>.Equals(IForeignKeyReference<ILocalTagDefinition> other)
            {
                throw new NotImplementedException();
            }

            bool IEquatable<IForeignKeyReference<M.IPersonalTagDefinition>>.Equals(IForeignKeyReference<M.IPersonalTagDefinition> other)
            {
                throw new NotImplementedException();
            }

            bool IEquatable<IForeignKeyReference<ILocalPersonalTagDefinition>>.Equals(IForeignKeyReference<ILocalPersonalTagDefinition> other)
            {
                throw new NotImplementedException();
            }
        }

        protected class SharedTagReference : ForeignKeyReference<SharedTagDefinition>, IForeignKeyReference<ILocalSharedTagDefinition>, IForeignKeyReference<M.ISharedTagDefinition>, IForeignKeyReference<ILocalTagDefinition>, IForeignKeyReference<M.ITagDefinition>
        {
            internal SharedTagReference(object syncRoot) : base(syncRoot) { }

            ILocalSharedTagDefinition IForeignKeyReference<ILocalSharedTagDefinition>.Entity => Entity;

            M.ISharedTagDefinition IForeignKeyReference<M.ISharedTagDefinition>.Entity => Entity;

            ILocalTagDefinition IForeignKeyReference<ILocalTagDefinition>.Entity => Entity;

            M.ITagDefinition IForeignKeyReference<M.ITagDefinition>.Entity => Entity;

            bool IEquatable<IForeignKeyReference<M.ITagDefinition>>.Equals(IForeignKeyReference<M.ITagDefinition> other)
            {
                throw new NotImplementedException();
            }

            bool IEquatable<IForeignKeyReference<ILocalTagDefinition>>.Equals(IForeignKeyReference<ILocalTagDefinition> other)
            {
                throw new NotImplementedException();
            }

            bool IEquatable<IForeignKeyReference<M.ISharedTagDefinition>>.Equals(IForeignKeyReference<M.ISharedTagDefinition> other)
            {
                throw new NotImplementedException();
            }

            bool IEquatable<IForeignKeyReference<ILocalSharedTagDefinition>>.Equals(IForeignKeyReference<ILocalSharedTagDefinition> other)
            {
                throw new NotImplementedException();
            }
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
