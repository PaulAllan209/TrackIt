using System.ComponentModel.DataAnnotations;
using TrackIt.Core.Extensions;
using TrackIt.Server.Attributes;

namespace TrackIt.Server.Dto.Account
{
    public class RoleDto : ISanitizeModel
    {
        public virtual void SanitizeModel()
        {
            Id = Id.NullIfWhiteSpace();
            Name = Name.NullIfWhiteSpace();
            Description = Description.NullIfWhiteSpace();
        }

        public string? Id { get; set; }

        [Required(ErrorMessage = "Role name is required"),
         StringLength(200, MinimumLength = 2, ErrorMessage = "Role name must be between 2 and 200 characters")]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public int UsersCount { get; set; }

        public PermissionDto[]? Permissions { get; set; }
    }
}
