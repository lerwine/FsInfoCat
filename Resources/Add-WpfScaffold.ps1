Param(
    [Parameter(Mandatory = $true)]
    [ValidatePattern('^[A-Z][A-Za-z\d]*$')]
    [string]$Name
)


[System.IO.File]::WriteAllText(($PSScriptRoot | Join-Path -ChildPath "src\FsInfoCat.Desktop\$($Name)Window.xaml"), @"
<Window xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:i="http://schemas.microsoft.com/expression/2010/interactivity"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    xmlns:local="clr-namespace:FsInfoCat.Desktop"
    xmlns:vm="clr-namespace:FsInfoCat.Desktop.ViewModels" mc:Ignorable="d" x:Class="FsInfoCat.Desktop.$($Name)Window"
    Title="$Name" Height="450" Width="800">
    <Window.DataContext>
        <vm:$($Name)ViewModel />
    </Window.DataContext>
</Window>
"@);

[System.IO.File]::WriteAllText(($PSScriptRoot | Join-Path -ChildPath "src\FsInfoCat.Desktop\$($Name)Window.xaml.cs"), @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FsInfoCat.Desktop.ViewModels;

namespace FsInfoCat.Desktop
{
    /// <summary>
    /// Interaction logic for $($Name)Window.xaml
    /// </summary>
    public partial class $($Name)Window : Window
    {
        public $($Name)ViewModel ViewModel { get { return DataContext as $($Name)ViewModel; } }
        
        public $($Name)Window()
        {
            InitializeComponent();
        }
    }
}
"@);

[System.IO.File]::WriteAllText(($PSScriptRoot | Join-Path -ChildPath "src\FsInfoCat.Desktop\ViewModels\$($Name)ViewModel.cs"), @"
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FsInfoCat.Desktop.Commands;

namespace FsInfoCat.Desktop.ViewModels
{
    /// <summary>
    /// View model for <see cref="$($Name)Window" />.xaml
    /// </summary>
    public class $($Name)ViewModel : DependencyObject
    {
        public $($Name)ViewModel()
        {
        }
    }
}
"@);