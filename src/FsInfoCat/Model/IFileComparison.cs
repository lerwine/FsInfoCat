using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    public interface IFileComparison : ITimeStampedEntity, IValidatableObject
    {
        bool AreEqual { get; }
        IFile File1 { get; }
        IFile File2 { get; }
    }
}
