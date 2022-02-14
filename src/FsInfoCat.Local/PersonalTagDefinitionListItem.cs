using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public class PersonalTagDefinitionListItem : PersonalTagDefinitionRow, ILocalTagDefinitionListItem, IEquatable<PersonalTagDefinitionListItem>
    {
        public const string VIEW_NAME = "vPersonalTagDefinitionListing";

        public long FileTagCount { get; set; }

        public long SubdirectoryTagCount { get; set; }

        public long VolumeTagCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<PersonalTagDefinitionListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(Id));

        protected bool ArePropertiesEqual([DisallowNull] ILocalTagDefinitionListItem other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] ITagDefinitionListItem other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(PersonalTagDefinitionListItem other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(ITagDefinitionListItem other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
