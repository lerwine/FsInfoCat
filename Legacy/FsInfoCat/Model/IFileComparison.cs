using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    public interface IFileComparison : ITimeStampedEntity, IValidatableObject
    {
        bool AreEqual { get; }

        IFile SourceFile { get; }

        IFile TargetFile { get; }
    }
}
