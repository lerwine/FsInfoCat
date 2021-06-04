using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class MainVM : DependencyObject
    {
        private readonly ILogger<MainVM> _logger;

        public MainVM(ILogger<MainVM> logger)
        {
            _logger = logger;
        }
    }
}
