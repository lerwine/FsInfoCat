using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    public static class DbConstants
    {
        public const int DbColMaxLen_SimpleName = 256;
        public const int DbColMaxLen_LongName = 1024;
        public const int DbColMaxLen_ShortName = 128;
        public const int DbColMaxLen_Identifier = 1024;
        public const int DbColMaxLen_FileName = 1024;
        public const uint DbColDefaultValue_MaxNameLength = 255;
        public const ushort DbColDefaultValue_MaxRecursionDepth = 256;
        public const ulong DbColDefaultValue_MaxTotalItems = ulong.MaxValue;

        #region Display Names

        // TODO: Make display name constants obsolete

        /// <summary>
        /// The display name for <see cref="IAudioProperties.EncodingBitrate"/>
        /// </summary>
        [System.Obsolete("Use [Display(Name = nameof(Properties.Resources.DisplayName_*), ResourceType = typeof(Properties.Resources))]")]
        public const string DisplayName_EncodingBitrate = "Encoding Bitrate";

        /// <summary>
        /// The display name for <see cref="IDocumentProperties.ClientID"/>
        /// </summary>
        [System.Obsolete("Use [Display(Name = nameof(Properties.Resources.DisplayName_*), ResourceType = typeof(Properties.Resources))]")]
        public const string DisplayName_ClientID = "Client ID";

        /// <summary>
        /// The display name for <see cref="IDocumentProperties.Contributor"/>
        /// </summary>
        [System.Obsolete("Use [Display(Name = nameof(Properties.Resources.DisplayName_*), ResourceType = typeof(Properties.Resources))]")]
        public const string DisplayName_Contributor = "Contributor";

        /// <summary>
        /// The display name for <see cref="IDocumentProperties.DateCreated"/>
        /// </summary>
        [System.Obsolete("Use [Display(Name = nameof(Properties.Resources.DisplayName_*), ResourceType = typeof(Properties.Resources))]")]
        public const string DisplayName_DateCreated = "Date Created";

        /// <summary>
        /// The display name for <see cref="IDocumentProperties.LastAuthor"/>
        /// </summary>
        [System.Obsolete("Use [Display(Name = nameof(Properties.Resources.DisplayName_*), ResourceType = typeof(Properties.Resources))]")]
        public const string DisplayName_LastAuthor = "Last Author";

        /// <summary>
        /// The display name for <see cref="IDocumentProperties.RevisionNumber"/>
        /// </summary>
        [System.Obsolete("Use [Display(Name = nameof(Properties.Resources.DisplayName_*), ResourceType = typeof(Properties.Resources))]")]
        public const string DisplayName_RevisionNumber = "Revision Number";

        /// <summary>
        /// The display name for <see cref="IDocumentProperties.Security"/>
        /// </summary>
        [System.Obsolete("Use [Display(Name = nameof(Properties.Resources.DisplayName_*), ResourceType = typeof(Properties.Resources))]")]
        public const string DisplayName_Security = "Security";

        /// <summary>
        /// The display name for <see cref="IDocumentProperties.Division"/>
        /// </summary>
        [System.Obsolete("Use [Display(Name = nameof(Properties.Resources.DisplayName_*), ResourceType = typeof(Properties.Resources))]")]
        public const string DisplayName_Division = "Division";

        /// <summary>
        /// The display name for <see cref="IDocumentProperties.DocumentID"/>
        /// </summary>
        [System.Obsolete("Use [Display(Name = nameof(Properties.Resources.DisplayName_*), ResourceType = typeof(Properties.Resources))]")]
        public const string DisplayName_DocumentID = "Document ID";

        /// <summary>
        /// The display name for <see cref="IDocumentProperties.Manager"/>
        /// </summary>
        [System.Obsolete("Use [Display(Name = nameof(Properties.Resources.DisplayName_*), ResourceType = typeof(Properties.Resources))]")]
        public const string DisplayName_Manager = "Manager";

        /// <summary>
        /// The display name for <see cref="IDocumentProperties.PresentationFormat"/>
        /// </summary>
        [System.Obsolete("Use [Display(Name = nameof(Properties.Resources.DisplayName_*), ResourceType = typeof(Properties.Resources))]")]
        public const string DisplayName_PresentationFormat = "Presentation Format";

        /// <summary>
        /// The display name for <see cref="IDocumentProperties.Version"/>
        /// </summary>
        [System.Obsolete("Use [Display(Name = nameof(Properties.Resources.DisplayName_*), ResourceType = typeof(Properties.Resources))]")]
        public const string DisplayName_Version = "Version Number";

        /// <summary>
        /// The display name for <see cref="IVideoProperties.Compression"/>
        /// </summary>
        [System.Obsolete("Use [Display(Name = nameof(Properties.Resources.DisplayName_*), ResourceType = typeof(Properties.Resources))]")]
        public const string DisplayName_Compression = "Compression";

        /// <summary>
        /// The display name for <see cref="IVideoProperties.Director"/>
        /// </summary>
        [System.Obsolete("Use [Display(Name = nameof(Properties.Resources.DisplayName_*), ResourceType = typeof(Properties.Resources))]")]
        public const string DisplayName_Director = "Director";

        /// <summary>
        /// The display name for <see cref="IVideoProperties.EncodingBitrate"/>
        /// </summary>
        [System.Obsolete("Use [Display(Name = nameof(Properties.Resources.DisplayName_*), ResourceType = typeof(Properties.Resources))]")]
        public const string DisplayName_EncodingDataRate = "Encoding Data Rate";

        /// <summary>
        /// The display name for <see cref="IVideoProperties.FrameHeight"/>
        /// </summary>
        [System.Obsolete("Use [Display(Name = nameof(Properties.Resources.DisplayName_*), ResourceType = typeof(Properties.Resources))]")]
        public const string DisplayName_FrameHeight = "Frame Height";

        /// <summary>
        /// The display name for <see cref="IVideoProperties.FrameRate"/>
        /// </summary>
        [System.Obsolete("Use [Display(Name = nameof(Properties.Resources.DisplayName_*), ResourceType = typeof(Properties.Resources))]")]
        public const string DisplayName_FrameRate = "Frame Rate";

        /// <summary>
        /// The display name for <see cref="IVideoProperties.FrameWidth"/>
        /// </summary>
        [System.Obsolete("Use [Display(Name = nameof(Properties.Resources.DisplayName_*), ResourceType = typeof(Properties.Resources))]")]
        public const string DisplayName_FrameWidth = "Frame Width";

        /// <summary>
        /// The display name for <see cref="IVideoProperties.HorizontalAspectRatio"/>
        /// </summary>
        [System.Obsolete("Use [Display(Name = nameof(Properties.Resources.DisplayName_*), ResourceType = typeof(Properties.Resources))]")]
        public const string DisplayName_HorizontalAspectRatio = "Horizontal Aspect Ratio";

        /// <summary>
        /// The display name for <see cref="IAudioProperties.StreamName"/> and <see cref="IVideoProperties.StreamName"/>
        /// </summary>
        [System.Obsolete("Use [Display(Name = nameof(Properties.Resources.DisplayName_*), ResourceType = typeof(Properties.Resources))]")]
        public const string DisplayName_StreamName = "Stream Name";

        /// <summary>
        /// The display name for <see cref="IAudioProperties.StreamNumber"/> and <see cref="IVideoProperties.StreamNumber"/>
        /// </summary>
        [System.Obsolete("Use [Display(Name = nameof(Properties.Resources.DisplayName_*), ResourceType = typeof(Properties.Resources))]")]
        public const string DisplayName_StreamNumber = "Stream Number";

        /// <summary>
        /// The display name for <see cref="IVideoProperties.VerticalAspectRatio"/>
        /// </summary>
        [System.Obsolete("Use [Display(Name = nameof(Properties.Resources.DisplayName_*), ResourceType = typeof(Properties.Resources))]")]
        public const string DisplayName_VerticalAspectRatio = "Vertical Aspect Ratio";

        #endregion
    }
}
