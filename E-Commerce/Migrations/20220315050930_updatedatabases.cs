using Microsoft.EntityFrameworkCore.Migrations;

namespace E_Commerce.Migrations
{
    public partial class updatedatabases : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblorder_tblAddresses_ad_id",
                table: "tblorder");

            migrationBuilder.DropIndex(
                name: "IX_tblorder_ad_id",
                table: "tblorder");

            migrationBuilder.AddColumn<int>(
                name: "qty",
                table: "tblorder",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblorder_qty",
                table: "tblorder",
                column: "qty");

            migrationBuilder.AddForeignKey(
                name: "FK_tblorder_tblAddresses_qty",
                table: "tblorder",
                column: "qty",
                principalTable: "tblAddresses",
                principalColumn: "ad_id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblorder_tblAddresses_qty",
                table: "tblorder");

            migrationBuilder.DropIndex(
                name: "IX_tblorder_qty",
                table: "tblorder");

            migrationBuilder.DropColumn(
                name: "qty",
                table: "tblorder");

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
        }
    }
}
