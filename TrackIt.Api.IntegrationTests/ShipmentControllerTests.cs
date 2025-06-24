using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using TrackIt.Server.Dto.TrackIt;
using Xunit.Abstractions;

namespace TrackIt.Api.IntegrationTests;

public class ShipmentControllerTests
{
    private readonly ITestOutputHelper _output;
    private static readonly IConfiguration Configuration = new ConfigurationBuilder()
        .AddUserSecrets<TrackItWebApplicationFactory>()
        .Build();

    private TrackItWebApplicationFactory _application;
    private HttpClient _client;

    public ShipmentControllerTests(ITestOutputHelper output)
    {
        _output = output;

        _application = new TrackItWebApplicationFactory("ShipmentControllerTests");
        _client = _application.CreateClient();
    }

    [Fact]
    public async Task CreateShipment_ReturnsCreatedResult_WhenValid()
    {
        // Arranged
        var username = Configuration["SupplierAccounts:SupplierAlphaUserName"];
        var password = Configuration["SupplierAccounts:SupplierAlphaPassword"];

        var token = await AuthHelper.GetAccessTokenAsync(_client, username, password);
        Assert.False(string.IsNullOrWhiteSpace(token), "Failed to acquire access token.");

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var recipientUserName = Configuration["CustomerAccounts:CustomerOneUserName"];

        var recipientId = await UserAccountHelper.GetUserIdByUserNameAsync(_client, recipientUserName, _output);
        Assert.False(string.IsNullOrEmpty(recipientId), "Failed to acquire recipient user ID.");

        var shipment = new ShipmentForCreationDto
        {
            Title = "Test Shipment",
            RecipientName = recipientUserName,
            RecipientAddress = "123 Test St",
            RecipientId = recipientId
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Shipment", shipment);

        // Assert
        var createdShipment = await response.Content.ReadFromJsonAsync<ShipmentDto>();
        response.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(createdShipment);
        Assert.Equal(shipment.Title, createdShipment.title);
        Assert.Equal(shipment.RecipientId, createdShipment.RecipientId);
    }

    [Fact]
    public async Task GetAllShipments_ReturnsOkResult_WithShipments()
    {
        // Arrange
        var username = Configuration["SupplierAccounts:SupplierAlphaUserName"];
        var password = Configuration["SupplierAccounts:SupplierAlphaPassword"];

        var token = await AuthHelper.GetAccessTokenAsync(_client, username, password);
        Assert.False(string.IsNullOrWhiteSpace(token), "Failed to acquire access token.");

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.GetAsync("/api/Shipment");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var shipments = await response.Content.ReadFromJsonAsync<IEnumerable<ShipmentDto>>();
        Assert.NotNull(shipments);

        // Verify pagination header
        Assert.True(response.Headers.Contains("X-Pagination"), "Response should contain pagination header");
    }

    [Fact]
    public async Task GetShipmentById_ReturnsOkResult_WhenShipmentExists()
    {
        // Arrange
        var username = Configuration["SupplierAccounts:SupplierAlphaUserName"];
        var password = Configuration["SupplierAccounts:SupplierAlphaPassword"];

        var token = await AuthHelper.GetAccessTokenAsync(_client, username, password);
        Assert.False(string.IsNullOrWhiteSpace(token), "Failed to acquire access token.");

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // First, create a shipment to retrieve
        var recipientUserName = Configuration["CustomerAccounts:CustomerOneUserName"];
        var recipientId = await UserAccountHelper.GetUserIdByUserNameAsync(_client, recipientUserName, _output);

        var newShipment = new ShipmentForCreationDto
        {
            Title = "Shipment To Retrieve",
            RecipientName = recipientUserName,
            RecipientAddress = "456 Retrieve St",
            RecipientId = recipientId
        };

        var createResponse = await _client.PostAsJsonAsync("/api/Shipment", newShipment);
        var createdShipment = await createResponse.Content.ReadFromJsonAsync<ShipmentDto>();
        Assert.NotNull(createdShipment);

        // Act 
        var response = await _client.GetAsync($"/api/Shipment/{createdShipment.Id}");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var shipment = await response.Content.ReadFromJsonAsync<ShipmentDto>();
        Assert.NotNull(shipment);
        Assert.Equal(createdShipment.Id, shipment.Id);
        Assert.Equal(newShipment.Title, shipment.title);
    }

    [Fact]
    public async Task GetShipmentById_ReturnsNotFound_WhenShipmentDoesNotExist()
    {
        // Arrange
        var username = Configuration["SupplierAccounts:SupplierAlphaUserName"];
        var password = Configuration["SupplierAccounts:SupplierAlphaPassword"];

        var token = await AuthHelper.GetAccessTokenAsync(_client, username, password);
        Assert.False(string.IsNullOrWhiteSpace(token), "Failed to acquire access token.");

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Use a non-existent GUID
        var nonExistentId = Guid.NewGuid().ToString();

        // Act
        var response = await _client.GetAsync($"/api/Shipment/{nonExistentId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task PatchShipment_ReturnsNoContent_WhenValid()
    {
        // Arrange
        var username = Configuration["SupplierAccounts:SupplierAlphaUserName"];
        var password = Configuration["SupplierAccounts:SupplierAlphaPassword"];

        var token = await AuthHelper.GetAccessTokenAsync(_client, username, password);
        Assert.False(string.IsNullOrWhiteSpace(token), "Failed to acquire access token.");

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // First, create a shipment to update
        var recipientUserName = Configuration["CustomerAccounts:CustomerOneUserName"];
        var recipientId = await UserAccountHelper.GetUserIdByUserNameAsync(_client, recipientUserName, _output);

        var newShipment = new ShipmentForCreationDto
        {
            Title = "Shipment To Update",
            RecipientName = recipientUserName,
            RecipientAddress = "789 Update St",
            RecipientId = recipientId
        };

        var createResponse = await _client.PostAsJsonAsync("/api/Shipment", newShipment);
        var createdShipment = await createResponse.Content.ReadFromJsonAsync<ShipmentDto>();
        Assert.NotNull(createdShipment);

        // Create JSON patch document to update the title
        var patchDoc = new[]
        {
            new { op = "replace", path = "/title", value = "Updated Shipment Title" }
        };

        var jsonContent = JsonSerializer.Serialize(patchDoc);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json-patch+json");

        // Act
        var response = await _client.PatchAsync($"/api/Shipment/{createdShipment.Id}", content);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        // Verify the update by retrieving the shipment
        var getResponse = await _client.GetAsync($"/api/Shipment/{createdShipment.Id}");
        var updatedShipment = await getResponse.Content.ReadFromJsonAsync<ShipmentDto>();

        Assert.NotNull(updatedShipment);
        Assert.Equal("Updated Shipment Title", updatedShipment.title);
    }

    [Fact]
    public async Task DeleteShipment_ReturnsNoContent_WhenShipmentExists()
    {
        // Arrange
        var username = Configuration["SupplierAccounts:SupplierAlphaUserName"];
        var password = Configuration["SupplierAccounts:SupplierAlphaPassword"];

        var token = await AuthHelper.GetAccessTokenAsync(_client, username, password);
        Assert.False(string.IsNullOrWhiteSpace(token), "Failed to acquire access token.");

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // First, create a shipment to delete
        var recipientUserName = Configuration["CustomerAccounts:CustomerOneUserName"];
        var recipientId = await UserAccountHelper.GetUserIdByUserNameAsync(_client, recipientUserName, _output);

        var newShipment = new ShipmentForCreationDto
        {
            Title = "Shipment To Delete",
            RecipientName = recipientUserName,
            RecipientAddress = "321 Delete St",
            RecipientId = recipientId
        };

        var createResponse = await _client.PostAsJsonAsync("/api/Shipment", newShipment);
        var createdShipment = await createResponse.Content.ReadFromJsonAsync<ShipmentDto>();
        Assert.NotNull(createdShipment);

        // Act
        var response = await _client.DeleteAsync($"/api/Shipment/{createdShipment.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        // Verify deletion by attempting to retrieve the shipment
        var getResponse = await _client.GetAsync($"/api/Shipment/{createdShipment.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task CreateShipment_ReturnsBadRequest_WhenModelIsInvalid()
    {
        // Arrange
        var username = Configuration["SupplierAccounts:SupplierAlphaUserName"];
        var password = Configuration["SupplierAccounts:SupplierAlphaPassword"];

        var token = await AuthHelper.GetAccessTokenAsync(_client, username, password);
        Assert.False(string.IsNullOrWhiteSpace(token), "Failed to acquire access token.");

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Create invalid shipment with missing required fields
        var invalidShipment = new ShipmentForCreationDto
        {
            Title = "", // Empty title
            RecipientName = "",
            RecipientAddress = "123 Test St",
            RecipientId = "invalid-id"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Shipment", invalidShipment);

        // Assert
        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    [Fact]
    public async Task GetAllShipments_FiltersCorrectly_WhenParametersProvided()
    {
        // Arrange
        var username = Configuration["SupplierAccounts:SupplierAlphaUserName"];
        var password = Configuration["SupplierAccounts:SupplierAlphaPassword"];

        var token = await AuthHelper.GetAccessTokenAsync(_client, username, password);
        Assert.False(string.IsNullOrWhiteSpace(token), "Failed to acquire access token.");

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Create a shipment with a specific title for searching
        var recipientUserName = Configuration["CustomerAccounts:CustomerOneUserName"];
        var recipientId = await UserAccountHelper.GetUserIdByUserNameAsync(_client, recipientUserName, _output);

        var uniqueTitle = $"Unique Search Title {Guid.NewGuid()}";
        var newShipment = new ShipmentForCreationDto
        {
            Title = uniqueTitle,
            RecipientName = recipientUserName,
            RecipientAddress = "123 Filter St",
            RecipientId = recipientId
        };

        await _client.PostAsJsonAsync("/api/Shipment", newShipment);

        // Act - Search with parameters
        var response = await _client.GetAsync($"/api/Shipment?SearchTitle={Uri.EscapeDataString(uniqueTitle)}&PageSize=5");

        // Assert
        response.EnsureSuccessStatusCode();
        var shipments = await response.Content.ReadFromJsonAsync<IEnumerable<ShipmentDto>>();
        Assert.NotNull(shipments);
        Assert.Contains(shipments, s => s.title == uniqueTitle);
    }

    [Fact]
    public async Task GetAllShipments_ReturnsUnauthorized_WhenRoleNotAllowed()
    {
        // Arrange
        var username = Configuration["SupplierAccounts:SupplierAlphaUserName"];
        var password = Configuration["SupplierAccounts:SupplierAlphaPassword"];

        var token = await AuthHelper.GetAccessTokenAsync(_client, username, password);
        Assert.False(string.IsNullOrWhiteSpace(token), "Failed to acquire access token.");

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act - Try to get shipments with a role the user doesn't have
        var response = await _client.GetAsync("/api/Shipment?role=InvalidRole");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetShipmentById_ReturnsBadRequest_WhenIdFormatIsInvalid()
    {
        // Arrange
        var username = Configuration["SupplierAccounts:SupplierAlphaUserName"];
        var password = Configuration["SupplierAccounts:SupplierAlphaPassword"];

        var token = await AuthHelper.GetAccessTokenAsync(_client, username, password);
        Assert.False(string.IsNullOrWhiteSpace(token), "Failed to acquire access token.");

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act - Use an invalid GUID format
        var invalidId = "not-a-guid";
        var response = await _client.GetAsync($"/api/Shipment/{invalidId}");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PatchShipment_ReturnsBadRequest_WhenPatchDocIsNull()
    {
        // Arrange
        var username = Configuration["SupplierAccounts:SupplierAlphaUserName"];
        var password = Configuration["SupplierAccounts:SupplierAlphaPassword"];

        var token = await AuthHelper.GetAccessTokenAsync(_client, username, password);
        Assert.False(string.IsNullOrWhiteSpace(token), "Failed to acquire access token.");

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Create shipment to patch
        var recipientUserName = Configuration["CustomerAccounts:CustomerOneUserName"];
        var recipientId = await UserAccountHelper.GetUserIdByUserNameAsync(_client, recipientUserName, _output);

        var newShipment = new ShipmentForCreationDto
        {
            Title = "Shipment To Patch",
            RecipientName = recipientUserName,
            RecipientAddress = "456 Patch St",
            RecipientId = recipientId
        };

        var createResponse = await _client.PostAsJsonAsync("/api/Shipment", newShipment);
        var createdShipment = await createResponse.Content.ReadFromJsonAsync<ShipmentDto>();

        // Act - Send null patch document
        var emptyContent = new StringContent("", Encoding.UTF8, "application/json-patch+json");
        var response = await _client.PatchAsync($"/api/Shipment/{createdShipment.Id}", emptyContent);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PatchShipment_ReturnsUnprocessableEntity_WhenInvalidPatch()
    {
        // Arrange
        var username = Configuration["SupplierAccounts:SupplierAlphaUserName"];
        var password = Configuration["SupplierAccounts:SupplierAlphaPassword"];

        var token = await AuthHelper.GetAccessTokenAsync(_client, username, password);
        Assert.False(string.IsNullOrWhiteSpace(token), "Failed to acquire access token.");

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Create shipment to patch
        var recipientUserName = Configuration["CustomerAccounts:CustomerOneUserName"];
        var recipientId = await UserAccountHelper.GetUserIdByUserNameAsync(_client, recipientUserName, _output);

        var newShipment = new ShipmentForCreationDto
        {
            Title = "Invalid Patch Shipment",
            RecipientName = recipientUserName,
            RecipientAddress = "789 Invalid St",
            RecipientId = recipientId
        };

        var createResponse = await _client.PostAsJsonAsync("/api/Shipment", newShipment);
        var createdShipment = await createResponse.Content.ReadFromJsonAsync<ShipmentDto>();

        // Create an invalid JSON patch document (setting title to empty string)
        var patchDoc = new[]
        {
        new { op = "replace", path = "/title", value = "" }
    };

        var jsonContent = JsonSerializer.Serialize(patchDoc);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json-patch+json");

        // Act
        var response = await _client.PatchAsync($"/api/Shipment/{createdShipment.Id}", content);

        // Assert
        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    [Fact]
    public async Task DeleteShipment_ReturnsBadRequest_WhenShipmentIdIsEmpty()
    {
        // Arrange
        var username = Configuration["SupplierAccounts:SupplierAlphaUserName"];
        var password = Configuration["SupplierAccounts:SupplierAlphaPassword"];

        var token = await AuthHelper.GetAccessTokenAsync(_client, username, password);
        Assert.False(string.IsNullOrWhiteSpace(token), "Failed to acquire access token.");

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act - empty shipment ID
        var response = await _client.DeleteAsync("/api/Shipment/");

        // Assert
        Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
    }

    [Fact]
    public async Task CreateShipment_ReturnsNotFound_WhenRecipientUserNotFound()
    {
        // Arrange
        var username = Configuration["SupplierAccounts:SupplierAlphaUserName"];
        var password = Configuration["SupplierAccounts:SupplierAlphaPassword"];

        var token = await AuthHelper.GetAccessTokenAsync(_client, username, password);
        Assert.False(string.IsNullOrWhiteSpace(token), "Failed to acquire access token.");

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Create shipment with non-existent recipient
        var shipment = new ShipmentForCreationDto
        {
            Title = "Invalid Recipient Shipment",
            RecipientName = "NonExistentUser_" + Guid.NewGuid(),
            RecipientAddress = "123 Nonexistent St",
            RecipientId = Guid.NewGuid().ToString() // This will be overridden by controller
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Shipment", shipment);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task PatchShipment_ReturnsNotFound_WhenShipmentDoesNotExist()
    {
        // Arrange
        var username = Configuration["SupplierAccounts:SupplierAlphaUserName"];
        var password = Configuration["SupplierAccounts:SupplierAlphaPassword"];

        var token = await AuthHelper.GetAccessTokenAsync(_client, username, password);
        Assert.False(string.IsNullOrWhiteSpace(token), "Failed to acquire access token.");

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Create a patch document
        var patchDoc = new[]
        {
        new { op = "replace", path = "/title", value = "Updated Non-Existent Shipment" }
    };

        var jsonContent = JsonSerializer.Serialize(patchDoc);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json-patch+json");

        // Use a non-existent shipment ID
        var nonExistentId = Guid.NewGuid().ToString();

        // Act
        var response = await _client.PatchAsync($"/api/Shipment/{nonExistentId}", content);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
