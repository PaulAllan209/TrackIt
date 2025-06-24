// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TrackIt.Core.Models.Account;
using TrackIt.Core.Models.Shipping;
using TrackIt.Core.Models.Shipping.Enums;
using TrackIt.Core.Services.Account;
using TrackIt.Server.Dto.Account;
using TrackIt.Server.Dto.TrackIt;

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

            CreateMap<ApplicationUser, UserRegisterDto>()
                .ForMember(d => d.Roles, map => map.Ignore());
            CreateMap<UserRegisterDto, ApplicationUser>()
                .ForMember(d => d.Roles, map => map.Ignore()) // Roles are passed by parameters in the service layer
                .ForMember(d => d.Id, map => map.Ignore()); // Just let IdentityUser from aspnetcore library handle Id generation


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

            /************************** SHIPMENT **************************/
            CreateMap<ShipmentDto, Shipment>()
                .ForMember(dest => dest.CurrentStatus, opt => opt.MapFrom(src => Enum.Parse<ShipmentStatus>(src.CurrentStatus)))
                .ReverseMap();

            CreateMap<ShipmentForCreationDto, Shipment>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.CurrentStatus, opt => opt.MapFrom(src => ShipmentStatus.ToShip))
                .ForMember(dest => dest.RecipientAddress, opt => opt.MapFrom(src => src.RecipientAddress))

                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))

                // Set timestamps
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))

                // Ignore navigation properties (handled by EF)
                .ForMember(dest => dest.Supplier, opt => opt.Ignore())
                .ForMember(dest => dest.Recipient, opt => opt.Ignore())
                .ForMember(dest => dest.StatusUpdates, opt => opt.Ignore())

                // Ignore optional fields that aren't in the DTO
                .ForMember(dest => dest.DeliveredAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());

            CreateMap<ShipmentForPatchDto, Shipment>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Shipment, ShipmentForPatchDto>();

            /************************** STATUS UPDATE **************************/
            CreateMap<StatusUpdate, StatusUpdateDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<StatusUpdateDto, StatusUpdate>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<ShipmentStatus>(src.Status)))
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Prevent overwriting the guid
                .ForMember(dest => dest.Shipment, opt => opt.Ignore()); // Ignore navigation property

            CreateMap<StatusUpdateForCreationDto, StatusUpdate>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<ShipmentStatus>(src.Status, true)));

            CreateMap<StatusUpdate, StatusUpdateForPatchDto>()
                .ForMember(dest => dest.ShipmentId, opt => opt.MapFrom(src => src.ShipmentId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ReverseMap();
        }
    }
}
