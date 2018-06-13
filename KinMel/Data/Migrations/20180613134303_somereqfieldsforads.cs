using Microsoft.EntityFrameworkCore.Migrations;

namespace KinMel.Data.Migrations
{
    public partial class somereqfieldsforads : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "ClassifiedAd",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "ClassifiedAd",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
