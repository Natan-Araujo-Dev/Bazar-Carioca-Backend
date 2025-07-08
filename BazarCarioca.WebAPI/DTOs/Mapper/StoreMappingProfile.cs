using AutoMapper;
using BazarCarioca.WebAPI.DTOs.Entities;
using BazarCarioca.WebAPI.Models;

namespace BazarCarioca.WebAPI.DTOs.Mapper
{
    public class StoreMappingProfile : Profile
    {
        public StoreMappingProfile()
        {
            CreateMap<Store, StoreDTO>()
                .ReverseMap();

            CreateMap<StoreCreateDTO, Store>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<StoreCreateDTO, StoreDTO>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<StoreUpdateDTO, Store>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
