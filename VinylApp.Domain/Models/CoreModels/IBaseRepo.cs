using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace VinylApp.Domain.Models.CoreModels
{
    public interface IBaseRepo<T> where T : BaseModel
    {
        Task<T> GetById(int id);
        Task<List<T>> GetAll();
        Task<List<T>> Find(Expression<Func<T, bool>> expression);
        Task Add(T entity);
        void Remove(T entity);
    }
}