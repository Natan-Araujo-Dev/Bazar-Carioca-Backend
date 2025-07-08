using AutoMapper;
using BazarCarioca.WebAPI.DTOs.Entities;
using BazarCarioca.WebAPI.Models;

namespace BazarCarioca.WebAPI.DTOs.Mapper
{
    public class ServiceMappingProfile : Profile
    {
        public ServiceMappingProfile()
        {
            CreateMap<Service, ServiceDTO>()
                .ReverseMap();
        }
    }
}
