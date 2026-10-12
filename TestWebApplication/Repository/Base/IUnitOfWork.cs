using TestWebApplication.Models;

namespace TestWebApplication.Repository.Base
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Category> categories { get; }

        IRepository<Item> items { get; }

        IEmpRepo employees { get; }

        IRepository<StockTransaction> stockTransactions { get; }

        int CommitChanges();
    }
}
