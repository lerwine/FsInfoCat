using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    /// <summary>
    /// View model for <see cref="DbEntityListingPageVM{TDbEntity, TItemVM}.Items"/> in the <see cref="PersonalTagDefinitionsPageVM"/> view model.
    /// </summary>
    public class PersonalTagDefinitionItemVM : DbEntityItemVM<PersonalTagDefinitionListItem>
    {
        #region ToggleCurrentItemActivation Property Members

        /// <summary>
        /// Occurs when the <see cref="ToggleCurrentItemActivation">ToggleCurrentItemActivation Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> ToggleActivationRequest;

        private static readonly DependencyPropertyKey ToggleCurrentItemActivationPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ToggleCurrentItemActivation),
            typeof(Commands.RelayCommand), typeof(PersonalTagDefinitionItemVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ToggleCurrentItemActivation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ToggleCurrentItemActivationProperty = ToggleCurrentItemActivationPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand ToggleCurrentItemActivation => (Commands.RelayCommand)GetValue(ToggleCurrentItemActivationProperty);

        /// <summary>
        /// Called when the ToggleCurrentItemActivation event is raised by <see cref="ToggleCurrentItemActivation" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="ToggleCurrentItemActivation" />.</param>
        private void RaiseToggleActivationRequest(object parameter)
        {
            try { OnToggleActivationRequest(parameter); }
            finally { ToggleActivationRequest?.Invoke(this, new(parameter)); }
        }

        /// <summary>
        /// Called when the <see cref="ToggleCurrentItemActivation">ToggleCurrentItemActivation Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="ToggleCurrentItemActivation" />.</param>
        protected virtual void OnToggleActivationRequest(object parameter)
        {
            // TODO: Implement OnToggleActivation Logic
        }

        #endregion
        #region OpenVolumesWindow Command Members

        /// <summary>
        /// Occurs when the <see cref="OpenVolumesWindow">OpenVolumesWindow Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> ViewVolumesRequest;

        private static readonly DependencyPropertyKey OpenVolumesWindowPropertyKey = DependencyProperty.RegisterReadOnly(nameof(OpenVolumesWindow),
            typeof(Commands.RelayCommand), typeof(PersonalTagDefinitionItemVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref=""/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OpenVolumesWindowProperty = OpenVolumesWindowPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand OpenVolumesWindow => (Commands.RelayCommand)GetValue(OpenVolumesWindowProperty);

        /// <summary>
        /// Called when the OpenVolumesWindow event is raised by <see cref="OpenVolumesWindow" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="OpenVolumesWindow" />.</param>
        private void RaiseViewVolumesRequest(object parameter)
        {
            try { OnViewVolumesRequest(parameter); }
            finally { ViewVolumesRequest?.Invoke(this, new(parameter)); }
        }

        /// <summary>
        /// Called when the <see cref="OpenVolumesWindow">OpenVolumesWindow Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="OpenVolumesWindow" />.</param>
        protected virtual void OnViewVolumesRequest(object parameter)
        {
            // TODO: Implement OnOpenVolumes Logic
        }

        #endregion
        #region OpenSubdirectoriesWindow Property Members

        /// <summary>
        /// Occurs when the <see cref="OpenSubdirectoriesWindow">OpenSubdirectoriesWindow Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> ViewSubdirectoriesRequest;

        private static readonly DependencyPropertyKey OpenSubdirectoriesWindowPropertyKey = DependencyProperty.RegisterReadOnly(nameof(OpenSubdirectoriesWindow),
            typeof(Commands.RelayCommand), typeof(PersonalTagDefinitionItemVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="OpenSubdirectoriesWindow"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OpenSubdirectoriesWindowProperty = OpenSubdirectoriesWindowPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand OpenSubdirectoriesWindow => (Commands.RelayCommand)GetValue(OpenSubdirectoriesWindowProperty);

        /// <summary>
        /// Called when the OpenSubdirectoriesWindow event is raised by <see cref="OpenSubdirectoriesWindow" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="OpenSubdirectoriesWindow" />.</param>
        private void RaiseViewSubdirectoriesRequest(object parameter)
        {
            try { OnViewSubdirectoriesRequest(parameter); }
            finally { ViewSubdirectoriesRequest?.Invoke(this, new(parameter)); }
        }

        /// <summary>
        /// Called when the <see cref="OpenSubdirectoriesWindow">OpenSubdirectoriesWindow Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="OpenSubdirectoriesWindow" />.</param>
        protected virtual void OnViewSubdirectoriesRequest(object parameter)
        {
            // TODO: Implement OnViewSubdirectoriesRequest Logic
        }

        #endregion
        #region OpenFilesWindow Property Members

        /// <summary>
        /// Occurs when the <see cref="OpenFilesWindow">OpenFilesWindow Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> ViewFilesRequest;

        private static readonly DependencyPropertyKey OpenFilesWindowPropertyKey = DependencyProperty.RegisterReadOnly(nameof(OpenFilesWindow),
            typeof(Commands.RelayCommand), typeof(PersonalTagDefinitionItemVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="OpenFilesWindow"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OpenFilesWindowProperty = OpenFilesWindowPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand OpenFilesWindow => (Commands.RelayCommand)GetValue(OpenFilesWindowProperty);

        /// <summary>
        /// Called when the OpenFilesWindow event is raised by <see cref="OpenFilesWindow" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="OpenFilesWindow" />.</param>
        private void RaiseViewFilesRequest(object parameter)
        {
            try { OnViewFilesRequest(parameter); }
            finally { ViewFilesRequest?.Invoke(this, new(parameter)); }
        }

        /// <summary>
        /// Called when the <see cref="OpenFilesWindow">OpenFilesWindow Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="OpenFilesWindow" />.</param>
        protected virtual void OnViewFilesRequest(object parameter)
        {
            // TODO: Implement OnViewFilesRequest Logic
        }

        #endregion
        #region Name Property Members

        private static readonly DependencyPropertyKey NamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(Name), typeof(string), typeof(PersonalTagDefinitionItemVM), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="Name"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NameProperty = NamePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Name { get => GetValue(NameProperty) as string; private set => SetValue(NamePropertyKey, value); }

        #endregion
        #region IsInactive Property Members

        private static readonly DependencyPropertyKey IsInactivePropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsInactive), typeof(bool), typeof(PersonalTagDefinitionItemVM),
                new PropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="IsInactive"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsInactiveProperty = IsInactivePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public bool IsInactive { get => (bool)GetValue(IsInactiveProperty); private set => SetValue(IsInactivePropertyKey, value); }

        #endregion
        #region Description Property Members

        private static readonly DependencyPropertyKey DescriptionPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Description), typeof(string), typeof(PersonalTagDefinitionItemVM), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="Description"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DescriptionProperty = DescriptionPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Description { get => GetValue(DescriptionProperty) as string; private set => SetValue(DescriptionPropertyKey, value); }

        #endregion
        #region VolumeTagCount Property Members

        private static readonly DependencyPropertyKey VolumeTagCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(VolumeTagCount), typeof(long), typeof(PersonalTagDefinitionItemVM),
                new PropertyMetadata(0L, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as PersonalTagDefinitionItemVM).OnVolumeTagCountPropertyChanged((long)e.OldValue, (long)e.NewValue)));

        /// <summary>
        /// Identifies the <see cref="VolumeTagCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VolumeTagCountProperty = VolumeTagCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public long VolumeTagCount { get => (int)GetValue(VolumeTagCountProperty); private set => SetValue(VolumeTagCountPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="VolumeTagCount"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="VolumeTagCount"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="VolumeTagCount"/> property.</param>
        protected void OnVolumeTagCountPropertyChanged(long oldValue, long newValue)
        {
            DeleteCurrentItem.IsEnabled = newValue == 0 && FileTagCount == 0;
        }

        #endregion
        #region SubdirectoryTagCount Property Members

        private static readonly DependencyPropertyKey SubdirectoryTagCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SubdirectoryTagCount), typeof(long), typeof(PersonalTagDefinitionItemVM),
                new PropertyMetadata(0L, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as PersonalTagDefinitionItemVM).OnSubdirectoryTagCountPropertyChanged((long)e.OldValue, (long)e.NewValue)));

        /// <summary>
        /// Identifies the <see cref="SubdirectoryTagCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SubdirectoryTagCountProperty = SubdirectoryTagCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public long SubdirectoryTagCount { get => (long)GetValue(SubdirectoryTagCountProperty); private set => SetValue(SubdirectoryTagCountPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="SubdirectoryTagCount"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="SubdirectoryTagCount"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="SubdirectoryTagCount"/> property.</param>
        protected void OnSubdirectoryTagCountPropertyChanged(long oldValue, long newValue)
        {
            // TODO: Implement OnSubdirectoryTagCountPropertyChanged Logic
        }

        #endregion
        #region FileTagCount Property Members

        private static readonly DependencyPropertyKey FileTagCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(FileTagCount), typeof(long), typeof(PersonalTagDefinitionItemVM),
                new PropertyMetadata(0L, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as PersonalTagDefinitionItemVM).OnFileTagCountPropertyChanged((long)e.OldValue, (long)e.NewValue)));

        /// <summary>
        /// Identifies the <see cref="FileTagCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FileTagCountProperty = FileTagCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public long FileTagCount { get => (long)GetValue(FileTagCountProperty); internal set => SetValue(FileTagCountPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="FileTagCount"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="FileTagCount"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="FileTagCount"/> property.</param>
        protected void OnFileTagCountPropertyChanged(long oldValue, long newValue)
        {
            DeleteCurrentItem.IsEnabled = newValue == 0 && VolumeTagCount == 0;
        }

        #endregion

        internal PersonalTagDefinitionItemVM([DisallowNull] PersonalTagDefinitionListItem model) : base(model)
        {
            SetValue(OpenVolumesWindowPropertyKey, new Commands.RelayCommand(RaiseViewVolumesRequest));
            SetValue(OpenSubdirectoriesWindowPropertyKey, new Commands.RelayCommand(RaiseViewSubdirectoriesRequest));
            SetValue(OpenFilesWindowPropertyKey, new Commands.RelayCommand(RaiseViewFilesRequest));
            SetValue(ToggleCurrentItemActivationPropertyKey, new Commands.RelayCommand(RaiseToggleActivationRequest));
            FileTagCount = model.FileTagCount;
            SubdirectoryTagCount = model.SubdirectoryTagCount;
            VolumeTagCount = model.VolumeTagCount;
            Name = model.Name;
            Description = model.Description;
            IsInactive = model.IsInactive;
        }

        protected override DbSet<PersonalTagDefinitionListItem> GetDbSet(LocalDbContext dbContext) => dbContext.PersonalTagDefinitionListing;

        protected override void OnNestedModelPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(PersonalTagDefinitionListItem.Description):
                    Dispatcher.CheckInvoke(() => Description = Model?.Description ?? "");
                    break;
                case nameof(PersonalTagDefinitionListItem.IsInactive):
                    Dispatcher.CheckInvoke(() => IsInactive = Model?.IsInactive ?? false);
                    break;
                case nameof(PersonalTagDefinitionListItem.Name):
                    Dispatcher.CheckInvoke(() => Name = Model?.Name ?? "");
                    break;
                case nameof(PersonalTagDefinitionListItem.FileTagCount):
                    Dispatcher.CheckInvoke(() => FileTagCount = Model?.FileTagCount ?? 0L);
                    break;
                case nameof(PersonalTagDefinitionListItem.SubdirectoryTagCount):
                    Dispatcher.CheckInvoke(() => SubdirectoryTagCount = Model?.SubdirectoryTagCount ?? 0L);
                    break;
                case nameof(PersonalTagDefinitionListItem.VolumeTagCount):
                    Dispatcher.CheckInvoke(() => VolumeTagCount = Model?.VolumeTagCount ?? 0L);
                    break;
            }
        }
    }
}
