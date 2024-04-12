using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INTEX.Migrations
{
    /// <inheritdoc />
    public partial class Recommendations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductRecommendations",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Recommendation1Id = table.Column<int>(type: "int", nullable: false),
                    Recommendation2Id = table.Column<int>(type: "int", nullable: false),
                    Recommendation3Id = table.Column<int>(type: "int", nullable: false),
                    Recommendation4Id = table.Column<int>(type: "int", nullable: false),
                    Recommendation5Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRecommendations", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_ProductRecommendations_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserRecommendation",
                columns: table => new
                {
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Recommendation1Id = table.Column<int>(type: "int", nullable: false),
                    Recommendation2Id = table.Column<int>(type: "int", nullable: false),
                    Recommendation3Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRecommendation", x => x.CustomerId);
                    table.ForeignKey(
                        name: "FK_UserRecommendation_AspNetUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductRecommendations");

            migrationBuilder.DropTable(
                name: "UserRecommendation");
        }
    }
}
