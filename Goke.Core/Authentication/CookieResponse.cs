using System.Net;
using System.Text.Json.Serialization;

namespace Goke.Core.Authentication
{
    public class CookieResponse
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }
        [JsonPropertyName("value")]
        public required string Value { get; set; }
        [JsonPropertyName("path")]
        public required string Path { get; set; }
        [JsonPropertyName("domain")]
        public required string Domain { get; set; }
        [JsonPropertyName("expires")]
        public DateTime? Expires { get; set; }
        [JsonPropertyName("secure")]
        public bool Secure { get; set; }
        [JsonPropertyName("httpOnly")]
        public bool HttpOnly { get; set; }
    }

    public class CookieCollectionResponse
    {
        [JsonPropertyName("cookies")]
        public required List<CookieResponse> Cookies { get; set; }
    }

    // cookie container is not serializable, so we need to create a separate class to store the cookie information
    public class CookieContainerResponse
    {
        [JsonPropertyName("cookieContainer")]
        public required CookieContainer CookieContainer { get; set; }
    }

}
