using Microsoft.EntityFrameworkCore.Migrations;

namespace E_Commerce.Migrations
{
    public partial class changesapply : Migration
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
                    Productidproduct_id = table.Column<int>(type: "int", nullable: true),
                    userid = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblcart", x => x.cartprouctid);
                    table.ForeignKey(
                        name: "FK_tblcart_tblproduct_Productidproduct_id",
                        column: x => x.Productidproduct_id,
                        principalTable: "tblproduct",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblcart_Productidproduct_id",
                table: "tblcart",
                column: "Productidproduct_id");
        }
    }
}
