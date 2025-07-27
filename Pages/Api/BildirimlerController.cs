using Microsoft.AspNetCore.Mvc;
using AspnetCoreStarter.Services.Interfaces;
using AspnetCoreStarter.Models;

namespace AspnetCoreStarter.Pages.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class BildirimlerController : ControllerBase
    {
        private readonly IBildirimService _bildirimService;

        public BildirimlerController(IBildirimService bildirimService)
        {
            _bildirimService = bildirimService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBildirimler(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? bildirimTuru = null,
            [FromQuery] string? bildirimDurumu = null)
        {
            try
            {
                var bildirimler = await _bildirimService.GetKullaniciBildirimlerAsync();
                
                // Enum değerlerini string'e çevir
                var bildirimlerDto = bildirimler.Select(b => new
                {
                    id = b.Id,
                    baslik = b.Baslik,
                    icerik = b.Icerik,
                    tur = b.Tur.ToString(),
                    durum = b.Durum.ToString(),
                    createDate = b.CreateDate,
                    updateDate = b.UpdateDate,
                    icon = b.Icon,
                    hedefUrl = b.HedefUrl,
                    kullaniciId = b.KullaniciId,
                    okunmaZamani = b.OkunmaZamani
                });
                
                return Ok(bildirimlerDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("okunmamis")]
        public async Task<IActionResult> GetOkunmamiaBildirimler()
        {
            try
            {
                var bildirimler = await _bildirimService.GetKullaniciBildirimlerAsync();
                var okunmamisBildirimler = bildirimler.Where(b => b.Durum == BildirimDurumu.Okunmamis).ToList();
                
                // Enum değerlerini string'e çevir
                var bildirimlerDto = okunmamisBildirimler.Select(b => new
                {
                    id = b.Id,
                    baslik = b.Baslik,
                    icerik = b.Icerik,
                    tur = b.Tur.ToString(),
                    durum = b.Durum.ToString(),
                    createDate = b.CreateDate,
                    updateDate = b.UpdateDate,
                    icon = b.Icon,
                    hedefUrl = b.HedefUrl,
                    kullaniciId = b.KullaniciId,
                    okunmaZamani = b.OkunmaZamani
                });
                
                return Ok(bildirimlerDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("sayilar")]
        public async Task<IActionResult> GetBildirimSayilari()
        {
            try
            {
                var okunmamisSayi = await _bildirimService.GetOkunmamisBildirimSayisiAsync();
                var istatistikler = await _bildirimService.GetBildirimIstatistikleriAsync();
                return Ok(new { okunmamisSayisi = okunmamisSayi, istatistikler = istatistikler });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("{id}/okundu")]
        public async Task<IActionResult> BildirimOkunduIsaretle(int id)
        {
            try
            {
                await _bildirimService.BildirimeOkunmuOlarakIsaretleAsync(id);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("tumunu-okundu")]
        public async Task<IActionResult> TumBildirimleriOkunduIsaretle()
        {
            try
            {
                await _bildirimService.TumBildirimleriOkunmuOlarakIsaretleAsync();
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> BildirimSil(int id)
        {
            try
            {
                await _bildirimService.BildirimiSilAsync(id);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("test")]
        public async Task<IActionResult> TestBildirimiOlustur()
        {
            try
            {
                var bildirim = await _bildirimService.CreateSistemBildirimAsync(
                    "Test Bildirimi", 
                    $"Bu bir test bildirimidir - {DateTime.Now:HH:mm:ss}",
                    "Test"
                );
                return Ok(new { success = true, bildirim = bildirim });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
