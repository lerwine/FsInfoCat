using System;

namespace FsInfoCat
{
    public interface ICrawlJobListItem : ICrawlJobLogRow
    {
        Guid ConfigurationId { get; }

        string ConfigurationDisplayName { get; }
    }
}

