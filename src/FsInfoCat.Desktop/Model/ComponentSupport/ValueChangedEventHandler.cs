namespace FsInfoCat.Desktop.Model.ComponentSupport
{
    /// <summary>
    /// Represents the method that will handle the <see cref="IPropertyContext.ValueChanged"/> event raised when the underlying value of
    /// a <see cref="IPropertyContext"/> has changed.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="ValueChangedEventArgs"/> that contains the event data.</param>
    public delegate void ValueChangedEventHandler(object sender, ValueChangedEventArgs e);
}
