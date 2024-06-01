using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItlaBankApp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AccountIdAsPK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Beneficiaries_Products_AccountId",
                table: "Beneficiaries");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Products_DestinationId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Products_OriginId",
                table: "Payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Products");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Beneficiaries_Products_AccountId",
                table: "Beneficiaries",
                column: "AccountId",
                principalTable: "Products",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Products_DestinationId",
                table: "Payments",
                column: "DestinationId",
                principalTable: "Products",
                principalColumn: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Products_OriginId",
                table: "Payments",
                column: "OriginId",
                principalTable: "Products",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Beneficiaries_Products_AccountId",
                table: "Beneficiaries");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Products_DestinationId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Products_OriginId",
                table: "Payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Beneficiaries_Products_AccountId",
                table: "Beneficiaries",
                column: "AccountId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Products_DestinationId",
                table: "Payments",
                column: "DestinationId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Products_OriginId",
                table: "Payments",
                column: "OriginId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
