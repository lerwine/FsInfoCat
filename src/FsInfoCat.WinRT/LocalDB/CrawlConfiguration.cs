using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsInfoCat.LocalDB
{
    public class CrawlConfiguration
    {
        private string _displayName = "";
        private string _notes = "";

        [Key]
        public virtual Guid Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Display name is required.")]
        [StringLength(1024, ErrorMessage = "Display name too long.")]
        public virtual string DisplayName { get => _displayName; set => _displayName = value.AsWsNormalizedOrEmpty(); }

        [Required]
        public virtual ushort MaxRecursionDepth { get; set; }

        public virtual ulong? MaxTotalItems { get; set; }

        [Range(typeof(long), "1", "9223372036854775807")]
        public virtual long? MaxDuration { get; set; }

        [Required(AllowEmptyStrings = true)]
        public virtual string Notes { get => _notes; set => _notes = value.EmptyIfNullOrWhiteSpace(); }

        //internal static void OnBuildEntity(EntityTypeBuilder<CrawlConfiguration> builder)
        //{
        //    _ = builder.HasOne(s => s.Root).WithOne(c => c.CrawlConfiguration).HasForeignKey<CrawlConfiguration>(nameof(RootId)).OnDelete(DeleteBehavior.Restrict);
        //}

    }
}
