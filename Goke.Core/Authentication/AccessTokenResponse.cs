using System.Text.Json.Serialization;

namespace Goke.Core.Authentication
{
    /// <summary>
    /// This class is used to store the token information received from the server.
    /// </summary>
    public class AccessTokenResponse
    {
        [JsonPropertyName("accessToken")]
        public required string AccessToken { get; set; }
    }

}
