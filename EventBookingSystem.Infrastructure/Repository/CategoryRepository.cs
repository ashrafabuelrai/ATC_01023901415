using EventBookingSystem.Application.Common.Interfaces;
using EventBookingSystem.Domain.Entities;
using EventBookingSystem.infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Infrastructure.Repository
{
    public  class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDBContext _db;
        public CategoryRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Category> Update(Category obj)
        {
            _db.Categories.Update(obj);
            return obj;
        }
    }
}
