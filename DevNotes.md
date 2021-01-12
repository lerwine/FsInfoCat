# Development Notes

## Tasks

| Name                                        | Project / Solution                 | Config Name         | OsPlatform | TargetFramework | Publish Dir                            |
| ------------------------------------------- | ---------------------------------- | ------------------- | ---------- | --------------- | -------------------------------------- |
| Build All (.NET Core, Current OS)           | FsInfoCat.sln                      | NetCoreDebug        | (any)      | net461          | n/a                                    |
| Build All (.NET Framework)                  | FsInfoCat.NetFramework.sln         | NetFrameworkDebug   | Windows    | net461          | n/a                                    |
| Build Web App                               | FsInfoCat.Web.csproj               | NetCoreDebug        | (any)      | netcoreapp3.1   | n/a                                    |
| Build Module (Windows PowerShell)           | FsInfoCat.PS.NetFramework.csproj   | NetFrameworkDebug   | Windows    | net461          | Debug/Windows/NetFramework/FsInfoCat   |
| Build Module (PS Core, Current OS)          | FsInfoCat.PS.csproj                | NetCoreDebug        | (any)      | netcoreapp3.1   | (by config/OS)                         |
| Build Module (PS Core, Windows)             | FsInfoCat.PS.csproj                | NetCoreDebug        | Windows    | netcoreapp3.1   | Debug/Windows/NetCore/FsInfoCat        |
| Build Module (PS Core, Linux)               | FsInfoCat.PS.csproj                | NetCoreDebug        | Linux      | netcoreapp3.1   | Debug/Linux/FsInfoCat                  |
| Build Module (PS Core, OSX)                 | FsInfoCat.PS.csproj                | NetCoreDebug        | OSX        | netcoreapp3.1   | Debug/OSX/FsInfoCat                    |
| Build Test Project (.NET Framework)         | FsInfoCat.Test.NetFramework.csproj | NetFrameworkDebug   | Windows    | net461          | n/a                                    |
| Build Test Project (.NET Core, Windows)     | FsInfoCat.Test.csproj              | NetCoreDebug        | Windows    | netcoreapp3.1   | n/a                                    |
| Build Test Project (.NET Core, Linux)       | FsInfoCat.Test.csproj              | NetCoreDebug        | Linux      | netcoreapp3.1   | n/a                                    |
| Build Test Project (.NET Core, OSX)         | FsInfoCat.Test.csproj              | NetCoreDebug        | OSX        | netcoreapp3.1   | n/a                                    |
| Build Common Library (.NET Framework)       | FsInfoCat.NetFramework.csproj      | NetFrameworkDebug   | Windows    | net461          | n/a                                    |
| Build Common Library (.NET Core, Windows)   | FsInfoCat.csproj                   | NetCoreDebug        | Windows    | netcoreapp3.1   | n/a                                    |
| Build Common Library (.NET Core, Linux)     | FsInfoCat.csproj                   | NetCoreDebug        | Linux      | netcoreapp3.1   | n/a                                    |
| Build Common Library (.NET Core, OSX)       | FsInfoCat.csproj                   | NetCoreDebug        | OSX        | netcoreapp3.1   | n/a                                    |
| Test Module (.NET Framework)                | FsInfoCat.Test.NetFramework.csproj | NetFrameworkDebug   | Windows    | net461          | n/a                                    |
| Test Module (PS Core)                       | FsInfoCat.Test.csproj              | NetCoreDebug        | (any)      | netcoreapp3.1   | n/a                                    |
| Test FsInfoCat (PS Core)                    | FsInfoCat.Test.csproj              | NetCoreDebug        | (any)      | netcoreapp3.1   | n/a                                    |
| Publish Module (Windows PowerShell)         | FsInfoCat.PS.NetFramework.csproj   | NetFrameworkRelease | Windows    | net461          | Release/Windows/NetFramework/FsInfoCat |
| Publish Module (PS Core, Windows)           | FsInfoCat.PS.csproj                | NetCoreRelease      | Windows    | netcoreapp3.1   | Release/Windows/NetCore/FsInfoCat      |
| Publish Module (PS Core, Linux)             | FsInfoCat.PS.csproj                | NetCoreRelease      | Linux      | netcoreapp3.1   | Release/Linux/FsInfoCat                |
| Publish Module (PS Core, OSX)               | FsInfoCat.PS.csproj                | NetCoreRelease      | OSX        | netcoreapp3.1   | Release/OSX/FsInfoCat                  |
| Publish Web App                             | FsInfoCat.Web.csproj               | NetCoreRelease      | OSX        | netcoreapp3.1   | ?                                      |

## Launch configurations

| Name                                        | Pre-Launch Task                    |
| ------------------------------------------- | ---------------------------------- |
| Debug Module (Windows PowerShell)           | Build Module (Windows PowerShell)  |
| Debug Module (PS Core, Current OS)          | Build Module (PS Core, Current OS) |
| Debug Web App                               | Build Web App                      |

## Build Configurations

### FsInfoCat.sln Build configurations

| Names                        | Project        | OsPlatform | TargetFramework | Publish Dir               | Build Task                                | Publish Task                      | Test Task                      | Launch Debug                    |
| ---------------------------- | -------------- | ---------- | --------------- | ------------------------- | ----------------------------------------- | --------------------------------- | ------------------------------ | ------------------------------- |
| NetCoreRelease, NetCoreDebug | FsInfoCat      | Windows    | netcoreapp3.1   | n/a                       | Build Common Library (.NET Core, Windows) | n/a                               | n/a                            | n/a                             |
| NetCoreRelease, NetCoreDebug | FsInfoCat.PS   | Windows    | netcoreapp3.1   | Windows/NetCore/FsInfoCat | Build Module (PS Core, Windows)           | Publish Module (PS Core, Windows) | Test Module (PS Core, Windows) | Debug Module (PS Core, Windows) |
| NetCoreRelease, NetCoreDebug | FsInfoCat      | Linux      | netcoreapp3.1   | n/a                       | Build Common Library (.NET Core, Linux)   | n/a                               | n/a                            | n/a                             |
| NetCoreRelease, NetCoreDebug | FsInfoCat.PS   | Linux      | netcoreapp3.1   | Linux/FsInfoCat           | Build Module (PS Core, Linux)             | Publish Module (PS Core, Linux)   | n/a                            | Debug Module (PS Core, Linux)   |
| NetCoreRelease, NetCoreDebug | FsInfoCat      | OSX        | netcoreapp3.1   | n/a                       | Build Common Library (.NET Core, OSX)     | n/a                               | n/a                            | n/a                             |
| NetCoreRelease, NetCoreDebug | FsInfoCat.PS   | OSX        | netcoreapp3.1   | OSX/FsInfoCat             | Build Module (PS Core, OSX)               | Publish Module (PS Core, OSX)     | n/a                            | Debug Module (PS Core, OSX)     |
| NetCoreRelease, NetCoreDebug | FsInfoCat.Web  | Windows    | netcoreapp3.1   | n/a                       | Build Web App                             | Publish Web App                   | Test FsInfoCat                 | Debug Web App                   |
| NetCoreDebug                 | FsInfoCat.Test | Windows    | netcoreapp3.1   | n/a                       | Build Test Project (.NET Core, Windows)   | n/a                               | n/a                            | n/a                             |
| NetCoreDebug                 | FsInfoCat.Test | Linux      | netcoreapp3.1   | n/a                       | Build Test Project (.NET Core, Linux)     | n/a                               | n/a                            | n/a                             |
| NetCoreDebug                 | FsInfoCat.Test | OSX        | netcoreapp3.1   | n/a                       | Build Test Project (.NET Core, OSX)       | n/a                               | n/a                            | n/a                             |

### FsInfoCat-NetFramework.sln Build configurations

| Names                                  | Project        | OsPlatform | TargetFramework | Publish Dir                    | Build Task                            | Publish Task                        | Test Task                        | Launch Debug                  |
| -------------------------------------- | -------------- | ---------- | --------------- | ------------------------------ | ------------------------------------- | ----------------------------------- | -------------------------------- | ----------------------------- |
| NetFrameworkRelease, NetFrameworkDebug | FsInfoCat      | Windows    | net461          | n/a                            | Build Common Library (.NET Framework) | n/a                                 | n/a                              | n/a                           |
| NetFrameworkRelease, NetFrameworkDebug | FsInfoCat.PS   | Windows    | net461          | Windows/NetFramework/FsInfoCat | Build Module (Windows PowerShell)     | Publish Module (Windows PowerShell) | Test Module (Windows PowerShell) | Debug Module (PowerShell ISE) |
| NetFrameworkRelease, NetFrameworkDebug | FsInfoCat.Test | Windows    | net461          | n/a                            | Build Test Project (.NET Framework)   | n/ a                                | n/a                              | n/a                           |

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
