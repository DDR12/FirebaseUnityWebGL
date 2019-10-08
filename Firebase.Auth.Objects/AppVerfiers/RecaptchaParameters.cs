using Newtonsoft.Json;
namespace Firebase.Auth
{
    /// <summary>
    /// Optional parameters that control the appearnce of a repactcha verifier control.
    /// </summary>
    public sealed class RecaptchaParameters
    {
        /// <summary>
        /// A Link to a custom icon to display in the recaptcha verifier control.
        /// </summary>
        [JsonProperty("badge", NullValueHandling = NullValueHandling.Ignore)]
        public string Badge { get; set; }

        /// <summary>
        /// Size defines whether the recaptcha is visible or not to the user.
        /// </summary>
        [JsonProperty("size"), JsonConverter(typeof(JavaScriptToCSharpEnumConverter))]
        public RecaptchaVerifierVisibility Visibility { get; set; } = RecaptchaVerifierVisibility.Normal;

        /// <summary>
        /// Type of the recaptcha verifier.
        /// </summary>
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; } = "recaptcha";

        /// <summary>
        /// Returns the default recaptcha look.
        /// </summary>
        public static RecaptchaParameters Default => new RecaptchaParameters() { Visibility = RecaptchaVerifierVisibility.Normal };

        /// <summary>
        /// Converts the parameters to json format.
        /// </summary>
        /// <param name="recaptchaParameters">The parameters to serialize into json.</param>
        /// <returns>The json format or null if the passed value is null.</returns>
        public static string ToJson(RecaptchaParameters recaptchaParameters)
        {
            if (recaptchaParameters == null)
                return null;
            return JsonConvert.SerializeObject(recaptchaParameters);
        }
    }
}
