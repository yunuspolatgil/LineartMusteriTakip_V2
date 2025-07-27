using AspnetCoreStarter.Models;
using AspnetCoreStarter.Services.Interfaces;
using AspnetCoreStarter.Data.Interfaces;
using System.Text.Json;

namespace AspnetCoreStarter.Services
{
    public class BildirimService : IBildirimService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BildirimService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IslemBildirimi> CreateSuccessBildirimAsync(string islemTuru, string entityTuru, int? entityId = null, string? entityAdi = null)
        {
            var bildirim = IslemBildirimi.CreateSuccess(islemTuru, entityTuru, entityId, entityAdi);
            
            await _unitOfWork.Bildirimler.AddAsync(bildirim);
            await _unitOfWork.SaveChangesAsync();

            return bildirim;
        }

        public async Task<IslemBildirimi> CreateErrorBildirimAsync(string islemTuru, string entityTuru, string hataMesaji, int? entityId = null, string? entityAdi = null)
        {
            var bildirim = IslemBildirimi.CreateError(islemTuru, entityTuru, hataMesaji, entityId, entityAdi);
            
            await _unitOfWork.Bildirimler.AddAsync(bildirim);
            await _unitOfWork.SaveChangesAsync();

            return bildirim;
        }

        public async Task<ValidationBildirimi> CreateValidationBildirimAsync(string formAdi, string alanAdi, string validationMesaji)
        {
            var bildirim = ValidationBildirimi.Create(formAdi, alanAdi, validationMesaji);
            
            await _unitOfWork.Bildirimler.AddAsync(bildirim);
            await _unitOfWork.SaveChangesAsync();

            return bildirim;
        }

        public async Task<SistemBildirimi> CreateSistemBildirimAsync(string baslik, string icerik, string? kategori = null, bool tumKullanicilara = true)
        {
            var bildirim = SistemBildirimi.Create(baslik, icerik, kategori, tumKullanicilara);
            
            await _unitOfWork.Bildirimler.AddAsync(bildirim);
            await _unitOfWork.SaveChangesAsync();

            return bildirim;
        }

        public async Task<MusteriBildirimi> CreateMusteriBildirimAsync(int musteriId, string musteriAdi, string islemTuru, string? islemYapan = null)
        {
            var bildirim = MusteriBildirimi.Create(musteriId, musteriAdi, islemTuru, islemYapan);
            
            await _unitOfWork.Bildirimler.AddAsync(bildirim);
            await _unitOfWork.SaveChangesAsync();

            return bildirim;
        }

        // Bildirim yönetimi
        public async Task<List<T>> GetAktifBildirimlerAsync<T>() where T : BaseBildirim
        {
            return await _unitOfWork.Bildirimler.GetBelirliTipBildirimlerAsync<T>();
        }

        public async Task<List<BaseBildirim>> GetKullaniciBildirimlerAsync(int? kullaniciId = null)
        {
            return await _unitOfWork.Bildirimler.GetKullaniciBildirimlerAsync(kullaniciId);
        }

        public async Task<bool> BildirimeOkunmuOlarakIsaretleAsync(int bildirimId)
        {
            return await _unitOfWork.Bildirimler.OkunduIsaretleAsync(bildirimId);
        }

        public async Task<bool> BildirimiSilAsync(int bildirimId)
        {
            var bildirim = await _unitOfWork.Bildirimler.GetByIdAsync(bildirimId);
            if (bildirim == null) return false;

            _unitOfWork.Bildirimler.Remove(bildirim);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> TumBildirimleriOkunmuOlarakIsaretleAsync(int? kullaniciId = null)
        {
            return await _unitOfWork.Bildirimler.TumunuOkunduIsaretleAsync(kullaniciId);
        }

        // JSON formatında bildirimler
        public async Task<string> GetBildirimlerJsonAsync(int? kullaniciId = null)
        {
            var bildirimler = await GetKullaniciBildirimlerAsync(kullaniciId);
            return JsonSerializer.Serialize(bildirimler, new JsonSerializerOptions 
            { 
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });
        }

        public async Task<string> GetSonBildirimlerJsonAsync(int adet = 5, int? kullaniciId = null)
        {
            var bildirimler = await _unitOfWork.Bildirimler.GetKullaniciBildirimlerAsync(kullaniciId, 1, adet);
            return JsonSerializer.Serialize(bildirimler, new JsonSerializerOptions 
            { 
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });
        }

        // İstatistikler
        public async Task<int> GetOkunmamisBildirimSayisiAsync(int? kullaniciId = null)
        {
            return await _unitOfWork.Bildirimler.GetOkunmamisSayisiAsync(kullaniciId);
        }

        public async Task<Dictionary<BildirimTuru, int>> GetBildirimIstatistikleriAsync(int? kullaniciId = null)
        {
            return await _unitOfWork.Bildirimler.GetBildirimIstatistikleriAsync(kullaniciId);
        }
    }
}
