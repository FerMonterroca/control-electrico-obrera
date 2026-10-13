using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControlElectricoObrera.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddHistoricalSnapshots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyNameSnapshot",
                table: "SpecialMaterialRequests",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerEmailSnapshot",
                table: "SpecialMaterialRequests",
                type: "character varying(320)",
                maxLength: 320,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerNameSnapshot",
                table: "SpecialMaterialRequests",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerPhoneSnapshot",
                table: "SpecialMaterialRequests",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyNameSnapshot",
                table: "QuoteRequests",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerEmailSnapshot",
                table: "QuoteRequests",
                type: "character varying(320)",
                maxLength: 320,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerNameSnapshot",
                table: "QuoteRequests",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerPhoneSnapshot",
                table: "QuoteRequests",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SkuOrCodeSnapshot",
                table: "QuoteItems",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.Sql(
                """
                UPDATE "QuoteRequests" AS request
                SET "CustomerNameSnapshot" = customer."Name",
                    "CustomerPhoneSnapshot" = customer."Phone",
                    "CustomerEmailSnapshot" = customer."Email",
                    "CompanyNameSnapshot" = customer."CompanyName"
                FROM "Customers" AS customer
                WHERE request."CustomerId" = customer."Id";

                UPDATE "SpecialMaterialRequests" AS request
                SET "CustomerNameSnapshot" = customer."Name",
                    "CustomerPhoneSnapshot" = customer."Phone",
                    "CustomerEmailSnapshot" = customer."Email",
                    "CompanyNameSnapshot" = customer."CompanyName"
                FROM "Customers" AS customer
                WHERE request."CustomerId" = customer."Id";
                """);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerNameSnapshot",
                table: "SpecialMaterialRequests",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerPhoneSnapshot",
                table: "SpecialMaterialRequests",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerNameSnapshot",
                table: "QuoteRequests",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerPhoneSnapshot",
                table: "QuoteRequests",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyNameSnapshot",
                table: "SpecialMaterialRequests");

            migrationBuilder.DropColumn(
                name: "CustomerEmailSnapshot",
                table: "SpecialMaterialRequests");

            migrationBuilder.DropColumn(
                name: "CustomerNameSnapshot",
                table: "SpecialMaterialRequests");

            migrationBuilder.DropColumn(
                name: "CustomerPhoneSnapshot",
                table: "SpecialMaterialRequests");

            migrationBuilder.DropColumn(
                name: "CompanyNameSnapshot",
                table: "QuoteRequests");

            migrationBuilder.DropColumn(
                name: "CustomerEmailSnapshot",
                table: "QuoteRequests");

            migrationBuilder.DropColumn(
                name: "CustomerNameSnapshot",
                table: "QuoteRequests");

            migrationBuilder.DropColumn(
                name: "CustomerPhoneSnapshot",
                table: "QuoteRequests");

            migrationBuilder.DropColumn(
                name: "SkuOrCodeSnapshot",
                table: "QuoteItems");
        }
    }
}
