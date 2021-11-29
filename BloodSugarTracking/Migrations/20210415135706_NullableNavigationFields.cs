using Microsoft.EntityFrameworkCore.Migrations;

namespace BloodSugarTracking.Migrations;

public partial class NullableNavigationFields : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_BloodSugarTestResults_Users_UserId",
            table: "BloodSugarTestResults");

        migrationBuilder.AlterColumn<int>(
            name: "UserId",
            table: "BloodSugarTestResults",
            type: "int",
            nullable: true,
            oldClrType: typeof(int),
            oldType: "int");

        migrationBuilder.AddForeignKey(
            name: "FK_BloodSugarTestResults_Users_UserId",
            table: "BloodSugarTestResults",
            column: "UserId",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_BloodSugarTestResults_Users_UserId",
            table: "BloodSugarTestResults");

        migrationBuilder.AlterColumn<int>(
            name: "UserId",
            table: "BloodSugarTestResults",
            type: "int",
            nullable: false,
            defaultValue: 0,
            oldClrType: typeof(int),
            oldType: "int",
            oldNullable: true);

        migrationBuilder.AddForeignKey(
            name: "FK_BloodSugarTestResults_Users_UserId",
            table: "BloodSugarTestResults",
            column: "UserId",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
