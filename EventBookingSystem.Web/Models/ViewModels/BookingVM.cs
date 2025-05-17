

using EventBookingSystem.Web.Models.DTOs.BookingDTO;

namespace EventBookingSystem.Web.Models.ViewModels
{
    public class BookingVM
    {
        public BookingDTO bookingDTO { get; set; }
        public string EventName { get; set; }
        public int EventId { get; set; }
        public string EventCategory { get; set; }
        public decimal EventPrice { get; set; }
        public string EventVenue { get; set; }
        public DateTime EventDate { get; set; }
        //English
        public string EventNameEN { get; set; }
        public string EventCategoryEN { get; set; }
        public string EventVenueEN { get; set; }
    }
}
