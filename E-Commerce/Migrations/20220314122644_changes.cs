using Microsoft.EntityFrameworkCore.Migrations;

namespace E_Commerce.Migrations
{
    public partial class changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblorder_tblproduct_productid",
                table: "tblorder");

            migrationBuilder.DropIndex(
                name: "IX_tblorder_productid",
                table: "tblorder");

            migrationBuilder.RenameColumn(
                name: "productid",
                table: "tblorder",
                newName: "product_id");

            migrationBuilder.AddColumn<int>(
                name: "prdproduct_id",
                table: "tblorder",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblorder_prdproduct_id",
                table: "tblorder",
                column: "prdproduct_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tblorder_tblproduct_prdproduct_id",
                table: "tblorder",
                column: "prdproduct_id",
                principalTable: "tblproduct",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblorder_tblproduct_prdproduct_id",
                table: "tblorder");

            migrationBuilder.DropIndex(
                name: "IX_tblorder_prdproduct_id",
                table: "tblorder");

            migrationBuilder.DropColumn(
                name: "prdproduct_id",
                table: "tblorder");

            migrationBuilder.RenameColumn(
                name: "product_id",
                table: "tblorder",
                newName: "productid");

            migrationBuilder.CreateIndex(
                name: "IX_tblorder_productid",
                table: "tblorder",
                column: "productid");

            migrationBuilder.AddForeignKey(
                name: "FK_tblorder_tblproduct_productid",
                table: "tblorder",
                column: "productid",
                principalTable: "tblproduct",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
