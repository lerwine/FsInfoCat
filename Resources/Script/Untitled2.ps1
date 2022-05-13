Add-Type -Path 'C:\Users\lerwi\Git\FsInfoCat\src\FsInfoCat\bin\Debug\net5.0\FsInfoCat.dll' -ErrorAction Stop;
Add-Type -Path 'C:\Users\lerwi\Git\FsInfoCat\src\FsInfoCat.Local\bin\Debug\net5.0\FsInfoCat.Local.dll' -ErrorAction Stop;
Add-Type -AssemblyName 'PresentationCore' -ErrorAction Stop;

Function Get-PropertyDefinitions {
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type[]]$Types,

        [AllowEmptyCollection()]
        [System.Collections.ObjectModel.Collection[Type]]$Ignore
    )

    Begin {
        $Emitted = $null;
        if ($PSBoundParameters.ContainsKey('Ignore')) {
            $Emitted = $Ignore;
        } else  {
            $Emitted = [System.Collections.ObjectModel.Collection[Type]]::new();
        }
    }

    Process {
        foreach ($t in $Types) {
            if (-not $Emitted.Contains($t)) {
                $Emitted.Add($t);
                $t.GetProperties() | Write-Output;
                if ($t.IsInterface) {
                    $Interfaces = $t.GetInterfaces();
                    if ($null -ne $Interfaces -and $Interfaces.Length -gt 0) {
                        foreach ($i in $Interfaces) {
                            Get-PropertyDefinitions -Types $i -Ignore $Emitted;
                        }
                    }
                }
            }
        }
    }
}

Function Get-ColumnVisibilityOptionsProperties {
    Param(
        [Parameter(Mandatory = $true)]
        [Type]$Interface,

        [Parameter(Mandatory = $true)]
        [string]$ClassName
    )

    $Ignore = @('IsChanged', 'Id', 'ExistingFileCount', 'CreatedOn', 'ModifiedOn');

    ((Get-PropertyDefinitions -Types ([FsInfoCat.IAudioPropertiesListItem])) | ForEach-Object {
        if ($Ignore -inotcontains $_.Name) {
            @"
        #region $($_.Name) Property Members

        /// <summary>
        /// Identifies the <see cref="$($_.Name)"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty $($_.Name)Property = DependencyPropertyBuilder<$ClassName, bool>
            .Register(nameof($($_.Name)))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as $ClassName)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool $($_.Name) { get => (bool)GetValue($($_.Name)Property); set => SetValue($($_.Name)Property, value); }

        #endregion
"@
        }
    }) | Out-String;
}

#$Code = Get-ColumnVisibilityOptionsProperties -Interface ([FsInfoCat.ICrawlConfigurationListItem]) -ClassName 'CrawlConfigPropertiesColumnVisibilityOptions<TEntity, TViewModel>';

$Code = ((Get-PropertyDefinitions -Types ([FsInfoCat.Local.ILocalRedundantSetListItem])) | ForEach-Object {
    if ($_.Name -ne 'IsChanged') {
        $DisplayAttribute = [System.Reflection.CustomAttributeExtensions]::GetCustomAttribute($_, [System.ComponentModel.DataAnnotations.DisplayAttribute]);
        $ShortName = $null;
        $DisplayName = $null;
        $Description = $null;
        if ($null -ne $DisplayAttribute) {
            $Description = $DisplayAttribute.GetDescription();
            $DisplayAttribute.GroupName;
            $ShortName = $DisplayAttribute.GetShortName();
            $DisplayName = $DisplayAttribute.GetName();
        }
        if ([string]::IsNullOrWhiteSpace($DisplayName)) {
            [System.ComponentModel.DisplayNameAttribute]$DisplayNameAttribute = [System.Reflection.CustomAttributeExtensions]::GetCustomAttribute($_, [System.ComponentModel.DisplayNameAttribute]);
            if ($null -ne $DisplayNameAttribute) {
                $DisplayName = $DisplayNameAttribute.DisplayName;
            }
        }
        if ([string]::IsNullOrWhiteSpace($Description)) {
            [System.ComponentModel.DescriptionAttribute]$DescriptionAttribute = [System.Reflection.CustomAttributeExtensions]::GetCustomAttribute($_, [System.ComponentModel.DescriptionAttribute]);
            if ($null -ne $DescriptionAttribute) {
                $Description = $DescriptionAttribute.Description;
            }
        }

        $UnderlyingType = $_.PropertyType;
        $TypeName = [Helpers.TypeExtensions]::ToCsTypeName($_.PropertyType, $true);
        if ($_.PropertyType.IsValueType -and $_.PropertyType.IsGenericType -and [Nullable`1].Equals($_.PropertyType.GetGenericTypeDefinition())) {
            $UnderlyingType = [Nullable]::GetUnderlyingType($_.PropertyType);
        }
        @"
        <Label ToolTip="" Content="$DisplayName" Padding="{DynamicResource DefaultSpacingTopLeftRight}" FontWeight="Bold"/>
        <TextBox Style="{DynamicResource MultiLineReadOnlyValueTextBox}" ToolTip="" Text="{Binding $_.Name}" Margin="{DynamicResource DefaultSpacingLeftRight}"/>
"@
    }
})
[System.Windows.Clipboard]::SetText($Code);
