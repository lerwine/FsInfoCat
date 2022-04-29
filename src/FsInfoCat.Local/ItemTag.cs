using System;

namespace FsInfoCat.Local
{
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
        }

        protected class SubdirectoryReference : ForeignKeyReference<Subdirectory>, IForeignKeyReference<ILocalSubdirectory>, IForeignKeyReference<ISubdirectory>
        {
            internal SubdirectoryReference(object syncRoot) : base(syncRoot) { }

            ILocalSubdirectory IForeignKeyReference<ILocalSubdirectory>.Entity => Entity;

            ISubdirectory IForeignKeyReference<ISubdirectory>.Entity => Entity;
        }

        protected class VolumeReference : ForeignKeyReference<Volume>, IForeignKeyReference<ILocalVolume>, IForeignKeyReference<IVolume>
        {
            internal VolumeReference(object syncRoot) : base(syncRoot) { }

            ILocalVolume IForeignKeyReference<ILocalVolume>.Entity => Entity;

            IVolume IForeignKeyReference<IVolume>.Entity => Entity;
        }

        protected class PersonalTagReference : ForeignKeyReference<PersonalTagDefinition>, IForeignKeyReference<ILocalPersonalTagDefinition>, IForeignKeyReference<IPersonalTagDefinition>, IForeignKeyReference<ITagDefinition>
        {
            internal PersonalTagReference(object syncRoot) : base(syncRoot) { }

            ILocalPersonalTagDefinition IForeignKeyReference<ILocalPersonalTagDefinition>.Entity => Entity;

            IPersonalTagDefinition IForeignKeyReference<IPersonalTagDefinition>.Entity => Entity;

            ITagDefinition IForeignKeyReference<ITagDefinition>.Entity => Entity;
        }

        protected class SharedTagReference : ForeignKeyReference<SharedTagDefinition>, IForeignKeyReference<ILocalSharedTagDefinition>, IForeignKeyReference<ISharedTagDefinition>, IForeignKeyReference<ITagDefinition>
        {
            internal SharedTagReference(object syncRoot) : base(syncRoot) { }

            ILocalSharedTagDefinition IForeignKeyReference<ILocalSharedTagDefinition>.Entity => Entity;

            ISharedTagDefinition IForeignKeyReference<ISharedTagDefinition>.Entity => Entity;

            ITagDefinition IForeignKeyReference<ITagDefinition>.Entity => Entity;
        }
    }
}
