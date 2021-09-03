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
    public class DocumentPropertiesItemVM : DbEntityItemVM<DocumentPropertySet>
    {
        public DocumentPropertiesItemVM(DocumentPropertySet entity) : base(entity)
        {
            // DEFERRED: Implement item view model
        }

        protected override DbSet<DocumentPropertySet> GetDbSet(LocalDbContext dbContext) => dbContext.DocumentPropertySets;

        protected override void OnNestedModelPropertyChanged(string propertyName)
        {
            throw new NotImplementedException();
        }
    }
}
