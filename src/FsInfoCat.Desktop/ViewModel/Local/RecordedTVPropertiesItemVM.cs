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
    public class RecordedTVPropertiesItemVM : DbEntityItemVM<RecordedTVPropertySet>
    {
        public RecordedTVPropertiesItemVM(RecordedTVPropertySet entity) : base(entity)
        {
            // TODO: Implement item view model
        }

        protected override DbSet<RecordedTVPropertySet> GetDbSet(LocalDbContext dbContext) => dbContext.RecordedTVPropertySets;

        protected override void OnNestedModelPropertyChanged(string propertyName)
        {
            throw new NotImplementedException();
        }
    }
}