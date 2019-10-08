using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Firebase.Auth
{
    /// <summary>
    /// Firebase authentication object.
    /// </summary>
    public sealed class FirebaseAuth : IDisposable
    {
        static IDictionary<string, FirebaseAuth> authInstances = null;
        static FirebaseAuth defaultInstance = null;
        /// <summary>
        /// Returns the <see cref="FirebaseAuth"/> associated with <see cref="FirebaseApp.DefaultInstance"/>
        /// <see cref="FirebaseAuth"/> will be created if required.
        /// </summary>
        public static FirebaseAuth DefaultInstance
        {
            get
            {
                if (defaultInstance == null)
                    defaultInstance = GetAuth(FirebaseApp.DefaultInstance);
                return defaultInstance;
            }
        }
        static FirebaseAuth()
        {
            authInstances = new Dictionary<string, FirebaseAuth>();
        }
        event EventHandler StateChangedEvent;
        /// <summary>
        /// Add/Remove an observer for changes to the user's sign-in state,
        /// this is triggered only when the user is signed in or signed out, it doesn't trigger when the auth token changed,
        /// to listen to auth token changes as well use <see cref="IdTokenChanged"/>
        /// </summary>
        public event EventHandler StateChanged
        {
            add
            {
                var id = FirebaseWebGLCSharpEventsCoordinator.SubscribeToEvent(StateChangedEvent, value);
                if (id != null)
                    AuthPInvoke.SubscribeToAuthChange_WebGL(App.Name, id.Value, OnAuthChangeCallback_AOT);
            }
            remove
            {
                var id = FirebaseWebGLCSharpEventsCoordinator.UnsubscribeFromEvent(StateChangedEvent, value);
                if (id != null)
                    AuthPInvoke.UnsubscribeToAuthChange_WebGL(id.Value);
            }
        }

        event EventHandler IdTokenChangedEvent;
        /// <summary>
        /// Adds an observer for changes to the signed-in user's ID token, which includes sign-in, sign-out, and token refresh events.
        /// </summary>
        public event EventHandler IdTokenChanged
        {
            add
            {
                var id = FirebaseWebGLCSharpEventsCoordinator.SubscribeToEvent(IdTokenChangedEvent, value);
                if(id != null)
                    AuthPInvoke.SubscribeToIdTokenChange_WebGL(App.Name, id.Value,  OnIdTokenChangedCallback_AOT);
            }
            remove
            {
                var id = FirebaseWebGLCSharpEventsCoordinator.UnsubscribeFromEvent(IdTokenChangedEvent, value);
                if (id != null)
                    AuthPInvoke.UnSubscribeToIdTokenChange_WebGL(id.Value);

            }
        }

        /// <summary>
        /// Get the <see cref="FirebaseApp"/> associated with this object.
        /// </summary>
        public FirebaseApp App { get; }

        /// <summary>
        /// The currently signed-in user (or null).
        /// </summary>
        public FirebaseUser CurrentUser
        {
            get
            {
                var json = AuthPInvoke.GetCurrentAuthUser_WebGL(App.Name);
                return FirebaseUser.FromJson(json);
            }
        }

        /// <summary>
        /// The current Auth instance's language code. 
        /// This is a readable/writable property. 
        /// When set to null, the default Firebase Console language setting is applied.
        /// The language code will propagate to email action templates (password reset, email verification and email change revocation),
        /// SMS templates for phone authentication, reCAPTCHA verifier and OAuth popup/redirect operations provided the specified providers support localization with the language code specified.
        /// </summary>
        public string LanguageCode
        {
            get => AuthPInvoke.GetLanguageCode_WebGL(App.Name);
            set => AuthPInvoke.SetLanguageCode_WebGL(App.Name, value);
        }

        /// <summary>
        /// When set, this property disables app verification for the purpose of testing phone authentication. 
        /// For this property to take effect, it needs to be set before rendering a reCAPTCHA app verifier.
        /// When this is disabled, a mock reCAPTCHA is rendered instead. 
        /// This is useful for manual testing during development or for automated integration tests.
        /// In order to use this feature, you will need to whitelist your phone number see https://firebase.google.com/docs/auth/web/phone-auth#test-with-whitelisted-phone-numbers via the Firebase Console.
        /// The default value is false (app verification is enabled).
        /// </summary>
        public bool AppVerificationDisabledForTesting
        {
            get => AuthPInvoke.GetAppVerificationDisabledForTesting_WebGL(App.Name);
            set => AuthPInvoke.SetAppVerificationDisabledForTesting_WebGL(App.Name, value);
        }
        private FirebaseAuth(FirebaseApp app)
        {
            App = app;
            authInstances.Add(App.Name, this);
        }
        /// <summary>
        /// 
        /// </summary>
        ~FirebaseAuth()
        {
            Dispose();
        }
        #region Shared Implementation
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            authInstances.Remove(App.Name);
        }


        /// <summary>
        /// Creates, and on success, logs in a user with the given email address and password.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="password">The user's chosen password.</param>
        /// <returns></returns>
        public Task<FirebaseUser> CreateUserWithEmailAndPasswordAsync(string email, string password)
        {
            PreconditionUtilities.CheckNotNullOrEmpty(email, nameof(email));
            PreconditionUtilities.CheckNotNullOrEmpty(password, nameof(password));
            var finalTask = new TaskCompletionSource<FirebaseUser>();
            var task = WebGLTaskManager.GetTask<SignInResult>();
            task.Task.ContinueWith(result =>
            {
                if (result.IsSuccess())
                    finalTask.SetResult(result.Result.User);
                else if (result.IsCanceled)
                    finalTask.SetCanceled();
                else
                    finalTask.SetException(result.Exception.InnerExceptions);
            });
            AuthPInvoke.CreateUserWithEmailAndPassword_WebGL(task.Task.Id, App.Name, email, password, WebGLTaskManager.DequeueTask);
            return finalTask.Task;
        }
        /// <summary>
        /// Gets the list of possible sign in methods for the given email address.
        /// This is useful to differentiate methods of sign-in for the same provider, eg. EmailAuthProvider which has 2 methods of sign-in, email/password and email/link.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Task<IEnumerable<string>> FetchProvidersForEmailAsync(string email)
        {
            PreconditionUtilities.CheckNotNullOrEmpty(email, nameof(email));
            var task = WebGLTaskManager.GetTask<IEnumerable<string>>();
            AuthPInvoke.FetchSignInMethodsForEmail_WebGL(task.Task.Id, App.Name, email, WebGLTaskManager.DequeueTask);
            return task.Task;
        }
        /// <summary>
        /// Sends a password reset email to the given email address.
        /// </summary>
        /// <param name="email">The email address with the password to be reset.</param>
        /// <returns></returns>
        public Task SendPasswordResetEmailAsync(string email)
        {
            return SendPasswordResetEmailAsync(email, null);
        }
        /// <summary>
        /// Asynchronously signs in with the given credentials.
        /// </summary>
        /// <returns></returns>
        public Task<SignInResult> SignInAndRetrieveDataWithCredentialAsync(Credential credential)
        {
            Credential.CheckIsEmpty(credential);
            var task = WebGLTaskManager.GetTask<SignInResult>();
            AuthPInvoke.SignInWithCredential_WebGL(task.Task.Id, App.Name, credential.FullJson, WebGLTaskManager.DequeueTask);
            return task.Task;
        }
        /// <summary>
        /// Asynchronously signs in with the given credentials.
        /// </summary>
        /// <returns></returns>
        public Task<FirebaseUser> SignInWithCredentialAsync(Credential credential)
        {
            TaskCompletionSource<FirebaseUser> finalTask = new TaskCompletionSource<FirebaseUser>();
            SignInAndRetrieveDataWithCredentialAsync(credential).ContinueWith(result =>
            {
                if (result.IsSuccess())
                    finalTask.SetResult(result.Result.User);
                else if (result.IsCanceled)
                    finalTask.SetCanceled();
                else
                    finalTask.SetException(result.Exception.InnerExceptions);
            });
            return finalTask.Task;
        }
        /// <summary>
        /// Asynchronously signs in as an anonymous user.
        /// If there is already an anonymous user signed in, that user will be returned; otherwise, a new anonymous user identity will be created and returned.
        /// </summary>
        /// <returns></returns>
        public Task<FirebaseUser> SignInAnonymouslyAsync()
        {
            TaskCompletionSource<FirebaseUser> finalTask = new TaskCompletionSource<FirebaseUser>();
            SignInAndRetrieveDataAnonymouslyAsync().ContinueWith(result =>
            {
                if (result.IsSuccess())
                    finalTask.SetResult(result.Result.User);
                else if (result.IsCanceled)
                    finalTask.SetCanceled();
                else
                    finalTask.SetException(result.Exception.InnerExceptions);
            });
            return finalTask.Task;
        }

        /// <summary>
        /// Asynchronously signs in using a custom token.
        /// Custom tokens are used to integrate Firebase Auth with existing auth systems, and must be generated by the auth backend.
        /// Fails with an error if the token is invalid, expired, or not accepted by the Firebase Auth service.
        /// </summary>
        /// <param name="token">The custom token to sign in with.</param>
        /// <returns></returns>
        public Task<FirebaseUser> SignInWithCustomTokenAsync(string token)
        {
            TaskCompletionSource<FirebaseUser> finalTask = new TaskCompletionSource<FirebaseUser>();
            SignInAndRetrieveDataWithCustomTokenAsync(token).ContinueWith(result =>
            {
                if (result.IsSuccess())
                    finalTask.SetResult(result.Result.User);
                else if (result.IsCanceled)
                    finalTask.SetCanceled();
                else
                    finalTask.SetException(result.Exception.InnerExceptions);
            });
            return finalTask.Task;
        }
        /// <summary>
        /// Asynchronously signs in using an email and password.
        /// Fails with an error if the email address and password do not match.
        /// Note: The user's password is NOT the password used to access the user's email account.
        /// The email address serves as a unique identifier for the user, and the password is used to access the user's account in your Firebase project.
        /// </summary>
        /// <param name="email">The users email address.</param>
        /// <param name="password">The users password.</param>
        /// <returns></returns>
        public Task<FirebaseUser> SignInWithEmailAndPasswordAsync(string email, string password)
        {
            TaskCompletionSource<FirebaseUser> finalTask = new TaskCompletionSource<FirebaseUser>();
            SignInAndRetrieveDataWithEmailAndPasswordAsync(email, password).ContinueWith(result =>
            {
                if (result.IsSuccess())
                    finalTask.SetResult(result.Result.User);
                else if (result.IsCanceled)
                    finalTask.SetCanceled();
                else
                    finalTask.SetException(result.Exception.InnerExceptions);
            });
            return finalTask.Task;
        }
        /// <summary>
        /// Removes any existing authentication credentials from this client.
        /// This function always succeeds.
        /// </summary>
        public void SignOut()
        {
            SignOutAsync();
        }
        #endregion

        #region WebGL-Goodies
        /// <summary>
        /// Asynchronously signs in using an email and password.
        /// Fails with an error if the email address and password do not match.
        /// Note: The user's password is NOT the password used to access the user's email account.
        /// The email address serves as a unique identifier for the user, and the password is used to access the user's account in your Firebase project.
        /// </summary>
        /// <param name="email">The users email address.</param>
        /// <param name="password">The users password.</param>
        /// <returns></returns>
        public Task<SignInResult> SignInAndRetrieveDataWithEmailAndPasswordAsync(string email, string password)
        {
            PreconditionUtilities.CheckNotNullOrEmpty(email, nameof(email));
            PreconditionUtilities.CheckNotNullOrEmpty(password, nameof(password));
            var task = WebGLTaskManager.GetTask<SignInResult>();
            AuthPInvoke.SignInWithEmailAndPassword_WebGL(task.Task.Id,
               App.Name, email, password, WebGLTaskManager.DequeueTask);
            return task.Task;
        }

        /// <summary>
        /// Asynchronously signs in as an anonymous user.
        /// If there is already an anonymous user signed in, that user will be returned; otherwise, a new anonymous user identity will be created and returned.
        /// </summary>
        /// <returns></returns>
        public Task<SignInResult> SignInAndRetrieveDataAnonymouslyAsync()
        {
            var task = WebGLTaskManager.GetTask<SignInResult>();
            AuthPInvoke.SignInAnonymously_WebGL(task.Task.Id, App.Name, WebGLTaskManager.DequeueTask);
            return task.Task;
        }
        /// <summary>
        /// Asynchronously signs in using a custom token.
        /// Custom tokens are used to integrate Firebase Auth with existing auth systems, and must be generated by the auth backend.
        /// Fails with an error if the token is invalid, expired, or not accepted by the Firebase Auth service.
        /// </summary>
        /// <param name="token">The custom token to sign in with.</param>
        /// <returns></returns>
        public Task<SignInResult> SignInAndRetrieveDataWithCustomTokenAsync(string token)
        {
            PreconditionUtilities.CheckNotNullOrEmpty(token, nameof(token));
            var task = WebGLTaskManager.GetTask<SignInResult>();
            AuthPInvoke.SignInWithCustomToken_WebGL(task.Task.Id, App.Name, token, WebGLTaskManager.DequeueTask);
            return task.Task;
        }
        /// <summary>
        /// Sets the current language to the default device/browser preference.
        /// </summary>
        public void UseDeviceLanguage()
        {
            AuthPInvoke.AuthUseDeviceLanguage_WebGL(App.Name);
        }
        /// <summary>
        /// Changes the current type of persistence on the current Auth instance for the currently saved Auth session and applies
        /// this type of persistence for future sign-in requests, including sign-in with redirect requests.
        /// This will return a task that will resolve once the state finishes copying from one type of storage to the other. 
        /// Calling a sign-in method after changing persistence will wait for that persistence change to complete before applying it on the new Auth state.
        /// This makes it easy for a user signing in to specify whether their session should be remembered or not. 
        /// It also makes it easier to never persist the Auth state for applications that are shared by other users or have sensitive data.
        /// The default for web browser apps and React Native apps is 'local' (provided the browser supports this mechanism)
        /// whereas it is 'none' for Node.js backend apps.
        /// </summary>
        /// <param name="persistence">The type of auth persistence to use.</param>
        /// <returns></returns>
        public Task SetPersistenceAsync(PersistenceType persistence)
        {
            var task = WebGLTaskManager.GetTask();
            AuthPInvoke.SetAuthPersistence_WebGL(task.Task.Id, App.Name, persistence.ToString().ToLower(), WebGLTaskManager.DequeueTask);
            return task.Task;
        }

        /// <summary>
        /// Sends a password reset email to the given email address.
        /// </summary>
        /// <param name="email">The email address with the password to be reset.</param>
        /// <param name="actionCodeSettings">The action code settings. If specified, the state/continue URL will be set as the "continueUrl" parameter in the password reset link. The default password reset landing page will use this to display a link to go back to the app if it is installed. If the actionCodeSettings is not specified, no URL is appended to the action URL. The state URL provided must belong to a domain that is whitelisted by the developer in the console. Otherwise an error will be thrown. Mobile app redirects will only be applicable if the developer configures and accepts the Firebase Dynamic Links terms of condition. The Android package name and iOS bundle ID will be respected only if they are configured in the same Firebase Auth project used.</param>
        /// <returns></returns>
        public Task SendPasswordResetEmailAsync(string email, ActionCodeSettings actionCodeSettings)
        {
            PreconditionUtilities.CheckNotNullOrEmpty(email, nameof(email));
            var task = WebGLTaskManager.GetTask();
            AuthPInvoke.SendPasswordResetEmail_WebGL(task.Task.Id, App.Name, email, ActionCodeSettings.ToJson(actionCodeSettings), WebGLTaskManager.DequeueTask);
            return task.Task;
        }
        /// <summary>
        /// Signs out the current user.
        /// </summary>
        /// <returns></returns>
        public Task SignOutAsync()
        {
            var task = WebGLTaskManager.GetTask();
            AuthPInvoke.SignOut_WebGL(task.Task.Id, App.Name, WebGLTaskManager.DequeueTask);
            return task.Task;
        }
        /// <summary>
        /// Checks a password reset code sent to the user by email or other out-of-band mechanism.
        /// Returns the user's email address if valid.
        /// </summary>
        /// <param name="code">A verification code sent to the user.</param>
        /// <returns>Returns the user's email address if valid.</returns>
        public Task<string> VerifyPasswordResetCodeAsync(string code)
        {
            var task = WebGLTaskManager.GetTask<string>();
            AuthPInvoke.VerifyPasswordResetCode_WebGL(task.Task.Id, App.Name, code, WebGLTaskManager.DequeueTask);
            return task.Task;
        }
        /// <summary>
        /// Asynchronously sets the provided user as currentUser on the current Auth instance. A new instance copy of the user provided will be made and set as currentUser.
        /// This will trigger <see cref="StateChanged"/> and <see cref="IdTokenChanged"/> listeners like any other sign in methods.
        /// The operation fails with an error if the user to be updated belongs to a different Firebase project.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task UpdateCurrentUserAsync(FirebaseUser user)
        {
            var task = WebGLTaskManager.GetTask();
            AuthPInvoke.UpdateCurrentAuthUser_WebGL(task.Task.Id, App.Name, user.NativeLibID, WebGLTaskManager.DequeueTask);
            return task.Task;
        }

        /// <summary>
        /// Checks if an incoming link is a sign-in with email link.
        /// </summary>
        /// <param name="emailLink">The Email Link to Check</param>
        /// <returns></returns>
        public bool IsSignInWithEmailLink(string emailLink)
        {
            PreconditionUtilities.CheckNotNullOrEmpty(emailLink, nameof(emailLink));
            return AuthPInvoke.IsSignedInWithEmailLink_WebGL(App.Name, emailLink);
        }

        /// <summary>
        /// Asynchronously signs in using a phone number. This method sends a code via SMS to the given phone number.
        /// After the user provides the code sent to their phone, call firebase.auth.ConfirmationResult.confirm with the code to sign the user in.
        /// For abuse prevention, this method also requires a firebase.auth.ApplicationVerifier. 
        /// The Firebase Auth SDK includes a reCAPTCHA-based implementation,  <see cref="RecaptchaParameters"/>
        /// </summary>
        /// <param name="phoneNumber">The user's phone number in E.164 format (e.g. +16505550101).</param>
        /// <param name="recaptchaParameters">Optional optins to control the looks of the recaptcha control that is displayed to prevent abuste, you can also leave empty, or use <see cref="RecaptchaParameters.Default"/></param>
        /// <returns></returns>
        public Task<SignInResult> SignInWithPhoneNumberAsync(string phoneNumber, RecaptchaParameters recaptchaParameters = null)
        {
            PreconditionUtilities.CheckNotNullOrEmpty(phoneNumber, nameof(phoneNumber));
            PlatformHandler.CaptureWebGLInput(false);
            var task = WebGLTaskManager.GetTask<SignInResult>();
            AuthPInvoke.SignInWithPhoneNumber_WebGL(task.Task.Id, App.Name, phoneNumber, RecaptchaParameters.ToJson(recaptchaParameters), WebGLTaskManager.DequeueTask);
            return task.Task;
        }

        /// <summary>
        /// Sends a sign-in email link to the user with the specified email.
        /// </summary>
        /// <param name="email">The email account to sign in with.</param>
        /// <param name="actionCodeSettings">The action code settings. 
        /// The action code settings which provides Firebase with instructions on how to construct the email link. 
        /// This includes the sign in completion URL or the deep link for mobile redirects, the mobile apps to use when the sign-in link is opened on an Android or iOS device. 
        /// Mobile app redirects will only be applicable if the developer configures and accepts the Firebase Dynamic Links terms of condition. 
        /// The Android package name and iOS bundle ID will be respected only if they are configured in the same Firebase Auth project used.</param>
        /// <returns></returns>
        public Task SendSignInLinkToEmailAsync(string email, ActionCodeSettings actionCodeSettings = null)
        {
            PreconditionUtilities.CheckNotNullOrEmpty(email, nameof(email));
            var task = WebGLTaskManager.GetTask();
            AuthPInvoke.SendSignInLinkToEmail_WebGL(task.Task.Id, App.Name, email, ActionCodeSettings.ToJson(actionCodeSettings), WebGLTaskManager.DequeueTask);
            return task.Task;
        }
        /// <summary>
        /// Asynchronously signs in using an email and sign-in email link. If no link is passed, the link is inferred from the current URL.
        /// Fails with an error if the email address is invalid or OTP in email link expires.
        /// Note: Confirm the link is a sign-in email link before calling this method
        /// </summary>
        /// <param name="email">The email account to sign in with.</param>
        /// <param name="emailLink">The optional link which contains the OTP needed to complete the sign in with email link. If not specified, the current URL is used instead.</param>
        /// <returns></returns>
        public Task<SignInResult> SignInWithEmailLinkAsync(string email, string emailLink = null)
        {
            PreconditionUtilities.CheckNotNullOrEmpty(email, nameof(email));
            // email link can be null, if it is null, the link is infered from the current URL of the browser.
            var task = WebGLTaskManager.GetTask<SignInResult>();
            AuthPInvoke.SignInWithEmailLink_WebGL(task.Task.Id, App.Name, email, emailLink, WebGLTaskManager.DequeueTask);
            return task.Task;
        }

        /// <summary>
        /// Applies a verification code sent to the user by email or other out-of-band mechanism.
        /// </summary>
        /// <param name="code">A verification code sent to the user.</param>
        /// <returns></returns>
        public Task ApplyActionCodeAsync(string code)
        {
            PreconditionUtilities.CheckNotNullOrEmpty(code, nameof(code));
            var task = WebGLTaskManager.GetTask();
            AuthPInvoke.ApplyAuthActionCode_WebGL(task.Task.Id, App.Name, code, WebGLTaskManager.DequeueTask);
            return task.Task;
        }

        /// <summary>
        /// Checks a verification code sent to the user by email or other out-of-band mechanism.
        /// </summary>
        /// <param name="code">The verification code to check.</param>
        /// <returns></returns>
        public Task<ActionCodeInfo> CheckActionCodeAsync(string code)
        {
            PreconditionUtilities.CheckNotNullOrEmpty(code, nameof(code));
            var task = WebGLTaskManager.GetTask<ActionCodeInfo>();
            AuthPInvoke.CheckAuthActionCode_WebGL(task.Task.Id, App.Name, code, WebGLTaskManager.DequeueTask);
            return task.Task;
        }
        /// <summary>
        /// Completes the password reset process, given a confirmation code and new password.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public Task ConfirmPassowrdResetAsync(string code, string newPassword)
        {
            PreconditionUtilities.CheckNotNullOrEmpty(code, nameof(code));
            PreconditionUtilities.CheckNotNullOrEmpty(newPassword, nameof(newPassword));
            var task = WebGLTaskManager.GetTask();
            AuthPInvoke.ConfirmPasswordReset_WebGL(task.Task.Id, App.Name, code, newPassword, WebGLTaskManager.DequeueTask);
            return task.Task;
        }

        /// <summary>
        /// Returns a <see cref="SignInResult"/> from the redirect-based sign-in flow.
        /// If sign-in succeeded, returns the signed in user.
        /// If sign-in was unsuccessful, fails with an error.
        /// If no redirect operation was called, returns a UserCredential with a null User.
        /// https://firebase.google.com/docs/reference/js/firebase.auth.Auth#get-redirect-result
        /// </summary>
        /// <returns></returns>
        public Task<SignInResult> GetRedirectResultAsync()
        {
            var task = WebGLTaskManager.GetTask<SignInResult>();
            AuthPInvoke.GetAuthRedirectResult_WebGL(task.Task.Id, App.Name, WebGLTaskManager.DequeueTask);
            return task.Task;
        }
        /// <summary>
        /// Authenticates a Firebase client using a full-page redirect flow. To handle the results and errors for this operation, refer to <see cref="GetRedirectResultAsync"/>
        /// </summary>
        /// <param name="providerID">The provider to authenticate. The provider has to be an OAuth provider. Non-OAuth providers like <see cref="EmailAuthProvider"/> will throw an error.</param>
        /// <param name="scopes">Scopes are what the provider accesses on the user account for example user_birthday user_friends etc...</param>
        /// <param name="parameters">The OAuth parameters for the provider for example Facebook some of its parameters are "auth_type" "display", for a complete list search google for ('provider name' auth provider parameters)</param>
        /// <returns></returns>
        public Task SignInWithRedirectAsync(string providerID, List<string> scopes = null, Dictionary<string,string> parameters = null)
        {
            PreconditionUtilities.CheckNotNullOrEmpty(providerID, nameof(providerID));
            PlatformHandler.CaptureWebGLInput(false);
            var task = WebGLTaskManager.GetTask();
            AuthPInvoke.SignInWithRedirect_WebGL(task.Task.Id, App.Name,new AuthProvider(providerID,scopes,parameters).ToJson() , WebGLTaskManager.DequeueTask);
            return task.Task;
        }
        /// <summary>
        /// Authenticates a Firebase client using a popup-based OAuth authentication flow.
        /// If succeeds, returns the signed in user along with the provider's credential. 
        /// If sign in was unsuccessful, returns an error object containing additional information about the error.
        /// </summary>
        /// <param name="providerID">The provider to authenticate. The provider has to be an OAuth provider. Non-OAuth providers like <see cref="EmailAuthProvider"/> will throw an error.</param>
        /// <param name="scopes">Scopes are what the provider accesses on the user account for example user_birthday user_friends etc...</param>
        /// <param name="parameters">The OAuth parameters for the provider for example Facebook some of its parameters are "auth_type" "display", for a complete list search google for ('provider name' auth provider parameters)</param>
        /// <returns></returns>
        public Task<SignInResult> SignInWithPopupAsync(string providerID, List<string> scopes = null, Dictionary<string, string> parameters = null)
        {
            PreconditionUtilities.CheckNotNullOrEmpty(providerID, nameof(providerID));
            PlatformHandler.CaptureWebGLInput(false);
            var task = WebGLTaskManager.GetTask<SignInResult>();
            AuthPInvoke.SignInWithPopup_WebGL(task.Task.Id, App.Name, new AuthProvider(providerID, scopes, parameters).ToJson(), WebGLTaskManager.DequeueTask);
            return task.Task;
        }
        #endregion

        
       

        private void NotifyUserChanged(FirebaseUser user, FirebaseError error)
        {
            StateChangedEvent?.Invoke(this, EventArgs.Empty);
        }
        private void NotifyIdTokenChanged(FirebaseUser user, FirebaseError error)
        {
            IdTokenChangedEvent?.Invoke(this, EventArgs.Empty);
        }

        [AOT.MonoPInvokeCallback(typeof(WebGLIdTokenChangedCallback))]
        static void OnIdTokenChangedCallback_AOT(string appName, string userJsonOrNull, string errorJson)
        {
            if (authInstances.TryGetValue(appName, out FirebaseAuth authInstance))
            {
                FirebaseUser user = FirebaseUser.FromJson(userJsonOrNull);
                FirebaseError error = FirebaseError.FromJson(errorJson);
                authInstance.NotifyIdTokenChanged(user, error);
            }
            else
            {
                Debug.Log($"Auth instance belonging to an app named: {appName} no longer exists.");
            }
        }
        [AOT.MonoPInvokeCallback(typeof(WebGLAuthChangedCallback))]
        static void OnAuthChangeCallback_AOT(string appName, string userJsonOrNull, string errorJson)
        {
            if (authInstances.TryGetValue(appName, out FirebaseAuth authInstance))
            {
                FirebaseUser user = FirebaseUser.FromJson(userJsonOrNull);
                FirebaseError error = FirebaseError.FromJson(errorJson);
                authInstance.NotifyUserChanged(user, error);
            }
            else
            {
                Debug.Log($"Auth instance belonging to an app named: {appName} no longer exists.");
            }
        }

        /// <summary>
        /// Gets or creates a <see cref="FirebaseAuth"/> instance for the passed app, if the app is null, the default app is used and default instance is returned.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static FirebaseAuth GetAuth(FirebaseApp app)
        {
            if (app == null)
            {
                if (defaultInstance == null)
                    defaultInstance = new FirebaseAuth(FirebaseApp.DefaultInstance);
                return defaultInstance;
            }
            return new FirebaseAuth(app);
        }
    }
}
