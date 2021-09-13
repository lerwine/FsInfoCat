using FsInfoCat.Local;
using System;
using System.Windows;

namespace FsInfoCat.Desktop.LocalData.GPSPropertySets
{
    public class DetailsViewModel : DependencyObject
    {
        public DetailsViewModel(GPSPropertySet fs, GPSPropertiesListItem entity)
        {
        }

        /// <summary>
        /// Occurs when the <see cref="SaveChanges"/> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> ChangesSaved;

        private void RaiseChangesSaved(object args) => ChangesSaved?.Invoke(this, new Commands.CommandEventArgs(args));

    }
}
