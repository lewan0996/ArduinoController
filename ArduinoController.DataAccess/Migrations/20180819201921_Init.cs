using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ArduinoController.DataAccess.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationUser",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    NormalizedUserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    NormalizedEmail = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArduinoDevice",
                columns: table => new
                {
                    MacAddress = table.Column<string>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArduinoDevice", x => x.MacAddress);
                    table.ForeignKey(
                        name: "FK_ArduinoDevice_ApplicationUser_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Procedures",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Procedures", x => new { x.UserId, x.Name });
                    table.ForeignKey(
                        name: "FK_Procedures_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Commands",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    ProcedureName = table.Column<string>(nullable: true),
                    ProcedureUserId = table.Column<string>(nullable: true),
                    PinNumber = table.Column<byte>(nullable: true),
                    Value = table.Column<byte>(nullable: true),
                    DigitalWriteCommand_PinNumber = table.Column<byte>(nullable: true),
                    DigitalWriteCommand_Value = table.Column<bool>(nullable: true),
                    NegateCommand_PinNumber = table.Column<byte>(nullable: true),
                    Duration = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commands", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Commands_Procedures_ProcedureUserId_ProcedureName",
                        columns: x => new { x.ProcedureUserId, x.ProcedureName },
                        principalTable: "Procedures",
                        principalColumns: new[] { "UserId", "Name" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArduinoDevice_ApplicationUserId",
                table: "ArduinoDevice",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Commands_ProcedureUserId_ProcedureName",
                table: "Commands",
                columns: new[] { "ProcedureUserId", "ProcedureName" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArduinoDevice");

            migrationBuilder.DropTable(
                name: "Commands");

            migrationBuilder.DropTable(
                name: "Procedures");

            migrationBuilder.DropTable(
                name: "ApplicationUser");
        }
    }
}
