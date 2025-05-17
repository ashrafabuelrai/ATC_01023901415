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
    internal class EventRepository : Repository<Event>, IEventRepository
    {
        private readonly ApplicationDBContext _db;
        public EventRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Event> Update(Event obj)
        {
            _db.Events.Update(obj);
            return obj;
        }

    }
}
