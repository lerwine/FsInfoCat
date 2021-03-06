namespace FsInfoCat.Models.Accounts
{
    public interface ILogin : IValidatableModel
    {
        /// <summary>
        /// Gets the user's login name.
        /// </summary>
        string LoginName { get; set; }
    }
}
