using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FsInfoCat.Local
{
    public class PersonalFileTagListItem : ItemTagListItem, IEquatable<PersonalFileTagListItem>
    {
        public const string VIEW_NAME = "vPersonalFileTagListing";

        internal static void OnBuildEntity(EntityTypeBuilder<PersonalFileTagListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(DefinitionId), nameof(TaggedId));

        public bool Equals(PersonalFileTagListItem other)
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
