using Microsoft.EntityFrameworkCore.Migrations;

namespace E_Commerce.Migrations
{
    public partial class redw : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblorder_tblorder_orderid1",
                table: "tblorder");

            migrationBuilder.DropIndex(
                name: "IX_tblorder_orderid1",
                table: "tblorder");

            migrationBuilder.DropColumn(
                name: "orderid1",
                table: "tblorder");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "orderid1",
                table: "tblorder",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblorder_orderid1",
                table: "tblorder",
                column: "orderid1");

            migrationBuilder.AddForeignKey(
                name: "FK_tblorder_tblorder_orderid1",
                table: "tblorder",
                column: "orderid1",
                principalTable: "tblorder",
                principalColumn: "orderid",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
