using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace parking_manager.Migrations
{
    /// <inheritdoc />
    public partial class AddParkingSpotType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SpotType",
                table: "ParkingSpots",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpotType",
                table: "ParkingSpots");
        }
    }
}
