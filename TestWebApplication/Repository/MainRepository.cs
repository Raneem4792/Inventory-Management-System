using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TestWebApplication.Data;
using TestWebApplication.Models;
using TestWebApplication.Repository.Base;

namespace TestWebApplication.Repository
{
    public class MainRepository<T> : IRepository<T> where T : class
    {
        public MainRepository(AppDbContext context)
        {
            _context = context;
        }

        protected readonly AppDbContext _context;

        public T FindById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public T SelectOne(Expression<Func<T, bool>> match)
        {
            return _context.Set<T>().FirstOrDefault(match);
        }

        public IEnumerable<T> FindAll()
        {
            return _context.Set<T>().ToList();
        }

        public async Task<T> FindByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> FindAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public IEnumerable<T> FindAll(params string[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return query.ToList();
        }

        public async Task<IEnumerable<T>> FindAllAsync(params string[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.ToListAsync();
        }

        //=========================================================================//

        public void AddOne(T myItem)
        {
            _context.Set<T>().Add(myItem);
            _context.SaveChanges();
        }

        public void UpdateOne(T myItem)
        {
            _context.Set<T>().Update(myItem);
            _context.SaveChanges();
        }

        public void DeleteOne(T myItem)
        {
            _context.Set<T>().Remove(myItem);
            _context.SaveChanges();
        }

        public void AddList(IEnumerable<T> myList)
        {
            _context.Set<T>().AddRange(myList);
            _context.SaveChanges();
        }

        public void UpdateList(IEnumerable<T> myList)
        {
            _context.Set<T>().UpdateRange(myList);
            _context.SaveChanges();
        }

        public void DeleteList(IEnumerable<T> myList)
        {
            _context.Set<T>().RemoveRange(myList);
            _context.SaveChanges();
        }
    }
}
