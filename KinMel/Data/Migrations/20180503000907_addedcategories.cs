using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace KinMel.Data.Migrations
{
    public partial class addedcategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ScreenSize",
                table: "ClassifiedAd",
                newName: "Mobile_ScreenSize");

            migrationBuilder.RenameColumn(
                name: "Ram",
                table: "ClassifiedAd",
                newName: "Mobile_Ram");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "ClassifiedAd",
                newName: "Computer_Type");

            migrationBuilder.RenameColumn(
                name: "Features",
                table: "ClassifiedAd",
                newName: "Computer_Features");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ISBN",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Features",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Battery",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HDD",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Processor",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProcessorGeneration",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Ram",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SSD",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ScreenSize",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScreenType",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VideoCard",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContractFor",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Salary",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkingDays",
                table: "ClassifiedAd",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "ISBN",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "Features",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "Battery",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "HDD",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "Processor",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "ProcessorGeneration",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "Ram",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "SSD",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "ScreenSize",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "ScreenType",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "VideoCard",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "ContractFor",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "Salary",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "WorkingDays",
                table: "ClassifiedAd");

            migrationBuilder.RenameColumn(
                name: "Mobile_ScreenSize",
                table: "ClassifiedAd",
                newName: "ScreenSize");

            migrationBuilder.RenameColumn(
                name: "Mobile_Ram",
                table: "ClassifiedAd",
                newName: "Ram");

            migrationBuilder.RenameColumn(
                name: "Computer_Type",
                table: "ClassifiedAd",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "Computer_Features",
                table: "ClassifiedAd",
                newName: "Features");
        }
    }
}
