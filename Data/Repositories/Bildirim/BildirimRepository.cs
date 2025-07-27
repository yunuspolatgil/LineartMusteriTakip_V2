using Microsoft.EntityFrameworkCore;
using AspnetCoreStarter.Data.Interfaces.Bildirim;
using AspnetCoreStarter.Models;

namespace AspnetCoreStarter.Data.Repositories.Bildirim
{
    public class BildirimRepository : Repository<BaseBildirim>, IBildirimRepository
    {
        public BildirimRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<BaseBildirim>> GetOkunmamisBildirimlerAsync(int? kullaniciId = null)
        {
            var query = _context.Bildirimler
                .Where(b => b.Durum == BildirimDurumu.Okunmamis);

            if (kullaniciId.HasValue)
            {
                query = query.Where(b => b.KullaniciId == kullaniciId || b.KullaniciId == null);
            }
            else
            {
                query = query.Where(b => b.KullaniciId == null);
            }

            return await query
                .OrderByDescending(b => b.CreateDate)
                .ToListAsync();
        }

        public async Task<List<BaseBildirim>> GetKullaniciBildirimlerAsync(int? kullaniciId = null, int sayfa = 1, int sayfaBoyutu = 10)
        {
            var query = _context.Bildirimler.AsQueryable();

            if (kullaniciId.HasValue)
            {
                query = query.Where(b => b.KullaniciId == kullaniciId || b.KullaniciId == null);
            }
            else
            {
                query = query.Where(b => b.KullaniciId == null);
            }

            return await query
                .OrderByDescending(b => b.CreateDate)
                .Skip((sayfa - 1) * sayfaBoyutu)
                .Take(sayfaBoyutu)
                .ToListAsync();
        }

        public async Task<int> GetOkunmamisSayisiAsync(int? kullaniciId = null)
        {
            var query = _context.Bildirimler
                .Where(b => b.Durum == BildirimDurumu.Okunmamis);

            if (kullaniciId.HasValue)
            {
                query = query.Where(b => b.KullaniciId == kullaniciId || b.KullaniciId == null);
            }
            else
            {
                query = query.Where(b => b.KullaniciId == null);
            }

            return await query.CountAsync();
        }

        public async Task<Dictionary<BildirimTuru, int>> GetBildirimIstatistikleriAsync(int? kullaniciId = null)
        {
            var query = _context.Bildirimler.AsQueryable();

            if (kullaniciId.HasValue)
            {
                query = query.Where(b => b.KullaniciId == kullaniciId || b.KullaniciId == null);
            }
            else
            {
                query = query.Where(b => b.KullaniciId == null);
            }

            var istatistikler = await query
                .GroupBy(b => b.Tur)
                .Select(g => new { Tur = g.Key, Sayisi = g.Count() })
                .ToListAsync();

            return istatistikler.ToDictionary(x => x.Tur, x => x.Sayisi);
        }

        public async Task<bool> TumunuOkunduIsaretleAsync(int? kullaniciId = null)
        {
            var query = _context.Bildirimler
                .Where(b => b.Durum == BildirimDurumu.Okunmamis);

            if (kullaniciId.HasValue)
            {
                query = query.Where(b => b.KullaniciId == kullaniciId || b.KullaniciId == null);
            }
            else
            {
                query = query.Where(b => b.KullaniciId == null);
            }

            var bildirimler = await query.ToListAsync();
            
            foreach (var bildirim in bildirimler)
            {
                bildirim.Durum = BildirimDurumu.Okunmus;
                bildirim.OkunmaZamani = DateTime.Now;
                bildirim.UpdateDate = DateTime.Now;
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> OkunduIsaretleAsync(int bildirimId)
        {
            var bildirim = await _context.Bildirimler.FindAsync(bildirimId);
            
            if (bildirim == null) return false;

            bildirim.Durum = BildirimDurumu.Okunmus;
            bildirim.OkunmaZamani = DateTime.Now;
            bildirim.UpdateDate = DateTime.Now;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<T>> GetBelirliTipBildirimlerAsync<T>(int? kullaniciId = null) where T : BaseBildirim
        {
            var query = _context.Bildirimler.OfType<T>();

            if (kullaniciId.HasValue)
            {
                query = query.Where(b => b.KullaniciId == kullaniciId || b.KullaniciId == null);
            }
            else
            {
                query = query.Where(b => b.KullaniciId == null);
            }

            return await query
                .OrderByDescending(b => b.CreateDate)
                .ToListAsync();
        }
    }
}
