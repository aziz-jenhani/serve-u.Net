using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace serveu.Migrations
{
    /// <inheritdoc />
    public partial class updateNameTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItems_FileEntities_ImageId",
                table: "MenuItems");

            migrationBuilder.DropIndex(
                name: "IX_MenuItems_ImageId",
                table: "MenuItems");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "MenuItems",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "MenuItems",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "MenuItemId",
                table: "MenuItems",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "ImageId",
                table: "MenuItems",
                newName: "restaurant_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "MenuCategories",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "MenuCategoryId",
                table: "MenuCategories",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Path",
                table: "FileEntities",
                newName: "path");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "FileEntities",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "FileEntities",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "FileEntities",
                newName: "created_at");

            migrationBuilder.AddColumn<int>(
                name: "category_id",
                table: "MenuItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "MenuItems",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "image_id",
                table: "MenuItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "MenuItems",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "MenuCategories",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "MenuCategories",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_image_id",
                table: "MenuItems",
                column: "image_id");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItems_FileEntities_image_id",
                table: "MenuItems",
                column: "image_id",
                principalTable: "FileEntities",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItems_FileEntities_image_id",
                table: "MenuItems");

            migrationBuilder.DropIndex(
                name: "IX_MenuItems_image_id",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "category_id",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "image_id",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "MenuCategories");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "MenuCategories");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "MenuItems",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "MenuItems",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "MenuItems",
                newName: "MenuItemId");

            migrationBuilder.RenameColumn(
                name: "restaurant_id",
                table: "MenuItems",
                newName: "ImageId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "MenuCategories",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "MenuCategories",
                newName: "MenuCategoryId");

            migrationBuilder.RenameColumn(
                name: "path",
                table: "FileEntities",
                newName: "Path");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "FileEntities",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "FileEntities",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "FileEntities",
                newName: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_ImageId",
                table: "MenuItems",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItems_FileEntities_ImageId",
                table: "MenuItems",
                column: "ImageId",
                principalTable: "FileEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
