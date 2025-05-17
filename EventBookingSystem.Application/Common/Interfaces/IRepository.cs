using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Application.Common.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProp = null, int pageSize = 0, int pageNumber = 1);
        Task<T> Get(Expression<Func<T, bool>> filter, string? includeProp = null, bool tracked = false);
        Task Add(T entity);
        Task Remove(T entity);
        Task RemoveRange(IEnumerable<T> entity);
    }
}
