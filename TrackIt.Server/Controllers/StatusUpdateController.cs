using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TrackIt.Core.Models.Account;
using TrackIt.Core.Models.Shipping;
using TrackIt.Core.Services.Shipping;
using TrackIt.Core.Services.Shipping.Interfaces;
using TrackIt.Server.Attributes;
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
        public async Task<IActionResult> CreateStatusUpdate([FromBody] StatusUpdateForCreationDto statusUpdateForCreationDto)
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity(statusUpdateForCreationDto);

            var statusUpdateEntity = _mapper.Map<StatusUpdate>(statusUpdateForCreationDto);

            var statusUpdateEntityReturned = await _statusUpdateService.CreateStatusUpdateAsync(statusUpdateEntity);

            var statusUpdateEntityDto = _mapper.Map<StatusUpdateDto>(statusUpdateEntityReturned);

            return Ok(statusUpdateEntityReturned);
        }
    }
}
