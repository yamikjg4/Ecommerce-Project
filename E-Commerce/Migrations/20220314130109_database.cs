using Microsoft.EntityFrameworkCore.Migrations;

namespace E_Commerce.Migrations
{
    public partial class database : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tblorder_ad_id",
                table: "tblorder");

            migrationBuilder.DropIndex(
                name: "IX_tblorder_product_id",
                table: "tblorder");

            migrationBuilder.CreateIndex(
                name: "IX_tblorder_ad_id",
                table: "tblorder",
                column: "ad_id");

            migrationBuilder.CreateIndex(
                name: "IX_tblorder_product_id",
                table: "tblorder",
                column: "product_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tblorder_ad_id",
                table: "tblorder");

            migrationBuilder.DropIndex(
                name: "IX_tblorder_product_id",
                table: "tblorder");

            migrationBuilder.CreateIndex(
                name: "IX_tblorder_ad_id",
                table: "tblorder",
                column: "ad_id",
                unique: true,
                filter: "[ad_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_tblorder_product_id",
                table: "tblorder",
                column: "product_id",
                unique: true,
                filter: "[product_id] IS NOT NULL");
        }
    }
}
