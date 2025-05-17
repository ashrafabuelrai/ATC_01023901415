using EventBookingSystem.Application.Common.Interfaces;
using EventBookingSystem.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDBContext _db;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDBContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        public async Task Add(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public async Task<T> Get(Expression<Func<T, bool>> filter, string? includeProp = null, bool tracked = false)
        {
            IQueryable<T> query;
            if (tracked)
            {
                query = dbSet;
            }
            else
            {
                query = dbSet.AsNoTracking();
            }
            query = query.Where(filter);
            if (!string.IsNullOrEmpty(includeProp))
            {
                foreach (var prop in includeProp.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(prop);
                }
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task< IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProp = null, int pageSize = 0, int pageNumber = 1)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (pageSize > 0)
            {
                if (pageSize > 100)
                {
                    pageSize = 100;
                }
                query = query.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            }
            if (!string.IsNullOrEmpty(includeProp))
            {
                foreach (var prop in includeProp.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(prop);
                }
            }
            return query.ToList();
        }

        public async Task Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public async Task RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}
