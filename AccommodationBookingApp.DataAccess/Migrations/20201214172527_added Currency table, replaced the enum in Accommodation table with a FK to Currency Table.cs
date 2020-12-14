using Microsoft.EntityFrameworkCore.Migrations;

namespace AccommodationBookingApp.DataAccess.Migrations
{
    public partial class addedCurrencytablereplacedtheenuminAccommodationtablewithaFKtoCurrencyTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Accommodations");

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "Accommodations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accommodations_CurrencyId",
                table: "Accommodations",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accommodations_Currencies_CurrencyId",
                table: "Accommodations",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accommodations_Currencies_CurrencyId",
                table: "Accommodations");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropIndex(
                name: "IX_Accommodations_CurrencyId",
                table: "Accommodations");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "Accommodations");

            migrationBuilder.AddColumn<int>(
                name: "Currency",
                table: "Accommodations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
