using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace KinMel.Data.Migrations
{
    public partial class imageurlsstring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ClassifiedAd",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrls",
                table: "ClassifiedAd",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrls",
                table: "ClassifiedAd");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ClassifiedAd",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
