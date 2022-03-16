using Microsoft.EntityFrameworkCore.Migrations;

namespace E_Commerce.Migrations
{
    public partial class dbchangenew2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblcart");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblcart",
                columns: table => new
                {
                    cartprouctid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    proidproduct_id = table.Column<int>(type: "int", nullable: true),
                    qty = table.Column<int>(type: "int", nullable: false),
                    userid = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblcart", x => x.cartprouctid);
                    table.ForeignKey(
                        name: "FK_tblcart_tblproduct_proidproduct_id",
                        column: x => x.proidproduct_id,
                        principalTable: "tblproduct",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblcart_proidproduct_id",
                table: "tblcart",
                column: "proidproduct_id");
        }
    }
}
