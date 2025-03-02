using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PatiliDost.Migrations
{
    /// <inheritdoc />
    public partial class VER : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a3990aab-b324-41fe-bc23-e2f1c6475d41");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c6e9aa91-682c-4733-98cc-2501d608d43c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fdeca7ff-a064-4eb4-b902-236f6e52c750");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6d70755d-b379-42c9-a29d-28d6678408d9", null, "Administrator", "ADMINISTRATOR" },
                    { "8dd38521-0c71-45be-95e4-0e893dd74f92", null, "Moderator", "MODERATOR" },
                    { "cc63907d-a75f-4fdb-b6b5-098150efe8d8", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6d70755d-b379-42c9-a29d-28d6678408d9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8dd38521-0c71-45be-95e4-0e893dd74f92");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cc63907d-a75f-4fdb-b6b5-098150efe8d8");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Contacts");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a3990aab-b324-41fe-bc23-e2f1c6475d41", null, "User", "USER" },
                    { "c6e9aa91-682c-4733-98cc-2501d608d43c", null, "Moderator", "MODERATOR" },
                    { "fdeca7ff-a064-4eb4-b902-236f6e52c750", null, "Administrator", "ADMINISTRATOR" }
                });
        }
    }
}
