using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FsInfoCat.Local
{
    public class PersonalSubdirectoryTagListItem : ItemTagListItem, IEquatable<PersonalSubdirectoryTagListItem>
    {
        public const string VIEW_NAME = "vPersonalSubdirectoryTagListing";

        internal static void OnBuildEntity(EntityTypeBuilder<PersonalSubdirectoryTagListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(DefinitionId), nameof(TaggedId));

        public bool Equals(PersonalSubdirectoryTagListItem other)
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
