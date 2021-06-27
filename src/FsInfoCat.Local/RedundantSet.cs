using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FsInfoCat.Local
{
    public class RedundantSet : LocalDbEntity, ILocalRedundantSet
    {
        #region Fields

        public static readonly Regex IPV4HostRegex = new(@"^((?<!\d)(0(?=\d))*(?!25[6-9]|([3-9]\d|1\d\d)\d)\d{1,3}\.?){4}(?<!\.)$", RegexOptions.Compiled);

        public static readonly Regex IPV6HostRegex = new(@"([a-f\d]{1,4}:){7}(:|[a-f\d]{1,4})|(?=(\w*:){2,7}\w*\]?$)(([a-f\d]{1,4}:)+|:):([a-f\d]{1,4}(:[a-f\d]{1,4})*)?", RegexOptions.Compiled);

        public static readonly Regex DnsRegex = new(@"[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?", RegexOptions.Compiled);

        public static readonly Regex HOST_NAME_OR_ADDRESS_FOR_URI_REGEX = new(@"^
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
        private readonly IPropertyChangeTracker<Guid> _binaryPropertiesId;
        private readonly IPropertyChangeTracker<BinaryPropertySet> _binaryProperties;
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

        public virtual Guid BinaryPropertiesId
        {
            get => _binaryPropertiesId.GetValue();
            set
            {
                if (_binaryPropertiesId.SetValue(value))
                {
                    BinaryPropertySet nav = _binaryProperties.GetValue();
                    if (!(nav is null || nav.Id.Equals(value)))
                        _binaryProperties.SetValue(null);
                }
            }
        }

        public virtual BinaryPropertySet BinaryProperties
        {
            get => _binaryProperties.GetValue();
            set
            {
                if (_binaryProperties.SetValue(value))
                {
                    if (value is null)
                        _binaryPropertiesId.SetValue(Guid.Empty);
                    else
                        _binaryPropertiesId.SetValue(value.Id);
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

        ILocalBinaryPropertySet ILocalRedundantSet.BinaryProperties { get => BinaryProperties; set => BinaryProperties = (BinaryPropertySet)value; }

        IBinaryPropertySet IRedundantSet.BinaryProperties { get => BinaryProperties; set => BinaryProperties = (BinaryPropertySet)value; }

        IEnumerable<ILocalRedundancy> ILocalRedundantSet.Redundancies => Redundancies.Cast<ILocalRedundancy>();

        IEnumerable<IRedundancy> IRedundantSet.Redundancies => Redundancies.Cast<IRedundancy>();

        #endregion

        public RedundantSet()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _remediationStatus = AddChangeTracker(nameof(RemediationStatus), RedundancyRemediationStatus.Unconfirmed);
            _reference = AddChangeTracker(nameof(Reference), "", TrimmedNonNullStringCoersion.Default);
            _notes = AddChangeTracker(nameof(Notes), "", NonWhiteSpaceOrEmptyStringCoersion.Default);
            _binaryPropertiesId = AddChangeTracker(nameof(BinaryPropertiesId), Guid.Empty);
            _binaryProperties = AddChangeTracker<BinaryPropertySet>(nameof(BinaryProperties), null);
        }

        protected override void OnPropertyChanging(PropertyChangingEventArgs args)
        {
            if (args.PropertyName == nameof(Id) && _id.IsChanged)
                throw new InvalidOperationException();
            base.OnPropertyChanging(args);
        }

        internal static void BuildEntity(EntityTypeBuilder<RedundantSet> builder)
        {
            builder.HasOne(sn => sn.BinaryProperties).WithMany(d => d.RedundantSets).HasForeignKey(nameof(BinaryPropertiesId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
        }

        internal static async Task<(Guid redundantSetId, XElement[] redundancies)> ImportAsync(LocalDbContext dbContext, ILogger<LocalDbContext> logger, Guid binaryPropertiesId, XElement redundantSetElement)
        {
            string n = nameof(Id);
            Guid redundantSetId = redundantSetElement.GetAttributeGuid(n).Value;
            logger.LogInformation($"Inserting {nameof(RedundantSet)} with Id {{Id}}", binaryPropertiesId);
            await new InsertQueryBuilder(nameof(LocalDbContext.RedundantSets), redundantSetElement, n).AppendGuid(nameof(BinaryPropertiesId), binaryPropertiesId)
                .AppendString(nameof(Reference)).AppendElementString(nameof(Notes)).AppendEnum<RedundancyRemediationStatus>(nameof(RemediationStatus))
                .AppendDateTime(nameof(CreatedOn)).AppendDateTime(nameof(ModifiedOn)).AppendDateTime(nameof(LastSynchronizedOn)).AppendGuid(nameof(UpstreamId))
                .ExecuteSqlAsync(dbContext.Database);
            return (redundantSetId, redundantSetElement.Elements(nameof(Redundancy)).ToArray());
        }
    }
}
