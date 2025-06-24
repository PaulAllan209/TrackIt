namespace TrackIt.Server.Authorization
{
    public static class AuthPolicies
    {
        ///<summary>Policy to allow viewing all user records.</summary>
        public const string ViewAllUsersPolicy = "View All Users";

        ///<summary>Policy to allow adding, removing and updating all user records.</summary>
        public const string ManageAllUsersPolicy = "Manage All Users";

        /// <summary>Policy to allow viewing details of all roles.</summary>
        public const string ViewAllRolesPolicy = "View All Roles";

        /// <summary>Policy to allow viewing details of all or specific roles (Requires roleName as parameter).</summary>
        public const string ViewRoleByRoleNamePolicy = "View Role by RoleName";

        /// <summary>Policy to allow adding, removing and updating all roles.</summary>
        public const string ManageAllRolesPolicy = "Manage All Roles";

        /// <summary>Policy to allow assigning roles the user has access to (Requires new and current roles as parameter).</summary>
        public const string AssignAllowedRolesPolicy = "Assign Allowed Roles";

        // Shipment policies
        public const string CreateShipmentPolicy = "Create Shipment";
        public const string ViewShipmentPolicy = "View Shipment";
        public const string UpdateShipmentPolicy = "Update Shipment";
        public const string DeleteShipmentPolicy = "Delete Shipment";

        // Status update policies
        public const string CreateStatusPolicy = "Create Status";
        public const string UpdateStatusPolicy = "Update Status";
        public const string ViewStatusHistoryPolicy = "View Status History";
        public const string DeleteStatusPolicy = "DeleteStatusPolicy";

        // Role-specific policies
        public const string SupplierOperationsPolicy = "Supplier Operations";
        public const string FacilityOperationsPolicy = "Facility Operations";
        public const string DeliveryOperationsPolicy = "Delivery Operations";
        public const string CustomerOperationsPolicy = "Customer Operations";
    }
}
