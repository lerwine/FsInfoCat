using FsInfoCat.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml;
using System.Xml.Linq;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Base class for all database entity objects which track the creation and modification dates as well as implementing the <see cref="IDbEntity" /> interface.
    /// This extends <see cref="NotifyDataErrorInfo" /> to facilitate change tracking and validation.
    /// </summary>
    /// <seealso cref="NotifyDataErrorInfo" />
    /// <seealso cref="IDbEntity" />
    public abstract class LocalDbEntity : DbEntity, ILocalDbEntity
    {
        internal const string ElementName_AccessError = "AccessError";

        #region Properties

        /// <summary>
        /// Gets the value of the primary key for the corresponding <see cref="Upstream.Model.IUpstreamDbEntity">upstream (remote) database entity</see>.
        /// </summary>
        /// <value>
        /// The value of the primary key of the corresponding <see cref="Upstream.Model.IUpstreamDbEntity">upstream (remote) database entity</see>;
        /// otherwise, <see langword="null" /> if there is no corresponding entity.
        /// </value>
        /// <remarks>
        /// If this value is <see langword="null" />, then <see cref="LastSynchronizedOn" /> should also be <see langword="null" />.
        /// Likewise, if this is not <see langword="null" />, then <see cref="LastSynchronizedOn" /> should not be <see langword="null" />, either.
        /// </remarks>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.UpstreamId), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual Guid? UpstreamId { get; set; }

        /// <summary>
        /// Gets the date and time when the current entity was sychronized with the corresponding <see cref="Upstream.Model.IUpstreamDbEntity">upstream (remote) database entity</see>.
        /// </summary>
        /// <value>
        /// date and time when the current entity was sychronized with the corresponding <see cref="Upstream.Model.IUpstreamDbEntity">upstream (remote) database entity</see>;
        /// otherwise, <see langword="null" /> if there is no corresponding entity.
        /// </value>
        /// <remarks>
        /// If this value is <see langword="null" />, then <see cref="UpstreamId" /> should also be <see langword="null" />.
        /// Likewise, if this is not <see langword="null" />, then <see cref="UpstreamId" /> should not be <see langword="null" />, either.
        /// </remarks>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.LastSynchronizedOn), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DateTime? LastSynchronizedOn { get; set; }

        #endregion

        /// <summary>
        /// This gets called to add xml attributes to the XML element being exported.
        /// </summary>
        /// <param name="element">The XML element representing the current entity object.</param>
        protected override void AddExportAttributes(XElement element)
        {
            Guid? upstreamId = UpstreamId;
            DateTime? lastSynchronizedOn = LastSynchronizedOn;
            if (upstreamId.HasValue)
                element.SetAttributeValue(nameof(UpstreamId), XmlConvert.ToString(upstreamId.Value));
            if (lastSynchronizedOn.HasValue)
                element.SetAttributeValue(nameof(LastSynchronizedOn), XmlConvert.ToString(lastSynchronizedOn.Value, XmlDateTimeSerializationMode.RoundtripKind));
        }

        /// <summary>
        /// This gets called whenever the current entity is being validated.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <param name="results">Contains validation results to be returned by the <see cref="DbEntity.Validate(ValidationContext)"/> method.</param>
        protected override void OnValidate(ValidationContext validationContext, List<ValidationResult> results)
        {
            base.OnValidate(validationContext, results);
            if (!string.IsNullOrWhiteSpace(validationContext.MemberName))
                switch (validationContext.MemberName)
                {
                    case nameof(LastSynchronizedOn):
                        if (LastSynchronizedOn.HasValue)
                        {
                            if (LastSynchronizedOn.Value.CompareTo(CreatedOn) < 0)
                                results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_LastSynchronizedOnBeforeCreatedOn,
                                    new string[] { nameof(LastSynchronizedOn) }));
                            else if (LastSynchronizedOn.Value.CompareTo(ModifiedOn) > 0)
                                results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_LastSynchronizedOnAfterModifiedOn,
                                    new string[] { nameof(LastSynchronizedOn) }));
                        }
                        else if (UpstreamId.HasValue)
                            results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_LastSynchronizedOnRequired,
                                    new string[] { nameof(LastSynchronizedOn) }));
                        break;
                    case nameof(UpstreamId):
                        if (LastSynchronizedOn.HasValue && !UpstreamId.HasValue)
                            results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_UpstreamIdRequired,
                                    new string[] { nameof(UpstreamId) }));
                        break;
                    default:
                        return;
                }
            lock (SyncRoot)
            {
                if (LastSynchronizedOn.HasValue)
                {
                    if (!UpstreamId.HasValue)
                        results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_UpstreamIdRequired,
                                new string[] { nameof(UpstreamId) }));
                    if (LastSynchronizedOn.Value.CompareTo(CreatedOn) < 0)
                        results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_LastSynchronizedOnBeforeCreatedOn,
                            new string[] { nameof(LastSynchronizedOn) }));
                    else if (LastSynchronizedOn.Value.CompareTo(ModifiedOn) > 0)
                        results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_LastSynchronizedOnAfterModifiedOn,
                            new string[] { nameof(LastSynchronizedOn) }));
                }
                else if (UpstreamId.HasValue)
                    results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_LastSynchronizedOnRequired,
                        new string[] { nameof(LastSynchronizedOn) }));
            }
        }
    }
}
