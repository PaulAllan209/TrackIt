﻿using System.Collections.ObjectModel;
using TrackIt.Core.Models.Account;

namespace TrackIt.Core.Services.Account
{
    public static class ApplicationPermissions
    {
        /************* USER PERMISSIONS *************/

        public const string UsersPermissionGroupName = "User Permissions";

        public static readonly ApplicationPermission ViewUsers = new(
            "View Users",
            "users.view",
            UsersPermissionGroupName,
            "Permission to view other users account details");

        public static readonly ApplicationPermission ManageUsers = new(
            "Manage Users",
            "users.manage",
            UsersPermissionGroupName,
            "Permission to create, delete and modify other users account details");

        /************* ROLE PERMISSIONS *************/

        public const string RolesPermissionGroupName = "Role Permissions";

        public static readonly ApplicationPermission ViewRoles = new(
            "View Roles",
            "roles.view",
            RolesPermissionGroupName,
            "Permission to view available roles");

        public static readonly ApplicationPermission ManageRoles = new(
            "Manage Roles",
            "roles.manage",
            RolesPermissionGroupName,
            "Permission to create, delete and modify roles");

        public static readonly ApplicationPermission AssignRoles = new(
            "Assign Roles",
            "roles.assign",
            RolesPermissionGroupName,
            "Permission to assign roles to users");

        /************* SHIPMENT PERMISSIONS *************/
        public const string ShipmentPermissionGroupName = "Shipment Permissions";

        public static readonly ApplicationPermission CreateShipment = new(
            "Create Shipment",
            "shipments.create",
            ShipmentPermissionGroupName,
            "Permission to create new shipments");

        public static readonly ApplicationPermission ViewShipment = new(
            "View Shipment",
            "shipments.view",
            ShipmentPermissionGroupName,
            "Permission to view shipment details");

        public static readonly ApplicationPermission UpdateShipment = new(
            "Update Shipment",
            "shipments.update",
            ShipmentPermissionGroupName,
            "Permission to modify shipment details");

        public static readonly ApplicationPermission DeleteShipment = new(
            "Delete Shipment",
            "shipments.delete",
            ShipmentPermissionGroupName,
            "Permission to delete shipments");

        /************* STATUS UPDATE PERMISSIONS *************/
        public const string StatusUpdatePermissionGroupName = "Status Update Permissions";

        public static readonly ApplicationPermission CreateStatus = new(
            "Create Status",
            "status.create",
            StatusUpdatePermissionGroupName,
            "Permission to create shipment status");

        public static readonly ApplicationPermission UpdateStatus = new(
            "Update Status",
            "status.update",
            StatusUpdatePermissionGroupName,
            "Permission to update shipment status");

        public static readonly ApplicationPermission ViewStatusHistory = new(
            "View Status History",
            "status.history.view",
            StatusUpdatePermissionGroupName,
            "Permission to view status history of shipments");

        public static readonly ApplicationPermission SetStatusDelivered = new(
            "Set Status to Delivered",
            "status.set.delivered",
            StatusUpdatePermissionGroupName,
            "Permission to mark shipments as delivered only");

        /************* ALL PERMISSIONS *************/

        public static readonly ReadOnlyCollection<ApplicationPermission> AllPermissions =
            new List<ApplicationPermission> {
                ViewUsers, ManageUsers,
                ViewRoles, ManageRoles, AssignRoles,
                CreateShipment, ViewShipment, UpdateShipment, DeleteShipment,
                CreateStatus, UpdateStatus, ViewStatusHistory, SetStatusDelivered
            }.AsReadOnly();

        /************* HELPER METHODS *************/

        public static ApplicationPermission? GetPermissionByName(string? permissionName)
        {
            return AllPermissions.SingleOrDefault(p => p.Name == permissionName);
        }

        public static ApplicationPermission? GetPermissionByValue(string? permissionValue)
        {
            return AllPermissions.SingleOrDefault(p => p.Value == permissionValue);
        }

        public static string[] GetAllPermissionValues()
        {
            return AllPermissions.Select(p => p.Value).ToArray();
        }

        public static string[] GetAdministrativePermissionValues()
        {
            return [ManageUsers, ManageRoles, AssignRoles];
        }
    }
}
