using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using TrackIt.Server.Dto.TrackIt;

namespace TrackIt.Api.IntegrationTests;

public class ShipmentControllerTests
{
    private static readonly IConfiguration Configuration = new ConfigurationBuilder()
        .AddUserSecrets<TrackItWebApplicationFactory>()
        .Build();

    [Fact]
    public async Task CreateShipment_ReturnsCreatedResult_WhenValid()
    {
        // Arrange
        var application = new TrackItWebApplicationFactory();
        var client = application.CreateClient();

        var username = Configuration["SupplierAccounts:SupplierAlphaUserName"];
        var password = Configuration["SupplierAccounts:SupplierAlphaPassword"];
        var token = await AuthHelper.GetAccessTokenAsync(client, username, password);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var shipment = new ShipmentForCreationDto
        {
            Title = "Test Shipment",
            RecipientName = Configuration[]
        }

        // Act

        // Assert
    }
}
