using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackIt.Server.Migrations
{
    /// <inheritdoc />
    public partial class FixedShipmentsRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shipment_AspNetUsers_ApplicationUserId",
                table: "Shipment");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Shipment",
                newName: "RecipientId");

            migrationBuilder.RenameIndex(
                name: "IX_Shipment_ApplicationUserId",
                table: "Shipment",
                newName: "IX_Shipment_RecipientId");

            migrationBuilder.AlterColumn<string>(
                name: "SupplierId",
                table: "Shipment",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_SupplierId",
                table: "Shipment",
                column: "SupplierId");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shipment_AspNetUsers_RecipientId",
                table: "Shipment");

            migrationBuilder.DropForeignKey(
                name: "FK_Shipment_AspNetUsers_SupplierId",
                table: "Shipment");

            migrationBuilder.DropIndex(
                name: "IX_Shipment_SupplierId",
                table: "Shipment");

            migrationBuilder.RenameColumn(
                name: "RecipientId",
                table: "Shipment",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Shipment_RecipientId",
                table: "Shipment",
                newName: "IX_Shipment_ApplicationUserId");

            migrationBuilder.AlterColumn<string>(
                name: "SupplierId",
                table: "Shipment",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Shipment_AspNetUsers_ApplicationUserId",
                table: "Shipment",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
