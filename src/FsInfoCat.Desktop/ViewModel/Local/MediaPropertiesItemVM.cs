using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public class MediaPropertiesItemVM : DbEntityItemVM<MediaPropertySet>
    {
        public MediaPropertiesItemVM(MediaPropertySet entity) : base(entity)
        {
            // TODO: Implement item view model
        }

        protected override DbSet<MediaPropertySet> GetDbSet(LocalDbContext dbContext) => dbContext.MediaPropertySets;

        protected override void OnModelPropertyChanged(string propertyName)
        {
            throw new NotImplementedException();
        }
    }
}
