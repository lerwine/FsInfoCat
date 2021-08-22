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
    public class GPSPropertiesItemVM : DbEntityItemVM<GPSPropertySet>
    {
        public GPSPropertiesItemVM(GPSPropertySet entity) : base(entity)
        {
            // TODO: Implement item view model
        }

        protected override DbSet<GPSPropertySet> GetDbSet(LocalDbContext dbContext) => dbContext.GPSPropertySets;

        protected override void OnModelPropertyChanged(string propertyName)
        {
            throw new NotImplementedException();
        }
    }
}
