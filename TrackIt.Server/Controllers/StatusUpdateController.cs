using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using TrackIt.Core.Models.Account;
using TrackIt.Core.Models.Shipping;
using TrackIt.Core.Models.Shipping.Enums;
using TrackIt.Core.RequestFeatures;
using TrackIt.Core.Services.Shipping;
using TrackIt.Core.Services.Shipping.Interfaces;
using TrackIt.Server.Attributes;
using TrackIt.Server.Authorization;
using TrackIt.Server.Dto.TrackIt;

namespace TrackIt.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StatusUpdateController : BaseApiController<StatusUpdateController>
    {
        private readonly IStatusUpdateService _statusUpdateService;
        private readonly UserManager<ApplicationUser> _userManager;

        public StatusUpdateController(
            ILogger<StatusUpdateController> logger,
            IMapper mapper,
            IStatusUpdateService statusUpdateService,
            UserManager<ApplicationUser> userManager)
            : base(logger, mapper)
        {
            _statusUpdateService = statusUpdateService;
            _userManager = userManager;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Policy = AuthPolicies.CreateStatusPolicy)]
        public async Task<IActionResult> CreateStatusUpdate([FromBody] StatusUpdateForCreationDto statusUpdateForCreationDto)
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity(statusUpdateForCreationDto);

            if (!IsValidShipmentStatusString(statusUpdateForCreationDto.Status))
                return BadRequest($"Invalid status value. Valid status values are " +
                    $"{ShipmentStatus.ToShip.ToString()}, " +
                    $"{ShipmentStatus.ToReceive.ToString()}, and " +
                    $"{ShipmentStatus.Completed.ToString()}");

            var userId = GetCurrentUserId();
            var userRoles = GetCurrentUserRoles();

            string roleToUse = GetHighestPrivilegeRole(userRoles);

            var statusUpdateEntity = _mapper.Map<StatusUpdate>(statusUpdateForCreationDto);

            var statusUpdateEntityReturned = await _statusUpdateService.CreateStatusUpdateAsync(statusUpdateEntity);

            var statusUpdateEntityDto = _mapper.Map<StatusUpdateDto>(statusUpdateEntityReturned);

            return Ok(statusUpdateEntityDto);
        }

        [HttpGet]
        [Authorize(Policy = AuthPolicies.ViewStatusHistoryPolicy)]
        public async Task<IActionResult> GetAllStatusUpdates([FromQuery] StatusUpdateParameters statusUpdateParameters)
        {
            var userId = GetCurrentUserId();
            var userRoles = GetCurrentUserRoles();

            // Check if the user is an admin
            bool isAdmin = userRoles.Contains(UserType.Admin, StringComparer.OrdinalIgnoreCase);

            var pagedResultStatusUpdates = await _statusUpdateService.GetAllStatusUpdatesAsync(isAdmin, statusUpdateParameters, trackChanges: false, userId);

            var statusUpdateDtos = _mapper.Map<IEnumerable<StatusUpdateDto>>(pagedResultStatusUpdates.statusUpdates);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResultStatusUpdates.metaData));


            return Ok(statusUpdateDtos);
        }

        [HttpGet("ByShipmentId/{shipmentId}")]
        [Authorize(Policy = AuthPolicies.ViewStatusHistoryPolicy)]
        public async Task<IActionResult> GetAllStatusUpdatesByShipmentId(string shipmentId)
        {
            if (string.IsNullOrEmpty(shipmentId))
                return BadRequest("ShipmentId is required.");

            // ShipmentId is a GUID, validate format
            if (!Guid.TryParse(shipmentId, out var shipmentGuid))
                return BadRequest("Invalid ShipmentId format.");

            var statusUpdateEntities = await _statusUpdateService.GetAllStatusUpdatesByShipmentIdAsync(shipmentId, trackChanges: false);

            var statusUpdateDtos = _mapper.Map<IEnumerable<StatusUpdateDto>>(statusUpdateEntities);

            return Ok(statusUpdateDtos);
        }

        [HttpGet("{statusUpdateId}")]
        [Authorize(Policy = AuthPolicies.ViewStatusHistoryPolicy)]
        public async Task<IActionResult> GetStatusUpdateById(string statusUpdateId)
        {
            if (string.IsNullOrEmpty(statusUpdateId))
                return BadRequest("Id is required");

            // Validate GUID format
            if (!Guid.TryParse(statusUpdateId, out var statusUpdateGuid))
                return BadRequest("Invalid ShipmentId format.");

            var userId = GetCurrentUserId();
            var userRoles = GetCurrentUserRoles();

            // Check if the user is an admin
            bool isAdmin = userRoles.Contains(UserType.Admin, StringComparer.OrdinalIgnoreCase);

            var statusUpdateEntity = await _statusUpdateService.GetStatusUpdateByIdAsync(statusUpdateId, isAdmin, trackChanges: false, userId);

            var statusUpdateDto = _mapper.Map<StatusUpdateDto>(statusUpdateEntity);

            return Ok(statusUpdateDto);
        }

        [HttpPatch("{statusUpdateId}")]
        [Authorize(Policy = AuthPolicies.UpdateStatusPolicy)]
        public async Task<IActionResult> PatchStatusUpdate(string statusUpdateId, [FromBody] JsonPatchDocument<StatusUpdateDto> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest("Patch document cannot be null.");

            // Validate GUID format
            if (!Guid.TryParse(statusUpdateId, out var statusUpdateGuid))
                return BadRequest("Invalid ShipmentId format.");

            var userId = GetCurrentUserId();
            var userRoles = GetCurrentUserRoles();

            // Check if the user is an admin
            bool isAdmin = userRoles.Contains(UserType.Admin, StringComparer.OrdinalIgnoreCase);

            var statusUpdateToUpdateEntity = await _statusUpdateService.GetStatusUpdateByIdAsync(statusUpdateId, isAdmin, trackChanges: true, userId);

            if (statusUpdateToUpdateEntity == null)
                return NotFound($"StatusUpdate with Id {statusUpdateId} could not be found.");

            // Map entity to DTO
            var statusUpdateToUpdateDto = _mapper.Map<StatusUpdateDto>(statusUpdateToUpdateEntity);

            // apply patch to DTO
            patchDoc.ApplyTo(statusUpdateToUpdateDto, error =>
            {
                ModelState.AddModelError(error.AffectedObject?.ToString() ?? "", error.ErrorMessage);
            });

            // IMPORTANT: After applying the patch, ModelState may contain errors from invalid patch operations.
            // However, ModelState does NOT automatically validate data annotations on the patched DTO.
            // If you want to ensure data annotation validation, call TryValidateModel(shipmentDto) here.
            // Example: TryValidateModel(shipmentDto);

            // This check ensures that no invalid data (from patch ops or model validation) is persisted.
            // If ModelState is invalid, return 422 Unprocessable Entity with error details.
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            // map DTO to entity
            _mapper.Map(statusUpdateToUpdateDto, statusUpdateToUpdateEntity);

            // Save Changes
            await _statusUpdateService.SaveChangesToDbAsync();

            return NoContent();
        }

        [HttpDelete("{statusUpdateId}")]
        [Authorize(Roles = UserType.Admin)]
        public async Task<IActionResult> DeleteStatusUpdateById(string statusUpdateId)
        {
            if (string.IsNullOrEmpty(statusUpdateId))
                return BadRequest("Id is required");

            // Validate GUID format
            if (!Guid.TryParse(statusUpdateId, out var statusUpdateGuid))
                return BadRequest("Invalid ShipmentId format.");

            var userId = GetCurrentUserId();
            var userRoles = GetCurrentUserRoles();

            // Check if the user is an admin
            bool isAdmin = userRoles.Contains(UserType.Admin, StringComparer.OrdinalIgnoreCase);

            await _statusUpdateService.DeleteStatusUpdateById(statusUpdateId, isAdmin, trackChanges: true, userId);

            return NoContent();
        }

        // Helper method for checking if the user inputs Status is correct.
        private static bool IsValidShipmentStatusString(string value)
        {
            return typeof(ShipmentStatus)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(f => f.GetCustomAttribute<EnumMemberAttribute>()?.Value)
                .Any(enumValue => string.Equals(enumValue, value, StringComparison.Ordinal));
        }
    }
}
