﻿namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for list item entities containing extended file properties for image files.
    /// </summary>
    /// <seealso cref="ILocalImagePropertiesRow" />
    /// <seealso cref="ILocalPropertiesListItem" />
    /// <seealso cref="IImagePropertiesListItem" />
    public interface ILocalImagePropertiesListItem : ILocalImagePropertiesRow, ILocalPropertiesListItem, IImagePropertiesListItem { }
}