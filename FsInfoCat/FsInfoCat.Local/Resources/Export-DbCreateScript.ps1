Param(
    [bool]$NoDropTables = $false,
    [string]$ImportPath = '../../FsInfoCat.UnitTests/Resources/TestData.xml'
)

Function Export-AccessErrorElement {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.XmlElement]$SourceElement,
        
        [Parameter(Mandatory = $true)]
        [System.IO.StreamWriter]$StreamWriter,
        
        [Parameter(Mandatory = $true)]
        [string]$TargetId,
        
        [Parameter(Mandatory = $true)]
        [ValidateSet('Volume', 'Subdirectory', 'File')]
        [string]$Type
    )
    
    Process {
        $StreamWriter.Write("INSERT INTO `"$($Type)AccessErrors`" (`"Id`", `"Message`"");
        if (-not $SourceElement.IsEmpty) { $StreamWriter.Write(', "Details"') }
        $StreamWriter.WriteLine(', "ErrorCode", "TargetId", "CreatedOn", "ModifiedOn")');
        $StreamWriter.Write("`tVALUES ('$($SourceElement.Id)', '$($SourceElement.Message.Replace("'", "''"))'");
        if (-not $SourceElement.IsEmpty) { $StreamWriter.Write(", '$($SourceElement.InnerText.Replace("'", "''"))'") }
        if ($null -ne $SourceElement.ErrorCode) {
            switch ($SourceElement.ErrorCode) {
                'UnexpectedError' { $StreamWriter.Write(', 0'); break; }
                'ReadError' { $StreamWriter.Write(', 1'); break; }
                'OpenError' { $StreamWriter.Write(', 2'); break; }
                default { $StreamWriter.Write(", $($SourceElement.DefaultDriveType)"); break; }
            }
        } else {
             $StreamWriter.Write(', 0');
        }
        $StreamWriter.Write(", '$($SourceElement.TargetId)'");
        $ModifiedOn = $CreatedOn = [System.Xml.XmlConvert]::ToDateTime($SourceElement.CreatedOn, [System.Xml.XmlDateTimeSerializationMode]::RoundtripKind);
        if ($null -ne $SourceElement.ModifiedOn) {
            $ModifiedOn = [System.Xml.XmlConvert]::ToDateTime($SourceElement.ModifiedOn, [System.Xml.XmlDateTimeSerializationMode]::RoundtripKind);
        }
        $StreamWriter.Write(", '$($CreatedOn.ToString('yyyy-MM-dd HH:mm:ss'))'");
        $StreamWriter.WriteLine(", '$($ModifiedOn.ToString('yyyy-MM-dd HH:mm:ss'))');");
    }
}

Function Export-VolumeElement {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.XmlElement]$SourceElement,
        
        [Parameter(Mandatory = $true)]
        [System.Xml.XmlNamespaceManager]$Nsmgr,

        [Parameter(Mandatory = $true)]
        [System.IO.StreamWriter]$StreamWriter,
        
        [Parameter(Mandatory = $true)]
        [string]$FileSystemId
    )
    
    Process {
        $StreamWriter.Write('INSERT INTO "Volumes" ("Id", "DisplayName", "VolumeName", "Identifier", "FileSystemId"');
        @('CaseSensitiveSearch', 'ReadOnly', 'MaxNameLength', 'Type', 'Notes', 'Status', 'UpstreamId', 'LastSynchronizedOn') | ForEach-Object {
            if ($null -ne $SourceElement.$_) { $StreamWriter.Write(", `"$_`"") }
        }
        $StreamWriter.WriteLine(', "CreatedOn", "ModifiedOn")');
        $VolumeName = $SourceElement.VolumeName;
        if ([string]::IsNullOrWhiteSpace($VolumeName)) { $VolumeName = '' }
        $StreamWriter.Write("`tVALUES ('$($SourceElement.Id)', '$($SourceElement.DisplayName.Replace("'", "''"))', '$VolumeName', '$($SourceElement.Identifier.Replace("'", "''"))', '$FileSystemId'");
        if ($null -ne $SourceElement.CaseSensitiveSearch) {
            if ([string]::IsNullOrWhiteSpace($SourceElement.CaseSensitiveSearch)) {
                $StreamWriter.Write(', NULL');
            } else {
                if ([System.Xml.XmlConvert]::ToBoolean($SourceElement.CaseSensitiveSearch)) { $StreamWriter.Write(', 1') } else { $StreamWriter.Write(', 0') }
            }
        }
        if ($null -ne $SourceElement.ReadOnly) {
            if ([string]::IsNullOrWhiteSpace($SourceElement.ReadOnly)) {
                $StreamWriter.Write(', NULL');
            } else {
                if ([System.Xml.XmlConvert]::ToBoolean($SourceElement.ReadOnly)) { $StreamWriter.Write(', 1') } else { $StreamWriter.Write(', 0') }
            }
        }
        if ($null -ne $SourceElement.MaxNameLength) {
            if ([string]::IsNullOrWhiteSpace($SourceElement.MaxNameLength)) {
                $StreamWriter.Write(', NULL');
            } else {
                $StreamWriter.Write(", $($SourceElement.MaxNameLength)");
            }
        }
        if ($null -ne $SourceElement.Type) {
            switch ($SourceElement.Type) {
                'Unknown' { $StreamWriter.Write(', 0'); break; }
                'NoRootDirectory' { $StreamWriter.Write(', 1'); break; }
                'Removable' { $StreamWriter.Write(', 2'); break; }
                'Fixed' { $StreamWriter.Write(', 3'); break; }
                'Network' { $StreamWriter.Write(', 4'); break; }
                'CDRom' { $StreamWriter.Write(', 5'); break; }
                'Ram' { $StreamWriter.Write(', 6'); break; }
                default { $StreamWriter.Write(", $($SourceElement.DefaultDriveType)"); break; }
            }
        }
        if ($null -ne $SourceElement.Notes) { $StreamWriter.Write(", '$($SourceElement.Notes.Replace("'", "''"))'") }
        if ($null -ne $SourceElement.Status) {
            switch ($SourceElement.Status) {
                'Unknown' { $StreamWriter.Write(', 0'); break; }
                'Controlled' { $StreamWriter.Write(', 1'); break; }
                'AccessError' { $StreamWriter.Write(', 2'); break; }
                'Offline' { $StreamWriter.Write(', 3'); break; }
                'Uncontrolled' { $StreamWriter.Write(', 4'); break; }
                'Relinquished' { $StreamWriter.Write(', 5'); break; }
                'Destroyed' { $StreamWriter.Write(', 6'); break; }
                default { $StreamWriter.Write(", $($SourceElement.DefaultDriveType)"); break; }
            }
        }
        if ($null -ne $SourceElement.UpstreamId) { $StreamWriter.Write(", '$($SourceElement.UpstreamId)'") }
        if ($null -ne $SourceElement.LastSynchronizedOn) {
            if ([string]::IsNullOrWhiteSpace($SourceElement.LastSynchronizedOn)) {
                $StreamWriter.Write(', NULL');
            } else {
                $DateTime = [System.Xml.XmlConvert]::ToDateTime($SourceElement.LastSynchronizedOn, [System.Xml.XmlDateTimeSerializationMode]::RoundtripKind);
                $StreamWriter.Write(", '$($DateTime.ToString('yyyy-MM-dd HH:mm:ss'))'");
            }
        }
        $ModifiedOn = $CreatedOn = [System.Xml.XmlConvert]::ToDateTime($SourceElement.CreatedOn, [System.Xml.XmlDateTimeSerializationMode]::RoundtripKind);
        if ($null -ne $SourceElement.ModifiedOn) {
            $ModifiedOn = [System.Xml.XmlConvert]::ToDateTime($SourceElement.ModifiedOn, [System.Xml.XmlDateTimeSerializationMode]::RoundtripKind);
        }
        $StreamWriter.Write(", '$($CreatedOn.ToString('yyyy-MM-dd HH:mm:ss'))'");
        $StreamWriter.WriteLine(", '$($ModifiedOn.ToString('yyyy-MM-dd HH:mm:ss'))');");
        @($SourceElement.SelectNodes('f:AccessError', $Nsmgr)) | Export-AccessErrorElement -StreamWriter $StreamWriter -TargetId $SourceElement.Id -Type 'Volume';
    }
}

Function Export-SymbolicNameElement {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.XmlElement]$SourceElement,
        
        [Parameter(Mandatory = $true)]
        [System.IO.StreamWriter]$StreamWriter,
        
        [Parameter(Mandatory = $true)]
        [string]$FileSystemId
    )
    
    Process {
        $StreamWriter.Write('INSERT INTO "SymbolicNames" ("Id", "Name", "FileSystemId"');
        if ($null -ne $SourceElement.Priority) { $StreamWriter.Write(', "Priority"') }
        if (-not $SourceElement.IsEmpty) { $StreamWriter.Write(', "Notes"') }
        @('IsInactive', 'UpstreamId', 'LastSynchronizedOn') | ForEach-Object {
            if ($null -ne $SourceElement.$_) { $StreamWriter.Write(", `"$_`"") }
        }
        $StreamWriter.WriteLine(', "CreatedOn", "ModifiedOn")');
        $StreamWriter.Write("`tVALUES ('$($SourceElement.Id)', '$($SourceElement.Name.Replace("'", "''"))', '$FileSystemId'");
        if ($null -ne $SourceElement.Priority) { $StreamWriter.Write(", $($SourceElement.Priority)") }
        if (-not $SourceElement.IsEmpty) { $StreamWriter.Write(", '$($SourceElement.InnerText.Replace("'", "''"))'") }
        if ($null -ne $SourceElement.IsInactive) {
            if ([System.Xml.XmlConvert]::ToBoolean($SourceElement.IsInactive)) { $StreamWriter.Write(', 1') } else { $StreamWriter.Write(', 0') }
        }
        if ($null -ne $SourceElement.UpstreamId) { $StreamWriter.Write(", '$($SourceElement.UpstreamId)'") }
        if ($null -ne $SourceElement.LastSynchronizedOn) {
            if ([string]::IsNullOrWhiteSpace($SourceElement.LastSynchronizedOn)) {
                $StreamWriter.Write(', NULL');
            } else {
                $DateTime = [System.Xml.XmlConvert]::ToDateTime($SourceElement.LastSynchronizedOn, [System.Xml.XmlDateTimeSerializationMode]::RoundtripKind);
                $StreamWriter.Write(", '$($DateTime.ToString('yyyy-MM-dd HH:mm:ss'))'");
            }
        }
        $ModifiedOn = $CreatedOn = [System.Xml.XmlConvert]::ToDateTime($SourceElement.CreatedOn, [System.Xml.XmlDateTimeSerializationMode]::RoundtripKind);
        if ($null -ne $SourceElement.ModifiedOn) {
            $ModifiedOn = [System.Xml.XmlConvert]::ToDateTime($SourceElement.ModifiedOn, [System.Xml.XmlDateTimeSerializationMode]::RoundtripKind);
        }
        $StreamWriter.Write(", '$($CreatedOn.ToString('yyyy-MM-dd HH:mm:ss'))'");
        $StreamWriter.WriteLine(", '$($ModifiedOn.ToString('yyyy-MM-dd HH:mm:ss'))');");
    }
}

Function Export-FileSystemElement {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.XmlElement]$SourceElement,
        
        [Parameter(Mandatory = $true)]
        [System.Xml.XmlNamespaceManager]$Nsmgr,

        [Parameter(Mandatory = $true)]
        [System.IO.StreamWriter]$StreamWriter
    )
    
    Process {
        $StreamWriter.Write('INSERT INTO "FileSystems" ("Id", "DisplayName"');
        @('CaseSensitiveSearch', 'ReadOnly', 'MaxNameLength', 'DefaultDriveType', 'Notes', 'IsInactive', 'UpstreamId', 'LastSynchronizedOn') | ForEach-Object {
            if ($null -ne $SourceElement.$_) { $StreamWriter.Write(", `"$_`"") }
        }
        $StreamWriter.WriteLine(', "CreatedOn", "ModifiedOn")');
        $StreamWriter.Write("`tVALUES ('$($SourceElement.Id)', '$($SourceElement.DisplayName.Replace("'", "''"))'");
        if ($null -ne $SourceElement.CaseSensitiveSearch) {
            if ([System.Xml.XmlConvert]::ToBoolean($SourceElement.CaseSensitiveSearch)) { $StreamWriter.Write(', 1') } else { $StreamWriter.Write(', 0') }
        }
        if ($null -ne $SourceElement.ReadOnly) {
            if ([System.Xml.XmlConvert]::ToBoolean($SourceElement.ReadOnly)) { $StreamWriter.Write(', 1') } else { $StreamWriter.Write(', 0') }
        }
        if ($null -ne $SourceElement.MaxNameLength) { $StreamWriter.Write(", $($SourceElement.MaxNameLength)") }
        if ($null -ne $SourceElement.DefaultDriveType) {
            if ([string]::IsNullOrWhiteSpace($SourceElement.DefaultDriveType)) {
                $StreamWriter.Write(', NULL');
            } else {
                switch ($SourceElement.DefaultDriveType) {
                    'Unknown' { $StreamWriter.Write(', 0'); break; }
                    'NoRootDirectory' { $StreamWriter.Write(', 1'); break; }
                    'Removable' { $StreamWriter.Write(', 2'); break; }
                    'Fixed' { $StreamWriter.Write(', 3'); break; }
                    'Network' { $StreamWriter.Write(', 4'); break; }
                    'CDRom' { $StreamWriter.Write(', 5'); break; }
                    'Ram' { $StreamWriter.Write(', 6'); break; }
                    default { $StreamWriter.Write(", $($SourceElement.DefaultDriveType)"); break; }
                }
            }
        }
        if ($null -ne $SourceElement.Notes) { $StreamWriter.Write(", '$($SourceElement.Notes.Replace("'", "''"))'") }
        if ($null -ne $SourceElement.IsInactive) {
            if ([System.Xml.XmlConvert]::ToBoolean($SourceElement.IsInactive)) { $StreamWriter.Write(', 1') } else { $StreamWriter.Write(', 0') }
        }
        if ($null -ne $SourceElement.UpstreamId) { $StreamWriter.Write(", '$($SourceElement.UpstreamId)'") }
        if ($null -ne $SourceElement.LastSynchronizedOn) {
            if ([string]::IsNullOrWhiteSpace($SourceElement.LastSynchronizedOn)) {
                $StreamWriter.Write(', NULL');
            } else {
                $DateTime = [System.Xml.XmlConvert]::ToDateTime($SourceElement.LastSynchronizedOn, [System.Xml.XmlDateTimeSerializationMode]::RoundtripKind);
                $StreamWriter.Write(", '$($DateTime.ToString('yyyy-MM-dd HH:mm:ss'))'");
            }
        }
        $ModifiedOn = $CreatedOn = [System.Xml.XmlConvert]::ToDateTime($SourceElement.CreatedOn, [System.Xml.XmlDateTimeSerializationMode]::RoundtripKind);
        if ($null -ne $SourceElement.ModifiedOn) {
            $ModifiedOn = [System.Xml.XmlConvert]::ToDateTime($SourceElement.ModifiedOn, [System.Xml.XmlDateTimeSerializationMode]::RoundtripKind);
        }
        $StreamWriter.Write(", '$($CreatedOn.ToString('yyyy-MM-dd HH:mm:ss'))'");
        $StreamWriter.WriteLine(", '$($ModifiedOn.ToString('yyyy-MM-dd HH:mm:ss'))');");
        @($SourceElement.SelectNodes('f:SymbolicName', $Nsmgr)) | Export-SymbolicNameElement -StreamWriter $StreamWriter -FileSystemId $SourceElement.Id -ErrorAction Stop;
        @($SourceElement.SelectNodes('f:Volume', $Nsmgr)) | Export-VolumeElement -StreamWriter $StreamWriter -FileSystemId $SourceElement.Id -Nsmgr $Nsmgr;
    }
}

Function Export-ContentInfoElement {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.XmlElement]$SourceElement,
        
        [Parameter(Mandatory = $true)]
        [System.IO.StreamWriter]$StreamWriter
    )
    
    Process {
        $StreamWriter.Write('INSERT INTO "ContentInfos" ("Id", "Length"');
        @('Hash', 'UpstreamId', 'LastSynchronizedOn') | ForEach-Object {
            if ($null -ne $SourceElement.$_) { $StreamWriter.Write(", `"$_`"") }
        }
        $StreamWriter.WriteLine(', "CreatedOn", "ModifiedOn")');
        $StreamWriter.Write("`tVALUES ('$($SourceElement.Id)', $($SourceElement.Length)");
        if ($null -ne $SourceElement.Hash) {
            $Hash = -join ([Convert]::FromBase64String("$($SourceElement.Hash)==") | ForEach-Object { $_.ToString('x2') });
            $StreamWriter.Write(", X'$Hash'");
        }
        if ($null -ne $SourceElement.UpstreamId) { $StreamWriter.Write(", '$($SourceElement.UpstreamId)'") }
        if ($null -ne $SourceElement.LastSynchronizedOn) {
            if ([string]::IsNullOrWhiteSpace($SourceElement.LastSynchronizedOn)) {
                $StreamWriter.Write(', NULL');
            } else {
                $DateTime = [System.Xml.XmlConvert]::ToDateTime($SourceElement.LastSynchronizedOn, [System.Xml.XmlDateTimeSerializationMode]::RoundtripKind);
                $StreamWriter.Write(", '$($DateTime.ToString('yyyy-MM-dd HH:mm:ss'))'");
            }
        }
        $ModifiedOn = $CreatedOn = [System.Xml.XmlConvert]::ToDateTime($SourceElement.CreatedOn, [System.Xml.XmlDateTimeSerializationMode]::RoundtripKind);
        if ($null -ne $SourceElement.ModifiedOn) {
            $ModifiedOn = [System.Xml.XmlConvert]::ToDateTime($SourceElement.ModifiedOn, [System.Xml.XmlDateTimeSerializationMode]::RoundtripKind);
        }
        $StreamWriter.Write(", '$($CreatedOn.ToString('yyyy-MM-dd HH:mm:ss'))'");
        $StreamWriter.WriteLine(", '$($ModifiedOn.ToString('yyyy-MM-dd HH:mm:ss'))');");
    }
}

Function Export-ExtendedPropertiesElement {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.XmlElement]$SourceElement,
        
        [Parameter(Mandatory = $true)]
        [System.IO.StreamWriter]$StreamWriter
    )
    
    Process {
        $StreamWriter.Write('INSERT INTO "ExtendedProperties" ("Id", "Width", "Height"');
        @('Duration', 'FrameCount', 'TrackNumber', 'Bitrate', 'FrameRate', 'SamplesPerPixel', 'PixelPerUnitX', 'PixelPerUnitY', 'Compression', 'XResNumerator', 'XResDenominator', 'YResNumerator',
            'YResDenominator', 'ResolutionXUnit', 'ResolutionYUnit', 'JPEGProc', 'JPEGQuality', 'DateTime', 'Title') | ForEach-Object {
            if ($null -ne $SourceElement.$_) { $StreamWriter.Write(", `"$_`"") }
        }
        if (-not $SourceElement.IsEmpty) { $StreamWriter.Write(', "Description"') }
        @('Copyright', 'SoftwareUsed', 'Artis', 'HostComputer', 'UpstreamId', 'LastSynchronizedOn') | ForEach-Object {
            if ($null -ne $SourceElement.$_) { $StreamWriter.Write(", `"$_`"") }
        }
        $StreamWriter.WriteLine(', "CreatedOn", "ModifiedOn")');
        $StreamWriter.Write("`tVALUES ('$($SourceElement.Id)', $($SourceElement.Width), $($SourceElement.Height)");
        @('Duration', 'FrameCount', 'TrackNumber', 'Bitrate', 'FrameRate', 'SamplesPerPixel', 'PixelPerUnitX', 'PixelPerUnitY', 'Compression', 'XResNumerator', 'XResDenominator', 'YResNumerator',
            'YResDenominator', 'ResolutionXUnit', 'ResolutionYUnit', 'JPEGProc', 'JPEGQuality') | ForEach-Object {
            if ($null -ne $SourceElement.$_) {
                if ([string]::IsNullOrWhiteSpace($SourceElement.$_)) {
                    $StreamWriter.Write(', NULL');
                } else {
                    $StreamWriter.Write(", $($SourceElement.$_)");
                }
            }
        }
        if ($null -ne $SourceElement.DateTime) {
            if ([string]::IsNullOrWhiteSpace($SourceElement.DateTime)) {
                $StreamWriter.Write(', NULL');
            } else {
                $DateTime = [System.Xml.XmlConvert]::ToDateTime($SourceElement.DateTime, [System.Xml.XmlDateTimeSerializationMode]::RoundtripKind);
                $StreamWriter.Write(", '$($DateTime.ToString('yyyy-MM-dd HH:mm:ss'))'");
            }
        }
        if ($null -ne $SourceElement.Title) { $StreamWriter.Write(", $($SourceElement.Title.Replace("'", "''"))") }
        if (-not $SourceElement.IsEmpty) { $StreamWriter.Write(", $($SourceElement.InnerText.Replace("'", "''"))") }
        @('Copyright', 'SoftwareUsed', 'Artis', 'HostComputer') | ForEach-Object {
            if ($null -ne $SourceElement.$_) { $StreamWriter.Write(", $($SourceElement.$_.Replace("'", "''"))") }
        }
        if ($null -ne $SourceElement.UpstreamId) { $StreamWriter.Write(", '$($SourceElement.UpstreamId)'") }
        if ($null -ne $SourceElement.LastSynchronizedOn) {
            if ([string]::IsNullOrWhiteSpace($SourceElement.LastSynchronizedOn)) {
                $StreamWriter.Write(', NULL');
            } else {
                $DateTime = [System.Xml.XmlConvert]::ToDateTime($SourceElement.LastSynchronizedOn, [System.Xml.XmlDateTimeSerializationMode]::RoundtripKind);
                $StreamWriter.Write(", '$($DateTime.ToString('yyyy-MM-dd HH:mm:ss'))'");
            }
        }
        $ModifiedOn = $CreatedOn = [System.Xml.XmlConvert]::ToDateTime($SourceElement.CreatedOn, [System.Xml.XmlDateTimeSerializationMode]::RoundtripKind);
        if ($null -ne $SourceElement.ModifiedOn) {
            $ModifiedOn = [System.Xml.XmlConvert]::ToDateTime($SourceElement.ModifiedOn, [System.Xml.XmlDateTimeSerializationMode]::RoundtripKind);
        }
        $StreamWriter.Write(", '$($CreatedOn.ToString('yyyy-MM-dd HH:mm:ss'))'");
        $StreamWriter.WriteLine(", '$($ModifiedOn.ToString('yyyy-MM-dd HH:mm:ss'))');");
    }
}

Function Export-FileElement {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.XmlElement]$SourceElement,
        
        [Parameter(Mandatory = $true)]
        [System.IO.StreamWriter]$StreamWriter,
        
        [Parameter(Mandatory = $true)]
        [string]$ParentId
    )
    
    Process {
        $StreamWriter.Write('INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId"');
        @('Options', 'LastHashCalculation', 'Notes', 'Deleted', 'ExtendedPropertyId', 'UpstreamId', 'LastSynchronizedOn') | ForEach-Object {
            if ($null -ne $SourceElement.$_) { $StreamWriter.Write(", `"$_`"") }
        }
        $StreamWriter.WriteLine(', "LastAccessed", "CreatedOn", "ModifiedOn")');
        $StreamWriter.Write("`tVALUES ('$($SourceElement.Id)', '$($SourceElement.Name.Replace("'", "''"))', '$($SourceElement.ContentId)', '$ParentId'");
        if ($null -ne $SourceElement.Options) {
            $Value = 0;
            ($SourceElement.Options -split '\s+') | ForEach-Object {
                switch ($SourceElement.Options) {
                    'None' { break; }
                    'DoNotCompare' { $value = $value -bor 1; break; }
                    'DoNotShow' { $value = $value -bor 2; break; }
                    'FlaggedForDeletion' { $value = $value -bor 4; break; }
                    'FlaggedForRescan' { $value = $value -bor 8; break; }
                    default { $value = $value -bor [int]::Parse($SourceElement.Options); break; }
                }
            }
            $StreamWriter.Write(", $Value");
        }
        if ($null -ne $SourceElement.LastHashCalculation) {
            $DateTime = [System.Xml.XmlConvert]::ToDateTime($SourceElement.LastHashCalculation, [System.Xml.XmlDateTimeSerializationMode]::RoundtripKind);
            $StreamWriter.Write(", '$($DateTime.ToString('yyyy-MM-dd HH:mm:ss'))'");
        }
        if ($null -ne $SourceElement.Notes) { $StreamWriter.Write(", '$($SourceElement.Notes.Replace("'", "''"))'") }
        if ($null -ne $SourceElement.Deleted) {
            if ([System.Xml.XmlConvert]::ToBoolean($SourceElement.Deleted)) { $StreamWriter.Write(', 1') } else { $StreamWriter.Write(', 0') }
        }
        if ($null -ne $SourceElement.ExtendedPropertyId) { $StreamWriter.Write(", '$($SourceElement.ExtendedPropertyId)'") }
        if ($null -ne $SourceElement.UpstreamId) { $StreamWriter.Write(", '$($SourceElement.UpstreamId)'") }
        if ($null -ne $SourceElement.LastSynchronizedOn) {
            if ([string]::IsNullOrWhiteSpace($SourceElement.LastSynchronizedOn)) {
                $StreamWriter.Write(', NULL');
            } else {
                $DateTime = [System.Xml.XmlConvert]::ToDateTime($SourceElement.LastSynchronizedOn, [System.Xml.XmlDateTimeSerializationMode]::RoundtripKind);
                $StreamWriter.Write(", '$($DateTime.ToString('yyyy-MM-dd HH:mm:ss'))'");
            }
        }
        $ModifiedOn = $CreatedOn = [System.Xml.XmlConvert]::ToDateTime($SourceElement.CreatedOn, [System.Xml.XmlDateTimeSerializationMode]::RoundtripKind);
        if ($null -ne $SourceElement.ModifiedOn) {
            $ModifiedOn = [System.Xml.XmlConvert]::ToDateTime($SourceElement.ModifiedOn, [System.Xml.XmlDateTimeSerializationMode]::RoundtripKind);
        }
        if ($null -ne $SourceElement.LastAccessed) {
            $DateTime = [System.Xml.XmlConvert]::ToDateTime($SourceElement.LastAccessed, [System.Xml.XmlDateTimeSerializationMode]::RoundtripKind);
            $StreamWriter.Write(", '$($DateTime.ToString('yyyy-MM-dd HH:mm:ss'))'");
        } else {
            $StreamWriter.Write(", '$($ModifiedOn.ToString('yyyy-MM-dd HH:mm:ss'))'");
        }
        $StreamWriter.Write(", '$($CreatedOn.ToString('yyyy-MM-dd HH:mm:ss'))'");
        $StreamWriter.WriteLine(", '$($ModifiedOn.ToString('yyyy-MM-dd HH:mm:ss'))');");
        @($SourceElement.SelectNodes('f:AccessError', $Nsmgr)) | Export-AccessErrorElement -StreamWriter $StreamWriter -TargetId $SourceElement.Id -Type 'File';
    }
}

Function Export-SubdirectoryElement {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.XmlElement]$SourceElement,
        
        [Parameter(Mandatory = $true)]
        [System.Xml.XmlNamespaceManager]$Nsmgr,

        [Parameter(Mandatory = $true)]
        [System.IO.StreamWriter]$StreamWriter,

        [string]$ParentId
    )
    
    Process {
        $StreamWriter.Write('INSERT INTO "Subdirectories" ("Id", "Name"');
        if ($PSBoundParameters.ContainsKey('ParentId')) { $StreamWriter.Write(', "ParentId"') } else { $StreamWriter.Write(', "VolumeId"') }
        @('Options', 'Notes', 'Deleted', 'UpstreamId', 'LastSynchronizedOn') | ForEach-Object {
            if ($null -ne $SourceElement.$_) { $StreamWriter.Write(", `"$_`"") }
        }
        $StreamWriter.WriteLine(', "LastAccessed", "CreatedOn", "ModifiedOn")');
        $StreamWriter.Write("`tVALUES ('$($SourceElement.Id)', '$($SourceElement.Name.Replace("'", "''"))'");
        if ($PSBoundParameters.ContainsKey('ParentId')) { $StreamWriter.Write(", '$ParentId'") } else { $StreamWriter.Write(", '$($SourceElement.ParentNode.Id)'") }
        if ($null -ne $SourceElement.Options) {
            $Value = 0;
            ($SourceElement.Options -split '\s+') | ForEach-Object {
                switch ($SourceElement.Options) {
                    'None' { break; }
                    'SkipSubdirectories' { $value = $value -bor 1; break; }
                    'Skip' { $value = $value -bor 2; break; }
                    'DoNotCompareFiles' { $value = $value -bor 4; break; }
                    'DoNotShow' { $value = $value -bor 8; break; }
                    'FlaggedForDeletion' { $value = $value -bor 16; break; }
                    'FlaggedForRescan' { $value = $value -bor 32; break; }
                    default { $value = $value -bor [int]::Parse($SourceElement.Options); break; }
                }
            }
            $StreamWriter.Write(", $Value");
        }
        if ($null -ne $SourceElement.Notes) { $StreamWriter.Write(", '$($SourceElement.Notes.Replace("'", "''"))'") }
        if ($null -ne $SourceElement.Deleted) {
            if ([System.Xml.XmlConvert]::ToBoolean($SourceElement.Deleted)) { $StreamWriter.Write(', 1') } else { $StreamWriter.Write(', 0') }
        }
        if ($null -ne $SourceElement.UpstreamId) { $StreamWriter.Write(", '$($SourceElement.UpstreamId)'") }
        if ($null -ne $SourceElement.LastSynchronizedOn) {
            if ([string]::IsNullOrWhiteSpace($SourceElement.LastSynchronizedOn)) {
                $StreamWriter.Write(', NULL');
            } else {
                $DateTime = [System.Xml.XmlConvert]::ToDateTime($SourceElement.LastSynchronizedOn, [System.Xml.XmlDateTimeSerializationMode]::RoundtripKind);
                $StreamWriter.Write(", '$($DateTime.ToString('yyyy-MM-dd HH:mm:ss'))'");
            }
        }
        $ModifiedOn = $CreatedOn = [System.Xml.XmlConvert]::ToDateTime($SourceElement.CreatedOn, [System.Xml.XmlDateTimeSerializationMode]::RoundtripKind);
        if ($null -ne $SourceElement.ModifiedOn) {
            $ModifiedOn = [System.Xml.XmlConvert]::ToDateTime($SourceElement.ModifiedOn, [System.Xml.XmlDateTimeSerializationMode]::RoundtripKind);
        }
        if ($null -ne $SourceElement.LastAccessed) {
            $DateTime = [System.Xml.XmlConvert]::ToDateTime($SourceElement.LastAccessed, [System.Xml.XmlDateTimeSerializationMode]::RoundtripKind);
            $StreamWriter.Write(", '$($DateTime.ToString('yyyy-MM-dd HH:mm:ss'))'");
        } else {
            $StreamWriter.Write(", '$($ModifiedOn.ToString('yyyy-MM-dd HH:mm:ss'))'");
        }
        $StreamWriter.Write(", '$($CreatedOn.ToString('yyyy-MM-dd HH:mm:ss'))'");
        $StreamWriter.WriteLine(", '$($ModifiedOn.ToString('yyyy-MM-dd HH:mm:ss'))');");
        @($SourceElement.SelectNodes('f:AccessError', $Nsmgr)) | Export-AccessErrorElement -StreamWriter $StreamWriter -TargetId $SourceElement.Id -Type 'Subdirectory';
        @($SourceElement.SelectNodes('f:Subdirectory', $Nsmgr)) | Export-SubdirectoryElement -StreamWriter $StreamWriter -ParentId $SourceElement.Id -Nsmgr $Nsmgr;
        @($SourceElement.SelectNodes('f:File', $Nsmgr)) | Export-FileElement -StreamWriter $StreamWriter -ParentId $SourceElement.Id;
    }
}

[Xml]$XmlDocument = '<DbCommands/>';
$XmlDocument.Load(($PSScriptRoot | Join-Path -ChildPath 'DbCommands.xml'));

if ([string]::IsNullOrWhiteSpace($Path)) { $Path = $PSScriptRoot | Join-Path -ChildPath 'CreateLocalDb.sql' }
$StreamWriter = [System.IO.StreamWriter]::new($Path, $false, [System.Text.UTF8Encoding]::new($false, $true));
try {
    if (-not $NoDropTables) {
        $StreamWriter.WriteLine('-- Deleting tables');
        $StreamWriter.WriteLine();
        foreach ($XmlElement in @($XmlDocument.DocumentElement.SelectNodes('DropTables/Text'))) {
            $StreamWriter.WriteLine("$($XmlElement.InnerText.Trim());");
        }
    }
    $StreamWriter.WriteLine();
    $StreamWriter.WriteLine('-- Creating tables');
    foreach ($XmlElement in @($XmlDocument.DocumentElement.SelectNodes('DbCreation/Text'))) {
        $StreamWriter.WriteLine();
        $StreamWriter.WriteLine("$($XmlElement.InnerText.Trim());");
    }
    if (-not [string]::IsNullOrWhiteSpace($ImportPath)) {
        if (-not [System.IO.Path]::IsPathRooted($ImportPath)) {
            $ImportPath = [System.IO.Path]::Combine($PSScriptRoot, $ImportPath);
        }
        $ImportPath = [System.IO.Path]::GetFullPath($ImportPath);
        [Xml]$LocalXmlDocument = '<Local xmlns="http://git.erwinefamily.net/FsInfoCat/V1/FsInfoCatExport.xsd"/>';
        $LocalXmlDocument.Load($ImportPath);
        $StreamWriter.WriteLine();
        $Nsmgr = [System.Xml.XmlNamespaceManager]::new($LocalXmlDocument.NameTable);
        $Nsmgr.AddNamespace('f', 'http://git.erwinefamily.net/FsInfoCat/V1/FsInfoCatExport.xsd');
        @($LocalXmlDocument.DocumentElement.SelectNodes('f:FileSystem', $Nsmgr)) | Export-FileSystemElement -StreamWriter $StreamWriter -Nsmgr $Nsmgr;
        @($LocalXmlDocument.DocumentElement.SelectNodes('f:ContentInfo', $Nsmgr)) | Export-ContentInfoElement -StreamWriter $StreamWriter;
        @($LocalXmlDocument.DocumentElement.SelectNodes('f:ExtendedProperties', $Nsmgr)) | Export-ExtendedPropertiesElement -StreamWriter $StreamWriter;
        @($LocalXmlDocument.DocumentElement.SelectNodes('f:FileSystem/f:Volume/f:RootDirectory', $Nsmgr)) | Export-SubdirectoryElement -StreamWriter $StreamWriter -Nsmgr $Nsmgr;
    }
    $StreamWriter.Flush();
} finally { $StreamWriter.Dispose() }

#>