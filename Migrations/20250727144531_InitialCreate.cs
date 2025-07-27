using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AspnetCoreStarter.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Musteriler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdSoyad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Adres = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Aktif = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Musteriler", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Musteriler",
                columns: new[] { "Id", "AdSoyad", "Adres", "Aktif", "CreateDate", "Email", "Telefon", "UpdateDate" },
                values: new object[,]
                {
                    { 1, "Ahmet Yılmaz", "İstanbul, Kadıköy", true, new DateTime(2025, 7, 27, 17, 45, 30, 274, DateTimeKind.Local).AddTicks(7184), "ahmet.yilmaz@email.com", "0532-123-4567", null },
                    { 2, "Ayşe Kaya", "Ankara, Çankaya", true, new DateTime(2025, 7, 27, 17, 45, 30, 274, DateTimeKind.Local).AddTicks(7457), "ayse.kaya@email.com", "0533-987-6543", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Musteriler_Email",
                table: "Musteriler",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Musteriler_Telefon",
                table: "Musteriler",
                column: "Telefon",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Musteriler");
        }
    }
}
