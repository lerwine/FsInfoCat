using FsInfoCat.Model;
using FsInfoCat.Model.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.LocalDb
{
    public class RedundantSet : ILocalRedundantSet
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

        public Guid? UpstreamId { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_LastSynchronized), ResourceType = typeof(ModelResources))]
        public DateTime? LastSynchronized { get; set; }

        #endregion

        #region Navigation Properties

        public HashSet<Redundancy> Redundancies { get; set; }

        #endregion

        #region Explicit Members

        IReadOnlyCollection<ILocalRedundancy> ILocalRedundantSet.Redundancies => Redundancies;

        IReadOnlyCollection<IRedundancy> IRedundantSet.Redundancies => Redundancies;

        #endregion
    }
}
