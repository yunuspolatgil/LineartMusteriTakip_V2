using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspnetCoreStarter.Migrations
{
    /// <inheritdoc />
    public partial class AddBildirimEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bildirimler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Baslik = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Icerik = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Tur = table.Column<int>(type: "int", nullable: false),
                    Durum = table.Column<int>(type: "int", nullable: false),
                    KullaniciId = table.Column<int>(type: "int", nullable: true),
                    OkunmaZamani = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SonGosterimZamani = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EkVeri = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    HedefUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Oncelik = table.Column<int>(type: "int", nullable: false),
                    SonGecerlilikTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Silinebilir = table.Column<bool>(type: "bit", nullable: false),
                    OtomatikKapat = table.Column<bool>(type: "bit", nullable: false),
                    OtomatikKapatmaSuresi = table.Column<int>(type: "int", nullable: false),
                    BildirimType = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    IslemTuru = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EntityTuru = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    EntityAdi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Basarili = table.Column<bool>(type: "bit", nullable: true),
                    HataMesaji = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MusteriId = table.Column<int>(type: "int", nullable: true),
                    MusteriBildirimi_IslemTuru = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MusteriAdi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IslemYapan = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Kategori = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TumKullanicilara = table.Column<bool>(type: "bit", nullable: true),
                    YayinlanmaZamani = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GosterimBaslangic = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GosterimBitis = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AnaSayfadaGoster = table.Column<bool>(type: "bit", nullable: true),
                    FormAdi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AlanAdi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ValidationMesaji = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bildirimler", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bildirimler");
        }
    }
}
