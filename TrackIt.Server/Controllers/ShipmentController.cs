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
        [Authorize(Policy = AuthPolicies.SupplierOperationsPolicy)]
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
                new { id = shipmentDtoToReturn.Id },
                shipmentDtoToReturn);
        }

        [HttpGet]
        [Authorize]
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

        [HttpGet("{id}", Name = nameof(GetShipmentById))]
        [Authorize]
        public async Task<IActionResult> GetShipmentById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Shipment ID cannot be empty.");

            var userId = GetCurrentUserId();
            var userRoles = GetCurrentUserRoles();

            // Get the proper role to use for data access - prioritizing higher privilages
            string roleToUse = GetHighestPrivilegeRole(userRoles);

            var shipment = await _shipmentService.GetShipmentByIdAsync(roleToUse, id, trackChanges: false, userId);
            var shipmentDto = _mapper.Map<ShipmentDto>(shipment);

            return Ok(shipmentDto);
        }

        [HttpPatch("{id}")]
        [Authorize]
        public async Task<IActionResult> PatchShipment(string id, [FromBody] JsonPatchDocument<ShipmentDto> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest("Patch document cannot be null.");

            var userId = GetCurrentUserId();
            var userRoles = GetCurrentUserRoles();
            string roleToUse = GetHighestPrivilegeRole(userRoles);

            var shipmentToUpdateEntity = await _shipmentService.GetShipmentByIdAsync(roleToUse, id, trackChanges: true, userId);

            if (shipmentToUpdateEntity == null)
                return NotFound();

            // Map entity to DTO
            var shipmentDto = _mapper.Map<ShipmentDto>(shipmentToUpdateEntity);

            // Apply the patch
            patchDoc.ApplyTo(shipmentDto, error =>
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

            // Map DTO back to entity
            _mapper.Map(shipmentDto, shipmentToUpdateEntity);

            // This function call just calls save changes in the repository layer
            await _shipmentService.UpdateShipmentAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteShipmentById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Shipment ID cannot be empty.");

            var userId = GetCurrentUserId();
            var userRoles = GetCurrentUserRoles();

            // Get the proper role to use for data access - prioritizing higher privilages
            string roleToUse = GetHighestPrivilegeRole(userRoles);

            await _shipmentService.DeleteShipmentAsync(roleToUse, id, trackChanges: true, userId);

            return NoContent();
        }

        private string GetHighestPrivilegeRole(string[] userRoles)
        {
            if (userRoles.Contains(UserType.Admin, StringComparer.OrdinalIgnoreCase))
                return UserType.Admin;
            if (userRoles.Contains(UserType.Supplier, StringComparer.OrdinalIgnoreCase))
                return UserType.Supplier;
            if (userRoles.Contains(UserType.Facility, StringComparer.OrdinalIgnoreCase))
                return UserType.Facility;
            if (userRoles.Contains(UserType.Delivery, StringComparer.OrdinalIgnoreCase))
                return UserType.Delivery;
            if (userRoles.Contains(UserType.Customer, StringComparer.OrdinalIgnoreCase))
                return UserType.Customer;

            return UserType.Customer; // Default to lowest privilege
        }
    }
}
