﻿namespace FsInfoCat.Local
{
    /// <summary>
    /// Interface for list item entities that represent a symbolic name for a file system type.
    /// </summary>
    /// <seealso cref="ILocalSymbolicNameRow" />
    /// <seealso cref="ISymbolicNameListItem" />
    public interface ILocalSymbolicNameListItem : ILocalSymbolicNameRow, ISymbolicNameListItem { }
}