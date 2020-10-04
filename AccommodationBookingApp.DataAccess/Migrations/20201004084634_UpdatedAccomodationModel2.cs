using Microsoft.EntityFrameworkCore.Migrations;

namespace AccommodationBookingApp.DataAccess.Migrations
{
    public partial class UpdatedAccomodationModel2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Accommodations",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

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
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accommodations_AspNetUsers_ApplicationUserId",
                table: "Accommodations");

            migrationBuilder.DropIndex(
                name: "IX_Accommodations_ApplicationUserId",
                table: "Accommodations");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationUserId",
                table: "Accommodations",
                type: "int",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
