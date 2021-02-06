# Development Notes

- [Development Notes](#development-notes)
  - [Search/replace patterns](#searchreplace-patterns)
    - [Add Working category](#add-working-category)
    - [Remove Working category](#remove-working-category)
    - [Add Ignore](#add-ignore)
    - [Remove Ignore](#remove-ignore)
    - [Obsolete references](#obsolete-references)
  - [Build Tasks](#build-tasks)
  - [Git Commands](#git-commands)
    - [Update forked repository from upstream](#update-forked-repository-from-upstream)
  - [MVC Web App CRUD scaffolding](#mvc-web-app-crud-scaffolding)
  - [WPF Desktop App Window MVVM scaffolding](#wpf-desktop-app-window-mvvm-scaffolding)
  - [Creating Dependency Properties for WPF View Models (Desktop App)](#creating-dependency-properties-for-wpf-view-models-desktop-app)
  - [Generating and testing password hashes](#generating-and-testing-password-hashes)

> [Home](../../README.md) | [Setup and Configuration](../README.md)

-------------------------------------------------------------------

## Search/replace patterns

### Add Working category

```regex
\[Test,\s+Property\("Priority",\s+(\d+)\)(,\s+Ignore(\(("[^"]*")?\))?,)?\]
[Test, Property("Priority", $1)$2, NUnit.Framework.Category("Working")]
```

### Remove Working category

```regex
\[Test, Property\("Priority",\s+(\d+)\)(,\s+Ignore(\(("[^"]*")?\))?,)?\s+NUnit.Framework.Category\("Working"\)\]
[Test, Property("Priority", $1)$2]
```

### Add Ignore

```regex
\[Test,\s+Property\("Priority",\s+(\d+)\)(,\s+NUnit.Framework.Category\("Working"\))?\]
[Test, Property("Priority", $1), Ignore$2]
```

### Remove Ignore

```regex
\[Test, Property\("Priority",\s+(\d+)\),\s+Ignore(,\s+NUnit.Framework.Category\("Working"\))?\]
[Test, Property("Priority", $1)$2]
```

### Obsolete references

```regex
\[Obsolete(\(("[^"]*")?\))?]
```

## Build Tasks

| Name                                  | Project / Solution      | Config  | RID       | TargetFramework | Publish / Deploy Dir                        | Log File                           |
| ------------------------------------- | ----------------------- | ------- | --------- | --------------- | ------------------------------------------- | ---------------------------------- |
| Publish All              | FsInfoCat.sln           | Release | win10-x64 | netcoreapp3.1   | ?                                           | Publish.Sln.Release.log            |
| Publish PS Module (Desktop)           | FsInfoCat.PS.csproj     | Release | win10-x64 | net461          | Setup\Distro\PS\FsInfoCat-Win64-Desktop.zip | FsInfoCat.PS.Desktop.Release.log   |
| Publish PS Module (Linux)             | FsInfoCat.PS.csproj     | Release | linux-x64 | netcoreapp3.1   | Setup\Distro\PS\FsInfoCat-Linux-NETCore.zip | FsInfoCat.PS.Linux.Release.log     |
| Publish PS Module (OSX)               | FsInfoCat.PS.csproj     | Release | osx-x64   | netcoreapp3.1   | Setup\Distro\PS\FsInfoCat-OSX-NETCore.zip   | FsInfoCat.PS.OSX.Release.log       |
| Run Unit Tests                        | FsInfoCat.Test.csproj   | Debug   | (current) | netcoreapp3.1   | n/a                                         | n/a                                |
| Build All                | FsInfoCat.sln           | Debug   | win10-x64 | netcoreapp3.1   | n/a                                         | All.Debug.log                      |
| Build PS Module (Desktop)             | FsInfoCat.PS.csproj     | Debug   | win10-x64 | net461          | Setup\bin\FsInfoCat                         | FsInfoCat.PS.Desktop.Debug.log     |
| Build PS Module (Linux)               | FsInfoCat.PS.csproj     | Debug   | linux-x64 | netcoreapp3.1   | Setup\bin\FsInfoCat                         | FsInfoCat.PS.Linux.Debug.log       |
| Build PS Module (OSX)                 | FsInfoCat.PS.csproj     | Debug   | osx-x64   | netcoreapp3.1   | Setup\bin\FsInfoCat                         | FsInfoCat.PS.OSX.Debug.log         |
| Build Web App                         | FsInfoCat.Web.csproj    | Debug   | win10-x64 | netcoreapp3.1   | n/a                                         | FsInfoCat.Web.Debug.log            |
| Build PS Module          | FsInfoCat.PS.csproj     | Debug   | win10-x64 | netcoreapp3.1   | Setup\bin\FsInfoCat                         | FsInfoCat.PS.Win64Core.Debug.log   |
| Build Test Project                    | FsInfoCat.Test.csproj   | Debug   | (current) | netcoreapp3.1   | n/a                                         | FsInfoCat.Test.log                 |
| Build Common Library (.NET Framework) | FsInfoCat.csproj        | Debug   | (current) | net461          | n/a                                         | FsInfoCat.Desktop.Debug.log        |
| Build Common Library     | FsInfoCat.csproj        | Debug   | (current) | netcoreapp3.1   | n/a                                         | FsInfoCat.Win64Core.Debug.log      |
| Build Common Library (Linux)          | FsInfoCat.csproj        | Debug   | (current) | netcoreapp3.1   | n/a                                         | FsInfoCat.Linux.Debug.log          |
| Build Common Library (OSX)            | FsInfoCat.csproj        | Debug   | (current) | netcoreapp3.1   | n/a                                         | FsInfoCat.OSX.Debug.log            |
| Build TestHelper Module               | TestHelper.csproj       | Debug   | (current) | netstandard2.0  | dev\bin\TestHelper                          | TestHelper.log                     |
| Build DevHelper Module                | DevHelper.csproj        | Debug   | (current) | netstandard2.0  | dev\bin\DevHelper                           | DevHelper.log                      |
| Build Text Template Host              | StandaloneT4Host.csproj | Debug   | (current) | net472          | Setup\Distro\Util\T4.zip                    | StandaloneT4Host.log               |
| Publish Web App                       | FsInfoCat.Web.csproj    | Release | (current) | netcoreapp3.1   | ?                                           | FsInfoCat.Web.Release.log          |
| Publish PS Module        | FsInfoCat.PS.csproj     | Release | win10-x64 | netcoreapp3.1   | Setup\Distro\PS\FsInfoCat-Win64-NETCore.zip | FsInfoCat.PS.Win64Core.Release.log |

## Git Commands

### Update forked repository from upstream

```powershell
git pull --tags origin main
git fetch upstream
git merge upstream/main
git push origin main:main
```

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
