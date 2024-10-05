﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TESTDB.Migrations
{
    /// <inheritdoc />
    public partial class fixedType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Types_typeId",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "typeId",
                table: "Items",
                newName: "TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Items_typeId",
                table: "Items",
                newName: "IX_Items_TypeId");

            migrationBuilder.AlterColumn<int>(
                name: "TypeId",
                table: "Items",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Types_TypeId",
                table: "Items",
                column: "TypeId",
                principalTable: "Types",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Types_TypeId",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "Items",
                newName: "typeId");

            migrationBuilder.RenameIndex(
                name: "IX_Items_TypeId",
                table: "Items",
                newName: "IX_Items_typeId");

            migrationBuilder.AlterColumn<int>(
                name: "typeId",
                table: "Items",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Types_typeId",
                table: "Items",
                column: "typeId",
                principalTable: "Types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
