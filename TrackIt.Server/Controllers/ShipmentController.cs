using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TrackIt.Core.Interfaces.Repository;
using TrackIt.Core.Models.Account;
using TrackIt.Core.Models.Shipping;
using TrackIt.Core.Models.Shipping.Enums;
using TrackIt.Core.Services.Shipping.Interfaces;
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
        [Authorize]
        public async Task<IActionResult> CreateShipment([FromBody] ShipmentForCreationDto shipmentForCreationDto)
        {
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
        public async Task<IActionResult> GetAllShipments([FromQuery] string? role = null)
        {
            var userId = GetCurrentUserId();
            var userRoles = GetCurrentUserRoles();

            // If role specified, verify user has that role
            if (!string.IsNullOrEmpty(role) && !userRoles.Contains(role))
                return Unauthorized();

            string roleToUse = !string.IsNullOrEmpty(role) ? role : GetHighestPrivilegeRole(userRoles);

            // For admin user you may not need userId but the repository expects it, so pass it anyway
            var shipmentEntities = await _shipmentService.GetAllShipmentAsync(roleToUse, trackChanges: false, userId);
            var shipmentDtos = _mapper.Map<IEnumerable<ShipmentDto>>(shipmentEntities);

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
