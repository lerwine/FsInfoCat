using System;

namespace FsInfoCat.Desktop.Commands
{
    /// <summary>
    /// A command which relays its functionality to other objects by invoking the provided delegate.
    /// </summary>
    public class DelegateCommand<THandlableEventArgs> : BaseCommand<THandlableEventArgs>
        where THandlableEventArgs : IHandlableEventArgs
    {
        private readonly Action<THandlableEventArgs> _execute;

        /// <summary>
        /// Creates a new relay command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public DelegateCommand(Action<THandlableEventArgs> execute) : this(execute, false) { }

        /// <summary>
        /// Creates a new relay command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="allowSimultaneousExecute">Whether the command can be invoked before the previous invocation completes.</param>
        /// <param name="isDisabled">Whether the command is initially disabled.</param>
        public DelegateCommand(Action<THandlableEventArgs> execute, bool allowSimultaneousExecute, bool isDisabled = false)
            : base(allowSimultaneousExecute, isDisabled)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        protected override void OnExecute(THandlableEventArgs parameter) { _execute(parameter); }
    }
}
