using AptitudeTest.Core.Interfaces;
using APTITUDETEST.Common.Data;
using Microsoft.EntityFrameworkCore;

namespace AptitudeTest.Data
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly AppDbContext _context;
        public RepositoryBase(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<T>> GetAll() => _context.Set<T>().AsNoTracking();

        public async Task<T> GetById(int id) => await _context.Set<T>().FindAsync(id);
        public void Create(T entity) => _context.Set<T>().Add(entity);

        public void Update(T entity) => _context.Set<T>().Update(entity);

        public void Delete(T entity) => _context.Set<T>().Remove(entity);

    }
}
