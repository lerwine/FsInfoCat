using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public class CrawlConfigListItem : CrawlConfigListItemBase, IEquatable<CrawlConfigListItem>
    {
        private const string VIEW_NAME = "vCrawlConfigListing";

        internal static void OnBuildEntity([DisallowNull] EntityTypeBuilder<CrawlConfigListItem> builder) => (builder ?? throw new ArgumentOutOfRangeException(nameof(builder)))
            .ToView(VIEW_NAME)
            .Property(nameof(VolumeIdentifier)).HasConversion(VolumeIdentifier.Converter);

        public bool Equals(CrawlConfigListItem other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(ICrawlConfigurationListItem other)
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
