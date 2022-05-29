# lib/netcoreapp3.1/Microsoft.WindowsAPICodePack.dll
# lib/netcoreapp3.1/Microsoft.WindowsAPICodePack.Shell.dll
Add-Type -Path "C:\Users\Lenny\.nuget\packages\microsoft-windowsapicodepack-core\1.1.4\lib\netcoreapp3.1\Microsoft.WindowsAPICodePack.dll" -ErrorAction Stop;
Add-Type -Path "C:\Users\Lenny\.nuget\packages\microsoft-windowsapicodepack-shell\1.1.4\lib\netcoreapp3.1\Microsoft.WindowsAPICodePack.Shell.dll" -ErrorAction Stop;

class PropertyValue {
    [string]$ColName;
    [string]$Value;
    PropertyValue([string]$ColName, [string]$Value) {
        $this.ColName = $ColName;
        $this.Value = $Value;
    }
}

Function ConvertTo-NormalizedString {
    Param(
        [Parameter(Mandatory = $true)]
        [AllowNull()]
        [AllowEmptyString()]
        [string]$Value
    )

    if ($null -eq $Value -or ($Value = $Value.Trim()).Length -eq 0) {
        '' | Write-Output;
    } else {
        ($Value -replace '[\s\r\n]+', '') | Write-Output;
    }
}

Function Format-MultiStringValue {
    Param(
        [Parameter(Mandatory = $true)]
        [AllowNull()]
        [AllowEmptyCollection()]
        [AllowEmptyString()]
        [string[]]$Values
    )

    if ($null -ne $Values -and $Values.Length -gt 0) {
        $NormalizedLines = @($Values | ForEach-Object { ConvertTo-NormalizedString -Value $_ } | Where-Object { $_.Length -gt 0 } | ForEach-Object { $_.Replace('`', '``') });
        if ($NormalizedLines.Count -gt 0) {
            ($NormalizedLines -join '`z') | Write-Output;
        }
    }
}

Function Get-SummaryProperties {
    Param(
        [Parameter(Mandatory = $true)]
        [Microsoft.WindowsAPICodePack.Shell.ShellFile]$ShellFile
    )
    $ShellPropertyCollection = $ShellFile.Properties.DefaultPropertyCollection;
    $SummaryProperties = $ShellFile.Properties.System;
    $SoftwareProperties = $ShellFile.Properties.System.Summary;

    if ($ShellPropertyCollection.Contains($SummaryProperties.ApplicationName.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($SummaryProperties.ApplicationName.Value)) {
        $Value = ConvertTo-NormalizedString -Value $SummaryProperties.ApplicationName.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('ApplicationName', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($SummaryProperties.Author.CanonicalName)) {
        $Value = Format-MultiStringValue -Values $SummaryProperties.Author.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('Author', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($SummaryProperties.Comment.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($SummaryProperties.Comment.Value)) {
        $Value = ConvertTo-NormalizedString -Value $SummaryProperties.Comment.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('Comment', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($SummaryProperties.Keywords.CanonicalName)) {
        $Value = Format-MultiStringValue -Values $SummaryProperties.Keywords.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('Keywords', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($SummaryProperties.Subject.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($SummaryProperties.Subject.Value)) {
        $Value = ConvertTo-NormalizedString -Value $SummaryProperties.Subject.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('Subject', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($SummaryProperties.Title.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($SummaryProperties.Title.Value)) {
        $Value = ConvertTo-NormalizedString -Value $SummaryProperties.Title.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('Title', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($SummaryProperties.FileDescription.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($SummaryProperties.FileDescription.Value)) {
        $Value = ConvertTo-NormalizedString -Value $SummaryProperties.FileDescription.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('FileDescription', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($SummaryProperties.FileVersion.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($SummaryProperties.FileVersion.Value)) {
        $Value = ConvertTo-NormalizedString -Value $SummaryProperties.FileVersion.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('FileVersion', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($SummaryProperties.Company.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($SummaryProperties.Company.Value)) {
        $Value = ConvertTo-NormalizedString -Value $SummaryProperties.Company.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('Company', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($SummaryProperties.ContentType.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($SummaryProperties.ContentType.Value)) {
        $Value = ConvertTo-NormalizedString -Value $SummaryProperties.ContentType.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('ContentType', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($SummaryProperties.Copyright.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($SummaryProperties.Copyright.Value)) {
        $Value = ConvertTo-NormalizedString -Value $SummaryProperties.Copyright.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('Copyright', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($SummaryProperties.ParentalRating.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($SummaryProperties.ParentalRating.Value)) {
        $Value = ConvertTo-NormalizedString -Value $SummaryProperties.ParentalRating.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('ParentalRating', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($SummaryProperties.Rating.CanonicalName) -and $null -ne $SummaryProperties.Rating.Value) {
        [PropertyValue]::new('Rating', $SummaryProperties.Rating.Value.ToString()) | Write-Output;
    }
    if ($ShellPropertyCollection.Contains($SummaryProperties.ItemAuthors.CanonicalName)) {
        $Value = Format-MultiStringValue -Values $SummaryProperties.ItemAuthors.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('ItemAuthors', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($SummaryProperties.ItemType.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($SummaryProperties.ItemType.Value)) {
        $Value = ConvertTo-NormalizedString -Value $SummaryProperties.ItemType.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('ItemType', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($SummaryProperties.ItemTypeText.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($SummaryProperties.ItemTypeText.Value)) {
        $Value = ConvertTo-NormalizedString -Value $SummaryProperties.ItemTypeText.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('ItemTypeText', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($SummaryProperties.Kind.CanonicalName)) {
        $Value = Format-MultiStringValue -Values $SummaryProperties.Kind.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('Kind', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($SummaryProperties.MIMEType.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($SummaryProperties.MIMEType.Value)) {
        $Value = ConvertTo-NormalizedString -Value $SummaryProperties.MIMEType.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('MIMEType', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($SummaryProperties.ParentalRatingReason.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($SummaryProperties.ParentalRatingReason.Value)) {
        $Value = ConvertTo-NormalizedString -Value $SummaryProperties.ParentalRatingReason.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('ParentalRatingReason', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($SummaryProperties.ParentalRatingsOrganization.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($SummaryProperties.ParentalRatingsOrganization.Value)) {
        $Value = ConvertTo-NormalizedString -Value $SummaryProperties.ParentalRatingsOrganization.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('ParentalRatingsOrganization', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($SummaryProperties.Sensitivity.CanonicalName) -and $null -ne $SummaryProperties.Sensitivity.Value) {
        [PropertyValue]::new('Sensitivity', $SummaryProperties.Rating.Value.ToString()) | Write-Output;
    }
    if ($ShellPropertyCollection.Contains($SummaryProperties.SensitivityText.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($SummaryProperties.SensitivityText.Value)) {
        $Value = ConvertTo-NormalizedString -Value $SummaryProperties.SensitivityText.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('SensitivityText', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($SummaryProperties.SimpleRating.CanonicalName) -and $null -ne $SummaryProperties.SimpleRating.Value) {
        [PropertyValue]::new('SimpleRating', $SummaryProperties.Rating.Value.ToString()) | Write-Output;
    }
    if ($ShellPropertyCollection.Contains($SummaryProperties.Trademarks.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($SummaryProperties.Trademarks.Value)) {
        $Value = ConvertTo-NormalizedString -Value $SummaryProperties.Trademarks.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('Trademarks', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($SoftwareProperties.ProductName.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($SoftwareProperties.ProductName.Value)) {
        $Value = ConvertTo-NormalizedString -Value $SoftwareProperties.ProductName.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('ProductName', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
}

Function Get-AudioProperties {
    Param(
        [Parameter(Mandatory = $true)]
        [Microsoft.WindowsAPICodePack.Shell.ShellFile]$ShellFile
    )
    $ShellPropertyCollection = $ShellFile.Properties.DefaultPropertyCollection;
    $AudioProperties = $ShellFile.Properties.System.Audio;
    if ($ShellPropertyCollection.Contains($AudioProperties.Compression.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($AudioProperties.Compression.Value)) {
        $Value = ConvertTo-NormalizedString -Value $AudioProperties.Compression.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('Compression', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($AudioProperties.EncodingBitrate.CanonicalName) -and $null -ne $AudioProperties.EncodingBitrate.Value) {
        [PropertyValue]::new('EncodingBitrate', $AudioProperties.EncodingBitrate.Value.ToString()) | Write-Output;
    }
    if ($ShellPropertyCollection.Contains($AudioProperties.Format.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($AudioProperties.Format.Value)) {
        $Value = ConvertTo-NormalizedString -Value $AudioProperties.Format.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('Format', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($AudioProperties.IsVariableBitrate.CanonicalName) -and $null -ne $AudioProperties.IsVariableBitrate.Value) {
        if ($AudioProperties.IsVariableBitrate.Value) {
            [PropertyValue]::new('IsVariableBitrate', '1') | Write-Output;
        } else {
            [PropertyValue]::new('IsVariableBitrate', '0') | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($AudioProperties.SampleRate.CanonicalName) -and $null -ne $AudioProperties.SampleRate.Value) {
        [PropertyValue]::new('SampleRate', $AudioProperties.SampleRate.Value.ToString()) | Write-Output;
    }
    if ($ShellPropertyCollection.Contains($AudioProperties.SampleSize.CanonicalName) -and $null -ne $AudioProperties.SampleSize.Value) {
        [PropertyValue]::new('SampleSize', $AudioProperties.SampleSize.Value.ToString()) | Write-Output;
    }
    if ($ShellPropertyCollection.Contains($AudioProperties.StreamName.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($AudioProperties.StreamName.Value)) {
        $Value = ConvertTo-NormalizedString -Value $AudioProperties.StreamName.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('StreamName', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($AudioProperties.StreamNumber.CanonicalName) -and $null -ne $AudioProperties.StreamNumber.Value) {
        [PropertyValue]::new('StreamNumber', $AudioProperties.StreamNumber.Value.ToString()) | Write-Output;
    }
}

Function Get-DocumentProperties {
    Param(
        [Parameter(Mandatory = $true)]
        [Microsoft.WindowsAPICodePack.Shell.ShellFile]$ShellFile,

        [Parameter(Mandatory = $true)]
        [DateTime]$CreationTime
    )
    $ShellPropertyCollection = $ShellFile.Properties.DefaultPropertyCollection;
    $DocumentProperties = $ShellFile.Properties.System.Document;

    if ($ShellPropertyCollection.Contains($DocumentProperties.ClientID.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($DocumentProperties.ClientID.Value)) {
        $Value = ConvertTo-NormalizedString -Value $DocumentProperties.ClientID.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('ClientID', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($DocumentProperties.Contributor.CanonicalName)) {
        $Value = Format-MultiStringValue -Values $DocumentProperties.Contributor.Value;
        if ($null -ne $Value) {
            [PropertyValue]::new('Contributor', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($DocumentProperties.DateCreated.CanonicalName) -and $null -ne $DocumentProperties.DateCreated.Value) {
        $DateCreated = $DocumentProperties.DateCreated.Value.ToString('yyyy-MM-dd HH:mm:ss');
        if ($DateCreated -ne $CreationTime.ToString('yyyy-MM-dd HH:mm:ss')) {
            [PropertyValue]::new('DateCreated', "'$DateCreated
            '") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($DocumentProperties.LastAuthor.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($DocumentProperties.LastAuthor.Value)) {
        $Value = ConvertTo-NormalizedString -Value $DocumentProperties.LastAuthor.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('LastAuthor', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($DocumentProperties.RevisionNumber.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($DocumentProperties.RevisionNumber.Value)) {
        $Value = ConvertTo-NormalizedString -Value $DocumentProperties.RevisionNumber.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('RevisionNumber', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($DocumentProperties.Security.CanonicalName) -and $null -ne $DocumentProperties.Security.Value) {
        [PropertyValue]::new('Security', $DocumentProperties.Security.Value.ToString()) | Write-Output;
    }
    if ($ShellPropertyCollection.Contains($DocumentProperties.Division.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($DocumentProperties.Division.Value)) {
        $Value = ConvertTo-NormalizedString -Value $DocumentProperties.Division.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('Division', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($DocumentProperties.DocumentID.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($DocumentProperties.DocumentID.Value)) {
        $Value = ConvertTo-NormalizedString -Value $DocumentProperties.DocumentID.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('DocumentID', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($DocumentProperties.Manager.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($DocumentProperties.Manager.Value)) {
        $Value = ConvertTo-NormalizedString -Value $DocumentProperties.Manager.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('Manager', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($DocumentProperties.PresentationFormat.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($DocumentProperties.PresentationFormat.Value)) {
        $Value = ConvertTo-NormalizedString -Value $DocumentProperties.PresentationFormat.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('PresentationFormat', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($DocumentProperties.Version.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($DocumentProperties.Version.Value)) {
        $Value = ConvertTo-NormalizedString -Value $DocumentProperties.Version.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('Version', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
}

Function Get-DRMProperties {
    Param(
        [Parameter(Mandatory = $true)]
        [Microsoft.WindowsAPICodePack.Shell.ShellFile]$ShellFile
    )
    $ShellPropertyCollection = $ShellFile.Properties.DefaultPropertyCollection;
    $DRMProperties = $ShellFile.Properties.System.DRM;

    if ($ShellPropertyCollection.Contains($DRMProperties.DatePlayExpires.CanonicalName) -and $null -ne $DRMProperties.DatePlayExpires.Value) {
        [PropertyValue]::new('DatePlayExpires', "'$($DRMProperties.DatePlayExpires.Value.ToString('yyyy-MM-dd HH:mm:ss'))'") | Write-Output;
    }
    if ($ShellPropertyCollection.Contains($DRMProperties.DatePlayStarts.CanonicalName) -and $null -ne $DRMProperties.DatePlayStarts.Value) {
        [PropertyValue]::new('DatePlayStarts', "'$($DRMProperties.DatePlayStarts.Value.ToString('yyyy-MM-dd HH:mm:ss'))'") | Write-Output;
    }
    if ($ShellPropertyCollection.Contains($DRMProperties.Description.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($DRMProperties.Description.Value)) {
        $Value = ConvertTo-NormalizedString -Value $DRMProperties.Description.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('Description', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($DRMProperties.IsProtected.CanonicalName) -and $null -ne $DRMProperties.IsProtected.Value) {
        if ($DRMProperties.IsProtected.Value) {
            [PropertyValue]::new('IsProtected', '1') | Write-Output;
        } else {
            [PropertyValue]::new('IsProtected', '0') | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($DRMProperties.PlayCount.CanonicalName) -and $null -ne $DRMProperties.PlayCount.Value) {
        [PropertyValue]::new('PlayCount', $DRMProperties.PlayCount.Value.ToString()) | Write-Output;
    }
}

Function Get-GPSProperties {
    Param(
        [Parameter(Mandatory = $true)]
        [Microsoft.WindowsAPICodePack.Shell.ShellFile]$ShellFile
    )
    $ShellPropertyCollection = $ShellFile.Properties.DefaultPropertyCollection;
    $GPSProperties = $ShellFile.Properties.System.GPS;

    if ($ShellPropertyCollection.Contains($GPSProperties.Latitude.CanonicalName) -and $null -ne $GPSProperties.Latitude.Value.Length -gt 0) {
        [PropertyValue]::new('LatitudeDegrees', $GPSProperties.Latitude.Value[0].ToString()) | Write-Output;
        if ($GPSProperties.Latitude.Value.Length -gt 1) {
            [PropertyValue]::new('LatitudeMinutes', $GPSProperties.Latitude.Value[1].ToString()) | Write-Output;
            if ($GPSProperties.Latitude.Value.Length -gt 2) {
                [PropertyValue]::new('LatitudeSeconds', $GPSProperties.Latitude.Value[2].ToString()) | Write-Output;
            }
        }
    }
    if ($ShellPropertyCollection.Contains($GPSProperties.LatitudeRef.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($GPSProperties.LatitudeRef.Value)) {
        $Value = ConvertTo-NormalizedString -Value $GPSProperties.LatitudeRef.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('LatitudeRef', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($GPSProperties.Longitude.CanonicalName) -and $null -ne $GPSProperties.Longitude.Value.Length -gt 0) {
        [PropertyValue]::new('LongitudeDegrees', $GPSProperties.Longitude.Value[0].ToString()) | Write-Output;
        if ($GPSProperties.Longitude.Value.Length -gt 1) {
            [PropertyValue]::new('LongitudeMinutes', $GPSProperties.Longitude.Value[1].ToString()) | Write-Output;
            if ($GPSProperties.Longitude.Value.Length -gt 2) {
                [PropertyValue]::new('LongitudeSeconds', $GPSProperties.Longitude.Value[2].ToString()) | Write-Output;
            }
        }
    }
    if ($ShellPropertyCollection.Contains($GPSProperties.LongitudeRef.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($GPSProperties.LongitudeRef.Value)) {
        $Value = ConvertTo-NormalizedString -Value $GPSProperties.LongitudeRef.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('LongitudeRef', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($GPSProperties.MeasureMode.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($GPSProperties.MeasureMode.Value)) {
        $Value = ConvertTo-NormalizedString -Value $GPSProperties.MeasureMode.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('MeasureMode', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($GPSProperties.ProcessingMethod.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($GPSProperties.ProcessingMethod.Value)) {
        $Value = ConvertTo-NormalizedString -Value $GPSProperties.ProcessingMethod.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('ProcessingMethod', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($GPSProperties.VersionID.CanonicalName) -and $null -ne $GPSProperties.VersionID.Value -and $GPSProperties.VersionID.Value.Length -gt 0) {
        [PropertyValue]::new('VersionID', "X'$(-join ($GPSProperties.VersionID.Value | ForEach-Object { $_.ToString('x2') }))'") | Write-Output;
    }
}

Function Get-ImageProperties {
    Param(
        [Parameter(Mandatory = $true)]
        [Microsoft.WindowsAPICodePack.Shell.ShellFile]$ShellFile
    )
    $ShellPropertyCollection = $ShellFile.Properties.DefaultPropertyCollection;
    $ImageProperties = $ShellFile.Properties.System.Image;

    if ($ShellPropertyCollection.Contains($ImageProperties.BitDepth.CanonicalName) -and $null -ne $ImageProperties.BitDepth.Value) {
        [PropertyValue]::new('BitDepth', $ImageProperties.BitDepth.Value.ToString()) | Write-Output;
    }
    if ($ShellPropertyCollection.Contains($ImageProperties.ColorSpace.CanonicalName) -and $null -ne $ImageProperties.ColorSpace.Value) {
        [PropertyValue]::new('ColorSpace', $ImageProperties.ColorSpace.Value.ToString()) | Write-Output;
    }
    if ($ShellPropertyCollection.Contains($ImageProperties.CompressedBitsPerPixel.CanonicalName) -and $null -ne $ImageProperties.CompressedBitsPerPixel.Value) {
        [PropertyValue]::new('CompressedBitsPerPixel', $ImageProperties.CompressedBitsPerPixel.Value.ToString()) | Write-Output;
    }
    if ($ShellPropertyCollection.Contains($ImageProperties.Compression.CanonicalName) -and $null -ne $ImageProperties.Compression.Value) {
        [PropertyValue]::new('Compression', $ImageProperties.Compression.Value.ToString()) | Write-Output;
    }
    if ($ShellPropertyCollection.Contains($ImageProperties.CompressionText.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($ImageProperties.CompressionText.Value)) {
        $Value = ConvertTo-NormalizedString -Value $ImageProperties.CompressionText.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('CompressionText', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($ImageProperties.HorizontalResolution.CanonicalName) -and $null -ne $ImageProperties.HorizontalResolution.Value) {
        [PropertyValue]::new('HorizontalResolution', $ImageProperties.HorizontalResolution.Value.ToString()) | Write-Output;
    }
    if ($ShellPropertyCollection.Contains($ImageProperties.HorizontalSize.CanonicalName) -and $null -ne $ImageProperties.HorizontalSize.Value) {
        [PropertyValue]::new('HorizontalSize', $ImageProperties.HorizontalSize.Value.ToString()) | Write-Output;
    }
    if ($ShellPropertyCollection.Contains($ImageProperties.ImageID.CanonicalName)) {
        $Value = ConvertTo-NormalizedString -Value $ImageProperties.ImageID.Value;
        if ($null -ne $Value) {
            [PropertyValue]::new('ImageID', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($ImageProperties.ResolutionUnit.CanonicalName) -and $null -ne $ImageProperties.ResolutionUnit.Value) {
        [PropertyValue]::new('ResolutionUnit', $ImageProperties.ResolutionUnit.Value.ToString()) | Write-Output;
    }
    if ($ShellPropertyCollection.Contains($ImageProperties.VerticalResolution.CanonicalName) -and $null -ne $ImageProperties.VerticalResolution.Value) {
        [PropertyValue]::new('VerticalResolution', $ImageProperties.VerticalResolution.Value.ToString()) | Write-Output;
    }
    if ($ShellPropertyCollection.Contains($ImageProperties.VerticalSize.CanonicalName) -and $null -ne $ImageProperties.VerticalSize.Value) {
        [PropertyValue]::new('VerticalSize', $ImageProperties.VerticalSize.Value.ToString()) | Write-Output;
    }
}

Function Get-MediaProperties {
    Param(
        [Parameter(Mandatory = $true)]
        [Microsoft.WindowsAPICodePack.Shell.ShellFile]$ShellFile
    )
    $ShellPropertyCollection = $ShellFile.Properties.DefaultPropertyCollection;
    $MediaProperties = $ShellFile.Properties.System.Media;

    if ($ShellPropertyCollection.Contains($MediaProperties.ContentDistributor.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($MediaProperties.ContentDistributor.Value)) {
        $Value = ConvertTo-NormalizedString -Value $MediaProperties.ContentDistributor.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('ContentDistributor', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($MediaProperties.CreatorApplication.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($MediaProperties.CreatorApplication.Value)) {
        $Value = ConvertTo-NormalizedString -Value $MediaProperties.CreatorApplication.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('CreatorApplication', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($MediaProperties.CreatorApplicationVersion.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($MediaProperties.CreatorApplicationVersion.Value)) {
        $Value = ConvertTo-NormalizedString -Value $MediaProperties.CreatorApplicationVersion.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('CreatorApplicationVersion', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($MediaProperties.DateReleased.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($MediaProperties.DateReleased.Value)) {
        $Value = ConvertTo-NormalizedString -Value $MediaProperties.DateReleased.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('DateReleased', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($MediaProperties.FrameCount.CanonicalName) -and $null -ne $MediaProperties.FrameCount.Value) {
        [PropertyValue]::new('FrameCount', $MediaProperties.FrameCount.Value.ToString()) | Write-Output;
    }
    if ($ShellPropertyCollection.Contains($MediaProperties.Producer.CanonicalName)) {
        $Value = Format-MultiStringValue -Values $MediaProperties.Producer.Value;
        if ($null -ne $Value) {
            [PropertyValue]::new('Producer', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($MediaProperties.ProtectionType.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($MediaProperties.ProtectionType.Value)) {
        $Value = ConvertTo-NormalizedString -Value $MediaProperties.ProtectionType.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('ProtectionType', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($MediaProperties.ProviderRating.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($MediaProperties.ProviderRating.Value)) {
        $Value = ConvertTo-NormalizedString -Value $MediaProperties.ProviderRating.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('ProviderRating', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($MediaProperties.ProviderStyle.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($MediaProperties.ProviderStyle.Value)) {
        $Value = ConvertTo-NormalizedString -Value $MediaProperties.ProviderStyle.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('ProviderStyle', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($MediaProperties.Publisher.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($MediaProperties.Publisher.Value)) {
        $Value = ConvertTo-NormalizedString -Value $MediaProperties.Publisher.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('Publisher', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($MediaProperties.Subtitle.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($MediaProperties.Subtitle.Value)) {
        $Value = ConvertTo-NormalizedString -Value $MediaProperties.Subtitle.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('Subtitle', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($MediaProperties.Writer.CanonicalName)) {
        $Value = Format-MultiStringValue -Values $MediaProperties.Writer.Value;
        if ($null -ne $Value) {
            [PropertyValue]::new('Writer', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($MediaProperties.Year.CanonicalName) -and $null -ne $MediaProperties.Year.Value) {
        [PropertyValue]::new('Year', $MediaProperties.Year.Value.ToString()) | Write-Output;
    }
}

Function Get-MusicProperties {
    Param(
        [Parameter(Mandatory = $true)]
        [Microsoft.WindowsAPICodePack.Shell.ShellFile]$ShellFile
    )
    $ShellPropertyCollection = $ShellFile.Properties.DefaultPropertyCollection;
    $MusicProperties = $ShellFile.Properties.System.Music;

    if ($ShellPropertyCollection.Contains($MusicProperties.AlbumArtist.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($MusicProperties.AlbumArtist.Value)) {
        $Value = ConvertTo-NormalizedString -Value $MusicProperties.AlbumArtist.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('AlbumArtist', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($MusicProperties.AlbumTitle.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($MusicProperties.AlbumTitle.Value)) {
        $Value = ConvertTo-NormalizedString -Value $MusicProperties.AlbumTitle.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('AlbumTitle', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($MusicProperties.Artist.CanonicalName)) {
        $Value = Format-MultiStringValue -Values $MusicProperties.Artist.Value;
        if ($null -ne $Value) {
            [PropertyValue]::new('Artist', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($MusicProperties.Composer.CanonicalName)) {
        $Value = Format-MultiStringValue -Values $MusicProperties.Composer.Value;
        if ($null -ne $Value) {
            [PropertyValue]::new('Composer', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($MusicProperties.Conductor.CanonicalName)) {
        $Value = Format-MultiStringValue -Values $MusicProperties.Conductor.Value;
        if ($null -ne $Value) {
            [PropertyValue]::new('Conductor', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($MusicProperties.DisplayArtist.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($MusicProperties.DisplayArtist.Value)) {
        $Value = ConvertTo-NormalizedString -Value $MusicProperties.DisplayArtist.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('DisplayArtist', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($MusicProperties.Genre.CanonicalName)) {
        $Value = Format-MultiStringValue -Values $MusicProperties.Genre.Value;
        if ($null -ne $Value) {
            [PropertyValue]::new('Genre', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($MusicProperties.PartOfSet.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($MusicProperties.PartOfSet.Value)) {
        $Value = ConvertTo-NormalizedString -Value $MusicProperties.PartOfSet.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('PartOfSet', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($MusicProperties.Period.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($MusicProperties.Period.Value)) {
        $Value = ConvertTo-NormalizedString -Value $MusicProperties.Period.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('Period', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($MusicProperties.TrackNumber.CanonicalName) -and $null -ne $MusicProperties.TrackNumber.Value) {
        [PropertyValue]::new('TrackNumber', $MusicProperties.TrackNumber.Value.ToString()) | Write-Output;
    }
}

Function Get-PhotoProperties {
    Param(
        [Parameter(Mandatory = $true)]
        [Microsoft.WindowsAPICodePack.Shell.ShellFile]$ShellFile
    )
    $ShellPropertyCollection = $ShellFile.Properties.DefaultPropertyCollection;
    $PhotoProperties = $ShellFile.Properties.System.Photo;

    if ($ShellPropertyCollection.Contains($PhotoProperties.CameraManufacturer.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($PhotoProperties.CameraManufacturer.Value)) {
        $Value = ConvertTo-NormalizedString -Value $PhotoProperties.CameraManufacturer.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('CameraManufacturer', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($PhotoProperties.CameraModel.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($PhotoProperties.CameraModel.Value)) {
        $Value = ConvertTo-NormalizedString -Value $PhotoProperties.CameraModel.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('CameraModel', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($PhotoProperties.DateTaken.CanonicalName) -and $null -ne $DocumentProperties.DateTaken.Value) {
        [PropertyValue]::new('DateTaken', "'$($PhotoProperties.DateTaken.Value.ToString('yyyy-MM-dd HH:mm:ss'))'") | Write-Output;
    }
    if ($ShellPropertyCollection.Contains($PhotoProperties.Event.CanonicalName)) {
        $Value = Format-MultiStringValue -Values $PhotoProperties.Event.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('Event', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($PhotoProperties.EXIFVersion.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($PhotoProperties.EXIFVersion.Value)) {
        $Value = ConvertTo-NormalizedString -Value $PhotoProperties.EXIFVersion.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('EXIFVersion', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($PhotoProperties.Orientation.CanonicalName) -and $null -ne $PhotoProperties.Orientation.Value) {
        [PropertyValue]::new('Orientation', $PhotoProperties.Orientation.Value.ToString()) | Write-Output;
    }
    if ($ShellPropertyCollection.Contains($PhotoProperties.OrientationText.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($PhotoProperties.OrientationText.Value)) {
        $Value = ConvertTo-NormalizedString -Value $PhotoProperties.OrientationText.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('OrientationText', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($PhotoProperties.PeopleNames.CanonicalName)) {
        $Value = Format-MultiStringValue -Values $PhotoProperties.PeopleNames.Value;
        if ($null -ne $Value) {
            [PropertyValue]::new('PeopleNames', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
}

Function Get-RecordedTVProperties {
    Param(
        [Parameter(Mandatory = $true)]
        [Microsoft.WindowsAPICodePack.Shell.ShellFile]$ShellFile
    )
    $ShellPropertyCollection = $ShellFile.Properties.DefaultPropertyCollection;
    $RecordedTVProperties = $ShellFile.Properties.System.RecordedTV;

    if ($ShellPropertyCollection.Contains($RecordedTVProperties.ChannelNumber.CanonicalName) -and $null -ne $RecordedTVProperties.ChannelNumber.Value) {
        [PropertyValue]::new('ChannelNumber', $RecordedTVProperties.ChannelNumber.Value.ToString()) | Write-Output;
    }
    if ($ShellPropertyCollection.Contains($RecordedTVProperties.EpisodeName.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($RecordedTVProperties.EpisodeName.Value)) {
        $Value = ConvertTo-NormalizedString -Value $RecordedTVProperties.EpisodeName.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('EpisodeName', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($RecordedTVProperties.IsDTVContent.CanonicalName) -and $null -ne $RecordedTVProperties.IsDTVContent.Value) {
        if ($RecordedTVProperties.IsDTVContent.Value) {
            [PropertyValue]::new('IsDTVContent', '1') | Write-Output;
        } else {
            [PropertyValue]::new('IsDTVContent', '0') | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($RecordedTVProperties.IsHDContent.CanonicalName) -and $null -ne $RecordedTVProperties.IsHDContent.Value) {
        if ($RecordedTVProperties.IsHDContent.Value) {
            [PropertyValue]::new('IsHDContent', '1') | Write-Output;
        } else {
            [PropertyValue]::new('IsHDContent', '0') | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($RecordedTVProperties.NetworkAffiliation.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($RecordedTVProperties.NetworkAffiliation.Value)) {
        $Value = ConvertTo-NormalizedString -Value $RecordedTVProperties.NetworkAffiliation.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('NetworkAffiliation', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($RecordedTVProperties.OriginalBroadcastDate.CanonicalName) -and $null -ne $DocumentProperties.OriginalBroadcastDate.Value) {
        [PropertyValue]::new('OriginalBroadcastDate', "'$($RecordedTVProperties.OriginalBroadcastDate.Value.ToString('yyyy-MM-dd HH:mm:ss'))'") | Write-Output;
    }
    if ($ShellPropertyCollection.Contains($RecordedTVProperties.ProgramDescription.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($RecordedTVProperties.ProgramDescription.Value)) {
        $Value = ConvertTo-NormalizedString -Value $RecordedTVProperties.ProgramDescription.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('ProgramDescription', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($RecordedTVProperties.StationCallSign.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($RecordedTVProperties.StationCallSign.Value)) {
        $Value = ConvertTo-NormalizedString -Value $RecordedTVProperties.StationCallSign.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('StationCallSign', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($RecordedTVProperties.StationName.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($RecordedTVProperties.StationName.Value)) {
        $Value = ConvertTo-NormalizedString -Value $RecordedTVProperties.StationName.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('StationName', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
}

Function Get-VideoProperties {
    Param(
        [Parameter(Mandatory = $true)]
        [Microsoft.WindowsAPICodePack.Shell.ShellFile]$ShellFile
    )
    $ShellPropertyCollection = $ShellFile.Properties.DefaultPropertyCollection;
    $VideoProperties = $ShellFile.Properties.System.Video;

    if ($ShellPropertyCollection.Contains($VideoProperties.Compression.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($VideoProperties.Compression.Value)) {
        $Value = ConvertTo-NormalizedString -Value $VideoProperties.Compression.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('Compression', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($VideoProperties.Director.CanonicalName)) {
        $Value = Format-MultiStringValue -Values $VideoProperties.Director.Value;
        if ($null -ne $Value) {
            [PropertyValue]::new('Director', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($VideoProperties.EncodingBitrate.CanonicalName) -and $null -ne $VideoProperties.EncodingBitrate.Value) {
        [PropertyValue]::new('EncodingBitrate', $VideoProperties.EncodingBitrate.Value.ToString()) | Write-Output;
    }
    if ($ShellPropertyCollection.Contains($VideoProperties.FrameHeight.CanonicalName) -and $null -ne $VideoProperties.FrameHeight.Value) {
        [PropertyValue]::new('FrameHeight', $VideoProperties.FrameHeight.Value.ToString()) | Write-Output;
    }
    if ($ShellPropertyCollection.Contains($VideoProperties.FrameRate.CanonicalName) -and $null -ne $VideoProperties.FrameRate.Value) {
        [PropertyValue]::new('FrameRate', $VideoProperties.FrameRate.Value.ToString()) | Write-Output;
    }
    if ($ShellPropertyCollection.Contains($VideoProperties.FrameWidth.CanonicalName) -and $null -ne $VideoProperties.FrameWidth.Value) {
        [PropertyValue]::new('FrameWidth', $VideoProperties.FrameWidth.Value.ToString()) | Write-Output;
    }
    if ($ShellPropertyCollection.Contains($VideoProperties.HorizontalAspectRatio.CanonicalName) -and $null -ne $VideoProperties.HorizontalAspectRatio.Value) {
        [PropertyValue]::new('HorizontalAspectRatio', $VideoProperties.HorizontalAspectRatio.Value.ToString()) | Write-Output;
    }
    if ($ShellPropertyCollection.Contains($VideoProperties.StreamName.CanonicalName) -and -not [string]::IsNullOrWhiteSpace($VideoProperties.StreamName.Value)) {
        $Value = ConvertTo-NormalizedString -Value $VideoProperties.StreamName.Value;
        if ($Value.Length -gt 0) {
            [PropertyValue]::new('StreamName', "'$($Value.Replace("'", "''"))'") | Write-Output;
        }
    }
    if ($ShellPropertyCollection.Contains($VideoProperties.StreamNumber.CanonicalName) -and $null -ne $VideoProperties.StreamNumber.Value) {
        [PropertyValue]::new('StreamNumber', $VideoProperties.StreamNumber.Value.ToString()) | Write-Output;
    }
    if ($ShellPropertyCollection.Contains($VideoProperties.VerticalAspectRatio.CanonicalName) -and $null -ne $VideoProperties.VerticalAspectRatio.Value) {
        [PropertyValue]::new('VerticalAspectRatio', $VideoProperties.VerticalAspectRatio.Value.ToString()) | Write-Output;
    }
}

$Script:SubdirectoryIdMap = @{};
$Script:Random = [Random]::new();

Function Import-Subdirectory {
    Param(
        [Parameter(Mandatory = $true)]
        [System.IO.DirectoryInfo]$DirectoryInfo,

        [Parameter(Mandatory = $true)]
        [System.IO.StringWriter]$Writer
    )

    if ($Script:SubdirectoryIdMap.ContainsKey($DirectoryInfo.FullName)) {
        $Script:SubdirectoryIdMap[$DirectoryInfo.FullName] | Write-Output;
    } else {
        $Id = [Guid]::NewGuid();
        $Script:SubdirectoryIdMap[$DirectoryInfo.FullName] = $Id;
        if ($null -eq $DirectoryInfo.Parent) {
            [System.Threading.Thread]::Sleep($Script:Random.Next(100, 1000));
            $Now = [DateTime]::Now.ToString('yyyy-MM-dd HH:mm:ss');
            $Writer.WriteLine('INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")');
            $Writer.WriteLine("    VALUES ('$Id', '$($DirectoryInfo.Name.Replace("'", "''") -replace '\\$', '')', '$Now', '$($DirectoryInfo.CreationTime.ToString('yyyy-MM-dd HH:mm:ss'))', '$($DirectoryInfo.LastWriteTime.ToString('yyyy-MM-dd HH:mm:ss'))', NULL, 'dfe10650-2f3a-48ee-ba9c-8777878e7850', '$Now', '$Now');");
        } else {
            $ParentId = Import-Subdirectory -DirectoryInfo $DirectoryInfo.Parent -Writer $Writer;
            [System.Threading.Thread]::Sleep($Script:Random.Next(100, 1000));
            $Now = [DateTime]::Now.ToString('yyyy-MM-dd HH:mm:ss');
            $Writer.WriteLine('INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")');
            $Writer.WriteLine("    VALUES ('$Id', '$($DirectoryInfo.Name.Replace("'", "''") -replace '\\$', '')', '$Now', '$($DirectoryInfo.CreationTime.ToString('yyyy-MM-dd HH:mm:ss'))', '$($DirectoryInfo.LastWriteTime.ToString('yyyy-MM-dd HH:mm:ss'))', '$ParentId', NULL, '$Now', '$Now');");
        }
        $Id | Write-Output;
    }
}

Function Get-FileProperties {
    Param(
        [Parameter(Mandatory = $true)]
        [System.IO.FileInfo]$FileInfo,

        [Parameter(Mandatory = $true)]
        [System.IO.StringWriter]$Writer
    )

    $ShellFile = [Microsoft.WindowsAPICodePack.Shell.ShellFile]::FromFilePath($FileInfo.FullName);
    [PropertyValue]::new('Name', "'$($FileInfo.Name.Replace("'", "''"))'") | Write-Output;
    [PropertyValue]::new('CreationTime', "`"$($FileInfo.CreationTime.ToString('yyyy-MM-dd HH:mm:ss'))`"") | Write-Output;
    [PropertyValue]::new('LastWriteTime', "`"$($FileInfo.LastWriteTime.ToString('yyyy-MM-dd HH:mm:ss'))`"") | Write-Output;

    $LinkedList = [System.Collections.Generic.LinkedList[System.IO.DirectoryInfo]]::new();
    for ($DirectoryInfo = $FileInfo.Directory; $null -ne $DirectoryInfo; $DirectoryInfo = $DirectoryInfo.Parent) {
        $LinkedList.AddFirst($DirectoryInfo) | Out-Null;
    }
    $ParentId = Import-Subdirectory -DirectoryInfo $FileInfo.Directory -Writer $Writer;
    [PropertyValue]::new('ParentId', "'$ParentId'") | Write-Output;
    $BinaryPropertiesId = [Guid]::NewGuid().ToString();
    Write-Progress -Activity 'Process File' -Status $FileInfo.FullName -CurrentOperation 'Calculating hash';
    $Stream = $FileInfo.Open([System.IO.FileMode]::Open);
    try {
        $MD5 = [System.Security.Cryptography.MD5]::Create();
        $Bytes = $MD5.ComputeHash($Stream);
        $Hash = -join ($Bytes | % { $_.ToString('x2') });
    } finally { $Stream.Close() }
    $Now = [DateTime]::Now.ToString('yyyy-MM-dd HH:mm:ss');
    [System.Threading.Thread]::Sleep($Script:Random.Next(100, 1000));
    $Writer.WriteLine('INSERT INTO "BinaryPropertySets" ("Id", "Length", "Hash", "CreatedOn", "ModifiedOn")');
    $Writer.WriteLine("    VALUES('$BinaryPropertiesId', $($FileInfo.Length), X'$Hash', '$Now', '$Now');");
    [PropertyValue]::new('BinaryPropertySetId', "'$BinaryPropertiesId'") | Write-Output;
    [PropertyValue]::new('LastHashCalculation', "'$Now'") | Write-Output;

    Write-Progress -Activity 'Process File' -Status $FileInfo.FullName -CurrentOperation 'Reading summary properties';
    $Values = @(Get-SummaryProperties -ShellFile $ShellFile);
    if ($Values.Count -gt 0) {
        $Id = [Guid]::NewGuid();
        $Now = [DateTime]::Now.ToString('yyyy-MM-dd HH:mm:ss');
        [System.Threading.Thread]::Sleep($Script:Random.Next(100, 1000));
        $Writer.WriteLine("INSERT INTO `"SummaryPropertySets`" (`"Id`", `"$(($Values | ForEach-Object { $_.ColName }) -join '", "')`", `"CreatedOn`", `"ModifiedOn`")");
        $Writer.WriteLine("    VALUES('$Id', $(($Values | ForEach-Object { $_.Value }) -join ', '), '$Now', '$Now');");
        [PropertyValue]::new('SummaryPropertySetId', "'$Id'") | Write-Output;
    }

    Write-Progress -Activity 'Process File' -Status $FileInfo.FullName -CurrentOperation 'Reading audio properties';
    $Values = @(Get-AudioProperties -ShellFile $ShellFile);
    if ($Values.Count -gt 0) {
        $Id = [Guid]::NewGuid();
        $Now = [DateTime]::Now.ToString('yyyy-MM-dd HH:mm:ss');
        [System.Threading.Thread]::Sleep($Script:Random.Next(100, 1000));
        $Writer.WriteLine("INSERT INTO `"AudioPropertySets`" (`"Id`", `"$(($Values | ForEach-Object { $_.ColName }) -join '", "')`", `"CreatedOn`", `"ModifiedOn`")");
        $Writer.WriteLine("    VALUES('$Id', $(($Values | ForEach-Object { $_.Value }) -join ', '), '$Now', '$Now');");
        [PropertyValue]::new('AudioPropertySetId', "'$Id'") | Write-Output;
    }

    Write-Progress -Activity 'Process File' -Status $FileInfo.FullName -CurrentOperation 'Reading document properties';
    $Values = @(Get-DocumentProperties -ShellFile $ShellFile -CreationTime $FileInfo.CreationTime);
    if ($Values.Count -gt 0) {

        $Id = [Guid]::NewGuid();
        $Now = [DateTime]::Now.ToString('yyyy-MM-dd HH:mm:ss');
        [System.Threading.Thread]::Sleep($Script:Random.Next(100, 1000));
        $Writer.WriteLine("INSERT INTO `"DocumentPropertySets`" (`"Id`", `"$(($Values | ForEach-Object { $_.ColName }) -join '", "')`", `"CreatedOn`", `"ModifiedOn`")");
        $Writer.WriteLine("    VALUES('$Id', $(($Values | ForEach-Object { $_.Value }) -join ', '), '$Now', '$Now');");
        [PropertyValue]::new('DocumentPropertySetId', "'$Id'") | Write-Output;
    }

    Write-Progress -Activity 'Process File' -Status $FileInfo.FullName -CurrentOperation 'Reading DRM properties';
    $Values = @(Get-DRMProperties -ShellFile $ShellFile);
    if ($Values.Count -gt 0) {
        $Id = [Guid]::NewGuid();
        $Now = [DateTime]::Now.ToString('yyyy-MM-dd HH:mm:ss');
        [System.Threading.Thread]::Sleep($Script:Random.Next(100, 1000));
        $Writer.WriteLine("INSERT INTO `"DRMPropertySets`" (`"Id`", `"$(($Values | ForEach-Object { $_.ColName }) -join '", "')`", `"CreatedOn`", `"ModifiedOn`")");
        $Writer.WriteLine("    VALUES('$Id', $(($Values | ForEach-Object { $_.Value }) -join ', '), '$Now', '$Now');");
        [PropertyValue]::new('DRMPropertySetId', "'$Id'") | Write-Output;
    }

    Write-Progress -Activity 'Process File' -Status $FileInfo.FullName -CurrentOperation 'Reading GPS properties';
    $Values = @(Get-GPSProperties -ShellFile $ShellFile);
    if ($Values.Count -gt 0) {
        $Id = [Guid]::NewGuid();
        $Now = [DateTime]::Now.ToString('yyyy-MM-dd HH:mm:ss');
        [System.Threading.Thread]::Sleep($Script:Random.Next(100, 1000));
        $Writer.WriteLine("INSERT INTO `"GPSPropertySets`" (`"Id`", `"$(($Values | ForEach-Object { $_.ColName }) -join '", "')`", `"CreatedOn`", `"ModifiedOn`")");
        $Writer.WriteLine("    VALUES('$Id', $(($Values | ForEach-Object { $_.Value }) -join ', '), '$Now', '$Now');");
        [PropertyValue]::new('GPSPropertySetId', "'$Id'") | Write-Output;
    }

    Write-Progress -Activity 'Process File' -Status $FileInfo.FullName -CurrentOperation 'Reading image properties';
    $Values = @(Get-ImageProperties -ShellFile $ShellFile);
    if ($Values.Count -gt 0) {
        $Id = [Guid]::NewGuid();
        $Now = [DateTime]::Now.ToString('yyyy-MM-dd HH:mm:ss');
        [System.Threading.Thread]::Sleep($Script:Random.Next(100, 1000));
        $Writer.WriteLine("INSERT INTO `"ImagePropertySets`" (`"Id`", `"$(($Values | ForEach-Object { $_.ColName }) -join '", "')`", `"CreatedOn`", `"ModifiedOn`")");
        $Writer.WriteLine("    VALUES('$Id', $(($Values | ForEach-Object { $_.Value }) -join ', '), '$Now', '$Now');");
        [PropertyValue]::new('ImagePropertySetId', "'$Id'") | Write-Output;
    }

    Write-Progress -Activity 'Process File' -Status $FileInfo.FullName -CurrentOperation 'Reading media properties';
    $Values = @(Get-MediaProperties -ShellFile $ShellFile);
    if ($Values.Count -gt 0) {
        $Id = [Guid]::NewGuid();
        $Now = [DateTime]::Now.ToString('yyyy-MM-dd HH:mm:ss');
        [System.Threading.Thread]::Sleep($Script:Random.Next(100, 1000));
        $Writer.WriteLine("INSERT INTO `"MediaPropertySets`" (`"Id`", `"$(($Values | ForEach-Object { $_.ColName }) -join '", "')`", `"CreatedOn`", `"ModifiedOn`")");
        $Writer.WriteLine("    VALUES('$Id', $(($Values | ForEach-Object { $_.Value }) -join ', '), '$Now', '$Now');");
        [PropertyValue]::new('MediaPropertySetId', "'$Id'") | Write-Output;
    }

    Write-Progress -Activity 'Process File' -Status $FileInfo.FullName -CurrentOperation 'Reading music properties';
    $Values = @(Get-MusicProperties -ShellFile $ShellFile);
    if ($Values.Count -gt 0) {
        $Id = [Guid]::NewGuid();
        $Now = [DateTime]::Now.ToString('yyyy-MM-dd HH:mm:ss');
        [System.Threading.Thread]::Sleep($Script:Random.Next(100, 1000));
        $Writer.WriteLine("INSERT INTO `"MusicPropertySets`" (`"Id`", `"$(($Values | ForEach-Object { $_.ColName }) -join '", "')`", `"CreatedOn`", `"ModifiedOn`")");
        $Writer.WriteLine("    VALUES('$Id', $(($Values | ForEach-Object { $_.Value }) -join ', '), '$Now', '$Now');");
        [PropertyValue]::new('MusicPropertySetId', "'$Id'") | Write-Output;
    }

    Write-Progress -Activity 'Process File' -Status $FileInfo.FullName -CurrentOperation 'Reading photo properties';
    $Values = @(Get-PhotoProperties -ShellFile $ShellFile);
    if ($Values.Count -gt 0) {
        $Id = [Guid]::NewGuid();
        $Now = [DateTime]::Now.ToString('yyyy-MM-dd HH:mm:ss');
        [System.Threading.Thread]::Sleep($Script:Random.Next(100, 1000));
        $Writer.WriteLine("INSERT INTO `"PhotoPropertySets`" (`"Id`", `"$(($Values | ForEach-Object { $_.ColName }) -join '", "')`", `"CreatedOn`", `"ModifiedOn`")");
        $Writer.WriteLine("    VALUES('$Id', $(($Values | ForEach-Object { $_.Value }) -join ', '), '$Now', '$Now');");
        [PropertyValue]::new('PhotoPropertySetId', "'$Id'") | Write-Output;
    }

    Write-Progress -Activity 'Process File' -Status $FileInfo.FullName -CurrentOperation 'Reading recorded TV properties';
    $Values = @(Get-RecordedTVProperties -ShellFile $ShellFile);
    if ($Values.Count -gt 0) {
        $Id = [Guid]::NewGuid();
        $Now = [DateTime]::Now.ToString('yyyy-MM-dd HH:mm:ss');
        [System.Threading.Thread]::Sleep($Script:Random.Next(100, 1000));
        $Writer.WriteLine("INSERT INTO `"RecordedTVPropertySets`" (`"Id`", `"$(($Values | ForEach-Object { $_.ColName }) -join '", "')`", `"CreatedOn`", `"ModifiedOn`")");
        $Writer.WriteLine("    VALUES('$Id', $(($Values | ForEach-Object { $_.Value }) -join ', '), '$Now', '$Now');");
        [PropertyValue]::new('RecordedTVPropertySetId', "'$Id'") | Write-Output;
    }

    Write-Progress -Activity 'Process File' -Status $FileInfo.FullName -CurrentOperation 'Reading video properties';
    $Values = @(Get-VideoProperties -ShellFile $ShellFile);
    if ($Values.Count -gt 0) {
        $Id = [Guid]::NewGuid();
        $Now = [DateTime]::Now.ToString('yyyy-MM-dd HH:mm:ss');
        [System.Threading.Thread]::Sleep($Script:Random.Next(100, 1000));
        $Writer.WriteLine("INSERT INTO `"VideoPropertySets`" (`"Id`", `"$(($Values | ForEach-Object { $_.ColName }) -join '", "')`", `"CreatedOn`", `"ModifiedOn`")");
        $Writer.WriteLine("    VALUES('$Id', $(($Values | ForEach-Object { $_.Value }) -join ', '), '$Now', '$Now');");
        [PropertyValue]::new('VideoPropertySetId', "'$Id'") | Write-Output;
    }
}

Function Import-File {
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [string]$LiteralPath,

        [Parameter(Mandatory = $true)]
        [System.IO.StringWriter]$Writer
    )

    Process {
        $FileInfo = [System.IO.FileInfo]::new($LiteralPath);
        Write-Progress -Activity 'Process Files' -Status $FileInfo.FullName -CurrentOperation 'Loading file';
        $FilePropertyValues = @(Get-FileProperties -FileInfo $FileInfo -Writer $Writer);
        $Now = [DateTime]::Now.ToString('yyyy-MM-dd HH:mm:ss');
        [System.Threading.Thread]::Sleep($Script:Random.Next(100, 1000));
        $Writer.WriteLine("INSERT INTO `"Files`" (`"Id`", `"$(($FilePropertyValues | ForEach-Object { $_.ColName }) -join '", "')`", `"CreatedOn`", `"ModifiedOn`")");
        $Writer.WriteLine("    VALUES('$([Guid]::NewGuid())', $(($FilePropertyValues | ForEach-Object { $_.Value }) -join ', '), '$Now', '$Now');");
    }
}

$Writer = [System.IO.StringWriter]::new();
$DirectoryId = Import-Subdirectory -DirectoryInfo ([System.IO.DirectoryInfo]::new('C:\Users\Lenny')) -Writer $Writer;

[System.Threading.Thread]::Sleep($Random.Next(1000, 10000));
$Now = [DateTime]::Now.ToString('yyyy-MM-dd HH:mm:ss');
$CrawlConfigurationId = [Guid]::NewGuid().ToString();
$Writer.WriteLine('INSERT INTO "CrawlConfigurations" ("Id", "DisplayName", "MaxRecursionDepth", "RootId", "StatusValue", "CreatedOn", "ModifiedOn")');
$Writer.WriteLine("    VALUES ('$CrawlConfigurationId', 'Personal folder', 256, '$DirectoryId', 0,	'$Now', '$Now');");
[System.Threading.Thread]::Sleep($Random.Next(1000, 10000));
$Now = [DateTime]::Now.ToString('yyyy-MM-dd HH:mm:ss');

$Writer.WriteLine('INSERT INTO "CrawlJobLogs" ("Id", "MaxRecursionDepth", "RootPath", "StatusCode", "CrawlStart", "CrawlEnd", "StatusMessage", "StatusDetail", "FoldersProcessed", "FilesProcessed", "CreatedOn", "ModifiedOn", "ConfigurationId")');
$Writer.WriteLine("    VALUES ('$([Guid]::NewGuid())', 256, 'C:\Users\Lenny', 2, '$Now', '$Now', '', '', 22, 18, '$Now', '$Now', '$CrawlConfigurationId');");
@(
    "C:\Users\Lenny\Downloads\6755PsSg.mp4",
    "C:\Users\Lenny\Downloads\Jennet Redhead cat.mp4",
    "C:\Users\Lenny\Downloads\ClientTransactionDetails.xls",
    "C:\Users\Lenny\Downloads\VdhCoAppSetup-1.6.3.exe",
    "C:\Users\Lenny\Dropbox\Cruise2019\05-11-2019\iCloud Photos\57926728061__71DEA427-4227-4B5F-97C9-3026A51827D5.JPG",
    "C:\Users\Lenny\Music\Eldorado\08. ILLUSIONS_IN_G_MAJOR.WAV",
    "C:\Users\Lenny\Music\Jeff Lynne's ELO - Alone In The Universe [2015] [MP3-VBR] [H4CKUS] [GloDLS]\01-jeff_lynnes_elo-when_i_was_a_boy.mp3",
    "C:\Users\Lenny\Music\Jeff Lynne's ELO - Alone In The Universe [2015] [MP3-VBR] [H4CKUS] [GloDLS]\06-jeff_lynnes_elo-aint_it_a_drag.mp3",
    "C:\Users\Lenny\OneDrive\Documents\Custom Office Templates\APA Essay.dotm",
    "C:\Users\Lenny\OneDrive\Music\Help! [UK]\The Beatles - I've Just Seen a Face.mp3",
    "C:\Users\Lenny\OneDrive\Music\John Denver\Rocky Mountain High.mp3"
    "C:\Users\Lenny\OneDrive\Music\Smashmouth\Smash Mouth - Walkin- On The Sun.mp3",
    "C:\Users\Lenny\OneDrive\Documents\My Shapes\Favorites.vssx",
    "C:\Users\Lenny\OneDrive\Documents\Work\GEN-002_GDIT_ISD_Herndon_Letterhead_13857_McLearen_Rd.docx",
    "C:\Users\Lenny\OneDrive\Documents\Work\Old NGIC contacts.csv",
    "C:\Users\Lenny\OneDrive\Documents\Work\P2SChildCertWork.pfx",
    "C:\Users\Lenny\Videos\2015-01\2301-175155.3gp",
    "C:\Users\Lenny\Videos\2015-01\2301-180239.3gp"
) | Import-File -Writer $Writer;
Write-Progress -Activity 'Process File' -Status 'Generated SQL' -Completed;
$Writer.ToString();
[DateTime]::Now.ToString('yyyy-MM-dd HH:mm:ss');
