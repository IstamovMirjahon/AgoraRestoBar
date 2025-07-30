using Agora.Application.DTOs;
using Agora.Application.Services;
using Agora.Domain.Entities;
using AutoMapper;

namespace Agora.Application.Mappings
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<Booking, BookingDto>();
            CreateMap<BannerService, BannerDto>();
        }
    }
}
