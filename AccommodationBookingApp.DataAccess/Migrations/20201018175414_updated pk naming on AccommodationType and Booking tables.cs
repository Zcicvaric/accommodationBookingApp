using Microsoft.EntityFrameworkCore.Migrations;

namespace AccommodationBookingApp.DataAccess.Migrations
{
    public partial class updatedpknamingonAccommodationTypeandBookingtables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accommodations_AccommodationType_AccommodationTypeId",
                table: "Accommodations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccommodationType",
                table: "AccommodationType");

            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "AccommodationTypeId",
                table: "AccommodationType");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Bookings",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "AccommodationType",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccommodationType",
                table: "AccommodationType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Accommodations_AccommodationType_AccommodationTypeId",
                table: "Accommodations",
                column: "AccommodationTypeId",
                principalTable: "AccommodationType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accommodations_AccommodationType_AccommodationTypeId",
                table: "Accommodations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccommodationType",
                table: "AccommodationType");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AccommodationType");

            migrationBuilder.AddColumn<int>(
                name: "BookingId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "AccommodationTypeId",
                table: "AccommodationType",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings",
                column: "BookingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccommodationType",
                table: "AccommodationType",
                column: "AccommodationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accommodations_AccommodationType_AccommodationTypeId",
                table: "Accommodations",
                column: "AccommodationTypeId",
                principalTable: "AccommodationType",
                principalColumn: "AccommodationTypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
