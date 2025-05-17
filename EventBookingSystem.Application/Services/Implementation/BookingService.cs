using AutoMapper;
using EventBookingSystem.Application.Common.DTOs.BookingDTO;
using EventBookingSystem.Application.Common.Interfaces;
using EventBookingSystem.Application.Services.Interface;
using EventBookingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Application.Services.Implementation
{
    public class BookingService:IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BookingService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task CreateBooking(BookingCreateDTO booking)
        {
            
            var bookingEntity = new Booking
            {
                EventId = booking.EventId,
                UserId = booking.UserId,
                BookingDate = DateTime.Now,
                
            };
            await _unitOfWork.Booking.Add(bookingEntity);
            await _unitOfWork.Save();
        }
        public async Task<BookingDTO> GetBookingById(int Id)
        {
            var booking= await _unitOfWork.Booking.Get(b => b.Id == Id);
            return _mapper.Map<BookingDTO>(booking);
        }
        public async Task<IEnumerable<BookingDTO>> GetAllBookingsByUserId(string UserId)
        {
            if (string.IsNullOrEmpty(UserId))
            {
                return new List<BookingDTO>();
            }
            else
            {
                var bookings = await _unitOfWork.Booking.GetAll(b => b.UserId == UserId);
                return _mapper.Map<IEnumerable<BookingDTO>>(bookings);
            }
        }

        public async Task<IEnumerable<BookingDTO>> GetAllBookings()
        {
            var bookings = await _unitOfWork.Booking.GetAll();
            return _mapper.Map<IEnumerable<BookingDTO>>(bookings);
        }
    }
}
