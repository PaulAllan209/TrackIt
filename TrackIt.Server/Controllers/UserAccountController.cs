﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TrackIt.Core.Models.Account;
using TrackIt.Core.Models.Shipping.Enums;
using TrackIt.Core.Services.Account;
using TrackIt.Server.Authorization;
using TrackIt.Server.Dto.Account;

namespace TrackIt.Server.Controllers
{
    [Route("api/account")]
    [Authorize]
    public class UserAccountController : BaseApiController<UserAccountController>
    {
        private readonly IUserAccountService _userAccountService;

        public UserAccountController(ILogger<UserAccountController> logger, IMapper mapper,
            IUserAccountService userAccountService, IAuthorizationService authorizationService) : base(logger, mapper)
        {
            _userAccountService = userAccountService;
        }

        [HttpGet("users/me")]
        [Authorize(AuthPolicies.ViewAllUsersPolicy)]
        [ProducesResponseType(200, Type = typeof(UserDto))]
        public async Task<IActionResult> GetCurrentUser()
        {
            return await GetUserById(GetCurrentUserId());
        }

        [HttpGet("users/{id}", Name = nameof(GetUserById))]
        [Authorize(AuthPolicies.ViewAllUsersPolicy)]
        [ProducesResponseType(200, Type = typeof(UserDto))]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUserById(string id)
        {
            var userVM = await GetUserViewModelHelper(id);

            if (userVM != null)
                return Ok(userVM);

            return NotFound(id);
        }

        [HttpGet("users/username/{userName}")]
        [Authorize(AuthPolicies.ViewAllUsersPolicy)]
        [ProducesResponseType(200, Type = typeof(UserDto))]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUserByUserName(string userName)
        {
            var appUser = await _userAccountService.GetUserByUserNameAsync(userName);

            var userVM = appUser != null ? await GetUserViewModelHelper(appUser.Id) : null;

            if (userVM != null)
                return Ok(userVM);

            return NotFound(userName);
        }

        [HttpGet("users")]
        [Authorize(AuthPolicies.ViewAllUsersPolicy)]
        [ProducesResponseType(200, Type = typeof(List<UserDto>))]
        public async Task<IActionResult> GetUsers()
        {
            return await GetUsers(-1, -1);
        }

        [HttpGet("users/{pageNumber:int}/{pageSize:int}")]
        [Authorize(AuthPolicies.ViewAllUsersPolicy)]
        [ProducesResponseType(200, Type = typeof(List<UserDto>))]
        public async Task<IActionResult> GetUsers(int pageNumber, int pageSize)
        {
            var usersAndRoles = await _userAccountService.GetUsersAndRolesAsync(pageNumber, pageSize);

            var usersVM = new List<UserDto>();

            foreach (var item in usersAndRoles)
            {
                var userVM = _mapper.Map<UserDto>(item.User);
                userVM.Roles = item.Roles;

                usersVM.Add(userVM);
            }

            return Ok(usersVM);
        }

        [HttpPut("users/me")]
        [Authorize(AuthPolicies.ManageAllUsersPolicy)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> UpdateCurrentUser([FromBody] UserEditDto user)
        {
            var userId = GetCurrentUserId($"Error retrieving the userId for user \"{user.UserName}\".");
            return await UpdateUser(userId, user);
        }

        [HttpPut("users/{id}")]
        [Authorize(AuthPolicies.ManageAllUsersPolicy)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserEditDto user)
        {
            var appUser = await _userAccountService.GetUserByIdAsync(id);
            var currentRoles = appUser != null
                ? (await _userAccountService.GetUserRolesAsync(appUser)).ToArray() : null;

            if (appUser == null)
                return NotFound(id);

            if (!string.IsNullOrWhiteSpace(user.Id) && id != user.Id)
                AddModelError("Conflicting user id in parameter and model data.", nameof(id));

            var isNewPassword = !string.IsNullOrWhiteSpace(user.NewPassword);
            var isNewUserName = !appUser.UserName!.Equals(user.UserName, StringComparison.OrdinalIgnoreCase);

            if (GetCurrentUserId() == id)
            {
                if (string.IsNullOrWhiteSpace(user.CurrentPassword))
                {
                    if (isNewPassword)
                        AddModelError("Current password is required when changing your own password.", "Password");

                    if (isNewUserName)
                        AddModelError("Current password is required when changing your own username.", "Username");
                }
                else if (isNewPassword || isNewUserName)
                {
                    if (!await _userAccountService.CheckPasswordAsync(appUser, user.CurrentPassword))
                        AddModelError("The username/password couple is invalid.");
                }
            }

            if (ModelState.IsValid)
            {
                _mapper.Map(user, appUser);

                var result = await _userAccountService.UpdateUserAsync(appUser, user.Roles);

                if (result.Succeeded)
                {
                    if (isNewPassword)
                    {
                        if (!string.IsNullOrWhiteSpace(user.CurrentPassword))
                            result = await _userAccountService.UpdatePasswordAsync(appUser, user.CurrentPassword,
                                user.NewPassword!);
                        else
                            result = await _userAccountService.ResetPasswordAsync(appUser, user.NewPassword!);
                    }

                    if (result.Succeeded)
                        return NoContent();
                }

                AddModelError(result.Errors);
            }

            return BadRequest(ModelState);
        }

        [HttpPatch("users/me")]
        [Authorize(AuthPolicies.ManageAllUsersPolicy)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateCurrentUser([FromBody] JsonPatchDocument<UserPatchDto> patch)
        {
            return await UpdateUser(GetCurrentUserId(), patch);
        }

        [HttpPatch("users/{id}")]
        [Authorize(AuthPolicies.ManageAllUsersPolicy)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] JsonPatchDocument<UserPatchDto> patch)
        {
            var appUser = await _userAccountService.GetUserByIdAsync(id);
            if (appUser == null)
                return NotFound(id);

            var userPVM = _mapper.Map<UserPatchDto>(appUser);
            patch.ApplyTo(userPVM, e => AddModelError(e.ErrorMessage));

            if (ModelState.IsValid)
            {
                _mapper.Map(userPVM, appUser);

                var result = await _userAccountService.UpdateUserAsync(appUser);

                if (result.Succeeded)
                    return NoContent();

                AddModelError(result.Errors);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("users")]
        [Authorize(AuthPolicies.ManageAllUsersPolicy)]
        [ProducesResponseType(201, Type = typeof(UserDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto user)
        {
            if (string.IsNullOrWhiteSpace(user.Password))
                AddModelError($"{nameof(user.Password)} is required when registering a new user.",
                    nameof(user.Password));

            if (user.Roles == null)
                AddModelError($"{nameof(user.Roles)} is required when registering a new user.", nameof(user.Roles));

            if (ModelState.IsValid)
            {
                var appUser = _mapper.Map<ApplicationUser>(user);
                var result = await _userAccountService.CreateUserAsync(appUser, user.Roles!, user.Password!);

                if (result.Succeeded)
                {
                    var userVM = await GetUserViewModelHelper(appUser.Id);
                    return CreatedAtAction(nameof(GetUserById), new { id = userVM?.Id }, userVM);
                }

                AddModelError(result.Errors);
            }

            return BadRequest(ModelState);
        }

        // Endpoint for registering users
        [HttpPost("users/RegisterPublic")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterPublic([FromBody] UserRegisterDto user)
        {
            if (string.IsNullOrWhiteSpace(user.Password))
                AddModelError($"{nameof(user.Password)} is required when registering a new user.",
                    nameof(user.Password));

            if (user.Roles == null)
                AddModelError($"{nameof(user.Roles)} is required when registering a new user.", nameof(user.Roles));

            if (user.Roles != null)
            {
                if (user.Roles.Contains(UserType.Admin, StringComparer.OrdinalIgnoreCase))
                    AddModelError("Registering as administrator is forbidden.");
            }

            if (ModelState.IsValid)
            {
                var appUser = _mapper.Map<ApplicationUser>(user);
                var result = await _userAccountService.CreateUserAsync(appUser, user.Roles!, user.Password!);

                if (result.Succeeded)
                {
                    var userVM = await GetUserViewModelHelper(appUser.Id);
                    return CreatedAtAction(nameof(GetUserById), new { id = userVM?.Id }, userVM);
                }

                AddModelError(result.Errors);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("users/batch")]
        [Authorize(AuthPolicies.ManageAllUsersPolicy)]
        [ProducesResponseType(201, Type = typeof(List<UserDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> RegisterBatch([FromBody] List<UserRegisterDto> users)
        {
            var createdUsers = new List<UserDto>();

            foreach (var user in users)
            {
                if (string.IsNullOrWhiteSpace(user.Password))
                {
                    AddModelError($"{nameof(user.Password)} is required when registering a new user.", nameof(user.Password));
                    continue;
                }

                if (user.Roles == null)
                {
                    AddModelError($"{nameof(user.Roles)} is required when registering a new user.", nameof(user.Roles));
                    continue;
                }

                if (!ModelState.IsValid)
                    continue;

                var appUser = _mapper.Map<ApplicationUser>(user);
                var result = await _userAccountService.CreateUserAsync(appUser, user.Roles!, user.Password!);

                if (result.Succeeded)
                {
                    var userDto = await GetUserViewModelHelper(appUser.Id);
                    if (userDto != null)
                        createdUsers.Add(userDto);
                }
                else
                {
                    AddModelError(result.Errors);
                }
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return CreatedAtAction(nameof(GetUsers), createdUsers);
        }



        [HttpDelete("users/{id}")]
        [Authorize(AuthPolicies.ManageAllUsersPolicy)]
        [ProducesResponseType(200, Type = typeof(UserDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var appUser = await _userAccountService.GetUserByIdAsync(id);

            if (appUser == null)
                return NotFound(id);

            var canDelete = await _userAccountService.TestCanDeleteUserAsync(id);
            if (!canDelete.Success)
            {
                AddModelError($"User \"{appUser.UserName}\" cannot be deleted at this time. " +
                    "Delete the associated records and try again.");
                AddModelError(canDelete.Errors, "Records");
            }

            if (ModelState.IsValid)
            {
                var userVM = await GetUserViewModelHelper(appUser.Id);
                var result = await _userAccountService.DeleteUserAsync(appUser);

                if (!result.Succeeded)
                {
                    throw new UserAccountException($"The following errors occurred whilst deleting user \"{id}\": " +
                        $"{string.Join(", ", result.Errors)}");
                }

                return Ok(userVM);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("users/unblock/{id}")]
        [Authorize(AuthPolicies.ManageAllUsersPolicy)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UnblockUser(string id)
        {
            var appUser = await _userAccountService.GetUserByIdAsync(id);

            if (appUser == null)
                return NotFound(id);

            appUser.LockoutEnd = null;
            var result = await _userAccountService.UpdateUserAsync(appUser);

            if (!result.Succeeded)
            {
                throw new UserAccountException($"The following errors occurred whilst unblocking user: " +
                    $"{string.Join(", ", result.Errors)}");
            }

            return NoContent();
        }

        [HttpGet("users/me/preferences")]
        [Authorize(AuthPolicies.ViewAllUsersPolicy)]
        [ProducesResponseType(200, Type = typeof(string))]
        public async Task<IActionResult> UserPreferences()
        {
            var userId = GetCurrentUserId();

            var appUser = await _userAccountService.GetUserByIdAsync(userId);
            if (appUser != null)
                return Ok(appUser.Configuration);

            return NotFound(userId);
        }

        [HttpPut("users/me/preferences")]
        [Authorize(AuthPolicies.ViewAllUsersPolicy)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UserPreferences([FromBody] string? data)
        {
            var userId = GetCurrentUserId();
            var appUser = await _userAccountService.GetUserByIdAsync(userId);

            if (appUser != null)
            {
                appUser.Configuration = data;
                var result = await _userAccountService.UpdateUserAsync(appUser);

                if (!result.Succeeded)
                {
                    throw new UserAccountException(
                        $"The following errors occurred whilst updating User Configurations: " +
                        $"{string.Join(", ", result.Errors)}");
                }

                return NoContent();
            }

            return NotFound(userId);
        }

        private async Task<UserDto?> GetUserViewModelHelper(string userId)
        {
            var userAndRoles = await _userAccountService.GetUserAndRolesAsync(userId);
            if (userAndRoles == null)
                return null;

            var userVM = _mapper.Map<UserDto>(userAndRoles.Value.User);
            userVM.Roles = userAndRoles.Value.Roles;

            return userVM;
        }
    }
}
