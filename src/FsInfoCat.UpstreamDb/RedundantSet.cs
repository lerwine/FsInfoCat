using FsInfoCat.Model;
using FsInfoCat.Model.Upstream;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.UpstreamDb
{
    public class RedundantSet : IUpstreamRedundantSet
    {
        private string _notes = "";

        internal static void BuildEntity(EntityTypeBuilder<RedundantSet> builder)
        {
            builder.HasKey(nameof(Id));
            builder.Property(nameof(Notes)).HasDefaultValue("").HasColumnType("nvarchar(max)").IsRequired();
        }

        public RedundantSet()
        {
            Redundancies = new HashSet<Redundancy>();
        }

        #region Column Properties

        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Notes { get => _notes; set => _notes = value ?? ""; }

        [Required]
        [Display(Name = nameof(ModelResources.DisplayName_CreatedOn), ResourceType = typeof(ModelResources))]
        public DateTime CreatedOn { get; set; }

        [Required]
        [Display(Name = nameof(ModelResources.DisplayName_ModifiedOn), ResourceType = typeof(ModelResources))]
        public DateTime ModifiedOn { get; set; }

        #endregion

        #region Navigation Properties

        public HashSet<Redundancy> Redundancies { get; set; }

        #endregion

        #region Explicit Members

        IReadOnlyCollection<IUpstreamRedundancy> IUpstreamRedundantSet.Redundancies => Redundancies;

        IReadOnlyCollection<IRedundancy> IRedundantSet.Redundancies => Redundancies;

        #endregion
    }
}
