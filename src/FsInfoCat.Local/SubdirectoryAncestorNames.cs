using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace FsInfoCat.Local
{
    public class SubdirectoryAncestorNames : ISubdirectoryAncestorName
    {
        public const string VIEW_NAME = "vSubdirectoryAncestorNames";

        private readonly object SyncRoot = new();

        private Guid? _id;
        private string _name = string.Empty;
        private string _ancestorNames = string.Empty;

        /// <summary>
        /// Gets the primary key value.
        /// </summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the current entity's primary key the database.</value>
        [Key]
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

        [StringLength(DbConstants.DbColMaxLen_FileName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual string Name { get => _name; set => _name = value ?? ""; }

        public virtual Guid? ParentId { get; set; }

        public string AncestorNames { get => _ancestorNames; set => _ancestorNames = value.EmptyIfNullOrWhiteSpace(); }

        internal static void OnBuildEntity(EntityTypeBuilder<SubdirectoryAncestorNames> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(Id));
    }
}
