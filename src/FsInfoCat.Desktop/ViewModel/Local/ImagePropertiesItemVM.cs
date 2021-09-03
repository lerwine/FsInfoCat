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
    public class ImagePropertiesItemVM : DbEntityItemVM<ImagePropertySet>
    {
        public ImagePropertiesItemVM(ImagePropertySet entity) : base(entity)
        {
            // DEFERRED: Implement item view model
        }

        protected override DbSet<ImagePropertySet> GetDbSet(LocalDbContext dbContext) => dbContext.ImagePropertySets;

        protected override void OnNestedModelPropertyChanged(string propertyName)
        {
            throw new NotImplementedException();
        }
    }
}
