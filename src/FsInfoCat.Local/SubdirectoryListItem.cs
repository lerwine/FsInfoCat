using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public class SubdirectoryListItem : SubdirectoryRow, ILocalSubdirectoryListItem, IEquatable<SubdirectoryListItem>
    {
        public const string VIEW_NAME = "vSubdirectoryListing";

        private string _crawlConfigDisplayName;

        public long SubdirectoryCount { get; set; }

        public long FileCount { get; set; }

        public long AccessErrorCount { get; set; }

        public long PersonalTagCount { get; set; }

        public long SharedTagCount { get; set; }

        public string CrawlConfigDisplayName { get => _crawlConfigDisplayName; set => _crawlConfigDisplayName = value.AsNonNullTrimmed(); }

        internal static void OnBuildEntity(EntityTypeBuilder<SubdirectoryListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(Id));

        protected bool ArePropertiesEqual([DisallowNull] ILocalSubdirectoryListItem other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] ISubdirectoryListItem other)
        {
            throw new NotImplementedException();
        }

        public virtual bool Equals(SubdirectoryListItem other)
        {
            throw new NotImplementedException();
        }

        public virtual bool Equals(ISubdirectoryListItem other)
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
            return base.ToString();
        }
    }
}
