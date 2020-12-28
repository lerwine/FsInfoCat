# Dev Environment Setup and Configuration

## Initial Setup

1. Download and run the [Visual Studio Code installer for Windows](https://go.microsoft.com/fwlink/?LinkID=534107).
   - [Reference Page](https://code.visualstudio.com/docs/setup/setup-overview)
2. Download and install [.NET Core 3.1 SDK](https://dotnet.microsoft.com/download)
   - If Visual Studio Code was running, you should restart it after intalling the .NET Core SDK.
   - [Reference Page](https://code.visualstudio.com/docs/languages/dotnet)
3. Open Visual Studio Code and Install C# Extension from Microsoft:
   1. Click on Extensions Listing button ![Extensions Button](./img/ExtensionsButton.png)
   2. Look for the C# extension and click 'Install' button if it is shown ![alt](./img/CsExtension.png).
    - [Reference Page](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp)
4. Restart Visual Studio Code.
5. Clone the repository (<https://github.com/lerwine/FsInfoCat.git>) locally, and open the folder in Visual Studio Code (*File* &rArr; *Open Folder...*).
6. Add missing dependencies.
   - As the project is loaded, watch for the alerts for missing dependencies. Alternative, you can click on alert icon on the lower left corner ![Alert Icon](img/AlertIcon.png) to see if there are any. If there are alerts for missing dependencies, click on the link to add those missing dependencies.

## Intialize Database

After the database has been created, you will need to create the password hash for administrative login:

1. Run the `PasswordHelper.ps1` script to generate a password hash.
2. Open `CreateDb.sql` and find the command that starts with `INSERT INTO dbo.Account`.
3. In the `Values` clause, locate the value following immediately following `'admin'`. This is the password hash for the
administrative account. Replace the existing password hash with the one that was generated in step 1.
4. Open a connection to the database and execute the script to create the tables. Any existing tables will be deleted and re-created.

## Configure Database Connection

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

## Development Setup

These steps are required for active development and are not required for the testing environent.

1. Open Visual Studio Code and click on Extensions Listing button ![Extensions Button](./img/ExtensionsButton.png) and install recommended extensions.

2. Install NPM support for VS Code:
    - From Extensions Listing, Look for the npm extesion and click 'Install' button if it is shown
    ![alt](./img/NpmExtension.png).
    - [Reference Page](https://marketplace.visualstudio.com/items?itemName=eg2.vscode-npm-script)
3. Install TypeScript Compiler
   1. Open Terminal (Ctrl+\`)
   2. Run command `npm install -g typescript`
4. Restart Visual Studio Code.
