using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local
{
    public class FileSystem
    {
        private string _displayName = "";

        public Guid Id { get; set; }

        public Guid DefaultSymbolicNameId { get; set; }

        [Display(Name = nameof(Properties.Resources.DisplayName_DefaultSymbolicName), ResourceType = typeof(Properties.Resources))]
        [Required(ErrorMessageResourceName = nameof(Properties.Resources.ErrorMessage_DefaultSymbolicNameRequired), ErrorMessageResourceType = typeof(Properties.Resources))]
        public virtual SymbolicName DefaultSymbolicName { get; set; }

        internal static void BuildEntity(EntityTypeBuilder<FileSystem> builder)
        {
            builder.HasKey(nameof(Id));
            builder.HasOne(fs => fs.DefaultSymbolicName).WithMany(d => d.FileSystemDefaults).HasForeignKey(nameof(DefaultSymbolicNameId)).IsRequired();
        }

        [Display(Name = nameof(Properties.Resources.DisplayName_DisplayName), ResourceType = typeof(Properties.Resources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(Properties.Resources.ErrorMessage_DisplayNameRequired), ErrorMessageResourceType = typeof(Properties.Resources))]
        [MaxLength(DbConstants.DbColMaxLen_DisplayName, ErrorMessageResourceName = nameof(Properties.Resources.ErrorMessage_NameLength), ErrorMessageResourceType = typeof(Properties.Resources))]
        public string DisplayName { get => _displayName; set => _displayName = value ?? ""; }
    }
}
