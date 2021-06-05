using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    public abstract class DbEntity : NotifyDataErrorInfo, IDbEntity
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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();
            OnValidate(validationContext, results);
            return results.ToArray();
        }

        protected virtual void OnValidate(ValidationContext validationContext, List<ValidationResult> results)
        {
            if (CreatedOn.CompareTo(ModifiedOn) > 0)
            {
                results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_CreatedOnAfterModifiedOn, new string[] { nameof(CreatedOn) }));
            }
        }
    }
}
