using Microsoft.EntityFrameworkCore.Migrations;

namespace E_Commerce.Migrations
{
    public partial class dbchangenew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblcart_tblproduct_cartproductproduct_id",
                table: "tblcart");

            migrationBuilder.DropColumn(
                name: "proid",
                table: "tblcart");

            migrationBuilder.RenameColumn(
                name: "cartproductproduct_id",
                table: "tblcart",
                newName: "proidproduct_id");

            migrationBuilder.RenameIndex(
                name: "IX_tblcart_cartproductproduct_id",
                table: "tblcart",
                newName: "IX_tblcart_proidproduct_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tblcart_tblproduct_proidproduct_id",
                table: "tblcart",
                column: "proidproduct_id",
                principalTable: "tblproduct",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblcart_tblproduct_proidproduct_id",
                table: "tblcart");

            migrationBuilder.RenameColumn(
                name: "proidproduct_id",
                table: "tblcart",
                newName: "cartproductproduct_id");

            migrationBuilder.RenameIndex(
                name: "IX_tblcart_proidproduct_id",
                table: "tblcart",
                newName: "IX_tblcart_cartproductproduct_id");

            migrationBuilder.AddColumn<int>(
                name: "proid",
                table: "tblcart",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_tblcart_tblproduct_cartproductproduct_id",
                table: "tblcart",
                column: "cartproductproduct_id",
                principalTable: "tblproduct",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
