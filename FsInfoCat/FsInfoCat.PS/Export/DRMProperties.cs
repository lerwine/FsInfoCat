using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace FsInfoCat.PS.Export
{
    public class DRMProperties : ExportSet.ExtendedPropertiesBase
    {
        [XmlAttribute(nameof(DatePlayExpires))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_DatePlayExpires { get => DatePlayExpires.ToDateTimeXml(); set => DatePlayExpires = value.FromXmlDateTime(); }
#pragma warning restore IDE1006 // Naming Styles
        /// <summary>
        /// Indicates when play expires for digital rights management.
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemDRM.DatePlayExpires"/>
        [XmlIgnore]
        public System.DateTime? DatePlayExpires { get; set; }

        [XmlAttribute(nameof(DatePlayStarts))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_DatePlayStarts { get => DatePlayStarts.ToDateTimeXml(); set => DatePlayStarts = value.FromXmlDateTime(); }
#pragma warning restore IDE1006 // Naming Styles
        /// <summary>
        /// Indicates when play starts for digital rights management.
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemDRM.DatePlayStarts"/>
        [XmlIgnore]
        public System.DateTime? DatePlayStarts { get; set; }

        /// <summary>
        /// Displays the description for digital rights management.
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemDRM.Description"/>
        public string Description { get; set; }

        /// <summary>
        /// Indicates whether the content is protected
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemDRM.IsProtected"/>
        public bool? IsProtected { get; set; }

        /// <summary>
        /// Indicates the play count for digital rights management.
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemDRM.PlayCount"/>
        public uint? PlayCount { get; set; }

        public IEnumerable<File> GetFiles() => throw new NotImplementedException();

        internal static DRMProperties Create(System.IO.FileInfo fileInfo)
        {
            throw new NotImplementedException();
        }
    }
}
