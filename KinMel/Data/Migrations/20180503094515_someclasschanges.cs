using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace KinMel.Data.Migrations
{
    public partial class someclasschanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MadeYear",
                table: "ClassifiedAd");

            migrationBuilder.RenameColumn(
                name: "Mileage",
                table: "ClassifiedAd",
                newName: "Motorcycle_Mileage");

            migrationBuilder.RenameColumn(
                name: "Engine",
                table: "ClassifiedAd",
                newName: "Motorcycle_Engine");

            migrationBuilder.RenameColumn(
                name: "Motorcycle_Color",
                table: "ClassifiedAd",
                newName: "TabletsAndIPads_Color");

            migrationBuilder.RenameColumn(
                name: "Motorcycle_Brand",
                table: "ClassifiedAd",
                newName: "TabletsAndIPads_Brand");

            migrationBuilder.RenameColumn(
                name: "Storage",
                table: "ClassifiedAd",
                newName: "TabletsAndIPads_Storage");

            migrationBuilder.RenameColumn(
                name: "Mobile_ScreenSize",
                table: "ClassifiedAd",
                newName: "TabletsAndIPads_ScreenSize");

            migrationBuilder.RenameColumn(
                name: "Mobile_Ram",
                table: "ClassifiedAd",
                newName: "TabletsAndIPads_Ram");

            migrationBuilder.RenameColumn(
                name: "PhoneOs",
                table: "ClassifiedAd",
                newName: "TabletsAndIPads_PhoneOs");

            migrationBuilder.RenameColumn(
                name: "FrontCamera",
                table: "ClassifiedAd",
                newName: "TabletsAndIPads_FrontCamera");

            migrationBuilder.RenameColumn(
                name: "Mobile_Color",
                table: "ClassifiedAd",
                newName: "Motorcycle_Color");

            migrationBuilder.RenameColumn(
                name: "Mobile_Brand",
                table: "ClassifiedAd",
                newName: "Motorcycle_Brand");

            migrationBuilder.RenameColumn(
                name: "BackCamera",
                table: "ClassifiedAd",
                newName: "TabletsAndIPads_BackCamera");

            migrationBuilder.RenameColumn(
                name: "Computer_Type",
                table: "ClassifiedAd",
                newName: "Motorcycle_Type");

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
                name: "ModelYear",
                table: "ClassifiedAd",
                newName: "Motorcycle_ModelYear");

            migrationBuilder.RenameColumn(
                name: "Color",
                table: "ClassifiedAd",
                newName: "Mobile_Color");

            migrationBuilder.RenameColumn(
                name: "Brand",
                table: "ClassifiedAd",
                newName: "Mobile_Brand");

            migrationBuilder.RenameColumn(
                name: "ISBN",
                table: "ClassifiedAd",
                newName: "Isbn");

            migrationBuilder.RenameColumn(
                name: "RealState_Features",
                table: "ClassifiedAd",
                newName: "TabletsAndIPads_Model");

            migrationBuilder.RenameColumn(
                name: "Motorcycle_ModelNo",
                table: "ClassifiedAd",
                newName: "Motorcycle_RegisteredDistrict");

            migrationBuilder.RenameColumn(
                name: "Motorcycle_Features",
                table: "ClassifiedAd",
                newName: "Motorcycle_Model");

            migrationBuilder.RenameColumn(
                name: "Mobile_ModelNo",
                table: "ClassifiedAd",
                newName: "Motorcycle_LotNo");

            migrationBuilder.RenameColumn(
                name: "Mobile_Features",
                table: "ClassifiedAd",
                newName: "PhoneOs");

            migrationBuilder.RenameColumn(
                name: "VideoCard",
                table: "ClassifiedAd",
                newName: "ModelYear");

            migrationBuilder.RenameColumn(
                name: "Computer_Features",
                table: "ClassifiedAd",
                newName: "Mobile_Model");

            migrationBuilder.RenameColumn(
                name: "ModelNo",
                table: "ClassifiedAd",
                newName: "WarrantyType");

            migrationBuilder.RenameColumn(
                name: "Features",
                table: "ClassifiedAd",
                newName: "WarrantyPeriod");

            migrationBuilder.AlterColumn<int>(
                name: "Motorcycle_TotalKm",
                table: "ClassifiedAd",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "TabletsAndIPads_Storage",
                table: "ClassifiedAd",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "TabletsAndIPads_Ram",
                table: "ClassifiedAd",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "TabletsAndIPads_FrontCamera",
                table: "ClassifiedAd",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "TabletsAndIPads_BackCamera",
                table: "ClassifiedAd",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "WorkingDays",
                table: "ClassifiedAd",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Salary",
                table: "ClassifiedAd",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ContractFor",
                table: "ClassifiedAd",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Mobile_ScreenSize",
                table: "ClassifiedAd",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "SSD",
                table: "ClassifiedAd",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "HDD",
                table: "ClassifiedAd",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Isbn",
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

            migrationBuilder.AddColumn<int>(
                name: "Engine",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LotNo",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Mileage",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegisteredDistrict",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Transmission",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AdDuration",
                table: "ClassifiedAd",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DeliveryCharges",
                table: "ClassifiedAd",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "UsedFor",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WarrantyIncludes",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "GraphicsCard",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Ram",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ScreenSize",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "BackCamera",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FrontCamera",
                table: "ClassifiedAd",
                nullable: true);

            migrationBuilder.AddColumn<double>(
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
                name: "Engine",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "LotNo",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "Mileage",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "RegisteredDistrict",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "Transmission",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "AdDuration",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "City",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "DeliveryCharges",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "UsedFor",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "WarrantyIncludes",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "GraphicsCard",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "Ram",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "ScreenSize",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "BackCamera",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "FrontCamera",
                table: "ClassifiedAd");

            migrationBuilder.DropColumn(
                name: "Storage",
                table: "ClassifiedAd");

            migrationBuilder.RenameColumn(
                name: "TabletsAndIPads_Storage",
                table: "ClassifiedAd",
                newName: "Storage");

            migrationBuilder.RenameColumn(
                name: "TabletsAndIPads_ScreenSize",
                table: "ClassifiedAd",
                newName: "Mobile_ScreenSize");

            migrationBuilder.RenameColumn(
                name: "TabletsAndIPads_Ram",
                table: "ClassifiedAd",
                newName: "Mobile_Ram");

            migrationBuilder.RenameColumn(
                name: "TabletsAndIPads_PhoneOs",
                table: "ClassifiedAd",
                newName: "PhoneOs");

            migrationBuilder.RenameColumn(
                name: "TabletsAndIPads_FrontCamera",
                table: "ClassifiedAd",
                newName: "FrontCamera");

            migrationBuilder.RenameColumn(
                name: "TabletsAndIPads_Color",
                table: "ClassifiedAd",
                newName: "Motorcycle_Color");

            migrationBuilder.RenameColumn(
                name: "TabletsAndIPads_Brand",
                table: "ClassifiedAd",
                newName: "Motorcycle_Brand");

            migrationBuilder.RenameColumn(
                name: "TabletsAndIPads_BackCamera",
                table: "ClassifiedAd",
                newName: "BackCamera");

            migrationBuilder.RenameColumn(
                name: "Motorcycle_Type",
                table: "ClassifiedAd",
                newName: "Computer_Type");

            migrationBuilder.RenameColumn(
                name: "Motorcycle_ModelYear",
                table: "ClassifiedAd",
                newName: "ModelYear");

            migrationBuilder.RenameColumn(
                name: "Motorcycle_Mileage",
                table: "ClassifiedAd",
                newName: "Mileage");

            migrationBuilder.RenameColumn(
                name: "Motorcycle_Engine",
                table: "ClassifiedAd",
                newName: "Engine");

            migrationBuilder.RenameColumn(
                name: "Motorcycle_Color",
                table: "ClassifiedAd",
                newName: "Mobile_Color");

            migrationBuilder.RenameColumn(
                name: "Motorcycle_Brand",
                table: "ClassifiedAd",
                newName: "Mobile_Brand");

            migrationBuilder.RenameColumn(
                name: "Mobile_ScreenSize",
                table: "ClassifiedAd",
                newName: "ScreenSize");

            migrationBuilder.RenameColumn(
                name: "Mobile_Ram",
                table: "ClassifiedAd",
                newName: "Ram");

            migrationBuilder.RenameColumn(
                name: "Mobile_Color",
                table: "ClassifiedAd",
                newName: "Color");

            migrationBuilder.RenameColumn(
                name: "Mobile_Brand",
                table: "ClassifiedAd",
                newName: "Brand");

            migrationBuilder.RenameColumn(
                name: "Computer_Type",
                table: "ClassifiedAd",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "Isbn",
                table: "ClassifiedAd",
                newName: "ISBN");

            migrationBuilder.RenameColumn(
                name: "TabletsAndIPads_Model",
                table: "ClassifiedAd",
                newName: "RealState_Features");

            migrationBuilder.RenameColumn(
                name: "Motorcycle_RegisteredDistrict",
                table: "ClassifiedAd",
                newName: "Motorcycle_ModelNo");

            migrationBuilder.RenameColumn(
                name: "Motorcycle_Model",
                table: "ClassifiedAd",
                newName: "Motorcycle_Features");

            migrationBuilder.RenameColumn(
                name: "Motorcycle_LotNo",
                table: "ClassifiedAd",
                newName: "Mobile_ModelNo");

            migrationBuilder.RenameColumn(
                name: "PhoneOs",
                table: "ClassifiedAd",
                newName: "Mobile_Features");

            migrationBuilder.RenameColumn(
                name: "Mobile_Model",
                table: "ClassifiedAd",
                newName: "Computer_Features");

            migrationBuilder.RenameColumn(
                name: "WarrantyType",
                table: "ClassifiedAd",
                newName: "ModelNo");

            migrationBuilder.RenameColumn(
                name: "WarrantyPeriod",
                table: "ClassifiedAd",
                newName: "Features");

            migrationBuilder.RenameColumn(
                name: "ModelYear",
                table: "ClassifiedAd",
                newName: "VideoCard");

            migrationBuilder.AlterColumn<string>(
                name: "Storage",
                table: "ClassifiedAd",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Mobile_Ram",
                table: "ClassifiedAd",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FrontCamera",
                table: "ClassifiedAd",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BackCamera",
                table: "ClassifiedAd",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Motorcycle_TotalKm",
                table: "ClassifiedAd",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ScreenSize",
                table: "ClassifiedAd",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "WorkingDays",
                table: "ClassifiedAd",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Salary",
                table: "ClassifiedAd",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ContractFor",
                table: "ClassifiedAd",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SSD",
                table: "ClassifiedAd",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "HDD",
                table: "ClassifiedAd",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ISBN",
                table: "ClassifiedAd",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "MadeYear",
                table: "ClassifiedAd",
                nullable: true);
        }
    }
}
