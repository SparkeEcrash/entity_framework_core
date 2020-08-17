using Microsoft.EntityFrameworkCore.Migrations;

namespace WizLib_DataAccess.Migrations
{
    public partial class addOneToOneFluentBookAndBookDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookDetail_Id",
                table: "Fluent_Book",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Fluent_Book_BookDetail_Id",
                table: "Fluent_Book",
                column: "BookDetail_Id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Fluent_Book_Fluent_BookDeatils_BookDetail_Id",
                table: "Fluent_Book",
                column: "BookDetail_Id",
                principalTable: "Fluent_BookDeatils",
                principalColumn: "BookDetail_Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fluent_Book_Fluent_BookDeatils_BookDetail_Id",
                table: "Fluent_Book");

            migrationBuilder.DropIndex(
                name: "IX_Fluent_Book_BookDetail_Id",
                table: "Fluent_Book");

            migrationBuilder.DropColumn(
                name: "BookDetail_Id",
                table: "Fluent_Book");
        }
    }
}
