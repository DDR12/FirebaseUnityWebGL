using Firebase.WebGL.Threading;
using System.Collections.Generic;

namespace Firebase.Auth
{
    /// <summary>
    /// This class helps match different platform sdks, when an sdk is missing a method but the other provides it, this class is a meeting point which both implementations can be accessed using same code instead of having the user deal with #if #else directive nightmare.
    /// </summary>
    public static class FirebaseAuthExtensions
    {
        /// <summary>
        /// [Effective only in WebGL build]
        /// When set to null, the default Firebase Console language setting is applied.
        /// The language code will propagate to email action templates (password reset, email verification and email change revocation),
        /// SMS templates for phone authentication, reCAPTCHA verifier and OAuth popup/redirect operations provided the specified providers support localization with the language code specified.
        /// </summary>
        /// <param name="firebaseAuth"></param>
        /// <param name="code">The language code.</param>
        public static void SetAuthLanguageCode_WebGL(this FirebaseAuth firebaseAuth, string code)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            firebaseAuth.LanguageCode = code;
#endif
        }
        /// <summary>
        /// [Effective only in WebGL build]
        /// Get the <see cref="FirebaseAuth"/> instance language code,
        /// This language code will propagate to email action templates (password reset, email verification and email change revocation),
        /// SMS templates for phone authentication, reCAPTCHA verifier and OAuth popup/redirect operations provided the specified providers support localization with the language code specified.
        /// </summary>
        public static string GetAuthLanguageCode_WebGL(this FirebaseAuth firebaseAuth)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            return firebaseAuth.LanguageCode;
#else
            return null;
#endif
        }


        /// <summary>
        /// [Effective only in WebGL build]
        /// When set, this property disables app verification for the purpose of testing phone authentication. 
        /// For this property to take effect, it needs to be set before rendering a reCAPTCHA app verifier.
        /// When this is disabled, a mock reCAPTCHA is rendered instead. 
        /// This is useful for manual testing during development or for automated integration tests.
        /// In order to use this feature, you will need to whitelist your phone number see also https://firebase.google.com/docs/auth/web/phone-auth#test-with-whitelisted-phone-numbers via the Firebase Console.
        /// The default value is false (app verification is enabled).
        /// </summary>
        /// <param name="firebaseAuth"></param>
        /// <param name="disabled"></param>
        public static void SetAppVerificationDisabledForTesting_WebGL(this FirebaseAuth firebaseAuth, bool disabled)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            firebaseAuth.AppVerificationDisabledForTesting = disabled;
#endif
        }
        /// <summary>
        /// [Effective only in WebGL build]
        /// When set, this property disables app verification for the purpose of testing phone authentication. 
        /// For this property to take effect, it needs to be set before rendering a reCAPTCHA app verifier.
        /// When this is disabled, a mock reCAPTCHA is rendered instead. 
        /// This is useful for manual testing during development or for automated integration tests.
        /// In order to use this feature, you will need to whitelist your phone number see also https://firebase.google.com/docs/auth/web/phone-auth#test-with-whitelisted-phone-numbers via the Firebase Console.
        /// The default value is false (app verification is enabled).
        /// </summary>
        /// <returns>True if app verification is disabled, false if it is enabled.</returns>
        public static bool GetAppVerificationDisabledForTesting_WebGL(this FirebaseAuth firebaseAuth)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            return firebaseAuth.AppVerificationDisabledForTesting;
#else
            return false;
#endif
        }

        /// <summary>
        /// [Effective only in WebGL build]
        /// Gets the refresh token of the user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>The refresh token or null.</returns>
        public static string GetRefreshToken_WebGL(this FirebaseUser user)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            return user.RefreshToken;
#else
            return null;
#endif
        }

        /// <summary>
        /// [The actionCodeSettings parameter is effective only in WebGL Builds.]
        /// Sends a verification email to a user.
        /// The verification process is completed by calling <see cref="ApplyActionCodeAsync_WebGL"/>.
        /// </summary>
        /// <param name="user"></param>
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
        public static Task SendEmailVerificationAsync(this FirebaseUser user, ActionCodeSettings actionCodeSettings)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            return user.SendEmailVerificationAsync(actionCodeSettings);
#else
            return user.SendEmailVerificationAsync();
#endif
        }

        /// <summary>
        /// Get the current token and info about it, Returns the current token if it has not expired.
        /// Otherwise, this will refresh the token and return a new one.
        /// Note: IdTokenResult will have only <see cref="IdTokenResult.Token"/> property populated in non-webgl platforms.
        /// <param name="user"></param>
        /// <param name="forceRefresh">Force refresh regardless of token expiration.</param>
        /// <returns>Force refresh regardless of token expiration.</returns>
        /// </summary>
        public static Task<IdTokenResult> GetIdTokenResultAsync(this FirebaseUser user, bool forceRefresh = false)
        {
            TaskCompletionSource<IdTokenResult> task = new TaskCompletionSource<IdTokenResult>();
#if !UNITY_EDITOR && UNITY_WEBGL
            user.GetIdTokenResultAsync(forceRefresh).ContinueWith(nativeTask =>
            {
                if (nativeTask.IsSuccess())
                    task.SetResult(nativeTask.Result);
                else if (nativeTask.IsCanceled)
                    task.SetCanceled();
                else
                    task.SetException(nativeTask.Exception.InnerExceptions);
            });
#else
            user.TokenAsync(forceRefresh).ContinueWith(nativeTask =>
            {
                if (nativeTask.IsSuccess())
                    task.SetResult(new IdTokenResult() { Token = nativeTask.Result });
                else if (nativeTask.IsCanceled)
                    task.SetCanceled();
                else
                    task.SetException(nativeTask.Exception);
            });
#endif
            return task.Task;
        }


        /// <summary>
        /// [Effective only in WebGL build]
        /// Sets the current auth language to the default device/browser preference.
        /// </summary>
        public static void UseDeviceLanguage(this FirebaseAuth firebaseAuth)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            firebaseAuth.UseDeviceLanguage();
#endif
        }

        /// <summary>
        /// [Effective only in WebGL build]
        /// Changes the current type of persistence on the current Auth instance for the currently saved Auth session and applies
        /// this type of persistence for future sign-in requests, including sign-in with redirect requests.
        /// This will return a task that will resolve once the state finishes copying from one type of storage to the other. 
        /// Calling a sign-in method after changing persistence will wait for that persistence change to complete before applying it on the new Auth state.
        /// This makes it easy for a user signing in to specify whether their session should be remembered or not. 
        /// It also makes it easier to never persist the Auth state for applications that are shared by other users or have sensitive data.
        /// The default for web browser apps and React Native apps is 'local' (provided the browser supports this mechanism)
        /// whereas it is 'none' for Node.js backend apps.
        /// </summary>
        /// <param name="firebaseAuth"></param>
        /// <param name="persistenceType">The type of auth persistence to use.</param>
        /// <returns></returns>
        public static Task SetPersistenceAsync_WebGL(this FirebaseAuth firebaseAuth, PersistenceType persistenceType)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            return firebaseAuth.SetPersistenceAsync(persistenceType);
#else
            var exception = PlatformHandler.GetWebGLOnlyFeatureException();
            return Task.FromException(exception);
#endif
        }

        /// <summary>
        /// [The actionCodeSettings parameter is effective only in WebGL.]
        /// Sends a password reset email to the given email address.
        /// </summary>
        /// <param name="firebaseAuth"></param>
        /// <param name="email">The email address with the password to be reset.</param>
        /// <param name="actionCodeSettings">The action code settings. If specified, the state/continue URL will be set as the "continueUrl" parameter in the password reset link. The default password reset landing page will use this to display a link to go back to the app if it is installed. If the actionCodeSettings is not specified, no URL is appended to the action URL. The state URL provided must belong to a domain that is whitelisted by the developer in the console. Otherwise an error will be thrown. Mobile app redirects will only be applicable if the developer configures and accepts the Firebase Dynamic Links terms of condition. The Android package name and iOS bundle ID will be respected only if they are configured in the same Firebase Auth project used.</param>
        /// <returns></returns>
        public static Task SendPasswordResetEmailAsync(this FirebaseAuth firebaseAuth, string email, ActionCodeSettings actionCodeSettings = null)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            return firebaseAuth.SendPasswordResetEmailAsync(email, actionCodeSettings);
#else
            return firebaseAuth.SendPasswordResetEmailAsync(email);
#endif
        }

        /// <summary>
        /// [Effective only in WebGL build]
        /// Checks a password reset code sent to the user by email or other out-of-band mechanism.
        /// Returns the user's email address if valid.
        /// </summary>
        /// <param name="firebaseAuth"></param>
        /// <param name="code">A verification code sent to the user.</param>
        /// <returns>Returns the user's email address if valid.</returns>
        public static Task<string> VerifyPasswordResetCodeAsync_WebGL(this FirebaseAuth firebaseAuth, string code)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            return firebaseAuth.VerifyPasswordResetCodeAsync(code);
#else
            var exception = PlatformHandler.GetWebGLOnlyFeatureException();
            return Task.FromException<string>(exception);
#endif
        }

        /// <summary>
        /// [Effective only in WebGL build]
        /// Asynchronously sets the provided user as currentUser on the current Auth instance. A new instance copy of the user provided will be made and set as currentUser.
        /// This will trigger <see cref="FirebaseAuth.StateChanged"/> and <see cref="FirebaseAuth.IdTokenChanged"/> listeners like any other sign in methods.
        /// The operation fails with an error if the user to be updated belongs to a different Firebase project.
        /// </summary>
        /// <param name="firebaseAuth"></param>
        /// <param name="user">The user to set as current user.</param>
        /// <returns></returns>
        public static Task UpdateCurrentUserAsync_WebGL(this FirebaseAuth firebaseAuth, FirebaseUser user)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            return firebaseAuth.UpdateCurrentUserAsync(user);
#else
            var exception = PlatformHandler.GetWebGLOnlyFeatureException();
            return Task.FromException(exception);
#endif
        }


        /// <summary>
        /// [Effective only in WebGL Build, returns false otherwise.]
        /// Checks if an incoming link is a sign-in with email link.
        /// </summary>
        /// <param name="firebaseAuth"></param>
        /// <param name="emailLink">The Email Link to Check</param>
        /// <returns></returns>
        public static bool IsSignInWithEmailLink_WebGL(this FirebaseAuth firebaseAuth, string emailLink)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            return firebaseAuth.IsSignInWithEmailLink(emailLink);
#else
            return false;
#endif
        }

        /// <summary>
        /// [Effective only in WebGL Build].
        /// Asynchronously signs in using a phone number. This method sends a code via SMS to the given phone number, a native in browser prompt to confirm the phone number is shown.
        /// After the user provides the code sent to their phone, confirmation is triggered with the verification id and code, if correct the user is logged in and this method returns the signed in user and <see cref="AdditionalUserInfo"/>
        /// For abuse prevention, this method also requires a <see cref="RecaptchaParameters"/>
        /// The Firebase Auth SDK includes a reCAPTCHA-based implementation,  <see cref="RecaptchaParameters"/>
        /// </summary>
        /// <param name="firebaseAuth"></param>
        /// <param name="phoneNumber">The user's phone number in E.164 format (e.g. +16505550101).</param>
        /// <param name="recaptchaParameters">Optional optins to control the looks of the recaptcha control that is displayed to prevent abuste, you can also leave empty, or use <see cref="RecaptchaParameters.Default"/></param>
        /// <returns></returns>
        public static Task<SignInResult> SignInWithPhoneNumberAsync_WebGL(this FirebaseAuth firebaseAuth, string phoneNumber, RecaptchaParameters recaptchaParameters = null)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            return firebaseAuth.SignInWithPhoneNumberAsync(phoneNumber, recaptchaParameters);
#else
            var exception = PlatformHandler.GetWebGLOnlyFeatureException();
            return Task.FromException<SignInResult>(exception);
#endif
        }

        /// <summary>
        /// [Effective only in WebGL Build].
        /// Sends a sign-in email link to the user with the specified email.
        /// </summary>
        /// <param name="firebaseAuth"></param>
        /// <param name="email">The email account to sign in with.</param>
        /// <param name="actionCodeSettings">The action code settings. 
        /// The action code settings which provides Firebase with instructions on how to construct the email link. 
        /// This includes the sign in completion URL or the deep link for mobile redirects, the mobile apps to use when the sign-in link is opened on an Android or iOS device. 
        /// Mobile app redirects will only be applicable if the developer configures and accepts the Firebase Dynamic Links terms of condition. 
        /// The Android package name and iOS bundle ID will be respected only if they are configured in the same Firebase Auth project used.</param>
        /// <returns></returns>
        public static Task SendSignInLinkToEmailAsync_WebGL(this FirebaseAuth firebaseAuth, string email, ActionCodeSettings actionCodeSettings = null)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            return firebaseAuth.SendSignInLinkToEmailAsync(email, actionCodeSettings);
#else
            var exception = PlatformHandler.GetWebGLOnlyFeatureException();

            return Task.FromException(exception);
#endif
        }

        /// <summary>
        /// [Effective only in WebGL Build].
        /// Asynchronously signs in using an email and sign-in email link. If no link is passed, the link is inferred from the current URL.
        /// Fails with an error if the email address is invalid or OTP in email link expires.
        /// Note: Confirm the link is a sign-in email link before calling this method
        /// </summary>
        /// <param name="firebaseAuth"></param>
        /// <param name="email">The email account to sign in with.</param>
        /// <param name="emailLink">The optional link which contains the OTP needed to complete the sign in with email link. If not specified, the current URL is used instead.</param>
        /// <returns></returns>
        public static Task<SignInResult> SignInWithEmailLinkAsync_WebGL(this FirebaseAuth firebaseAuth, string email, string emailLink = null)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            return firebaseAuth.SignInWithEmailLinkAsync(email, emailLink);
#else
            var exception = PlatformHandler.GetWebGLOnlyFeatureException();

            return Task.FromException<SignInResult>(exception);
#endif
        }

        /// <summary>
        /// [Effective only in WebGL Build].
        /// Applies a verification code sent to the user by email or other out-of-band mechanism.
        /// </summary>
        /// <param name="firebaseAuth"></param>
        /// <param name="code">A verification code sent to the user.</param>
        /// <returns></returns>
        public static Task ApplyActionCodeAsync_WebGL(this FirebaseAuth firebaseAuth, string code)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            return firebaseAuth.ApplyActionCodeAsync(code);
#else
            var exception = PlatformHandler.GetWebGLOnlyFeatureException();

            return Task.FromException(exception);
#endif
        }

        /// <summary>
        /// [Effective only in WebGL Build].
        /// Checks a verification code sent to the user by email or other out-of-band mechanism.
        /// </summary>
        /// <param name="firebaseAuth"></param>
        /// <param name="code">The action code to check.</param>
        /// <returns></returns>
        public static Task<ActionCodeInfo> CheckActionCodeAsync_WebGL(this FirebaseAuth firebaseAuth, string code)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            return firebaseAuth.CheckActionCodeAsync(code);
#else
            var exception = PlatformHandler.GetWebGLOnlyFeatureException();

            return Task.FromException<ActionCodeInfo>(exception);
#endif
        }

        /// <summary>
        /// [Effective only in WebGL Build].
        /// Completes the password reset process, given a confirmation code and new password.
        /// </summary>
        /// <param name="firebaseAuth"></param>
        /// <param name="code">The confirmation code.</param>
        /// <param name="newPassword">The new passowrd.</param>
        /// <returns></returns>
        public static Task ConfirmPassowrdResetAsync_WebGL(this FirebaseAuth firebaseAuth, string code, string newPassword)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            return firebaseAuth.ConfirmPassowrdResetAsync(code, newPassword);
#else
            var exception = PlatformHandler.GetWebGLOnlyFeatureException();

            return Task.FromException(exception);
#endif
        }

        /// <summary>
        /// [Effective only in WebGL Build].
        /// Authenticates a Firebase client using a full-page redirect flow. 
        /// To handle the results and errors for this operation, refer to <see cref="GetRedirectResultAsync_WebGL"/>
        /// </summary>
        /// <param name="firebaseAuth"></param>
        /// <param name="providerID">The provider to authenticate. The provider has to be an OAuth provider. Non-OAuth providers like <see cref="EmailAuthProvider"/> will throw an error.</param>
        /// <param name="scopes">Scopes are what the provider accesses on the user account for example user_birthday user_friends etc...</param>
        /// <param name="parameters">The OAuth parameters for the provider for example Facebook some of its parameters are "auth_type" "display", for a complete list search google for ('provider name' auth provider parameters)</param>
        /// <returns></returns>
        public static Task SignInWithRedirectAsync_WebGL(this FirebaseAuth firebaseAuth, string providerID, List<string> scopes = null, Dictionary<string, string> parameters = null)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            return firebaseAuth.SignInWithRedirectAsync(providerID, scopes, parameters);
#else
            var exception = PlatformHandler.GetWebGLOnlyFeatureException();

            return Task.FromException(exception);
#endif
        }
        /// <summary>
        /// [Effective only in WebGL Build].
        /// When you use a redirect auth flow, you leave the unity app/game page and directed to an auth page, depends on the provider, when you're redirected back you can use this method to fetch the sign in/sign up result from redirecting.
        /// Returns a <see cref="SignInResult"/> from the redirect-based sign-in flow.
        /// If sign-in succeeded, returns the signed in user.
        /// If sign-in was unsuccessful, fails with an error.
        /// If no redirect operation was called, returns a <see cref="SignInResult"/> with a null User.
        /// </summary>
        /// <returns></returns>
        public static Task<SignInResult> GetRedirectResultAsync_WebGL(this FirebaseAuth firebaseAuth)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            return firebaseAuth.GetRedirectResultAsync();
#else
            var exception = PlatformHandler.GetWebGLOnlyFeatureException();

            return Task.FromException<SignInResult>(exception);
#endif
        }

        /// <summary>
        /// [Effective only in WebGL Build].
        /// Authenticates a Firebase client using a popup-based OAuth authentication flow.
        /// If succeeds, returns the signed in user along with the provider's credential. 
        /// If sign in was unsuccessful, returns an error object containing additional information about the error.
        /// </summary>
        /// <param name="firebaseAuth"></param>
        /// <param name="providerID">The provider to authenticate. The provider has to be an OAuth provider. Non-OAuth providers like <see cref="EmailAuthProvider"/> will throw an error.</param>
        /// <param name="scopes">Scopes are what the provider accesses on the user account for example user_birthday user_friends etc...</param>
        /// <param name="parameters">The OAuth parameters for the provider for example Facebook some of its parameters are "auth_type" "display", for a complete list search google for ('provider name' auth provider parameters)</param>
        /// <returns></returns>
        public static Task<SignInResult> SignInWithPopupAsync_WebGL(this FirebaseAuth firebaseAuth, string providerID, List<string> scopes = null,
            Dictionary<string, string> parameters = null)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            return firebaseAuth.SignInWithPopupAsync(providerID, scopes, parameters);
#else
            var exception = PlatformHandler.GetWebGLOnlyFeatureException();

            return Task.FromException<SignInResult>(exception);
#endif
        }

        /// <summary>
        /// [Effective only in WebGL Build].
        /// Reauthenticates the current user with the specified OAuth provider using a full-page redirect flow.
        /// This will make you leave current page the app/game is running on and redirect you back to it when auth successful, so your app/game will have to reload again.
        /// To get information about the sign in process from redirecting when you're back to unity app/gamep page call <see cref="GetRedirectResultAsync_WebGL"/>
        /// Since unity's app takes seconds to load on web, it is not recommended to sign user with redirect flow, better user <see cref="ReauthenticateWithPopupAsync_WebGL(FirebaseUser, string, List{string}, Dictionary{string, string})"/>
        /// </summary>
        /// <param name="user"></param>
        /// <param name="providerID">The provider to authenticate. The provider has to be an OAuth provider. Non-OAuth providers like <see cref="EmailAuthProvider"/> will throw an error.</param>
        /// <param name="scopes">Scopes are what the provider accesses on the user account for example user_birthday user_friends etc...</param>
        /// <param name="parameters">The OAuth parameters for the provider for example Facebook some of its parameters are "auth_type" "display", for a complete list search google for ('provider name' auth provider parameters)</param>
        /// <returns></returns>
        public static Task ReauthenticateWithRedirectAsync_WebGL(this FirebaseUser user, string providerID, List<string> scopes = null, Dictionary<string, string> parameters = null)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            return user.ReauthenticateWithRedirectAsync(providerID, scopes, parameters);
#else
            var exception = PlatformHandler.GetWebGLOnlyFeatureException();

            return Task.FromException(exception);
#endif
        }

        /// <summary>
        /// [Effective only in WebGL Build].
        /// Reauthenticates the current user with the specified provider using a pop-up based OAuth flow.
        /// If the reauthentication is successful, the returned result will contain the user and the provider's credential.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="providerID">The provider to authenticate. The provider has to be an OAuth provider. Non-OAuth providers like <see cref="EmailAuthProvider"/> will throw an error.</param>
        /// <param name="scopes">Scopes are what the provider accesses on the user account for example user_birthday user_friends etc...</param>
        /// <param name="parameters">The OAuth parameters for the provider for example Facebook some of its parameters are "auth_type" "display", for a complete list search google for ('provider name' auth provider parameters)</param>
        /// <returns></returns>
        public static Task<SignInResult> ReauthenticateWithPopupAsync_WebGL(this FirebaseUser user, string providerID, List<string> scopes = null, Dictionary<string, string> parameters = null)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            return user.ReauthenticateWithPopupAsync(providerID, scopes, parameters);
#else
            var exception = PlatformHandler.GetWebGLOnlyFeatureException();

            return Task.FromException<SignInResult>(exception);
#endif
        }

        /// <summary>
        /// [Effective only in WebGL Build].
        /// Re-authenticates a user using a fresh credential. 
        /// Use before operations such as <see cref="FirebaseUser.UpdatePasswordAsync"/> that require tokens from recent sign-in attempts.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="phoneNumber">The user's phone number in E.164 format (e.g. +16505550101).</param>
        /// <param name="recaptchaParameters">Optional optins to control the looks of the recaptcha control that is displayed to prevent abuste, you can also leave empty, or use <see cref="RecaptchaParameters.Default"/></param>
        /// <returns></returns>
        public static Task<SignInResult> ReauthenticateWithPhoneNumberAsync_WebGL(this FirebaseUser user, string phoneNumber, RecaptchaParameters recaptchaParameters = null)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            return user.ReauthenticateWithPhoneNumberAsync(phoneNumber, recaptchaParameters);
#else
            var exception = PlatformHandler.GetWebGLOnlyFeatureException();

            return Task.FromException<SignInResult>(exception);
#endif
        }

        /// <summary>
        /// [Effective only in WebGL Build].
        /// Links the authenticated provider to the user account using a full-page redirect flow.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="providerID">The provider to authenticate. The provider has to be an OAuth provider. Non-OAuth providers like <see cref="EmailAuthProvider"/> will throw an error.</param>
        /// <param name="scopes">Scopes are what the provider accesses on the user account for example user_birthday user_friends etc...</param>
        /// <param name="parameters">The OAuth parameters for the provider for example Facebook some of its parameters are "auth_type" "display", for a complete list search google for ('provider name' auth provider parameters)</param>
        /// <returns></returns>
        public static Task LinkWithRedirectAsync_WebGL(this FirebaseUser user, string providerID, List<string> scopes = null, Dictionary<string, string> parameters = null)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            return user.LinkWithRedirectAsync(providerID, scopes, parameters);
#else
            var exception = PlatformHandler.GetWebGLOnlyFeatureException();

            return Task.FromException(exception);
#endif
        }

        /// <summary>
        /// [Effective only in WebGL Build].
        /// Links the authenticated provider to the user account using a pop-up based OAuth flow.
        /// If the linking is successful, the returned result will contain the user and the provider's credential.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="providerID">The provider to authenticate. The provider has to be an OAuth provider. Non-OAuth providers like <see cref="EmailAuthProvider"/> will throw an error.</param>
        /// <param name="scopes">Scopes are what the provider accesses on the user account for example user_birthday user_friends etc...</param>
        /// <param name="parameters">The OAuth parameters for the provider for example Facebook some of its parameters are "auth_type" "display", for a complete list search google for ('provider name' auth provider parameters)</param>
        /// <returns></returns>
        public static Task<SignInResult> LinkWithPopupAsync_WebGL(this FirebaseUser user, string providerID, List<string> scopes = null, Dictionary<string, string> parameters = null)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            return user.LinkWithPopupAsync(providerID, scopes, parameters);
#else
            var exception = PlatformHandler.GetWebGLOnlyFeatureException();
            return Task.FromException<SignInResult>(exception);
#endif
        }

        /// <summary>
        /// [Effective only in WebGL Build].
        /// Links the user account with the given phone number.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="phoneNumber">The user's phone number in E.164 format (e.g. +16505550101).</param>
        /// <param name="recaptchaParameters">Optional optins to control the looks of the recaptcha control that is displayed to prevent abuste, you can also leave empty, or use <see cref="RecaptchaParameters.Default"/></param>
        /// <returns></returns>
        public static Task<SignInResult> LinkWithPhoneNumberAsync_WebGL(this FirebaseUser user, string phoneNumber, RecaptchaParameters recaptchaParameters = null)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            return user.LinkWithPhoneNumberAsync(phoneNumber, recaptchaParameters);
#else
            var exception = PlatformHandler.GetWebGLOnlyFeatureException();
            return Task.FromException<SignInResult>(exception);
#endif
        }

    }
}