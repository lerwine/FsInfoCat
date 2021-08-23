using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FsInfoCat.Local
{
    public abstract class RedundantSetRow : LocalDbEntity, IRedundantSetRow, ISimpleIdentityReference<RedundantSetRow>
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
        private readonly IPropertyChangeTracker<string> _reference;
        private readonly IPropertyChangeTracker<string> _notes;
        private readonly IPropertyChangeTracker<Guid> _binaryPropertiesId;

        #endregion

        #region Properties

        [Key]
        public virtual Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

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
                    OnBinaryPropertiesIdChanged(value);
            }
        }

        RedundantSetRow IIdentityReference<RedundantSetRow>.Entity => this;

        IDbEntity IIdentityReference.Entity => this;

        protected virtual void OnBinaryPropertiesIdChanged(Guid value) { }

        #endregion

        public RedundantSetRow()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _reference = AddChangeTracker(nameof(Reference), "", TrimmedNonNullStringCoersion.Default);
            _notes = AddChangeTracker(nameof(Notes), "", NonWhiteSpaceOrEmptyStringCoersion.Default);
            _binaryPropertiesId = AddChangeTracker(nameof(BinaryPropertiesId), Guid.Empty);
        }

        protected override void OnPropertyChanging(PropertyChangingEventArgs args)
        {
            if (args.PropertyName == nameof(Id) && _id.IsChanged)
                throw new InvalidOperationException();
            base.OnPropertyChanging(args);
        }

        IEnumerable<Guid> IIdentityReference.GetIdentifiers()
        {
            yield return Id;
        }
    }
}
