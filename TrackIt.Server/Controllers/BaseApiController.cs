using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TrackIt.Core.Models.Shipping.Enums;
using TrackIt.Core.Services.Account;
using TrackIt.Server.Attributes;
using TrackIt.Server.Services;

namespace TrackIt.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SanitizeModel]
    public class BaseApiController<T> : ControllerBase
    {
        protected readonly IMapper _mapper;
        protected readonly ILogger<T> _logger;

        public BaseApiController(ILogger<T> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }

        protected string GetCurrentUserId(string errorMsg = "Error retrieving the userId for the current user.")
        {
            return Utilities.GetUserId(User) ?? throw new UserNotFoundException(errorMsg);
        }

        protected string[] GetCurrentUserRoles(string errorMsg = "Error retrieving the roles for the current user.")
        {
            return Utilities.GetRoles(User) ?? throw new UserRoleException(errorMsg); 
        }

        protected void AddModelError(IEnumerable<string> errors, string key = "")
        {
            foreach (var error in errors)
            {
                AddModelError(error, key);
            }
        }

        protected void AddModelError(string error, string key = "")
        {
            ModelState.AddModelError(key, error);
        }

        protected string GetHighestPrivilegeRole(string[] userRoles)
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
