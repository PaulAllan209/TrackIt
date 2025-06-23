using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TrackIt.Server.Dto.TrackIt;
using Xunit.Abstractions;

namespace TrackIt.Api.IntegrationTests;

public class ShipmentControllerTests
{
    private readonly ITestOutputHelper _output;
    private static readonly IConfiguration Configuration = new ConfigurationBuilder()
        .AddUserSecrets<TrackItWebApplicationFactory>()
        .Build();

    public ShipmentControllerTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task CreateShipment_ReturnsCreatedResult_WhenValid()
    {
        // Arrange
        var application = new TrackItWebApplicationFactory();
        var client = application.CreateClient();

        var username = Configuration["SupplierAccounts:SupplierAlphaUserName"];
        var password = Configuration["SupplierAccounts:SupplierAlphaPassword"];

        var token = await AuthHelper.GetAccessTokenAsync(client, username, password);
        Assert.False(string.IsNullOrWhiteSpace(token), "Failed to acquire access token.");

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var recipientUserName = Configuration["CustomerAccounts:CustomerOneUserName"];

        var recipientId = await UserAccountHelper.GetUserIdByUserNameAsync(client, recipientUserName, _output);
        Assert.False(string.IsNullOrEmpty(recipientId), "Failed to acquire recipient user ID.");

        var shipment = new ShipmentForCreationDto
        {
            Title = "Test Shipment",
            RecipientName = recipientUserName,
            RecipientAddress = "123 Test St",
            RecipientId = recipientId
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/Shipment", shipment);

        // Assert
        var createdShipment = await response.Content.ReadFromJsonAsync<ShipmentDto>();
        response.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(createdShipment);
        Assert.Equal(shipment.Title, createdShipment.title);
        Assert.Equal(shipment.RecipientId, createdShipment.RecipientId);
    }
}
