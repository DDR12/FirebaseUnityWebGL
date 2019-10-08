namespace Firebase.Auth
{
    /// <summary>
    /// Interface implemented by each identity provider.
    /// </summary>
    public interface IUserInfo
    {
        /// <summary>
        /// Gets the display name associated with the user, if any.
        /// </summary>
        string DisplayName { get; set; }
        /// <summary>
        /// Gets email associated with the user, if any.
        /// </summary>
        string Email { get; set; }
        /// <summary>
        /// Gets the photo url associated with the user, if any.
        /// </summary>
        System.Uri PhotoUrl { get; set; }
        /// <summary>
        /// Gets the provider ID for the user (For example, "Facebook").
        /// </summary>
        string ProviderId { get; set; }
        /// <summary>
        /// Gets the unique user ID for the user.
        /// </summary>
        string UserId { get; set; }
    }
}
