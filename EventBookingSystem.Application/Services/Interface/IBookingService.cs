using EventBookingSystem.Application.Common.DTOs.BookingDTO;
using EventBookingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Application.Services.Interface
{
    public interface IBookingService
    {
        Task CreateBooking(BookingCreateDTO booking);
        Task<BookingDTO> GetBookingById(int Id);
        Task<IEnumerable<BookingDTO>> GetAllBookingsByUserId(string UserId);
        Task<IEnumerable<BookingDTO>> GetAllBookings();

    }
}
