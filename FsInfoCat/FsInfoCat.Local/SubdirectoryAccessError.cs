using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FsInfoCat.Local
{
    public class SubdirectoryAccessError : DbEntity, IAccessError<Subdirectory>, IAccessError<ILocalSubdirectory>, IAccessError<ISubdirectory>
    {
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<AccessErrorCode> _errorCode;
        private readonly IPropertyChangeTracker<string> _message;
        private readonly IPropertyChangeTracker<string> _details;
        private readonly IPropertyChangeTracker<Guid> _targetId;
        private readonly IPropertyChangeTracker<Subdirectory> _target;

        #endregion

        #region Properties

        [Key]
        public virtual Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [StringLength(DbConstants.DbColMaxLen_FileName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual string Message { get => _message.GetValue(); set => _message.SetValue(value); }

        [Required(AllowEmptyStrings = true)]
        public virtual string Details { get => _details.GetValue(); set => _details.SetValue(value); }

        [Required]
        public AccessErrorCode ErrorCode { get => _errorCode.GetValue(); set => _errorCode.SetValue(value); }

        public virtual Guid TargetId
        {
            get => _targetId.GetValue();
            set
            {
                if (_targetId.SetValue(value))
                {
                    Subdirectory nav = _target.GetValue();
                    if (!(nav is null || nav.Id.Equals(value)))
                        _targetId.SetValue(null);
                }
            }
        }

        [Required]
        public Subdirectory Target
        {
            get => _target.GetValue();
            set
            {
                if (_target.SetValue(value))
                {
                    if (value is null)
                        _targetId.SetValue(Guid.Empty);
                    else
                        _targetId.SetValue(value.Id);
                }
            }
        }

        #endregion

        #region Explicit Members

        ILocalSubdirectory IAccessError<ILocalSubdirectory>.Target { get => Target; set => Target = (Subdirectory)value; }

        ISubdirectory IAccessError<ISubdirectory>.Target { get => Target; set => Target = (Subdirectory)value; }

        IDbEntity IAccessError.Target { get => Target; set => Target = (Subdirectory)value; }

        #endregion

        public SubdirectoryAccessError()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _errorCode = AddChangeTracker(nameof(AccessErrorCode), AccessErrorCode.OpenError);
            _message = AddChangeTracker(nameof(Message), "", NonNullStringCoersion.Default);
            _details = AddChangeTracker(nameof(Details), "", NonNullStringCoersion.Default);
            _targetId = AddChangeTracker(nameof(TargetId), Guid.Empty);
            _target = AddChangeTracker<Subdirectory>(nameof(Target), null);
        }

        protected override void OnValidate(ValidationContext validationContext, List<ValidationResult> results)
        {
            base.OnValidate(validationContext, results);
            if (!string.IsNullOrWhiteSpace(validationContext.MemberName))
                switch (validationContext.MemberName)
                {
                    case nameof(Message):
                    case nameof(ErrorCode):
                        break;
                    default:
                        return;
                }
            string name = Message;
            LocalDbContext dbContext;
            if (string.IsNullOrEmpty(name) || (dbContext = validationContext.GetService<LocalDbContext>()) is null)
                return;
            Guid id = Id;
            AccessErrorCode errorCode = ErrorCode;
            if (dbContext.SubdirectoryAccessErrors.Any(e => e.ErrorCode == errorCode && e.Message == name && id != e.Id))
                results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_DuplicateMessage, new string[] { nameof(Message) }));
        }
    }
}