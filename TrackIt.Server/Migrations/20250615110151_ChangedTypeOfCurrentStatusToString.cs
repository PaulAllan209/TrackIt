using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackIt.Server.Migrations
{
    /// <inheritdoc />
    public partial class ChangedTypeOfCurrentStatusToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shipment_AspNetUsers_RecipientId",
                table: "Shipment");

            migrationBuilder.DropForeignKey(
                name: "FK_Shipment_AspNetUsers_SupplierId",
                table: "Shipment");

            migrationBuilder.DropForeignKey(
                name: "FK_StatusUpdate_Shipment_ShipmentId",
                table: "StatusUpdate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StatusUpdate",
                table: "StatusUpdate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shipment",
                table: "Shipment");

            migrationBuilder.RenameTable(
                name: "StatusUpdate",
                newName: "StatusUpdates");

            migrationBuilder.RenameTable(
                name: "Shipment",
                newName: "Shipments");

            migrationBuilder.RenameIndex(
                name: "IX_StatusUpdate_ShipmentId",
                table: "StatusUpdates",
                newName: "IX_StatusUpdates_ShipmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Shipment_SupplierId",
                table: "Shipments",
                newName: "IX_Shipments_SupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_Shipment_RecipientId",
                table: "Shipments",
                newName: "IX_Shipments_RecipientId");

            migrationBuilder.AlterColumn<string>(
                name: "CurrentStatus",
                table: "Shipments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StatusUpdates",
                table: "StatusUpdates",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shipments",
                table: "Shipments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Shipments_AspNetUsers_RecipientId",
                table: "Shipments",
                column: "RecipientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Shipments_AspNetUsers_SupplierId",
                table: "Shipments",
                column: "SupplierId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StatusUpdates_Shipments_ShipmentId",
                table: "StatusUpdates",
                column: "ShipmentId",
                principalTable: "Shipments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shipments_AspNetUsers_RecipientId",
                table: "Shipments");

            migrationBuilder.DropForeignKey(
                name: "FK_Shipments_AspNetUsers_SupplierId",
                table: "Shipments");

            migrationBuilder.DropForeignKey(
                name: "FK_StatusUpdates_Shipments_ShipmentId",
                table: "StatusUpdates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StatusUpdates",
                table: "StatusUpdates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shipments",
                table: "Shipments");

            migrationBuilder.RenameTable(
                name: "StatusUpdates",
                newName: "StatusUpdate");

            migrationBuilder.RenameTable(
                name: "Shipments",
                newName: "Shipment");

            migrationBuilder.RenameIndex(
                name: "IX_StatusUpdates_ShipmentId",
                table: "StatusUpdate",
                newName: "IX_StatusUpdate_ShipmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Shipments_SupplierId",
                table: "Shipment",
                newName: "IX_Shipment_SupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_Shipments_RecipientId",
                table: "Shipment",
                newName: "IX_Shipment_RecipientId");

            migrationBuilder.AlterColumn<int>(
                name: "CurrentStatus",
                table: "Shipment",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StatusUpdate",
                table: "StatusUpdate",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shipment",
                table: "Shipment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Shipment_AspNetUsers_RecipientId",
                table: "Shipment",
                column: "RecipientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Shipment_AspNetUsers_SupplierId",
                table: "Shipment",
                column: "SupplierId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StatusUpdate_Shipment_ShipmentId",
                table: "StatusUpdate",
                column: "ShipmentId",
                principalTable: "Shipment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
