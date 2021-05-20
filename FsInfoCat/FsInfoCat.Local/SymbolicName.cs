using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local
{
    public class SymbolicName
    {
        private string _name = "";

        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(Properties.Resources.ErrorMessage_NameRequired), ErrorMessageResourceType = typeof(Properties.Resources))]
        [MaxLength(DbConstants.DbColMaxLen_SimpleName, ErrorMessageResourceName = nameof(Properties.Resources.ErrorMessage_NameLength), ErrorMessageResourceType = typeof(Properties.Resources))]
        public string Name { get => _name; set => _name = value ?? ""; }

        [Display(Name = nameof(Properties.Resources.DisplayName_FileSystemDefaults), ResourceType = typeof(Properties.Resources))]
        public virtual HashSet<FileSystem> FileSystemDefaults { get; set; }

        public SymbolicName()
        {
            FileSystemDefaults = new HashSet<FileSystem>();
        }

        internal static void BuildEntity(EntityTypeBuilder<SymbolicName> builder)
        {
            builder.HasKey(nameof(Id));
            builder.Property(nameof(Name)).HasMaxLength(DbConstants.DbColMaxLen_SimpleName).IsRequired();
        }
    }
}
