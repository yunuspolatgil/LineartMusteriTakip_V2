using System.ComponentModel.DataAnnotations;

namespace AspnetCoreStarter.Models
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        
        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        
        [Display(Name = "Güncellenme Tarihi")]
        public DateTime? UpdateDate { get; set; }
    }
}
