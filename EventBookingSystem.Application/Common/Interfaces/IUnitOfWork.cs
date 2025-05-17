using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IEventRepository Event { get; }
        ICategoryRepository Category { get; }
        IBookingRepository Booking { get; }
        IUserRepository User { get; }
        IEventImageRepository EventImage { get; }
        Task Save();
    }
}
