using System;

namespace FsInfoCat.Model
{
    public interface ITimeStampedEntity
    {
        DateTime CreatedOn { get; }

        DateTime ModifiedOn { get; }
    }
}
