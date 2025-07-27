using System.ComponentModel.DataAnnotations;

namespace AspnetCoreStarter.Models
{
    public class Musteri : BaseEntity
    {
        [Required(ErrorMessage = "Ad Soyad alanı zorunludur")]
        [StringLength(100, ErrorMessage = "Ad Soyad en fazla 100 karakter olabilir")]
        [Display(Name = "Ad Soyad")]
        public string AdSoyad { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Telefon alanı zorunludur")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz")]
        [Display(Name = "Telefon")]
        public string Telefon { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "E-mail alanı zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-mail adresi giriniz")]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = string.Empty;
        
        [StringLength(500, ErrorMessage = "Adres en fazla 500 karakter olabilir")]
        [Display(Name = "Adres")]
        public string? Adres { get; set; }
        
        [Display(Name = "Durum")]
        public bool Aktif { get; set; } = true;
        
        // Hesaplanan özellikler
        [Display(Name = "Durum")]
        public string DurumText => Aktif ? "Aktif" : "Pasif";
    }
}
