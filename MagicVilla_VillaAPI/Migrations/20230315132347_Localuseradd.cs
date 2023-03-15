using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVillaVillaAPI.Migrations
{
    /// <inheritdoc />
    public partial class Localuseradd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LocalUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalUsers", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "ImageUrl" },
                values: new object[] { new DateTime(2023, 3, 15, 18, 53, 46, 828, DateTimeKind.Local).AddTicks(4136), "https://dotnetmastery.com/bluevillaimages/villa3.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreateDate", "ImageUrl" },
                values: new object[] { new DateTime(2023, 3, 15, 18, 53, 46, 828, DateTimeKind.Local).AddTicks(4153), "https://dotnetmastery.com/bluevillaimages/villa1.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreateDate", "ImageUrl" },
                values: new object[] { new DateTime(2023, 3, 15, 18, 53, 46, 828, DateTimeKind.Local).AddTicks(4156), "https://dotnetmastery.com/bluevillaimages/villa4.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreateDate", "ImageUrl" },
                values: new object[] { new DateTime(2023, 3, 15, 18, 53, 46, 828, DateTimeKind.Local).AddTicks(4159), "https://dotnetmastery.com/bluevillaimages/villa5.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreateDate", "ImageUrl" },
                values: new object[] { new DateTime(2023, 3, 15, 18, 53, 46, 828, DateTimeKind.Local).AddTicks(4162), "https://dotnetmastery.com/bluevillaimages/villa2.jpg" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocalUsers");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "ImageUrl" },
                values: new object[] { new DateTime(2023, 2, 28, 15, 49, 10, 158, DateTimeKind.Local).AddTicks(992), "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa3.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreateDate", "ImageUrl" },
                values: new object[] { new DateTime(2023, 2, 28, 15, 49, 10, 158, DateTimeKind.Local).AddTicks(1007), "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa1.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreateDate", "ImageUrl" },
                values: new object[] { new DateTime(2023, 2, 28, 15, 49, 10, 158, DateTimeKind.Local).AddTicks(1009), "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa4.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreateDate", "ImageUrl" },
                values: new object[] { new DateTime(2023, 2, 28, 15, 49, 10, 158, DateTimeKind.Local).AddTicks(1013), "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa5.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreateDate", "ImageUrl" },
                values: new object[] { new DateTime(2023, 2, 28, 15, 49, 10, 158, DateTimeKind.Local).AddTicks(1014), "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa2.jpg" });
        }
    }
}
