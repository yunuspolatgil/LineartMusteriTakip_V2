using System.ComponentModel.DataAnnotations;

namespace AspnetCoreStarter.Models
{
    public abstract class BaseBildirim : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string Baslik { get; set; } = string.Empty;

        [Required]
        [MaxLength(1000)]
        public string Icerik { get; set; } = string.Empty;

        [Required]
        public BildirimTuru Tur { get; set; }

        [Required]
        public BildirimDurumu Durum { get; set; } = BildirimDurumu.Okunmamis;

        public int? KullaniciId { get; set; }

        public DateTime? OkunmaZamani { get; set; }

        public DateTime? SonGosterimZamani { get; set; }

        [MaxLength(500)]
        public string? EkVeri { get; set; } // JSON format için

        [MaxLength(200)]
        public string? HedefUrl { get; set; }

        [MaxLength(100)]
        public string? Icon { get; set; }

        public int Oncelik { get; set; } = 1; // 1: Düşük, 2: Normal, 3: Yüksek, 4: Kritik

        public DateTime? SonGecerlilikTarihi { get; set; }

        public bool Silinebilir { get; set; } = true;

        public bool OtomatikKapat { get; set; } = true;

        public int OtomatikKapatmaSuresi { get; set; } = 5000; // ms

        // Computed Properties
        public string TurText => Tur switch
        {
            BildirimTuru.Basari => "Başarı",
            BildirimTuru.Hata => "Hata",
            BildirimTuru.Uyari => "Uyarı",
            BildirimTuru.Bilgi => "Bilgi",
            BildirimTuru.Sistem => "Sistem",
            _ => "Bilinmeyen"
        };

        public string DurumText => Durum switch
        {
            BildirimDurumu.Okunmamis => "Okunmamış",
            BildirimDurumu.Okunmus => "Okunmuş",
            BildirimDurumu.Arsivlenmis => "Arşivlenmiş",
            BildirimDurumu.Silindi => "Silindi",
            _ => "Bilinmeyen"
        };

        public string OncelikText => Oncelik switch
        {
            1 => "Düşük",
            2 => "Normal",
            3 => "Yüksek",
            4 => "Kritik",
            _ => "Normal"
        };

        public string OncelikCssClass => Oncelik switch
        {
            1 => "text-secondary",
            2 => "text-primary",
            3 => "text-warning",
            4 => "text-danger",
            _ => "text-primary"
        };

        public string TurCssClass => Tur switch
        {
            BildirimTuru.Basari => "alert-success",
            BildirimTuru.Hata => "alert-danger",
            BildirimTuru.Uyari => "alert-warning",
            BildirimTuru.Bilgi => "alert-info",
            BildirimTuru.Sistem => "alert-primary",
            _ => "alert-secondary"
        };

        public string TurBadgeClass => Tur switch
        {
            BildirimTuru.Basari => "bg-label-success",
            BildirimTuru.Hata => "bg-label-danger",
            BildirimTuru.Uyari => "bg-label-warning",
            BildirimTuru.Bilgi => "bg-label-info",
            BildirimTuru.Sistem => "bg-label-primary",
            _ => "bg-label-secondary"
        };

        public string DefaultIcon => Tur switch
        {
            BildirimTuru.Basari => "ti ti-check-circle",
            BildirimTuru.Hata => "ti ti-x-circle",
            BildirimTuru.Uyari => "ti ti-alert-triangle",
            BildirimTuru.Bilgi => "ti ti-info-circle",
            BildirimTuru.Sistem => "ti ti-settings",
            _ => "ti ti-bell"
        };

        public string GetIcon() => !string.IsNullOrEmpty(Icon) ? Icon : DefaultIcon;

        public bool IsExpired => SonGecerlilikTarihi.HasValue && SonGecerlilikTarihi < DateTime.Now;

        public bool IsActive => Durum == BildirimDurumu.Okunmamis && !IsExpired;

        public string GetJsonData()
        {
            return System.Text.Json.JsonSerializer.Serialize(new
            {
                id = Id,
                baslik = Baslik,
                icerik = Icerik,
                tur = Tur.ToString(),
                durum = Durum.ToString(),
                turText = TurText,
                durumText = DurumText,
                oncelik = Oncelik,
                oncelikText = OncelikText,
                icon = GetIcon(),
                turCssClass = TurCssClass,
                turBadgeClass = TurBadgeClass,
                oncelikCssClass = OncelikCssClass,
                hedefUrl = HedefUrl,
                otomatikKapat = OtomatikKapat,
                otomatikKapatmaSuresi = OtomatikKapatmaSuresi,
                silinebilir = Silinebilir,
                createDate = CreateDate.ToString("dd.MM.yyyy HH:mm"),
                okunmaZamani = OkunmaZamani?.ToString("dd.MM.yyyy HH:mm"),
                isExpired = IsExpired,
                isActive = IsActive
            });
        }
    }

    public enum BildirimTuru
    {
        Basari = 1,
        Hata = 2,
        Uyari = 3,
        Bilgi = 4,
        Sistem = 5
    }

    public enum BildirimDurumu
    {
        Okunmamis = 1,
        Okunmus = 2,
        Arsivlenmis = 3,
        Silindi = 4
    }
}
