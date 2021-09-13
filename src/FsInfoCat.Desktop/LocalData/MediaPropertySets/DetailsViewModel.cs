using FsInfoCat.Local;
using System;
using System.Windows;

namespace FsInfoCat.Desktop.LocalData.MediaPropertySets
{
    public class DetailsViewModel : DependencyObject
    {
        private MediaPropertySet fs;
        private MediaPropertiesListItem entity;

        public DetailsViewModel(MediaPropertySet fs, MediaPropertiesListItem entity)
        {
            this.fs = fs;
            this.entity = entity;
        }

        /// <summary>
        /// Occurs when the <see cref="SaveChanges"/> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> ChangesSaved;

        private void RaiseChangesSaved(object args) => ChangesSaved?.Invoke(this, new Commands.CommandEventArgs(args));
    }
}
