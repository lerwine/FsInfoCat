$CanIgnore = @(
     'Size',
    # 'ItemType',
    'DateModified',
    # 'DateCreated',
    'DateAccessed',
    'Attributes',
    'OfflineStatus',
    'Availability',
    'PerceivedType',
    'Owner',
    # 'Kind',
    # 'DateTaken',
    'ContributingArtists',
    # 'Album',
    # 'Year',
    # 'Genre',
    # 'Conductors',
    # 'Tags',
    # 'Rating',
    #'Authors',
    # 'Title',
    # 'Subject',
    'Categories',
    #'Comments',
    # 'Copyright',
    '#',
    'Length',
    'BitRate',
    #'Protected',
    # 'CameraModel',
    # 'Dimensions',
    'CameraMaker',
    # 'Company',
    # 'FileDescription',
    'MastersKeywords',
    'MastersKeywords',
    'ProgramName',
    # 'Duration',
    'IsOnline',
    'IsRecurring',
    'Location',
    'OptionalAttendeeAddresses',
    'OptionalAttendees',
    'OrganizerAddress',
    'OrganizerName',
    'ReminderTime',
    'RequiredAttendeeAddresses',
    'RequiredAttendees',
    'Resources',
    'MeetingStatus',
    'Free/busyStatus',
    'TotalSize',
    'AccountName',
    'TaskStatus',
    'Computer',
    'Anniversary',
    'Assistant''sName',
    'Assistant''sPhone',
    'Birthday',
    'BusinessAddress',
    'BusinessCity',
    'BusinessCountry/region',
    'BusinessP.O.Box',
    'BusinessPostalCode',
    'BusinessStateOrProvince',
    'BusinessStreet',
    'BusinessFax',
    'BusinessHomePage',
    'BusinessPhone',
    'CallbackNumber',
    'CarPhone',
    'Children',
    'CompanyMainPhone',
    'Department',
    'E-mailAddress',
    'E-mail2',
    'E-mail3',
    'E-mailList',
    'E-mailDisplayName',
    'FileAs',
    'FirstName',
    'FullName',
    'Gender',
    'GivenName',
    'Hobbies',
    'HomeAddress',
    'HomeCity',
    'HomeCountry/region',
    'HomeP.O.Box',
    'HomePostalCode',
    'HomeStateOrProvince',
    'HomeStreet',
    'HomeFax',
    'HomePhone',
    'IMAddresses',
    'Initials',
    'JobTitle',
    'Label',
    'LastName',
    'MailingAddress',
    'MiddleName',
    'CellPhone',
    'Nickname',
    'OfficeLocation',
    'OtherAddress',
    'OtherCity',
    'OtherCountry/region',
    'OtherP.O.Box',
    'OtherPostalCode',
    'OtherStateOrProvince',
    'OtherStreet',
    'Pager',
    'PersonalTitle',
    'City',
    'Country/region',
    'P.O.Box',
    'PostalCode',
    'StateOrProvince',
    'Street',
    'PrimaryE-mail',
    'PrimaryPhone',
    'Profession',
    'Spouse/Partner',
    'Suffix',
    'TTY/TTDPhone',
    'Telex',
    'Webpage',
    'ContentStatus',
    # 'ContentType',
    'DateAcquired',
    'DateArchived',
    'DateCompleted',
    'DeviceCategory',
    'Connected',
    'DiscoveryMethod',
    'FriendlyName',
    'LocalComputer',
    'Manufacturer',
    'Model',
    'Paired',
    'Classification',
    'Status',
    'Status',
    # 'ClientID',
    # 'Contributors',
    'ContentCreated',
    'LastPrinted',
    'DateLastSaved',
    # 'Division',
    # 'DocumentID',
    'Pages',
    'Slides',
    'TotalEditingTime',
    # 'WordCount',
    'DueDate',
    'EndDate',
    'FileCount',
    'FileExtension',
    'Filename',
    # 'FileVersion',
    'FlagColor',
    'FlagStatus',
    'SpaceFree',
    'Group',
    'SharingType',
    # 'BitDepth',
    # 'HorizontalResolution',
    'Width',
    # 'VerticalResolution',
    'Height',
    'Importance',
    'IsAttachment',
    'IsDeleted',
    'EncryptionStatus',
    'HasFlag',
    'IsCompleted',
    'Incomplete',
    'ReadStatus',
    'Shared',
    'Creators',
    'Date',
    'FolderName',
    'FolderPath',
    'Folder',
    'Participants',
    'Path',
    'ByLocation',
    'Type',
    'ContactNames',
    'EntryType',
    'Language',
    'DateVisited',
    # 'Description',
    'LinkStatus',
    'LinkTarget',
    'URL',
    'MediaCreated',
    # 'DateReleased',
    'EncodedBy',
    'EpisodeNumber',
    # 'Producers',
    # 'Publisher',
    'SeasonNumber',
    # 'Subtitle',
    'UserWebURL',
    'Writers',
    'Attachments',
    'BccAddresses',
    'Bcc',
    'CcAddresses',
    'Cc',
    'ConversationID',
    'DateReceived',
    'DateSent',
    'FromAddresses',
    'From',
    'HasAttachments',
    'SenderAddress',
    'SenderName',
    'Store',
    'ToAddresses',
    'ToDoTitle',
    'To',
    'Mileage',
    # 'AlbumArtist',
    'SortAlbumArtist',
    # 'AlbumID',
    'SortAlbum',
    'SortContributingArtists',
    'Beats-per-minute',
    #'Composers',
    'SortComposer',
    'Disc',
    'InitialKey',
    'PartOfACompilation',
    # 'Mood',
    # 'PartOfSet',
    # 'Period',
    # 'Color',
    # 'ParentalRating',
    # 'ParentalRatingReason',
    'SpaceUsed',
    # 'EXIFVersion',
    # 'Event',
    'ExposureBias',
    'ExposureProgram',
    'ExposureTime',
    'F-stop',
    'FlashMode',
    'FocalLength',
    '35mmFocalLength',
    'ISOSpeed',
    'LensMaker',
    'LensModel',
    'LightSource',
    'MaxAperture',
    'MeteringMode',
    # 'Orientation',
    # 'People',
    'ProgramMode',
    'Saturation',
    'SubjectDistance',
    'WhiteBalance',
    'Priority',
    # 'Project',
    # 'ChannelNumber',
    # 'EpisodeName',
    'ClosedCaptioning',
    'Rerun',
    'SAP',
    # 'BroadcastDate',
    # 'ProgramDescription',
    # 'RecordingTime',
    # 'StationCallSign',
    # 'StationName',
    'Summary',
    'Snippets',
    'AutoSummary',
    'Relevance',
    'FileOwnership',
    # 'Sensitivity',
    'SharedWith',
    'SharingStatus',
    # 'ProductName',
    'ProductVersion',
    'SupportLink',
    #'Source',
    'StartDate',
    'Sharing',
    'AvailabilityStatus',
    'Status',
    'BillingInformation',
    'Complete',
    'TaskOwner',
    'SortTitle',
    'TotalFileSize',
    # 'LegalTrademarks',
    # 'VideoCompression',
    # 'Directors',
    # 'DataRate',
    # 'FrameHeight',
    # 'FrameRate',
    # 'FrameWidth',
    'Spherical',
    'Stereo',
    'VideoOrientation'
    # 'TotalBitrate',
);
class PropertyMapTarget {
    [string[]]$Category;
    [string]$PropertyName;
}
$PropertyMap = @{
    'TotalBitrate' = [PropertyMapTarget]@{ Category = 'Video'; PropertyName = 'VerticalAspectRatio' };
    'Source' = [PropertyMapTarget]@{ Category = 'Summary'; PropertyName = 'SourceItem' };
    'RecordingTime' = [PropertyMapTarget]@{ Category = 'RecordedTV'; PropertyName = 'RecordingTime' };
    'Project' = [PropertyMapTarget]@{ Category = 'Summary'; PropertyName = 'Project' };
    'Mood' = [PropertyMapTarget]@{ Category = 'Music'; PropertyName = 'Mood' };
    'AlbumID' = [PropertyMapTarget]@{ Category = 'Music'; PropertyName = 'AlbumID' };
    'WordCount' = [PropertyMapTarget]@{ Category = ([string[]]@('Document')); PropertyName = 'WordCount' };
    'Dimensions' = [PropertyMapTarget]@{ Category = ([string[]]@('Image')); PropertyName = 'Dimensions' };
    
    'Compression' = [PropertyMapTarget]@{ Category = ([string[]]@('Audio', 'Image', 'Video')); PropertyName = 'Compression' };
    'VideoCompression' = [PropertyMapTarget]@{ Category = ([string[]]@('Video')); PropertyName = 'Compression' };
    'EncodingBitrate' = [PropertyMapTarget]@{ Category = ([string[]]@('Audio')); PropertyName = 'EncodingBitrate' };
    'DataRate' = [PropertyMapTarget]@{ Category = ([string[]]@('Video')); PropertyName = 'EncodingBitrate' };
    'Format' = [PropertyMapTarget]@{ Category = ([string[]]@('Audio')); PropertyName = 'Format' };
    'IsVariableBitrate' = [PropertyMapTarget]@{ Category = ([string[]]@('Audio')); PropertyName = 'IsVariableBitrate' };
    'SampleRate' = [PropertyMapTarget]@{ Category = ([string[]]@('Audio')); PropertyName = 'SampleRate' };
    'SampleSize' = [PropertyMapTarget]@{ Category = ([string[]]@('Audio')); PropertyName = 'SampleSize' };
    'StreamName' = [PropertyMapTarget]@{ Category = ([string[]]@('Audio', 'Video')); PropertyName = 'StreamName' };
    'StreamNumber' = [PropertyMapTarget]@{ Category = ([string[]]@('Audio', 'Video')); PropertyName = 'StreamNumber' };
    'ClientID' = [PropertyMapTarget]@{ Category = ([string[]]@('Document')); PropertyName = 'ClientID' };
    'Contributor' = [PropertyMapTarget]@{ Category = ([string[]]@('Document')); PropertyName = 'Contributor' };
    'Contributors' = [PropertyMapTarget]@{ Category = ([string[]]@('Document')); PropertyName = 'Contributor' };
    'DateCreated' = [PropertyMapTarget]@{ Category = ([string[]]@('Document')); PropertyName = 'DateCreated' };
    'LastAuthor' = [PropertyMapTarget]@{ Category = ([string[]]@('Document')); PropertyName = 'LastAuthor' };
    'RevisionNumber' = [PropertyMapTarget]@{ Category = ([string[]]@('Document')); PropertyName = 'RevisionNumber' };
    'Security' = [PropertyMapTarget]@{ Category = ([string[]]@('Document')); PropertyName = 'Security' };
    'Division' = [PropertyMapTarget]@{ Category = ([string[]]@('Document')); PropertyName = 'Division' };
    'DocumentID' = [PropertyMapTarget]@{ Category = ([string[]]@('Document')); PropertyName = 'DocumentID' };
    'Manager' = [PropertyMapTarget]@{ Category = ([string[]]@('Document')); PropertyName = 'Manager' };
    'PresentationFormat' = [PropertyMapTarget]@{ Category = ([string[]]@('Document')); PropertyName = 'PresentationFormat' };
    'Version' = [PropertyMapTarget]@{ Category = ([string[]]@('Document')); PropertyName = 'Version' };
    'DatePlayExpires' = [PropertyMapTarget]@{ Category = ([string[]]@('DRM')); PropertyName = 'DatePlayExpires' };
    'DatePlayStarts' = [PropertyMapTarget]@{ Category = ([string[]]@('DRM')); PropertyName = 'DatePlayStarts' };
    'Description' = [PropertyMapTarget]@{ Category = ([string[]]@('DRM')); PropertyName = 'Description' };
    'IsProtected' = [PropertyMapTarget]@{ Category = ([string[]]@('DRM')); PropertyName = 'IsProtected' };
    'Protected' = [PropertyMapTarget]@{ Category = ([string[]]@('DRM')); PropertyName = 'IsProtected' };
    'PlayCount' = [PropertyMapTarget]@{ Category = ([string[]]@('DRM')); PropertyName = 'PlayCount' };
    'AreaInformation' = [PropertyMapTarget]@{ Category = ([string[]]@('GPS')); PropertyName = 'AreaInformation' };
    'LatitudeDegrees' = [PropertyMapTarget]@{ Category = ([string[]]@('GPS')); PropertyName = 'LatitudeDegrees' };
    'LatitudeMinutes' = [PropertyMapTarget]@{ Category = ([string[]]@('GPS')); PropertyName = 'LatitudeMinutes' };
    'LatitudeSeconds' = [PropertyMapTarget]@{ Category = ([string[]]@('GPS')); PropertyName = 'LatitudeSeconds' };
    'LatitudeRef' = [PropertyMapTarget]@{ Category = ([string[]]@('GPS')); PropertyName = 'LatitudeRef' };
    'LongitudeDegrees' = [PropertyMapTarget]@{ Category = ([string[]]@('GPS')); PropertyName = 'LongitudeDegrees' };
    'LongitudeMinutes' = [PropertyMapTarget]@{ Category = ([string[]]@('GPS')); PropertyName = 'LongitudeMinutes' };
    'LongitudeSeconds' = [PropertyMapTarget]@{ Category = ([string[]]@('GPS')); PropertyName = 'LongitudeSeconds' };
    'LongitudeRef' = [PropertyMapTarget]@{ Category = ([string[]]@('GPS')); PropertyName = 'LongitudeRef' };
    'MeasureMode' = [PropertyMapTarget]@{ Category = ([string[]]@('GPS')); PropertyName = 'MeasureMode' };
    'ProcessingMethod' = [PropertyMapTarget]@{ Category = ([string[]]@('GPS')); PropertyName = 'ProcessingMethod' };
    'VersionID' = [PropertyMapTarget]@{ Category = ([string[]]@('GPS')); PropertyName = 'VersionID' };
    'BitDepth' = [PropertyMapTarget]@{ Category = ([string[]]@('Image')); PropertyName = 'BitDepth' };
    'ColorSpace' = [PropertyMapTarget]@{ Category = ([string[]]@('Image')); PropertyName = 'ColorSpace' };
    'Color' = [PropertyMapTarget]@{ Category = ([string[]]@('Image')); PropertyName = 'ColorSpace' };
    'CompressedBitsPerPixel' = [PropertyMapTarget]@{ Category = ([string[]]@('Image')); PropertyName = 'CompressedBitsPerPixel' };
    'CompressionText' = [PropertyMapTarget]@{ Category = ([string[]]@('Image')); PropertyName = 'CompressionText' };
    'HorizontalResolution' = [PropertyMapTarget]@{ Category = ([string[]]@('Image')); PropertyName = 'HorizontalResolution' };
    'HorizontalSize' = [PropertyMapTarget]@{ Category = ([string[]]@('Image')); PropertyName = 'HorizontalSize' };
    'ImageID' = [PropertyMapTarget]@{ Category = ([string[]]@('Image')); PropertyName = 'ImageID' };
    'ResolutionUnit' = [PropertyMapTarget]@{ Category = ([string[]]@('Image')); PropertyName = 'ResolutionUnit' };
    'VerticalResolution' = [PropertyMapTarget]@{ Category = ([string[]]@('Image')); PropertyName = 'VerticalResolution' };
    'VerticalSize' = [PropertyMapTarget]@{ Category = ([string[]]@('Image')); PropertyName = 'VerticalSize' };
    'ContentDistributor' = [PropertyMapTarget]@{ Category = 'Media'; PropertyName = 'ContentDistributor' };
    'CreatorApplicationVersion' = [PropertyMapTarget]@{ Category = 'Media'; PropertyName = 'CreatorApplicationVersion' };
    'DateReleased' = [PropertyMapTarget]@{ Category = 'Media'; PropertyName = 'DateReleased' };
    'Duration' = [PropertyMapTarget]@{ Category = 'Media'; PropertyName = 'Duration' };
    'DVDID' = [PropertyMapTarget]@{ Category = 'Media'; PropertyName = 'DVDID' };
    'FrameCount' = [PropertyMapTarget]@{ Category = 'Media'; PropertyName = 'FrameCount' };
    'Producer' = [PropertyMapTarget]@{ Category = 'Media'; PropertyName = 'Producer' };
    'Producers' = [PropertyMapTarget]@{ Category = 'Media'; PropertyName = 'Producer' };
    'ProtectionType' = [PropertyMapTarget]@{ Category = 'Media'; PropertyName = 'ProtectionType' };
    'ProviderRating' = [PropertyMapTarget]@{ Category = 'Media'; PropertyName = 'ProviderRating' };
    'ProviderStyle' = [PropertyMapTarget]@{ Category = 'Media'; PropertyName = 'ProviderStyle' };
    'Publisher' = [PropertyMapTarget]@{ Category = 'Media'; PropertyName = 'Publisher' };
    'Subtitle' = [PropertyMapTarget]@{ Category = 'Media'; PropertyName = 'Subtitle' };
    'Writer' = [PropertyMapTarget]@{ Category = 'Media'; PropertyName = 'Writer' };
    'Year' = [PropertyMapTarget]@{ Category = 'Media'; PropertyName = 'Year' };
    'AlbumArtist' = [PropertyMapTarget]@{ Category = 'Music'; PropertyName = 'AlbumArtist' };
    'Album' = [PropertyMapTarget]@{ Category = 'Music'; PropertyName = 'AlbumTitle' };
    'AlbumTitle' = [PropertyMapTarget]@{ Category = 'Music'; PropertyName = 'AlbumTitle' };
    'Artist' = [PropertyMapTarget]@{ Category = 'Music'; PropertyName = 'Artist' };
    'ChannelCount' = [PropertyMapTarget]@{ Category = 'Music'; PropertyName = 'ChannelCount' };
    'Composer' = [PropertyMapTarget]@{ Category = 'Music'; PropertyName = 'Composer' };
    'Composers' = [PropertyMapTarget]@{ Category = 'Music'; PropertyName = 'Composer' };
    'Conductor' = [PropertyMapTarget]@{ Category = 'Music'; PropertyName = 'Conductor' };
    'Conductors' = [PropertyMapTarget]@{ Category = 'Music'; PropertyName = 'Conductor' };
    'DisplayArtist' = [PropertyMapTarget]@{ Category = 'Music'; PropertyName = 'DisplayArtist' };
    'Genre' = [PropertyMapTarget]@{ Category = 'Music'; PropertyName = 'Genre' };
    'PartOfSet' = [PropertyMapTarget]@{ Category = 'Music'; PropertyName = 'PartOfSet' };
    'Period' = [PropertyMapTarget]@{ Category = 'Music'; PropertyName = 'Period' };
    'TrackNumber' = [PropertyMapTarget]@{ Category = 'Music'; PropertyName = 'TrackNumber' };
    'CameraManufacturer' = [PropertyMapTarget]@{ Category = 'Photo'; PropertyName = 'CameraManufacturer' };
    'CameraModel' = [PropertyMapTarget]@{ Category = 'Photo'; PropertyName = 'CameraModel' };
    'DateTaken' = [PropertyMapTarget]@{ Category = 'Photo'; PropertyName = 'DateTaken' };
    'Event' = [PropertyMapTarget]@{ Category = 'Photo'; PropertyName = 'Event' };
    'EXIFVersion' = [PropertyMapTarget]@{ Category = 'Photo'; PropertyName = 'EXIFVersion' };
    'Orientation' = [PropertyMapTarget]@{ Category = 'Photo'; PropertyName = 'Orientation' };
    'OrientationText' = [PropertyMapTarget]@{ Category = 'Photo'; PropertyName = 'OrientationText' };
    'PeopleNames' = [PropertyMapTarget]@{ Category = 'Photo'; PropertyName = 'PeopleNames' };
    'People' = [PropertyMapTarget]@{ Category = 'Photo'; PropertyName = 'PeopleNames' };
    'ChannelNumber' = [PropertyMapTarget]@{ Category = 'RecordedTV'; PropertyName = 'ChannelNumber' };
    'EpisodeName' = [PropertyMapTarget]@{ Category = 'RecordedTV'; PropertyName = 'EpisodeName' };
    'IsDTVContent' = [PropertyMapTarget]@{ Category = 'RecordedTV'; PropertyName = 'IsDTVContent' };
    'IsHDContent' = [PropertyMapTarget]@{ Category = 'RecordedTV'; PropertyName = 'IsHDContent' };
    'NetworkAffiliation' = [PropertyMapTarget]@{ Category = 'RecordedTV'; PropertyName = 'NetworkAffiliation' };
    'OriginalBroadcastDate' = [PropertyMapTarget]@{ Category = 'RecordedTV'; PropertyName = 'OriginalBroadcastDate' };
    'BroadcastDate' = [PropertyMapTarget]@{ Category = 'RecordedTV'; PropertyName = 'OriginalBroadcastDate' };
    'ProgramDescription' = [PropertyMapTarget]@{ Category = 'RecordedTV'; PropertyName = 'ProgramDescription' };
    'StationCallSign' = [PropertyMapTarget]@{ Category = 'RecordedTV'; PropertyName = 'StationCallSign' };
    'StationName' = [PropertyMapTarget]@{ Category = 'RecordedTV'; PropertyName = 'StationName' };
    'ApplicationName' = [PropertyMapTarget]@{ Category = 'Summary'; PropertyName = 'ApplicationName' };
    'Author' = [PropertyMapTarget]@{ Category = 'Summary'; PropertyName = 'Author' };
    'Authors' = [PropertyMapTarget]@{ Category = 'Summary'; PropertyName = 'Author' };
    'Comment' = [PropertyMapTarget]@{ Category = 'Summary'; PropertyName = 'Comment' };
    'Comments' = [PropertyMapTarget]@{ Category = 'Summary'; PropertyName = 'Comment' };
    'Keywords' = [PropertyMapTarget]@{ Category = 'Summary'; PropertyName = 'Keywords' };
    'Subject' = [PropertyMapTarget]@{ Category = 'Summary'; PropertyName = 'Subject' };
    'Title' = [PropertyMapTarget]@{ Category = 'Summary'; PropertyName = 'Title' };
    'FileDescription' = [PropertyMapTarget]@{ Category = 'Summary'; PropertyName = 'FileDescription' };
    'FileVersion' = [PropertyMapTarget]@{ Category = 'Summary'; PropertyName = 'FileVersion' };
    'Company' = [PropertyMapTarget]@{ Category = 'Summary'; PropertyName = 'Company' };
    'ContentType' = [PropertyMapTarget]@{ Category = 'Summary'; PropertyName = 'ContentType' };
    'Copyright' = [PropertyMapTarget]@{ Category = 'Summary'; PropertyName = 'Copyright' };
    'ParentalRating' = [PropertyMapTarget]@{ Category = 'Summary'; PropertyName = 'ParentalRating' };
    'Rating' = [PropertyMapTarget]@{ Category = 'Summary'; PropertyName = 'Rating' };
    'ItemAuthors' = [PropertyMapTarget]@{ Category = 'Summary'; PropertyName = 'ItemAuthors' };
    'ItemType' = [PropertyMapTarget]@{ Category = 'Summary'; PropertyName = 'ItemType' };
    'Type' = [PropertyMapTarget]@{ Category = 'Summary'; PropertyName = 'ItemType' };
    'ItemTypeText' = [PropertyMapTarget]@{ Category = 'Summary'; PropertyName = 'ItemTypeText' };
    'Kind' = [PropertyMapTarget]@{ Category = 'Summary'; PropertyName = 'Kind' };
    'MIMEType' = [PropertyMapTarget]@{ Category = 'Summary'; PropertyName = 'MIMEType' };
    'ParentalRatingReason' = [PropertyMapTarget]@{ Category = 'Summary'; PropertyName = 'ParentalRatingReason' };
    'ParentalRatingsOrganization' = [PropertyMapTarget]@{ Category = 'Summary'; PropertyName = 'ParentalRatingsOrganization' };
    'Sensitivity' = [PropertyMapTarget]@{ Category = 'Summary'; PropertyName = 'Sensitivity' };
    'SensitivityText' = [PropertyMapTarget]@{ Category = 'Summary'; PropertyName = 'SensitivityText' };
    'SimpleRating' = [PropertyMapTarget]@{ Category = 'Summary'; PropertyName = 'SimpleRating' };
    'Trademarks' = [PropertyMapTarget]@{ Category = 'Summary'; PropertyName = 'Trademarks' };
    'LegalTrademarks' = [PropertyMapTarget]@{ Category = 'Summary'; PropertyName = 'Trademarks' };
    'ProductName' = [PropertyMapTarget]@{ Category = 'Summary'; PropertyName = 'ProductName' };
    'Director' = [PropertyMapTarget]@{ Category = 'Video'; PropertyName = 'Director' };
    'Directors' = [PropertyMapTarget]@{ Category = 'Video'; PropertyName = 'Director' };
    'FrameHeight' = [PropertyMapTarget]@{ Category = 'Video'; PropertyName = 'FrameHeight' };
    'FrameRate' = [PropertyMapTarget]@{ Category = 'Video'; PropertyName = 'FrameRate' };
    'FrameWidth' = [PropertyMapTarget]@{ Category = 'Video'; PropertyName = 'FrameWidth' };
    'HorizontalAspectRatio' = [PropertyMapTarget]@{ Category = 'Video'; PropertyName = 'HorizontalAspectRatio' };
    'VerticalAspectRatio' = [PropertyMapTarget]@{ Category = 'Video'; PropertyName = 'VerticalAspectRatio' };
}
<#
#$CanIgnore | % { if ($null -eq $PropertyMap[$_]) { "    '$_'," } else { "    # '$_'," } }
$CanIgnore | ? { $null -ne $PropertyMap[$_] }

([\r\n]+\s*///[^\r\n]*)+
(\w+),
'$1' = [PropertyMapTarget]@{ Category = 'GPS'; PropertyName = '$1' };
#>

<#
@($Com=(New-Object -ComObject Shell.Application).NameSpace('C:\');1..400 | ForEach-Object {$com.GetDetailsOf($com.Items,$_)} | Where-Object {$_} | ForEach-Object { -join (($_ -split '\s+') | ForEach-Object { $_.Substring(0, 1).ToUpper() + $_.Substring(1) }) }) -join "',`n    '"
$Com=(New-Object -ComObject Shell.Application).NameSpace('C:\');1..400 | ForEach-Object { [PSObject]@{ N = $_; V = $com.GetDetailsOf($com.Items,$_) }} | Where-Object {$_.V} | ForEach-Object {
    $k = -join (($_.V -split '\s+') | ForEach-Object { $_.Substring(0, 1).ToUpper() + $_.Substring(1) });
    "[PSObject]@{ PropId = $($_.N); Key = `"$k`" }"
}
#>


$shell  = New-Object -COMObject Shell.Application
$myfile = Get-Item -LiteralPath "D:\Virtual Drives\V\ALSScan\Aurora Zvezda - Hijinks BTS » PissRIP Free Pissing Videos - .mp4"
$file   = $myfile.Name
$path   = $myfile.DirectoryName

$shellfolder = $shell.Namespace($path);
$shellfile   = $shellfolder.ParseName($file);

$ByType = @{};
1..1024 | ForEach-Object {$shellfolder.GetDetailsOf($shellfolder.Items,$_)} | Where-Object {$_} | ForEach-Object { -join (($_ -split '\s+') | ForEach-Object { $_.Substring(0, 1).ToUpper() + $_.Substring(1) }) } | ForEach-Object { 
    $v = $shellfile.ExtendedProperty($_);
    if ($null -ne $v) {
        if ($v -is [string]) {
            if ($v.Trim().Length -gt 0) {
                if ($PropertyMap.ContainsKey($_)) {
                    $m = $PropertyMap[$_];
                    foreach ($c in $m.Category) {
                        if ($ByType.ContainsKey($c)) {
                            $ByType[$c][$m.PropertyName] = $v;
                        } else {
                            $h = @{ };
                            $h[$m.PropertyName] = $v;
                            $ByType[$c] = $h;
                        }
                    }
                } else {
                    if ($CanIgnore -notcontains $_) { Write-Warning -Message "Unmapped property name: $_" } else { Write-Host -Object "$_=$v" -ForegroundColor Cyan }
                }
            }
        } else {
            if ($PropertyMap.ContainsKey($_)) {
                $m = $PropertyMap[$_];
                foreach ($c in $m.Category) {
                    if ($ByType.ContainsKey($c)) {
                        $ByType[$c][$m.PropertyName] = $v;
                    } else {
                        $h = @{ };
                        $h[$m.PropertyName] = $v;
                        $ByType[$c] = $h;
                    }
                }
            } else {
                if ($CanIgnore -notcontains $_) { Write-Warning -Message "Unmapped property name: $_" }
            }
        }
    }
}
$LinkedList = [System.Collections.Generic.LinkedList[System.IO.DirectoryInfo]]::new();
for ($DirectoryInfo = $myfile.Directory; $null -ne $DirectoryInfo; $DirectoryInfo = $DirectoryInfo.Parent) {
    $LinkedList.AddFirst($DirectoryInfo) | Out-Null;
}
$ParentId = [Guid]::NewGuid().ToString();
foreach ($DirectoryInfo in $LinkedList) {
    $Id = [Guid]::NewGuid().ToString();
    $Now = [DateTime]::Now.ToString('yyyy-MM-dd HH:mm:ss');
    @"
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('$Id', '$($DirectoryInfo.Name -replace '\\$', '')', '$Now', '$($DirectoryInfo.CreationTime.ToString('yyyy-MM-dd HH:mm:ss')))', '$($DirectoryInfo.LastWriteTime.ToString('yyyy-MM-dd HH:mm:ss'))', '$ParentId', NULL, '$Now', '$Now');
"@
    $ParentId =  $Id;
}
$BinaryPropertiesId = [Guid]::NewGuid().ToString();
$Now = [DateTime]::Now.ToString('yyyy-MM-dd HH:mm:ss');
$Stream = $myfile.Open([System.IO.FileMode]::Open);
try {
    $MD5 = [System.Security.Cryptography.MD5]::Create();
    $Bytes = $MD5.ComputeHash($Stream);
    $Hash = -join ($Bytes | % { $_.ToString('x2') });
} finally { $Stream.Close() }
@"
INSERT INTO "BinaryPropertySets" ("Id", "Length", "Hash", "CreatedOn", "ModifiedOn")
    VALUES('$BinaryPropertiesId', $($myfile.Length), X'$Hash', '$Now', '$Now');
"@
$FileId = [Guid]::NewGuid().ToString();
$Now = [DateTime]::Now.ToString('yyyy-MM-dd HH:mm:ss');
$Sql1 = 'INSERT INTO "Files" ("Id", "Name", "CreationTime", "LastWriteTime", "ParentId", "BinaryPropertySetId"';
$Sql2 = "VALUES ('$FileId', '$($myfile.Name)', '$($myfile.CreationTime.ToString('yyyy-MM-dd HH:mm:ss'))', '$($myfile.LastWriteTime.ToString('yyyy-MM-dd HH:mm:ss'))', '$ParentId', '$BinaryPropertySetId'";
foreach ($t in $ByType.Keys) {
    $Now = [DateTime]::Now.ToString('yyyy-MM-dd HH:mm:ss');
    $h = $ByType[$t];
    $pn = @($h.Keys);
    $Id = [Guid]::NewGuid().ToString();
    $Sql1 = "$Sql1, `"$($t)PropertySetId`"";
    $Sql2 = "$Sql2, '$Id'";
    $SqlA = "INSERT INTO `"$($t)PropertySets`" (`"Id`"";
    $SqlB = "VALUES('$Id'";
    $pn | ForEach-Object {
        $v = $h[$_];
        $SqlA = "$SqlA, `"$_`"";
        if ($v -is [string]) {
            $SqlB = "$SqlB, '$($v.Replace("'", "''"))'";
        } else {
            if ($v -is [bool]) {
                if ($v) {
                    $SqlB = "$SqlB, 1";
                } else {
                    $SqlB = "$SqlB. 0";
                }
            } else {
                $SqlB = "$SqlB, $v";
            }
        }
    }
    @"
$SqlA, "CreatedOn", "ModifiedOn")
    $SqlB, '$Now', '$Now');
"@
}
$Now = [DateTime]::Now.ToString('yyyy-MM-dd HH:mm:ss');
@"
$Sql1, "CreatedOn", "ModifiedOn")
    $Sql2, '$Now', '$Now');
"@
<#
$shellfolder = $null;
$shellfile = $null;
$shell = $null;
#>

