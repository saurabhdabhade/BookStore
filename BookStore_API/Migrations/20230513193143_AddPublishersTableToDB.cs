using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStore_API.Migrations
{
    /// <inheritdoc />
    public partial class AddPublishersTableToDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Publishers_PublisherID",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_PublisherID",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "PublisherID",
                table: "Books");

            migrationBuilder.AddColumn<string>(
                name: "PublisherEmail",
                table: "Publishers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublisherEmail",
                table: "Publishers");

            migrationBuilder.AddColumn<int>(
                name: "PublisherID",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_PublisherID",
                table: "Books",
                column: "PublisherID");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Publishers_PublisherID",
                table: "Books",
                column: "PublisherID",
                principalTable: "Publishers",
                principalColumn: "PublisherID");
        }
    }
}
