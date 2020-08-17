using Microsoft.EntityFrameworkCore.Migrations;

namespace WizLib_DataAccess.Migrations
{
    public partial class addManyToManyBookAuthorCorrection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fluent_BookAuthor_Fluent_Author_Fluent_AuthorAuthor_Id",
                table: "Fluent_BookAuthor");

            migrationBuilder.DropForeignKey(
                name: "FK_Fluent_BookAuthor_Fluent_Book_Author_Id",
                table: "Fluent_BookAuthor");

            migrationBuilder.DropIndex(
                name: "IX_Fluent_BookAuthor_Fluent_AuthorAuthor_Id",
                table: "Fluent_BookAuthor");

            migrationBuilder.DropColumn(
                name: "Fluent_AuthorAuthor_Id",
                table: "Fluent_BookAuthor");

            migrationBuilder.CreateIndex(
                name: "IX_Fluent_BookAuthor_Book_Id",
                table: "Fluent_BookAuthor",
                column: "Book_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Fluent_BookAuthor_Fluent_Author_Author_Id",
                table: "Fluent_BookAuthor",
                column: "Author_Id",
                principalTable: "Fluent_Author",
                principalColumn: "Author_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fluent_BookAuthor_Fluent_Book_Book_Id",
                table: "Fluent_BookAuthor",
                column: "Book_Id",
                principalTable: "Fluent_Book",
                principalColumn: "Book_Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fluent_BookAuthor_Fluent_Author_Author_Id",
                table: "Fluent_BookAuthor");

            migrationBuilder.DropForeignKey(
                name: "FK_Fluent_BookAuthor_Fluent_Book_Book_Id",
                table: "Fluent_BookAuthor");

            migrationBuilder.DropIndex(
                name: "IX_Fluent_BookAuthor_Book_Id",
                table: "Fluent_BookAuthor");

            migrationBuilder.AddColumn<int>(
                name: "Fluent_AuthorAuthor_Id",
                table: "Fluent_BookAuthor",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fluent_BookAuthor_Fluent_AuthorAuthor_Id",
                table: "Fluent_BookAuthor",
                column: "Fluent_AuthorAuthor_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Fluent_BookAuthor_Fluent_Author_Fluent_AuthorAuthor_Id",
                table: "Fluent_BookAuthor",
                column: "Fluent_AuthorAuthor_Id",
                principalTable: "Fluent_Author",
                principalColumn: "Author_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Fluent_BookAuthor_Fluent_Book_Author_Id",
                table: "Fluent_BookAuthor",
                column: "Author_Id",
                principalTable: "Fluent_Book",
                principalColumn: "Book_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
