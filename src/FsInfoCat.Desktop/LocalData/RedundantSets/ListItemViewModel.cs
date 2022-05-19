using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.LocalData.RedundantSets
{
    public class ListItemViewModel : RedundantSetListItemViewModel<RedundantSetListItem>, ILocalCrudEntityRowViewModel<RedundantSetListItem>
    {
        #region LastSynchronizedOn Property Members

        private static readonly DependencyPropertyKey LastSynchronizedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastSynchronizedOn), typeof(DateTime?), typeof(ListItemViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="LastSynchronizedOn"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LastSynchronizedOnProperty = LastSynchronizedOnPropertyKey.DependencyProperty;

        public DateTime? LastSynchronizedOn { get => (DateTime?)GetValue(LastSynchronizedOnProperty); private set => SetValue(LastSynchronizedOnPropertyKey, value); }

        #endregion
        #region SynchronizeNow Command Property Members

        /// <summary>
        /// Occurs when the <see cref="SynchronizeNow"/> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> SynchronizeNowCommand;

        private static readonly DependencyPropertyKey SynchronizeNowPropertyKey = DependencyPropertyBuilder<ListItemViewModel, Commands.RelayCommand>
            .Register(nameof(SynchronizeNow))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="SynchronizeNow"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SynchronizeNowProperty = SynchronizeNowPropertyKey.DependencyProperty;

        public Commands.RelayCommand SynchronizeNow => (Commands.RelayCommand)GetValue(SynchronizeNowProperty);

        /// <summary>
        /// Called when the SynchronizeNow event is raised by <see cref="SynchronizeNow" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="SynchronizeNow" />.</param>
        protected void RaiseSynchronizeNowCommand(object parameter) => SynchronizeNowCommand?.Invoke(this, new(parameter));
        // {
        //   try { OnSynchronizeNowCommand(parameter); }
        //   finally { SynchronizeNowCommand?.Invoke(this, new(parameter)); }
        // }

        /// <summary>
        /// Called when the <see cref="SynchronizeNow">SynchronizeNow Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="SynchronizeNow" />.</param>
        protected virtual void OnSynchronizeNowCommand(object parameter)
        {
            // TODO: Implement OnSynchronizeNowCommand Logic
        }

        #endregion
        #region ShowRedundantFiles Command Property Members

        /// <summary>
        /// Occurs when the <see cref="ShowRedundantFiles"/> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> ShowRedundantFilesCommand;

        private static readonly DependencyPropertyKey ShowRedundantFilesPropertyKey = DependencyPropertyBuilder<ListItemViewModel, Commands.RelayCommand>
            .Register(nameof(ShowRedundantFiles))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ShowRedundantFiles"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowRedundantFilesProperty = ShowRedundantFilesPropertyKey.DependencyProperty;

        public Commands.RelayCommand ShowRedundantFiles => (Commands.RelayCommand)GetValue(ShowRedundantFilesProperty);

        /// <summary>
        /// Called when the ShowRedundantFiles event is raised by <see cref="ShowRedundantFiles" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="ShowRedundantFiles" />.</param>
        protected void RaiseShowRedundantFilesCommand(object parameter) // => ShowRedundantFilesCommand?.Invoke(this, new(parameter));
        {
            try { OnShowRedundantFilesCommand(parameter); }
            finally { ShowRedundantFilesCommand?.Invoke(this, new(parameter)); }
        }

        /// <summary>
        /// Called when the <see cref="ShowRedundantFiles">ShowRedundantFiles Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="ShowRedundantFiles" />.</param>
        protected virtual void OnShowRedundantFilesCommand(object parameter)
        {
            // TODO: Implement OnShowRedundantFilesCommand Logic
        }

        #endregion
        public ListItemViewModel([DisallowNull] RedundantSetListItem entity)
            : base(entity)
        {
            SetValue(SynchronizeNowPropertyKey, new Commands.RelayCommand(RaiseSynchronizeNowCommand));
            SetValue(ShowRedundantFilesPropertyKey, new Commands.RelayCommand(RaiseShowRedundantFilesCommand));
            // TODO: Implement ListItemViewModel
        }
    }
}
