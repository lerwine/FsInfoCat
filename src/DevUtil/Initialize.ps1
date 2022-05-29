$BasePath = [Environment]::GetFolderPath([Environment+SpecialFolder]::UserProfile) | Join-Path -ChildPath '.nuget\packages';

('microsoft.entityframeworkcore\5.0.9\lib\netstandard2.1\Microsoft.EntityFrameworkCore.dll', 'microsoft.extensions.hosting\5.0.0\lib\netstandard2.1\Microsoft.Extensions.Hosting.dll',
    'microsoft.extensions.hosting.abstractions\5.0.0\lib\netstandard2.1\Microsoft.Extensions.Hosting.Abstractions.dll',
    'microsoft.entityframeworkcore.relational\5.0.9\lib\netstandard2.1\Microsoft.EntityFrameworkCore.Relational.dll',
    'microsoft.entityframeworkcore.abstractions\5.0.9\lib\netstandard2.1\Microsoft.EntityFrameworkCore.Abstractions.dll',
    'microsoft.extensions.logging.abstractions\5.0.0\lib\netstandard2.0\Microsoft.Extensions.Logging.Abstractions.dll', 'microsoft-windowsapicodepack-core\1.1.4\lib\netcoreapp3.1\Microsoft.WindowsAPICodePack.dll',
    'microsoft-windowsapicodepack-shell\1.1.4\lib\netcoreapp3.1\Microsoft.WindowsAPICodePack.Shell.dll') | ForEach-Object {
    Add-Type -Path ($BasePath | Join-Path -ChildPath $_) -ErrorAction Stop;
}
