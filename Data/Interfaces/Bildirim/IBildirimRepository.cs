using AspnetCoreStarter.Models;

namespace AspnetCoreStarter.Data.Interfaces.Bildirim
{
    public interface IBildirimRepository : IRepository<BaseBildirim>
    {
        Task<List<BaseBildirim>> GetOkunmamisBildirimlerAsync(int? kullaniciId = null);
        Task<List<BaseBildirim>> GetKullaniciBildirimlerAsync(int? kullaniciId = null, int sayfa = 1, int sayfaBoyutu = 10);
        Task<int> GetOkunmamisSayisiAsync(int? kullaniciId = null);
        Task<Dictionary<BildirimTuru, int>> GetBildirimIstatistikleriAsync(int? kullaniciId = null);
        Task<bool> TumunuOkunduIsaretleAsync(int? kullaniciId = null);
        Task<bool> OkunduIsaretleAsync(int bildirimId);
        Task<List<T>> GetBelirliTipBildirimlerAsync<T>(int? kullaniciId = null) where T : BaseBildirim;
    }
}
