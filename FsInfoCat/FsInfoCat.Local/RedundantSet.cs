using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace FsInfoCat.Local
{
    public class RedundantSet : LocalDbEntity, ILocalRedundantSet
    {
        #region Fields

        public static readonly Regex IPV4HostRegex = new Regex(@"^((?<!\d)(0(?=\d))*(?!25[6-9]|([3-9]\d|1\d\d)\d)\d{1,3}\.?){4}(?<!\.)$", RegexOptions.Compiled);

        public static readonly Regex IPV6HostRegex = new Regex(@"([a-f\d]{1,4}:){7}(:|[a-f\d]{1,4})|(?=(\w*:){2,7}\w*\]?$)(([a-f\d]{1,4}:)+|:):([a-f\d]{1,4}(:[a-f\d]{1,4})*)?", RegexOptions.Compiled);

        public static readonly Regex DnsRegex = new Regex(@"[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?", RegexOptions.Compiled);

        public static readonly Regex HOST_NAME_OR_ADDRESS_FOR_URI_REGEX = new Regex(@"^
(
    (?<ipv4>((?<!\d)(0(?=\d))*(?!25[6-9]|([3-9]\d|1\d\d)\d)\d{1,3}\.?){4}(?<!\.))
|
    (?=(\[[a-f\d:]+\]|[a-f\d:]+)$)
    \[?(?<ipv6>([a-f\d]{1,4}:){7}(:|[a-f\d]{1,4})|(?=(\w*:){2,7}\w*\]?$)(([a-f\d]{1,4}:)+|:):([a-f\d]{1,4}(:[a-f\d]{1,4})*)?)\]?
|
    (?=[\w-.]{1,255}(?![\w-.]))
    (?<dns>[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?)
)$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<RedundancyRemediationStatus> _remediationStatus;
        private readonly IPropertyChangeTracker<string> _reference;
        private readonly IPropertyChangeTracker<string> _notes;
        private readonly IPropertyChangeTracker<Guid> _contentInfoId;
        private readonly IPropertyChangeTracker<ContentInfo> _contentInfo;
        private HashSet<Redundancy> _redundancies = new();

        #endregion

        #region Properties

        [Key]
        public virtual Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        [Required]
        public virtual RedundancyRemediationStatus RemediationStatus { get => _remediationStatus.GetValue(); set => _remediationStatus.SetValue(value); }

        [Required(AllowEmptyStrings = true)]
        [StringLength(DbConstants.DbColMaxLen_ShortName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual string Reference { get => _reference.GetValue(); set => _reference.SetValue(value); }

        [Required(AllowEmptyStrings = true)]
        public virtual string Notes { get => _notes.GetValue(); set => _notes.SetValue(value); }

        public virtual Guid ContentInfoId
        {
            get => _contentInfoId.GetValue();
            set
            {
                if (_contentInfoId.SetValue(value))
                {
                    ContentInfo nav = _contentInfo.GetValue();
                    if (!(nav is null || nav.Id.Equals(value)))
                        _contentInfo.SetValue(null);
                }
            }
        }

        public virtual ContentInfo ContentInfo
        {
            get => _contentInfo.GetValue();
            set
            {
                if (_contentInfo.SetValue(value))
                {
                    if (value is null)
                        _contentInfoId.SetValue(Guid.Empty);
                    else
                        _contentInfoId.SetValue(value.Id);
                }
            }
        }

        public virtual HashSet<Redundancy> Redundancies
        {
            get => _redundancies;
            set => CheckHashSetChanged(_redundancies, value, h => _redundancies = h);
        }

        #endregion

        #region Explicit Members

        ILocalContentInfo ILocalRedundantSet.ContentInfo { get => ContentInfo; set => ContentInfo = (ContentInfo)value; }

        IContentInfo IRedundantSet.ContentInfo { get => ContentInfo; set => ContentInfo = (ContentInfo)value; }

        IEnumerable<ILocalRedundancy> ILocalRedundantSet.Redundancies => Redundancies.Cast<ILocalRedundancy>();

        IEnumerable<IRedundancy> IRedundantSet.Redundancies => Redundancies.Cast<IRedundancy>();

        #endregion

        public RedundantSet()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _remediationStatus = AddChangeTracker(nameof(RemediationStatus), RedundancyRemediationStatus.Unconfirmed);
            _reference = AddChangeTracker(nameof(Reference), "", NonNullStringCoersion.Default);
            _notes = AddChangeTracker(nameof(Notes), "", NonNullStringCoersion.Default);
            _contentInfoId = AddChangeTracker(nameof(ContentInfoId), Guid.Empty);
            _contentInfo = AddChangeTracker<ContentInfo>(nameof(ContentInfo), null);
        }

        internal static void BuildEntity(EntityTypeBuilder<RedundantSet> builder)
        {
            builder.HasOne(sn => sn.ContentInfo).WithMany(d => d.RedundantSets).HasForeignKey(nameof(ContentInfoId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
        }

        internal static (Guid redundantSetId, XElement[] redundancies) Import(LocalDbContext dbContext, ILogger<LocalDbContext> logger, Guid contentInfoId, XElement redundantSetElement)
        {
            XName n = nameof(Id);
            Guid redundantSetId = redundantSetElement.GetAttributeGuid(n).Value;
            StringBuilder sql = new StringBuilder("INSERT INTO \"").Append(nameof(LocalDbContext.RedundantSets)).Append("\" (\"").Append(nameof(Id)).Append("\" , \"").Append(nameof(ContentInfoId)).Append('"');
            List<object> values = new();
            values.Add(redundantSetId);
            values.Add(contentInfoId);
            foreach (XAttribute attribute in redundantSetElement.Attributes().Where(a => a.Name != n))
            {
                sql.Append(", \"").Append(attribute.Name.LocalName).Append('"');
                switch (attribute.Name.LocalName)
                {
                    case nameof(Reference):
                    case nameof(Notes):
                        values.Add(attribute.Value);
                        break;
                    case nameof(RemediationStatus):
                        values.Add(Enum.Parse<RedundancyRemediationStatus>(attribute.Value));
                        break;
                    case nameof(CreatedOn):
                    case nameof(ModifiedOn):
                    case nameof(LastSynchronizedOn):
                        values.Add(XmlConvert.ToDateTime(attribute.Value, XmlDateTimeSerializationMode.RoundtripKind));
                        break;
                    case nameof(UpstreamId):
                        values.Add(XmlConvert.ToGuid(attribute.Value));
                        break;
                    default:
                        throw new NotSupportedException($"Attribute {attribute.Name} is not supported for {nameof(RedundantSet)}");
                }
            }
            sql.Append(") Values({0}");
            for (int i = 1; i < values.Count; i++)
                sql.Append(", {").Append(i).Append('}');
            logger.LogInformation($"Inserting {nameof(RedundantSet)} with Id {{Id}}", contentInfoId);
            dbContext.Database.ExecuteSqlRaw(sql.Append(')').ToString(), values.ToArray());
            return (redundantSetId, redundantSetElement.Elements(nameof(Redundancy)).ToArray());
        }
    }
}
