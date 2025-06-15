// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TrackIt.Core.Models.Account;
using TrackIt.Core.Services.Account;
using TrackIt.Server.Dto.Account;

namespace TrackIt.Server.Configuration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserDto>()
                   .ForMember(d => d.Roles, map => map.Ignore());
            CreateMap<UserDto, ApplicationUser>()
                .ForMember(d => d.Roles, map => map.Ignore())
                .ForMember(d => d.Id, map => map.Condition(src => src.Id != null));

            CreateMap<ApplicationUser, UserEditDto>()
                .ForMember(d => d.Roles, map => map.Ignore());
            CreateMap<UserEditDto, ApplicationUser>()
                .ForMember(d => d.Roles, map => map.Ignore())
                .ForMember(d => d.Id, map => map.Condition(src => src.Id != null));

            CreateMap<ApplicationUser, UserPatchDto>()
                .ReverseMap();

            CreateMap<ApplicationRole, RoleDto>()
                .ForMember(d => d.Permissions, map => map.MapFrom(s => s.Claims))
                .ForMember(d => d.UsersCount, map => map.MapFrom(s => s.Users != null ? s.Users.Count : 0))
                .ReverseMap();
            CreateMap<RoleDto, ApplicationRole>()
                .ForMember(d => d.Id, map => map.Condition(src => src.Id != null));

            CreateMap<IdentityRoleClaim<string>, ClaimDto>()
                .ForMember(d => d.Type, map => map.MapFrom(s => s.ClaimType))
                .ForMember(d => d.Value, map => map.MapFrom(s => s.ClaimValue))
                .ReverseMap();

            CreateMap<ApplicationPermission, PermissionDto>()
                .ReverseMap();

            CreateMap<IdentityRoleClaim<string>, PermissionDto>()
                .ConvertUsing(s => ((PermissionDto)ApplicationPermissions.GetPermissionByValue(s.ClaimValue))!);
        }
    }
}
