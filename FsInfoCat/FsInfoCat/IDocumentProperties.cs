namespace FsInfoCat
{
    public interface IDocumentProperties : IPropertySet
    {
        /// <summary>
        /// Gets the Client ID
        /// </summary>
        /// <remarks>ID: {276D7BB0-5B34-4FB0-AA4B-158ED12A1809}, 100</remarks>
        string ClientID { get; set; }

        /// <summary>
        /// Gets the Contributor
        /// </summary>
        /// <remarks>ID: {F334115E-DA1B-4509-9B3D-119504DC7ABB}, 100</remarks>
        string[] Contributor { get; set; }

        /// <summary>
        /// Gets the Date Created
        /// </summary>
        /// <remarks>This property is stored in the document, not obtained from the file system.
        /// <para>ID: {F29F85E0-4FF9-1068-AB91-08002B27B3D9}, 12 (SummaryInformation)</para></remarks>
        System.DateTime? DateCreated { get; set; }

        /// <summary>
        /// Gets the Last Author
        /// </summary>
        /// <remarks>ID: {F29F85E0-4FF9-1068-AB91-08002B27B3D9}, 8 (SummaryInformation)</remarks>
        string LastAuthor { get; set; }

        /// <summary>
        /// Gets the Revision Number
        /// </summary>
        /// <remarks>ID: {F29F85E0-4FF9-1068-AB91-08002B27B3D9}, 9 (SummaryInformation)</remarks>
        string RevisionNumber { get; set; }

        /// <summary>
        /// Access control information, from SummaryInfo propset
        /// </summary>
        /// <remarks>ID: {F29F85E0-4FF9-1068-AB91-08002B27B3D9}, 19 (SummaryInformation)</remarks>
        int? Security { get; set; }

        /// <summary>
        /// Gets the Division
        /// </summary>
        /// <remarks>ID: {1E005EE6-BF27-428B-B01C-79676ACD2870}, 100</remarks>
        string Division { get; set; }

        /// <summary>
        /// Gets the Document ID
        /// </summary>
        /// <remarks>ID: {E08805C8-E395-40DF-80D2-54F0D6C43154}, 100</remarks>
        string DocumentID { get; set; }

        /// <summary>
        /// Gets the Manager
        /// </summary>
        /// <remarks>ID: {D5CDD502-2E9C-101B-9397-08002B2CF9AE}, 14 (DocumentSummaryInformation)</remarks>
        string Manager { get; set; }

        /// <summary>
        /// Gets the Presentation Format
        /// </summary>
        /// <remarks>ID: {D5CDD502-2E9C-101B-9397-08002B2CF9AE}, 3 (DocumentSummaryInformation)</remarks>
        string PresentationFormat { get; set; }

        /// <summary>
        /// Gets the Version
        /// </summary>
        /// <remarks>ID: {D5CDD502-2E9C-101B-9397-08002B2CF9AE}, 29 (DocumentSummaryInformation)</remarks>
        string Version { get; set; }
    }
}
