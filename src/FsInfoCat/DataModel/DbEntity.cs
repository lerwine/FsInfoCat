using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Linq;

namespace FsInfoCat
{
    /// <summary>
    /// Base class for all database entity objects which track the creation and modification dates as well as implementing the <see cref="IDbEntity" /> interface.
    /// This extends <see cref="NotifyDataErrorInfo" /> to facilitate change tracking and validation.
    /// </summary>
    /// <seealso cref="NotifyDataErrorInfo" />
    /// <seealso cref="IDbEntity" />
    public abstract partial class DbEntity : IDbEntity
    {
        /// <summary>
        /// Gets the synchronization object.
        /// </summary>
        /// <value>The object to use for asynchronous locks.</value>
        protected readonly object SyncRoot = new();

        #region Properties

        /// <summary>
        /// Gets or sets the database entity creation date/time.
        /// </summary>
        /// <value>The date and time when the database entity was created.</value>
        /// <remarks>This value is automatically updated before it is inserted into the database.
        /// <para>For local databases, this value is the system-<see cref="DateTimeKind.Local" /> date and time. For upstream (remote) databases, this is the
        /// <see cref="DateTimeKind.Utc">UTC</see> date and time.</para></remarks>
        [Required]
        [Display(Name = nameof(Properties.Resources.DisplayName_CreatedOn), ResourceType = typeof(Properties.Resources))]
        public virtual DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the database entity modification date/time.
        /// </summary>
        /// <value>The date and time when the database entity was last modified.</value>
        /// <remarks>This value is automatically updated before it is saved to the database.
        /// <para>For local databases, this value is the system-<see cref="DateTimeKind.Local" /> date and time. For upstream (remote) databases, this is the
        /// <see cref="DateTimeKind.Utc">UTC</see> date and time.</para></remarks>
        [Required]
        [Display(Name = nameof(Properties.Resources.DisplayName_ModifiedOn), ResourceType = typeof(Properties.Resources))]
        public virtual DateTime ModifiedOn { get; set; }

        #endregion

        /// <summary>
        /// Creates a new database entity object.
        /// </summary>
        protected DbEntity()
        {
            CreatedOn = ModifiedOn = DateTime.Now;
        }

        /// <summary>
        /// This gets called to add xml attributes to the XML element being exported.
        /// </summary>
        /// <param name="element">The XML element representing the current entity object.</param>
        protected virtual void AddExportAttributes([DisallowNull] XElement element)
        {
            element.SetAttributeValue(nameof(CreatedOn), XmlConvert.ToString(CreatedOn, XmlDateTimeSerializationMode.RoundtripKind));
            element.SetAttributeValue(nameof(ModifiedOn), XmlConvert.ToString(ModifiedOn, XmlDateTimeSerializationMode.RoundtripKind));
        }

        /// <summary>
        /// Determines whether the specified object is valid.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>A collection that holds failed-validation information.</returns>
        public IEnumerable<ValidationResult> Validate([DisallowNull] ValidationContext validationContext)
        {
            List<ValidationResult> results = new();
            OnValidate(validationContext, results);
            return results.ToArray();
        }

        /// <summary>
        /// This gets called whenever the object is being validated.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <param name="results">Contains validation results to be returned by the <see cref="Validate(ValidationContext)"/> method.</param>
        protected virtual void OnValidate([DisallowNull] ValidationContext validationContext, [DisallowNull] List<ValidationResult> results)
        {
            if (!string.IsNullOrWhiteSpace(validationContext.MemberName))
                switch (validationContext.MemberName)
                {
                    case nameof(CreatedOn):
                    case nameof(ModifiedOn):
                        break;
                    default:
                        return;
                }
            if (CreatedOn.CompareTo(ModifiedOn) > 0)
                results.Add(new ValidationResult(Properties.Resources.ErrorMessage_CreatedOnAfterModifiedOn, new string[] { nameof(CreatedOn) }));
        }
    }
}
