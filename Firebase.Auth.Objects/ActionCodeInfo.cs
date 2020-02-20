using Newtonsoft.Json;
namespace Firebase.Auth
{
    /// <summary>
    /// A response from FirebaseAuth.CheckActionCode
    /// </summary>
    public class ActionCodeInfo
    {
        [JsonProperty("operation")]
        internal string OperationString { get; set; }

        /// <summary>
        /// Action code data.
        /// </summary>
        [JsonProperty("data")]
        public ActionData Data { get; set; }


        /// <summary>
        /// The type of operation that generated the action code.
        /// </summary>
        [JsonIgnore]
        public Type Operation
        {
            get
            {
                switch (OperationString)
                {
                    case PasswordResetAction:
                        return Type.PasswordReset;
                    case EmailSignInAction:
                        return Type.EmailSignInAction;
                    case RecoverEmailAction:
                        return Type.RecoverEmail;
                    case VerifyEmailAction:
                        return Type.VerifyEmail;
                    default:
                        return Type.Unknown;
                }
            }
            set
            {
                switch (value)
                {
                    case Type.PasswordReset:
                        OperationString = PasswordResetAction;
                        break;
                    case Type.VerifyEmail:
                        OperationString = VerifyEmailAction;
                        break;
                    case Type.RecoverEmail:
                        OperationString = RecoverEmailAction;
                        break;
                    case Type.EmailSignInAction:
                        OperationString = EmailSignInAction;
                        break;
                    default:
                        OperationString = null;
                        break;
                }
            }
        }
        /// <summary>
        /// The data associated with the action code.
        /// For the `PASSWORD_RESET`, `VERIFY_EMAIL`, and `RECOVER_EMAIL` actions, this object
        /// contains an `email` field with the address the email was sent to.
        /// For the `RECOVER_EMAIL` action, which allows a user to undo an email address
        /// change, this object also contains a `fromEmail` field with the user account's
        /// new email address. After the action completes, the user's email address will
        /// revert to the value in the `email` field from the value in `fromEmail` field.
        /// </summary>
        public class ActionData
        {
            /// <summary>
            /// The email address that received the action email.
            /// </summary>
            [JsonProperty("email")]
            public string Email { get; set; }
            /// <summary>
            /// The email address that sent the email.
            /// </summary>
            [JsonProperty("fromEmail")]
            public string FromEmail { get; set; }
        }

        internal const string PasswordResetAction = "PASSWORD_RESET";
        internal const string VerifyEmailAction = "VERIFY_EMAIL";
        internal const string RecoverEmailAction = "RECOVER_EMAIL";
        internal const string EmailSignInAction = "EMAIL_SIGNIN";

        /// <summary>
        /// An enumeration of the possible email action types.
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// An undefined action.
            /// </summary>
            Unknown,
            /// <summary>
            /// The password reset action.
            /// </summary>
            PasswordReset,
            /// <summary>
            /// The email verification action.
            /// </summary>
            VerifyEmail,
            /// <summary>
            /// The email revocation action.
            /// </summary>
            RecoverEmail,
            /// <summary>
            /// The email link sign-in action.
            /// </summary>
            EmailSignInAction,
        }
    }
}
