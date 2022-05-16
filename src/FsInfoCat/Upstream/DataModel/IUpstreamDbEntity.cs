using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Base interface for all database entity objects for the database which is hosted on the local machine.
    /// </summary>
    /// <seealso cref="IDbEntity" />
    /// <seealso cref="Local.ILocalDbEntity" />
    public interface IUpstreamDbEntity : IDbEntity
    {
        /// <summary>
        /// Gets the user who created the current record.
        /// </summary>
        /// <value>The user who created the current record.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_CreatedBy), ResourceType = typeof(Properties.Resources))]
        IUserProfile CreatedBy { get; }

        /// <summary>
        /// Gets the user who last modified the current record.
        /// </summary>
        /// <value>The user who last modified the current record.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_ModifiedBy), ResourceType = typeof(Properties.Resources))]
        IUserProfile ModifiedBy { get; }
    }
}
