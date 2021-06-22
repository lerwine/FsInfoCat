namespace FsInfoCat
{
    public interface IPhotoPropertySet : IPropertySet
    {
        /// <summary>
        /// Gets the Camera Manufacturer
        /// </summary>
        /// <remarks>PropertyTagEquipMake
        /// <para>ID: {14B81DA1-0135-4D31-96D9-6CBFC9671A99}, 271 (ImageProperties)</para></remarks>
        string CameraManufacturer { get; set; }

        /// <summary>
        /// Gets the Camera Model
        /// </summary>
        /// <remarks>PropertyTagEquipModel
        /// <para>ID: {14B81DA1-0135-4D31-96D9-6CBFC9671A99}, 272 (ImageProperties)</para></remarks>
        string CameraModel { get; set; }

        /// <summary>
        /// Gets the Date Taken
        /// </summary>
        /// <remarks>PropertyTagExifDTOrig
        /// <para>ID: {14B81DA1-0135-4D31-96D9-6CBFC9671A99}, 36867 (ImageProperties)</para></remarks>
        System.DateTime? DateTaken { get; set; }

        /// <summary>
        /// Return the event at which the photo was taken
        /// </summary>
        /// <remarks>ID: {14B81DA1-0135-4D31-96D9-6CBFC9671A99}, 18248 (ImageProperties)</remarks>
        string[] Event { get; set; }

        /// <summary>
        /// Returns the EXIF version.
        /// </summary>
        /// <remarks>ID: {D35F743A-EB2E-47F2-A286-844132CB1427}, 100</remarks>
        string EXIFVersion { get; set; }

        /// <summary>
        /// Gets the Orientation
        /// </summary>
        /// <remarks>This is the image orientation viewed in terms of rows and columns.
        /// <para>ID: {14B81DA1-0135-4D31-96D9-6CBFC9671A99}, 274 (ImageProperties)</para></remarks>
        ushort? Orientation { get; set; }

        /// <summary>
        /// The user-friendly form of System.Photo.Orientation
        /// </summary>
        /// <remarks>Not intended to be parsed programmatically.
        /// <para>ID: {A9EA193C-C511-498A-A06B-58E2776DCC28}, 100</para></remarks>
        string OrientationText { get; set; }

        /// <summary>
        /// The people tags on an image.
        /// </summary>
        /// <remarks>ID: {E8309B6E-084C-49B4-B1FC-90A80331B638}, 100</remarks>
        string[] PeopleNames { get; set; }
    }
}
