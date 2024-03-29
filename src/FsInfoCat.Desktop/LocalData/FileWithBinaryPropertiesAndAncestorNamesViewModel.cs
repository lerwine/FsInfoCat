using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.LocalData
{
    public class FileWithBinaryPropertiesAndAncestorNamesViewModel : FileWithBinaryPropertiesAndAncestorNamesViewModel<FileWithBinaryPropertiesAndAncestorNames>
    {
        public FileWithBinaryPropertiesAndAncestorNamesViewModel([DisallowNull] FileWithBinaryPropertiesAndAncestorNames entity) : base(entity)
        {
        }
    }
}
