using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local
{
    public abstract class LocalDbEntity : DbEntity, ILocalDbEntity
    {
        #region Fields

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

        protected override void BeforeSave(ValidationContext validationContext)
        {
            base.BeforeSave(validationContext);
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
            EntityEntry entry = validationContext.GetService<EntityEntry>();
            if (entry is null)
                return;
            switch (entry.State)
            {
                case EntityState.Added:
                    if (UpstreamId.HasValue && !LastSynchronizedOn.HasValue)
                        LastSynchronizedOn = CreatedOn;
                    break;
                case EntityState.Modified:
                    Guid? id = UpstreamId;
                    if (id.HasValue && entry.Property(nameof(UpstreamId)).IsModified)
                        LastSynchronizedOn = ModifiedOn;
                    break;
            }
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