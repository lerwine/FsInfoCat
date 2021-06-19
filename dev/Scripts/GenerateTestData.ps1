Add-Type -Path ($PSScriptRoot | Join-Path -ChildPath '..\..\FsInfoCat\FsInfoCat.Desktop\bin\Debug\net5.0-windows\Microsoft.Data.Sqlite.dll');
Add-Type -Path ($PSScriptRoot | Join-Path -ChildPath '..\..\FsInfoCat\FsInfoCat.Desktop\bin\Debug\net5.0-windows\Microsoft.WindowsAPICodePack.dll');
Add-Type -Path ($PSScriptRoot | Join-Path -ChildPath '..\..\FsInfoCat\FsInfoCat.Desktop\bin\Debug\net5.0-windows\Microsoft.WindowsAPICodePack.Shell.dll');

Function Expand-NestedTypes {
    [CmdletBinding()]
    Param(
        # Types to generate test stubs for.
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type]$InputType
    )

    Process {
        $Types = $InputType.GetNestedTypes();
        $InputType | Write-Output;
        if ($Types.Length -gt 0) { $Types | Expand-NestedTypes }
    }
}

Function Get-NestedNamespace {
    [CmdletBinding()]
    Param(
        # Types to generate test stubs for.
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type]$InputType
    )

    Process {
        if ($InputType.IsNested) {
            "$($InputType.DeclaringType | Get-NestedNamespace)::($($InputType.DeclaringType.Name))" | Write-Output;
        } else {
            if ($null -eq $InputType.Namespace) { '' | Write-Output } else { $InputType.Namespace | Write-Output }
        }
    }
}

if ($null -eq $Script:ProjectAssemblies) {
    Set-Variable -Name 'ProjectAssemblies' -Option Constant -Scope Script -Value (&{
        $Dictionary = [System.Collections.Generic.Dictionary[string,System.Collections.ObjectModel.ReadOnlyCollection[Type]]]::new();
        foreach ($g in ((Get-ChildItem -LiteralPath ($PSScriptRoot | Join-Path -ChildPath "..\..\FsInfoCat\FsInfoCat.UnitTests\bin\Debug\net5.0-windows7.0") -Filter '*.dll') | Where-Object {
            -not ($_.Name.StartsWith('Microsoft.TestPlatForm') -or $_.Name.StartsWith('Microsoft.VisualStudio') -or $_.Name.StartsWith('testhost'))
        } | ForEach-Object {
            (Add-Type -LiteralPath $_.FullName -ErrorAction Stop -PassThru) | Expand-NestedTypes | Where-Object { $_.IsPublic };
        } | Group-Object -Property @{ Expression = { $_.Assembly.GetName().Name } })) {
            $Dictionary.Add($g.Name, [System.Collections.ObjectModel.ReadOnlyCollection[Type]]::new(([Type[]]@($g.Group))));
        }
        return [System.Collections.ObjectModel.ReadOnlyDictionary[string,System.Collections.ObjectModel.ReadOnlyCollection[Type]]]::new($Dictionary);
    });

    Set-Variable -Name 'ProjectNamespaces' -Option Constant -Scope Script -Value (&{
        $Dictionary = [System.Collections.Generic.Dictionary[string,System.Collections.ObjectModel.ReadOnlyCollection[Type]]]::new();
        foreach ($g in ($Script:ProjectAssemblies.Keys | ForEach-Object { $Script:ProjectAssemblies[$_] | Write-Output } | Group-Object -Property @{ Expression = { $_ | Get-NestedNamespace } })) {
            $Dictionary.Add($g.Name, [System.Collections.ObjectModel.ReadOnlyCollection[Type]]::new(([Type[]]@($g.Group))));
        }
        return [System.Collections.ObjectModel.ReadOnlyDictionary[string,System.Collections.ObjectModel.ReadOnlyCollection[Type]]]::new($Dictionary);
    });
}

<#
Function New-AppUser {
    Param(
        [Parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true)]
        [string]$DisplayName,
        [Parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true)]
        [ValidateSet('None', 'Viewer', 'User', 'Crawler', 'Admin')]
        [string]$Role,
        [Parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true)]
        [Guid]$AccountID,
        [Parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true)]
        [string]$LoginName,
        [Parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true)]
        [DateTime]$CreatedOn,
        [Parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true)]
        [Guid]$CreatedBy,
        [Parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true)]
        [DateTime]$ModifiedOn,
        [Parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true)]
        [Guid]$ModifiedBy
    )

    New-Object -TypeName 'System.Management.Automation.PSObject' -Property $PSBoundParameters;
}

$Script:AppUserCollection = @(
    (New-AppUser -DisplayName 'FS InfoCat Administrator' -Role 'Admin' -AccountID ([Guid]::Empty) -LoginName 'admin' `
        -CreatedOn ([DateTime]::new(2014, 2, 14, 13, 41, 25, 171, [DateTimeKind]::Local)) -CreatedBy ([Guid]::Empty) `
        -ModifiedOn ([DateTime]::new(2014, 2, 14, 13, 41, 25, 171, [DateTimeKind]::Local)) -ModifiedBy ([Guid]::Empty)),
    (New-AppUser -DisplayName 'Leonard Erwine' -Role 'Admin' -AccountID ([Guid]::new("90b932df-3ab5-4299-a3f3-dd1655cbf93e")) -LoginName 'erwinel' `
        -CreatedOn ([DateTime]::new(2014, 6, 5, 14, 50, 37, 729, [DateTimeKind]::Local)) -CreatedBy ([Guid]::Empty) `
        -ModifiedOn ([DateTime]::new(2014, 6, 5, 14, 50, 37, 729, [DateTimeKind]::Local)) -ModifiedBy ([Guid]::Empty)),
    (New-AppUser -DisplayName 'Tanya Blackwell' -Role 'Admin' -AccountID ([Guid]::new("7050184e-a998-4088-b547-d70cd806b2c7")) -LoginName 'blackwellt' `
        -CreatedOn ([DateTime]::new(2014, 9, 22, 14, 21, 47, 861, [DateTimeKind]::Local)) -CreatedBy ([Guid]::Empty) `
        -ModifiedOn ([DateTime]::new(2014, 9, 22, 14, 21, 47, 861, [DateTimeKind]::Local)) -ModifiedBy ([Guid]::Empty)),
    (New-AppUser -DisplayName 'Nur Murillo' -Role 'Crawler' -AccountID ([Guid]::new("96077af4-e35d-45b5-9094-02213cd0ba80")) -LoginName 'murillon' `
        -CreatedOn ([DateTime]::new(2015, 1, 8, 8, 19, 32, 381, [DateTimeKind]::Local)) -CreatedBy ([Guid]::new("7050184e-a998-4088-b547-d70cd806b2c7")) `
        -ModifiedOn ([DateTime]::new(2015, 8, 3, 16, 45, 24, 631, [DateTimeKind]::Local)) -ModifiedBy ([Guid]::new("90b932df-3ab5-4299-a3f3-dd1655cbf93e"))),
    (New-AppUser -DisplayName 'Hubert Davies' -Role 'Crawler' -AccountID ([Guid]::new("0cf932e4-be15-4797-84ec-3eb9d86a9376")) -LoginName 'daviesh' `
        -CreatedOn ([DateTime]::new(2015, 4, 22, 16, 11, 52, 39, [DateTimeKind]::Local)) -CreatedBy ([Guid]::new("7050184e-a998-4088-b547-d70cd806b2c7")) `
        -ModifiedOn ([DateTime]::new(2020, 3, 17, 16, 9, 49, 842, [DateTimeKind]::Local)) -ModifiedBy ([Guid]::Empty)),
    (New-AppUser -DisplayName 'Uma Graham' -Role 'Crawler' -AccountID ([Guid]::new("8389a46a-0617-4180-80fd-9f3719b77b71")) -LoginName 'grahamu' `
        -CreatedOn ([DateTime]::new(2015, 11, 15, 16, 11, 29, 725, [DateTimeKind]::Local)) -CreatedBy ([Guid]::new("7050184e-a998-4088-b547-d70cd806b2c7")) `
        -ModifiedOn ([DateTime]::new(2015, 11, 15, 16, 11, 29, 725, [DateTimeKind]::Local)) -ModifiedBy ([Guid]::new("7050184e-a998-4088-b547-d70cd806b2c7"))),
    (New-AppUser -DisplayName 'Mae Mcleod' -Role 'None' -AccountID ([Guid]::new("79010060-4bb5-47b8-ad33-7f92623a5f3e")) -LoginName 'mcleodm' `
        -CreatedOn ([DateTime]::new(2016, 2, 23, 16, 35, 3, 408, [DateTimeKind]::Local)) -CreatedBy ([Guid]::new("90b932df-3ab5-4299-a3f3-dd1655cbf93e")) `
        -ModifiedOn ([DateTime]::new(2020, 7, 22, 15, 6, 58, 108, [DateTimeKind]::Local)) -ModifiedBy ([Guid]::Empty)),
    (New-AppUser -DisplayName 'Moses Conner' -Role 'User' -AccountID ([Guid]::new("71fffce1-ed11-4efe-92fe-7096c27a4665")) -LoginName 'connerm' `
        -CreatedOn ([DateTime]::new(2016, 5, 31, 16, 16, 23, 404, [DateTimeKind]::Local)) -CreatedBy ([Guid]::new("79010060-4bb5-47b8-ad33-7f92623a5f3e")) `
        -ModifiedOn ([DateTime]::new(2016, 5, 31, 16, 16, 23, 404, [DateTimeKind]::Local)) -ModifiedBy ([Guid]::new("79010060-4bb5-47b8-ad33-7f92623a5f3e"))),
    (New-AppUser -DisplayName 'Ayaan Mercado' -Role 'User' -AccountID ([Guid]::new("ef412251-042e-4ef3-8afb-5afef51dc66f")) -LoginName 'mercadoa' `
        -CreatedOn ([DateTime]::new(2016, 9, 5, 16, 37, 18, 775, [DateTimeKind]::Local)) -CreatedBy ([Guid]::new("79010060-4bb5-47b8-ad33-7f92623a5f3e")) `
        -ModifiedOn ([DateTime]::new(2016, 9, 5, 16, 37, 18, 775, [DateTimeKind]::Local)) -ModifiedBy ([Guid]::new("79010060-4bb5-47b8-ad33-7f92623a5f3e"))),
    (New-AppUser -DisplayName 'Roberto Mccabe' -Role 'Crawler' -AccountID ([Guid]::new("ad3c3d3d-6675-4f21-be3e-16e4027194a5")) -LoginName 'mccaber' `
        -CreatedOn ([DateTime]::new(2016, 12, 9, 16, 2, 13, 128, [DateTimeKind]::Local)) -CreatedBy ([Guid]::new("8389a46a-0617-4180-80fd-9f3719b77b71")) `
        -ModifiedOn ([DateTime]::new(2020, 12, 29, 16, 43, 55, 384, [DateTimeKind]::Local)) -ModifiedBy ([Guid]::Empty)),
    (New-AppUser -DisplayName 'Daria Nicholls' -Role 'None' -AccountID ([Guid]::new("19bb64d7-3f9f-4ff5-ae29-089b2f74a991")) -LoginName 'nichollsd' `
        -CreatedOn ([DateTime]::new(2017, 3, 13, 16, 5, 32, 768, [DateTimeKind]::Local)) -CreatedBy ([Guid]::new("8389a46a-0617-4180-80fd-9f3719b77b71")) `
        -ModifiedOn ([DateTime]::new(2017, 12, 12, 16, 58, 48, 67, [DateTimeKind]::Local)) -ModifiedBy ([Guid]::new("90b932df-3ab5-4299-a3f3-dd1655cbf93e"))),
    (New-AppUser -DisplayName 'Affan Hebert' -Role 'User' -AccountID ([Guid]::new("e642c8a8-c020-40a9-85d5-b5fa41c8a74e")) -LoginName 'heberta' `
        -CreatedOn ([DateTime]::new(2017, 6, 13, 16, 35, 56, 515, [DateTimeKind]::Local)) -CreatedBy ([Guid]::new("ad3c3d3d-6675-4f21-be3e-16e4027194a5")) `
        -ModifiedOn ([DateTime]::new(2020, 10, 17, 16, 2, 29, 736, [DateTimeKind]::Local)) -ModifiedBy ([Guid]::new("7050184e-a998-4088-b547-d70cd806b2c7"))),
    (New-AppUser -DisplayName 'Lucian Firth' -Role 'User' -AccountID ([Guid]::new("c60904fe-8bae-4300-80dd-679dd174db42")) -LoginName 'firthl' `
        -CreatedOn ([DateTime]::new(2017, 9, 12, 16, 35, 58, 453, [DateTimeKind]::Local)) -CreatedBy ([Guid]::new("19bb64d7-3f9f-4ff5-ae29-089b2f74a991")) `
        -ModifiedOn ([DateTime]::new(2017, 9, 12, 16, 35, 58, 453, [DateTimeKind]::Local)) -ModifiedBy ([Guid]::new("19bb64d7-3f9f-4ff5-ae29-089b2f74a991"))),
    (New-AppUser -DisplayName 'Dennis Little' -Role 'Viewer' -AccountID ([Guid]::new("ff79cdf6-0e76-4a9a-b6c1-a65537d57082")) -LoginName 'littled' `
        -CreatedOn ([DateTime]::new(2018, 3, 12, 9, 19, 25, 554, [DateTimeKind]::Local)) -CreatedBy ([Guid]::new("ef412251-042e-4ef3-8afb-5afef51dc66f")) `
        -ModifiedOn ([DateTime]::new(2018, 3, 12, 9, 19, 25, 554, [DateTimeKind]::Local)) -ModifiedBy ([Guid]::new("ef412251-042e-4ef3-8afb-5afef51dc66f"))),
    (New-AppUser -DisplayName 'Martyna Ortiz' -Role 'Admin' -AccountID ([Guid]::new("19cffdda-94bb-40be-bfb9-198e822831c9")) -LoginName 'ortizm' `
        -CreatedOn ([DateTime]::new(2018, 6, 9, 16, 9, 36, 915, [DateTimeKind]::Local)) -CreatedBy ([Guid]::new("ff79cdf6-0e76-4a9a-b6c1-a65537d57082")) `
        -ModifiedOn ([DateTime]::new(2018, 6, 9, 16, 9, 36, 915, [DateTimeKind]::Local)) -ModifiedBy ([Guid]::new("ff79cdf6-0e76-4a9a-b6c1-a65537d57082"))),
    (New-AppUser -DisplayName 'Luc Pennington' -Role 'Crawler' -AccountID ([Guid]::new("7511575a-4b07-4a58-be0e-3b36033d56db")) -LoginName 'penningtonl' `
        -CreatedOn ([DateTime]::new(2018, 9, 5, 9, 45, 21, 156, [DateTimeKind]::Local)) -CreatedBy ([Guid]::Empty) `
        -ModifiedOn ([DateTime]::new(2020, 12, 16, 16, 0, 13, 569, [DateTimeKind]::Local)) -ModifiedBy ([Guid]::new("7050184e-a998-4088-b547-d70cd806b2c7"))),
    (New-AppUser -DisplayName 'Hajra Mcmahon' -Role 'Viewer' -AccountID ([Guid]::new("c1faca99-ade5-4ece-a3e3-1672b34facb0")) -LoginName 'mcmahonh' `
        -CreatedOn ([DateTime]::new(2018, 12, 1, 10, 32, 34, 315, [DateTimeKind]::Local)) -CreatedBy ([Guid]::new("8389a46a-0617-4180-80fd-9f3719b77b71")) `
        -ModifiedOn ([DateTime]::new(2018, 12, 1, 10, 32, 34, 315, [DateTimeKind]::Local)) -ModifiedBy ([Guid]::new("8389a46a-0617-4180-80fd-9f3719b77b71"))),
    (New-AppUser -DisplayName 'Kaiden Reynolds' -Role 'Crawler' -AccountID ([Guid]::new("b511096c-af4e-47e3-9de0-d3fb7aa7a09f")) -LoginName 'reynoldsk' `
        -CreatedOn ([DateTime]::new(2019, 2, 23, 16, 53, 3, 178, [DateTimeKind]::Local)) -CreatedBy ([Guid]::new("90b932df-3ab5-4299-a3f3-dd1655cbf93e")) `
        -ModifiedOn ([DateTime]::new(2020, 9, 6, 16, 47, 43, 817, [DateTimeKind]::Local)) -ModifiedBy ([Guid]::new("b511096c-af4e-47e3-9de0-d3fb7aa7a09f"))),
    (New-AppUser -DisplayName 'Samiyah Chandler' -Role 'User' -AccountID ([Guid]::new("0867eee2-d7a7-4714-9c35-568739c03a93")) -LoginName 'chandlers' `
        -CreatedOn ([DateTime]::new(2019, 5, 17, 16, 38, 8, 98, [DateTimeKind]::Local)) -CreatedBy ([Guid]::new("e642c8a8-c020-40a9-85d5-b5fa41c8a74e")) `
        -ModifiedOn ([DateTime]::new(2019, 5, 17, 16, 38, 8, 98, [DateTimeKind]::Local)) -ModifiedBy ([Guid]::new("e642c8a8-c020-40a9-85d5-b5fa41c8a74e"))),
    (New-AppUser -DisplayName 'Carwyn Leblanc' -Role 'Viewer' -AccountID ([Guid]::new("59370d4e-78ad-4352-9188-b5bb8162356a")) -LoginName 'leblancc' `
        -CreatedOn ([DateTime]::new(2019, 8, 6, 9, 40, 33, 585, [DateTimeKind]::Local)) -CreatedBy ([Guid]::new("b511096c-af4e-47e3-9de0-d3fb7aa7a09f")) `
        -ModifiedOn ([DateTime]::new(2020, 5, 27, 16, 39, 19, 526, [DateTimeKind]::Local)) -ModifiedBy ([Guid]::new("7050184e-a998-4088-b547-d70cd806b2c7"))),
    (New-AppUser -DisplayName 'Sabrina Fraser' -Role 'Viewer' -AccountID ([Guid]::new("18defc9b-7b07-4df6-934f-97903c5c0df2")) -LoginName 'frasers' `
        -CreatedOn ([DateTime]::new(2019, 10, 21, 16, 13, 28, 232, [DateTimeKind]::Local)) -CreatedBy ([Guid]::new("0867eee2-d7a7-4714-9c35-568739c03a93")) `
        -ModifiedOn ([DateTime]::new(2019, 10, 21, 16, 13, 28, 232, [DateTimeKind]::Local)) -ModifiedBy ([Guid]::new("0867eee2-d7a7-4714-9c35-568739c03a93"))),
    (New-AppUser -DisplayName 'Woodrow Lindsay' -Role 'User' -AccountID ([Guid]::new("7e2cd7b2-32de-420e-8ac2-6ab9a1c735c7")) -LoginName 'lindsayw' `
        -CreatedOn ([DateTime]::new(2020, 1, 5, 16, 46, 22, 951, [DateTimeKind]::Local)) -CreatedBy ([Guid]::new("b511096c-af4e-47e3-9de0-d3fb7aa7a09f")) `
        -ModifiedOn ([DateTime]::new(2020, 11, 18, 16, 14, 4, 945, [DateTimeKind]::Local)) -ModifiedBy ([Guid]::new("7e2cd7b2-32de-420e-8ac2-6ab9a1c735c7")))
);

Function Out-AppUsers {
    [CmdletBinding()]
    Param ()

    $Random = [Random]::new();
    foreach ($AppUser in $Script:AppUserCollection) {
        $AccountID = 'Guid.Empty';
        if (-not $AppUser.AccountID.Equals([Guid]::Empty)) { $AccountID = "new Guid(`"$($AppUser.AccountID.ToString('d'))`")" }
        $CreatedBy = 'Guid.Empty';
        if (-not $AppUser.CreatedBy.Equals([Guid]::Empty)) { $CreatedBy = "new Guid(`"$($AppUser.CreatedBy.ToString('d'))`")" }
        $ModifiedBy = 'Guid.Empty';
        if (-not $AppUser.ModifiedBy.Equals([Guid]::Empty)) { $ModifiedBy = "new Guid(`"$($AppUser.ModifiedBy.ToString('d'))`")" }
        $d = $AppUser.CreatedOn;
        $CreatedOnText = $CreatedOnNormalized = "new DateTime($($d.Year), $($d.Month), $($d.Day), $($d.Hour), $($d.Minute), $($d.Second), $($d.Millisecond), DateTimeKind.Local)";
        switch ($Random.Next(5)) {
            0 {
                $d = $AppUser.CreatedOn.ToUniversalTime();
                $CreatedOnText = "new DateTime($($d.Year), $($d.Month), $($d.Day), $($d.Hour), $($d.Minute), $($d.Second), $($d.Millisecond), DateTimeKind.Utc)";
                break;
            }
            1 {
                $CreatedOnText = "new DateTime($($d.Year), $($d.Month), $($d.Day), $($d.Hour), $($d.Minute), $($d.Second), $($d.Millisecond), DateTimeKind.Unspecified)";
                break;
            }
        }
        $d = $AppUser.ModifiedOn;
        $ModifiedOnText = $ModifiedOnNormalized = "new DateTime($($d.Year), $($d.Month), $($d.Day), $($d.Hour), $($d.Minute), $($d.Second), $($d.Millisecond), DateTimeKind.Local)";
        switch ($Random.Next(5)) {
            0 {
                $d = $AppUser.ModifiedOn.ToUniversalTime();
                $ModifiedOnText = "new DateTime($($d.Year), $($d.Month), $($d.Day), $($d.Hour), $($d.Minute), $($d.Second), $($d.Millisecond), DateTimeKind.Utc)";
                break;
            }
            1 {
                $ModifiedOnText = "new DateTime($($d.Year), $($d.Month), $($d.Day), $($d.Hour), $($d.Minute), $($d.Second), $($d.Millisecond), DateTimeKind.Unspecified)";
                break;
            }
        }

    @"
                new object[]
                {
                    new AppUser(new Account { AccountID = $AccountID, DisplayName = "$($AppUser.DisplayName)", LoginName = "$($AppUser.LoginName)",
                        Role = UserRole.$($AppUser.Role), Notes = "",
                        CreatedOn = $CreatedOnText, CreatedBy = $CreatedBy,
                        ModifiedOn = $ModifiedOnText, ModifiedBy = $ModifiedBy }),
                    new AppUser(new Account { AccountID = $AccountID, DisplayName = "$($AppUser.DisplayName)", LoginName = "$($AppUser.LoginName)",
                        Role = UserRole.$($AppUser.Role), Notes = "",
                        CreatedOn = $CreatedOnNormalized, CreatedBy = $CreatedBy,
                        ModifiedOn = $ModifiedOnNormalized, ModifiedBy = $ModifiedBy }),
                },
"@

    }
}
#>

Function ConvertTo-NormalizedDate {
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [DateTime]$Value
    )

    Process {
        if ($Value -gt $Value.Date.AddHours(17)) {
            $Value = $Value.Subtract([TimeSpan]::FromHours($Value.Hour - 16));
        } else {
            if ($Value -lt $Value.Date.AddHours(8)) {
                $Value = $Value.Subtract([TimeSpan]::FromHours($Value.Hour + 8));
            }
        }
        if ($Now.DayOfWeek -eq [DayOfWeek]::Saturday) {
            $Value.Subtract([TimeSpan]::FromDays(1.0));
        } else {
            if ($Now.DayOfWeek -eq [DayOfWeek]::Sunday) {
                $Value.Subtract([TimeSpan]::FromDays(2.0));
            } else {
                $Value;
            }
        }
    }
}

Function Read-TargetType {
    [CmdletBinding()]
    Param()
    [Type[]]$Types = @();
    [PSCustomObject[]]$SelectionList = @();
    [string[]]$Names = $Script:ProjectAssemblies.Keys | Out-GridView -Title 'Pick assembly(s) (select none for all assemblies)' -OutputMode Multiple;
    if ($Names.Length -eq 0) {
        [string[]]$Names = $Script:ProjectNamespaces.Key | Out-GridView -Title 'Pick namespace(s) (select none for all namespaces)' -OutputMode Multiple;
        if ($Names.Length -eq 0) {
            $Types = @($Script:ProjectAssemblies.Keys | ForEach-Object { $Script:ProjectAssemblies[$_] | Write-Output });
            [PSCustomObject[]]$SelectionList = @($Types | Select-Object -Property 'Name', @{ Label = 'BaseType'; Expression = {
                if ($_.IsEnum) {
                    '(enum)';
                } else {
                    if ($_.Equals([System.ValueType])) {
                        '(struct)'
                    } else {
                        if ($_.Equals([System.Object])) {
                            '(object)'
                        } else {
                            $_.FullName;
                        }
                    }
                }
            } }, @{ Label = 'Namespace'; Expression = { Get-NestedNamespace -InputType $_ } }, @{ Label = 'Assembly'; Expression = { $_.Assembly.GetName().Name } }, 'IsAbstract', 'IsCollectible', 'IsCOMObject',
                'IsInterface');
        } else {
            [Type[]]$Types = @($Names | ForEach-Object { $Script:ProjectNamespaces[$_] });
            if ($Names.Count -gt 1) {
                [PSCustomObject[]]$SelectionList = @($Types | Select-Object -Property 'Name', @{ Label = 'BaseType'; Expression = {
                    if ($_.IsEnum) {
                        '(enum)';
                    } else {
                        if ($_.Equals([System.ValueType])) {
                            '(struct)'
                        } else {
                            if ($_.Equals([System.Object])) {
                                '(object)'
                            } else {
                                $_.FullName;
                            }
                        }
                    }
                } }, @{ Label = 'Namespace'; Expression = { Get-NestedNamespace -InputType $_ } }, @{ Label = 'Assembly'; Expression = { $_.Assembly.GetName().Name } }, 'IsAbstract', 'IsCollectible', 'IsCOMObject',
                    'IsInterface');
            } else {
                [PSCustomObject[]]$SelectionList = @($Types | Select-Object -Property 'Name', @{ Label = 'BaseType'; Expression = {
                    if ($_.IsEnum) {
                        '(enum)';
                    } else {
                        if ($_.Equals([System.ValueType])) {
                            '(struct)'
                        } else {
                            if ($_.Equals([System.Object])) {
                                '(object)'
                            } else {
                                $_.FullName;
                            }
                        }
                    }
                } }, @{ Label = 'Assembly'; Expression = { $_.Assembly.GetName().Name } }, 'IsAbstract', 'IsCollectible', 'IsCOMObject',
                    'IsInterface');
            }
        }
    } else {
        [Type[]]$Types = @($Names | ForEach-Object { $Script:ProjectAssemblies[$_] });
        if ($Names.Count -gt 1) {
            [PSCustomObject[]]$SelectionList = @($Types | Select-Object -Property 'Name', @{ Label = 'BaseType'; Expression = {
                if ($_.IsEnum) {
                    '(enum)';
                } else {
                    if ($_.Equals([System.ValueType])) {
                        '(struct)'
                    } else {
                        if ($_.Equals([System.Object])) {
                            '(object)'
                        } else {
                            $_.FullName;
                        }
                    }
                }
            } }, @{ Label = 'Namespace'; Expression = { Get-NestedNamespace -InputType $_ } }, @{ Label = 'Assembly'; Expression = { $_.Assembly.GetName().Name } }, 'IsAbstract', 'IsCollectible', 'IsCOMObject',
                'IsInterface');
        } else {
            [PSCustomObject[]]$SelectionList = @($Types | Select-Object -Property 'Name', @{ Label = 'BaseType'; Expression = {
                if ($_.IsEnum) {
                    '(enum)';
                } else {
                    if ($_.Equals([System.ValueType])) {
                        '(struct)'
                    } else {
                        if ($_.Equals([System.Object])) {
                            '(object)'
                        } else {
                            $_.FullName;
                        }
                    }
                }
            } }, @{ Label = 'Namespace'; Expression = { Get-NestedNamespace -InputType $_ } }, 'IsAbstract', 'IsCollectible', 'IsCOMObject',
                'IsInterface');
        }
    }

    $PickedItems = $SelectionList | Out-GridView -Title 'Select target type(s)' -OutputMode Multiple;
    $PickedItems | ForEach-Object { $Types[$SelectionList.IndexOf($_)] }
}

Function Add-TypeNamespaces {
    [CmdletBinding()]
    Param(
        # Type to extract namespace from.
        [Parameter(Mandatory = $true)]
        [AllowEmptyCollection()]
        [System.Collections.ObjectModel.Collection[string]]$Namespaces,
        # Type to extract namespace from.
        [Parameter(Mandatory = $true, ValueFromPipeline = $true, ParameterSetName = 'ByType')]
        [Type]$InputType,
        # Method to extract namespace from.
        [Parameter(Mandatory = $true, ParameterSetName = 'ByMethod')]
        [System.Reflection.MethodInfo]$Method,
        # Method to extract namespace from.
        [Parameter(Mandatory = $true, ParameterSetName = 'ByConstructor')]
        [System.Reflection.ConstructorInfo]$Constructor,
        [Parameter(ParameterSetName = 'ByType')]
        [switch]$IncludeMembers,
        [AllowEmptyCollection()]
        [System.Collections.ObjectModel.Collection[System.Reflection.MemberInfo]]$Exclude
    )

    Begin {
        $ExcludeMemberInfos = $Exclude;
        if (-not $PSBoundParameters.ContainsKey('Exclude')) { $ExcludeMemberInfos = [System.Collections.ObjectModel.Collection[System.Reflection.MemberInfo]]::new() }
    }
    Process {
        switch ($PsCmdlet.ParameterSetName) {
            'ByMethod' {
                $ExcludeMemberInfos.Add($Method);
                if (-not $ExcludeMemberInfos.Contains($Method.ReturnType)) {
                    Add-TypeNamespaces -Namespaces $Namespaces -InputType $Method.ReturnType -Exclude $ExcludeMemberInfos;
                }
                foreach ($MethodParm in $Method.GetParameters()) {
                    if (-not $ExcludeMemberInfos.Contains($MethodParm.ParameterType)) {
                        Add-TypeNamespaces -Namespaces $Namespaces -InputType $MethodParm.ParameterType -Exclude $ExcludeMemberInfos;
                    }
                }
                if ($Method.ContainsGenericParameters) {
                    if ($Method.IsGenericMethodDefinition) {
                        foreach ($g in $Method.GetGenericArguments()) {
                            Add-TypeNamespaces -Namespaces $Namespaces -InputType $g -Exclude $ExcludeMemberInfos;
                        }
                    } else {
                        foreach ($g in $Method.GetGenericArguments()) {
                            if (-not $ExcludeMemberInfos.Contains($g)) {
                                Add-TypeNamespaces -Namespaces $Namespaces -InputType $g -Exclude $ExcludeMemberInfos;
                            }
                        }
                    }
                }
                break;
            }
            'ByConstructor' {
                $ExcludeMemberInfos.Add($Constructor);
                foreach ($ConstructorParm in $Constructor.GetParameters()) {
                    if (-not $ExcludeMemberInfos.Contains($ConstructorParm.ParameterType)) {
                        Add-TypeNamespaces -Namespaces $Namespaces -InputType $ConstructorParm.ParameterType -Exclude $ExcludeMemberInfos;
                    }
                }
            }
            Default {
                $ExcludeMemberInfos.Add($InputType);
                if ($InputType.HasElementType) {
                    if ($IncludeMembers.IsPresent) {
                        Add-TypeNamespaces -Namespaces $Namespaces -InputType $InputType.GetElementType() -Exclude $ExcludeMemberInfos -IncludeMembers;
                    } else {
                        Add-TypeNamespaces -Namespaces $Namespaces -InputType $InputType.GetElementType() -Exclude $ExcludeMemberInfos;
                    }
                } else {
                    if ($InputType.IsNested) {
                        Add-TypeNamespaces -Namespaces $Namespaces -InputType $InputType.DeclaringType -Exclude $ExcludeMemberInfos;
                    } else {
                        if (-not ([string]::IsNullOrEmpty($InputType.Namespace) -or $Namespaces.Contains($InputType.Namespace))) { $Namespaces.Add($InputType.Namespace) }
                    }
                    if ($InputType.IsGenericTypeDefinition) {
                        foreach ($g in $InputType.GetGenericArguments()) {
                            Add-TypeNamespaces -Namespaces $Namespaces -InputType $g -Exclude $ExcludeMemberInfos;
                        }
                    } else {
                        if ($InputType.IsGenericType) {
                            foreach ($g in $InputType.GetGenericArguments()) {
                                if (-not $ExcludeMemberInfos.Contains($g)) {
                                    Add-TypeNamespaces -Namespaces $Namespaces -InputType $g -Exclude $ExcludeMemberInfos;
                                }
                            }
                        }
                    }
                    if ($InputType.IsGenericParameter) {
                        foreach ($g in $InputType.GetGenericParameterConstraints()) {
                            if (-not $ExcludeMemberInfos.Contains($g)) {
                                Add-TypeNamespaces -Namespaces $Namespaces -InputType $g -Exclude $ExcludeMemberInfos;
                            }
                        }
                    }
                    if ($IncludeMembers) {
                        foreach ($PropertyInfo in $InputType.GetProperties())
                        {
                            if (-not $ExcludeMemberInfos.Contains($PropertyInfo)) {
                                $ExcludeMemberInfos.Add($PropertyInfo);
                                if (-not $ExcludeMemberInfos.Contains($PropertyInfo.PropertyType)) {
                                    Add-TypeNamespaces -Namespaces $Namespaces -InputType $PropertyInfo.PropertyType -Exclude $ExcludeMemberInfos;
                                }
                                foreach ($IndexParm in $PropertyInfo.GetIndexParameters()) {
                                    if (-not $ExcludeMemberInfos.Contains($IndexParm.ParameterType)) {
                                        Add-TypeNamespaces -Namespaces $Namespaces -InputType $IndexParm.ParameterType -Exclude $ExcludeMemberInfos;
                                    }
                                }
                            }
                        }
                        foreach ($m in $InputType.GetMethods()) {
                            if (-not $ExcludeMemberInfos.Contains($m)) {
                                Add-TypeNamespaces -Namespaces $Namespaces -Method $m -Exclude $ExcludeMemberInfos;
                            }
                        }
                        foreach ($c in $InputType.GetConstructors()) {
                            if (-not $ExcludeMemberInfos.Contains($c)) {
                                Add-TypeNamespaces -Namespaces $Namespaces -Constructor $c -Exclude $ExcludeMemberInfos;
                            }
                        }
                    }
                }
            }
        }
    }
}

Function Build-TestStub {
    [CmdletBinding()]
    Param(
        # Types to generate test stubs for.
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type]$InputType
    )


    Begin {
        $LcFirstRegex = [System.Text.RegularExpressions.Regex]::new('^[A-Z]([A-Z](?=[^a-z]))*');
        $UcFirstRegex = [System.Text.RegularExpressions.Regex]::new('^[a-z]');
        $ToLowerSb = { $args[0].Value.ToLower() };
        $ToUpperSb = { $args[0].Value.ToUpper() };
    }
    Process {
        $TestClassName = "$($UcFirstRegex.Replace($InputType.Name, $ToUpperSb).Replace('`', '_'))Tests";
        if ($InputType.IsNested) {
            $t = $InputType;
            do {
                $t = $t.DeclaringType;
                $TestClassName = "$($UcFirstRegex.Replace($t.Name, $ToUpperSb).Replace('`', '_'))$TestClassName";
            } while ($t.IsNested);
        }
        $OutputPath = ($PSScriptRoot | Join-Path -ChildPath "..\..\FsInfoCat\FsInfoCat.UnitTests") | Join-Path -ChildPath "$TestClassName.cs";
        if ($OutputPath | Test-Path) {
            $i = 0;
            $n = $TestClassName;
            do {
                $i++;
                $TestClassName = "$n$i";
                $OutputPath = ($PSScriptRoot | Join-Path -ChildPath "..\..\FsInfoCat\FsInfoCat.UnitTests") | Join-Path -ChildPath "$TestClassName.cs";
            } while ($OutputPath | Test-Path);
        }
        $Namespaces = [System.Collections.ObjectModel.Collection[string]]::new();
        $Namespaces.Add('Microsoft.VisualStudio.TestTools.UnitTesting');
        Add-TypeNamespaces -Namespaces $Namespaces -InputType $InputType -IncludeMembers;
        $StringWriter = [System.IO.StringWriter]::new();
        $Namespaces | Sort-Object | ForEach-Object { $StringWriter.WriteLine("using $_;") }
        $StringWriter.WriteLine('');
        $StringWriter.WriteLine("namespace FsInfoCat.UnitTests");
        $StringWriter.WriteLine("{");
        $StringWriter.WriteLine("    [TestClass]");
        $StringWriter.WriteLine("    public class $TestClassName");
        $StringWriter.WriteLine("    {");
        $StringWriter.WriteLine("        private static TestContext _testContext;");
        $StringWriter.WriteLine("");
        $StringWriter.WriteLine("        [ClassInitialize]");
        $StringWriter.WriteLine("        public static void OnClassInitialize(TestContext testContext)");
        $StringWriter.WriteLine("        {");
        $StringWriter.WriteLine("            _testContext = testContext;");
        $StringWriter.WriteLine("        }");
        $MethodNames = [System.Collections.ObjectModel.Collection[string]]::new();
        $MethodDescriptions = [System.Collections.ObjectModel.Collection[string]]::new();
        $PropertyInfoArray = $InputType.GetProperties();
        foreach ($ConstructorInfo in $InputType.GetConstructors()) {
            Write-Information -MessageData "Building code for constructor $ConstructorInfo" -InformationAction Continue;
            $Description = "new $([FsInfoCat.Services]::ToCsTypeName($InputType, $true) -replace '`\d*$', '')";
            $Name = "New$($InputType.Name -replace '`\d*$', '')";
            $cp = $ConstructorInfo.GetParameters();
            foreach ($p in $cp) {
                $Name = "$Name$($UcFirstRegex.Replace($p.ParameterType.Name, $ToUpperSb) -replace '`\d*$', '')";
            }
            $Name = "$($Name)TestMethod";
            if ($cp.Length -gt 0) {
                $Description = "$Description($(($cp | ForEach-Object { [FsInfoCat.Services]::ToCsTypeName($_.ParameterType, $true) }) -join ', '))";
            } else {
                $Description = "$Description()";
            }
            if ($MethodNames.Contains($Name)) {
                $i = 1;
                $n = $Name;
                do {
                    $i++;
                    $Name = "$n$i";
                } while ($MethodNames.Contains($Name));
            }
            $MethodNames.Add($Name);
            if ($MethodDescriptions.Contains($Description)) {
                $i = 1;
                $d = $Description;
                do {
                    $i++;
                    $Description = "$d #$i";
                } while ($MethodDescriptions.Contains($Description));
            }
            $MethodDescriptions.Add($Description);
            $StringWriter.WriteLine('');
            $StringWriter.WriteLine("        [TestMethod(`"$Description`")]");
            $StringWriter.WriteLine("        public void $Name()");
            $StringWriter.WriteLine('        {');
            $StringWriter.WriteLine('            Assert.Inconclusive("Test not implemented");');
            $StringWriter.WriteLine("            // TODO: Implement test for $Description");
            $StringWriter.WriteLine('');
            foreach ($p in $cp) {
                $StringWriter.WriteLine("            $([FsInfoCat.Services]::ToCsTypeName($p.ParameterType, $true)) $($LcFirstRegex.Replace($p.Name, $ToLowerSb))Arg = default;");
            }
            foreach ($PropertyInfo in $PropertyInfoArray) {
                if ($PropertyInfo.GetIndexParameters().Length -eq 0 -and $PropertyInfo.CanRead -and $null -ne $PropertyInfo.GetGetMethod()) {
                    $StringWriter.WriteLine("            $([FsInfoCat.Services]::ToCsTypeName($PropertyInfo.PropertyType, $true)) expected$( $UcFirstRegex.Replace($PropertyInfo.Name, $ToUpperSb)) = default;");
                }
            }
            if ($PropertyInfoArray.Count -gt 0 -or $cp.Length -gt 0) {
                $StringWriter.WriteLine('');
            }
            $StringWriter.Write("            $([FsInfoCat.Services]::ToCsTypeName($InputType, $true)) target = new(");
            if ($cp.Length -gt 0) {
                $StringWriter.WriteLine('');
                $StringWriter.Write("$($LcFirstRegex.Replace($cp[0].Name, $ToLowerSb))Arg");
                foreach ($p in ($cp | Select-Object -Skip 1)) {
                    $StringWriter.Write(", $($LcFirstRegex.Replace($p.Name, $ToLowerSb))Arg");
                }
            }
            $StringWriter.WriteLine(");");
            $StringWriter.WriteLine('');
            foreach ($PropertyInfo in $PropertyInfoArray) {
                if ($PropertyInfo.CanRead -and $null -ne $PropertyInfo.GetGetMethod()) {
                    if ($PropertyInfo.GetIndexParameters().Length -gt 0) {
                        $StringWriter.WriteLine("            // TODO: Validate target.$($PropertyInfo.Name)[$(($PropertyInfo.GetIndexParameters() | ForEach-Object { $_.Name }) -join ', ')]");
                    } else {
                        $StringWriter.WriteLine("            $([FsInfoCat.Services]::ToCsTypeName($PropertyInfo.PropertyType, $true)) actual$($UcFirstRegex.Replace($PropertyInfo.Name, $ToUpperSb)) = target.$($PropertyInfo.Name);");
                        $StringWriter.WriteLine("            Assert.AreEqual(expected$($UcFirstRegex.Replace($PropertyInfo.Name, $ToUpperSb)), actual$($UcFirstRegex.Replace($PropertyInfo.Name, $ToUpperSb)));");
                    }
                }
            }
            $StringWriter.WriteLine('        }');
        }
        foreach ($PropertyInfo in $PropertyInfoArray) {
            Write-Information -MessageData "Building code for property $PropertyInfo" -InformationAction Continue;
            $Getter = $null;
            if ($PropertyInfo.CanRead) { $Getter = $PropertyInfo.GetGetMethod() }
            $Setter = $null;
            if ($PropertyInfo.CanWrite) { $Setter = $PropertyInfo.GetSetMethod() }
            $IsStatic = $false;
            if ($null -eq $Setter) { $IsStatic = $Getter.IsStatic } else { if ($null -ne $Getter) { $IsStatic = $Setter.IsStatic } }
            $IndexParameters = $PropertyInfo.GetIndexParameters();
            $Name = $UcFirstRegex.Replace($PropertyInfo.Name, $ToUpperSb);
            $Description = $PropertyInfo.Name;
            $CsName = $PropertyInfo.Name;
            if ($IndexParameters.Count -gt 0) {
                $Name = "$Name$(-join ($IndexParameters | ForEach-Object { $UcFirstRegex.Replace($_.Name, $ToUpperSb) -replace '`\d+$', '' }))";
                $Description = "$Description[$(($IndexParameters | ForEach-Object { [FsInfoCat.Services]::ToCsTypeName($_.ParameterType, $true) }) -join ', ')]";
                $CsName = "[$(($IndexParameters | ForEach-Object { "$($LcFirstRegex.Replace($_.Name, $ToLowerSb))Index" }) -join ', ')]";
            }
            $Description = "$([FsInfoCat.Services]::ToCsTypeName($PropertyInfo.PropertyType, $true)) $Description";
            $Name = "$($Name)TestMethod";
            if ($MethodNames.Contains($Name)) {
                $i = 1;
                $n = $Name;
                do {
                    $i++;
                    $Name = "$n$i";
                } while ($MethodNames.Contains($Name));
            }
            $MethodNames.Add($Name);
            if ($MethodDescriptions.Contains($Description)) {
                $i = 1;
                $d = $Description;
                do {
                    $i++;
                    $Description = "$d #$i";
                } while ($MethodDescriptions.Contains($Description));
            }
            $MethodDescriptions.Add($Description);
            $StringWriter.WriteLine('');
            $StringWriter.WriteLine("        [TestMethod(`"$Description`")]");
            $StringWriter.WriteLine("        public void $Name()");
            $StringWriter.WriteLine('        {');
            $StringWriter.WriteLine('            Assert.Inconclusive("Test not implemented");');
            $StringWriter.WriteLine("            // TODO: Implement test for $Description");
            $StringWriter.WriteLine('');

            foreach ($p in $IndexParameters) {
                $StringWriter.WriteLine("            $([FsInfoCat.Services]::ToCsTypeName($p.ParameterType, $true)) $($LcFirstRegex.Replace($p.Name, $ToLowerSb))Index = default;");
            }
            if (-not $IsStatic) {
                $StringWriter.WriteLine("            $([FsInfoCat.Services]::ToCsTypeName($InputType, $true)) target = default; // TODO: Create and initialize $([FsInfoCat.Services]::ToCsTypeName($InputType, $true)) instance");
            }
            if ($null -ne $Getter) {
                $StringWriter.WriteLine("            $([FsInfoCat.Services]::ToCsTypeName($PropertyInfo.PropertyType, $true)) expectedValue = default;");
            }
            if ($null -ne $Setter) {
                if ($IsStatic) {
                    $StringWriter.WriteLine("            $([FsInfoCat.Services]::ToCsTypeName($InputType, $true)).$CsName = default;")
                } else {
                    if ($IndexParameters.Count -gt 0) {
                        $StringWriter.WriteLine("            target$CsName = default;")
                    } else {
                        $StringWriter.WriteLine("            target.$CsName = default;")
                    }
                }
            }
            if ($null -ne $Getter) {
                if ($IsStatic) {
                    $StringWriter.WriteLine("            $([FsInfoCat.Services]::ToCsTypeName($PropertyInfo.PropertyType, $true)) actualValue = $([FsInfoCat.Services]::ToCsTypeName($InputType, $true)).$CsName;");
                } else {
                    if ($IndexParameters.Count -gt 0) {
                        $StringWriter.WriteLine("            $([FsInfoCat.Services]::ToCsTypeName($PropertyInfo.PropertyType, $true)) actualValue = target$CsName;");
                    } else {
                        $StringWriter.WriteLine("            $([FsInfoCat.Services]::ToCsTypeName($PropertyInfo.PropertyType, $true)) actualValue = target.$CsName;");
                    }
                }
                $StringWriter.WriteLine('            Assert.AreEqual(expectedValue, actualValue);');
            }
            $StringWriter.WriteLine('        }');
        }
        foreach ($MethodInfo in ($InputType.GetMethods() | Where-Object { -not $_.IsSpecialName })) {
            Write-Information -MessageData "Building code for method $MethodInfo" -InformationAction Continue;
            $CsName = $MethodInfo.Name -replace '`\d*$', '';
            $Name = $UcFirstRegex.Replace($MethodInfo.Name, $ToUpperSb).Replace('`', '_');
            $cp = $MethodInfo.GetParameters();
            foreach ($p in $cp) {
                $Name = "$Name$($UcFirstRegex.Replace($p.ParameterType.Name, $ToUpperSb) -replace '`\d*$', '')";
            }
            $Name = "$($Name)TestMethod";
            if ($MethodInfo.IsGenericMethodDefinition) {
                $CsName = "$CsName<$(($MethodInfo.GetGenericArguments() | ForEach-Object { $_.Name }) -join ', ')>";
            } else {
                if ($MethodInfo.IsGenericMethod) {
                    $CsName = "$CsName<$(($MethodInfo.GetGenericArguments() | ForEach-Object { [FsInfoCat.Services]::ToCsTypeName($_, $true) }) -join ', ')>";
                }
            }
            $Description = "$([FsInfoCat.Services]::ToCsTypeName($MethodInfo.ReturnType, $true)) $CsName";
            if ($cp.Length -gt 0) {
                if ($cp[0].IsOut) {
                    $Description = "$Description(out $([FsInfoCat.Services]::ToCsTypeName($cp[0].ParameterType, $true))";
                } else {
                    if ($cp[0].ParameterType.IsByRef) {
                        $Description = "$Description(ref $([FsInfoCat.Services]::ToCsTypeName($cp[0].ParameterType.GetElementType(), $true))";
                    } else {
                        $Description = "$Description($([FsInfoCat.Services]::ToCsTypeName($cp[0].ParameterType, $true))";
                    }
                }
                foreach ($p in ($cp | Select-Object -Skip 1)) {
                    if ($p.IsOut) {
                        $Description = "$Description, out $([FsInfoCat.Services]::ToCsTypeName($p.ParameterType, $true))";
                    } else {
                        if ($p.ParameterType.IsByRef) {
                            $Description = "$Description, ref $([FsInfoCat.Services]::ToCsTypeName($p.ParameterType.GetElementType(), $true))";
                        } else {
                            $Description = "$Description, $([FsInfoCat.Services]::ToCsTypeName($p.ParameterType, $true))";
                        }
                    }
                }
                $Description = "$Description)";
            } else {
                $Description = "$Description()";
            }
            if ($MethodNames.Contains($Name)) {
                $i = 1;
                $n = $Name;
                do {
                    $i++;
                    $Name = "$n$i";
                    Write-Host -Object "Trying name $Name" -ForegroundColor Cyan;
                } while ($MethodNames.Contains($Name));
            }
            $MethodNames.Add($Name);
            if ($MethodDescriptions.Contains($Description)) {
                $i = 1;
                $d = $Description;
                do {
                    $i++;
                    $Description = "$d #$i";
                    Write-Host -Object "Trying description $Description" -ForegroundColor Cyan;
                } while ($MethodDescriptions.Contains($Description));
            }
            Write-Host -Object "Using description $Description" -ForegroundColor Cyan;
            $MethodDescriptions.Add($Description);
            $StringWriter.WriteLine('');
            $StringWriter.WriteLine("        [TestMethod(`"$Description`")]");
            $StringWriter.WriteLine("        public void $Name()");
            $StringWriter.WriteLine('        {');
            $StringWriter.WriteLine('            Assert.Inconclusive("Test not implemented");');
            $StringWriter.WriteLine("            // TODO: Implement test for $Description");
            $StringWriter.WriteLine('');
            foreach ($p in $cp) {
                if ($p.IsOut) {
                    $StringWriter.WriteLine("            $([FsInfoCat.Services]::ToCsTypeName($p.ParameterType, $true)) expected$($UcFirstRegex.Replace($p.Name, $ToUpperSb)) = default;")
                } else {
                    if ($p.ParameterType.IsByRef) {
                        $StringWriter.WriteLine("            $([FsInfoCat.Services]::ToCsTypeName($p.ParameterType.GetElementType(), $true)) $($LcFirstRegex.Replace($p.Name, $ToLowerSb))Arg = default;")
                        $StringWriter.WriteLine("            $([FsInfoCat.Services]::ToCsTypeName($p.ParameterType.GetElementType(), $true)) expected$($UcFirstRegex.Replace($p.Name, $ToUpperSb)) = default;")
                    } else {
                        $StringWriter.WriteLine("            $([FsInfoCat.Services]::ToCsTypeName($p.ParameterType, $true)) $($LcFirstRegex.Replace($p.Name, $ToLowerSb))Arg = default;");
                    }
                }
            }
            if ($MethodInfo.IsStatic) {
                if ($MethodInfo.ReturnType -eq [Void]) {
                    $StringWriter.Write("            $([FsInfoCat.Services]::ToCsTypeName($InputType, $true)).$CsName(");
                } else {
                    $StringWriter.WriteLine("            $([FsInfoCat.Services]::ToCsTypeName($MethodInfo.ReturnType, $true)) expectedReturnValue = default;");
                    $StringWriter.Write("            $([FsInfoCat.Services]::ToCsTypeName($MethodInfo.ReturnType, $true)) actualReturnValue = $([FsInfoCat.Services]::ToCsTypeName($InputType, $true)).$CsName(");
                }
            } else {
                $StringWriter.WriteLine("            $([FsInfoCat.Services]::ToCsTypeName($InputType, $true)) target = default; // TODO: Create and initialize $([FsInfoCat.Services]::ToCsTypeName($InputType, $true)) instance");
                if ($MethodInfo.ReturnType -eq [Void]) {
                    $StringWriter.Write("            target.$CsName(");
                } else {
                    $StringWriter.WriteLine("            $([FsInfoCat.Services]::ToCsTypeName($MethodInfo.ReturnType, $true)) expectedReturnValue = default;");
                    $StringWriter.Write("            $([FsInfoCat.Services]::ToCsTypeName($MethodInfo.ReturnType, $true)) actualReturnValue = target.$CsName(");
                }
            }
            if ($cp.Length -gt 0) {
                if ($cp[0].IsOut) {
                    $StringWriter.Write("out $([FsInfoCat.Services]::ToCsTypeName($cp[0].ParameterType, $true)) $($cp[0].Name)Value");
                } else {
                    if ($cp[0].ParameterType.IsByRef) {
                        $StringWriter.Write("ref $($LcFirstRegex.Replace($cp[0].Name, $ToLowerSb))Arg");
                    } else {
                        $StringWriter.Write("$($LcFirstRegex.Replace($cp[0].Name, $ToLowerSb))Arg");
                    }
                }
                foreach ($p in ($cp | Select-Object -Skip 1)) {
                    if ($p.IsOut) {
                        $StringWriter.Write(", out $([FsInfoCat.Services]::ToCsTypeName($p.ParameterType, $true)) $($LcFirstRegex.Replace($p.Name, $ToLowerSb))Value");
                    } else {
                        if ($p.ParameterType.IsByRef) {
                            $StringWriter.Write("ref $($LcFirstRegex.Replace($p.Name, $ToLowerSb))Arg");
                        } else {
                            $StringWriter.Write("$($LcFirstRegex.Replace($p.Name, $ToLowerSb))Arg");
                        }
                    }
                }
            }
            $StringWriter.WriteLine(");");
            if ($MethodInfo.ReturnType -ne [Void]) {
                $StringWriter.WriteLine('            Assert.AreEqual(expectedReturnValue, actualReturnValue);');
            }
            foreach ($p in $cp) {
                if ($p.IsOut) {
                    $StringWriter.WriteLine("            Assert.AreEqual(expected$($UcFirstRegex.Replace($p.Name, $ToUpperSb)), $($LcFirstRegex.Replace($p.Name, $ToLowerSb))Value);")
                } else {
                    if ($p.ParameterType.IsByRef) {
                        $StringWriter.WriteLine("            Assert.AreEqual(expected$($UcFirstRegex.Replace($p.Name, $ToUpperSb)), $($LcFirstRegex.Replace($p.Name, $ToLowerSb))Arg);")
                    }
                }
            }
            $StringWriter.WriteLine('        }');
        }
        $StringWriter.WriteLine('    }');
        $StringWriter.WriteLine('}');
        $StringWriter.ToString();
        [System.IO.File]::WriteAllText($OutputPath, $StringWriter.ToString(), [System.Text.UTF8Encoding]::new($false, $true));
        Write-Host -Object "Created file $OutputPath";
    }
}

Function Read-FileSystemItemExtendedProperties {
    [CmdletBinding(DefaultParameterSetName="WC")]
    param (
        # Specifies a path to one or more file system items. Wildcards are permitted.
        [Parameter(Mandatory=$true,
                   Position=0,
                   ParameterSetName="WC",
                   ValueFromPipeline=$true,
                   HelpMessage="Path to one or more file system items.")]
        [ValidateNotNullOrEmpty()]
        [SupportsWildcards()]
        [string[]]$Path,

        # Specifies a path to one or more locations. Unlike the Path parameter, the value of the LiteralPath parameter is
        # used exactly as it is typed. No characters are interpreted as wildcards. If the path includes escape characters,
        # enclose it in single quotation marks. Single quotation marks tell Windows PowerShell not to interpret any
        # characters as escape sequences.
        [Parameter(Mandatory=$true,
                   ParameterSetName="LP",
                   ValueFromPipelineByPropertyName=$true,
                   HelpMessage="Literal path to one or more file system items.")]
        [Alias("PSPath", "FullName")]
        [ValidateNotNullOrEmpty()]
        [string[]]$LiteralPath
    )

    process {
        if ($PSBoundParameters.ContainsKey('Path')) {
            Read-FileSystemItemExtendedProperties -LiteralPath ($Path | Resolve-Path | Select-Object -ExpandProperty 'Path');
        } else {
            $LiteralPath | ForEach-Object {
                $Item = Get-Item -LiteralPath $_;
                $ShellObject = $null;
                if ($Item.PSIsContainer) {
                    $ShellObject = [Microsoft.WindowsAPICodePack.Shell.ShellFileSystemFolder]::FromFolderPath($Item.FullName);
                } else {
                    $ShellObject = [Microsoft.WindowsAPICodePack.Shell.ShellFile]::FromFilePath($Item.FullName);
                }
                $Item | Add-Member -MemberType NoteProperty -Name 'IsLink' -Value $ShellObject.IsLink -Force;
                $Name = $ShellObject.GetDisplayName([Microsoft.WindowsAPICodePack.Shell.DisplayNameType]::RelativeToParentEditing);
                if ([string]::IsNullOrWhiteSpace($Name)) {
                    $Item | Add-Member -MemberType NoteProperty -Name 'DisplayName' -Value $ShellObject.Name -Force;
                } else {
                    $Item | Add-Member -MemberType NoteProperty -Name 'DisplayName' -Value $Name -Force;
                }
                $Properties = @($Item.PSObject.Properties | Select-Object -ExpandProperty 'Name');
                [Xml]$MetaData = '<MetaData/>'
                $ShellObject.Properties.DefaultPropertyCollection | ForEach-Object {
                    $XmlElement = $MetaData.DocumentElement.AppendChild($MetaData.CreateElement('Property'));
                    $Name = $_.CanonicalName;
                    if (-not [string]::IsNullOrWhiteSpace($Name)) {
                        $i = $Name.LastIndexOf('.') + 1;
                        if ($i -gt 0) { $Name = $Name.Substring($i) }
                        if ($Properties -inotcontains $Name) {
                            $Properties += $Name;
                            $Item | Add-Member -MemberType NoteProperty -Name $Name -Value $_.ValueAsObject;
                            $XmlElement.Attributes.Append($MetaData.CreateAttribute("Name")).Value = $Name;
                        }
                        $XmlElement.Attributes.Append($MetaData.CreateAttribute("CanonicalName")).Value = $_.CanonicalName;
                    }
                    $XmlElement.Attributes.Append($MetaData.CreateAttribute("PropertyId")).Value = $_.PropertyKey.PropertyId.ToString();
                    $XmlElement.Attributes.Append($MetaData.CreateAttribute("FormatId")).Value = $_.PropertyKey.FormatId.ToString();
                    $XmlElement.Attributes.Append($MetaData.CreateAttribute("DisplayType")).Value = $_.Description.DisplayType.ToString('F');
                    $XmlElement.Attributes.Append($MetaData.CreateAttribute("GroupingRange")).Value = $_.Description.GroupingRange.ToString('F');
                    $XmlElement.Attributes.Append($MetaData.CreateAttribute("DisplayType")).Value = $_.Description.DisplayType;
                    if ($null -ne $_.Description.DisplayName) { $XmlElement.InnerText = $_.Description.DisplayName }
                }
                $Item | Add-Member -MemberType NoteProperty -Name '__MetaData' -Value $MetaData;
                $Item | Write-Output;
            }
        }
    }
}

class Win32LogicalDisk {
    [ushort]$Access;
    [ulong]$Availability;
    [ulong]$BlockSize;
    [string]$Caption;
    [bool]$Compressed;
    [uint]$ConfigManagerErrorCode;
    [bool]$ConfigManagerUserConfig;
    [string]$CreationClassName;
    [string]$Description;
    [string]$DeviceID;
    [uint]$DriveType;
    [bool]$ErrorCleared;
    [string]$ErrorDescription;
    [string]$ErrorMethodology;
    [string]$FileSystem;
    [ulong]$FreeSpace;
    [Nullable[DateTime]]$InstallDate;
    [uint]$LastErrorCode;
    [uint]$MaximumComponentLength;
    [uint]$MediaType;
    [string]$Name;
    [ulong]$NumberOfBlocks;
    [string]$PNPDeviceID;
    [ushort[]]$PowerManagementCapabilities;
    [bool]$PowerManagementSupported;
    [string]$ProviderName;
    [string]$PSComputerName;
    [string]$Purpose;
    [bool]$QuotasDisabled;
    [bool]$QuotasIncomplete;
    [bool]$QuotasRebuilding;
    [ulong]$Size;
    [string]$Status;
    [ushort]$StatusInfo;
    [bool]$SupportsDiskQuotas;
    [bool]$SupportsFileBasedCompression;
    [string]$SystemCreationClassName;
    [string]$SystemName;
    [bool]$VolumeDirty;
    [string]$VolumeName;
    [string]$VolumeSerialNumber;
    Win32LogicalDisk([Microsoft.Management.Infrastructure.CimInstance]$CimInstance) {
        $this.Access = $CimInstance.Access;
        $this.Access = $CimInstance.Access;
        $this.Availability = $CimInstance.Availability;
        $this.BlockSize = $CimInstance.BlockSize;
        $this.Caption = $CimInstance.Caption;
        $this.Compressed = $CimInstance.Compressed;
        $this.ConfigManagerErrorCode = $CimInstance.ConfigManagerErrorCode;
        $this.ConfigManagerUserConfig = $CimInstance.ConfigManagerUserConfig;
        $this.CreationClassName = $CimInstance.CreationClassName;
        $this.Description = $CimInstance.Description;
        $this.DeviceID = $CimInstance.DeviceID;
        $this.DriveType = $CimInstance.DriveType;
        $this.ErrorCleared = $CimInstance.ErrorCleared;
        $this.ErrorDescription = $CimInstance.ErrorDescription;
        $this.ErrorMethodology = $CimInstance.ErrorMethodology;
        $this.FileSystem = $CimInstance.FileSystem;
        $this.FreeSpace = $CimInstance.FreeSpace;
        $this.InstallDate = $CimInstance.InstallDate;
        $this.LastErrorCode = $CimInstance.LastErrorCode;
        $this.MaximumComponentLength = $CimInstance.MaximumComponentLength;
        $this.MediaType = $CimInstance.MediaType;
        $this.Name = $CimInstance.Name;
        $this.NumberOfBlocks = $CimInstance.NumberOfBlocks;
        $this.PNPDeviceID = $CimInstance.PNPDeviceID;
        $this.PowerManagementCapabilities = $CimInstance.PowerManagementCapabilities;
        $this.PowerManagementSupported = $CimInstance.PowerManagementSupported;
        $this.ProviderName = $CimInstance.ProviderName;
        $this.PSComputerName = $CimInstance.PSComputerName;
        $this.Purpose = $CimInstance.Purpose;
        $this.QuotasDisabled = $CimInstance.QuotasDisabled;
        $this.QuotasIncomplete = $CimInstance.QuotasIncomplete;
        $this.QuotasRebuilding = $CimInstance.QuotasRebuilding;
        $this.Size = $CimInstance.Size;
        $this.Status = $CimInstance.Status;
        $this.StatusInfo = $CimInstance.StatusInfo;
        $this.SupportsDiskQuotas = $CimInstance.SupportsDiskQuotas;
        $this.SupportsFileBasedCompression = $CimInstance.SupportsFileBasedCompression;
        $this.SystemCreationClassName = $CimInstance.SystemCreationClassName;
        $this.SystemName = $CimInstance.SystemName;
        $this.VolumeDirty = $CimInstance.VolumeDirty;
        $this.VolumeName = $CimInstance.VolumeName;
        $this.VolumeSerialNumber = $CimInstance.VolumeSerialNumber;
    }
}

Function Get-Win32LogicalDisk {
    [CmdletBinding()]
    [OutputType([Win32LogicalDisk])]
    Param(
        [Parameter(Mandatory = $true, Position = 0)]
        [ValidateNotNullOrEmpty()]
        [ValidateScript({ [System.IO.Path]::IsPathFullyQualified($_) })]
        [string]$Path
    )

    $p = [System.IO.Path]::GetPathRoot($Path);
    foreach ($CimInstance in  (Get-CimInstance -ClassName 'Win32_LogicalDisk')) {
        $RelatedInstance = $CimInstance | Get-CimAssociatedInstance -Association 'Win32_LogicalDiskRootDirectory';
        if ($null -ne $RelatedInstance -and $RelatedInstance.Name -ieq $p) {
            return [Win32LogicalDisk]::new($CimInstance);
        }
    }
}

Function Get-TestFileSystemElement {
    [CmdletBinding(DefaultParameterSetName = 'Id')]
    Param(
        [Parameter(Mandatory = $true, ParameterSetName = 'Id')]
        [ValidateNotNull()]
        [Guid]$Id,

        [Parameter(Mandatory = $true, ParameterSetName = 'LogicalDisk')]
        [ValidateNotNull()]
        [Win32LogicalDisk]$LogicalDisk,

        [Parameter(Mandatory = $true)]
        [ValidateNotNull()]
        [ValidateScript({ $null -ne $_.DocumentElement -and $_.DocumentElement.NamespaceURI.Length -eq 0 -and $_.DocumentElement.LocalName -eq 'SampleData' })]
        [System.Xml.XmlDocument]$SampleData
    )

    if ($PSCmdlet.ParameterSetName -eq 'LogicalDisk') {
        $SymbolicNameElement = $SampleData.DocumentElement.SelectSingleNode("FileSystem/SymbolicName[text()=`"$($LogicalDisk.FileSystem)`"]");
        if ($null -eq $SymbolicNameElement) {
            $XmlElement = $SampleData.DocumentElement.AppendChild($SampleData.CreateElement('FileSystem'));
            $XmlElement.Attributes.Append($SampleData.CreateAttribute('Id')).Value = [Guid]::NewGuid().ToString('D');
            $XmlElement.Attributes.Append($SampleData.CreateAttribute('DisplayName')).Value = $LogicalDisk.FileSystem;
            if ($LogicalDisk.DriveType -eq [System.IO.DriveType]::CDRom) {
                $XmlElement.Attributes.Append($SampleData.CreateAttribute('ReadOnly')).Value = 'true';
            }
            $SymbolicNameElement = $XmlElement.AppendChild($SampleData.CreateElement('SymbolicName'));
            $SymbolicNameElement.Attributes.Append($SampleData.CreateAttribute('Id')).Value = [Guid]::NewGuid().ToString('D');
            $SymbolicNameElement.InnerText = $LogicalDisk.FileSystem;
            $CreatedOn = [DateTime]::Now.ToString('yyyy-MM-dd HH:mm:ss');
            $SymbolicNameElement.Attributes.Append($SampleData.CreateAttribute('CreatedOn')).Value = $CreatedOn;
            $SymbolicNameElement.Attributes.Append($SampleData.CreateAttribute('ModifiedOn')).Value = $CreatedOn;
            $XmlElement.Attributes.Append($SampleData.CreateAttribute('CreatedOn')).Value = $CreatedOn;
            $XmlElement.Attributes.Append($SampleData.CreateAttribute('ModifiedOn')).Value = $CreatedOn;
            $XmlElement | Write-Output;
        } else {
            $SymbolicNameElement.ParentNode | Write-Output;
        }
    } else {
        $SampleData.DocumentElement.SelectSingleNode("FileSystem[@Id=`"$Id`"]") | Write-Output;
    }
}

Function Get-TestVolumeElement {
    [CmdletBinding(DefaultParameterSetName = 'Id')]
    Param(
        [Parameter(Mandatory = $true, ParameterSetName = 'Id')]
        [ValidateNotNull()]
        [Guid]$Id,

        [Parameter(Mandatory = $true, ParameterSetName = 'LogicalDisk')]
        [ValidateNotNull()]
        [Win32LogicalDisk]$LogicalDisk,

        [Parameter(Mandatory = $true)]
        [ValidateNotNull()]
        [ValidateScript({ $null -ne $_.DocumentElement -and $_.DocumentElement.NamespaceURI.Length -eq 0 -and $_.DocumentElement.LocalName -eq 'SampleData' })]
        [System.Xml.XmlDocument]$SampleData
    )

    if ($PSCmdlet.ParameterSetName -eq 'LogicalDisk') {
        $TestFileSystemElement = Get-TestFileSystemElement -LogicalDisk $LogicalDisk -SampleData $SampleData;
        $Identifier = '';
        if ($LogicalDisk.DriveType -eq [System.IO.DriveType]::Network) {
            $Identifier = $LogicalDisk.ProviderName -replace '/$', '';
        } else {
            $Identifier = "urn:volume:id:$($LogicalDisk.VolumeSerialNumber.Substring(0, 4))-$($LogicalDisk.VolumeSerialNumber.Substring(4))";
        }
        $XmlElement = $TestFileSystemElement.SelectSingleNode("FileSystem/Volume[@Identifer=`"$Identifier`"]");
        if ($null -eq $XmlElement) {
            $XmlElement = $TestFileSystemElement.AppendChild($SampleData.CreateElement('Volume'));
            $XmlElement.Attributes.Append($SampleData.CreateAttribute('Id')).Value = [Guid]::NewGuid().ToString('D');
            $XmlElement.Attributes.Append($SampleData.CreateAttribute('VolumeName')).Value = '' + $LogicalDisk.VolumeName;
            $XmlElement.Attributes.Append($SampleData.CreateAttribute('Identifier')).Value = $Identifier;
            if ([string]::IsNullOrWhiteSpace($LogicalDisk.Name)) {
                $XmlElement.Attributes.Append($SampleData.CreateAttribute('DisplayName')).Value = $LogicalDisk.VolumeName;
            } else {
                $XmlElement.Attributes.Append($SampleData.CreateAttribute('DisplayName')).Value = $LogicalDisk.Name;
            }
            $XmlElement.Attributes.Append($SampleData.CreateAttribute('Type')).Value = $LogicalDisk.DriveType;
            $CreatedOn = [DateTime]::Now.ToString('yyyy-MM-dd HH:mm:ss');
            $XmlElement.Attributes.Append($SampleData.CreateAttribute('CreatedOn')).Value = $CreatedOn;
            $XmlElement.Attributes.Append($SampleData.CreateAttribute('ModifiedOn')).Value = $CreatedOn;
            $XmlElement | Write-Output;
        }
    }
}

Add-Type -Path ($PSScriptRoot | Join-Path -ChildPath 'GenerateTestData.cs') -ErrorAction Stop;

Function Import-LocalTestData {
    [CmdletBinding(DefaultParameterSetName="WC")]
    Param (
        # The path to one or more file system items to import. Wildcards are permitted.
        [Parameter(Mandatory = $true, Position = 0, ParameterSetName = "WC", ValueFromPipeline = $true, HelpMessage = "Path to one or more file system items.")]
        [ValidateNotNullOrEmpty()]
        [SupportsWildcards()]
        [string[]]$Path,

        # Literal path of file system items to import. Unlike the Path parameter, the value of the LiteralPath parameter is
        # used exactly as it is typed. No characters are interpreted as wildcards. If the path includes escape characters,
        # enclose it in single quotation marks. Single quotation marks tell Windows PowerShell not to interpret any
        # characters as escape sequences.
        [Parameter(Mandatory = $true, ParameterSetName = "LP", ValueFromPipelineByPropertyName = $true, HelpMessage = "Literal path to one or more file system items.")]
        [Alias("PSPath", "FullName")]
        [ValidateNotNullOrEmpty()]
        [string[]]$LiteralPath,

        # Literal path to XML file.
        [Parameter(Mandatory = $true, ParameterSetName = "ChildItems")]
        [ValidateNotNullOrEmpty()]
        [System.Xml.XmlElement]$ParentElement,

        # Literal path to XML file.
        [Parameter(Mandatory = $true, HelpMessage = "Literal path to XML file.", ParameterSetName = "WC")]
        [Parameter(Mandatory = $true, HelpMessage = "Literal path to XML file.", ParameterSetName = "LP")]
        [ValidateNotNullOrEmpty()]
        [string]$XmlFile,

        [ushort]$MaxRecursionDepth = 256,

        [ulong]$MaxTotalItems = [ulong]::MaxValue
    )

    Begin {
        $XmlDocument = [System.Xml.XmlDocument]::new();
        if ($XmlFile | Test-Path -PathType Leaf) {
            $XmlDocument.Load($XmlFile);
            if ($null -eq $XmlDocument.DocumentElement) {
                $XmlDocument = $null;
                Write-Error -Message 'Failed to load XML' -Category ReadError -ErrorId 'XmlDocument.Load' -TargetObject $XmlFile -CategoryReason 'Document element was null';
                return;
            }
            if ($XmlDocument.DocumentElement.NamespaceURI.Length -gt 0 -or $XmlDocument.DocumentElement.LocalName -ne 'SampleData') {
                Write-Error -Message 'Invalid root element name.' -Category InvalidData -ErrorId 'XmlDocument.DocumentElement' -TargetObject $XmlFile -CategoryReason "Document element was {$($XmlDocument.DocumentElement.NamespaceURI):$($XmlDocument.DocumentElement.LocalName)} instead of {:SampleData}.";
                $XmlDocument = $null;
                return;
            }
        } else {
            $XmlDocument.AppendChild($XmlDocument.CreateElement('SampleData')) | Out-Null;
        }
    }

    Process {
        if ($PSCmdlet.ParameterSetName -eq 'ChildItems') {
            if ($null -eq $ParentElement.FileSystemInfo) {
                Write-Error -Message 'Invalid parent element' -Category InvalidArgument -ErrorId 'ParentElement.FileSystemInfo' -TargetObject $ParentElement -CategoryTargetName 'FileSystemInfo';
            } else {
                $OwnerDocument = $ParentElement.OwnerDocument;
                $RemainingMaxTotalItems = $MaxTotalItems;
                if ($ParentElement.FileSystemInfo -is [System.IO.FileInfo]) {
                    return $RemainingMaxTotalItems - 1;
                } else {
                    if ($RemainingMaxTotalItems -gt 0) {
                        foreach ($FileInfo in $ParentElement.FileSystemInfo.GetFiles()) {
                            #$RemainingMaxTotalItems =
                            throw 'Not implemented'
                        }
                        [System.IO.FileInfo[]]$Files = $ParentElement.FileSystemInfo.GetFiles();
                        if ($RemainingMaxTotalItems -lt $Files.Length) {
                            $Files = @($Files[0..($RemainingMaxTotalItems-1)]);
                            $RemainingMaxTotalItems = 0;
                        } else {
                            $RemainingMaxTotalItems -= $Files.Length;
                        }
                        [System.IO.DirectoryInfo[]]$Directories = @();
                        if ($RemainingMaxTotalItems -gt 0 -and $MaxRecursionDepth -gt 0) {
                            $Directories = $ParentElement.FileSystemInfo.GetDirectories();
                            if ($RemainingMaxTotalItems -lt $Directories.Length) {
                                $Directories = @($Directories[0..($RemainingMaxTotalItems-1)]);
                                $RemainingMaxTotalItems = 0;
                            } else {
                                $RemainingMaxTotalItems -= $Directories.Length;
                            }
                        }
                        if ($Files.Length -gt 0) {
                            foreach ($FileInfo in $Files) {
                                $XmlElement = $ParentElement.SelectSingleNode("File[@Name=`"$($FileInfo.Name)\""]");
                                if ($null -eq $XmlElement) {
                                    $XmlElement = $ParentElement.AppendChild($OwnerDocument.CreateElement('File'));
                                    $XmlElement.Attributes.Append($OwnerDocument.CreateAttribute('Id')).Value = [Guid]::NewGuid().ToString('D');
                                    $XmlElement.Attributes.Append($OwnerDocument.CreateAttribute('Name')).Value = $FileInfo.Name;

                                }
                            }
                        }
                        if ($Directories.Length -gt 0) {
                            foreach ($DirectoryInfo in $Directories) {
                                $XmlElement = $ParentElement.SelectSingleNode("Subdirectory[@Name=`"$($DirectoryInfo.Name)\""]");
                                if ($null -eq $XmlElement) {
                                    $XmlElement = $ParentElement.AppendChild($OwnerDocument.CreateElement('Subdirectory'));
                                    $XmlElement.Attributes.Append($OwnerDocument.CreateAttribute('Id')).Value = [Guid]::NewGuid().ToString('D');
                                    $XmlElement.Attributes.Append($OwnerDocument.CreateAttribute('Name')).Value = $DirectoryInfo.Name;

                                }

                                $XmlElement | Add-Member -MemberType NoteProperty -Name 'FileSystemInfo' -Value $DirectoryInfo -Force;
                                $RemainingMaxTotalItems = Import-LocalTestData -Parent $XmlElement -MaxRecursionDepth ($MaxRecursionDepth - 1) -MaxTotalItems $RemainingMaxTotalItems;
                                if ($RemainingMaxTotalItems -lt 1) { break }
                            }
                        }
                    }
                }
            }
        } else {
            foreach ($FileSystemInfo in ((&{
                if ($args[0] -eq 'WC') {
                    ($Path | Resolve-Path | Select-Object -ExpandProperty 'Path') | Write-Output;
                } else {
                    $LiteralPath | Write-Output;
                }
            }($PSCmdlet.ParameterSetName)) | ForEach-Object {
                if ([System.IO.File]::Exists($InputPath)) {
                    [System.IO.FileInfo]::new($InputPath) | Write-Output;
                } else {
                    if ([System.IO.Directory]::Exists($InputPath)) {
                        [System.IO.DirectoryInfo]::new($InputPath) | Write-Output;
                    } else {
                        Write-Error -Message "Path not found: $InputPath" -Category ObjectNotFound -ErrorId 'FileSystemInfo.Exists' -TargetObject $InputPath;
                    }
                }
            })) {
                [System.IO.DirectoryInfo]$DirectoryInfo = (&{
                    if ($args[0] -is [System.IO.FileInfo]) { return $args[0].Directory }
                    return $args[0];
                }($FileSystemInfo));
                $Stack = [System.Collections.Generic.Stack[System.IO.DirectoryInfo]]::new();
                do {
                    $Stack.Push($DirectoryInfo);
                } while ($null -ne ($DirectoryInfo = $DirectoryInfo.Parent));
                $DirectoryInfo = $Stack.Pop();
                $LogicalDisk = Get-Win32LogicalDisk -Path $DirectoryInfo.FullName;
                if ($null -ne $LogicalDisk) {
                    $TestVolumeElement = Get-TestVolumeElement -LogicalDisk $LogicalDisk -SampleData $XmlDocument;
                    $XmlElement = $TestVolumeElement.SelectSingleNode('RootDirectory');
                    $ModifiedOn = [DateTime]::Now.ToString('yyyy-MM-dd HH:mm:ss');
                    if ($null -eq $XmlElement) {
                        $XmlElement = $TestVolumeElement.AppendChild($XmlDocument.CreateElement('RootDirectory'));
                        $XmlElement.Attributes.Append($SampleData.CreateAttribute('Id')).Value = [Guid]::NewGuid().ToString('D');
                        $XmlElement.Attributes.Append($SampleData.CreateAttribute('Name')).Value = $DirectoryInfo.Name;
                        $XmlElement.Attributes.Append($SampleData.CreateAttribute('CreationTime')).Value = $DirectoryInfo.CreationTime.ToString('yyyy-MM-dd HH:mm:ss');
                        $XmlElement.Attributes.Append($SampleData.CreateAttribute('LastWriteTime')).Value = $DirectoryInfo.LastWriteTime.ToString('yyyy-MM-dd HH:mm:ss');
                        $XmlElement.Attributes.Append($SampleData.CreateAttribute('LastAccessed')).Value = $ModifiedOn;
                        $XmlElement.Attributes.Append($SampleData.CreateAttribute('CreatedOn')).Value = $ModifiedOn;
                        $XmlElement.Attributes.Append($SampleData.CreateAttribute('ModifiedOn')).Value = $ModifiedOn;
                    } else {
                        $NewCreationTime = $DirectoryInfo.CreationTime.ToString('yyyy-MM-dd HH:mm:ss');
                        $NewLastWriteTime = $DirectoryInfo.LastWriteTime.ToString('yyyy-MM-dd HH:mm:ss');
                        $OldCreationTime = $XmlElement.SelectSingleNode('@CreationTime');
                        $OldLastWriteTime = $XmlElement.SelectSingleNode('@LastWriteTime');
                        $OldLastAccessed = $XmlElement.SelectSingleNode('@LastAccessed');
                        if ($NewCreationTime -ne $OldCreationTime.Value -or $NewLastWriteTime -ne $OldLastWriteTime.Value -or $OldLastAccessed.Value -ne $ModifiedOn) {
                            $OldLastAccessed.Value = $ModifiedOn;
                            $XmlElement.SelectSingleNode('@ModifiedOn').Value = $ModifiedOn;
                            $OldCreationTime.Value = $NewCreationTime;
                            $OldLastWriteTime.Value = $LastWriteTime;
                        }
                    }
                    while ($Stack.Count -gt 0) {
                        $ParentDirectoryInfo = $DirectoryInfo;
                        $DirectoryInfo = $Stack.Pop();
                        $ParentElement = $XmlElement;
                        $XmlElement = $ParentElement.SelectSingleNode("Subdirectory[@Name=`"$($DirectoryInfo.Name)`"");
                        if ($null -eq $XmlElement) {
                            $XmlElement = $ParentElement.SelectNodes('Subdirectory') | Where-Object {
                                $_.Name -ieq $DirectoryInfo.Name
                            } | Select-Object -First 1;
                        }
                        $ModifiedOn = [DateTime]::Now.ToString('yyyy-MM-dd HH:mm:ss');
                        if ($null -eq $XmlElement) {
                            $XmlElement = $ParentElement.AppendChild($XmlDocument.CreateElement('Subdirectory'));
                            $XmlElement.Attributes.Append($SampleData.CreateAttribute('Id')).Value = [Guid]::NewGuid().ToString('D');
                            $XmlElement.Attributes.Append($SampleData.CreateAttribute('Name')).Value = $DirectoryInfo.Name;
                            $XmlElement.Attributes.Append($SampleData.CreateAttribute('CreationTime')).Value = $DirectoryInfo.CreationTime.ToString('yyyy-MM-dd HH:mm:ss');
                            $XmlElement.Attributes.Append($SampleData.CreateAttribute('LastWriteTime')).Value = $DirectoryInfo.LastWriteTime.ToString('yyyy-MM-dd HH:mm:ss');
                            $XmlElement.Attributes.Append($SampleData.CreateAttribute('LastAccessed')).Value = $ModifiedOn;
                            $XmlElement.Attributes.Append($SampleData.CreateAttribute('CreatedOn')).Value = $ModifiedOn;
                            $XmlElement.Attributes.Append($SampleData.CreateAttribute('ModifiedOn')).Value = $ModifiedOn;
                        } else {
                            $NewCreationTime = $DirectoryInfo.CreationTime.ToString('yyyy-MM-dd HH:mm:ss');
                            $NewLastWriteTime = $DirectoryInfo.LastWriteTime.ToString('yyyy-MM-dd HH:mm:ss');
                            $OldCreationTime = $XmlElement.SelectSingleNode('@CreationTime');
                            $OldLastWriteTime = $XmlElement.SelectSingleNode('@LastWriteTime');
                            $OldLastAccessed = $XmlElement.SelectSingleNode('@LastAccessed');
                            if ($NewCreationTime -ne $OldCreationTime.Value -or $NewLastWriteTime -ne $OldLastWriteTime.Value -or $OldLastAccessed.Value -ne $ModifiedOn) {
                                $OldLastAccessed.Value = $ModifiedOn;
                                $XmlElement.SelectSingleNode('@ModifiedOn').Value = $ModifiedOn;
                                $OldCreationTime.Value = $NewCreationTime;
                                $OldLastWriteTime.Value = $LastWriteTime;
                            }
                        }
                    }
                    $CrawlConfigurationElement = $XmlElement.SelectSingleNode('CrawlConfiguration');
                    $ActualMaxRecursionDepth = $MaxRecursionDepth;
                    $ActualMaxTotalItems = $MaxTotalItems;
                    if ($null -eq $CrawlConfigurationElement) {
                        $CreatedOn = [DateTime]::Now.ToString('yyyy-MM-dd HH:mm:ss');
                        $CrawlConfigurationElement = $XmlElement.AppendChild($XmlDocument.CreateElement('CrawlConfiguration'));
                        $CrawlConfigurationElement.Attributes.Append($SampleData.CreateAttribute('Id')).Value = [Guid]::NewGuid().ToString('D');
                        if ($ActualMaxRecursionDepth -ne 256) {
                            $XmlElement.Attributes.Append($SampleData.CreateAttribute('MaxRecursionDepth')).Value = $ActualMaxRecursionDepth.ToString();
                        }
                        if ($ActualMaxTotalItems -ne [ulong]::MaxValue) {
                            $XmlElement.Attributes.Append($SampleData.CreateAttribute('MaxTotalItems')).Value = $ActualMaxTotalItems.ToString();
                        }
                        $XmlElement.Attributes.Append($SampleData.CreateAttribute('CreatedOn')).Value = $CreatedOn;
                        $XmlElement.Attributes.Append($SampleData.CreateAttribute('ModifiedOn')).Value = $CreatedOn;
                    } else {
                        $XmlAttribute = $CrawlConfigurationElement.SelectSingleNode('@MaxRecursionDepth');
                        if ($null -ne $XmlAttribute) { $ActualMaxRecursionDepth = [ushort]::Parse($XmlAttribute.Value) }
                        $XmlAttribute = $CrawlConfigurationElement.SelectSingleNode('@MaxTotalItems');
                        if ($null -ne $XmlAttribute) { $ActualMaxTotalItems = [ulong]::Parse($XmlAttribute.Value) }
                    }
                    $XmlElement | Add-Member -MemberType NoteProperty -Name 'FileSystemInfo' -Value $FileSystemInfo -Force;
                    (Import-LocalTestData -Parent $XmlElement -MaxRecursionDepth $ActualMaxRecursionDepth -MaxTotalItems $ActualMaxTotalItems) | Out-Null;
                } else {
                    Write-Error -Message "Failed to get logical volume for path: $($DirectoryInfo.FullName)" -Category ResourceUnavailable -ErrorId 'Win32_LogicalDisk' -TargetObject $ParentDirectoryInfo;
                }
            }
        }
    }

    End {
        $XmlWriter = [System.Xml.XmlWriter]::Create($XmlFile, [System.Xml.XmlWriterSettings]@{
            Indent = $true;
            Encoding = [System.Text.UTF8Encoding]::new($false, $false);
        });
        try {
            $XmlDocument.WriteTo($XmlWriter);
            $XmlWriter.Flush();
        } finally { $XmlWriter.Close() }
    }
}

$FileSystem = [GenerateTestData.FileSystem]::new();
$FileSystem.DisplayName = "Test";
$SymbolicName = [GenerateTestData.SymbolicName]::new();
$SymbolicName.Name = 'SN';
$FileSystem.SymbolicNames.Add($SymbolicName);
$SymbolicName = [GenerateTestData.SymbolicName]::new();
$SymbolicName.Name = 'SN2';
$SymbolicName.Notes = 'My Notes';
$FileSystem.SymbolicNames.Add($SymbolicName);
$FileSystem.Notes = 'hey there';
$Volume = [GenerateTestData.Volume]::new();
$Volume.DisplayName = "C:\";
$Volume.VolumeName = "MyVolume";
$Volume.Identifier = [Uri]::new('\\mysite\myshare', [UriKind]::Absolute);
$FileSystem.Volumes.Add($Volume);
$VolumeAccessError = [GenerateTestData.VolumeAccessError]::new();
$VolumeAccessError.Message = 'MyError';
$VolumeAccessError.ErrorCode = 12;
$Volume.AccessErrors.Add($VolumeAccessError);
$Subdirectory = [GenerateTestData.Subdirectory]::new();
$Subdirectory.Name = 'C:\';
$Volume.RootDirectory = $Subdirectory;
$SubdirectoryAccessError = [GenerateTestData.SubdirectoryAccessError]::new();
$SubdirectoryAccessError.Message = 'hey you';
$SubdirectoryAccessError.Details = 'Get offa my cloud';
$SubdirectoryAccessError.ErrorCode = 54;
$Subdirectory.AccessErrors.Add($SubdirectoryAccessError);
$CrawlConfiguration = [GenerateTestData.CrawlConfiguration]::new();
$CrawlConfiguration.DisplayName = "MyConfig";
$CrawlConfiguration.MaxRecursionDepth = 255;
$CrawlConfiguration.MaxTotalItems = 100;
$Subdirectory.CrawlConfiguration = $CrawlConfiguration;
$XmlSerializer = [System.Xml.Serialization.XmlSerializer]::new([GenerateTestData.FileSystem]);
$sw = [System.IO.StringWriter]::new();
$XmlSerializer.Serialize($sw, $FileSystem);
$sw.ToString()

<#
cd C:\Users\lerwi\Git\FsInfoCat\dev\Scripts
.\GenerateTestData.ps1

($PSScriptRoot | Join-Path -ChildPath '..\..\Resources') | Import-LocalTestData -XmlFile ($PSScriptRoot | Join-Path -ChildPath '..\..\FsInfoCat\FsInfoCat.UnitTests\Resources\SampleData.xml');

$Item = Read-FileSystemItemExtendedProperties -LiteralPath ($PSScriptRoot | Join-Path -ChildPath '..\..\Resources\Reference\CategorizedSources.xml');
($Item.PSObject.Properties | Select-Object -Property 'Name', 'Value') | Out-GridView;
[System.Windows.Clipboard]::SetText($Item.__MetaData.OuterXml)


Add-Type -Path '..\..\FsInfoCat\FsInfoCat.Desktop\bin\Debug\net5.0-windows\SQLitePCLRaw.provider.dynamic_cdecl.dll'
Add-Type -Path '..\..\FsInfoCat\FsInfoCat.Desktop\bin\Debug\net5.0-windows\SQLitePCLRaw.batteries_v2.dll'
Add-Type -Path '..\..\FsInfoCat\FsInfoCat.Desktop\bin\Debug\net5.0-windows\SQLitePCLRaw.nativelibrary.dll'
Add-Type -Path '..\..\FsInfoCat\FsInfoCat.Desktop\bin\Debug\net5.0-windows\SQLitePCLRaw.core.dll'

Add-Type -Path '..\..\FsInfoCat\FsInfoCat.Desktop\bin\Debug\net5.0-windows\runtimes\win-x64\native\e_sqlite3.dll';
Add-Type -Path '..\..\FsInfoCat\FsInfoCat.Desktop\bin\Debug\net5.0-windows\Microsoft.Data.Sqlite.dll'
Add-Type -Path '..\..\FsInfoCat\FsInfoCat.Desktop\bin\Debug\net5.0-windows\Microsoft.WindowsAPICodePack.dll'
Add-Type -Path '..\..\FsInfoCat\FsInfoCat.Desktop\bin\Debug\net5.0-windows\Microsoft.WindowsAPICodePack.Shell.dll'
$ConnectionStringBuilder = [Microsoft.Data.Sqlite.SqliteConnectionStringBuilder]::new();
$ConnectionStringBuilder.DataSource = 'C:\Users\lerwi\Git\FsInfoCat\FsInfoCat\FsInfoCat.UnitTests\Resources\TestLocal.db';
$ConnectionStringBuilder.Mode = [Microsoft.Data.Sqlite.SqliteOpenMode]::ReadWrite;
$Connection = [Microsoft.Data.Sqlite.SqliteConnection]::new($ConnectionStringBuilder.ConnectionString);
$Connection.Open();
#>
