using AspnetCoreStarter.Models;

namespace AspnetCoreStarter.Services.Interfaces
{
    public interface IBildirimService
    {
        // Bildirim oluşturma
        Task<IslemBildirimi> CreateSuccessBildirimAsync(string islemTuru, string entityTuru, int? entityId = null, string? entityAdi = null);
        Task<IslemBildirimi> CreateErrorBildirimAsync(string islemTuru, string entityTuru, string hataMesaji, int? entityId = null, string? entityAdi = null);
        Task<ValidationBildirimi> CreateValidationBildirimAsync(string formAdi, string alanAdi, string validationMesaji);
        Task<SistemBildirimi> CreateSistemBildirimAsync(string baslik, string icerik, string? kategori = null, bool tumKullanicilara = true);
        Task<MusteriBildirimi> CreateMusteriBildirimAsync(int musteriId, string musteriAdi, string islemTuru, string? islemYapan = null);

        // Bildirim yönetimi
        Task<List<T>> GetAktifBildirimlerAsync<T>() where T : BaseBildirim;
        Task<List<BaseBildirim>> GetKullaniciBildirimlerAsync(int? kullaniciId = null);
        Task<bool> BildirimeOkunmuOlarakIsaretleAsync(int bildirimId);
        Task<bool> BildirimiSilAsync(int bildirimId);
        Task<bool> TumBildirimleriOkunmuOlarakIsaretleAsync(int? kullaniciId = null);

        // JSON formatında bildirimler
        Task<string> GetBildirimlerJsonAsync(int? kullaniciId = null);
        Task<string> GetSonBildirimlerJsonAsync(int adet = 5, int? kullaniciId = null);
        
        // İstatistikler
        Task<int> GetOkunmamisBildirimSayisiAsync(int? kullaniciId = null);
        Task<Dictionary<BildirimTuru, int>> GetBildirimIstatistikleriAsync(int? kullaniciId = null);
    }
}
