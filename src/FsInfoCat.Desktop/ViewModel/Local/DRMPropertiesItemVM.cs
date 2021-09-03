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
    public class DRMPropertiesItemVM : DbEntityItemVM<DRMPropertySet>
    {
        public DRMPropertiesItemVM(DRMPropertySet entity) : base(entity)
        {
            // DEFERRED: Implement item view model
        }

        protected override DbSet<DRMPropertySet> GetDbSet(LocalDbContext dbContext) => dbContext.DRMPropertySets;

        protected override void OnNestedModelPropertyChanged(string propertyName)
        {
            throw new NotImplementedException();
        }
    }
}
