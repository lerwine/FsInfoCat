namespace FsInfoCat
{
    public interface ISummaryProperties
    {
        /// <summary>
        /// Gets the Application Name
        /// </summary>
        /// <remarks>ID: {F29F85E0-4FF9-1068-AB91-08002B27B3D9}, 18 (SummaryInformation)</remarks>
        string ApplicationName { get; }

        /// <summary>
        /// Gets the Author
        /// </summary>
        /// <remarks>ID: {F29F85E0-4FF9-1068-AB91-08002B27B3D9}, 4 (SummaryInformation)</remarks>
        MultiStringValue Author { get; }

        /// <summary>
        /// Gets the comments
        /// </summary>
        /// <remarks>ID: {F29F85E0-4FF9-1068-AB91-08002B27B3D9}, 6 (SummaryInformation)</remarks>
        string Comment { get; }

        /// <summary>
        /// Gets the keywords for the item
        /// </summary>
        /// <remarks>Also referred to as tags.
        /// <para>ID: {F29F85E0-4FF9-1068-AB91-08002B27B3D9}, 5 (SummaryInformation)</para></remarks>
        MultiStringValue Keywords { get; }

        /// <summary>
        /// Gets the Subject
        /// </summary>
        /// <remarks>ID: {F29F85E0-4FF9-1068-AB91-08002B27B3D9}, 3 (SummaryInformation)</remarks>
        string Subject { get; }

        /// <summary>
        /// Gets the Title of the item.
        /// </summary>
        /// <remarks>ID: {F29F85E0-4FF9-1068-AB91-08002B27B3D9}, 2 (SummaryInformation)</remarks>
        string Title { get; }

        /// <summary>
        /// Gets the company or publisher.
        /// </summary>
        /// <remarks>ID: {D5CDD502-2E9C-101B-9397-08002B2CF9AE}, 15 (DocumentSummaryInformation)</remarks>
        string Company { get; }

        /// <summary>
        /// Gets the Content Type
        /// </summary>
        /// <remarks>ID: {D5CDD502-2E9C-101B-9397-08002B2CF9AE}, 26 (DocumentSummaryInformation)</remarks>
        string ContentType { get; }

        /// <summary>
        /// Gets the Copyright
        /// </summary>
        /// <remarks>ID: {64440492-4C8B-11D1-8B70-080036B11A03}, 11 (MEDIAFILESUMMARYINFORMATION)</remarks>
        string Copyright { get; }

        /// <summary>
        /// Gets the Parental Rating
        /// </summary>
        /// <remarks>ID: {64440492-4C8B-11D1-8B70-080036B11A03}, 21 (MEDIAFILESUMMARYINFORMATION)</remarks>
        string ParentalRating { get; }

        /// <summary>
        /// Indicates the users preference rating of an item on a scale of 1-99
        /// </summary>
        /// <remarks>1-12 = One Star, 13-37 = Two Stars, 38-62 = Three Stars, 63-87 = Four Stars, 88-99 = Five Stars.
        /// <para>ID: {64440492-4C8B-11D1-8B70-080036B11A03}, 9 (MEDIAFILESUMMARYINFORMATION)</para></remarks>
        uint? Rating { get; }

        /// <summary>
        /// This is the generic list of authors associated with an item.
        /// </summary>
        /// <remarks>For example, the artist name for a track is the item author.
        /// <para>ID: {D0A04F0A-462A-48A4-BB2F-3706E88DBD7D}, 100</para></remarks>
        MultiStringValue ItemAuthors { get; }

        /// <summary>
        /// This is the canonical type of the item and is intended to be programmatically parsed.
        /// </summary>
        /// <remarks>If there is no canonical type, the value is VT_EMPTY. If the item is a file (ie, System.FileName is not VT_EMPTY), the value is the same as System.FileExtension. Use System.ItemTypeText when you want to display the type to end users in a view. (If the item is a file, passing the System.ItemType value to PSFormatForDisplay will result in the same value as System.ItemTypeText.)
        /// <para>ID: {28636AA6-953D-11D2-B5D6-00C04FD918D0}, 11 (ShellDetails)</para></remarks>
        string ItemType { get; }

        /// <summary>
        /// This is the user friendly type name of the item.
        /// </summary>
        /// <remarks>This is not intended to be programmatically parsed. If System.ItemType is VT_EMPTY, the value of this property is also VT_EMPTY. If the item is a file, the value of this property is the same as if you passed the file's System.ItemType value to PSFormatForDisplay. This property should not be confused with System.Kind, where System.Kind is a high-level user friendly kind name.
        /// <para>ID: {B725F130-47EF-101A-A5F1-02608C9EEBAC}, 4 (Storage)</para></remarks>
        string ItemTypeText { get; }

        /// <summary>
        /// System.Kind is used to map extensions to various Search folders.
        /// </summary>
        /// <remarks>Extensions are mapped to Kinds at HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\Explorer\KindMap The list of kinds is not extensible.
        /// <para>ID: {1E3EE840-BC2B-476C-8237-2ACD1A839B22}, 3</para></remarks>
        MultiStringValue Kind { get; }

        /// <summary>
        /// The MIME type.
        /// </summary>
        /// <remarks>Eg, for EML files: 'message/rfc822'.
        /// <para>ID: {0B63E350-9CCC-11D0-BCDB-00805FCCCE04}, 5</para></remarks>
        string MIMEType { get; }

        /// <summary>
        /// Gets the Parental Rating Reason
        /// </summary>
        /// <remarks>ID: {10984E0A-F9F2-4321-B7EF-BAF195AF4319}, 100</remarks>
        string ParentalRatingReason { get; }

        /// <summary>
        /// Gets the Parental Ratings Organization
        /// </summary>
        /// <remarks>ID: {A7FE0840-1344-46F0-8D37-52ED712A4BF9}, 100</remarks>
        string ParentalRatingsOrganization { get; }

        /// <summary>
        /// Gets the Sensitivity
        /// </summary>
        /// <remarks>ID: {F8D3F6AC-4874-42CB-BE59-AB454B30716A}, 100</remarks>
        ushort? Sensitivity { get; }

        /// <summary>
        /// This is the user-friendly form of System.Sensitivity.
        /// </summary>
        /// <remarks>Not intended to be parsed programmatically.
        /// <para>ID: {D0C7F054-3F72-4725-8527-129A577CB269}, 100</para></remarks>
        string SensitivityText { get; }

        /// <summary>
        /// Indicates the users preference rating of an item on a scale of 0-5
        /// </summary>
        /// <remarks>0=unrated, 1=One Star, 2=Two Stars, 3=Three Stars, 4=Four Stars, 5=Five Stars
        /// <para>ID: {A09F084E-AD41-489F-8076-AA5BE3082BCA}, 100</para></remarks>
        uint? SimpleRating { get; }

        /// <summary>
        /// Gets the Trademarks
        /// </summary>
        /// <remarks>ID: {0CEF7D53-FA64-11D1-A203-0000F81FEDEE}, 9 (VERSION)</remarks>
        string Trademarks { get; }

        /// <summary>
        /// Gets the Product Name
        /// </summary>
        /// <remarks>ID: {0CEF7D53-FA64-11D1-A203-0000F81FEDEE}, 7 (VERSION)</remarks>
        string ProductName { get; }
    }
}
