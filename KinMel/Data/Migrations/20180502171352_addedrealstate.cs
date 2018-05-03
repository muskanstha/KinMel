using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace KinMel.Data.Migrations
{
    public partial class addedrealstate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Motorcycle_Features",
                table: "ClassifiedAd",
                newName: "RealState_Features");

            migrationBuilder.RenameColumn(
                name: "Mobile_Features",
                table: "ClassifiedAd",
                newName: "Motorcycle_Features");

            migrationBuilder.RenameColumn(
                name: "Features",
                table: "ClassifiedAd",
                newName: "Mobile_Features");

            migrationBuilder.AddColumn<string>(
                name: "Features",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Floors",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Furnishing",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LandSize",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PropertyType",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalRooms",
                table: "ClassifiedAd",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Features",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "Floors",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "Furnishing",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "LandSize",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "PropertyType",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "TotalRooms",
                table: "ClassifiedAd");

            migrationBuilder.RenameColumn(
                name: "RealState_Features",
                table: "ClassifiedAd",
                newName: "Motorcycle_Features");

            migrationBuilder.RenameColumn(
                name: "Motorcycle_Features",
                table: "ClassifiedAd",
                newName: "Mobile_Features");

            migrationBuilder.RenameColumn(
                name: "Mobile_Features",
                table: "ClassifiedAd",
                newName: "Features");
        }
    }
}
