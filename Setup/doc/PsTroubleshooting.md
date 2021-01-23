# Troubleshooting

## Could not load file or assembly

This usually happens when you are trying to import a module compiled for PowerShell Core from PowerShell Desktop or vice-versa.

Example error.

```PowerShell
PS C:\Users\lerwi\Git\FsInfoCat> Import-Module -Name 'Setup\bin\FsInfoCat'
Import-Module : The specified module 'Setup\bin\FsInfoCat' was not loaded because no valid module file was found in any module directory.
At line:1 char:1
+ Import-Module -Name 'Setup\bin\FsInfoCat'
+ ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    + CategoryInfo          : ResourceUnavailable: (Setup\bin\FsInfoCat:String) [Import-Module], FileNotFoundException
    + FullyQualifiedErrorId : Modules_ModuleNotFound,Microsoft.PowerShell.Commands.ImportModuleCommand
```

```
Starting OmniSharp server at 1/22/2021, 12:05:18 PM
    Target: c:\Users\lerwi\Git\FsInfoCat\src\FsInfoCat.sln

OmniSharp server started.
    Path: c:\Users\lerwi\.vscode\extensions\ms-dotnettools.csharp-1.23.8\.omnisharp\1.37.5\OmniSharp.exe
    PID: 18924

[info]: OmniSharp.Stdio.Host
        Starting OmniSharp on Windows 6.2.9200.0 (x64)
[info]: OmniSharp.Services.DotNetCliService
        DotNetPath set to dotnet
[info]: OmniSharp.MSBuild.Discovery.MSBuildLocator
        Located 2 MSBuild instance(s)
            1: Visual Studio Build Tools 2017 15.9.28307.1216 - "C:\Program Files (x86)\Microsoft Visual Studio\2017\BuildTools\MSBuild\15.0\Bin"
            2: StandAlone 16.8.0 - "c:\Users\lerwi\.vscode\extensions\ms-dotnettools.csharp-1.23.8\.omnisharp\1.37.5\.msbuild\Current\Bin"
[warn]: OmniSharp.CompositionHostBuilder
        It looks like you have Visual Studio lower than VS 2019 16.3 installed.
 Try updating Visual Studio to the most recent release to enable better MSBuild support.
[info]: OmniSharp.MSBuild.Discovery.MSBuildLocator
        MSBUILD_EXE_PATH environment variable set to 'c:\Users\lerwi\.vscode\extensions\ms-dotnettools.csharp-1.23.8\.omnisharp\1.37.5\.msbuild\Current\Bin\MSBuild.exe'
[info]: OmniSharp.MSBuild.Discovery.MSBuildLocator
        Registered MSBuild instance: StandAlone 16.8.0 - "c:\Users\lerwi\.vscode\extensions\ms-dotnettools.csharp-1.23.8\.omnisharp\1.37.5\.msbuild\Current\Bin"
            CscToolExe = csc.exe
            CscToolPath = c:\Users\lerwi\.vscode\extensions\ms-dotnettools.csharp-1.23.8\.omnisharp\1.37.5\.msbuild\Current\Bin\Roslyn
            MSBuildExtensionsPath = c:\Users\lerwi\.vscode\extensions\ms-dotnettools.csharp-1.23.8\.omnisharp\1.37.5\.msbuild
            MSBuildToolsPath = c:\Users\lerwi\.vscode\extensions\ms-dotnettools.csharp-1.23.8\.omnisharp\1.37.5\.msbuild\Current\Bin
[info]: OmniSharp.Cake.CakeProjectSystem
        Detecting Cake files in 'c:\Users\lerwi\Git\FsInfoCat\src'.
[info]: OmniSharp.Cake.CakeProjectSystem
        Could not find any Cake files
[info]: OmniSharp.MSBuild.ProjectSystem
        Detecting projects in 'c:\Users\lerwi\Git\FsInfoCat\src\FsInfoCat.sln'.
[info]: OmniSharp.MSBuild.ProjectManager
        Queue project update for 'c:\Users\lerwi\Git\FsInfoCat\src\FsInfoCat\FsInfoCat.csproj'
[info]: OmniSharp.MSBuild.ProjectManager
        Queue project update for 'c:\Users\lerwi\Git\FsInfoCat\src\FsInfoCat.Web\FsInfoCat.Web.csproj'
[info]: OmniSharp.MSBuild.ProjectManager
        Queue project update for 'c:\Users\lerwi\Git\FsInfoCat\src\FsInfoCat.PS\FsInfoCat.PS.csproj'
[info]: OmniSharp.MSBuild.ProjectManager
        Queue project update for 'c:\Users\lerwi\Git\FsInfoCat\src\FsInfoCat.Test\FsInfoCat.Test.csproj'
[info]: OmniSharp.Script.ScriptProjectSystem
        Detecting CSX files in 'c:\Users\lerwi\Git\FsInfoCat\src'.
[info]: OmniSharp.Script.ScriptProjectSystem
        Could not find any CSX files
[info]: OmniSharp.WorkspaceInitializer
        Invoking Workspace Options Provider: OmniSharp.Roslyn.CSharp.Services.CSharpFormattingWorkspaceOptionsProvider, Order: 0
[info]: OmniSharp.MSBuild.ProjectManager
        Loading project: c:\Users\lerwi\Git\FsInfoCat\src\FsInfoCat\FsInfoCat.csproj
[info]: OmniSharp.WorkspaceInitializer
        Invoking Workspace Options Provider: OmniSharp.Roslyn.CSharp.Services.Completion.CompletionOptionsProvider, Order: 0
[info]: OmniSharp.WorkspaceInitializer
        Invoking Workspace Options Provider: OmniSharp.Roslyn.CSharp.Services.RenameWorkspaceOptionsProvider, Order: 100
[info]: OmniSharp.WorkspaceInitializer
        Invoking Workspace Options Provider: OmniSharp.Roslyn.CSharp.Services.ImplementTypeWorkspaceOptionsProvider, Order: 110
[info]: OmniSharp.WorkspaceInitializer
        Invoking Workspace Options Provider: OmniSharp.Roslyn.CSharp.Services.BlockStructureWorkspaceOptionsProvider, Order: 140
[info]: OmniSharp.WorkspaceInitializer
        Configuration finished.
[info]: OmniSharp.Stdio.Host
        Omnisharp server running using Stdio at location 'c:\Users\lerwi\Git\FsInfoCat\src' on host 19180.
[fail]: OmniSharp.MSBuild.ProjectLoader
        The TargetFramework value 'netcoreapp3.1,net461' was not recognized. It may be misspelled. If not, then the TargetFrameworkIdentifier and/or TargetFrameworkVersion properties must be specified explicitly.
[warn]: OmniSharp.MSBuild.ProjectManager
        Failed to load project file 'c:\Users\lerwi\Git\FsInfoCat\src\FsInfoCat\FsInfoCat.csproj'.
c:\Users\lerwi\Git\FsInfoCat\src\FsInfoCat\FsInfoCat.csproj
C:\Program Files\dotnet\sdk\3.1.102\Sdks\Microsoft.NET.Sdk\targets\Microsoft.NET.TargetFrameworkInference.targets(93,5): Error: The TargetFramework value 'netcoreapp3.1,net461' was not recognized. It may be misspelled. If not, then the TargetFrameworkIdentifier and/or TargetFrameworkVersion properties must be specified explicitly.

[info]: OmniSharp.MSBuild.ProjectManager
        Loading project: c:\Users\lerwi\Git\FsInfoCat\src\FsInfoCat.Web\FsInfoCat.Web.csproj
[fail]: OmniSharp.MSBuild.ProjectLoader
        Project '..\FsInfoCat\FsInfoCat.csproj' targets 'netcoreapp3.1,net461'. It cannot be referenced by a project that targets '.NETCoreApp,Version=v3.1'.
[warn]: OmniSharp.MSBuild.ProjectManager
        Failed to load project file 'c:\Users\lerwi\Git\FsInfoCat\src\FsInfoCat.Web\FsInfoCat.Web.csproj'.
c:\Users\lerwi\Git\FsInfoCat\src\FsInfoCat.Web\FsInfoCat.Web.csproj
c:\Users\lerwi\.vscode\extensions\ms-dotnettools.csharp-1.23.8\.omnisharp\1.37.5\.msbuild\Current\Bin\Microsoft.Common.CurrentVersion.targets(1662,5): Error: Project '..\FsInfoCat\FsInfoCat.csproj' targets 'netcoreapp3.1,net461'. It cannot be referenced by a project that targets '.NETCoreApp,Version=v3.1'.

[info]: OmniSharp.MSBuild.ProjectManager
        Loading project: c:\Users\lerwi\Git\FsInfoCat\src\FsInfoCat.PS\FsInfoCat.PS.csproj
[fail]: OmniSharp.MSBuild.ProjectLoader
        The TargetFramework value 'netcoreapp3.1,net461' was not recognized. It may be misspelled. If not, then the TargetFrameworkIdentifier and/or TargetFrameworkVersion properties must be specified explicitly.
[warn]: OmniSharp.MSBuild.ProjectManager
        Failed to load project file 'c:\Users\lerwi\Git\FsInfoCat\src\FsInfoCat.PS\FsInfoCat.PS.csproj'.
c:\Users\lerwi\Git\FsInfoCat\src\FsInfoCat.PS\FsInfoCat.PS.csproj
C:\Program Files\dotnet\sdk\3.1.102\Sdks\Microsoft.NET.Sdk\targets\Microsoft.NET.TargetFrameworkInference.targets(93,5): Error: The TargetFramework value 'netcoreapp3.1,net461' was not recognized. It may be misspelled. If not, then the TargetFrameworkIdentifier and/or TargetFrameworkVersion properties must be specified explicitly.

[info]: OmniSharp.MSBuild.ProjectManager
        Loading project: c:\Users\lerwi\Git\FsInfoCat\src\FsInfoCat.Test\FsInfoCat.Test.csproj
[fail]: OmniSharp.MSBuild.ProjectLoader
        Project '..\FsInfoCat\FsInfoCat.csproj' targets 'netcoreapp3.1,net461'. It cannot be referenced by a project that targets '.NETCoreApp,Version=v3.1'.
[fail]: OmniSharp.MSBuild.ProjectLoader
        Project '..\FsInfoCat.PS\FsInfoCat.PS.csproj' targets 'netcoreapp3.1,net461'. It cannot be referenced by a project that targets '.NETCoreApp,Version=v3.1'.
[warn]: OmniSharp.MSBuild.ProjectManager
        Failed to load project file 'c:\Users\lerwi\Git\FsInfoCat\src\FsInfoCat.Test\FsInfoCat.Test.csproj'.
c:\Users\lerwi\Git\FsInfoCat\src\FsInfoCat.Test\FsInfoCat.Test.csproj
c:\Users\lerwi\.vscode\extensions\ms-dotnettools.csharp-1.23.8\.omnisharp\1.37.5\.msbuild\Current\Bin\Microsoft.Common.CurrentVersion.targets(1662,5): Error: Project '..\FsInfoCat\FsInfoCat.csproj' targets 'netcoreapp3.1,net461'. It cannot be referenced by a project that targets '.NETCoreApp,Version=v3.1'.
c:\Users\lerwi\.vscode\extensions\ms-dotnettools.csharp-1.23.8\.omnisharp\1.37.5\.msbuild\Current\Bin\Microsoft.Common.CurrentVersion.targets(1662,5): Error: Project '..\FsInfoCat.PS\FsInfoCat.PS.csproj' targets 'netcoreapp3.1,net461'. It cannot be referenced by a project that targets '.NETCoreApp,Version=v3.1'.

[fail]: OmniSharp.MSBuild.ProjectManager
        Attempted to update project that is not loaded: c:\Users\lerwi\Git\FsInfoCat\src\FsInfoCat\FsInfoCat.csproj
[fail]: OmniSharp.MSBuild.ProjectManager
        Attempted to update project that is not loaded: c:\Users\lerwi\Git\FsInfoCat\src\FsInfoCat.Web\FsInfoCat.Web.csproj
[fail]: OmniSharp.MSBuild.ProjectManager
        Attempted to update project that is not loaded: c:\Users\lerwi\Git\FsInfoCat\src\FsInfoCat.PS\FsInfoCat.PS.csproj
[fail]: OmniSharp.MSBuild.ProjectManager
        Attempted to update project that is not loaded: c:\Users\lerwi\Git\FsInfoCat\src\FsInfoCat.Test\FsInfoCat.Test.csproj

```
