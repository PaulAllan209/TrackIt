using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TrackIt.Core.Interfaces.Repository;
using TrackIt.Core.Interfaces.Services;
using TrackIt.Core.Models.Account;
using TrackIt.Core.Models.TrackIt;
using TrackIt.Core.Models.TrackIt.Enums;
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

            return Ok(shipmentDtoToReturn);
        }

        [HttpGet("{role}")]
        [Authorize]
        public async Task<IActionResult> GetAllShipments(string role)
        {
            if (!UserType.AllRoles.Contains(role, StringComparer.OrdinalIgnoreCase))
                return BadRequest("Invalid role specified.");

            var userRoles = GetCurrentUserRoles();
            if (!userRoles.Contains(role, StringComparer.OrdinalIgnoreCase))
            {
                return Unauthorized();
            }

            var userId = GetCurrentUserId();

            // For admin user you may not need userId but the repository expects it, so pass it anyway
            var shipmentEntities = await _shipmentService.GetAllShipmentAsync(role, trackChanges: false, userId);

            var shipmentDtos = _mapper.Map<IEnumerable<ShipmentDto>>(shipmentEntities);

            return Ok(shipmentDtos);
        }
    }
}
