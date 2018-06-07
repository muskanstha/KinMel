using Microsoft.EntityFrameworkCore.Migrations;

namespace KinMel.Data.Migrations
{
    public partial class somechangesadandnotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ActionId",
                table: "Notification",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<double>(
                name: "DeliveryCharges",
                table: "ClassifiedAd",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "ClassifiedAd",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "ClassifiedAd",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "ClassifiedAd");

            migrationBuilder.AlterColumn<int>(
                name: "ActionId",
                table: "Notification",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "DeliveryCharges",
                table: "ClassifiedAd",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);
        }
    }
}
