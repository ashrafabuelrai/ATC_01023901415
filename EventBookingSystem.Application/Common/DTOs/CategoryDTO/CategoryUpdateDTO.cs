using EventBookingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Application.Common.DTOs.CategoryDTO
{
    public class CategoryUpdateDTO
    {
        public string Name { get; set; }
        //English
        public string? NameEN { get; set; }

    }
}
