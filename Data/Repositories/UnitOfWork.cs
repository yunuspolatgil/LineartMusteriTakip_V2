using Microsoft.EntityFrameworkCore.Storage;
using AspnetCoreStarter.Data.Interfaces;
using AspnetCoreStarter.Data.Interfaces.Musteri;
using AspnetCoreStarter.Data.Interfaces.Bildirim;

namespace AspnetCoreStarter.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;

        // Repository instances injected via constructor
        public IMusteriRepository Musteriler { get; }
        public IBildirimRepository Bildirimler { get; }

        public UnitOfWork(ApplicationDbContext context, IMusteriRepository musteriRepository, IBildirimRepository bildirimRepository)
        {
            _context = context;
            Musteriler = musteriRepository;
            Bildirimler = bildirimRepository;
        }

        // Transaction management
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
