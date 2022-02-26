using Microsoft.EntityFrameworkCore.Migrations;

namespace E_Commerce.Migrations
{
    public partial class changesapplynew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "categorycat_id",
                table: "tblproduct",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblproduct_categorycat_id",
                table: "tblproduct",
                column: "categorycat_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tblproduct_tblcategory_categorycat_id",
                table: "tblproduct",
                column: "categorycat_id",
                principalTable: "tblcategory",
                principalColumn: "cat_id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblproduct_tblcategory_categorycat_id",
                table: "tblproduct");

            migrationBuilder.DropIndex(
                name: "IX_tblproduct_categorycat_id",
                table: "tblproduct");

            migrationBuilder.DropColumn(
                name: "categorycat_id",
                table: "tblproduct");
        }
    }
}
