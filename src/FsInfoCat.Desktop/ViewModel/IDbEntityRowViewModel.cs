using System;

namespace FsInfoCat.Desktop.ViewModel
{
    public interface IDbEntityRowViewModel
    {
        DateTime CreatedOn { get; }
        DateTime ModifiedOn { get; }
        DbEntity Entity { get; }
    }

    public interface IDbEntityRowViewModel<TEntity> : IDbEntityRowViewModel
        where TEntity : DbEntity
    {
        new TEntity Entity { get; set; }
    }
    public interface IFsItemRowViewModel : IDbEntityRowViewModel
    {
        new IDbFsItemRow Entity { get; }
    }
    public interface IFsItemListItemViewModel : IFsItemRowViewModel
    {
        new IDbFsItemListItem Entity { get; }
    }
    public interface ISubdirectoryRowViewModel : IFsItemRowViewModel
    {
        new ISubdirectoryRow Entity { get; }
    }
    public interface ISubdirectoryListItemViewModel : IFsItemListItemViewModel, ISubdirectoryRowViewModel
    {
        new ISubdirectoryListItem Entity { get; }
    }
    public interface ISubdirectoryListItemWithAncestorNamesViewModel : ISubdirectoryListItemViewModel
    {
        new ISubdirectoryListItemWithAncestorNames Entity { get; }
    }
    public interface IFileRowViewModel : IFsItemRowViewModel
    {
        new IFileRow Entity { get; }
    }
    public interface IFileListItemViewModel : IFsItemListItemViewModel, IFileRowViewModel
    {
    }
    public interface ICrawlJobRowViewModel : IDbEntityRowViewModel
    {
        new ICrawlJobLogRow Entity { get; }
    }
    public interface ICrawlJobListItemViewModel : ICrawlJobRowViewModel
    {
        new ICrawlJobListItem Entity { get; }
    }
    public interface ICrawlConfigurationRowViewModel : IDbEntityRowViewModel
    {
        new ICrawlConfigurationRow Entity { get; }
    }
    public interface ICrawlConfigListItemViewModel : ICrawlConfigurationRowViewModel
    {
        new ICrawlConfigurationListItem Entity { get; }
    }
    public interface ICrawlConfigDetailsViewModel : ICrawlConfigurationRowViewModel
    {
        new ICrawlConfiguration Entity { get; }
    }
    public interface IFileSystemRowViewModel : IDbEntityRowViewModel
    {
        new IFileSystemRow Entity { get; }
    }
    public interface IFileWithAncestorNamesViewModel : IFileRowViewModel, ICrudEntityRowViewModel
    {
        new IFileListItemWithAncestorNames Entity { get; }
    }
    public interface IFileWithBinaryPropertiesAndAncestorNamesViewModel : IFileWithAncestorNamesViewModel
    {
        new IFileListItemWithBinaryPropertiesAndAncestorNames Entity { get; }
    }
    public interface IFileWithBinaryPropertiesViewModel : IFileRowViewModel, ICrudEntityRowViewModel
    {
        new IFileListItemWithBinaryProperties Entity { get; }
    }
}
