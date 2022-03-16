using Microsoft.EntityFrameworkCore.Migrations;

namespace E_Commerce.Migrations
{
    public partial class datatable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblorder_tblAddresses_Addressad_id",
                table: "tblorder");

            migrationBuilder.DropForeignKey(
                name: "FK_tblorder_tblproduct_prdproduct_id",
                table: "tblorder");

            migrationBuilder.DropIndex(
                name: "IX_tblorder_Addressad_id",
                table: "tblorder");

            migrationBuilder.DropIndex(
                name: "IX_tblorder_prdproduct_id",
                table: "tblorder");

            migrationBuilder.DropColumn(
                name: "Addressad_id",
                table: "tblorder");

            migrationBuilder.DropColumn(
                name: "prdproduct_id",
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

            migrationBuilder.AddForeignKey(
                name: "FK_tblorder_tblAddresses_ad_id",
                table: "tblorder",
                column: "ad_id",
                principalTable: "tblAddresses",
                principalColumn: "ad_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tblorder_tblproduct_product_id",
                table: "tblorder",
                column: "product_id",
                principalTable: "tblproduct",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblorder_tblAddresses_ad_id",
                table: "tblorder");

            migrationBuilder.DropForeignKey(
                name: "FK_tblorder_tblproduct_product_id",
                table: "tblorder");

            migrationBuilder.DropIndex(
                name: "IX_tblorder_ad_id",
                table: "tblorder");

            migrationBuilder.DropIndex(
                name: "IX_tblorder_product_id",
                table: "tblorder");

            migrationBuilder.AddColumn<int>(
                name: "Addressad_id",
                table: "tblorder",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "prdproduct_id",
                table: "tblorder",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblorder_Addressad_id",
                table: "tblorder",
                column: "Addressad_id");

            migrationBuilder.CreateIndex(
                name: "IX_tblorder_prdproduct_id",
                table: "tblorder",
                column: "prdproduct_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tblorder_tblAddresses_Addressad_id",
                table: "tblorder",
                column: "Addressad_id",
                principalTable: "tblAddresses",
                principalColumn: "ad_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tblorder_tblproduct_prdproduct_id",
                table: "tblorder",
                column: "prdproduct_id",
                principalTable: "tblproduct",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
