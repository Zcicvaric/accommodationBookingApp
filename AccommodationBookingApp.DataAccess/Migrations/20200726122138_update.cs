using Microsoft.EntityFrameworkCore.Migrations;

namespace AccommodationBookingApp.DataAccess.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accommodations_AspNetUsers_UserId",
                table: "Accommodations");

            migrationBuilder.DropIndex(
                name: "IX_Accommodations_UserId",
                table: "Accommodations");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Accommodations");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Accommodations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accommodations_ApplicationUserId",
                table: "Accommodations",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accommodations_AspNetUsers_ApplicationUserId",
                table: "Accommodations",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accommodations_AspNetUsers_ApplicationUserId",
                table: "Accommodations");

            migrationBuilder.DropIndex(
                name: "IX_Accommodations_ApplicationUserId",
                table: "Accommodations");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Accommodations");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Accommodations",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accommodations_UserId",
                table: "Accommodations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accommodations_AspNetUsers_UserId",
                table: "Accommodations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
