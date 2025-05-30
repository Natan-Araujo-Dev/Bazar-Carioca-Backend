using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BazarCarioca.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class Models_update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
            name: "FK_ProductTypes_Stores_StoreId",
            table: "ProductTypes");

            migrationBuilder.AlterColumn<int>(
                name: "StoreId",
                table: "ProductTypes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTypes_Stores_StoreId",
                table: "ProductTypes",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "OpeningTime",
                table: "Stores",
                type: "time(4)",
                precision: 4,
                nullable: true,
                oldClrType: typeof(TimeOnly),
                oldType: "time(6)",
                oldMaxLength: 4,
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "ClosingTime",
                table: "Stores",
                type: "time(4)",
                precision: 4,
                nullable: true,
                oldClrType: typeof(TimeOnly),
                oldType: "time(6)",
                oldMaxLength: 4,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CellphoneNumber",
                table: "Stores",
                type: "varchar(9)",
                maxLength: 9,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 9,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "StoreId",
                table: "ProductTypes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
            name: "FK_ProductTypes_Stores_StoreId",
            table: "ProductTypes");

            migrationBuilder.AlterColumn<int>(
                name: "StoreId",
                table: "ProductTypes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTypes_Stores_StoreId",
                table: "ProductTypes",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "OpeningTime",
                table: "Stores",
                type: "time(6)",
                maxLength: 4,
                nullable: true,
                oldClrType: typeof(TimeOnly),
                oldType: "time(4)",
                oldPrecision: 4,
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "ClosingTime",
                table: "Stores",
                type: "time(6)",
                maxLength: 4,
                nullable: true,
                oldClrType: typeof(TimeOnly),
                oldType: "time(4)",
                oldPrecision: 4,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CellphoneNumber",
                table: "Stores",
                type: "int",
                maxLength: 9,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(9)",
                oldMaxLength: 9,
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "StoreId",
                table: "ProductTypes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
