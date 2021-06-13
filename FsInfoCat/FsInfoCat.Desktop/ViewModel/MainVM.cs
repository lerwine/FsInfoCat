using Microsoft.EntityFrameworkCore;
using FsInfoCat.Local;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;

namespace FsInfoCat.Desktop.ViewModel
{
    public class MainVM : DependencyObject
    {
        private ILogger<MainVM> _logger;
    }
}
