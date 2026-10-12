using TestWebApplication.Data;
using TestWebApplication.Models;
using TestWebApplication.Repository.Base;

namespace TestWebApplication.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(AppDbContext context) 
        { 
            _context = context;
            categories = new MainRepository<Category>(_context);
            items = new MainRepository<Item>(_context);
            employees = new EmpRepo(_context);
            stockTransactions = new MainRepository<StockTransaction>(_context);

        }

        private readonly AppDbContext _context;

        public IRepository<Category> categories { get; private set; }

        public IRepository<Item> items { get; private set; }

        public IEmpRepo employees { get; private set; }

        public IRepository<StockTransaction> stockTransactions { get; private set; }

        public int CommitChanges()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
