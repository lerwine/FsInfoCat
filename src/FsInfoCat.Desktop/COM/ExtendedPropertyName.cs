using System.ComponentModel;

namespace FsInfoCat.Desktop.COM
{
    public enum ExtendedPropertyName
    {
        [AmbientValue("Item type")]
        ItemType,

        [AmbientValue("Perceived type")]
        PerceivedType,

        [AmbientValue("Kind")]
        Kind,

        [AmbientValue("Date taken")]
        DateTaken,

        [AmbientValue("Contributing artists")]
        ContributingArtists,

        [AmbientValue("Album")]
        Album,

        [AmbientValue("Year")]
        Year,

        [AmbientValue("Genre")]
        Genre,

        [AmbientValue("Conductors")]
        Conductors,

        [AmbientValue("Tags")]
        Tags,

        [AmbientValue("Rating")]
        Rating,

        [AmbientValue("Authors")]
        Authors,

        [AmbientValue("Title")]
        Title,

        [AmbientValue("Subject")]
        Subject,

        [AmbientValue("Categories")]
        Categories,

        [AmbientValue("Comments")]
        Comments,

        [AmbientValue("Copyright")]
        Copyright,

        [AmbientValue("#")]
        TrackNumber,

        [AmbientValue("Length")]
        Length,

        [AmbientValue("Bit rate")]
        BitRate,

        [AmbientValue("Protected")]
        Protected,

        [AmbientValue("Camera model")]
        CameraModel,

        [AmbientValue("Dimensions")]
        Dimensions,

        [AmbientValue("Camera maker")]
        CameraMaker,

        [AmbientValue("Company")]
        Company,

        [AmbientValue("File description")]
        FileDescription,

        [AmbientValue("Masters keywords")]
        MastersKeywords,

        [AmbientValue("Program name")]
        ProgramName,

        [AmbientValue("Duration")]
        Duration,

        [AmbientValue("Is online")]
        IsOnline,

        [AmbientValue("Is recurring")]
        IsRecurring,

        [AmbientValue("Location")]
        Location,

        [AmbientValue("Optional attendee addresses")]
        OptionalAttendeeAddresses,

        [AmbientValue("Optional attendees")]
        OptionalAttendees,

        [AmbientValue("Organizer address")]
        OrganizerAddress,

        [AmbientValue("Organizer name")]
        OrganizerName,

        [AmbientValue("Reminder time")]
        ReminderTime,

        [AmbientValue("Required attendee addresses")]
        RequiredAttendeeAddresses,

        [AmbientValue("Required attendees")]
        RequiredAttendees,

        [AmbientValue("Resources")]
        Resources,

        [AmbientValue("Meeting status")]
        MeetingStatus,

        [AmbientValue("Free/busy status")]
        FreeBusyStatus,

        //[AmbientValue("Total size")]
        //TotalSize,

        [AmbientValue("Account name")]
        AccountName,

        [AmbientValue("Task status")]
        TaskStatus,

        //[AmbientValue("Computer")]
        //Computer,

        [AmbientValue("Anniversary")]
        Anniversary,

        [AmbientValue("Assistant's name")]
        AssistantName,

        [AmbientValue("Assistant's phone")]
        AssistantPhone,

        [AmbientValue("Birthday")]
        Birthday,

        [AmbientValue("Business address")]
        BusinessAddress,

        [AmbientValue("Business city")]
        BusinessCity,

        [AmbientValue("Business country/region")]
        BusinessCountryRegion,

        [AmbientValue("Business P.O. box")]
        BusinessPOBox,

        [AmbientValue("Business postal code")]
        BusinessPostalCode,

        [AmbientValue("Business state or province")]
        BusinessStateOrProvince,

        [AmbientValue("Business street")]
        BusinessStreet,

        [AmbientValue("Business fax")]
        BusinessFax,

        [AmbientValue("Business home page")]
        BusinessHomePage,

        [AmbientValue("Business phone")]
        BusinessPhone,

        [AmbientValue("Callback number")]
        CallbackNumber,

        [AmbientValue("Car phone")]
        CarPhone,

        [AmbientValue("Children")]
        Children,

        [AmbientValue("Company main phone")]
        CompanyMainPhone,

        [AmbientValue("Department")]
        Department,

        [AmbientValue("E-mail address")]
        EMailAddress,

        [AmbientValue("E-mail2")]
        EMail2,

        [AmbientValue("E-mail3")]
        EMail3,

        [AmbientValue("E-mail list")]
        EMailList,

        [AmbientValue("E-mail display name")]
        EMailDisplayName,

        [AmbientValue("File as")]
        FileAs,

        [AmbientValue("First name")]
        FirstName,

        [AmbientValue("Full name")]
        FullName,

        [AmbientValue("Gender")]
        Gender,

        [AmbientValue("Given name")]
        GivenName,

        [AmbientValue("Hobbies")]
        Hobbies,

        [AmbientValue("Home address")]
        HomeAddress,

        [AmbientValue("Home city")]
        HomeCity,

        [AmbientValue("Home country/region")]
        HomeCountryRegion,

        [AmbientValue("Home P.O. box")]
        HomePOBox,

        [AmbientValue("Home postal code")]
        HomePostalCode,

        [AmbientValue("Home state or province")]
        HomeStateOrProvince,

        [AmbientValue("Home street")]
        HomeStreet,

        [AmbientValue("Home fax")]
        HomeFax,

        [AmbientValue("Home phone")]
        HomePhone,

        [AmbientValue("IM addresses")]
        IMAddresses,

        [AmbientValue("Initials")]
        Initials,

        [AmbientValue("Job title")]
        JobTitle,

        [AmbientValue("Label")]
        Label,

        [AmbientValue("Last name")]
        LastName,

        [AmbientValue("Mailing address")]
        MailingAddress,

        [AmbientValue("Middle name")]
        MiddleName,

        [AmbientValue("Cell phone")]
        CellPhone,

        [AmbientValue("Nickname")]
        Nickname,

        [AmbientValue("Office location")]
        OfficeLocation,

        [AmbientValue("Other address")]
        OtherAddress,

        [AmbientValue("Other city")]
        OtherCity,

        [AmbientValue("Other country/region")]
        OtherCountryRegion,

        [AmbientValue("Other P.O. box")]
        OtherPOBox,

        [AmbientValue("Other postal code")]
        OtherPostalCode,

        [AmbientValue("Other state or province")]
        OtherStateOrProvince,

        [AmbientValue("Other street")]
        OtherStreet,

        [AmbientValue("Pager")]
        Pager,

        [AmbientValue("Personal title")]
        PersonalTitle,

        [AmbientValue("City")]
        City,

        [AmbientValue("Country/region")]
        CountryRegion,

        [AmbientValue("P.O. box")]
        POBox,

        [AmbientValue("Postal code")]
        PostalCode,

        [AmbientValue("State or province")]
        StateOrProvince,

        [AmbientValue("Street")]
        Street,

        [AmbientValue("Primary e-mail")]
        PrimaryEMail,

        [AmbientValue("Primary phone")]
        PrimaryPhone,

        [AmbientValue("Profession")]
        Profession,

        [AmbientValue("Spouse/Partner")]
        SpousePartner,

        [AmbientValue("Suffix")]
        Suffix,

        [AmbientValue("TTY/TTD phone")]
        TTYTTDPhone,

        [AmbientValue("Telex")]
        Telex,

        [AmbientValue("Webpage")]
        Webpage,

        [AmbientValue("Content status")]
        ContentStatus,

        [AmbientValue("Content type")]
        ContentType,

        [AmbientValue("Date acquired")]
        DateAcquired,

        [AmbientValue("Date archived")]
        DateArchived,

        [AmbientValue("Date completed")]
        DateCompleted,

        [AmbientValue("Device category")]
        DeviceCategory,

        [AmbientValue("Connected")]
        Connected,

        [AmbientValue("Discovery method")]
        DiscoveryMethod,

        [AmbientValue("Friendly name")]
        FriendlyName,

        [AmbientValue("Local computer")]
        LocalComputer,

        [AmbientValue("Manufacturer")]
        Manufacturer,

        [AmbientValue("Model")]
        Model,

        [AmbientValue("Paired")]
        Paired,

        [AmbientValue("Classification")]
        Classification,

        [AmbientValue("Status")]
        Status,

        [AmbientValue("Client ID")]
        ClientID,

        [AmbientValue("Contributors")]
        Contributors,

        [AmbientValue("Content created")]
        ContentCreated,

        [AmbientValue("Last printed")]
        LastPrinted,

        [AmbientValue("Date last saved")]
        DateLastSaved,

        [AmbientValue("Division")]
        Division,

        [AmbientValue("Document ID")]
        DocumentID,

        [AmbientValue("Pages")]
        Pages,

        [AmbientValue("Slides")]
        Slides,

        [AmbientValue("Total editing time")]
        TotalEditingTime,

        [AmbientValue("Word count")]
        WordCount,

        [AmbientValue("Due date")]
        DueDate,

        [AmbientValue("End date")]
        EndDate,

        [AmbientValue("File count")]
        FileCount,

        [AmbientValue("File extension")]
        FileExtension,

        //[AmbientValue("Filename")]
        //Filename,

        [AmbientValue("File version")]
        FileVersion,

        [AmbientValue("Flag color")]
        FlagColor,

        [AmbientValue("Flag status")]
        FlagStatus,

        //[AmbientValue("Space free")]
        //SpaceFree,

        [AmbientValue("Group")]
        Group,

        [AmbientValue("Sharing type")]
        SharingType,

        [AmbientValue("Bit depth")]
        BitDepth,

        [AmbientValue("Horizontal resolution")]
        HorizontalResolution,

        [AmbientValue("Width")]
        Width,

        [AmbientValue("Vertical resolution")]
        VerticalResolution,

        [AmbientValue("Height")]
        Height,

        [AmbientValue("Importance")]
        Importance,

        [AmbientValue("Is attachment")]
        IsAttachment,

        [AmbientValue("Is deleted")]
        IsDeleted,

        [AmbientValue("Encryption status")]
        EncryptionStatus,

        [AmbientValue("Has flag")]
        HasFlag,

        [AmbientValue("Is completed")]
        IsCompleted,

        [AmbientValue("Incomplete")]
        Incomplete,

        [AmbientValue("Read status")]
        ReadStatus,

        //[AmbientValue("Shared")]
        //Shared,

        [AmbientValue("Creators")]
        Creators,

        [AmbientValue("Date")]
        Date,

        //[AmbientValue("Folder name")]
        //FolderName,

        //[AmbientValue("Folder path")]
        //FolderPath,

        //[AmbientValue("Folder")]
        //Folder,

        [AmbientValue("Participants")]
        Participants,

        //[AmbientValue("Path")]
        //Path,

        [AmbientValue("By location")]
        ByLocation,

        //[AmbientValue("Type")]
        //Type,

        [AmbientValue("Contact names")]
        ContactNames,

        [AmbientValue("Entry type")]
        EntryType,

        [AmbientValue("Language")]
        Language,

        [AmbientValue("Date visited")]
        DateVisited,

        [AmbientValue("Description")]
        Description,

        [AmbientValue("Link status")]
        LinkStatus,

        [AmbientValue("Link target")]
        LinkTarget,

        [AmbientValue("URL")]
        URL,

        [AmbientValue("Media created")]
        MediaCreated,

        [AmbientValue("Date released")]
        DateReleased,

        [AmbientValue("Encoded by")]
        EncodedBy,

        [AmbientValue("Episode number")]
        EpisodeNumber,

        [AmbientValue("Producers")]
        Producers,

        [AmbientValue("Publisher")]
        Publisher,

        [AmbientValue("Season number")]
        SeasonNumber,

        [AmbientValue("Subtitle")]
        Subtitle,

        [AmbientValue("User web URL")]
        UserWebURL,

        [AmbientValue("Writers")]
        Writers,

        [AmbientValue("Attachments")]
        Attachments,

        [AmbientValue("Bcc addresses")]
        BccAddresses,

        [AmbientValue("Bcc")]
        Bcc,

        [AmbientValue("Cc addresses")]
        CcAddresses,

        [AmbientValue("Cc")]
        Cc,

        [AmbientValue("Conversation ID")]
        ConversationID,

        [AmbientValue("Date received")]
        DateReceived,

        [AmbientValue("Date sent")]
        DateSent,

        [AmbientValue("From addresses")]
        FromAddresses,

        [AmbientValue("From")]
        From,

        [AmbientValue("Has attachments")]
        HasAttachments,

        [AmbientValue("Sender address")]
        SenderAddress,

        [AmbientValue("Sender name")]
        SenderName,

        [AmbientValue("Store")]
        Store,

        [AmbientValue("To addresses")]
        ToAddresses,

        [AmbientValue("To do title")]
        ToDoTitle,

        [AmbientValue("To")]
        To,

        [AmbientValue("Mileage")]
        Mileage,

        [AmbientValue("Album artist")]
        AlbumArtist,

        [AmbientValue("Sort album artist")]
        SortAlbumArtist,

        [AmbientValue("Album ID")]
        AlbumID,

        [AmbientValue("Sort album")]
        SortAlbum,

        [AmbientValue("Sort contributing artists")]
        SortContributingArtists,

        [AmbientValue("Beats-per-minute")]
        BeatsPerMinute,

        [AmbientValue("Composers")]
        Composers,

        [AmbientValue("Sort composer")]
        SortComposer,

        [AmbientValue("Disc")]
        Disc,

        [AmbientValue("Initial key")]
        InitialKey,

        [AmbientValue("Part of a compilation")]
        PartOfACompilation,

        [AmbientValue("Mood")]
        Mood,

        [AmbientValue("Part of set")]
        PartOfSet,

        [AmbientValue("Period")]
        Period,

        [AmbientValue("Color")]
        Color,

        [AmbientValue("Parental rating")]
        ParentalRating,

        [AmbientValue("Parental rating reason")]
        ParentalRatingReason,

        //[AmbientValue("Space used")]
        //SpaceUsed,

        [AmbientValue("EXIF version")]
        EXIFVersion,

        [AmbientValue("Event")]
        Event,

        [AmbientValue("Exposure bias")]
        ExposureBias,

        [AmbientValue("Exposure program")]
        ExposureProgram,

        [AmbientValue("Exposure time")]
        ExposureTime,

        [AmbientValue("F-stop")]
        FStop,

        [AmbientValue("Flash mode")]
        FlashMode,

        [AmbientValue("Focal length")]
        FocalLength,

        [AmbientValue("35mm focal length")]
        FocalLength35mm,

        [AmbientValue("ISO speed")]
        ISOSpeed,

        [AmbientValue("Lens maker")]
        LensMaker,

        [AmbientValue("Lens model")]
        LensModel,

        [AmbientValue("Light source")]
        LightSource,

        [AmbientValue("Max aperture")]
        MaxAperture,

        [AmbientValue("Metering mode")]
        MeteringMode,

        [AmbientValue("Orientation")]
        Orientation,

        [AmbientValue("People")]
        People,

        [AmbientValue("Program mode")]
        ProgramMode,

        [AmbientValue("Saturation")]
        Saturation,

        [AmbientValue("Subject distance")]
        SubjectDistance,

        [AmbientValue("White balance")]
        WhiteBalance,

        [AmbientValue("Priority")]
        Priority,

        [AmbientValue("Project")]
        Project,

        [AmbientValue("Channel number")]
        ChannelNumber,

        [AmbientValue("Episode name")]
        EpisodeName,

        [AmbientValue("Closed captioning")]
        ClosedCaptioning,

        [AmbientValue("Rerun")]
        Rerun,

        [AmbientValue("SAP")]
        SAP,

        [AmbientValue("Broadcast date")]
        BroadcastDate,

        [AmbientValue("Program description")]
        ProgramDescription,

        [AmbientValue("Recording time")]
        RecordingTime,

        [AmbientValue("Station call sign")]
        StationCallSign,

        [AmbientValue("Station name")]
        StationName,

        [AmbientValue("Summary")]
        Summary,

        [AmbientValue("Snippets")]
        Snippets,

        [AmbientValue("Auto summary")]
        AutoSummary,

        [AmbientValue("Relevance")]
        Relevance,

        [AmbientValue("File ownership")]
        FileOwnership,

        [AmbientValue("Sensitivity")]
        Sensitivity,

        [AmbientValue("Shared with")]
        SharedWith,

        //[AmbientValue("Sharing status")]
        //SharingStatus,

        [AmbientValue("Product name")]
        ProductName,

        [AmbientValue("Product version")]
        ProductVersion,

        [AmbientValue("Support link")]
        SupportLink,

        [AmbientValue("Source")]
        Source,

        [AmbientValue("Start date")]
        StartDate,

        [AmbientValue("Sharing")]
        Sharing,

        //[AmbientValue("Availability status")]
        //AvailabilityStatus,

        [AmbientValue("Billing information")]
        BillingInformation,

        [AmbientValue("Complete")]
        Complete,

        [AmbientValue("Task owner")]
        TaskOwner,

        [AmbientValue("Sort title")]
        SortTitle,

        [AmbientValue("Total file size")]
        TotalFileSize,

        [AmbientValue("Legal trademarks")]
        LegalTrademarks,

        [AmbientValue("Video compression")]
        VideoCompression,

        [AmbientValue("Directors")]
        Directors,

        [AmbientValue("Data rate")]
        DataRate,

        [AmbientValue("Frame height")]
        FrameHeight,

        [AmbientValue("Frame rate")]
        FrameRate,

        [AmbientValue("Frame width")]
        FrameWidth,

        [AmbientValue("Spherical")]
        Spherical,

        [AmbientValue("Stereo")]
        Stereo,

        [AmbientValue("Video orientation")]
        VideoOrientation,

        [AmbientValue("Total bitrate")]
        TotalBitrate,

        [AmbientValue("Font style")]
        FontStyle,

        [AmbientValue("Show/hide")]
        ShowHide,

        [AmbientValue("Designed for")]
        DesignedFor,

        [AmbientValue("Category")]
        Category,

        [AmbientValue("Designer/foundry")]
        DesignerFoundry,

        [AmbientValue("Font embeddability")]
        FontEmbeddability,

        [AmbientValue("Font type")]
        FontType,

        [AmbientValue("Family")]
        Family,

        [AmbientValue("Collection")]
        Collection,

        [AmbientValue("Font file names")]
        FontFileNames,

        [AmbientValue("Font version")]
        FontVersion,

        //[AmbientValue("Name")]
        //Name,

        //[AmbientValue("Size")]
        //Size,

        //[AmbientValue("Date modified")]
        //DateModified,

        [AmbientValue("Date created")]
        DateCreated,

        //[AmbientValue("Date accessed")]
        //DateAccessed,

        [AmbientValue("Attributes")]
        Attributes,

        [AmbientValue("Offline status")]
        OfflineStatus,

        [AmbientValue("Availability")]
        Availability,

        [AmbientValue("Owner")]
        Owner,

    }
}
