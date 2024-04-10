using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INTEX.Migrations
{
    /// <inheritdoc />
    public partial class AddressCompositeIndexUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Addresses_AddressLine1",
                table: "Addresses");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_AddressLine1_AddressLine2_City_State_Code_Country",
                table: "Addresses",
                columns: new[] { "AddressLine1", "AddressLine2", "City", "State", "Code", "Country" },
                unique: true,
                filter: "[AddressLine1] IS NOT NULL AND [AddressLine2] IS NOT NULL AND [City] IS NOT NULL AND [State] IS NOT NULL AND [Code] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Addresses_AddressLine1_AddressLine2_City_State_Code_Country",
                table: "Addresses");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_AddressLine1",
                table: "Addresses",
                column: "AddressLine1");
        }
    }
}
