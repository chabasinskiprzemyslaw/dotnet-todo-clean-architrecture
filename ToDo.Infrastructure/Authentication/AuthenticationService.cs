using Azure;
using System.Net.Http.Json;
using ToDo.Application.Abstractions.Authentication;
using ToDo.Domain.Users;

namespace ToDo.Infrastructure.Authentication;

internal sealed class AuthenticationService : IAuthenticationService
{
    private const string PasswordCredentialType = "password";

    private readonly HttpClient _httpClient;

    public AuthenticationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> RegisterAsync(
        User user,
        string password,
        CancellationToken cancellationToken = default)
    {
        var userRepresentationModel = UserRepresentationModel.FromUser(user);

        userRepresentationModel.Credentials = new CredentialRepresentationModel[]
        {
            new()
            {
                Value = password,
                Temporary = false,
                Type = PasswordCredentialType
            }
        };

        var response = await _httpClient.PostAsJsonAsync(
            "users",
            userRepresentationModel,
            cancellationToken);

        return ExtractIdentityIdFromLocationHeader(response);
        
    }

    private static string ExtractIdentityIdFromLocationHeader(
        HttpResponseMessage httpResponseMessage)
    {
        const string usersSegmentName = "users/";

        var locationHeader = httpResponseMessage.Headers.Location?.PathAndQuery;

        if (string.IsNullOrEmpty(locationHeader))
        {
            throw new InvalidOperationException("The location header is missing.");
        }

        var userSegmentValueIndex = locationHeader.IndexOf(usersSegmentName, StringComparison.InvariantCultureIgnoreCase);

        if (userSegmentValueIndex == -1)
        {
            throw new InvalidOperationException("The location header does not contain the user segment.");
        }

        var userIdentityId = locationHeader.Substring(userSegmentValueIndex + usersSegmentName.Length);

        return userIdentityId;
    }
}
