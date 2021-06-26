namespace FsInfoCat
{
    public interface IDocumentProperties
    {
        /// <summary>
        /// Gets the Client ID
        /// </summary>
        /// <value>
        /// System.
        /// </value>
        /// <remarks>
        /// Document.ClientID
        /// <list type="bullet">
        /// <item><term>Name</term><description>Client ID</description></item>
        /// <item><term>Format ID</term><description>{276D7BB0-5B34-4FB0-AA4B-158ED12A1809} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-document-clientid">[Reference Link]</a></description></item>
        /// </list></remarks>
        string ClientID { get; }

        /// <summary>
        /// Gets the Contributor
        /// </summary>
        /// <value>
        /// The document contributor.
        /// </value>
        /// <remarks>
        /// <list type="bullet">
        /// <item><term>Name</term><description>Contributor</description></item>
        /// <item><term>Format ID</term><description>{F334115E-DA1B-4509-9B3D-119504DC7ABB} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-document-contributor">[Reference Link]</a></description></item>
        /// </list></remarks>
        MultiStringValue Contributor { get; }

        /// <summary>
        /// Gets the Date Created
        /// </summary>
        /// <value>
        /// The date and time that a document was created.
        /// </value>
        /// <remarks>
        /// This property is stored in the document, not obtained from the file system.
        /// <list type="bullet">
        /// <item><term>Name</term><description>Date Created</description></item>
        /// <item><term>Format ID</term><description>{F29F85E0-4FF9-1068-AB91-08002B27B3D9} (SummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>12</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-document-datecreated">[Reference Link]</a></description></item>
        /// </list></remarks>
        System.DateTime? DateCreated { get; }

        /// <summary>
        /// Gets the Last Author
        /// </summary>
        /// <value>
        /// The last person to save the document, as stored in the document.
        /// </value>
        /// <remarks>
        /// <list type="bullet">
        /// <item><term>Name</term><description>Last Author</description></item>
        /// <item><term>Format ID</term><description>{F29F85E0-4FF9-1068-AB91-08002B27B3D9} (SummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>8</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-document-lastauthor">[Reference Link]</a></description></item>
        /// </list></remarks>
        string LastAuthor { get; }

        /// <summary>
        /// Gets the Revision Number
        /// </summary>
        /// <value>
        /// The revision number.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Revision Number</description></item>
        /// <item><term>Format ID</term><description>{F29F85E0-4FF9-1068-AB91-08002B27B3D9} (SummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>9</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-document-revisionnumber">[Reference Link]</a></description></item>
        /// </list></remarks>
        string RevisionNumber { get; }

        /// <summary>
        /// Access control information, from SummaryInfo propset
        /// </summary>
        /// <value>
        /// Access control information, from SummaryInfo propset.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Security</description></item>
        /// <item><term>Format ID</term><description>{F29F85E0-4FF9-1068-AB91-08002B27B3D9} (SummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>19</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-document-security">[Reference Link]</a></description></item>
        /// </list></remarks>
        int? Security { get; }

        /// <summary>
        /// Gets the Division
        /// </summary>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Division</description></item>
        /// <item><term>Format ID</term><description>{1E005EE6-BF27-428B-B01C-79676ACD2870} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-document-division">[Reference Link]</a></description></item>
        /// </list></remarks>
        string Division { get; }

        /// <summary>
        /// Gets the Document ID
        /// </summary>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Document ID</description></item>
        /// <item><term>Format ID</term><description>{E08805C8-E395-40DF-80D2-54F0D6C43154} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-document-documentid">[Reference Link]</a></description></item>
        /// </list></remarks>
        string DocumentID { get; }

        /// <summary>
        /// Gets the Manager
        /// </summary>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Manager</description></item>
        /// <item><term>Format ID</term><description>{D5CDD502-2E9C-101B-9397-08002B2CF9AE} (DocumentSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>14</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-document-manager">[Reference Link]</a></description></item>
        /// </list></remarks>
        string Manager { get; }

        /// <summary>
        /// Gets the Presentation Format
        /// </summary>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Presentation Format</description></item>
        /// <item><term>Format ID</term><description>{D5CDD502-2E9C-101B-9397-08002B2CF9AE} (DocumentSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>3</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-document-presentationformat">[Reference Link]</a></description></item>
        /// </list></remarks>
        string PresentationFormat { get; }

        /// <summary>
        /// Gets the Version
        /// </summary>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Version Number</description></item>
        /// <item><term>Format ID</term><description>{D5CDD502-2E9C-101B-9397-08002B2CF9AE} (DocumentSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>29</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-document-version">[Reference Link]</a></description></item>
        /// </list></remarks>
        string Version { get; }
    }
}
