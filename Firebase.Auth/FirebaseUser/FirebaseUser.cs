using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Firebase.Auth
{
    /// <summary>
    /// This class allows you to manipulate the profile of a user, link to and unlink from authentication providers, and refresh authentication tokens.
    /// </summary>
    public sealed class FirebaseUser : UserInfo, IDisposable
    {
        /// <summary>
        /// ID of the user reference on the Javascript side.
        /// </summary>
        [JsonProperty("nativeLibID")]
        internal uint NativeLibID { get; private set; }
        /// <summary>
        /// Gets the display name associated with the user, if any.
        /// </summary>
        public override string DisplayName => AuthPInvoke.GetUserDisplayName_WebGL(NativeLibID);
        /// <summary>
        /// Gets email associated with the user, if any.
        /// </summary>
        public override string Email => AuthPInvoke.GetUserEmail_WebGL(NativeLibID);
        /// <summary>
        /// True if user signed in anonymously.
        /// </summary>
        public bool IsAnonymous => AuthPInvoke.GetIsUserAnnonymous_WebGL(NativeLibID);
        /// <summary>
        /// True if the email address associated with this user has been verified.
        /// </summary>
        public bool IsEmailVerified => AuthPInvoke.GetIsUserEmailVerified_WebGL(NativeLibID);
        /// <summary>
        /// Additional info about this user.
        /// </summary>
        public UserMetadata Metadata => JsonConvert.DeserializeObject<UserMetadata>(AuthPInvoke.GetUserMetadata_WebGL(NativeLibID));
        /// <summary>
        /// The user phone number or null.
        /// </summary>
        public string PhoneNumber => AuthPInvoke.GetUserPhoneNumber_WebGL(NativeLibID);
        /// <summary>
        /// The photo url associated with the user, if any.
        /// </summary>
        public override Uri PhotoUrl => FirebaseApp.UrlStringToUri(AuthPInvoke.GetUserPhotoUrl_WebGL(NativeLibID));
        /// <summary>
        /// Gets the third party profile data associated with this user returned by the authentication server, if any.
        /// </summary>
        public List<IUserInfo> ProviderData => JsonConvert.DeserializeObject<List<UserInfo>>(AuthPInvoke.GetUserProviderData_WebGL(NativeLibID)).Cast<IUserInfo>().ToList();
        /// <summary>
        /// Gets the provider ID for the user (For example, "Facebook").
        /// </summary>
        public override string ProviderId => AuthPInvoke.GetUserProviderID_WebGL(NativeLibID);
        /// <summary>
        /// Gets the unique Firebase user ID for the user.
        /// </summary>
        public override string UserId => AuthPInvoke.GetUserID_WebGL(NativeLibID);

        #region WebGL-Extra Goodies
        /// <summary>
        /// The auth refresh token for this user, used to manually force refresh the current auth token of this user.
        /// </summary>
        public string RefreshToken => AuthPInvoke.GetUserRefreshToken_WebGL(NativeLibID);

        #endregion
        private FirebaseUser() { }
        /// <summary>
        /// 
        /// </summary>
        ~FirebaseUser()
        {
            Dispose();
        }
        #region Default Methods
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            AuthPInvoke.ReleaseUser_WebGL(NativeLibID);
        }
        /// <summary>
        /// Deletes and signs out the user.
        /// Important: this is a security-sensitive operation that requires the user to have recently signed in.
        /// If this requirement isn't met, ask the user to authenticate again and then call <see cref="ReauthenticateAndRetrieveDataAsync(Credential)"/>
        /// </summary>
        /// <returns></returns>
        public Task DeleteAsync()
        {
            var task = WebGLTaskManager.GetTask();
            AuthPInvoke.DeleteUser_WebGL(task.ID, NativeLibID, WebGLTaskManager.DequeueTask);
            return task.Promise.Task;
        }

        /// <summary>
        /// Links the user account with the given credentials.
        /// </summary>
        /// <param name="credential">The auth credential.</param>
        /// <returns></returns>
        public Task<SignInResult> LinkAndRetrieveDataWithCredentialAsync(Credential credential)
        {
            Credential.CheckIsEmpty(credential);
            var task = WebGLTaskManager.GetTask<SignInResult>();
            AuthPInvoke.LinkWithCredential_WebGL(task.ID, NativeLibID, credential.FullJson, WebGLTaskManager.DequeueTask);
            return task.Promise.Task;
        }

        /// <summary>
        /// Convenience function for <see cref="LinkAndRetrieveDataWithCredentialAsync(Credential)"/> that discards the returned <see cref="AdditionalUserInfo"/> in <see cref="SignInResult"/>.
        /// </summary>
        /// <param name="credential"></param>
        /// <returns></returns>
        public Task<FirebaseUser> LinkWithCredentialAsync(Credential credential)
        {
            TaskCompletionSource<FirebaseUser> task = new TaskCompletionSource<FirebaseUser>();
            var originalTask = LinkAndRetrieveDataWithCredentialAsync(credential);
            originalTask.ContinueWith(result =>
            {
                if (result.IsSuccess())
                    task.SetResult(result.Result.User);
                else if (result.IsCanceled)
                    task.SetCanceled();
                else
                    task.SetException(result.Exception);
            });
            return task.Task;
        }

        /// <summary>
        /// Re-authenticates a user using a fresh credential. 
        /// Use before operations such as <see cref="UpdatePasswordAsync(string)"/> that require tokens from recent sign-in attempts.
        /// </summary>
        /// <param name="credential"></param>
        /// <returns></returns>
        public Task<SignInResult> ReauthenticateAndRetrieveDataAsync(Credential credential)
        {
            Credential.CheckIsEmpty(credential);
            var task = WebGLTaskManager.GetTask<SignInResult>();
            AuthPInvoke.ReauthenticateWithCredential_WebGL(task.ID, NativeLibID, credential.FullJson, WebGLTaskManager.DequeueTask);
            return task.Promise.Task;
        }
        /// <summary>
        /// Convenience function for <see cref="ReauthenticateAndRetrieveDataAsync"/> that discards the returned <see cref="AdditionalUserInfo"/> data.
        /// </summary>
        /// <param name="credential"></param>
        /// <returns></returns>
        public Task ReauthenticateAsync(Credential credential)
        {
            return ReauthenticateAndRetrieveDataAsync(credential);
        }

        /// <summary>
        /// Refreshes the current user, if signed in.
        /// </summary>
        /// <returns></returns>
        public Task ReloadAsync()
        {
            var task = WebGLTaskManager.GetTask();
            AuthPInvoke.ReloadUser_WebGL(task.ID, NativeLibID, WebGLTaskManager.DequeueTask);
            return task.Promise.Task;
        }

        /// <summary>
        /// Initiates email verification for the user.
        /// </summary>
        /// <returns></returns>
        public Task SendEmailVerificationAsync()
        {
            return SendEmailVerificationAsync(null);
        }

        /// <summary>
        /// Returns a JSON Web Token (JWT) used to identify the user to a Firebase service.
        /// Returns the current token if it has not expired.
        /// Otherwise, this will refresh the token and return a new one.
        /// </summary>
        /// <param name="forceRefresh">Force refresh regardless of token expiration.</param>
        /// <returns></returns>
        public Task<string> TokenAsync(bool forceRefresh = false)
        {
            var task = WebGLTaskManager.GetTask<string>();
            AuthPInvoke.GetIdToken_WebGL(task.ID, NativeLibID, forceRefresh, WebGLTaskManager.DequeueTask);
            return task.Promise.Task;
        }

        /// <summary>
        /// Unlinks a provider from a user account.
        /// </summary>
        /// <param name="provider">The id of the provider from which we wish to unlink this user.</param>
        /// <returns></returns>
        public Task<FirebaseUser> UnlinkAsync(string provider)
        {
            PreconditionUtilities.CheckNotNullOrEmpty(provider, nameof(provider));
            var task = WebGLTaskManager.GetTask<FirebaseUser>();
            AuthPInvoke.UnlinkUser_WebGL(task.ID, NativeLibID, provider, WebGLTaskManager.DequeueTask);
            return task.Promise.Task;
        }

        /// <summary>
        /// Updates the user's email address.
        /// An email will be sent to the original email address(if it was set) that allows to revoke the email address change, in order to protect them from account hijacking.
        /// Important: this is a security sensitive operation that requires the user to have recently signed in. 
        /// If this requirement isn't met, ask the user to authenticate again and then call <see cref="ReauthenticateAndRetrieveDataAsync(Credential)"/>
        /// </summary>
        /// <param name="newEmail">The new email address.</param>
        /// <returns></returns>
        public Task UpdateEmailAsync(string newEmail)
        {
            PreconditionUtilities.CheckNotNullOrEmpty(newEmail, nameof(newEmail));
            var task = WebGLTaskManager.GetTask();
            AuthPInvoke.UpdateEmail_WebGL(task.ID, NativeLibID, newEmail, WebGLTaskManager.DequeueTask);
            return task.Promise.Task;
        }
        /// <summary>
        /// Updates the user's password.
        /// Important: this is a security sensitive operation that requires the user to have recently signed in. 
        /// If this requirement isn't met, ask the user to authenticate again and then call <see cref="ReauthenticateAndRetrieveDataAsync(Credential)"/>
        /// </summary>
        /// <param name="newPassword">The new password.</param>
        /// <returns></returns>
        public Task UpdatePasswordAsync(string newPassword)
        {
            PreconditionUtilities.CheckNotNullOrEmpty(newPassword, nameof(newPassword));
            var task = WebGLTaskManager.GetTask();
            AuthPInvoke.UpdatePassword_WebGL(task.ID, NativeLibID, newPassword, WebGLTaskManager.DequeueTask);
            return task.Promise.Task;
        }

        /// <summary>
        /// Updates the user's phone number.
        /// Credential must have been created with <see cref="PhoneAuthProvider.GetCredential(string, string)"/>.
        /// </summary>
        /// <param name="credential"></param>
        /// <returns></returns>
        public Task<FirebaseUser> UpdatePhoneNumberCredentialAsync(Credential credential)
        {
            var task = WebGLTaskManager.GetTask<FirebaseUser>();
            Credential.CheckIsEmpty(credential);
            var nativeTask = WebGLTaskManager.GetTask();
            nativeTask.Promise.Task.ContinueWith(result =>
            {
                if (result.IsSuccess())
                    task.Promise.SetResult(this);
                else if (result.IsCanceled)
                    task.Promise.SetCanceled();
                else
                    task.Promise.SetException(result.Exception);
            });
            AuthPInvoke.UpdatePhoneNumber_WebGL(task.ID, NativeLibID, credential.FullJson, WebGLTaskManager.DequeueTask);
            return task.Promise.Task;
        }

        /// <summary>
        /// Updates a user's profile data.
        /// </summary>
        /// <param name="profile">The data to update, if you add a property name and a value of null for it, this property will be cleared, passing an empty dictionary will reset all values on the user profile.</param>
        /// <returns></returns>
        public Task UpdateUserProfileAsync(UserProfile profile)
        {
            var task = WebGLTaskManager.GetTask();
            AuthPInvoke.UpdateProfile_WebGL(task.ID, NativeLibID, JsonConvert.SerializeObject(profile), WebGLTaskManager.DequeueTask);
            return task.Promise.Task;
        }
        #endregion

        #region WebGL-Extra Goodies
        /// <summary>
        /// Get the current token and info about it, Returns the current token if it has not expired.
        /// Otherwise, this will refresh the token and return a new one.
        /// </summary>
        /// <param name="forceRefresh">Force refresh regardless of token expiration.</param>
        /// <returns>Force refresh regardless of token expiration.</returns>
        public Task<IdTokenResult> GetIdTokenResultAsync(bool forceRefresh = false)
        {
            var task = WebGLTaskManager.GetTask<IdTokenResult>();
            AuthPInvoke.GetIdTokenResult_WebGL(task.ID, NativeLibID, forceRefresh, WebGLTaskManager.DequeueTask);
            return task.Promise.Task;
        }
        /// <summary>
        /// Sends a verification email to a user.
        /// The verification process is completed by calling <see cref="FirebaseAuth.ApplyActionCodeAsync"/>.
        /// </summary>
        /// <param name="actionCodeSettings">
        /// The action code settings.
        /// If specified, the state/continue URL will be set as the "continueUrl" parameter in the email verification link. 
        /// The default email verification landing page will use this to display a link to go back to the app if it is installed. 
        /// If the actionCodeSettings is not specified, no URL is appended to the action URL.
        /// The state URL provided must belong to a domain that is whitelisted by the developer in the console. 
        /// Otherwise an error will be thrown. 
        /// Mobile app redirects will only be applicable if the developer configures and accepts the Firebase Dynamic Links terms of condition. 
        /// The Android package name and iOS bundle ID will be respected only if they are configured in the same Firebase Auth project used.
        /// </param>
        /// <returns></returns>
        public Task SendEmailVerificationAsync(ActionCodeSettings actionCodeSettings)
        {
            var task = WebGLTaskManager.GetTask();
            AuthPInvoke.SendEmailVerification_WebGL(task.ID, NativeLibID, ActionCodeSettings.ToJson(actionCodeSettings),
                WebGLTaskManager.DequeueTask);
            return task.Promise.Task;
        }

        /// <summary>
        /// Reauthenticates the current user with the specified OAuth provider using a full-page redirect flow.
        /// </summary>
        /// <param name="providerID">The provider to authenticate. The provider has to be an OAuth provider. Non-OAuth providers like <see cref="EmailAuthProvider"/> will throw an error.</param>
        /// <param name="scopes">Scopes are what the provider accesses on the user account for example user_birthday user_friends etc...</param>
        /// <param name="parameters">The OAuth parameters for the provider for example Facebook some of its parameters are "auth_type" "display", for a complete list search google for ('provider name' auth provider parameters)</param>
        /// <returns></returns>
        public Task ReauthenticateWithRedirectAsync(string providerID, List<string> scopes = null, Dictionary<string,string> parameters = null)
        {
            PreconditionUtilities.CheckNotNullOrEmpty(providerID, nameof(providerID));
            var task = WebGLTaskManager.GetTask();
            PlatformHandler.CaptureWebGLInput(false);
            AuthProvider provider = new AuthProvider(providerID, scopes, parameters);
            AuthPInvoke.ReauthenticateWithRedirect_WebGL(task.ID, NativeLibID, provider.ToJson(),
            WebGLTaskManager.DequeueTask);
            return task.Promise.Task;
        }
        /// <summary>
        /// Reauthenticates the current user with the specified provider using a pop-up based OAuth flow.
        /// If the reauthentication is successful, the returned result will contain the user and the provider's credential.
        /// </summary>
        /// <param name="providerID">The provider to authenticate. The provider has to be an OAuth provider. Non-OAuth providers like <see cref="EmailAuthProvider"/> will throw an error.</param>
        /// <param name="scopes">Scopes are what the provider accesses on the user account for example user_birthday user_friends etc...</param>
        /// <param name="parameters">The OAuth parameters for the provider for example Facebook some of its parameters are "auth_type" "display", for a complete list search google for ('provider name' auth provider parameters)</param>
        /// <returns></returns>
        public Task<SignInResult> ReauthenticateWithPopupAsync(string providerID, List<string> scopes = null, Dictionary<string, string> parameters = null)
        {
            PreconditionUtilities.CheckNotNullOrEmpty(providerID, nameof(providerID));
            var task = WebGLTaskManager.GetTask<SignInResult>();
            PlatformHandler.CaptureWebGLInput(false);
            AuthProvider provider = new AuthProvider(providerID, scopes, parameters);
            AuthPInvoke.ReauthenticateWithPopup_WebGL(task.ID, NativeLibID, provider.ToJson(), WebGLTaskManager.DequeueTask);
            return task.Promise.Task;
        }
        /// <summary>
        /// Re-authenticates a user using a fresh credential. 
        /// Use before operations such as firebase.User.updatePassword that require tokens from recent sign-in attempts.
        /// </summary>
        /// <param name="phoneNumber">The user's phone number in E.164 format (e.g. +16505550101).</param>
        /// <param name="recaptchaParameters">Optional optins to control the looks of the recaptcha control that is displayed to prevent abuste, you can also leave empty, or use <see cref="RecaptchaParameters.Default"/></param>
        /// <returns></returns>
        public Task<SignInResult> ReauthenticateWithPhoneNumberAsync(string phoneNumber, RecaptchaParameters  recaptchaParameters = null)
        {
            PreconditionUtilities.CheckNotNullOrEmpty(phoneNumber, nameof(phoneNumber));
            var task = WebGLTaskManager.GetTask<SignInResult>();
            AuthPInvoke.ReauthenticateWithPhoneNumber_WebGL(task.ID, NativeLibID, phoneNumber, RecaptchaParameters.ToJson(recaptchaParameters), WebGLTaskManager.DequeueTask);
            return task.Promise.Task;
        }


        /// <summary>
        /// Links the authenticated provider to the user account using a full-page redirect flow.
        /// </summary>
        /// <param name="providerID">The provider to authenticate. The provider has to be an OAuth provider. Non-OAuth providers like <see cref="EmailAuthProvider"/> will throw an error.</param>
        /// <param name="scopes">Scopes are what the provider accesses on the user account for example user_birthday user_friends etc...</param>
        /// <param name="parameters">The OAuth parameters for the provider for example Facebook some of its parameters are "auth_type" "display", for a complete list search google for ('provider name' auth provider parameters)</param>
        /// <returns></returns>
        public Task LinkWithRedirectAsync(string providerID, List<string> scopes = null, Dictionary<string, string> parameters = null)
        {
            PreconditionUtilities.CheckNotNullOrEmpty(providerID, nameof(providerID));
            var task = WebGLTaskManager.GetTask();
            PlatformHandler.CaptureWebGLInput(false);
            AuthProvider provider = new AuthProvider(providerID, scopes, parameters);
            AuthPInvoke.LinkUserWithRedirect_WebGL(task.ID, NativeLibID, provider.ToJson(), WebGLTaskManager.DequeueTask);
            return task.Promise.Task;
        }

        /// <summary>
        /// Links the authenticated provider to the user account using a pop-up based OAuth flow.
        /// If the linking is successful, the returned result will contain the user and the provider's credential.
        /// </summary>
        /// <param name="providerID">The provider to authenticate. The provider has to be an OAuth provider. Non-OAuth providers like <see cref="EmailAuthProvider"/> will throw an error.</param>
        /// <param name="scopes">Scopes are what the provider accesses on the user account for example user_birthday user_friends etc...</param>
        /// <param name="parameters">The OAuth parameters for the provider for example Facebook some of its parameters are "auth_type" "display", for a complete list search google for ('provider name' auth provider parameters)</param>
        /// <returns></returns>
        public Task<SignInResult> LinkWithPopupAsync(string providerID, List<string> scopes = null, Dictionary<string, string> parameters = null)
        {
            PreconditionUtilities.CheckNotNullOrEmpty(providerID, nameof(providerID));
            var task = WebGLTaskManager.GetTask<SignInResult>();
            PlatformHandler.CaptureWebGLInput(false);
            AuthProvider provider = new AuthProvider(providerID, scopes, parameters);
            AuthPInvoke.LinkWithPopup_WebGL(task.ID, NativeLibID, provider.ToJson(), WebGLTaskManager.DequeueTask);
            return task.Promise.Task;
        }
        /// <summary>
        /// Links the user account with the given phone number.
        /// </summary>
        /// <param name="phoneNumber">The user's phone number in E.164 format (e.g. +16505550101).</param>
        /// <param name="recaptchaParameters">Optional optins to control the looks of the recaptcha control that is displayed to prevent abuste, you can also leave empty, or use <see cref="RecaptchaParameters.Default"/></param>
        /// <returns></returns>
        public Task<SignInResult> LinkWithPhoneNumberAsync(string phoneNumber, RecaptchaParameters  recaptchaParameters = null)
        {
            PreconditionUtilities.CheckNotNullOrEmpty(phoneNumber, nameof(phoneNumber));
            var task = WebGLTaskManager.GetTask<SignInResult>();
            PlatformHandler.CaptureWebGLInput(false);
            AuthPInvoke.LinkWithPhoneNumber_WebGL(task.ID, NativeLibID, phoneNumber, RecaptchaParameters.ToJson(recaptchaParameters), WebGLTaskManager.DequeueTask);
            return task.Promise.Task;
        }
        #endregion

        internal static FirebaseUser FromJson(string json)
        {
            if (string.IsNullOrEmpty(json))
                return null;
            return JsonConvert.DeserializeObject<FirebaseUser>(json);
        }
    }
}