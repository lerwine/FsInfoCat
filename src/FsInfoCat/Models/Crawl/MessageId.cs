using System;
using System.ComponentModel;

namespace FsInfoCat.Models.Crawl
{
    public enum MessageId
    {
        [Description("No path was provied")]
        NoPathProvided,

        [Description("Error getting machine identifier.")]
        ErrorGettingMachineIdentifier,

        [Description("Path is invalid.")]
        InvalidPath,

        [Description("Path not found.")]
        PathNotFound,

        [Description("Unable to obtain name or length.")]
        FileSystemInfoPropertyAccessError,

        [Description("Unable to obtain creation time.")]
        CreationTimeAccessError,

        [Description("Unable to obtain last write time.")]
        LastWriteTimeAccessError,

        [Description("Unable to obtain file system attributes.")]
        AttributesAccessError,

        [Description("Unable to enmerate files.")]
        DirectoryFilesAccessError,

        [Description("Unable to enmerate subdirectories.")]
        SubdirectoriesAccessError,

        [Description("Crawl operation stopped.")]
        CrawlOperationStopped,

        [Description("Maximum crawl depth has been reached.")]
        MaxDepthReached,

        [Description("Maximum crawl item count has been reached.")]
        MaxItemsReached,

        [Description("An unexpected error has occurred.")]
        UnexpectedError
    }
}
