using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace KinMel.Data.Migrations
{
    public partial class classifiedsv3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<string>(
                name: "BackCamera",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FrontCamera",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneOs",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ram",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScreenSize",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Storage",
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
                name: "BackCamera",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "FrontCamera",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "PhoneOs",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "Ram",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "ScreenSize",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "Storage",
                table: "ClassifiedAd");

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
        }
    }
}
