using Microsoft.EntityFrameworkCore.Migrations;

namespace AccommodationBookingApp.DataAccess.Migrations
{
    public partial class updatedtableswithnewcolumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "requireApproval",
                table: "Accommodations",
                newName: "RequireApproval");

            migrationBuilder.AddColumn<int>(
                name: "AccommodationTypeId",
                table: "Accommodations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PricePerNight",
                table: "Accommodations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Accommodations",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AccommodationType",
                columns: table => new
                {
                    AccommodationTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccommodationType", x => x.AccommodationTypeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accommodations_AccommodationTypeId",
                table: "Accommodations",
                column: "AccommodationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Accommodations_UserId",
                table: "Accommodations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accommodations_AccommodationType_AccommodationTypeId",
                table: "Accommodations",
                column: "AccommodationTypeId",
                principalTable: "AccommodationType",
                principalColumn: "AccommodationTypeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Accommodations_Users_UserId",
                table: "Accommodations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accommodations_AccommodationType_AccommodationTypeId",
                table: "Accommodations");

            migrationBuilder.DropForeignKey(
                name: "FK_Accommodations_Users_UserId",
                table: "Accommodations");

            migrationBuilder.DropTable(
                name: "AccommodationType");

            migrationBuilder.DropIndex(
                name: "IX_Accommodations_AccommodationTypeId",
                table: "Accommodations");

            migrationBuilder.DropIndex(
                name: "IX_Accommodations_UserId",
                table: "Accommodations");

            migrationBuilder.DropColumn(
                name: "AccommodationTypeId",
                table: "Accommodations");

            migrationBuilder.DropColumn(
                name: "PricePerNight",
                table: "Accommodations");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Accommodations");

            migrationBuilder.RenameColumn(
                name: "RequireApproval",
                table: "Accommodations",
                newName: "requireApproval");
        }
    }
}
