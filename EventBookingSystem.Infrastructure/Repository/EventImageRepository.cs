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
    public class EventImageRepository : Repository<EventImage>, IEventImageRepository
    {
        private readonly ApplicationDBContext _db;
        public EventImageRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }
        public async Task<EventImage> Update(EventImage obj)
        {
            _db.Eventmages.Update(obj);
            return obj;
        }
    }
}
