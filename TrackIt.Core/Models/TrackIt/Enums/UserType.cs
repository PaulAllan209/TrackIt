using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackIt.Core.Models.TrackIt.Enums
{
    public static class UserType
    {
        public const string Admin = "Admin";
        public const string Supplier = "Supplier";
        public const string Facility = "Facility";
        public const string Delivery = "Delivery";
        public const string Customer = "Customer";

        public static readonly string[] AllRoles = new[] { Admin, Supplier, Facility, Delivery, Customer };
    }
}
