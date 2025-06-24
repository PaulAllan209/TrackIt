using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace TrackIt.Api.IntegrationTests
{
    public static class UserAccountHelper
    {
        public static async Task<string?> GetUserIdByUserNameAsync(HttpClient client, string username, ITestOutputHelper _output)
        {
            var response = await client.GetAsync($"/api/account/users/username/{username}");

            var rawContent = await response.Content.ReadAsStringAsync();
            //_output.WriteLine("Get user id response: " + rawContent);

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
            return json.GetProperty("id").GetString();
        }
    }
}
