using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file GPS information properties.
    /// </summary>
    /// <seealso cref="IGPSPropertiesListItem" />
    /// <seealso cref="IGPSPropertySet" />
    public interface IGPSPropertiesRow : IPropertiesRow, IGPSProperties, IEquatable<IGPSPropertiesRow> { }
}
