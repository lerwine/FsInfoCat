using System;

namespace FsInfoCat
{
    public interface ICrawlJobListItem : ICrawlJobLogRow, IEquatable<ICrawlJobListItem>
    {
        Guid ConfigurationId { get; }

        string ConfigurationDisplayName { get; }
    }
}
