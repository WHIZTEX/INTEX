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
            migrationBuilder.AddColumn<int>(
                name: "RecommendationId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductRecommendations_Products_Recommendation1Id",
                        column: x => x.Recommendation1Id,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductRecommendations_Products_Recommendation2Id",
                        column: x => x.Recommendation2Id,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductRecommendations_Products_Recommendation3Id",
                        column: x => x.Recommendation3Id,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductRecommendations_Products_Recommendation4Id",
                        column: x => x.Recommendation4Id,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductRecommendations_Products_Recommendation5Id",
                        column: x => x.Recommendation5Id,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRecommendation_Products_Recommendation1Id",
                        column: x => x.Recommendation1Id,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRecommendation_Products_Recommendation2Id",
                        column: x => x.Recommendation2Id,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRecommendation_Products_Recommendation3Id",
                        column: x => x.Recommendation3Id,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductRecommendations_Recommendation1Id",
                table: "ProductRecommendations",
                column: "Recommendation1Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductRecommendations_Recommendation2Id",
                table: "ProductRecommendations",
                column: "Recommendation2Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductRecommendations_Recommendation3Id",
                table: "ProductRecommendations",
                column: "Recommendation3Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductRecommendations_Recommendation4Id",
                table: "ProductRecommendations",
                column: "Recommendation4Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductRecommendations_Recommendation5Id",
                table: "ProductRecommendations",
                column: "Recommendation5Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRecommendation_Recommendation1Id",
                table: "UserRecommendation",
                column: "Recommendation1Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRecommendation_Recommendation2Id",
                table: "UserRecommendation",
                column: "Recommendation2Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRecommendation_Recommendation3Id",
                table: "UserRecommendation",
                column: "Recommendation3Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductRecommendations");

            migrationBuilder.DropTable(
                name: "UserRecommendation");

            migrationBuilder.DropColumn(
                name: "RecommendationId",
                table: "AspNetUsers");
        }
    }
}
