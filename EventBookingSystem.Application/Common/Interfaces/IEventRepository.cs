using EventBookingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Application.Common.Interfaces
{
    public interface IEventRepository:IRepository<Event>
    {
        Task<Event> Update(Event obj);
    }
}
