using EventBookingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Application.Common.Interfaces
{
    public interface IBookingRepository:IRepository<Booking>
    {
        Task<Booking> Update(Booking obj);
    }
}
