using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace FsInfoCat.Local
{
    // TODO: Document SharedTagDefinitionRow class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public abstract class SharedTagDefinitionRow : LocalDbEntity, ILocalTagDefinitionRow
    {
        private Guid? _id;
        private string _name = string.Empty;
        private string _description = string.Empty;

        /// <summary>
        /// Gets the primary key value.
        /// </summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the current entity's primary key the database.</value>
        [Key]
        [BackingField(nameof(_id))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Id), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual Guid Id
        {
            get => _id ?? Guid.Empty;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (_id.HasValue)
                    {
                        if (!_id.Value.Equals(value))
                            throw new InvalidOperationException();
                    }
                    else if (value.Equals(Guid.Empty))
                        return;
                    _id = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [StringLength(DbConstants.DbColMaxLen_SimpleName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [NotNull]
        [BackingField(nameof(_name))]
        public virtual string Name { get => _name; set => _name = value.AsWsNormalizedOrEmpty(); }

        [Required(AllowEmptyStrings = true)]
        [NotNull]
        [BackingField(nameof(_description))]
        public string Description { get => _description; set => _description = value.EmptyIfNullOrWhiteSpace(); }

        [Required]
        public bool IsInactive { get; set; }

        protected virtual bool ArePropertiesEqual([DisallowNull] ILocalTagDefinitionRow other)
        {
            // TODO: Implement ArePropertiesEqual(ILocalTagDefinitionRow)
            throw new NotImplementedException();
        }

        protected virtual bool ArePropertiesEqual([DisallowNull] ITagDefinitionRow other)
        {
            // TODO: Implement ArePropertiesEqual(ITagDefinitionRow)
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            Guid? id = _id;
            if (id.HasValue) return id.Value.GetHashCode();
            return HashCode.Combine(_name, _description, IsInactive, UpstreamId, LastSynchronizedOn, CreatedOn, ModifiedOn);
        }

        /// <summary>
        /// Gets the unique identifier of the current entity if it has been assigned.
        /// </summary>
        /// <param name="result">Receives the unique identifier value.</param>
        /// <returns><see langword="true" /> if the <see cref="Id" /> property has been set; otherwise, <see langword="false" />.</returns>
        public bool TryGetId(out Guid result)
        {
            Guid? id = _id;
            if (id.HasValue)
            {
                result = id.Value;
                return true;
            }
            result = Guid.Empty;
            return false;
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
