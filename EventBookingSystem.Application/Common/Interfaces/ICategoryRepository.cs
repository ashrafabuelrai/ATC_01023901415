using EventBookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Application.Common.Interfaces
{
    public interface ICategoryRepository:IRepository<Category>
    {
        Task<Category> Update(Category obj);
       
    }
}
