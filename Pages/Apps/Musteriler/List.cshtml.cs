using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AspnetCoreStarter.Data.Interfaces;
using AspnetCoreStarter.Models;
using AspnetCoreStarter.Services.Interfaces;

namespace AspnetCoreStarter.Pages.Apps.Musteriler
{
    [ValidateAntiForgeryToken]
    public class ListModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBildirimService _bildirimService;

        public ListModel(IUnitOfWork unitOfWork, IBildirimService bildirimService)
        {
            _unitOfWork = unitOfWork;
            _bildirimService = bildirimService;
        }

        public IList<Musteri> Musteriler { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var musteriler = await _unitOfWork.Musteriler.GetAllAsync();
            Musteriler = musteriler.ToList();
        }

        public async Task<IActionResult> OnGetDataAsync()
        {
            try
            {
                var musteriler = await _unitOfWork.Musteriler.GetAllAsync();
                
                var data = musteriler.Select(m => new
                {
                    id = m.Id,
                    adSoyad = m.AdSoyad,
                    email = m.Email,
                    telefon = m.Telefon ?? "",
                    adres = m.Adres ?? "",
                    aktif = m.Aktif,
                    durumText = m.DurumText,
                    createDate = m.CreateDate.ToString("dd.MM.yyyy")
                });

                return new JsonResult(new { data });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAddAsync(string adSoyad, string email, string telefon, string adres, bool aktif)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(adSoyad) || string.IsNullOrWhiteSpace(email))
                {
                    return new JsonResult(new { success = false, message = "Ad Soyad ve E-posta alanları zorunludur!" });
                }

                // E-posta kontrolü
                var existingMusteri = await _unitOfWork.Musteriler.GetByEmailAsync(email.Trim().ToLower());
                if (existingMusteri != null)
                {
                    return new JsonResult(new { success = false, message = "Bu e-posta adresi zaten kayıtlı!" });
                }

                var yeniMusteri = new Musteri
                {
                    AdSoyad = adSoyad.Trim(),
                    Email = email.Trim().ToLower(),
                    Telefon = string.IsNullOrWhiteSpace(telefon) ? null : telefon.Trim(),
                    Adres = string.IsNullOrWhiteSpace(adres) ? null : adres.Trim(),
                    Aktif = aktif,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                };

                await _unitOfWork.Musteriler.AddAsync(yeniMusteri);
                await _unitOfWork.SaveChangesAsync();

                // Başarı bildirimi oluştur (sadece müşteri bildirimi)
                await _bildirimService.CreateMusteriBildirimAsync(yeniMusteri.Id, yeniMusteri.AdSoyad, "Eklendi");

                return new JsonResult(new { success = true, message = "Müşteri başarıyla eklendi!" });
            }
            catch (Exception ex)
            {
                // İç exception'ı da logla
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                
                // Hata bildirimi oluştur
                await _bildirimService.CreateErrorBildirimAsync("Create", "Müşteri", innerMessage);
                
                return new JsonResult(new { success = false, message = $"Kayıt hatası: {innerMessage}" });
            }
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostEditAsync(int id, string adSoyad, string email, string telefon, string adres, bool aktif)
        {
            try
            {
                var musteri = await _unitOfWork.Musteriler.GetByIdAsync(id);
                if (musteri == null)
                {
                    return new JsonResult(new { success = false, message = "Müşteri bulunamadı!" });
                }

                if (string.IsNullOrWhiteSpace(adSoyad) || string.IsNullOrWhiteSpace(email))
                {
                    return new JsonResult(new { success = false, message = "Ad Soyad ve E-posta alanları zorunludur!" });
                }

                // E-posta kontrolü (kendisi hariç)
                var existingMusteri = await _unitOfWork.Musteriler.GetByEmailAsync(email.Trim().ToLower());
                if (existingMusteri != null && existingMusteri.Id != id)
                {
                    return new JsonResult(new { success = false, message = "Bu e-posta adresi zaten başka bir müşteri tarafından kullanılıyor!" });
                }

                musteri.AdSoyad = adSoyad.Trim();
                musteri.Email = email.Trim().ToLower();
                musteri.Telefon = telefon?.Trim();
                musteri.Adres = adres?.Trim();
                musteri.Aktif = aktif;
                musteri.UpdateDate = DateTime.Now;

                _unitOfWork.Musteriler.Update(musteri);
                await _unitOfWork.SaveChangesAsync();

                // Başarı bildirimi oluştur (sadece müşteri bildirimi)
                await _bildirimService.CreateMusteriBildirimAsync(musteri.Id, musteri.AdSoyad, "Güncellendi");

                return new JsonResult(new { success = true, message = "Müşteri başarıyla güncellendi!" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                var musteri = await _unitOfWork.Musteriler.GetByIdAsync(id);
                if (musteri == null)
                {
                    return new JsonResult(new { success = false, message = "Müşteri bulunamadı!" });
                }

                var musteriAdi = musteri.AdSoyad;
                
                _unitOfWork.Musteriler.Remove(musteri);
                await _unitOfWork.SaveChangesAsync();

                // Başarı bildirimi oluştur (sadece müşteri bildirimi)
                await _bildirimService.CreateMusteriBildirimAsync(id, musteriAdi, "Silindi");

                return new JsonResult(new { success = true, message = "Müşteri başarıyla silindi!" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }
    }
}
