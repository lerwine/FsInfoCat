using FsInfoCat.Local;
using System;
using System.Windows;

namespace FsInfoCat.Desktop.LocalData.PersonalTagDefinitions
{
    public class DetailsViewModel : DependencyObject
    {
        public DetailsViewModel(PersonalTagDefinition fs, PersonalTagDefinitionListItem entity)
        {
        }

        /// <summary>
        /// Occurs when the <see cref="SaveChanges"/> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> ChangesSaved;

        private void RaiseChangesSaved(object args) => ChangesSaved?.Invoke(this, new Commands.CommandEventArgs(args));
    }
}
