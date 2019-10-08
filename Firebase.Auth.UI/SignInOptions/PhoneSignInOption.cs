using Newtonsoft.Json;

namespace Firebase.Auth.UI
{
    /// <summary>
    /// Configue additional options for the sign in with Phone provider on the Auth UI.
    /// </summary>
    public sealed class PhoneSignInOption : SignInOption
    {
        /// <summary>
        /// Optionally blacklist certain contries from logging in with phone, this is the contries codes.
        /// </summary>
        [JsonProperty("blacklistedCountries", NullValueHandling = NullValueHandling.Ignore)]
        public string[] BlacklistedCountries { get; set; }
        /// <summary>
        /// The default selected country code when sign in with phone ui is rendered.
        /// </summary>
        [JsonProperty("defaultCountry", NullValueHandling = NullValueHandling.Ignore)]
        public string DefaultCountry { get; set; }
        /// <summary>
        /// The default national number when the sign in with phone ui is rendered.
        /// </summary>
        [JsonProperty("defaultNationalNumber", NullValueHandling = NullValueHandling.Ignore)]
        public string DefaultNationalNumber { get; set; }
        /// <summary>
        /// A login hint shown to the users when the sign in with phone ui is rendered.
        /// </summary>
        [JsonProperty("loginHint", NullValueHandling = NullValueHandling.Ignore)]
        public string LoginHint { get; set; }
        /// <summary>
        /// Optional parameters to control the recaptcha control appearance when the user submits their phone number, this is to prevent abuse, you can leave null or use <see cref="RecaptchaParameters.Default"/>
        /// </summary>
        [JsonProperty("recaptchaParameters", NullValueHandling = NullValueHandling.Ignore)]
        public RecaptchaParameters RecaptchaParameters { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("whitelistedCountries", NullValueHandling = NullValueHandling.Ignore)]
        public string[] WhitelistedCountries { get; set; }
    }
}
