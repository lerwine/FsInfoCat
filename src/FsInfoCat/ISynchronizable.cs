namespace FsInfoCat
{
    /// <summary>
    /// Interface for objects that have a synchyronization property.
    /// </summary>
    /// <seealso cref="Model.IHasMembershipKeyReference" />
    public interface ISynchronizable
    {
        /// <summary>
        /// The object to use for synchronizing access to the implementing object.
        /// </summary>
        object SyncRoot { get; }
    }
}
