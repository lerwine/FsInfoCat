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
    public class AudioPropertiesItemVM : DbEntityItemVM<AudioPropertySet>
    {
        public AudioPropertiesItemVM(AudioPropertySet entity) : base(entity)
        {
            // TODO: Implement item view model
        }

        protected override DbSet<AudioPropertySet> GetDbSet(LocalDbContext dbContext) => dbContext.AudioPropertySets;

        protected override void OnNestedModelPropertyChanged(string propertyName)
        {
            throw new NotImplementedException();
        }
    }
}
