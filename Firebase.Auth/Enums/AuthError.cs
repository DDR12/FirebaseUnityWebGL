namespace Firebase.Auth
{
    /// <summary>
    /// All possible error codes from asynchronous calls.
    /// </summary>
    public enum AuthError
    {
        /// <summary>
        /// Function will be implemented in a later revision of the API.
        /// </summary>
        Unimplemented = -1,
        /// <summary>
        /// Success.
        /// </summary>
        None = 0,
        /// <summary>
        /// This indicates an internal error.
        /// Common error code for all API Methods.
        /// </summary>
        Failure = 1,
        /// <summary>
        /// Indicates a validation error with the custom token.
        /// </summary>
        InvalidCustomToken = 2,
        /// <summary>
        /// Indicates the service account and the API key belong to different projects.
        /// </summary>
        CustomTokenMismatch = 3,
        /// <summary>
        /// Indicates the IDP token or requestUri is invalid.
        /// </summary>
        InvalidCredential = 4,
        /// <summary>
        /// Indicates the user’s account is disabled on the server.
        /// Caused by "Sign in with credential" methods.
        /// </summary>
        UserDisabled = 5,
        /// <summary>
        /// Indicates an account already exists with the same email address but using different sign-in credentials.
        /// Account linking is required. Caused by "Sign in with credential" methods.
        /// </summary>
        AccountExistsWithDifferentCredentials = 6,
        /// <summary>
        /// Indicates the administrator disabled sign in with the specified identity provider.
        /// </summary>
        OperationNotAllowed = 7,
        /// <summary>
        /// Indicates the email used to attempt a sign up is already in use.
        /// </summary>
        EmailAlreadyInUse = 8,
        /// <summary>
        /// Indicates the user has attemped to change email or password more than 5 minutes after signing in, and will need to refresh the credentials.
        /// Caused by "Set account info" methods.
        /// </summary>
        RequiresRecentLogin = 9,
        /// <summary>
        /// Indicates an attempt to link with a credential that has already been linked with a different Firebase account.
        /// </summary>
        CredentialAlreadyInUse = 10,
        /// <summary>
        /// Indicates an invalid email address.
        /// </summary>
        InvalidEmail = 11,
        /// <summary>
        /// Indicates the user attempted sign in with a wrong password.
        /// Caused by "Sign in with password" methods.
        /// </summary>
        WrongPassword = 12,
        /// <summary>
        /// Indicates that too many requests were made to a server method.
        /// Common error code for all API methods.
        /// </summary>
        TooManyRequests = 13,
        /// <summary>
        /// Indicates the user account was not found.
        /// Send password request email error code. Common error code for all API methods.
        /// </summary>
        UserNotFound = 14,
        /// <summary>
        /// Indicates an attempt to link a provider to which the account is already linked.
        /// Caused by "Link credential" methods.
        /// </summary>
        ProviderAlreadyLinked  = 15,
        /// <summary>
        /// Indicates an attempt to unlink a provider that is not linked.
        /// </summary>
        NoSuchProvider = 16,
        /// <summary>
        /// Indicates user's saved auth credential is invalid, the user needs to sign in again.
        /// Caused by requests with an STS id token.
        /// </summary>
        InvalidUserToken = 17,
        /// <summary>
        /// Indicates the saved token has expired.
        /// For example, the user may have changed account password on another device.
        /// The user needs to sign in again on the device that made this request. Caused by requests with an STS id token.
        /// </summary>
        UserTokenExpired = 18,
        /// <summary>
        /// Indicates a network error occurred (such as a timeout, interrupted connection, or unreachable host).
        /// These types of errors are often recoverable with a retry. Common error code for all API Methods.
        /// </summary>
        NetworkRequestFailed = 19,
        /// <summary>
        /// Indicates an invalid API key was supplied in the request.
        /// For Android these should no longer occur (as of 2016 v3). Common error code for all API Methods.
        /// </summary>
        InvalidApiKey = 20,
        /// <summary>
        /// Indicates the App is not authorized to use Firebase Authentication with the provided API Key.
        /// </summary>
        AppNotAuthorized = 21,
        /// <summary>
        /// Indicates that an attempt was made to reauthenticate with a user which is not the current user.
        /// </summary>
        UserMismatch = 22,
        /// <summary>
        /// Indicates an attempt to set a password that is considered too weak.
        /// </summary>
        WeakPassword = 23,
        /// <summary>
        /// Internal api usage error code when there is no signed-in user and getAccessToken is called.
        /// </summary>
        NoSignedInUser = 24,
        /// <summary>
        /// This can happen when certain methods on App are performed, when the auth API is not loaded.
        /// </summary>
        ApiNotAvailable = 25,
        /// <summary>
        /// Indicates the out-of-band authentication code is expired.
        /// </summary>
        ExpiredActionCode = 26,
        /// <summary>
        /// Indicates the out-of-band authentication code is invalid.
        /// </summary>
        InvalidActionCode = 27,
        /// <summary>
        /// Indicates that there are invalid parameters in the payload during a "send password reset email" attempt.
        /// </summary>
        InvalidMessagePayload = 28,
        /// <summary>
        /// Indicates that an invalid phone number was provided.
        /// This is caused when the user is entering a phone number for verification.
        /// </summary>
        InvalidPhoneNumber = 29,
        /// <summary>
        /// Indicates that a phone number was not provided during phone number verification.
        /// </summary>
        MissingPhoneNumber = 30,
        /// <summary>
        /// Indicates that the recipient email is invalid.
        /// </summary>
        InvalidRecipientEmail = 31,
        /// <summary>
        /// Indicates that the sender email is invalid during a "send password reset email" attempt.
        /// </summary>
        InvalidSender = 32,
        /// <summary>
        /// Indicates that an invalid verification code was used in the verifyPhoneNumber request.
        /// </summary>
        InvalidVerificationCode = 33,
        /// <summary>
        /// Indicates that an invalid verification ID was used in the verifyPhoneNumber request.
        /// </summary>
        InvalidVerificationId = 34,
        /// <summary>
        /// Indicates that the phone auth credential was created with an empty verification code.
        /// </summary>
        MissingVerificationCode = 35,
        /// <summary>
        /// Indicates that the phone auth credential was created with an empty verification ID.
        /// </summary>
        MissingVerificationId = 36,
        /// <summary>
        /// Indicates that an email address was expected but one was not provided.
        /// </summary>
        MissingEmail = 37,
        /// <summary>
        /// Represents the error code for when an application attempts to create an email/password account with an empty/null password field.
        /// </summary>
        MissingPassword = 38,
        /// <summary>
        /// Indicates that the quota of SMS messages for a given project has been exceeded.
        /// </summary>
        QuotaExceeded = 39,
        /// <summary>
        /// Thrown when one or more of the credentials passed to a method fail to identify and/or authenticate the user subject of that operation.
        /// Inspect the error message to find out the specific cause. 
        /// </summary>
        RetryPhoneAuth = 40,
        /// <summary>
        /// Indicates that the SMS code has expired.
        /// </summary>
        SessionExpired = 41,
        /// <summary>
        /// Indicates that the app could not be verified by Firebase during phone number authentication.
        /// </summary>
        AppNotVerified = 42,
        /// <summary>
        /// Indicates a general failure during the app verification flow.
        /// </summary>
        AppVerificationFailed = 43,
        /// <summary>
        /// Indicates that the reCAPTCHA token is not valid.
        /// </summary>
        CaptchaCheckFailed = 44,
        /// <summary>
        /// Indicates that an invalid APNS device token was used in the verifyClient request.
        /// </summary>
        InvalidAppCredential = 45,
        /// <summary>
        /// Indicates that the APNS device token is missing in the verifyClient request.
        /// </summary>
        MissingAppCredential = 46,
        /// <summary>
        /// Indicates that the clientID used to invoke a web flow is invalid.
        /// </summary>
        InvalidClientId = 47,
        /// <summary>
        /// Indicates that the domain specified in the continue URI is not valid.
        /// </summary>
        InvalidContinueUri = 48,
        /// <summary>
        /// Indicates that a continue URI was not provided in a request to the backend which requires one.
        /// </summary>
        MissingContinueUri = 49,
        /// <summary>
        /// Indicates an error occurred while attempting to access the keychain.
        /// Common error code for all API Methods.
        /// </summary>
        KeychainError = 50,
        /// <summary>
        /// Indicates that the APNs device token could not be obtained.
        /// The app may not have set up remote notification correctly, or may have failed to forward the APNs device token to FIRAuth if app delegate swizzling is disabled.
        /// </summary>
        MissingAppToken = 51,
        /// <summary>
        /// Indicates that the iOS bundle ID is missing when an iOS App Store ID is provided.
        /// </summary>
        MissingIosBundleId = 52,
        /// <summary>
        /// Indicates that the app fails to forward remote notification to FIRAuth.
        /// </summary>
        NotificationNotForwarded = 53,
        /// <summary>
        /// Indicates that the domain specified in the continue URL is not white- listed in the Firebase console.
        /// </summary>
        UnauthorizedDomain = 54,
        /// <summary>
        /// Indicates that an attempt was made to present a new web context while one was already being presented.
        /// </summary>
        WebContextAlreadyPresented = 55,
        /// <summary>
        /// Indicates that the URL presentation was cancelled prematurely by the user.
        /// </summary>
        WebContextCancelled = 56,
        /// <summary>
        /// Indicates that Dynamic Links in the Firebase Console is not activated.
        /// </summary>
        DynamicLinkNotActivated = 57,
        /// <summary>
        /// Indicates that the operation was cancelled.
        /// </summary>
        Cancelled = 58,
        /// <summary>
        /// Indicates that the provider id given for the web operation is invalid.
        /// </summary>
        InvalidProviderId = 59,
        /// <summary>
        /// Indicates that an internal error occurred during a web operation.
        /// </summary>
        WebInternalError = 60,
        /// <summary>
        /// Indicates that 3rd party cookies or data are disabled, or that there was a problem with the browser.
        /// </summary>
        WebStorateUnsupported = 61,
        /// <summary>
        /// Indicates that the provided tenant ID does not match the Auth instance's tenant ID.
        /// </summary>
        TenantIdMismatch = 62,
        /// <summary>
        /// ndicates that a request was made to the backend with an associated tenant ID for an operation that does not support multi-tenancy.
        /// </summary>
        UnsupportedTenantOperation = 63,
        /// <summary>
        /// Indicates that an FDL domain used for an out of band code flow is either not configured or is unauthorized for the current project.
        /// </summary>
        InvalidLinkDomain = 64,
        /// <summary>
        /// Indicates that credential related request data is invalid.
        /// This can occur when there is a project number mismatch (sessionInfo, spatula header, temporary proof), an incorrect temporary proof phone number, or during game center sign in when the user is already signed into a different game center account.
        /// </summary>
        RejectedCredential = 65,
        /// <summary>
        /// Indicates that the phone number provided in the MFA sign in flow to be verified does not correspond to a phone second factor for the user.
        /// </summary>
        PhoneNumberNotFound = 66,
        /// <summary>
        /// Indicates that a request was made to the backend with an invalid tenant ID.
        /// </summary>
        InvalidTenantId = 67,
        /// <summary>
        /// Client id is missing.
        /// </summary>
        MissingClientIdentifier = 68,
        /// <summary>
        /// 
        /// </summary>
        MissingMultiFactorSession = 69,
        /// <summary>
        /// 
        /// </summary>
        MissingMultiFactorInfo = 70,
        /// <summary>
        /// 
        /// </summary>
        InvalidMultiFactorSession = 71,
        /// <summary>
        /// 
        /// </summary>
        MultiFactorInfoNotFound = 72,
        /// <summary>
        /// 
        /// </summary>
        AdminRestrictedOperation = 73,
        /// <summary>
        /// 
        /// </summary>
        UnverifiedEmail = 74,
        /// <summary>
        /// 
        /// </summary>
        SecondFactorAlreadyEnrolled = 75,
        /// <summary>
        /// 
        /// </summary>
        MaximumSecondFactorCountExceeded = 76,
        /// <summary>
        /// 
        /// </summary>
        UnsupportedFirstFactor = 77,
        /// <summary>
        /// 
        /// </summary>
        EmailChangeNeedsVerification = 78,
    }


}
