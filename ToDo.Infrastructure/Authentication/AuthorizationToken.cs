using System.Text.Json.Serialization;

namespace ToDo.Infrastructure.Authentication;

public sealed class AuthorizationToken
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = string.Empty;
}
