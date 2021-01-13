# Testing Environment Setup / Install Instructions

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

## Database Setup

1. Follow the instructions in [](DatabaseSetup.md) to initialize the database
