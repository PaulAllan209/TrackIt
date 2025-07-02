using System.Diagnostics.CodeAnalysis;
using TrackIt.Core.Models.Account;

namespace TrackIt.Server.Dto.Account
{
    public class PermissionDto
    {
        public string? Name { get; set; }
        public string? Value { get; set; }
        public string? GroupName { get; set; }
        public string? Description { get; set; }

        [return: NotNullIfNotNull(nameof(permission))]
        public static explicit operator PermissionDto?(ApplicationPermission? permission)
        {
            if (permission == null)
                return null;

            return new PermissionDto
            {
                Name = permission.Name,
                Value = permission.Value,
                GroupName = permission.GroupName,
                Description = permission.Description
            };
        }
    }
}
