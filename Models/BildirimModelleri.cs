using System.ComponentModel.DataAnnotations;

namespace AspnetCoreStarter.Models
{
    // Sistem Bildirimleri (Genel bildirimler, güncellemeler vs.)
    public class SistemBildirimi : BaseBildirim
    {
        public SistemBildirimi()
        {
            Tur = BildirimTuru.Sistem;
        }

        [MaxLength(50)]
        public string? Kategori { get; set; } // "Güncelleme", "Bakım", "Duyuru" vs.

        public bool TumKullanicilara { get; set; } = true;

        public DateTime? YayinlanmaZamani { get; set; }

        public DateTime? GosterimBaslangic { get; set; }
        
        public DateTime? GosterimBitis { get; set; }

        public bool AnaSayfadaGoster { get; set; } = false;

        public static SistemBildirimi Create(string baslik, string icerik, string? kategori = null, bool tumKullanicilara = true)
        {
            return new SistemBildirimi
            {
                Baslik = baslik,
                Icerik = icerik,
                Kategori = kategori,
                TumKullanicilara = tumKullanicilara,
                Tur = BildirimTuru.Sistem,
                Oncelik = 1,
                YayinlanmaZamani = DateTime.Now,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now
            };
        }
    }

    // Müşteri İşlemleri Bildirimleri
    public class MusteriBildirimi : BaseBildirim
    {
        public MusteriBildirimi()
        {
            Tur = BildirimTuru.Bilgi;
        }

        public int MusteriId { get; set; }
        
        [MaxLength(50)]
        public string IslemTuru { get; set; } = string.Empty; // "Ekleme", "Güncelleme", "Silme"

        [MaxLength(100)]
        public string MusteriAdi { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? IslemYapan { get; set; }

        public static MusteriBildirimi Create(int musteriId, string musteriAdi, string islemTuru, string? islemYapan = null)
        {
            return new MusteriBildirimi
            {
                MusteriId = musteriId,
                MusteriAdi = musteriAdi,
                IslemTuru = islemTuru,
                IslemYapan = islemYapan,
                Baslik = $"Müşteri {islemTuru}",
                Icerik = $"{musteriAdi} müşterisi {islemTuru.ToLower()} işlemi gerçekleştirildi.",
                Tur = BildirimTuru.Bilgi,
                Oncelik = 2,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now
            };
        }
    }

    // İşlem Sonucu Bildirimleri (CRUD işlemler için)
    public class IslemBildirimi : BaseBildirim
    {
        public IslemBildirimi()
        {
            OtomatikKapat = true;
            OtomatikKapatmaSuresi = 3000;
        }

        [MaxLength(50)]
        public string IslemTuru { get; set; } = string.Empty; // "Create", "Update", "Delete"

        [MaxLength(50)]
        public string EntityTuru { get; set; } = string.Empty; // "Musteri", "Kullanici" vs.

        public int? EntityId { get; set; }

        [MaxLength(200)]
        public string? EntityAdi { get; set; }

        public bool Basarili { get; set; }

        [MaxLength(500)]
        public string? HataMesaji { get; set; }

        public static IslemBildirimi CreateSuccess(string islemTuru, string entityTuru, int? entityId = null, string? entityAdi = null)
        {
            return new IslemBildirimi
            {
                Tur = BildirimTuru.Basari,
                IslemTuru = islemTuru,
                EntityTuru = entityTuru,
                EntityId = entityId,
                EntityAdi = entityAdi,
                Basarili = true,
                Baslik = $"{entityTuru} {GetIslemText(islemTuru)}",
                Icerik = $"{entityAdi ?? entityTuru} başarıyla {GetIslemText(islemTuru).ToLower()}.",
                Oncelik = 2,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now
            };
        }

        public static IslemBildirimi CreateError(string islemTuru, string entityTuru, string hataMesaji, int? entityId = null, string? entityAdi = null)
        {
            return new IslemBildirimi
            {
                Tur = BildirimTuru.Hata,
                IslemTuru = islemTuru,
                EntityTuru = entityTuru,
                EntityId = entityId,
                EntityAdi = entityAdi,
                Basarili = false,
                HataMesaji = hataMesaji,
                Baslik = $"{entityTuru} {GetIslemText(islemTuru)} Hatası",
                Icerik = $"{entityAdi ?? entityTuru} {GetIslemText(islemTuru).ToLower()} sırasında hata oluştu: {hataMesaji}",
                Oncelik = 3,
                OtomatikKapat = false,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now
            };
        }

        private static string GetIslemText(string islemTuru) => islemTuru.ToLower() switch
        {
            "create" => "Eklendi",
            "update" => "Güncellendi", 
            "delete" => "Silindi",
            _ => "İşlendi"
        };
    }

    // Validation Bildirimleri (Form hataları için)
    public class ValidationBildirimi : BaseBildirim
    {
        public ValidationBildirimi()
        {
            Tur = BildirimTuru.Uyari;
            OtomatikKapat = false;
            Silinebilir = true;
        }

        [MaxLength(50)]
        public string FormAdi { get; set; } = string.Empty;

        [MaxLength(50)]
        public string AlanAdi { get; set; } = string.Empty;

        [MaxLength(500)]
        public string ValidationMesaji { get; set; } = string.Empty;

        public static ValidationBildirimi Create(string formAdi, string alanAdi, string validationMesaji)
        {
            return new ValidationBildirimi
            {
                FormAdi = formAdi,
                AlanAdi = alanAdi,
                ValidationMesaji = validationMesaji,
                Baslik = "Form Doğrulama Hatası",
                Icerik = $"{alanAdi}: {validationMesaji}",
                Oncelik = 2,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now
            };
        }
    }
}
