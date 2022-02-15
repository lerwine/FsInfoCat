using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

namespace FsInfoCat.Local
{
    public abstract class LocalDbEntity : DbEntity, ILocalDbEntity
    {
        internal const string ElementName_AccessError = "AccessError";

        #region Properties

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_UpstreamId), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual Guid? UpstreamId { get; set; }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_LastSynchronizedOn), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DateTime? LastSynchronizedOn { get; set; }

        #endregion

        protected override void AddExportAttributes(XElement element)
        {
            Guid? upstreamId = UpstreamId;
            DateTime? lastSynchronizedOn = LastSynchronizedOn;
            if (upstreamId.HasValue)
                element.SetAttributeValue(nameof(UpstreamId), XmlConvert.ToString(upstreamId.Value));
            if (lastSynchronizedOn.HasValue)
                element.SetAttributeValue(nameof(LastSynchronizedOn), XmlConvert.ToString(lastSynchronizedOn.Value, XmlDateTimeSerializationMode.RoundtripKind));
        }

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
