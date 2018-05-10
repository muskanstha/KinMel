using Microsoft.EntityFrameworkCore.Migrations;

namespace KinMel.Data.Migrations
{
    public partial class modifyrating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RateByFirstName",
                table: "Rating");

            migrationBuilder.AlterColumn<string>(
                name: "RatedById",
                table: "Rating",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rating_RatedById",
                table: "Rating",
                column: "RatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_AspNetUsers_RatedById",
                table: "Rating",
                column: "RatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rating_AspNetUsers_RatedById",
                table: "Rating");

            migrationBuilder.DropIndex(
                name: "IX_Rating_RatedById",
                table: "Rating");

            migrationBuilder.AlterColumn<string>(
                name: "RatedById",
                table: "Rating",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RateByFirstName",
                table: "Rating",
                nullable: true);
        }
    }
}
