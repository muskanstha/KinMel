using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace KinMel.Data.Migrations
{
    public partial class addedMotorcycle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Mobile_ModelNo",
                table: "ClassifiedAd",
                newName: "Motorcycle_ModelNo");

            migrationBuilder.RenameColumn(
                name: "Mobile_Features",
                table: "ClassifiedAd",
                newName: "Motorcycle_Features");

            migrationBuilder.RenameColumn(
                name: "Mobile_Color",
                table: "ClassifiedAd",
                newName: "Motorcycle_Color");

            migrationBuilder.RenameColumn(
                name: "Mobile_Brand",
                table: "ClassifiedAd",
                newName: "Motorcycle_Brand");

            migrationBuilder.RenameColumn(
                name: "TotalKm",
                table: "ClassifiedAd",
                newName: "Motorcycle_TotalKm");

            migrationBuilder.RenameColumn(
                name: "ModelNo",
                table: "ClassifiedAd",
                newName: "Mobile_ModelNo");

            migrationBuilder.RenameColumn(
                name: "Features",
                table: "ClassifiedAd",
                newName: "Mobile_Features");

            migrationBuilder.RenameColumn(
                name: "Color",
                table: "ClassifiedAd",
                newName: "Mobile_Color");

            migrationBuilder.RenameColumn(
                name: "Brand",
                table: "ClassifiedAd",
                newName: "Mobile_Brand");

            migrationBuilder.AlterColumn<string>(
                name: "Motorcycle_TotalKm",
                table: "ClassifiedAd",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Features",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModelNo",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalKm",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Engine",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "MadeYear",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Mileage",
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
                name: "Features",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "ModelNo",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "TotalKm",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "Engine",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "MadeYear",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "Mileage",
                table: "ClassifiedAd");

            migrationBuilder.RenameColumn(
                name: "Motorcycle_TotalKm",
                table: "ClassifiedAd",
                newName: "TotalKm");

            migrationBuilder.RenameColumn(
                name: "Motorcycle_ModelNo",
                table: "ClassifiedAd",
                newName: "Mobile_ModelNo");

            migrationBuilder.RenameColumn(
                name: "Motorcycle_Features",
                table: "ClassifiedAd",
                newName: "Mobile_Features");

            migrationBuilder.RenameColumn(
                name: "Motorcycle_Color",
                table: "ClassifiedAd",
                newName: "Mobile_Color");

            migrationBuilder.RenameColumn(
                name: "Motorcycle_Brand",
                table: "ClassifiedAd",
                newName: "Mobile_Brand");

            migrationBuilder.RenameColumn(
                name: "Mobile_ModelNo",
                table: "ClassifiedAd",
                newName: "ModelNo");

            migrationBuilder.RenameColumn(
                name: "Mobile_Features",
                table: "ClassifiedAd",
                newName: "Features");

            migrationBuilder.RenameColumn(
                name: "Mobile_Color",
                table: "ClassifiedAd",
                newName: "Color");

            migrationBuilder.RenameColumn(
                name: "Mobile_Brand",
                table: "ClassifiedAd",
                newName: "Brand");

            migrationBuilder.AlterColumn<int>(
                name: "TotalKm",
                table: "ClassifiedAd",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
