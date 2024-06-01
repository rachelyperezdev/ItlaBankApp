using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItlaBankApp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Beneficiaries_BeneficiaryId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Products_DestinationId",
                table: "Payments");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Beneficiaries_BeneficiaryId",
                table: "Payments",
                column: "BeneficiaryId",
                principalTable: "Beneficiaries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Products_DestinationId",
                table: "Payments",
                column: "DestinationId",
                principalTable: "Products",
                principalColumn: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Beneficiaries_BeneficiaryId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Products_DestinationId",
                table: "Payments");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Beneficiaries_BeneficiaryId",
                table: "Payments",
                column: "BeneficiaryId",
                principalTable: "Beneficiaries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Products_DestinationId",
                table: "Payments",
                column: "DestinationId",
                principalTable: "Products",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
