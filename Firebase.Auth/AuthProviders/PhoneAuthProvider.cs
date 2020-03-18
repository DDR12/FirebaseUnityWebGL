using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Firebase.Auth
{
    /// <summary>
    /// Use phone number text messages to authenticate.
    /// Allows developers to use the phone number and SMS verification codes to authenticate a user.
    /// </summary>
    public sealed class PhoneAuthProvider
    {
        static IDictionary<int, PhoneAuthCallbacksDataStructure> PendingAuthCallbacks = null;
        /// <summary>
        /// 
        /// </summary>
        public static string PROVIDER_ID => AuthPInvoke.PhoneProviderGetProviderID_WebGL();
        /// <summary>
        /// This corresponds to the sign-in method identifier as returned in firebase.auth.Auth.fetchSignInMethodsForEmail.
        /// </summary>
        public static string PHONE_SIGN_IN_METHOD => AuthPInvoke.PhoneProviderGetSignInMethod_WebGL();

        /// <summary>
        /// Callback used when phone number auto-verification succeeded.
        /// </summary>
        /// <param name="credential"></param>
        public delegate void VerificationCompleted(Credential credential);
        /// <summary>
        /// Callback used when phone number verification fails.
        /// </summary>
        /// <param name="error"></param>
        public delegate void VerificationFailed(string error);
        /// <summary>
        /// Callback used when a timeout occurs.
        /// </summary>
        /// <param name="verificationId"></param>
        public delegate void CodeAutoRetrievalTimeOut(string verificationId);
        /// <summary>
        /// Callback used when a verification code is sent to the given number.
        /// </summary>
        /// <param name="verificationId"></param>
        /// <param name="forceResendingToken"></param>
        public delegate void CodeSent(string verificationId, ForceResendingToken forceResendingToken);
        private class PhoneAuthCallbacksDataStructure
        {
            public VerificationCompleted verificationCompleted;
            public VerificationFailed verificationFailed;
            public CodeSent codeSent;
            public CodeAutoRetrievalTimeOut timeOut;
            public PhoneAuthProvider provider;
            public ForceResendingToken resendingToken;
            public PhoneAuthCallbacksDataStructure(PhoneAuthProvider provider)
            {
                this.provider = provider;
            }
        }
        private class PhoneVerifyResult
        {
            [JsonProperty("verificationId")]
            public string VerificationID { get; set; }
            [JsonProperty("verificationCode")]
            public string VerificationCode { get; set; }
        }
        /// <summary>
        /// 
        /// </summary>
        public PhoneAuthProvider() { }
        internal PhoneAuthProvider(FirebaseAuth firebaseAuth) : this()
        {

        }
        static PhoneAuthProvider()
        {
            PendingAuthCallbacks = new Dictionary<int, PhoneAuthCallbacksDataStructure>();
        }
        /// <summary>
        /// Start the phone number authentication operation.
        /// </summary>
        /// <param name="phoneNumber">The phone number identifier supplied by the user. Its format is normalized on the server, so it can be in any format here.</param>
        /// <param name="autoVerifyTimeOutMs">The time out for SMS auto retrieval, in miliseconds. Currently SMS auto retrieval is only supported on Android. If 0, do not do SMS auto retrieval. If positive, try to auto-retrieve the SMS verification code. If larger than MaxTimeoutMs, clamped to MaxTimeoutMs. When the time out is exceeded, codeAutoRetrievalTimeOut is called.</param>
        /// <param name="forceResendingToken">If NULL, assume this is a new phone number to verify. If not-NULL, bypass the verification session deduping and force resending a new SMS. This token is received by the CodeSent callback. This should only be used when the user presses a Resend SMS button.</param>
        /// <param name="verificationCompleted">Phone number auto-verification succeeded. Called when auto-sms-retrieval or instant validation succeeds. Provided with the completed credential.</param>
        /// <param name="verificationFailed">Phone number verification failed with an error. For example, quota exceeded or unknown phone number format. Provided with a description of the error.</param>
        public void VerifyPhoneNumber(string phoneNumber, uint autoVerifyTimeOutMs, ForceResendingToken forceResendingToken, VerificationCompleted verificationCompleted, PhoneAuthProvider.VerificationFailed verificationFailed)
        {
            this.VerifyPhoneNumber(phoneNumber, autoVerifyTimeOutMs, forceResendingToken, verificationCompleted, verificationFailed, null, null);
        }
        /// <summary>
        /// Start the phone number authentication operation.
        /// </summary>
        /// <param name="phoneNumber">The phone number identifier supplied by the user. Its format is normalized on the server, so it can be in any format here.</param>
        /// <param name="autoVerifyTimeOutMs">The time out for SMS auto retrieval, in miliseconds. Currently SMS auto retrieval is only supported on Android. If 0, do not do SMS auto retrieval. If positive, try to auto-retrieve the SMS verification code. If larger than MaxTimeoutMs, clamped to MaxTimeoutMs. When the time out is exceeded, codeAutoRetrievalTimeOut is called.</param>
        /// <param name="forceResendingToken">If NULL, assume this is a new phone number to verify. If not-NULL, bypass the verification session deduping and force resending a new SMS. This token is received by the CodeSent callback. This should only be used when the user presses a Resend SMS button.</param>
        /// <param name="verificationCompleted">Phone number auto-verification succeeded. Called when auto-sms-retrieval or instant validation succeeds. Provided with the completed credential.</param>
        /// <param name="verificationFailed">Phone number verification failed with an error. For example, quota exceeded or unknown phone number format. Provided with a description of the error.</param>
        /// <param name="codeSent">SMS message with verification code sent to phone number. Provided with the verification id to pass along to GetCredential along with the sent code, and a token to use if the user requests another SMS message be sent.</param>
        public void VerifyPhoneNumber(string phoneNumber, uint autoVerifyTimeOutMs, ForceResendingToken forceResendingToken, VerificationCompleted verificationCompleted, PhoneAuthProvider.VerificationFailed verificationFailed, PhoneAuthProvider.CodeSent codeSent)
        {
            this.VerifyPhoneNumber(phoneNumber, autoVerifyTimeOutMs, forceResendingToken, verificationCompleted, verificationFailed, codeSent, null);
        }
        /// <summary>
        /// Start the phone number authentication operation.
        /// </summary>
        /// <param name="phoneNumber">The phone number identifier supplied by the user. Its format is normalized on the server, so it can be in any format here.</param>
        /// <param name="autoVerifyTimeOutMs">The time out for SMS auto retrieval, in miliseconds. Currently SMS auto retrieval is only supported on Android. If 0, do not do SMS auto retrieval. If positive, try to auto-retrieve the SMS verification code. If larger than MaxTimeoutMs, clamped to MaxTimeoutMs. When the time out is exceeded, codeAutoRetrievalTimeOut is called.</param>
        /// <param name="forceResendingToken">If NULL, assume this is a new phone number to verify. If not-NULL, bypass the verification session deduping and force resending a new SMS. This token is received by the CodeSent callback. This should only be used when the user presses a Resend SMS button.</param>
        /// <param name="verificationCompleted">Phone number auto-verification succeeded. Called when auto-sms-retrieval or instant validation succeeds. Provided with the completed credential.</param>
        /// <param name="verificationFailed">Phone number verification failed with an error. For example, quota exceeded or unknown phone number format. Provided with a description of the error.</param>
        /// <param name="codeAutoRetrievalTimeOut">The timeout specified has expired. Provided with the verification id for the transaction that timed out.</param>
        public void VerifyPhoneNumber(string phoneNumber, uint autoVerifyTimeOutMs, ForceResendingToken forceResendingToken, VerificationCompleted verificationCompleted, PhoneAuthProvider.VerificationFailed verificationFailed, PhoneAuthProvider.CodeAutoRetrievalTimeOut codeAutoRetrievalTimeOut)
        {
            this.VerifyPhoneNumber(phoneNumber, autoVerifyTimeOutMs, forceResendingToken, verificationCompleted, verificationFailed, null, codeAutoRetrievalTimeOut);
        }
        /// <summary>
        /// Start the phone number authentication operation.
        /// </summary>
        /// <param name="phoneNumber">The phone number identifier supplied by the user. Its format is normalized on the server, so it can be in any format here.</param>
        /// <param name="autoVerifyTimeOutMs">The time out for SMS auto retrieval, in miliseconds. Currently SMS auto retrieval is only supported on Android. If 0, do not do SMS auto retrieval. If positive, try to auto-retrieve the SMS verification code. If larger than MaxTimeoutMs, clamped to MaxTimeoutMs. When the time out is exceeded, codeAutoRetrievalTimeOut is called.</param>
        /// <param name="forceResendingToken">If NULL, assume this is a new phone number to verify. If not-NULL, bypass the verification session deduping and force resending a new SMS. This token is received by the CodeSent callback. This should only be used when the user presses a Resend SMS button.</param>
        /// <param name="verificationCompleted">Phone number auto-verification succeeded. Called when auto-sms-retrieval or instant validation succeeds. Provided with the completed credential.</param>
        /// <param name="verificationFailed">Phone number verification failed with an error. For example, quota exceeded or unknown phone number format. Provided with a description of the error.</param>
        /// <param name="codeSent">SMS message with verification code sent to phone number. Provided with the verification id to pass along to GetCredential along with the sent code, and a token to use if the user requests another SMS message be sent.</param>
        /// <param name="codeAutoRetrievalTimeOut">The timeout specified has expired. Provided with the verification id for the transaction that timed out.</param>
        public void VerifyPhoneNumber(string phoneNumber, uint autoVerifyTimeOutMs, ForceResendingToken forceResendingToken, VerificationCompleted verificationCompleted, PhoneAuthProvider.VerificationFailed verificationFailed, PhoneAuthProvider.CodeSent codeSent, PhoneAuthProvider.CodeAutoRetrievalTimeOut codeAutoRetrievalTimeOut)
        {
            PreconditionUtilities.CheckNotNullOrEmpty(phoneNumber, nameof(phoneNumber));

            PhoneAuthCallbacksDataStructure phoneAuthCallbacksData = new PhoneAuthCallbacksDataStructure(this)
            {
                verificationCompleted = verificationCompleted,
                verificationFailed = verificationFailed,
                codeSent = codeSent,
                timeOut = codeAutoRetrievalTimeOut,
                resendingToken = forceResendingToken,
            };
            var task = WebGLTaskManager.GetTask<PhoneVerifyResult>();
            PendingAuthCallbacks.Add(task.ID, phoneAuthCallbacksData);
            var recaptchaJson = RecaptchaParameters.ToJson(RecaptchaParameters.Default);
            AuthPInvoke.PhoneAuthProviderVerifyPhoneNumber_WebGL(task.ID, phoneNumber, recaptchaJson, WebGLTaskManager.DequeueTask);
            task.Promise.Task.ContinueWith(result =>
            {
                ExecuteCallbacks(result, task.ID);
            });
        }

        private void ExecuteCallbacks(Task<PhoneVerifyResult> result, int id)
        {
            if (PendingAuthCallbacks.TryGetValue(id, out PhoneAuthCallbacksDataStructure phoneAuthCallbacksData))
            {
                if (result.IsSuccess())
                {
                    if (result.Result == null || string.IsNullOrEmpty(result.Result.VerificationID))
                    {
                        phoneAuthCallbacksData.verificationFailed?.Invoke("Verification failed");
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(result.Result.VerificationCode))
                            phoneAuthCallbacksData.verificationCompleted?.Invoke(GetCredential(result.Result.VerificationID, result.Result.VerificationCode));
                        else
                            phoneAuthCallbacksData.verificationFailed?.Invoke("Verification code is wrong or missing.");

                        phoneAuthCallbacksData.codeSent?.Invoke(result.Result.VerificationID, phoneAuthCallbacksData.resendingToken);
                    }
                }
                else if (result.IsCanceled)
                {
                    phoneAuthCallbacksData.verificationFailed?.Invoke("Verification is cancelled.");
                }
                else
                {
                    phoneAuthCallbacksData.verificationFailed?.Invoke(result.Exception.InnerException.Message);
                }

                PendingAuthCallbacks.Remove(id);
            }
        }

        /// <summary>
        /// [All this shit above could be replaced with this beautiful method, but it is WebGL only and we need to match unity's other sdks signature :( ]
        /// Starts a phone number authentication flow by sending a verification code to the given phone number.
        /// Returns an ID that can be passed to <see cref="GetCredential"/> to identify this flow.
        /// For abuse prevention, this method also requires a firebase.auth.ApplicationVerifier.
        /// The Firebase Auth SDK includes a reCAPTCHA-based implementation, <see cref="RecaptchaParameters"/>.
        /// </summary>
        /// <param name="phoneNumber">The user's phone number in E.164 format (e.g. +16505550101).</param>
        /// <param name="recaptchaParameters">Optional parameters to edit the looks of the recaptcha control.</param>
        /// <returns>A task for the verification ID.</returns>
        public Task<string> VerifyPhoneNumberAsync(string phoneNumber, RecaptchaParameters recaptchaParameters = null)
        {
            var task = WebGLTaskManager.GetTask<string>();
            AuthPInvoke.PhoneAuthProviderVerifyPhoneNumber_WebGL(task.ID, phoneNumber, RecaptchaParameters.ToJson(recaptchaParameters),
                WebGLTaskManager.DequeueTask);
            return task.Promise.Task;
        }
        /// <summary>
        /// Creates a phone auth credential, given the verification ID from <see cref="VerifyPhoneNumberAsync"/> and the code that was sent to the user's mobile device.
        /// </summary>
        /// <param name="verificationID">The verification ID returned from <see cref="VerifyPhoneNumberAsync"/>.</param>
        /// <param name="verificationCode">The verification code sent to the user's mobile device.</param>
        /// <returns>New <see cref="Credential"/>.</returns>
        public static Credential GetCredential(string verificationID, string verificationCode)
        {
            return Credential.FromJson(AuthPInvoke.PhoneProviderGetCredential_WebGL(verificationID, verificationCode));
        }
        /// <summary>
        /// Return the <see cref="PhoneAuthProvider"/> for the specified auth.
        /// </summary>
        /// <param name="auth">The Auth session for which we want to get a <see cref="PhoneAuthProvider"/>.</param>
        /// <returns>a <see cref="PhoneAuthProvider"/> for the given auth object.</returns>
        public static PhoneAuthProvider GetInstance(FirebaseAuth auth)
        {
            return new PhoneAuthProvider();
        }
    }
}