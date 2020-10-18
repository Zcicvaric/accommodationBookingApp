using Microsoft.EntityFrameworkCore.Migrations;

namespace AccommodationBookingApp.DataAccess.Migrations
{
    public partial class updatedpknamingonAccommodation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Accommodations",
                table: "Accommodations");

            migrationBuilder.DropColumn(
                name: "AccommodationId",
                table: "Accommodations");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Accommodations",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accommodations",
                table: "Accommodations",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Accommodations",
                table: "Accommodations");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Accommodations");

            migrationBuilder.AddColumn<int>(
                name: "AccommodationId",
                table: "Accommodations",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accommodations",
                table: "Accommodations",
                column: "AccommodationId");
        }
    }
}
