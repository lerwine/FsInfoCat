# Dev Environment Setup and Configuration

> [Home](../../README.md) | [Setup and Configuration](../README.md) | [Development Operations](doc/DevOps.md)

-------------------------------------------------------------------------------------------------------------

## Intialize Database

### Install prerequisite global tools

Open Terminal (Ctrl+\`) and run the following commands:

```powershell
dotnet tool install --global dotnet-user-secrets
```

After the database has been created, you will need to create the password hash for administrative login:

1. Run the `PasswordHelper.ps1` script to generate a password hash.
2. Open `CreateDb.sql` and find the command that starts with `INSERT INTO dbo.Account`.
3. In the `Values` clause, locate the value following immediately following `'admin'`. This is the password hash for the
administrative account. Replace the existing password hash with the one that was generated in step 1.
4. Open a connection to the database and execute the script to create the tables. Any existing tables will be deleted and re-created.

### Configure Database Connection

1. Open the `src\FsInfoCat.Web\FsInfoCat.Web.csproj` file. If the `UserSecretsId` element existings within `Project/PropertyGroup`, remove it.
2. Open the `src\FsInfoCat.Web\appsettings.json` file and modify the `ConnectionStrings/FsInfoCat` property. This contains a sequence of *key*__=__*value* pairs that are separated by semicolons.
3. Enable secret storage and store database password:
    Open Terminal (Ctrl+\`) and run the following commands:
    *(Replace `password in quotes` with the actual database password)*

    ```powershell
    cd src/FsInfoCat.WebApp
    dotnet user-secrets init
    dotnet user-secrets set "DBPassword" "password in quotes"`
    ```

## Testing Setup

These steps are required in order to run the unit tests.

### Upgrade Pester

> The instructions in this section are adapted from the [Install Instructions on Pester's website](https://pester.dev/docs/introduction/installation).

Before upgrading Pester, check to see if you are already running version 5 (or later). Open Terminal (Ctrl+\`) and run the following command:

```powershell
(Get-Module -ListAvailable -Name 'Pester') | Select-Object -ExpandProperty 'Version'
```

#### Upgrading from version 3

Open PowerShell as administrator and run the following commands:

```powershell
$module = "$($env:ProgramFiles)\WindowsPowerShell\Modules\Pester";
takeown /F $module /A /R
icacls $module /reset
icacls $module /grant "*S-1-5-32-544:F" /inheritance:d /T
Remove-Item -Path $module -Recurse -Force -Confirm:$false
Install-Module -Name Pester -Force -SkipPublisherCheck
```

#### Upgrading from version 4

Open PowerShell as administrator and run the following commands:

```powershell
Uninstall-Module -Name Pester -Force -ErrorAction Stop
Install-Module -Name Pester -Force -SkipPublisherCheck
```

## Development Setup

These steps are required for active development and are not required for the testing environent.

### Visual Studio Code Setup

1. Open Visual Studio Code and click on Extensions Listing button ![Extensions Button](./img/ExtensionsButton.png) and install recommended extensions.
2. Install NPM support for VS Code:
    - From Extensions Listing, Look for the npm extesion and click 'Install' button if it is shown
    ![alt](./img/NpmExtension.png).
    - [Reference Page](https://marketplace.visualstudio.com/items?itemName=eg2.vscode-npm-script)
3. Install TypeScript Compiler
   1. Open Terminal (Ctrl+\`)
   2. Run command `npm install -g typescript`
4. Restart Visual Studio Code.

### Global Tools setup

Open Terminal (Ctrl+\`) and run the following commands:

```powershell
dotnet tool install --global dotnet-ef
dotnet tool install --global dotnet-aspnet-codegenerator --version 3.1.4
dotnet tool install --global Microsoft.Web.LibraryManager.Cli
```

-------------------------------------------------------------------------------------------------------------

> [Home](../../README.md) | [Setup and Configuration](../README.md) | [Development Operations](doc/DevOps.md)
