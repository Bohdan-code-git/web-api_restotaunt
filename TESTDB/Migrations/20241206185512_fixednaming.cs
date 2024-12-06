using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TESTDB.Migrations
{
    /// <inheritdoc />
    public partial class fixednaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_News_NewsTypes_typeId",
                table: "News");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Types",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Statuses",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "NewsTypes",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "typeId",
                table: "News",
                newName: "TypeId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "News",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "imageURL",
                table: "News",
                newName: "ImageURL");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "News",
                newName: "Description");

            migrationBuilder.RenameIndex(
                name: "IX_News_typeId",
                table: "News",
                newName: "IX_News_TypeId");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "Items",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Items",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "imageURL",
                table: "Items",
                newName: "ImageURL");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Items",
                newName: "Description");

            migrationBuilder.AddColumn<string>(
                name: "Adress",
                table: "Order",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_News_NewsTypes_TypeId",
                table: "News",
                column: "TypeId",
                principalTable: "NewsTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_News_NewsTypes_TypeId",
                table: "News");

            migrationBuilder.DropColumn(
                name: "Adress",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Types",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Statuses",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "NewsTypes",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "News",
                newName: "typeId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "News",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "ImageURL",
                table: "News",
                newName: "imageURL");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "News",
                newName: "description");

            migrationBuilder.RenameIndex(
                name: "IX_News_TypeId",
                table: "News",
                newName: "IX_News_typeId");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Items",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Items",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "ImageURL",
                table: "Items",
                newName: "imageURL");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Items",
                newName: "description");

            migrationBuilder.AddForeignKey(
                name: "FK_News_NewsTypes_typeId",
                table: "News",
                column: "typeId",
                principalTable: "NewsTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
