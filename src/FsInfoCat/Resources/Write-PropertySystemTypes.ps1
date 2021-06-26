$Path = $PSScriptRoot | Join-Path -ChildPath 'PropertySystem.xml';
$BaseUrl = 'https://docs.microsoft.com/en-us/windows/win32/properties/props-system';
[Xml]$Xml = '<Properties/>';
$Xml.Load($Path);
$ClipboardText = '';
foreach ($SetElement in @($Xml.DocumentElement.SelectNodes('Set'))) {
    [Xml]$ResultXml = '<CommentDoc/>';
    $ResultXml.DocumentElement.Attributes.Append($ResultXml.CreateAttribute('Name')).Value = $SetElement.Name;
    foreach ($PropertyElement in @($SetElement.SelectNodes('Property[not(count(Column)=0)]'))) {
        $TextElement = $PropertyElement.SelectSingleNode('Text');
        $Win32Element = $PropertyElement.SelectSingleNode('Win32');
        $FormatElement = $Xml.DocumentElement.SelectSingleNode("Format[@ID=`"$($Win32Element.FormatID)`"]");
        
        $Summary = ('' + $TextElement.Summary).Trim();
        if ($Summary.Length -eq 0) { $Summary = '' + $TextElement.DisplayName }
        $ResultXml.PreserveWhitespace = $true;
        $ResultXml.DocumentElement.AppendChild($ResultXml.CreateTextNode("`n    ")) | Out-Null;
        $DocElement = $ResultXml.DocumentElement.AppendChild($ResultXml.CreateElement('property'));
        $DocElement.AppendChild($ResultXml.CreateTextNode("`n        /// ")) | Out-Null;
        $XmlElement = $DocElement.AppendChild($ResultXml.CreateElement('summary'));
        $XmlElement.AppendChild($ResultXml.CreateTextNode("`n        /// $($Summary)`n        /// ")) | Out-Null;
        $DocElement.AppendChild($ResultXml.CreateTextNode("`n        /// ")) | Out-Null;
        if (-not [string]::IsNullOrWhiteSpace($TextElement.ValueDesc)) {
            $XmlElement = $DocElement.AppendChild($ResultXml.CreateElement('value'));
            $XmlElement.AppendChild($ResultXml.CreateTextNode("`n        /// $($TextElement.ValueDesc)`n        /// ")) | Out-Null;
            $DocElement.AppendChild($ResultXml.CreateTextNode("`n        /// ")) | Out-Null;
        }
        $XmlElement = $DocElement.AppendChild($ResultXml.CreateElement('remarks'));
        if (-not $TextElement.IsEmpty) {
            $XmlElement.AppendChild($ResultXml.CreateTextNode("`n        /// $($TextElement.InnerText)`n        /// ")) | Out-Null;
        }
        #$DocElement.AppendChild($ResultXml.CreateTextNode("`n        /// ")) | Out-Null;
        $XmlElement = $XmlElement.AppendChild($ResultXml.CreateElement('list'));
        $XmlElement.Attributes.Append($ResultXml.CreateAttribute('type')).Value = 'bullet';
        $XmlElement.AppendChild($ResultXml.CreateTextNode("`n        /// ")) | Out-Null;
        $ItemElement = $XmlElement.AppendChild($ResultXml.CreateElement('item'));
        $ItemElement.AppendChild($ResultXml.CreateElement('term')).InnerText = 'Name';
        $ItemElement.AppendChild($ResultXml.CreateElement('description')).InnerText = $TextElement.DisplayName;
        $XmlElement.AppendChild($ResultXml.CreateTextNode("`n        /// ")) | Out-Null;
        $ItemElement = $XmlElement.AppendChild($ResultXml.CreateElement('item'));
        $ItemElement.AppendChild($ResultXml.CreateElement('term')).InnerText = 'Format ID';
        $n = '';
        if ($null -ne $FormatElement) { $n = '' + $FormatElement.Name }
        if ($n.Length -gt 0) {
            $ItemElement.AppendChild($ResultXml.CreateElement('description')).InnerText = "$($Win32Element.FormatID) ($n)";
        } else {
            $ItemElement.AppendChild($ResultXml.CreateElement('description')).InnerText = $Win32Element.FormatID;
        }
        $XmlElement.AppendChild($ResultXml.CreateTextNode("`n        /// ")) | Out-Null;
        $ItemElement = $XmlElement.AppendChild($ResultXml.CreateElement('item'));
        $ItemElement.AppendChild($ResultXml.CreateElement('term')).InnerText = 'Property ID';
        $ItemElement.AppendChild($ResultXml.CreateElement('description')).InnerText = $Win32Element.ID;
        $XmlElement.AppendChild($ResultXml.CreateTextNode("`n        /// ")) | Out-Null;
        $Url = ('' + $Win32Element.InnerText).Trim();
        if ($Url.Length -gt 0) {
            $ItemElement = $XmlElement.AppendChild($ResultXml.CreateElement('item'));
            $e = $ItemElement.AppendChild($ResultXml.CreateElement('description')).AppendChild($ResultXml.CreateElement('a'));
            $e.Attributes.Append($ResultXml.CreateAttribute('href')).Value = $Win32Element.InnerText;
            $e.InnerText = '[Reference Link]';
        }
        $XmlElement.AppendChild($ResultXml.CreateTextNode("`n        /// ")) | Out-Null;
        $DocElement.AppendChild($ResultXml.CreateTextNode("`n    ")) | Out-Null;
    }
    $ResultXml.DocumentElement.AppendChild($ResultXml.CreateTextNode("`n")) | Out-Null;
    $ClipboardText = "`n$ClipboardText$($ResultXml.OuterXml.Trim())"
}
[System.Windows.Clipboard]::SetText($ClipboardText);
<#
    <property>
    </property>
    <property>
    </property>
    <property>
    </property>
    <property>
    </property>
    <property>
    </property>
    <property>
    </property>
    <property>
    </property>
</CommentDoc><CommentDoc Name="Audio">
</CommentDoc><CommentDoc Name="Image">
    <property>
    </property>
    <property>
    </property>
    <property>
    </property>
    <property>
    </property>
    <property>
    </property>
    <property>
    </property>
    <property>
    </property>
    <property>
    </property>
    <property>
    </property>
    <property>
    </property>
    <property>
    </property>
</CommentDoc><CommentDoc Name="Media">
    <property>
    </property>
    <property>
    </property>
    <property>
    </property>
    <property>
    </property>
    <property>
    </property>
    <property>
    </property>
    <property>
    </property>
    <property>
    </property>
    <property>
    </property>
    <property>
    </property>
    <property>
    </property>
    <property>
    </property>
    <property>
    </property>
    <property>
    </property>
    <property>
    </property>
</CommentDoc><CommentDoc Name="Music">
    <property>
    </property>
    <property>
    </property>
    <property>
        /// <summary>
        /// Gets the Contributing Artist
        /// </summary>
        /// <value>
        /// System.
        /// </value>
        /// <remarks>
        /// Music.Artist
        /// <list type="bullet">
        /// <item><term>Name</term><description>Contributing Artist</description></item>
        /// <item><term>Format ID</term><description>{56A3372E-CE9C-11D2-9F0E-006097C686F6} (MUSIC)</description></item>
        /// <item><term>Property ID</term><description>2</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-artist">[Reference Link]</a></description></item>
        /// </list></remarks>
    </property>
    <property>
        /// <summary>
        /// Gets the Composer
        /// </summary>
        /// <value>
        /// System.
        /// </value>
        /// <remarks>
        /// Music.Composer
        /// <list type="bullet">
        /// <item><term>Name</term><description>Composer</description></item>
        /// <item><term>Format ID</term><description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>19</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-composer">[Reference Link]</a></description></item>
        /// </list></remarks>
    </property>
    <property>
        /// <summary>
        /// Gets the Conductor
        /// </summary>
        /// <value>
        /// System.
        /// </value>
        /// <remarks>
        /// Music.Conductor
        /// <list type="bullet">
        /// <item><term>Name</term><description>Conductor</description></item>
        /// <item><term>Format ID</term><description>{56A3372E-CE9C-11D2-9F0E-006097C686F6} (MUSIC)</description></item>
        /// <item><term>Property ID</term><description>36</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-conductor">[Reference Link]</a></description></item>
        /// </list></remarks>
    </property>
    <property>
        /// <summary>
        /// This property returns the best representation of Album Artist for a given music file based upon AlbumArtist, ContributingArtist and compilation info.
        /// </summary>
        /// <value>
        /// This property returns the best representation of the album artist for a specific music file based upon System.
        /// </value>
        /// <remarks>
        /// Music.AlbumArtist, System.Music.Artist, and System.Music.IsCompilation information.
        /// <list type="bullet">
        /// <item><term>Name</term><description>Display Artist</description></item>
        /// <item><term>Format ID</term><description>{FD122953-FA93-4EF7-92C3-04C946B2F7C8} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-displayartist">[Reference Link]</a></description></item>
        /// </list></remarks>
    </property>
    <property>
        /// <summary>
        /// Gets the Genre
        /// </summary>
        /// <value>
        /// System.
        /// </value>
        /// <remarks>
        /// Music.Genre
        /// <list type="bullet">
        /// <item><term>Name</term><description>Genre</description></item>
        /// <item><term>Format ID</term><description>{56A3372E-CE9C-11D2-9F0E-006097C686F6} (MUSIC)</description></item>
        /// <item><term>Property ID</term><description>11</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-genre">[Reference Link]</a></description></item>
        /// </list></remarks>
    </property>
    <property>
        /// <summary>
        /// Gets the Part of Set
        /// </summary>
        /// <value>
        /// System.
        /// </value>
        /// <remarks>
        /// Music.PartOfSet
        /// <list type="bullet">
        /// <item><term>Name</term><description>Part of Set</description></item>
        /// <item><term>Format ID</term><description>{56A3372E-CE9C-11D2-9F0E-006097C686F6} (MUSIC)</description></item>
        /// <item><term>Property ID</term><description>37</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-partofset">[Reference Link]</a></description></item>
        /// </list></remarks>
    </property>
    <property>
        /// <summary>
        /// Gets the Period
        /// </summary>
        /// <value>
        /// System.
        /// </value>
        /// <remarks>
        /// Music.Period
        /// <list type="bullet">
        /// <item><term>Name</term><description>Period</description></item>
        /// <item><term>Format ID</term><description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>31</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-period">[Reference Link]</a></description></item>
        /// </list></remarks>
    </property>
    <property>
        /// <summary>
        /// Gets the Track Number
        /// </summary>
        /// <value>
        /// System.
        /// </value>
        /// <remarks>
        /// Music.TrackNumber
        /// <list type="bullet">
        /// <item><term>Name</term><description>#</description></item>
        /// <item><term>Format ID</term><description>{56A3372E-CE9C-11D2-9F0E-006097C686F6} (MUSIC)</description></item>
        /// <item><term>Property ID</term><description>7</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-music-tracknumber">[Reference Link]</a></description></item>
        /// </list></remarks>
    </property>
</CommentDoc><CommentDoc Name="Photo">
    <property>
        /// <summary>
        /// Gets the Camera Manufacturer
        /// </summary>
        /// <value>
        /// The manufacturer name of the camera that took the photo, in a string format.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Camera Manufacturer</description></item>
        /// <item><term>Format ID</term><description>{14B81DA1-0135-4D31-96D9-6CBFC9671A99} (ImageProperties)</description></item>
        /// <item><term>Property ID</term><description>271</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-photo-cameramanufacturer">[Reference Link]</a></description></item>
        /// </list></remarks>
    </property>
    <property>
        /// <summary>
        /// Gets the Camera Model
        /// </summary>
        /// <value>
        /// The model name of the camera that shot the photo, in string form.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Camera Model</description></item>
        /// <item><term>Format ID</term><description>{14B81DA1-0135-4D31-96D9-6CBFC9671A99} (ImageProperties)</description></item>
        /// <item><term>Property ID</term><description>272</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-photo-cameramodel">[Reference Link]</a></description></item>
        /// </list></remarks>
    </property>
    <property>
        /// <summary>
        /// Gets the Date Taken
        /// </summary>
        /// <value>
        /// The date when the photo was taken, as read from the camera in the file's Exchangeable Image File (EXIF) tag.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Date Taken</description></item>
        /// <item><term>Format ID</term><description>{14B81DA1-0135-4D31-96D9-6CBFC9671A99} (ImageProperties)</description></item>
        /// <item><term>Property ID</term><description>36867</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-photo-datetaken">[Reference Link]</a></description></item>
        /// </list></remarks>
    </property>
    <property>
        /// <summary>
        /// Return the event at which the photo was taken
        /// </summary>
        /// <value>
        /// The event where the photo was taken.
        /// </value>
        /// <remarks>
        /// The end-user provides this value.
        /// <list type="bullet">
        /// <item><term>Name</term><description>Event Name</description></item>
        /// <item><term>Format ID</term><description>{14B81DA1-0135-4D31-96D9-6CBFC9671A99} (ImageProperties)</description></item>
        /// <item><term>Property ID</term><description>18248</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-photo-event">[Reference Link]</a></description></item>
        /// </list></remarks>
    </property>
    <property>
        /// <summary>
        /// Returns the EXIF version.
        /// </summary>
        /// <value>
        /// The Exchangeable Image File (EXIF) version.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>EXIF Version</description></item>
        /// <item><term>Format ID</term><description>{D35F743A-EB2E-47F2-A286-844132CB1427} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-photo-exifversion">[Reference Link]</a></description></item>
        /// </list></remarks>
    </property>
    <property>
        /// <summary>
        /// Gets the Orientation
        /// </summary>
        /// <value>
        /// The orientation of the photo when it was taken, as specified in the Exchangeable Image File (EXIF) information and in terms of rows and columns.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Orientation</description></item>
        /// <item><term>Format ID</term><description>{14B81DA1-0135-4D31-96D9-6CBFC9671A99} (ImageProperties)</description></item>
        /// <item><term>Property ID</term><description>274</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-photo-orientation">[Reference Link]</a></description></item>
        /// </list></remarks>
    </property>
    <property>
        /// <summary>
        /// The user-friendly form of System.Photo.Orientation
        /// </summary>
        /// <value>
        /// The user-friendly form of System.
        /// </value>
        /// <remarks>
        /// Photo.Orientation. Not intended to be parsed programmatically.
        /// <list type="bullet">
        /// <item><term>Name</term><description>Orientation</description></item>
        /// <item><term>Format ID</term><description>{A9EA193C-C511-498A-A06B-58E2776DCC28} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-photo-orientationtext">[Reference Link]</a></description></item>
        /// </list></remarks>
    </property>
    <property>
        /// <summary>
        /// The people tags on an image.
        /// </summary>
        /// <value>
        /// The people tags on an image.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>People Tags</description></item>
        /// <item><term>Format ID</term><description>{E8309B6E-084C-49B4-B1FC-90A80331B638} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-photo-peoplenames">[Reference Link]</a></description></item>
        /// </list></remarks>
    </property>
</CommentDoc><CommentDoc Name="RecordedTV">
    <property>
        /// <summary>
        /// Gets the Channel Number
        /// </summary>
        /// <value>
        /// Example: 42 The recorded TV channels.
        /// </value>
        /// <remarks>
        /// For example, 42, 5, 53.
        /// <list type="bullet">
        /// <item><term>Name</term><description>Channel Number</description></item>
        /// <item><term>Format ID</term><description>{6D748DE2-8D38-4CC3-AC60-F009B057C557} (Format)</description></item>
        /// <item><term>Property ID</term><description>7</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-recordedtv-channelnumber">[Reference Link]</a></description></item>
        /// </list></remarks>
    </property>
    <property>
        /// <summary>
        /// Gets the Episode Name
        /// </summary>
        /// <value>
        /// Example: "Nowhere to Hyde" The names of recorded TV episodes.
        /// </value>
        /// <remarks>
        /// For example, "Nowhere to Hyde".
        /// <list type="bullet">
        /// <item><term>Name</term><description>Episode Name</description></item>
        /// <item><term>Format ID</term><description>{6D748DE2-8D38-4CC3-AC60-F009B057C557} (Format)</description></item>
        /// <item><term>Property ID</term><description>2</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-recordedtv-episodename">[Reference Link]</a></description></item>
        /// </list></remarks>
    </property>
    <property>
        /// <summary>
        /// Indicates whether the video is DTV
        /// </summary>
        /// <value>
        /// System.
        /// </value>
        /// <remarks>
        /// RecordedTV.IsDTVContent
        /// <list type="bullet">
        /// <item><term>Name</term><description>Is DTV Content</description></item>
        /// <item><term>Format ID</term><description>{6D748DE2-8D38-4CC3-AC60-F009B057C557} (Format)</description></item>
        /// <item><term>Property ID</term><description>17</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-recordedtv-isdtvcontent">[Reference Link]</a></description></item>
        /// </list></remarks>
    </property>
    <property>
        /// <summary>
        /// Indicates whether the video is HDTV
        /// </summary>
        /// <value>
        /// System.
        /// </value>
        /// <remarks>
        /// RecordedTV.IsHDContent
        /// <list type="bullet">
        /// <item><term>Name</term><description>Is HDTV Content</description></item>
        /// <item><term>Format ID</term><description>{6D748DE2-8D38-4CC3-AC60-F009B057C557} (Format)</description></item>
        /// <item><term>Property ID</term><description>18</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-recordedtv-ishdcontent">[Reference Link]</a></description></item>
        /// </list></remarks>
    </property>
    <property>
        /// <summary>
        /// Gets the Network Affiliation
        /// </summary>
        /// <value>
        /// System.
        /// </value>
        /// <remarks>
        /// RecordedTV.NetworkAffiliation
        /// <list type="bullet">
        /// <item><term>Name</term><description>TV Network Affiliation</description></item>
        /// <item><term>Format ID</term><description>{2C53C813-FB63-4E22-A1AB-0B331CA1E273} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-recordedtv-networkaffiliation">[Reference Link]</a></description></item>
        /// </list></remarks>
    </property>
    <property>
        /// <summary>
        /// Gets the Original Broadcast Date
        /// </summary>
        /// <value>
        /// System.
        /// </value>
        /// <remarks>
        /// RecordedTV.OriginalBroadcastDate
        /// <list type="bullet">
        /// <item><term>Name</term><description>Original Broadcast Date</description></item>
        /// <item><term>Format ID</term><description>{4684FE97-8765-4842-9C13-F006447B178C} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-recordedtv-originalbroadcastdate">[Reference Link]</a></description></item>
        /// </list></remarks>
    </property>
    <property>
        /// <summary>
        /// Gets the Program Description
        /// </summary>
        /// <value>
        /// System.
        /// </value>
        /// <remarks>
        /// RecordedTV.ProgramDescription
        /// <list type="bullet">
        /// <item><term>Name</term><description>Program Description</description></item>
        /// <item><term>Format ID</term><description>{6D748DE2-8D38-4CC3-AC60-F009B057C557} (Format)</description></item>
        /// <item><term>Property ID</term><description>3</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-recordedtv-programdescription">[Reference Link]</a></description></item>
        /// </list></remarks>
    </property>
    <property>
        /// <summary>
        /// Gets the Station Call Sign
        /// </summary>
        /// <value>
        /// Example: "TOONP" Any recorded station call signs.
        /// </value>
        /// <remarks>
        /// For example, "TOONP".
        /// <list type="bullet">
        /// <item><term>Name</term><description>Station Call Sign</description></item>
        /// <item><term>Format ID</term><description>{6D748DE2-8D38-4CC3-AC60-F009B057C557} (Format)</description></item>
        /// <item><term>Property ID</term><description>5</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-recordedtv-stationcallsign">[Reference Link]</a></description></item>
        /// </list></remarks>
    </property>
    <property>
        /// <summary>
        /// Gets the Station Name
        /// </summary>
        /// <value>
        /// System.
        /// </value>
        /// <remarks>
        /// RecordedTV.StationName
        /// <list type="bullet">
        /// <item><term>Name</term><description>Station Name</description></item>
        /// <item><term>Format ID</term><description>{1B5439E7-EBA1-4AF8-BDD7-7AF1D4549493} (Format)</description></item>
        /// <item><term>Property ID</term><description>100</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-recordedtv-stationname">[Reference Link]</a></description></item>
        /// </list></remarks>
    </property>
</CommentDoc><CommentDoc Name="Software">
    <property>
        /// <summary>
        /// Gets the Product Name
        /// </summary>
        /// <value>
        /// System.
        /// </value>
        /// <remarks>
        /// Software.ProductName
        /// <list type="bullet">
        /// <item><term>Name</term><description>Product Name</description></item>
        /// <item><term>Format ID</term><description>{0CEF7D53-FA64-11D1-A203-0000F81FEDEE} (VERSION)</description></item>
        /// <item><term>Property ID</term><description>7</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-software-productname">[Reference Link]</a></description></item>
        /// </list></remarks>
    </property>
</CommentDoc><CommentDoc Name="Video">
    <property>
        /// <summary>
        /// Indicates the level of compression for the video stream.
        /// </summary>
        /// <value>
        /// Specifies the video compression format.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Compression</description></item>
        /// <item><term>Format ID</term><description>{64440491-4C8B-11D1-8B70-080036B11A03} (VideoSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>10</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-video-compression">[Reference Link]</a></description></item>
        /// </list></remarks>
    </property>
    <property>
        /// <summary>
        /// Gets the Director
        /// </summary>
        /// <value>
        /// Indicates the person who directed the video.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Director</description></item>
        /// <item><term>Format ID</term><description>{64440492-4C8B-11D1-8B70-080036B11A03} (MediaFileSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>20</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-video-director">[Reference Link]</a></description></item>
        /// </list></remarks>
    </property>
    <property>
        /// <summary>
        /// Indicates the data rate in "bits per second" for the video stream.
        /// </summary>
        /// <value>
        /// Indicates the data rate in "bits per second" for the video stream.
        /// </value>
        /// <remarks>
        /// "DataRate".
        /// <list type="bullet">
        /// <item><term>Name</term><description>Encoding Data Rate</description></item>
        /// <item><term>Format ID</term><description>{64440491-4C8B-11D1-8B70-080036B11A03} (VideoSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>8</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-video-encodingbitrate">[Reference Link]</a></description></item>
        /// </list></remarks>
    </property>
    <property>
        /// <summary>
        /// Indicates the frame height for the video stream.
        /// </summary>
        /// <value>
        /// Indicates the frame height for the video stream.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Frame Height</description></item>
        /// <item><term>Format ID</term><description>{64440491-4C8B-11D1-8B70-080036B11A03} (VideoSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>4</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-video-frameheight">[Reference Link]</a></description></item>
        /// </list></remarks>
    </property>
    <property>
        /// <summary>
        /// Indicates the frame rate in "frames per millisecond" for the video stream.
        /// </summary>
        /// <value>
        /// Indicates the frame rate for the video stream, in frames per 1000 seconds.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Frame Rate</description></item>
        /// <item><term>Format ID</term><description>{64440491-4C8B-11D1-8B70-080036B11A03} (VideoSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>6</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-video-framerate">[Reference Link]</a></description></item>
        /// </list></remarks>
    </property>
    <property>
        /// <summary>
        /// Indicates the frame width for the video stream.
        /// </summary>
        /// <value>
        /// Indicates the frame width for the video stream.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Frame Width</description></item>
        /// <item><term>Format ID</term><description>{64440491-4C8B-11D1-8B70-080036B11A03} (VideoSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>3</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-video-framewidth">[Reference Link]</a></description></item>
        /// </list></remarks>
    </property>
    <property>
        /// <summary>
        /// Indicates the horizontal portion of the aspect ratio.
        /// </summary>
        /// <value>
        /// The X portion of XX:YY, like 16:9.
        /// </value>
        /// <remarks>
        /// Indicates the horizontal portion of the pixel aspect ratio. The X portion of XX:YY. For example, 10 is the X portion of 10:11.
        /// <list type="bullet">
        /// <item><term>Name</term><description>Horizontal Aspect Ratio</description></item>
        /// <item><term>Format ID</term><description>{64440491-4C8B-11D1-8B70-080036B11A03} (VideoSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>42</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-video-horizontalaspectratio">[Reference Link]</a></description></item>
        /// </list></remarks>
    </property>
    <property>
        /// <summary>
        /// Indicates the name for the video stream.
        /// </summary>
        /// <value>
        /// Indicates the name for the video stream.
        /// </value>
        /// <remarks>
        /// "StreamName".
        /// <list type="bullet">
        /// <item><term>Name</term><description>Stream Name</description></item>
        /// <item><term>Format ID</term><description>{64440491-4C8B-11D1-8B70-080036B11A03} (VideoSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>2</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-video-streamname">[Reference Link]</a></description></item>
        /// </list></remarks>
    </property>
    <property>
        /// <summary>
        /// Gets the Stream Number
        /// </summary>
        /// <value>
        /// Indicates the ordinal number of the stream being played.
        /// </value>
        /// <remarks><list type="bullet">
        /// <item><term>Name</term><description>Stream Number</description></item>
        /// <item><term>Format ID</term><description>{64440491-4C8B-11D1-8B70-080036B11A03} (VideoSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>11</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-video-streamnumber">[Reference Link]</a></description></item>
        /// </list></remarks>
    </property>
    <property>
        /// <summary>
        /// Indicates the vertical portion of the aspect ratio
        /// </summary>
        /// <value>
        /// The Y portion of XX:YY, like 16:9.
        /// </value>
        /// <remarks>
        /// Indicates the horizontal portion of the pixel aspect ratio. The Y portion of XX:YY. For example, 11 is the Y portion of 10:11 .
        /// <list type="bullet">
        /// <item><term>Name</term><description>Vertical Aspect Ratio</description></item>
        /// <item><term>Format ID</term><description>{64440491-4C8B-11D1-8B70-080036B11A03} (VideoSummaryInformation)</description></item>
        /// <item><term>Property ID</term><description>45</description></item>
        /// <item><description><a href="https://docs.microsoft.com/en-us/windows/win32/properties/props-system-video-verticalaspectratio">[Reference Link]</a></description></item>
        /// </list></remarks>
    </property>
</CommentDoc><CommentDoc Name="Supplemental">
</CommentDoc><CommentDoc Name="ZipFolder">
</CommentDoc>
#>
<#
$XmlWriter = [System.Xml.XmlWriter]::Create($Path, [System.Xml.XmlWriterSettings]@{
    Indent = $true;
    Encoding = [System.Text.UTF8Encoding]::new($false, $false);
});
try {
    $Xml.WriteTo($XmlWriter);
    $XmlWriter.Flush();
} finally { $XmlWriter.Close() }
#>