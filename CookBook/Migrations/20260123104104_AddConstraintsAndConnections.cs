using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CookBook.Migrations
{
    /// <inheritdoc />
    public partial class AddConstraintsAndConnections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngredientsInRecipes_Ingredients_IngredientId",
                table: "IngredientsInRecipes");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Recipes",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Recipes",
                type: "character varying(32000)",
                maxLength: 32000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Ingredients",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_Name",
                table: "Ingredients",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientsInRecipes_Ingredients_IngredientId",
                table: "IngredientsInRecipes",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngredientsInRecipes_Ingredients_IngredientId",
                table: "IngredientsInRecipes");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_Name",
                table: "Ingredients");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Recipes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Recipes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(32000)",
                oldMaxLength: 32000);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Ingredients",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128);

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientsInRecipes_Ingredients_IngredientId",
                table: "IngredientsInRecipes",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
