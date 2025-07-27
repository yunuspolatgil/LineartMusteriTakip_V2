using Microsoft.EntityFrameworkCore;
using AspnetCoreStarter.Data.Interfaces.Musteri;
using AspnetCoreStarter.Models;

namespace AspnetCoreStarter.Data.Repositories.Musteri
{
    public class MusteriRepository : Repository<Models.Musteri>, IMusteriRepository
    {
        public MusteriRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Models.Musteri>> GetAktifMusterilerAsync()
        {
            return await _dbSet.Where(m => m.Aktif).ToListAsync();
        }

        public async Task<IEnumerable<Models.Musteri>> GetPasifMusterilerAsync()
        {
            return await _dbSet.Where(m => !m.Aktif).ToListAsync();
        }

        public async Task<Models.Musteri?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(m => m.Email == email);
        }

        public async Task<Models.Musteri?> GetByTelefonAsync(string telefon)
        {
            return await _dbSet.FirstOrDefaultAsync(m => m.Telefon == telefon);
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
        {
            var query = _dbSet.Where(m => m.Email == email);
            
            if (excludeId.HasValue)
            {
                query = query.Where(m => m.Id != excludeId.Value);
            }
            
            return await query.AnyAsync();
        }

        public async Task<bool> TelefonExistsAsync(string telefon, int? excludeId = null)
        {
            var query = _dbSet.Where(m => m.Telefon == telefon);
            
            if (excludeId.HasValue)
            {
                query = query.Where(m => m.Id != excludeId.Value);
            }
            
            return await query.AnyAsync();
        }

        public async Task<IEnumerable<Models.Musteri>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllAsync();
            }

            searchTerm = searchTerm.ToLower().Trim();

            return await _dbSet
                .Where(m => 
                    m.AdSoyad.ToLower().Contains(searchTerm) ||
                    m.Email.ToLower().Contains(searchTerm) ||
                    m.Telefon.Contains(searchTerm) ||
                    (m.Adres != null && m.Adres.ToLower().Contains(searchTerm))
                )
                .ToListAsync();
        }
    }
}
