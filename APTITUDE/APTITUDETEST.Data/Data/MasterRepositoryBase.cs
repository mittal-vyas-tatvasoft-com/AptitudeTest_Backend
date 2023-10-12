using APTITUDETEST.Common.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace AptitudeTest.Data.Data
{
    public class MasterRepositoryBase<T> : RepositoryBase<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected DbSet<T> _dbSet;
        public MasterRepositoryBase(AppDbContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public int CheckUncheck(Expression<Func<T, bool>> filter, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> expression)
        {
            return _dbSet.Where(filter).ExecuteUpdate(expression);
        }
    }
}
