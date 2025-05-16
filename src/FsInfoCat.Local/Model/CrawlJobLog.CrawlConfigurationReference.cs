using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model;

public partial class CrawlJobLog
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    // CodeQL [cs/inconsistent-equals-and-gethashcode]: GetHashCode() of base class is sufficient
    protected class CrawlConfigurationReference : ForeignKeyReference<CrawlConfiguration>, IForeignKeyReference<ILocalCrawlConfiguration>, IForeignKeyReference<ICrawlConfiguration>
    {
        internal CrawlConfigurationReference(object syncRoot) : base(syncRoot) { }

        ILocalCrawlConfiguration IForeignKeyReference<ILocalCrawlConfiguration>.Entity => Entity;

        ICrawlConfiguration IForeignKeyReference<ICrawlConfiguration>.Entity => Entity;

        bool IEquatable<IForeignKeyReference<ILocalCrawlConfiguration>>.Equals(IForeignKeyReference<ILocalCrawlConfiguration> other)
        {
            throw new NotImplementedException();
        }

        bool IEquatable<IForeignKeyReference<ICrawlConfiguration>>.Equals(IForeignKeyReference<ICrawlConfiguration> other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj) => obj is not null && ((obj is ForeignKeyReference<CrawlConfiguration> other) ? Equals(other) : base.Equals(obj));
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
