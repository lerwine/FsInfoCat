using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml;
using System.Xml.Linq;

namespace FsInfoCat
{
    public abstract partial class DbEntity : NotifyDataErrorInfo, IDbEntity
    {
        #region Fields

        private readonly IPropertyChangeTracker<DateTime> _createdOn;
        private readonly IPropertyChangeTracker<DateTime> _modifiedOn;

        #endregion

        #region Properties

        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_CreatedOn), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DateTime CreatedOn { get => _createdOn.GetValue(); set => _createdOn.SetValue(value); }

        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_ModifiedOn), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DateTime ModifiedOn { get => _modifiedOn.GetValue(); set => _modifiedOn.SetValue(value); }

        #endregion

        public DbEntity()
        {
            _modifiedOn = AddChangeTracker(nameof(ModifiedOn), (_createdOn = AddChangeTracker(nameof(CreatedOn), DateTime.Now)).GetValue());
        }

        protected virtual void AddExportAttributes(XElement element)
        {
            element.SetAttributeValue(nameof(CreatedOn), XmlConvert.ToString(CreatedOn, XmlDateTimeSerializationMode.RoundtripKind));
            element.SetAttributeValue(nameof(ModifiedOn), XmlConvert.ToString(ModifiedOn, XmlDateTimeSerializationMode.RoundtripKind));
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new();
            OnValidate(validationContext, results);
            return results.ToArray();
        }

        protected virtual void OnValidate(ValidationContext validationContext, List<ValidationResult> results)
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

        void IDbEntity.BeforeSave(ValidationContext validationContext) => BeforeSave(validationContext);

        protected virtual void BeforeSave(ValidationContext validationContext)
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
            EntityEntry entry = validationContext.GetService<EntityEntry>();
            if (entry is null)
                return;
            switch (entry.State)
            {
                case EntityState.Added:
                    CreatedOn = ModifiedOn = DateTime.Now;
                    break;
                case EntityState.Modified:
                    ModifiedOn = DateTime.Now;
                    break;
            }
        }
    }
}
