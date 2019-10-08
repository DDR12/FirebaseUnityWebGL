using Newtonsoft.Json;
using System.Collections.Generic;

namespace Firebase.Auth
{
    internal sealed class AuthProvider
    {
        [JsonProperty("providerId")]
        public string ProviderId { get; set; }

        [JsonProperty("scopes", NullValueHandling = NullValueHandling.Ignore)]
        List<string> Scopes { get; set; }

        [JsonProperty("customParameters", NullValueHandling = NullValueHandling.Ignore)]
        Dictionary<string, string> Parameters { get; set; }

        public AuthProvider(string providerID) : this(providerID, null, null)
        {
        }

        public AuthProvider(string providerID, List<string> scopes, Dictionary<string, string> parameters)
        {
            ProviderId = providerID;
            Scopes = scopes;
            Parameters = parameters;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

    }
}
