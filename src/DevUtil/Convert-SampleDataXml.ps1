$Script:TypeMapping = @{
    AudioPropertySet = @{
        TableName = 'AudioPropertySets';
        ColTypes = @{
            EncodingBitrate = [System.Data.DbType]::UInt32;
            IsVariableBitrate = [System.Data.DbType]::Boolean;
            SampleRate = [System.Data.DbType]::UInt32;
            SampleSize = [System.Data.DbType]::UInt32;
            StreamNumber = [System.Data.DbType]::UInt16;
        };
    };
    BinaryPropertySet = @{
        TableName = 'BinaryPropertySets';
        ColTypes = @{
            Length = [System.Data.DbType]::Int64;
            Hash = [System.Data.DbType]::Binary;
        };
    };
    Comparison = @{
        TableName = 'Comparisons';
        ColTypes = @{
            AreEqual = [System.Data.DbType]::Boolean;
            ComparedOn = [System.Data.DbType]::DateTime;
        };
    };
    CrawlConfiguration = @{
        TableName = 'CrawlConfigurations';
        ColTypes = @{
            StatusValue = [System.Data.DbType]::Byte;
            LastCrawlStart = [System.Data.DbType]::DateTime;
            LastCrawlEnd = [System.Data.DbType]::DateTime;
            NextScheduledStart = [System.Data.DbType]::DateTime;
            RescheduleInterval = [System.Data.DbType]::Int64;
            RescheduleFromJobEnd = [System.Data.DbType]::Boolean;
            RescheduleAfterFail = [System.Data.DbType]::Boolean;
            MaxRecursionDepth = [System.Data.DbType]::UInt16;
            MaxTotalItems = [System.Data.DbType]::UInt64;
            TTL = [System.Data.DbType]::Int64;
        };
    };
    CrawlJobLog = @{
        TableName = 'CrawlJobLogs';
        ColTypes = @{
            StatusCode = [System.Data.DbType]::Byte;
            CrawlStart = [System.Data.DbType]::DateTime;
            CrawlEnd = [System.Data.DbType]::DateTime;
            FoldersProcessed = [System.Data.DbType]::Int64;
            FilesProcessed = [System.Data.DbType]::Int64;
        };
    };
    DocumentPropertySet = @{
        TableName = 'DocumentPropertySets';
        ColTypes = @{
            DateCreated = [System.Data.DbType]::DateTime;
            Security = [System.Data.DbType]::Int32;
        };
    };
    DRMPropertySet = @{
        TableName = 'DRMPropertySets';
        ColTypes = @{
            DatePlayExpires = [System.Data.DbType]::DateTime;
            DatePlayStarts = [System.Data.DbType]::DateTime;
            IsProtected = [System.Data.DbType]::Boolean;
            PlayCount = [System.Data.DbType]::UInt32;
        };
    };
    File = @{
        TableName = 'Files';
        ColTypes = @{
            LastAccessed = [System.Data.DbType]::DateTime;
            CreationTime = [System.Data.DbType]::DateTime;
            LastWriteTime = [System.Data.DbType]::DateTime;
            Options = [System.Data.DbType]::Byte;
            Status = [System.Data.DbType]::Byte;
            LastHashCalculation = [System.Data.DbType]::DateTime;
        };
    };
    FileAccessError = @{
        TableName = 'FileAccessErrors';
        ColTypes = @{
            ErrorCode = [System.Data.DbType]::Int32;
        };
    };
    FileSystem = @{
        TableName = 'FileSystems';
        ColTypes = @{
            IsInactive = [System.Data.DbType]::Boolean;
        };
    };
    GPSPropertySet = @{
        TableName = 'GPSPropertySets';
        ColTypes = @{
            LatitudeDegrees = [System.Data.DbType]::Double;
            LatitudeMinutes = [System.Data.DbType]::Double;
            LatitudeSeconds = [System.Data.DbType]::Double;
            LongitudeDegrees = [System.Data.DbType]::Double;
            LongitudeMinutes = [System.Data.DbType]::Double;
            LongitudeSeconds = [System.Data.DbType]::Double;
            VersionID = [System.Data.DbType]::Binary;
        };
    };
    ImagePropertySet = @{
        TableName = 'ImagePropertySets';
        ColTypes = @{
            BitDepth = [System.Data.DbType]::UInt32;
            ColorSpace = [System.Data.DbType]::UInt16;
            CompressedBitsPerPixel = [System.Data.DbType]::Double;
            Compression = [System.Data.DbType]::UInt16;
            HorizontalResolution = [System.Data.DbType]::Double;
            HorizontalSize = [System.Data.DbType]::UInt32;
            ResolutionUnit = [System.Data.DbType]::Int16;
            VerticalResolution = [System.Data.DbType]::Double;
            VerticalSize = [System.Data.DbType]::UInt32;
        };
    };
    MediaPropertySet = @{
        TableName = 'MediaPropertySets';
        ColTypes = @{
            Duration = [System.Data.DbType]::UInt64;
            FrameCount = [System.Data.DbType]::UInt32;
            Year = [System.Data.DbType]::UInt32;
        };
    };
    MusicPropertySet = @{
        TableName = 'MusicPropertySets';
        ColTypes = @{
            ChannelCount = [System.Data.DbType]::UInt32;
            TrackNumber = [System.Data.DbType]::UInt32;
        };
    };
    PersonalTagDefinition = @{
        TableName = 'PersonalTagDefinitions';
        ColTypes = @{
            IsInactive = [System.Data.DbType]::Boolean;
        };
    };
    PhotoPropertySet = @{
        TableName = 'PhotoPropertySets';
        ColTypes = @{
            DateTaken = [System.Data.DbType]::DateTime;
            Orientation = [System.Data.DbType]::UInt16;
        };
    };
    RecordedTVPropertySet = @{
        TableName = 'RecordedTVPropertySets';
        ColTypes = @{
            ChannelNumber = [System.Data.DbType]::UInt32;
            IsDTVContent = [System.Data.DbType]::Boolean;
            IsHDContent = [System.Data.DbType]::Boolean;
            OriginalBroadcastDate = [System.Data.DbType]::DateTime;
        };
    };
    Redundancy = @{ 
        TableName = 'Redundancies';
        ColTypes = @{};
    };
    RedundantSet = @{
        TableName = 'RedundantSets';
        ColTypes = @{
            Status = [System.Data.DbType]::Byte;
        };
    };
    SharedTagDefinition = @{
        TableName = 'SharedTagDefinitions';
        ColTypes = @{
            IsInactive = [System.Data.DbType]::Boolean;
        };
    };
    Subdirectory = @{
        TableName = 'Subdirectories';
        ColTypes = @{
            LastAccessed = [System.Data.DbType]::DateTime;
            CreationTime = [System.Data.DbType]::DateTime;
            LastWriteTime = [System.Data.DbType]::DateTime;
            Options = [System.Data.DbType]::Byte;
            Status = [System.Data.DbType]::Byte;
        };
    };
    SubdirectoryAccessError = @{
        TableName = 'SubdirectoryAccessErrors';
        ColTypes = @{
            ErrorCode = [System.Data.DbType]::Int32;
        };
    };
    SummaryPropertySet = @{
        TableName = 'SummaryPropertySets';
        ColTypes = @{
            Rating = [System.Data.DbType]::UInt32;
            Sensitivity = [System.Data.DbType]::UInt16;
            SimpleRating = [System.Data.DbType]::UInt32;
        };
    };
    SymbolicName = @{
        TableName = 'SymbolicNames';
        ColTypes = @{
            Priority = [System.Data.DbType]::Int32;
            IsInactive = [System.Data.DbType]::Boolean;
        };
    };
    VideoPropertySet = @{
        TableName = 'VideoPropertySets';
        ColTypes = @{
            EncodingBitrate = [System.Data.DbType]::UInt32;
            FrameHeight = [System.Data.DbType]::UInt32;
            FrameRate = [System.Data.DbType]::UInt32;
            FrameWidth = [System.Data.DbType]::UInt32;
            HorizontalAspectRatio = [System.Data.DbType]::UInt32;
            StreamNumber = [System.Data.DbType]::UInt16;
            VerticalAspectRatio = [System.Data.DbType]::UInt32;
        };
    };
    Volume = @{
        TableName = 'Volumes';
        ColTypes = @{
            ReadOnly = [System.Data.DbType]::Boolean;
            MaxNameLength = [System.Data.DbType]::UInt32;
            Type = [System.Data.DbType]::Byte;
            Status = [System.Data.DbType]::Byte;
        };
    };
    VolumeAccessError = @{
        TableName = 'VolumeAccessErrors';
        ColTypes = @{
            ErrorCode = [System.Data.DbType]::Int32;
        };
    };
}

Function Import-SampleData {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.XmlElement]$Element,
        
        [Parameter(Mandatory = $true)]
        [System.IO.TextWriter]$Writer,

        [string]$ParentName,

        [string]$ParentId
    )

    Process {
        $Hash = $Script:TypeMapping[$Element.LocalName];
        if ($null -eq $Hash) {
            Write-Warning -Message "No mapping for $($Element.LocalName)";
        } else {
            $Writer.Write('INSERT INTO "');
            $Writer.Write($Hash['TableName']);
            $Hash = $Hash['ColTypes'];
            $Writer.Write('" ("');
            $Names = @(@($Element.Attributes) | ForEach-Object { $_.LocalName });
            if ($PSBoundParameters.ContainsKey('ParentName')) {
                if ($PSBoundParameters.ContainsKey('ParentId')) {
                    $Names += @($ParentName);
                } else {
                    Write-Warning -Message "ParentName not specified while ParentId is '$ParentId'";
                }
            } else {
                if ($PSBoundParameters.ContainsKey('ParentId')) {
                    Write-Warning -Message "ParentName not specified while ParentId is '$ParentId'";
                }
            }
            $Writer.Write($Names -join '", "');
            $Writer.WriteLine('")');
            $Writer.Write("    VALUES (");
            $a = $Element.Attributes[0];
            if ($Hash.ContainsKey($a.LocalName)) {
                switch ($Hash[$a.LocalName]) {
                    { $_ -eq [System.Data.DbType]::Boolean } {
                        if ([System.Xml.XmlConvert]::ToBoolean($a.Value)) {
                            $Writer.Write('1');
                        } else {
                            $Writer.Write('0');
                        }
                        break;
                    }
                    { $_ -eq [System.Data.DbType]::Binary } {
                        $Writer.Write("X'");
                        $Writer.Write($a.Value);
                        $Writer.Write("'");
                        break;
                    }
                    { $_ -eq [System.Data.DbType]::DateTime } {
                        $Writer.Write("'");
                        $Writer.Write([System.Xml.XmlConvert]::ToString([System.Xml.XmlConvert]::ToDateTime($a.Value, 'yyyy-MM-ddTHH:mm:ss'), 'yyyy-MM-dd HH:mm:ss'));
                        $Writer.Write("'");
                        break;
                    }
                    default {
                        $Writer.Write($a.Value);
                        break;
                    }
                }
            } else {
                $Writer.Write("'");
                $Writer.Write($a.Value);
                $Writer.Write("'");
            }
            foreach ($a in @(@($Element.Attributes) | Select-Object -Skip 1)) {
                if ($Hash.ContainsKey($a.LocalName)) {
                    switch ($Hash[$a.LocalName]) {
                        { $_ -eq [System.Data.DbType]::Boolean } {
                            if ([System.Xml.XmlConvert]::ToBoolean($a.Value)) {
                                $Writer.Write(', 1');
                            } else {
                                $Writer.Write(', 0');
                            }
                            break;
                        }
                        { $_ -eq [System.Data.DbType]::Binary } {
                            $Writer.Write(", X'");
                            $Writer.Write($a.Value);
                            $Writer.Write("'");
                            break;
                        }
                        { $_ -eq [System.Data.DbType]::DateTime } {
                            $Writer.Write(", '");
                            $Writer.Write([System.Xml.XmlConvert]::ToString([System.Xml.XmlConvert]::ToDateTime($a.Value, 'yyyy-MM-ddTHH:mm:ss'), 'yyyy-MM-dd HH:mm:ss'));
                            $Writer.Write("'");
                            break;
                        }
                        default {
                            $Writer.Write(", ");
                            $Writer.Write($a.Value);
                            break;
                        }
                    }
                } else {
                    $Writer.Write(", '");
                    $Writer.Write($a.Value);
                    $Writer.Write("'");
                }
            }
            if ($PSBoundParameters.ContainsKey('ParentName') -and $PSBoundParameters.ContainsKey('ParentId')) {
                $Writer.Write(", '");
                $Writer.Write($ParentId);
                $Writer.WriteLine("');");
            } else {
                $Writer.WriteLine(');');
            }
        }
    }
}

Function Import-FileSampleData {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.XmlElement]$Element,
        
        [Parameter(Mandatory = $true)]
        [System.IO.TextWriter]$Writer,

        [Parameter(Mandatory = $true)]
        [string]$ParentId
    )

    Process {
        Import-SampleData -Element $Element -Writer $Writer -ParentName 'ParentId' -ParentId $ParentId;
        $Element.SelectNodes('FileAccessError') | Import-SampleData -Writer $Writer -ParentName 'FileId' -ParentId $Element.Id -ErrorAction Stop;
    }
}

Function Import-CrawlConfigurationSampleData {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.XmlElement]$Element,
        
        [Parameter(Mandatory = $true)]
        [System.IO.TextWriter]$Writer,

        [Parameter(Mandatory = $true)]
        [string]$RootId
    )

    Process {
        Import-SampleData -Element $Element -Writer $Writer -ParentName 'RootId' -ParentId $RootId;
        $Element.SelectNodes('CrawlJobLog') | Import-SampleData -Writer $Writer -ParentName 'CrawlConfigurationId' -ParentId $Element.Id -ErrorAction Stop;
    }
}

Function Import-SubdirectorySampleData {
    [CmdletBinding(DefaultParameterSetName = 'Subdirectory')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.XmlElement]$Element,
        
        [Parameter(Mandatory = $true)]
        [System.IO.TextWriter]$Writer,

        [Parameter(Mandatory = $true, ParameterSetName = 'Volume')]
        [string]$VolumeId,

        [Parameter(Mandatory = $true, ParameterSetName = 'Subdirectory')]
        [string]$ParentId
    )

    Process {
        if ($PSBoundParameters.ContainsKey('VolumeId')) {
            Import-SampleData -Element $Element -Writer $Writer -ParentName 'VolumeId' -ParentId $VolumeId;
        } else {
            Import-SampleData -Element $Element -Writer $Writer -ParentName 'ParentId' -ParentId $ParentId;
        }
        $Element.SelectNodes('SubdirectoryAccessError') | Import-SampleData -Writer $Writer -ParentName 'SubdirectoryId' -ParentId $Element.Id -ErrorAction Stop;
        $Element.SelectNodes('Subdirectory') | Import-SubdirectorySampleData -Writer $Writer -ParentId $Element.Id -ErrorAction Stop;
        $Element.SelectNodes('File') | Import-FileSampleData -Writer $Writer -ParentId $Element.Id -ErrorAction Stop;
        $Element.SelectNodes('CrawlConfiguration') | Import-CrawlConfigurationSampleData -Writer $Writer -RootId $Element.Id -ErrorAction Stop;
    }
}

Function Import-VolumeSampleData {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.XmlElement]$Element,
        
        [Parameter(Mandatory = $true)]
        [System.IO.TextWriter]$Writer,

        [string]$FileSystemId
    )

    Process {
        Import-SampleData -Element $Element -Writer $Writer -ParentName 'FileSystemId' -ParentId $FileSystemId;
        $Element.SelectNodes('VolumeAccessError') | Import-SampleData -Writer $Writer -ParentName 'VolumeId' -ParentId $Element.Id -ErrorAction Stop;
        $Element.SelectNodes('Subdirectory') | Import-SubdirectorySampleData -Writer $Writer -VolumeId $Element.Id -ErrorAction Stop;
    }
}

Function Import-FileSystemSampleData {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.XmlElement]$Element,
        
        [Parameter(Mandatory = $true)]
        [System.IO.TextWriter]$Writer
    )

    Process {
        Import-SampleData -Element $Element -Writer $Writer;
        $Element.SelectNodes('SymbolicName') | Import-SampleData -Writer $Writer -ParentName 'FileSystemId' -ParentId $Element.Id -ErrorAction Stop;
        $Element.SelectNodes('Volume') | Import-VolumeSampleData -Writer $Writer -FileSystemId $Element.Id -ErrorAction Stop;
    }
}

$InputPath = $PSScriptRoot | Join-Path -ChildPath '..\FsInfoCat.UnitTests\Resources\SampleData.xml';
$OutputPath = $PSScriptRoot | Join-Path -ChildPath '..\FsInfoCat.Local\Resources\SampleData.sql';
$Xml = [Xml]::new();
$Xml.Load($InputPath);
if ($null -ne $Xml.DocumentElement) {
    #$Writer = [System.IO.StreamWriter]::new($OutputPath, $false, [System.Text.UTF8Encoding]::new($false, $false));
    $Writer = [System.IO.StringWriter]::new();
    try {
        $Xml.DocumentElement.SelectNodes('BinaryPropertySet') | Import-SampleData -Writer $Writer -ErrorAction Stop;
        $Xml.DocumentElement.SelectNodes('SummaryPropertySet') | Import-SampleData -Writer $Writer -ErrorAction Stop;
        $Xml.DocumentElement.SelectNodes('AudioPropertySet') | Import-SampleData -Writer $Writer -ErrorAction Stop;
        $Xml.DocumentElement.SelectNodes('DocumentPropertySet') | Import-SampleData -Writer $Writer -ErrorAction Stop;
        $Xml.DocumentElement.SelectNodes('DRMPropertySet') | Import-SampleData -Writer $Writer -ErrorAction Stop;
        $Xml.DocumentElement.SelectNodes('GPSPropertySet') | Import-SampleData -Writer $Writer -ErrorAction Stop;
        $Xml.DocumentElement.SelectNodes('ImagePropertySet') | Import-SampleData -Writer $Writer -ErrorAction Stop;
        $Xml.DocumentElement.SelectNodes('MediaPropertySet') | Import-SampleData -Writer $Writer -ErrorAction Stop;
        $Xml.DocumentElement.SelectNodes('MusicPropertySet') | Import-SampleData -Writer $Writer -ErrorAction Stop;
        $Xml.DocumentElement.SelectNodes('PhotoPropertySet') | Import-SampleData -Writer $Writer -ErrorAction Stop;
        $Xml.DocumentElement.SelectNodes('RecordedTVPropertySet') | Import-SampleData -Writer $Writer -ErrorAction Stop;
        $Xml.DocumentElement.SelectNodes('VideoPropertySet') | Import-SampleData -Writer $Writer -ErrorAction Stop;
        $Xml.DocumentElement.SelectNodes('PersonalTagDefinition') | Import-SampleData -Writer $Writer -ErrorAction Stop;
        $Xml.DocumentElement.SelectNodes('SharedTagDefinition') | Import-SampleData -Writer $Writer -ErrorAction Stop;
        $Xml.DocumentElement.SelectNodes('FileSystem') | Import-FileSystemSampleData -Writer $Writer -ErrorAction Stop;
        $Writer.Flush();
        $Writer.ToString();
    } finally {
        $Writer.Close();
    }
}