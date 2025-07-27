using AspnetCoreStarter.Data.Interfaces.Musteri;
using AspnetCoreStarter.Data.Interfaces.Bildirim;

namespace AspnetCoreStarter.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // Specific repositories for each module
        IMusteriRepository Musteriler { get; }
        IBildirimRepository Bildirimler { get; }
        
        // Future modules can be added here:
        // ISiparisRepository Siparisler { get; }
        // IUrunRepository Urunler { get; }
        
        // Transaction management
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
