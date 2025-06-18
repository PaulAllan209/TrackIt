using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackIt.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedShipmentTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Shipments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Shipments");
        }
    }
}
