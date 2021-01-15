# Development Notes

- [Development Notes](#development-notes)
  - [Git Commands](#git-commands)
    - [Update forked repository from upstream](#update-forked-repository-from-upstream)
  - [Build Tasks](#build-tasks)
  - [MVC Web App CRUD scaffolding](#mvc-web-app-crud-scaffolding)
  - [WPF Desktop App Window MVVM scaffolding](#wpf-desktop-app-window-mvvm-scaffolding)
  - [Creating Dependency Properties for WPF View Models (Desktop App)](#creating-dependency-properties-for-wpf-view-models-desktop-app)
  - [Generating and testing password hashes](#generating-and-testing-password-hashes)

> [Home](../../README.md) | [Setup and Configuration](../README.md)

-------------------------------------------------------------------

## Git Commands

### Update forked repository from upstream

```powershell
git pull --tags origin main
git fetch upstream
git merge upstream/main
git push origin main:main
```

## Build Tasks

| Name                                  | Project / Solution                 | Config Name         | OsPlatform | TargetFramework | Publish Dir                            |
| ------------------------------------- | ---------------------------------- | ------------------- | ---------- | --------------- | -------------------------------------- |
| Build All (.NET Core)                 | FsInfoCat.sln                      | NetCoreDebug        | (any)      | netcoreapp3.1   | n/a                                    |
| Build All (.NET Framework)            | FsInfoCat-NetFramework.sln         | NetFrameworkDebug   | Windows    | net461          | n/a                                    |
| Build Web App                         | FsInfoCat.Web.csproj               | NetCoreDebug        | (any)      | netcoreapp3.1   | n/a                                    |
| Build Module (Windows PowerShell)     | FsInfoCat.PS.NetFramework.csproj   | NetFrameworkDebug   | Windows    | net461          | Debug/Windows/NetFramework/FsInfoCat   |
| Build Module (PS Core)                | FsInfoCat.PS.csproj                | NetCoreDebug        | (any)      | netcoreapp3.1   | (by config/OS)                         |
| Build Test Project (.NET Framework)   | FsInfoCat.Test.NetFramework.csproj | NetFrameworkDebug   | Windows    | net461          | n/a                                    |
| Build Test Project (.NET Core)        | FsInfoCat.Test.csproj              | NetCoreDebug        | (any)      | netcoreapp3.1   | n/a                                    |
| Build Common Library (.NET Framework) | FsInfoCat.NetFramework.csproj      | NetFrameworkDebug   | Windows    | net461          | n/a                                    |
| Build Common Library (.NET Core)      | FsInfoCat.csproj                   | NetCoreDebug        | (any)      | netcoreapp3.1   | n/a                                    |
| Run Unit Tests                        | FsInfoCat.Test.csproj              | NetCoreDebug        | (any)      | netcoreapp3.1   | n/a                                    |
| Publish Web App                       | FsInfoCat.Web.csproj               | NetCoreRelease      | OSX        | netcoreapp3.1   | ?                                      |
| Publish All (.NET Core, Windows)      | FsInfoCat.sln                      | NetCoreRelease      | Windows    | netcoreapp3.1   | ?                                      |
| Publish Module (Windows PowerShell)   | FsInfoCat.PS.NetFramework.csproj   | NetFrameworkRelease | Windows    | net461          | Release/Windows/NetFramework/FsInfoCat |
| Publish Module (PS Core, Windows)     | FsInfoCat.PS.csproj                | NetCoreRelease      | Windows    | netcoreapp3.1   | Release/Windows/NetCore/FsInfoCat      |
| Publish Module (PS Core, Linux)       | FsInfoCat.PS.csproj                | NetCoreRelease      | Linux      | netcoreapp3.1   | Release/Linux/FsInfoCat                |
| Publish Module (PS Core, OSX)         | FsInfoCat.PS.csproj                | NetCoreRelease      | OSX        | netcoreapp3.1   | Release/OSX/FsInfoCat                  |

## MVC Web App CRUD scaffolding

1. Create a new model class in the `src\FsInfoCat.Web\Controllers` folder.
   - You can run the `Add-MvcScaffold.ps1` script to create the class. It will then ask if you want to
   proceed to create the controller and views. You can answer 'no' if you want to modify the model before
   creating the controller and views.
2. Creat the controller and views.
   1. Run the `Add-MvcScaffold.ps1` script. When it prompts you for the name, give the name of the model
   object (without the extension).

## WPF Desktop App Window MVVM scaffolding

Run the `Add-WpfScaffold.ps1` script. When it prompts you for a name, give a camel-cased name,
which starts with a capital letter and containing only numbers and letters.
This should not end in `Window` or `ViewModel`, since the files created will have that appended to the name you give.

## Creating Dependency Properties for WPF View Models (Desktop App)

Run the `New-DependencyProperty.ps1` script. After answering a series of prompts,
the c# code for the new dependency property will be copied to the windows clipboard.

## Generating and testing password hashes

Run the `PasswordHelper.ps1` script to create and testing password hash strings that are compatible with those stored in the database.
