using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TrackIt.Core.Interfaces.Repositories;

namespace TrackIt.Core.Infrastructure.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected ApplicationDbContext _appDbContext;

        public RepositoryBase(ApplicationDbContext dataContext)
        {
            _appDbContext = dataContext;
        }

        public IQueryable<T> FindAll(bool trackChanges)
        {
            if (trackChanges)
                return _appDbContext.Set<T>().AsTracking();
            else
                return _appDbContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
        {
            if (trackChanges)
                return _appDbContext.Set<T>().Where(expression).AsTracking();
            else
                return _appDbContext.Set<T>().Where(expression).AsNoTracking();
        }
        public async Task CreateAsync(T entity)
        {
            await _appDbContext.Set<T>().AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _appDbContext.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            _appDbContext.Set<T>().Update(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }
    }
}
