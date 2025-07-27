using Microsoft.EntityFrameworkCore;
using AspnetCoreStarter.Models;

namespace AspnetCoreStarter.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<Musteri> Musteriler { get; set; }
        public DbSet<BaseBildirim> Bildirimler { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Musteri entity konfigürasyonu
            modelBuilder.Entity<Musteri>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.AdSoyad)
                    .IsRequired()
                    .HasMaxLength(100);
                
                entity.Property(e => e.Telefon)
                    .HasMaxLength(20);
                
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255);
                
                entity.Property(e => e.Adres)
                    .HasMaxLength(500);
                
                entity.Property(e => e.CreateDate)
                    .IsRequired()
                    .HasDefaultValueSql("GETDATE()");
                
                entity.Property(e => e.UpdateDate)
                    .IsRequired(false);

                // Unique constraints
                entity.HasIndex(e => e.Email).IsUnique();
                
                // Table name
                entity.ToTable("Musteriler");
            });

            // Seed data (opsiyonel)
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Musteri>().HasData(
                new Musteri
                {
                    Id = 1,
                    AdSoyad = "Ahmet Yılmaz",
                    Email = "ahmet.yilmaz@email.com",
                    Telefon = "0532-123-4567",
                    Adres = "İstanbul, Kadıköy",
                    Aktif = true,
                    CreateDate = new DateTime(2024, 1, 1, 10, 0, 0)
                },
                new Musteri
                {
                    Id = 2,
                    AdSoyad = "Ayşe Kaya",
                    Email = "ayse.kaya@email.com",
                    Telefon = "0533-987-6543",
                    Adres = "Ankara, Çankaya",
                    Aktif = true,
                    CreateDate = new DateTime(2024, 1, 2, 11, 0, 0)
                }
            );

            // BaseBildirim entity konfigürasyonu
            modelBuilder.Entity<BaseBildirim>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Baslik).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Icerik).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.Tur).IsRequired();
                entity.Property(e => e.Durum).IsRequired();
                entity.Property(e => e.CreateDate).IsRequired();
                entity.Property(e => e.KullaniciId).IsRequired(false);

                // Discriminator for inheritance
                entity.HasDiscriminator<string>("BildirimType")
                    .HasValue<SistemBildirimi>("Sistem")
                    .HasValue<MusteriBildirimi>("Musteri")
                    .HasValue<IslemBildirimi>("Islem")
                    .HasValue<ValidationBildirimi>("Validation");
            });
        }
    }
}
