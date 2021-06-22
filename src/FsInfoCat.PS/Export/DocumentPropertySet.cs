using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace FsInfoCat.PS.Export
{
    public class DocumentPropertySet : ExportSet.ExtendedPropertySetBase
    {
        /// <summary>
        /// Gets the Client ID
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemDocument.ClientID"/>
        public string ClientID { get; set; }

        /// <summary>
        /// Gets the Contributor
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemDocument.Contributor"/>
        public string[] Contributor { get; set; }

        [XmlAttribute(nameof(DateCreated))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_DateCreated { get => DateCreated.ToDateTimeXml(); set => DateCreated = value.FromXmlDateTime(); }
#pragma warning restore IDE1006 // Naming Styles
        /// <summary>
        /// Gets the Date Created
        /// </summary>
        /// <remarks>This property is stored in the document, not obtained from the file system.</remarks>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemDocument.DateCreated"/>
        [XmlIgnore]
        public System.DateTime? DateCreated { get; set; }

        /// <summary>
        /// Gets the Last Author
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemDocument.LastAuthor"/>
        public string LastAuthor { get; set; }

        /// <summary>
        /// Gets the Revision Number
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemDocument.RevisionNumber"/>
        public string RevisionNumber { get; set; }

        /// <summary>
        /// Access control information, from SummaryInfo propset
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemDocument.Security"/>
        public int? Security { get; set; }

        /// <summary>
        /// Gets the Division
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemDocument.Division"/>
        public string Division { get; set; }

        /// <summary>
        /// Gets the Document ID
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemDocument.DocumentID"/>
        public string DocumentID { get; set; }

        /// <summary>
        /// Gets the Manager
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemDocument.Manager"/>
        public string Manager { get; set; }

        /// <summary>
        /// Gets the Presentation Format
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemDocument.PresentationFormat"/>
        public string PresentationFormat { get; set; }

        /// <summary>
        /// Gets the Version
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemDocument.Version"/>
        public string Version { get; set; }

        public IEnumerable<File> GetFiles() => throw new NotImplementedException();

        internal static DocumentPropertySet Create(System.IO.FileInfo fileInfo)
        {
            throw new NotImplementedException();
        }
    }
}
