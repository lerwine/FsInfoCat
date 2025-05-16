using System;

namespace FsInfoCat.Desktop.Commands
{
    /// <summary>
    /// A command which relays its functionality to other objects by invoking the provided delegate.
    /// </summary>
    /// <remarks>
    /// Creates a new relay command.
    /// </remarks>
    /// <param name="execute">The execution logic.</param>
    /// <param name="allowSimultaneousExecute">Whether the command can be invoked before the previous invocation completes.</param>
    /// <param name="isDisabled">Whether the command is initially disabled.</param>
    public class DelegateCommand<THandlableEventArgs>(Action<THandlableEventArgs> execute, bool allowSimultaneousExecute, bool isDisabled = false) : BaseCommand<THandlableEventArgs>(allowSimultaneousExecute, isDisabled)
        where THandlableEventArgs : IHandlableEventArgs
    {
        private readonly Action<THandlableEventArgs> _execute = execute ?? throw new ArgumentNullException(nameof(execute));

        /// <summary>
        /// Creates a new relay command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public DelegateCommand(Action<THandlableEventArgs> execute) : this(execute, false) { }

        protected override void OnExecute(THandlableEventArgs parameter) { _execute(parameter); }
    }
}
