using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FsInfoCat.Local
{
    public class SharedSubdirectoryTagListItem : ItemTagListItem, IEquatable<SharedFileTagListItem>
    {
        public const string VIEW_NAME = "vSharedSubdirectoryTagListing";

        internal static void OnBuildEntity(EntityTypeBuilder<SharedSubdirectoryTagListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(DefinitionId), nameof(TaggedId));

        public bool Equals(SharedFileTagListItem other)
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
