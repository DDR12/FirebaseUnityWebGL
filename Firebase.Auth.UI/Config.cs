using Newtonsoft.Json;

namespace Firebase.Auth.UI
{
    /// <summary>
    /// Contains optional and mandtory options to provide for firebaseui library to control the looks and behaviour of the shown ui instance.
    /// </summary>
    public sealed class Config
    {
        /// <summary>
        /// Whether to automatically upgrade existing anonymous users on sign-in/sign-up. See Upgrading anonymous users https://github.com/firebase/firebaseui-web#upgrading-anonymous-users
        /// Default: false When set to true, signInFailure callback is required to be provided to handle merge conflicts.
        /// [Required: NO]
        /// </summary>
        [JsonProperty("autoUpgradeAnonymousUsers", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AutoUpgradeAnonymousUsers { get; set; }

        [JsonProperty("credentialHelper", NullValueHandling = NullValueHandling.Ignore)]
        internal string CredentialHelperString { get; set; } = UI.CredentialHelper.AccountChooser;

        /// <summary>
        /// The Credential Helper to use. See Credential Helper. 
        /// Defaults to <see cref="CredentialHelper.Type.AccountChooser"/>
        /// [Required: NO]
        /// </summary>
        public CredentialHelper.Type CredentialHelper
        {
            get
            {
                if (CredentialHelperString == UI.CredentialHelper.NONE)
                    return UI.CredentialHelper.Type.None;
                if (CredentialHelperString == UI.CredentialHelper.GoogleYolo)
                    return UI.CredentialHelper.Type.GoogleYolo;
                return UI.CredentialHelper.Type.AccountChooser;
            }
            set
            {
                switch (value)
                {
                    case UI.CredentialHelper.Type.None:
                        CredentialHelperString = UI.CredentialHelper.NONE;
                        break;
                    case UI.CredentialHelper.Type.GoogleYolo:
                        CredentialHelperString = UI.CredentialHelper.GoogleYolo;
                        break;
                    default:
                        CredentialHelperString = UI.CredentialHelper.AccountChooser;
                        break;
                }
            }
        }

        /// <summary>
        /// The redirect URL parameter name for the sign-in success URL. See Overwriting the sign-in success URL. 
        /// Default: "signInSuccessUrl"
        /// [Required: NO]
        /// </summary>
        [JsonProperty("queryParameterForSignInSuccessUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string QueryParameterForSignInSuccessUrl { get; set; }

        /// <summary>
        /// The redirect URL parameter name for the “mode” of the Widget. See https://github.com/firebase/firebaseui-web#firebaseui-widget-modes
        /// Default: "mode"
        /// [Required: NO]
        /// </summary>
        [JsonProperty("queryParameterForWidgetMode", NullValueHandling = NullValueHandling.Ignore)]
        public string QueryParameterForWidgetMode { get; set; }

        /// <summary>
        /// The sign-in flow to use for IDP providers: redirect or popup. 
        /// Default: <see cref="SignInFlow.Redirect"/>
        /// [Required: NO]
        /// </summary>
        [JsonProperty("signInFlow"), JsonConverter(typeof(JavaScriptToCSharpEnumConverter))]
        public SignInFlow SignInFlow { get; set; } = SignInFlow.Redirect;
        

        /// <summary>
        /// The list of providers enabled for signing into your app. 
        /// The order you specify them will be the order they are displayed on the sign-in provider selection screen.
        /// [Required: YES]
        /// </summary>
        [JsonProperty("signInOptions", NullValueHandling = NullValueHandling.Ignore)]
        public SignInOption[] SignInOptions { get; set; }


        /// <summary>
        /// If not null the user will be redirected to the 'redirect url' defaults to false, this is best because if you redirect the page will reload and so is the unity player.
        /// The URL where to redirect the user after a successful sign-in. 
        /// [Required: NO]
        /// </summary>
        [JsonProperty("signInSuccessUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string SignInSuccessUrl { get; set; }

        /// <summary>
        /// The URL of the Terms of Service page or a callback function to be invoked when Terms of Service link is clicked.
        /// [Required: YES]
        /// </summary>
        [JsonProperty("tosUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string TermsOfServiceUrl { get; set; }

        /// <summary>
        /// The URL of the Privacy Policy page or a callback function to be invoked when Privacy Policy link is clicked.
        /// [Required: YES]
        /// </summary>
        [JsonProperty("privacyPolicyUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string PrivacyPolicyUrl { get; set; }

        /// <summary>
        /// Name of the website/app/game, appears to users, if not used, the app link/name will be displayed.
        /// [Required: NO]
        /// </summary>
        [JsonProperty("siteName", NullValueHandling = NullValueHandling.Ignore)]
        public string SiteName { get; set; }

        /// <summary>
        /// [Required: NO]
        /// </summary>
        [JsonProperty("widgetUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string WidgetUrl { get; set; }

        /// <summary>
        /// Serializes the config object into JSON format to be used on web.
        /// </summary>
        /// <returns>A string representation of the JSON format of this config file.</returns>
        internal string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
