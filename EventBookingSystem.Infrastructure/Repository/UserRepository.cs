using EventBookingSystem.Application.Common.Interfaces;
using EventBookingSystem.Domain.Entities;
using EventBookingSystem.infrastructure.Data;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Infrastructure.Repository
{
    internal class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        private readonly ApplicationDBContext _db;
        public UserRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }

        public async Task< ApplicationUser> Update(ApplicationUser obj)
        {
            _db.ApplicationUsers.Update(obj);
            return obj;
        }
    }
}
