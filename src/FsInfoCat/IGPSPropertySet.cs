using System;

namespace FsInfoCat
{
    /// <summary>Interface for database objects that contain extended file GPS property values.</summary>
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="IGPSProperties" />
    public interface IGPSPropertySet : IPropertySet, IGPSProperties, IEquatable<IGPSPropertySet> { }
}
