using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Base class for entities that represent a symbolic name for a file system type.
    /// </summary>
    public abstract class SymbolicNameRow : LocalDbEntity, ILocalSymbolicNameRow
    {
        #region Fields

        /// <summary>
        /// The default file system symbolic name to use when the file system cannot be detected.
        /// </summary>
        public const string Fallback_Symbolic_Name = "NTFS";

        private Guid? _id;
        private string _name = string.Empty;
        private string _notes = string.Empty;

        #endregion

        #region Properties

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

        /// <summary>
        /// Gets the symbolic name.
        /// </summary>
        /// <value>The symbolic name which refers to a file system type.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Name), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [StringLength(DbConstants.DbColMaxLen_SimpleName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [NotNull]
        [BackingField(nameof(_name))]
        public virtual string Name { get => _name; set => _name = value ?? ""; }

        /// <summary>
        /// Gets the custom notes for the current symbolic name.
        /// </summary>
        /// <value>The custom notes to associate with the current symblic name.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Notes), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Required(AllowEmptyStrings = true)]
        [NotNull]
        [BackingField(nameof(_notes))]
        public virtual string Notes { get => _notes; set => _notes = value.EmptyIfNullOrWhiteSpace(); }

        /// <summary>
        /// Gets a value indicating whether this symbolic name is inactive.
        /// </summary>
        /// <value><see langword="true" /> if this symbolic name  is inactive; otherwise, <see langword="false" />.</value>
        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_IsInactive), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual bool IsInactive { get; set; }

        /// <summary>
        /// Gets the priority for this symbolic name.
        /// </summary>
        /// <value>The priority of this symbolic name in relation to other symbolic names that refer to the same file system type, with lower values being higher priority.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Priority), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Required]
        public virtual int Priority { get; set; }

        /// <summary>
        /// Gets the primary key of the associated filesystem.
        /// </summary>
        /// <value>The <see cref="FileSystemRow.Id"/> value of the associated <see cref="FileSystem"/>.</value>
        [Required]
        public virtual Guid FileSystemId { get; set; }

        #endregion

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected override void OnValidate(ValidationContext validationContext, List<ValidationResult> results)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            base.OnValidate(validationContext, results);
            if (string.IsNullOrWhiteSpace(validationContext.MemberName) || validationContext.MemberName == nameof(Name))
            {
                string name = Name;
                LocalDbContext dbContext;
                using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
                if (string.IsNullOrEmpty(name) || (dbContext = serviceScope.ServiceProvider.GetService<LocalDbContext>()) is null)
                    return;
                Guid id = Id;
                if (dbContext.SymbolicNames.Any(sn => id != sn.Id && sn.Name == name))
                    results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_DuplicateName, new string[] { nameof(Name) }));
            }
        }

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="ILocalSymbolicNameRow" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected virtual bool ArePropertiesEqual([DisallowNull] ILocalSymbolicNameRow other)
        {
            // TODO: Implement ArePropertiesEqual(ILocalSymbolicNameRow)
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="ISymbolicNameRow" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected virtual bool ArePropertiesEqual([DisallowNull] ISymbolicNameRow other)
        {
            // TODO: Implement ArePropertiesEqual(ISymbolicNameRow)
            throw new NotImplementedException();
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public override int GetHashCode()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            Guid? id = _id;
            if (id.HasValue) return id.Value.GetHashCode();
            HashCode hash = new();
            hash.Add(_name);
            hash.Add(_notes);
            hash.Add(IsInactive);
            hash.Add(Priority);
            hash.Add(FileSystemId);
            hash.Add(UpstreamId);
            hash.Add(LastSynchronizedOn);
            hash.Add(CreatedOn);
            hash.Add(ModifiedOn);
            return hash.ToHashCode();
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
}
