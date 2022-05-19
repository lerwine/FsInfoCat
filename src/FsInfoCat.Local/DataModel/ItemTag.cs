using System;

namespace FsInfoCat.Local
{
    // TODO: Document ItemTag class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    [Obsolete("Use FsInfoCat.Local.Model.ItemTag")]
    public abstract class ItemTag : ItemTagRow, ILocalItemTag
    {
        protected abstract ILocalDbEntity GetTagged();

        protected abstract ILocalTagDefinition GetDefinition();

        public abstract bool TryGetDefinitionId(out Guid definitionId);

        public abstract bool TryGetTaggedId(out Guid taggedId);

        IDbEntity IItemTag.Tagged => GetTagged();

        ITagDefinition IItemTag.Definition => GetDefinition();

        ILocalDbEntity ILocalItemTag.Tagged => GetTagged();

        ILocalTagDefinition ILocalItemTag.Definition => GetDefinition();

        protected class FileReference : ForeignKeyReference<DbFile>, IForeignKeyReference<ILocalFile>, IForeignKeyReference<IFile>
        {
            internal FileReference(object syncRoot) : base(syncRoot) { }

            ILocalFile IForeignKeyReference<ILocalFile>.Entity => Entity;

            IFile IForeignKeyReference<IFile>.Entity => Entity;

            bool IEquatable<IForeignKeyReference<ILocalFile>>.Equals(IForeignKeyReference<ILocalFile> other)
            {
                throw new NotImplementedException();
            }

            bool IEquatable<IForeignKeyReference<IFile>>.Equals(IForeignKeyReference<IFile> other)
            {
                throw new NotImplementedException();
            }
        }

        protected class SubdirectoryReference : ForeignKeyReference<Subdirectory>, IForeignKeyReference<ILocalSubdirectory>, IForeignKeyReference<ISubdirectory>
        {
            internal SubdirectoryReference(object syncRoot) : base(syncRoot) { }

            ILocalSubdirectory IForeignKeyReference<ILocalSubdirectory>.Entity => Entity;

            ISubdirectory IForeignKeyReference<ISubdirectory>.Entity => Entity;

            bool IEquatable<IForeignKeyReference<ISubdirectory>>.Equals(IForeignKeyReference<ISubdirectory> other)
            {
                throw new NotImplementedException();
            }

            bool IEquatable<IForeignKeyReference<ILocalSubdirectory>>.Equals(IForeignKeyReference<ILocalSubdirectory> other)
            {
                throw new NotImplementedException();
            }
        }

        protected class VolumeReference : ForeignKeyReference<Volume>, IForeignKeyReference<ILocalVolume>, IForeignKeyReference<IVolume>
        {
            internal VolumeReference(object syncRoot) : base(syncRoot) { }

            ILocalVolume IForeignKeyReference<ILocalVolume>.Entity => Entity;

            IVolume IForeignKeyReference<IVolume>.Entity => Entity;

            bool IEquatable<IForeignKeyReference<IVolume>>.Equals(IForeignKeyReference<IVolume> other)
            {
                throw new NotImplementedException();
            }

            bool IEquatable<IForeignKeyReference<ILocalVolume>>.Equals(IForeignKeyReference<ILocalVolume> other)
            {
                throw new NotImplementedException();
            }
        }

        protected class PersonalTagReference : ForeignKeyReference<PersonalTagDefinition>, IForeignKeyReference<ILocalPersonalTagDefinition>, IForeignKeyReference<IPersonalTagDefinition>, IForeignKeyReference<ILocalTagDefinition>, IForeignKeyReference<ITagDefinition>
        {
            internal PersonalTagReference(object syncRoot) : base(syncRoot) { }

            ILocalPersonalTagDefinition IForeignKeyReference<ILocalPersonalTagDefinition>.Entity => Entity;

            IPersonalTagDefinition IForeignKeyReference<IPersonalTagDefinition>.Entity => Entity;

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

            bool IEquatable<IForeignKeyReference<IPersonalTagDefinition>>.Equals(IForeignKeyReference<IPersonalTagDefinition> other)
            {
                throw new NotImplementedException();
            }

            bool IEquatable<IForeignKeyReference<ILocalPersonalTagDefinition>>.Equals(IForeignKeyReference<ILocalPersonalTagDefinition> other)
            {
                throw new NotImplementedException();
            }
        }

        protected class SharedTagReference : ForeignKeyReference<SharedTagDefinition>, IForeignKeyReference<ILocalSharedTagDefinition>, IForeignKeyReference<ISharedTagDefinition>, IForeignKeyReference<ILocalTagDefinition>, IForeignKeyReference<ITagDefinition>
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
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
