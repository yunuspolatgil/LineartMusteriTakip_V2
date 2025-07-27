using AspnetCoreStarter.Models;

namespace AspnetCoreStarter.Data.Interfaces.Musteri
{
    public interface IMusteriRepository : IRepository<Models.Musteri>
    {
        Task<IEnumerable<Models.Musteri>> GetAktifMusterilerAsync();
        Task<IEnumerable<Models.Musteri>> GetPasifMusterilerAsync();
        Task<Models.Musteri?> GetByEmailAsync(string email);
        Task<Models.Musteri?> GetByTelefonAsync(string telefon);
        Task<bool> EmailExistsAsync(string email, int? excludeId = null);
        Task<bool> TelefonExistsAsync(string telefon, int? excludeId = null);
        Task<IEnumerable<Models.Musteri>> SearchAsync(string searchTerm);
    }
}
