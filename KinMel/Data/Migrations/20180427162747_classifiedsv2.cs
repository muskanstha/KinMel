using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace KinMel.Data.Migrations
{
    public partial class classifiedsv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "ClassifiedAd",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DoorsNo",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Features",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FuelType",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModelNo",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModelYear",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalKm",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "ClassifiedAd",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brand",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "DoorsNo",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "Features",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "FuelType",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "ModelNo",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "ModelYear",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "TotalKm",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "ClassifiedAd");

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "ClassifiedAd",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
