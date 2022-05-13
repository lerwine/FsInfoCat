using System;

namespace FsInfoCat
{
    public interface IForeignKeyReference : IHasSimpleIdentifier, ISynchronizable
    {
        IHasSimpleIdentifier Entity { get; }
        bool HasId();
        void SetId(Guid? id);
    }

    public interface IForeignKeyReference<TEntity> : IForeignKeyReference
        where TEntity : IHasSimpleIdentifier
    {
        new TEntity Entity { get; }
    }
}
