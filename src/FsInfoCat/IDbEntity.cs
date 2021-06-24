using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>
    /// Base interface for all database entity objects which track the creation and modification dates as well as implementing the
    /// <see cref="IValidatableObject" /> and  <see cref="IRevertibleChangeTracking" /> interfaces.
    /// </summary>
    /// <seealso cref="IValidatableObject" />
    /// <seealso cref="IRevertibleChangeTracking" />
    public interface IDbEntity : IValidatableObject, IRevertibleChangeTracking
    {
        /// <summary>
        /// Gets or sets the database entity creation date/time.
        /// </summary>
        /// <value>The date and time when the database entity was created.</value>
        /// <remarks>For local databases, this value is the system-<see cref="DateTimeKind.Local"/> date and time. For upstream (remote) databases, this is the
        /// <see cref="DateTimeKind.Utc">UTC</see> date and time.</remarks>
        DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the database entity modification date/time.
        /// </summary>
        /// <value>The date and time when the database entity was last modified.</value>
        /// <remarks>For local databases, this value is the system-<see cref="DateTimeKind.Local"/> date and time. For upstream (remote) databases, this is the
        /// <see cref="DateTimeKind.Utc">UTC</see> date and time.</remarks>
        DateTime ModifiedOn { get; set; }

        /// <summary>
        /// This gets called before the entity is inserted or updated into the database.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <remarks>This allows the entity to update relevant properties before being saved to the database, such as updating the entity modification date/time.</remarks>
        void BeforeSave(ValidationContext validationContext);
    }
}
