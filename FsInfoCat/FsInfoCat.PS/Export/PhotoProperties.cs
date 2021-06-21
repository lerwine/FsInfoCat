using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace FsInfoCat.PS.Export
{
    public class PhotoProperties : ExportSet.ExtendedPropertiesBase
    {
        /// <summary>
        /// Gets the Camera Manufacturer
        /// </summary>
        /// <remarks>PropertyTagEquipMake</remarks>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemPhoto.CameraManufacturer"/>
        public string CameraManufacturer { get; set; }

        /// <summary>
        /// Gets the Camera Model
        /// </summary>
        /// <remarks>PropertyTagEquipModel</remarks>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemPhoto.CameraModel"/>
        public string CameraModel { get; set; }

        [XmlAttribute(nameof(DateTaken))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_DateTaken { get => DateTaken.ToDateTimeXml(); set => DateTaken = value.FromXmlDateTime(); }
#pragma warning restore IDE1006 // Naming Styles
        /// <summary>
        /// Gets the Date Taken
        /// </summary>
        /// <remarks>PropertyTagExifDTOrig</remarks>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemPhoto.DateTaken"/>
        [XmlIgnore]
        public System.DateTime? DateTaken { get; set; }

        /// <summary>
        /// Return the event at which the photo was taken
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemPhoto.Event"/>
        public string[] Event { get; set; }

        /// <summary>
        /// Returns the EXIF version.
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemPhoto.EXIFVersion"/>
        public string EXIFVersion { get; set; }

        /// <summary>
        /// Gets the Orientation
        /// </summary>
        /// <remarks>This is the image orientation viewed in terms of rows and columns.</remarks>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemPhoto.Orientation"/>
        public ushort? Orientation { get; set; }

        /// <summary>
        /// The user-friendly form of System.Photo.Orientation
        /// </summary>
        /// <remarks>Not intended to be parsed programmatically.</remarks>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemPhoto.OrientationText"/>
        public string OrientationText { get; set; }

        /// <summary>
        /// The people tags on an image.
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemPhoto.PeopleNames"/>
        public string[] PeopleNames { get; set; }

        public IEnumerable<File> GetFiles() => throw new NotImplementedException();

        internal static PhotoProperties Create(System.IO.FileInfo fileInfo)
        {
            throw new NotImplementedException();
        }
    }
}
