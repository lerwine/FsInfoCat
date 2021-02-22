using System;
using System.ComponentModel;
using static FsInfoCat.Util.ExtensionMethods;

namespace FsInfoCat.Models.Crawl
{
    public enum MessageId
    {
        [Description(MESSAGE_NO_PATH_PROVIDED)]
        NoPathProvided,

        [Description(MESSAGE_ERROR_GETTING_MACHINE_IDENTIFIER)]
        ErrorGettingMachineIdentifier,

        [Description(MESSAGE_INVALID_PATH)]
        InvalidPath,

        [Description(MESSAGE_INVALID_ABSOLUTE_FILE_URI)]
        InvalidAbsoluteFileUri,

        [Description(MESSAGE_PATH_NOT_FOUND)]
        PathNotFound,

        [Description(MESSAGE_FILESYSTEM_INFO_PROPERTY_ACCESS_ERROR)]
        FileSystemInfoPropertyAccessError,

        [Description(MESSAGE_CREATION_TIME_ACCESS_ERROR)]
        CreationTimeAccessError,

        [Description(MESSAGE_LAST_WRITE_TIME_ACCESS_ERROR)]
        LastWriteTimeAccessError,

        [Description(MESSAGE_ATTRIBUTES_ACCESS_ERROR)]
        AttributesAccessError,

        [Description(MESSAGE_DIRECTORY_FILES_ACCESS_ERROR)]
        DirectoryFilesAccessError,

        [Description(MESSAGE_SUBDIRECTORIES_ACCESS_ERROR)]
        SubdirectoriesAccessError,

        [Description(MESSAGE_CRAWL_OPERATION_STOPPED)]
        CrawlOperationStopped,

        [Description(MESSAGE_MAX_DEPTH_REACHED)]
        MaxDepthReached,

        [Description(MESSAGE_MAX_ITEMS_REACHED)]
        MaxItemsReached,

        [Description(MESSAGE_UNEXPECTED_ERROR)]
        UnexpectedError
    }
}
