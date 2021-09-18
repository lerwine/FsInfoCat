using System;

namespace FsInfoCat.Desktop.Commands
{
    /// <summary>
    /// A command which relays its functionality to other objects by invoking the provided delegate.
    /// </summary>
    public class RelayCommand : BaseCommand
    {
        private readonly Action<object> _execute;

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public RelayCommand(Action execute) : this(execute, false) { }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public RelayCommand(Action<object> execute) : this(execute, false) { }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="allowSimultaneousExecute">Whether the command can be invoked before the previous invocation completes.</param>
        /// <param name="isDisabled">Whether the command is initially disabled.</param>
        public RelayCommand(Action execute, bool allowSimultaneousExecute, bool isDisabled = false)
            : base(allowSimultaneousExecute, isDisabled)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));
            _execute = (object obj) => execute();
        }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="allowSimultaneousExecute">Whether the command can be invoked before the previous invocation completes.</param>
        /// <param name="isDisabled">Whether the command is initially disabled.</param>
        public RelayCommand(Action<object> execute, bool allowSimultaneousExecute, bool isDisabled = false)
            : base(allowSimultaneousExecute, isDisabled)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        protected override void OnExecute(object parameter) { _execute(parameter); }
    }
}
