using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json;

namespace TrackIt.Api.IntegrationTests;

public static class AuthHelper
{
    public static async Task<string> GetAccessTokenAsync(HttpClient client, string username, string password)
    {
        var form = new Dictionary<string, string>
        {
            ["username"] = username,
            ["password"] = password,
            ["grant_type"] = "password",
            ["client_id"] = "salad_spa",
            ["scope"] = "openid email phone profile offline_access roles"
        };

        var response = await client.PostAsync("/connect/token", new FormUrlEncodedContent(form));
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        return payload.GetProperty("access_token").GetString();
    }
}
