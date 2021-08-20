using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public class SharedTagDefinitionItemVM : DbEntityItemVM<SharedTagDefinition>
    {
        #region ToggleCurrentItemActivation Property Members

        /// <summary>
        /// Occurs when the <see cref="ToggleCurrentItemActivation">ToggleCurrentItemActivation Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> ToggleActivationRequest;

        private static readonly DependencyPropertyKey ToggleCurrentItemActivationPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ToggleCurrentItemActivation),
            typeof(Commands.RelayCommand), typeof(SharedTagDefinitionItemVM), new PropertyMetadata(null));

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
            typeof(Commands.RelayCommand), typeof(SharedTagDefinitionItemVM), new PropertyMetadata(null));

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
            typeof(Commands.RelayCommand), typeof(SharedTagDefinitionItemVM), new PropertyMetadata(null));

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
            typeof(Commands.RelayCommand), typeof(SharedTagDefinitionItemVM), new PropertyMetadata(null));

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

        private static readonly DependencyPropertyKey NamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(Name), typeof(string), typeof(SharedTagDefinitionItemVM), new PropertyMetadata(""));

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

        private static readonly DependencyPropertyKey IsInactivePropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsInactive), typeof(bool), typeof(SharedTagDefinitionItemVM),
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

        private static readonly DependencyPropertyKey DescriptionPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Description), typeof(string), typeof(SharedTagDefinitionItemVM), new PropertyMetadata(""));

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
        #region VolumeCount Property Members

        private static readonly DependencyPropertyKey VolumeCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(VolumeCount), typeof(int), typeof(SharedTagDefinitionItemVM),
                new PropertyMetadata(0, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as SharedTagDefinitionItemVM).OnVolumeCountPropertyChanged((int)e.OldValue, (int)e.NewValue)));

        /// <summary>
        /// Identifies the <see cref="VolumeCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VolumeCountProperty = VolumeCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public int VolumeCount { get => (int)GetValue(VolumeCountProperty); private set => SetValue(VolumeCountPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="VolumeCount"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="VolumeCount"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="VolumeCount"/> property.</param>
        private void OnVolumeCountPropertyChanged(int oldValue, int newValue)
        {
            DeleteCurrentItem.IsEnabled = newValue == 0 && FileCount == 0;
        }

        #endregion
        #region SubdirectoryCount Property Members

        private static readonly DependencyPropertyKey SubdirectoryCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SubdirectoryCount), typeof(int), typeof(SharedTagDefinitionItemVM),
                new PropertyMetadata(0, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as SharedTagDefinitionItemVM).OnSubdirectoryCountPropertyChanged((int)e.OldValue, (int)e.NewValue)));

        /// <summary>
        /// Identifies the <see cref="SubdirectoryCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SubdirectoryCountProperty = SubdirectoryCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public int SubdirectoryCount { get => (int)GetValue(SubdirectoryCountProperty); private set => SetValue(SubdirectoryCountPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="SubdirectoryCount"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="SubdirectoryCount"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="SubdirectoryCount"/> property.</param>
        private void OnSubdirectoryCountPropertyChanged(int oldValue, int newValue)
        {
            // TODO: Implement OnSubdirectoryCountPropertyChanged Logic
        }

        #endregion
        #region FileCount Property Members

        private static readonly DependencyPropertyKey FileCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(FileCount), typeof(int), typeof(SharedTagDefinitionItemVM),
                new PropertyMetadata(0, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as SharedTagDefinitionItemVM).OnFileCountPropertyChanged((int)e.OldValue, (int)e.NewValue)));

        /// <summary>
        /// Identifies the <see cref="FileCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FileCountProperty = FileCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public int FileCount { get => (int)GetValue(FileCountProperty); internal set => SetValue(FileCountPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="FileCount"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="FileCount"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="FileCount"/> property.</param>
        private void OnFileCountPropertyChanged(int oldValue, int newValue)
        {
            DeleteCurrentItem.IsEnabled = newValue == 0 && VolumeCount == 0;
        }

        #endregion

        internal SharedTagDefinitionItemVM([DisallowNull] SharedTagDefinitionsPageVM.EntityAndCounts result)
            : this(result.Entity)
        {
            VolumeCount = result.VolumeCount;
            SubdirectoryCount = result.SubdirectoryCount;
            FileCount = result.FileCount;
        }

        internal SharedTagDefinitionItemVM([DisallowNull] SharedTagDefinition model) : base(model)
        {
            Name = model.Name;
            Description = model.Description;
            IsInactive = model.IsInactive;
            SetValue(OpenVolumesWindowPropertyKey, new Commands.RelayCommand(RaiseViewVolumesRequest));
            SetValue(OpenSubdirectoriesWindowPropertyKey, new Commands.RelayCommand(RaiseViewSubdirectoriesRequest));
            SetValue(OpenFilesWindowPropertyKey, new Commands.RelayCommand(RaiseViewFilesRequest));
            SetValue(ToggleCurrentItemActivationPropertyKey, new Commands.RelayCommand(RaiseToggleActivationRequest));
        }

        protected override DbSet<SharedTagDefinition> GetDbSet(LocalDbContext dbContext) => dbContext.SharedTagDefinitions;

        protected override void OnModelPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(SharedTagDefinition.Description):
                    Description = Model?.Description;
                    break;
                case nameof(SharedTagDefinition.IsInactive):
                    IsInactive = Model?.IsInactive ?? false;
                    break;
                case nameof(SharedTagDefinition.Name):
                    Name = Model?.Name;
                    break;
            }
        }
    }
}
