using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace FsInfoCat.Local
{
    public class VolumeAccessError : DbEntity, IAccessError<Volume>, IAccessError<ILocalVolume>, IAccessError<IVolume>
    {
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<AccessErrorCode> _errorCode;
        private readonly IPropertyChangeTracker<string> _message;
        private readonly IPropertyChangeTracker<string> _details;
        private readonly IPropertyChangeTracker<Guid> _targetId;
        private readonly IPropertyChangeTracker<Volume> _target;

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
                    Volume nav = _target.GetValue();
                    if (!(nav is null || nav.Id.Equals(value)))
                        _targetId.SetValue(null);
                }
            }
        }

        [Required]
        public Volume Target
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

        ILocalVolume IAccessError<ILocalVolume>.Target { get => Target; set => Target = (Volume)value; }

        IVolume IAccessError<IVolume>.Target { get => Target; set => Target = (Volume)value; }

        IDbEntity IAccessError.Target { get => Target; set => Target = (Volume)value; }

        #endregion

        public VolumeAccessError()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _errorCode = AddChangeTracker(nameof(AccessErrorCode), AccessErrorCode.OpenError);
            _message = AddChangeTracker(nameof(Message), "", NonNullStringCoersion.Default);
            _details = AddChangeTracker(nameof(Details), "", NonNullStringCoersion.Default);
            _targetId = AddChangeTracker(nameof(TargetId), Guid.Empty);
            _target = AddChangeTracker<Volume>(nameof(Target), null);
        }

        protected override void OnValidate(ValidationContext validationContext, List<ValidationResult> results)
        {
            base.OnValidate(validationContext, results);
            if (!string.IsNullOrWhiteSpace(validationContext.MemberName))
                switch (validationContext.MemberName)
                {
                    case nameof(ErrorCode):
                    case nameof(Message):
                        break;
                    default:
                        return;
                }
            string message = Message;
            LocalDbContext dbContext;
            if (string.IsNullOrEmpty(Message) || (dbContext = validationContext.GetService<LocalDbContext>()) is null)
                return;
            Guid id = Id;
            AccessErrorCode errorCode = ErrorCode;
            if (dbContext.VolumeAccessErrors.Any(e => e.ErrorCode == errorCode && e.Message == message && id != e.Id))
                results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_DuplicateMessage, new string[] { nameof(Message) }));
        }

        internal static void Import(LocalDbContext dbContext, ILogger<LocalDbContext> logger, Guid volumeId, XElement accessErrorElement)
        {
            XName n = nameof(Id);
            Guid accessErrorId = accessErrorElement.GetAttributeGuid(n).Value;
            StringBuilder sql = new StringBuilder("INSERT INTO \"").Append(nameof(LocalDbContext.VolumeAccessErrors)).Append("\" (\"").Append(nameof(Id)).Append("\" , \"").Append(nameof(TargetId)).Append('"');
            List<object> values = new();
            values.Add(accessErrorId);
            values.Add(volumeId);
            foreach (XAttribute attribute in accessErrorElement.Attributes().Where(a => a.Name != n))
            {
                sql.Append(", \"").Append(attribute.Name.LocalName).Append('"');
                switch (attribute.Name.LocalName)
                {
                    case nameof(Message):
                        values.Add(attribute.Value);
                        break;
                    case nameof(ErrorCode):
                        values.Add(Enum.ToObject(typeof(AccessErrorCode), Enum.Parse<AccessErrorCode>(attribute.Value)));
                        break;
                    case nameof(CreatedOn):
                    case nameof(ModifiedOn):
                        values.Add(XmlConvert.ToDateTime(attribute.Value, XmlDateTimeSerializationMode.RoundtripKind));
                        break;
                    default:
                        throw new NotSupportedException($"Attribute {attribute.Name} is not supported for {nameof(VolumeAccessError)}");
                }
            }
            if (!accessErrorElement.IsEmpty)
            {
                string details = accessErrorElement.Value;
                if (details.Trim().Length > 0)
                {
                    sql.Append(", \"").Append(nameof(Details)).Append('"');
                    values.Add(details);
                }
            }
            sql.Append(") Values({0}");
            for (int i = 1; i < values.Count; i++)
                sql.Append(", {").Append(i).Append('}');
            logger.LogInformation($"Inserting {nameof(VolumeAccessError)} with Id {{Id}}", volumeId);
            dbContext.Database.ExecuteSqlRaw(sql.Append(')').ToString(), values.ToArray());
        }

        internal XElement Export(bool includeTargetId = false)
        {
            XElement result = new(LocalDbEntity.ElementName_AccessError,
                new XAttribute(nameof(Id), XmlConvert.ToString(Id)),
                new XAttribute(nameof(Message), Message),
                new XAttribute(nameof(ErrorCode), Enum.GetName(ErrorCode))
            );
            if (includeTargetId)
            {
                Guid targetId = TargetId;
                if (!targetId.Equals(Guid.Empty))
                    result.SetAttributeValue(nameof(TargetId), XmlConvert.ToString(targetId));
            }
            AddExportAttributes(result);
            if (Details.Trim().Length > 0)
                result.Add(new XCData(Details));
            return result;
        }
    }
}
