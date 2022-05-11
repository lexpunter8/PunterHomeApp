using DataModels;
using Microsoft.EntityFrameworkCore;
using PunterHomeDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PunterHomeAdapters.Interfaces
{
    public interface IShoppingListQueryRepository
    {
        IEnumerable<ShoppingListModel> GetAll();
    }

    public class BaseRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DbContextOptions<HomeAppDbContext> options;

        public BaseRepository(DbContextOptions<HomeAppDbContext> options)
        {
            this.options = options;
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            using var context = new HomeAppDbContext(options);
            return context.Set<T>()
                .Where(expression)
                .AsNoTracking();
        }

        public IQueryable<T> GetAllAsync()
        {
            using var context = new HomeAppDbContext(options);
            return context.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(ISpecification<T> specification)
        {
            using var context = new HomeAppDbContext(options);
            return context.Set<T>()
                .Where(i => specification.IsSatisfiedBy(i))
                .AsNoTracking();
        }
    }

    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAllAsync();

        IQueryable<T> FindByCondition(ISpecification<T> specification);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
    }
}
