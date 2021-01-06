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

$AppUserCollection = @(
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

$Random = [Random]::new();
foreach ($AppUser in $AppUserCollection) {
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
