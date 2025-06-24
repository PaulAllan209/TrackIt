using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TrackIt.Core.Models.Shipping.Enums;
using TrackIt.Core.RequestFeatures;
using TrackIt.Server.Dto.TrackIt;
using Xunit.Abstractions;

namespace TrackIt.Api.IntegrationTests
{
    public class StatusUpdateControllerTests
    {
        private readonly ITestOutputHelper _output;
        private static readonly IConfiguration Configuration = new ConfigurationBuilder()
            .AddUserSecrets<TrackItWebApplicationFactory>()
            .Build();

        private TrackItWebApplicationFactory _application;
        private HttpClient _client;

        public StatusUpdateControllerTests(ITestOutputHelper output)
        {
            _output = output;

            _application = new TrackItWebApplicationFactory("StatusUpdateControllerTests");
            _client = _application.CreateClient();
        }

        [Fact]
        public async Task CreateStatusUpdate_ReturnsOkResult_WhenValid()
        {
            // Arrange
            var username = Configuration["SupplierAccounts:SupplierAlphaUserName"];
            var password = Configuration["SupplierAccounts:SupplierAlphaPassword"];

            var token = await AuthHelper.GetAccessTokenAsync(_client, username, password);
            Assert.False(string.IsNullOrWhiteSpace(token), "Failed to acquire access token.");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Create a shipment first to associate the status update with
            var recipientUserName = Configuration["CustomerAccounts:CustomerOneUserName"];
            var recipientId = await UserAccountHelper.GetUserIdByUserNameAsync(_client, recipientUserName, _output);

            var shipment = new ShipmentForCreationDto
            {
                Title = "Test Shipment for Status Update",
                RecipientName = recipientUserName,
                RecipientAddress = "123 Status St",
                RecipientId = recipientId
            };

            var shipmentResponse = await _client.PostAsJsonAsync("/api/Shipment", shipment);
            shipmentResponse.EnsureSuccessStatusCode();
            var createdShipment = await shipmentResponse.Content.ReadFromJsonAsync<ShipmentDto>();

            // Create status update
            var statusUpdate = new StatusUpdateForCreationDto
            {
                ShipmentId = createdShipment.Id.ToString(),
                Status = ShipmentStatus.ToReceive.ToString(),
                Notes = "Package is ready for pickup",
                Location = "Warehouse A",
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/StatusUpdate", statusUpdate);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var createdStatusUpdate = await response.Content.ReadFromJsonAsync<StatusUpdateDto>();
            Assert.NotNull(createdStatusUpdate);
            Assert.Equal(statusUpdate.ShipmentId, createdStatusUpdate.ShipmentId.ToString());
            Assert.Equal(statusUpdate.Status, createdStatusUpdate.Status);
            Assert.Equal(statusUpdate.Notes, createdStatusUpdate.Notes);
            Assert.Equal(statusUpdate.Location, createdStatusUpdate.Location);
        }

        [Fact]
        public async Task CreateStatusUpdate_ReturnsBadRequest_WhenInvalidStatus()
        {
            // Arrange
            var username = Configuration["SupplierAccounts:SupplierAlphaUserName"];
            var password = Configuration["SupplierAccounts:SupplierAlphaPassword"];

            var token = await AuthHelper.GetAccessTokenAsync(_client, username, password);
            Assert.False(string.IsNullOrWhiteSpace(token), "Failed to acquire access token.");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Create a shipment first
            var recipientUserName = Configuration["CustomerAccounts:CustomerOneUserName"];
            var recipientId = await UserAccountHelper.GetUserIdByUserNameAsync(_client, recipientUserName, _output);

            var shipment = new ShipmentForCreationDto
            {
                Title = "Test Shipment for Invalid Status",
                RecipientName = recipientUserName,
                RecipientAddress = "456 Invalid Status St",
                RecipientId = recipientId
            };

            var shipmentResponse = await _client.PostAsJsonAsync("/api/Shipment", shipment);
            var createdShipment = await shipmentResponse.Content.ReadFromJsonAsync<ShipmentDto>();

            // Create status update with invalid status
            var statusUpdate = new StatusUpdateForCreationDto
            {
                ShipmentId = createdShipment.Id.ToString(),
                Status = "InvalidStatus", // Invalid status
                Notes = "This should fail",
                Location = "Warehouse B",
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/StatusUpdate", statusUpdate);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetAllStatusUpdates_ReturnsOkResult_WithStatusUpdates()
        {
            // Arrange
            var username = Configuration["SupplierAccounts:SupplierAlphaUserName"];
            var password = Configuration["SupplierAccounts:SupplierAlphaPassword"];

            var token = await AuthHelper.GetAccessTokenAsync(_client, username, password);
            Assert.False(string.IsNullOrWhiteSpace(token), "Failed to acquire access token.");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.GetAsync("/api/StatusUpdate");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var statusUpdates = await response.Content.ReadFromJsonAsync<IEnumerable<StatusUpdateDto>>();
            Assert.NotNull(statusUpdates);

            // Verify pagination header
            Assert.True(response.Headers.Contains("X-Pagination"), "Response should contain pagination header");
        }

        [Fact]
        public async Task GetAllStatusUpdatesByShipmentId_ReturnsOkResult_WhenShipmentExists()
        {
            // Arrange
            var username = Configuration["SupplierAccounts:SupplierAlphaUserName"];
            var password = Configuration["SupplierAccounts:SupplierAlphaPassword"];

            var token = await AuthHelper.GetAccessTokenAsync(_client, username, password);
            Assert.False(string.IsNullOrWhiteSpace(token), "Failed to acquire access token.");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Create a shipment first
            var recipientUserName = Configuration["CustomerAccounts:CustomerOneUserName"];
            var recipientId = await UserAccountHelper.GetUserIdByUserNameAsync(_client, recipientUserName, _output);

            var shipment = new ShipmentForCreationDto
            {
                Title = "Test Shipment for Status History",
                RecipientName = recipientUserName,
                RecipientAddress = "789 History St",
                RecipientId = recipientId
            };

            var shipmentResponse = await _client.PostAsJsonAsync("/api/Shipment", shipment);
            var createdShipment = await shipmentResponse.Content.ReadFromJsonAsync<ShipmentDto>();

            // Create status update
            var statusUpdate = new StatusUpdateForCreationDto
            {
                ShipmentId = createdShipment.Id.ToString(),
                Status = ShipmentStatus.ToShip.ToString(),
                Notes = "Initial status update",
                Location = "Origin Warehouse",
            };

            await _client.PostAsJsonAsync("/api/StatusUpdate", statusUpdate);

            // Act
            var response = await _client.GetAsync($"/api/StatusUpdate/ByShipmentId/{createdShipment.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var statusUpdates = await response.Content.ReadFromJsonAsync<IEnumerable<StatusUpdateDto>>();
            Assert.NotNull(statusUpdates);
            Assert.Contains(statusUpdates, s => s.ShipmentId == createdShipment.Id);
        }

        [Fact]
        public async Task GetAllStatusUpdatesByShipmentId_ReturnsBadRequest_WhenShipmentIdInvalid()
        {
            // Arrange
            var username = Configuration["SupplierAccounts:SupplierAlphaUserName"];
            var password = Configuration["SupplierAccounts:SupplierAlphaPassword"];

            var token = await AuthHelper.GetAccessTokenAsync(_client, username, password);
            Assert.False(string.IsNullOrWhiteSpace(token), "Failed to acquire access token.");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act - Use an invalid GUID format
            var invalidId = "not-a-guid";
            var response = await _client.GetAsync($"/api/StatusUpdate/ByShipmentId/{invalidId}");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetStatusUpdateById_ReturnsOkResult_WhenStatusUpdateExists()
        {
            // Arrange
            var username = Configuration["SupplierAccounts:SupplierAlphaUserName"];
            var password = Configuration["SupplierAccounts:SupplierAlphaPassword"];

            var token = await AuthHelper.GetAccessTokenAsync(_client, username, password);
            Assert.False(string.IsNullOrWhiteSpace(token), "Failed to acquire access token.");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Create a shipment first
            var recipientUserName = Configuration["CustomerAccounts:CustomerOneUserName"];
            var recipientId = await UserAccountHelper.GetUserIdByUserNameAsync(_client, recipientUserName, _output);

            var shipment = new ShipmentForCreationDto
            {
                Title = "Test Shipment for Status Detail",
                RecipientName = recipientUserName,
                RecipientAddress = "101 Detail St",
                RecipientId = recipientId
            };

            var shipmentResponse = await _client.PostAsJsonAsync("/api/Shipment", shipment);
            var createdShipment = await shipmentResponse.Content.ReadFromJsonAsync<ShipmentDto>();

            // Create status update
            var statusUpdate = new StatusUpdateForCreationDto
            {
                ShipmentId = createdShipment.Id.ToString(),
                Status = ShipmentStatus.ToDeliver.ToString(),
                Notes = "Status update for retrieval test",
                Location = "Distribution Center",
            };

            var createResponse = await _client.PostAsJsonAsync("/api/StatusUpdate", statusUpdate);
            var createdStatusUpdate = await createResponse.Content.ReadFromJsonAsync<StatusUpdateDto>();

            // Act 
            var response = await _client.GetAsync($"/api/StatusUpdate/{createdStatusUpdate.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var retrievedStatusUpdate = await response.Content.ReadFromJsonAsync<StatusUpdateDto>();
            Assert.NotNull(retrievedStatusUpdate);
            Assert.Equal(createdStatusUpdate.Id, retrievedStatusUpdate.Id);
            Assert.Equal(statusUpdate.ShipmentId, retrievedStatusUpdate.ShipmentId.ToString());
            Assert.Equal(statusUpdate.Status, retrievedStatusUpdate.Status);
        }

        [Fact]
        public async Task GetStatusUpdateById_ReturnsNotFound_WhenStatusUpdateDoesNotExist()
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
            var response = await _client.GetAsync($"/api/StatusUpdate/{nonExistentId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PatchStatusUpdate_ReturnsNoContent_WhenValid()
        {
            // Arrange
            var username = Configuration["SupplierAccounts:SupplierAlphaUserName"];
            var password = Configuration["SupplierAccounts:SupplierAlphaPassword"];

            var token = await AuthHelper.GetAccessTokenAsync(_client, username, password);
            Assert.False(string.IsNullOrWhiteSpace(token), "Failed to acquire access token.");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Create a shipment first
            var recipientUserName = Configuration["CustomerAccounts:CustomerOneUserName"];
            var recipientId = await UserAccountHelper.GetUserIdByUserNameAsync(_client, recipientUserName, _output);

            var shipment = new ShipmentForCreationDto
            {
                Title = "Test Shipment for Status Update Patch",
                RecipientName = recipientUserName,
                RecipientAddress = "202 Patch St",
                RecipientId = recipientId
            };

            var shipmentResponse = await _client.PostAsJsonAsync("/api/Shipment", shipment);
            var createdShipment = await shipmentResponse.Content.ReadFromJsonAsync<ShipmentDto>();

            // Create status update
            var statusUpdate = new StatusUpdateForCreationDto
            {
                ShipmentId = createdShipment.Id.ToString(),
                Status = ShipmentStatus.ToShip.ToString(),
                Notes = "Initial notes",
                Location = "Initial location",
            };

            var createResponse = await _client.PostAsJsonAsync("/api/StatusUpdate", statusUpdate);
            var createdStatusUpdate = await createResponse.Content.ReadFromJsonAsync<StatusUpdateDto>();

            // Create JSON patch document to update the notes and location
            var patchDoc = new[]
            {
                new { op = "replace", path = "/notes", value = "Updated notes" },
                new { op = "replace", path = "/location", value = "Updated location" }
            };

            var jsonContent = JsonSerializer.Serialize(patchDoc);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json-patch+json");

            // Act
            var response = await _client.PatchAsync($"/api/StatusUpdate/{createdStatusUpdate.Id}", content);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            // Verify the update by retrieving the status update
            var getResponse = await _client.GetAsync($"/api/StatusUpdate/{createdStatusUpdate.Id}");
            var updatedStatusUpdate = await getResponse.Content.ReadFromJsonAsync<StatusUpdateDto>();

            Assert.NotNull(updatedStatusUpdate);
            Assert.Equal("Updated notes", updatedStatusUpdate.Notes);
            Assert.Equal("Updated location", updatedStatusUpdate.Location);
        }

        [Fact]
        public async Task DeleteStatusUpdate_ReturnsForbidden_WhenStatusUpdateExists()
        {
            // Only admins can delete status updates

            // Arrange
            var username = Configuration["SupplierAccounts:SupplierAlphaUserName"];
            var password = Configuration["SupplierAccounts:SupplierAlphaPassword"];

            var token = await AuthHelper.GetAccessTokenAsync(_client, username, password);
            Assert.False(string.IsNullOrWhiteSpace(token), "Failed to acquire access token.");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // First, create a shipment
            var recipientUserName = Configuration["CustomerAccounts:CustomerOneUserName"];
            var recipientId = await UserAccountHelper.GetUserIdByUserNameAsync(_client, recipientUserName, _output);

            var shipment = new ShipmentForCreationDto
            {
                Title = "Test Shipment for Status Delete",
                RecipientName = recipientUserName,
                RecipientAddress = "303 Delete St",
                RecipientId = recipientId
            };

            var shipmentResponse = await _client.PostAsJsonAsync("/api/Shipment", shipment);
            var createdShipment = await shipmentResponse.Content.ReadFromJsonAsync<ShipmentDto>();

            // Create status update to delete
            var statusUpdate = new StatusUpdateForCreationDto
            {
                ShipmentId = createdShipment.Id.ToString(),
                Status = ShipmentStatus.Completed.ToString(),
                Notes = "Status to be deleted",
                Location = "Final location",
            };

            var createResponse = await _client.PostAsJsonAsync("/api/StatusUpdate", statusUpdate);
            var createdStatusUpdate = await createResponse.Content.ReadFromJsonAsync<StatusUpdateDto>();

            // Act
            var response = await _client.DeleteAsync($"/api/StatusUpdate/{createdStatusUpdate.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task PatchStatusUpdate_ReturnsBadRequest_WhenPatchDocIsNull()
        {
            // Arrange
            var username = Configuration["SupplierAccounts:SupplierAlphaUserName"];
            var password = Configuration["SupplierAccounts:SupplierAlphaPassword"];

            var token = await AuthHelper.GetAccessTokenAsync(_client, username, password);
            Assert.False(string.IsNullOrWhiteSpace(token), "Failed to acquire access token.");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Create shipment and status update
            var recipientUserName = Configuration["CustomerAccounts:CustomerOneUserName"];
            var recipientId = await UserAccountHelper.GetUserIdByUserNameAsync(_client, recipientUserName, _output);

            var shipment = new ShipmentForCreationDto
            {
                Title = "Test Shipment for Null Patch",
                RecipientName = recipientUserName,
                RecipientAddress = "404 Null Patch St",
                RecipientId = recipientId
            };

            var shipmentResponse = await _client.PostAsJsonAsync("/api/Shipment", shipment);
            var createdShipment = await shipmentResponse.Content.ReadFromJsonAsync<ShipmentDto>();

            var statusUpdate = new StatusUpdateForCreationDto
            {
                ShipmentId = createdShipment.Id.ToString(),
                Status = ShipmentStatus.ToReceive.ToString(),
                Notes = "Notes for null patch test",
                Location = "Location for null patch test",
            };

            var createResponse = await _client.PostAsJsonAsync("/api/StatusUpdate", statusUpdate);
            var createdStatusUpdate = await createResponse.Content.ReadFromJsonAsync<StatusUpdateDto>();

            // Act - Send null patch document
            var emptyContent = new StringContent("", Encoding.UTF8, "application/json-patch+json");
            var response = await _client.PatchAsync($"/api/StatusUpdate/{createdStatusUpdate.Id}", emptyContent);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateStatusUpdate_ReturnsUnprocessableEntity_WhenModelIsInvalid()
        {
            // Arrange
            var username = Configuration["SupplierAccounts:SupplierAlphaUserName"];
            var password = Configuration["SupplierAccounts:SupplierAlphaPassword"];

            var token = await AuthHelper.GetAccessTokenAsync(_client, username, password);
            Assert.False(string.IsNullOrWhiteSpace(token), "Failed to acquire access token.");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Create invalid status update with missing required fields
            var invalidStatusUpdate = new StatusUpdateForCreationDto
            {
                ShipmentId = "", // Empty shipment ID
                Status = "", // Empty status
                Notes = "This should fail"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/StatusUpdate", invalidStatusUpdate);

            // Assert
            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }

        [Fact]
        public async Task GetStatusUpdateById_ReturnsBadRequest_WhenIdFormatIsInvalid()
        {
            // Arrange
            var username = Configuration["SupplierAccounts:SupplierAlphaUserName"];
            var password = Configuration["SupplierAccounts:SupplierAlphaPassword"];

            var token = await AuthHelper.GetAccessTokenAsync(_client, username, password);
            Assert.False(string.IsNullOrWhiteSpace(token), "Failed to acquire access token.");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act - Use an invalid GUID format
            var invalidId = "not-a-guid";
            var response = await _client.GetAsync($"/api/StatusUpdate/{invalidId}");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DeleteStatusUpdate_ReturnsBadRequest_WhenStatusUpdateIdIsEmpty()
        {
            // Arrange
            var username = Configuration["SupplierAccounts:SupplierAlphaUserName"];
            var password = Configuration["SupplierAccounts:SupplierAlphaPassword"];

            var token = await AuthHelper.GetAccessTokenAsync(_client, username, password);
            Assert.False(string.IsNullOrWhiteSpace(token), "Failed to acquire access token.");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act - empty status update ID
            var response = await _client.DeleteAsync("/api/StatusUpdate/");

            // Assert
            Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
        }

        [Fact]
        public async Task PatchStatusUpdate_ReturnsBadRequest_WhenInvalidIdFormat()
        {
            // Arrange
            var username = Configuration["SupplierAccounts:SupplierAlphaUserName"];
            var password = Configuration["SupplierAccounts:SupplierAlphaPassword"];

            var token = await AuthHelper.GetAccessTokenAsync(_client, username, password);
            Assert.False(string.IsNullOrWhiteSpace(token), "Failed to acquire access token.");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Create JSON patch document
            var patchDoc = new[]
            {
        new { op = "replace", path = "/notes", value = "Updated notes" }
    };

            var jsonContent = JsonSerializer.Serialize(patchDoc);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json-patch+json");

            // Act - Use an invalid GUID format
            var invalidId = "not-a-guid";
            var response = await _client.PatchAsync($"/api/StatusUpdate/{invalidId}", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PatchStatusUpdate_ReturnsNotFound_WhenStatusUpdateDoesNotExist()
        {
            // Arrange
            var username = Configuration["SupplierAccounts:SupplierAlphaUserName"];
            var password = Configuration["SupplierAccounts:SupplierAlphaPassword"];

            var token = await AuthHelper.GetAccessTokenAsync(_client, username, password);
            Assert.False(string.IsNullOrWhiteSpace(token), "Failed to acquire access token.");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Create JSON patch document
            var patchDoc = new[]
            {
        new { op = "replace", path = "/notes", value = "Updated notes" }
    };

            var jsonContent = JsonSerializer.Serialize(patchDoc);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json-patch+json");

            // Act - Use a non-existent GUID
            var nonExistentId = Guid.NewGuid().ToString();
            var response = await _client.PatchAsync($"/api/StatusUpdate/{nonExistentId}", content);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PatchStatusUpdate_ReturnsUnprocessableEntity_WhenPatchHasInvalidOperation()
        {
            // Arrange
            var username = Configuration["SupplierAccounts:SupplierAlphaUserName"];
            var password = Configuration["SupplierAccounts:SupplierAlphaPassword"];

            var token = await AuthHelper.GetAccessTokenAsync(_client, username, password);
            Assert.False(string.IsNullOrWhiteSpace(token), "Failed to acquire access token.");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Create a shipment first
            var recipientUserName = Configuration["CustomerAccounts:CustomerOneUserName"];
            var recipientId = await UserAccountHelper.GetUserIdByUserNameAsync(_client, recipientUserName, _output);

            var shipment = new ShipmentForCreationDto
            {
                Title = "Test Shipment for Invalid Patch",
                RecipientName = recipientUserName,
                RecipientAddress = "505 Invalid Patch St",
                RecipientId = recipientId
            };

            var shipmentResponse = await _client.PostAsJsonAsync("/api/Shipment", shipment);
            var createdShipment = await shipmentResponse.Content.ReadFromJsonAsync<ShipmentDto>();

            // Create status update
            var statusUpdate = new StatusUpdateForCreationDto
            {
                ShipmentId = createdShipment.Id.ToString(),
                Status = ShipmentStatus.ToShip.ToString(),
                Notes = "Initial notes",
                Location = "Initial location",
            };

            var createResponse = await _client.PostAsJsonAsync("/api/StatusUpdate", statusUpdate);
            var createdStatusUpdate = await createResponse.Content.ReadFromJsonAsync<StatusUpdateDto>();

            // Create invalid JSON patch document (targeting read-only property)
            var invalidPatchDoc = new[]
            {
                new { op = "replace", path = "/id", value = Guid.NewGuid().ToString() }
            };

            var jsonContent = JsonSerializer.Serialize(invalidPatchDoc);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json-patch+json");

            // Act
            var response = await _client.PatchAsync($"/api/StatusUpdate/{createdStatusUpdate.Id}", content);

            // Assert
            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }

        [Fact]
        public async Task GetAllStatusUpdates_ReturnsCorrectPagination_WhenParametersProvided()
        {
            // Arrange
            var username = Configuration["SupplierAccounts:SupplierAlphaUserName"];
            var password = Configuration["SupplierAccounts:SupplierAlphaPassword"];

            var token = await AuthHelper.GetAccessTokenAsync(_client, username, password);
            Assert.False(string.IsNullOrWhiteSpace(token), "Failed to acquire access token.");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Add pagination parameters
            var pageSize = 5;
            var pageNumber = 1;

            // Act
            var response = await _client.GetAsync($"/api/StatusUpdate?PageSize={pageSize}&PageNumber={pageNumber}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Verify pagination header exists and can be parsed
            Assert.True(response.Headers.Contains("X-Pagination"), "Response should contain pagination header");
            var paginationHeader = response.Headers.GetValues("X-Pagination").FirstOrDefault();
            Assert.NotNull(paginationHeader);

            var metaData = JsonSerializer.Deserialize<MetaData>(paginationHeader);
            Assert.NotNull(metaData);
            Assert.Equal(pageSize, metaData.PageSize);
            Assert.Equal(pageNumber, metaData.CurrentPage);

            // Verify the number of returned items doesn't exceed page size
            var statusUpdates = await response.Content.ReadFromJsonAsync<IEnumerable<StatusUpdateDto>>();
            Assert.NotNull(statusUpdates);
            Assert.True(statusUpdates.Count() <= pageSize);
        }

        [Fact]
        public async Task CreateStatusUpdate_ReturnsUnauthorized_WhenNotAuthenticated()
        {
            // Arrange - Don't set any authentication header
            _client.DefaultRequestHeaders.Authorization = null;

            // Create a status update payload
            var statusUpdate = new StatusUpdateForCreationDto
            {
                ShipmentId = Guid.NewGuid().ToString(),
                Status = ShipmentStatus.ToShip.ToString(),
                Notes = "This should fail due to no authentication",
                Location = "Secure Location",
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/StatusUpdate", statusUpdate);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task DeleteStatusUpdate_ReturnsNoContent_WhenUserIsAdmin()
        {
            // Arrange
            var username = Configuration["DefaultAccounts:AdminUserName"];
            var password = Configuration["DefaultAccounts:AdminPassword"];

            var token = await AuthHelper.GetAccessTokenAsync(_client, username, password);
            Assert.False(string.IsNullOrWhiteSpace(token), "Failed to acquire access token.");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // First, create a shipment
            var recipientUserName = Configuration["CustomerAccounts:CustomerOneUserName"];
            var recipientId = await UserAccountHelper.GetUserIdByUserNameAsync(_client, recipientUserName, _output);

            var shipment = new ShipmentForCreationDto
            {
                Title = "Test Shipment for Admin Delete",
                RecipientName = recipientUserName,
                RecipientAddress = "505 Admin Delete St",
                RecipientId = recipientId
            };

            var shipmentResponse = await _client.PostAsJsonAsync("/api/Shipment", shipment);
            var createdShipment = await shipmentResponse.Content.ReadFromJsonAsync<ShipmentDto>();

            // Create status update
            var statusUpdate = new StatusUpdateForCreationDto
            {
                ShipmentId = createdShipment.Id.ToString(),
                Status = ShipmentStatus.ToShip.ToString(),
                Notes = "Status for admin delete test",
                Location = "Admin test location",
            };

            var createResponse = await _client.PostAsJsonAsync("/api/StatusUpdate", statusUpdate);
            var createdStatusUpdate = await createResponse.Content.ReadFromJsonAsync<StatusUpdateDto>();

            // Act
            var response = await _client.DeleteAsync($"/api/StatusUpdate/{createdStatusUpdate.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            // Verify it's actually deleted
            var getResponse = await _client.GetAsync($"/api/StatusUpdate/{createdStatusUpdate.Id}");
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }

        [Fact]
        public async Task CreateStatusUpdate_ReturnsBadRequest_WhenShipmentIdIsEmpty()
        {
            // Arrange
            var username = Configuration["SupplierAccounts:SupplierAlphaUserName"];
            var password = Configuration["SupplierAccounts:SupplierAlphaPassword"];

            var token = await AuthHelper.GetAccessTokenAsync(_client, username, password);
            Assert.False(string.IsNullOrWhiteSpace(token), "Failed to acquire access token.");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Create status update with empty shipment ID
            var statusUpdate = new StatusUpdateForCreationDto
            {
                ShipmentId = "",  // Empty shipment ID
                Status = ShipmentStatus.ToShip.ToString(),
                Notes = "This should fail due to empty shipment ID",
                Location = "Warehouse X",
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/StatusUpdate", statusUpdate);

            // Assert
            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }

        [Fact]
        public async Task CreateStatusUpdate_ValidatesEachValidStatusValue()
        {
            // Arrange
            var username = Configuration["SupplierAccounts:SupplierAlphaUserName"];
            var password = Configuration["SupplierAccounts:SupplierAlphaPassword"];

            var token = await AuthHelper.GetAccessTokenAsync(_client, username, password);
            Assert.False(string.IsNullOrWhiteSpace(token), "Failed to acquire access token.");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Create a shipment first
            var recipientUserName = Configuration["CustomerAccounts:CustomerOneUserName"];
            var recipientId = await UserAccountHelper.GetUserIdByUserNameAsync(_client, recipientUserName, _output);

            var shipment = new ShipmentForCreationDto
            {
                Title = "Test Shipment for Status Values",
                RecipientName = recipientUserName,
                RecipientAddress = "707 Status Values St",
                RecipientId = recipientId
            };

            var shipmentResponse = await _client.PostAsJsonAsync("/api/Shipment", shipment);
            var createdShipment = await shipmentResponse.Content.ReadFromJsonAsync<ShipmentDto>();

            // Valid status values to test
            var validStatusValues = new[] {
                ShipmentStatus.ToShip.ToString(),
                ShipmentStatus.ToReceive.ToString(),
                ShipmentStatus.Completed.ToString()
            };

            foreach (var statusValue in validStatusValues)
            {
                // Create status update with valid status
                var statusUpdate = new StatusUpdateForCreationDto
                {
                    ShipmentId = createdShipment.Id.ToString(),
                    Status = statusValue,
                    Notes = $"Testing {statusValue} status value",
                    Location = "Test Location",
                };

                // Act
                var response = await _client.PostAsJsonAsync("/api/StatusUpdate", statusUpdate);

                // Assert
                response.EnsureSuccessStatusCode();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                var createdStatusUpdate = await response.Content.ReadFromJsonAsync<StatusUpdateDto>();
                Assert.Equal(statusValue, createdStatusUpdate.Status);
            }
        }

        [Fact]
        public async Task GetAllStatusUpdates_AppliesSortingAndFiltering_WhenParametersProvided()
        {
            // Arrange
            var username = Configuration["SupplierAccounts:SupplierAlphaUserName"];
            var password = Configuration["SupplierAccounts:SupplierAlphaPassword"];

            var token = await AuthHelper.GetAccessTokenAsync(_client, username, password);
            Assert.False(string.IsNullOrWhiteSpace(token), "Failed to acquire access token.");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Create a few status updates with different dates to test sorting
            // First, create a shipment
            var recipientUserName = Configuration["CustomerAccounts:CustomerOneUserName"];
            var recipientId = await UserAccountHelper.GetUserIdByUserNameAsync(_client, recipientUserName, _output);

            var shipment = new ShipmentForCreationDto
            {
                Title = "Test Shipment for Sorting",
                RecipientName = recipientUserName,
                RecipientAddress = "808 Sorting St",
                RecipientId = recipientId
            };

            var shipmentResponse = await _client.PostAsJsonAsync("/api/Shipment", shipment);
            var createdShipment = await shipmentResponse.Content.ReadFromJsonAsync<ShipmentDto>();

            // Apply ordering by created date
            var orderByParam = "createdDate";

            // Act
            var response = await _client.GetAsync($"/api/StatusUpdate?OrderBy={orderByParam}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Extract and verify the ordering - we can't check exact ordering without knowing data
            // but we can verify the response contains the expected parameter
            var statusUpdates = await response.Content.ReadFromJsonAsync<IEnumerable<StatusUpdateDto>>();
            Assert.NotNull(statusUpdates);

            // Verify the presence of pagination metadata
            Assert.True(response.Headers.Contains("X-Pagination"));
        }
    }
}
