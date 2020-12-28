# Development Notes

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
