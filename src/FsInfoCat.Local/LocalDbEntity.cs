using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml;
using System.Xml.Linq;

namespace FsInfoCat.Local
{
    public abstract class LocalDbEntity : DbEntity, ILocalDbEntity
    {
        #region Fields

        internal const string ElementName_AccessError = "AccessError";
        private readonly IPropertyChangeTracker<Guid?> _upstreamId;
        private readonly IPropertyChangeTracker<DateTime?> _lastSynchronizedOn;

        #endregion

        #region Properties

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_UpstreamId), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual Guid? UpstreamId { get => _upstreamId.GetValue(); set => _upstreamId.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_LastSynchronizedOn), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DateTime? LastSynchronizedOn { get => _lastSynchronizedOn.GetValue(); set => _lastSynchronizedOn.SetValue(value); }

        #endregion

        public LocalDbEntity()
        {
            _upstreamId = AddChangeTracker<Guid?>(nameof(UpstreamId), null);
            _lastSynchronizedOn = AddChangeTracker<DateTime?>(nameof(LastSynchronizedOn), null);
        }

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
                    case nameof(ModifiedOn):
                    case nameof(CreatedOn):
                    case nameof(LastSynchronizedOn):
                    case nameof(UpstreamId):
                        break;
                    default:
                        return;
                }
            lock (SyncRoot)
            {
                if (LastSynchronizedOn.HasValue)
                {
                    if (results.Count > 0)
                        return;
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
