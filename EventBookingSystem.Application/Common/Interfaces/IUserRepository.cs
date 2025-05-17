using EventBookingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Application.Common.Interfaces
{
    public interface IUserRepository:IRepository<ApplicationUser>
    {
        Task<ApplicationUser> Update(ApplicationUser obj);
    }
}
