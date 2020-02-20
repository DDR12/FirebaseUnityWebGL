using System.Runtime.InteropServices;

namespace Firebase.Auth
{
    internal class AuthPInvoke
    {
        [DllImport("__Internal")]
        public static extern string GetUserPhoneNumber_WebGL(uint id);
        [DllImport("__Internal")]
        public static extern string GetUserProviderData_WebGL(uint id);
        [DllImport("__Internal")]
        public static extern string GetUserRefreshToken_WebGL(uint id);
        [DllImport("__Internal")]
        public static extern string GetUserID_WebGL(uint id);
        [DllImport("__Internal")]
        public static extern string GetUserDisplayName_WebGL(uint id);
        [DllImport("__Internal")]
        public static extern string GetUserEmail_WebGL(uint id);
        [DllImport("__Internal")]
        public static extern string GetUserPhotoUrl_WebGL(uint id);
        [DllImport("__Internal")]
        public static extern string GetUserProviderID_WebGL(uint id);
        [DllImport("__Internal")]
        public static extern string GetUserMetadata_WebGL(uint id);
        [DllImport("__Internal")]
        public static extern void DeleteUser_WebGL(int taskID, uint userID, VoidTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void GetIdToken_WebGL(int taskID, uint userID, bool forceRefresh, GenericTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void GetIdTokenResult_WebGL(int taskID, uint userID, bool forceRefresh, GenericTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void LinkWithCredential_WebGL(int taskID, uint userID, string credentialJson, GenericTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void LinkWithPhoneNumber_WebGL(int taskID, uint userID, string phoneNumber, string recaptchaParametersJsons, GenericTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void LinkWithPopup_WebGL(int taskID, uint userID, string providerJson, GenericTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void LinkUserWithRedirect_WebGL(int taskID, uint userID, string providerJson, VoidTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void ReauthenticateWithCredential_WebGL(int taskID, uint userID, string authCredentialJson, GenericTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void ReauthenticateWithPhoneNumber_WebGL(int taskID, uint userID, string phoneNumber, string recaptchaParametersJson, GenericTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void ReauthenticateWithPopup_WebGL(int taskID, uint userID, string providerJson, GenericTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void ReauthenticateWithRedirect_WebGL(int taskID, uint userID, string providerJson, VoidTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void ReloadUser_WebGL(int taskID, uint userID, VoidTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void SendEmailVerification_WebGL(int taskID, uint userID, string actionCodeSettingsJson, VoidTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void UnlinkUser_WebGL(int taskID, uint userID, string providerID, GenericTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void UpdateEmail_WebGL(int taskID, uint userID, string newEmail, VoidTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void UpdatePassword_WebGL(int taskID, uint userID, string newPassword, VoidTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void UpdatePhoneNumber_WebGL(int taskID, uint userID, string phoneCredentialsJson, VoidTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void UpdateProfile_WebGL(int taskID, uint userID, string updateDataJson, VoidTaskWebGLDelegate callcack);
        [DllImport("__Internal")]
        public static extern bool GetIsUserAnnonymous_WebGL(uint id);
        [DllImport("__Internal")]
        public static extern bool GetIsUserEmailVerified_WebGL(uint id);
        [DllImport("__Internal")]
        public static extern string GetCurrentAuthUser_WebGL(string appName);
        [DllImport("__Internal")]
        public static extern void ReleaseUser_WebGL(uint id);
        [DllImport("__Internal")]
        public static extern void VerifyPasswordResetCode_WebGL(int taskID, string appName, string code, GenericTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void UpdateCurrentAuthUser_WebGL(int taskID, string appName, uint nativeLibUserID, VoidTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void SignOut_WebGL(int taskID, string appName, VoidTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void SignInWithRedirect_WebGL(int taskID, string appName, string providerJson, GenericTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void SignInWithPopup_WebGL(int taskID, string appName, string providerJson, GenericTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void SignInWithPhoneNumber_WebGL(int taskID, string appName, string phoneNumber, string recaptchaParameters, GenericTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void SignInWithEmailLink_WebGL(int taskID, string appName, string email, string emailLink, GenericTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void SignInWithEmailAndPassword_WebGL(int taskID, string appName, string email, string password, GenericTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void SignInWithCustomToken_WebGL(int taskID, string appName, string token, GenericTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void SignInWithCredential_WebGL(int taskID, string appName, string credentialJsonObject, GenericTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void SignInAnonymously_WebGL(int taskID, string appName, GenericTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void SetAuthPersistence_WebGL(int taskID, string appName, string persistence, VoidTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void SendSignInLinkToEmail_WebGL(int taskID, string appName, string email, string actionCodeSettingsJson, VoidTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void SendPasswordResetEmail_WebGL(int taskID, string appName, string email, string actionCodeSettingsJson, VoidTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void GetAuthRedirectResult_WebGL(int taskID, string appName, GenericTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void FetchSignInMethodsForEmail_WebGL(int taskID, string appName, string email, GenericTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void ApplyAuthActionCode_WebGL(int taskID, string appName, string code, VoidTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void CheckAuthActionCode_WebGL(int taskID, string appName, string code, GenericTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void ConfirmPasswordReset_WebGL(int taskID, string appName, string code, string newPassword, VoidTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void CreateUserWithEmailAndPassword_WebGL(int taskID, string appName, string email, string password, GenericTaskWebGLDelegate callback);
        [DllImport("__Internal")]
        public static extern void SubscribeToAuthChange_WebGL(string appName, WebGLAuthChangedCallback callback);
        [DllImport("__Internal")]
        public static extern void UnsubscribeToAuthChange_WebGL(string appName);
        [DllImport("__Internal")]
        public static extern void SubscribeToIdTokenChange_WebGL(string appName, WebGLIdTokenChangedCallback callback);

        [DllImport("__Internal")]
        public static extern void UnSubscribeToIdTokenChange_WebGL(string appName);

        [DllImport("__Internal")]
        public static extern bool IsSignedInWithEmailLink_WebGL(string appName, string emailLink);

        [DllImport("__Internal")]
        public static extern string GetLanguageCode_WebGL(string appName);

        [DllImport("__Internal")]
        public static extern void SetLanguageCode_WebGL(string appName, string languageCode);

        [DllImport("__Internal")]
        public static extern void AuthUseDeviceLanguage_WebGL(string appName);

        [DllImport("__Internal")]
        public static extern bool GetAppVerificationDisabledForTesting_WebGL(string appName);
        [DllImport("__Internal")]
        public static extern void SetAppVerificationDisabledForTesting_WebGL(string appName, bool disabled);

        [DllImport("__Internal")]
        public static extern string GetEmailProviderID_WebGL();

        [DllImport("__Internal")]
        public static extern string GetEmailCredential_WebGL(string email, string password);

        [DllImport("__Internal")]
        public static extern string GetEmailLinkSignInMethod_WebGL();

        [DllImport("__Internal")]
        public static extern string GetEmailPasswordSignInMethod_WebGL();

        [DllImport("__Internal")]
        public static extern string GetEmailLinkCredential_WebGL(string email, string emailLink);

        [DllImport("__Internal")]
        public static extern string GetFacebookCredential_WebGL(string token);

        [DllImport("__Internal")]
        public static extern string GetFacebookProviderID_WebGL();

        [DllImport("__Internal")]
        public static extern string GetFacebookSignInMethod_WebGL();

        [DllImport("__Internal")]
        public static extern string GetGithubCredential_WebGL(string token);


        [DllImport("__Internal")]
        public static extern string GetGithubProviderID_WebGL();


        [DllImport("__Internal")]
        public static extern string GetGithubSignInMethod_WebGL();

        [DllImport("__Internal")]
        public static extern string GetGoogleCredential_WebGL(string idToken, string accessToken);

        [DllImport("__Internal")]
        public static extern string GetGoogleProviderID_WebGL();

        [DllImport("__Internal")]
        public static extern string GetGoogleSignInMethod_WebGL();

        [DllImport("__Internal")]
        public static extern string GetOAuthCredential_WebGL(string providerID, string idToken, string accessToken);

        [DllImport("__Internal")]
        public static extern void PhoneAuthProviderVerifyPhoneNumber_WebGL(int taskID, string phoneNumber, string recaptchaParametersJson, GenericTaskWebGLDelegate callback);

        [DllImport("__Internal")]
        public static extern string PhoneProviderGetProviderID_WebGL();

        [DllImport("__Internal")]
        public static extern string PhoneProviderGetSignInMethod_WebGL();

        [DllImport("__Internal")]
        public static extern string PhoneProviderGetCredential_WebGL(string verificationID, string verificationCode);

        [DllImport("__Internal")]
        public static extern string GetTwitterProviderID_WebGL();

        [DllImport("__Internal")]
        public static extern string GetTwitterSignInMethod_WebGL();

        [DllImport("__Internal")]
        public static extern string TwitterProviderGetCredentials_WebGL(string token, string secret);

        [DllImport("__Internal")]
        public static extern string GetActionCodeStringFromEnumIndex_WebGL(int enumIndex);
    }
}
