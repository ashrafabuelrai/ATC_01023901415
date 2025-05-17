using AutoMapper;
using EventBookingSystem.Application.Common.DTOs.EventDTO;
using EventBookingSystem.Application.Common.Interfaces;
using EventBookingSystem.Application.Services.Interface;
using EventBookingSystem.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using EventBookingSystem.Application.Common.Utility;

namespace EventBookingSystem.Application.Services.Implementation
{
    public class EventService: IEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _webRootPath;
        private readonly IMapper _mapper;
        public EventService(string webRootPath, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _webRootPath = webRootPath;
        }
        public async Task CreateEvent(EventCreateDTO eventDTO)
        {
            
            var files = eventDTO.files;
            var eventEntity = _mapper.Map<Event>(eventDTO);
            
            await _unitOfWork.Event.Add(eventEntity);
            await _unitOfWork.Save();
            if (files != null)
            {
                foreach (var file in files)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string eventPath = @"images\events\event-" + eventEntity.Id;
                    string finalPath = Path.Combine(_webRootPath, eventPath);

                    if (!Directory.Exists(finalPath))
                    {
                        Directory.CreateDirectory(finalPath);
                    }
                    using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    EventImage eventImage = new EventImage()
                    {
                        ImageUrl = @"\" + eventPath + @"\" + fileName,
                        EventId = eventEntity.Id
                    };


                    if (eventEntity.EventImages == null)
                    {
                        eventEntity.EventImages = new List<EventImage>();
                    }
                    eventEntity.EventImages.Add(eventImage);

                }
                await _unitOfWork.Event.Update(eventEntity);
                await _unitOfWork.Save();

            }
        }
        public async Task<EventDTO> GetEventById(int id)
        {
            var eventEntity = await _unitOfWork.Event.Get(e => e.Id == id, "Category,EventImages");
            List<Image> images = new List<Image>();
            foreach (var image in eventEntity.EventImages) images.Add(new Image { Id=image.Id,Url=image.ImageUrl});
            EventDTO eventDTO = new EventDTO() {
                Name = eventEntity.Name,
                Description=eventEntity.Description,
                Venue=eventEntity.Venue,
                Price=eventEntity.Price,
                Date=eventEntity.Date,
                CategoryId=eventEntity.CategoryId,
                Category=eventEntity.Category.Name,
                Images = images,
                //English
                DescriptionEN =eventEntity.DescriptionEN,
                NameEN=eventEntity.NameEN,
                VenueEN=eventEntity.VenueEN,
                CategoryEN = eventEntity.Category.NameEN
                
                
            };
            return eventDTO;
        }
        public async Task<IEnumerable<EventDTO>> GetAllEvents()
        {
            var events = await _unitOfWork.Event.GetAll(null,"Category,EventImages");
            List<EventDTO> listEventDTO = new List<EventDTO>();
            foreach(var eventDTO in events)
            {
                List<Image> images = new List<Image>();
                foreach (var image in eventDTO.EventImages) images.Add(new Image { Id = image.Id, Url = image.ImageUrl });
                EventDTO obj = new EventDTO()
                {
                    Id=eventDTO.Id,
                    Name = eventDTO.Name,
                    Category = eventDTO.Category.Name,
                    Date=eventDTO.Date,
                    Description=eventDTO.Description,
                    Venue=eventDTO.Venue,
                    Price=eventDTO.Price,
                    CategoryId=eventDTO.CategoryId,
                    Images =images,

                    //English
                    NameEN=eventDTO.NameEN,
                    VenueEN=eventDTO.VenueEN,
                    DescriptionEN=eventDTO.DescriptionEN,
                    CategoryEN=eventDTO.Category.NameEN
                    
                };
                listEventDTO.Add(obj);
            }
            
            return listEventDTO;
        }
        public async Task UpdateEvent(int Id, EventUpdateDTO eventDTO)
        {
            var eventEntity = await _unitOfWork.Event.Get(e => e.Id == Id);
            _mapper.Map(eventDTO, eventEntity);
            var files = eventDTO.files;
            if (files != null)
            {
                foreach (var file in files)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string eventPath = @"images\events\event-" + eventEntity.Id;
                    string finalPath = Path.Combine(_webRootPath, eventPath);

                    if (!Directory.Exists(finalPath))
                    {
                        Directory.CreateDirectory(finalPath);
                    }
                    using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    EventImage eventImage = new EventImage()
                    {
                        ImageUrl = @"\" + eventPath + @"\" + fileName,
                        EventId = eventEntity.Id
                    };


                    if (eventEntity.EventImages == null)
                    {
                        eventEntity.EventImages = new List<EventImage>();
                    }
                    eventEntity.EventImages.Add(eventImage);

                }
                await _unitOfWork.Event.Update(eventEntity);
                await _unitOfWork.Save();

            }
        }
        public async Task DeleteEvent(int id)
        {
            var eventEntity = await _unitOfWork.Event.Get(e => e.Id == id);
            string evntPath = @"images\events\event-" + id;
            string finalPath = Path.Combine(_webRootPath, evntPath);

            if (Directory.Exists(finalPath))
            {
                string[] filePaths = Directory.GetFiles(finalPath);
                foreach (string filePath in filePaths)
                {
                    System.IO.File.Delete(filePath);
                }
                Directory.Delete(finalPath);
            }
            await _unitOfWork.Event.Remove(eventEntity);
            await _unitOfWork.Save();
        }
        public async Task<IEnumerable<EventDTO>> GetEventsByCategoryId(int categoryId)
        {
            var events = await _unitOfWork.Event.GetAll(e => e.CategoryId == categoryId);
            return _mapper.Map<IEnumerable<EventDTO>>(events);
        }

        public async Task<IEnumerable<EventDTO>> GetEventsByUserId(string UserId)
        {
            var bookings = await _unitOfWork.Booking.GetAll(e => e.UserId == UserId, "Event");
            var events=bookings.Select(b=>b.Event);
            return _mapper.Map<IEnumerable<EventDTO>>(events);
        }
    }
    
        
}
