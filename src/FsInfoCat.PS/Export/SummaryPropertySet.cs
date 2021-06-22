using System;
using System.Collections.Generic;

namespace FsInfoCat.PS.Export
{
    public class SummaryPropertySet : ExportSet.ExtendedPropertySetBase
    {
        /// <summary>
        /// Gets the Application Name
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystem.ApplicationName"/>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Gets the Author
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystem.Author"/>
        public string[] Author { get; set; }

        /// <summary>
        /// Gets the comments
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystem.Comment"/>
        public string Comment { get; set; }

        /// <summary>
        /// Gets the keywords for the item
        /// </summary>
        /// <remarks>Also referred to as tags.</remarks>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystem.Keywords"/>
        public string[] Keywords { get; set; }

        /// <summary>
        /// Gets the Subject
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystem.Subject"/>
        public string Subject { get; set; }

        /// <summary>
        /// Gets the Title of the item.
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystem.Title"/>
        public string Title { get; set; }

        /// <summary>
        /// Gets the Category
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystem.Category"/>
        public string[] Category { get; set; }

        /// <summary>
        /// Gets the company or publisher.
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystem.Company"/>
        public string Company { get; set; }

        /// <summary>
        /// Gets the Content Type
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystem.ContentType"/>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets the Copyright
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystem.Copyright"/>
        public string Copyright { get; set; }

        /// <summary>
        /// Gets the Parental Rating
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystem.ParentalRating"/>
        public string ParentalRating { get; set; }

        /// <summary>
        /// Indicates the users preference rating of an item on a scale of 1-99
        /// </summary>
        /// <remarks>1-12 = One Star, 13-37 = Two Stars, 38-62 = Three Stars, 63-87 = Four Stars, 88-99 = Five Stars.</remarks>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystem.Rating"/>
        public uint? Rating { get; set; }

        /// <summary>
        /// This is the generic list of authors associated with an item.
        /// </summary>
        /// <remarks>For example, the artist name for a track is the item author.</remarks>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystem.ItemAuthors"/>
        public string[] ItemAuthors { get; set; }

        /// <summary>
        /// This is the canonical type of the item and is intended to be programmatically parsed.
        /// </summary>
        /// <remarks>If there is no canonical type, the value is VT_EMPTY. If the item is a file (ie, System.FileName is not VT_EMPTY), the value is the same as
        /// System.FileExtension.Use System.ItemTypeText when you want to display the type to end users in a view. (If the item is a file, passing the System.ItemType
        /// value to PSFormatForDisplay will result in the same value as System.ItemTypeText.)</remarks>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystem.ItemType"/>
        public string ItemType { get; set; }

        /// <summary>
        /// This is the user friendly type name of the item.
        /// </summary>
        /// <remarks>This is not intended to be programmatically parsed. If System.ItemType is VT_EMPTY, the value of this property is also VT_EMPTY.
        /// If the item is a file, the value of this property is the same as if you passed the file's System.ItemType value to PSFormatForDisplay.This property should not be confused with System.Kind, where System.Kind is a high-level user friendly kind name.</remarks>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystem.ItemTypeText"/>
        public string ItemTypeText { get; set; }

        /// <summary>
        /// System.Kind is used to map extensions to various Search folders.
        /// </summary>
        /// <remarks>Extensions are mapped to Kinds at HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\Explorer\KindMap The list of kinds is not extensible.</remarks>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystem.Kind"/>
        public string Kind { get; set; }

        /// <summary>
        /// The MIME type.
        /// </summary>
        /// <remarks>Eg, for EML files: 'message/rfc822'.</remarks>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystem.MIMEType"/>
        public string MIMEType { get; set; }

        /// <summary>
        /// Gets the Parental Rating Reason
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystem.ParentalRatingReason"/>
        public string ParentalRatingReason { get; set; }

        /// <summary>
        /// Gets the Parental Ratings Organization
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystem.ParentalRatingsOrganization"/>
        public string ParentalRatingsOrganization { get; set; }

        /// <summary>
        /// Gets the Sensitivity
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystem.Sensitivity"/>
        public ushort? Sensitivity { get; set; }

        /// <summary>
        /// This is the user-friendly form of System.Sensitivity.
        /// </summary>
        /// <remarks>Not intended to be parsed programmatically.</remarks>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystem.SensitivityText"/>
        public string SensitivityText { get; set; }

        /// <summary>
        /// Indicates the users preference rating of an item on a scale of 0-5
        /// </summary>
        /// <remarks>0=unrated, 1=One Star, 2=Two Stars, 3=Three Stars, 4=Four Stars, 5=Five Stars</remarks>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystem.SimpleRating"/>
        public uint? SimpleRating { get; set; }

        /// <summary>
        /// Gets the Trademarks
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystem.Trademarks"/>
        public string Trademarks { get; set; }

        /// <summary>
        /// Gets the Product Name
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemSoftware.ProductName"/>
        public string ProductName { get; set; }

        public IEnumerable<File> GetFiles() => throw new NotImplementedException();

        internal static SummaryPropertySet Create(System.IO.FileInfo fileInfo)
        {
            throw new NotImplementedException();
        }
    }
}
