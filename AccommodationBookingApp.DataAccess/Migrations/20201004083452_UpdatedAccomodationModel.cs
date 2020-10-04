using Microsoft.EntityFrameworkCore.Migrations;

namespace AccommodationBookingApp.DataAccess.Migrations
{
    public partial class UpdatedAccomodationModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accommodations_AspNetUsers_ApplicationUserId1",
                table: "Accommodations");

            migrationBuilder.DropIndex(
                name: "IX_Accommodations_ApplicationUserId1",
                table: "Accommodations");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "Accommodations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "Accommodations",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accommodations_ApplicationUserId1",
                table: "Accommodations",
                column: "ApplicationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Accommodations_AspNetUsers_ApplicationUserId1",
                table: "Accommodations",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
