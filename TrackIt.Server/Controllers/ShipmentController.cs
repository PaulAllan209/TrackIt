using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TrackIt.Core.Interfaces.Repository;
using TrackIt.Core.Models.Account;
using TrackIt.Core.Models.Shipping;
using TrackIt.Core.Models.Shipping.Enums;
using TrackIt.Core.RequestFeatures;
using TrackIt.Core.Services.Shipping.Interfaces;
using TrackIt.Server.Attributes;
using TrackIt.Server.Authorization;
using TrackIt.Server.Dto.TrackIt;
using TrackIt.Server.Services;

namespace TrackIt.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShipmentController : BaseApiController<ShipmentController>
    {
        private readonly IShipmentService _shipmentService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ShipmentController(
            ILogger<ShipmentController> logger,
            IMapper mapper,
            IShipmentService shipmentService,
            UserManager<ApplicationUser> userManager)
            : base(logger, mapper)
        {
            _shipmentService = shipmentService;
            _userManager = userManager;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Policy = AuthPolicies.CreateShipmentPolicy)]
        public async Task<IActionResult> CreateShipment([FromBody] ShipmentForCreationDto shipmentForCreationDto)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var shipmentEntity = _mapper.Map<Shipment>(shipmentForCreationDto);

            // Get supplier id from current logged in context
            shipmentEntity.SupplierId = GetCurrentUserId();

            // Get recipient id from given name
            var recipientUser = await _userManager.FindByNameAsync(shipmentForCreationDto.RecipientName);
            if (recipientUser == null)
            {
                return NotFound("Recipient user not found");
            }

            shipmentEntity.RecipientId = recipientUser.Id;

            var shipmentEntityReturned = await _shipmentService.CreateShipmentAsync(shipmentEntity);

            var shipmentDtoToReturn = _mapper.Map<ShipmentDto>(shipmentEntityReturned);

            return CreatedAtAction(nameof(GetShipmentById),
                new { shipmentId = shipmentDtoToReturn.Id },
                shipmentDtoToReturn);
        }

        [HttpGet]
        [Authorize(Policy = AuthPolicies.ViewShipmentPolicy)]
        public async Task<IActionResult> GetAllShipments([FromQuery] ShipmentParameters shipmentParameters, string? role = null)
        {
            var userId = GetCurrentUserId();
            var userRoles = GetCurrentUserRoles();

            // If role specified, verify user has that role
            if (!string.IsNullOrEmpty(role) && !userRoles.Contains(role))
                return Unauthorized();

            string roleToUse = !string.IsNullOrEmpty(role) ? role : GetHighestPrivilegeRole(userRoles);

            // For admin user you may not need userId but the repository expects it, so pass it anyway
            var pagedResultShipments = await _shipmentService.GetAllShipmentAsync(roleToUse, shipmentParameters, trackChanges: false, userId);

            var shipmentDtos = _mapper.Map<IEnumerable<ShipmentDto>>(pagedResultShipments.shipments);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResultShipments.metaData));

            return Ok(shipmentDtos);
        }

        [HttpGet("{shipmentId}", Name = nameof(GetShipmentById))]
        [Authorize(Policy = AuthPolicies.ViewShipmentPolicy)]
        public async Task<IActionResult> GetShipmentById(string shipmentId)
        {
            if (string.IsNullOrEmpty(shipmentId))
                return BadRequest("Shipment ID cannot be empty.");

            // Validate GUIDs for Id to prevent unnecessary DB calls
            if (!Guid.TryParse(shipmentId, out _))
                return BadRequest("Invalid shipment ID format.");

            var shipment = await _shipmentService.GetShipmentByIdAsync(shipmentId, trackChanges: false);
            var shipmentDto = _mapper.Map<ShipmentDto>(shipment);

            return Ok(shipmentDto);
        }

        [HttpPatch("{id}")]
        [Authorize(Policy = AuthPolicies.UpdateShipmentPolicy)]
        public async Task<IActionResult> PatchShipment(string id, [FromBody] JsonPatchDocument<ShipmentForPatchDto> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest("Patch document cannot be null.");

            var shipmentToUpdateEntity = await _shipmentService.GetShipmentByIdAsync(id, trackChanges: true);

            if (shipmentToUpdateEntity == null)
                return NotFound();

            // Map entity to DTO
            var shipmentForPatchDto = _mapper.Map<ShipmentForPatchDto>(shipmentToUpdateEntity);

            // Apply the patch
            patchDoc.ApplyTo(shipmentForPatchDto, error =>
            {
                ModelState.AddModelError(error.AffectedObject?.ToString() ?? "", error.ErrorMessage);
            });


            // IMPORTANT: After applying the patch, ModelState may contain errors from invalid patch operations.
            // However, ModelState does NOT automatically validate data annotations on the patched DTO.
            // If you want to ensure data annotation validation, call TryValidateModel(shipmentDto) here.
            // Example: TryValidateModel(shipmentDto);

            // This check ensures that no invalid data (from patch ops or model validation) is persisted.
            // If ModelState is invalid, return 422 Unprocessable Entity with error details.
            TryValidateModel(shipmentForPatchDto);
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            // Map DTO back to entity
            _mapper.Map(shipmentForPatchDto, shipmentToUpdateEntity);

            // This function call just calls save changes in the repository layer
            await _shipmentService.UpdateShipmentAsync();

            return NoContent();
        }

        [HttpDelete("{shipmentId}")]
        [Authorize(Policy = AuthPolicies.DeleteShipmentPolicy)]
        public async Task<IActionResult> DeleteShipmentById(string shipmentId)
        {
            if (string.IsNullOrEmpty(shipmentId))
                return BadRequest("Shipment ID cannot be empty.");

            await _shipmentService.DeleteShipmentAsync(shipmentId, trackChanges: true);

            return NoContent();
        }
    }
}
