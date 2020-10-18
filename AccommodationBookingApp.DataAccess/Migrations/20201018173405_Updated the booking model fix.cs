using Microsoft.EntityFrameworkCore.Migrations;

namespace AccommodationBookingApp.DataAccess.Migrations
{
    public partial class Updatedthebookingmodelfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "approvalStatus",
                table: "Bookings",
                newName: "ApprovalStatus");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ApprovalStatus",
                table: "Bookings",
                newName: "approvalStatus");
        }
    }
}
