﻿namespace FsInfoCat.Desktop.ViewModel
{
    public interface IFileWithAncestorNamesViewModel : IFileRowViewModel, ICrudEntityRowViewModel
    {
        new IFileListItemWithAncestorNames Entity { get; }
    }
}