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
    public class MusicPropertiesItemVM : DbEntityItemVM<MusicPropertySet>
    {
        public MusicPropertiesItemVM(MusicPropertySet entity) : base(entity)
        {
            // TODO: Implement item view model
        }

        protected override DbSet<MusicPropertySet> GetDbSet(LocalDbContext dbContext) => dbContext.MusicPropertySets;

        protected override void OnModelPropertyChanged(string propertyName)
        {
            throw new NotImplementedException();
        }
    }
}
