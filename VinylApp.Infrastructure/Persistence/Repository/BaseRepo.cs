using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VinylApp.Domain.Models.CoreModels;
using VinylApp.Infrastructure.Persistence.DbContexts;

namespace VinylApp.Infrastructure.Persistence.Repository
{
    public class BaseRepo<T> : IBaseRepo<T> where T : BaseModel
    {
        private protected readonly VinylAppContext _context;

        protected BaseRepo(VinylAppContext context)
        {
            _context = context;
        }

        public virtual async Task Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public async Task<List<T>> Find(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().Where(expression).ToListAsync();
        }

        public async Task<List<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            var foundEntity = await _context.Set<T>().FindAsync(id);
            return foundEntity;
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
    }
}
