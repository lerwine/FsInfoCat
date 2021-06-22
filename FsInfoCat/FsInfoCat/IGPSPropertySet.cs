namespace FsInfoCat
{
    public interface IGPSPropertySet : IPropertySet
    {
        /// <summary>
        /// Represents the name of the GPS area
        /// </summary>
        /// <remarks>ID: {972E333E-AC7E-49F1-8ADF-A70D07A9BCAB}, 100</remarks>
        string AreaInformation { get; set; }

        /// <summary>
        /// Indicates the latitude degrees.
        /// </summary>
        /// <remarks>This is the value at index 0 from an array of three values.
        /// <para>ID: {8727CFFF-4868-4EC6-AD5B-81B98521D1AB}, 100</para></remarks>
        double? LatitudeDegrees { get; set; }

        /// <summary>
        /// Indicates the latitude minutes.
        /// </summary>
        /// <remarks>This is the value at index 1 from an array of three values.
        /// <para>ID: {8727CFFF-4868-4EC6-AD5B-81B98521D1AB}, 100</para></remarks>
        double? LatitudeMinutes { get; set; }

        /// <summary>
        /// Indicates the latitude seconds.
        /// </summary>
        /// <remarks>This is the value at index 2 from an array of three values.
        /// <para>ID: {8727CFFF-4868-4EC6-AD5B-81B98521D1AB}, 100</para></remarks>
        double? LatitudeSeconds { get; set; }

        /// <summary>
        /// Indicates whether latitude is north or south latitude
        /// </summary>
        /// <remarks>ID: {029C0252-5B86-46C7-ACA0-2769FFC8E3D4}, 100</remarks>
        string LatitudeRef { get; set; }

        /// <summary>
        /// Indicates the longitude degrees.
        /// </summary>
        /// <remarks>This is the value at index 0 from an array of three values.
        /// <para>ID: {C4C4DBB2-B593-466B-BBDA-D03D27D5E43A}, 100</para></remarks>
        double? LongitudeDegrees { get; set; }

        /// <summary>
        /// Indicates the longitude minutes.
        /// </summary>
        /// <remarks>This is the value at index 1 from an array of three values.
        /// <para>ID: {C4C4DBB2-B593-466B-BBDA-D03D27D5E43A}, 100</para></remarks>
        double? LongitudeMinutes { get; set; }

        /// <summary>
        /// Indicates the longitude seconds.
        /// </summary>
        /// <remarks>This is the value at index 2 from an array of three values.
        /// <para>ID: {C4C4DBB2-B593-466B-BBDA-D03D27D5E43A}, 100</para></remarks>
        double? LongitudeSeconds { get; set; }

        /// <summary>
        /// Indicates whether longitude is east or west longitude
        /// </summary>
        /// <remarks>ID: {33DCF22B-28D5-464C-8035-1EE9EFD25278}, 100</remarks>
        string LongitudeRef { get; set; }

        /// <summary>
        /// Indicates the GPS measurement mode.
        /// </summary>
        /// <remarks>eg: 2-dimensional, 3-dimensional
        /// <para>ID: {A015ED5D-AAEA-4D58-8A86-3C586920EA0B}, 100</para></remarks>
        string MeasureMode { get; set; }

        /// <summary>
        /// Indicates the name of the method used for location finding
        /// </summary>
        /// <remarks>ID: {59D49E61-840F-4AA9-A939-E2099B7F6399}, 100</remarks>
        string ProcessingMethod { get; set; }

        /// <summary>
        /// Indicates the version of the GPS information
        /// </summary>
        /// <remarks>ID: {22704DA4-C6B2-4A99-8E56-F16DF8C92599}, 100</remarks>
        byte[] VersionID { get; set; }
    }
}
