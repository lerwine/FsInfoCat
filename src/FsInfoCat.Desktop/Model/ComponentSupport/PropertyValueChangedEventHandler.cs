namespace FsInfoCat.Desktop.Model.ComponentSupport
{
    /// <summary>
    /// Represents the method that will handle the <see cref="IModelContext.PropertyValueChanged"/> event raised when a property is changed on the
    /// underlying <see cref="ITypeDescriptorContext.Instance"/> of an <see cref="IModelContext"/> object.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="PropertyValueChangedEventArgs"/> that contains the event data.</param>
    public delegate void PropertyValueChangedEventHandler(object sender, PropertyValueChangedEventArgs e);
}
