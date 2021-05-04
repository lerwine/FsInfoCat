namespace FsInfoCat.Components
{
    /// <summary>
    /// Base interface objects that represent a view of a <see cref="System.ComponentModel.PropertyDescriptor"/>.
    /// </summary>
    public interface IPropertyModel
    {
        /// <summary>
        /// The <see cref="System.ComponentModel.MemberDescriptor.Name">name</see> (identifier) of the property.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The name to display in the user interface.
        /// </summary>
        /// <remarks>For types that derive from <see cref="System.Data.Common.DbConnectionStringBuilder"/> this also contains the key that is used within the connection string.
        /// <para>This value returns the value of the <see cref="System.ComponentModel.DisplayNameAttribute">DisplayName</see> attribute or the <see cref="System.ComponentModel.MemberDescriptor.Name"/> of
        /// the property if no <see cref="System.ComponentModel.DisplayNameAttribute">DisplayName</see> was used or its value was empty.</para></remarks>
        string DisplayName { get; }

        /// <summary>
        /// The name of the category to which the member belongs
        /// </summary>
        /// <remarks>This value returns the value of the <see cref="System.ComponentModel.CategoryAttribute">Category</see> attribute or the value of
        /// the <see cref="System.ComponentModel.CategoryAttribute.Default">Default Category (<c>&quot;Misc&quot;</c>)</see> if
        /// no <see cref="System.ComponentModel.CategoryAttribute">Category</see> was used or its value was empty.</remarks>
        string Category { get; }

        /// <summary>
        /// The description of the property.
        /// </summary>
        /// <remarks>This value returns the value of the <see cref="System.ComponentModel.DescriptionAttribute">Description</see> attribute or an empty string
        /// (<c>&quot;&quot;</c>) if no <see cref="System.ComponentModel.DescriptionAttribute">Description</see> was used.</remarks>
        string Description { get; }

        /// <summary>
        /// Indicates whether this property is read-only.
        /// </summary>
        /// <remarks>If this is <see langword="true"/>, that may only be due to the usage of the <see cref="System.ComponentModel.ReadOnlyAttribute">ReadOnly</see> and
        /// does not necessarily mean that the property has no mutator.</remarks>
        bool IsReadOnly { get; }

        /// <summary>
        /// The full <see cref="System.ComponentModel.PropertyDescriptor.PropertyType">type</see> name for the property.
        /// </summary>
        string TypeFullName { get; }

        /// <summary>
        /// The simple <see cref="System.ComponentModel.PropertyDescriptor.PropertyType">type</see> name for the property.
        /// </summary>
        string TypeSimpleName { get; }
    }
}
