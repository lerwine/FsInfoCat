﻿namespace FsInfoCat.Local
{
    /// <summary>
    /// Represents a listing item for set of files that have the same size, Hash and remediation status.
    /// </summary>
    /// <seealso cref="IRedundantSetListItem" />
    /// <seealso cref="ILocalRedundantSetRow" />
    public interface ILocalRedundantSetListItem : IRedundantSetListItem, ILocalRedundantSetRow { }
}