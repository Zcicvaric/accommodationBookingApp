using Microsoft.EntityFrameworkCore.Migrations;

namespace AccommodationBookingApp.DataAccess.Migrations
{
    public partial class updatedtheApplicationUserModeltosupportadditionaldataforhostregistration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "AspNetUsers");
        }
    }
}
