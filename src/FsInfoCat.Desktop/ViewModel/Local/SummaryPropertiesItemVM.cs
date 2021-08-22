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
    public class SummaryPropertiesItemVM : DbEntityItemVM<SummaryPropertySet>
    {
        public SummaryPropertiesItemVM(SummaryPropertySet entity) : base(entity)
        {
            // TODO: Implement item view model
        }

        protected override DbSet<SummaryPropertySet> GetDbSet(LocalDbContext dbContext) => dbContext.SummaryPropertySets;

        protected override void OnNestedModelPropertyChanged(string propertyName)
        {
            throw new NotImplementedException();
        }
    }
}
