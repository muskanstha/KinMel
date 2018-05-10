using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KinMel.Data.Migrations
{
    public partial class addnotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Condition",
                table: "ClassifiedAd",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NotificationToId = table.Column<string>(nullable: true),
                    NotificationFromId = table.Column<string>(nullable: true),
                    NotificationText = table.Column<string>(nullable: true),
                    ActionController = table.Column<string>(nullable: true),
                    Action = table.Column<string>(nullable: true),
                    ActionId = table.Column<int>(nullable: false),
                    IsRead = table.Column<bool>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notification_AspNetUsers_NotificationFromId",
                        column: x => x.NotificationFromId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notification_AspNetUsers_NotificationToId",
                        column: x => x.NotificationToId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notification_NotificationFromId",
                table: "Notification",
                column: "NotificationFromId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_NotificationToId",
                table: "Notification",
                column: "NotificationToId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.AlterColumn<string>(
                name: "Condition",
                table: "ClassifiedAd",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
