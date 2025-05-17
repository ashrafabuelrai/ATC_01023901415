using EventBookingSystem.Application.Common.Interfaces;
using EventBookingSystem.Domain.Entities;
using EventBookingSystem.infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Infrastructure.Repository
{
    public class UnitOfWork :IUnitOfWork
    {
        private readonly ApplicationDBContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IEventRepository Event { get; private set; }

        public ICategoryRepository Category { get; private set; }

        public IBookingRepository Booking { get; private set; }

        public IUserRepository User { get; private set; }
        public IEventImageRepository EventImage { get; set; }
        public UnitOfWork(ApplicationDBContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            Event = new EventRepository(_db);
            Booking = new BookingRepository(_db);
            User = new UserRepository(_db);
            Category = new CategoryRepository(_db);
            EventImage = new EventImageRepository(_db);
        }
        public async Task Save()
        {
           await _db.SaveChangesAsync();
        }
    }
}
