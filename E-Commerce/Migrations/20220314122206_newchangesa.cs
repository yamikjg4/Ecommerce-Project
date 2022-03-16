using Microsoft.EntityFrameworkCore.Migrations;

namespace E_Commerce.Migrations
{
    public partial class newchangesa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblorder_tblAddresses_ad_id",
                table: "tblorder");

            migrationBuilder.DropForeignKey(
                name: "FK_tblorder_tblproduct_productidproduct_id",
                table: "tblorder");

            migrationBuilder.DropIndex(
                name: "IX_tblorder_ad_id",
                table: "tblorder");

            migrationBuilder.RenameColumn(
                name: "productidproduct_id",
                table: "tblorder",
                newName: "productid");

            migrationBuilder.RenameIndex(
                name: "IX_tblorder_productidproduct_id",
                table: "tblorder",
                newName: "IX_tblorder_productid");

            migrationBuilder.AddColumn<int>(
                name: "Addressad_id",
                table: "tblorder",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblorder_Addressad_id",
                table: "tblorder",
                column: "Addressad_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tblorder_tblAddresses_Addressad_id",
                table: "tblorder",
                column: "Addressad_id",
                principalTable: "tblAddresses",
                principalColumn: "ad_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tblorder_tblproduct_productid",
                table: "tblorder",
                column: "productid",
                principalTable: "tblproduct",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblorder_tblAddresses_Addressad_id",
                table: "tblorder");

            migrationBuilder.DropForeignKey(
                name: "FK_tblorder_tblproduct_productid",
                table: "tblorder");

            migrationBuilder.DropIndex(
                name: "IX_tblorder_Addressad_id",
                table: "tblorder");

            migrationBuilder.DropColumn(
                name: "Addressad_id",
                table: "tblorder");

            migrationBuilder.RenameColumn(
                name: "productid",
                table: "tblorder",
                newName: "productidproduct_id");

            migrationBuilder.RenameIndex(
                name: "IX_tblorder_productid",
                table: "tblorder",
                newName: "IX_tblorder_productidproduct_id");

            migrationBuilder.CreateIndex(
                name: "IX_tblorder_ad_id",
                table: "tblorder",
                column: "ad_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tblorder_tblAddresses_ad_id",
                table: "tblorder",
                column: "ad_id",
                principalTable: "tblAddresses",
                principalColumn: "ad_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tblorder_tblproduct_productidproduct_id",
                table: "tblorder",
                column: "productidproduct_id",
                principalTable: "tblproduct",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
