using System;

namespace FsInfoCat
{
    /// <summary>
    /// The results of a byte-for-byte comparison of 2 files.
    /// </summary>
    /// <remarks></remarks>
    public interface IComparison : IDbEntity
    {
        /// <summary>
        /// Gets or sets the primary key of the source file in the comparison.
        /// </summary>
        /// <value>The primary key of the <see cref="SourceFile"/>.</value>
        /// <remarks>This is also part of this entity's compound primary key.</remarks>
        Guid SourceFileId { get; set; }

        /// <summary>
        /// Gets or sets the primary key of the target file in the comparison.
        /// </summary>
        /// <value>The primary key of the <see cref="TargetFile"/>.</value>
        /// <remarks>This is also part of this entity's compound primary key.</remarks>
        Guid TargetFileId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="SourceFile"/> and <see cref="TargetFile"/> are identical byte-for-byte.
        /// </summary>
        /// <value><see langword="true"/> if <see cref="SourceFile"/> and <see cref="TargetFile"/> are identical byte-for-byte; otherwise, <see langword="false"/>.</value>
        bool AreEqual { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the files were compared.
        /// </summary>
        /// <value>The date and time when <see cref="SourceFile"/> was compared to <see cref="TargetFile"/>.</value>
        DateTime ComparedOn { get; set; }

        /// <summary>
        /// Gets or sets the source file.
        /// </summary>
        /// <value>The generic <see cref="IFile"/> that represents the source file.</value>
        IFile SourceFile { get; set; }

        /// <summary>
        /// Gets or sets the target file.
        /// </summary>
        /// <value>The generic <see cref="IFile"/> that represents the target file.</value>
        IFile TargetFile { get; set; }
    }
}
