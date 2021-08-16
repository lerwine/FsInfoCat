using System;

namespace FsInfoCat.Desktop.Commands
{
    /// <summary>
    /// Event data for en event issued by a <see cref="RelayCommand"/>.
    /// <para>Extends <see cref="EventArgs" />.</para>
    /// </summary>
    /// <seealso cref="EventArgs" />
    public class CommandEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandEventArgs"/> class.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method.</param>
        public CommandEventArgs(object parameter) { Parameter = parameter; }

        /// <summary>
        /// Gets the parameter value for the command.
        /// </summary>
        /// <value>The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method.</value>
        public object Parameter { get; }
    }
}
