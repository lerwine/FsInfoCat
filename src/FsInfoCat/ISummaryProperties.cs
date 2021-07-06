using FsInfoCat.Collections;

namespace FsInfoCat
{
    /// <summary>
    /// Represents extended file summary properties.
    /// </summary>
    /// <seealso cref="ISummaryPropertySet"/>
    /// <seealso cref="Local.ILocalSummaryPropertySet"/>
    /// <seealso cref="Upstream.IUpstreamSummaryPropertySet"/>
    /// <seealso cref="FilePropertiesComparer.Equals(ISummaryProperties, ISummaryProperties)"/>
    /// <seealso cref="Local.IFileDetailProvider.GetSummaryPropertiesAsync(System.Threading.CancellationToken)"/>
    /// <seealso cref="IDbContext.FindMatchingAsync(ISummaryProperties, System.Threading.CancellationToken)"/>
    public interface ISummaryProperties
    {
        /// <summary>
        /// Gets the Application Name
        /// </summary>
        /// <value>
        /// The name of the application that created this file or item.
        /// </value>
        /// <remarks>This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null"/>.
        /// <para>Do not use version numbers to identify the application's specific version.</para>
        /// <list type="bullet">
        /// <item><term>Name</term><description>Application Name</description></item>
        /// <item><term>Format ID</term><description>{F29F85E0-4FF9-1068-AB91-08002B27B3D9} (SummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>18</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-applicationname">[Reference Link]</a></description></item>
        /// </list></remarks>
        string ApplicationName { get; }

        /// <summary>
        /// Gets the Author
        /// </summary>
        /// <value>
        /// The author or authors of the document.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Author</description></item>
        /// <item><term>Format ID</term><description>{F29F85E0-4FF9-1068-AB91-08002B27B3D9} (SummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>4</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-author">[Reference Link]</a></description></item>
        /// </list></remarks>
        MultiStringValue Author { get; }

        /// <summary>
        /// Gets the comments
        /// </summary>
        /// <value>
        /// The comment attached to a file, typically added by a user.
        /// </value>
        /// <remarks>This value should be trimmed, with white-space-only converted to <see langword="null"/>.
        /// <list type="bullet">
        /// <item><term>Name</term><description>Comments</description></item>
        /// <item><term>Format ID</term><description>{F29F85E0-4FF9-1068-AB91-08002B27B3D9} (SummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>6</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-comment">[Reference Link]</a></description></item>
        /// </list></remarks>
        string Comment { get; }

        /// <summary>
        /// Gets the keywords for the item
        /// </summary>
        /// <value>
        /// The set of keywords (also known as "tags") assigned to the item.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Tags</description></item>
        /// <item><term>Format ID</term><description>{F29F85E0-4FF9-1068-AB91-08002B27B3D9} (SummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>5</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-keywords">[Reference Link]</a></description></item>
        /// </list></remarks>
        MultiStringValue Keywords { get; }

        /// <summary>
        /// Gets the Subject
        /// </summary>
        /// <value>
        /// The subject of a document.
        /// </value>
        /// <remarks>This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null"/>.
        /// <para>This property maps to the OLE document property Subject.</para>
        /// <list type="bullet">
        /// <item><term>Name</term><description>Subject</description></item>
        /// <item><term>Format ID</term><description>{F29F85E0-4FF9-1068-AB91-08002B27B3D9} (SummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>3</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-subject">[Reference Link]</a></description></item>
        /// </list></remarks>
        string Subject { get; }

        /// <summary>
        /// Gets the Title of the item.
        /// </summary>
        /// <value>
        /// The title of the item.
        /// </value>
        /// <remarks>This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null"/>.
        /// <list type="bullet">
        /// <item><term>Name</term><description>Title</description></item>
        /// <item><term>Format ID</term><description>{F29F85E0-4FF9-1068-AB91-08002B27B3D9} (SummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>2</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-title">[Reference Link]</a></description></item>
        /// </list></remarks>
        string Title { get; }

        /// <summary>
        /// Gets the company or publisher.
        /// </summary>
        /// <value>
        /// The company or publisher.
        /// </value>
        /// <remarks>This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null"/>.
        /// <list type="bullet">
        /// <item><term>Name</term><description>Company Name</description></item>
        /// <item><term>Format ID</term><description>{D5CDD502-2E9C-101B-9397-08002B2CF9AE} (DocumentSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>15</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-company">[Reference Link]</a></description></item>
        /// </list></remarks>
        string Company { get; }

        /// <summary>
        /// Gets the Content Type
        /// </summary>
        /// <value>
        /// The content type
        /// </value>
        /// <remarks>This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null"/>.
        /// <list type="bullet">
        /// <item><term>Name</term><description>Content Type</description></item>
        /// <item><term>Format ID</term><description>{D5CDD502-2E9C-101B-9397-08002B2CF9AE} (DocumentSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>26</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-contenttype">[Reference Link]</a></description></item>
        /// </list></remarks>
        string ContentType { get; }

        /// <summary>
        /// Gets the Copyright
        /// </summary>
        /// <value>
        /// The copyright information stored as a string.
        /// </value>
        /// <remarks>This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null"/>.
        /// <list type="bullet">
        /// <item><term>Name</term><description>Copyright</description></item>
        /// <item><term>Format ID</term><description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>11</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-copyright">[Reference Link]</a></description></item>
        /// </list></remarks>
        string Copyright { get; }

        /// <summary>
        /// Gets the Parental Rating
        /// </summary>
        /// <value>
        /// The parental rating stored in a format typically determined by the organization named in System.
        /// </value>
        /// <remarks>This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null"/>.
        /// <list type="bullet">
        /// <item><term>Name</term><description>Parental Rating</description></item>
        /// <item><term>Format ID</term><description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>21</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-parentalrating">[Reference Link]</a></description></item>
        /// </list></remarks>
        string ParentalRating { get; }

        /// <summary>
        /// Indicates the users preference rating of an item on a scale of 1-99
        /// </summary>
        /// <value>
        /// 1-12 = One Star, 13-37 = Two Stars, 38-62 = Three Stars, 63-87 = Four Stars, 88-99 = Five Stars.
        /// </value>
        /// <remarks>
        /// This is the rating system used by the Windows Vista Shell.
        /// <list type="bullet">
        /// <item><term>Name</term><description>Rating</description></item>
        /// <item><term>Format ID</term><description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>9</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-rating">[Reference Link]</a></description></item>
        /// </list></remarks>
        uint? Rating { get; }

        /// <summary>
        /// This is the generic list of authors associated with an item.
        /// </summary>
        /// <value>
        /// Generic list of authors associated with an item.
        /// </value>
        /// <remarks>
        /// For example, the artist name for a music track is the item author.
        /// <list type="bullet">
        /// <item><term>Name</term><description>Creators</description></item>
        /// <item><term>Format ID</term><description>{D0A04F0A-462A-48A4-BB2F-3706E88DBD7D} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-itemauthors">[Reference Link]</a></description></item>
        /// </list></remarks>
        MultiStringValue ItemAuthors { get; }

        /// <summary>
        /// Gets the canonical item type.
        /// </summary>
        /// <value>
        /// The canonical type of the item, intended to be programmatically parsed.
        /// </value>
        /// <remarks>This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null"/>.
        /// <para>If there is no canonical type, the value is VT_EMPTY. If the item is a file (ie, System.FileName is not VT_EMPTY), the value is the same as
        /// System.FileExtension.Use System.ItemTypeText when you want to display the type to end users in a view. (If the item is a file, passing the System.ItemType value
        /// to PSFormatForDisplay will result in the same value as System.ItemTypeText.)</para>
        /// <list type="bullet">
        /// <item><term>Name</term><description>Item Type</description></item>
        /// <item><term>Format ID</term><description>{28636AA6-953D-11D2-B5D6-00C04FD918D0} (ShellDetails)</description></item>
        /// <item><term>Property ID</term><description>11</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-itemtype">[Reference Link]</a></description></item>
        /// </list></remarks>
        string ItemType { get; }

        /// <summary>
        /// Gets the item type name.
        /// </summary>
        /// <value>
        /// This is the user friendly type name of the item.
        /// </value>
        /// <remarks>This value should be trimmed, with white-space-only converted to <see langword="null"/>.
        /// <para>This is not intended to be programmatically parsed. If System.ItemType is VT_EMPTY, the value of this property is also VT_EMPTY.
        /// If the item is a file, the value of this property is the same as if you passed the file's System.ItemType value to PSFormatForDisplay.
        /// This property should not be confused with System.Kind, where System.Kind is a high-level user friendly kind name.</para>
        /// <list type="bullet">
        /// <item><term>Name</term><description>Item Type</description></item>
        /// <item><term>Format ID</term><description>{B725F130-47EF-101A-A5F1-02608C9EEBAC} (Storage)</description></item>
        /// <item><term>Property ID</term><description>4</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-itemtypetext">[Reference Link]</a></description></item>
        /// </list></remarks>
        string ItemTypeText { get; }

        /// <summary>
        /// Search folder extension mappings
        /// </summary>
        /// <value>
        /// System.Kind values that are used to map extensions to various Search folders.
        /// </value>
        /// <remarks>
        /// Extensions are mapped to Kinds at HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\Explorer\KindMap The list of kinds is not extensible.
        /// <list type="bullet">
        /// <item><term>Name</term><description>File Kind</description></item>
        /// <item><term>Format ID</term><description>{1E3EE840-BC2B-476C-8237-2ACD1A839B22} (Format)</description></item>
        /// <item><term>Property ID</term><description>3</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-kind">[Reference Link]</a></description></item>
        /// </list></remarks>
        MultiStringValue Kind { get; }

        /// <summary>
        /// Gets the MIME type
        /// </summary>
        /// <value>
        /// The MIME type.
        /// </value>
        /// <remarks>This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null"/>.
        /// <para>Eg, for EML files: 'message/rfc822'.</para>
        /// <list type="bullet">
        /// <item><term>Name</term><description>MIME-Type</description></item>
        /// <item><term>Format ID</term><description>{0B63E350-9CCC-11D0-BCDB-00805FCCCE04} (Format)</description></item>
        /// <item><term>Property ID</term><description>5</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-mimetype">[Reference Link]</a></description></item>
        /// </list></remarks>
        string MIMEType { get; }

        /// <summary>
        /// Gets the Parental Rating Reason
        /// </summary>
        /// <value>
        /// Explains file ratings.
        /// </value>
        /// <remarks>This value should be trimmed, with white-space-only converted to <see langword="null"/>.
        /// <list type="bullet">
        /// <item><term>Name</term><description>Parental Rating Reason</description></item>
        /// <item><term>Format ID</term><description>{10984E0A-F9F2-4321-B7EF-BAF195AF4319} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-parentalratingreason">[Reference Link]</a></description></item>
        /// </list></remarks>
        string ParentalRatingReason { get; }

        /// <summary>
        /// Gets the Parental Ratings Organization
        /// </summary>
        /// <value>
        /// The name of the organization whose rating system is used for System.
        /// </value>
        /// <remarks>This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null"/>.
        /// <para>ParentalRating.</para>
        /// <list type="bullet">
        /// <item><term>Name</term><description>Parental Ratings Organization</description></item>
        /// <item><term>Format ID</term><description>{A7FE0840-1344-46F0-8D37-52ED712A4BF9} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-parentalratingsorganization">[Reference Link]</a></description></item>
        /// </list></remarks>
        string ParentalRatingsOrganization { get; }

        /// <summary>
        /// Gets the Sensitivity
        /// </summary>
        /// <value>
        /// The Sensitivity value.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Sensitivity</description></item>
        /// <item><term>Format ID</term><description>{F8D3F6AC-4874-42CB-BE59-AB454B30716A} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-sensitivity">[Reference Link]</a></description></item>
        /// </list></remarks>
        ushort? Sensitivity { get; }

        /// <summary>
        /// Gets the user-friendly Sensitivity value.
        /// </summary>
        /// <value>
        /// The user-friendly form of System.Sensitivity.
        /// </value>
        /// <remarks>This value should be trimmed, with white-space-only converted to <see langword="null"/>.
        /// <para>This value is not intended to be parsed programmatically.</para>
        /// <list type="bullet">
        /// <item><term>Name</term><description>Sensitivity</description></item>
        /// <item><term>Format ID</term><description>{D0C7F054-3F72-4725-8527-129A577CB269} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-sensitivitytext">[Reference Link]</a></description></item>
        /// </list></remarks>
        string SensitivityText { get; }

        /// <summary>
        /// Indicates the users preference rating of an item on a scale of 0-5
        /// </summary>
        /// <value>
        /// 0=unrated, 1=One Star, 2=Two Stars, 3=Three Stars, 4=Four Stars, 5=Five Stars
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Simple Rating</description></item>
        /// <item><term>Format ID</term><description>{A09F084E-AD41-489F-8076-AA5BE3082BCA} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-simplerating">[Reference Link]</a></description></item>
        /// </list></remarks>
        uint? SimpleRating { get; }

        /// <summary>
        /// Gets the Legal Trademarks
        /// </summary>
        /// <value>
        /// The trademark associated with the item, in a string format.
        /// </value>
        /// <remarks>This value should be trimmed, with white-space-only converted to <see langword="null"/>.
        /// <list type="bullet">
        /// <item><term>Name</term><description>Legal Trademarks</description></item>
        /// <item><term>Format ID</term><description>{0CEF7D53-FA64-11D1-A203-0000F81FEDEE} (VERSION)</description></item>
        /// <item><term>Property ID</term><description>9</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-trademarks">[Reference Link]</a></description></item>
        /// </list></remarks>
        string Trademarks { get; }

        /// <summary>
        /// Gets the Product Name
        /// </summary>
        /// <value>
        /// System.
        /// </value>
        /// <remarks>This value should be white-space normalized and trimmed, with white-space-only converted to <see langword="null"/>.
        /// <list type="bullet">
        /// <item><term>Name</term><description>Product Name</description></item>
        /// <item><term>Format ID</term><description>{0CEF7D53-FA64-11D1-A203-0000F81FEDEE} (VERSION)</description></item>
        /// <item><term>Property ID</term><description>7</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-software-productname">[Reference Link]</a></description></item>
        /// </list></remarks>
        string ProductName { get; }
    }
}
