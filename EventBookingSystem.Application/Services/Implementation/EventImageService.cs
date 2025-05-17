using AutoMapper;
using EventBookingSystem.Application.Common.DTOs.EventImageDTO;
using EventBookingSystem.Application.Common.Interfaces;
using EventBookingSystem.Application.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Application.Services.Implementation
{
    public class EventImageService : IEventImageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _webRootPath;
        private readonly IMapper _mapper;
        public EventImageService(IUnitOfWork unitOfWork, string webRootPath,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _webRootPath = webRootPath;
            _mapper = mapper;
        }
        public async Task DeleteEventImageById(int Id)
        {

            var image = await _unitOfWork.EventImage.Get(i => i.Id==Id);
            var filePath = Path.Combine(_webRootPath, "images", image.ImageUrl);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            // 3. Remove from DB
            await _unitOfWork.EventImage.Remove(image);
            await _unitOfWork.Save();
        }

        public async Task<EventImageDTO> GetEventImageById(int Id)
        {
            var image = await _unitOfWork.EventImage.Get(i => i.Id==Id);
            return _mapper.Map<EventImageDTO>(image);
        }
    }
}
