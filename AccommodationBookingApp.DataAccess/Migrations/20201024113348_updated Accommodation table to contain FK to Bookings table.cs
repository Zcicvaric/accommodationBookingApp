using Microsoft.EntityFrameworkCore.Migrations;

namespace AccommodationBookingApp.DataAccess.Migrations
{
    public partial class updatedAccommodationtabletocontainFKtoBookingstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Bookings_AccommodationId",
                table: "Bookings",
                column: "AccommodationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Accommodations_AccommodationId",
                table: "Bookings",
                column: "AccommodationId",
                principalTable: "Accommodations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Accommodations_AccommodationId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_AccommodationId",
                table: "Bookings");
        }
    }
}
